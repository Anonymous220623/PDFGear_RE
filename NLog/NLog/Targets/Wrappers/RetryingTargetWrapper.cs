// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.RetryingTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("RetryingWrapper", IsWrapper = true)]
public class RetryingTargetWrapper : WrapperTargetBase
{
  private readonly object _retrySyncObject = new object();

  public RetryingTargetWrapper()
    : this((Target) null, 3, 100)
  {
  }

  public RetryingTargetWrapper(
    string name,
    Target wrappedTarget,
    int retryCount,
    int retryDelayMilliseconds)
    : this(wrappedTarget, retryCount, retryDelayMilliseconds)
  {
    this.Name = name;
  }

  public RetryingTargetWrapper(Target wrappedTarget, int retryCount, int retryDelayMilliseconds)
  {
    this.WrappedTarget = wrappedTarget;
    this.RetryCount = retryCount;
    this.RetryDelayMilliseconds = retryDelayMilliseconds;
    this.OptimizeBufferReuse = this.GetType() == typeof (RetryingTargetWrapper);
  }

  [DefaultValue(3)]
  public int RetryCount { get; set; }

  [DefaultValue(100)]
  public int RetryDelayMilliseconds { get; set; }

  protected override void WriteAsyncThreadSafe(IList<AsyncLogEventInfo> logEvents)
  {
    lock (this._retrySyncObject)
    {
      for (int index = 0; index < logEvents.Count; ++index)
      {
        if (!this.IsInitialized)
          logEvents[index].Continuation((Exception) null);
        else
          this.WriteAsyncThreadSafe(logEvents[index]);
      }
    }
  }

  protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
  {
    lock (this._retrySyncObject)
      this.Write(logEvent);
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    AsyncContinuation continuation = (AsyncContinuation) null;
    int counter = 0;
    continuation = (AsyncContinuation) (ex =>
    {
      if (ex == null)
      {
        logEvent.Continuation((Exception) null);
      }
      else
      {
        int num1 = Interlocked.Increment(ref counter);
        InternalLogger.Warn(ex, "RetryingWrapper(Name={0}): Error while writing to '{1}'. Try {2}/{3}", (object) this.Name, (object) this.WrappedTarget, (object) num1, (object) this.RetryCount);
        if (num1 >= this.RetryCount)
        {
          InternalLogger.Warn("Too many retries. Aborting.");
          logEvent.Continuation(ex);
        }
        else
        {
          int num2 = 0;
          while (num2 < this.RetryDelayMilliseconds)
          {
            int num3 = Math.Min(100, this.RetryDelayMilliseconds - num2);
            AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds((double) num3));
            num2 += num3;
            if (!this.IsInitialized)
            {
              InternalLogger.Warn<string>("RetryingWrapper(Name={0}): Target closed. Aborting.", this.Name);
              logEvent.Continuation(ex);
              return;
            }
          }
          this.WrappedTarget.WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(continuation));
        }
      }
    });
    this.WrappedTarget.WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(continuation));
  }
}
