// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ASCII85
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ASCII85
{
  private long[] hex_indices = new long[4]
  {
    16777216L /*0x01000000*/,
    65536L /*0x010000*/,
    256L /*0x0100*/,
    1L
  };
  private long[] base_85_indices = new long[5]
  {
    52200625L,
    614125L,
    7225L,
    85L,
    1L
  };
  private int m_specialCases;
  private int m_returns;
  private int m_dataSize;
  private int m_outputPointer;

  public byte[] decode(byte[] encodedData)
  {
    this.m_dataSize = encodedData.Length;
    for (int index = 0; index < this.m_dataSize; ++index)
    {
      if (encodedData[index] == (byte) 122)
        ++this.m_specialCases;
      else if (encodedData[index] == (byte) 10)
        ++this.m_returns;
    }
    byte[] sourceArray = new byte[this.m_dataSize - this.m_returns + 1 + this.m_specialCases * 3];
    for (int index1 = 0; index1 < this.m_dataSize; ++index1)
    {
      long num1 = 0;
      int num2 = (int) encodedData[index1];
      while (true)
      {
        switch (num2)
        {
          case 10:
          case 13:
            ++index1;
            num2 = index1 != this.m_dataSize ? (int) encodedData[index1] : 0;
            continue;
          case 122:
            goto label_11;
          default:
            goto label_14;
        }
      }
label_11:
      for (int index2 = 0; index2 < 4; ++index2)
      {
        sourceArray[this.m_outputPointer] = (byte) 0;
        ++this.m_outputPointer;
      }
      continue;
label_14:
      if (this.m_dataSize - index1 > 4 && num2 > 32 /*0x20*/ && num2 < 118)
      {
        for (int index3 = 0; index3 < 5; ++index3)
        {
          if (index1 < encodedData.Length)
            num2 = (int) encodedData[index1];
          for (; num2 == 10 || num2 == 13; num2 = index1 != this.m_dataSize ? (int) encodedData[index1] : 0)
            ++index1;
          ++index1;
          if (num2 > 32 /*0x20*/ && num2 < 118 || num2 == 126)
            num1 += (long) (num2 - 33) * this.base_85_indices[index3];
        }
        for (int index4 = 0; index4 < 4; ++index4)
        {
          sourceArray[this.m_outputPointer] = (byte) ((ulong) (num1 / this.hex_indices[index4]) & (ulong) byte.MaxValue);
          ++this.m_outputPointer;
        }
        --index1;
      }
    }
    byte[] destinationArray = new byte[this.m_outputPointer];
    Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, this.m_outputPointer);
    return destinationArray;
  }
}
