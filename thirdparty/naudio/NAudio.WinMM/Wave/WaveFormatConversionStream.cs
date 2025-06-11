// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFormatConversionStream
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using NAudio.Wave.Compression;
using System;

#nullable disable
namespace NAudio.Wave;

public class WaveFormatConversionStream : WaveStream
{
  private readonly WaveFormatConversionProvider conversionProvider;
  private readonly WaveFormat targetFormat;
  private readonly long length;
  private long position;
  private readonly WaveStream sourceStream;
  private bool isDisposed;

  public WaveFormatConversionStream(WaveFormat targetFormat, WaveStream sourceStream)
  {
    this.sourceStream = sourceStream;
    this.targetFormat = targetFormat;
    this.conversionProvider = new WaveFormatConversionProvider(targetFormat, (IWaveProvider) sourceStream);
    this.length = this.EstimateSourceToDest((long) (int) sourceStream.Length);
    this.position = 0L;
  }

  public static WaveStream CreatePcmStream(WaveStream sourceStream)
  {
    if (sourceStream.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
      return sourceStream;
    WaveFormat targetFormat = AcmStream.SuggestPcmFormat(sourceStream.WaveFormat);
    if (targetFormat.SampleRate < 8000)
    {
      if (sourceStream.WaveFormat.Encoding != WaveFormatEncoding.G723)
        throw new InvalidOperationException("Invalid suggested output format, please explicitly provide a target format");
      targetFormat = new WaveFormat(8000, 16 /*0x10*/, 1);
    }
    return (WaveStream) new WaveFormatConversionStream(targetFormat, sourceStream);
  }

  public override long Position
  {
    get => this.position;
    set
    {
      value -= value % (long) this.BlockAlign;
      this.sourceStream.Position = this.EstimateDestToSource(value);
      this.position = this.EstimateSourceToDest(this.sourceStream.Position);
      this.conversionProvider.Reposition();
    }
  }

  [Obsolete("can be unreliable, use of this method not encouraged")]
  public int SourceToDest(int source) => (int) this.EstimateSourceToDest((long) source);

  private long EstimateSourceToDest(long source)
  {
    long num = source * (long) this.targetFormat.AverageBytesPerSecond / (long) this.sourceStream.WaveFormat.AverageBytesPerSecond;
    return num - num % (long) this.targetFormat.BlockAlign;
  }

  private long EstimateDestToSource(long dest)
  {
    long num = dest * (long) this.sourceStream.WaveFormat.AverageBytesPerSecond / (long) this.targetFormat.AverageBytesPerSecond;
    return (long) (int) (num - num % (long) this.sourceStream.WaveFormat.BlockAlign);
  }

  [Obsolete("can be unreliable, use of this method not encouraged")]
  public int DestToSource(int dest) => (int) this.EstimateDestToSource((long) dest);

  public override long Length => this.length;

  public override WaveFormat WaveFormat => this.targetFormat;

  public override int Read(byte[] buffer, int offset, int count)
  {
    int num = this.conversionProvider.Read(buffer, offset, count);
    this.position += (long) num;
    return num;
  }

  protected override void Dispose(bool disposing)
  {
    if (!this.isDisposed)
    {
      this.isDisposed = true;
      if (disposing)
      {
        this.sourceStream.Dispose();
        this.conversionProvider.Dispose();
      }
    }
    base.Dispose(disposing);
  }
}
