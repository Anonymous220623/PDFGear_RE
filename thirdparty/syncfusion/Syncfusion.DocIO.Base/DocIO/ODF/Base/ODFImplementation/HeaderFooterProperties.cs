// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.HeaderFooterProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class HeaderFooterProperties : MarginBorderProperties
{
  private double m_minHeight;
  private List<OTextBodyItem> m_textBodyItem;

  internal List<OTextBodyItem> TextBodyItems
  {
    get
    {
      if (this.m_textBodyItem == null)
        this.m_textBodyItem = new List<OTextBodyItem>();
      return this.m_textBodyItem;
    }
    set => this.m_textBodyItem = value;
  }

  internal double MinHeight
  {
    get => this.m_minHeight;
    set => this.m_minHeight = value;
  }

  internal new void Dispose() => base.Dispose();
}
