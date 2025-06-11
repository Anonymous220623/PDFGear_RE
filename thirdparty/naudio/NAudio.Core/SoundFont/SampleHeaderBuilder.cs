// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.SampleHeaderBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class SampleHeaderBuilder : StructureBuilder<SampleHeader>
{
  public override SampleHeader Read(BinaryReader br)
  {
    SampleHeader sampleHeader = new SampleHeader();
    byte[] bytes = br.ReadBytes(20);
    sampleHeader.SampleName = ByteEncoding.Instance.GetString(bytes, 0, bytes.Length);
    sampleHeader.Start = br.ReadUInt32();
    sampleHeader.End = br.ReadUInt32();
    sampleHeader.StartLoop = br.ReadUInt32();
    sampleHeader.EndLoop = br.ReadUInt32();
    sampleHeader.SampleRate = br.ReadUInt32();
    sampleHeader.OriginalPitch = br.ReadByte();
    sampleHeader.PitchCorrection = br.ReadSByte();
    sampleHeader.SampleLink = br.ReadUInt16();
    sampleHeader.SFSampleLink = (SFSampleLink) br.ReadUInt16();
    this.data.Add(sampleHeader);
    return sampleHeader;
  }

  public override void Write(BinaryWriter bw, SampleHeader sampleHeader)
  {
  }

  public override int Length => 46;

  internal void RemoveEOS() => this.data.RemoveAt(this.data.Count - 1);

  public SampleHeader[] SampleHeaders => this.data.ToArray();
}
