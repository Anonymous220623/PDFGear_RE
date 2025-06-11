// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Id3v2Tag
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.Wave;

public class Id3v2Tag
{
  private long tagStartPosition;
  private long tagEndPosition;
  private byte[] rawData;

  public static Id3v2Tag ReadTag(Stream input)
  {
    try
    {
      return new Id3v2Tag(input);
    }
    catch (FormatException ex)
    {
      return (Id3v2Tag) null;
    }
  }

  public static Id3v2Tag Create(IEnumerable<KeyValuePair<string, string>> tags)
  {
    return Id3v2Tag.ReadTag(Id3v2Tag.CreateId3v2TagStream(tags));
  }

  private static byte[] FrameSizeToBytes(int n)
  {
    byte[] bytes = BitConverter.GetBytes(n);
    Array.Reverse((Array) bytes);
    return bytes;
  }

  private static byte[] CreateId3v2Frame(string key, string value)
  {
    if (string.IsNullOrEmpty(key))
      throw new ArgumentNullException(nameof (key));
    if (string.IsNullOrEmpty(value))
      throw new ArgumentNullException(nameof (value));
    if (key.Length != 4)
      throw new ArgumentOutOfRangeException(nameof (key), $"key {key} must be 4 characters long");
    byte[] numArray1 = new byte[2]
    {
      byte.MaxValue,
      (byte) 254
    };
    byte[] numArray2 = new byte[3];
    byte[] numArray3 = new byte[2];
    byte[] numArray4;
    if (key == "COMM")
      numArray4 = ByteArrayExtensions.Concat(new byte[1]
      {
        (byte) 1
      }, numArray2, numArray3, numArray1, Encoding.Unicode.GetBytes(value));
    else
      numArray4 = ByteArrayExtensions.Concat(new byte[1]
      {
        (byte) 1
      }, numArray1, Encoding.Unicode.GetBytes(value));
    return ByteArrayExtensions.Concat(Encoding.UTF8.GetBytes(key), Id3v2Tag.FrameSizeToBytes(numArray4.Length), new byte[2], numArray4);
  }

  private static byte[] GetId3TagHeaderSize(int size)
  {
    byte[] id3TagHeaderSize = new byte[4];
    for (int index = id3TagHeaderSize.Length - 1; index >= 0; --index)
    {
      id3TagHeaderSize[index] = (byte) (size % 128 /*0x80*/);
      size /= 128 /*0x80*/;
    }
    return id3TagHeaderSize;
  }

  private static byte[] CreateId3v2TagHeader(IEnumerable<byte[]> frames)
  {
    int size = 0;
    foreach (byte[] frame in frames)
      size += frame.Length;
    return ByteArrayExtensions.Concat(Encoding.UTF8.GetBytes("ID3"), new byte[2]
    {
      (byte) 3,
      (byte) 0
    }, new byte[1], Id3v2Tag.GetId3TagHeaderSize(size));
  }

  private static Stream CreateId3v2TagStream(IEnumerable<KeyValuePair<string, string>> tags)
  {
    List<byte[]> frames = new List<byte[]>();
    foreach (KeyValuePair<string, string> tag in tags)
      frames.Add(Id3v2Tag.CreateId3v2Frame(tag.Key, tag.Value));
    byte[] id3v2TagHeader = Id3v2Tag.CreateId3v2TagHeader((IEnumerable<byte[]>) frames);
    MemoryStream id3v2TagStream = new MemoryStream();
    id3v2TagStream.Write(id3v2TagHeader, 0, id3v2TagHeader.Length);
    foreach (byte[] buffer in frames)
      id3v2TagStream.Write(buffer, 0, buffer.Length);
    id3v2TagStream.Position = 0L;
    return (Stream) id3v2TagStream;
  }

  private Id3v2Tag(Stream input)
  {
    this.tagStartPosition = input.Position;
    BinaryReader binaryReader = new BinaryReader(input);
    byte[] numArray1 = binaryReader.ReadBytes(10);
    if (numArray1.Length >= 3 && numArray1[0] == (byte) 73 && numArray1[1] == (byte) 68 && numArray1[2] == (byte) 51)
    {
      if (((int) numArray1[5] & 64 /*0x40*/) == 64 /*0x40*/)
      {
        byte[] numArray2 = binaryReader.ReadBytes(4);
        int num1 = (int) numArray2[0] * 2097152 /*0x200000*/ + (int) numArray2[1] * 16384 /*0x4000*/ + (int) numArray2[2] * 128 /*0x80*/;
        int num2 = (int) numArray2[3];
      }
      int count = (int) numArray1[6] * 2097152 /*0x200000*/ + (int) numArray1[7] * 16384 /*0x4000*/ + (int) numArray1[8] * 128 /*0x80*/ + (int) numArray1[9];
      binaryReader.ReadBytes(count);
      if (((int) numArray1[5] & 16 /*0x10*/) == 16 /*0x10*/)
        binaryReader.ReadBytes(10);
      this.tagEndPosition = input.Position;
      input.Position = this.tagStartPosition;
      this.rawData = binaryReader.ReadBytes((int) (this.tagEndPosition - this.tagStartPosition));
    }
    else
    {
      input.Position = this.tagStartPosition;
      throw new FormatException("Not an ID3v2 tag");
    }
  }

  public byte[] RawData => this.rawData;
}
