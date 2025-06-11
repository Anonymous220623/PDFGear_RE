// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfAnnotationCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a collection of annotation.</summary>
/// <remarks>
/// Each annotation contained in the collection may be referenced from the collection associated with only one page. Attempting to share an annotation among multiple pages produces unpredictable behavior. This requirement applies only to the annotation itself, not to subsidiary objects, which can be shared among multiple annotations without causing any difficulty.
/// </remarks>
public class PdfAnnotationCollection : 
  IList<PdfAnnotation>,
  ICollection<PdfAnnotation>,
  IEnumerable<PdfAnnotation>,
  IEnumerable,
  IDisposable
{
  private PdfTypeArray _annots;
  private PdfIndirectList _list;
  private Dictionary<IntPtr, PdfAnnotation> _mgr = new Dictionary<IntPtr, PdfAnnotation>();

  /// <summary>
  /// Gets a value indicating whether the underlying array has been disposed of.
  /// </summary>
  /// <value>true if object has been disposed of; otherwise, false.</value>
  public bool IsDisposed => this._annots.IsDisposed;

  /// <summary>
  /// Gets page with which this collection is assosiated.Gets page with which this collection is associated. All annotations added to the collection should be associated with this page only.
  /// </summary>
  public PdfPage Page { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotationCollection" /> which will be associated with the specified page.
  /// </summary>
  /// <param name="page">The page that will be associated with this instance.</param>
  internal PdfAnnotationCollection(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (page.Document == null)
      throw new ArgumentNullException("page.Document");
    this.Page = page.Dictionary != null ? page : throw new ArgumentNullException("page.Dictionary");
    this._list = PdfIndirectList.FromPdfDocument(this.Page.Document);
    if (page.Dictionary.ContainsKey("Annots"))
    {
      PdfTypeBase pdfTypeBase = page.Dictionary["Annots"];
      if (pdfTypeBase is PdfTypeArray)
        this._annots = pdfTypeBase as PdfTypeArray;
      else if (pdfTypeBase is PdfTypeIndirect)
        this._annots = (pdfTypeBase as PdfTypeIndirect).Direct as PdfTypeArray;
      if (this._annots == null)
        throw new PdfParserException(Error.err0036);
    }
    else
    {
      this._annots = PdfTypeArray.Create();
      int num = this._list.Add((PdfTypeBase) this._annots);
      page.Dictionary["Annots"] = num > 0 ? (PdfTypeBase) PdfTypeIndirect.Create(this._list, num) : throw new UnknownErrorException(Error.err0037);
    }
  }

  /// <summary>Gets annotations associated with the given page</summary>
  /// <param name="page">The page you want to get annotations for.</param>
  /// <returns>The instance of <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotationCollection" />, ornull if the page does not contain any annotation.</returns>
  internal static PdfAnnotationCollection GetAnnotations(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (page.Document == null)
      throw new ArgumentNullException("page.Document");
    if (page.Dictionary == null)
      throw new ArgumentNullException("page.Dictionary");
    if (!page.Dictionary.ContainsKey("Annots"))
      return (PdfAnnotationCollection) null;
    return page.Dictionary["Annots"].Is<PdfTypeNull>() ? (PdfAnnotationCollection) null : new PdfAnnotationCollection(page);
  }

  /// <summary>
  /// Creates an empty collection of annotations for specified page.
  /// </summary>
  /// <param name="page">The page you want to create annotations for.</param>
  /// <returns>Not empty collection if any annotations already exist on the page, empty collection otherwise.</returns>
  internal static PdfAnnotationCollection Create(PdfPage page) => new PdfAnnotationCollection(page);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotationCollection" />.
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.Annotations.PdfAnnotationCollection" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    for (int index = this.Count - 1; index >= 0; --index)
    {
      PdfAnnotation annotation = this.MgrGetAnnotation(index);
      if ((PdfWrapper) annotation != (PdfWrapper) null)
        annotation.Dispose();
    }
    this._annots.Dispose();
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Gets or sets the annotation at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The annotation at the specified index.</returns>
  public PdfAnnotation this[int index]
  {
    get
    {
      if (index < 0 || index >= this._annots.Count)
        throw new IndexOutOfRangeException();
      return this.MgrCreateAnnotation(index);
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null)
        throw new ArgumentNullException();
      if (this._list.Add((PdfTypeBase) value.Dictionary) <= 0)
        throw new UnknownErrorException(Error.err0037);
      this._annots.SetAt(index, (PdfTypeBase) value.Dictionary, this._list);
      this.MgrAdd(value);
    }
  }

  /// <summary>
  /// Gets the number of annotations contained in the collection.
  /// </summary>
  public int Count => this._annots.Count;

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>Adds an annotation to the collection</summary>
  /// <param name="item">The annotation to add to the collection</param>
  public void Add(PdfAnnotation item)
  {
    if ((PdfWrapper) item == (PdfWrapper) null)
      throw new ArgumentNullException();
    if (this._list.Add((PdfTypeBase) item.Dictionary) <= 0)
      throw new UnknownErrorException(Error.err0037);
    this._annots.Add((PdfTypeBase) item.Dictionary, this._list);
    this.MgrAdd(item);
  }

  /// <summary>Removes all annotations from the collection</summary>
  public void Clear()
  {
    this._annots.Clear();
    this.MgrClear();
  }

  /// <summary>
  /// Determines whether the collection contains a specific annotation.
  /// </summary>
  /// <param name="item">The annotation to locate in the collection.</param>
  /// <returns>true if annotation is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfAnnotation item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific annotation in the collection.
  /// </summary>
  /// <param name="item">The annotation to locate in the collection</param>
  /// <returns>The index of annotation if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfAnnotation item)
  {
    if ((PdfWrapper) item == (PdfWrapper) null)
      throw new ArgumentNullException(nameof (item));
    int num = 0;
    foreach (PdfTypeBase annot in this._annots)
    {
      switch (annot)
      {
        case PdfTypeDictionary _ when annot.Handle == item.Dictionary.Handle:
          return num;
        case PdfTypeIndirect _:
          PdfTypeIndirect pdfTypeIndirect = annot as PdfTypeIndirect;
          if (pdfTypeIndirect.Direct is PdfTypeDictionary && (pdfTypeIndirect.Direct as PdfTypeDictionary).Handle == item.Dictionary.Handle)
            return num;
          break;
      }
      ++num;
    }
    return -1;
  }

  /// <summary>
  /// Inserts an annotation to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which annotation should be inserted.</param>
  /// <param name="item">The annotation to insert into the collection.</param>
  public void Insert(int index, PdfAnnotation item)
  {
    if ((PdfWrapper) item == (PdfWrapper) null)
      throw new ArgumentNullException();
    if (this._list.Add((PdfTypeBase) item.Dictionary) <= 0)
      throw new UnknownErrorException(Error.err0037);
    this._annots.Insert(index, (PdfTypeBase) item.Dictionary, this._list);
    this.MgrAdd(item);
  }

  /// <summary>
  /// Removes the first occurrence of a specific annotation from the collection.
  /// </summary>
  /// <param name="item">The annotation to remove from the collection.</param>
  /// <returns>
  /// true if annotation was successfully removed from the collection;
  /// otherwise, false. This method also returns false if annotation is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfAnnotation item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>Removes the annotation at the specified index.</summary>
  /// <param name="index">The zero-based index of the annotation to remove.</param>
  public void RemoveAt(int index)
  {
    this.MgrRemove(index);
    this._annots.RemoveAt(index);
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfAnnotation[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    foreach (PdfAnnotation pdfAnnotation in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfAnnotation;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfAnnotation> GetEnumerator()
  {
    return (IEnumerator<PdfAnnotation>) new CollectionEnumerator<PdfAnnotation>((IList<PdfAnnotation>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Determines whether the collection contains a specific annotation.
  /// </summary>
  /// <param name="dictionary">The annotation dictionary to locate in the collection.</param>
  /// <returns>The annotation if found in the collection; otherwise, null</returns>
  public PdfAnnotation GetByDictionary(PdfTypeBase dictionary)
  {
    if (dictionary == null || dictionary.IsDisposed)
      return (PdfAnnotation) null;
    if (dictionary is PdfTypeIndirect)
      dictionary = (dictionary as PdfTypeIndirect).Direct;
    if (dictionary == null || dictionary.IsDisposed)
      return (PdfAnnotation) null;
    foreach (PdfAnnotation byDictionary in this)
    {
      if (!byDictionary.Dictionary.IsDisposed && byDictionary.Dictionary.Handle == dictionary.Handle)
        return byDictionary;
    }
    return (PdfAnnotation) null;
  }

  private bool IsEqualTypes(PdfTypeBase dict1, PdfTypeBase dict2)
  {
    if (dict1 == null || dict2 == null || !dict1.Is<PdfTypeDictionary>() || !dict2.Is<PdfTypeDictionary>())
      return false;
    PdfTypeDictionary pdfTypeDictionary1 = dict1.As<PdfTypeDictionary>();
    PdfTypeDictionary pdfTypeDictionary2 = dict2.As<PdfTypeDictionary>();
    return pdfTypeDictionary1.ContainsKey("Subtype") && pdfTypeDictionary2.ContainsKey("Subtype") && (pdfTypeDictionary1["Subtype"] as PdfTypeName).Value == (pdfTypeDictionary2["Subtype"] as PdfTypeName).Value;
  }

  private PdfAnnotation MgrGetAnnotation(int index)
  {
    PdfTypeDictionary pdfTypeDictionary = this._annots[index].As<PdfTypeDictionary>(false);
    if (pdfTypeDictionary != null && this._mgr.ContainsKey(pdfTypeDictionary.Handle))
    {
      PdfAnnotation annotation = this._mgr[pdfTypeDictionary.Handle];
      if (!annotation.Dictionary.IsDisposed)
        return annotation;
      this._mgr.Remove(pdfTypeDictionary.Handle);
    }
    return (PdfAnnotation) null;
  }

  private PdfAnnotation MgrCreateAnnotation(int index)
  {
    PdfAnnotation annotation1 = this.MgrGetAnnotation(index);
    if ((PdfWrapper) annotation1 != (PdfWrapper) null && this.IsEqualTypes((PdfTypeBase) annotation1.Dictionary, this._annots[index]))
      return annotation1;
    PdfAnnotation annotation2 = PdfAnnotation.Create(this._annots[index], this.Page);
    this._mgr.Add(annotation2.Dictionary.Handle, annotation2);
    return annotation2;
  }

  private void MgrAdd(PdfAnnotation item)
  {
    if (this._mgr.ContainsKey(item.Dictionary.Handle))
      return;
    this._mgr.Add(item.Dictionary.Handle, item);
  }

  private void MgrRemove(int index)
  {
    PdfAnnotation annotation = this.MgrGetAnnotation(index);
    if (!((PdfWrapper) annotation != (PdfWrapper) null))
      return;
    this._mgr.Remove(annotation.Dictionary.Handle);
  }

  private void MgrClear() => this._mgr.Clear();
}
