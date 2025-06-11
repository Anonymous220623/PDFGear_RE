// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.AiffFileReader
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NAudio.Wave;

public class AiffFileReader : WaveStream
{
  private readonly WaveFormat waveFormat;
  private readonly bool ownInput;
  private readonly long dataPosition;
  private readonly int dataChunkLength;
  private readonly List<AiffFileReader.AiffChunk> chunks = new List<AiffFileReader.AiffChunk>();
  private Stream waveStream;
  private readonly object lockObject = new object();

  public AiffFileReader(string aiffFile)
    : this((Stream) File.OpenRead(aiffFile))
  {
    this.ownInput = true;
  }

  public AiffFileReader(Stream inputStream)
  {
    this.waveStream = inputStream;
    AiffFileReader.ReadAiffHeader(this.waveStream, out this.waveFormat, out this.dataPosition, out this.dataChunkLength, this.chunks);
    this.Position = 0L;
  }

  public static void ReadAiffHeader(
    Stream stream,
    out WaveFormat format,
    out long dataChunkPosition,
    out int dataChunkLength,
    List<AiffFileReader.AiffChunk> chunks)
  {
    dataChunkPosition = -1L;
    format = (WaveFormat) null;
    BinaryReader br = new BinaryReader(stream);
    int num1 = !(AiffFileReader.ReadChunkName(br) != "FORM") ? (int) AiffFileReader.ConvertInt(br.ReadBytes(4)) : throw new FormatException("Not an AIFF file - no FORM header.");
    string str = AiffFileReader.ReadChunkName(br);
    if (str != "AIFC" && str != "AIFF")
      throw new FormatException("Not an AIFF file - no AIFF/AIFC header.");
    dataChunkLength = 0;
    while (br.BaseStream.Position < br.BaseStream.Length)
    {
      AiffFileReader.AiffChunk aiffChunk = AiffFileReader.ReadChunkHeader(br);
      if (!(aiffChunk.ChunkName == "\0\0\0\0") && br.BaseStream.Position + (long) aiffChunk.ChunkLength <= br.BaseStream.Length)
      {
        if (aiffChunk.ChunkName == "COMM")
        {
          short channels = AiffFileReader.ConvertShort(br.ReadBytes(2));
          int num2 = (int) AiffFileReader.ConvertInt(br.ReadBytes(4));
          short bits = AiffFileReader.ConvertShort(br.ReadBytes(2));
          double rate = IEEE.ConvertFromIeeeExtended(br.ReadBytes(10));
          format = new WaveFormat((int) rate, (int) bits, (int) channels);
          if (aiffChunk.ChunkLength > 18U && str == "AIFC")
          {
            if (new string(br.ReadChars(4)).ToLower() != "none")
              throw new FormatException("Compressed AIFC is not supported.");
            br.ReadBytes((int) aiffChunk.ChunkLength - 22);
          }
          else
            br.ReadBytes((int) aiffChunk.ChunkLength - 18);
        }
        else if (aiffChunk.ChunkName == "SSND")
        {
          uint num3 = AiffFileReader.ConvertInt(br.ReadBytes(4));
          int num4 = (int) AiffFileReader.ConvertInt(br.ReadBytes(4));
          dataChunkPosition = (long) (aiffChunk.ChunkStart + 16U /*0x10*/ + num3);
          dataChunkLength = (int) aiffChunk.ChunkLength - 8;
          br.BaseStream.Position += (long) (aiffChunk.ChunkLength - 8U);
        }
        else
        {
          chunks?.Add(aiffChunk);
          br.BaseStream.Position += (long) aiffChunk.ChunkLength;
        }
      }
      else
        break;
    }
    if (format == null)
      throw new FormatException("Invalid AIFF file - No COMM chunk found.");
    if (dataChunkPosition == -1L)
      throw new FormatException("Invalid AIFF file - No SSND chunk found.");
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

  public override long Length => (long) this.dataChunkLength;

  public long SampleCount
  {
    get
    {
      if (this.waveFormat.Encoding == WaveFormatEncoding.Pcm || this.waveFormat.Encoding == WaveFormatEncoding.Extensible || this.waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
        return (long) (this.dataChunkLength / this.BlockAlign);
      throw new FormatException("Sample count is calculated only for the standard encodings");
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
      if (this.Position + (long) count > (long) this.dataChunkLength)
        count = this.dataChunkLength - (int) this.Position;
      byte[] buffer = new byte[count];
      int num1 = this.waveStream.Read(buffer, offset, count);
      int num2 = this.WaveFormat.BitsPerSample / 8;
      for (int index = 0; index < num1; index += num2)
      {
        if (this.WaveFormat.BitsPerSample == 8)
          array[index] = buffer[index];
        else if (this.WaveFormat.BitsPerSample == 16 /*0x10*/)
        {
          array[index] = buffer[index + 1];
          array[index + 1] = buffer[index];
        }
        else if (this.WaveFormat.BitsPerSample == 24)
        {
          array[index] = buffer[index + 2];
          array[index + 1] = buffer[index + 1];
          array[index + 2] = buffer[index];
        }
        else
        {
          if (this.WaveFormat.BitsPerSample != 32 /*0x20*/)
            throw new FormatException("Unsupported PCM format.");
          array[index] = buffer[index + 3];
          array[index + 1] = buffer[index + 2];
          array[index + 2] = buffer[index + 1];
          array[index + 3] = buffer[index];
        }
      }
      return num1;
    }
  }

  private static uint ConvertInt(byte[] buffer)
  {
    if (buffer.Length != 4)
      throw new Exception("Incorrect length for long.");
    return (uint) ((int) buffer[0] << 24 | (int) buffer[1] << 16 /*0x10*/ | (int) buffer[2] << 8) | (uint) buffer[3];
  }

  private static short ConvertShort(byte[] buffer)
  {
    if (buffer.Length != 2)
      throw new Exception("Incorrect length for int.");
    return (short) ((int) buffer[0] << 8 | (int) buffer[1]);
  }

  private static AiffFileReader.AiffChunk ReadChunkHeader(BinaryReader br)
  {
    return new AiffFileReader.AiffChunk((uint) br.BaseStream.Position, AiffFileReader.ReadChunkName(br), AiffFileReader.ConvertInt(br.ReadBytes(4)));
  }

  private static string ReadChunkName(BinaryReader br) => new string(br.ReadChars(4));

  public struct AiffChunk(uint start, string name, uint length)
  {
    public string ChunkName = name;
    public uint ChunkLength = length + (uint) (length % 2U == 1U);
    public uint ChunkStart = start;
  }
}
