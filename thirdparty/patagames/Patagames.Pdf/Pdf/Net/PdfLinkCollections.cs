// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfLinkCollections
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the collection of named destination insde a PDF document.
/// </summary>
public class PdfLinkCollections : 
  IList<PdfLink>,
  ICollection<PdfLink>,
  IEnumerable<PdfLink>,
  IEnumerable
{
  /// <summary>Pdf document</summary>
  private PdfPage _page;
  private ListObjectManager<IntPtr, PdfLink> _mgr = new ListObjectManager<IntPtr, PdfLink>();

  /// <summary>
  /// Initializes a new instance of the PdfLinkCollections class.
  /// </summary>
  /// <param name="page">Page which contains this collection of links.</param>
  internal PdfLinkCollections(PdfPage page)
  {
    this._page = page;
    int start_pos = 0;
    IntPtr link_annot = IntPtr.Zero;
    while (Pdfium.FPDFLink_Enumerate(this._page.Handle, ref start_pos, out link_annot))
      this._mgr.Add(link_annot, new PdfLink(this._page, link_annot));
  }

  /// <summary>Find a link at specified point on a document page</summary>
  /// <param name="x">The x coordinate of the point, specified in page coordinate system</param>
  /// <param name="y">The y coordinate of the point, specified in page coordinate system</param>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.PdfLink" /> class that represent the found link. Null if there’s no link at that point.</returns>
  /// <remarks>The point coordinate are specified in page coordinate system.
  /// You can convert coordinate from screen system to page system using <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function.</remarks>
  public PdfLink GetLinkAtPoint(float x, float y)
  {
    IntPtr linkAtPoint = Pdfium.FPDFLink_GetLinkAtPoint(this._page.Handle, (double) x, (double) y);
    return linkAtPoint == IntPtr.Zero ? (PdfLink) null : this._mgr.Get(linkAtPoint);
  }

  /// <summary>Find a link at specified point on a document page</summary>
  /// <param name="point">The <see cref="T:Patagames.Pdf.FS_POINTF" /> structure that represents point, specified in page coordinate system</param>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.PdfLink" /> class that represents the found link. Null if there’s no link at that point.</returns>
  /// <remarks>The point coordinate are specified in page coordinate system.
  /// You can convert coordinate from screen system to page system using <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function.</remarks>
  public PdfLink GetLinkAtPoint(FS_POINTF point) => this.GetLinkAtPoint(point.X, point.Y);

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfLink" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfLink" /> at the specified index.</returns>
  public PdfLink this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this._mgr.GetByIndex(index) : throw new ArgumentOutOfRangeException();
    }
    set => throw new NotSupportedException(Error.err0051);
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfLink" /> contained in the collection.
  /// </summary>
  public int Count => this._mgr.Count;

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => true;

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfLink" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfLink" />  to add to the collection</param>
  public void Add(PdfLink item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.PdfLink" /> from the collection
  /// </summary>
  public void Clear() => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfLink" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfLink" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfLink" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfLink item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfLink[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfLink pdfLink in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfLink;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfLink> GetEnumerator()
  {
    return (IEnumerator<PdfLink>) new CollectionEnumerator<PdfLink>((IList<PdfLink>) this);
  }

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfLink" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfLink" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfLink" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfLink item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Net.PdfLink" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Net.PdfLink" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfLink" /> to insert into the collection.</param>
  public void Insert(int index, PdfLink item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfLink" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfLink" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfLink" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfLink" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfLink item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.PdfLink" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Net.PdfLink" />  to remove.</param>
  public void RemoveAt(int index) => throw new NotSupportedException(Error.err0051);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
