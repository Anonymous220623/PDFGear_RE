// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfTrapNetAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a Trap Network annotation</summary>
/// <remarks>
/// A trap network annotation defines the trapping characteristics for a page of a PDF document.
/// (Trapping is the process of adding marks to a page along color boundaries to avoid unwanted visual artifacts resulting
/// from misregistration of colorants when the page is printed.)
/// <note type="note">This annotation type is currently not supported by the SDK.</note>
/// </remarks>
public class PdfTrapNetAnnotation : PdfAnnotation
{
  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.Pdf3DAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfTrapNetAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("TrapNet");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfTrapNetAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfTrapNetAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
