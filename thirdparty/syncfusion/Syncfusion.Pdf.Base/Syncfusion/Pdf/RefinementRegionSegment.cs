// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.RefinementRegionSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class RefinementRegionSegment : JBIG2BaseSegment
{
  private bool m_inlineImage;
  private int m_noOfReferedToSegments;
  private int[] m_referedToSegments;

  public RefinementRegionSegment(
    JBIG2StreamDecoder streamDecoder,
    bool inlineImage,
    int[] referedToSegments,
    int noOfReferedToSegments)
    : base(streamDecoder)
  {
    this.m_inlineImage = inlineImage;
    this.m_referedToSegments = referedToSegments;
    this.m_noOfReferedToSegments = noOfReferedToSegments;
  }
}
