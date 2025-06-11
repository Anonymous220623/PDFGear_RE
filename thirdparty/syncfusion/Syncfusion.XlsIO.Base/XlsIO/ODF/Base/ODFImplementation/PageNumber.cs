// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.PageNumber
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class PageNumber
{
  private PageNumberFormat m_pageNumber;
  private bool m_numberLetterSync;
  private bool m_pageFixed;
  private int m_pageAdjust;
  private SelectPage m_selectPage;
  private string m_content;

  internal SelectPage SelectPage
  {
    get => this.m_selectPage;
    set => this.m_selectPage = value;
  }

  internal int PageAdjust
  {
    get => this.m_pageAdjust;
    set => this.m_pageAdjust = value;
  }

  internal bool PageFixed
  {
    get => this.m_pageFixed;
    set => this.m_pageFixed = value;
  }

  internal bool NumberLetterSync
  {
    get => this.m_numberLetterSync;
    set => this.m_numberLetterSync = value;
  }

  internal PageNumberFormat PgNumber
  {
    get => this.m_pageNumber;
    set => this.m_pageNumber = value;
  }

  internal string Content
  {
    get => this.m_content;
    set => this.m_content = value;
  }
}
