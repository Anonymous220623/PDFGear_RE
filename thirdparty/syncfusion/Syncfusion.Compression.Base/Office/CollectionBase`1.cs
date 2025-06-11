// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.CollectionBase`1
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class CollectionBase<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
  private System.Collections.Generic.List<T> m_arrItems;

  public int Capacity
  {
    get => this.m_arrItems.Capacity;
    set => this.m_arrItems.Capacity = value;
  }

  public int Count => this.m_arrItems.Count;

  protected internal System.Collections.Generic.List<T> InnerList => this.m_arrItems;

  protected IList<T> List => (IList<T>) this.m_arrItems;

  public T this[int i]
  {
    get => this.m_arrItems[i];
    set
    {
      T arrItem = this.m_arrItems[i];
      this.OnSet(i, arrItem, value);
      this.m_arrItems[i] = value;
      this.OnSetComplete(i, arrItem, value);
    }
  }

  public CollectionBase() => this.m_arrItems = new System.Collections.Generic.List<T>();

  public CollectionBase(int capacity) => this.m_arrItems = new System.Collections.Generic.List<T>(capacity);

  public void Clear()
  {
    this.OnClear();
    this.m_arrItems.Clear();
    this.OnClearComplete();
  }

  public void Insert(int index, T item)
  {
    this.OnInsert(index, item);
    this.m_arrItems.Insert(index, item);
    this.OnInsertComplete(index, item);
  }

  public virtual IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.m_arrItems.GetEnumerator();

  protected virtual void OnClear()
  {
  }

  protected virtual void OnClearComplete()
  {
  }

  protected virtual void OnInsert(int index, T value)
  {
  }

  protected virtual void OnInsertComplete(int index, T value)
  {
  }

  protected virtual void OnRemove(int index, T value)
  {
  }

  protected virtual void OnRemoveComplete(int index, T value)
  {
  }

  protected virtual void OnSet(int index, T oldValue, T newValue)
  {
  }

  protected virtual void OnSetComplete(int index, T oldValue, T newValue)
  {
  }

  public void RemoveAt(int index)
  {
    T obj = this[index];
    this.OnRemove(index, obj);
    this.m_arrItems.RemoveAt(index);
    this.OnRemoveComplete(index, obj);
  }

  internal object Clone()
  {
    CollectionBase<T> collectionBase = (CollectionBase<T>) this.MemberwiseClone();
    collectionBase.m_arrItems = new System.Collections.Generic.List<T>(this.Count);
    return (object) collectionBase;
  }

  public int IndexOf(T item) => this.m_arrItems.IndexOf(item);

  public virtual void Add(T item)
  {
    int count = this.Count;
    this.OnInsert(count, item);
    this.m_arrItems.Add(item);
    this.OnInsertComplete(count, item);
  }

  public bool Contains(T item) => this.m_arrItems.Contains(item);

  public void CopyTo(T[] array, int arrayIndex) => this.m_arrItems.CopyTo(array, arrayIndex);

  public bool IsReadOnly => false;

  public bool Remove(T item)
  {
    int index = this.IndexOf(item);
    bool flag = false;
    if (index >= 0)
    {
      this.RemoveAt(index);
      flag = true;
    }
    return flag;
  }

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.m_arrItems).GetEnumerator();
}
