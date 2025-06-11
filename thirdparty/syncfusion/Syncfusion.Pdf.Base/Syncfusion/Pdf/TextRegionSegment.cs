// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TextRegionSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class TextRegionSegment : JBIG2BaseSegment
{
  private TextRegionFlags m_textRegionFlags = new TextRegionFlags();
  private TextRegionHuffmanFlags m_textRegionHuffmanFlags = new TextRegionHuffmanFlags();
  private int m_noOfSymbolInstances;
  private bool m_inlineImage;
  private short[] m_symbolRegionAdaptiveTemplateX = new short[2];
  private short[] symbolRegionAdaptiveTemplateY = new short[2];
  private HuffmanDecoder m_huffDecoder = new HuffmanDecoder();
  private BitOperation m_bitOperation = new BitOperation();
  private RectangularArrays m_rectangularArrays = new RectangularArrays();

  public TextRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage)
    : base(streamDecoder)
  {
    this.m_inlineImage = inlineImage;
  }

  public override void readSegment()
  {
    base.readSegment();
    this.ReadTextRegionFlags();
    short[] numArray = new short[4];
    this.m_decoder.ReadByte(numArray);
    this.m_noOfSymbolInstances = this.m_bitOperation.GetInt32(numArray);
    int referedToSegCount = this.m_segmentHeader.ReferedToSegCount;
    int[] referredToSegments = this.m_segmentHeader.ReferredToSegments;
    IList list1 = (IList) new List<object>();
    IList list2 = (IList) new List<object>();
    int length = 0;
    for (int index = 0; index < referedToSegCount; ++index)
    {
      JBIG2Segment segment = this.m_decoder.FindSegment(referredToSegments[index]);
      switch (segment.m_segmentHeader.SegmentType)
      {
        case 0:
          list2.Add((object) segment);
          length += ((SymbolDictionarySegment) segment).NoOfExportedSymbols;
          break;
        case 53:
          list1.Add((object) segment);
          break;
      }
    }
    int symbolCodeLength = 0;
    for (int index = 1; index < length; index <<= 1)
      ++symbolCodeLength;
    int index1 = 0;
    JBIG2Image[] symbols = new JBIG2Image[length];
    foreach (JBIG2Segment jbiG2Segment in (IEnumerable) list2)
    {
      if (jbiG2Segment.m_segmentHeader.SegmentType == 0)
      {
        foreach (JBIG2Image bitmap in ((SymbolDictionarySegment) jbiG2Segment).getBitmaps())
        {
          symbols[index1] = bitmap;
          ++index1;
        }
      }
    }
    int[][] huffmanFSTable = (int[][]) null;
    int[][] huffmanDSTable = (int[][]) null;
    int[][] huffmanDTTable = (int[][]) null;
    int[][] huffmanRDWTable = (int[][]) null;
    int[][] huffmanRDHTable = (int[][]) null;
    int[][] huffmanRDXTable = (int[][]) null;
    int[][] huffmanRDYTable = (int[][]) null;
    int[][] huffmanRSizeTable = (int[][]) null;
    bool huffman = this.m_textRegionFlags.GetFlagValue("SB_HUFF") != 0;
    if (huffman)
    {
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_FS"))
      {
        case 0:
          huffmanFSTable = this.m_huffDecoder.huffmanTableF;
          break;
        case 1:
          huffmanFSTable = this.m_huffDecoder.huffmanTableG;
          break;
      }
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_DS"))
      {
        case 0:
          huffmanDSTable = this.m_huffDecoder.huffmanTableH;
          break;
        case 1:
          huffmanDSTable = this.m_huffDecoder.huffmanTableI;
          break;
        case 2:
          huffmanDSTable = this.m_huffDecoder.huffmanTableJ;
          break;
      }
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_DT"))
      {
        case 0:
          huffmanDTTable = this.m_huffDecoder.huffmanTableK;
          break;
        case 1:
          huffmanDTTable = this.m_huffDecoder.huffmanTableL;
          break;
        case 2:
          huffmanDTTable = this.m_huffDecoder.huffmanTableM;
          break;
      }
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_RDW"))
      {
        case 0:
          huffmanRDWTable = this.m_huffDecoder.huffmanTableN;
          break;
        case 1:
          huffmanRDWTable = this.m_huffDecoder.huffmanTableO;
          break;
      }
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_RDH"))
      {
        case 0:
          huffmanRDHTable = this.m_huffDecoder.huffmanTableN;
          break;
        case 1:
          huffmanRDHTable = this.m_huffDecoder.huffmanTableO;
          break;
      }
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_RDX"))
      {
        case 0:
          huffmanRDXTable = this.m_huffDecoder.huffmanTableN;
          break;
        case 1:
          huffmanRDXTable = this.m_huffDecoder.huffmanTableO;
          break;
      }
      switch (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_RDY"))
      {
        case 0:
          huffmanRDYTable = this.m_huffDecoder.huffmanTableN;
          break;
        case 1:
          huffmanRDYTable = this.m_huffDecoder.huffmanTableO;
          break;
      }
      if (this.m_textRegionHuffmanFlags.GetFlagValue("SB_HUFF_RSIZE") == 0)
        huffmanRSizeTable = this.m_huffDecoder.huffmanTableA;
    }
    int[][] table1 = this.m_rectangularArrays.ReturnRectangularIntArray(36, 4);
    int[][] table2 = this.m_rectangularArrays.ReturnRectangularIntArray(length + 1, 4);
    int[][] symbolCodeTable;
    if (huffman)
    {
      this.m_decoder.ConsumeRemainingBits();
      for (int index2 = 0; index2 < 32 /*0x20*/; ++index2)
        table1[index2] = new int[4]
        {
          index2,
          this.m_decoder.ReadBits(4),
          0,
          0
        };
      table1[32 /*0x20*/] = new int[4]
      {
        259,
        this.m_decoder.ReadBits(4),
        2,
        0
      };
      table1[33] = new int[4]
      {
        515,
        this.m_decoder.ReadBits(4),
        3,
        0
      };
      table1[34] = new int[4]
      {
        523,
        this.m_decoder.ReadBits(4),
        7,
        0
      };
      table1[35] = new int[3]
      {
        0,
        0,
        this.m_huffDecoder.jbig2HuffmanEOT
      };
      int[][] table3 = this.m_huffDecoder.BuildTable(table1, 35);
      for (int index3 = 0; index3 < length; ++index3)
        table2[index3] = new int[4]{ index3, 0, 0, 0 };
      int index4 = 0;
label_58:
      while (index4 < length)
      {
        int intResult = this.m_huffmanDecoder.DecodeInt(table3).IntResult;
        if (intResult > 512 /*0x0200*/)
        {
          int num = intResult - 512 /*0x0200*/;
          while (true)
          {
            if (num != 0 && index4 < length)
            {
              table2[index4++][1] = 0;
              --num;
            }
            else
              goto label_58;
          }
        }
        else if (intResult > 256 /*0x0100*/)
        {
          int num = intResult - 256 /*0x0100*/;
          while (true)
          {
            if (num != 0 && index4 < length)
            {
              table2[index4][1] = table2[index4 - 1][1];
              ++index4;
              --num;
            }
            else
              goto label_58;
          }
        }
        else
          table2[index4++][1] = intResult;
      }
      table2[length][1] = 0;
      table2[length][2] = this.m_huffDecoder.jbig2HuffmanEOT;
      symbolCodeTable = this.m_huffDecoder.BuildTable(table2, length);
      this.m_decoder.ConsumeRemainingBits();
    }
    else
    {
      symbolCodeTable = (int[][]) null;
      this.m_arithmeticDecoder.ResetIntegerStats(symbolCodeLength);
      this.m_arithmeticDecoder.Start();
    }
    bool symbolRefine = this.m_textRegionFlags.GetFlagValue("SB_REFINE") != 0;
    int flagValue1 = this.m_textRegionFlags.GetFlagValue("LOG_SB_STRIPES");
    int flagValue2 = this.m_textRegionFlags.GetFlagValue("SB_DEF_PIXEL");
    int flagValue3 = this.m_textRegionFlags.GetFlagValue("SB_COMB_OP");
    bool transposed = this.m_textRegionFlags.GetFlagValue("TRANSPOSED") != 0;
    int flagValue4 = this.m_textRegionFlags.GetFlagValue("REF_CORNER");
    int flagValue5 = this.m_textRegionFlags.GetFlagValue("SB_DS_OFFSET");
    int flagValue6 = this.m_textRegionFlags.GetFlagValue("SB_R_TEMPLATE");
    if (symbolRefine)
      this.m_arithmeticDecoder.ResetRefinementStats(flagValue6, (ArithmeticDecoderStats) null);
    JBIG2Image bitmap1 = new JBIG2Image(this.regionBitmapWidth, this.regionBitmapHeight, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
    bitmap1.ReadTextRegion(huffman, symbolRefine, this.m_noOfSymbolInstances, flagValue1, length, symbolCodeTable, symbolCodeLength, symbols, flagValue2, flagValue3, transposed, flagValue4, flagValue5, huffmanFSTable, huffmanDSTable, huffmanDTTable, huffmanRDWTable, huffmanRDHTable, huffmanRDXTable, huffmanRDYTable, huffmanRSizeTable, flagValue6, this.m_symbolRegionAdaptiveTemplateX, this.symbolRegionAdaptiveTemplateY, this.m_decoder);
    if (this.m_inlineImage)
    {
      JBIG2Image pageBitmap = this.m_decoder.FindPageSegement(this.m_segmentHeader.PageAssociation).pageBitmap;
      int flagValue7 = this.regionFlags.GetFlagValue("EXTERNAL_COMBINATION_OPERATOR");
      pageBitmap.Combine(bitmap1, this.regionBitmapXLocation, this.regionBitmapYLocation, (long) flagValue7);
    }
    else
    {
      bitmap1.BitmapNumber = this.m_segmentHeader.SegmentNumber;
      this.m_decoder.appendBitmap(bitmap1);
    }
    this.m_decoder.ConsumeRemainingBits();
  }

  private void ReadTextRegionFlags()
  {
    short[] numArray1 = new short[2];
    this.m_decoder.ReadByte(numArray1);
    this.m_textRegionFlags.setFlags(this.m_bitOperation.GetInt16(numArray1));
    if (this.m_textRegionFlags.GetFlagValue("SB_HUFF") != 0)
    {
      short[] numArray2 = new short[2];
      this.m_decoder.ReadByte(numArray2);
      this.m_textRegionHuffmanFlags.setFlags(this.m_bitOperation.GetInt16(numArray2));
    }
    bool flag = this.m_textRegionFlags.GetFlagValue("SB_REFINE") != 0;
    int flagValue = this.m_textRegionFlags.GetFlagValue("SB_R_TEMPLATE");
    if (!flag || flagValue != 0)
      return;
    this.m_symbolRegionAdaptiveTemplateX[0] = this.ReadAtValue();
    this.symbolRegionAdaptiveTemplateY[0] = this.ReadAtValue();
    this.m_symbolRegionAdaptiveTemplateX[1] = this.ReadAtValue();
    this.symbolRegionAdaptiveTemplateY[1] = this.ReadAtValue();
  }
}
