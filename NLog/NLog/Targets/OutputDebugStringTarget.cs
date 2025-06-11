// Decompiled with JetBrains decompiler
// Type: NLog.Targets.OutputDebugStringTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;

#nullable disable
namespace NLog.Targets;

[Target("OutputDebugString")]
public sealed class OutputDebugStringTarget : TargetWithLayout
{
  public OutputDebugStringTarget() => this.OptimizeBufferReuse = true;

  public OutputDebugStringTarget(string name)
    : this()
  {
    this.Name = name;
  }

  protected override void Write(LogEventInfo logEvent)
  {
    NativeMethods.OutputDebugString(this.RenderLogEvent(this.Layout, logEvent));
  }
}
