// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfViewerAutoScrollHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace PDFKit.Utils;

public class PdfViewerAutoScrollHelper : DependencyObject, IDisposable
{
  private const int AutoScrollTimerMinIntervalMilliseconds = 50;
  private const int AutoScrollTimerMaxIntervalMilliseconds = 1000;
  private static readonly DependencyPropertyKey statePropertyKey;
  public static readonly DependencyProperty StateProperty;
  private readonly PdfViewer viewer;
  private DispatcherTimer autoScrollTimer;
  private DispatcherTimer pauseTimer;
  private bool disposedValue;
  private int lastAutoScrollIntervalMilliseconds = 50;
  public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register(nameof (Speed), typeof (double), typeof (PdfViewerAutoScrollHelper), new PropertyMetadata((object) 1.0));

  static PdfViewerAutoScrollHelper()
  {
    PdfViewerAutoScrollHelper.statePropertyKey = DependencyProperty.RegisterReadOnly(nameof (State), typeof (PdfViewerAutoScrollState), typeof (PdfViewerAutoScrollHelper), new PropertyMetadata((object) PdfViewerAutoScrollState.Stop, (PropertyChangedCallback) ((s, a) =>
    {
      if (!(s is PdfViewerAutoScrollHelper autoScrollHelper2) || !(a.OldValue is PdfViewerAutoScrollState oldValue2) || !(a.NewValue is PdfViewerAutoScrollState newValue2))
        return;
      autoScrollHelper2.OnStateChanged(oldValue2, newValue2);
    })));
    PdfViewerAutoScrollHelper.StateProperty = PdfViewerAutoScrollHelper.statePropertyKey.DependencyProperty;
  }

  public PdfViewerAutoScrollHelper(PdfViewer viewer)
  {
    this.viewer = viewer ?? throw new ArgumentNullException(nameof (viewer));
    this.autoScrollTimer = new DispatcherTimer(DispatcherPriority.Render);
    this.autoScrollTimer.Interval = TimeSpan.FromMilliseconds((double) this.lastAutoScrollIntervalMilliseconds);
    this.autoScrollTimer.Tick += new EventHandler(this.AutoScrollTimer_Tick);
    this.pauseTimer = new DispatcherTimer(DispatcherPriority.Render);
    this.pauseTimer.Interval = TimeSpan.FromSeconds(0.2);
    this.pauseTimer.Tick += new EventHandler(this.PauseTimer_Tick);
  }

  public PdfViewerAutoScrollState State
  {
    get => (PdfViewerAutoScrollState) this.GetValue(PdfViewerAutoScrollHelper.StateProperty);
    private set => this.SetValue(PdfViewerAutoScrollHelper.statePropertyKey, (object) value);
  }

  public double Speed
  {
    get => (double) this.GetValue(PdfViewerAutoScrollHelper.SpeedProperty);
    set => this.SetValue(PdfViewerAutoScrollHelper.SpeedProperty, (object) value);
  }

  public void StartAutoScroll()
  {
    if (this.State == PdfViewerAutoScrollState.Scrolling)
      return;
    this.CancelPauseTimer();
    this.State = PdfViewerAutoScrollState.Scrolling;
    this.lastAutoScrollIntervalMilliseconds = 50;
    this.autoScrollTimer.Interval = TimeSpan.FromMilliseconds((double) this.lastAutoScrollIntervalMilliseconds);
    this.autoScrollTimer.Start();
  }

  public void StopAutoScroll()
  {
    this.ThrowIfDisposed();
    this.CancelPauseTimer();
    this.autoScrollTimer.Stop();
    this.State = PdfViewerAutoScrollState.Stop;
  }

  public void Pause(int milliseconds)
  {
    this.ThrowIfDisposed();
    this.CancelPauseTimer();
    this.autoScrollTimer.Stop();
    if (this.State == PdfViewerAutoScrollState.Stop)
      return;
    if (this.State == PdfViewerAutoScrollState.Scrolling)
      this.State = PdfViewerAutoScrollState.Pause;
    if (milliseconds <= 0)
      return;
    this.pauseTimer.Interval = TimeSpan.FromMilliseconds((double) milliseconds);
    this.pauseTimer.Start();
  }

  public void Resume()
  {
    this.ThrowIfDisposed();
    this.CancelPauseTimer();
    if (this.State != PdfViewerAutoScrollState.Pause)
      return;
    this.State = PdfViewerAutoScrollState.Scrolling;
    this.lastAutoScrollIntervalMilliseconds = 50;
    this.autoScrollTimer.Interval = TimeSpan.FromMilliseconds((double) this.lastAutoScrollIntervalMilliseconds);
    this.autoScrollTimer.Start();
  }

  private void CancelPauseTimer()
  {
    this.ThrowIfDisposed();
    this.pauseTimer.Stop();
  }

  private void OnPauseCompleted()
  {
    this.CancelPauseTimer();
    this.StartAutoScroll();
  }

  private void AutoScrollTimer_Tick(object sender, EventArgs e)
  {
    ScrollViewer scrollOwner = this.viewer.ScrollOwner;
    if (scrollOwner == null || scrollOwner.ScrollableHeight <= 0.0)
      return;
    double verticalOffset = scrollOwner.VerticalOffset;
    if (this.Speed > 0.0 && verticalOffset >= scrollOwner.ScrollableHeight - 1.0 || this.Speed < 0.0 && verticalOffset <= 1.0)
      return;
    this.AdjustAutoScrollTimerInterval();
    double num = this.Speed * 10.0 * this.autoScrollTimer.Interval.TotalSeconds;
    scrollOwner.ScrollToVerticalOffset(verticalOffset + num);
  }

  private void PauseTimer_Tick(object sender, EventArgs e) => this.OnPauseCompleted();

  private void AdjustAutoScrollTimerInterval()
  {
    int num = Math.Min(Math.Max(this.viewer.LastRenderingMilliseconds, 50), 1000);
    if (num == this.lastAutoScrollIntervalMilliseconds)
      return;
    this.lastAutoScrollIntervalMilliseconds = num;
    this.autoScrollTimer.Interval = TimeSpan.FromMilliseconds((double) num);
  }

  private void OnStateChanged(PdfViewerAutoScrollState oldState, PdfViewerAutoScrollState newState)
  {
    EventHandler<PdfViewerAutoScrollStateChangedEventArgs> stateChanged = this.StateChanged;
    if (stateChanged == null)
      return;
    stateChanged((object) this, new PdfViewerAutoScrollStateChangedEventArgs(oldState, newState));
  }

  public event EventHandler<PdfViewerAutoScrollStateChangedEventArgs> StateChanged;

  private void ThrowIfDisposed()
  {
    if (this.disposedValue)
      throw new ObjectDisposedException(nameof (PdfViewerAutoScrollHelper));
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (!disposing)
      ;
    this.CancelPauseTimer();
    this.pauseTimer = (DispatcherTimer) null;
    this.autoScrollTimer.Stop();
    this.autoScrollTimer = (DispatcherTimer) null;
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
