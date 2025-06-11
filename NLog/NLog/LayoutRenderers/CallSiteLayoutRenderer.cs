// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.CallSiteLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("callsite")]
[ThreadAgnostic]
[ThreadSafe]
public class CallSiteLayoutRenderer : LayoutRenderer, IUsesStackTrace
{
  public CallSiteLayoutRenderer()
  {
    this.ClassName = true;
    this.MethodName = true;
    this.CleanNamesOfAnonymousDelegates = false;
    this.IncludeNamespace = true;
    this.FileName = false;
    this.IncludeSourcePath = true;
  }

  [DefaultValue(true)]
  public bool ClassName { get; set; }

  [DefaultValue(true)]
  public bool IncludeNamespace { get; set; }

  [DefaultValue(true)]
  public bool MethodName { get; set; }

  [DefaultValue(false)]
  public bool CleanNamesOfAnonymousDelegates { get; set; }

  [DefaultValue(false)]
  public bool CleanNamesOfAsyncContinuations { get; set; }

  [DefaultValue(0)]
  public int SkipFrames { get; set; }

  [DefaultValue(false)]
  public bool FileName { get; set; }

  [DefaultValue(true)]
  public bool IncludeSourcePath { get; set; }

  [DefaultValue(true)]
  public bool CaptureStackTrace { get; set; } = true;

  StackTraceUsage IUsesStackTrace.StackTraceUsage
  {
    get
    {
      if (!this.CaptureStackTrace)
        return StackTraceUsage.None;
      return this.FileName ? StackTraceUsage.WithSource : StackTraceUsage.WithoutSource;
    }
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (logEvent.CallSiteInformation == null)
      return;
    if (this.ClassName || this.MethodName)
    {
      MethodBase stackFrameMethod = logEvent.CallSiteInformation.GetCallerStackFrameMethod(this.SkipFrames);
      if (this.ClassName)
      {
        string str = logEvent.CallSiteInformation.GetCallerClassName(stackFrameMethod, this.IncludeNamespace, this.CleanNamesOfAsyncContinuations, this.CleanNamesOfAnonymousDelegates);
        if (string.IsNullOrEmpty(str))
          str = "<no type>";
        builder.Append(str);
      }
      if (this.MethodName)
      {
        string str = logEvent.CallSiteInformation.GetCallerMemberName(stackFrameMethod, false, this.CleanNamesOfAsyncContinuations, this.CleanNamesOfAnonymousDelegates);
        if (string.IsNullOrEmpty(str))
          str = "<no method>";
        if (this.ClassName)
          builder.Append(".");
        builder.Append(str);
      }
    }
    if (!this.FileName)
      return;
    string callerFilePath = logEvent.CallSiteInformation.GetCallerFilePath(this.SkipFrames);
    if (string.IsNullOrEmpty(callerFilePath))
      return;
    int callerLineNumber = logEvent.CallSiteInformation.GetCallerLineNumber(this.SkipFrames);
    this.AppendFileName(builder, callerFilePath, callerLineNumber);
  }

  private void AppendFileName(StringBuilder builder, string fileName, int lineNumber)
  {
    builder.Append("(");
    if (this.IncludeSourcePath)
      builder.Append(fileName);
    else
      builder.Append(Path.GetFileName(fileName));
    builder.Append(":");
    builder.AppendInvariant(lineNumber);
    builder.Append(")");
  }
}
