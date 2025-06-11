// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFormatExtensible
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Dmo;
using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class WaveFormatExtensible : WaveFormat
{
  private short wValidBitsPerSample;
  private int dwChannelMask;
  private Guid subFormat;

  private WaveFormatExtensible()
  {
  }

  public WaveFormatExtensible(int rate, int bits, int channels)
    : base(rate, bits, channels)
  {
    this.waveFormatTag = WaveFormatEncoding.Extensible;
    this.extraSize = (short) 22;
    this.wValidBitsPerSample = (short) bits;
    for (int index = 0; index < channels; ++index)
      this.dwChannelMask |= 1 << index;
    if (bits == 32 /*0x20*/)
      this.subFormat = AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT;
    else
      this.subFormat = AudioMediaSubtypes.MEDIASUBTYPE_PCM;
  }

  public WaveFormat ToStandardWaveFormat()
  {
    if (this.subFormat == AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT && this.bitsPerSample == (short) 32 /*0x20*/)
      return WaveFormat.CreateIeeeFloatWaveFormat(this.sampleRate, (int) this.channels);
    return this.subFormat == AudioMediaSubtypes.MEDIASUBTYPE_PCM ? new WaveFormat(this.sampleRate, (int) this.bitsPerSample, (int) this.channels) : (WaveFormat) this;
  }

  public Guid SubFormat => this.subFormat;

  public override void Serialize(BinaryWriter writer)
  {
    base.Serialize(writer);
    writer.Write(this.wValidBitsPerSample);
    writer.Write(this.dwChannelMask);
    byte[] byteArray = this.subFormat.ToByteArray();
    writer.Write(byteArray, 0, byteArray.Length);
  }

  public override string ToString()
  {
    return $"WAVE_FORMAT_EXTENSIBLE {AudioMediaSubtypes.GetAudioSubtypeName(this.subFormat)} {$"{this.SampleRate}Hz {this.Channels} channels {this.BitsPerSample} bit"}";
  }
}
