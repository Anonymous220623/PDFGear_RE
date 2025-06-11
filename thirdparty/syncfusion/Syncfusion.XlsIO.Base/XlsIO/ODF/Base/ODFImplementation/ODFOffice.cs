// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.ODFOffice
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
