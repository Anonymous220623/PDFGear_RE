// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.ListHeader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ODF.Base.ODFImplementation;
using Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.Styles;

#nullable disable
namespace Syncfusion.DocIO.ODFConverter.Base.ODFImplementation;

internal class ListHeader
{
  private Heading m_heading;
  private List m_list;
  private ODFParagraphProperties m_paragraph;

  internal ODFParagraphProperties Paragraph
  {
    get => this.m_paragraph;
    set => this.m_paragraph = value;
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
}
