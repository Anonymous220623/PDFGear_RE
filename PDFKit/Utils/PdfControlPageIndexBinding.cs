// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfControlPageIndexBinding
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.Utils;

internal abstract class PdfControlPageIndexBinding : IDisposable
{
  private DispatcherTimer timer;
  private int pageIndex;
  private TimeSpan lastScrollTime;
  private TimeSpan lastPageIndexChangeTime;
  private ScrollViewer scrollViewer;
  private bool disposedValue;

  public PdfControlPageIndexBinding()
  {
    this.timer = new DispatcherTimer()
    {
      Interval = TimeSpan.FromMilliseconds(50.0)
    };
    this.timer.Tick += new EventHandler(this.Timer_Tick);
  }

  public int PageIndex
  {
    get => this.pageIndex;
    set
    {
      if (this.pageIndex == value)
        return;
      this.pageIndex = value;
      this.lastPageIndexChangeTime = TimeSpan.FromTicks(Stopwatch.GetTimestamp());
      this.UpdateViewerPageIndex();
      EventHandler pageIndexChanged = this.PageIndexChanged;
      if (pageIndexChanged != null)
        pageIndexChanged((object) this, EventArgs.Empty);
    }
  }

  private bool IsElementLoaded
  {
    get => this.ScrollInfo is FrameworkElement scrollInfo && scrollInfo.IsLoaded;
  }

  protected abstract IPdfScrollInfo ScrollInfo { get; }

  private bool IsFromScroll
  {
    get => (TimeSpan.FromTicks(Stopwatch.GetTimestamp()) - this.lastScrollTime).Milliseconds < 10;
  }

  protected abstract int PageIndexInternal { get; set; }

  protected void OnBeforeDocumentChanged()
  {
    if (!this.IsElementLoaded)
      return;
    this.timer.Stop();
    this.pageIndex = -1;
    this.lastPageIndexChangeTime = new TimeSpan();
  }

  protected void OnCurrentPageChanged()
  {
    if (!this.IsElementLoaded || this.ScrollInfo?.Document == null)
      return;
    if ((TimeSpan.FromTicks(Stopwatch.GetTimestamp()) - this.lastPageIndexChangeTime).TotalMilliseconds > 100.0)
    {
      this.timer.Stop();
      this.PageIndex = this.PageIndexInternal;
    }
    else
      this.timer.Start();
  }

  protected void OnScrollOwnerChanged()
  {
    if (!this.IsElementLoaded)
      return;
    this.UpdateScrollOwner();
  }

  protected virtual void OnLoaded()
  {
    this.UpdateScrollOwner();
    this.PageIndex = this.PageIndexInternal;
  }

  protected virtual void OnUnloaded() => this.UpdateScrollOwner();

  protected void UpdateScrollOwner()
  {
    if (this.scrollViewer != null)
    {
      this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
      this.scrollViewer = (ScrollViewer) null;
    }
    ScrollViewer scrollOwner;
    int num;
    if (this.IsElementLoaded)
    {
      scrollOwner = this.ScrollInfo.ScrollOwner;
      num = scrollOwner != null ? 1 : 0;
    }
    else
      num = 0;
    if (num != 0)
    {
      this.scrollViewer = scrollOwner;
      this.scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
    }
    this.lastScrollTime = new TimeSpan();
  }

  private void UpdateViewerPageIndex()
  {
    if (!this.IsElementLoaded)
      return;
    this.timer.Stop();
    if (this.PageIndexInternal == this.PageIndex)
      return;
    if (!this.IsFromScroll)
      this.ScrollInfo?.ScrollToPage(this.PageIndex);
    this.PageIndexInternal = this.PageIndex;
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    this.timer.Stop();
    this.PageIndex = this.PageIndexInternal;
  }

  private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.lastScrollTime = TimeSpan.FromTicks(Stopwatch.GetTimestamp());
  }

  public event EventHandler PageIndexChanged;

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (!disposing)
      ;
    if (this.IsElementLoaded)
      this.OnUnloaded();
    this.timer.Stop();
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
