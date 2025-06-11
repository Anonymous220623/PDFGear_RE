// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represent the collection of line points.</summary>
/// <typeparam name="T">Underlying type used with this collection.</typeparam>
/// <remarks>
/// The maximum number of points in a collection depends on the usage context.
/// Currently it is 2 if type of T is <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />,
/// 3 if type of T is <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" /> and unlimited for others.
/// </remarks>
public class PdfLinePointCollection<T> : 
  IList<FS_POINTF>,
  ICollection<FS_POINTF>,
  IEnumerable<FS_POINTF>,
  IEnumerable
  where T : PdfAnnotation
{
  private int _maxLen = -1;
  private PdfTypeArray _cl;

  internal PdfTypeArray LinePoints => this._cl;

  /// <summary>
  /// Gets a value indicating whether the underlying array has been disposed of.
  /// </summary>
  /// <value>true if object has been disposed of; otherwise, false.</value>
  public bool IsDisposed => this._cl.IsDisposed;

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />
  /// </summary>
  public PdfLinePointCollection()
  {
    this._maxLen = !(typeof (T) == typeof (PdfFreeTextAnnotation)) ? (!(typeof (T) == typeof (PdfLineAnnotation)) ? -1 : 2) : 3;
    this._cl = PdfTypeArray.Create();
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> with specified line points array
  /// </summary>
  /// <param name="linePoints">The line points array.</param>
  /// <remarks>
  /// Line points array may be modified to across the PDF internal rules.
  /// </remarks>
  public PdfLinePointCollection(PdfTypeArray linePoints)
  {
    if (linePoints == null)
      throw new ArgumentNullException();
    this._maxLen = !(typeof (T) == typeof (PdfFreeTextAnnotation)) ? (!(typeof (T) == typeof (PdfLineAnnotation)) ? -1 : 2) : 3;
    this._cl = linePoints;
    if (this._maxLen < 0)
      return;
    if (this._cl.Count > this._maxLen * 2)
    {
      for (int index = this._cl.Count - 1; index >= this._maxLen * 2; --index)
        this._cl.RemoveAt(index);
    }
    else
    {
      if (this._cl.Count % 2 == 0)
        return;
      this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(0.0f));
    }
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> with specified points
  /// </summary>
  /// <param name="p1">First poitn of the callout line</param>
  /// <param name="p2">Second poitn of the callout line</param>
  public PdfLinePointCollection(FS_POINTF p1, FS_POINTF p2)
    : this()
  {
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p1.X));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p1.Y));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p2.X));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p2.Y));
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> with specified points
  /// </summary>
  /// <param name="p1">First poitn of the callout line</param>
  /// <param name="p2">Second poitn of the callout line</param>
  /// <param name="p3">Third poitn of the callout line</param>
  public PdfLinePointCollection(FS_POINTF p1, FS_POINTF p2, FS_POINTF p3)
    : this()
  {
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p1.X));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p1.Y));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p2.X));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p2.Y));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p3.X));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(p3.Y));
  }

  /// <summary>Releases all lresources used by the collection.</summary>
  ~PdfLinePointCollection()
  {
    if (this._cl == null)
      return;
    this._cl.Dispose();
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.FS_POINTF" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.FS_POINTF" /> at the specified index.</returns>
  public FS_POINTF this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      return new FS_POINTF(this._cl[index * 2].As<PdfTypeNumber>().FloatValue, this._cl[index * 2 + 1].As<PdfTypeNumber>().FloatValue);
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      this._cl[index * 2] = (PdfTypeBase) PdfTypeNumber.Create(value.X);
      this._cl[index * 2 + 1] = (PdfTypeBase) PdfTypeNumber.Create(value.Y);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.FS_POINTF" /> contained in the collection.
  /// </summary>
  public int Count => this._cl.Count / 2;

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.FS_POINTF" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_POINTF" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.FS_POINTF" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(FS_POINTF item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.FS_POINTF" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.FS_POINTF" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.FS_POINTF" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(FS_POINTF item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.FS_POINTF" /> from the collection
  /// </summary>
  public void Clear() => this._cl.Clear();

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.FS_POINTF" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.FS_POINTF" />  to remove.</param>
  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.Count)
      throw new ArgumentOutOfRangeException();
    this._cl.RemoveAt(index * 2 + 1);
    this._cl.RemoveAt(index * 2);
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(FS_POINTF[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (FS_POINTF fsPointf in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = fsPointf;
    }
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.FS_POINTF" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_POINTF" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.FS_POINTF" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.FS_POINTF" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(FS_POINTF item)
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
  public IEnumerator<FS_POINTF> GetEnumerator()
  {
    return (IEnumerator<FS_POINTF>) new CollectionEnumerator<FS_POINTF>((IList<FS_POINTF>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.FS_POINTF" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_POINTF" />  to add to the collection</param>
  public void Add(FS_POINTF item)
  {
    if (this._maxLen >= 0 && this.Count >= this._maxLen)
      throw new IndexOutOfRangeException(Error.err0049);
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(item.X));
    this._cl.Add((PdfTypeBase) PdfTypeNumber.Create(item.Y));
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.FS_POINTF" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.FS_POINTF" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_POINTF" /> to insert into the collection.</param>
  public void Insert(int index, FS_POINTF item)
  {
    if (this._maxLen >= 0 && this.Count >= this._maxLen)
      throw new IndexOutOfRangeException(Error.err0049);
    this._cl.Insert(index * 2, (PdfTypeBase) PdfTypeNumber.Create(item.X));
    this._cl.Insert(index * 2 + 1, (PdfTypeBase) PdfTypeNumber.Create(item.Y));
  }
}
