// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PageInformationSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class PageInformationSegment : JBIG2Segment
{
  private int m_pageBitmapHeight;
  private int m_pageBitmapWidth;
  private int m_yResolution;
  private int m_xResolution;
  private BitOperation m_bitOperation = new BitOperation();
  private int m_pageStriping;
  private JBIG2Image m_pageBitmap;
  private PageInformationFlags m_pageInformationFlags = new PageInformationFlags();

  internal PageInformationSegment(JBIG2StreamDecoder streamDecoder)
    : base(streamDecoder)
  {
  }

  internal PageInformationFlags pageInformationFlags => this.m_pageInformationFlags;

  internal JBIG2Image pageBitmap => this.m_pageBitmap;

  public override void readSegment()
  {
    short[] numArray1 = new short[4];
    this.m_decoder.ReadByte(numArray1);
    this.m_pageBitmapWidth = this.m_bitOperation.GetInt32(numArray1);
    short[] numArray2 = new short[4];
    this.m_decoder.ReadByte(numArray2);
    this.m_pageBitmapHeight = this.m_bitOperation.GetInt32(numArray2);
    short[] numArray3 = new short[4];
    this.m_decoder.ReadByte(numArray3);
    this.m_xResolution = this.m_bitOperation.GetInt32(numArray3);
    short[] numArray4 = new short[4];
    this.m_decoder.ReadByte(numArray4);
    this.m_yResolution = this.m_bitOperation.GetInt32(numArray4);
    this.m_pageInformationFlags.setFlags((int) this.m_decoder.ReadByte());
    short[] numArray5 = new short[2];
    this.m_decoder.ReadByte(numArray5);
    this.m_pageStriping = this.m_bitOperation.GetInt16(numArray5);
    int flagValue = this.m_pageInformationFlags.GetFlagValue("DEFAULT_PIXEL_VALUE");
    this.m_pageBitmap = new JBIG2Image(this.m_pageBitmapWidth, this.m_pageBitmapHeight != -1 ? this.m_pageBitmapHeight : this.m_pageStriping & (int) short.MaxValue, this.m_arithmeticDecoder, this.m_huffmanDecoder, this.m_mmrDecoder);
    this.m_pageBitmap.Clear(flagValue);
  }

  internal int pageBitmapHeight => this.m_pageBitmapHeight;
}
