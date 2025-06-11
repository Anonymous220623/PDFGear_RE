// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.MasterPage
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class MasterPage : INamedObject
{
  private string m_name;
  private string m_pageLayoutName;
  private HeaderFooterContent m_header;
  private HeaderFooterContent m_headerLeft;
  private HeaderFooterContent m_footer;
  private HeaderFooterContent m_footerLeft;
  private HeaderFooterContent m_firstPageHeader;
  private HeaderFooterContent m_firstPageFooter;

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string PageLayoutName
  {
    get => this.m_pageLayoutName;
    set => this.m_pageLayoutName = value;
  }

  internal HeaderFooterContent Header
  {
    get => this.m_header;
    set => this.m_header = value;
  }

  internal HeaderFooterContent HeaderLeft
  {
    get => this.m_headerLeft;
    set => this.m_headerLeft = value;
  }

  internal HeaderFooterContent Footer
  {
    get => this.m_footer;
    set => this.m_footer = value;
  }

  internal HeaderFooterContent FooterLeft
  {
    get => this.m_footerLeft;
    set => this.m_footerLeft = value;
  }

  internal HeaderFooterContent FirstPageHeader
  {
    get => this.m_firstPageHeader;
    set => this.m_firstPageHeader = value;
  }

  internal HeaderFooterContent FirstPageFooter
  {
    get => this.m_firstPageFooter;
    set => this.m_firstPageFooter = value;
  }

  internal void Dispose()
  {
    if (this.m_header != null)
    {
      this.m_header.Dispose();
      this.m_header = (HeaderFooterContent) null;
    }
    if (this.m_headerLeft != null)
    {
      this.m_headerLeft.Dispose();
      this.m_headerLeft = (HeaderFooterContent) null;
    }
    if (this.m_footer != null)
    {
      this.m_footer.Dispose();
      this.m_footer = (HeaderFooterContent) null;
    }
    if (this.m_footerLeft == null)
      return;
    this.m_footerLeft.Dispose();
    this.m_footerLeft = (HeaderFooterContent) null;
  }
}
