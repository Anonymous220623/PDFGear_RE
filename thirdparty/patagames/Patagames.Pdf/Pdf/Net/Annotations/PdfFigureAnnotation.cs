// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfFigureAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents square or circle annotation</summary>
/// <remarks>
/// Square and circle annotations (PDF 1.3) display, respectively, a rectangle or an ellipse on the page.
/// When opened, they display a pop-up window containing the
/// text of the associated note. The rectangle or ellipse is inscribed within the annotation rectangle
/// defined by the <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" /> property.
/// Figure 8.8 shows two annotations, each with a border width of 18 points.
/// <note type="note">Despite the names square and circle, the width and height of the annotation rectangle need not be equal.</note>
/// <para></para>
/// <list type="table">
/// <item><term><img src="../Media/Figure8.8.png" /></term></item>
/// <listheader>
/// <term>FIGURE 8.8 Square and circle annotations</term>
/// </listheader>
/// </list>
/// </remarks>
public abstract class PdfFigureAnnotation : PdfMarkupAnnotation
{
  private PdfBorderEffect _borderEffect;
  private PdfBorderStyle _borderStyle;

  /// <summary>
  /// Gets or sets a border style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width and dash pattern to be used in drawing the rectangle or ellipse.
  /// </summary>
  /// <remarks>
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over the  <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfFigureAnnotation.BorderStyle" /> properties.</note>
  /// <note type="note">If the value of BorderStyle property is null, the rectanle or ellipse are drawn as a solid line with a width of 1 point.</note>
  /// </remarks>
  public PdfBorderStyle BorderStyle
  {
    get
    {
      if (!this.IsExists("BS"))
        return (PdfBorderStyle) null;
      if ((PdfWrapper) this._borderStyle == (PdfWrapper) null || this._borderStyle.Dictionary.IsDisposed)
        this._borderStyle = new PdfBorderStyle(this.Dictionary["BS"]);
      return this._borderStyle;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("BS"))
        this.Dictionary.Remove("BS");
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary["BS"] = (PdfTypeBase) value.Dictionary;
      this._borderStyle = value;
    }
  }

  /// <summary>
  /// Gets or sets the interior color with which to fill the annotation’s rectangle or ellipse.
  /// </summary>
  public virtual FS_COLOR InteriorColor
  {
    get
    {
      return !this.IsExists("IC") ? new FS_COLOR(0) : new FS_COLOR(this.Dictionary["IC"].As<PdfTypeArray>());
    }
    set
    {
      if (value.A == 0 && this.Dictionary.ContainsKey("IC"))
      {
        this.Dictionary.Remove("IC");
      }
      else
      {
        if (value.A == 0)
          return;
        this.Dictionary["IC"] = (PdfTypeBase) value.ToArray();
      }
    }
  }

  /// <summary>
  /// Gets or sets a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" /> describing an effect applied to the border described by the <see cref="P:Patagames.Pdf.Net.Annotations.PdfFigureAnnotation.BorderStyle" /> property.
  /// </summary>
  public PdfBorderEffect BorderEffect
  {
    get
    {
      if (!this.IsExists("BE"))
        return (PdfBorderEffect) null;
      if ((PdfWrapper) this._borderEffect == (PdfWrapper) null || this._borderEffect.Dictionary.IsDisposed)
        this._borderEffect = new PdfBorderEffect(this.Dictionary["BE"]);
      return this._borderEffect;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("BE"))
        this.Dictionary.Remove("BE");
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary["BE"] = (PdfTypeBase) value.Dictionary;
      this._borderEffect = value;
    }
  }

  /// <summary>Gets or sets an inner rectangle of the annotation.</summary>
  /// <remarks>
  /// The inner rectangle is the actual boundaries of the underlying square or circle.
  /// Such a difference can occur in situations where a border effect causes the size of the <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" /> to increase beyond that of the square or circle.
  /// </remarks>
  public float[] InnerRectangle
  {
    get
    {
      if (!this.IsExists("RD"))
        return (float[]) null;
      float[] innerRectangle = new float[4];
      PdfTypeArray pdfTypeArray = this.Dictionary["RD"].As<PdfTypeArray>();
      int count = pdfTypeArray.Count;
      if (count > 0)
        innerRectangle[0] = (pdfTypeArray[0] as PdfTypeNumber).FloatValue;
      if (count > 1)
        innerRectangle[1] = (pdfTypeArray[1] as PdfTypeNumber).FloatValue;
      if (count > 2)
        innerRectangle[2] = (pdfTypeArray[2] as PdfTypeNumber).FloatValue;
      if (count > 3)
        innerRectangle[3] = (pdfTypeArray[3] as PdfTypeNumber).FloatValue;
      return innerRectangle;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("RD"))
      {
        this.Dictionary.Remove("RD");
      }
      else
      {
        if (value == null)
          return;
        PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
        if (value.Length != 0)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[0]));
        if (value.Length > 1)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[1]));
        if (value.Length > 2)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[2]));
        if (value.Length > 3)
          pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value[3]));
        this.Dictionary["RD"] = (PdfTypeBase) pdfTypeArray;
      }
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfFigureAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfFigureAnnotation(PdfPage page)
    : base(page)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfFigureAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfFigureAnnotation(PdfPage page, PdfTypeBase dictionary)
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
    FS_COLOR interiorColor = this.InteriorColor;
    FS_COLOR color = this.Color;
    FS_COLOR fillColor = interiorColor.A > 0 ? new FS_COLOR(this.Opacity, interiorColor) : new FS_COLOR(0);
    FS_COLOR strokeColor = color.A > 0 ? new FS_COLOR(this.Opacity, color) : new FS_COLOR(0);
    FS_RECTF rect = this.Rectangle;
    if ((PdfWrapper) this.BorderEffect == (PdfWrapper) null || this.BorderEffect.Effect != BorderEffects.Cloudy || this.BorderEffect.Intensity <= 0)
      rect = AnnotDrawing.GetInnerRectangle(this.Rectangle, this.InnerRectangle);
    foreach (PdfPageObject pdfPageObject in this is PdfSquareAnnotation ? AnnotDrawing.CreateSquare(strokeColor, fillColor, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Width : 1f, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.DashPattern : (float[]) null, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Style : BorderStyles.Solid, (PdfWrapper) this.BorderEffect != (PdfWrapper) null ? this.BorderEffect.Effect : BorderEffects.None, (PdfWrapper) this.BorderEffect != (PdfWrapper) null ? this.BorderEffect.Intensity : 0, rect) : AnnotDrawing.CreateCircle(strokeColor, fillColor, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Width : 1f, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.DashPattern : (float[]) null, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Style : BorderStyles.Solid, (PdfWrapper) this.BorderEffect != (PdfWrapper) null ? this.BorderEffect.Effect : BorderEffects.None, (PdfWrapper) this.BorderEffect != (PdfWrapper) null ? this.BorderEffect.Intensity : 0, rect))
      this.NormalAppearance.Add(pdfPageObject);
    FS_RECTF rectangle = this.Rectangle;
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Normal))).SetAt("BBox", (PdfTypeBase) rectangle.ToArray());
    this.Rectangle = rectangle;
  }
}
