// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfWebLinkCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents collection of <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> objects.
/// </summary>
/// <remarks>
/// Weblinks are those links implicitly embedded in PDF pages. PDF also has a type of annotation
/// called "link", FPDFTEXT doesn't deal with that kind of link. FPDFTEXT weblink feature is
/// useful for automatically detecting links in the page contents. For example, things like
/// "http://www.patagames.com" will be detected, so applications can allow user to click on those
/// characters to activate the link, even the PDF doesn't come with link annotations.
/// <see cref="M:Patagames.Pdf.Net.PdfWebLinkCollection.Dispose" /> must be called to release resources.
/// </remarks>
public class PdfWebLinkCollection : 
  IList<PdfWebLink>,
  ICollection<PdfWebLink>,
  IEnumerable<PdfWebLink>,
  IEnumerable,
  IDisposable
{
  private ListObjectManager<int, PdfWebLink> _mgr = new ListObjectManager<int, PdfWebLink>();

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the collection has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the Pdfium SDK handle that the web links is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Find a web link at specified point on a document page</summary>
  /// <param name="point">The point, specified in page coordinate system</param>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.PdfLink" /> class that represent the found link. Null if there’s no link at that point.</returns>
  /// <remarks>The point coordinate are specified in page coordinate system.
  /// You can convert coordinate from screen system to page system using <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function.</remarks>
  public PdfWebLink GetWebLinkAtPoint(FS_POINTF point) => this.GetWebLinkAtPoint(point.X, point.Y);

  /// <summary>Find a web link at specified point on a document page</summary>
  /// <param name="x">The x coordinate of the point, specified in page coordinate system</param>
  /// <param name="y">The y coordinate of the point, specified in page coordinate system</param>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.PdfLink" /> class that represent the found link. Null if there’s no link at that point.</returns>
  /// <remarks>The point coordinate are specified in page coordinate system.
  /// You can convert coordinate from screen system to page system using <see cref="M:Patagames.Pdf.Net.PdfPage.DeviceToPage(System.Int32,System.Int32,System.Int32,System.Int32,Patagames.Pdf.Enums.PageRotate,System.Int32,System.Int32)" /> function.</remarks>
  public PdfWebLink GetWebLinkAtPoint(float x, float y)
  {
    foreach (PdfWebLink webLinkAtPoint in this)
    {
      foreach (FS_RECTF rect in webLinkAtPoint.UrlInfo.Rects)
      {
        if ((double) x >= (double) rect.left && (double) x <= (double) rect.right && (double) y >= (double) rect.bottom && (double) y <= (double) rect.top)
          return webLinkAtPoint;
      }
    }
    return (PdfWebLink) null;
  }

  internal PdfWebLinkCollection(PdfText textPage)
  {
    this.Handle = Pdfium.FPDFLink_LoadWebLinks(textPage.Handle);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
  }

  /// <summary>Releases all resources used by the PdfDocument.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDFLink_CloseWebLinks(this.Handle);
    this.Handle = IntPtr.Zero;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfWebLinkCollection()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfWebLinkCollection));
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> at the specified index.</returns>
  public PdfWebLink this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      if (!this._mgr.Contains(index))
        this._mgr.Add(index, new PdfWebLink(this, index));
      return this._mgr.Get(index);
    }
    set => throw new NotSupportedException(Error.err0051);
  }

  /// <summary>
  /// Gets the number of of detected web links actually contained in the collections.
  /// </summary>
  public int Count => Pdfium.FPDFLink_CountWebLinks(this.Handle);

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => true;

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.Net.PdfWebLink" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfWebLink" />  to add to the collection</param>
  public void Add(PdfWebLink item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> from the collection
  /// </summary>
  public void Clear() => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfWebLink" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfWebLink item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfWebLink[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfWebLink pdfWebLink in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfWebLink;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfWebLink> GetEnumerator()
  {
    return (IEnumerator<PdfWebLink>) new CollectionEnumerator<PdfWebLink>((IList<PdfWebLink>) this);
  }

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfWebLink item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> to insert into the collection.</param>
  public void Insert(int index, PdfWebLink item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfWebLink item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.PdfWebLink" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.Net.PdfWebLink" />  to remove.</param>
  public void RemoveAt(int index) => throw new NotSupportedException(Error.err0051);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
