// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationDragEditControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Services;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class AnnotationDragEditControl : 
  UserControl,
  IAnnotationControl<PdfFigureAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  private FS_RECTF curRect;
  private bool changed;
  private bool isPressed;
  internal Canvas LayoutRoot;
  internal ResizeView DragResizeView;
  internal Ellipse ResizePlaceholderEllipse;
  private bool _contentLoaded;

  public AnnotationDragEditControl(PdfFigureAnnotation annot, IAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot;
    this.Holder = holder;
    this.curRect = annot.GetRECT();
    this.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 24, (byte) 146, byte.MaxValue));
  }

  public AnnotationDragEditControl(PdfLinkAnnotation annot, IAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.LinkAnnotation = annot;
    this.Holder = holder;
    this.curRect = annot.GetRECT();
    this.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 24, (byte) 146, byte.MaxValue));
  }

  public PdfFigureAnnotation Annotation { get; }

  public PdfLinkAnnotation LinkAnnotation { get; }

  public IAnnotationHolder Holder { get; }

  public AnnotationCanvas ParentCanvas => (AnnotationCanvas) this.Parent;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  private void ResetDraggers()
  {
    this.DragResizeView.Width = this.LayoutRoot.ActualWidth;
    this.DragResizeView.Height = this.LayoutRoot.ActualHeight;
  }

  private FS_RECTF? GetNewRect()
  {
    FS_RECTF pageRect;
    if (!this.ParentCanvas?.PdfViewer.TryGetPageRect(this.Annotation.Page.PageIndex, new Rect(Canvas.GetLeft((UIElement) this), Canvas.GetTop((UIElement) this), this.LayoutRoot.Width, this.LayoutRoot.Height), out pageRect))
      return new FS_RECTF?();
    if ((double) pageRect.Width < 2.0)
      pageRect.right = pageRect.left + 2f;
    if ((double) pageRect.Height < 2.0)
      pageRect.bottom = pageRect.top - 2f;
    return new FS_RECTF?(pageRect);
  }

  private FS_RECTF? GetLinkNewRect()
  {
    FS_RECTF pageRect;
    if (!this.ParentCanvas?.PdfViewer.TryGetPageRect(this.LinkAnnotation.Page.PageIndex, new Rect(Canvas.GetLeft((UIElement) this), Canvas.GetTop((UIElement) this), this.LayoutRoot.Width, this.LayoutRoot.Height), out pageRect))
      return new FS_RECTF?();
    if ((double) pageRect.Width < 2.0)
      pageRect.right = pageRect.left + 2f;
    if ((double) pageRect.Height < 2.0)
      pageRect.bottom = pageRect.top - 2f;
    return new FS_RECTF?(pageRect);
  }

  private void ResizeView_ResizeDragStarted(object sender, ResizeViewResizeDragStartedEventArgs e)
  {
    if (this.Annotation is PdfCircleAnnotation)
      this.ResizePlaceholderEllipse.Opacity = 1.0;
    if (this.Annotation is PdfSquareAnnotation || e.Operation != ResizeViewOperation.Move || this.LinkAnnotation != null)
      this.DragResizeView.BorderThickness = new Thickness(1.0);
    else
      this.DragResizeView.BorderThickness = new Thickness();
  }

  private void DragResizeView_ResizeDragging(object sender, ResizeViewResizeDragEventArgs e)
  {
    if (!(this.Annotation is PdfCircleAnnotation))
      return;
    this.ResizePlaceholderEllipse.Width = e.NewSize.Width;
    this.ResizePlaceholderEllipse.Height = e.NewSize.Height;
  }

  private void ResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return;
    this.ResizePlaceholderEllipse.Opacity = 0.0;
    this.DragResizeView.BorderThickness = new Thickness();
    this.LayoutRoot.Width = e.NewSize.Width;
    this.LayoutRoot.Height = e.NewSize.Height;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    double length1 = left + e.OffsetX;
    double length2 = top + e.OffsetY;
    Canvas.SetLeft((UIElement) this, length1);
    Canvas.SetTop((UIElement) this, length2);
    PdfViewer pdfViewer = this.ParentCanvas.PdfViewer;
    PdfPage page1 = this.Annotation?.Page;
    PdfPage page2 = this.LinkAnnotation?.Page;
    if (pdfViewer == null || page1 == null && page2 == null)
      return;
    if ((PdfWrapper) this.Annotation != (PdfWrapper) null)
    {
      FS_RECTF? newRect = this.GetNewRect();
      if (!newRect.HasValue)
        return;
      using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
      {
        this.Annotation.Opacity = 1f;
        this.Annotation.Rectangle = newRect.Value;
      }
      this.Annotation.TryRedrawAnnotation();
      this.OnPageClientBoundsChanged();
      if (this.Annotation.GetRECT().IntersectsWith(new FS_RECTF(0.0f, page1.Height, page1.Width, 0.0f)))
        return;
      this.Holder.Cancel();
    }
    else
    {
      using (dataContext.OperationManager.TraceAnnotationChange(this.LinkAnnotation.Page))
        this.LinkAnnotation.Rectangle = this.GetLinkNewRect().Value;
      PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(page2.Document);
      if (pdfControl != null)
      {
        int? pageIndex1 = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl)?.CurrentHolder?.CurrentPage?.PageIndex;
        int pageIndex2 = page2.PageIndex;
        int num = pageIndex1.GetValueOrDefault() == pageIndex2 & pageIndex1.HasValue ? 1 : 0;
      }
      for (int index = 0; index < 3; ++index)
      {
        bool flag = page2.IsDisposed;
        if (!flag)
          flag = PdfDocumentStateService.CanDisposePage(page2);
        ProgressiveStatus progressiveStatus;
        if (!flag && PdfObjectExtensions.TryGetProgressiveStatus(page2, out progressiveStatus))
          flag = progressiveStatus != ProgressiveStatus.ToBeContinued && progressiveStatus != ProgressiveStatus.Failed;
        if (flag)
        {
          try
          {
            PageDisposeHelper.DisposePage(page2);
            PdfDocumentStateService.TryRedrawViewerCurrentPage(page2);
            break;
          }
          catch
          {
            break;
          }
        }
      }
    }
  }

  public void OnPageClientBoundsChanged()
  {
    Rect clientRect;
    if (!this.ParentCanvas.PdfViewer.TryGetClientRect(this.Annotation.Page.PageIndex, this.Annotation.GetRECT(), out clientRect))
      return;
    double num = clientRect.Width / (double) this.Annotation.GetRECT().Width;
    PdfBorderStyle borderStyle = this.Annotation.BorderStyle;
    if (borderStyle != null)
    {
      double width = (double) borderStyle.Width;
    }
    this.LayoutRoot.Width = clientRect.Width;
    this.LayoutRoot.Height = clientRect.Height;
    Canvas.SetLeft((UIElement) this, clientRect.Left);
    Canvas.SetTop((UIElement) this, clientRect.Top);
    this.ResetDraggers();
  }

  public void OnPageClientLinkBoundsChanged()
  {
    Rect clientRect;
    if (!this.ParentCanvas.PdfViewer.TryGetClientRect(this.LinkAnnotation.Page.PageIndex, this.LinkAnnotation.GetRECT(), out clientRect))
      return;
    double num = clientRect.Width / (double) this.LinkAnnotation.GetRECT().Width;
    this.LayoutRoot.Width = clientRect.Width;
    this.LayoutRoot.Height = clientRect.Height;
    Canvas.SetLeft((UIElement) this, clientRect.Left);
    Canvas.SetTop((UIElement) this, clientRect.Top);
    this.ResetDraggers();
  }

  private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ResetDraggers();
  }

  public bool OnPropertyChanged(string propertyName)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return false;
    if (this.Annotation is PdfSquareAnnotation annotation2)
    {
      if (propertyName == "ShapeFill" || propertyName == "ShapeStroke" || propertyName == "ShapeThickness")
      {
        using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
        {
          if (propertyName == "ShapeThickness")
          {
            if ((PdfWrapper) annotation2.BorderStyle == (PdfWrapper) null)
              annotation2.BorderStyle = new PdfBorderStyle();
            annotation2.BorderStyle.Width = dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeThickness;
          }
          if (propertyName == "ShapeFill")
          {
            FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeFill)).ToPdfColor();
            annotation2.InteriorColor = pdfColor;
          }
          if (propertyName == "ShapeStroke")
          {
            FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeStroke)).ToPdfColor();
            annotation2.Color = pdfColor;
          }
          annotation2.RegenerateAppearances();
        }
        annotation2.TryRedrawAnnotation();
      }
    }
    else if (this.Annotation is PdfCircleAnnotation annotation1 && (propertyName == "EllipseFill" || propertyName == "EllipseStroke" || propertyName == "EllipseThickness"))
    {
      using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
      {
        if (propertyName == "EllipseThickness")
        {
          if ((PdfWrapper) annotation1.BorderStyle == (PdfWrapper) null)
            annotation1.BorderStyle = new PdfBorderStyle();
          annotation1.BorderStyle.Width = dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseThickness;
        }
        if (propertyName == "EllipseFill")
        {
          FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseFill)).ToPdfColor();
          annotation1.InteriorColor = pdfColor;
        }
        if (propertyName == "EllipseStroke")
        {
          FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseStroke)).ToPdfColor();
          annotation1.Color = pdfColor;
        }
        annotation1.RegenerateAppearances();
      }
      annotation1.TryRedrawAnnotation();
    }
    return false;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/annotationdrageditcontrol.xaml", UriKind.Relative));
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
        this.LayoutRoot = (Canvas) target;
        this.LayoutRoot.SizeChanged += new SizeChangedEventHandler(this.LayoutRoot_SizeChanged);
        break;
      case 2:
        this.DragResizeView = (ResizeView) target;
        break;
      case 3:
        this.ResizePlaceholderEllipse = (Ellipse) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
