// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfRefObjectsCollection
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
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents a collection of referred objects.</summary>
public class PdfRefObjectsCollection : 
  IList<REFOBJ>,
  ICollection<REFOBJ>,
  IEnumerable<REFOBJ>,
  IEnumerable,
  IDisposable
{
  private PdfDocument _doc;
  private int _count;
  private ListObjectManager<IntPtr, REFOBJ> _mgr = new ListObjectManager<IntPtr, REFOBJ>();

  /// <summary>
  /// Gets the Pdfium SDK handle that the collection is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// Construct new instance of <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfRefObjectsCollection" /> class from given Handle
  /// </summary>
  /// <param name="doc">A PDF document</param>
  /// <param name="Handle">Handle to the list of object</param>
  /// <param name="Count">Number of items in the given collection</param>
  internal PdfRefObjectsCollection(PdfDocument doc, IntPtr Handle, int Count)
  {
    this.Handle = Handle;
    this._count = Count;
    this._doc = doc;
  }

  /// <summary>
  /// Gets the list of referenced objets from the catalog of the specified PDF document
  /// </summary>
  /// <param name="doc">A PDF document</param>
  /// <param name="parsedOnly">Flag indicating that only previously parsed objects should be examined.</param>
  /// <param name="root">The root object from which to start parsing the document.</param>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException">
  /// <para>This method cannot accept newly created documents. If you pass such a document as an argument, the <see cref="T:Patagames.Pdf.Net.Exceptions.RequiredDataIsAbsentException" /> will be thrown.</para>
  /// <para>To get the instance of the PdfDocument class that can be passed to the FromPdfDocument method, you must save the document to a temporary file or an array of bytes, and then open it with the <see cref="O:Patagames.Pdf.Net.PdfDocument.Load" />.</para>
  /// </exception>
  /// <returns>Instance of <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfRefObjectsCollection" /> class</returns>
  public static PdfRefObjectsCollection FromPdfDocument(
    PdfDocument doc,
    bool parsedOnly = false,
    PdfTypeBase root = null)
  {
    try
    {
      if (doc == null)
        throw new ArgumentNullException();
      int count;
      IntPtr refsToObjects = Pdfium.FPDFHOLDER_GetRefsToObjects(doc.Handle, out count, parsedOnly, root == null ? IntPtr.Zero : root.Handle);
      if (refsToObjects == IntPtr.Zero)
        throw Pdfium.ProcessLastError();
      return new PdfRefObjectsCollection(doc, refsToObjects, count);
    }
    catch (RequiredDataIsAbsentException ex)
    {
      throw new RequiredDataIsAbsentException(Error.err0057);
    }
  }

  /// <summary>
  /// Dispose <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfRefObjectsCollection" />
  /// </summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfRefObjectsCollection" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDFHOLDER_ReleaseRefsToObjects(this.Handle);
    this.Handle = IntPtr.Zero;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfRefObjectsCollection()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && this.Handle != IntPtr.Zero)
      throw new MemoryLeakException(nameof (PdfRefObjectsCollection));
  }

  /// <summary>
  /// Determines whether the collection contains an object with specified object number/&gt;.
  /// </summary>
  /// <param name="objectNumber">The object number to locate in the collection.</param>
  /// <returns>true if object with specified number is found in the collection; otherwise, false.</returns>
  public bool Contains(int objectNumber) => this.IndexOf(objectNumber) >= 0;

  /// <summary>
  /// Determines the index of an object with specific object number in the collection.
  /// </summary>
  /// <param name="objectNumber">The object number to locate in the collection</param>
  /// <returns>The index of  object with specified object number if found in the collection; otherwise, -1.</returns>
  public int IndexOf(int objectNumber)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].ObjectNumber == objectNumber)
        return index;
    }
    return -1;
  }

  /// <summary>Gets the object with specified object number</summary>
  /// <param name="objectNumber">The object number to locate in the collection.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.REFOBJ" /> with the specified object number.</returns>
  public REFOBJ GetBy(int objectNumber)
  {
    int index = this.IndexOf(objectNumber);
    return index >= 0 ? this[index] : (REFOBJ) null;
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.REFOBJ" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.REFOBJ" /> at the specified index.</returns>
  public REFOBJ this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      IntPtr num = new IntPtr(this.Handle.ToInt64() + (long) (REFOBJ._unsafeSize * index));
      REFOBJ refobj = this._mgr.Get(num);
      if (refobj == null)
      {
        refobj = new REFOBJ(this._doc, num);
        this._mgr.Add(num, refobj);
      }
      return refobj;
    }
    set => throw new NotSupportedException(Error.err0051);
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.REFOBJ" /> contained in the collection.
  /// </summary>
  public int Count => this._count;

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => true;

  /// <summary>
  /// Adds a <see cref="T:Patagames.Pdf.REFOBJ" />  to the collection
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.REFOBJ" />  to add to the collection</param>
  public void Add(REFOBJ item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes all <see cref="T:Patagames.Pdf.REFOBJ" /> from the collection
  /// </summary>
  public void Clear() => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.REFOBJ" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.REFOBJ" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.REFOBJ" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(REFOBJ item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(REFOBJ[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (REFOBJ refobj in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = refobj;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<REFOBJ> GetEnumerator()
  {
    return (IEnumerator<REFOBJ>) new CollectionEnumerator<REFOBJ>((IList<REFOBJ>) this);
  }

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.REFOBJ" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.REFOBJ" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.REFOBJ" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(REFOBJ item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Inserts a <see cref="T:Patagames.Pdf.REFOBJ" /> to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which <see cref="T:Patagames.Pdf.REFOBJ" /> should be inserted.</param>
  /// <param name="item">The <see cref="T:Patagames.Pdf.REFOBJ" /> to insert into the collection.</param>
  public void Insert(int index, REFOBJ item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes the first occurrence of a specific <see cref="T:Patagames.Pdf.REFOBJ" /> from the collection.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.REFOBJ" /> to remove from the collection.</param>
  /// <returns>
  /// true if <see cref="T:Patagames.Pdf.REFOBJ" /> was successfully removed from the collection;
  /// otherwise, false. This method also returns false if <see cref="T:Patagames.Pdf.REFOBJ" /> is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(REFOBJ item) => throw new NotSupportedException(Error.err0051);

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.REFOBJ" />  at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the <see cref="T:Patagames.Pdf.REFOBJ" />  to remove.</param>
  public void RemoveAt(int index) => throw new NotSupportedException(Error.err0051);

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
