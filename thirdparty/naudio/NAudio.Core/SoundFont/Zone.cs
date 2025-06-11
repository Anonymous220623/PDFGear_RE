// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.Zone
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public class Zone
{
  internal ushort generatorIndex;
  internal ushort modulatorIndex;
  internal ushort generatorCount;
  internal ushort modulatorCount;

  public override string ToString()
  {
    return $"Zone {this.generatorCount} Gens:{this.generatorIndex} {this.modulatorCount} Mods:{this.modulatorIndex}";
  }

  public Modulator[] Modulators { get; set; }

  public Generator[] Generators { get; set; }
}
