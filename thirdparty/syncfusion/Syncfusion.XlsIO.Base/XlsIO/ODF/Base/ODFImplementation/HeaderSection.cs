// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.HeaderSection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class HeaderSection
{
  private OParagraph m_paragraph;

  internal OParagraph Paragraph
  {
    get
    {
      if (this.m_paragraph == null)
        this.m_paragraph = new OParagraph();
      return this.m_paragraph;
    }
    set => this.m_paragraph = value;
  }

  internal void Dispose()
  {
    if (this.m_paragraph == null)
      return;
    this.m_paragraph.Dispose();
    this.m_paragraph = (OParagraph) null;
  }
}
