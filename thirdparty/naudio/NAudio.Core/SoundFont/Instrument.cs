// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.Instrument
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public class Instrument
{
  internal ushort startInstrumentZoneIndex;
  internal ushort endInstrumentZoneIndex;

  public string Name { get; set; }

  public Zone[] Zones { get; set; }

  public override string ToString() => this.Name;
}
