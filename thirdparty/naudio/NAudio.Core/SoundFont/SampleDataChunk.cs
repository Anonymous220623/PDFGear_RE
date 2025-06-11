// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.SampleDataChunk
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class SampleDataChunk
{
  public SampleDataChunk(RiffChunk chunk)
  {
    string str = chunk.ReadChunkID();
    if (str != "sdta")
      throw new InvalidDataException($"Not a sample data chunk ({str})");
    this.SampleData = chunk.GetData();
  }

  public byte[] SampleData { get; private set; }
}
