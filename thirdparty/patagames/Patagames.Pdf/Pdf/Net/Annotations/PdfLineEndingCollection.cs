// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfLineEndingCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>
/// Represent the collection of the line ending styles to be used in drawing the annotation’s border or lines
/// </summary>
/// <remarks>A collectiom may contain max two styles.</remarks>
public class PdfLineEndingCollection : 
  IList<LineEndingStyles>,
  ICollection<LineEndingStyles>,
  IEnumerable<LineEndingStyles>,
  IEnumerable
{
  private PdfTypeArray _le;

  internal PdfTypeArray LineEndingArray => this._le;

  /// <summary>
  /// Gets a value indicating whether the underlying array has been disposed of.
  /// </summary>
  /// <value>true if object has been disposed of; otherwise, false.</value>
  public bool IsDisposed => this._le.IsDisposed;

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineEndingCollection" /> with specified line ending styles array
  /// </summary>
  /// <param name="lineEnding">The line ending styles array.</param>
  public PdfLineEndingCollection(PdfTypeArray lineEnding)
  {
    this._le = lineEnding != null ? lineEnding : throw new ArgumentNullException();
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineEndingCollection" /> with specified line ending styles array
  /// </summary>
  /// <param name="first">The line ending style for the first point of the callout line.</param>
  /// <param name="second">The line ending style for the first point of the callout line.</param>
  public PdfLineEndingCollection(LineEndingStyles first, LineEndingStyles second)
  {
    this._le = PdfTypeArray.Create();
    this._le.Add((PdfTypeBase) PdfTypeName.Create(this.GetName(first)));
    this._le.Add((PdfTypeBase) PdfTypeName.Create(this.GetName(second)));
  }

  /// <summary>Releases all lresources used by the collection.</summary>
  ~PdfLineEndingCollection()
  {
    if (this._le == null)
      return;
    this._le.Dispose();
  }

  private string GetName(LineEndingStyles value)
  {
    string str = Pdfium.GetEnumDescription((Enum) value) ?? "";
    return !(str.Trim() == "") ? str : throw new ArgumentException(string.Format(Error.err0047, (object) "LineEnding", (object) "is one of the LineEndingStyles enum"));
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> at the specified index.</returns>
  public LineEndingStyles this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      LineEndingStyles result = LineEndingStyles.None;
      if (Pdfium.GetEnumDescription<LineEndingStyles>(this._le[index].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) "LE"));
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      this._le[index] = (PdfTypeBase) PdfTypeName.Create(this.GetName(value));
    }
  }

  /// <summary>Gets the number of items contained in the collection.</summary>
  public int Count => this._le.Count;

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(LineEndingStyles item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  item in the collection.
  /// </summary>
  /// <param name="item">The item to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(LineEndingStyles item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>Removes all items from the collection</summary>
  public void Clear() => this._le.Clear();

  /// <summary>Removes the item at the specified index.</summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" />  to remove.</param>
  public void RemoveAt(int index) => this._le.RemoveAt(index);

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(LineEndingStyles[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException();
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (LineEndingStyles lineEndingStyles in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = lineEndingStyles;
    }
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(LineEndingStyles item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<LineEndingStyles> GetEnumerator()
  {
    return (IEnumerator<LineEndingStyles>) new CollectionEnumerator<LineEndingStyles>((IList<LineEndingStyles>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" />  to add to the collection</param>
  public void Add(LineEndingStyles item)
  {
    if (this.Count >= 2)
      throw new IndexOutOfRangeException(Error.err0049);
    this._le.Add((PdfTypeBase) PdfTypeName.Create(this.GetName(item)));
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Enums.LineEndingStyles" /> to insert into the collection.</param>
  public void Insert(int index, LineEndingStyles item)
  {
    if (this.Count >= 2)
      throw new IndexOutOfRangeException(Error.err0049);
    this._le.Insert(index, (PdfTypeBase) PdfTypeName.Create(this.GetName(item)));
  }
}
