// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CollectionImpl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class CollectionImpl : OwnerHolder
{
  private List<object> m_innerList;

  public int Count => this.m_innerList.Count;

  internal IList InnerList => (IList) this.m_innerList;

  protected CollectionImpl(WordDocument doc, OwnerHolder owner)
    : base(doc, owner)
  {
    this.m_innerList = new List<object>();
  }

  internal CollectionImpl(WordDocument doc, OwnerHolder owner, int capacity)
    : base(doc, owner)
  {
    this.m_innerList = new List<object>(capacity);
  }

  internal CollectionImpl() => this.m_innerList = new List<object>();

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_innerList.GetEnumerator();

  internal override void Close()
  {
    if (this.m_innerList != null)
    {
      this.m_innerList.Clear();
      this.m_innerList = (List<object>) null;
    }
    base.Close();
  }
}
