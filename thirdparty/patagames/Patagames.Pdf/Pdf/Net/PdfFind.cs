// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFind
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a search context</summary>
public class PdfFind : IDisposable
{
  private IntPtr _searchHandle;
  private PdfText _text;

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>Gets found text information object</summary>
  [Obsolete("Please use FoundText instead", false)]
  public PdfTextInfo FindedText => this._text.GetTextInfo(this.CharIndex, this.CharsCount);

  /// <summary>Gets found text information object</summary>
  public PdfTextInfo FoundText => this._text.GetTextInfo(this.CharIndex, this.CharsCount);

  /// <summary>
  /// Gets the starting character index of the search result.
  /// </summary>
  public int CharIndex => Pdfium.FPDFText_GetSchResultIndex(this._searchHandle);

  /// <summary>
  /// Gets the number of matched characters in the search result
  /// </summary>
  public int CharsCount => Pdfium.FPDFText_GetSchCount(this._searchHandle);

  /// <summary>Search in the direction from page start to end.</summary>
  /// <returns>Whether a match is found</returns>
  public bool FindNext() => Pdfium.FPDFText_FindNext(this._searchHandle);

  /// <summary>Search in the direction from page end to start.</summary>
  /// <returns>Whether a match is found.</returns>
  public bool FindPrev() => Pdfium.FPDFText_FindPrev(this._searchHandle);

  private PdfFind(IntPtr searchHandle, PdfText text)
  {
    this._searchHandle = searchHandle;
    this._text = text;
  }

  /// <summary>Start a search.</summary>
  /// <param name="findWhat">A string match pattern</param>
  /// <param name="text">PdfText object.</param>
  /// <param name="flags">Option flags. See <see cref="T:Patagames.Pdf.Enums.FindFlags" /> for details </param>
  /// <param name="startIndex">Start from this character. -1 for end of the page</param>
  /// <returns>A object fot represents the search context.</returns>
  public static PdfFind Find(string findWhat, PdfText text, FindFlags flags, int startIndex)
  {
    IntPtr start = Pdfium.FPDFText_FindStart(text.Handle, findWhat, flags, startIndex);
    if (start == IntPtr.Zero)
      return (PdfFind) null;
    if (Pdfium.FPDFText_FindNext(start))
      return new PdfFind(start, text);
    Pdfium.FPDFText_FindClose(start);
    return (PdfFind) null;
  }

  /// <summary>Release a search context</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    if (this._searchHandle != IntPtr.Zero)
      Pdfium.FPDFText_FindClose(this._searchHandle);
    this._searchHandle = IntPtr.Zero;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfFind()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfFind));
  }
}
