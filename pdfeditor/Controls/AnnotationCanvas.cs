// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.AnnotationCanvas
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Controls;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls.Annotations;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Controls.PageContents;
using pdfeditor.Controls.PdfViewerDecorators;
using pdfeditor.Controls.Screenshots;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Models.Operations;
using pdfeditor.Services;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using PDFKit;
using PDFKit.Events;
using PDFKit.Utils;
using PDFKit.Utils.PageContents;
using PDFKit.Utils.StampUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

public class AnnotationCanvas : Canvas
{
  private PDFEraserUtil pdfEraserUtil;
  private DoubleClickHelper doubleClickHelper;
  private AnnotationTooltipService tooltipService;
  private Rectangle hitTestElement;
  private ScrollViewer scrollViewer;
  public FS_RECTF ImageRect;
  public int ImageIndex = -1;
  public ImageControl ImageControl;
  private AnnotationHolderManager holders;
  private AnnotationFocusControl focusControl;
  private ScreenshotDialog screenshotDialog;
  private bool viewerDragged;
  private Point? viewerPressedPoint;
  private AnnotationContextMenuHolder annotationContextMenuHolder;
  private SelectTextContextMenuHolder selectTextContextMenuHolder;
  private TextObjectContextMenuHolder textObjectContextMenuHolder;
  private PageDefaultContextMenuHolder pageDefaultContextMenuHolder;
  private AnnotationPopupHolder popupHolder;
  private TextObjectHolder textObjectHolder;
  private PdfViewerAutoScrollHelper pdfViewerAutoScrollHelper;
  private Rectangle textObjRect;
  public Point StampStartPoint;
  public static readonly DependencyProperty PdfViewerProperty = DependencyProperty.Register(nameof (PdfViewer), typeof (PdfViewer), typeof (AnnotationCanvas), new PropertyMetadata((object) null, new PropertyChangedCallback(AnnotationCanvas.OnPdfViewerPropertyChanged)));
  public static readonly DependencyProperty SelectedAnnotationProperty = DependencyProperty.Register(nameof (SelectedAnnotation), typeof (PdfAnnotation), typeof (AnnotationCanvas), new PropertyMetadata((object) null, new PropertyChangedCallback(AnnotationCanvas.OnSelectedAnnotationPropertyChanged)));
  public static readonly DependencyProperty IsAnnotationVisibleProperty = DependencyProperty.Register(nameof (IsAnnotationVisible), typeof (bool), typeof (AnnotationCanvas), new PropertyMetadata((object) true, new PropertyChangedCallback(AnnotationCanvas.OnIsAnnotationVisiblePropertyChanged)));
  private PageObjectTypes[] editingPageObjectTypes;
  public static readonly DependencyProperty EditingPageObjectTypeProperty = DependencyProperty.Register(nameof (EditingPageObjectType), typeof (PageObjectType), typeof (AnnotationCanvas), new PropertyMetadata((object) PageObjectType.None, new PropertyChangedCallback(AnnotationCanvas.OnEditingPageObjectTypePropertyChanged)));
  public static readonly DependencyProperty AutoScrollSpeedProperty = DependencyProperty.Register(nameof (AutoScrollSpeed), typeof (int), typeof (AnnotationCanvas), new PropertyMetadata((object) 1, new PropertyChangedCallback(AnnotationCanvas.OnAutoScrollSpeedPropertyChanged)));

