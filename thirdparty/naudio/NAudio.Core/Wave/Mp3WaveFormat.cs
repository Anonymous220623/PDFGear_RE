// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Mp3WaveFormat
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class Mp3WaveFormat : WaveFormat
{
  public Mp3WaveFormatId id;
  public Mp3WaveFormatFlags flags;
  public ushort blockSize;
  public ushort framesPerBlock;
  public ushort codecDelay;
  private const short Mp3WaveFormatExtraBytes = 12;

  public Mp3WaveFormat(int sampleRate, int channels, int blockSize, int bitRate)
  {
    this.waveFormatTag = WaveFormatEncoding.MpegLayer3;
    this.channels = (short) channels;
    this.averageBytesPerSecond = bitRate / 8;
    this.bitsPerSample = (short) 0;
    this.blockAlign = (short) 1;
    this.sampleRate = sampleRate;
    this.extraSize = (short) 12;
    this.id = Mp3WaveFormatId.Mpeg;
    this.flags = Mp3WaveFormatFlags.PaddingIso;
    this.blockSize = (ushort) blockSize;
    this.framesPerBlock = (ushort) 1;
    this.codecDelay = (ushort) 0;
  }
}
