// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.RiffChunk
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class RiffChunk
{
  private string chunkID;
  private BinaryReader riffFile;

  public static RiffChunk GetTopLevelChunk(BinaryReader file)
  {
    RiffChunk topLevelChunk = new RiffChunk(file);
    topLevelChunk.ReadChunk();
    return topLevelChunk;
  }

  private RiffChunk(BinaryReader file)
  {
    this.riffFile = file;
    this.chunkID = "????";
    this.ChunkSize = 0U;
    this.DataOffset = 0L;
  }

  public string ReadChunkID()
  {
    byte[] bytes = this.riffFile.ReadBytes(4);
    if (bytes.Length != 4)
      throw new InvalidDataException("Couldn't read Chunk ID");
    return ByteEncoding.Instance.GetString(bytes, 0, bytes.Length);
  }

  private void ReadChunk()
  {
    this.chunkID = this.ReadChunkID();
    this.ChunkSize = this.riffFile.ReadUInt32();
    this.DataOffset = this.riffFile.BaseStream.Position;
  }

  public RiffChunk GetNextSubChunk()
  {
    if (this.riffFile.BaseStream.Position + 8L >= this.DataOffset + (long) this.ChunkSize)
      return (RiffChunk) null;
    RiffChunk nextSubChunk = new RiffChunk(this.riffFile);
    nextSubChunk.ReadChunk();
    return nextSubChunk;
  }

  public byte[] GetData()
  {
    this.riffFile.BaseStream.Position = this.DataOffset;
    byte[] data = this.riffFile.ReadBytes((int) this.ChunkSize);
    if ((long) data.Length != (long) this.ChunkSize)
      throw new InvalidDataException($"Couldn't read chunk's data Chunk: {this}, read {data.Length} bytes");
    return data;
  }

  public string GetDataAsString()
  {
    byte[] data = this.GetData();
    return data == null ? (string) null : ByteEncoding.Instance.GetString(data, 0, data.Length);
  }

  public T GetDataAsStructure<T>(StructureBuilder<T> s)
  {
    this.riffFile.BaseStream.Position = this.DataOffset;
    if ((long) s.Length != (long) this.ChunkSize)
      throw new InvalidDataException($"Chunk size is: {this.ChunkSize} so can't read structure of: {s.Length}");
    return s.Read(this.riffFile);
  }

  public T[] GetDataAsStructureArray<T>(StructureBuilder<T> s)
  {
    this.riffFile.BaseStream.Position = this.DataOffset;
    if ((long) this.ChunkSize % (long) s.Length != 0L)
      throw new InvalidDataException($"Chunk size is: {this.ChunkSize} not a multiple of structure size: {s.Length}");
    int length = (int) ((long) this.ChunkSize / (long) s.Length);
    T[] asStructureArray = new T[length];
    for (int index = 0; index < length; ++index)
      asStructureArray[index] = s.Read(this.riffFile);
    return asStructureArray;
  }

  public string ChunkID
  {
    get => this.chunkID;
    set
    {
      if (value == null)
        throw new ArgumentNullException("ChunkID may not be null");
      this.chunkID = value.Length == 4 ? value : throw new ArgumentException("ChunkID must be four characters");
    }
  }

  public uint ChunkSize { get; private set; }

  public long DataOffset { get; private set; }

  public override string ToString()
  {
    return $"RiffChunk ID: {this.ChunkID} Size: {this.ChunkSize} Data Offset: {this.DataOffset}";
  }
}
