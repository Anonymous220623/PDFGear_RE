// Decompiled with JetBrains decompiler
// Type: NLog.Targets.MemoryTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets;

[Target("Memory")]
public sealed class MemoryTarget : TargetWithLayout
{
  public MemoryTarget()
  {
    this.Logs = (IList<string>) new List<string>();
    this.OptimizeBufferReuse = true;
  }

  public MemoryTarget(string name)
    : this()
  {
    this.Name = name;
  }

  public IList<string> Logs { get; }

  [DefaultValue(0)]
  public int MaxLogsCount { get; set; }

  protected override void Write(LogEventInfo logEvent)
  {
    if (this.MaxLogsCount > 0 && this.Logs.Count >= this.MaxLogsCount)
      this.Logs.RemoveAt(0);
    this.Logs.Add(this.RenderLogEvent(this.Layout, logEvent));
  }
}
