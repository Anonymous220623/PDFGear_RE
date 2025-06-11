// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFileWriter
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using NAudio.Wave.SampleProviders;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.Wave;

public class WaveFileWriter : Stream
{
  private Stream outStream;
  private readonly BinaryWriter writer;
  private long dataSizePos;
  private long factSampleCountPos;
  private long dataChunkSize;
  private readonly WaveFormat format;
  private readonly string filename;
  private readonly byte[] value24 = new byte[3];

  public static void CreateWaveFile16(string filename, ISampleProvider sourceProvider)
  {
    WaveFileWriter.CreateWaveFile(filename, (IWaveProvider) new SampleToWaveProvider16(sourceProvider));
  }

  public static void CreateWaveFile(string filename, IWaveProvider sourceProvider)
  {
    using (WaveFileWriter waveFileWriter = new WaveFileWriter(filename, sourceProvider.WaveFormat))
    {
      byte[] buffer = new byte[sourceProvider.WaveFormat.AverageBytesPerSecond * 4];
      while (true)
      {
        int count = sourceProvider.Read(buffer, 0, buffer.Length);
        if (count != 0)
          waveFileWriter.Write(buffer, 0, count);
        else
          break;
      }
    }
  }

  public static void WriteWavFileToStream(Stream outStream, IWaveProvider sourceProvider)
  {
    using (WaveFileWriter waveFileWriter = new WaveFileWriter((Stream) new IgnoreDisposeStream(outStream), sourceProvider.WaveFormat))
    {
      byte[] buffer = new byte[sourceProvider.WaveFormat.AverageBytesPerSecond * 4];
      while (true)
      {
        int count = sourceProvider.Read(buffer, 0, buffer.Length);
        if (count != 0)
          waveFileWriter.Write(buffer, 0, count);
        else
          break;
      }
      outStream.Flush();
    }
  }

  public WaveFileWriter(Stream outStream, WaveFormat format)
  {
    this.outStream = outStream;
    this.format = format;
    this.writer = new BinaryWriter(outStream, Encoding.UTF8);
    this.writer.Write(Encoding.UTF8.GetBytes("RIFF"));
    this.writer.Write(0);
    this.writer.Write(Encoding.UTF8.GetBytes("WAVE"));
    this.writer.Write(Encoding.UTF8.GetBytes("fmt "));
    format.Serialize(this.writer);
    this.CreateFactChunk();
    this.WriteDataChunkHeader();
  }

