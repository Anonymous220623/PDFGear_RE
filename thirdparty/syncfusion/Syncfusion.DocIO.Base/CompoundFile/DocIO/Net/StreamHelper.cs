// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.StreamHelper
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public static class StreamHelper
{
  public const int IntSize = 4;
  private const int ShortSize = 2;
  private const int DoubleSize = 8;

  public static short ReadInt16(Stream stream, byte[] buffer)
  {
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToInt16(buffer, 0) : throw new Exception("Invalid Data");
  }

  public static int ReadInt32(Stream stream, byte[] buffer)
  {
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToInt32(buffer, 0) : throw new Exception("Invalid data");
  }

  public static double ReadDouble(Stream stream, byte[] buffer)
  {
    return stream.Read(buffer, 0, 8) == 8 ? BitConverter.ToDouble(buffer, 0) : throw new Exception("Invalid data");
  }

  public static int WriteInt16(Stream stream, short value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, 2);
    return 2;
  }

  public static int WriteInt32(Stream stream, int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, 4);
    return 4;
  }

  public static int WriteDouble(Stream stream, double value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, 8);
    return 8;
  }

  public static string GetAsciiString(Stream stream, int roundedSize)
  {
    byte[] buffer = new byte[4];
    int count = StreamHelper.ReadInt32(stream, buffer);
    byte[] numArray = new byte[count];
    if (stream.Read(numArray, 0, count) != count)
      throw new IOException();
    for (int index = 0; index < count; ++index)
    {
      if (numArray[index] == (byte) 0)
      {
        count = index;
        break;
      }
    }
    return StreamHelper.RemoveLastZero(Encoding.Default.GetString(numArray, 0, count));
  }

  internal static string GetAsciiString(Stream stream, int roundedSize, int codePage)
  {
    byte[] buffer = new byte[4];
    int count = StreamHelper.ReadInt32(stream, buffer);
    byte[] numArray = new byte[count];
    if (stream.Read(numArray, 0, count) != count)
      throw new IOException();
    roundedSize = count;
    for (int index = 0; index < count; ++index)
    {
      if (numArray[index] == (byte) 0)
      {
        count = index;
        break;
      }
    }
    Encoding encoding;
    try
    {
      encoding = Encoding.GetEncoding(codePage);
    }
    catch
    {
      encoding = Encoding.Default;
    }
    return StreamHelper.RemoveLastZero(encoding.GetString(numArray, 0, count));
  }

  public static string GetUnicodeString(Stream stream, int roundedSize)
  {
    byte[] buffer = new byte[4];
    int count = StreamHelper.ReadInt32(stream, buffer) * 2;
    byte[] numArray = new byte[count];
    if (stream.Read(numArray, 0, count) != count)
      throw new IOException();
    string str = StreamHelper.RemoveLastZero(Encoding.Unicode.GetString(numArray, 0, count));
    int num = count % 4;
    if (num != 0)
      stream.Position += (long) (4 - num);
    return StreamHelper.RemoveLastZero(str);
  }

  public static int WriteAsciiString(Stream stream, string value, bool align)
  {
    Encoding encoding = Encoding.Default;
    return StreamHelper.WriteString(stream, value, encoding, align);
  }

  public static int WriteUnicodeString(Stream stream, string value)
  {
    return StreamHelper.WriteString(stream, value, Encoding.Unicode, true);
  }

  public static int WriteString(Stream stream, string value, Encoding encoding, bool align)
  {
    if (string.IsNullOrEmpty(value))
      value = "\0\0\0\0";
    else if (value[value.Length - 1] != char.MinValue)
      value += "\0";
    if (value.Length % 4 != 0)
    {
      int num = value.Length % 4;
      for (int index = 0; index < 4 - num; ++index)
        value += "\0";
    }
    byte[] bytes1 = encoding.GetBytes(value);
    int length = bytes1.Length;
    byte[] bytes2 = BitConverter.GetBytes(value.Length);
    stream.Write(bytes2, 0, bytes2.Length);
    stream.Write(bytes1, 0, length);
    int num1 = 4 + length;
    if (align)
    {
      int num2 = num1 % 4;
      if (num2 != 0)
      {
        int num3 = 0;
        for (int index = 4 - num2; num3 < index; ++num3)
          stream.WriteByte((byte) 0);
        num1 += 4 - num2;
      }
    }
    return num1;
  }

  public static void AddPadding(Stream stream, ref int iWrittenSize)
  {
    int num1 = iWrittenSize % 4;
    if (num1 == 0)
      return;
    int num2 = 0;
    int num3 = 4 - num1;
    while (num2 < num3)
    {
      stream.WriteByte((byte) 0);
      ++num2;
      ++iWrittenSize;
    }
  }

  private static string RemoveLastZero(string value)
  {
    for (int length = value != null ? value.Length : 0; length > 0 && value[length - 1] == char.MinValue; --length)
      value = value.Substring(0, length - 1);
    return value;
  }
}
