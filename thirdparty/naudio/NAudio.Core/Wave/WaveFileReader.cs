// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFileReader
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.FileFormats.Wav;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NAudio.Wave;

public class WaveFileReader : WaveStream
{
  private readonly WaveFormat waveFormat;
  private readonly bool ownInput;
  private readonly long dataPosition;
  private readonly long dataChunkLength;
  private readonly object lockObject = new object();
  private Stream waveStream;

  public WaveFileReader(string waveFile)
    : this((Stream) File.OpenRead(waveFile), true)
  {
  }

  public WaveFileReader(Stream inputStream)
    : this(inputStream, false)
  {
  }

  private WaveFileReader(Stream inputStream, bool ownInput)
  {
    this.waveStream = inputStream;
    WaveFileChunkReader waveFileChunkReader = new WaveFileChunkReader();
    try
    {
      waveFileChunkReader.ReadWaveHeader(inputStream);
      this.waveFormat = waveFileChunkReader.WaveFormat;
      this.dataPosition = waveFileChunkReader.DataChunkPosition;
      this.dataChunkLength = waveFileChunkReader.DataChunkLength;
      this.ExtraChunks = waveFileChunkReader.RiffChunks;
    }
    catch
    {
      if (ownInput)
        inputStream.Dispose();
      throw;
    }
    this.Position = 0L;
    this.ownInput = ownInput;
  }

  public List<RiffChunk> ExtraChunks { get; }

  public byte[] GetChunkData(RiffChunk chunk)
  {
    long position = this.waveStream.Position;
    this.waveStream.Position = chunk.StreamPosition;
    byte[] buffer = new byte[chunk.Length];
    this.waveStream.Read(buffer, 0, buffer.Length);
    this.waveStream.Position = position;
    return buffer;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.waveStream != null)
    {
      if (this.ownInput)
        this.waveStream.Dispose();
      this.waveStream = (Stream) null;
    }
    base.Dispose(disposing);
  }

  public override WaveFormat WaveFormat => this.waveFormat;

  public override long Length => this.dataChunkLength;

  public long SampleCount
  {
    get
    {
      if (this.waveFormat.Encoding == WaveFormatEncoding.Pcm || this.waveFormat.Encoding == WaveFormatEncoding.Extensible || this.waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
        return this.dataChunkLength / (long) this.BlockAlign;
      throw new InvalidOperationException("Sample count is calculated only for the standard encodings");
    }
  }

  public override long Position
  {
    get => this.waveStream.Position - this.dataPosition;
    set
    {
      lock (this.lockObject)
      {
        value = Math.Min(value, this.Length);
        value -= value % (long) this.waveFormat.BlockAlign;
        this.waveStream.Position = value + this.dataPosition;
      }
    }
  }

  public override int Read(byte[] array, int offset, int count)
  {
    if (count % this.waveFormat.BlockAlign != 0)
      throw new ArgumentException($"Must read complete blocks: requested {count}, block align is {this.WaveFormat.BlockAlign}");
    lock (this.lockObject)
    {
      if (this.Position + (long) count > this.dataChunkLength)
        count = (int) (this.dataChunkLength - this.Position);
      return this.waveStream.Read(array, offset, count);
    }
  }

  public float[] ReadNextSampleFrame()
  {
    switch (this.waveFormat.Encoding)
    {
      case WaveFormatEncoding.Pcm:
      case WaveFormatEncoding.IeeeFloat:
      case WaveFormatEncoding.Extensible:
        float[] numArray = new float[this.waveFormat.Channels];
        int count = this.waveFormat.Channels * (this.waveFormat.BitsPerSample / 8);
        byte[] buffer = new byte[count];
        int num = this.Read(buffer, 0, count);
        if (num == 0)
          return (float[]) null;
        if (num < count)
          throw new InvalidDataException("Unexpected end of file");
        int startIndex = 0;
        for (int index = 0; index < this.waveFormat.Channels; ++index)
        {
          if (this.waveFormat.BitsPerSample == 16 /*0x10*/)
          {
            numArray[index] = (float) BitConverter.ToInt16(buffer, startIndex) / 32768f;
            startIndex += 2;
          }
          else if (this.waveFormat.BitsPerSample == 24)
          {
            numArray[index] = (float) ((int) (sbyte) buffer[startIndex + 2] << 16 /*0x10*/ | (int) buffer[startIndex + 1] << 8 | (int) buffer[startIndex]) / 8388608f;
            startIndex += 3;
          }
          else if (this.waveFormat.BitsPerSample == 32 /*0x20*/ && this.waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
          {
            numArray[index] = BitConverter.ToSingle(buffer, startIndex);
            startIndex += 4;
          }
          else
          {
            if (this.waveFormat.BitsPerSample != 32 /*0x20*/)
              throw new InvalidOperationException("Unsupported bit depth");
            numArray[index] = (float) BitConverter.ToInt32(buffer, startIndex) / (float) int.MaxValue;
            startIndex += 4;
          }
        }
        return numArray;
      default:
        throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
    }
  }

  [Obsolete("Use ReadNextSampleFrame instead (this version does not support stereo properly)")]
  public bool TryReadFloat(out float sampleValue)
  {
    float[] numArray = this.ReadNextSampleFrame();
    sampleValue = numArray != null ? numArray[0] : 0.0f;
    return numArray != null;
  }
}
