// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.CollectionImpl
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal abstract class CollectionImpl : OwnerHolder, ICollectionBase, IOfficeMathEntity
{
  internal List<object> m_innerList;

  public int Count => this.m_innerList.Count;

  internal IList InnerList => (IList) this.m_innerList;

  internal CollectionImpl(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_innerList = new List<object>();
  }

  internal override void Close()
  {
    if (this.m_innerList != null)
    {
      this.m_innerList.Clear();
      this.m_innerList = (List<object>) null;
    }
    base.Close();
  }

  internal void Add(object item) => this.m_innerList.Add(item);

  public void Remove(IOfficeMathEntity item) => this.m_innerList.Remove((object) item);

  public void Clear() => this.m_innerList.Clear();
}
