// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.AutomaticStyles
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class AutomaticStyles : CommonStyles
{
  private PageLayoutCollection m_pageLayout;

  internal PageLayoutCollection PageLayoutCollection
  {
    get
    {
      if (this.m_pageLayout == null)
        this.m_pageLayout = new PageLayoutCollection();
      return this.m_pageLayout;
    }
    set => this.m_pageLayout = value;
  }
}
