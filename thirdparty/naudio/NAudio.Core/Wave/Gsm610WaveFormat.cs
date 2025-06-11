// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Gsm610WaveFormat
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class Gsm610WaveFormat : WaveFormat
{
  private readonly short samplesPerBlock;

  public Gsm610WaveFormat()
  {
    this.waveFormatTag = WaveFormatEncoding.Gsm610;
    this.channels = (short) 1;
    this.averageBytesPerSecond = 1625;
    this.bitsPerSample = (short) 0;
    this.blockAlign = (short) 65;
    this.sampleRate = 8000;
    this.extraSize = (short) 2;
    this.samplesPerBlock = (short) 320;
  }

  public short SamplesPerBlock => this.samplesPerBlock;

  public override void Serialize(BinaryWriter writer)
  {
    base.Serialize(writer);
    writer.Write(this.samplesPerBlock);
  }
}
