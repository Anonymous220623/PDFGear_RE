// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Decoder.StreamExtensions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Decoder;

internal static class StreamExtensions
{
  private static byte[] m_jpegSignature = new byte[2]
  {
    byte.MaxValue,
    (byte) 216
  };
  private static byte[] m_pngSignature = new byte[8]
  {
    (byte) 137,
    (byte) 80 /*0x50*/,
    (byte) 78,
    (byte) 71,
    (byte) 13,
    (byte) 10,
    (byte) 26,
    (byte) 10
  };
  private static byte[] m_bmpSignature = new byte[2]
  {
    (byte) 66,
    (byte) 77
  };

  internal static int ReadUInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    stream.Read(buffer, 0, 4);
    return ((int) buffer[0] << 24) + ((int) buffer[1] << 16 /*0x10*/) + ((int) buffer[2] << 8) + (int) buffer[3];
  }

  internal static int ReadInt32(Stream stream)
  {
    byte[] buffer = new byte[4];
    stream.Read(buffer, 0, 4);
    return (int) buffer[0] + ((int) buffer[1] << 8) + ((int) buffer[2] << 16 /*0x10*/) + ((int) buffer[3] << 24);
  }

  internal static int ReadUInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    stream.Read(buffer, 0, 2);
    return ((int) buffer[0] << 8) + (int) buffer[1];
  }

  internal static int ReadInt16(Stream stream)
  {
    byte[] buffer = new byte[2];
    stream.Read(buffer, 0, 2);
    return (int) buffer[0] | (int) buffer[1] << 8;
  }

  internal static int ReadWord(Stream stream)
  {
    return stream.ReadByte() + (stream.ReadByte() << 8) & (int) ushort.MaxValue;
  }

  internal static int ReadShortLE(Stream stream)
  {
    int num = StreamExtensions.ReadWord(stream);
    if (num > (int) short.MaxValue)
      num -= 65536 /*0x010000*/;
    return num;
  }

  internal static string ReadString(Stream stream, int len)
  {
    char[] chArray = new char[len];
    for (int index = 0; index < len; ++index)
      chArray[index] = (char) stream.ReadByte();
    return new string(chArray);
  }

  internal static void Reset(Stream stream) => stream.Position = 0L;

  internal static void Skip(Stream stream, int noOfBytes)
  {
    long length = stream.Length - stream.Position;
    byte[] buffer = (long) noOfBytes <= length ? new byte[noOfBytes] : new byte[length];
    stream.Read(buffer, 0, buffer.Length);
  }

  internal static int ReadByte(Stream stream) => stream.ReadByte();

  internal static byte[] ReadByte(Stream stream, int len)
  {
    byte[] numArray = new byte[len];
    for (int index = 0; index < len; ++index)
      numArray[index] = (byte) stream.ReadByte();
    StreamExtensions.Skip(stream, 4);
    return numArray;
  }

  internal static bool IsJpeg(Stream stream)
  {
    stream.Position = 0L;
    for (int index = 0; index < StreamExtensions.m_jpegSignature.Length; ++index)
    {
      if ((int) StreamExtensions.m_jpegSignature[index] != stream.ReadByte())
        return false;
    }
    return true;
  }

  internal static bool IsBmp(Stream stream)
  {
    stream.Position = 0L;
    for (int index = 0; index < StreamExtensions.m_bmpSignature.Length; ++index)
    {
      if ((int) StreamExtensions.m_bmpSignature[index] != stream.ReadByte())
        return false;
    }
    return true;
  }

  internal static bool IsPng(Stream stream)
  {
    stream.Position = 0L;
    for (int index = 0; index < StreamExtensions.m_pngSignature.Length; ++index)
    {
      if ((int) StreamExtensions.m_pngSignature[index] != stream.ReadByte())
        return false;
    }
    return true;
  }
}
