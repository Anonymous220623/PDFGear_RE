// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationPopupControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Utils;
using pdfeditor.Utils.Behaviors;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class AnnotationPopupControl : UserControl, IComponentConnector
{
  private readonly AnnotationCanvas annotationCanvas;
  private PopupAnnotationWrapper wrapper;
  private MainViewModel mainViewModel;
  internal Border LayoutRoot;
  internal ResizeView DragResizeView;
  internal Grid TitleBar;
  internal TextBlock ModificationDateText;
  internal TextBlock ModificationDateTextShort;
  internal Grid TextContent;
  internal TextBox TextContentBox;
  internal TextBoxEditBehavior PopupContentTextBehavior;
  private bool _contentLoaded;

  public AnnotationPopupControl(AnnotationCanvas annotationCanvas, PopupAnnotationWrapper wrapper)
  {
    this.annotationCanvas = annotationCanvas;
    this.wrapper = wrapper ?? throw new ArgumentNullException(nameof (wrapper));
    this.InitializeComponent();
    this.LayoutRoot.DataContext = (object) wrapper;
    this.PopupContentTextBehavior.Text = wrapper.Contents;
    this.Loaded += new RoutedEventHandler(this.AnnotationPopupControl_Loaded);
    this.SizeChanged += new SizeChangedEventHandler(this.AnnotationPopupControl_SizeChanged);
  }

  public PopupAnnotationWrapper Wrapper => this.wrapper;

  private void Button_Click(object sender, RoutedEventArgs e) => this.wrapper.IsOpen = false;

  private void ResizeView_ResizeDragStarted(object sender, ResizeViewResizeDragStartedEventArgs e)
  {
    Panel.SetZIndex((UIElement) this.DragResizeView, 1);
  }

  private void ResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    if (!this.IsLoaded || this.mainViewModel?.Document == null)
      return;
    Panel.SetZIndex((UIElement) this.DragResizeView, 0);
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    double x = left + e.OffsetX;
    double y = top + e.OffsetY;
    DpiScale dpi = VisualTreeHelper.GetDpi((Visual) this);
    Point pagePoint;
    if (!this.annotationCanvas.PdfViewer.TryGetPagePoint(this.wrapper.Page.PageIndex, new Point(x, y), out pagePoint))
      return;
    FS_RECTF rectangle = this.wrapper.Rectangle;
    FS_RECTF fsRectf = new FS_RECTF(pagePoint.X, pagePoint.Y, pagePoint.X + (double) rectangle.Width, pagePoint.Y - (double) rectangle.Height);
    if (e.Operation != ResizeViewOperation.Move)
    {
      double num1 = Math.Max(10.0, e.NewSize.Width / dpi.PixelsPerDip);
      double num2 = Math.Max(10.0, e.NewSize.Height / dpi.PixelsPerDip);
      fsRectf.right = fsRectf.left + (float) num1;
      fsRectf.bottom = fsRectf.top - (float) num2;
    }
    using (this.mainViewModel.OperationManager.TraceAnnotationChange(this.wrapper.Page))
      this.wrapper.Rectangle = fsRectf;
    if (this.Parent is PopupAnnotationCollection parent)
      parent.UpdatePosition();
    this.annotationCanvas.UpdateViewerFlyoutExtendWidth();
  }

  private void TextBoxEditBehavior_TextChanged(object sender, EventArgs e)
  {
    if (this.mainViewModel?.Document == null || ((TextBoxEditBehavior) sender).Text == this.wrapper.Contents)
      return;
    using (this.mainViewModel.OperationManager.TraceAnnotationChange(this.wrapper.Page))
      this.wrapper.Contents = ((TextBoxEditBehavior) sender).Text;
    this.mainViewModel.PageEditors?.NotifyPageAnnotationChanged(this.wrapper?.Annotation?.Page?.PageIndex ?? -1);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    this.annotationCanvas.PopupHolder.SetPopupHovered((PdfAnnotation) this.wrapper.Annotation, true);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this.annotationCanvas.PopupHolder.SetPopupHovered((PdfAnnotation) this.wrapper.Annotation, false);
  }

  protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
  {
    base.OnMouseDoubleClick(e);
    if (typeof (TextBoxBase).IsAssignableFrom(e.OriginalSource.GetType()))
      return;
    this.annotationCanvas.HolderManager.Select(this.wrapper.Parent, false);
  }

  private void AnnotationPopupControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.mainViewModel = this.DataContext as MainViewModel;
  }

  private void AnnotationPopupControl_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (e.NewSize.Width < 170.0)
    {
      this.ModificationDateTextShort.Visibility = Visibility.Visible;
      this.ModificationDateText.Visibility = Visibility.Collapsed;
    }
    else
    {
      this.ModificationDateTextShort.Visibility = Visibility.Collapsed;
      this.ModificationDateText.Visibility = Visibility.Visible;
    }
  }

  public void Apply() => this.PopupContentTextBehavior.Apply();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/annotationpopupcontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Border) target;
        break;
      case 2:
        this.DragResizeView = (ResizeView) target;
        break;
      case 3:
        this.TitleBar = (Grid) target;
        break;
      case 4:
        this.ModificationDateText = (TextBlock) target;
        break;
      case 5:
        this.ModificationDateTextShort = (TextBlock) target;
        break;
      case 6:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
        break;
      case 7:
        this.TextContent = (Grid) target;
        break;
      case 8:
        this.TextContentBox = (TextBox) target;
        break;
      case 9:
        this.PopupContentTextBehavior = (TextBoxEditBehavior) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
