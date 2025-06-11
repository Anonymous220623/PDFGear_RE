// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ChainsawTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Targets;

[Target("Chainsaw")]
public class ChainsawTarget : NLogViewerTarget
{
  public ChainsawTarget()
  {
    this.IncludeNLogData = false;
    this.OptimizeBufferReuse = this.GetType() == typeof (ChainsawTarget);
  }

  public ChainsawTarget(string name)
    : this()
  {
    this.Name = name;
  }
}
