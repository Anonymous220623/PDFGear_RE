// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.CallSiteFileNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("callsite-filename")]
[ThreadAgnostic]
[ThreadSafe]
public class CallSiteFileNameLayoutRenderer : LayoutRenderer, IUsesStackTrace, IStringValueRenderer
{
  [DefaultValue(true)]
  public bool IncludeSourcePath { get; set; } = true;

  [DefaultValue(0)]
  public int SkipFrames { get; set; }

  [DefaultValue(true)]
  public bool CaptureStackTrace { get; set; } = true;

  StackTraceUsage IUsesStackTrace.StackTraceUsage
  {
    get => !this.CaptureStackTrace ? StackTraceUsage.None : StackTraceUsage.WithSource;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.GetStringValue(logEvent));
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    if (logEvent.CallSiteInformation != null)
    {
      string callerFilePath = logEvent.CallSiteInformation.GetCallerFilePath(this.SkipFrames);
      if (!string.IsNullOrEmpty(callerFilePath))
        return !this.IncludeSourcePath ? Path.GetFileName(callerFilePath) : callerFilePath;
    }
    return string.Empty;
  }
}
