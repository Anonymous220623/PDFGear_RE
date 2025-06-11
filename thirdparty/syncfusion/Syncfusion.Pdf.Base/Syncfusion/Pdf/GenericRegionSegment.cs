// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.GenericRegionSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class GenericRegionSegment : JBIG2BaseSegment
{
  private GenericRegionFlags m_genericRegionFlags = new GenericRegionFlags();
  private bool m_inlineImage;
  private bool m_unknownLength;

  public GenericRegionSegment(JBIG2StreamDecoder streamDecoder, bool inlineImage)
    : base(streamDecoder)
  {
    this.m_inlineImage = inlineImage;
  }

  public override void readSegment()
  {
    base.readSegment();
    this.ReadGenericRegionFlags();
    bool useMMR = this.m_genericRegionFlags.GetFlagValue("MMR") != 0;
    int flagValue1 = this.m_genericRegionFlags.GetFlagValue("GB_TEMPLATE");
    short[] adaptiveTemplateX = new short[4];
    short[] adaptiveTemplateY = new short[4];
    if (!useMMR)
    {
      if (flagValue1 == 0)
      {
        adaptiveTemplateX[0] = this.ReadAtValue();
        adaptiveTemplateY[0] = this.ReadAtValue();
        adaptiveTemplateX[1] = this.ReadAtValue();
        adaptiveTemplateY[1] = this.ReadAtValue();
        adaptiveTemplateX[2] = this.ReadAtValue();
        adaptiveTemplateY[2] = this.ReadAtValue();
        adaptiveTemplateX[3] = this.ReadAtValue();
        adaptiveTemplateY[3] = this.ReadAtValue();
      }
      else
      {
        adaptiveTemplateX[0] = this.ReadAtValue();
        adaptiveTemplateY[0] = this.ReadAtValue();
      }
      this.m_arithmeticDecoder.ResetGenericStats(flagValue1, (ArithmeticDecoderStats) null);
      this.m_arithmeticDecoder.Start();
    }
    bool typicalPredictionGenericDecodingOn = this.m_genericRegionFlags.GetFlagValue("TPGDON") != 0;
    int num1 = this.m_segmentHeader.DataLength;
    if (num1 == -1)
    {
      this.m_unknownLength = true;
      short num2;
      short num3;
      if (useMMR)
      {
        num2 = (short) 0;
        num3 = (short) 0;
      }
      else
      {
        num2 = (short) byte.MaxValue;
        num3 = (short) 172;
      }
      int num4 = 0;
      short num5;
      do
      {
        short num6;
        do
        {
          num6 = this.m_decoder.ReadByte();
          ++num4;
        }
        while ((int) num6 != (int) num2);
        num5 = this.m_decoder.ReadByte();
        ++num4;
      }
      while ((int) num5 != (int) num3);
      num1 = num4 - 2;
      this.m_decoder.MovePointer(-num4);
    }
    JBIG2Image bitmap = new JBIG2Image(this.regionBitmapWidth, this.regionBitmapHeight, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
    bitmap.Clear(0);
    bitmap.ReadBitmap(useMMR, flagValue1, typicalPredictionGenericDecodingOn, false, (JBIG2Image) null, adaptiveTemplateX, adaptiveTemplateY, useMMR ? 0 : num1 - 18);
    if (this.m_inlineImage)
    {
      PageInformationSegment pageSegement = this.m_decoder.FindPageSegement(this.m_segmentHeader.PageAssociation);
      JBIG2Image pageBitmap = pageSegement.pageBitmap;
      int flagValue2 = this.regionFlags.GetFlagValue("EXTERNAL_COMBINATION_OPERATOR");
      if (pageSegement.pageBitmapHeight == -1 && this.regionBitmapYLocation + this.regionBitmapHeight > pageBitmap.Height)
        pageBitmap.Expand(this.regionBitmapYLocation + this.regionBitmapHeight, pageSegement.pageInformationFlags.GetFlagValue("DEFAULT_PIXEL_VALUE"));
      pageBitmap.Combine(bitmap, this.regionBitmapXLocation, this.regionBitmapYLocation, (long) flagValue2);
    }
    else
    {
      bitmap.BitmapNumber = this.m_segmentHeader.SegmentNumber;
      this.m_decoder.appendBitmap(bitmap);
    }
    if (!this.m_unknownLength)
      return;
    this.m_decoder.MovePointer(4);
  }

  private void ReadGenericRegionFlags()
  {
    this.m_genericRegionFlags.setFlags((int) this.m_decoder.ReadByte());
  }
}
