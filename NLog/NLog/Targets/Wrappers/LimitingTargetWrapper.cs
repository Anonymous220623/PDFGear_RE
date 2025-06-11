// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.LimitingTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Time;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("LimitingWrapper", IsWrapper = true)]
public class LimitingTargetWrapper : WrapperTargetBase
{
  private DateTime _firstWriteInInterval;

  public LimitingTargetWrapper()
    : this((Target) null)
  {
  }

  public LimitingTargetWrapper(string name, Target wrappedTarget)
    : this(wrappedTarget, 1000, TimeSpan.FromHours(1.0))
  {
    this.Name = name;
  }

  public LimitingTargetWrapper(Target wrappedTarget)
    : this(wrappedTarget, 1000, TimeSpan.FromHours(1.0))
  {
  }

  public LimitingTargetWrapper(Target wrappedTarget, int messageLimit, TimeSpan interval)
  {
    this.MessageLimit = messageLimit;
    this.Interval = interval;
    this.WrappedTarget = wrappedTarget;
    this.OptimizeBufferReuse = this.GetType() == typeof (LimitingTargetWrapper);
  }

  [DefaultValue(1000)]
  public int MessageLimit { get; set; }

  [DefaultValue(typeof (TimeSpan), "01:00")]
  public TimeSpan Interval { get; set; }

  public DateTime IntervalResetsAt => this._firstWriteInInterval + this.Interval;

  public int MessagesWrittenCount { get; private set; }

  protected override void InitializeTarget()
  {
    if (this.MessageLimit <= 0)
      throw new NLogConfigurationException("The LimitingTargetWrapper's MessageLimit property must be > 0.");
    if (this.Interval <= TimeSpan.Zero)
      throw new NLogConfigurationException("The LimitingTargetWrapper's property Interval must be > 0.");
    base.InitializeTarget();
    this.ResetInterval();
    InternalLogger.Trace<string, int, TimeSpan>("LimitingWrapper(Name={0}): Initialized with MessageLimit={1} and Interval={2}.", this.Name, this.MessageLimit, this.Interval);
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    if (this.IsIntervalExpired())
    {
      this.ResetInterval();
      InternalLogger.Debug<string, TimeSpan>("LimitingWrapper(Name={0}): New interval of '{1}' started.", this.Name, this.Interval);
    }
    if (this.MessagesWrittenCount < this.MessageLimit)
    {
      this.WrappedTarget.WriteAsyncLogEvent(logEvent);
      ++this.MessagesWrittenCount;
    }
    else
    {
      logEvent.Continuation((Exception) null);
      InternalLogger.Trace<string, int>("LimitingWrapper(Name={0}): Discarded event, because MessageLimit of '{1}' was reached.", this.Name, this.MessageLimit);
    }
  }

  private void ResetInterval()
  {
    this._firstWriteInInterval = TimeSource.Current.Time;
    this.MessagesWrittenCount = 0;
  }

  private bool IsIntervalExpired()
  {
    return TimeSource.Current.Time - this._firstWriteInInterval > this.Interval;
  }
}
