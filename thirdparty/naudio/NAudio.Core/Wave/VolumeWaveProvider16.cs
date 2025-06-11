// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.VolumeWaveProvider16
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class VolumeWaveProvider16 : IWaveProvider
{
  private readonly IWaveProvider sourceProvider;
  private float volume;

  public VolumeWaveProvider16(IWaveProvider sourceProvider)
  {
    this.Volume = 1f;
    this.sourceProvider = sourceProvider;
    if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
      throw new ArgumentException("Expecting PCM input");
    if (sourceProvider.WaveFormat.BitsPerSample != 16 /*0x10*/)
      throw new ArgumentException("Expecting 16 bit");
  }

  public float Volume
  {
    get => this.volume;
    set => this.volume = value;
  }

  public WaveFormat WaveFormat => this.sourceProvider.WaveFormat;

  public int Read(byte[] buffer, int offset, int count)
  {
    int num1 = this.sourceProvider.Read(buffer, offset, count);
    if ((double) this.volume == 0.0)
    {
      for (int index = 0; index < num1; ++index)
        buffer[offset++] = (byte) 0;
    }
    else if ((double) this.volume != 1.0)
    {
      for (int index = 0; index < num1; index += 2)
      {
        float num2 = (float) (short) ((int) buffer[offset + 1] << 8 | (int) buffer[offset]) * this.volume;
        short num3 = (short) num2;
        if ((double) this.Volume > 1.0)
        {
          if ((double) num2 > (double) short.MaxValue)
            num3 = short.MaxValue;
          else if ((double) num2 < (double) short.MinValue)
            num3 = short.MinValue;
        }
        buffer[offset++] = (byte) ((uint) num3 & (uint) byte.MaxValue);
        buffer[offset++] = (byte) ((uint) num3 >> 8);
      }
    }
    return num1;
  }
}
