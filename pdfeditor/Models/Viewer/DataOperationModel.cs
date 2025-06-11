// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Viewer.DataOperationModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using pdfeditor.Controls;
using pdfeditor.Utils;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Models.Viewer;

internal class DataOperationModel : ViewerOperationModel<DataOperationModel, bool>
{
  private Border elementContainer;
  private WeakReference<AnnotationCanvas> weakAnnotCanvas;
  private UIElement previewElement;
  private bool cancelQueued;
  private DateTime createTime;

  public DataOperationModel(PdfViewer viewer)
    : base(viewer)
  {
    viewer.OverrideCursor = Cursors.SizeAll;
    this.createTime = DateTime.Now;
  }

  protected AnnotationCanvas AnnotCanvas
  {
    get
    {
      AnnotationCanvas target;
      return this.weakAnnotCanvas != null && this.weakAnnotCanvas.TryGetTarget(out target) ? target : (AnnotationCanvas) null;
    }
  }

  protected Border ElementContainer => this.elementContainer;

  public object Data { get; set; }

  public T GetData<T>() => this.Data is T data ? data : default (T);

  public UIElement PreviewElement
  {
    get => this.previewElement;
    set
    {
      if (this.previewElement == value)
        return;
      this.previewElement = value;
      this.CreatePreview();
    }
  }

  public FS_SIZEF SizeInDocument { get; set; }

  protected Point MousePositionFromViewer { get; private set; }

  public int CurrentPage { get; protected set; }

  public FS_POINTF PositionFromDocument { get; protected set; }

  protected virtual bool RotatePreviewElementWithPage => false;

  public Size GetPreviewSize(int pageIndex)
  {
    PdfViewer viewer = this.Viewer;
    FS_SIZEF sizeInDocument = this.SizeInDocument;
    Rect clientRect;
    if (viewer == null || !viewer.TryGetClientRect(pageIndex, new FS_RECTF(0.0f, sizeInDocument.Height, sizeInDocument.Width, 0.0f), out clientRect))
      return new Size(0.0, 0.0);
    PageRotate rotation;
    Pdfium.FPDF_GetPageRotationByIndex(viewer.Document.Handle, pageIndex, out rotation);
    return !this.RotatePreviewElementWithPage && (rotation == PageRotate.Rotate90 || rotation == PageRotate.Rotate270) ? new Size(clientRect.Height, clientRect.Width) : clientRect.Size;
  }

  protected virtual void CreatePreview()
  {
    if (this.IsDisposed || this.IsCompleted)
      return;
    if (this.elementContainer != null)
      this.elementContainer.Child = (UIElement) null;
    UIElement previewElement = this.PreviewElement;
    if (previewElement != null)
    {
      if (this.elementContainer == null)
      {
        Border border = new Border();
        border.IsHitTestVisible = false;
        this.elementContainer = border;
      }
      this.elementContainer.Child = previewElement;
      PdfObjectExtensions.GetAnnotationCanvas(this.Viewer);
      this.UpdateAnnotCanvas();
      this.AnnotCanvas?.Children.Add((UIElement) this.elementContainer);
    }
    this.UpdatePagePosition();
    this.UpdatePreview();
  }

  private void UpdateAnnotCanvas()
  {
    if (this.IsDisposed)
      return;
    AnnotationCanvas annotCanvas = this.AnnotCanvas;
    if (this.elementContainer != null && annotCanvas != null)
      annotCanvas.Children.Remove((UIElement) this.elementContainer);
    this.weakAnnotCanvas?.SetTarget((AnnotationCanvas) null);
    this.weakAnnotCanvas = (WeakReference<AnnotationCanvas>) null;
    PdfViewer viewer = this.Viewer;
    if (viewer == null)
      return;
    this.weakAnnotCanvas = new WeakReference<AnnotationCanvas>(PdfObjectExtensions.GetAnnotationCanvas(viewer));
  }

  protected virtual void OnMouseMove(MouseEventArgs e)
  {
  }

  protected virtual void OnMouseDown(MouseButtonEventArgs e)
  {
  }

  protected virtual void OnMouseUp(MouseButtonEventArgs e)
  {
  }

  protected virtual void OnPreviewKeyDown(KeyEventArgs e)
  {
  }

