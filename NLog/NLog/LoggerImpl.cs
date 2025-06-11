// Decompiled with JetBrains decompiler
// Type: NLog.LoggerImpl
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Config;
using NLog.Filters;
using NLog.Internal;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace NLog;

internal static class LoggerImpl
{
  private const int StackTraceSkipMethods = 0;

  internal static void Write(
    [NotNull] Type loggerType,
    [NotNull] TargetWithFilterChain targetsForLevel,
    LogEventInfo logEvent,
    LogFactory factory)
  {
    StackTraceUsage stackTraceUsage = targetsForLevel.GetStackTraceUsage();
    if (stackTraceUsage != StackTraceUsage.None && !logEvent.HasStackTrace)
      LoggerImpl.CaptureCallSiteInfo(factory, loggerType, logEvent, stackTraceUsage);
    AsyncContinuation onException = SingleCallContinuation.Completed;
    if (factory.ThrowExceptions)
    {
      int originalThreadId = AsyncHelpers.GetManagedThreadId();
      onException = (AsyncContinuation) (ex =>
      {
        if (ex != null && AsyncHelpers.GetManagedThreadId() == originalThreadId)
          throw new NLogRuntimeException("Exception occurred in NLog", ex);
      });
    }
    if (targetsForLevel.NextInChain == null && logEvent.CanLogEventDeferMessageFormat())
      logEvent.MessageFormatter = LogMessageTemplateFormatter.DefaultAutoSingleTarget.MessageFormatter;
    IList<Filter> filterList = (IList<Filter>) null;
    FilterResult filterResult = FilterResult.Neutral;
    for (TargetWithFilterChain targetWithFilterChain = targetsForLevel; targetWithFilterChain != null; targetWithFilterChain = targetWithFilterChain.NextInChain)
    {
      FilterResult result = filterList == targetWithFilterChain.FilterChain ? filterResult : LoggerImpl.GetFilterResult(targetWithFilterChain.FilterChain, logEvent, targetWithFilterChain.DefaultResult);
      if (!LoggerImpl.WriteToTargetWithFilterChain(targetWithFilterChain.Target, result, logEvent, onException))
        break;
      filterResult = result;
      filterList = targetWithFilterChain.FilterChain;
    }
  }

  private static void CaptureCallSiteInfo(
    LogFactory factory,
    Type loggerType,
    LogEventInfo logEvent,
    StackTraceUsage stackTraceUsage)
  {
    try
    {
      StackTrace stackTrace = new StackTrace(0, stackTraceUsage == StackTraceUsage.WithSource);
      logEvent.GetCallSiteInformationInternal().SetStackTrace(stackTrace, loggerType: loggerType);
    }
    catch (Exception ex)
    {
      if (factory.ThrowExceptions || ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Error(ex, "Failed to capture CallSite for Logger {0}. Platform might not support ${{callsite}}", (object) logEvent.LoggerName);
    }
  }

  private static bool WriteToTargetWithFilterChain(
    Target target,
    FilterResult result,
    LogEventInfo logEvent,
    AsyncContinuation onException)
  {
    if (result == FilterResult.Ignore || result == FilterResult.IgnoreFinal)
    {
      if (InternalLogger.IsDebugEnabled)
        InternalLogger.Debug<string, LogLevel>("{0}.{1} Rejecting message because of a filter.", logEvent.LoggerName, logEvent.Level);
      return result != FilterResult.IgnoreFinal;
    }
    target.WriteAsyncLogEvent(logEvent.WithContinuation(onException));
    return result != FilterResult.LogFinal;
  }

  private static FilterResult GetFilterResult(
    IList<Filter> filterChain,
    LogEventInfo logEvent,
    FilterResult defaultFilterResult)
  {
    FilterResult filterResult1 = defaultFilterResult;
    if (filterChain != null)
    {
      if (filterChain.Count != 0)
      {
        try
        {
          for (int index = 0; index < filterChain.Count; ++index)
          {
            FilterResult filterResult2 = filterChain[index].GetFilterResult(logEvent);
            if (filterResult2 != FilterResult.Neutral)
              return filterResult2;
          }
          return defaultFilterResult;
        }
        catch (Exception ex)
        {
          InternalLogger.Warn(ex, "Exception during filter evaluation. Message will be ignore.");
          if (!ex.MustBeRethrown())
            return FilterResult.Ignore;
          throw;
        }
      }
    }
    return filterResult1;
  }
}
