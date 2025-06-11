// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TraceTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace NLog.Targets;

[Target("Trace")]
public sealed class TraceTarget : TargetWithLayout
{
  [DefaultValue(false)]
  public bool RawWrite { get; set; }

  [DefaultValue(false)]
  public bool EnableTraceFail { get; set; }

  public TraceTarget() => this.OptimizeBufferReuse = true;

  public TraceTarget(string name)
    : this()
  {
    this.Name = name;
  }

  protected override void Write(LogEventInfo logEvent)
  {
    string message = this.RenderLogEvent(this.Layout, logEvent);
    if (this.RawWrite || logEvent.Level <= LogLevel.Debug)
      Trace.WriteLine(message);
    else if (logEvent.Level == LogLevel.Info)
      Trace.TraceInformation(message);
    else if (logEvent.Level == LogLevel.Warn)
      Trace.TraceWarning(message);
    else if (logEvent.Level == LogLevel.Error)
      Trace.TraceError(message);
    else if (logEvent.Level >= LogLevel.Fatal)
    {
      if (this.EnableTraceFail)
        Trace.Fail(message);
      else
        Trace.TraceError(message);
    }
    else
      Trace.WriteLine(message);
  }
}
