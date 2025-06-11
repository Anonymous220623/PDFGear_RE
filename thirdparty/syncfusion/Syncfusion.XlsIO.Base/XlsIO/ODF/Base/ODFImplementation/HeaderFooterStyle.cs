// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.HeaderFooterStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class HeaderFooterStyle
{
  private HeaderFooterProperties m_headerFooterProperties;
  private bool m_isHeader;

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

  internal void Dispose()
  {
    if (this.m_headerFooterProperties == null)
      return;
    this.m_headerFooterProperties.Dispose();
    this.m_headerFooterProperties = (HeaderFooterProperties) null;
  }
}