  public WaveFileWriter(string filename, WaveFormat format)
    : this((Stream) new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), format)
  {
    this.filename = filename;
  }

  private void WriteDataChunkHeader()
  {
    this.writer.Write(Encoding.UTF8.GetBytes("data"));
    this.dataSizePos = this.outStream.Position;
    this.writer.Write(0);
  }

  private void CreateFactChunk()
  {
    if (!this.HasFactChunk())
      return;
    this.writer.Write(Encoding.UTF8.GetBytes("fact"));
    this.writer.Write(4);
    this.factSampleCountPos = this.outStream.Position;
    this.writer.Write(0);
  }

  private bool HasFactChunk()
  {
    return this.format.Encoding != WaveFormatEncoding.Pcm && this.format.BitsPerSample != 0;
  }

  public string Filename => this.filename;

  public override long Length => this.dataChunkSize;

  public TimeSpan TotalTime
  {
    get
    {
      return TimeSpan.FromSeconds((double) this.Length / (double) this.WaveFormat.AverageBytesPerSecond);
    }
  }

  public WaveFormat WaveFormat => this.format;

  public override bool CanRead => false;

  public override bool CanWrite => true;

  public override bool CanSeek => false;

  public override int Read(byte[] buffer, int offset, int count)
  {
    throw new InvalidOperationException("Cannot read from a WaveFileWriter");
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    throw new InvalidOperationException("Cannot seek within a WaveFileWriter");
  }

  public override void SetLength(long value)
  {
    throw new InvalidOperationException("Cannot set length of a WaveFileWriter");
  }

  public override long Position
  {
    get => this.dataChunkSize;
    set => throw new InvalidOperationException("Repositioning a WaveFileWriter is not supported");
  }

  [Obsolete("Use Write instead")]
  public void WriteData(byte[] data, int offset, int count) => this.Write(data, offset, count);

  public override void Write(byte[] data, int offset, int count)
  {
    if (this.outStream.Length + (long) count > (long) uint.MaxValue)
      throw new ArgumentException("WAV file too large", nameof (count));
    this.outStream.Write(data, offset, count);
    this.dataChunkSize += (long) count;
  }

  public void WriteSample(float sample)
  {
    if (this.WaveFormat.BitsPerSample == 16 /*0x10*/)
    {
      this.writer.Write((short) ((double) short.MaxValue * (double) sample));
      this.dataChunkSize += 2L;
    }
    else if (this.WaveFormat.BitsPerSample == 24)
    {
      byte[] bytes = BitConverter.GetBytes((int) (2147483648.0 * (double) sample));
      this.value24[0] = bytes[1];
      this.value24[1] = bytes[2];
      this.value24[2] = bytes[3];
      this.writer.Write(this.value24);
      this.dataChunkSize += 3L;
    }
    else if (this.WaveFormat.BitsPerSample == 32 /*0x20*/ && this.WaveFormat.Encoding == WaveFormatEncoding.Extensible)
    {
      this.writer.Write((int) ushort.MaxValue * (int) sample);
      this.dataChunkSize += 4L;
    }
    else
    {
      if (this.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
        throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
      this.writer.Write(sample);
      this.dataChunkSize += 4L;
    }
  }

  public void WriteSamples(float[] samples, int offset, int count)
  {
    for (int index = 0; index < count; ++index)
      this.WriteSample(samples[offset + index]);
  }

  [Obsolete("Use WriteSamples instead")]
  public void WriteData(short[] samples, int offset, int count)
  {
    this.WriteSamples(samples, offset, count);
  }

  public void WriteSamples(short[] samples, int offset, int count)
  {
    if (this.WaveFormat.BitsPerSample == 16 /*0x10*/)
    {
      for (int index = 0; index < count; ++index)
        this.writer.Write(samples[index + offset]);
      this.dataChunkSize += (long) (count * 2);
    }
    else if (this.WaveFormat.BitsPerSample == 24)
    {
      for (int index = 0; index < count; ++index)
      {
        byte[] bytes = BitConverter.GetBytes((int) ushort.MaxValue * (int) samples[index + offset]);
        this.value24[0] = bytes[1];
        this.value24[1] = bytes[2];
        this.value24[2] = bytes[3];
        this.writer.Write(this.value24);
      }
      this.dataChunkSize += (long) (count * 3);
    }
    else if (this.WaveFormat.BitsPerSample == 32 /*0x20*/ && this.WaveFormat.Encoding == WaveFormatEncoding.Extensible)
    {
      for (int index = 0; index < count; ++index)
        this.writer.Write((int) ushort.MaxValue * (int) samples[index + offset]);
      this.dataChunkSize += (long) (count * 4);
    }
    else
    {
      if (this.WaveFormat.BitsPerSample != 32 /*0x20*/ || this.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
        throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
      for (int index = 0; index < count; ++index)
        this.writer.Write((float) samples[index + offset] / 32768f);
      this.dataChunkSize += (long) (count * 4);
    }
  }

  public override void Flush()
  {
    long position = this.writer.BaseStream.Position;
    this.UpdateHeader(this.writer);
    this.writer.BaseStream.Position = position;
  }

  protected override void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    if (this.outStream == null)
      return;
    try
    {
      this.UpdateHeader(this.writer);
    }
    finally
    {
      this.outStream.Dispose();
      this.outStream = (Stream) null;
    }
  }

  protected virtual void UpdateHeader(BinaryWriter writer)
  {
    writer.Flush();
    this.UpdateRiffChunk(writer);
    this.UpdateFactChunk(writer);
    this.UpdateDataChunk(writer);
  }

  private void UpdateDataChunk(BinaryWriter writer)
  {
    writer.Seek((int) this.dataSizePos, SeekOrigin.Begin);
    writer.Write((uint) this.dataChunkSize);
  }

  private void UpdateRiffChunk(BinaryWriter writer)
  {
    writer.Seek(4, SeekOrigin.Begin);
    writer.Write((uint) ((ulong) this.outStream.Length - 8UL));
  }

  private void UpdateFactChunk(BinaryWriter writer)
  {
    if (!this.HasFactChunk())
      return;
    int num = this.format.BitsPerSample * this.format.Channels;
    if (num == 0)
      return;
    writer.Seek((int) this.factSampleCountPos, SeekOrigin.Begin);
    writer.Write((int) (this.dataChunkSize * 8L / (long) num));
  }

  ~WaveFileWriter() => this.Dispose(false);
}
