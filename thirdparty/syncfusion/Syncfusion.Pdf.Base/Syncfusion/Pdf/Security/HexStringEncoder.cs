// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.HexStringEncoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class HexStringEncoder : IHexEncoder
{
  private static readonly byte[] table = new byte[16 /*0x10*/]
  {
    (byte) 48 /*0x30*/,
    (byte) 49,
    (byte) 50,
    (byte) 51,
    (byte) 52,
    (byte) 53,
    (byte) 54,
    (byte) 55,
    (byte) 56,
    (byte) 57,
    (byte) 97,
    (byte) 98,
    (byte) 99,
    (byte) 100,
    (byte) 101,
    (byte) 102
  };
  private static readonly byte[] tableDecode = HexStringEncoder.CreateDecodeTable(HexStringEncoder.table);

  private static byte[] CreateDecodeTable(byte[] value)
  {
    byte[] decodeTable = new byte[128 /*0x80*/];
    for (int index = 0; index < value.Length; ++index)
      decodeTable[(int) value[index]] = (byte) index;
    decodeTable[65] = decodeTable[97];
    decodeTable[66] = decodeTable[98];
    decodeTable[67] = decodeTable[99];
    decodeTable[68] = decodeTable[100];
    decodeTable[69] = decodeTable[101];
    decodeTable[70] = decodeTable[102];
    return decodeTable;
  }

  public int Encode(byte[] data, int offset, int length, Stream outputStream)
  {
    for (int index = offset; index < offset + length; ++index)
    {
      int num = (int) data[index];
      outputStream.WriteByte(HexStringEncoder.table[num >> 4]);
      outputStream.WriteByte(HexStringEncoder.table[num & 15]);
    }
    return length * 2;
  }

  private static bool Ignore(char character)
  {
    return character == '\n' || character == '\r' || character == '\t' || character == ' ';
  }

  public int Decode(byte[] data, int offset, int length, Stream outputStream)
  {
    int num1 = 0;
    int num2 = offset + length;
    while (num2 > offset && HexStringEncoder.Ignore((char) data[num2 - 1]))
      --num2;
    int index1 = offset;
    while (index1 < num2)
    {
      while (index1 < num2 && HexStringEncoder.Ignore((char) data[index1]))
        ++index1;
      byte[] tableDecode1 = HexStringEncoder.tableDecode;
      byte[] numArray1 = data;
      int index2 = index1;
      int index3 = index2 + 1;
      int index4 = (int) numArray1[index2];
      byte num3 = tableDecode1[index4];
      while (index3 < num2 && HexStringEncoder.Ignore((char) data[index3]))
        ++index3;
      byte[] tableDecode2 = HexStringEncoder.tableDecode;
      byte[] numArray2 = data;
      int index5 = index3;
      index1 = index5 + 1;
      int index6 = (int) numArray2[index5];
      byte num4 = tableDecode2[index6];
      outputStream.WriteByte((byte) ((uint) num3 << 4 | (uint) num4));
      ++num1;
    }
    return num1;
  }

  public int DecodeString(string data, Stream outputStream)
  {
    int num1 = 0;
    int length = data.Length;
    while (length > 0 && HexStringEncoder.Ignore(data[length - 1]))
      --length;
    int index1 = 0;
    while (index1 < length)
    {
      while (index1 < length && HexStringEncoder.Ignore(data[index1]))
        ++index1;
      byte[] tableDecode1 = HexStringEncoder.tableDecode;
      string str1 = data;
      int index2 = index1;
      int index3 = index2 + 1;
      int index4 = (int) str1[index2];
      byte num2 = tableDecode1[index4];
      while (index3 < length && HexStringEncoder.Ignore(data[index3]))
        ++index3;
      byte[] tableDecode2 = HexStringEncoder.tableDecode;
      string str2 = data;
      int index5 = index3;
      index1 = index5 + 1;
      int index6 = (int) str2[index5];
      byte num3 = tableDecode2[index6];
      outputStream.WriteByte((byte) ((uint) num2 << 4 | (uint) num3));
      ++num1;
    }
    return num1;
  }
}
