// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Viewer.ViewerOperationModel`2
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using PDFKit;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Models.Viewer;

internal abstract class ViewerOperationModel<T, TResult> : IDisposable where T : ViewerOperationModel<T, TResult>
{
  private readonly WeakReference<PdfViewer> weakViewer;
  private readonly WeakReference<Window> weakWindow;
  private WeakReference<ScrollViewer> weakScrollOwner;
  private bool disposedValue;
  private TaskCompletionSource<ViewOperationResult<TResult>> taskCompletionSource;

  public System.Threading.Tasks.Task<ViewOperationResult<TResult>> Task
  {
    get => this.taskCompletionSource.Task;
  }

  public bool IsDisposed => this.disposedValue;

  protected bool IsCompleted => this.Task.IsCompleted;

  public ViewOperationResult<TResult> Result
  {
    get => !this.Task.IsCompleted ? (ViewOperationResult<TResult>) null : this.Task.Result;
  }

  public ViewerOperationModel(PdfViewer viewer)
  {
    ViewerOperationModel<T, TResult> viewerOperationModel = this;
    this.taskCompletionSource = new TaskCompletionSource<ViewOperationResult<TResult>>();
    if (viewer != null)
    {
      this.weakViewer = new WeakReference<PdfViewer>(viewer);
      Window window = Window.GetWindow((DependencyObject) viewer);
      if (window != null)
        this.weakWindow = new WeakReference<Window>(window);
    }
    this.AddEventHandler();
    viewer.Dispatcher.InvokeAsync((Action) (() =>
    {
      if (viewerOperationModel.Window == null || viewer == null || viewer.IsVisible)
        return;
      viewerOperationModel.OnCompleted(false, default (TResult));
    }), DispatcherPriority.Input);
  }

  public PdfViewer Viewer
  {
    get
    {
      PdfViewer target;
      return this.weakViewer != null && this.weakViewer.TryGetTarget(out target) ? target : (PdfViewer) null;
    }
  }

  protected Window Window
  {
    get
    {
      Window target;
      return this.weakWindow != null && this.weakWindow.TryGetTarget(out target) ? target : (Window) null;
    }
  }

  protected ScrollViewer ScrollOwner
  {
    get
    {
      ScrollViewer target;
      return this.weakScrollOwner != null && this.weakScrollOwner.TryGetTarget(out target) ? target : (ScrollViewer) null;
    }
  }

  protected virtual void OnCompleted(bool success, TResult result)
  {
    if (this.IsCompleted)
      throw new ArgumentException("IsCompleted");
    ScrollViewer scrollOwner = this.ScrollOwner;
    this.RemoveEventHandler();
    this.OnScrollContainerChanged(scrollOwner, (ScrollViewer) null);
    ViewOperationResult<TResult> result1 = success ? new ViewOperationResult<TResult>(result) : (ViewOperationResult<TResult>) null;
    this.taskCompletionSource.SetResult(result1);
    ViewerOperationEventHandler<T, TResult> completed = this.Completed;
    if (completed == null)
      return;
    completed((T) this, this.CreateCompletedEventArgs(result1));
  }

  protected abstract ViewerOperationCompletedEventArgs<TResult> CreateCompletedEventArgs(
    ViewOperationResult<TResult> result);

  public event ViewerOperationEventHandler<T, TResult> Completed;

  protected virtual void OnViewerLoaded()
  {
  }

  protected virtual void OnViewerScrollChanged()
  {
  }

  protected virtual void OnViewerZoomChanged()
  {
  }

  protected virtual void OnScrollContainerChanged(ScrollViewer oldValue, ScrollViewer newValue)
  {
  }

  private void Viewer_Loaded(object sender, EventArgs e) => this.OnViewerLoaded();

  private void ScrollOwner_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.OnViewerScrollChanged();
  }

  private void Viewer_ScrollOwnerChanged(object sender, EventArgs e)
  {
    ScrollViewer scrollOwner = this.ScrollOwner;
    this.UpdateScrollOwner();
    this.OnScrollContainerChanged(scrollOwner, this.ScrollOwner);
    this.OnViewerScrollChanged();
  }

  private void Viewer_ZoomChanged(object sender, EventArgs e) => this.OnViewerZoomChanged();

  private void AddEventHandler()
  {
    this.RemoveEventHandler();
    PdfViewer viewer = this.Viewer;
    if (viewer == null)
      return;
    WeakEventManager<PdfViewer, EventArgs>.AddHandler(viewer, "Loaded", new EventHandler<EventArgs>(this.Viewer_Loaded));
    WeakEventManager<PdfViewer, EventArgs>.AddHandler(viewer, "ZoomChanged", new EventHandler<EventArgs>(this.Viewer_ZoomChanged));
    WeakEventManager<PdfViewer, EventArgs>.AddHandler(viewer, "ScrollOwnerChanged", new EventHandler<EventArgs>(this.Viewer_ScrollOwnerChanged));
    ScrollViewer scrollOwner = this.ScrollOwner;
    this.UpdateScrollOwner();
    this.OnScrollContainerChanged(scrollOwner, this.ScrollOwner);
  }

  private void RemoveEventHandler()
  {
    PdfViewer viewer = this.Viewer;
    if (viewer != null)
    {
      WeakEventManager<PdfViewer, EventArgs>.RemoveHandler(viewer, "Loaded", new EventHandler<EventArgs>(this.Viewer_Loaded));
      WeakEventManager<PdfViewer, EventArgs>.RemoveHandler(viewer, "ZoomChanged", new EventHandler<EventArgs>(this.Viewer_ZoomChanged));
      WeakEventManager<PdfViewer, EventArgs>.RemoveHandler(viewer, "ScrollOwnerChanged", new EventHandler<EventArgs>(this.Viewer_ScrollOwnerChanged));
    }
    ScrollViewer scrollOwner = this.ScrollOwner;
    if (scrollOwner != null)
      WeakEventManager<ScrollViewer, ScrollChangedEventArgs>.RemoveHandler(scrollOwner, "ScrollChanged", new EventHandler<ScrollChangedEventArgs>(this.ScrollOwner_ScrollChanged));
    this.weakScrollOwner?.SetTarget((ScrollViewer) null);
    this.weakScrollOwner = (WeakReference<ScrollViewer>) null;
  }

  private void UpdateScrollOwner()
  {
    ScrollViewer scrollOwner = this.ScrollOwner;
    if (scrollOwner != null)
      WeakEventManager<ScrollViewer, ScrollChangedEventArgs>.RemoveHandler(scrollOwner, "ScrollChanged", new EventHandler<ScrollChangedEventArgs>(this.ScrollOwner_ScrollChanged));
    scrollOwner = this.Viewer?.ScrollOwner;
    this.weakScrollOwner = new WeakReference<ScrollViewer>(scrollOwner);
    if (scrollOwner == null)
      return;
    WeakEventManager<ScrollViewer, ScrollChangedEventArgs>.AddHandler(scrollOwner, "ScrollChanged", new EventHandler<ScrollChangedEventArgs>(this.ScrollOwner_ScrollChanged));
    scrollOwner.Dispatcher.InvokeAsync((Action) (() => scrollOwner.Focus()), DispatcherPriority.Background);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      if (!this.IsCompleted)
        this.OnCompleted(false, default (TResult));
      this.weakViewer?.SetTarget((PdfViewer) null);
    }
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
