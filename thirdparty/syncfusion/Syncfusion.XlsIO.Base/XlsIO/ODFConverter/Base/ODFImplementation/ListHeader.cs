// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation.ListHeader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.ODF.Base.ODFImplementation;
using Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation.Styles;

#nullable disable
namespace Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation;

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
