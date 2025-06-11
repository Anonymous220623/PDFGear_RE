// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.FilteringTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using NLog.Filters;
using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("FilteringWrapper", IsWrapper = true)]
public class FilteringTargetWrapper : WrapperTargetBase
{
  public FilteringTargetWrapper()
    : this((Target) null, (ConditionExpression) null)
  {
  }

  public FilteringTargetWrapper(string name, Target wrappedTarget, ConditionExpression condition)
    : this(wrappedTarget, condition)
  {
    this.Name = name;
  }

  public FilteringTargetWrapper(Target wrappedTarget, ConditionExpression condition)
  {
    this.WrappedTarget = wrappedTarget;
    this.Condition = condition;
  }

  public ConditionExpression Condition
  {
    get
    {
      return !(this.Filter is ConditionBasedFilter filter) ? (ConditionExpression) null : filter.Condition;
    }
    set => this.Filter = (Filter) FilteringTargetWrapper.CreateFilter(value);
  }

  [RequiredParameter]
  public Filter Filter { get; set; }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (this.OptimizeBufferReuse || this.WrappedTarget == null || !this.WrappedTarget.OptimizeBufferReuse)
      return;
    this.OptimizeBufferReuse = this.GetType() == typeof (FilteringTargetWrapper);
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    if (FilteringTargetWrapper.ShouldLogEvent(logEvent, this.Filter))
      this.WrappedTarget.WriteAsyncLogEvent(logEvent);
    else
      logEvent.Continuation((Exception) null);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    this.WrappedTarget.WriteAsyncLogEvents(logEvents.Filter<AsyncLogEventInfo, Filter>(this.Filter, (Func<AsyncLogEventInfo, Filter, bool>) ((logEvent, filter) => FilteringTargetWrapper.ShouldLogEvent(logEvent, filter))));
  }

  private static bool ShouldLogEvent(AsyncLogEventInfo logEvent, Filter filter)
  {
    switch (filter.GetFilterResult(logEvent.LogEvent))
    {
      case FilterResult.Ignore:
      case FilterResult.IgnoreFinal:
        logEvent.Continuation((Exception) null);
        return false;
      default:
        return true;
    }
  }

  private static ConditionBasedFilter CreateFilter(ConditionExpression value)
  {
    if (value == null)
      return (ConditionBasedFilter) null;
    return new ConditionBasedFilter()
    {
      Condition = value,
      DefaultFilterResult = FilterResult.Ignore
    };
  }
}
