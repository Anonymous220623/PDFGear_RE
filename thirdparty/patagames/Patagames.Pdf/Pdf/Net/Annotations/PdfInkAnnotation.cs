// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfInkAnnotation
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

/// <summary>Represents an Ink annotation</summary>
/// <remarks>
/// An ink annotation (PDF 1.3) represents a freehand “scribble” composed of one or
/// more disjoint paths. When opened, it displays a pop-up window containing the
/// text of the associated note.
/// </remarks>
public class PdfInkAnnotation : PdfMarkupAnnotation
{
  private PdfInkPointCollection _inkList;
  private PdfBorderStyle _lineStyle;

  /// <summary>
  /// Gets or sets a line style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width and dash pattern to be used in drawing the paths.
  /// </summary>
  /// <remarks>
  /// <note type="note">The annotation's appearance stream, if present, takes precedence over the  <see cref="P:Patagames.Pdf.Net.Annotations.PdfInkAnnotation.InkList" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfInkAnnotation.LineStyle" /> properties.</note>
  /// <note type="note">If the value of LineStyle property is null, the ink list is drawn as a solid line with a width of 1 point.</note>
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
  /// Gets or sets a collection of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLinePointCollection`1" />, each representing a stroked path.
  /// </summary>
  /// <remarks>
  /// Each item is a series of alternating horizontal and vertical coordinates in default user space,
  /// specifying points along the path.When drawn, the points are connected by
  /// straight lines or curves in an implementation-dependent way.
  /// <note type="note">
  /// PdfViewer always use straight lines to connect the points along each path.
  /// </note>
  /// </remarks>
  public PdfInkPointCollection InkList
  {
    get
    {
      if (!this.IsExists(nameof (InkList)))
        return (PdfInkPointCollection) null;
      if (this._inkList == null || this._inkList.InkList.IsDisposed)
        this._inkList = new PdfInkPointCollection(this.Dictionary[nameof (InkList)].As<PdfTypeArray>());
      return this._inkList;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey(nameof (InkList)))
        this.Dictionary.Remove(nameof (InkList));
      else if (value != null)
        this.Dictionary[nameof (InkList)] = (PdfTypeBase) value.InkList;
      this._inkList = value;
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfInkAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfInkAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Ink");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfInkAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfInkAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }

  /// <summary>
  /// Re-creates the appearance of the annotation based on its properties.
  /// </summary>
  /// <remarks>
  /// When the annotation does not have an Appearance stream (old style of annotations),
  /// they are not rendered by the Pdfium engine.
  /// Calling this function creates an appearance stream based on the default parameters and the properties of this annotation.
  /// </remarks>
  public override void RegenerateAppearances()
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    if (this.InkList == null || this.InkList.Count <= 0)
      return;
    List<PdfPathObject> pdfPathObjectList = new List<PdfPathObject>();
    foreach (PdfLinePointCollection<PdfInkAnnotation> ink in this.InkList)
      pdfPathObjectList.AddRange((IEnumerable<PdfPathObject>) AnnotDrawing.CreateLines(new FS_COLOR(this.Opacity, this.Color), (IList<FS_POINTF>) ink, LineJoin.Round, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Width : 1f, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.DashPattern : (float[]) null, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Style : BorderStyles.Solid, false, BorderEffects.None, 0));
    foreach (PdfPageObject pdfPageObject in pdfPathObjectList)
      this.NormalAppearance.Add(pdfPageObject);
    this.GenerateAppearance(AppearanceStreamModes.Normal);
  }
}
