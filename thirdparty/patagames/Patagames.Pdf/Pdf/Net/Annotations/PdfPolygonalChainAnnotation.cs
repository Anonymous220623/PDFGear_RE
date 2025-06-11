// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents polygon or polyline annotations</summary>
/// <remarks>
/// Polygon annotations (PDF 1.5) display closed polygons on the page.
/// Such polygons may have any number of vertices connected by straight lines.
/// Polyline annotations (PDF 1.5) are similar to polygons, except that the first and last vertex are not implicitly connected.
/// </remarks>
public abstract class PdfPolygonalChainAnnotation : PdfMarkupAnnotation
{
  private PdfLinePointCollection<PdfPolygonalChainAnnotation> _vertices;
  private PdfBorderStyle _lineStyle;

  /// <summary>
  /// Gets or sets a collection of the <see cref="T:Patagames.Pdf.FS_POINTF" /> specifying the coordinates and style of each vertex, in default user space.
  /// </summary>
  public PdfLinePointCollection<PdfPolygonalChainAnnotation> Vertices
  {
    get
    {
      if (!this.IsExists(nameof (Vertices)))
        return (PdfLinePointCollection<PdfPolygonalChainAnnotation>) null;
      if (this._vertices == null || this._vertices.IsDisposed)
        this._vertices = new PdfLinePointCollection<PdfPolygonalChainAnnotation>(this.Dictionary[nameof (Vertices)].As<PdfTypeArray>());
      return this._vertices;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey(nameof (Vertices)))
        this.Dictionary.Remove(nameof (Vertices));
      else if (value != null)
        this.Dictionary[nameof (Vertices)] = (PdfTypeBase) value.LinePoints;
      this._vertices = value;
    }
  }

  /// <summary>
  /// Gets or sets a line style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width and dash pattern to be used in drawing the line.
  /// </summary>
  /// <remarks>
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over the <see cref="P:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation.Vertices" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation.LineStyle" /> properties.</note>
  /// <note type="note">If the value of LineStyle property is null, the line is drawn as a solid line with a width of 1 point.</note>
  /// </remarks>
  public PdfBorderStyle LineStyle
  {
    get
    {
      if (!this.IsExists("BS"))
        return (PdfBorderStyle) null;
      if ((PdfWrapper) this._lineStyle == (PdfWrapper) null || this._lineStyle.Dictionary.IsDisposed)
        this._lineStyle = new PdfBorderStyle(this.Dictionary["BS"]);
      return this._lineStyle;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("BS"))
        this.Dictionary.Remove("BS");
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary["BS"] = (PdfTypeBase) value.Dictionary;
      this._lineStyle = value;
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfPolygonalChainAnnotation(PdfPage page)
    : base(page)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfPolygonalChainAnnotation(PdfPage page, PdfTypeBase dictionary)
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
    if (this.Vertices == null || this.Vertices.Count < 2)
      return;
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    FS_COLOR strokeColor = new FS_COLOR(this.Opacity, this.Color);
    FS_COLOR interiorColor = this is PdfPolylineAnnotation ? (this as PdfPolylineAnnotation).InteriorColor : new FS_COLOR(0);
    PdfLineEndingCollection lineEnding = this is PdfPolylineAnnotation ? (this as PdfPolylineAnnotation).LineEnding : (PdfLineEndingCollection) null;
    List<PdfPathObject> lines = AnnotDrawing.CreateLines(strokeColor, (IList<FS_POINTF>) this.Vertices, LineJoin.Miter, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Width : 1f, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.DashPattern : (float[]) null, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Style : BorderStyles.Solid, this is PdfPolygonAnnotation, !(this is PdfPolygonAnnotation) || !((PdfWrapper) (this as PdfPolygonAnnotation).BorderEffect != (PdfWrapper) null) ? BorderEffects.None : (this as PdfPolygonAnnotation).BorderEffect.Effect, !(this is PdfPolygonAnnotation) || !((PdfWrapper) (this as PdfPolygonAnnotation).BorderEffect != (PdfWrapper) null) ? 0 : (this as PdfPolygonAnnotation).BorderEffect.Intensity);
    if (lineEnding != null && lineEnding.Count > 1)
    {
      lines.AddRange((IEnumerable<PdfPathObject>) AnnotDrawing.CreateLineEnding(this.Vertices[0], this.Vertices[1], lineEnding[0], (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Width : 1f, strokeColor, interiorColor, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Style : BorderStyles.Solid, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.DashPattern : (float[]) null));
      lines.AddRange((IEnumerable<PdfPathObject>) AnnotDrawing.CreateLineEnding(this.Vertices[this.Vertices.Count - 1], this.Vertices[this.Vertices.Count - 2], lineEnding[1], (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Width : 1f, strokeColor, interiorColor, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Style : BorderStyles.Solid, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.DashPattern : (float[]) null));
    }
    foreach (PdfPageObject pdfPageObject in lines)
      this.NormalAppearance.Add(pdfPageObject);
    this.GenerateAppearance(AppearanceStreamModes.Normal);
  }
}
