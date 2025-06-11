// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.MasterPageCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class MasterPageCollection : CollectionBase<MasterPage>
{
  private Dictionary<string, MasterPage> m_dictMasterPages;

  internal Dictionary<string, MasterPage> DictMasterPages
  {
    get
    {
      if (this.m_dictMasterPages == null)
        this.m_dictMasterPages = new Dictionary<string, MasterPage>();
      return this.m_dictMasterPages;
    }
    set => this.m_dictMasterPages = value;
  }

  internal string Add(MasterPage page)
  {
    string key = page.Name;
    if (string.IsNullOrEmpty(page.Name))
      key = CollectionBase<MasterPage>.GenerateDefaultName("mp", (ICollection) this.DictMasterPages.Values);
    if (!this.DictMasterPages.ContainsKey(key))
    {
      string str = this.ContainsValue(page);
      if (str != null)
      {
        key = str;
      }
      else
      {
        page.Name = key;
        this.DictMasterPages.Add(key, page);
      }
    }
    return key;
  }

  private string ContainsValue(MasterPage page)
  {
    string str = (string) null;
    foreach (MasterPage masterPage in this.DictMasterPages.Values)
    {
      if (masterPage.Equals((object) page))
      {
        str = masterPage.Name;
        break;
      }
    }
    return str;
  }

  internal void Remove(string key)
  {
    if (!this.DictMasterPages.ContainsKey(key))
      return;
    this.DictMasterPages.Remove(key);
  }

  internal void Dispose()
  {
    if (this.m_dictMasterPages == null)
      return;
    foreach (MasterPage masterPage in this.m_dictMasterPages.Values)
      masterPage.Dispose();
    this.m_dictMasterPages.Clear();
    this.m_dictMasterPages = (Dictionary<string, MasterPage>) null;
  }
}
