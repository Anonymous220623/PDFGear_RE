// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.DumpModeCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class DumpModeCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name) : 
  TiffCodec(tif, scheme, name)
{
  public override bool Init() => true;

  public override bool CanDecode => true;

  public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    return this.DumpModeDecode(buffer, offset, count, plane);
  }

  public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.DumpModeDecode(buffer, offset, count, plane);
  }

  public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    return this.DumpModeDecode(buffer, offset, count, plane);
  }

  public override bool Seek(int row)
  {
    this.m_tif.m_rawcp += row * this.m_tif.m_scanlinesize;
    this.m_tif.m_rawcc -= row * this.m_tif.m_scanlinesize;
    return true;
  }

  private bool DumpModeDecode(byte[] buffer, int offset, int count, short plane)
  {
    if (this.m_tif.m_rawcc < count)
      return false;
    Buffer.BlockCopy((Array) this.m_tif.m_rawdata, this.m_tif.m_rawcp, (Array) buffer, offset, count);
    this.m_tif.m_rawcp += count;
    this.m_tif.m_rawcc -= count;
    return true;
  }
}
