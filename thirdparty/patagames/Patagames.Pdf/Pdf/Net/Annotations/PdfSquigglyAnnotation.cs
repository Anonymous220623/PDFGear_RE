// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfSquigglyAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents the squiggly annotation</summary>
/// <remarks>
/// Please find details in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextMarkupAnnotation" />
/// </remarks>
public class PdfSquigglyAnnotation : PdfTextMarkupAnnotation
{
  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSquigglyAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfSquigglyAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Squiggly");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfSquigglyAnnotation" /> with quadrilateral parameters as specified.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="leftBottom">Left-bottom point of the quadrilateral.</param>
  /// <param name="rightTop">Right-top point of the quadrilateral.</param>
  /// <param name="rightBottom">Right-bottom point of the quadrilateral.</param>
  /// <param name="leftTop">Left-top point of the quadrilateral.</param>
  /// <param name="Color">The color of this annotation.</param>
  public PdfSquigglyAnnotation(
    PdfPage page,
    FS_POINTF leftTop,
    FS_POINTF rightTop,
    FS_POINTF rightBottom,
    FS_POINTF leftBottom,
    FS_COLOR Color)
    : base(page, leftTop, rightTop, rightBottom, leftBottom, Color)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Squiggly");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfSquigglyAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfSquigglyAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
