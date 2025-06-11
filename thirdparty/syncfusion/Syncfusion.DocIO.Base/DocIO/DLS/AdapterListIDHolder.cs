// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.AdapterListIDHolder
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class AdapterListIDHolder
{
  [ThreadStatic]
  private static AdapterListIDHolder m_instance;
  private Dictionary<int, string> m_listStyleIDtoName;
  private Dictionary<int, string> m_lfoStyleIDtoName;

  private AdapterListIDHolder()
  {
  }

  internal static AdapterListIDHolder Instance
  {
    get
    {
      if (AdapterListIDHolder.m_instance == null)
        AdapterListIDHolder.m_instance = new AdapterListIDHolder();
      return AdapterListIDHolder.m_instance;
    }
  }

  internal Dictionary<int, string> ListStyleIDtoName
  {
    get
    {
      if (this.m_listStyleIDtoName == null)
        this.m_listStyleIDtoName = new Dictionary<int, string>();
      return this.m_listStyleIDtoName;
    }
  }

  internal Dictionary<int, string> LfoStyleIDtoName
  {
    get
    {
      if (this.m_lfoStyleIDtoName == null)
        this.m_lfoStyleIDtoName = new Dictionary<int, string>();
      return this.m_lfoStyleIDtoName;
    }
  }

  internal bool ContainsListName(string name)
  {
    bool flag = false;
    IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this.m_listStyleIDtoName.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (enumerator.Value.Equals((object) name))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  internal void Close()
  {
    if (this.m_listStyleIDtoName != null)
    {
      this.m_listStyleIDtoName.Clear();
      this.m_listStyleIDtoName = (Dictionary<int, string>) null;
    }
    if (this.m_lfoStyleIDtoName != null)
    {
      this.m_lfoStyleIDtoName.Clear();
      this.m_lfoStyleIDtoName = (Dictionary<int, string>) null;
    }
    AdapterListIDHolder.m_instance = (AdapterListIDHolder) null;
  }
}
