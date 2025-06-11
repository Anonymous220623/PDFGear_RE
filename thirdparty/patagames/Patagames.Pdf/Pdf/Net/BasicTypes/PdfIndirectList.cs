// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfIndirectList
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the list of indirect objects</summary>
public class PdfIndirectList : IEnumerable<PdfTypeBase>, IEnumerable
{
  private MgrIntObj _manager = new MgrIntObj();

  /// <summary>
  /// Gets the Pdfium SDK handle that the object is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets the number of the last object in the list.</summary>
  public int LastObjectNum => Pdfium.FPDFHOLDER_GetLastObjNum(this.Handle);

  /// <summary>
  /// Gets or sets the object from the list with the specified number
  /// </summary>
  /// <param name="number">The object's number to get or set.</param>
  /// <returns>The object with the specified number.</returns>
  /// <exception cref="T:System.ArgumentNullException">value is null.</exception>
  public PdfTypeBase this[int number]
  {
    get => this._manager.Create(Pdfium.FPDFHOLDER_GetIndirectObject(this.Handle, number));
    set
    {
      if (value == null)
        throw new ArgumentNullException();
      Pdfium.FPDFHOLDER_InsertIndirectObject(this.Handle, number, value != null ? value.Handle : IntPtr.Zero);
      this._manager.Add(value);
    }
  }

  /// <summary>Gets the number of elements contained in the list.</summary>
  public uint Count => Pdfium.FPDFHOLDER_GetCount(this.Handle);

  /// <summary>
  /// Construct new instance of PdfIndirectList class from given Handle
  /// </summary>
  /// <param name="Handle">Handle to the list of object</param>
  internal PdfIndirectList(IntPtr Handle) => this.Handle = Handle;

  /// <summary>
  /// Gets the list of indirect objects from specified PDF document
  /// </summary>
  /// <param name="doc">A PDF document</param>
  /// <returns>The list of Indirect objects or null if any error is occured.</returns>
  public static PdfIndirectList FromPdfDocument(PdfDocument doc)
  {
    if (doc == null)
      return (PdfIndirectList) null;
    IntPtr Handle = Pdfium.FPDFHOLDER_FromPdfDocument(doc.Handle);
    return !(Handle == IntPtr.Zero) ? new PdfIndirectList(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>
  /// Gets the list of indirect objects from specified Forms Data Format document
  /// </summary>
  /// <param name="fdf">A Forms Data Format document</param>
  /// <returns>The list of Indirect objects or null if any error is occured.</returns>
  public static PdfIndirectList FromFdfDocument(FdfDocument fdf)
  {
    if (fdf == null)
      return (PdfIndirectList) null;
    IntPtr Handle = Pdfium.FPDFHOLDER_FromFdfDocument(fdf.Handle);
    return !(Handle == IntPtr.Zero) ? new PdfIndirectList(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Adds an object to the end of the list.</summary>
  /// <param name="indirectObject">The object to add to the list.</param>
  /// <returns>Return the number of oject which was added.</returns>
  /// <exception cref="T:System.ArgumentNullException">value is null.</exception>
  public int Add(PdfTypeBase indirectObject)
  {
    if (indirectObject == null)
      throw new ArgumentNullException(nameof (indirectObject));
    int num = Pdfium.FPDFHOLDER_AddIndirectObject(this.Handle, indirectObject != null ? indirectObject.Handle : IntPtr.Zero);
    this._manager.Add(indirectObject);
    return num;
  }

  /// <summary>
  /// Destroy specified object and remove its from the list.
  /// </summary>
  /// <param name="objectNumber">The object's number to remove from the list.</param>
  public void Remove(int objectNumber)
  {
    PdfTypeBase pdfTypeBase = this[objectNumber];
    IntPtr handle = IntPtr.Zero;
    if (pdfTypeBase != null)
      handle = pdfTypeBase.Handle;
    Pdfium.FPDFHOLDER_ReleaseIndirectObject(this.Handle, objectNumber);
    if (!(handle != IntPtr.Zero))
      return;
    this._manager.Remove(handle);
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator`1 that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfTypeBase> GetEnumerator()
  {
    return (IEnumerator<PdfTypeBase>) new PdfIndirectListEnumerator(this);
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator`1 that can be used to iterate through the collection.</returns>
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
