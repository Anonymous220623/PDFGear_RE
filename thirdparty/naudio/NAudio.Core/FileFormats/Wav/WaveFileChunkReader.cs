// Decompiled with JetBrains decompiler
// Type: NAudio.FileFormats.Wav.WaveFileChunkReader
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NAudio.FileFormats.Wav;

public class WaveFileChunkReader
{
  private WaveFormat waveFormat;
  private long dataChunkPosition;
  private long dataChunkLength;
  private List<RiffChunk> riffChunks;
  private readonly bool strictMode;
  private bool isRf64;
  private readonly bool storeAllChunks;
  private long riffSize;

  public WaveFileChunkReader()
  {
    this.storeAllChunks = true;
    this.strictMode = false;
  }

  public void ReadWaveHeader(Stream stream)
  {
    this.dataChunkPosition = -1L;
    this.waveFormat = (WaveFormat) null;
    this.riffChunks = new List<RiffChunk>();
    this.dataChunkLength = 0L;
    BinaryReader binaryReader = new BinaryReader(stream);
    this.ReadRiffHeader(binaryReader);
    this.riffSize = (long) binaryReader.ReadUInt32();
    if (binaryReader.ReadInt32() != ChunkIdentifier.ChunkIdentifierToInt32("WAVE"))
      throw new FormatException("Not a WAVE file - no WAVE header");
    if (this.isRf64)
      this.ReadDs64Chunk(binaryReader);
    int int32_1 = ChunkIdentifier.ChunkIdentifierToInt32("data");
    int int32_2 = ChunkIdentifier.ChunkIdentifierToInt32("fmt ");
    long num1 = Math.Min(this.riffSize + 8L, stream.Length);
    while (stream.Position <= num1 - 8L)
    {
      int chunkIdentifier = binaryReader.ReadInt32();
      uint num2 = binaryReader.ReadUInt32();
      if (chunkIdentifier == int32_1)
      {
        this.dataChunkPosition = stream.Position;
        if (!this.isRf64)
          this.dataChunkLength = (long) num2;
        stream.Position += (long) num2;
      }
      else if (chunkIdentifier == int32_2)
      {
        this.waveFormat = num2 <= (uint) int.MaxValue ? WaveFormat.FromFormatChunk(binaryReader, (int) num2) : throw new InvalidDataException($"Format chunk length must be between 0 and {int.MaxValue}.");
      }
      else
      {
        if ((long) num2 > stream.Length - stream.Position)
        {
          if (!this.strictMode)
            break;
          break;
        }
        if (this.storeAllChunks)
        {
          if (num2 > (uint) int.MaxValue)
            throw new InvalidDataException($"RiffChunk chunk length must be between 0 and {int.MaxValue}.");
          this.riffChunks.Add(WaveFileChunkReader.GetRiffChunk(stream, chunkIdentifier, (int) num2));
        }
        stream.Position += (long) num2;
      }
      if (num2 % 2U != 0U && binaryReader.PeekChar() == 0)
        ++stream.Position;
    }
    if (this.waveFormat == null)
      throw new FormatException("Invalid WAV file - No fmt chunk found");
    if (this.dataChunkPosition == -1L)
      throw new FormatException("Invalid WAV file - No data chunk found");
  }

  private void ReadDs64Chunk(BinaryReader reader)
  {
    int int32 = ChunkIdentifier.ChunkIdentifierToInt32("ds64");
    int num = reader.ReadInt32() == int32 ? reader.ReadInt32() : throw new FormatException("Invalid RF64 WAV file - No ds64 chunk found");
    this.riffSize = reader.ReadInt64();
    this.dataChunkLength = reader.ReadInt64();
    reader.ReadInt64();
    reader.ReadBytes(num - 24);
  }

  private static RiffChunk GetRiffChunk(Stream stream, int chunkIdentifier, int chunkLength)
  {
    return new RiffChunk(chunkIdentifier, chunkLength, stream.Position);
  }

  private void ReadRiffHeader(BinaryReader br)
  {
    int num = br.ReadInt32();
    if (num == ChunkIdentifier.ChunkIdentifierToInt32("RF64"))
      this.isRf64 = true;
    else if (num != ChunkIdentifier.ChunkIdentifierToInt32("RIFF"))
      throw new FormatException("Not a WAVE file - no RIFF header");
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public long DataChunkPosition => this.dataChunkPosition;

  public long DataChunkLength => this.dataChunkLength;

  public List<RiffChunk> RiffChunks => this.riffChunks;
}
