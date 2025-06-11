// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.CallSiteLineNumberLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("callsite-linenumber")]
[ThreadAgnostic]
[ThreadSafe]
public class CallSiteLineNumberLayoutRenderer : LayoutRenderer, IUsesStackTrace, IRawValue
{
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
    int? lineNumber = this.GetLineNumber(logEvent);
    if (!lineNumber.HasValue)
      return;
    builder.AppendInvariant(lineNumber.Value);
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) this.GetLineNumber(logEvent);
    return true;
  }

  private int? GetLineNumber(LogEventInfo logEvent)
  {
    return logEvent.CallSiteInformation == null ? new int?() : new int?(logEvent.CallSiteInformation.GetCallerLineNumber(this.SkipFrames));
  }
}
