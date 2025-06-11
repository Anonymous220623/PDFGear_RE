// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.ModulatorBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class ModulatorBuilder : StructureBuilder<Modulator>
{
  public override Modulator Read(BinaryReader br)
  {
    Modulator modulator = new Modulator();
    modulator.SourceModulationData = new ModulatorType(br.ReadUInt16());
    modulator.DestinationGenerator = (GeneratorEnum) br.ReadUInt16();
    modulator.Amount = br.ReadInt16();
    modulator.SourceModulationAmount = new ModulatorType(br.ReadUInt16());
    modulator.SourceTransform = (TransformEnum) br.ReadUInt16();
    this.data.Add(modulator);
    return modulator;
  }

  public override void Write(BinaryWriter bw, Modulator o)
  {
  }

  public override int Length => 10;

  public Modulator[] Modulators => this.data.ToArray();
}
