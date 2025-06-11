// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.UniqueInstanceTypeList
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

internal class UniqueInstanceTypeList
{
  private Dictionary<int, Dictionary<Type, object>> m_dictItems = new Dictionary<int, Dictionary<Type, object>>();

  public void AddShape(ShapeImpl shape)
  {
    int key = shape != null ? shape.Instance : throw new ArgumentNullException(nameof (shape));
    Dictionary<Type, object> dictionary;
    if (!this.m_dictItems.TryGetValue(key, out dictionary))
    {
      dictionary = new Dictionary<Type, object>();
      this.m_dictItems[key] = dictionary;
    }
    dictionary[shape.GetType()] = (object) null;
  }

  public IEnumerable UniquePairs()
  {
    foreach (int instance in this.m_dictItems.Keys)
    {
      Dictionary<Type, object> dictTypes = this.m_dictItems[instance];
      foreach (Type shapeType in dictTypes.Keys)
        yield return (object) new KeyValuePair<int, Type>(instance, shapeType);
    }
  }
}
