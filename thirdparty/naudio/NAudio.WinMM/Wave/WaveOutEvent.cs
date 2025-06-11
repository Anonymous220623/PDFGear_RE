// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveOutEvent
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Threading;

#nullable disable
namespace NAudio.Wave;

public class WaveOutEvent : IWavePlayer, IDisposable, IWavePosition
{
  private readonly object waveOutLock;
  private readonly SynchronizationContext syncContext;
  private IntPtr hWaveOut;
  private WaveOutBuffer[] buffers;
  private IWaveProvider waveStream;
  private volatile PlaybackState playbackState;
  private AutoResetEvent callbackEvent;

  public event EventHandler<StoppedEventArgs> PlaybackStopped;

  public int DesiredLatency { get; set; }

  public int NumberOfBuffers { get; set; }

  public int DeviceNumber { get; set; } = -1;

  public WaveOutEvent()
  {
    this.syncContext = SynchronizationContext.Current;
    if (this.syncContext != null && (this.syncContext.GetType().Name == "LegacyAspNetSynchronizationContext" || this.syncContext.GetType().Name == "AspNetSynchronizationContext"))
      this.syncContext = (SynchronizationContext) null;
    this.DesiredLatency = 300;
    this.NumberOfBuffers = 2;
    this.waveOutLock = new object();
  }

  public void Init(IWaveProvider waveProvider)
  {
    if (this.playbackState != PlaybackState.Stopped)
      throw new InvalidOperationException("Can't re-initialize during playback");
    if (this.hWaveOut != IntPtr.Zero)
    {
      this.DisposeBuffers();
      this.CloseWaveOut();
    }
    this.callbackEvent = new AutoResetEvent(false);
    this.waveStream = waveProvider;
    int byteSize = waveProvider.WaveFormat.ConvertLatencyToByteSize((this.DesiredLatency + this.NumberOfBuffers - 1) / this.NumberOfBuffers);
    MmResult result;
    lock (this.waveOutLock)
      result = WaveInterop.waveOutOpenWindow(out this.hWaveOut, (IntPtr) this.DeviceNumber, this.waveStream.WaveFormat, this.callbackEvent.SafeWaitHandle.DangerousGetHandle(), IntPtr.Zero, WaveInterop.WaveInOutOpenFlags.CallbackEvent);
    MmException.Try(result, "waveOutOpen");
    this.buffers = new WaveOutBuffer[this.NumberOfBuffers];
    this.playbackState = PlaybackState.Stopped;
    for (int index = 0; index < this.NumberOfBuffers; ++index)
      this.buffers[index] = new WaveOutBuffer(this.hWaveOut, byteSize, this.waveStream, this.waveOutLock);
  }

  public void Play()
  {
    if (this.buffers == null || this.waveStream == null)
      throw new InvalidOperationException("Must call Init first");
    if (this.playbackState == PlaybackState.Stopped)
    {
      this.playbackState = PlaybackState.Playing;
      this.callbackEvent.Set();
      ThreadPool.QueueUserWorkItem((WaitCallback) (state => this.PlaybackThread()), (object) null);
    }
    else
    {
      if (this.playbackState != PlaybackState.Paused)
        return;
      this.Resume();
      this.callbackEvent.Set();
    }
  }

  private void PlaybackThread()
  {
    Exception e = (Exception) null;
    try
    {
      this.DoPlayback();
    }
    catch (Exception ex)
    {
      e = ex;
    }
    finally
    {
      this.playbackState = PlaybackState.Stopped;
      this.RaisePlaybackStoppedEvent(e);
    }
  }

  private void DoPlayback()
  {
    while (this.playbackState != PlaybackState.Stopped)
    {
      if (!this.callbackEvent.WaitOne(this.DesiredLatency))
      {
        int playbackState = (int) this.playbackState;
      }
      if (this.playbackState == PlaybackState.Playing)
      {
        int num = 0;
        foreach (WaveOutBuffer buffer in this.buffers)
        {
          if (buffer.InQueue || buffer.OnDone())
            ++num;
        }
        if (num == 0)
        {
          this.playbackState = PlaybackState.Stopped;
          this.callbackEvent.Set();
        }
      }
    }
  }

  public void Pause()
  {
    if (this.playbackState != PlaybackState.Playing)
      return;
    this.playbackState = PlaybackState.Paused;
    MmResult result;
    lock (this.waveOutLock)
      result = WaveInterop.waveOutPause(this.hWaveOut);
    if (result != MmResult.NoError)
      throw new MmException(result, "waveOutPause");
  }

  private void Resume()
  {
    if (this.playbackState != PlaybackState.Paused)
      return;
    MmResult result;
    lock (this.waveOutLock)
      result = WaveInterop.waveOutRestart(this.hWaveOut);
    if (result != MmResult.NoError)
      throw new MmException(result, "waveOutRestart");
    this.playbackState = PlaybackState.Playing;
  }

  public void Stop()
  {
    if (this.playbackState == PlaybackState.Stopped)
      return;
    this.playbackState = PlaybackState.Stopped;
    MmResult result;
    lock (this.waveOutLock)
      result = WaveInterop.waveOutReset(this.hWaveOut);
    if (result != MmResult.NoError)
      throw new MmException(result, "waveOutReset");
    this.callbackEvent.Set();
  }

  public long GetPosition() => WaveOutUtils.GetPositionBytes(this.hWaveOut, this.waveOutLock);

  public WaveFormat OutputWaveFormat => this.waveStream.WaveFormat;

  public PlaybackState PlaybackState => this.playbackState;

  public float Volume
  {
    get => WaveOutUtils.GetWaveOutVolume(this.hWaveOut, this.waveOutLock);
    set => WaveOutUtils.SetWaveOutVolume(value, this.hWaveOut, this.waveOutLock);
  }

  public void Dispose()
  {
    GC.SuppressFinalize((object) this);
    this.Dispose(true);
  }

  protected void Dispose(bool disposing)
  {
    this.Stop();
    if (disposing)
      this.DisposeBuffers();
    this.CloseWaveOut();
  }

  private void CloseWaveOut()
  {
    if (this.callbackEvent != null)
    {
      this.callbackEvent.Close();
      this.callbackEvent = (AutoResetEvent) null;
    }
    lock (this.waveOutLock)
    {
      if (!(this.hWaveOut != IntPtr.Zero))
        return;
      int num = (int) WaveInterop.waveOutClose(this.hWaveOut);
      this.hWaveOut = IntPtr.Zero;
    }
  }

  private void DisposeBuffers()
  {
    if (this.buffers == null)
      return;
    foreach (WaveOutBuffer buffer in this.buffers)
      buffer.Dispose();
    this.buffers = (WaveOutBuffer[]) null;
  }

  ~WaveOutEvent() => this.Dispose(false);

  private void RaisePlaybackStoppedEvent(Exception e)
  {
    EventHandler<StoppedEventArgs> handler = this.PlaybackStopped;
    if (handler == null)
      return;
    if (this.syncContext == null)
      handler((object) this, new StoppedEventArgs(e));
    else
      this.syncContext.Post((SendOrPostCallback) (state => handler((object) this, new StoppedEventArgs(e))), (object) null);
  }
}
