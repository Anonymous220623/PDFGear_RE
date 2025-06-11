// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation.Text
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.ODF.Base.ODFImplementation;
using Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation.Styles;

#nullable disable
namespace Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation;

internal class Text
{
  private OParagraph m_paragraph;
  private Heading m_heading;
  private List m_list;
  private NumberedParagraph m_numberedParagraph;
  private Section m_section;
  private OTableOfContent m_tableOfContent;
  private bool m_isSoftPageBreak;
  private OParagraphCollection m_paraItem;

  internal OParagraphCollection ParagraphItem
  {
    get
    {
      if (this.m_paraItem == null)
        this.m_paraItem = new OParagraphCollection();
      return this.m_paraItem;
    }
    set => this.m_paraItem = value;
  }

  internal bool IsSoftPageBreak
  {
    get => this.m_isSoftPageBreak;
    set => this.m_isSoftPageBreak = value;
  }

  internal OTableOfContent TableOfContent
  {
    get => this.m_tableOfContent;
    set => this.m_tableOfContent = value;
  }

  internal Section Section
  {
    get => this.m_section;
    set => this.m_section = value;
  }

  internal NumberedParagraph NumberedParagraph
  {
    get => this.m_numberedParagraph;
    set => this.m_numberedParagraph = value;
  }

  internal List List
  {
    get => this.m_list;
    set => this.m_list = value;
  }

  internal Heading Heading
  {
    get => this.m_heading;
    set => this.m_heading = value;
  }

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
}
