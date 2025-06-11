// Decompiled with JetBrains decompiler
// Type: HandyControl.Collections.Deque`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace HandyControl.Collections;

[Serializable]
public class Deque<T> : IEnumerable<T>, IEnumerable, ICollection
{
  [NonSerialized]
  private object _syncRoot;
  private List<T> _front;
  private List<T> _back;
  private int _frontDeleted;
  private int _backDeleted;

  public int Capacity => this._front.Capacity + this._back.Capacity;

  public int Count => this._front.Count + this._back.Count - this._frontDeleted - this._backDeleted;

  public bool IsEmpty => this.Count == 0;

  public IEnumerable<T> Reversed
  {
    get
    {
      int i;
      if (this._back.Count - this._backDeleted > 0)
      {
        for (i = this._back.Count - 1; i >= this._backDeleted; --i)
          yield return this._back[i];
      }
      if (this._front.Count - this._frontDeleted > 0)
      {
        for (i = this._frontDeleted; i < this._front.Count; ++i)
          yield return this._front[i];
      }
    }
  }

  public Deque()
  {
    this._front = new List<T>();
    this._back = new List<T>();
  }

  public Deque(int capacity)
  {
    if (capacity < 0)
      throw new ArgumentException("Capacity cannot be negative");
    int capacity1 = capacity / 2;
    int capacity2 = capacity - capacity1;
    this._front = new List<T>(capacity1);
    this._back = new List<T>(capacity2);
  }

  public Deque(IEnumerable<T> backCollection)
    : this(backCollection, (IEnumerable<T>) null)
  {
  }

  public Deque(IEnumerable<T> backCollection, IEnumerable<T> frontCollection)
  {
    if (backCollection == null && frontCollection == null)
      throw new ArgumentException("Collections cannot both be null");
    this._front = frontCollection != null ? new List<T>(frontCollection) : new List<T>();
    this._back = backCollection != null ? new List<T>(backCollection) : new List<T>();
  }

  public void AddFirst(T item)
  {
    if (this._frontDeleted > 0 && this._front.Count == this._front.Capacity)
    {
      this._front.RemoveRange(0, this._frontDeleted);
      this._frontDeleted = 0;
    }
    this._front.Add(item);
  }

  public void AddLast(T item)
  {
    if (this._backDeleted > 0 && this._back.Count == this._back.Capacity)
    {
      this._back.RemoveRange(0, this._backDeleted);
      this._backDeleted = 0;
    }
    this._back.Add(item);
  }

  public void AddRangeFirst(IEnumerable<T> range)
  {
    if (range == null)
      return;
    foreach (T obj in range)
      this.AddFirst(obj);
  }

  public void AddRangeLast(IEnumerable<T> range)
  {
    if (range == null)
      return;
    foreach (T obj in range)
      this.AddLast(obj);
  }

  public void Clear()
  {
    this._front.Clear();
    this._back.Clear();
    this._frontDeleted = 0;
    this._backDeleted = 0;
  }

  public bool Contains(T item)
  {
    for (int frontDeleted = this._frontDeleted; frontDeleted < this._front.Count; ++frontDeleted)
    {
      if (object.Equals((object) this._front[frontDeleted], (object) item))
        return true;
    }
    for (int backDeleted = this._backDeleted; backDeleted < this._back.Count; ++backDeleted)
    {
      if (object.Equals((object) this._back[backDeleted], (object) item))
        return true;
    }
    return false;
  }

  public void CopyTo(T[] array, int index)
  {
    if (array == null)
      throw new ArgumentNullException("Array cannot be null");
    if (index < 0)
      throw new ArgumentOutOfRangeException("Index cannot be negative");
    if (array.Length < index + this.Count)
      throw new ArgumentException("Index is invalid");
    int num = index;
    foreach (T obj in this)
      array[num++] = obj;
  }

  public IEnumerator<T> GetEnumerator()
  {
    int i;
    if (this._front.Count - this._frontDeleted > 0)
    {
      for (i = this._front.Count - 1; i >= this._frontDeleted; --i)
        yield return this._front[i];
    }
    if (this._back.Count - this._backDeleted > 0)
    {
      for (i = this._backDeleted; i < this._back.Count; ++i)
        yield return this._back[i];
    }
  }

  public T PeekFirst()
  {
    if (this._front.Count > this._frontDeleted)
      return this._front[this._front.Count - 1];
    if (this._back.Count > this._backDeleted)
      return this._back[this._backDeleted];
    throw new InvalidOperationException("Can't peek at empty Deque");
  }

  public T PeekLast()
  {
    if (this._back.Count > this._backDeleted)
      return this._back[this._back.Count - 1];
    if (this._front.Count > this._frontDeleted)
      return this._front[this._frontDeleted];
    throw new InvalidOperationException("Can't peek at empty Deque");
  }

  public T PopFirst()
  {
    T obj;
    if (this._front.Count > this._frontDeleted)
    {
      obj = this._front[this._front.Count - 1];
      this._front.RemoveAt(this._front.Count - 1);
    }
    else
    {
      if (this._back.Count <= this._backDeleted)
        throw new InvalidOperationException("Can't pop empty Deque");
      obj = this._back[this._backDeleted];
      ++this._backDeleted;
    }
    return obj;
  }

  public T PopLast()
  {
    T obj;
    if (this._back.Count > this._backDeleted)
    {
      obj = this._back[this._back.Count - 1];
      this._back.RemoveAt(this._back.Count - 1);
    }
    else
    {
      if (this._front.Count <= this._frontDeleted)
        throw new InvalidOperationException("Can't pop empty Deque");
      obj = this._front[this._frontDeleted];
      ++this._frontDeleted;
    }
    return obj;
  }

  public void Reverse()
  {
    List<T> front = this._front;
    this._front = this._back;
    this._back = front;
    int frontDeleted = this._frontDeleted;
    this._frontDeleted = this._backDeleted;
    this._backDeleted = frontDeleted;
  }

  public T[] ToArray()
  {
    if (this.Count == 0)
      return new T[0];
    T[] array = new T[this.Count];
    this.CopyTo(array, 0);
    return array;
  }

  public void TrimExcess()
  {
    if (this._frontDeleted > 0)
    {
      this._front.RemoveRange(0, this._frontDeleted);
      this._frontDeleted = 0;
    }
    if (this._backDeleted > 0)
    {
      this._back.RemoveRange(0, this._backDeleted);
      this._backDeleted = 0;
    }
    this._front.TrimExcess();
    this._back.TrimExcess();
  }

  public bool TryPeekFirst(out T item)
  {
    if (!this.IsEmpty)
    {
      item = this.PeekFirst();
      return true;
    }
    item = default (T);
    return false;
  }

  public bool TryPeekLast(out T item)
  {
    if (!this.IsEmpty)
    {
      item = this.PeekLast();
      return true;
    }
    item = default (T);
    return false;
  }

  public bool TryPopFirst(out T item)
  {
    if (!this.IsEmpty)
    {
      item = this.PopFirst();
      return true;
    }
    item = default (T);
    return false;
  }

  public bool TryPopLast(out T item)
  {
    if (!this.IsEmpty)
    {
      item = this.PopLast();
      return true;
    }
    item = default (T);
    return false;
  }

  bool ICollection.IsSynchronized => false;

  object ICollection.SyncRoot
  {
    get
    {
      if (this._syncRoot == null)
        Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
      return this._syncRoot;
    }
  }

  void ICollection.CopyTo(Array array, int index) => this.CopyTo((T[]) array, index);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
