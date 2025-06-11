// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StackTraceUsageUtils
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog.Internal;

internal static class StackTraceUsageUtils
{
  private static readonly Assembly nlogAssembly = typeof (StackTraceUsageUtils).GetAssembly();
  private static readonly Assembly mscorlibAssembly = typeof (string).GetAssembly();
  private static readonly Assembly systemAssembly = typeof (Debug).GetAssembly();

  internal static StackTraceUsage Max(StackTraceUsage u1, StackTraceUsage u2)
  {
    return (StackTraceUsage) Math.Max((int) u1, (int) u2);
  }

  public static int GetFrameCount(this StackTrace strackTrace) => strackTrace.FrameCount;

  public static string GetStackFrameMethodName(
    MethodBase method,
    bool includeMethodInfo,
    bool cleanAsyncMoveNext,
    bool cleanAnonymousDelegates)
  {
    if (method == (MethodBase) null)
      return (string) null;
    string stackFrameMethodName = method.Name;
    Type declaringType = method.DeclaringType;
    if (cleanAsyncMoveNext && stackFrameMethodName == "MoveNext" && declaringType?.DeclaringType != (Type) null && declaringType.Name.StartsWith("<"))
    {
      int num = declaringType.Name.IndexOf('>', 1);
      if (num > 1)
      {
        stackFrameMethodName = declaringType.Name.Substring(1, num - 1);
        if (stackFrameMethodName.StartsWith("<"))
          stackFrameMethodName = stackFrameMethodName.Substring(1, stackFrameMethodName.Length - 1);
      }
    }
    if (cleanAnonymousDelegates && stackFrameMethodName.StartsWith("<") && stackFrameMethodName.Contains("__") && stackFrameMethodName.Contains(">"))
    {
      int startIndex = stackFrameMethodName.IndexOf('<') + 1;
      int num = stackFrameMethodName.IndexOf('>');
      stackFrameMethodName = stackFrameMethodName.Substring(startIndex, num - startIndex);
    }
    if (includeMethodInfo && stackFrameMethodName == method.Name)
      stackFrameMethodName = method.ToString();
    return stackFrameMethodName;
  }

  public static string GetStackFrameMethodClassName(
    MethodBase method,
    bool includeNameSpace,
    bool cleanAsyncMoveNext,
    bool cleanAnonymousDelegates)
  {
    if (method == (MethodBase) null)
      return (string) null;
    Type declaringType = method.DeclaringType;
    if (cleanAsyncMoveNext && method.Name == "MoveNext" && declaringType?.DeclaringType != (Type) null && declaringType.Name.StartsWith("<") && declaringType.Name.IndexOf('>', 1) > 1)
      declaringType = declaringType.DeclaringType;
    if (!includeNameSpace && declaringType?.DeclaringType != (Type) null && declaringType.IsNested && declaringType.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
      return declaringType.DeclaringType.Name;
    string frameMethodClassName = includeNameSpace ? declaringType?.FullName : declaringType?.Name;
    if (cleanAnonymousDelegates && frameMethodClassName != null)
    {
      int length = frameMethodClassName.IndexOf("+<>", StringComparison.Ordinal);
      if (length >= 0)
        frameMethodClassName = frameMethodClassName.Substring(0, length);
    }
    return frameMethodClassName;
  }

  [MethodImpl(MethodImplOptions.NoInlining)]
  public static string GetClassFullName()
  {
    string empty = string.Empty;
    return StackTraceUsageUtils.GetClassFullName(new StackFrame(2, false));
  }

  public static string GetClassFullName(StackFrame stackFrame)
  {
    string classFullName = StackTraceUsageUtils.LookupClassNameFromStackFrame(stackFrame);
    if (string.IsNullOrEmpty(classFullName))
    {
      classFullName = StackTraceUsageUtils.GetClassFullName(new StackTrace(false));
      if (string.IsNullOrEmpty(classFullName))
        classFullName = stackFrame.GetMethod()?.Name ?? string.Empty;
    }
    return classFullName;
  }

  private static string GetClassFullName(StackTrace stackTrace)
  {
    foreach (StackFrame frame in stackTrace.GetFrames())
    {
      string classFullName = StackTraceUsageUtils.LookupClassNameFromStackFrame(frame);
      if (!string.IsNullOrEmpty(classFullName))
        return classFullName;
    }
    return string.Empty;
  }

  public static Assembly LookupAssemblyFromStackFrame(StackFrame stackFrame)
  {
    MethodBase method = stackFrame.GetMethod();
    if (method == (MethodBase) null)
      return (Assembly) null;
    Type declaringType = method.DeclaringType;
    Assembly assembly1 = (object) declaringType != null ? declaringType.GetAssembly() : (Assembly) null;
    if ((object) assembly1 == null)
      assembly1 = method.Module?.Assembly;
    Assembly assembly2 = assembly1;
    if (assembly2 == StackTraceUsageUtils.nlogAssembly)
      return (Assembly) null;
    if (assembly2 == StackTraceUsageUtils.mscorlibAssembly)
      return (Assembly) null;
    return assembly2 == StackTraceUsageUtils.systemAssembly ? (Assembly) null : assembly2;
  }

  public static string LookupClassNameFromStackFrame(StackFrame stackFrame)
  {
    MethodBase method = stackFrame.GetMethod();
    if (method != (MethodBase) null && StackTraceUsageUtils.LookupAssemblyFromStackFrame(stackFrame) != (Assembly) null)
    {
      string frameMethodClassName = StackTraceUsageUtils.GetStackFrameMethodClassName(method, true, true, true);
      if (!string.IsNullOrEmpty(frameMethodClassName))
      {
        if (!frameMethodClassName.StartsWith("System.", StringComparison.Ordinal))
          return frameMethodClassName;
      }
      else
      {
        string str = method.Name ?? string.Empty;
        if (str != "lambda_method" && str != "MoveNext")
          return str;
      }
    }
    return string.Empty;
  }
}
