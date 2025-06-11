// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.MetaProperties
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Office;

public class MetaProperties
{
  private List<object> m_innerList;
  internal XmlDocument m_contentTypeSchemaProperties;

  public MetaProperty this[int index] => this.InnerList[index] as MetaProperty;

  public int Count => this.m_innerList.Count;

  internal IList InnerList => (IList) this.m_innerList;

  public MetaProperty FindByName(string name)
  {
    foreach (MetaProperty inner in this.m_innerList)
    {
      if (inner.DisplayName == name)
        return inner;
    }
    return (MetaProperty) null;
  }

  internal MetaProperties() => this.m_innerList = new List<object>();

  internal void Add(MetaProperty metaProperty)
  {
    this.InnerList.Add((object) metaProperty);
    metaProperty.Parent = this;
  }

  internal void Remove(MetaProperty metaProperty) => this.InnerList.Remove((object) metaProperty);

  internal void Close()
  {
    while (this.InnerList.Count > 0)
      this.Remove(this[this.InnerList.Count - 1]);
  }
}
