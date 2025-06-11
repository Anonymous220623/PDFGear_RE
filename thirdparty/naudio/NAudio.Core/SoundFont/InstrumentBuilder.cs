// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.InstrumentBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.SoundFont;

internal class InstrumentBuilder : StructureBuilder<Instrument>
{
  private Instrument lastInstrument;

  public override Instrument Read(BinaryReader br)
  {
    Instrument instrument = new Instrument();
    string str = Encoding.UTF8.GetString(br.ReadBytes(20), 0, 20);
    if (str.IndexOf(char.MinValue) >= 0)
      str = str.Substring(0, str.IndexOf(char.MinValue));
    instrument.Name = str;
    instrument.startInstrumentZoneIndex = br.ReadUInt16();
    if (this.lastInstrument != null)
      this.lastInstrument.endInstrumentZoneIndex = (ushort) ((uint) instrument.startInstrumentZoneIndex - 1U);
    this.data.Add(instrument);
    this.lastInstrument = instrument;
    return instrument;
  }

  public override void Write(BinaryWriter bw, Instrument instrument)
  {
  }

  public override int Length => 22;

  public void LoadZones(Zone[] zones)
  {
    for (int index = 0; index < this.data.Count - 1; ++index)
    {
      Instrument instrument = this.data[index];
      instrument.Zones = new Zone[(int) instrument.endInstrumentZoneIndex - (int) instrument.startInstrumentZoneIndex + 1];
      Array.Copy((Array) zones, (int) instrument.startInstrumentZoneIndex, (Array) instrument.Zones, 0, instrument.Zones.Length);
    }
    this.data.RemoveAt(this.data.Count - 1);
  }

  public Instrument[] Instruments => this.data.ToArray();
}
