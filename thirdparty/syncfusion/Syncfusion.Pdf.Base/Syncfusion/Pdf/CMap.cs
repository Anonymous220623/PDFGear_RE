// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CMap
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class CMap
{
  private MemoryStream m_cmapStream = new MemoryStream();
  private int[] m_format6Glyph;
  private int[] m_characterAsInt;
  private int[] m_platformId;
  private int[] m_platformSpecificId;
  private int[] m_offset;
  private int m_firstCode;
  private int m_entryCount;
  private int m_format6TableLength;

  internal CMap()
  {
  }

  internal MemoryStream CreateCMapStream()
  {
    this.m_platformId = new int[2]{ 1, 3 };
    this.m_platformSpecificId = new int[2]{ 0, 1 };
    this.m_offset = new int[2]{ 20, 131102 };
    this.GenerateFormat6Table();
    this.WriteCMAPHeader();
    this.WriteFormat6Table();
    this.GenerateFormat4Table();
    return this.m_cmapStream;
  }

  internal void WriteCMAPHeader()
  {
    this.WriteShort((short) 0);
    this.WriteShort((short) 2);
    for (int index = 0; index < 2; ++index)
    {
      this.WriteShort((short) this.m_platformId[index]);
      this.WriteShort((short) this.m_platformSpecificId[index]);
      this.WriteInt(this.m_offset[index]);
    }
  }

  internal void WriteFormat6Table()
  {
    this.WriteShort((short) 6);
    this.m_format6TableLength /= 4;
    this.WriteShort((short) this.m_format6TableLength);
    this.WriteShort((short) 0);
    this.WriteShort((short) this.m_firstCode);
    this.WriteShort((short) this.m_entryCount);
    for (int index = 0; index < this.m_format6Glyph.Length; ++index)
      this.WriteShort((short) this.m_format6Glyph[index]);
  }

  internal void GenerateFormat6Table()
  {
    Dictionary<double, double> dictionary = new Dictionary<double, double>();
    for (int key = 0; key < (int) ushort.MaxValue; ++key)
      dictionary.Add((double) key, (double) key);
    dictionary[0.0] = 32.0;
    dictionary[65529.0] = 13.0;
    dictionary[65530.0] = 9.0;
    dictionary[65531.0] = 10.0;
    dictionary[65532.0] = 11.0;
    dictionary[65534.0] = 12.0;
    List<int> intList = new List<int>();
    this.m_firstCode = 0;
    this.m_entryCount = dictionary.Count;
    this.m_format6Glyph = new int[this.m_firstCode + this.m_entryCount];
    for (int key = 0; key < this.m_format6Glyph.Length; ++key)
    {
      if (dictionary.ContainsKey((double) key))
        this.m_format6Glyph[key] = (int) dictionary[(double) key];
    }
    this.m_format6TableLength = (this.m_entryCount - this.m_firstCode) * 2 + 10;
    this.m_offset[1] = this.m_offset[0] + this.m_format6TableLength;
  }

  internal void GenerateFormat4Table()
  {
    int num1 = 1;
    List<int> intList = new List<int>();
    for (int index = 0; index < this.m_format6Glyph.Length; ++index)
    {
      if (this.m_format6Glyph[index] != 0)
      {
        int num2 = index - this.m_format6Glyph[index];
        if (!intList.Contains(num2))
          intList.Add(num2);
      }
    }
    int count = intList.Count;
    int num3 = count * 2;
    while (num1 <= count)
      num1 *= 2;
    int num4 = 2 * (num1 / 2);
    int num5 = (int) Math.Log((double) (num4 / 2), 2.0);
    int num6 = 2 * count - num4;
    int[] numArray1 = new int[count];
    int[] numArray2 = new int[count];
    int[] numArray3 = new int[count];
    int[] numArray4 = new int[count];
    int index1 = -1;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    for (int key = 0; key < this.m_format6Glyph.Length; ++key)
    {
      int num7 = key - this.m_format6Glyph[key];
      if (intList.Contains(num7))
        dictionary.Add(key, num7);
    }
    int num8 = -1;
    foreach (KeyValuePair<int, int> keyValuePair in dictionary)
    {
      try
      {
        if (num8 != keyValuePair.Value)
        {
          ++index1;
          if (index1 < numArray2.Length)
          {
            numArray2[index1] = keyValuePair.Key;
            num8 = keyValuePair.Value;
          }
        }
        if (index1 < numArray1.Length)
          numArray1[index1] = keyValuePair.Key;
      }
      catch
      {
      }
    }
    for (int index2 = 0; index2 < count; ++index2)
      numArray4[index2] = 0;
    for (int index3 = 0; index3 < intList.Count; ++index3)
      numArray3[index3] = 65536 /*0x010000*/ - intList[index3];
    numArray2[count - 1] = (int) ushort.MaxValue;
    numArray1[count - 1] = (int) ushort.MaxValue;
    int num9 = 16 /*0x10*/ + 8 * count;
    this.WriteShort((short) 4);
    this.WriteShort((short) num9);
    this.WriteShort((short) 0);
    this.WriteShort((short) num3);
    this.WriteShort((short) num4);
    this.WriteShort((short) num5);
    this.WriteShort((short) num6);
    for (int index4 = 0; index4 < count; ++index4)
      this.WriteShort((short) numArray1[index4]);
    this.WriteShort((short) 0);
    for (int index5 = 0; index5 < count; ++index5)
      this.WriteShort((short) numArray2[index5]);
    for (int index6 = 0; index6 < count; ++index6)
      this.WriteShort((short) numArray3[index6]);
    for (int index7 = 0; index7 < count; ++index7)
      this.WriteShort((short) numArray4[index7]);
  }

  private void WriteShort(short value)
  {
    byte[] buffer = new byte[2]{ (byte) 0, (byte) value };
    buffer[0] = (byte) ((uint) value >> 8);
    this.m_cmapStream.Write(buffer, 0, 2);
  }

  private void WriteInt(int value)
  {
    byte[] buffer = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) value
    };
    buffer[2] = (byte) (value >> 8);
    buffer[1] = (byte) (value >> 16 /*0x10*/);
    buffer[0] = (byte) (value >> 24);
    this.m_cmapStream.Write(buffer, 0, 4);
  }

  private void WriteString(string value)
  {
    byte[] buffer = new byte[value.Length];
    int index = 0;
    foreach (char ch in value)
    {
      buffer[index] = (byte) ch;
      ++index;
    }
    this.m_cmapStream.Write(buffer, 0, 4);
  }

  public void WriteBytes(byte[] buffer) => this.m_cmapStream.Write(buffer, 0, buffer.Length);
}
