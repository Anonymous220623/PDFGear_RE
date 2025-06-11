// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.Modulator
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public class Modulator
{
  public ModulatorType SourceModulationData { get; set; }

  public GeneratorEnum DestinationGenerator { get; set; }

  public short Amount { get; set; }

  public ModulatorType SourceModulationAmount { get; set; }

  public TransformEnum SourceTransform { get; set; }

  public override string ToString()
  {
    return $"Modulator {this.SourceModulationData} {this.DestinationGenerator} {this.Amount} {this.SourceModulationAmount} {this.SourceTransform}";
  }
}
