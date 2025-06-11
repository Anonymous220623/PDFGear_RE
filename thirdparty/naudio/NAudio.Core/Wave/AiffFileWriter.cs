// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.AiffFileWriter
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.Wave;

public class AiffFileWriter : Stream
{
  private Stream outStream;
  private BinaryWriter writer;
  private long dataSizePos;
  private long commSampleCountPos;
  private long dataChunkSize = 8;
  private WaveFormat format;
  private string filename;
  private byte[] value24 = new byte[3];

  public static void CreateAiffFile(string filename, WaveStream sourceProvider)
  {
    using (AiffFileWriter aiffFileWriter = new AiffFileWriter(filename, sourceProvider.WaveFormat))
    {
      byte[] buffer = new byte[16384 /*0x4000*/];
      while (sourceProvider.Position < sourceProvider.Length)
      {
        int count1 = Math.Min((int) (sourceProvider.Length - sourceProvider.Position), buffer.Length);
        int count2 = sourceProvider.Read(buffer, 0, count1);
        if (count2 == 0)
          break;
        aiffFileWriter.Write(buffer, 0, count2);
      }
    }
  }

  public AiffFileWriter(Stream outStream, WaveFormat format)
  {
    this.outStream = outStream;
    this.format = format;
    this.writer = new BinaryWriter(outStream, Encoding.UTF8);
    this.writer.Write(Encoding.UTF8.GetBytes("FORM"));
    this.writer.Write(0);
    this.writer.Write(Encoding.UTF8.GetBytes("AIFF"));
    this.CreateCommChunk();
    this.WriteSsndChunkHeader();
  }

