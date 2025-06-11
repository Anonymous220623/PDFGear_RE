// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.FallbackGroupTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("FallbackGroup", IsCompound = true)]
public class FallbackGroupTarget : CompoundTargetBase
{
  private long _currentTarget;

  public FallbackGroupTarget()
    : this(new Target[0])
  {
  }

  public FallbackGroupTarget(string name, params Target[] targets)
    : this(targets)
  {
    this.Name = name;
  }

  public FallbackGroupTarget(params Target[] targets)
    : base(targets)
  {
    this.OptimizeBufferReuse = this.GetType() == typeof (FallbackGroupTarget);
  }

  public bool ReturnToFirstOnSuccess { get; set; }

  protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent) => this.Write(logEvent);

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    AsyncContinuation continuation = (AsyncContinuation) null;
    int tryCounter = 0;
    int targetToInvoke = 0;
    continuation = (AsyncContinuation) (ex =>
    {
      if (ex == null)
      {
        if (this.ReturnToFirstOnSuccess && Interlocked.Read(ref this._currentTarget) != 0L)
        {
          InternalLogger.Debug<string, Target>("FallbackGroup(Name={0}): Target '{1}' succeeded. Returning to the first one.", this.Name, this.Targets[targetToInvoke]);
          Interlocked.Exchange(ref this._currentTarget, 0L);
        }
        logEvent.Continuation((Exception) null);
      }
      else
      {
        InternalLogger.Warn(ex, "FallbackGroup(Name={0}): Target '{1}' failed. Proceeding to the next one.", (object) this.Name, (object) this.Targets[targetToInvoke]);
        ++tryCounter;
        int num = (targetToInvoke + 1) % this.Targets.Count;
        Interlocked.CompareExchange(ref this._currentTarget, (long) num, (long) targetToInvoke);
        targetToInvoke = tryCounter < this.Targets.Count ? num : -1;
        if (targetToInvoke >= 0)
          this.Targets[targetToInvoke].WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(continuation));
        else
          logEvent.Continuation(ex);
      }
    });
    targetToInvoke = (int) Interlocked.Read(ref this._currentTarget);
    for (int index = 0; index < this.Targets.Count; ++index)
    {
      if (index != targetToInvoke)
        this.Targets[index].PrecalculateVolatileLayouts(logEvent.LogEvent);
    }
    this.Targets[targetToInvoke].WriteAsyncLogEvent(logEvent.LogEvent.WithContinuation(continuation));
  }
}
