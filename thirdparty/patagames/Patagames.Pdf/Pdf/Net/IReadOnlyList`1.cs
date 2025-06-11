// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.IReadOnlyList`1
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
/// Represents a strongly-typed, read-only collection of elements.
/// </summary>
/// <typeparam name="T">The type of the elements.</typeparam>
public interface IReadOnlyList<T> : IEnumerable<T>, IEnumerable
{
  /// <summary>
  /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire System.Collections.Generic.List`1.
  /// </summary>
  /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
  /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">match is null.</exception>
  T Find(Predicate<T> match);

  /// <summary>Gets the element at the specified index.</summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The element at the specified index.</returns>
  /// <exception cref="T:System.IndexOutOfRangeException">index is not a valid index in the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />.</exception>
  T this[int index] { get; }

  /// <summary>
  ///  Determines the index of a specific item in the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />.
  /// </summary>
  /// <param name="item">The object to locate in the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />.</param>
  /// <returns>The index of item if found in the list; otherwise, -1.</returns>
  int IndexOf(T item);

  /// <summary>
  ///  Gets the number of elements contained in the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />.
  /// </summary>
  int Count { get; }

  /// <summary>
  /// Gets a value indicating whether the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" /> is read-only.
  /// </summary>
  bool IsReadOnly { get; }

  /// <summary>
  /// Determines whether the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" /> contains a specific value.
  /// </summary>
  /// <param name="item">The object to locate in the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />.</param>
  /// <returns>rue if item is found in the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />; otherwise, false.</returns>
  bool Contains(T item);

  /// <summary>
  /// Copies the elements of the <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
  /// </summary>
  /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
  /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> less than 0.</exception>
  /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:Patagames.Pdf.Net.IReadOnlyList`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
  void CopyTo(T[] array, int arrayIndex);
}
