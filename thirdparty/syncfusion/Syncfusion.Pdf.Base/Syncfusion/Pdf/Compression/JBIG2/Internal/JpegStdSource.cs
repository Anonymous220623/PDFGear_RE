// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.JpegStdSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class JpegStdSource : SourceMgr
{
  private static readonly byte[] dummy_EOI = new byte[2]
  {
    byte.MaxValue,
    (byte) 217
  };
  protected JpegCodec m_sp;

  public JpegStdSource(JpegCodec sp)
  {
    this.initInternalBuffer((byte[]) null, 0);
    this.m_sp = sp;
  }

  public override void init_source()
  {
    Tiff tiff = this.m_sp.GetTiff();
    this.initInternalBuffer(tiff.m_rawdata, tiff.m_rawcc);
  }

  public override bool fill_input_buffer()
  {
    this.initInternalBuffer(JpegStdSource.dummy_EOI, 2);
    return true;
  }
}
