// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.PdfCcittEncoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class PdfCcittEncoder
{
  private const int c_G3code_Eol = -1;
  private const int c_G3code_Invalid = -2;
  private const int c_G3code_Eof = -3;
  private const int c_G3code_Incomp = -4;
  private const int c_Length = 0;
  private const int c_Code = 1;
  private const int c_Runlen = 2;
  private const int c_Eol = 1;
  private static byte[] s_tableZeroSpan;
  private static byte[] s_tableOneSpan;
  private static int[][] s_terminatingWhiteCodes;
  private static int[][] s_terminatingBlackCodes;
  private static int[] s_horizontalTabel = new int[3]
  {
    3,
    1,
    0
  };
  private static int[] s_passcode = new int[3]{ 4, 1, 0 };
  private static int[] s_maskTabel = new int[9]
  {
    0,
    1,
    3,
    7,
    15,
    31 /*0x1F*/,
    63 /*0x3F*/,
    (int) sbyte.MaxValue,
    (int) byte.MaxValue
  };
  private static int[][] s_verticalTable;
  private int m_rowbytes;
  private int m_rowPixels;
  private int m_countBit = 8;
  private int m_data;
  private byte[] m_refline;
  private List<byte> m_outBuf = new List<byte>();
  private byte[] m_imageData;
  private int m_offsetData;

  static PdfCcittEncoder()
  {
    PdfCcittEncoder.CreteTableZeroSpan();
    PdfCcittEncoder.CreteTableOneSpan();
    PdfCcittEncoder.CreateTerminatingWhiteCodes();
    PdfCcittEncoder.CreateTerminatingBlackCodes();
    PdfCcittEncoder.CreateVerticalTable();
  }

  public byte[] EncodeData(byte[] data, int width, int height)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.m_rowPixels = width;
    this.m_rowbytes = (int) Math.Ceiling((double) this.m_rowPixels / 8.0);
    this.m_refline = new byte[this.m_rowbytes];
    this.m_imageData = data;
    this.m_offsetData = 0;
    for (int index = this.m_rowbytes * height; index > 0; index -= this.m_rowbytes)
    {
      this.Fax3Encode();
      Array.Copy((Array) this.m_imageData, this.m_offsetData, (Array) this.m_refline, 0, this.m_rowbytes);
      this.m_offsetData += this.m_rowbytes;
    }
    this.Fax4Encode();
    byte[] numArray = new byte[this.m_outBuf.Count];
    int index1 = 0;
    for (int count = this.m_outBuf.Count; index1 < count; ++index1)
      numArray[index1] = this.m_outBuf[index1];
    return numArray;
  }

  private static void CreateVerticalTable()
  {
    PdfCcittEncoder.s_verticalTable = new int[7][]
    {
      new int[3]{ 7, 3, 0 },
      new int[3]{ 6, 3, 0 },
      new int[3]{ 3, 3, 0 },
      new int[3]{ 1, 1, 0 },
      new int[3]{ 3, 2, 0 },
      new int[3]{ 6, 2, 0 },
      new int[3]{ 7, 2, 0 }
    };
  }

  private static void CreteTableZeroSpan()
  {
    PdfCcittEncoder.s_tableZeroSpan = new byte[256 /*0x0100*/]
    {
      (byte) 8,
      (byte) 7,
      (byte) 6,
      (byte) 6,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
  }

  private static void CreteTableOneSpan()
  {
    PdfCcittEncoder.s_tableOneSpan = new byte[256 /*0x0100*/]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 2,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 3,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 4,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 5,
      (byte) 6,
      (byte) 6,
      (byte) 7,
      (byte) 8
    };
  }

  private static void CreateTerminatingWhiteCodes()
  {
    PdfCcittEncoder.s_terminatingWhiteCodes = new int[109][]
    {
      new int[3]{ 8, 53, 0 },
      new int[3]{ 6, 7, 1 },
      new int[3]{ 4, 7, 2 },
      new int[3]{ 4, 8, 3 },
      new int[3]{ 4, 11, 4 },
      new int[3]{ 4, 12, 5 },
      new int[3]{ 4, 14, 6 },
      new int[3]{ 4, 15, 7 },
      new int[3]{ 5, 19, 8 },
      new int[3]{ 5, 20, 9 },
      new int[3]{ 5, 7, 10 },
      new int[3]{ 5, 8, 11 },
      new int[3]{ 6, 8, 12 },
      new int[3]{ 6, 3, 13 },
      new int[3]{ 6, 52, 14 },
      new int[3]{ 6, 53, 15 },
      new int[3]{ 6, 42, 16 /*0x10*/ },
      new int[3]{ 6, 43, 17 },
      new int[3]{ 7, 39, 18 },
      new int[3]{ 7, 12, 19 },
      new int[3]{ 7, 8, 20 },
      new int[3]{ 7, 23, 21 },
      new int[3]{ 7, 3, 22 },
      new int[3]{ 7, 4, 23 },
      new int[3]{ 7, 40, 24 },
      new int[3]{ 7, 43, 25 },
      new int[3]{ 7, 19, 26 },
      new int[3]{ 7, 36, 27 },
      new int[3]{ 7, 24, 28 },
      new int[3]{ 8, 2, 29 },
      new int[3]{ 8, 3, 30 },
      new int[3]{ 8, 26, 31 /*0x1F*/ },
      new int[3]{ 8, 27, 32 /*0x20*/ },
      new int[3]{ 8, 18, 33 },
      new int[3]{ 8, 19, 34 },
      new int[3]{ 8, 20, 35 },
      new int[3]{ 8, 21, 36 },
      new int[3]{ 8, 22, 37 },
      new int[3]{ 8, 23, 38 },
      new int[3]{ 8, 40, 39 },
      new int[3]{ 8, 41, 40 },
      new int[3]{ 8, 42, 41 },
      new int[3]{ 8, 43, 42 },
      new int[3]{ 8, 44, 43 },
      new int[3]{ 8, 45, 44 },
      new int[3]{ 8, 4, 45 },
      new int[3]{ 8, 5, 46 },
      new int[3]{ 8, 10, 47 },
      new int[3]{ 8, 11, 48 /*0x30*/ },
      new int[3]{ 8, 82, 49 },
      new int[3]{ 8, 83, 50 },
      new int[3]{ 8, 84, 51 },
      new int[3]{ 8, 85, 52 },
      new int[3]{ 8, 36, 53 },
      new int[3]{ 8, 37, 54 },
      new int[3]{ 8, 88, 55 },
      new int[3]{ 8, 89, 56 },
      new int[3]{ 8, 90, 57 },
      new int[3]{ 8, 91, 58 },
      new int[3]{ 8, 74, 59 },
      new int[3]{ 8, 75, 60 },
      new int[3]{ 8, 50, 61 },
      new int[3]{ 8, 51, 62 },
      new int[3]{ 8, 52, 63 /*0x3F*/ },
      new int[3]{ 5, 27, 64 /*0x40*/ },
      new int[3]{ 5, 18, 128 /*0x80*/ },
      new int[3]{ 6, 23, 192 /*0xC0*/ },
      new int[3]{ 7, 55, 256 /*0x0100*/ },
      new int[3]{ 8, 54, 320 },
      new int[3]{ 8, 55, 384 },
      new int[3]{ 8, 100, 448 },
      new int[3]{ 8, 101, 512 /*0x0200*/ },
      new int[3]{ 8, 104, 576 },
      new int[3]{ 8, 103, 640 },
      new int[3]{ 9, 204, 704 },
      new int[3]{ 9, 205, 768 /*0x0300*/ },
      new int[3]{ 9, 210, 832 },
      new int[3]{ 9, 211, 896 },
      new int[3]{ 9, 212, 960 },
      new int[3]{ 9, 213, 1024 /*0x0400*/ },
      new int[3]{ 9, 214, 1088 },
      new int[3]{ 9, 215, 1152 },
      new int[3]{ 9, 216, 1216 },
      new int[3]{ 9, 217, 1280 /*0x0500*/ },
      new int[3]{ 9, 218, 1344 },
      new int[3]{ 9, 219, 1408 },
      new int[3]{ 9, 152, 1472 },
      new int[3]{ 9, 153, 1536 /*0x0600*/ },
      new int[3]{ 9, 154, 1600 },
      new int[3]{ 6, 24, 1664 },
      new int[3]{ 9, 155, 1728 },
      new int[3]{ 11, 8, 1792 /*0x0700*/ },
      new int[3]{ 11, 12, 1856 },
      new int[3]{ 11, 13, 1920 },
      new int[3]{ 12, 18, 1984 },
      new int[3]{ 12, 19, 2048 /*0x0800*/ },
      new int[3]{ 12, 20, 2112 },
      new int[3]{ 12, 21, 2176 },
      new int[3]{ 12, 22, 2240 },
      new int[3]{ 12, 23, 2304 /*0x0900*/ },
      new int[3]{ 12, 28, 2368 },
      new int[3]{ 12, 29, 2432 },
      new int[3]{ 12, 30, 2496 },
      new int[3]{ 12, 31 /*0x1F*/, 2560 /*0x0A00*/ },
      new int[3]{ 12, 1, -1 },
      new int[3]{ 9, 1, -2 },
      new int[3]{ 10, 1, -2 },
      new int[3]{ 11, 1, -2 },
      new int[3]{ 12, 0, -2 }
    };
  }

  private static void CreateTerminatingBlackCodes()
  {
    PdfCcittEncoder.s_terminatingBlackCodes = new int[109][]
    {
      new int[3]{ 10, 55, 0 },
      new int[3]{ 3, 2, 1 },
      new int[3]{ 2, 3, 2 },
      new int[3]{ 2, 2, 3 },
      new int[3]{ 3, 3, 4 },
      new int[3]{ 4, 3, 5 },
      new int[3]{ 4, 2, 6 },
      new int[3]{ 5, 3, 7 },
      new int[3]{ 6, 5, 8 },
      new int[3]{ 6, 4, 9 },
      new int[3]{ 7, 4, 10 },
      new int[3]{ 7, 5, 11 },
      new int[3]{ 7, 7, 12 },
      new int[3]{ 8, 4, 13 },
      new int[3]{ 8, 7, 14 },
      new int[3]{ 9, 24, 15 },
      new int[3]{ 10, 23, 16 /*0x10*/ },
      new int[3]{ 10, 24, 17 },
      new int[3]{ 10, 8, 18 },
      new int[3]{ 11, 103, 19 },
      new int[3]{ 11, 104, 20 },
      new int[3]{ 11, 108, 21 },
      new int[3]{ 11, 55, 22 },
      new int[3]{ 11, 40, 23 },
      new int[3]{ 11, 23, 24 },
      new int[3]{ 11, 24, 25 },
      new int[3]{ 12, 202, 26 },
      new int[3]{ 12, 203, 27 },
      new int[3]{ 12, 204, 28 },
      new int[3]{ 12, 205, 29 },
      new int[3]{ 12, 104, 30 },
      new int[3]{ 12, 105, 31 /*0x1F*/ },
      new int[3]{ 12, 106, 32 /*0x20*/ },
      new int[3]{ 12, 107, 33 },
      new int[3]{ 12, 210, 34 },
      new int[3]{ 12, 211, 35 },
      new int[3]{ 12, 212, 36 },
      new int[3]{ 12, 213, 37 },
      new int[3]{ 12, 214, 38 },
      new int[3]{ 12, 215, 39 },
      new int[3]{ 12, 108, 40 },
      new int[3]{ 12, 109, 41 },
      new int[3]{ 12, 218, 42 },
      new int[3]{ 12, 219, 43 },
      new int[3]{ 12, 84, 44 },
      new int[3]{ 12, 85, 45 },
      new int[3]{ 12, 86, 46 },
      new int[3]{ 12, 87, 47 },
      new int[3]{ 12, 100, 48 /*0x30*/ },
      new int[3]{ 12, 101, 49 },
      new int[3]{ 12, 82, 50 },
      new int[3]{ 12, 83, 51 },
      new int[3]{ 12, 36, 52 },
      new int[3]{ 12, 55, 53 },
      new int[3]{ 12, 56, 54 },
      new int[3]{ 12, 39, 55 },
      new int[3]{ 12, 40, 56 },
      new int[3]{ 12, 88, 57 },
      new int[3]{ 12, 89, 58 },
      new int[3]{ 12, 43, 59 },
      new int[3]{ 12, 44, 60 },
      new int[3]{ 12, 90, 61 },
      new int[3]{ 12, 102, 62 },
      new int[3]{ 12, 103, 63 /*0x3F*/ },
      new int[3]{ 10, 15, 64 /*0x40*/ },
      new int[3]{ 12, 200, 128 /*0x80*/ },
      new int[3]{ 12, 201, 192 /*0xC0*/ },
      new int[3]{ 12, 91, 256 /*0x0100*/ },
      new int[3]{ 12, 51, 320 },
      new int[3]{ 12, 52, 384 },
      new int[3]{ 12, 53, 448 },
      new int[3]{ 13, 108, 512 /*0x0200*/ },
      new int[3]{ 13, 109, 576 },
      new int[3]{ 13, 74, 640 },
      new int[3]{ 13, 75, 704 },
      new int[3]{ 13, 76, 768 /*0x0300*/ },
      new int[3]{ 13, 77, 832 },
      new int[3]{ 13, 114, 896 },
      new int[3]{ 13, 115, 960 },
      new int[3]{ 13, 116, 1024 /*0x0400*/ },
      new int[3]{ 13, 117, 1088 },
      new int[3]{ 13, 118, 1152 },
      new int[3]{ 13, 119, 1216 },
      new int[3]{ 13, 82, 1280 /*0x0500*/ },
      new int[3]{ 13, 83, 1344 },
      new int[3]{ 13, 84, 1408 },
      new int[3]{ 13, 85, 1472 },
      new int[3]{ 13, 90, 1536 /*0x0600*/ },
      new int[3]{ 13, 91, 1600 },
      new int[3]{ 13, 100, 1664 },
      new int[3]{ 13, 101, 1728 },
      new int[3]{ 11, 8, 1792 /*0x0700*/ },
      new int[3]{ 11, 12, 1856 },
      new int[3]{ 11, 13, 1920 },
      new int[3]{ 12, 18, 1984 },
      new int[3]{ 12, 19, 2048 /*0x0800*/ },
      new int[3]{ 12, 20, 2112 },
      new int[3]{ 12, 21, 2176 },
      new int[3]{ 12, 22, 2240 },
      new int[3]{ 12, 23, 2304 /*0x0900*/ },
      new int[3]{ 12, 28, 2368 },
      new int[3]{ 12, 29, 2432 },
      new int[3]{ 12, 30, 2496 },
      new int[3]{ 12, 31 /*0x1F*/, 2560 /*0x0A00*/ },
      new int[3]{ 12, 1, -1 },
      new int[3]{ 9, 1, -2 },
      new int[3]{ 10, 1, -2 },
      new int[3]{ 11, 1, -2 },
      new int[3]{ 12, 0, -2 }
    };
  }

  private void Putcode(int[] table) => this.PutBits(table[1], table[0]);

  private void PutSpan(int span, int[][] tab)
  {
    int[] numArray1;
    for (; span >= 2624; span -= numArray1[2])
    {
      numArray1 = tab[103];
      this.PutBits(numArray1[1], numArray1[0]);
    }
    if (span >= 64 /*0x40*/)
    {
      int[] numArray2 = tab[63 /*0x3F*/ + (span >> 6)];
      this.PutBits(numArray2[1], numArray2[0]);
      span -= numArray2[2];
    }
    this.PutBits(tab[span][1], tab[span][0]);
  }

  private void PutBits(int bits, int length)
  {
    for (; length > this.m_countBit; this.m_countBit = 8)
    {
      this.m_data |= bits >> length - this.m_countBit;
      length -= this.m_countBit;
      this.m_outBuf.Add((byte) this.m_data);
      this.m_data = 0;
    }
    this.m_data |= (bits & PdfCcittEncoder.s_maskTabel[length]) << this.m_countBit - length;
    this.m_countBit -= length;
    if (this.m_countBit != 0)
      return;
    this.m_outBuf.Add((byte) this.m_data);
    this.m_data = 0;
    this.m_countBit = 8;
  }

  private void Fax3Encode()
  {
    int num1 = 0;
    int num2 = this.Pixel(this.m_imageData, this.m_offsetData, 0) != 0 ? 0 : this.Finddiff(this.m_imageData, this.m_offsetData, 0, this.m_rowPixels, 0);
    int num3 = this.Pixel(this.m_refline, 0, 0) != 0 ? 0 : this.Finddiff(this.m_refline, 0, 0, this.m_rowPixels, 0);
    while (true)
    {
      int num4 = this.Finddiff2(this.m_refline, 0, num3, this.m_rowPixels, this.Pixel(this.m_refline, 0, num3));
      if (num4 >= num2)
      {
        int num5 = num3 - num2;
        if (-3 > num5 || num5 > 3)
        {
          int num6 = this.Finddiff2(this.m_imageData, this.m_offsetData, num2, this.m_rowPixels, this.Pixel(this.m_imageData, this.m_offsetData, num2));
          this.Putcode(PdfCcittEncoder.s_horizontalTabel);
          if (num1 + num2 == 0 || this.Pixel(this.m_imageData, this.m_offsetData, num1) == 0)
          {
            this.PutSpan(num2 - num1, PdfCcittEncoder.s_terminatingWhiteCodes);
            this.PutSpan(num6 - num2, PdfCcittEncoder.s_terminatingBlackCodes);
          }
          else
          {
            this.PutSpan(num2 - num1, PdfCcittEncoder.s_terminatingBlackCodes);
            this.PutSpan(num6 - num2, PdfCcittEncoder.s_terminatingWhiteCodes);
          }
          num1 = num6;
        }
        else
        {
          this.Putcode(PdfCcittEncoder.s_verticalTable[num5 + 3]);
          num1 = num2;
        }
      }
      else
      {
        this.Putcode(PdfCcittEncoder.s_passcode);
        num1 = num4;
      }
      if (num1 < this.m_rowPixels)
      {
        num2 = this.Finddiff(this.m_imageData, this.m_offsetData, num1, this.m_rowPixels, this.Pixel(this.m_imageData, this.m_offsetData, num1));
        num3 = this.Finddiff(this.m_refline, 0, this.Finddiff(this.m_refline, 0, num1, this.m_rowPixels, this.Pixel(this.m_imageData, this.m_offsetData, num1) ^ 1), this.m_rowPixels, this.Pixel(this.m_imageData, this.m_offsetData, num1));
      }
      else
        break;
    }
  }

  private void Fax4Encode()
  {
    this.PutBits(1, 12);
    this.PutBits(1, 12);
    if (this.m_countBit == 8)
      return;
    this.m_outBuf.Add((byte) this.m_data);
    this.m_data = 0;
    this.m_countBit = 8;
  }

  private int Pixel(byte[] data, int offset, int bit)
  {
    int num = 0;
    if (bit < this.m_rowPixels)
      num = ((int) data[offset + (bit >> 3)] & (int) byte.MaxValue) >> 7 - (bit & 7) & 1;
    return num;
  }

  private int FindFirstSpan(byte[] bp, int offset, int bs, int be)
  {
    int num1 = be - bs;
    int index = offset + (bs >> 3);
    int num2;
    int firstSpan;
    if (num1 > 0 && (num2 = bs & 7) != 0)
    {
      firstSpan = (int) PdfCcittEncoder.s_tableOneSpan[(int) bp[index] << num2 & (int) byte.MaxValue];
      if (firstSpan > 8 - num2)
        firstSpan = 8 - num2;
      if (firstSpan > num1)
        firstSpan = num1;
      if (num2 + firstSpan < 8)
        return firstSpan;
      num1 -= firstSpan;
      ++index;
    }
    else
      firstSpan = 0;
    while (num1 >= 8)
    {
      if (bp[index] != byte.MaxValue)
        return firstSpan + (int) PdfCcittEncoder.s_tableOneSpan[(int) bp[index] & (int) byte.MaxValue];
      firstSpan += 8;
      num1 -= 8;
      ++index;
    }
    if (num1 > 0)
    {
      int num3 = (int) PdfCcittEncoder.s_tableOneSpan[(int) bp[index] & (int) byte.MaxValue];
      firstSpan += num3 > num1 ? num1 : num3;
    }
    return firstSpan;
  }

  private int FindZeroSpan(byte[] bp, int offset, int bs, int be)
  {
    int num1 = be - bs;
    int index = offset + (bs >> 3);
    int num2;
    int zeroSpan;
    if (num1 > 0 && (num2 = bs & 7) != 0)
    {
      zeroSpan = (int) PdfCcittEncoder.s_tableZeroSpan[(int) bp[index] << num2 & (int) byte.MaxValue];
      if (zeroSpan > 8 - num2)
        zeroSpan = 8 - num2;
      if (zeroSpan > num1)
        zeroSpan = num1;
      if (num2 + zeroSpan < 8)
        return zeroSpan;
      num1 -= zeroSpan;
      ++index;
    }
    else
      zeroSpan = 0;
    while (num1 >= 8)
    {
      if (bp[index] != (byte) 0)
        return zeroSpan + (int) PdfCcittEncoder.s_tableZeroSpan[(int) bp[index] & (int) byte.MaxValue];
      zeroSpan += 8;
      num1 -= 8;
      ++index;
    }
    if (num1 > 0)
    {
      int num3 = (int) PdfCcittEncoder.s_tableZeroSpan[(int) bp[index] & (int) byte.MaxValue];
      zeroSpan += num3 > num1 ? num1 : num3;
    }
    return zeroSpan;
  }

  private int Finddiff(byte[] bp, int offset, int bs, int be, int color)
  {
    return bs + (color != 0 ? this.FindFirstSpan(bp, offset, bs, be) : this.FindZeroSpan(bp, offset, bs, be));
  }

  private int Finddiff2(byte[] bp, int offset, int bs, int be, int color)
  {
    return bs >= be ? be : this.Finddiff(bp, offset, bs, be, color);
  }
}
