// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.ZoneBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class ZoneBuilder : StructureBuilder<Zone>
{
  private Zone lastZone;

  public override Zone Read(BinaryReader br)
  {
    Zone zone = new Zone();
    zone.generatorIndex = br.ReadUInt16();
    zone.modulatorIndex = br.ReadUInt16();
    if (this.lastZone != null)
    {
      this.lastZone.generatorCount = (ushort) ((uint) zone.generatorIndex - (uint) this.lastZone.generatorIndex);
      this.lastZone.modulatorCount = (ushort) ((uint) zone.modulatorIndex - (uint) this.lastZone.modulatorIndex);
    }
    this.data.Add(zone);
    this.lastZone = zone;
    return zone;
  }

  public override void Write(BinaryWriter bw, Zone zone)
  {
  }

  public void Load(Modulator[] modulators, Generator[] generators)
  {
    for (int index = 0; index < this.data.Count - 1; ++index)
    {
      Zone zone = this.data[index];
      zone.Generators = new Generator[(int) zone.generatorCount];
      Array.Copy((Array) generators, (int) zone.generatorIndex, (Array) zone.Generators, 0, (int) zone.generatorCount);
      zone.Modulators = new Modulator[(int) zone.modulatorCount];
      Array.Copy((Array) modulators, (int) zone.modulatorIndex, (Array) zone.Modulators, 0, (int) zone.modulatorCount);
    }
    this.data.RemoveAt(this.data.Count - 1);
  }

  public Zone[] Zones => this.data.ToArray();

  public override int Length => 4;
}
