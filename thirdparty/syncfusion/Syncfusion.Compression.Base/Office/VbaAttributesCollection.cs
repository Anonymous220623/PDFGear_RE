// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.VbaAttributesCollection
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Office;

internal class VbaAttributesCollection : CollectionBase<VbaAttribute>
{
  private VbaModule m_module;

  internal VbaAttributesCollection(VbaModule module) => this.m_module = module;

  internal VbaAttribute this[string name]
  {
    get
    {
      foreach (VbaAttribute vbaAttribute in (CollectionBase<VbaAttribute>) this)
      {
        if (string.Equals(name, vbaAttribute.Name, StringComparison.OrdinalIgnoreCase))
          return vbaAttribute;
      }
      return (VbaAttribute) null;
    }
  }

  internal VbaAttribute AddAttribute(string name, string value, bool isText)
  {
    VbaAttribute vbaAttribute = new VbaAttribute();
    vbaAttribute.Name = name;
    vbaAttribute.Value = value;
    vbaAttribute.IsText = isText;
    this.Add(vbaAttribute);
    return vbaAttribute;
  }

  internal VbaAttributesCollection Clone(VbaModule parent)
  {
    VbaAttributesCollection attributesCollection = (VbaAttributesCollection) this.Clone();
    attributesCollection.m_module = parent;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      VbaAttribute inner = this.InnerList[index];
      attributesCollection.Add(inner.Clone());
    }
    return attributesCollection;
  }
}
