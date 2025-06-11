// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfQuadPointsCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a collection of the quadrilaterals</summary>
public class PdfQuadPointsCollection : 
  IList<FS_QUADPOINTSF>,
  ICollection<FS_QUADPOINTSF>,
  IEnumerable<FS_QUADPOINTSF>,
  IEnumerable
{
  private PdfLink _link;
  private PdfTypeArray _qp;

  internal PdfTypeArray QuadPoints
  {
    get
    {
      this.SwitchToQP();
      return this._qp;
    }
  }

  /// <summary>
  /// Initializes a new instance of the PdfQuadPointsCollection class.
  /// </summary>
  /// <param name="link">A Link from which the collection is builded.</param>
  internal PdfQuadPointsCollection(PdfLink link)
  {
    this._link = link;
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create(this._link.Handle);
    if (!pdfTypeDictionary.ContainsKey(nameof (QuadPoints)))
      return;
    this._qp = pdfTypeDictionary[nameof (QuadPoints)].As<PdfTypeArray>();
  }

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.PdfQuadPointsCollection" /> with specified quad points array
  /// </summary>
  /// <param name="quadPoints">The array of 8 numbers specifying the coordinates of quadrilateral given in the order x1, y1, x2, y2, x3, y3, x4, y4. </param>
  /// <remarks>
  /// Quad points array may be modified to across the PDF internal rules.
  /// </remarks>
  public PdfQuadPointsCollection(PdfTypeArray quadPoints)
  {
    if (quadPoints == null)
      throw new ArgumentNullException();
    this._link = (PdfLink) null;
    this._qp = quadPoints;
    int num = this._qp.Count % 8;
    if (num == 0)
      return;
    for (int index = 0; index < 8 - num; ++index)
      this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(0.0f));
  }

  /// <summary>
  /// Initializes a new instance of the PdfQuadPointsCollection class.
  /// </summary>
  public PdfQuadPointsCollection()
  {
    this._link = (PdfLink) null;
    this._qp = PdfTypeArray.Create();
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> at the specified index.</returns>
  public FS_QUADPOINTSF this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      double floatValue1 = (double) this._qp[index * 8].As<PdfTypeNumber>().FloatValue;
      float floatValue2 = this._qp[index * 8 + 1].As<PdfTypeNumber>().FloatValue;
      float floatValue3 = this._qp[index * 8 + 2].As<PdfTypeNumber>().FloatValue;
      float floatValue4 = this._qp[index * 8 + 3].As<PdfTypeNumber>().FloatValue;
      float floatValue5 = this._qp[index * 8 + 4].As<PdfTypeNumber>().FloatValue;
      float floatValue6 = this._qp[index * 8 + 5].As<PdfTypeNumber>().FloatValue;
      float floatValue7 = this._qp[index * 8 + 6].As<PdfTypeNumber>().FloatValue;
      float floatValue8 = this._qp[index * 8 + 7].As<PdfTypeNumber>().FloatValue;
      double y1 = (double) floatValue2;
      double x2 = (double) floatValue3;
      double y2 = (double) floatValue4;
      double x3 = (double) floatValue5;
      double y3 = (double) floatValue6;
      double x4 = (double) floatValue7;
      double y4 = (double) floatValue8;
      return new FS_QUADPOINTSF((float) floatValue1, (float) y1, (float) x2, (float) y2, (float) x3, (float) y3, (float) x4, (float) y4);
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      this._qp[index * 8] = (PdfTypeBase) PdfTypeNumber.Create(value.x1);
      this._qp[index * 8 + 1] = (PdfTypeBase) PdfTypeNumber.Create(value.y1);
      this._qp[index * 8 + 2] = (PdfTypeBase) PdfTypeNumber.Create(value.x2);
      this._qp[index * 8 + 3] = (PdfTypeBase) PdfTypeNumber.Create(value.y2);
      this._qp[index * 8 + 4] = (PdfTypeBase) PdfTypeNumber.Create(value.x3);
      this._qp[index * 8 + 5] = (PdfTypeBase) PdfTypeNumber.Create(value.y3);
      this._qp[index * 8 + 6] = (PdfTypeBase) PdfTypeNumber.Create(value.x4);
      this._qp[index * 8 + 7] = (PdfTypeBase) PdfTypeNumber.Create(value.y4);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> contained in the collection.
  /// </summary>
  public int Count
  {
    get
    {
      return this._qp != null ? this._qp.Count / 8 : Pdfium.FPDFLink_CountQuadPoints(this._link.Handle);
    }
  }

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(FS_QUADPOINTSF item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(FS_QUADPOINTSF item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> from the collection
  /// </summary>
  public void Clear()
  {
    if (this._qp == null)
      return;
    this._qp.Clear();
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(FS_QUADPOINTSF[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (FS_QUADPOINTSF fsQuadpointsf in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = fsQuadpointsf;
    }
  }

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />  to add to the collection</param>
  public void Add(FS_QUADPOINTSF item)
  {
    this.SwitchToQP();
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.x1));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.y1));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.x2));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.y2));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.x3));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.y3));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.x4));
    this._qp.Add((PdfTypeBase) PdfTypeNumber.Create(item.y4));
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<FS_QUADPOINTSF> GetEnumerator()
  {
    return (IEnumerator<FS_QUADPOINTSF>) new CollectionEnumerator<FS_QUADPOINTSF>((IList<FS_QUADPOINTSF>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(FS_QUADPOINTSF item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" />  to remove.</param>
  public void RemoveAt(int index)
  {
    this.SwitchToQP();
    this._qp.RemoveAt(index * 8 + 7);
    this._qp.RemoveAt(index * 8 + 6);
    this._qp.RemoveAt(index * 8 + 5);
    this._qp.RemoveAt(index * 8 + 4);
    this._qp.RemoveAt(index * 8 + 3);
    this._qp.RemoveAt(index * 8 + 2);
    this._qp.RemoveAt(index * 8 + 1);
    this._qp.RemoveAt(index * 8);
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> to insert into the collection.</param>
  public void Insert(int index, FS_QUADPOINTSF item)
  {
    this.SwitchToQP();
    this._qp.Insert(index * 8, (PdfTypeBase) PdfTypeNumber.Create(item.x1));
    this._qp.Insert(index * 8 + 1, (PdfTypeBase) PdfTypeNumber.Create(item.y1));
    this._qp.Insert(index * 8 + 2, (PdfTypeBase) PdfTypeNumber.Create(item.x2));
    this._qp.Insert(index * 8 + 3, (PdfTypeBase) PdfTypeNumber.Create(item.y2));
    this._qp.Insert(index * 8 + 4, (PdfTypeBase) PdfTypeNumber.Create(item.x3));
    this._qp.Insert(index * 8 + 5, (PdfTypeBase) PdfTypeNumber.Create(item.y3));
    this._qp.Insert(index * 8 + 6, (PdfTypeBase) PdfTypeNumber.Create(item.x4));
    this._qp.Insert(index * 8 + 7, (PdfTypeBase) PdfTypeNumber.Create(item.y4));
  }

  private void SwitchToQP()
  {
    if (this._qp != null)
      return;
    this._qp = PdfTypeArray.Create();
    PdfTypeDictionary.Create(this._link.Handle)["QuadPoints"] = (PdfTypeBase) this._qp;
  }
}
