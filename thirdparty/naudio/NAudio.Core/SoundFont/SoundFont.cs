// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.SoundFont
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.SoundFont;

public class SoundFont
{
  private InfoChunk info;
  private PresetsChunk presetsChunk;
  private SampleDataChunk sampleData;

  public SoundFont(string fileName)
    : this((Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read))
  {
  }

  public SoundFont(Stream sfFile)
  {
    using (sfFile)
    {
      RiffChunk topLevelChunk = RiffChunk.GetTopLevelChunk(new BinaryReader(sfFile));
      string str = topLevelChunk.ChunkID == "RIFF" ? topLevelChunk.ReadChunkID() : throw new InvalidDataException("Not a RIFF file");
      if (str != "sfbk")
        throw new InvalidDataException($"Not a SoundFont ({str})");
      RiffChunk nextSubChunk = topLevelChunk.GetNextSubChunk();
      this.info = nextSubChunk.ChunkID == "LIST" ? new InfoChunk(nextSubChunk) : throw new InvalidDataException($"Not info list found ({nextSubChunk.ChunkID})");
      this.sampleData = new SampleDataChunk(topLevelChunk.GetNextSubChunk());
      this.presetsChunk = new PresetsChunk(topLevelChunk.GetNextSubChunk());
    }
  }

  public InfoChunk FileInfo => this.info;

  public Preset[] Presets => this.presetsChunk.Presets;

  public Instrument[] Instruments => this.presetsChunk.Instruments;

  public SampleHeader[] SampleHeaders => this.presetsChunk.SampleHeaders;

  public byte[] SampleData => this.sampleData.SampleData;

  public override string ToString()
  {
    return $"Info Chunk:\r\n{this.info}\r\nPresets Chunk:\r\n{this.presetsChunk}";
  }
}
