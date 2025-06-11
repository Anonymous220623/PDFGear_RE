// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CustomXmlSchemaCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CustomXmlSchemaCollection : ICustomXmlSchemaCollection, IEnumerable
{
  private readonly List<string> m_Items = new List<string>();

  public int Count => this.m_Items.Count;

  public string this[int index]
  {
    get => this.m_Items[index];
    set => this.m_Items[index] = value;
  }

  public void Add(string name) => this.m_Items.Add(name);

  public void Clear() => this.m_Items.Clear();

  public ICustomXmlSchemaCollection Clone()
  {
    CustomXmlSchemaCollection schemaCollection = new CustomXmlSchemaCollection();
    foreach (string name in this)
      schemaCollection.Add(name);
    return (ICustomXmlSchemaCollection) schemaCollection;
  }

  public int IndexOf(string value) => this.m_Items.IndexOf(value);

  public void Remove(string name) => this.m_Items.Remove(name);

  public void RemoveAt(int index) => this.m_Items.RemoveAt(index);

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_Items.GetEnumerator();
}
