// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfWebLink
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents information about weblinks in a page</summary>
public class PdfWebLink
{
  /// <summary>Gets the URL information for a detected web link.</summary>
  /// <returns>URL information for a detected web link</returns>
  public string Url { get; private set; }

  /// <summary>
  /// Gets the URL information (including bounding rects) for a detected web link.
  /// </summary>
  /// <returns>URL information for a detected web link</returns>
  public PdfUrlInfo UrlInfo { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfWebLink" /> class.
  /// </summary>
  /// <param name="links">Web links collection.</param>
  /// <param name="index">Zero-based index for the link in the specified collection</param>
  internal PdfWebLink(PdfWebLinkCollection links, int index)
  {
    this.Url = Pdfium.FPDFLink_GetURL(links.Handle, index);
    this.UrlInfo = new PdfUrlInfo(links.Handle, index);
  }
}
