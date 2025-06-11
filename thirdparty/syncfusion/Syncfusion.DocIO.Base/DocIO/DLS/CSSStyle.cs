// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CSSStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class CSSStyle
{
  private List<CSSStyleItem> m_styleCollection;

  internal List<CSSStyleItem> StyleCollection
  {
    get
    {
      if (this.m_styleCollection == null)
        this.m_styleCollection = new List<CSSStyleItem>();
      return this.m_styleCollection;
    }
    set => this.m_styleCollection = value;
  }

  internal CSSStyleItem GetCSSStyleItem(string styleName, CSSStyleItem.CssStyleType styleType)
  {
    for (int index = 0; index < this.StyleCollection.Count; ++index)
    {
      CSSStyleItem style = this.StyleCollection[index];
      if (style.StyleName == styleName && style.StyleType == styleType)
        return style;
    }
    return (CSSStyleItem) null;
  }

  internal void Close()
  {
    if (this.m_styleCollection == null)
      return;
    for (int index = 0; index < this.m_styleCollection.Count; index = index - 1 + 1)
    {
      CSSStyleItem style = this.m_styleCollection[index];
      this.m_styleCollection.Remove(style);
      style.Close();
    }
  }
}