  public AiffFileWriter(string filename, WaveFormat format)
    : this((Stream) new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), format)
  {
    this.filename = filename;
  }

  private void WriteSsndChunkHeader()
  {
    this.writer.Write(Encoding.UTF8.GetBytes("SSND"));
    this.dataSizePos = this.outStream.Position;
    this.writer.Write(0);
    this.writer.Write(0);
    this.writer.Write(this.SwapEndian(this.format.BlockAlign));
  }

  private byte[] SwapEndian(short n)
  {
    return new byte[2]
    {
      (byte) ((uint) n >> 8),
      (byte) ((uint) n & (uint) byte.MaxValue)
    };
  }

  private byte[] SwapEndian(int n)
  {
    return new byte[4]
    {
      (byte) (n >> 24 & (int) byte.MaxValue),
      (byte) (n >> 16 /*0x10*/ & (int) byte.MaxValue),
      (byte) (n >> 8 & (int) byte.MaxValue),
      (byte) (n & (int) byte.MaxValue)
    };
  }

  private void CreateCommChunk()
  {
    this.writer.Write(Encoding.UTF8.GetBytes("COMM"));
    this.writer.Write(this.SwapEndian(18));
    this.writer.Write(this.SwapEndian((short) this.format.Channels));
    this.commSampleCountPos = this.outStream.Position;
    this.writer.Write(0);
    this.writer.Write(this.SwapEndian((short) this.format.BitsPerSample));
    this.writer.Write(IEEE.ConvertToIeeeExtended((double) this.format.SampleRate));
  }

  public string Filename => this.filename;

  public override long Length => this.dataChunkSize;

  public WaveFormat WaveFormat => this.format;

  public override bool CanRead => false;

  public override bool CanWrite => true;

  public override bool CanSeek => false;

  public override int Read(byte[] buffer, int offset, int count)
  {
    throw new InvalidOperationException("Cannot read from an AiffFileWriter");
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    throw new InvalidOperationException("Cannot seek within an AiffFileWriter");
  }

  public override void SetLength(long value)
  {
    throw new InvalidOperationException("Cannot set length of an AiffFileWriter");
  }

  public override long Position
  {
    get => this.dataChunkSize;
    set => throw new InvalidOperationException("Repositioning an AiffFileWriter is not supported");
  }

  public override void Write(byte[] data, int offset, int count)
  {
    byte[] buffer = new byte[data.Length];
    int num = this.format.BitsPerSample / 8;
    for (int index1 = 0; index1 < data.Length; ++index1)
    {
      int index2 = (int) Math.Floor((double) index1 / (double) num) * num + (num - index1 % num - 1);
      buffer[index1] = data[index2];
    }
    this.outStream.Write(buffer, offset, count);
    this.dataChunkSize += (long) count;
  }

  public void WriteSample(float sample)
  {
    if (this.WaveFormat.BitsPerSample == 16 /*0x10*/)
    {
      this.writer.Write(this.SwapEndian((short) ((double) short.MaxValue * (double) sample)));
      this.dataChunkSize += 2L;
    }
    else if (this.WaveFormat.BitsPerSample == 24)
    {
      byte[] bytes = BitConverter.GetBytes((int) (2147483648.0 * (double) sample));
      this.value24[2] = bytes[1];
      this.value24[1] = bytes[2];
      this.value24[0] = bytes[3];
      this.writer.Write(this.value24);
      this.dataChunkSize += 3L;
    }
    else
    {
      if (this.WaveFormat.BitsPerSample != 32 /*0x20*/ || this.WaveFormat.Encoding != WaveFormatEncoding.Extensible)
        throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
      this.writer.Write(this.SwapEndian((int) ushort.MaxValue * (int) sample));
      this.dataChunkSize += 4L;
    }
  }

  public void WriteSamples(float[] samples, int offset, int count)
  {
    for (int index = 0; index < count; ++index)
      this.WriteSample(samples[offset + index]);
  }

  public void WriteSamples(short[] samples, int offset, int count)
  {
    if (this.WaveFormat.BitsPerSample == 16 /*0x10*/)
    {
      for (int index = 0; index < count; ++index)
        this.writer.Write(this.SwapEndian(samples[index + offset]));
      this.dataChunkSize += (long) (count * 2);
    }
    else if (this.WaveFormat.BitsPerSample == 24)
    {
      for (int index = 0; index < count; ++index)
      {
        byte[] bytes = BitConverter.GetBytes((int) ushort.MaxValue * (int) samples[index + offset]);
        this.value24[2] = bytes[1];
        this.value24[1] = bytes[2];
        this.value24[0] = bytes[3];
        this.writer.Write(this.value24);
      }
      this.dataChunkSize += (long) (count * 3);
    }
    else
    {
      if (this.WaveFormat.BitsPerSample != 32 /*0x20*/ || this.WaveFormat.Encoding != WaveFormatEncoding.Extensible)
        throw new InvalidOperationException("Only 16, 24 or 32 bit PCM audio data supported");
      for (int index = 0; index < count; ++index)
        this.writer.Write(this.SwapEndian((int) ushort.MaxValue * (int) samples[index + offset]));
      this.dataChunkSize += (long) (count * 4);
    }
  }

  public override void Flush() => this.writer.Flush();

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
    this.Flush();
    writer.Seek(4, SeekOrigin.Begin);
    writer.Write(this.SwapEndian((int) (this.outStream.Length - 8L)));
    this.UpdateCommChunk(writer);
    this.UpdateSsndChunk(writer);
  }

  private void UpdateCommChunk(BinaryWriter writer)
  {
    writer.Seek((int) this.commSampleCountPos, SeekOrigin.Begin);
    writer.Write(this.SwapEndian((int) (this.dataChunkSize * 8L / (long) this.format.BitsPerSample / (long) this.format.Channels)));
  }

  private void UpdateSsndChunk(BinaryWriter writer)
  {
    writer.Seek((int) this.dataSizePos, SeekOrigin.Begin);
    writer.Write(this.SwapEndian((int) this.dataChunkSize));
  }

  ~AiffFileWriter() => this.Dispose(false);
}
