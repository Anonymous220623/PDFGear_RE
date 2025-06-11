// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JBIG2StreamDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class JBIG2StreamDecoder
{
  private ArithmeticDecoder m_arithmeticDecoder;
  private IList m_bitmaps = (IList) new List<object>();
  private byte[] m_globalData;
  private HuffmanDecoder m_huffmanDecoder;
  private MMRDecoder m_mmrDecoder;
  private int m_noOfPages = -1;
  private bool m_noOfPagesKnown;
  private bool m_randomAccessOrganisation;
  private Jbig2StreamReader m_reader;
  private IList m_segments = (IList) new List<object>();
  private BitOperation m_bitOperation = new BitOperation();

  internal ArithmeticDecoder ArithDecoder => this.m_arithmeticDecoder;

  internal HuffmanDecoder HuffDecoder => this.m_huffmanDecoder;

  internal MMRDecoder MmrDecoder => this.m_mmrDecoder;

  internal void appendBitmap(JBIG2Image bitmap) => this.m_bitmaps.Add((object) bitmap);

  private bool CheckHeader()
  {
    short[] objA = new short[8]
    {
      (short) 151,
      (short) 74,
      (short) 66,
      (short) 50,
      (short) 13,
      (short) 10,
      (short) 26,
      (short) 10
    };
    short[] numArray = new short[8];
    this.m_reader.ReadByte(numArray);
    return object.Equals((object) objA, (object) numArray);
  }

  internal void ConsumeRemainingBits() => this.m_reader.ConsumeRemainingBits();

  internal void DecodeJBIG2(byte[] data)
  {
    this.m_reader = new Jbig2StreamReader(data);
    this.ResetDecoder();
    if (!this.CheckHeader())
    {
      this.m_noOfPagesKnown = true;
      this.m_randomAccessOrganisation = false;
      this.m_noOfPages = 1;
      if (this.m_globalData != null)
      {
        this.m_reader = new Jbig2StreamReader(this.m_globalData);
        this.m_huffmanDecoder = new HuffmanDecoder(this.m_reader);
        this.m_mmrDecoder = new MMRDecoder(this.m_reader);
        this.m_arithmeticDecoder = new ArithmeticDecoder(this.m_reader);
        this.ReadSegments();
        this.m_reader = new Jbig2StreamReader(data);
      }
      else
        this.m_reader.MovePointer(-8);
    }
    else
    {
      this.SetFileHeaderFlags();
      if (this.m_noOfPagesKnown)
        this.m_noOfPages = this.m_noOfPages;
    }
    this.m_huffmanDecoder = new HuffmanDecoder(this.m_reader);
    this.m_mmrDecoder = new MMRDecoder(this.m_reader);
    this.m_arithmeticDecoder = new ArithmeticDecoder(this.m_reader);
    this.ReadSegments();
  }

  internal JBIG2Image FindBitmap(int bitmapNumber)
  {
    foreach (JBIG2Image bitmap in (IEnumerable) this.m_bitmaps)
    {
      if (bitmap.BitmapNumber == bitmapNumber)
        return bitmap;
    }
    return (JBIG2Image) null;
  }

  internal PageInformationSegment FindPageSegement(int page)
  {
    foreach (JBIG2Segment segment in (IEnumerable) this.m_segments)
    {
      SegmentHeader segmentHeader = segment.m_segmentHeader;
      if (segmentHeader.SegmentType == 48 /*0x30*/ && segmentHeader.PageAssociation == page)
        return (PageInformationSegment) segment;
    }
    return (PageInformationSegment) null;
  }

  internal JBIG2Segment FindSegment(int segmentNumber)
  {
    foreach (JBIG2Segment segment in (IEnumerable) this.m_segments)
    {
      if (segment.m_segmentHeader.SegmentNumber == segmentNumber)
        return segment;
    }
    return (JBIG2Segment) null;
  }

  private int getnoOfPages()
  {
    short[] numArray = new short[4];
    this.m_reader.ReadByte(numArray);
    return this.m_bitOperation.GetInt32(numArray);
  }

  internal JBIG2Image GetPageAsJBIG2Bitmap(int i) => this.FindPageSegement(1).pageBitmap;

  private void HandlePageAssociation(SegmentHeader segmentHeader)
  {
    int num;
    if (segmentHeader.IsPageAssociationSizeSet)
    {
      short[] numArray = new short[4];
      this.m_reader.ReadByte(numArray);
      num = this.m_bitOperation.GetInt32(numArray);
    }
    else
      num = (int) this.m_reader.ReadByte();
    segmentHeader.PageAssociation = num;
  }

  private void HandleReferedToSegmentNumbers(SegmentHeader segmentHeader)
  {
    int referedToSegCount = segmentHeader.ReferedToSegCount;
    int[] numArray1 = new int[referedToSegCount];
    int segmentNumber = segmentHeader.SegmentNumber;
    if (segmentNumber <= 256 /*0x0100*/)
    {
      for (int index = 0; index < referedToSegCount; ++index)
        numArray1[index] = (int) this.m_reader.ReadByte();
    }
    else if (segmentNumber <= 65536 /*0x010000*/)
    {
      short[] numArray2 = new short[2];
      for (int index = 0; index < referedToSegCount; ++index)
      {
        this.m_reader.ReadByte(numArray2);
        numArray1[index] = this.m_bitOperation.GetInt16(numArray2);
      }
    }
    else
    {
      short[] numArray3 = new short[4];
      for (int index = 0; index < referedToSegCount; ++index)
      {
        this.m_reader.ReadByte(numArray3);
        numArray1[index] = this.m_bitOperation.GetInt32(numArray3);
      }
    }
    segmentHeader.ReferredToSegments = numArray1;
  }

  private void HandleSegmentDataLength(SegmentHeader segmentHeader)
  {
    short[] numArray = new short[4];
    this.m_reader.ReadByte(numArray);
    int int32 = this.m_bitOperation.GetInt32(numArray);
    segmentHeader.DataLength = int32;
  }

  private void HandleSegmentHeaderFlags(SegmentHeader segmentHeader)
  {
    short SegmentHeaderFlags = this.m_reader.ReadByte();
    segmentHeader.SetSegmentHeaderFlags(SegmentHeaderFlags);
  }

  private void HandleSegmentNumber(SegmentHeader segmentHeader)
  {
    short[] numArray = new short[4];
    this.m_reader.ReadByte(numArray);
    int int32 = this.m_bitOperation.GetInt32(numArray);
    segmentHeader.SegmentNumber = int32;
  }

  private void HandleSegmentReferredToCountAndRententionFlags(SegmentHeader segmentHeader)
  {
    short num1 = this.m_reader.ReadByte();
    int num2 = ((int) num1 & 224 /*0xE0*/) >> 5;
    short[] buf = (short[]) null;
    short num3 = (short) ((int) num1 & 31 /*0x1F*/);
    if (num2 <= 4)
      buf = new short[1]{ num3 };
    else if (num2 == 7)
    {
      short[] number = new short[4]
      {
        num3,
        (short) 0,
        (short) 0,
        (short) 0
      };
      for (int index = 1; index < 4; ++index)
        number[index] = this.m_reader.ReadByte();
      num2 = this.m_bitOperation.GetInt32(number);
      buf = new short[(int) Math.Ceiling(4.0 + (double) (num2 + 1) / 8.0) - 4];
      this.m_reader.ReadByte(buf);
    }
    segmentHeader.ReferedToSegCount = num2;
    segmentHeader.RententionFlags = buf;
  }

  internal void MovePointer(int i) => this.m_reader.MovePointer(i);

  internal int ReadBit() => this.m_reader.ReadBit();

  internal int ReadBits(int num) => this.m_reader.ReadBits(num);

  internal short ReadByte() => this.m_reader.ReadByte();

  internal void ReadByte(short[] buff) => this.m_reader.ReadByte(buff);

  private void ReadSegmentHeader(SegmentHeader segmentHeader)
  {
    this.HandleSegmentNumber(segmentHeader);
    this.HandleSegmentHeaderFlags(segmentHeader);
    this.HandleSegmentReferredToCountAndRententionFlags(segmentHeader);
    this.HandleReferedToSegmentNumbers(segmentHeader);
    this.HandlePageAssociation(segmentHeader);
    if (segmentHeader.SegmentType == 51)
      return;
    this.HandleSegmentDataLength(segmentHeader);
  }

  private void ReadSegments()
  {
    bool flag = false;
    while (!this.m_reader.Getfinished() && !flag)
    {
      SegmentHeader segmentHeader = new SegmentHeader();
      this.ReadSegmentHeader(segmentHeader);
      JBIG2Segment jbiG2Segment = (JBIG2Segment) null;
      int segmentType = segmentHeader.SegmentType;
      int[] referredToSegments = segmentHeader.ReferredToSegments;
      int referedToSegCount = segmentHeader.ReferedToSegCount;
      switch (segmentType)
      {
        case 0:
          jbiG2Segment = (JBIG2Segment) new SymbolDictionarySegment(this);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 4:
          jbiG2Segment = (JBIG2Segment) new TextRegionSegment(this, false);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 6:
          jbiG2Segment = (JBIG2Segment) new TextRegionSegment(this, true);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 7:
          jbiG2Segment = (JBIG2Segment) new TextRegionSegment(this, true);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 16 /*0x10*/:
          jbiG2Segment = (JBIG2Segment) new PatternDictionarySegment(this);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 20:
          jbiG2Segment = (JBIG2Segment) new HalftoneRegionSegment(this, false);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 22:
          jbiG2Segment = (JBIG2Segment) new HalftoneRegionSegment(this, true);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 23:
          jbiG2Segment = (JBIG2Segment) new HalftoneRegionSegment(this, true);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 36:
          jbiG2Segment = (JBIG2Segment) new GenericRegionSegment(this, false);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 38:
          jbiG2Segment = (JBIG2Segment) new GenericRegionSegment(this, true);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 39:
          jbiG2Segment = (JBIG2Segment) new GenericRegionSegment(this, true);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 40:
          jbiG2Segment = (JBIG2Segment) new RefinementRegionSegment(this, false, referredToSegments, referedToSegCount);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 42:
          jbiG2Segment = (JBIG2Segment) new RefinementRegionSegment(this, true, referredToSegments, referedToSegCount);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 43:
          jbiG2Segment = (JBIG2Segment) new RefinementRegionSegment(this, true, referredToSegments, referedToSegCount);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 48 /*0x30*/:
          jbiG2Segment = (JBIG2Segment) new PageInformationSegment(this);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 49:
          continue;
        case 50:
          jbiG2Segment = (JBIG2Segment) new EndOfStripeSegment(this);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
        case 51:
          flag = true;
          continue;
        case 62:
          jbiG2Segment = (JBIG2Segment) new ExtensionSegment(this);
          jbiG2Segment.m_segmentHeader = segmentHeader;
          break;
      }
      if (!this.m_randomAccessOrganisation && jbiG2Segment != null)
        jbiG2Segment.readSegment();
      this.m_segments.Add((object) jbiG2Segment);
    }
    if (!this.m_randomAccessOrganisation)
      return;
    foreach (JBIG2Segment segment in (IEnumerable) this.m_segments)
      segment.readSegment();
  }

  private void ResetDecoder()
  {
    this.m_noOfPagesKnown = false;
    this.m_randomAccessOrganisation = false;
    this.m_noOfPages = -1;
    this.m_segments.Clear();
    this.m_bitmaps.Clear();
  }

  private void SetFileHeaderFlags()
  {
    short num1 = this.m_reader.ReadByte();
    int num2 = (int) num1 & 252;
    this.m_randomAccessOrganisation = ((int) num1 & 1) == 0;
    this.m_noOfPagesKnown = ((int) num1 & 2) == 0;
  }

  internal IList AllSegments => this.m_segments;

  internal byte[] GlobalData
  {
    set => this.m_globalData = value;
  }

  internal int NumberOfPages => this.m_noOfPages;

  internal bool NumberOfPagesKnown => this.m_noOfPagesKnown;

  internal bool RandomAccessOrganisationUsed => this.m_randomAccessOrganisation;
}
