// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HuffmanDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf;

internal class HuffmanDecoder
{
  private Jbig2StreamReader reader;
  internal int jbig2HuffmanLOW = int.Parse("fffffffd", NumberStyles.HexNumber);
  internal int jbig2HuffmanOOB = int.Parse("fffffffe", NumberStyles.HexNumber);
  internal int jbig2HuffmanEOT = int.Parse("ffffffff", NumberStyles.HexNumber);
  internal int[][] huffmanTableA;
  internal int[][] huffmanTableB;
  internal int[][] huffmanTableC;
  internal int[][] huffmanTableD;
  internal int[][] huffmanTableE;
  internal int[][] huffmanTableF;
  internal int[][] huffmanTableG;
  internal int[][] huffmanTableH;
  internal int[][] huffmanTableI;
  internal int[][] huffmanTableJ;
  internal int[][] huffmanTableK;
  internal int[][] huffmanTableL;
  internal int[][] huffmanTableM;
  internal int[][] huffmanTableN;
  internal int[][] huffmanTableO;

  internal HuffmanDecoder(Jbig2StreamReader reader)
  {
    this.reader = reader;
    this.Initialize();
  }

  internal HuffmanDecoder() => this.Initialize();

  internal void Initialize()
  {
    this.huffmanTableA = new int[5][]
    {
      new int[4]{ 0, 1, 4, 0 },
      new int[4]{ 16 /*0x10*/, 2, 8, 2 },
      new int[4]{ 272, 3, 16 /*0x10*/, 6 },
      new int[4]{ 65808, 3, 32 /*0x20*/, 7 },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableB = new int[8][]
    {
      new int[4]{ 0, 1, 0, 0 },
      new int[4]{ 1, 2, 0, 2 },
      new int[4]{ 2, 3, 0, 6 },
      new int[4]{ 3, 4, 3, 14 },
      new int[4]{ 11, 5, 6, 30 },
      new int[4]{ 75, 6, 32 /*0x20*/, 62 },
      new int[4]{ 0, 6, this.jbig2HuffmanOOB, 63 /*0x3F*/ },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableC = new int[10][]
    {
      new int[4]{ 0, 1, 0, 0 },
      new int[4]{ 1, 2, 0, 2 },
      new int[4]{ 2, 3, 0, 6 },
      new int[4]{ 3, 4, 3, 14 },
      new int[4]{ 11, 5, 6, 30 },
      new int[4]{ 0, 6, this.jbig2HuffmanOOB, 62 },
      new int[4]{ 75, 7, 32 /*0x20*/, 254 },
      new int[4]{ -256, 8, 8, 254 },
      new int[4]
      {
        -257,
        8,
        this.jbig2HuffmanLOW,
        (int) byte.MaxValue
      },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableD = new int[7][]
    {
      new int[4]{ 1, 1, 0, 0 },
      new int[4]{ 2, 2, 0, 2 },
      new int[4]{ 3, 3, 0, 6 },
      new int[4]{ 4, 4, 3, 14 },
      new int[4]{ 12, 5, 6, 30 },
      new int[4]{ 76, 5, 32 /*0x20*/, 31 /*0x1F*/ },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableE = new int[9][]
    {
      new int[4]{ 1, 1, 0, 0 },
      new int[4]{ 2, 2, 0, 2 },
      new int[4]{ 3, 3, 0, 6 },
      new int[4]{ 4, 4, 3, 14 },
      new int[4]{ 12, 5, 6, 30 },
      new int[4]{ 76, 6, 32 /*0x20*/, 62 },
      new int[4]{ -255, 7, 8, 126 },
      new int[4]
      {
        -256,
        7,
        this.jbig2HuffmanLOW,
        (int) sbyte.MaxValue
      },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableF = new int[15][]
    {
      new int[4]{ 0, 2, 7, 0 },
      new int[4]{ 128 /*0x80*/, 3, 7, 2 },
      new int[4]{ 256 /*0x0100*/, 3, 8, 3 },
      new int[4]{ -1024, 4, 9, 8 },
      new int[4]{ -512, 4, 8, 9 },
      new int[4]{ -256, 4, 7, 10 },
      new int[4]{ -32, 4, 5, 11 },
      new int[4]{ 512 /*0x0200*/, 4, 9, 12 },
      new int[4]{ 1024 /*0x0400*/, 4, 10, 13 },
      new int[4]{ -2048, 5, 10, 28 },
      new int[4]{ (int) sbyte.MinValue, 5, 6, 29 },
      new int[4]{ -64, 5, 5, 30 },
      new int[4]{ -2049, 6, this.jbig2HuffmanLOW, 62 },
      new int[4]{ 2048 /*0x0800*/, 6, 32 /*0x20*/, 63 /*0x3F*/ },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableG = new int[16 /*0x10*/][]
    {
      new int[4]{ -512, 3, 8, 0 },
      new int[4]{ 256 /*0x0100*/, 3, 8, 1 },
      new int[4]{ 512 /*0x0200*/, 3, 9, 2 },
      new int[4]{ 1024 /*0x0400*/, 3, 10, 3 },
      new int[4]{ -1024, 4, 9, 8 },
      new int[4]{ -256, 4, 7, 9 },
      new int[4]{ -32, 4, 5, 10 },
      new int[4]{ 0, 4, 5, 11 },
      new int[4]{ 128 /*0x80*/, 4, 7, 12 },
      new int[4]{ (int) sbyte.MinValue, 5, 6, 26 },
      new int[4]{ -64, 5, 5, 27 },
      new int[4]{ 32 /*0x20*/, 5, 5, 28 },
      new int[4]{ 64 /*0x40*/, 5, 6, 29 },
      new int[4]{ -1025, 5, this.jbig2HuffmanLOW, 30 },
      new int[4]{ 2048 /*0x0800*/, 5, 32 /*0x20*/, 31 /*0x1F*/ },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableH = new int[22][]
    {
      new int[4]{ 0, 2, 1, 0 },
      new int[4]{ 0, 2, this.jbig2HuffmanOOB, 1 },
      new int[4]{ 4, 3, 4, 4 },
      new int[4]{ -1, 4, 0, 10 },
      new int[4]{ 22, 4, 4, 11 },
      new int[4]{ 38, 4, 5, 12 },
      new int[4]{ 2, 5, 0, 26 },
      new int[4]{ 70, 5, 6, 27 },
      new int[4]{ 134, 5, 7, 28 },
      new int[4]{ 3, 6, 0, 58 },
      new int[4]{ 20, 6, 1, 59 },
      new int[4]{ 262, 6, 7, 60 },
      new int[4]{ 646, 6, 10, 61 },
      new int[4]{ -2, 7, 0, 124 },
      new int[4]{ 390, 7, 8, 125 },
      new int[4]{ -15, 8, 3, 252 },
      new int[4]{ -5, 8, 1, 253 },
      new int[4]{ -7, 9, 1, 508 },
      new int[4]{ -3, 9, 0, 509 },
      new int[4]{ -16, 9, this.jbig2HuffmanLOW, 510 },
      new int[4]{ 1670, 9, 32 /*0x20*/, 511 /*0x01FF*/ },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableI = new int[23][]
    {
      new int[4]{ 0, 2, this.jbig2HuffmanOOB, 0 },
      new int[4]{ -1, 3, 1, 2 },
      new int[4]{ 1, 3, 1, 3 },
      new int[4]{ 7, 3, 5, 4 },
      new int[4]{ -3, 4, 1, 10 },
      new int[4]{ 43, 4, 5, 11 },
      new int[4]{ 75, 4, 6, 12 },
      new int[4]{ 3, 5, 1, 26 },
      new int[4]{ 139, 5, 7, 27 },
      new int[4]{ 267, 5, 8, 28 },
      new int[4]{ 5, 6, 1, 58 },
      new int[4]{ 39, 6, 2, 59 },
      new int[4]{ 523, 6, 8, 60 },
      new int[4]{ 1291, 6, 11, 61 },
      new int[4]{ -5, 7, 1, 124 },
      new int[4]{ 779, 7, 9, 125 },
      new int[4]{ -31, 8, 4, 252 },
      new int[4]{ -11, 8, 2, 253 },
      new int[4]{ -15, 9, 2, 508 },
      new int[4]{ -7, 9, 1, 509 },
      new int[4]{ -32, 9, this.jbig2HuffmanLOW, 510 },
      new int[4]{ 3339, 9, 32 /*0x20*/, 511 /*0x01FF*/ },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableJ = new int[22][]
    {
      new int[4]{ -2, 2, 2, 0 },
      new int[4]{ 6, 2, 6, 1 },
      new int[4]{ 0, 2, this.jbig2HuffmanOOB, 2 },
      new int[4]{ -3, 5, 0, 24 },
      new int[4]{ 2, 5, 0, 25 },
      new int[4]{ 70, 5, 5, 26 },
      new int[4]{ 3, 6, 0, 54 },
      new int[4]{ 102, 6, 5, 55 },
      new int[4]{ 134, 6, 6, 56 },
      new int[4]{ 198, 6, 7, 57 },
      new int[4]{ 326, 6, 8, 58 },
      new int[4]{ 582, 6, 9, 59 },
      new int[4]{ 1094, 6, 10, 60 },
      new int[4]{ -21, 7, 4, 122 },
      new int[4]{ -4, 7, 0, 123 },
      new int[4]{ 4, 7, 0, 124 },
      new int[4]{ 2118, 7, 11, 125 },
      new int[4]{ -5, 8, 0, 252 },
      new int[4]{ 5, 8, 0, 253 },
      new int[4]{ -22, 8, this.jbig2HuffmanLOW, 254 },
      new int[4]{ 4166, 8, 32 /*0x20*/, (int) byte.MaxValue },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableK = new int[14][]
    {
      new int[4]{ 1, 1, 0, 0 },
      new int[4]{ 2, 2, 1, 2 },
      new int[4]{ 4, 4, 0, 12 },
      new int[4]{ 5, 4, 1, 13 },
      new int[4]{ 7, 5, 1, 28 },
      new int[4]{ 9, 5, 2, 29 },
      new int[4]{ 13, 6, 2, 60 },
      new int[4]{ 17, 7, 2, 122 },
      new int[4]{ 21, 7, 3, 123 },
      new int[4]{ 29, 7, 4, 124 },
      new int[4]{ 45, 7, 5, 125 },
      new int[4]{ 77, 7, 6, 126 },
      new int[4]{ 141, 7, 32 /*0x20*/, (int) sbyte.MaxValue },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableL = new int[14][]
    {
      new int[4]{ 1, 1, 0, 0 },
      new int[4]{ 2, 2, 0, 2 },
      new int[4]{ 3, 3, 1, 6 },
      new int[4]{ 5, 5, 0, 28 },
      new int[4]{ 6, 5, 1, 29 },
      new int[4]{ 8, 6, 1, 60 },
      new int[4]{ 10, 7, 0, 122 },
      new int[4]{ 11, 7, 1, 123 },
      new int[4]{ 13, 7, 2, 124 },
      new int[4]{ 17, 7, 3, 125 },
      new int[4]{ 25, 7, 4, 126 },
      new int[4]{ 41, 8, 5, 254 },
      new int[4]{ 73, 8, 32 /*0x20*/, (int) byte.MaxValue },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableM = new int[14][]
    {
      new int[4]{ 1, 1, 0, 0 },
      new int[4]{ 2, 3, 0, 4 },
      new int[4]{ 7, 3, 3, 5 },
      new int[4]{ 3, 4, 0, 12 },
      new int[4]{ 5, 4, 1, 13 },
      new int[4]{ 4, 5, 0, 28 },
      new int[4]{ 15, 6, 1, 58 },
      new int[4]{ 17, 6, 2, 59 },
      new int[4]{ 21, 6, 3, 60 },
      new int[4]{ 29, 6, 4, 61 },
      new int[4]{ 45, 6, 5, 62 },
      new int[4]{ 77, 7, 6, 126 },
      new int[4]{ 141, 7, 32 /*0x20*/, (int) sbyte.MaxValue },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableN = new int[6][]
    {
      new int[4]{ 0, 1, 0, 0 },
      new int[4]{ -2, 3, 0, 4 },
      new int[4]{ -1, 3, 0, 5 },
      new int[4]{ 1, 3, 0, 6 },
      new int[4]{ 2, 3, 0, 7 },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
    this.huffmanTableO = new int[14][]
    {
      new int[4]{ 0, 1, 0, 0 },
      new int[4]{ -1, 3, 0, 4 },
      new int[4]{ 1, 3, 0, 5 },
      new int[4]{ -2, 4, 0, 12 },
      new int[4]{ 2, 4, 0, 13 },
      new int[4]{ -4, 5, 1, 28 },
      new int[4]{ 3, 5, 1, 29 },
      new int[4]{ -8, 6, 2, 60 },
      new int[4]{ 5, 6, 2, 61 },
      new int[4]{ -24, 7, 4, 124 },
      new int[4]{ 9, 7, 4, 125 },
      new int[4]{ -25, 7, this.jbig2HuffmanLOW, 126 },
      new int[4]{ 25, 7, 32 /*0x20*/, (int) sbyte.MaxValue },
      new int[4]{ 0, 0, this.jbig2HuffmanEOT, 0 }
    };
  }

  internal DecodeIntResult DecodeInt(int[][] table)
  {
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; table[index][2] != this.jbig2HuffmanEOT; ++index)
    {
      for (; num1 < table[index][1]; ++num1)
      {
        int num3 = this.reader.ReadBit();
        num2 = num2 << 1 | num3;
      }
      if (num2 == table[index][3])
      {
        if (table[index][2] == this.jbig2HuffmanOOB)
          return new DecodeIntResult(-1, false);
        int intResult;
        if (table[index][2] == this.jbig2HuffmanLOW)
        {
          int num4 = this.reader.ReadBits(32 /*0x20*/);
          intResult = table[index][0] - num4;
        }
        else if (table[index][2] > 0)
        {
          int num5 = this.reader.ReadBits(table[index][2]);
          intResult = table[index][0] + num5;
        }
        else
          intResult = table[index][0];
        return new DecodeIntResult(intResult, true);
      }
    }
    return new DecodeIntResult(-1, false);
  }

  internal int[][] BuildTable(int[][] table, int length)
  {
    int index1;
    for (index1 = 0; index1 < length; ++index1)
    {
      int index2 = index1;
      while (index2 < length && table[index2][1] == 0)
        ++index2;
      if (index2 != length)
      {
        for (int index3 = index2 + 1; index3 < length; ++index3)
        {
          if (table[index3][1] > 0 && table[index3][1] < table[index2][1])
            index2 = index3;
        }
        if (index2 != index1)
        {
          int[] numArray = table[index2];
          for (int index4 = index2; index4 > index1; --index4)
            table[index4] = table[index4 - 1];
          table[index1] = numArray;
        }
      }
      else
        break;
    }
    table[index1] = table[length];
    int num1 = 0;
    int num2 = 0;
    int[][] numArray1 = table;
    int index5 = num1;
    int index6 = index5 + 1;
    int[] numArray2 = numArray1[index5];
    int num3 = num2;
    int num4 = num3 + 1;
    numArray2[3] = num3;
    for (; table[index6][2] != this.jbig2HuffmanEOT; ++index6)
    {
      int num5 = num4 << table[index6][1] - table[index6 - 1][1];
      int[] numArray3 = table[index6];
      int num6 = num5;
      num4 = num6 + 1;
      numArray3[3] = num6;
    }
    return table;
  }
}
