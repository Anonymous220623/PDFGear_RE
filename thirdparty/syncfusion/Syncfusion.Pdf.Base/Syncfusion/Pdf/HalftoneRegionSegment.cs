// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HalftoneRegionSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class HalftoneRegionSegment : JBIG2BaseSegment
{
  private HalftoneRegionFlags m_halftoneRegionFlags = new HalftoneRegionFlags();
  private BitOperation m_bitOperation = new BitOperation();
  private bool inlineImage;

  internal HalftoneRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage)
    : base(streamDecoder)
  {
    this.inlineImage = inlineImage;
  }

  public override void readSegment()
  {
    base.readSegment();
    this.ReadHalftoneRegionFlags();
    short[] numArray1 = new short[4];
    this.m_decoder.ReadByte(numArray1);
    int int32_1 = this.m_bitOperation.GetInt32(numArray1);
    short[] numArray2 = new short[4];
    this.m_decoder.ReadByte(numArray2);
    int int32_2 = this.m_bitOperation.GetInt32(numArray2);
    short[] numArray3 = new short[4];
    this.m_decoder.ReadByte(numArray3);
    int int32_3 = this.m_bitOperation.GetInt32(numArray3);
    short[] numArray4 = new short[4];
    this.m_decoder.ReadByte(numArray4);
    int int32_4 = this.m_bitOperation.GetInt32(numArray4);
    short[] numArray5 = new short[2];
    this.m_decoder.ReadByte(numArray5);
    int int16_1 = this.m_bitOperation.GetInt16(numArray5);
    short[] numArray6 = new short[2];
    this.m_decoder.ReadByte(numArray6);
    int int16_2 = this.m_bitOperation.GetInt16(numArray6);
    PatternDictionarySegment segment = (PatternDictionarySegment) this.m_decoder.FindSegment(this.m_segmentHeader.ReferredToSegments[0]);
    int num1 = 0;
    for (int index = 1; index < segment.Size; index <<= 1)
      ++num1;
    JBIG2Image bitmap1 = segment.GetBitmaps()[0];
    int width = bitmap1.Width;
    int height = bitmap1.Height;
    bool useMMR = this.m_halftoneRegionFlags.GetFlagValue("H_MMR") != 0;
    int flagValue1 = this.m_halftoneRegionFlags.GetFlagValue("H_TEMPLATE");
    if (!useMMR)
    {
      this.m_arithmeticDecoder.ResetGenericStats(flagValue1, (ArithmeticDecoderStats) null);
      this.m_arithmeticDecoder.Start();
    }
    int flagValue2 = this.m_halftoneRegionFlags.GetFlagValue("H_DEF_PIXEL");
    JBIG2Image bitmap2 = new JBIG2Image(this.regionBitmapWidth, this.regionBitmapHeight, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
    bitmap2.Clear(flagValue2);
    bool useSkip = this.m_halftoneRegionFlags.GetFlagValue("H_ENABLE_SKIP") != 0;
    JBIG2Image skipBitmap = (JBIG2Image) null;
    if (useSkip)
    {
      skipBitmap = new JBIG2Image(int32_1, int32_2, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
      skipBitmap.Clear(0);
      for (int col = 0; col < int32_2; ++col)
      {
        for (int row = 0; row < int32_1; ++row)
        {
          int num2 = int32_3 + col * int16_2 + row * int16_1;
          int num3 = int32_4 + col * int16_1 - row * int16_2;
          if (num2 + width >> 8 <= 0 || num2 >> 8 >= this.regionBitmapWidth || num3 + height >> 8 <= 0 || num3 >> 8 >= this.regionBitmapHeight)
            skipBitmap.SetPixel(col, row, 1);
        }
      }
    }
    int[] numArray7 = new int[int32_1 * int32_2];
    short[] adaptiveTemplateX = new short[4];
    short[] adaptiveTemplateY = new short[4];
    adaptiveTemplateX[0] = flagValue1 <= 1 ? (short) 3 : (short) 2;
    adaptiveTemplateY[0] = (short) -1;
    adaptiveTemplateX[1] = (short) -3;
    adaptiveTemplateY[1] = (short) -1;
    adaptiveTemplateX[2] = (short) 2;
    adaptiveTemplateY[2] = (short) -2;
    adaptiveTemplateX[3] = (short) -2;
    adaptiveTemplateY[3] = (short) -2;
    for (int index1 = num1 - 1; index1 >= 0; --index1)
    {
      JBIG2Image jbiG2Image = new JBIG2Image(int32_1, int32_2, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
      jbiG2Image.ReadBitmap(useMMR, flagValue1, false, useSkip, skipBitmap, adaptiveTemplateX, adaptiveTemplateY, -1);
      int index2 = 0;
      for (int row = 0; row < int32_2; ++row)
      {
        for (int col = 0; col < int32_1; ++col)
        {
          int num4 = jbiG2Image.GetPixel(col, row) ^ numArray7[index2] & 1;
          numArray7[index2] = numArray7[index2] << 1 | num4;
          ++index2;
        }
      }
    }
    int flagValue3 = this.m_halftoneRegionFlags.GetFlagValue("H_COMB_OP");
    int index3 = 0;
    for (int col = 0; col < int32_2; ++col)
    {
      int num5 = int32_3 + col * int16_2;
      int num6 = int32_4 + col * int16_1;
      for (int row = 0; row < int32_1; ++row)
      {
        if (!useSkip || skipBitmap.GetPixel(col, row) != 1)
        {
          JBIG2Image bitmap3 = segment.GetBitmaps()[numArray7[index3]];
          bitmap2.Combine(bitmap3, num5 >> 8, num6 >> 8, (long) flagValue3);
        }
        num5 += int16_1;
        num6 -= int16_2;
        ++index3;
      }
    }
    if (this.inlineImage)
    {
      JBIG2Image pageBitmap = this.m_decoder.FindPageSegement(this.m_segmentHeader.PageAssociation).pageBitmap;
      int flagValue4 = this.regionFlags.GetFlagValue("EXTERNAL_COMBINATION_OPERATOR");
      pageBitmap.Combine(bitmap2, this.regionBitmapXLocation, this.regionBitmapYLocation, (long) flagValue4);
    }
    else
    {
      bitmap2.BitmapNumber = this.m_segmentHeader.SegmentNumber;
      this.m_decoder.appendBitmap(bitmap2);
    }
  }

  private void ReadHalftoneRegionFlags()
  {
    this.m_halftoneRegionFlags.setFlags((int) this.m_decoder.ReadByte());
  }
}
