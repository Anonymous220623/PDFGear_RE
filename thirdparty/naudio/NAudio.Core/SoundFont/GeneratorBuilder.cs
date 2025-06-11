// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.GeneratorBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class GeneratorBuilder : StructureBuilder<Generator>
{
  public override Generator Read(BinaryReader br)
  {
    Generator generator = new Generator();
    generator.GeneratorType = (GeneratorEnum) br.ReadUInt16();
    generator.UInt16Amount = br.ReadUInt16();
    this.data.Add(generator);
    return generator;
  }

  public override void Write(BinaryWriter bw, Generator o)
  {
  }

  public override int Length => 4;

  public Generator[] Generators => this.data.ToArray();

  public void Load(Instrument[] instruments)
  {
    foreach (Generator generator in this.Generators)
    {
      if (generator.GeneratorType == GeneratorEnum.Instrument)
        generator.Instrument = instruments[(int) generator.UInt16Amount];
    }
  }

  public void Load(SampleHeader[] sampleHeaders)
  {
    foreach (Generator generator in this.Generators)
    {
      if (generator.GeneratorType == GeneratorEnum.SampleID)
        generator.SampleHeader = sampleHeaders[(int) generator.UInt16Amount];
    }
  }
}
