// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.WeakCollection`1
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace CommomLib.Commom;

public class WeakCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : class
{
  private List<WeakReference> internalList = new List<WeakReference>();

  public int Count
  {
    get
    {
      lock (this.internalList)
      {
        if (this.internalList.Count == 0)
          return 0;
        if (this.internalList.Count == 1 && this.internalList[0].Target != null)
          return 1;
        if (this.internalList.Count == 2 && this.internalList[0].Target != null && this.internalList[1].Target != null)
          return 2;
        if (this.internalList.Count == 3 && this.internalList[0].Target != null && this.internalList[1].Target != null && this.internalList[2].Target != null)
          return 3;
        if (this.internalList.Count == 4 && this.internalList[0].Target != null && this.internalList[1].Target != null && this.internalList[2].Target != null && this.internalList[3].Target != null)
          return 4;
        WeakCollection<T>.WeakRefEnumerator weakRefEnumerator = new WeakCollection<T>.WeakRefEnumerator(this.internalList);
        int count = 0;
        while (weakRefEnumerator.MoveNext())
          ++count;
        return count;
      }
    }
  }

  public bool IsReadOnly => false;

  public void Add(T item)
  {
    lock (this.internalList)
      this.internalList.Add(new WeakReference((object) item));
  }

  public void Clear()
  {
    lock (this.internalList)
      this.internalList.Clear();
  }

  public bool Contains(T item)
  {
    lock (this.internalList)
    {
      WeakCollection<T>.WeakRefEnumerator weakRefEnumerator = new WeakCollection<T>.WeakRefEnumerator(this.internalList);
      while (weakRefEnumerator.MoveNext())
      {
        if (object.Equals((object) weakRefEnumerator.Current, (object) item))
          return true;
      }
      return false;
    }
  }

  public void CopyTo(T[] array, int arrayIndex)
  {
    lock (this.internalList)
    {
      WeakCollection<T>.WeakRefEnumerator weakRefEnumerator = new WeakCollection<T>.WeakRefEnumerator(this.internalList);
      int num = 0;
      while (weakRefEnumerator.MoveNext())
      {
        array[arrayIndex + num] = weakRefEnumerator.Current;
        ++num;
      }
    }
  }

  public IEnumerator<T> GetEnumerator()
  {
    WeakCollection<T>.WeakRefEnumerator enumerable = new WeakCollection<T>.WeakRefEnumerator(this.internalList);
    while (enumerable.MoveNext())
      yield return enumerable.Current;
  }

  public bool Remove(T item)
  {
    lock (this.internalList)
    {
      WeakCollection<T>.WeakRefEnumerator weakRefEnumerator = new WeakCollection<T>.WeakRefEnumerator(this.internalList);
      while (weakRefEnumerator.MoveNext())
      {
        if (object.Equals((object) weakRefEnumerator.Current, (object) item))
        {
          this.internalList.RemoveAt(weakRefEnumerator.CurrentIndex);
          return true;
        }
      }
      return false;
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  private struct WeakRefEnumerator
  {
    private List<WeakReference> _list;
    private T _current;
    private int _readIndex;
    private int _writeIndex;

    internal WeakRefEnumerator(List<WeakReference> list)
    {
      this._list = list;
      this._readIndex = 0;
      this._writeIndex = 0;
      this._current = default (T);
    }

    internal T Current => this._current;

    internal int CurrentIndex => this._writeIndex - 1;

    internal void Dispose()
    {
      if (this._readIndex != this._writeIndex)
      {
        this._list.RemoveRange(this._writeIndex, this._readIndex - this._writeIndex);
        this._readIndex = this._writeIndex = this._list.Count;
      }
      this._current = default (T);
    }

    internal bool MoveNext()
    {
      for (; this._readIndex < this._list.Count; ++this._readIndex)
      {
        WeakReference weakReference = this._list[this._readIndex];
        this._current = (T) weakReference.Target;
        if ((object) this._current != null)
        {
          if (this._writeIndex != this._readIndex)
            this._list[this._writeIndex] = weakReference;
          ++this._readIndex;
          ++this._writeIndex;
          return true;
        }
      }
      this.Dispose();
      return false;
    }
  }
}
