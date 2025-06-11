// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfCrossReferenceTable
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the cross-reference table</summary>
/// <remarks>
/// The cross-reference table contains information that permits random access to
/// indirect objects within the file so that the entire file need not be read to locate any
/// particular object. The table contains a one-line entry for each indirect object,
/// specifying the location of that object within the body of the file. (Beginning with
/// PDF 1.5, some or all of the cross-reference information may alternatively be
/// contained in cross-reference streams;)
/// </remarks>
public class PdfCrossReferenceTable : IEnumerable<PdfCrossRefItem>, IEnumerable
{
  /// <summary>
  /// Gets the Pdfium SDK handle that the object is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets the number of elements contained in the list.</summary>
  public uint Count => Pdfium.FPDFCROSSREF_GetCount(this.Handle);

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfCrossRefItem" /> with specified object number.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfCrossRefItem" /> at the specified index.</returns>
  public PdfCrossRefItem this[int index]
  {
    get
    {
      if (index < 0 || (long) index >= (long) this.Count)
        throw new ArgumentOutOfRangeException();
      int num = 0;
      foreach (PdfCrossRefItem pdfCrossRefItem in this)
      {
        if (num++ == index)
          return pdfCrossRefItem;
      }
      throw new ArgumentOutOfRangeException();
    }
  }

  /// <summary>
  /// Construct new instance of PdfCrossReferenceTable class from given Handle
  /// </summary>
  /// <param name="Handle">Handle to the PDF document</param>
  internal PdfCrossReferenceTable(IntPtr Handle) => this.Handle = Handle;

  /// <summary>
  /// Gets the cross reference table from specified PDF document
  /// </summary>
  /// <param name="doc">A PDF document</param>
  /// <returns>The new instance of list PdfCrossReferenceTable.</returns>
  public static PdfCrossReferenceTable FromPdfDocument(PdfDocument doc)
  {
    return doc == null ? (PdfCrossReferenceTable) null : new PdfCrossReferenceTable(doc.Handle);
  }

  /// <summary>Removes specified object from the table.</summary>
  /// <param name="objectNumber">The object's number to remove from the table.</param>
  public void Remove(int objectNumber) => Pdfium.FPDFCROSSREF_Remove(this.Handle, objectNumber);

  /// <summary>Rebuild cross-reference table.</summary>
  public void Rebuild() => Pdfium.FPDFCROSSREF_Rebuild(this.Handle);

  /// <summary>Shrink the table up to specified object.</summary>
  /// <param name="objectNumber">The object's number to remove from the table.</param>
  public void Shrink(int objectNumber) => Pdfium.FPDFCROSSREF_Shrink(this.Handle, objectNumber);

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator`1 that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfCrossRefItem> GetEnumerator()
  {
    return (IEnumerator<PdfCrossRefItem>) new PdfCrossReferenceTableEnumerator(this);
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator`1 that can be used to iterate through the collection.</returns>
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
