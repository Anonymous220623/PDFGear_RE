// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OParagraphCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

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
