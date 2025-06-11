// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.RawSourceWaveStream
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.Wave;

public class RawSourceWaveStream : WaveStream
{
  private readonly Stream sourceStream;
  private readonly WaveFormat waveFormat;

  public RawSourceWaveStream(Stream sourceStream, WaveFormat waveFormat)
  {
    this.sourceStream = sourceStream;
    this.waveFormat = waveFormat;
  }

  public RawSourceWaveStream(byte[] byteStream, int offset, int count, WaveFormat waveFormat)
  {
    this.sourceStream = (Stream) new MemoryStream(byteStream, offset, count);
    this.waveFormat = waveFormat;
  }

  public override WaveFormat WaveFormat => this.waveFormat;

  public override long Length => this.sourceStream.Length;

  public override long Position
  {
    get => this.sourceStream.Position;
    set => this.sourceStream.Position = value - value % (long) this.waveFormat.BlockAlign;
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    try
    {
      return this.sourceStream.Read(buffer, offset, count);
    }
    catch (EndOfStreamException ex)
    {
      return 0;
    }
  }
}
