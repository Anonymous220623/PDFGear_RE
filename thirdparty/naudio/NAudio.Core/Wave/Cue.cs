// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Cue
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave;

public class Cue
{
  public int Position { get; }

  public string Label { get; }

  public Cue(int position, string label)
  {
    this.Position = position;
    this.Label = label ?? string.Empty;
  }
}
