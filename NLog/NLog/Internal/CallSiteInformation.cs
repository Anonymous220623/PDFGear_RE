// Decompiled with JetBrains decompiler
// Type: NLog.Internal.CallSiteInformation
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal class CallSiteInformation
{
  public void SetStackTrace(StackTrace stackTrace, int? userStackFrame = null, Type loggerType = null)
  {
    this.StackTrace = stackTrace;
    if (!userStackFrame.HasValue && stackTrace != null)
    {
      StackFrame[] frames = stackTrace.GetFrames();
      int? nullable1 = loggerType != (Type) null ? CallSiteInformation.FindCallingMethodOnStackTrace(frames, loggerType) : new int?(0);
      int? nullable2 = nullable1.HasValue ? new int?(CallSiteInformation.SkipToUserStackFrameLegacy(frames, nullable1.Value)) : nullable1;
      int? nullable3 = nullable1;
      this.UserStackFrameNumber = nullable3 ?? 0;
      nullable3 = nullable2;
      int? nullable4 = nullable1;
      this.UserStackFrameNumberLegacy = !(nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() & nullable3.HasValue == nullable4.HasValue) ? nullable2 : new int?();
    }
    else
    {
      int? nullable = userStackFrame;
      this.UserStackFrameNumber = nullable ?? 0;
      nullable = new int?();
      this.UserStackFrameNumberLegacy = nullable;
    }
  }

  public void SetCallerInfo(
    string callerClassName,
    string callerMemberName,
    string callerFilePath,
    int callerLineNumber)
  {
    this.CallerClassName = callerClassName;
    this.CallerMemberName = callerMemberName;
    this.CallerFilePath = callerFilePath;
    this.CallerLineNumber = new int?(callerLineNumber);
  }

  public StackFrame UserStackFrame
  {
    get => this.StackTrace?.GetFrame(this.UserStackFrameNumberLegacy ?? this.UserStackFrameNumber);
  }

  public int UserStackFrameNumber { get; private set; }

  public int? UserStackFrameNumberLegacy { get; private set; }

  public StackTrace StackTrace { get; private set; }

  public MethodBase GetCallerStackFrameMethod(int skipFrames)
  {
    return this.StackTrace?.GetFrame(this.UserStackFrameNumber + skipFrames)?.GetMethod();
  }

  public string GetCallerClassName(
    MethodBase method,
    bool includeNameSpace,
    bool cleanAsyncMoveNext,
    bool cleanAnonymousDelegates)
  {
    if (!string.IsNullOrEmpty(this.CallerClassName))
    {
      if (includeNameSpace)
        return this.CallerClassName;
      int num = this.CallerClassName.LastIndexOf('.');
      return num < 0 || num >= this.CallerClassName.Length - 1 ? this.CallerClassName : this.CallerClassName.Substring(num + 1);
    }
    MethodBase methodBase = method;
    if ((object) methodBase == null)
      methodBase = this.GetCallerStackFrameMethod(0);
    method = methodBase;
    if (method == (MethodBase) null)
      return string.Empty;
    int? frameNumberLegacy;
    int num1;
    if (!cleanAsyncMoveNext)
    {
      frameNumberLegacy = this.UserStackFrameNumberLegacy;
      num1 = frameNumberLegacy.HasValue ? 1 : 0;
    }
    else
      num1 = 1;
    cleanAsyncMoveNext = num1 != 0;
    int num2;
    if (!cleanAnonymousDelegates)
    {
      frameNumberLegacy = this.UserStackFrameNumberLegacy;
      num2 = frameNumberLegacy.HasValue ? 1 : 0;
    }
    else
      num2 = 1;
    cleanAnonymousDelegates = num2 != 0;
    return StackTraceUsageUtils.GetStackFrameMethodClassName(method, includeNameSpace, cleanAsyncMoveNext, cleanAnonymousDelegates) ?? string.Empty;
  }

  public string GetCallerMemberName(
    MethodBase method,
    bool includeMethodInfo,
    bool cleanAsyncMoveNext,
    bool cleanAnonymousDelegates)
  {
    if (!string.IsNullOrEmpty(this.CallerMemberName))
      return this.CallerMemberName;
    MethodBase methodBase = method;
    if ((object) methodBase == null)
      methodBase = this.GetCallerStackFrameMethod(0);
    method = methodBase;
    if (method == (MethodBase) null)
      return string.Empty;
    int? frameNumberLegacy;
    int num1;
    if (!cleanAsyncMoveNext)
    {
      frameNumberLegacy = this.UserStackFrameNumberLegacy;
      num1 = frameNumberLegacy.HasValue ? 1 : 0;
    }
    else
      num1 = 1;
    cleanAsyncMoveNext = num1 != 0;
    int num2;
    if (!cleanAnonymousDelegates)
    {
      frameNumberLegacy = this.UserStackFrameNumberLegacy;
      num2 = frameNumberLegacy.HasValue ? 1 : 0;
    }
    else
      num2 = 1;
    cleanAnonymousDelegates = num2 != 0;
    return StackTraceUsageUtils.GetStackFrameMethodName(method, includeMethodInfo, cleanAsyncMoveNext, cleanAnonymousDelegates) ?? string.Empty;
  }

  public string GetCallerFilePath(int skipFrames)
  {
    return !string.IsNullOrEmpty(this.CallerFilePath) ? this.CallerFilePath : this.StackTrace?.GetFrame(this.UserStackFrameNumber + skipFrames)?.GetFileName() ?? string.Empty;
  }

  public int GetCallerLineNumber(int skipFrames)
  {
    if (this.CallerLineNumber.HasValue)
      return this.CallerLineNumber.Value;
    StackFrame frame = this.StackTrace?.GetFrame(this.UserStackFrameNumber + skipFrames);
    return frame == null ? 0 : frame.GetFileLineNumber();
  }

  public string CallerClassName { get; private set; }

  public string CallerMemberName { get; private set; }

  public string CallerFilePath { get; private set; }

  public int? CallerLineNumber { get; private set; }

  private static int? FindCallingMethodOnStackTrace(StackFrame[] stackFrames, Type loggerType)
  {
    if (stackFrames == null || stackFrames.Length == 0)
      return new int?();
    int? nullable1 = new int?();
    int? nullable2 = new int?();
    for (int index = 0; index < stackFrames.Length; ++index)
    {
      StackFrame stackFrame = stackFrames[index];
      if (!CallSiteInformation.SkipAssembly(stackFrame))
      {
        if (!nullable2.HasValue)
          nullable2 = new int?(index);
        if (CallSiteInformation.IsLoggerType(stackFrame, loggerType))
          nullable1 = new int?();
        else if (!nullable1.HasValue)
          nullable1 = new int?(index);
      }
    }
    return nullable1 ?? nullable2;
  }

  private static int SkipToUserStackFrameLegacy(StackFrame[] stackFrames, int firstUserStackFrame)
  {
    for (int stackFrameLegacy = firstUserStackFrame; stackFrameLegacy < stackFrames.Length; ++stackFrameLegacy)
    {
      StackFrame stackFrame = stackFrames[stackFrameLegacy];
      if (!CallSiteInformation.SkipAssembly(stackFrame))
      {
        if (stackFrame.GetMethod()?.Name == "MoveNext" && stackFrames.Length > stackFrameLegacy)
        {
          Type declaringType = stackFrames[stackFrameLegacy + 1].GetMethod()?.DeclaringType;
          if (declaringType?.Namespace == "System.Runtime.CompilerServices" || declaringType == typeof (ExecutionContext))
            continue;
        }
        return stackFrameLegacy;
      }
    }
    return firstUserStackFrame;
  }

  private static bool SkipAssembly(StackFrame frame)
  {
    Assembly assembly = StackTraceUsageUtils.LookupAssemblyFromStackFrame(frame);
    return assembly == (Assembly) null || LogManager.IsHiddenAssembly(assembly);
  }

  private static bool IsLoggerType(StackFrame frame, Type loggerType)
  {
    Type declaringType = frame.GetMethod()?.DeclaringType;
    if (!(declaringType != (Type) null))
      return false;
    return loggerType == declaringType || declaringType.IsSubclassOf(loggerType) || loggerType.IsAssignableFrom(declaringType);
  }
}
