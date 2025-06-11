// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.CueWaveFileWriter
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.Wave;

public class CueWaveFileWriter(string fileName, WaveFormat waveFormat) : WaveFileWriter(fileName, waveFormat)
{
  private CueList cues;

  public void AddCue(int position, string label)
  {
    if (this.cues == null)
      this.cues = new CueList();
    this.cues.Add(new Cue(position, label));
  }

  private void WriteCues(BinaryWriter w)
  {
    if (this.cues == null)
      return;
    int length = this.cues.GetRiffChunks().Length;
    w.Seek(0, SeekOrigin.End);
    if (w.BaseStream.Length % 2L == 1L)
      w.Write((byte) 0);
    w.Write(this.cues.GetRiffChunks(), 0, length);
    w.Seek(4, SeekOrigin.Begin);
    w.Write((int) (w.BaseStream.Length - 8L));
  }

  protected override void UpdateHeader(BinaryWriter writer)
  {
    base.UpdateHeader(writer);
    this.WriteCues(writer);
  }
}
