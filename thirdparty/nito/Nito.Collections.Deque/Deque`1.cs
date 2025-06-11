// Decompiled with JetBrains decompiler
// Type: Nito.Collections.Deque`1
// Assembly: Nito.Collections.Deque, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AAACFF4B-3CF8-41B7-BD25-F9042A065111
// Assembly location: D:\PDFGear\bin\Nito.Collections.Deque.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable
namespace Nito.Collections;

[DebuggerDisplay("Count = {Count}, Capacity = {Capacity}")]
[DebuggerTypeProxy(typeof (Deque<>.DebugView))]
public sealed class Deque<T> : 
  IList<T>,
  ICollection<T>,
  IEnumerable<T>,
  IEnumerable,
  IReadOnlyList<T>,
  IReadOnlyCollection<T>,
  IList,
  ICollection
{
  private const int DefaultCapacity = 8;
  private T[] _buffer;
  private int _offset;

  public Deque(int capacity)
  {
    this._buffer = capacity >= 0 ? new T[capacity] : throw new ArgumentOutOfRangeException(nameof (capacity), "Capacity may not be negative.");
  }

  public Deque(IEnumerable<T> collection)
  {
    IReadOnlyCollection<T> collection1 = collection != null ? CollectionHelpers.ReifyCollection<T>(collection) : throw new ArgumentNullException(nameof (collection));
    int count = collection1.Count;
    if (count > 0)
    {
      this._buffer = new T[count];
      this.DoInsertRange(0, collection1);
    }
    else
      this._buffer = new T[8];
  }

  public Deque()
    : this(8)
  {
  }

  bool ICollection<T>.IsReadOnly => false;

  public T this[int index]
  {
    get
    {
      Deque<T>.CheckExistingIndexArgument(this.Count, index);
      return this.DoGetItem(index);
    }
    set
    {
      Deque<T>.CheckExistingIndexArgument(this.Count, index);
      this.DoSetItem(index, value);
    }
  }

  public void Insert(int index, T item)
  {
    Deque<T>.CheckNewIndexArgument(this.Count, index);
    this.DoInsert(index, item);
  }

  public void RemoveAt(int index)
  {
    Deque<T>.CheckExistingIndexArgument(this.Count, index);
    this.DoRemoveAt(index);
  }

  public int IndexOf(T item)
  {
    EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
    int num = 0;
    foreach (T y in this)
    {
      if (equalityComparer.Equals(item, y))
        return num;
      ++num;
    }
    return -1;
  }

  void ICollection<T>.Add(T item) => this.DoInsert(this.Count, item);

  bool ICollection<T>.Contains(T item)
  {
    EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
    foreach (T y in this)
    {
      if (equalityComparer.Equals(item, y))
        return true;
    }
    return false;
  }

  void ICollection<T>.CopyTo(T[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    int count = this.Count;
    Deque<T>.CheckRangeArguments(array.Length, arrayIndex, count);
    this.CopyToArray((Array) array, arrayIndex);
  }

  private void CopyToArray(Array array, int arrayIndex = 0)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (this.IsSplit)
    {
      int length = this.Capacity - this._offset;
      Array.Copy((Array) this._buffer, this._offset, array, arrayIndex, length);
      Array.Copy((Array) this._buffer, 0, array, arrayIndex + length, this.Count - length);
    }
    else
      Array.Copy((Array) this._buffer, this._offset, array, arrayIndex, this.Count);
  }

  public bool Remove(T item)
  {
    int index = this.IndexOf(item);
    if (index == -1)
      return false;
    this.DoRemoveAt(index);
    return true;
  }

  public IEnumerator<T> GetEnumerator()
  {
    int count = this.Count;
    for (int i = 0; i != count; ++i)
      yield return this.DoGetItem(i);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  private static bool IsT(object? value)
  {
    if (value is T)
      return true;
    return value == null && (object) default (T) == null;
  }

  int IList.Add(object? value)
  {
    if (value == null && (object) default (T) != null)
      throw new ArgumentNullException(nameof (value), "Value cannot be null.");
    if (!Deque<T>.IsT(value))
      throw new ArgumentException("Value is of incorrect type.", nameof (value));
    this.AddToBack((T) value);
    return this.Count - 1;
  }

  bool IList.Contains(object? value)
  {
    return Deque<T>.IsT(value) && ((ICollection<T>) this).Contains((T) value);
  }

  int IList.IndexOf(object? value) => !Deque<T>.IsT(value) ? -1 : this.IndexOf((T) value);

  void IList.Insert(int index, object? value)
  {
    if (value == null && (object) default (T) != null)
      throw new ArgumentNullException(nameof (value), "Value cannot be null.");
    if (!Deque<T>.IsT(value))
      throw new ArgumentException("Value is of incorrect type.", nameof (value));
    this.Insert(index, (T) value);
  }

  bool IList.IsFixedSize => false;

  bool IList.IsReadOnly => false;

  void IList.Remove(object? value)
  {
    if (!Deque<T>.IsT(value))
      return;
    this.Remove((T) value);
  }

  object? IList.this[int index]
  {
    get => (object) this[index];
    set
    {
      if (value == null && (object) default (T) != null)
        throw new ArgumentNullException(nameof (value), "Value cannot be null.");
      this[index] = Deque<T>.IsT(value) ? (T) value : throw new ArgumentException("Value is of incorrect type.", nameof (value));
    }
  }

  void ICollection.CopyTo(Array array, int index)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array), "Destination array cannot be null.");
    Deque<T>.CheckRangeArguments(array.Length, index, this.Count);
    try
    {
      this.CopyToArray(array, index);
    }
    catch (ArrayTypeMismatchException ex)
    {
      throw new ArgumentException("Destination array is of incorrect type.", nameof (array), (Exception) ex);
    }
    catch (RankException ex)
    {
      throw new ArgumentException("Destination array must be single dimensional.", nameof (array), (Exception) ex);
    }
  }

  bool ICollection.IsSynchronized => false;

  object ICollection.SyncRoot => (object) this;

  private static void CheckNewIndexArgument(int sourceLength, int index)
  {
    if (index < 0 || index > sourceLength)
      throw new ArgumentOutOfRangeException(nameof (index), $"Invalid new index {index.ToString()} for source length {sourceLength.ToString()}");
  }

  private static void CheckExistingIndexArgument(int sourceLength, int index)
  {
    if (index < 0 || index >= sourceLength)
      throw new ArgumentOutOfRangeException(nameof (index), $"Invalid existing index {index.ToString()} for source length {sourceLength.ToString()}");
  }

  private static void CheckRangeArguments(int sourceLength, int offset, int count)
  {
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset), "Invalid offset " + offset.ToString());
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count), "Invalid count " + count.ToString());
    if (sourceLength - offset < count)
      throw new ArgumentException($"Invalid offset ({offset.ToString()}) or count + ({count.ToString()}) for source length {sourceLength.ToString()}");
  }

  private bool IsEmpty => this.Count == 0;

  private bool IsFull => this.Count == this.Capacity;

  private bool IsSplit => this._offset > this.Capacity - this.Count;

  public int Capacity
  {
    get => this._buffer.Length;
    set
    {
      if (value < this.Count)
        throw new ArgumentOutOfRangeException(nameof (value), "Capacity cannot be set to a value less than Count");
      if (value == this._buffer.Length)
        return;
      T[] objArray = new T[value];
      this.CopyToArray((Array) objArray);
      this._buffer = objArray;
      this._offset = 0;
    }
  }

  public int Count { get; private set; }

  private int DequeIndexToBufferIndex(int index) => (index + this._offset) % this.Capacity;

  private T DoGetItem(int index) => this._buffer[this.DequeIndexToBufferIndex(index)];

  private void DoSetItem(int index, T item)
  {
    this._buffer[this.DequeIndexToBufferIndex(index)] = item;
  }

  private void DoInsert(int index, T item)
  {
    this.EnsureCapacityForOneElement();
    if (index == 0)
      this.DoAddToFront(item);
    else if (index == this.Count)
      this.DoAddToBack(item);
    else
      this.DoInsertRange(index, (IReadOnlyCollection<T>) new T[1]
      {
        item
      });
  }

  private void DoRemoveAt(int index)
  {
    if (index == 0)
      this.DoRemoveFromFront();
    else if (index == this.Count - 1)
      this.DoRemoveFromBack();
    else
      this.DoRemoveRange(index, 1);
  }

  private int PostIncrement(int value)
  {
    int offset = this._offset;
    this._offset += value;
    this._offset %= this.Capacity;
    return offset;
  }

  private int PreDecrement(int value)
  {
    this._offset -= value;
    if (this._offset < 0)
      this._offset += this.Capacity;
    return this._offset;
  }

  private void DoAddToBack(T value)
  {
    this._buffer[this.DequeIndexToBufferIndex(this.Count)] = value;
    ++this.Count;
  }

  private void DoAddToFront(T value)
  {
    this._buffer[this.PreDecrement(1)] = value;
    ++this.Count;
  }

  private T DoRemoveFromBack()
  {
    T obj = this._buffer[this.DequeIndexToBufferIndex(this.Count - 1)];
    --this.Count;
    return obj;
  }

  private T DoRemoveFromFront()
  {
    --this.Count;
    return this._buffer[this.PostIncrement(1)];
  }

  private void DoInsertRange(int index, IReadOnlyCollection<T> collection)
  {
    int count = collection.Count;
    if (index < this.Count / 2)
    {
      int num1 = index;
      int num2 = this.Capacity - count;
      for (int index1 = 0; index1 != num1; ++index1)
        this._buffer[this.DequeIndexToBufferIndex(num2 + index1)] = this._buffer[this.DequeIndexToBufferIndex(index1)];
      this.PreDecrement(count);
    }
    else
    {
      int num3 = this.Count - index;
      int num4 = index + count;
      for (int index2 = num3 - 1; index2 != -1; --index2)
        this._buffer[this.DequeIndexToBufferIndex(num4 + index2)] = this._buffer[this.DequeIndexToBufferIndex(index + index2)];
    }
    int index3 = index;
    foreach (T obj in (IEnumerable<T>) collection)
    {
      this._buffer[this.DequeIndexToBufferIndex(index3)] = obj;
      ++index3;
    }
    this.Count += count;
  }

  private void DoRemoveRange(int index, int collectionCount)
  {
    if (index == 0)
    {
      this.PostIncrement(collectionCount);
      this.Count -= collectionCount;
    }
    else if (index == this.Count - collectionCount)
    {
      this.Count -= collectionCount;
    }
    else
    {
      if (index + collectionCount / 2 < this.Count / 2)
      {
        int num1 = index;
        int num2 = collectionCount;
        for (int index1 = num1 - 1; index1 != -1; --index1)
          this._buffer[this.DequeIndexToBufferIndex(num2 + index1)] = this._buffer[this.DequeIndexToBufferIndex(index1)];
        this.PostIncrement(collectionCount);
      }
      else
      {
        int num3 = this.Count - collectionCount - index;
        int num4 = index + collectionCount;
        for (int index2 = 0; index2 != num3; ++index2)
          this._buffer[this.DequeIndexToBufferIndex(index + index2)] = this._buffer[this.DequeIndexToBufferIndex(num4 + index2)];
      }
      this.Count -= collectionCount;
    }
  }

  private void EnsureCapacityForOneElement()
  {
    if (!this.IsFull)
      return;
    this.Capacity = this.Capacity == 0 ? 1 : this.Capacity * 2;
  }

  public void AddToBack(T value)
  {
    this.EnsureCapacityForOneElement();
    this.DoAddToBack(value);
  }

  public void AddToFront(T value)
  {
    this.EnsureCapacityForOneElement();
    this.DoAddToFront(value);
  }

  public void InsertRange(int index, IEnumerable<T> collection)
  {
    Deque<T>.CheckNewIndexArgument(this.Count, index);
    IReadOnlyCollection<T> collection1 = CollectionHelpers.ReifyCollection<T>(collection);
    int count = collection1.Count;
    if (count > this.Capacity - this.Count)
      this.Capacity = checked (this.Count + count);
    if (count == 0)
      return;
    this.DoInsertRange(index, collection1);
  }

  public void RemoveRange(int offset, int count)
  {
    Deque<T>.CheckRangeArguments(this.Count, offset, count);
    if (count == 0)
      return;
    this.DoRemoveRange(offset, count);
  }

  public T RemoveFromBack()
  {
    if (this.IsEmpty)
      throw new InvalidOperationException("The deque is empty.");
    return this.DoRemoveFromBack();
  }

  public T RemoveFromFront()
  {
    if (this.IsEmpty)
      throw new InvalidOperationException("The deque is empty.");
    return this.DoRemoveFromFront();
  }

  public void Clear()
  {
    this._offset = 0;
    this.Count = 0;
  }

  public T[] ToArray()
  {
    T[] array = new T[this.Count];
    ((ICollection<T>) this).CopyTo(array, 0);
    return array;
  }

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly Deque<T> deque;

    public DebugView(Deque<T> deque) => this.deque = deque;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items => this.deque.ToArray();
  }
}