  private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    if ((DateTime.Now - this.createTime).TotalMilliseconds < 200.0)
    {
      this.createTime = new DateTime();
    }
    else
    {
      if (this.Viewer.IsMouseCaptured)
        this.Viewer.ReleaseMouseCapture();
      if (DataOperationModel.IsScrollBar(e.OriginalSource))
        return;
      if (this.CurrentPage != -1 && !this.cancelQueued)
      {
        this.UpdateMousePosition((MouseEventArgs) e);
        this.OnMouseUp(e);
      }
      else
        (this.ScrollOwner.Dispatcher ?? DispatcherHelper.UIDispatcher)?.InvokeAsync((Action) (() =>
        {
          if (this.IsCompleted)
            return;
          this.OnCompleted(false, false);
        }), DispatcherPriority.Input);
    }
  }

  private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    bool? isMouseOver = this.ScrollOwner?.IsMouseOver;
    if (isMouseOver.HasValue && isMouseOver.GetValueOrDefault())
    {
      if (DataOperationModel.IsScrollBar(e.OriginalSource))
        return;
      if (!this.Viewer.IsMouseCaptured)
        this.Viewer.CaptureMouse();
      this.UpdateMousePosition((MouseEventArgs) e);
      this.OnMouseDown(e);
    }
    else
    {
      if (this.IsCompleted)
        return;
      this.cancelQueued = true;
    }
  }

  private static bool IsScrollBar(object element)
  {
    if (element == null || !(element is DependencyObject reference))
      return false;
    for (int index = 0; index < 5; ++index)
    {
      reference = (reference is FrameworkElement frameworkElement ? frameworkElement.Parent : (DependencyObject) null) ?? VisualTreeHelper.GetParent(reference);
      if (reference == null)
        return false;
      if (reference is ScrollBar)
        return true;
    }
    return false;
  }

  private void ScrollOwner_PreviewMouseMove(object sender, MouseEventArgs e)
  {
    this.UpdateMousePosition(e);
    this.OnMouseMove(e);
  }

  private void ScrollOwner_MouseLeave(object sender, MouseEventArgs e)
  {
    this.UpdateMousePosition(e);
  }

  private void ScrollOwner_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    this.OnPreviewKeyDown(e);
  }

  private void Window_Deactivated(object sender, EventArgs e)
  {
    if (this.IsCompleted)
      return;
    this.OnCompleted(false, false);
  }

  private void UpdateMousePosition(MouseEventArgs e)
  {
    if (this.IsCompleted)
      return;
    PdfViewer viewer = this.Viewer;
    if (viewer == null)
      return;
    this.MousePositionFromViewer = e.GetPosition((IInputElement) viewer);
    this.UpdatePagePosition();
    this.UpdatePreview();
  }

  private void AddInputEventHandler(ScrollViewer scrollOwner)
  {
    PdfViewer viewer = this.Viewer;
    if (scrollOwner == null)
      return;
    Window window = Window.GetWindow((DependencyObject) scrollOwner);
    if (window == null)
      return;
    window.Deactivated += new EventHandler(this.Window_Deactivated);
    WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler((UIElement) window, "PreviewMouseDown", new EventHandler<MouseButtonEventArgs>(this.Window_PreviewMouseDown));
    WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler((UIElement) window, "PreviewMouseUp", new EventHandler<MouseButtonEventArgs>(this.Window_PreviewMouseUp));
    WeakEventManager<Window, EventArgs>.AddHandler(window, "Deactivated", new EventHandler<EventArgs>(this.Window_Deactivated));
    WeakEventManager<UIElement, MouseEventArgs>.AddHandler((UIElement) scrollOwner, "PreviewMouseMove", new EventHandler<MouseEventArgs>(this.ScrollOwner_PreviewMouseMove));
    WeakEventManager<UIElement, MouseEventArgs>.AddHandler((UIElement) scrollOwner, "MouseLeave", new EventHandler<MouseEventArgs>(this.ScrollOwner_MouseLeave));
    WeakEventManager<UIElement, KeyEventArgs>.AddHandler((UIElement) scrollOwner, "PreviewKeyDown", new EventHandler<KeyEventArgs>(this.ScrollOwner_PreviewKeyDown));
  }

  private void RemoveInputEventHandler(ScrollViewer scrollOwner)
  {
    PdfViewer viewer = this.Viewer;
    if (scrollOwner != null)
    {
      WeakEventManager<UIElement, MouseEventArgs>.RemoveHandler((UIElement) scrollOwner, "PreviewMouseMove", new EventHandler<MouseEventArgs>(this.ScrollOwner_PreviewMouseMove));
      WeakEventManager<UIElement, MouseEventArgs>.RemoveHandler((UIElement) scrollOwner, "MouseLeave", new EventHandler<MouseEventArgs>(this.ScrollOwner_MouseLeave));
      WeakEventManager<UIElement, KeyEventArgs>.RemoveHandler((UIElement) scrollOwner, "PreviewKeyDown", new EventHandler<KeyEventArgs>(this.ScrollOwner_PreviewKeyDown));
    }
    Window window = this.Window;
    if (window == null)
      return;
    WeakEventManager<UIElement, MouseButtonEventArgs>.RemoveHandler((UIElement) window, "PreviewMouseDown", new EventHandler<MouseButtonEventArgs>(this.Window_PreviewMouseDown));
    WeakEventManager<UIElement, MouseButtonEventArgs>.RemoveHandler((UIElement) window, "PreviewMouseUp", new EventHandler<MouseButtonEventArgs>(this.Window_PreviewMouseUp));
    WeakEventManager<Window, EventArgs>.RemoveHandler(window, "Deactivated", new EventHandler<EventArgs>(this.Window_Deactivated));
  }

  protected virtual void UpdatePagePosition()
  {
    if (this.IsDisposed)
      throw new ObjectDisposedException(nameof (DataOperationModel));
    if (this.IsCompleted)
      return;
    PdfViewer viewer = this.Viewer;
    ScrollViewer scrollOwner = this.ScrollOwner;
    UIElement previewElement = this.PreviewElement;
    if (viewer == null || scrollOwner == null || previewElement == null || !viewer.IsLoaded)
    {
      this.OnCompleted(true, true);
    }
    else
    {
      Point positionFromViewer = this.MousePositionFromViewer;
      Point pagePoint;
      int page = viewer.DeviceToPage(positionFromViewer.X, positionFromViewer.Y, out pagePoint);
      this.CurrentPage = page;
      this.PositionFromDocument = pagePoint.ToPdfPoint();
      if (page == -1 || !scrollOwner.IsMouseOver)
        this.elementContainer.Opacity = 0.0;
      else
        this.elementContainer.Opacity = 1.0;
    }
  }

  protected virtual void UpdatePreview()
  {
    if (this.IsDisposed)
      throw new ObjectDisposedException(nameof (DataOperationModel));
    if (this.IsCompleted)
      return;
    Point positionFromViewer = this.MousePositionFromViewer;
    Size previewSize = this.GetPreviewSize(this.CurrentPage);
    this.elementContainer.Width = previewSize.Width;
    this.elementContainer.Height = previewSize.Height;
    if (this.previewElement is FrameworkElement previewElement)
    {
      previewElement.Width = previewSize.Width;
      previewElement.Height = previewSize.Height;
    }
    Canvas.SetLeft((UIElement) this.elementContainer, positionFromViewer.X);
    Canvas.SetTop((UIElement) this.elementContainer, positionFromViewer.Y);
  }

  protected override ViewerOperationCompletedEventArgs<bool> CreateCompletedEventArgs(
    ViewOperationResult<bool> result)
  {
    return new ViewerOperationCompletedEventArgs<bool>(result);
  }

  protected override void OnViewerLoaded()
  {
    base.OnViewerLoaded();
    this.UpdatePagePosition();
    this.UpdatePreview();
  }

  protected override void OnViewerScrollChanged()
  {
    base.OnViewerScrollChanged();
    this.UpdatePagePosition();
    this.UpdatePreview();
  }

  protected override void OnViewerZoomChanged()
  {
    base.OnViewerZoomChanged();
    this.UpdatePagePosition();
    this.UpdatePreview();
  }

  protected override void OnScrollContainerChanged(ScrollViewer oldValue, ScrollViewer newValue)
  {
    base.OnScrollContainerChanged(oldValue, newValue);
    this.RemoveInputEventHandler(oldValue);
    this.AddInputEventHandler(newValue);
  }

  protected override void OnCompleted(bool success, bool result)
  {
    if (!this.IsCompleted)
    {
      this.UpdateAnnotCanvas();
      if (this.elementContainer != null)
        this.elementContainer.Child = (UIElement) null;
      this.elementContainer = (Border) null;
      PdfViewer viewer = this.Viewer;
      if (viewer != null)
        viewer.OverrideCursor = (Cursor) null;
    }
    base.OnCompleted(success, result);
  }

  protected override void Dispose(bool disposing)
  {
    this.Data = (object) null;
    base.Dispose(disposing);
  }
}
