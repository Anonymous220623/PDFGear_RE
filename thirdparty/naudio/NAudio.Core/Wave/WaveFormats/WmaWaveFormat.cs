// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFormats.WmaWaveFormat
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.WaveFormats;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal class WmaWaveFormat : WaveFormat
{
  private short wValidBitsPerSample;
  private int dwChannelMask;
  private int dwReserved1;
  private int dwReserved2;
  private short wEncodeOptions;
  private short wReserved3;

  public WmaWaveFormat(int sampleRate, int bitsPerSample, int channels)
    : base(sampleRate, bitsPerSample, channels)
  {
    this.wValidBitsPerSample = (short) bitsPerSample;
    switch (channels)
    {
      case 1:
        this.dwChannelMask = 1;
        break;
      case 2:
        this.dwChannelMask = 3;
        break;
    }
    this.waveFormatTag = WaveFormatEncoding.WindowsMediaAudio;
  }
}
