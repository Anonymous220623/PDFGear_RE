// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.CueWaveFileReader
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.Wave;

public class CueWaveFileReader : WaveFileReader
{
  private CueList cues;

  public CueWaveFileReader(string fileName)
    : base(fileName)
  {
  }

  public CueWaveFileReader(Stream inputStream)
    : base(inputStream)
  {
  }

  public CueList Cues
  {
    get
    {
      if (this.cues == null)
        this.cues = CueList.FromChunks((WaveFileReader) this);
      return this.cues;
    }
  }
}
