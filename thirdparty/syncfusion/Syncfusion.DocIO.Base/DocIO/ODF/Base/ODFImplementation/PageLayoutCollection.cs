// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.PageLayoutCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class PageLayoutCollection : CollectionBase<PageLayout>
{
  private Dictionary<string, PageLayout> m_dictStyles;

  internal Dictionary<string, PageLayout> DictStyles
  {
    get
    {
      if (this.m_dictStyles == null)
        this.m_dictStyles = new Dictionary<string, PageLayout>();
      return this.m_dictStyles;
    }
    set => this.m_dictStyles = value;
  }

  internal string Add(PageLayout layout)
  {
    string key = layout.Name;
    if (string.IsNullOrEmpty(layout.Name))
      key = CollectionBase<PageLayout>.GenerateDefaultName("pl", (ICollection) this.DictStyles.Values);
    if (!this.DictStyles.ContainsKey(key))
    {
      layout.Name = key;
      this.DictStyles.Add(key, layout);
    }
    return key;
  }

  internal void Remove(string key)
  {
    if (!this.DictStyles.ContainsKey(key))
      return;
    this.DictStyles.Remove(key);
  }

  internal void Dispose()
  {
    if (this.m_dictStyles == null)
      return;
    foreach (DefaultPageLayout defaultPageLayout in this.m_dictStyles.Values)
      defaultPageLayout.Dispose();
    this.m_dictStyles.Clear();
    this.m_dictStyles = (Dictionary<string, PageLayout>) null;
  }
}
