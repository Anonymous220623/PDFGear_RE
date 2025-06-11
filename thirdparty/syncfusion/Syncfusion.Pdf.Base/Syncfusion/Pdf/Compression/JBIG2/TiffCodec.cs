// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.TiffCodec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class TiffCodec
{
  protected Tiff m_tif;
  protected internal Syncfusion.Pdf.Compression.JBIG2.Compression m_scheme;
  protected internal string m_name;

  public TiffCodec(Tiff tif, Syncfusion.Pdf.Compression.JBIG2.Compression scheme, string name)
  {
    this.m_scheme = scheme;
    this.m_tif = tif;
    this.m_name = name;
  }

  public virtual bool CanDecode => false;

  public virtual bool Init() => true;

  public virtual bool SetupDecode() => true;

  public virtual bool PreDecode(short plane) => true;

  public virtual bool DecodeRow(byte[] buffer, int offset, int count, short plane)
  {
    return this.noDecode("scanline");
  }

  public virtual bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
  {
    return this.noDecode("strip");
  }

  public virtual bool DecodeTile(byte[] buffer, int offset, int count, short plane)
  {
    return this.noDecode("tile");
  }

  public virtual void Close()
  {
  }

  public virtual bool Seek(int row) => false;

  public virtual void Cleanup()
  {
  }

  public virtual int DefStripSize(int size)
  {
    if (size < 1)
    {
      int num = this.m_tif.ScanlineSize();
      size = 8192 /*0x2000*/ / (num == 0 ? 1 : num);
      if (size == 0)
        size = 1;
    }
    return size;
  }

  public virtual void DefTileSize(ref int width, ref int height)
  {
    if (width < 1)
      width = 256 /*0x0100*/;
    if (height < 1)
      height = 256 /*0x0100*/;
    if ((width & 15) != 0)
      width = Tiff.roundUp(width, 16 /*0x10*/);
    if ((height & 15) == 0)
      return;
    height = Tiff.roundUp(height, 16 /*0x10*/);
  }

  private bool noDecode(string method)
  {
    this.m_tif.FindCodec(this.m_tif.m_dir.td_compression);
    return false;
  }
}
