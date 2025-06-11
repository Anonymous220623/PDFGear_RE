// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.ReadOnlyList`1
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents a strongly typed list of objects that can be accessed by index.
/// Provides methods to search, sort, and manipulate lists.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class ReadOnlyList<T> : IEnumerable<T>, IEnumerable
{
  private List<T> _baseList = new List<T>();

  /// <summary>
  /// Searches for the specified object and returns the zero-based index of the
  /// first occurrence within the entire System.Collections.Generic.List.
  /// </summary>
  /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
  /// <returns>he zero-based index of the first occurrence of item within the entire System.Collections.Generic.List, if found; otherwise, –1.</returns>
  public int IndexOf(T item) => this._baseList.IndexOf(item);

  /// <summary>Gets or sets the element at the specified index.</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The element at the specified index.</returns>
  public T this[int index]
  {
    get => this._baseList[index];
    internal set => this._baseList[index] = value;
  }

  /// <summary>
  /// Determines whether an element is in the System.Collections.Generic.List.
  /// </summary>
  /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
  /// <returns>true if item is found in the System.Collections.Generic.List; otherwise, false</returns>
  public bool Contains(T item) => this._baseList.Contains(item);

  /// <summary>
  /// Copies the entire System.Collections.Generic.List to a compatible one-dimensional array, starting at the specified index of the target array.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements
  /// copied from System.Collections.Generic.List. The System.Array must have
  /// zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(T[] array, int arrayIndex) => this._baseList.CopyTo(array, arrayIndex);

  /// <summary>
  /// Gets the number of elements actually contained in the System.Collections.Generic.List.
  /// </summary>
  public int Count => this._baseList.Count;

  /// <summary>
  /// Gets a value indicating whether the IList is read-only.
  /// </summary>
  public bool IsReadOnly => true;

  /// <summary>
  ///  Returns an enumerator that iterates through the System.Collections.Generic.List.
  /// </summary>
  /// <returns>A System.Collections.Generic.List.Enumerator for the System.Collections.Generic.List.</returns>
  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._baseList.GetEnumerator();

  /// <summary>
  ///  Returns an enumerator that iterates through the System.Collections.Generic.List.
  /// </summary>
  /// <returns>A System.Collections.Generic.List.Enumerator for the System.Collections.Generic.List.</returns>
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._baseList.GetEnumerator();

  internal void Insert(int index, T item) => this._baseList.Insert(index, item);

  internal void RemoveAt(int index) => this._baseList.RemoveAt(index);

  internal void Add(T item) => this._baseList.Add(item);

  internal void AddRange(IEnumerable<T> collection) => this._baseList.AddRange(collection);

  internal void Clear() => this._baseList.Clear();

  internal bool Remove(T item) => this._baseList.Remove(item);

  /// <summary>
  /// Searches for an element that matches the conditions defined by the specified
  ///  predicate, and returns the first occurrence within the entire System.Collections.Generic.List.
  /// </summary>
  /// <param name="match">The System.Predicate delegate that defines the conditions of the element to search for.</param>
  /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.
  /// </returns>
  public T Find(Predicate<T> match) => this._baseList.Find(match);
}
