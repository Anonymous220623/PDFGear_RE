// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.EndOfStripeSegment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class EndOfStripeSegment(JBIG2StreamDecoder streamDecoder) : JBIG2Segment(streamDecoder)
{
  public override void readSegment()
  {
    for (int index = 0; index < this.m_segmentHeader.DataLength; ++index)
    {
      int num = (int) this.m_decoder.ReadByte();
    }
  }
}
