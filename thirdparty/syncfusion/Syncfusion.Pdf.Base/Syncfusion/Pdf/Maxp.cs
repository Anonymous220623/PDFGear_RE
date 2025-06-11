// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Maxp
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class Maxp(FontFile2 fontsource) : TableBase(fontsource)
{
  private int m_id = 1;
  private float m_version;
  private ushort m_numGlyphs;

  internal override int Id => this.m_id;

  public float Version
  {
    get => this.m_version;
    private set => this.m_version = value;
  }

  public ushort NumGlyphs
  {
    get => this.m_numGlyphs;
    private set => this.m_numGlyphs = value;
  }

  public override void Read(ReadFontArray reader)
  {
    this.m_version = (float) reader.getnextshort();
    this.m_version = this.Version + (float) ((int) reader.getnextUshort() / 65536 /*0x010000*/);
    this.m_numGlyphs = reader.getnextUshort();
  }
}
