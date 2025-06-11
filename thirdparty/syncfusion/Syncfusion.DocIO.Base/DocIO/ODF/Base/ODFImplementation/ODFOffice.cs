// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ODFOffice
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ODFOffice
{
  private DocumentContent m_docContent;
  private DocumentStyles m_docStyles;

  internal DocumentContent DocContent
  {
    get
    {
      if (this.m_docContent == null)
        this.m_docContent = new DocumentContent();
      return this.m_docContent;
    }
    set => this.m_docContent = value;
  }

  internal DocumentStyles DocStyles
  {
    get
    {
      if (this.m_docStyles == null)
        this.m_docStyles = new DocumentStyles();
      return this.m_docStyles;
    }
    set => this.m_docStyles = value;
  }
}
