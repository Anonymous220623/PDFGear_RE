// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SymbolDictionarySegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class SymbolDictionarySegment(JBIG2StreamDecoder streamDecoder) : JBIG2Segment(streamDecoder)
{
  private int m_noOfExportedSymbols;
  private int m_noOfNewSymbols;
  private JBIG2Image[] bitmaps;
  private SymbolDictionaryFlags m_symbolDictionaryFlags = new SymbolDictionaryFlags();
  private ArithmeticDecoderStats m_genericRegionStats;
  private ArithmeticDecoderStats m_refinementRegionStats;
  private HuffmanDecoder m_huffDecoder = new HuffmanDecoder();
  private BitOperation m_bitOperation = new BitOperation();
  private RectangularArrays m_rectangularArrays = new RectangularArrays();
  private short[] m_symbolDictionaryAdaptiveTemplateX = new short[4];
  private short[] m_symbolDictionaryAdaptiveTemplateY = new short[4];
  private short[] m_symbolDictionaryRAdaptiveTemplateX = new short[2];
  private short[] m_symbolDictionaryRAdaptiveTemplateY = new short[2];

  internal int NoOfExportedSymbols
  {
    get => this.m_noOfExportedSymbols;
    set => this.m_noOfExportedSymbols = value;
  }

  public override void readSegment()
  {
    this.ReadSymbolDictionaryFlags();
    IList list = (IList) new List<object>();
    int num1 = 0;
    int referedToSegCount = this.m_segmentHeader.ReferedToSegCount;
    int[] referredToSegments = this.m_segmentHeader.ReferredToSegments;
    for (int index = 0; index < referedToSegCount; ++index)
    {
      JBIG2Segment segment = this.m_decoder.FindSegment(referredToSegments[index]);
      switch (segment.m_segmentHeader.SegmentType)
      {
        case 0:
          num1 += ((SymbolDictionarySegment) segment).m_noOfExportedSymbols;
          break;
        case 53:
          list.Add((object) segment);
          break;
      }
    }
    int num2 = 0;
    for (int index = 1; index < num1 + this.m_noOfNewSymbols && index >= 0; index <<= 1)
      ++num2;
    JBIG2Image[] symbols = new JBIG2Image[num1 + this.m_noOfNewSymbols];
    int num3 = 0;
    SymbolDictionarySegment dictionarySegment = (SymbolDictionarySegment) null;
    for (int index1 = 0; index1 < referedToSegCount; ++index1)
    {
      JBIG2Segment segment = this.m_decoder.FindSegment(referredToSegments[index1]);
      if (segment.m_segmentHeader.SegmentType == 0)
      {
        dictionarySegment = (SymbolDictionarySegment) segment;
        for (int index2 = 0; index2 < dictionarySegment.m_noOfExportedSymbols; ++index2)
          symbols[num3++] = dictionarySegment.bitmaps[index2];
      }
    }
    int[][] table1 = (int[][]) null;
    int[][] table2 = (int[][]) null;
    int[][] table3 = (int[][]) null;
    int[][] table4 = (int[][]) null;
    bool huffman = this.m_symbolDictionaryFlags.GetFlagValue("SD_HUFF") != 0;
    int flagValue1 = this.m_symbolDictionaryFlags.GetFlagValue("SD_HUFF_DH");
    int flagValue2 = this.m_symbolDictionaryFlags.GetFlagValue("SD_HUFF_DW");
    int flagValue3 = this.m_symbolDictionaryFlags.GetFlagValue("SD_HUFF_BM_SIZE");
    int flagValue4 = this.m_symbolDictionaryFlags.GetFlagValue("SD_HUFF_AGG_INST");
    if (huffman)
    {
      switch (flagValue1)
      {
        case 0:
          table1 = this.m_huffDecoder.huffmanTableD;
          break;
        case 1:
          table1 = this.m_huffDecoder.huffmanTableE;
          break;
        default:
          table1 = (int[][]) null;
          break;
      }
      switch (flagValue2)
      {
        case 0:
          table2 = this.m_huffDecoder.huffmanTableB;
          break;
        case 1:
          table2 = this.m_huffDecoder.huffmanTableC;
          break;
        default:
          table2 = (int[][]) null;
          break;
      }
      table3 = flagValue3 != 0 ? (int[][]) null : this.m_huffDecoder.huffmanTableA;
      table4 = flagValue4 != 0 ? (int[][]) null : this.m_huffDecoder.huffmanTableA;
    }
    int flagValue5 = this.m_symbolDictionaryFlags.GetFlagValue("BITMAP_CC_USED");
    int flagValue6 = this.m_symbolDictionaryFlags.GetFlagValue("SD_TEMPLATE");
    if (!huffman)
    {
      if (flagValue5 != 0 && dictionarySegment != null)
        this.m_arithmeticDecoder.ResetGenericStats(flagValue6, dictionarySegment.m_genericRegionStats);
      else
        this.m_arithmeticDecoder.ResetGenericStats(flagValue6, (ArithmeticDecoderStats) null);
      this.m_arithmeticDecoder.ResetIntegerStats(num2);
      this.m_arithmeticDecoder.Start();
    }
    int flagValue7 = this.m_symbolDictionaryFlags.GetFlagValue("SD_REF_AGG");
    int flagValue8 = this.m_symbolDictionaryFlags.GetFlagValue("SD_R_TEMPLATE");
    if (flagValue7 != 0)
    {
      if (flagValue5 != 0 && dictionarySegment != null)
        this.m_arithmeticDecoder.ResetRefinementStats(flagValue8, dictionarySegment.m_refinementRegionStats);
      else
        this.m_arithmeticDecoder.ResetRefinementStats(flagValue8, (ArithmeticDecoderStats) null);
    }
    int[] numArray1 = new int[this.m_noOfNewSymbols];
    int num4 = 0;
    int index3 = 0;
    while (index3 < this.m_noOfNewSymbols)
    {
      int num5 = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IadhStats).IntResult : this.m_huffmanDecoder.DecodeInt(table1).IntResult;
      num4 += num5;
      int width1 = 0;
      int width2 = 0;
      int index4 = index3;
      while (true)
      {
        DecodeIntResult decodeIntResult = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IadwStats) : this.m_huffmanDecoder.DecodeInt(table2);
        if (decodeIntResult.BooleanResult)
        {
          int intResult1 = decodeIntResult.IntResult;
          width1 += intResult1;
          if (huffman && flagValue7 == 0)
          {
            numArray1[index3] = width1;
            width2 += width1;
          }
          else if (flagValue7 == 1)
          {
            int noOfSymbolInstances = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IaaiStats).IntResult : this.m_huffmanDecoder.DecodeInt(table4).IntResult;
            if (noOfSymbolInstances == 1)
            {
              int index5;
              int intResult2;
              int intResult3;
              if (huffman)
              {
                index5 = this.m_decoder.ReadBits(num2);
                intResult2 = this.m_huffmanDecoder.DecodeInt(this.m_huffDecoder.huffmanTableO).IntResult;
                intResult3 = this.m_huffmanDecoder.DecodeInt(this.m_huffDecoder.huffmanTableO).IntResult;
                this.m_decoder.ConsumeRemainingBits();
                this.m_arithmeticDecoder.Start();
              }
              else
              {
                index5 = (int) this.m_arithmeticDecoder.DecodeIAID((long) num2, this.m_arithmeticDecoder.IaidStats);
                intResult2 = this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IardxStats).IntResult;
                intResult3 = this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IardyStats).IntResult;
              }
              JBIG2Image referredToBitmap = symbols[index5];
              JBIG2Image jbiG2Image = new JBIG2Image(width1, num4, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
              jbiG2Image.ReadGenericRefinementRegion(flagValue8, false, referredToBitmap, intResult2, intResult3, this.m_symbolDictionaryRAdaptiveTemplateX, this.m_symbolDictionaryRAdaptiveTemplateY);
              symbols[num1 + index3] = jbiG2Image;
            }
            else
            {
              JBIG2Image jbiG2Image = new JBIG2Image(width1, num4, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
              jbiG2Image.ReadTextRegion(huffman, true, noOfSymbolInstances, 0, num1 + index3, (int[][]) null, num2, symbols, 0, 0, false, 1, 0, this.m_huffDecoder.huffmanTableF, this.m_huffDecoder.huffmanTableH, this.m_huffDecoder.huffmanTableK, this.m_huffDecoder.huffmanTableO, this.m_huffDecoder.huffmanTableO, this.m_huffDecoder.huffmanTableO, this.m_huffDecoder.huffmanTableO, this.m_huffDecoder.huffmanTableA, flagValue8, this.m_symbolDictionaryRAdaptiveTemplateX, this.m_symbolDictionaryRAdaptiveTemplateY, this.m_decoder);
              symbols[num1 + index3] = jbiG2Image;
            }
          }
          else
          {
            JBIG2Image jbiG2Image = new JBIG2Image(width1, num4, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
            jbiG2Image.ReadBitmap(false, flagValue6, false, false, (JBIG2Image) null, this.m_symbolDictionaryAdaptiveTemplateX, this.m_symbolDictionaryAdaptiveTemplateY, 0);
            symbols[num1 + index3] = jbiG2Image;
          }
          ++index3;
        }
        else
          break;
      }
      if (huffman && flagValue7 == 0)
      {
        int intResult = this.m_huffmanDecoder.DecodeInt(table3).IntResult;
        this.m_decoder.ConsumeRemainingBits();
        JBIG2Image jbiG2Image = new JBIG2Image(width2, num4, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
        if (intResult == 0)
        {
          int num6 = width2 % 8;
          int Size2 = (int) Math.Ceiling((double) width2 / 8.0);
          short[] buff = new short[num4 * (width2 + 7 >> 3)];
          this.m_decoder.ReadByte(buff);
          short[][] numArray2 = this.m_rectangularArrays.ReturnRectangularShortArray(num4, Size2);
          int index6 = 0;
          for (int index7 = 0; index7 < num4; ++index7)
          {
            for (int index8 = 0; index8 < Size2; ++index8)
            {
              numArray2[index7][index8] = buff[index6];
              ++index6;
            }
          }
          int row = 0;
          int col = 0;
          for (int index9 = 0; index9 < num4; ++index9)
          {
            for (int index10 = 0; index10 < Size2; ++index10)
            {
              if (index10 == Size2 - 1)
              {
                short num7 = numArray2[index9][index10];
                for (int index11 = 7; index11 >= num6; --index11)
                {
                  short num8 = (short) (1 << index11);
                  int num9 = ((int) num7 & (int) num8) >> index11;
                  jbiG2Image.SetPixel(col, row, num9);
                  ++col;
                }
                ++row;
                col = 0;
              }
              else
              {
                short num10 = numArray2[index9][index10];
                for (int index12 = 7; index12 >= 0; --index12)
                {
                  short num11 = (short) (1 << index12);
                  int num12 = ((int) num10 & (int) num11) >> index12;
                  jbiG2Image.SetPixel(col, row, num12);
                  ++col;
                }
              }
            }
          }
        }
        else
          jbiG2Image.ReadBitmap(true, 0, false, false, (JBIG2Image) null, (short[]) null, (short[]) null, intResult);
        int x = 0;
        for (; index4 < index3; ++index4)
        {
          symbols[num1 + index4] = jbiG2Image.GetSlice(x, 0, numArray1[index4], num4);
          x += numArray1[index4];
        }
      }
    }
    this.bitmaps = new JBIG2Image[this.m_noOfExportedSymbols];
    int num13;
    int num14 = num13 = 0;
    bool flag = false;
    while (num13 < num1 + this.m_noOfNewSymbols)
    {
      int num15 = !huffman ? this.m_arithmeticDecoder.DecodeInt(this.m_arithmeticDecoder.IaexStats).IntResult : this.m_huffmanDecoder.DecodeInt(this.m_huffDecoder.huffmanTableA).IntResult;
      if (flag)
      {
        for (int index13 = 0; index13 < num15; ++index13)
          this.bitmaps[num14++] = symbols[num13++];
      }
      else
        num13 += num15;
      flag = !flag;
    }
    int flagValue9 = this.m_symbolDictionaryFlags.GetFlagValue("BITMAP_CC_RETAINED");
    if (!huffman && flagValue9 == 1)
    {
      this.m_genericRegionStats = this.m_genericRegionStats.copy();
      if (flagValue7 == 1)
        this.m_refinementRegionStats = this.m_refinementRegionStats.copy();
    }
    this.m_decoder.ConsumeRemainingBits();
  }

  private void ReadSymbolDictionaryFlags()
  {
    short[] numArray1 = new short[2];
    this.m_decoder.ReadByte(numArray1);
    this.m_symbolDictionaryFlags.setFlags(this.m_bitOperation.GetInt16(numArray1));
    int flagValue1 = this.m_symbolDictionaryFlags.GetFlagValue("SD_HUFF");
    int flagValue2 = this.m_symbolDictionaryFlags.GetFlagValue("SD_TEMPLATE");
    if (flagValue1 == 0)
    {
      if (flagValue2 == 0)
      {
        this.m_symbolDictionaryAdaptiveTemplateX[0] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateY[0] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateX[1] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateY[1] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateX[2] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateY[2] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateX[3] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateY[3] = this.ReadAtValue();
      }
      else
      {
        this.m_symbolDictionaryAdaptiveTemplateX[0] = this.ReadAtValue();
        this.m_symbolDictionaryAdaptiveTemplateY[0] = this.ReadAtValue();
      }
    }
    int flagValue3 = this.m_symbolDictionaryFlags.GetFlagValue("SD_REF_AGG");
    int flagValue4 = this.m_symbolDictionaryFlags.GetFlagValue("SD_R_TEMPLATE");
    if (flagValue3 != 0 && flagValue4 == 0)
    {
      this.m_symbolDictionaryRAdaptiveTemplateX[0] = this.ReadAtValue();
      this.m_symbolDictionaryRAdaptiveTemplateY[0] = this.ReadAtValue();
      this.m_symbolDictionaryRAdaptiveTemplateX[1] = this.ReadAtValue();
      this.m_symbolDictionaryRAdaptiveTemplateY[1] = this.ReadAtValue();
    }
    short[] numArray2 = new short[4];
    this.m_decoder.ReadByte(numArray2);
    this.m_noOfExportedSymbols = this.m_bitOperation.GetInt32(numArray2);
    short[] numArray3 = new short[4];
    this.m_decoder.ReadByte(numArray3);
    this.m_noOfNewSymbols = this.m_bitOperation.GetInt32(numArray3);
  }

  public JBIG2Image[] getBitmaps() => this.bitmaps;
}
