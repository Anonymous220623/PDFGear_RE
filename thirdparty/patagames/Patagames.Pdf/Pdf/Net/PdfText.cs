// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfText
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the text information for the current page</summary>
public class PdfText : IDisposable
{
  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// <value>true if the control has been disposed of; otherwise, false.</value>
  /// </summary>
  public bool IsDisposed { get; private set; }

  /// <summary>
  /// Gets the Pdfium SDK handle that the PdfText is bound to
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>
  /// <para>Gets number of characters in a page or -1 for error.</para>
  /// <para>Generated characters, like additional space characters, new line characters, are also counted.</para>
  /// </summary>
  /// <remarks><para>Characters in a page form a "stream", inside the stream, each character has an index.
  /// We will use the index parameters in many functions.</para>
  /// <para>The first character in the page has an index value of zero.</para></remarks>
  public int CountChars
  {
    get => !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFText_CountChars(this.Handle) : 0;
  }

  /// <summary>
  /// Gets the instance of PdfWebLinkCollection class that represents the web link on current page
  /// </summary>
  /// <remarks>See remarks section of <see cref="T:Patagames.Pdf.Net.PdfWebLinkCollection" /> class for more details.</remarks>
  public PdfWebLinkCollection WebLinks { get; private set; }

  /// <summary>Initializes a new instance of the PdfText class.</summary>
  /// <param name="pdfPage">Instance of <see cref="T:Patagames.Pdf.Net.PdfPage" /> class</param>
  internal PdfText(PdfPage pdfPage)
  {
    this.Handle = Pdfium.FPDFText_LoadPage(pdfPage.Handle);
    if (this.Handle == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    this.WebLinks = new PdfWebLinkCollection(this);
  }

  /// <summary>Releases all resources used by the PdfText.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize)
      return;
    this.WebLinks.Dispose();
    if (this.Handle != IntPtr.Zero)
      Pdfium.FPDFText_ClosePage(this.Handle);
    this.Handle = IntPtr.Zero;
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PdfText()
  {
    if (PdfCommon.IsCheckForMemoryLeaks && !this.IsDisposed)
      throw new MemoryLeakException(nameof (PdfText));
  }

  /// <summary>Get Unicode of a character in a page</summary>
  /// <param name="index">Zero-based index of the character</param>
  /// <returns>The the particular character. If a character is not encoded in Unicode and engine can't convert to Unicode,
  /// the return value will be zero character (\0).</returns>
  public char GetCharacter(int index) => Pdfium.FPDFText_GetUnicodeEx(this.Handle, index);

  /// <summary>Get the font size of a particular character</summary>
  /// <param name="index">Zero-based index of the character.</param>
  /// <returns>The font size of the particular character, measured in points (about 1/72 inch). This is the typographic size of the font (so called "em size").</returns>
  public float GetFontSize(int index) => (float) Pdfium.FPDFText_GetFontSize(this.Handle, index);

  /// <summary>Get bounding box of a particular character.</summary>
  /// <param name="index">Zero-based index of the character.</param>
  /// <remarks>All positions are measured in PDF "user space"</remarks>
  /// <returns>Bounding box of a particular character.</returns>
  public FS_RECTF GetCharBox(int index)
  {
    double left;
    double right;
    double bottom;
    double top;
    Pdfium.FPDFText_GetCharBox(this.Handle, index, out left, out right, out bottom, out top);
    return new FS_RECTF()
    {
      bottom = (float) bottom,
      left = (float) left,
      right = (float) right,
      top = (float) top
    };
  }

  /// <summary>
  /// Get the index of a character at or nearby a certain position on the page
  /// </summary>
  /// <param name="x">X position in PDF "user space"</param>
  /// <param name="y">Y position in PDF "user space"</param>
  /// <param name="xTolerance">An x-axis tolerance value for character hit detection, in point unit.</param>
  /// <param name="yTolerance">A y-axis tolerance value for character hit detection, in point unit</param>
  /// <returns>The zero-based index of the character at, or nearby the point (x,y).
  /// If there is no character at or nearby the point, return value will be -1.
  /// If an error occurs, -3 will be returned.
  /// </returns>
  public int GetCharIndexAtPos(float x, float y, float xTolerance, float yTolerance)
  {
    return Pdfium.FPDFText_GetCharIndexAtPos(this.Handle, (double) x, (double) y, (double) xTolerance, (double) yTolerance);
  }

  /// <summary>Extract text string from the page</summary>
  /// <param name="index">Index for the start characters</param>
  /// <param name="count">Number of characters to be extracted</param>
  /// <returns>Text string from the page</returns>
  /// <remarks>This function ignores characters without unicode information</remarks>
  public string GetText(int index, int count) => Pdfium.FPDFText_GetText(this.Handle, index, count);

  /// <summary>Extract text information structure from the page</summary>
  /// <param name="index">Index for the start characters</param>
  /// <param name="count">Number of characters to be extracted</param>
  /// <returns>The instane of <see cref="T:Patagames.Pdf.Net.PdfText" /> class that represent text information of the current page</returns>
  /// <remarks>This function ignores characters without unicode information</remarks>
  public PdfTextInfo GetTextInfo(int index, int count) => new PdfTextInfo(this, index, count);

  /// <summary>
  /// Extract text within a rectangular boundary on the page specified by a coordinate pair, a width, and a height.
  /// </summary>
  /// <param name="left">Left boundary</param>
  /// <param name="top">Top boundary</param>
  /// <param name="right">Right boundary</param>
  /// <param name="bottom">Bottom boundary.</param>
  /// <returns>Text within a rectangular boundary on the page</returns>
  public string GetBoundedText(float left, float top, float right, float bottom)
  {
    return Pdfium.FPDFText_GetBoundedText(this.Handle, (double) left, (double) top, (double) right, (double) bottom);
  }

  /// <summary>
  /// Extract text within a rectangular boundary on the page specified by a <see cref="T:Patagames.Pdf.FS_RECTF" /> structure.
  /// </summary>
  /// <param name="rect">Rectangle boundary</param>
  /// <returns>Text within a rectangular boundary on the page</returns>
  public string GetBoundedText(FS_RECTF rect)
  {
    return this.GetBoundedText(rect.left, rect.top, rect.right, rect.bottom);
  }

  /// <summary>Start a search.</summary>
  /// <param name="findWhat">A string match pattern</param>
  /// <param name="flags">ption flags. See <see cref="T:Patagames.Pdf.Enums.FindFlags" /> for details.</param>
  /// <param name="startIndex">Start from this character. -1 for end of the page.</param>
  /// <returns>A object fot represents the search context or null if nothing found.</returns>
  public PdfFind Find(string findWhat, FindFlags flags, int startIndex)
  {
    return PdfFind.Find(findWhat, this, flags, startIndex);
  }
}
