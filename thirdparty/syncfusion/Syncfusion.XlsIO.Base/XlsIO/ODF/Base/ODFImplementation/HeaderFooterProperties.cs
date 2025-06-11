// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.HeaderFooterProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
