// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.BytesAssistant
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class BytesAssistant
{
  public const int CID_BYTES_COUNT = 4;

  public static char GetUnicodeChar(int i) => (char) i;

  public static int GetInt(byte[] bytes, int offset, int count)
  {
    byte[] bytes1 = new byte[count];
    for (int index = 0; index < count; ++index)
      bytes1[index] = bytes[offset + index];
    return BytesAssistant.GetInt(bytes1);
  }

  public static int GetInt(byte[] bytes)
  {
    byte[] bytes1 = BytesAssistant.GetBytes(bytes);
    if (bytes1 == null)
      return -1;
    int num = 0;
    int length = bytes1.Length;
    for (int index = 0; index < length; ++index)
    {
      num |= length <= index ? 0 : (int) bytes1[index] & (int) byte.MaxValue;
      if (index < length - 1)
        num <<= 8;
    }
    return num;
  }

  private static byte[] GetBytes(byte[] bytes)
  {
    if (bytes == null || bytes.Length > 4)
      return (byte[]) null;
    if (bytes.Length == 0)
      return bytes;
    int length1 = bytes.Length;
    int length2 = (length1 / 2 + length1 % 2) * 2;
    byte[] bytes1 = new byte[length2];
    if (length1 == 1)
    {
      bytes1[0] = (byte) 0;
      bytes1[1] = bytes[0];
    }
    else if (length2 == 2)
    {
      bytes1[0] = bytes[0];
      bytes1[1] = bytes[1];
    }
    else if (length1 == 3)
    {
      bytes1[0] = (byte) 0;
      bytes1[1] = bytes[0];
      bytes1[2] = bytes[1];
      bytes1[3] = bytes[2];
    }
    else
    {
      bytes1[0] = bytes[0];
      bytes1[1] = bytes[1];
      bytes1[2] = bytes[2];
      bytes1[3] = bytes[3];
    }
    return bytes1;
  }

  public static int ToInt16(byte[] bytes, int offset, int count)
  {
    if (bytes.Length <= offset)
      return -1;
    List<byte> byteList = new List<byte>();
    for (int index = 0; index < count; ++index)
    {
      if (offset + index < bytes.Length)
        byteList.Add(bytes[offset + index]);
      else
        byteList.Insert(0, (byte) 0);
    }
    return BytesAssistant.GetInt(byteList.ToArray());
  }

  public static int ToInt32(byte[] bytes, int offset, int count)
  {
    if (bytes.Length - 3 <= offset)
      return -1;
    List<byte> byteList = new List<byte>();
    for (int index = 0; index < count; ++index)
    {
      if (offset + index < bytes.Length)
        byteList.Add(bytes[offset + index]);
      else
        byteList.Insert(0, (byte) 0);
    }
    return BytesAssistant.GetInt(byteList.ToArray());
  }

  public static byte[] RemoveWhiteSpace(byte[] data)
  {
    int length1 = data.Length;
    int length2 = 0;
    int index1 = 0;
    while (index1 < length1)
    {
      byte num = data[index1];
      if (num == (byte) 0)
        --length2;
      switch ((int) num - 9)
      {
        case 0:
        case 1:
        case 3:
        case 4:
          --length2;
          goto case 2;
        case 2:
          if (index1 != length2)
            data[length2] = data[index1];
          if (length2 < length1)
          {
            byte[] numArray = data;
            data = new byte[length2];
            for (int index2 = 0; index2 < length2; ++index2)
              data[index2] = numArray[index2];
          }
          ++index1;
          ++length2;
          continue;
        default:
          if (num == (byte) 32 /*0x20*/)
          {
            --length2;
            goto case 2;
          }
          goto case 2;
      }
    }
    return data;
  }

  public static void GetBytes(int b, byte[] output)
  {
    byte[] numArray = new byte[4]
    {
      (byte) (b & (int) byte.MaxValue),
      (byte) (b >> 8 & (int) byte.MaxValue),
      (byte) (b >> 16 /*0x10*/ & (int) byte.MaxValue),
      (byte) (b >> 24)
    };
    for (int index = 0; index < output.Length; ++index)
      output[index] = numArray[index];
  }
}
