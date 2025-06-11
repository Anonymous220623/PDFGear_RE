// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JBIG2BaseSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class JBIG2BaseSegment : JBIG2Segment
{
  protected internal int regionBitmapWidth;
  protected internal int regionBitmapHeight;
  protected internal int regionBitmapXLocation;
  protected internal int regionBitmapYLocation;
  protected internal RegionFlags regionFlags = new RegionFlags();
  private BitOperation m_bitOperation = new BitOperation();

  public JBIG2BaseSegment(JBIG2StreamDecoder streamDecoder)
    : base(streamDecoder)
  {
  }

  public override void readSegment()
  {
    short[] numArray1 = new short[4];
    this.m_decoder.ReadByte(numArray1);
    this.regionBitmapWidth = this.m_bitOperation.GetInt32(numArray1);
    short[] numArray2 = new short[4];
    this.m_decoder.ReadByte(numArray2);
    this.regionBitmapHeight = this.m_bitOperation.GetInt32(numArray2);
    short[] numArray3 = new short[4];
    this.m_decoder.ReadByte(numArray3);
    this.regionBitmapXLocation = this.m_bitOperation.GetInt32(numArray3);
    short[] numArray4 = new short[4];
    this.m_decoder.ReadByte(numArray4);
    this.regionBitmapYLocation = this.m_bitOperation.GetInt32(numArray4);
    this.regionFlags.setFlags((int) this.m_decoder.ReadByte());
  }
}
