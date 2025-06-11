// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.PostFilteringTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("PostFilteringWrapper", IsWrapper = true)]
public class PostFilteringTargetWrapper : WrapperTargetBase
{
  private static object boxedTrue = (object) true;

  public PostFilteringTargetWrapper()
    : this((Target) null)
  {
  }

  public PostFilteringTargetWrapper(Target wrappedTarget)
    : this((string) null, wrappedTarget)
  {
  }

  public PostFilteringTargetWrapper(string name, Target wrappedTarget)
  {
    this.Name = name;
    this.WrappedTarget = wrappedTarget;
    this.Rules = (IList<FilteringRule>) new List<FilteringRule>();
  }

  public ConditionExpression DefaultFilter { get; set; }

  [ArrayParameter(typeof (FilteringRule), "when")]
  public IList<FilteringRule> Rules { get; private set; }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (this.OptimizeBufferReuse || this.WrappedTarget == null || !this.WrappedTarget.OptimizeBufferReuse)
      return;
    this.OptimizeBufferReuse = this.GetType() == typeof (PostFilteringTargetWrapper);
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    this.Write((IList<AsyncLogEventInfo>) new AsyncLogEventInfo[1]
    {
      logEvent
    });
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected override void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    InternalLogger.Trace<string, int>("PostFilteringWrapper(Name={0}): Running on {1} events", this.Name, logEvents.Count);
    ConditionExpression state = this.EvaluateAllRules(logEvents) ?? this.DefaultFilter;
    if (state == null)
    {
      this.WrappedTarget.WriteAsyncLogEvents(logEvents);
    }
    else
    {
      InternalLogger.Trace<string, ConditionExpression>("PostFilteringWrapper(Name={0}): Filter to apply: {1}", this.Name, state);
      IList<AsyncLogEventInfo> logEvents1 = logEvents.Filter<AsyncLogEventInfo, ConditionExpression>(state, (Func<AsyncLogEventInfo, ConditionExpression, bool>) ((logEvent, filter) => PostFilteringTargetWrapper.ApplyFilter(logEvent, filter)));
      InternalLogger.Trace<string, int>("PostFilteringWrapper(Name={0}): After filtering: {1} events.", this.Name, logEvents1.Count);
      if (logEvents1.Count <= 0)
        return;
      InternalLogger.Trace<string, Target>("PostFilteringWrapper(Name={0}): Sending to {1}", this.Name, this.WrappedTarget);
      this.WrappedTarget.WriteAsyncLogEvents(logEvents1);
    }
  }

  private static bool ApplyFilter(AsyncLogEventInfo logEvent, ConditionExpression resultFilter)
  {
    object obj = resultFilter.Evaluate(logEvent.LogEvent);
    if (PostFilteringTargetWrapper.boxedTrue.Equals(obj))
      return true;
    logEvent.Continuation((Exception) null);
    return false;
  }

  private ConditionExpression EvaluateAllRules(IList<AsyncLogEventInfo> logEvents)
  {
    if (this.Rules.Count == 0)
      return (ConditionExpression) null;
    for (int index1 = 0; index1 < logEvents.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.Rules.Count; ++index2)
      {
        FilteringRule rule = this.Rules[index2];
        object obj = rule.Exists.Evaluate(logEvents[index1].LogEvent);
        if (PostFilteringTargetWrapper.boxedTrue.Equals(obj))
        {
          InternalLogger.Trace<string, ConditionExpression>("PostFilteringWrapper(Name={0}): Rule matched: {1}", this.Name, rule.Exists);
          return rule.Filter;
        }
      }
    }
    return (ConditionExpression) null;
  }
}
