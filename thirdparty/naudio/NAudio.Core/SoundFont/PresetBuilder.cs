// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.PresetBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.SoundFont;

internal class PresetBuilder : StructureBuilder<Preset>
{
  private Preset lastPreset;

  public override Preset Read(BinaryReader br)
  {
    Preset preset = new Preset();
    string str = Encoding.UTF8.GetString(br.ReadBytes(20), 0, 20);
    if (str.IndexOf(char.MinValue) >= 0)
      str = str.Substring(0, str.IndexOf(char.MinValue));
    preset.Name = str;
    preset.PatchNumber = br.ReadUInt16();
    preset.Bank = br.ReadUInt16();
    preset.startPresetZoneIndex = br.ReadUInt16();
    preset.library = br.ReadUInt32();
    preset.genre = br.ReadUInt32();
    preset.morphology = br.ReadUInt32();
    if (this.lastPreset != null)
      this.lastPreset.endPresetZoneIndex = (ushort) ((uint) preset.startPresetZoneIndex - 1U);
    this.data.Add(preset);
    this.lastPreset = preset;
    return preset;
  }

  public override void Write(BinaryWriter bw, Preset preset)
  {
  }

  public override int Length => 38;

  public void LoadZones(Zone[] presetZones)
  {
    for (int index = 0; index < this.data.Count - 1; ++index)
    {
      Preset preset = this.data[index];
      preset.Zones = new Zone[(int) preset.endPresetZoneIndex - (int) preset.startPresetZoneIndex + 1];
      Array.Copy((Array) presetZones, (int) preset.startPresetZoneIndex, (Array) preset.Zones, 0, preset.Zones.Length);
    }
    this.data.RemoveAt(this.data.Count - 1);
  }

  public Preset[] Presets => this.data.ToArray();
}
