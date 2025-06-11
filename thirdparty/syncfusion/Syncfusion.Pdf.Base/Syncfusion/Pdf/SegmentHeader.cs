// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SegmentHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class SegmentHeader
{
  private int m_segmentNumber;
  private int m_segmentType;
  private bool m_pageAssociationSizeSet;
  private bool m_deferredNonRetainSet;
  private int m_referredToSegmentCount;
  private short[] m_rententionFlags;
  private int[] m_referredToSegments;
  private int m_pageAssociation;
  private int m_dataLength;

  internal int SegmentNumber
  {
    get => this.m_segmentNumber;
    set => this.m_segmentNumber = value;
  }

  internal int SegmentType
  {
    get => this.m_segmentType;
    set => this.m_segmentType = value;
  }

  internal bool IsPageAssociationSizeSet => this.m_pageAssociationSizeSet;

  internal int ReferedToSegCount
  {
    get => this.m_referredToSegmentCount;
    set => this.m_referredToSegmentCount = value;
  }

  internal int DataLength
  {
    get => this.m_dataLength;
    set => this.m_dataLength = value;
  }

  internal int PageAssociation
  {
    get => this.m_pageAssociation;
    set => this.m_pageAssociation = value;
  }

  internal short[] RententionFlags
  {
    get => this.m_rententionFlags;
    set => this.m_rententionFlags = value;
  }

  internal bool IsDeferredNonRetainSet => this.m_deferredNonRetainSet;

  internal int[] ReferredToSegments
  {
    get => this.m_referredToSegments;
    set => this.m_referredToSegments = value;
  }

  public void SetSegmentHeaderFlags(short SegmentHeaderFlags)
  {
    this.m_segmentType = (int) SegmentHeaderFlags & 63 /*0x3F*/;
    this.m_pageAssociationSizeSet = ((int) SegmentHeaderFlags & 64 /*0x40*/) == 64 /*0x40*/;
    this.m_deferredNonRetainSet = ((int) SegmentHeaderFlags & 80 /*0x50*/) == 80 /*0x50*/;
  }
}
