// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfDestinationCollections
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
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the collection of named destination insde a PDF document.
/// </summary>
public class PdfDestinationCollections : 
  IList<PdfDestination>,
  ICollection<PdfDestination>,
  IEnumerable<PdfDestination>,
  IEnumerable
{
  /// <summary>Pdf document</summary>
  private PdfDocument _doc;
  private ListObjectManager<IntPtr, PdfDestination> _mgr = new ListObjectManager<IntPtr, PdfDestination>();
  private PdfNameTreeCollection _nameTree;

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfDestinationCollections" /> class.
  /// </summary>
  /// <param name="document">Document which contains this collection of destinations.</param>
  internal PdfDestinationCollections(PdfDocument document)
  {
    this._doc = document;
    if (!Pdfium.IsFullAPI)
      return;
    this._nameTree = new PdfNameTreeCollection(this._doc, "Dests");
  }

  /// <summary>Get a special destination by the name.</summary>
  /// <param name="name">The name of a special named destination. Can't be NULL or empty string.</param>
  /// <returns>The instance of PdfDestination class.</returns>
  /// <remarks>This method have problem with non english national character encoding</remarks>
  public PdfDestination GetByName(string name)
  {
    if ((name ?? "").Trim() == "")
      throw new ArgumentException(Error.err0004, nameof (name));
    IntPtr namedDestByName = Pdfium.FPDF_GetNamedDestByName(this._doc.Handle, name);
    if (!(namedDestByName != IntPtr.Zero))
      return (PdfDestination) null;
    if (this._mgr.Contains(namedDestByName))
      return this._mgr.Get(namedDestByName);
    PdfDestination byName = new PdfDestination(this._doc, namedDestByName, name);
    this._mgr.Add(namedDestByName, byName);
    return byName;
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfDestination" /> from the collection.
  /// </summary>
  /// <param name="name">The <see cref="T:Patagames.Pdf.Net.PdfDestination" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfDestination" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfDestination" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(string name)
  {
    PdfDestination pdfDestination = name != null ? this[name] : throw new ArgumentNullException(nameof (name));
    return pdfDestination != null && this.Remove(pdfDestination);
  }

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfDestination" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfDestination" />  to add to the collection</param>
  /// <param name="name">The name of the <paramref name="item" /></param>
  public void Add(PdfDestination item, string name)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if ((name ?? "").Trim() == "")
      throw new ArgumentException(Error.err0004);
    if (this[name] != null)
      throw new ArgumentException(Error.err0061);
    this[name] = item;
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> by its name.
  /// </summary>
  /// <param name="name">The name of a special named destination. Can't be NULL or empty string.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfDestination" /> with the specified name.</returns>
  /// <remarks>This method have problem with non english national character encoding</remarks>
  public PdfDestination this[string name]
  {
    get => this.GetByName(name);
    set
    {
      if (!Pdfium.IsFullAPI)
        throw new NoLicenseException(536870917U /*0x20000005*/, Error.errLicense3);
      if (value == null)
        throw new ArgumentNullException();
      if (value.Array.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Array.Handle) != IntPtr.Zero)
        throw new ArgumentException(string.Format(Error.err0067, (object) "destination", (object) "object"));
      this._nameTree[name] = !((value.Name ?? "").Trim() != "") ? (PdfTypeBase) value.Array : throw new ArgumentException(string.Format(Error.err0067, (object) "named destination", (object) "object"));
      value.Name = name;
      this._mgr.Add(value.Handle, value);
      if (!this._doc.Root.ContainsKey("Dests") || !this._doc.Root["Dests"].Is<PdfTypeDictionary>() || !this._doc.Root["Dests"].As<PdfTypeDictionary>().ContainsKey(name))
        return;
      this._doc.Root["Dests"].As<PdfTypeDictionary>().Remove(name);
    }
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfDestination" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfDestination" /> at the specified index.</returns>
  public PdfDestination this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      string name;
      IntPtr namedDest = Pdfium.FPDF_GetNamedDest(this._doc.Handle, index, out name);
      if (namedDest == IntPtr.Zero)
        throw Pdfium.ProcessLastError();
      if (this._mgr.Contains(namedDest))
        return this._mgr.Get(namedDest);
      PdfDestination pdfDestination = new PdfDestination(this._doc, namedDest, name);
      this._mgr.Add(namedDest, pdfDestination);
      return pdfDestination;
    }
    set
    {
      PdfDestination pdfDestination = this[index];
      this[pdfDestination.Name] = value;
      this._mgr.Remove(pdfDestination.Handle);
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfDestination" /> contained in the collection.
  /// </summary>
  public int Count => Pdfium.FPDF_CountNamedDests(this._doc.Handle);

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.PdfDestination" /> from the collection
  /// </summary>
  public void Clear()
  {
    if (!Pdfium.IsFullAPI)
      throw new NoLicenseException(536870917U /*0x20000005*/, Error.errLicense3);
    this._nameTree.Clear();
    this._mgr.Clear();
    if (!this._doc.Root.ContainsKey("Dests"))
      return;
    this._doc.Root.Remove("Dests");
  }

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfDestination" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfDestination" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfDestination" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfDestination item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfDestination[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfDestination pdfDestination in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfDestination;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfDestination> GetEnumerator()
  {
    return (IEnumerator<PdfDestination>) new CollectionEnumerator<PdfDestination>((IList<PdfDestination>) this);
  }

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfDestination" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfDestination" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfDestination" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfDestination item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfDestination" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfDestination" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfDestination" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfDestination" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfDestination item)
  {
    if (!Pdfium.IsFullAPI)
      throw new NoLicenseException(536870917U /*0x20000005*/, Error.errLicense3);
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (item.Name == null)
      throw new ArgumentException(Error.err0004);
    this._mgr.Remove(item.Handle);
    int num = this._nameTree.IndexOf(item.Name);
    if (num >= 0)
      this._nameTree.Remove(item.Name);
    if (this._doc.Root.ContainsKey("Dests") && this._doc.Root["Dests"].Is<PdfTypeDictionary>() && this._doc.Root["Dests"].As<PdfTypeDictionary>().ContainsKey(item.Name))
    {
      num = 0;
      this._doc.Root["Dests"].As<PdfTypeDictionary>().Remove(item.Name);
    }
    if (num < 0)
      return false;
    item.Name = (string) null;
    return true;
  }

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.PdfDestination" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Net.PdfDestination" />  to remove.</param>
  public void RemoveAt(int index) => this.Remove(this[index]);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfDestination" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfDestination" />  to add to the collection</param>
  [Obsolete("This method is not supported. Please use Add(PdfDestination item, string name) instead.", true)]
  public void Add(PdfDestination item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Net.PdfDestination" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Net.PdfDestination" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfDestination" /> to insert into the collection.</param>
  [Obsolete("This method is not supported. Please use Add(PdfDestination item, string name) instead.", true)]
  public void Insert(int index, PdfDestination item)
  {
    throw new NotSupportedException(Error.err0051);
  }
}
