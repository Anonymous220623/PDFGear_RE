// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.PresetsChunk
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;
using System.Text;

#nullable disable
namespace NAudio.SoundFont;

public class PresetsChunk
{
  private PresetBuilder presetHeaders = new PresetBuilder();
  private ZoneBuilder presetZones = new ZoneBuilder();
  private ModulatorBuilder presetZoneModulators = new ModulatorBuilder();
  private GeneratorBuilder presetZoneGenerators = new GeneratorBuilder();
  private InstrumentBuilder instruments = new InstrumentBuilder();
  private ZoneBuilder instrumentZones = new ZoneBuilder();
  private ModulatorBuilder instrumentZoneModulators = new ModulatorBuilder();
  private GeneratorBuilder instrumentZoneGenerators = new GeneratorBuilder();
  private SampleHeaderBuilder sampleHeaders = new SampleHeaderBuilder();

  internal PresetsChunk(RiffChunk chunk)
  {
    string str = chunk.ReadChunkID();
    if (str != "pdta")
      throw new InvalidDataException($"Not a presets data chunk ({str})");
    RiffChunk nextSubChunk;
    while ((nextSubChunk = chunk.GetNextSubChunk()) != null)
    {
      string chunkId = nextSubChunk.ChunkID;
      if (chunkId != null && chunkId.Length == 4)
      {
        switch (chunkId[1])
        {
          case 'B':
            switch (chunkId)
            {
              case "PBAG":
                goto label_15;
              case "IBAG":
                goto label_19;
              default:
                goto label_23;
            }
          case 'G':
            switch (chunkId)
            {
              case "PGEN":
                goto label_17;
              case "IGEN":
                goto label_21;
              default:
                goto label_23;
            }
          case 'H':
            switch (chunkId)
            {
              case "PHDR":
                break;
              case "SHDR":
                goto label_22;
              default:
                goto label_23;
            }
            break;
          case 'M':
            switch (chunkId)
            {
              case "PMOD":
                goto label_16;
              case "IMOD":
                goto label_20;
              default:
                goto label_23;
            }
          case 'N':
            if (chunkId == "INST")
              goto label_18;
            goto label_23;
          case 'b':
            switch (chunkId)
            {
              case "pbag":
                goto label_15;
              case "ibag":
                goto label_19;
              default:
                goto label_23;
            }
          case 'g':
            switch (chunkId)
            {
              case "pgen":
                goto label_17;
              case "igen":
                goto label_21;
              default:
                goto label_23;
            }
          case 'h':
            switch (chunkId)
            {
              case "phdr":
                break;
              case "shdr":
                goto label_22;
              default:
                goto label_23;
            }
            break;
          case 'm':
            switch (chunkId)
            {
              case "pmod":
                goto label_16;
              case "imod":
                goto label_20;
              default:
                goto label_23;
            }
          case 'n':
            if (chunkId == "inst")
              goto label_18;
            goto label_23;
          default:
            goto label_23;
        }
        nextSubChunk.GetDataAsStructureArray<Preset>((StructureBuilder<Preset>) this.presetHeaders);
        continue;
label_15:
        nextSubChunk.GetDataAsStructureArray<Zone>((StructureBuilder<Zone>) this.presetZones);
        continue;
label_16:
        nextSubChunk.GetDataAsStructureArray<Modulator>((StructureBuilder<Modulator>) this.presetZoneModulators);
        continue;
label_17:
        nextSubChunk.GetDataAsStructureArray<Generator>((StructureBuilder<Generator>) this.presetZoneGenerators);
        continue;
label_18:
        nextSubChunk.GetDataAsStructureArray<Instrument>((StructureBuilder<Instrument>) this.instruments);
        continue;
label_19:
        nextSubChunk.GetDataAsStructureArray<Zone>((StructureBuilder<Zone>) this.instrumentZones);
        continue;
label_20:
        nextSubChunk.GetDataAsStructureArray<Modulator>((StructureBuilder<Modulator>) this.instrumentZoneModulators);
        continue;
label_21:
        nextSubChunk.GetDataAsStructureArray<Generator>((StructureBuilder<Generator>) this.instrumentZoneGenerators);
        continue;
label_22:
        nextSubChunk.GetDataAsStructureArray<SampleHeader>((StructureBuilder<SampleHeader>) this.sampleHeaders);
        continue;
      }
label_23:
      throw new InvalidDataException($"Unknown chunk type {nextSubChunk.ChunkID}");
    }
    this.instrumentZoneGenerators.Load(this.sampleHeaders.SampleHeaders);
    this.instrumentZones.Load(this.instrumentZoneModulators.Modulators, this.instrumentZoneGenerators.Generators);
    this.instruments.LoadZones(this.instrumentZones.Zones);
    this.presetZoneGenerators.Load(this.instruments.Instruments);
    this.presetZones.Load(this.presetZoneModulators.Modulators, this.presetZoneGenerators.Generators);
    this.presetHeaders.LoadZones(this.presetZones.Zones);
    this.sampleHeaders.RemoveEOS();
  }

  public Preset[] Presets => this.presetHeaders.Presets;

  public Instrument[] Instruments => this.instruments.Instruments;

  public SampleHeader[] SampleHeaders => this.sampleHeaders.SampleHeaders;

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("Preset Headers:\r\n");
    foreach (Preset preset in this.presetHeaders.Presets)
      stringBuilder.AppendFormat("{0}\r\n", (object) preset);
    stringBuilder.Append("Instruments:\r\n");
    foreach (Instrument instrument in this.instruments.Instruments)
      stringBuilder.AppendFormat("{0}\r\n", (object) instrument);
    return stringBuilder.ToString();
  }
}
