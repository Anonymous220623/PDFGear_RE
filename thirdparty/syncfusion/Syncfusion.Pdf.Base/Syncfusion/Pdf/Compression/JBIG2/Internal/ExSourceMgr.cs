// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.ExSourceMgr
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class ExSourceMgr : SourceMgr
{
  private const int INPUT_BUF_SIZE = 4096 /*0x1000*/;
  private DecompressStruct m_cinfo;
  private Stream m_infile;
  private byte[] m_buffer;
  private bool m_start_of_file;

  public ExSourceMgr(DecompressStruct cinfo)
  {
    this.m_cinfo = cinfo;
    this.m_buffer = new byte[4096 /*0x1000*/];
  }

  public void Attach(Stream infile)
  {
    this.m_infile = infile;
    this.m_infile.Seek(0L, SeekOrigin.Begin);
    this.initInternalBuffer((byte[]) null, 0);
  }

  public override void init_source() => this.m_start_of_file = true;

  public override bool fill_input_buffer()
  {
    int size = this.m_infile.Read(this.m_buffer, 0, 4096 /*0x1000*/);
    if (size <= 0)
    {
      this.m_buffer[0] = byte.MaxValue;
      this.m_buffer[1] = (byte) 217;
      size = 2;
    }
    this.initInternalBuffer(this.m_buffer, size);
    this.m_start_of_file = false;
    return true;
  }
}
