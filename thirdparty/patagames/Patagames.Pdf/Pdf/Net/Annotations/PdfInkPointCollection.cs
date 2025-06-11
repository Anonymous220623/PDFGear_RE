// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfInkPointCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>
/// Represents a collection in which each item is a collection of line points
/// </summary>
public class PdfInkPointCollection : 
  IList<PdfLinePointCollection<PdfInkAnnotation>>,
  ICollection<PdfLinePointCollection<PdfInkAnnotation>>,
  IEnumerable<PdfLinePointCollection<PdfInkAnnotation>>,
  IEnumerable
{
  private PdfTypeArray _il;
  private Dictionary<IntPtr, PdfLinePointCollection<PdfInkAnnotation>> _mgr = new Dictionary<IntPtr, PdfLinePointCollection<PdfInkAnnotation>>();

  internal PdfTypeArray InkList => this._il;

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfInkPointCollection" />
  /// </summary>
  public PdfInkPointCollection() => this._il = PdfTypeArray.Create();

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfInkPointCollection" /> with specified ink list points
  /// </summary>
  /// <param name="inkList">The ink list points array.</param>
  /// <remarks>
  /// Ink list points array may be modified to across the PDF internal rules.
  /// </remarks>
  public PdfInkPointCollection(PdfTypeArray inkList)
  {
    this._il = inkList != null ? inkList : throw new ArgumentNullException();
  }

  /// <summary>Releases all lresources used by the collection.</summary>
  ~PdfInkPointCollection() => this._il.Dispose();

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> at the specified index.</returns>
  public PdfLinePointCollection<PdfInkAnnotation> this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.MgrGet(index) : throw new IndexOutOfRangeException();
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      this._il[index] = (PdfTypeBase) value.LinePoints;
      this.MgrAdd(value);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> contained in the collection.
  /// </summary>
  public int Count => this._il.Count;

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfLinePointCollection<PdfInkAnnotation> item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfLinePointCollection<PdfInkAnnotation> item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].LinePoints.Handle == item.LinePoints.Handle)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> from the collection
  /// </summary>
  public void Clear()
  {
    this._il.Clear();
    this._mgr.Clear();
  }

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />  to remove.</param>
  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.Count)
      throw new ArgumentOutOfRangeException();
    this._mgr.Remove(this._il[index].Handle);
    this._il.RemoveAt(index);
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfLinePointCollection<PdfInkAnnotation>[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfLinePointCollection<PdfInkAnnotation> linePointCollection in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = linePointCollection;
    }
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfLinePointCollection<PdfInkAnnotation> item)
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
  public IEnumerator<PdfLinePointCollection<PdfInkAnnotation>> GetEnumerator()
  {
    return (IEnumerator<PdfLinePointCollection<PdfInkAnnotation>>) new CollectionEnumerator<PdfLinePointCollection<PdfInkAnnotation>>((IList<PdfLinePointCollection<PdfInkAnnotation>>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />  to add to the collection</param>
  public void Add(PdfLinePointCollection<PdfInkAnnotation> item)
  {
    this._il.Add((PdfTypeBase) item.LinePoints);
    this.MgrAdd(item);
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" /> to insert into the collection.</param>
  public void Insert(int index, PdfLinePointCollection<PdfInkAnnotation> item)
  {
    this._il.Insert(index, (PdfTypeBase) item.LinePoints);
    this.MgrAdd(item);
  }

  private PdfLinePointCollection<PdfInkAnnotation> MgrGet(int index)
  {
    IntPtr handle = this._il[index].As<PdfTypeArray>().Handle;
    if (this._mgr.ContainsKey(handle))
    {
      PdfLinePointCollection<PdfInkAnnotation> linePointCollection = this._mgr[handle];
      if (!linePointCollection.LinePoints.IsDisposed)
        return linePointCollection;
      this._mgr.Remove(handle);
    }
    PdfLinePointCollection<PdfInkAnnotation> linePointCollection1 = new PdfLinePointCollection<PdfInkAnnotation>(this._il[index].As<PdfTypeArray>());
    this._mgr.Add(handle, linePointCollection1);
    return linePointCollection1;
  }

  private void MgrAdd(PdfLinePointCollection<PdfInkAnnotation> item)
  {
    if (this._mgr.ContainsKey(item.LinePoints.Handle))
      return;
    this._mgr.Add(item.LinePoints.Handle, item);
  }
}
