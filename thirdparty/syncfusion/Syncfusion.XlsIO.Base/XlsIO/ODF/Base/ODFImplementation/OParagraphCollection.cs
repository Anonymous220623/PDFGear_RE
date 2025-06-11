// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.OParagraphCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class OParagraphCollection
{
  private List<OParagraph> m_Paragraph;

  internal List<OParagraph> Paragraph
  {
    get
    {
      if (this.m_Paragraph == null)
        this.m_Paragraph = new List<OParagraph>();
      return this.m_Paragraph;
    }
    set => this.m_Paragraph = value;
  }
}
