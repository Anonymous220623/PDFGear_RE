// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.JHUFF_TBL
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class JHUFF_TBL
{
  private readonly byte[] m_bits = new byte[17];
  private readonly byte[] m_huffval = new byte[256 /*0x0100*/];
  private bool m_sent_table;

  internal JHUFF_TBL()
  {
  }

  internal byte[] Bits => this.m_bits;

  internal byte[] Huffval => this.m_huffval;

  public bool Sent_table
  {
    get => this.m_sent_table;
    set => this.m_sent_table = value;
  }
}
