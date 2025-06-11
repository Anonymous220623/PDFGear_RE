// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.OJpegSrcManager
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class OJpegSrcManager : SourceMgr
{
  protected OJpegCodec m_sp;

  public OJpegSrcManager(OJpegCodec sp)
  {
    this.initInternalBuffer((byte[]) null, 0);
    this.m_sp = sp;
  }

  public override void init_source()
  {
  }

  public override bool fill_input_buffer()
  {
    this.m_sp.GetTiff();
    byte[] mem = (byte[]) null;
    uint len = 0;
    this.m_sp.OJPEGWriteStream(out mem, out len);
    this.initInternalBuffer(mem, (int) len);
    return true;
  }

  public override void skip_input_data(int num_bytes) => this.m_sp.GetTiff();

  public override bool resync_to_restart(DecompressStruct cinfo, int desired)
  {
    this.m_sp.GetTiff();
    return false;
  }

  public override void term_source()
  {
  }
}
