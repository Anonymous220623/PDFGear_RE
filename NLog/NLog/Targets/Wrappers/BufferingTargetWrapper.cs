// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.BufferingTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("BufferingWrapper", IsWrapper = true)]
public class BufferingTargetWrapper : WrapperTargetBase
{
  private LogEventInfoBuffer _buffer;
  private Timer _flushTimer;
  private readonly object _lockObject = new object();

  public BufferingTargetWrapper()
    : this((Target) null)
  {
  }

  public BufferingTargetWrapper(string name, Target wrappedTarget)
    : this(wrappedTarget)
  {
    this.Name = name;
  }

  public BufferingTargetWrapper(Target wrappedTarget)
    : this(wrappedTarget, 100)
  {
  }

  public BufferingTargetWrapper(Target wrappedTarget, int bufferSize)
    : this(wrappedTarget, bufferSize, -1)
  {
  }

  public BufferingTargetWrapper(Target wrappedTarget, int bufferSize, int flushTimeout)
    : this(wrappedTarget, bufferSize, flushTimeout, BufferingTargetWrapperOverflowAction.Flush)
  {
  }

  public BufferingTargetWrapper(
    Target wrappedTarget,
    int bufferSize,
    int flushTimeout,
    BufferingTargetWrapperOverflowAction overflowAction)
  {
    this.WrappedTarget = wrappedTarget;
    this.BufferSize = bufferSize;
    this.FlushTimeout = flushTimeout;
    this.SlidingTimeout = true;
    this.OverflowAction = overflowAction;
  }

  [DefaultValue(100)]
  public int BufferSize { get; set; }

  [DefaultValue(-1)]
  public int FlushTimeout { get; set; }

  [DefaultValue(true)]
  public bool SlidingTimeout { get; set; }

  [DefaultValue("Flush")]
  public BufferingTargetWrapperOverflowAction OverflowAction { get; set; }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    this.WriteEventsInBuffer("Flush Async");
    base.FlushAsync(asyncContinuation);
  }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    this._buffer = new LogEventInfoBuffer(this.BufferSize, false, 0);
    InternalLogger.Trace<string>("BufferingWrapper(Name={0}): Create Timer", this.Name);
    this._flushTimer = new Timer(new TimerCallback(this.FlushCallback), (object) null, -1, -1);
  }

  protected override void CloseTarget()
  {
    Timer flushTimer = this._flushTimer;
    if (flushTimer != null)
    {
      this._flushTimer = (Timer) null;
      if (flushTimer.WaitForDispose(TimeSpan.FromSeconds(1.0)))
      {
        if (this.OverflowAction == BufferingTargetWrapperOverflowAction.Discard)
          this._buffer.GetEventsAndClear();
        else
          this.WriteEventsInBuffer("Closing Target");
      }
    }
    base.CloseTarget();
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    this.PrecalculateVolatileLayouts(logEvent.LogEvent);
    int num = this._buffer.Append(logEvent);
    if (num >= this.BufferSize)
    {
      if (this.OverflowAction != BufferingTargetWrapperOverflowAction.Flush)
        return;
      this.WriteEventsInBuffer("Exceeding BufferSize");
    }
    else
    {
      if (this.FlushTimeout <= 0 || !this.SlidingTimeout && num != 1)
        return;
      this._flushTimer.Change(this.FlushTimeout, -1);
    }
  }

  private void FlushCallback(object state)
  {
    bool flag = false;
    try
    {
      flag = Monitor.TryEnter(this._lockObject, Math.Min(this.FlushTimeout / 2, 100));
      if (flag)
      {
        if (this._flushTimer == null)
          return;
        this.WriteEventsInBuffer((string) null);
      }
      else
      {
        if (this._buffer.Count <= 0)
          return;
        this._flushTimer?.Change(this.FlushTimeout, -1);
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "BufferingWrapper(Name={0}): Error in flush procedure.", (object) this.Name);
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
    finally
    {
      if (flag)
        Monitor.Exit(this._lockObject);
    }
  }

  private void WriteEventsInBuffer(string reason)
  {
    if (this.WrappedTarget == null)
    {
      InternalLogger.Error<string>("BufferingWrapper(Name={0}): WrappedTarget is NULL", this.Name);
    }
    else
    {
      lock (this._lockObject)
      {
        AsyncLogEventInfo[] eventsAndClear = this._buffer.GetEventsAndClear();
        if (eventsAndClear.Length == 0)
          return;
        if (reason != null)
          InternalLogger.Trace<string, int, string>("BufferingWrapper(Name={0}): Writing {1} events ({2})", this.Name, eventsAndClear.Length, reason);
        this.WrappedTarget.WriteAsyncLogEvents(eventsAndClear);
      }
    }
  }
}
