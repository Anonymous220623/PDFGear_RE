// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfTextMarkupAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>
/// Represents highlight, underline, squiggly and strikeout annotations
/// </summary>
/// <remarks>
/// Text markup annotations appear as highlights, underlines, strikeouts (all PDF 1.3),
/// or jagged(“squiggly”) underlines(PDF 1.4) in the text of a document.
/// When opened, they display a pop-up window containing the text of the associated note.
/// </remarks>
public abstract class PdfTextMarkupAnnotation : PdfMarkupAnnotation
{
  private PdfQuadPointsCollection _quadPoints;

  /// <summary>
  /// Gets or sets a list of <see cref="T:Patagames.Pdf.FS_QUADPOINTSF" /> structures specifying the coordinates of quadrilaterals in default user space.
  /// </summary>
  /// <remarks>
  /// Each quadrilateral encompasses a word or group of contiguous words in the text underlying the annotation.
  /// The coordinates for each quadrilateral are given in the four vertices in counterclockwise order (see Figure 8.9).
  /// The text is oriented with respect to the edge connecting points (x1, y1) and (x2, y2).
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over QuadPoints property</note>
  /// 
  /// <list type="table">
  /// <item><term><img src="../Media/Figure8.9QuadPoints.png" /></term></item>
  /// <listheader>
  /// <term>FIGURE 8.9 QuadPoints specification</term>
  /// </listheader>
  /// </list>
  /// </remarks>
  public PdfQuadPointsCollection QuadPoints
  {
    get
    {
      if (!this.IsExists(nameof (QuadPoints)))
        return (PdfQuadPointsCollection) null;
      if (this._quadPoints == null || this._quadPoints.QuadPoints.IsDisposed)
        this._quadPoints = new PdfQuadPointsCollection(this.Dictionary[nameof (QuadPoints)].As<PdfTypeArray>());
      return this._quadPoints;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey(nameof (QuadPoints)))
        this.Dictionary.Remove(nameof (QuadPoints));
      else if (value != null)
        this.Dictionary[nameof (QuadPoints)] = (PdfTypeBase) value.QuadPoints;
      this._quadPoints = value;
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextMarkupAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfTextMarkupAnnotation(PdfPage page)
    : base(page)
  {
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextMarkupAnnotation" /> with quadrilateral parameters as specified.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="leftBottom">Left-bottom point of the quadrilateral point.</param>
  /// <param name="rightTop">Right-top point of the quadrilateral point.</param>
  /// <param name="rightBottom">Right-bottom point of the quadrilateral point.</param>
  /// <param name="leftTop">Left-top point of the quadrilateral point.</param>
  /// <param name="Color">The color of this annotation.</param>
  public PdfTextMarkupAnnotation(
    PdfPage page,
    FS_POINTF leftTop,
    FS_POINTF rightTop,
    FS_POINTF rightBottom,
    FS_POINTF leftBottom,
    FS_COLOR Color)
    : this(page)
  {
    PdfTypeArray quadPoints = PdfTypeArray.Create();
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(leftTop.X));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(leftTop.Y));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(rightTop.X));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(rightTop.Y));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(leftBottom.X));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(leftBottom.Y));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(rightBottom.X));
    quadPoints.Add((PdfTypeBase) PdfTypeNumber.Create(rightBottom.Y));
    this.QuadPoints = new PdfQuadPointsCollection(quadPoints);
    this.Color = Color;
    if (Color.A < (int) byte.MaxValue)
      this.Opacity = (float) Color.A / (float) byte.MaxValue;
    this.RegenerateAppearances();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfTextMarkupAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  /// <summary>
  /// Re-creates the appearance of the annotation based on its properties
  /// </summary>
  /// <remarks>
  /// When the annotation does not have an Appearance stream (old style annotations),
  /// they are not rendered by the Pdfium engine.
  /// Calling this function creates an appearance stream based on the default parameters and the properties of this annotation.
  /// </remarks>
  public override void RegenerateAppearances()
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    FS_COLOR fsColor = new FS_COLOR(this.Opacity, this.Color);
    List<PdfPathObject> pdfPathObjectList;
    switch (this)
    {
      case PdfSquigglyAnnotation _:
        pdfPathObjectList = AnnotDrawing.CreateSquiggly(fsColor, this.QuadPoints, 4f, 1.7f);
        break;
      case PdfStrikeoutAnnotation _:
        pdfPathObjectList = AnnotDrawing.CreateUnderlineStrikeout(fsColor, this.QuadPoints, 43f, 6f);
        break;
      case PdfUnderlineAnnotation _:
        pdfPathObjectList = AnnotDrawing.CreateUnderlineStrikeout(fsColor, this.QuadPoints, 14.1f, 6f);
        break;
      default:
        pdfPathObjectList = AnnotDrawing.CreateHighlight(fsColor, this.QuadPoints);
        break;
    }
    foreach (PdfPageObject pdfPageObject in pdfPathObjectList)
      this.NormalAppearance.Add(pdfPageObject);
    this.GenerateAppearance(AppearanceStreamModes.Normal);
  }
}
