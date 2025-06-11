// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfPageObjectsCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the collection of objects on a page.</summary>
public class PdfPageObjectsCollection : 
  IList<PdfPageObject>,
  ICollection<PdfPageObject>,
  IEnumerable<PdfPageObject>,
  IEnumerable,
  IDisposable
{
  private IntPtr _formContentForDel;
  private MgrPageObj _manager = new MgrPageObj();

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the Pdfium SDK handle that the collection is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfFormObject" /> that contains this collection of page objects.
  /// </summary>
  /// <value>The <see cref="T:Patagames.Pdf.Net.PdfFormObject" /> or null if the collection of page objects belongs directly to the page and not to a form object.</value>
  public PdfFormObject Form { get; private set; }

  /// <summary>
  /// Initializes a new instance of the PdfPageObjectsCollection class.
  /// </summary>
  /// <param name="page">Page which contains this collection of objects.</param>
  internal PdfPageObjectsCollection(PdfPage page)
  {
    this.Handle = page != null ? Pdfium.FPDFPageObj_GetListFromPage(page.Handle) : throw new ArgumentNullException();
    this.Form = (PdfFormObject) null;
  }

  /// <summary>
  /// Initializes a new instance of the PdfPageObjectsCollection class.
  /// </summary>
  /// <param name="form">the <see cref="T:Patagames.Pdf.Net.PdfFormObject" /> that contains this collection of objects.</param>
  internal PdfPageObjectsCollection(PdfFormObject form)
  {
    IntPtr formContent = form != null ? Pdfium.FPDFFormObj_GetFormContent(form.Handle) : throw new ArgumentNullException(nameof (form));
    this.Handle = !(formContent == IntPtr.Zero) ? Pdfium.FPDFPageObj_GetListFromForm(formContent) : throw new ArgumentException();
    this.Form = form;
  }

  /// <summary>
  /// Initializes a new instance of the PdfPageObjectsCollection class.
  /// </summary>
  /// <param name="doc"></param>
  /// <param name="resDict"></param>
  /// <param name="stream"></param>
  internal PdfPageObjectsCollection(PdfDocument doc, IntPtr resDict, IntPtr stream)
  {
    this._formContentForDel = Pdfium.FPDFFormContent_Create(doc.Handle, stream, resDict);
    this.Handle = Pdfium.FPDFPageObj_GetListFromForm(this._formContentForDel);
    if (this._formContentForDel == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    Pdfium.FPDFFormContent_Parse(this._formContentForDel);
    this.Form = (PdfFormObject) null;
  }

  /// <summary>Releases all resources used by the PdfPageCollection.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    for (int index = this.Count - 1; index >= 0; --index)
    {
      if (this[index] is IDisposable disposable)
        disposable.Dispose();
    }
    if (this._formContentForDel != IntPtr.Zero)
      Pdfium.FPDFFormContent_Delete(this._formContentForDel);
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> at the specified index.</returns>
  public PdfPageObject this[int index]
  {
    get
    {
      IntPtr Handle = index >= 0 && index < this.Count ? Pdfium.FPDFPageObjectList_GetObject(this.Handle, index) : throw new IndexOutOfRangeException();
      PdfPageObject pdfPageObject = !(Handle == IntPtr.Zero) ? this._manager.Create(Handle) : throw Pdfium.ProcessLastError();
      pdfPageObject.Container = this;
      return pdfPageObject;
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      this.RemoveAt(index);
      this.Insert(index, value);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> contained in the collection.
  /// </summary>
  public int Count
  {
    get => !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFPageObjectList_CountObject(this.Handle) : 0;
  }

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfPageObject" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfPageObject item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfPageObject item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> from the collection
  /// </summary>
  public void Clear()
  {
    for (int index = this.Count - 1; index >= 0; --index)
      this.RemoveAt(index);
    this._manager.Clear();
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfPageObject[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfPageObject pdfPageObject in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfPageObject;
    }
  }

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfPageObject" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfPageObject" />  to add to the collection</param>
  public void Add(PdfPageObject item) => this.Insert(this.Count, item);

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfPageObject> GetEnumerator()
  {
    return (IEnumerator<PdfPageObject>) new CollectionEnumerator<PdfPageObject>((IList<PdfPageObject>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfPageObject item)
  {
    int count = this.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this[index].Handle == item.Handle)
      {
        this.RemoveAt(index);
        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.PdfPageObject" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Net.PdfPageObject" />  to remove.</param>
  public void RemoveAt(int index)
  {
    IntPtr handle = index >= 0 && index < this.Count ? Pdfium.FPDFPageObjectList_GetObject(this.Handle, index) : throw new IndexOutOfRangeException();
    Pdfium.FPDFPageObjectList_RemoveObject(this.Handle, index);
    PdfPageObject pdfPageObject = this._manager.Get(handle);
    if (pdfPageObject == null)
      return;
    pdfPageObject.Container = (PdfPageObjectsCollection) null;
    this._manager.Remove(handle);
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> to insert into the collection.</param>
  /// <exception cref="T:System.NotSupportedException">The collection is read-only.</exception>
  public void Insert(int index, PdfPageObject item)
  {
    if (index < 0 || index > this.Count)
      throw new IndexOutOfRangeException();
    if (this.IsReadOnly)
      throw new NotSupportedException();
    Pdfium.FPDFPageObjectList_InsertObject(this.Handle, item.Handle, index);
    this._manager.Add(item);
    item.Container = this;
  }

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfPageObject" />  to the collection
  /// </summary>
  /// <param name="item"></param>
  [Obsolete("This method is obsolete. Please use Add instead.", false)]
  public void InsertObject(PdfPageObject item) => this.Add(item);

  /// <summary>
  /// Insert an object to the collection. The page object is automatically freed.
  /// </summary>
  /// <param name="item">Instance of <see cref="T:Patagames.Pdf.Net.PdfPageObject" /> class. It can be <see cref="T:Patagames.Pdf.Net.PdfImageObject" /> for example.</param>
  /// ;
  ///             <param name="insertAfter">After that index an object will be inserted. -1 for add at the collection's head</param>
  /// <remarks><note type="warning">If you insert the same objects multiple times, then an error will occur during disposing those objects.</note></remarks>
  [Obsolete("This method is obsolete. Please use Insert instead.", false)]
  public void InsertObject(PdfPageObject item, int insertAfter)
  {
    this.Insert(insertAfter + 1, item);
  }

  /// <summary>
  /// Calculate a bounding rectangle enclosing all objects in the collection.
  /// </summary>
  /// <returns>An FS_RECTF structure that represents the bounding box.</returns>
  public FS_RECTF CalcuateBoundingBox()
  {
    float l = float.MaxValue;
    float r = float.MinValue;
    float t = float.MinValue;
    float b = float.MaxValue;
    foreach (PdfPageObject pdfPageObject in this)
    {
      FS_RECTF boundingBox = pdfPageObject.BoundingBox;
      if ((double) l > (double) Math.Min(boundingBox.left, boundingBox.right))
        l = Math.Min(boundingBox.left, boundingBox.right);
      if ((double) r < (double) Math.Max(boundingBox.left, boundingBox.right))
        r = Math.Max(boundingBox.left, boundingBox.right);
      if ((double) b > (double) Math.Min(boundingBox.bottom, boundingBox.top))
        b = Math.Min(boundingBox.bottom, boundingBox.top);
      if ((double) t < (double) Math.Max(boundingBox.bottom, boundingBox.top))
        t = Math.Max(boundingBox.bottom, boundingBox.top);
    }
    return new FS_RECTF(l, t, r, b);
  }
}
