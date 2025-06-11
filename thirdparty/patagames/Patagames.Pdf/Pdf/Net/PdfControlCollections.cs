// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfControlCollections
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

/// <summary>Represents a collection of widget annotations</summary>
public class PdfControlCollections : 
  IReadOnlyList<PdfControl>,
  IEnumerable<PdfControl>,
  IEnumerable,
  ICollection<PdfControl>
{
  private PdfInteractiveForms _forms;
  private PdfField _field;
  private PdfPage _page;
  private ListObjectManager<IntPtr, PdfControl> _mgr = new ListObjectManager<IntPtr, PdfControl>();
  private List<PdfControl> _newlyCreated = new List<PdfControl>();

  /// <summary>
  /// Initializes a new instance of the control collections for specified form object
  /// </summary>
  /// <param name="forms">Interactive forms</param>
  /// <remarks>
  /// this collection can contain widgets for total forms, for field or for page. Its depends by a specified parameters.
  /// </remarks>
  internal PdfControlCollections(PdfInteractiveForms forms)
  {
    this._forms = forms != null ? forms : throw new ArgumentNullException(nameof (forms));
    this._field = (PdfField) null;
    this._page = (PdfPage) null;
  }

  /// <summary>
  /// Initializes a new instance of the control collections for specified field
  /// </summary>
  /// <param name="forms">Interactive forms</param>
  /// <param name="field">PDF Field object</param>
  /// <remarks>
  /// this collection can contain widgets for total forms, for field or for page. Its depends by a specified parameters.
  /// </remarks>
  internal PdfControlCollections(PdfInteractiveForms forms, PdfField field)
  {
    this._forms = forms != null ? forms : throw new ArgumentNullException(nameof (forms));
    this._field = field;
    this._page = (PdfPage) null;
  }

  /// <summary>
  /// Initializes a new instance of the control collections for specified page
  /// </summary>
  /// <param name="forms">Interactive forms</param>
  /// <param name="page">PDF Page object</param>
  /// <remarks>
  /// this collection can contain widgets for total forms, for field or for page. Its depends by a specified parameters.
  /// </remarks>
  internal PdfControlCollections(PdfInteractiveForms forms, PdfPage page)
  {
    this._forms = forms != null ? forms : throw new ArgumentNullException(nameof (forms));
    this._field = (PdfField) null;
    this._page = page;
  }

  /// <summary>
  /// Search collection for the widget with the specified dictionary.
  /// </summary>
  /// <param name="dict">Widget annotaion's dictionary</param>
  /// <returns>Found widget or null if nothing found.</returns>
  public PdfControl GetControlByDict(PdfTypeDictionary dict)
  {
    return dict == null ? (PdfControl) null : this.GetByHandle(Pdfium.FPDFInterForm_GetControlByDict(this._forms.Handle, dict.Handle));
  }

  /// <summary>
  /// Search collection for the widget with the specified handle.
  /// </summary>
  /// <param name="handle">Widget annotaion's handle</param>
  /// <returns>Found widget or null if nothing found.</returns>
  public PdfControl GetByHandle(IntPtr handle)
  {
    if (handle == IntPtr.Zero)
      return (PdfControl) null;
    if (this._mgr.Contains(handle))
      return this._mgr.Get(handle);
    PdfControl byHandle = PdfControl.Create(this._forms, handle);
    this._mgr.Add(handle, byHandle);
    return byHandle;
  }

  /// <summary>Get the control that currently has input focus.</summary>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfControl" /> that currently has input focus; or null.</returns>
  public PdfControl GetFocused()
  {
    IntPtr focusAnnot = Pdfium.FORM_GetFocusAnnot(this._forms._fillforms.Handle);
    return focusAnnot != IntPtr.Zero ? this.GetControlByDict(PdfTypeDictionary.Create(focusAnnot)) : (PdfControl) null;
  }

  /// <summary>
  /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire System.Collections.Generic.List`1.
  /// </summary>
  /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
  /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">match is null.</exception>
  public PdfControl Find(Predicate<PdfControl> match)
  {
    if (match == null)
      throw new ArgumentNullException(nameof (match));
    foreach (PdfControl pdfControl in this)
    {
      if (match(pdfControl))
        return pdfControl;
    }
    return (PdfControl) null;
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfControl" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfControl" /> at the specified index.</returns>
  public PdfControl this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      IntPtr zero = IntPtr.Zero;
      IntPtr handle;
      if (this._page != null)
      {
        handle = Pdfium.FPDFInterForm_GetPageControl(this._forms.Handle, this._page.Handle, index);
      }
      else
      {
        if (this._field != null && this._field.Handle == IntPtr.Zero)
          return this._newlyCreated[index];
        handle = this._field == null ? Pdfium.FPDFInterForm_GetControl(this._forms.Handle, index, (string) null) : Pdfium.FPDFFormField_GetControl(this._field.Handle, index);
      }
      return this.GetByHandle(handle);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfControl" /> contained in the collection.
  /// </summary>
  public int Count
  {
    get
    {
      if (this._page != null)
        return Pdfium.FPDFInterForm_CountPageControls(this._forms.Handle, this._page.Handle);
      if (this._field != null && this._field.Handle == IntPtr.Zero)
        return this._newlyCreated.Count;
      return this._field != null ? Pdfium.FPDFFormField_CountControls(this._field.Handle) : Pdfium.FPDFInterForm_CountControls(this._forms.Handle);
    }
  }

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => true;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfControl" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfControl" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfControl" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfControl item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfControl" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfControl" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfControl" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfControl item)
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
  public void CopyTo(PdfControl[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfControl pdfControl in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfControl;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfControl> GetEnumerator()
  {
    return (IEnumerator<PdfControl>) new CollectionEnumerator<PdfControl>((IReadOnlyList<PdfControl>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  internal void AddNewItem(PdfControl pdfControl) => this._newlyCreated.Add(pdfControl);

  internal void AfterLoadField()
  {
    foreach (PdfControl pdfControl in this._newlyCreated)
    {
      pdfControl.AfterLoadField();
      if (pdfControl.Handle != IntPtr.Zero)
        this._mgr.Add(pdfControl.Handle, pdfControl);
    }
    this._newlyCreated.Clear();
  }

  /// <summary>This method is not supported.</summary>
  public void Add(PdfControl item) => throw new NotSupportedException();

  /// <summary>
  /// Removes the first occurrence of a specific control from the <see cref="T:Patagames.Pdf.Net.PdfControlCollections" />
  /// </summary>
  /// <param name="item">The control to remove from the <see cref="T:Patagames.Pdf.Net.PdfControlCollections" /></param>
  /// <returns>true if <paramref name="item" /> was successfully removed from <see cref="T:Patagames.Pdf.Net.PdfControlCollections" />;
  /// otherwise, false. This method also returns false if item is not found in the original <see cref="T:Patagames.Pdf.Net.PdfControlCollections" />.</returns>
  public bool Remove(PdfControl item)
  {
    IntPtr key = item != null ? item.Handle : throw new ArgumentNullException(nameof (item));
    item.RemoveFromDom();
    this._mgr.Remove(key);
    this._forms.ReloadForms();
    return true;
  }

  /// <summary>
  /// Removes all items from the <see cref="T:Patagames.Pdf.Net.PdfControlCollections" />.
  /// </summary>
  public void Clear()
  {
    foreach (PdfControl pdfControl in this)
      pdfControl.RemoveFromDom();
    this._mgr.Clear();
    this._forms.ReloadForms();
  }
}
