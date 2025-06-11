// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfLinkAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>
/// A link annotation represents either a hypertext link to a destination elsewhere in the document
/// or an action to be performed.
/// </summary>
public class PdfLinkAnnotation : PdfAnnotation
{
  /// <summary>
  /// Get a <see cref="T:Patagames.Pdf.Net.PdfLink" /> associated with this annotation.
  /// </summary>
  public PdfLink Link { get; private set; }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinkAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfLinkAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create(nameof (Link));
    this.Link = new PdfLink(this.Page, this.Dictionary.Handle);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinkAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfLinkAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
    this.Link = new PdfLink(this.Page, this.Dictionary.Handle);
  }
}
