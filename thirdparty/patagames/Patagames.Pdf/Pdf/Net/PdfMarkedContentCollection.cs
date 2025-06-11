// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfMarkedContentCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the collection of <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> objects.
/// </summary>
/// <remarks>This list is read only.</remarks>
public class PdfMarkedContentCollection : 
  IList<PdfMarkedContent>,
  ICollection<PdfMarkedContent>,
  IEnumerable<PdfMarkedContent>,
  IEnumerable
{
  private PdfPageObject _pageObject;

  /// <summary>
  /// Initialize a new instance of <see cref="T:Patagames.Pdf.Net.PdfMarkedContentCollection" /> class.
  /// </summary>
  /// <param name="pageObject"><see cref="T:Patagames.Pdf.Net.PdfPageObject" /></param>
  public PdfMarkedContentCollection(PdfPageObject pageObject) => this._pageObject = pageObject;

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> at the specified index.</returns>
  public PdfMarkedContent this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      string markName = Pdfium.FPDFPageObj_GetMarkName(this._pageObject.Handle, index);
      PropertyListTypes markParamType = Pdfium.FPDFPageObj_GetMarkParamType(this._pageObject.Handle, index);
      IntPtr markParam = Pdfium.FPDFPageObj_GetMarkParam(this._pageObject.Handle, index);
      bool flag = Pdfium.FPDFPageObj_MarkHasMCID(this._pageObject.Handle, index);
      PdfTypeDictionary pdfTypeDictionary = (PdfTypeDictionary) null;
      if (markParam != IntPtr.Zero)
        pdfTypeDictionary = new PdfTypeDictionary(markParam);
      int num = flag ? 1 : 0;
      int paramType = (int) markParamType;
      PdfTypeDictionary parameters = pdfTypeDictionary;
      return new PdfMarkedContent(markName, num != 0, (PropertyListTypes) paramType, parameters);
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      if (value != null)
        Pdfium.FPDFPageObj_SetMark(this._pageObject.Handle, index, value.Tag, value.ParamType, value.Parameters != null ? value.Parameters.Handle : IntPtr.Zero);
      else
        Pdfium.FPDFPageObj_SetMark(this._pageObject.Handle, index, "", PropertyListTypes.None, IntPtr.Zero);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> contained in the collection.
  /// </summary>
  public int Count => Pdfium.FPDFPageObj_CountMarks(this._pageObject.Handle);

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfMarkedContent item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfMarkedContent item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> from the collection
  /// </summary>
  public void Clear()
  {
    int count = this.Count;
    for (int index = 0; index < count; ++index)
      Pdfium.FPDFPageObj_DeleteLastMark(this._pageObject.Handle);
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfMarkedContent[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfMarkedContent pdfMarkedContent in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfMarkedContent;
    }
  }

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" />  to add to the collection</param>
  public void Add(PdfMarkedContent item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    Pdfium.FPDFPageObj_AddMark(this._pageObject.Handle, item.Tag, item.Parameters != null ? item.Parameters.Handle : IntPtr.Zero, item.ParamType == PropertyListTypes.DirectDict);
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfMarkedContent> GetEnumerator()
  {
    return (IEnumerator<PdfMarkedContent>) new CollectionEnumerator<PdfMarkedContent>((IList<PdfMarkedContent>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfMarkedContent item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" />  to remove.</param>
  public void RemoveAt(int index)
  {
    int count = this.Count;
    if (index < 0 || index >= count)
      throw new ArgumentOutOfRangeException();
    if (count <= 1)
      this.Clear();
    else if (index == count - 1)
    {
      Pdfium.FPDFPageObj_DeleteLastMark(this._pageObject.Handle);
    }
    else
    {
      for (int index1 = index; index1 < count - 1; ++index1)
        this[index1] = this[index1 + 1];
      Pdfium.FPDFPageObj_DeleteLastMark(this._pageObject.Handle);
    }
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfMarkedContent" /> to insert into the collection.</param>
  public void Insert(int index, PdfMarkedContent item)
  {
    int count = this.Count;
    if (index < 0 || index > count)
      throw new ArgumentOutOfRangeException();
    if (index == count)
    {
      this.Add(item);
    }
    else
    {
      this.Add(item);
      for (int index1 = this.Count - 1; index1 > index; --index1)
        this[index1] = this[index1 - 1];
      this[index] = item;
    }
  }
}