  public AnnotationCanvas()
  {
    this.HorizontalAlignment = HorizontalAlignment.Stretch;
    this.VerticalAlignment = VerticalAlignment.Stretch;
    this.ClipToBounds = true;
    this.doubleClickHelper = new DoubleClickHelper((UIElement) this);
    this.doubleClickHelper.MouseDoubleClick += new MouseButtonEventHandler(this.DoubleClickHelper_MouseDoubleClick);
    Rectangle rectangle1 = new Rectangle();
    rectangle1.IsHitTestVisible = false;
    rectangle1.Fill = (Brush) Brushes.Transparent;
    rectangle1.UseLayoutRounding = false;
    this.hitTestElement = rectangle1;
    this.ImageControl = new ImageControl();
    this.ImageControl.Visibility = Visibility.Collapsed;
    this.Children.Add((UIElement) this.ImageControl);
    this.InternalChildren.Add((UIElement) this.hitTestElement);
    Panel.SetZIndex((UIElement) this.hitTestElement, -1);
    this.holders = new AnnotationHolderManager(this);
    this.holders.SelectedAnnotationChanged += new EventHandler(this.Holders_SelectedAnnotationChanged);
    this.holders.CurrentHolderChanged += new EventHandler(this.Holders_CurrentHolderChanged);
    this.focusControl = new AnnotationFocusControl(this);
    this.InternalChildren.Add((UIElement) this.focusControl);
    Panel.SetZIndex((UIElement) this.focusControl, 1);
    OperationManager.BeforeOperationInvoked += new EventHandler(this.OperationManager_BeforeOperationInvoked);
    OperationManager.AfterOperationInvoked += new EventHandler(this.OperationManager_AfterOperationInvoked);
    SystemParameters.StaticPropertyChanged += new PropertyChangedEventHandler(this.SystemParameters_StaticPropertyChanged);
    this.UpdateViewerFlyoutExtendWidth();
    this.annotationContextMenuHolder = new AnnotationContextMenuHolder(this);
    this.selectTextContextMenuHolder = new SelectTextContextMenuHolder(this)
    {
      ShowRecentColorInContextMenu = false
    };
    this.textObjectContextMenuHolder = new TextObjectContextMenuHolder(this);
    this.pageDefaultContextMenuHolder = new PageDefaultContextMenuHolder(this);
    this.screenshotDialog = new ScreenshotDialog();
    this.screenshotDialog.Visibility = Visibility.Collapsed;
    this.screenshotDialog.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ScreenshotDialog_IsVisibleChanged);
    this.InternalChildren.Add((UIElement) this.screenshotDialog);
    Panel.SetZIndex((UIElement) this.screenshotDialog, 2);
    this.SizeChanged += new SizeChangedEventHandler(this.AnnotationCanvas_SizeChanged);
    Rectangle rectangle2 = new Rectangle();
    rectangle2.Stroke = (Brush) new SolidColorBrush(Colors.Black);
    rectangle2.StrokeThickness = 1.0;
    this.textObjRect = rectangle2;
    this.Children.Add((UIElement) this.textObjRect);
    this.popupHolder = new AnnotationPopupHolder(this);
    this.textObjectHolder = new TextObjectHolder(this);
    this.pdfEraserUtil = new PDFEraserUtil();
  }

  protected PdfDocument Document => this.PdfViewer?.Document;

  public AnnotationHolderManager HolderManager => this.holders;

  public AnnotationPopupHolder PopupHolder => this.popupHolder;

  public TextObjectHolder TextObjectHolder => this.textObjectHolder;

  public PdfViewerAutoScrollHelper AutoScrollHelper => this.pdfViewerAutoScrollHelper;

  internal TextObjectContextMenuHolder TextObjectContextMenuHolder
  {
    get => this.textObjectContextMenuHolder;
  }

  public ScreenshotDialog ScreenshotDialog => this.screenshotDialog;

  private MainViewModel VM => this.DataContext as MainViewModel;

  public PdfViewer PdfViewer
  {
    get => (PdfViewer) this.GetValue(AnnotationCanvas.PdfViewerProperty);
    set => this.SetValue(AnnotationCanvas.PdfViewerProperty, (object) value);
  }

  private static void OnPdfViewerPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is AnnotationCanvas annotationCanvas))
      return;
    annotationCanvas.tooltipService?.Dispose();
    annotationCanvas.tooltipService = (AnnotationTooltipService) null;
    annotationCanvas.popupHolder.ClearAnnotationPopup();
    annotationCanvas.focusControl.Annotation = (PdfAnnotation) null;
    if (e.OldValue is PdfViewer oldValue)
      annotationCanvas.RemoveViewerEventHandler(oldValue);
    if (e.NewValue is PdfViewer newValue)
    {
      annotationCanvas.AddViewerEventHandler(newValue);
      annotationCanvas.popupHolder.InitAnnotationPopup(newValue.Document?.Pages?.CurrentPage);
      annotationCanvas.tooltipService = new AnnotationTooltipService(newValue);
    }
    annotationCanvas.UpdateAutoScrollHelper();
  }

  public PdfAnnotation SelectedAnnotation
  {
    get => (PdfAnnotation) this.GetValue(AnnotationCanvas.SelectedAnnotationProperty);
    set => this.SetValue(AnnotationCanvas.SelectedAnnotationProperty, (object) value);
  }

  private static void OnSelectedAnnotationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is AnnotationCanvas sender))
      return;
    sender.holders.Select(e.NewValue as PdfAnnotation, false);
    if (e.NewValue is PdfAnnotation)
      sender.focusControl.Annotation = (PdfAnnotation) null;
    sender.UpdateHoverAnnotationBorder();
    EventHandler annotationChanged = sender.SelectedAnnotationChanged;
    if (annotationChanged != null)
      annotationChanged((object) sender, EventArgs.Empty);
    sender.popupHolder.SetPopupSelected((PdfAnnotation) e.OldValue, false);
    sender.popupHolder.SetPopupSelected((PdfAnnotation) e.NewValue, true);
    sender.UpdateViewerFlyoutExtendWidth();
  }

  public bool IsAnnotationVisible
  {
    get => (bool) this.GetValue(AnnotationCanvas.IsAnnotationVisibleProperty);
    set => this.SetValue(AnnotationCanvas.IsAnnotationVisibleProperty, (object) value);
  }

  private static void OnIsAnnotationVisiblePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is AnnotationCanvas annotationCanvas))
      return;
    if (e.NewValue is bool newValue && newValue)
    {
      PdfPage currentPage = annotationCanvas.PdfViewer?.Document?.Pages?.CurrentPage;
      if (currentPage == null)
        return;
      annotationCanvas.popupHolder.InitAnnotationPopup(currentPage);
    }
    else
    {
      annotationCanvas.popupHolder.ClearAnnotationPopup();
      annotationCanvas.holders.CancelAll();
      annotationCanvas.UpdateHoverAnnotationBorder();
    }
  }

  public PageObjectType EditingPageObjectType
  {
    get => (PageObjectType) this.GetValue(AnnotationCanvas.EditingPageObjectTypeProperty);
    set => this.SetValue(AnnotationCanvas.EditingPageObjectTypeProperty, (object) value);
  }

  private static void OnEditingPageObjectTypePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((PageObjectType) e.NewValue == (PageObjectType) e.OldValue || !(d is AnnotationCanvas annotationCanvas))
      return;
    annotationCanvas.textObjectHolder.CancelTextObject();
    annotationCanvas.editingPageObjectTypes = (PageObjectTypes[]) null;
    switch ((PageObjectType) e.NewValue)
    {
      case PageObjectType.Text:
        annotationCanvas.editingPageObjectTypes = new PageObjectTypes[1]
        {
          PageObjectTypes.PDFPAGE_TEXT
        };
        break;
      case PageObjectType.Path:
        annotationCanvas.editingPageObjectTypes = new PageObjectTypes[1]
        {
          PageObjectTypes.PDFPAGE_PATH
        };
        break;
      case PageObjectType.Image:
        annotationCanvas.editingPageObjectTypes = new PageObjectTypes[1]
        {
          PageObjectTypes.PDFPAGE_IMAGE
        };
        break;
      case PageObjectType.Form:
        annotationCanvas.editingPageObjectTypes = new PageObjectTypes[1]
        {
          PageObjectTypes.PDFPAGE_FORM
        };
        break;
    }
    annotationCanvas.UpdateHoverAnnotationBorder();
  }

  public int AutoScrollSpeed
  {
    get => (int) this.GetValue(AnnotationCanvas.AutoScrollSpeedProperty);
    set => this.SetValue(AnnotationCanvas.AutoScrollSpeedProperty, (object) value);
  }

  private static void OnAutoScrollSpeedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is AnnotationCanvas annotationCanvas) || !(e.NewValue is int newValue) || annotationCanvas.pdfViewerAutoScrollHelper == null)
      return;
    annotationCanvas.pdfViewerAutoScrollHelper.Speed = (double) newValue;
  }

  public event EventHandler SelectedAnnotationChanged;

  private void Holders_SelectedAnnotationChanged(object sender, EventArgs e)
  {
    this.SelectedAnnotation = this.holders.SelectedAnnotation;
  }

  private void Holders_CurrentHolderChanged(object sender, EventArgs e)
  {
  }

  private void OperationManager_BeforeOperationInvoked(object sender, EventArgs e)
  {
    this.holders.CancelAll();
    this.focusControl.Annotation = (PdfAnnotation) null;
    this.textObjectHolder.CancelTextObject();
    this.UpdateHoverAnnotationBorder();
    this.popupHolder.ClearAnnotationPopup();
  }

  private void OperationManager_AfterOperationInvoked(object sender, EventArgs e)
  {
    this.popupHolder.InitAnnotationPopup(this.PdfViewer.CurrentPage);
  }

  private void AddViewerEventHandler(PdfViewer viewer)
  {
    if (viewer == null)
      return;
    viewer.MouseDown += new MouseButtonEventHandler(this.Viewer_MouseDown);
    viewer.PreviewMouseDown += new MouseButtonEventHandler(this.Viewer_PreviewMouseDown);
    viewer.PreviewMouseMove += new MouseEventHandler(this.Viewer_PreviewMouseMove);
    viewer.PreviewMouseUp += new MouseButtonEventHandler(this.Viewer_PreviewMouseUp);
    viewer.MouseUp += new MouseButtonEventHandler(this.Viewer_MouseUp);
    viewer.LostMouseCapture += new MouseEventHandler(this.Viewer_LostMouseCapture);
    viewer.AnnotationMouseMoved += new EventHandler<AnnotationMouseEventArgs>(this.Viewer_AnnotationMouseMoved);
    viewer.AnnotationMouseEntered += new EventHandler<AnnotationMouseEventArgs>(this.Viewer_AnnotationMouseEntered);
    viewer.AnnotationMouseExited += new EventHandler<AnnotationMouseEventArgs>(this.Viewer_AnnotationMouseExited);
    viewer.AnnotationMouseClick += new EventHandler<AnnotationMouseClickEventArgs>(this.Viewer_AnnotationMouseClick);
    viewer.SizeChanged += new SizeChangedEventHandler(this.Viewer_SizeChanged);
    viewer.ScrollOwnerChanged += new EventHandler(this.Viewer_ScrollOwnerChanged);
    viewer.BeforeDocumentChanged += new EventHandler<DocumentClosingEventArgs>(this.Viewer_BeforeDocumentChanged);
    viewer.AfterDocumentChanged += new EventHandler(this.Viewer_AfterDocumentChanged);
    viewer.CurrentPageChanged += new EventHandler(this.Viewer_CurrentPageChanged);
    viewer.MouseModeChanged += new EventHandler(this.Viewer_MouseModeChanged);
    viewer.PreviewMouseWheel += new MouseWheelEventHandler(this.Viewer_PreviewMouseWheel);
    viewer.BeforeLinkClicked += new EventHandler<PdfBeforeLinkClickedEventArgs>(this.Viewer_BeforeLinkClicked);
    this.scrollViewer = viewer.ScrollOwner;
    if (this.scrollViewer == null)
      return;
    MouseMiddleButtonScrollExtensions.SetIsEnabled(this.scrollViewer, true);
    MouseMiddleButtonScrollExtensions.SetShowCursorAtStartPoint(this.scrollViewer, true);
    this.scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ScrollOwner_ScrollChanged);
  }

  private void RemoveViewerEventHandler(PdfViewer viewer)
  {
    if (viewer == null)
      return;
    viewer.MouseDown -= new MouseButtonEventHandler(this.Viewer_MouseDown);
    viewer.PreviewMouseDown -= new MouseButtonEventHandler(this.Viewer_PreviewMouseDown);
    viewer.PreviewMouseMove -= new MouseEventHandler(this.Viewer_PreviewMouseMove);
    viewer.PreviewMouseUp -= new MouseButtonEventHandler(this.Viewer_PreviewMouseUp);
    viewer.MouseUp -= new MouseButtonEventHandler(this.Viewer_MouseUp);
    viewer.LostMouseCapture -= new MouseEventHandler(this.Viewer_LostMouseCapture);
    viewer.AnnotationMouseMoved -= new EventHandler<AnnotationMouseEventArgs>(this.Viewer_AnnotationMouseMoved);
    viewer.AnnotationMouseEntered -= new EventHandler<AnnotationMouseEventArgs>(this.Viewer_AnnotationMouseEntered);
    viewer.AnnotationMouseExited -= new EventHandler<AnnotationMouseEventArgs>(this.Viewer_AnnotationMouseExited);
    viewer.AnnotationMouseClick -= new EventHandler<AnnotationMouseClickEventArgs>(this.Viewer_AnnotationMouseClick);
    viewer.SizeChanged -= new SizeChangedEventHandler(this.Viewer_SizeChanged);
    viewer.ScrollOwnerChanged -= new EventHandler(this.Viewer_ScrollOwnerChanged);
    viewer.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.Viewer_BeforeDocumentChanged);
    viewer.AfterDocumentChanged -= new EventHandler(this.Viewer_AfterDocumentChanged);
    viewer.CurrentPageChanged -= new EventHandler(this.Viewer_CurrentPageChanged);
    viewer.MouseModeChanged -= new EventHandler(this.Viewer_MouseModeChanged);
    viewer.PreviewMouseWheel -= new MouseWheelEventHandler(this.Viewer_PreviewMouseWheel);
    viewer.BeforeLinkClicked -= new EventHandler<PdfBeforeLinkClickedEventArgs>(this.Viewer_BeforeLinkClicked);
    if (this.scrollViewer != null)
    {
      MouseMiddleButtonScrollExtensions.SetIsEnabled(this.scrollViewer, false);
      MouseMiddleButtonScrollExtensions.SetShowCursorAtStartPoint(this.scrollViewer, false);
      this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollOwner_ScrollChanged);
    }
    this.scrollViewer = (ScrollViewer) null;
  }

  protected override void OnGotMouseCapture(MouseEventArgs e)
  {
    if (e.OriginalSource != this)
      return;
    this.hitTestElement.IsHitTestVisible = true;
    base.OnGotMouseCapture(e);
  }

  protected override void OnLostMouseCapture(MouseEventArgs e)
  {
    if (e.OriginalSource != this)
      return;
    this.hitTestElement.IsHitTestVisible = false;
    base.OnLostMouseCapture(e);
  }

  private void AnnotationCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateHoverAnnotationBorder();
  }

  private void UpdateHoverAnnotationBorder()
  {
    if (!this.IsAnnotationVisible)
      this.focusControl.Annotation = (PdfAnnotation) null;
    this.focusControl.InvalidateMeasure();
    this.holders.OnPageClientBoundsChanged();
    this.ImageControl.UpdateImageborder();
    this.popupHolder.UpdatePanelsPosition();
    this.textObjectHolder.OnPageClientBoundsChanged();
    this.screenshotDialog.Width = this.ActualWidth;
    this.screenshotDialog.Height = this.ActualHeight;
    this.UpdateHoverPageObjectRect(Rect.Empty);
  }

  private void Viewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    this.viewerDragged = false;
    if (this.doubleClickHelper.ProcessMouseClick(e))
      e.Handled = true;
    else if (this.VM.AnnotationToolbar.InkButtonModel.ToolbarSettingModel[3] is ToolbarSettingInkEraserModel inkEraserModel && inkEraserModel.IsChecked && this.VM.AnnotationToolbar.InkButtonModel.IsChecked && this.PdfViewer.CaptureMouse())
    {
      e.Handled = true;
      Point position = e.GetPosition((IInputElement) this);
      Point pagePoint;
      this.pdfEraserUtil.MouseDownRecord(this.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint), this.VM, this.Document, inkEraserModel, position, pagePoint);
    }
    else if (this.holders.CurrentHolder != null && this.holders.CurrentHolder.State != AnnotationHolderState.Selected)
    {
      e.Handled = true;
      if (e.ChangedButton != MouseButton.Right)
        return;
      this.holders.CancelAll();
    }
    else
    {
      if (!this.holders.IsAnnotationDoubleClicked((MouseEventArgs) e))
        this.holders.CancelAll();
      this.textObjectHolder.CancelTextObject();
      Point position = e.GetPosition((IInputElement) this);
      Point pagePoint;
      int page = this.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint);
      if (page == -1)
        return;
      if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed && this.holders.CurrentHolder == null)
      {
        bool flag = false;
        if ((this.VM.AnnotationToolbar.LinkButtonModel.IsChecked || this.VM.AnnotationToolbar.TextBoxButtonModel.IsChecked || this.VM.AnnotationToolbar.TextButtonModel.IsChecked) && this.Document.Pages[page].Annots != null)
        {
          foreach (PdfAnnotation annot in this.Document.Pages[page].Annots)
          {
            if (annot is PdfLinkAnnotation && AnnotationHitTestHelper.HitTest(annot, position))
            {
              flag = true;
              break;
            }
            if (annot is PdfFreeTextAnnotation && AnnotationHitTestHelper.HitTest(annot, position))
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
        {
          if (this.StartCreateNewAnnot(this.Document.Pages[page], pagePoint.ToPdfPoint()))
          {
            if (this.holders.CurrentHolder.IsTextMarkupAnnotation)
            {
              this.PdfViewer.CaptureMouse();
            }
            else
            {
              e.Handled = true;
              this.CaptureMouse();
            }
          }
          e.Handled = e.LeftButton != MouseButtonState.Pressed;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AnnotationCanvas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AnnotationCanvas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, MouseModes, object> target2 = AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, MouseModes, object>> p1 = AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (AnnotationCanvas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__0.Target((CallSite) AnnotationCanvas.\u003C\u003Eo__78.\u003C\u003Ep__0, this.VM.ViewerMouseMode);
      object obj2 = target2((CallSite) p1, obj1, MouseModes.PanTool);
      if (!target1((CallSite) p2, obj2))
        return;
      this.VM.ViewToolbar.PauseAutoScroll(0);
    }
  }

  private void Viewer_MouseDown(object sender, MouseButtonEventArgs e)
  {
    this.viewerPressedPoint = new Point?(e.GetPosition((IInputElement) this));
    Point position = e.GetPosition((IInputElement) this);
    Point pagePoint;
    int page1 = this.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint);
    if (e.MiddleButton == MouseButtonState.Pressed && !this.ImageControl.ImageControlState && this.VM.AnnotationMode != AnnotationMode.Link)
    {
      this.pdfViewerAutoScrollHelper?.StopAutoScroll();
      this.VM.ExitTransientMode();
      this.VM.ReleaseViewerFocusAsync(false);
      if (this.scrollViewer != null)
        MouseMiddleButtonScrollExtensions.TryEnterScrollMode(this.scrollViewer);
    }
    if (page1 < 0)
      return;
    PdfPage page2 = this.Document.Pages[page1];
    PageObjectTypes[] editingPageObjectTypes = this.editingPageObjectTypes;
    if (editingPageObjectTypes != null && editingPageObjectTypes.Length != 0)
    {
      e.Handled = true;
      this.PdfViewer.DeselectText();
      PdfPageObject[] pointObjects = PageObjectHitTestHelper.GetPointObjects(page2, pagePoint, editingPageObjectTypes);
      if (pointObjects.Length != 0 && pointObjects[0] is PdfTextObject textObject)
        this.textObjectHolder.SelectTextObject(page2, textObject);
    }
    int Index;
    if (this.ImageControl.Visibility != Visibility.Visible || !PageImageUtils.ImageTestHitTest(this.Document.Pages[page1], pagePoint, out Index))
      return;
    if (Index != this.ImageControl.imageindex)
    {
      this.ImageControl.CreateImageborder(this, this.Document, page1, Index, this.PdfViewer);
      this.ImageControl.Visibility = Visibility.Visible;
    }
    this.ImageControl.clickStartPosition = position;
  }

  protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseDown(e);
    if (!this.doubleClickHelper.ProcessMouseClick(e))
      return;
    e.Handled = true;
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (e.ChangedButton != MouseButton.Right || e.RightButton != MouseButtonState.Pressed || this.holders.CurrentHolder == null || this.holders.CurrentHolder.State != AnnotationHolderState.CreatingNew)
      return;
    this.ReleaseMouseCapture();
    this.holders.CurrentHolder.Cancel();
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this);
    Point pagePoint;
    if (this.ProcessMouseMove(this.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint), pagePoint.ToPdfPoint()))
      e.Handled = true;
    base.OnMouseMove(e);
  }

  protected override async void OnMouseUp(MouseButtonEventArgs e)
  {
    AnnotationCanvas annotationCanvas = this;
    // ISSUE: reference to a compiler-generated method
    annotationCanvas.\u003C\u003En__0(e);
    annotationCanvas.viewerDragged = false;
    annotationCanvas.viewerPressedPoint = new Point?();
    if (!annotationCanvas.isAllowClick())
      ;
    else if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Released)
    {
      Point position = e.GetPosition((IInputElement) annotationCanvas);
      Point pagePoint;
      int page = annotationCanvas.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint);
      if (await annotationCanvas.ProcessMouseUpAsync(page, pagePoint.ToPdfPoint(), (Action) (() =>
      {
        e.Handled = true;
        this.ReleaseMouseCapture();
      })))
      {
        annotationCanvas.RouteClickEventToPdfViewer(e);
      }
      else
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (annotationCanvas.ReleaseMouseCapture());
      }
    }
    else if (e.ChangedButton != MouseButton.Right)
      ;
    else
    {
      if (annotationCanvas.SelectedAnnotation is PdfLinkAnnotation selectedAnnotation && annotationCanvas.VM.AnnotationToolbar.LinkButtonModel.IsChecked)
        new LinkRightMenu(annotationCanvas, selectedAnnotation, annotationCanvas.Document).Show();
      if (!annotationCanvas.holders.IsAnnotationDoubleClicked((MouseEventArgs) e))
        ;
      else
      {
        int num = await annotationCanvas.annotationContextMenuHolder.ShowAsync() ? 1 : 0;
      }
    }
  }

  private void Viewer_PreviewMouseMove(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this);
    Point pagePoint;
    int num = this.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint);
    if (this.VM.ViewerOperationModel != null)
      return;
    if (this.VM.AnnotationToolbar.LinkButtonModel.IsChecked)
    {
      this.Cursor = Cursors.Cross;
      PDFKit.PdfControl.GetPdfControl(this.Document).Viewer.OverrideCursor = Cursors.Cross;
    }
    else
    {
      if (this.VM.AnnotationToolbar.InkButtonModel.ToolbarSettingModel[3] is ToolbarSettingInkEraserModel inkEraserModel && inkEraserModel.IsChecked)
      {
        if (this.VM.AnnotationToolbar.InkButtonModel.IsChecked)
        {
          try
          {
            this.pdfEraserUtil.MouseStyle(this.Document, this, inkEraserModel, this.VM);
            if (num < 0)
              num = this.VM.SelectedPageIndex;
            if (this.Document.Pages[num].Annots == null || num < 0 || e.LeftButton != MouseButtonState.Pressed)
              return;
            bool flag = false;
            for (int index = 0; index < this.Document.Pages[num].Annots.Count; ++index)
            {
              if (this.Document.Pages[num].Annots[index] is PdfInkAnnotation)
                flag = this.pdfEraserUtil.DeleteInk(this.Document, num, position, inkEraserModel) | flag;
            }
            if (!flag)
              return;
            this.Document.Pages[num].TryRedrawPageAsync();
            return;
          }
          catch
          {
            return;
          }
        }
      }
      if (this.VM.AnnotationMode == AnnotationMode.Ink)
      {
        Cursor cursor = CursorHelper.CreateCursor(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Style\\\\Resources\\\\MousePen.png"), yHotSpot: 32U /*0x20*/);
        this.Cursor = cursor;
        PDFKit.PdfControl.GetPdfControl(this.Document).Viewer.OverrideCursor = cursor;
      }
      else
      {
        this.Cursor = (Cursor) null;
        if (this.Document != null)
        {
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
          if (pdfControl != null)
            pdfControl.Viewer.OverrideCursor = (Cursor) null;
        }
        this.ProcessMouseMove(num, pagePoint.ToPdfPoint());
        if (num >= 0)
        {
          PdfPage page = this.Document.Pages[num];
          PageObjectTypes[] editingPageObjectTypes = this.editingPageObjectTypes;
          if (this.ImageControl.Visibility == Visibility.Visible && PageImageUtils.ImageTestHitTest(this.Document.Pages[num], pagePoint, out int _) || this.ImageControl.IsMoved || this.ImageControl.mousePosition != ImageControl.MousePosition.None)
          {
            this.Cursor = Cursors.SizeAll;
            PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(this.Document);
            if (this.ImageControl.ImageControlState)
            {
              if (pdfControl != null && pdfControl.Viewer.OverrideCursor != Cursors.Hand)
                pdfControl.Viewer.OverrideCursor = Cursors.SizeAll;
              this.PdfViewer.DeselectText();
              if (e.LeftButton == MouseButtonState.Pressed && this.ImageControl.mousePosition != ImageControl.MousePosition.None)
              {
                this.ImageControl.ImageControlReSizeImage(position);
                return;
              }
              if (e.LeftButton != MouseButtonState.Pressed)
                return;
              this.ImageControl.MoveImageBorder(position);
              return;
            }
          }
          if (editingPageObjectTypes != null)
          {
            PdfPageObject[] pointObjects = PageObjectHitTestHelper.GetPointObjects(page, pagePoint, editingPageObjectTypes);
            Rect clientRect;
            if (!this.textObjectContextMenuHolder.IsOpen && pointObjects.Length != 0 && this.PdfViewer.TryGetClientRect(num, pointObjects[0].BoundingBox, out clientRect))
            {
              this.UpdateHoverPageObjectRect(clientRect);
              return;
            }
          }
        }
        this.UpdateHoverPageObjectRect(Rect.Empty);
      }
    }
  }

  private async void Viewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    AnnotationCanvas relativeTo = this;
    if (!relativeTo.isAllowClick())
      return;
    if (relativeTo.VM.AnnotationToolbar.InkButtonModel.ToolbarSettingModel[3] is ToolbarSettingInkEraserModel settingInkEraserModel && settingInkEraserModel.IsChecked && settingInkEraserModel.IsPartial == ToolbarSettingInkEraserModel.EraserType.Partial && relativeTo.VM.AnnotationToolbar.InkButtonModel.IsChecked)
    {
      Mouse.Captured?.ReleaseMouseCapture();
      relativeTo.pdfEraserUtil.CommitRedoRecords(relativeTo.VM, relativeTo.Document);
    }
    if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Released)
    {
      Point position = e.GetPosition((IInputElement) relativeTo);
      Point pagePoint;
      int page = relativeTo.PdfViewer.DeviceToPage(position.X, position.Y, out pagePoint);
      // ISSUE: reference to a compiler-generated method
      if (!await relativeTo.ProcessMouseUpAsync(page, pagePoint.ToPdfPoint(), new Action(relativeTo.\u003CViewer_PreviewMouseUp\u003Eb__85_0)))
        relativeTo.PdfViewer.ReleaseMouseCapture();
    }
    // ISSUE: reference to a compiler-generated field
    if (AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AnnotationCanvas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, bool> target1 = AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__2.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, bool>> p2 = AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__2;
    // ISSUE: reference to a compiler-generated field
    if (AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, MouseModes, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AnnotationCanvas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, MouseModes, object> target2 = AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, MouseModes, object>> p1 = AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (AnnotationCanvas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj1 = AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__0.Target((CallSite) AnnotationCanvas.\u003C\u003Eo__85.\u003C\u003Ep__0, relativeTo.VM.ViewerMouseMode);
    object obj2 = target2((CallSite) p1, obj1, MouseModes.PanTool);
    if (!target1((CallSite) p2, obj2))
      return;
    relativeTo.VM.ViewToolbar.PauseAutoScroll(1);
  }

  private async void Viewer_MouseUp(object sender, MouseButtonEventArgs e)
  {
    AnnotationCanvas annotationCanvas = this;
    if (e.ChangedButton != MouseButton.Left && e.ChangedButton != MouseButton.Right)
      return;
    bool dragged = annotationCanvas.viewerDragged;
    if (!dragged && annotationCanvas.viewerPressedPoint.HasValue)
    {
      Point point = annotationCanvas.viewerPressedPoint.Value;
      Point position = e.GetPosition((IInputElement) annotationCanvas);
      if (Math.Abs(position.X - point.X) > 2.0 || Math.Abs(position.Y - point.Y) > 2.0)
        dragged = true;
    }
    annotationCanvas.viewerPressedPoint = new Point?();
    if (annotationCanvas.ImageControl.Visibility == Visibility.Visible && annotationCanvas.ImageControl.IsMoved)
      annotationCanvas.ImageControl.ImageControlMoveImage(e.GetPosition((IInputElement) annotationCanvas));
    if (e.ChangedButton == MouseButton.Right | dragged)
    {
      annotationCanvas.viewerDragged = false;
      annotationCanvas.viewerPressedPoint = new Point?();
      int num = await annotationCanvas.selectTextContextMenuHolder.ShowAsync(e.ChangedButton == MouseButton.Left) ? 1 : 0;
    }
    if (dragged)
      return;
    Point position1 = e.GetPosition((IInputElement) annotationCanvas);
    Point pagePoint;
    int page = annotationCanvas.PdfViewer.DeviceToPage(position1.X, position1.Y, out pagePoint);
    if (e.ChangedButton == MouseButton.Right)
    {
      annotationCanvas.tooltipService.HideTooltip();
      if (page < 0)
        return;
      annotationCanvas.pageDefaultContextMenuHolder.Show();
    }
    else
    {
      if (e.ChangedButton != MouseButton.Left || annotationCanvas.Document == null || page == -1 || annotationCanvas.VM.AnnotationMode != AnnotationMode.None)
        return;
      int Index;
      if (PageImageUtils.ImageTestHitTest(annotationCanvas.Document.Pages[page], pagePoint, out Index))
      {
        annotationCanvas.ImageControl.CreateImageborder(annotationCanvas, annotationCanvas.Document, page, Index, annotationCanvas.PdfViewer);
        annotationCanvas.ImageControl.Visibility = Visibility.Visible;
      }
      else
      {
        if (annotationCanvas.ImageControl.IsMoved)
          return;
        annotationCanvas.ImageControl.Visibility = Visibility.Collapsed;
      }
    }
  }

  private void Viewer_LostMouseCapture(object sender, MouseEventArgs e)
  {
    if (!(this.VM.AnnotationToolbar.InkButtonModel.ToolbarSettingModel[3] is ToolbarSettingInkEraserModel settingInkEraserModel) || !settingInkEraserModel.IsChecked || settingInkEraserModel.IsPartial != ToolbarSettingInkEraserModel.EraserType.Partial || !this.VM.AnnotationToolbar.InkButtonModel.IsChecked)
      return;
    this.pdfEraserUtil.CommitRedoRecords(this.VM, this.Document);
  }

  public bool HasSelectedText() => AnnotationCanvas.HasSelectedText(this.PdfViewer);

  private static bool HasSelectedText(PdfViewer viewer)
  {
    return viewer != null && viewer.SelectInfo.StartPage != -1 && viewer.SelectInfo.EndPage != -1 && (viewer.SelectInfo.StartIndex != -1 || viewer.SelectInfo.EndIndex != -1) && !string.IsNullOrWhiteSpace(viewer.SelectedText);
  }

  private bool ProcessMouseMove(int pageIndex, FS_POINTF pagePoint)
  {
    return pageIndex >= 0 && this.Document != null && pageIndex < this.Document.Pages.Count && this.ProcessCreateNewAnnot(this.Document.Pages[pageIndex], pagePoint);
  }

  private async Task<bool> ProcessMouseUpAsync(
    int pageIndex,
    FS_POINTF pagePoint,
    Action setHandledFunc)
  {
    if (pageIndex < 0 || this.Document == null || pageIndex >= this.Document.Pages.Count || !this.ProcessCreateNewAnnot(this.Document.Pages[pageIndex], pagePoint))
      return false;
    Action action = setHandledFunc;
    if (action != null)
      action();
    return !await this.CompleteCreateNewAnnotAsync();
  }

  private bool StartCreateNewAnnot(PdfPage page, FS_POINTF pagePoint)
  {
    if (this.holders.CurrentHolder != null && this.holders.CurrentHolder.State != AnnotationHolderState.None || this.Document == null)
      return false;
    switch (this.VM.AnnotationMode)
    {
      case AnnotationMode.Line:
        this.holders.Line.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Ink:
        this.holders.Ink.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Shape:
        this.holders.Square.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Highlight:
        this.holders.Highlight.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Underline:
        this.holders.Underline.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Strike:
        this.holders.Strikeout.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.HighlightArea:
        this.holders.HighlightArea.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Note:
        this.holders.Text.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Ellipse:
        this.holders.Circle.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Stamp:
        return this.holders.CurrentHolder != null;
      case AnnotationMode.Text:
        this.holders.FreeText.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.TextBox:
        this.holders.FreeText.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      case AnnotationMode.Link:
        this.holders.Link.StartCreateNew(page, pagePoint);
        goto case AnnotationMode.Stamp;
      default:
        return false;
    }
  }

  private bool ProcessCreateNewAnnot(PdfPage page, FS_POINTF pagePoint)
  {
    if (page == null)
      return false;
    IAnnotationHolder currentHolder = this.holders.CurrentHolder;
    if ((currentHolder != null ? (currentHolder.State != AnnotationHolderState.CreatingNew ? 1 : 0) : 1) != 0)
      return false;
    currentHolder.ProcessCreateNew(page, pagePoint);
    return true;
  }

  private async Task<bool> CompleteCreateNewAnnotAsync()
  {
    IAnnotationHolder holder = this.holders.CurrentHolder;
    if (holder == null)
      return false;
    System.Collections.Generic.IReadOnlyList<PdfAnnotation> newAsync = await holder.CompleteCreateNewAsync();
    if (newAsync == null || newAsync.Count <= 0)
      return false;
    if (!this.VM.IsAnnotationVisible)
      this.VM.IsAnnotationVisible = true;
    if (!holder.IsTextMarkupAnnotation && newAsync.Count == 1 && holder != this.holders.Ink)
      this.holders.Select(newAsync[0], true);
    this.focusControl.Annotation = (PdfAnnotation) null;
    this.UpdateHoverAnnotationBorder();
    return true;
  }

  private void RouteClickEventToPdfViewer(MouseButtonEventArgs e)
  {
    MouseButtonEventArgs e1 = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
    e1.RoutedEvent = UIElement.MouseDownEvent;
    e1.Source = (object) this.PdfViewer;
    this.PdfViewer.RaiseEvent((RoutedEventArgs) e1);
    MouseButtonEventArgs e2 = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
    e2.RoutedEvent = UIElement.MouseUpEvent;
    e2.Source = (object) this.PdfViewer;
    this.doubleClickHelper.MouseDoubleClick -= new MouseButtonEventHandler(this.DoubleClickHelper_MouseDoubleClick);
    this.PdfViewer.RaiseEvent((RoutedEventArgs) e2);
    this.doubleClickHelper.MouseDoubleClick += new MouseButtonEventHandler(this.DoubleClickHelper_MouseDoubleClick);
  }

  private void Viewer_AnnotationMouseMoved(object sender, AnnotationMouseEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed)
      e.Handled = true;
    if (e.PdfAnnotation is PdfLinkAnnotation && this.VM.AnnotationToolbar.LinkButtonModel.IsChecked)
    {
      this.PdfViewer.DeselectText();
      if (e.PdfAnnotation is PdfFileAttachmentAnnotation)
        return;
      e.Handled = true;
      this.holders.Select(e.PdfAnnotation, false);
      this.focusControl.Annotation = (PdfAnnotation) null;
      this.UpdateHoverAnnotationBorder();
    }
    else
    {
      if (!this.VM.AnnotationToolbar.LinkButtonModel.IsChecked)
        return;
      this.PdfViewer.DeselectText();
      if (e.PdfAnnotation is PdfFileAttachmentAnnotation || this.holders.CurrentHolder != null && this.holders.CurrentHolder.State == AnnotationHolderState.CreatingNew)
        return;
      e.Handled = true;
      this.holders.Select((PdfAnnotation) null, false);
      this.focusControl.Annotation = (PdfAnnotation) null;
      this.UpdateHoverAnnotationBorder();
    }
  }

  private void Viewer_AnnotationMouseEntered(object sender, AnnotationMouseEventArgs e)
  {
    if (this.VM.AnnotationToolbar.InkButtonModel.IsChecked || this.VM.AnnotationToolbar.LinkButtonModel.IsChecked && !(e.PdfAnnotation is PdfLinkAnnotation))
      return;
    this.focusControl.Annotation = this.EditingPageObjectType != PageObjectType.None || !((PdfWrapper) this.SelectedAnnotation != (PdfWrapper) e.PdfAnnotation) || this.holders.CurrentHolder != null && this.holders.CurrentHolder.IsTextMarkupAnnotation && this.holders.CurrentHolder.State == AnnotationHolderState.CreatingNew ? (PdfAnnotation) null : e.PdfAnnotation;
    if (this.VM.AnnotationToolbar.LinkButtonModel.IsChecked && !(this.focusControl.Annotation is PdfLinkAnnotation))
      return;
    this.UpdateHoverAnnotationBorder();
    this.popupHolder.SetPopupHovered(e.PdfAnnotation, true);
  }

  private void Viewer_AnnotationMouseExited(object sender, AnnotationMouseEventArgs e)
  {
    this.focusControl.Annotation = (PdfAnnotation) null;
    this.UpdateHoverAnnotationBorder();
    if (this.VM.AnnotationToolbar.InkButtonModel.IsChecked || this.VM.AnnotationToolbar.LinkButtonModel.IsChecked && !(e.PdfAnnotation is PdfLinkAnnotation))
      return;
    this.popupHolder.SetPopupHovered(e.PdfAnnotation, false);
  }

  private async void Viewer_AnnotationMouseClick(object sender, AnnotationMouseClickEventArgs e)
  {
    if (this.VM.AnnotationToolbar.InkButtonModel.IsChecked || this.VM.AnnotationToolbar.LinkButtonModel.IsChecked && !(e.PdfAnnotation is PdfLinkAnnotation))
      return;
    if (!this.VM.AnnotationToolbar.LinkButtonModel.IsChecked && e.PdfAnnotation is PdfLinkAnnotation)
    {
      if (e.ChangeButton != MouseButton.Right)
        return;
      e.Handled = true;
    }
    else
    {
      if (this.VM.AnnotationToolbar.LinkButtonModel.IsChecked && !(e.PdfAnnotation is PdfLinkAnnotation) || this.EditingPageObjectType != PageObjectType.None)
        return;
      if (e.ChangeButton == MouseButton.Left)
      {
        this.PdfViewer.DeselectText();
        if (e.PdfAnnotation is PdfFileAttachmentAnnotation)
          return;
        e.Handled = true;
        this.holders.Select(e.PdfAnnotation, false);
        this.focusControl.Annotation = (PdfAnnotation) null;
        this.UpdateHoverAnnotationBorder();
      }
      else
      {
        if (e.ChangeButton != MouseButton.Right)
          return;
        e.Handled = true;
        this.PdfViewer.DeselectText();
        this.holders.Select(e.PdfAnnotation, false);
        this.focusControl.Annotation = (PdfAnnotation) null;
        this.UpdateHoverAnnotationBorder();
        if (e.PdfAnnotation is PdfLinkAnnotation)
          return;
        if (await this.annotationContextMenuHolder.ShowAsync() || !(e.PdfAnnotation is PdfFileAttachmentAnnotation))
          return;
        e.Handled = true;
      }
    }
  }

  private async void DoubleClickHelper_MouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    AnnotationCanvas annotationCanvas = this;
    if (annotationCanvas.holders.IsAnnotationDoubleClicked((MouseEventArgs) e))
    {
      PdfAnnotation selectedAnnotation = annotationCanvas.holders.SelectedAnnotation;
      if (!((PdfWrapper) selectedAnnotation != (PdfWrapper) null))
        return;
      if (selectedAnnotation is PdfStampAnnotation pdfStampAnnotation && StampUtil.IsTextStamp(pdfStampAnnotation))
      {
        string stampTextContent = StampUtil.GetStampTextContent(pdfStampAnnotation);
        string str = "";
        if (pdfStampAnnotation.Color.A != 0)
        {
          object[] objArray = new object[4]
          {
            (object) pdfStampAnnotation.Color.A,
            null,
            null,
            null
          };
          FS_COLOR color = pdfStampAnnotation.Color;
          objArray[1] = (object) color.R;
          color = pdfStampAnnotation.Color;
          objArray[2] = (object) color.G;
          color = pdfStampAnnotation.Color;
          objArray[3] = (object) color.B;
          str = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", objArray);
        }
        StampEditWin stampEditWin = new StampEditWin(new StampTextModel()
        {
          FontColor = str,
          TextContent = stampTextContent ?? ""
        });
        if (stampEditWin.ShowDialog().GetValueOrDefault())
        {
          using (annotationCanvas.VM.OperationManager.TraceAnnotationChange(pdfStampAnnotation.Page))
          {
            FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(stampEditWin.StampTextModel.FontColor)).ToPdfColor();
            string textContent = stampEditWin.StampTextModel.TextContent;
            StampIconNames iconName;
            if (StampUtil.TryGetStandardIconName(stampEditWin.StampTextModel.TextContent, out iconName))
              pdfStampAnnotation.StandardIconName = iconName;
            else
              pdfStampAnnotation.ExtendedIconName = textContent;
            StampAnnotationHolder.SetDefaultTextStampContent(pdfStampAnnotation, textContent);
            pdfStampAnnotation.Color = pdfColor;
          }
          pdfStampAnnotation.TryRedrawAnnotation();
          annotationCanvas.VM.PageEditors.NotifyPageAnnotationChanged(pdfStampAnnotation.Page.PageIndex);
        }
      }
      if (annotationCanvas.popupHolder.IsPopupVisible(selectedAnnotation) || !annotationCanvas.popupHolder.TryShowPopup(selectedAnnotation))
        return;
      e.Handled = true;
    }
    else
    {
      PdfAnnotation pointAnnotation = annotationCanvas.PdfViewer.GetPointAnnotation(e.GetPosition((IInputElement) annotationCanvas.PdfViewer), out int _);
      if (pointAnnotation is PdfFileAttachmentAnnotation attachAnnot)
      {
        CommomLib.Commom.GAManager.SendEvent("PDFAttachment", "Open", "Count", 1L);
        bool flag = true;
        if (!AttachmentFileUtils.IsUrl(attachAnnot?.FileSpecification))
        {
          if (string.IsNullOrEmpty(await AttachmentFileUtils.ExtraAttachmentFromAnnotation(attachAnnot)))
            flag = false;
        }
        if (flag)
        {
          annotationCanvas.tooltipService?.HideTooltip();
          if (CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.AnnotationFileAttachmentOpenWarning, UtilManager.GetProductName(), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
          {
            int num = await AttachmentFileUtils.OpenAttachmentFromAnnotation(attachAnnot).ConfigureAwait(false) ? 1 : 0;
          }
        }
        e.Handled = true;
      }
      else
      {
        if (pointAnnotation is PdfLinkAnnotation pdfLinkAnnotation && annotationCanvas.VM.AnnotationToolbar.LinkButtonModel.IsChecked)
        {
          CommomLib.Commom.GAManager.SendEvent("PDFLink", "Editor", "Count", 1L);
          float docZoom = Ioc.Default.GetRequiredService<MainViewModel>().ViewToolbar.DocZoom;
          LinkAnnotationUtils.LinkAnnotationop(pdfLinkAnnotation, annotationCanvas.Document, pointAnnotation.Page, docZoom, annotationCanvas.VM);
        }
        // ISSUE: reference to a compiler-generated method
        await annotationCanvas.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(annotationCanvas.\u003CDoubleClickHelper_MouseDoubleClick\u003Eb__100_0));
        attachAnnot = (PdfFileAttachmentAnnotation) null;
      }
    }
  }

  private void Viewer_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateHoverAnnotationBorder();
    this.UpdateViewerFlyoutExtendWidth();
  }

  private void Viewer_ScrollOwnerChanged(object sender, EventArgs e)
  {
    if (this.scrollViewer != null)
    {
      MouseMiddleButtonScrollExtensions.SetIsEnabled(this.scrollViewer, false);
      MouseMiddleButtonScrollExtensions.SetShowCursorAtStartPoint(this.scrollViewer, false);
      this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollOwner_ScrollChanged);
    }
    this.scrollViewer = this.PdfViewer?.ScrollOwner;
    if (this.scrollViewer != null)
    {
      MouseMiddleButtonScrollExtensions.SetIsEnabled(this.scrollViewer, true);
      MouseMiddleButtonScrollExtensions.SetShowCursorAtStartPoint(this.scrollViewer, true);
      this.scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ScrollOwner_ScrollChanged);
    }
    this.UpdateHoverAnnotationBorder();
  }

  public void UpdateViewerFlyoutExtendWidth()
  {
    if (this.PdfViewer == null)
      return;
    DpiScale dpi = VisualTreeHelper.GetDpi((Visual) this);
    this.PdfViewer.FlyoutExtentWidth = this.popupHolder.GetMaxPopupWidth() * dpi.PixelsPerDip + 20.0;
  }

  private void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "HorizontalScrollBarHeight") && !(e.PropertyName == "VerticalScrollBarWidth"))
      return;
    this.UpdateViewerFlyoutExtendWidth();
  }

  private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    if (Mouse.LeftButton == MouseButtonState.Pressed)
      this.viewerDragged = true;
    this.UpdateHoverAnnotationBorder();
    if (e.ExtentWidthChange == 0.0)
      return;
    this.UpdateViewerFlyoutExtendWidth();
  }

  private void Viewer_BeforeDocumentChanged(object sender, DocumentClosingEventArgs e)
  {
    this.SelectedAnnotation = (PdfAnnotation) null;
    this.focusControl.Annotation = (PdfAnnotation) null;
    this.UpdateHoverAnnotationBorder();
    this.popupHolder.ClearAnnotationPopup();
  }

  private void Viewer_AfterDocumentChanged(object sender, EventArgs e)
  {
    this.popupHolder.InitAnnotationPopup(this.PdfViewer.Document?.Pages?.CurrentPage);
  }

  private void Viewer_CurrentPageChanged(object sender, EventArgs e)
  {
    this.popupHolder.ClearAnnotationPopup();
    if (this.PdfViewer.Document == null)
      return;
    PdfPage page = (PdfPage) null;
    try
    {
      page = this.PdfViewer.CurrentPage;
    }
    catch
    {
      CommomLib.Commom.GAManager.SendEvent("AnnotCanvas", "PageChanged", "Crash", 1L);
    }
    if (page == null)
      return;
    this.popupHolder.InitAnnotationPopup(page);
    IAnnotationHolder currentHolder = this.holders.CurrentHolder;
    if ((currentHolder != null ? (!currentHolder.IsTextMarkupAnnotation ? 1 : 0) : 0) != 0 && this.holders.Stamp.State != AnnotationHolderState.CreatingNew)
      this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.holders.CancelAll()));
    this.textObjectHolder.CancelTextObject();
  }

  private void Viewer_MouseModeChanged(object sender, EventArgs e)
  {
    this.PdfViewer?.DeselectText();
    this.holders.CancelAll();
    this.UpdateHoverAnnotationBorder();
  }

  private void Viewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    this.pdfViewerAutoScrollHelper?.Pause(200);
  }

  private async void Viewer_BeforeLinkClicked(object sender, PdfBeforeLinkClickedEventArgs e)
  {
    if (e.Link?.Action == null)
      return;
    switch (e.Link.Action.ActionType)
    {
      case ActionTypes.CurrentDoc:
      case ActionTypes.Uri:
      case ActionTypes.Application:
        if (e.Link.Action.ActionType != ActionTypes.Uri && e.Link.Action.ActionType != ActionTypes.Application)
          break;
        this.tooltipService.HideTooltip();
        if (CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkActionToUri.Replace("XXX", LinkAnnotationUtils.GetLinkUrlOrFileName(e.Link)), UtilManager.GetProductName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
        {
          e.Cancel = true;
          break;
        }
        if (!(e.Link.Action is PdfLaunchAction action))
          break;
        try
        {
          if (await AttachmentFileUtils.OpenFileSpecAsync(action?.FileSpecification))
            break;
          int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkOpenFileFailed, UtilManager.GetProductName());
          break;
        }
        catch
        {
          int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkOpenFileFailed, UtilManager.GetProductName());
          break;
        }
      default:
        e.Cancel = true;
        break;
    }
  }

  public async Task StartScreenShotAsync(ScreenshotDialogMode mode)
  {
    AnnotationCanvas annotationCanvas = this;
    if (annotationCanvas.Document == null)
      ;
    else
    {
      if (annotationCanvas.screenshotDialog.Visibility == Visibility.Visible)
        annotationCanvas.screenshotDialog.Close();
      annotationCanvas.Margin = new Thickness();
      ScreenshotDialogResult result = await annotationCanvas.screenshotDialog.ShowDialogAsync(mode);
      if (result == null)
        ;
      else if (result.Mode == ScreenshotDialogMode.ExtractText || result.Mode == ScreenshotDialogMode.Ocr)
      {
        if (!result.Completed)
          ;
        else
        {
          ExtractTextResultDialog textResultDialog = new ExtractTextResultDialog(annotationCanvas.Document, result);
          textResultDialog.Owner = (Window) Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
          textResultDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
          textResultDialog.ShowDialog();
        }
      }
      else if (result.Mode == ScreenshotDialogMode.Screenshot)
      {
        if (!result.Completed)
          ;
        else
          (Application.Current.Windows.OfType<ComparisonWindow>().FirstOrDefault<ComparisonWindow>() ?? new ComparisonWindow()).SetContent(annotationCanvas.Document, result);
      }
      else if (result.Mode != ScreenshotDialogMode.CropPage)
        ;
      else if (!result.Completed)
        ;
      else if (result.ApplyPageIndex == null)
        ;
      else
      {
        CommomLib.Commom.GAManager.SendEvent("CropPage", "DoCrop", "Count", 1L);
        List<(int, FS_RECTF, FS_RECTF)> list = new List<(int, FS_RECTF, FS_RECTF)>();
        foreach (int index in result.ApplyPageIndex)
        {
          PdfPage page = annotationCanvas.Document.Pages[index];
          FS_RECTF fsRectf = result.BeforeRect.Value;
          FS_RECTF selectedRect = result.SelectedRect;
          list.Add((index, fsRectf, selectedRect));
          page.SetPageCropBox(result.SelectedRect);
          page.ReloadPage();
          annotationCanvas.VM.ViewToolbar.DocSizeMode = SizeModes.Zoom;
          annotationCanvas.VM.ViewToolbar.DocZoom = 1f;
          annotationCanvas.VM.PageEditors.FlushViewerAndThumbnail();
          annotationCanvas.PdfViewer.UpdateLayout();
          annotationCanvas.PdfViewer.UpdateDocLayout();
          annotationCanvas.PdfViewer.TryRedrawVisiblePageAsync();
        }
        annotationCanvas.VM.OperationManager.AddOperationAsync((Func<PdfDocument, Task>) (async doc =>
        {
          foreach ((int index, FS_RECTF boxSize, FS_RECTF _) in list)
          {
            PdfPage page = doc.Pages[index];
            page.SetPageCropBox(boxSize);
            page.ReloadPage();
          }
          this.VM.PageEditors.FlushViewerAndThumbnail();
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
          if (pdfControl == null)
            return;
          pdfControl.UpdateLayout();
          pdfControl.UpdateDocLayout();
          await pdfControl.TryRedrawVisiblePageAsync();
        }), (Func<PdfDocument, Task>) (async doc =>
        {
          foreach ((int index, FS_RECTF _, FS_RECTF boxSize) in list)
          {
            PdfPage page = doc.Pages[index];
            page.SetPageCropBox(boxSize);
            page.ReloadPage();
          }
          this.VM.PageEditors.FlushViewerAndThumbnail();
          PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
          if (pdfControl == null)
            return;
          pdfControl.UpdateLayout();
          pdfControl.UpdateDocLayout();
          await pdfControl.TryRedrawVisiblePageAsync();
        }));
      }
    }
  }

  public void CloseScreenShot()
  {
    if (this.screenshotDialog.Visibility != Visibility.Visible)
      return;
    this.screenshotDialog.Close();
  }

  private void ScreenshotDialog_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    ScreenshotDialog screenshotDialog = this.screenshotDialog;
    if ((screenshotDialog != null ? (screenshotDialog.Visibility != 0 ? 1 : 0) : 1) == 0)
      return;
    this.UpdateViewerFlyoutExtendWidth();
    EventHandler screenshotDialogClosed = this.ScreenshotDialogClosed;
    if (screenshotDialogClosed == null)
      return;
    screenshotDialogClosed((object) this, EventArgs.Empty);
  }

  private bool isAllowClick()
  {
    return this.VM.AnnotationMode != AnnotationMode.Stamp || DateTime.Now.Subtract(this.VM.AnnotationToolbar.StampImgFileOkTime).TotalSeconds >= 1.0;
  }

  public void UpdateHoverPageObjectRect(Rect rect)
  {
    if (rect.IsEmpty || rect.Width == 0.0 || rect.Height == 0.0)
    {
      this.textObjRect.Visibility = Visibility.Collapsed;
    }
    else
    {
      this.textObjRect.Visibility = Visibility.Visible;
      Panel.SetZIndex((UIElement) this.textObjRect, 2);
      Canvas.SetLeft((UIElement) this.textObjRect, rect.Left - 1.0);
      Canvas.SetTop((UIElement) this.textObjRect, rect.Top - 1.0);
      this.textObjRect.Width = rect.Width + 2.0;
      this.textObjRect.Height = rect.Height + 2.0;
    }
  }

  public void UpdateAutoScrollHelper()
  {
    this.pdfViewerAutoScrollHelper?.Dispose();
    this.pdfViewerAutoScrollHelper = (PdfViewerAutoScrollHelper) null;
    if (this.VM.ViewToolbar?.AutoScrollButtonModel != null)
      this.VM.ViewToolbar.AutoScrollButtonModel.IsChecked = false;
    if (this.PdfViewer == null)
      return;
    this.pdfViewerAutoScrollHelper = new PdfViewerAutoScrollHelper(this.PdfViewer)
    {
      Speed = (double) this.AutoScrollSpeed
    };
  }

  public void TryShowTextObjectContextMenu() => this.textObjectContextMenuHolder?.ShowAsync();

  public event EventHandler ScreenshotDialogClosed;
}
