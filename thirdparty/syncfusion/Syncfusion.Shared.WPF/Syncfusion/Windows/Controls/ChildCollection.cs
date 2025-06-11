// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.ChildCollection
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class ChildCollection : IList<object>, ICollection<object>, IEnumerable<object>, IEnumerable
{
  private Collection<object> children;

  public ChildCollection() => this.children = new Collection<object>();

  public int Count => this.children.Count;

  public bool IsReadOnly => ((ICollection<object>) this.children).IsReadOnly;

  public object this[int index]
  {
    get => this.children[index];
    set => this.children[index] = value;
  }

  public int IndexOf(object item) => this.children.IndexOf(item);

  public void Insert(int index, object item) => this.children.Insert(index, item);

  public void RemoveAt(int index) => this.children.RemoveAt(index);

  public void Add(object item) => this.children.Add(item);

  public void Clear() => this.children.Clear();

  public bool Contains(object item) => this.children.Contains(item);

  public void CopyTo(object[] array, int arrayIndex) => this.children.CopyTo(array, arrayIndex);

  public bool Remove(object item) => this.children.Remove(item);

  public IEnumerator<object> GetEnumerator() => this.children.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.children).GetEnumerator();
}
