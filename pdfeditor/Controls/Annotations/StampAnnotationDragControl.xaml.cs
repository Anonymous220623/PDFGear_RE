// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.StampAnnotationDragControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using PDFKit.Utils.StampUtils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class StampAnnotationDragControl : 
  UserControl,
  IAnnotationControl<PdfStampAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  internal Canvas LayoutRoot;
  internal Rectangle AnnotationDrag;
  internal ResizeView DragResizeView;
  private bool _contentLoaded;

  public StampAnnotationDragControl(PdfStampAnnotation annot, IAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot;
    this.Holder = holder;
    this.Loaded += new RoutedEventHandler(this.StampAnnotationDragControl_Loaded);
    if (!StampUtil.IsFormControl(annot))
      return;
    this.DragResizeView.DragMode = ResizeViewOperation.ResizeCornerAndMove;
    this.DragResizeView.IsCompactMode = true;
  }

  private void StampAnnotationDragControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.OnPageClientBoundsChanged();
  }

  public PdfStampAnnotation Annotation { get; }

  public IAnnotationHolder Holder { get; }

  public AnnotationCanvas ParentCanvas => (AnnotationCanvas) this.Parent;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  private void ResizeView_ResizeDragStarted(object sender, ResizeViewResizeDragStartedEventArgs e)
  {
    if (e.Operation == ResizeViewOperation.Move)
    {
      this.DragResizeView.DragPlaceholderFill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 51, (byte) 0, (byte) 122, (byte) 204));
      this.DragResizeView.BorderBrush = (Brush) Brushes.Transparent;
    }
    else
    {
      this.DragResizeView.DragPlaceholderFill = (Brush) Brushes.Transparent;
      this.DragResizeView.BorderBrush = (Brush) new SolidColorBrush(Color.FromArgb((byte) 51, (byte) 0, (byte) 122, (byte) 204));
    }
  }

  private async void ResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    StampAnnotationDragControl element = this;
    element.DragResizeView.BorderBrush = (Brush) Brushes.Transparent;
    Canvas.SetLeft((UIElement) element.AnnotationDrag, 0.0);
    Canvas.SetTop((UIElement) element.AnnotationDrag, 0.0);
    PdfPage page;
    if (!(element.DataContext is MainViewModel dataContext))
    {
      page = (PdfPage) null;
    }
    else
    {
      double left = Canvas.GetLeft((UIElement) element);
      double top = Canvas.GetTop((UIElement) element);
      element.LayoutRoot.Width = e.NewSize.Width;
      element.LayoutRoot.Height = e.NewSize.Height;
      double length1 = left + e.OffsetX;
      double length2 = top + e.OffsetY;
      Canvas.SetLeft((UIElement) element, length1);
      Canvas.SetTop((UIElement) element, length2);
      // ISSUE: explicit non-virtual call
      PdfViewer pdfViewer = __nonvirtual (element.ParentCanvas).PdfViewer;
      // ISSUE: explicit non-virtual call
      page = __nonvirtual (element.Annotation)?.Page;
      if (pdfViewer == null)
        page = (PdfPage) null;
      else if (page == null)
      {
        page = (PdfPage) null;
      }
      else
      {
        // ISSUE: explicit non-virtual call
        PdfPageObjectsCollection normalAppearance = __nonvirtual (element.Annotation).NormalAppearance;
        PdfImageObject pdfImageObject = normalAppearance != null ? normalAppearance.OfType<PdfImageObject>().FirstOrDefault<PdfImageObject>() : (PdfImageObject) null;
        FS_RECTF? newRect = element.GetNewRect();
        if (newRect.HasValue)
        {
          // ISSUE: explicit non-virtual call
          using (dataContext.OperationManager.TraceAnnotationChange(__nonvirtual (element.Annotation).Page))
          {
            // ISSUE: explicit non-virtual call
            __nonvirtual (element.Annotation).Opacity = 1f;
            // ISSUE: explicit non-virtual call
            __nonvirtual (element.Annotation).Rectangle = newRect.Value;
          }
        }
        if (pdfImageObject != null)
        {
          await page.TryRedrawPageAsync();
          // ISSUE: reference to a compiler-generated method
          element.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(element.\u003CResizeView_ResizeDragCompleted\u003Eb__13_0));
        }
        else
        {
          // ISSUE: explicit non-virtual call
          __nonvirtual (element.Annotation).TryRedrawAnnotation();
          // ISSUE: explicit non-virtual call
          __nonvirtual (element.OnPageClientBoundsChanged());
        }
        // ISSUE: explicit non-virtual call
        if (__nonvirtual (element.Annotation).GetRECT().IntersectsWith(new FS_RECTF(0.0f, page.Height, page.Width, 0.0f)))
        {
          page = (PdfPage) null;
        }
        else
        {
          // ISSUE: explicit non-virtual call
          __nonvirtual (element.Holder).Cancel();
          page = (PdfPage) null;
        }
      }
    }
  }

  private FS_RECTF? GetNewRect()
  {
    PdfViewer pdfViewer = this.ParentCanvas?.PdfViewer;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    double width = this.LayoutRoot.Width;
    double height = this.LayoutRoot.Height;
    if (width == 0.0 || height == 0.0)
      return new FS_RECTF?();
    FS_RECTF pageRect;
    return pdfViewer.TryGetPageRect(this.Annotation.Page.PageIndex, new Rect(left, top, width, height), out pageRect) ? new FS_RECTF?(pageRect) : new FS_RECTF?();
  }

  public void OnPageClientBoundsChanged()
  {
    object dataContext = this.DataContext;
    Rect clientRect;
    if (!this.ParentCanvas.PdfViewer.TryGetClientRect(this.Annotation.Page.PageIndex, this.Annotation.GetRECT(), out clientRect))
      return;
    this.AnnotationDrag.Width = clientRect.Width;
    this.AnnotationDrag.Height = clientRect.Height;
    this.LayoutRoot.Width = clientRect.Width;
    this.LayoutRoot.Height = clientRect.Height;
    Canvas.SetLeft((UIElement) this, clientRect.Left);
    Canvas.SetTop((UIElement) this, clientRect.Top);
    this.ResetDraggers();
  }

  private void ResetDraggers()
  {
    this.DragResizeView.Width = this.LayoutRoot.ActualWidth;
    this.DragResizeView.Height = this.LayoutRoot.ActualHeight;
  }

  public bool OnPropertyChanged(string propertyName) => false;

  private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ResetDraggers();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/stampannotationdragcontrol.xaml", UriKind.Relative));
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
        this.AnnotationDrag = (Rectangle) target;
        break;
      case 3:
        this.DragResizeView = (ResizeView) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
