// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.DefaultPageLayout
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class DefaultPageLayout
{
  private HeaderFooterStyle m_headerStyle;
  private HeaderFooterStyle m_footerStyle;
  private PageLayoutProperties m_pageLayoutProperties;

  internal HeaderFooterStyle HeaderStyle
  {
    get
    {
      if (this.m_headerStyle == null)
        this.m_headerStyle = new HeaderFooterStyle();
      return this.m_headerStyle;
    }
    set => this.m_headerStyle = value;
  }

  internal HeaderFooterStyle FooterStyle
  {
    get
    {
      if (this.m_footerStyle == null)
        this.m_footerStyle = new HeaderFooterStyle();
      return this.m_footerStyle;
    }
    set => this.m_footerStyle = value;
  }

  internal PageLayoutProperties PageLayoutProperties
  {
    get
    {
      if (this.m_pageLayoutProperties == null)
        this.m_pageLayoutProperties = new PageLayoutProperties();
      return this.m_pageLayoutProperties;
    }
    set => this.m_pageLayoutProperties = value;
  }

  internal void Dispose()
  {
    if (this.m_headerStyle != null)
    {
      this.m_headerStyle.Dispose();
      this.m_headerStyle = (HeaderFooterStyle) null;
    }
    if (this.m_footerStyle != null)
    {
      this.m_footerStyle.Dispose();
      this.m_footerStyle = (HeaderFooterStyle) null;
    }
    if (this.m_pageLayoutProperties == null)
      return;
    this.m_pageLayoutProperties.Dispose();
    this.m_pageLayoutProperties = (PageLayoutProperties) null;
  }
}
