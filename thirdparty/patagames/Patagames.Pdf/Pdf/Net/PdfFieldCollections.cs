// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFieldCollections
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a collection of fields in document</summary>
public class PdfFieldCollections : 
  IReadOnlyList<PdfField>,
  IEnumerable<PdfField>,
  IEnumerable,
  ICollection<PdfField>
{
  private PdfInteractiveForms _forms;
  private ListObjectManager<IntPtr, PdfField> _mgr = new ListObjectManager<IntPtr, PdfField>();

  /// <summary>
  /// Initializes a new instance of the field collections for specified form object
  /// </summary>
  /// <param name="forms">Interactive forms</param>
  internal PdfFieldCollections(PdfInteractiveForms forms) => this._forms = forms;

  /// <summary>
  /// Search collection for the field with the specified dictionary.
  /// </summary>
  /// <param name="dict">Field's dictionary</param>
  /// <returns>Found field or null if nothing found.</returns>
  public PdfField GetFieldByDict(PdfTypeDictionary dict)
  {
    if (dict == null)
      return (PdfField) null;
    IntPtr fieldByDict1 = Pdfium.FPDFInterForm_GetFieldByDict(this._forms.Handle, dict.Handle);
    if (fieldByDict1 == IntPtr.Zero)
      return (PdfField) null;
    if (this._mgr.Contains(fieldByDict1))
      return this._mgr.Get(fieldByDict1);
    PdfField fieldByDict2 = PdfField.Create(this._forms, fieldByDict1);
    this._mgr.Add(fieldByDict1, fieldByDict2);
    return fieldByDict2;
  }

  /// <summary>
  /// Search collection for the field with the specified handle.
  /// </summary>
  /// <param name="handle">Field's handle</param>
  /// <returns>Found field or null if nothing found.</returns>
  public PdfField GetByHandle(IntPtr handle)
  {
    if (handle == IntPtr.Zero)
      return (PdfField) null;
    if (this._mgr.Contains(handle))
      return this._mgr.Get(handle);
    PdfField byHandle = PdfField.Create(this._forms, handle);
    this._mgr.Add(handle, byHandle);
    return byHandle;
  }

  /// <summary>
  /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire System.Collections.Generic.List`1.
  /// </summary>
  /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
  /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">match is null.</exception>
  public PdfField Find(Predicate<PdfField> match)
  {
    if (match == null)
      throw new ArgumentNullException(nameof (match));
    foreach (PdfField pdfField in this)
    {
      if (match(pdfField))
        return pdfField;
    }
    return (PdfField) null;
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfField" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfField" /> at the specified index.</returns>
  public PdfField this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      IntPtr field = Pdfium.FPDFInterForm_GetField(this._forms.Handle, index);
      if (field == IntPtr.Zero)
        throw Pdfium.ProcessLastError();
      if (this._mgr.Contains(field))
        return this._mgr.Get(field);
      PdfField pdfField = PdfField.Create(this._forms, field);
      this._mgr.Add(field, pdfField);
      return pdfField;
    }
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfField" /> with the specified <paramref name="name" />
  /// </summary>
  /// <param name="name">The <see cref="P:Patagames.Pdf.Net.PdfField.FullName" /> of the field to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfField" /> with the specified <paramref name="name" />; null if found nothing.</returns>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="name" /> is null.</exception>
  public PdfField this[string name]
  {
    get
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return this.GetByHandle(Pdfium.FPDFInterForm_GetField(this._forms.Handle, 0, name));
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfField" /> contained in the collection.
  /// </summary>
  public int Count => Pdfium.FPDFInterForm_CountFields(this._forms.Handle);

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfField" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfField" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfField" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfField item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfField" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfField" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfField" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfField item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].Handle == item.Handle)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfField[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfField pdfField in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfField;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfField> GetEnumerator()
  {
    return (IEnumerator<PdfField>) new CollectionEnumerator<PdfField>((IReadOnlyList<PdfField>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds a form field to the <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" />.
  /// </summary>
  /// <param name="item">The form field to add to the <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" />.</param>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> is null.</exception>
  public void Add(PdfField item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (item.Handle != IntPtr.Zero)
      return;
    if (!item.IsTerminalField())
      throw new DomException(Error.err0073);
    if (this._forms.Dictionary == null)
      Pdfium.FPDFInterForm_InitEmptyFormDict(item.InterForms.Handle);
    if (!this._forms.Dictionary.ContainsKey("Fields") || !this._forms.Dictionary["Fields"].Is<PdfTypeArray>())
      this._forms.Dictionary["Fields"] = (PdfTypeBase) PdfTypeArray.Create();
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(this._forms.FillForms.Document);
    list.Add((PdfTypeBase) item.Dictionary);
    PdfTypeBase fieldAttribute = item.GetFieldAttribute("Parent");
    PdfTypeDictionary pdfTypeDictionary = fieldAttribute == null || !fieldAttribute.Is<PdfTypeDictionary>() ? (PdfTypeDictionary) null : fieldAttribute.As<PdfTypeDictionary>();
    if (pdfTypeDictionary != null)
    {
      if (pdfTypeDictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(pdfTypeDictionary.Handle) != IntPtr.Zero)
        throw new ArgumentException(string.Format(Error.err0067, (object) "parent field", (object) "object"));
      list.Add((PdfTypeBase) pdfTypeDictionary);
      if (!this._forms.ContainsField(pdfTypeDictionary))
        this._forms.Dictionary["Fields"].As<PdfTypeArray>().AddIndirect(list, (PdfTypeBase) pdfTypeDictionary);
    }
    else if (!this._forms.ContainsField(item.Dictionary))
      this._forms.Dictionary["Fields"].As<PdfTypeArray>().AddIndirect(list, (PdfTypeBase) item.Dictionary);
    Pdfium.FPDFInterForm_LoadField(this._forms.Handle, item.Dictionary.Handle);
    IntPtr fieldByDict = Pdfium.FPDFInterForm_GetFieldByDict(this._forms.Handle, item.Dictionary.Handle);
    item.UpdateHandle(fieldByDict);
    item.Controls.AfterLoadField();
  }

  /// <summary>
  /// Removes the first occurrence of a specific form field from the <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" />
  /// </summary>
  /// <param name="item">The form field to remove from the <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" /></param>
  /// <returns>true if <paramref name="item" /> was successfully removed from <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" />;
  /// otherwise, false. This method also returns false if item is not found in the original <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" />.</returns>
  public bool Remove(PdfField item)
  {
    IntPtr key = item != null ? item.Handle : throw new ArgumentNullException(nameof (item));
    if (!this._forms.RemoveField(item))
      return false;
    this._mgr.Remove(key);
    this._forms.ReloadForms();
    return true;
  }

  /// <summary>
  /// Removes all items from the <see cref="T:Patagames.Pdf.Net.PdfFieldCollections" />.
  /// </summary>
  public void Clear()
  {
    foreach (PdfField pdfField in this)
    {
      foreach (PdfControl control in pdfField.Controls)
        control.RemoveFromDom();
      pdfField.UpdateHandle(IntPtr.Zero);
    }
    this._mgr.Clear();
    if (this._forms.Dictionary != null)
      this._forms.Dictionary.Remove("Fields");
    this._forms.ReloadForms();
  }
}
