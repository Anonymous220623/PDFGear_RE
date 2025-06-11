// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfCircleAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents the circle annotation</summary>
/// <remarks>
/// See details in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfFigureAnnotation" />
/// </remarks>
public class PdfCircleAnnotation : PdfFigureAnnotation
{
  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfCircleAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfCircleAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Circle");
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfCircleAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="rect">The rectangle where an annotation should be placed.</param>
  /// <param name="strokeColor">The stroke color of a square</param>
  /// <param name="fillColor">Color of the fill of a square annotation.</param>
  /// <remarks>
  /// The  opacity of the annotation will be equal to the Alpha component  of the fill color.
  /// If it is equal to 0 then to the Alpha component of the of the stroke color.
  /// </remarks>
  public PdfCircleAnnotation(
    PdfPage page,
    FS_RECTF rect,
    FS_COLOR strokeColor,
    FS_COLOR fillColor)
    : this(page)
  {
    this.Flags = AnnotationFlags.Print;
    this.Opacity = (fillColor.A == 0 ? (float) strokeColor.A : (float) fillColor.A) / (float) byte.MaxValue;
    this.Color = strokeColor;
    this.InteriorColor = fillColor;
    this.Rectangle = rect;
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfCircleAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfCircleAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
