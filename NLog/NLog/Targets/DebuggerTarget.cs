// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DebuggerTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace NLog.Targets;

[Target("Debugger")]
public sealed class DebuggerTarget : TargetWithLayoutHeaderAndFooter
{
  public DebuggerTarget() => this.OptimizeBufferReuse = true;

  public DebuggerTarget(string name)
    : this()
  {
    this.Name = name;
  }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (!Debugger.IsLogging())
      InternalLogger.Debug<string>("Debugger(Name={0}): System.Diagnostics.Debugger.IsLogging()==false. Output has been disabled.", this.Name);
    if (this.Header == null)
      return;
    Debugger.Log(LogLevel.Off.Ordinal, string.Empty, this.RenderLogEvent(this.Header, LogEventInfo.CreateNullEvent()) + "\n");
  }

  protected override void CloseTarget()
  {
    if (this.Footer != null)
      Debugger.Log(LogLevel.Off.Ordinal, string.Empty, this.RenderLogEvent(this.Footer, LogEventInfo.CreateNullEvent()) + "\n");
    base.CloseTarget();
  }

  protected override void Write(LogEventInfo logEvent)
  {
    if (!Debugger.IsLogging())
      return;
    string message;
    if (this.OptimizeBufferReuse)
    {
      using (ReusableObjectCreator<StringBuilder>.LockOject lockOject = this.ReusableLayoutBuilder.Allocate())
      {
        this.Layout.RenderAppendBuilder(logEvent, lockOject.Result);
        lockOject.Result.Append('\n');
        message = lockOject.Result.ToString();
      }
    }
    else
      message = this.RenderLogEvent(this.Layout, logEvent) + "\n";
    Debugger.Log(logEvent.Level.Ordinal, logEvent.LoggerName, message);
  }
}
