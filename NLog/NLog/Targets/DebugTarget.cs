// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DebugTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Targets;

[Target("Debug")]
public sealed class DebugTarget : TargetWithLayout
{
  public DebugTarget()
  {
    this.LastMessage = string.Empty;
    this.Counter = 0;
    this.OptimizeBufferReuse = true;
  }

  public DebugTarget(string name)
    : this()
  {
    this.Name = name;
  }

  public int Counter { get; private set; }

  public string LastMessage { get; private set; }

  protected override void Write(LogEventInfo logEvent)
  {
    ++this.Counter;
    this.LastMessage = this.RenderLogEvent(this.Layout, logEvent);
  }
}
