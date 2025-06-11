// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationFreeTextEditor
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Models.Annotations;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using PDFKit.Utils.PdfRichTextStrings;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class AnnotationFreeTextEditor : 
  UserControl,
  IAnnotationControl<PdfFreeTextAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  private ResizeViewOperation dragModeWithoutMove;
  private DateTime createTime;
  private RichTextBox rtb;
  private ScrollViewer rtbSv;
  private FS_RECTF curRect;
  private PdfDefaultAppearance da;
  private ScaleTransform rtbTrans;
  private bool boundsChanged;
  private bool sizeChanged;
  private bool textChanged;
  private FreeTextAnnotation annotModel;
  private PdfPage page;
  private bool gotoEditingFromOuter;
  private ScrollViewer sv;
  private PageRotate actualRotate;
  private double borderWidth;
  private bool sizeChanging;
  private bool firstSizeChanged = true;
  internal Grid LayoutRoot;
  internal Canvas RichTextBoxContainer;
  internal Border RichTextBoxBorder;
  internal Border TextPlaceholderContainer;
  internal TextBlock TextAnnotionPlaceholder;
  internal Canvas DraggerCanvas;
  internal ResizeView DraggerResizeView;
  private bool _contentLoaded;

  public AnnotationFreeTextEditor(PdfFreeTextAnnotation annot, FreeTextAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot ?? throw new ArgumentNullException(nameof (annot));
    this.Holder = (IAnnotationHolder) (holder ?? throw new ArgumentNullException(nameof (holder)));
    this.curRect = this.Annotation.GetRECT();
    this.Loaded += new RoutedEventHandler(this.AnnotationFreeTextEditor_Loaded);
    this.page = this.Annotation.Page;
    this.annotModel = (FreeTextAnnotation) AnnotationFactory.Create((PdfAnnotation) this.Annotation);
    this.createTime = DateTime.Now;
    this.rtb = RichTextBoxUtils.CreateRichTextBox(this.Annotation);
    if (string.IsNullOrEmpty(this.Annotation.Contents))
    {
      if (this.Annotation.Intent != AnnotationIntent.FreeTextTypeWriter)
      {
        PdfBorderStyle borderStyle = this.Annotation.BorderStyle;
        if (borderStyle != null)
        {
          double width = (double) borderStyle.Width;
        }
      }
      this.TextPlaceholderContainer.Visibility = Visibility.Visible;
    }
    PageRotate adjustRotate;
    double rotate = (double) annot.GetRotate(out PageRotate? _, out adjustRotate);
    this.actualRotate = PdfRotateUtils.AnnotRotation(adjustRotate, annot.Page.Rotation);
    if (this.actualRotate != PageRotate.Normal)
      this.RichTextBoxContainer.RenderTransform = (Transform) new RotateTransform((double) (360 - (int) this.actualRotate * 90));
    if (annot.Intent == AnnotationIntent.FreeTextTypeWriter)
    {
      this.dragModeWithoutMove = this.actualRotate == PageRotate.Normal || this.actualRotate == PageRotate.Rotate180 ? ResizeViewOperation.LeftCenter | ResizeViewOperation.RightCenter : ResizeViewOperation.CenterTop | ResizeViewOperation.CenterBottom;
      this.DraggerResizeView.BorderBrush = (Brush) new SolidColorBrush(Color.FromArgb((byte) 60, (byte) 0, (byte) 0, (byte) 0));
      this.RichTextBoxBorder.Opacity = 0.0;
      this.TextAnnotionPlaceholder.Margin = new Thickness(6.0, 2.5, 0.0, 0.0);
      this.TextAnnotionPlaceholder.VerticalAlignment = VerticalAlignment.Center;
      this.TextAnnotionPlaceholder.TextWrapping = TextWrapping.NoWrap;
    }
    else
      this.dragModeWithoutMove = ResizeViewOperation.ResizeAll;
    this.DraggerResizeView.DragMode = this.dragModeWithoutMove | ResizeViewOperation.Move;
  }

  public PdfFreeTextAnnotation Annotation { get; }

  public IAnnotationHolder Holder { get; }

  public AnnotationCanvas ParentCanvas => (AnnotationCanvas) this.Parent;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  public RichTextBox GetRichTextBox() => this.rtb;

  private void SetBoundsChangedFlag()
  {
    if (this.boundsChanged)
      return;
    this.boundsChanged = true;
  }

  public async void OnPageClientBoundsChanged()
  {
    AnnotationFreeTextEditor element = this;
    PdfViewer viewer;
    if (element.rtb == null)
      viewer = (PdfViewer) null;
    else if (element.rtbTrans == null)
    {
      viewer = (PdfViewer) null;
    }
    else
    {
      // ISSUE: explicit non-virtual call
      viewer = __nonvirtual (element.ParentCanvas)?.PdfViewer;
      if (!element.IsLoaded || viewer == null)
      {
        element.Opacity = 0.0;
        element.IsHitTestVisible = false;
        viewer = (PdfViewer) null;
      }
      else
      {
        Size oldSize = new Size(element.rtb.ActualWidth, element.rtb.ActualHeight);
        await element.ExtendRichTextBoxAsync();
        if (oldSize != new Size(element.rtb.ActualWidth, element.rtb.ActualHeight))
          element.UpdateCurrentRect();
        float r = Math.Min(100f, element.page.Width);
        Rect clientRect1;
        if (!viewer.TryGetClientRect(element.page.PageIndex, new FS_RECTF(0.0f, 1f, r, 0.0f), out clientRect1))
        {
          viewer = (PdfViewer) null;
        }
        else
        {
          double num1 = element.page.Rotation == PageRotate.Normal || element.page.Rotation == PageRotate.Rotate180 ? clientRect1.Width / ((double) r * 96.0 / 72.0) : clientRect1.Height / ((double) r * 96.0 / 72.0);
          element.rtbTrans.ScaleX = num1;
          element.rtbTrans.ScaleY = num1;
          PdfBorderStyleModel borderStyle = element.annotModel.BorderStyle;
          float num2 = (float) ((borderStyle != null ? (double) borderStyle.Width : 1.0) * 96.0 / 72.0);
          element.borderWidth = (double) num2 * num1;
          element.RichTextBoxBorder.BorderThickness = new Thickness(element.borderWidth);
          double val1_1 = element.rtb.Width * num1 + element.borderWidth * 2.0;
          double val1_2 = element.rtb.Height * num1 + element.borderWidth * 2.0;
          Rect clientRect2;
          if (viewer.TryGetClientRect(element.page.PageIndex, element.curRect, out clientRect2))
          {
            if (Math.Abs(clientRect2.Width - val1_1) / num1 > 0.2)
              element.RichTextBoxBorder.Width = Math.Max(val1_1, 0.0);
            else
              element.RichTextBoxBorder.Width = clientRect2.Width;
            if (Math.Abs(clientRect2.Height - val1_2) / num1 > 0.2)
              element.RichTextBoxBorder.Height = Math.Max(val1_2, 0.0);
            else
              element.RichTextBoxBorder.Height = clientRect2.Height;
            double num3 = 0.0;
            double num4 = 0.0;
            if (num1 > 0.0)
            {
              num3 = element.RichTextBoxBorder.Width / num1;
              num4 = element.RichTextBoxBorder.Height / num1;
            }
            element.TextPlaceholderContainer.Width = Math.Max(num3 - (double) num2 * 2.0, 0.0);
            element.TextPlaceholderContainer.Height = Math.Max(num4 - (double) num2 * 2.0, 0.0);
            element.UpdateRotatePosition();
            Canvas.SetLeft((UIElement) element, clientRect2.Left);
            Canvas.SetTop((UIElement) element, clientRect2.Top);
          }
          element.ResetDraggers();
          viewer = (PdfViewer) null;
        }
      }
    }
  }

  private void UpdateRotatePosition()
  {
    foreach (UIElement element in this.RichTextBoxContainer.Children.OfType<UIElement>())
    {
      double length = 0.0;
      if (element == this.rtb || element == this.TextPlaceholderContainer)
        length = this.borderWidth;
      if (this.actualRotate == PageRotate.Normal)
      {
        Canvas.SetTop(element, length);
        Canvas.SetLeft(element, length);
      }
      if (this.actualRotate == PageRotate.Rotate90)
      {
        Canvas.SetTop(element, length);
        Canvas.SetLeft(element, -this.RichTextBoxBorder.ActualWidth + length);
      }
      else if (this.actualRotate == PageRotate.Rotate180)
      {
        Canvas.SetLeft(element, -this.RichTextBoxBorder.ActualWidth + length);
        Canvas.SetTop(element, -this.RichTextBoxBorder.ActualHeight + length);
      }
      else if (this.actualRotate == PageRotate.Rotate270)
      {
        Canvas.SetTop(element, -this.RichTextBoxBorder.ActualHeight + length);
        Canvas.SetLeft(element, length);
      }
    }
  }

  private void ResetDraggers()
  {
    if (this.actualRotate == PageRotate.Normal || this.actualRotate == PageRotate.Rotate180)
    {
      this.DraggerCanvas.Width = this.RichTextBoxBorder.ActualWidth;
      this.DraggerCanvas.Height = this.RichTextBoxBorder.ActualHeight;
      this.DraggerResizeView.Width = this.RichTextBoxBorder.ActualWidth;
      this.DraggerResizeView.Height = this.RichTextBoxBorder.ActualHeight;
    }
    else
    {
      this.DraggerCanvas.Width = this.RichTextBoxBorder.ActualHeight;
      this.DraggerCanvas.Height = this.RichTextBoxBorder.ActualWidth;
      this.DraggerResizeView.Width = this.RichTextBoxBorder.ActualHeight;
      this.DraggerResizeView.Height = this.RichTextBoxBorder.ActualWidth;
    }
  }

  private void AnnotationFreeTextEditor_Loaded(object sender, RoutedEventArgs e)
  {
    this.sv = this.ParentCanvas?.PdfViewer?.Parent as ScrollViewer;
    this.InitRichTextBox();
    this.Annotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    this.ParentCanvas.PdfViewer.InvalidateVisual();
    this.UpdateRotatePosition();
  }

  private void InitRichTextBox()
  {
    if (!PdfDefaultAppearance.TryParse(this.annotModel.DefaultAppearance, out this.da))
      this.da = PdfDefaultAppearance.Default;
    if (this.annotModel.Color != FS_COLOR.Empty)
      this.da.FillColor = this.annotModel.Color;
    Color color1 = Colors.Black;
    Color color2 = Colors.Transparent;
    if (this.annotModel.Intent != AnnotationIntent.FreeTextTypeWriter)
    {
      color1 = this.da.StrokeColor.ToColor();
      color2 = this.da.FillColor.ToColor();
    }
    this.RichTextBoxBorder.BorderBrush = (Brush) new SolidColorBrush(color1);
    this.rtbTrans = new ScaleTransform();
    this.rtb.RenderTransform = (Transform) this.rtbTrans;
    this.TextPlaceholderContainer.RenderTransform = (Transform) this.rtbTrans;
    this.rtb.Background = (Brush) new SolidColorBrush(color2);
    this.rtb.BorderThickness = new Thickness();
    if (VisualTreeHelper.GetChild((DependencyObject) this.rtb, 0) is FrameworkElement child && child.FindName("PART_ContentHost") is ScrollViewer name)
    {
      this.rtbSv = name;
      this.rtbSv.ScrollChanged += new ScrollChangedEventHandler(this.RtbSv_ScrollChanged);
    }
    this.RichTextBoxContainer.Children.Add((UIElement) this.rtb);
    this.rtb.TextChanged += new TextChangedEventHandler(this.Rtb_TextChanged);
    this.rtb.PreviewKeyDown += new KeyEventHandler(this.Rtb_PreviewKeyDown);
    this.rtb.KeyDown += new KeyEventHandler(this.Rtb_KeyDown);
    this.rtb.SizeChanged += new SizeChangedEventHandler(this.Rtb_SizeChanged);
    if (this.gotoEditingFromOuter)
      this.GoToEditingCore(new Point?(new Point(0.0, 0.0)));
    else
      this.ExitEditing(false);
    this.OnPageClientBoundsChanged();
  }

  private void Rtb_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Delete)
      return;
    e.Handled = true;
  }

  private async void Rtb_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    AnnotationFreeTextEditor annotationFreeTextEditor = this;
    if (annotationFreeTextEditor.sizeChanging)
      return;
    annotationFreeTextEditor.sizeChanging = true;
    try
    {
      if (!(annotationFreeTextEditor.DataContext is MainViewModel vm))
        return;
      if (annotationFreeTextEditor.firstSizeChanged)
      {
        annotationFreeTextEditor.firstSizeChanged = false;
        await annotationFreeTextEditor.ExtendRichTextBoxAsync();
      }
      double scaleX = annotationFreeTextEditor.rtbTrans.ScaleX;
      double left = annotationFreeTextEditor.RichTextBoxBorder.BorderThickness.Left;
      double num1 = annotationFreeTextEditor.rtb.Width * scaleX + left * 2.0;
      double num2 = annotationFreeTextEditor.rtb.Height * scaleX + left * 2.0;
      if (double.IsNaN(annotationFreeTextEditor.RichTextBoxBorder.Width) || Math.Abs(annotationFreeTextEditor.RichTextBoxBorder.Width - num1) / scaleX > 0.2)
        annotationFreeTextEditor.RichTextBoxBorder.Width = num1;
      if (double.IsNaN(annotationFreeTextEditor.RichTextBoxBorder.Height) || Math.Abs(annotationFreeTextEditor.RichTextBoxBorder.Height - num2) / scaleX > 0.2)
        annotationFreeTextEditor.RichTextBoxBorder.Height = num2;
      annotationFreeTextEditor.UpdateCurrentRect();
      bool flag = annotationFreeTextEditor.sizeChanged && annotationFreeTextEditor.boundsChanged;
      // ISSUE: explicit non-virtual call
      if (!flag && __nonvirtual (annotationFreeTextEditor.Annotation).Intent == AnnotationIntent.FreeTextTypeWriter && !string.IsNullOrEmpty(annotationFreeTextEditor.GetRichTextBoxText()))
        flag = true;
      if (flag)
      {
        // ISSUE: explicit non-virtual call
        using (vm.OperationManager.TraceAnnotationChange(__nonvirtual (annotationFreeTextEditor.Annotation).Page))
        {
          // ISSUE: explicit non-virtual call
          __nonvirtual (annotationFreeTextEditor.Annotation).Rectangle = annotationFreeTextEditor.curRect;
        }
        annotationFreeTextEditor.sizeChanged = false;
      }
      vm = (MainViewModel) null;
    }
    finally
    {
      annotationFreeTextEditor.sizeChanging = false;
    }
  }

  protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
  {
    base.OnPreviewMouseWheel(e);
    ScrollViewer scrollOwner = this.ParentCanvas?.PdfViewer?.ScrollOwner;
    if (scrollOwner != null)
    {
      MouseWheelEventArgs input = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
      input.RoutedEvent = UIElement.MouseWheelEvent;
      input.Source = (object) scrollOwner;
      InputManager.Current.ProcessInput((InputEventArgs) input);
    }
    e.Handled = true;
  }

  private void RichTextBoxContainer_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (this.rtb == null)
      return;
    this.GoToEditingCore(new Point?(e.GetPosition((IInputElement) this.rtb)));
  }

  private async Task ExtendRichTextBoxAsync()
  {
    AnnotationFreeTextEditor annotationFreeTextEditor = this;
    if (!annotationFreeTextEditor.IsLoaded || annotationFreeTextEditor.rtb == null)
      return;
    annotationFreeTextEditor.rtb.SizeChanged -= new SizeChangedEventHandler(annotationFreeTextEditor.Rtb_SizeChanged);
    // ISSUE: explicit non-virtual call
    await RichTextBoxUtils.ExtendRichTextBoxAsync(annotationFreeTextEditor.rtb, __nonvirtual (annotationFreeTextEditor.Annotation).Intent == AnnotationIntent.FreeTextTypeWriter);
    lock (annotationFreeTextEditor.rtb)
    {
      annotationFreeTextEditor.rtb.SizeChanged -= new SizeChangedEventHandler(annotationFreeTextEditor.Rtb_SizeChanged);
      annotationFreeTextEditor.rtb.SizeChanged += new SizeChangedEventHandler(annotationFreeTextEditor.Rtb_SizeChanged);
    }
  }

  private void UpdateCurrentRect()
  {
    if (this.ParentCanvas == null)
      return;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    double num1 = this.RichTextBoxBorder.Width;
    double num2 = this.RichTextBoxBorder.Height;
    if (this.actualRotate == PageRotate.Rotate90 || this.actualRotate == PageRotate.Rotate270)
    {
      double num3 = num2;
      num2 = num1;
      num1 = num3;
    }
    FS_RECTF pageRect;
    if (double.IsNaN(left) || double.IsNaN(top) || double.IsNaN(num1) || double.IsNaN(num2) || !this.ParentCanvas?.PdfViewer.TryGetPageRect(this.Annotation.Page.PageIndex, new Rect(left, top, num1, num2), out pageRect) || (double) Math.Abs(this.curRect.left - pageRect.left) <= 0.8 && (double) Math.Abs(this.curRect.top - pageRect.top) <= 0.8 && (double) Math.Abs(this.curRect.right - pageRect.right) <= 0.8 && (double) Math.Abs(this.curRect.bottom - pageRect.bottom) <= 0.8)
      return;
    this.curRect = pageRect;
    this.SetBoundsChangedFlag();
  }

  private void Rtb_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Escape)
      return;
    e.Handled = true;
    this.ExitEditing(this.textChanged);
    this.textChanged = false;
  }

  private void RtbSv_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.OnPageClientBoundsChanged();
  }

  private void Rtb_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.textChanged = true;
    if (string.IsNullOrEmpty(this.GetRichTextBoxText()))
    {
      PdfViewer pdfViewer = this.ParentCanvas?.PdfViewer;
      if (!this.IsLoaded || pdfViewer == null)
        return;
      float r = Math.Min(100f, this.page.Width);
      Rect clientRect;
      if (!pdfViewer.TryGetClientRect(this.page.PageIndex, new FS_RECTF(0.0f, 1f, r, 0.0f), out clientRect))
        return;
      if (this.page.Rotation != PageRotate.Normal && this.page.Rotation != PageRotate.Rotate180)
      {
        double num1 = clientRect.Height / ((double) r * 96.0 / 72.0);
      }
      else
      {
        double num2 = clientRect.Width / ((double) r * 96.0 / 72.0);
      }
      if (this.Annotation.Intent != AnnotationIntent.FreeTextTypeWriter)
      {
        double width = (double) this.Annotation.BorderStyle.Width;
      }
      this.TextPlaceholderContainer.Visibility = Visibility.Visible;
    }
    else
      this.TextPlaceholderContainer.Visibility = Visibility.Collapsed;
  }

  private void RichTextBoxBorder_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ResetDraggers();
    this.UpdateRotatePosition();
  }

  private void DraggerResizeView_ResizeDragStarted(
    object sender,
    ResizeViewResizeDragStartedEventArgs e)
  {
    this.ExitEditing(false);
  }

  private void DraggerResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    if (!(this.DataContext is MainViewModel))
      return;
    Border richTextBoxBorder1 = this.RichTextBoxBorder;
    Size newSize = e.NewSize;
    double width1 = newSize.Width;
    richTextBoxBorder1.Width = width1;
    Border richTextBoxBorder2 = this.RichTextBoxBorder;
    newSize = e.NewSize;
    double height1 = newSize.Height;
    richTextBoxBorder2.Height = height1;
    double width2 = this.RichTextBoxBorder.Width;
    Thickness borderThickness1 = this.RichTextBoxBorder.BorderThickness;
    double left1 = borderThickness1.Left;
    double num1 = width2 - left1;
    borderThickness1 = this.RichTextBoxBorder.BorderThickness;
    double right = borderThickness1.Right;
    double num2 = num1 - right;
    double height2 = this.RichTextBoxBorder.Height;
    Thickness borderThickness2 = this.RichTextBoxBorder.BorderThickness;
    double top1 = borderThickness2.Top;
    double num3 = height2 - top1;
    borderThickness2 = this.RichTextBoxBorder.BorderThickness;
    double bottom = borderThickness2.Bottom;
    double num4 = num3 - bottom;
    if (this.actualRotate == PageRotate.Rotate90 || this.actualRotate == PageRotate.Rotate270)
    {
      double num5 = num4;
      num4 = num2;
      num2 = num5;
    }
    double val2_1 = Math.Max(0.0, num2 / this.rtbTrans.ScaleX);
    double val2_2 = Math.Max(0.0, num4 / this.rtbTrans.ScaleX);
    this.sizeChanged = false;
    if (val2_1 != this.rtb.Width || val2_2 != this.rtb.Height)
    {
      this.rtb.Width = Math.Max(0.0, val2_1);
      this.rtb.Height = Math.Max(0.0, val2_2);
      this.sizeChanged = true;
    }
    if (e.OffsetX != 0.0 || e.OffsetY != 0.0)
    {
      double left2 = Canvas.GetLeft((UIElement) this);
      double top2 = Canvas.GetTop((UIElement) this);
      Canvas.SetLeft((UIElement) this, left2 + e.OffsetX);
      Canvas.SetTop((UIElement) this, top2 + e.OffsetY);
    }
    this.UpdateCurrentRect();
    bool saveChange = this.textChanged;
    if (this.Annotation.Intent == AnnotationIntent.FreeTextTypeWriter && string.IsNullOrEmpty(this.GetRichTextBoxText()))
      saveChange = false;
    this.ExitEditing(saveChange);
    if (!this.curRect.IntersectsWith(new FS_RECTF(0.0f, this.page.Height, this.page.Width, 0.0f)))
      this.Holder.Cancel();
    else
      this.OnPageClientBoundsChanged();
  }

  private void DraggerResizeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if ((DateTime.Now - this.createTime).TotalSeconds >= 0.1)
      return;
    e.Handled = true;
    this.GoToEditingCore(new Point?(e.GetPosition((IInputElement) this.rtb)));
  }

  private void DraggerResizeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    this.GoToEditingCore(new Point?(e.GetPosition((IInputElement) this.rtb)));
  }

  public void GoToEditing()
  {
    this.gotoEditingFromOuter = true;
    this.GoToEditingCore(new Point?());
  }

  private void GoToEditingCore(Point? position)
  {
    if (this.rtb == null)
      return;
    this.DraggerResizeView.DragMode = this.dragModeWithoutMove;
    this.rtb.IsReadOnly = false;
    this.rtb.Focus();
    if (!position.HasValue)
      return;
    this.rtb.CaretPosition = this.rtb.GetPositionFromPoint(position.Value, true);
  }

  private async void ExitEditing(bool saveChange, bool applying = false)
  {
    MainViewModel vm;
    if (this.rtb == null)
      vm = (MainViewModel) null;
    else if (this.DraggerResizeView == null)
      vm = (MainViewModel) null;
    else if ((PdfWrapper) this.Annotation == (PdfWrapper) null)
    {
      vm = (MainViewModel) null;
    }
    else
    {
      this.rtb.IsReadOnly = true;
      if (this.rtb.IsFocused && !this.rtb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)))
        Keyboard.ClearFocus();
      this.DraggerResizeView.DragMode = this.dragModeWithoutMove | ResizeViewOperation.Move;
      vm = this.ParentCanvas?.DataContext as MainViewModel;
      if (saveChange | applying && this.annotModel.Intent == AnnotationIntent.FreeTextTypeWriter)
      {
        try
        {
          if (string.IsNullOrEmpty(this.GetRichTextBoxText()))
          {
            await this.ParentCanvas.HolderManager.DeleteAnnotationAsync((PdfAnnotation) this.Annotation);
            vm = (MainViewModel) null;
            return;
          }
        }
        catch
        {
        }
      }
      IDisposable disposable = (IDisposable) null;
      try
      {
        MainViewModel mainViewModel = vm;
        disposable = mainViewModel != null ? mainViewModel.OperationManager.TraceAnnotationChange(this.Annotation.Page) : (IDisposable) null;
        this.Annotation.Rectangle = this.curRect;
        if (!saveChange)
        {
          vm = (MainViewModel) null;
        }
        else
        {
          PdfRichTextStyle? defaultStyle = new PdfRichTextStyle?();
          PdfRichTextStyle pdfRichTextStyle;
          if (PdfRichTextStyle.TryParse(this.Annotation.DefaultStyle, out pdfRichTextStyle))
            defaultStyle = new PdfRichTextStyle?(pdfRichTextStyle);
          PdfRichTextString pdfRichTextString = PdfRichTextStringHelper.FromRichTextBox(this.rtb, defaultStyle, this.Annotation?.Name);
          this.Annotation.RichText = pdfRichTextString.ToString();
          if (this.Annotation.Contents != pdfRichTextString.ContentText)
          {
            this.Annotation.Contents = pdfRichTextString.ContentText;
            vm?.PageEditors?.NotifyPageAnnotationChanged(this.Annotation.Page.PageIndex);
          }
          this.Annotation.DefaultStyle = pdfRichTextString.DefaultStyle.ToString();
          vm = (MainViewModel) null;
        }
      }
      finally
      {
        disposable?.Dispose();
      }
    }
  }

  public void Apply()
  {
    if (this.rtb == null || this.rtbSv == null)
      return;
    this.UpdateCurrentRect();
    this.ExitEditing(this.textChanged, true);
    ScrollViewer sv = this.sv;
    if (sv != null)
    {
      // ISSUE: method pointer
      sv.ScrollChanged += new ScrollChangedEventHandler((object) this, __methodptr(\u003CApply\u003Eg__Sv_ScrollChanged\u007C53_0));
    }
    this.Annotation.TryRedrawAnnotation();
    if (sv == null)
      return;
    // ISSUE: method pointer
    sv.ScrollChanged -= new ScrollChangedEventHandler((object) this, __methodptr(\u003CApply\u003Eg__Sv_ScrollChanged\u007C53_0));
  }

  private string GetRichTextBoxText()
  {
    if (this.rtb == null)
      return string.Empty;
    string text = new TextRange(this.rtb.Document.ContentStart, this.rtb.Document.ContentEnd).Text;
    return string.IsNullOrEmpty(text) || text == "\r\n" || text == "\n" ? string.Empty : text;
  }

  public bool OnPropertyChanged(string propertyName)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return false;
    if (propertyName == "TextFontColor" || propertyName == "TextFontSize")
    {
      PdfFreeTextAnnotation annotation = this.Annotation;
      if ((annotation != null ? (annotation.Intent != AnnotationIntent.FreeTextTypeWriter ? 1 : 0) : 1) != 0)
        return false;
    }
    if (propertyName == "TextBoxFontColor" || propertyName == "TextBoxFontSize")
    {
      PdfFreeTextAnnotation annotation = this.Annotation;
      if ((annotation != null ? (annotation.Intent == AnnotationIntent.FreeTextTypeWriter ? 1 : 0) : 0) != 0)
        return false;
    }
    TextRange textRange = new TextRange(this.rtb.Document.ContentStart, this.rtb.Document.ContentEnd);
    PdfRichTextStyle pdfRichTextStyle;
    bool flag1 = PdfRichTextStyle.TryParse(this.Annotation.DefaultStyle, out pdfRichTextStyle);
    PdfDefaultAppearance pdfFontStyle;
    bool flag2 = PdfDefaultAppearance.TryParse(this.Annotation.DefaultAppearance, out pdfFontStyle);
    bool flag3 = false;
    switch (propertyName)
    {
      case "TextFontColor":
        Color color1 = (Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextFontColor);
        textRange.ApplyPropertyValue(Control.ForegroundProperty, (object) new SolidColorBrush(color1));
        if (flag1)
          pdfRichTextStyle.Color = new Color?(color1);
        if (flag2)
          pdfFontStyle.FillColor = color1.ToPdfColor();
        flag3 = true;
        break;
      case "TextBoxFontColor":
        Color color2 = (Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontColor);
        textRange.ApplyPropertyValue(Control.ForegroundProperty, (object) new SolidColorBrush(color2));
        if (flag1)
          pdfRichTextStyle.Color = new Color?(color2);
        if (flag2)
          pdfFontStyle.FillColor = color2.ToPdfColor();
        flag3 = true;
        break;
      case "TextFontSize":
        float textFontSize = dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextFontSize;
        double num1 = (double) textFontSize * 96.0 / 72.0;
        textRange.ApplyPropertyValue(Control.FontSizeProperty, (object) num1);
        if (flag1)
          pdfRichTextStyle.FontSize = new float?(textFontSize);
        if (flag2)
          pdfFontStyle.FontSize = textFontSize;
        flag3 = true;
        break;
      case "TextBoxFontSize":
        float textBoxFontSize = dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.TextBoxFontSize;
        double num2 = (double) textBoxFontSize * 96.0 / 72.0;
        textRange.ApplyPropertyValue(Control.FontSizeProperty, (object) num2);
        if (flag1)
          pdfRichTextStyle.FontSize = new float?(textBoxFontSize);
        if (flag2)
          pdfFontStyle.FontSize = textBoxFontSize;
        flag3 = true;
        break;
    }
    if (flag3 & flag1)
    {
      using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
      {
        this.Annotation.DefaultStyle = pdfRichTextStyle.ToString();
        this.Annotation.DefaultAppearance = pdfFontStyle.ToString();
      }
    }
    return flag3;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/annotationfreetexteditor.xaml", UriKind.Relative));
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
        this.LayoutRoot = (Grid) target;
        break;
      case 2:
        this.RichTextBoxContainer = (Canvas) target;
        this.RichTextBoxContainer.MouseDown += new MouseButtonEventHandler(this.RichTextBoxContainer_MouseDown);
        break;
      case 3:
        this.RichTextBoxBorder = (Border) target;
        this.RichTextBoxBorder.SizeChanged += new SizeChangedEventHandler(this.RichTextBoxBorder_SizeChanged);
        break;
      case 4:
        this.TextPlaceholderContainer = (Border) target;
        break;
      case 5:
        this.TextAnnotionPlaceholder = (TextBlock) target;
        break;
      case 6:
        this.DraggerCanvas = (Canvas) target;
        break;
      case 7:
        this.DraggerResizeView = (ResizeView) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
