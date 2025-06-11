// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.AdpcmWaveFormat
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class AdpcmWaveFormat : WaveFormat
{
  private short samplesPerBlock;
  private short numCoeff;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
  private short[] coefficients;

  private AdpcmWaveFormat()
    : this(8000, 1)
  {
  }

  public int SamplesPerBlock => (int) this.samplesPerBlock;

  public int NumCoefficients => (int) this.numCoeff;

  public short[] Coefficients => this.coefficients;

  public AdpcmWaveFormat(int sampleRate, int channels)
    : base(sampleRate, 0, channels)
  {
    this.waveFormatTag = WaveFormatEncoding.Adpcm;
    this.extraSize = (short) 32 /*0x20*/;
    switch (this.sampleRate)
    {
      case 8000:
      case 11025:
        this.blockAlign = (short) 256 /*0x0100*/;
        break;
      case 22050:
        this.blockAlign = (short) 512 /*0x0200*/;
        break;
      default:
        this.blockAlign = (short) 1024 /*0x0400*/;
        break;
    }
    this.bitsPerSample = (short) 4;
    this.samplesPerBlock = (short) (((int) this.blockAlign - 7 * channels) * 8 / ((int) this.bitsPerSample * channels) + 2);
    this.averageBytesPerSecond = this.SampleRate * (int) this.blockAlign / (int) this.samplesPerBlock;
    this.numCoeff = (short) 7;
    this.coefficients = new short[14]
    {
      (short) 256 /*0x0100*/,
      (short) 0,
      (short) 512 /*0x0200*/,
      (short) -256,
      (short) 0,
      (short) 0,
      (short) 192 /*0xC0*/,
      (short) 64 /*0x40*/,
      (short) 240 /*0xF0*/,
      (short) 0,
      (short) 460,
      (short) -208,
      (short) 392,
      (short) -232
    };
  }

  public override void Serialize(BinaryWriter writer)
  {
    base.Serialize(writer);
    writer.Write(this.samplesPerBlock);
    writer.Write(this.numCoeff);
    foreach (short coefficient in this.coefficients)
      writer.Write(coefficient);
  }

  public override string ToString()
  {
    return $"Microsoft ADPCM {this.SampleRate} Hz {this.channels} channels {this.bitsPerSample} bits per sample {this.samplesPerBlock} samples per block";
  }
}
