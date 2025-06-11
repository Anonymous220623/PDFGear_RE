// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfScreenAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents Screen annotation</summary>
/// <remarks>
/// A screen annotation specifies a region of a page upon which media clips
/// may be played.It also serves as an object from which actions can be triggered.
/// <note type="note">This annotation is currently not supported by the SDK.</note>
/// </remarks>
public class PdfScreenAnnotation : PdfAnnotation
{
  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfScreenAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfScreenAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Screen");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfScreenAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfScreenAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
