// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.HeaderFooterStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class HeaderFooterStyle
{
  private HeaderFooterProperties m_headerFooterProperties;
  private bool m_isHeader;
  private double m_headerDistance;
  private double m_footerDistance;

  internal bool IsHeader
  {
    get => this.m_isHeader;
    set => this.m_isHeader = value;
  }

  internal HeaderFooterProperties HeaderFooterproperties
  {
    get
    {
      if (this.m_headerFooterProperties == null)
        this.m_headerFooterProperties = new HeaderFooterProperties();
      return this.m_headerFooterProperties;
    }
    set => this.m_headerFooterProperties = value;
  }

  internal double HeaderDistance
  {
    get => this.m_headerDistance;
    set => this.m_headerDistance = value;
  }

  internal double FooterDistance
  {
    get => this.m_footerDistance;
    set => this.m_footerDistance = value;
  }

  internal void Dispose()
  {
    if (this.m_headerFooterProperties == null)
      return;
    this.m_headerFooterProperties.Dispose();
    this.m_headerFooterProperties = (HeaderFooterProperties) null;
  }
}
