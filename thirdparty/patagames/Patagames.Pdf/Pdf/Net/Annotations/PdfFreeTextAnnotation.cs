// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents a free text annotation</summary>
/// <remarks>
/// A free text annotation (PDF 1.3) displays text directly on the page.
/// Unlike an ordinary text annotation (see <see cref="T:Patagames.Pdf.Net.Annotations.PdfTextAnnotation" />), a free text annotation
/// has no open or closed state; instead of being displayed in a pop-up window, the
/// text is always visible.
/// </remarks>
public class PdfFreeTextAnnotation : PdfMarkupAnnotation
{
  private PdfLinePointCollection<PdfFreeTextAnnotation> _calloutLine;
  private PdfLineEndingCollection _lineEnding;
  private PdfBorderEffect _borderEffect;
  private PdfBorderStyle _borderStyle;

  /// <summary>
  /// Gets or sets the default appearance string to be used in formatting the text.
  /// </summary>
  /// <remarks>
  /// <note type="note">The Appearance Stream, if present, takes precedence over the DefaultAppearance property; </note>
  /// </remarks>
  public string DefaultAppearance
  {
    get
    {
      return !this.Dictionary.ContainsKey("DA") ? (string) null : (this.Dictionary["DA"] as PdfTypeString).UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("DA"))
      {
        this.Dictionary.Remove("DA");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["DA"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets the horizontal alignment of text content.
  /// </summary>
  public JustifyTypes TextAlignment
  {
    get
    {
      return !this.IsExists("Q") ? JustifyTypes.Left : (JustifyTypes) this.Dictionary["Q"].As<PdfTypeNumber>().IntValue;
    }
    set => this.Dictionary["Q"] = (PdfTypeBase) PdfTypeNumber.Create((int) value);
  }

  /// <summary>Gets or sets a default style string.</summary>
  /// <remarks>
  /// <note type="note">This property is ignored when regenerates the annotation’s appearance stream.</note>
  /// </remarks>
  public string DefaultStyle
  {
    get
    {
      return !this.IsExists("DS") ? (string) null : this.Dictionary["DS"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("DS"))
      {
        this.Dictionary.Remove("DS");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["DS"] = (PdfTypeBase) PdfTypeString.Create(value, true);
      }
    }
  }

  /// <summary>
  /// Gets or sets a collection of the <see cref="T:Patagames.Pdf.FS_POINTF" /> that represent callout line attached to the free text annotation.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Callout line main contain three or two points.
  /// Three points represent the starting, knee point, and ending coordinates of the line in default user space,
  /// as shown in Figure. Two points represent the starting and ending coordinates of the line.
  /// </para>
  /// <img src="../Media/CalloutLine.png" />
  /// <note type="note">When callout is used the <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.Intent" /> property must be set to <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.FreeTextCallout" /></note>
  /// </remarks>
  public PdfLinePointCollection<PdfFreeTextAnnotation> CalloutLine
  {
    get
    {
      if (!this.IsExists("CL"))
        return (PdfLinePointCollection<PdfFreeTextAnnotation>) null;
      if (this._calloutLine == null || this._calloutLine.IsDisposed)
        this._calloutLine = new PdfLinePointCollection<PdfFreeTextAnnotation>(this.Dictionary["CL"].As<PdfTypeArray>());
      return this._calloutLine;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("CL"))
        this.Dictionary.Remove("CL");
      else if (value != null)
        this.Dictionary["CL"] = (PdfTypeBase) value.LinePoints;
      this._calloutLine = value;
    }
  }

  /// <summary>
  /// Gets or sets a list of line ending styles to be used in drawing the <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.CalloutLine" />.
  /// </summary>
  /// <remarks>
  /// The first and second elements of the collection specify the line ending styles for the endpoints defined, respectively, by the
  /// first and second point in the <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.CalloutLine" /> collection.
  /// <para>Default value: <strong>[None None]</strong>.</para>
  /// </remarks>
  public PdfLineEndingCollection LineEnding
  {
    get
    {
      if (!this.IsExists("LE"))
        return (PdfLineEndingCollection) null;
      if (this._lineEnding == null || this._lineEnding.IsDisposed)
      {
        PdfTypeArray lineEnding = this.Dictionary["LE"].AsArrayOf<PdfTypeName>();
        this.Dictionary["LE"] = (PdfTypeBase) lineEnding;
        this._lineEnding = new PdfLineEndingCollection(lineEnding);
      }
      return this._lineEnding;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("LE"))
        this.Dictionary.Remove("LE");
      else if (value != null)
      {
        if (value.LineEndingArray.Count == 0 && this.Dictionary.ContainsKey("LE"))
          this.Dictionary.Remove("LE");
        else if (value.LineEndingArray.Count > 0)
          this.Dictionary["LE"] = (PdfTypeBase) value.LineEndingArray;
      }
      this._lineEnding = value;
    }
  }

  /// <summary>
  /// Gets or sets a name describing the intent of the free text annotation.
  /// </summary>
  /// <remarks>
  /// Valid values are <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.FreeTextCallout" />, which means that the annotation is intended to function as a callout, and <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.FreeTextTypeWriter" />, which means that the annotation is intended to function as a click-to-type or typewriter object.
  /// </remarks>
  public AnnotationIntent Intent
  {
    get
    {
      AnnotationIntent internalIntent = this.InternalIntent;
      switch (internalIntent)
      {
        case AnnotationIntent.Unknown:
        case AnnotationIntent.None:
        case AnnotationIntent.FreeTextCallout:
        case AnnotationIntent.FreeTextTypeWriter:
          return internalIntent;
        default:
          throw new PdfParserException(string.Format(Error.err0045, (object) "IT"));
      }
    }
    set
    {
      switch (value)
      {
        case AnnotationIntent.None:
        case AnnotationIntent.FreeTextCallout:
        case AnnotationIntent.FreeTextTypeWriter:
          this.InternalIntent = value;
          break;
        default:
          throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Intent), (object) "are None, FreeTextCallout and FreeTextTypeWriter"));
      }
    }
  }

  /// <summary>
  /// Gets or sets a border effect (<see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" />) used in conjunction with the <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.BorderStyle" />.
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

  /// <summary>
  /// Gets or sets a border style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width and dash pattern to be used in drawing the annotation’s border.
  /// </summary>
  /// <remarks>
  /// <note type="note">The annotation's appearance streame, if present, takes precedence over BorderStyle property.</note>
  /// <note type="note">If neither the value of Border nor the value of BorderStyle property is present, the border is drawn as a solid line with a width of 1 point.</note>
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
  /// Gets or sets a four numbers describing the inner rectangle of the annotation.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The inner rectangle is where the annotation’s text should be displayed.
  /// Any border styles and/or border effects specified by <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.BorderStyle" /> and
  /// <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.BorderEffect" /> properties, respectively, are applied to the border of the inner rectangle.
  /// </para>
  /// <para>
  /// The four numbers correspond to the differences in default user space between
  /// the left, top, right, and bottom coordinates of <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" /> and those of the inner rectangle, respectively.
  /// </para>
  /// <para>
  /// Each value must be greater than or equal to 0. The sum of the
  /// top and bottom differences must be less than the height of <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" />, and the sum of
  /// the left and right differences must be less than the width of <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" />.
  /// </para>
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
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfFreeTextAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("FreeText");
  }

  /// <summary>
  /// Create a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" /> with <see cref="T:Patagames.Pdf.Net.PdfTextObject" /> and parameters as specified.
  /// </summary>
  /// <param name="page">The <see cref="T:Patagames.Pdf.Net.PdfPage" /> where watremark must be placed.</param>
  /// <param name="text">The <see cref="T:Patagames.Pdf.Net.PdfTextObject" /> that will be used as a watermark.</param>
  /// <param name="x">The x-coordinate of this annotation.</param>
  /// <param name="y">The y-coordinate of this annotation.</param>
  /// <param name="rotation">The rotation degree.</param>
  /// <param name="scale">Absolute scale.</param>
  public PdfFreeTextAnnotation(
    PdfPage page,
    PdfTextObject text,
    float x,
    float y,
    float rotation = 0.0f,
    float scale = 1f)
    : this(page)
  {
    string textUnicode = text.TextUnicode;
    FS_COLOR fillColor = text.FillColor;
    float fontSize = text.FontSize;
    string str1 = $"/Arial {fontSize} Tf {(ValueType) (float) ((double) fillColor.R / (double) byte.MaxValue)} {(ValueType) (float) ((double) fillColor.G / (double) byte.MaxValue)} {(ValueType) (float) ((double) fillColor.B / (double) byte.MaxValue)} rg";
    string str2 = $"text-align:left;font-size:{fontSize}pt;font-style:normal;font-weight:normal;color:#{fillColor.R:X2}{fillColor.G:X2}{fillColor.B:X2};font-family:Arial";
    this.Flags = AnnotationFlags.Print;
    this.DefaultAppearance = str1.Replace(",", ".");
    this.DefaultStyle = str2;
    if ((textUnicode ?? "").Trim() != "")
      this.RichText = $"<?xml version=\"1.0\"?><body xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:xfa=\"http://www.xfa.org/schema/xfa-data/1.0/\" xfa:APIVersion=\"Pdfium.NEt SDK:3.0.0\" xfa:spec=\"2.0.2\"><p dir=\"ltr\"><span style=\"{str2}\">{textUnicode}</span></p></body>";
    this.Contents = textUnicode;
    this.Opacity = (float) text.FillColor.A / (float) byte.MaxValue;
    FS_MATRIX matrix = text.Matrix;
    matrix.Scale(scale, scale);
    matrix.Rotate((float) ((double) rotation * 3.1400001049041748 / 180.0));
    text.Matrix = matrix;
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    this.NormalAppearance.Add((PdfPageObject) text);
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    FS_RECTF rectangle = this.Rectangle;
    this.Rectangle = new FS_RECTF(x, y + rectangle.Height, x + rectangle.Width, y);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfFreeTextAnnotation(PdfPage page, PdfTypeBase dictionary)
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
  /// <note type="note"><see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.DefaultAppearance" /> should be set in the format /StockFontName fontSize Tf r g b rg". Text matrix, if specified, is ignored.  </note>
  /// <note type="note">The <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RichText" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation.DefaultStyle" /> properties are ignored.</note>
  /// </remarks>
  public override void RegenerateAppearances()
  {
    this.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    List<PdfPathObject> pdfPathObjectList = (List<PdfPathObject>) null;
    FS_RECTF innerRectangle1 = AnnotDrawing.GetInnerRectangle(this.Rectangle, this.InnerRectangle);
    string fontName;
    float fontSize;
    FS_COLOR strokeColor;
    FS_COLOR fillColor;
    this.ParseDefaultAppearance(this.DefaultAppearance, this.Opacity, out fontName, out fontSize, out strokeColor, out fillColor, out FS_MATRIX _);
    List<PdfTextObject> textObjects = AnnotDrawing.CreateTextObjects(this.Page, this.Contents, fontName, fontSize, strokeColor, fillColor, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Width : 1f, this.TextAlignment, innerRectangle1);
    List<PdfPathObject> square = AnnotDrawing.CreateSquare(fillColor, new FS_COLOR(this.Opacity, this.Color), (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Width : 1f, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.DashPattern : (float[]) null, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Style : BorderStyles.Solid, (PdfWrapper) this.BorderEffect != (PdfWrapper) null ? this.BorderEffect.Effect : BorderEffects.None, (PdfWrapper) this.BorderEffect != (PdfWrapper) null ? this.BorderEffect.Intensity : 0, innerRectangle1);
    if (this.Intent == AnnotationIntent.FreeTextCallout)
      pdfPathObjectList = AnnotDrawing.CreateCalloutLines(fillColor, new FS_COLOR(this.Opacity, this.Color), (IList<FS_POINTF>) this.CalloutLine, LineJoin.Round, this.LineEnding, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Width : 1f, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.DashPattern : (float[]) null, (PdfWrapper) this.BorderStyle != (PdfWrapper) null ? this.BorderStyle.Style : BorderStyles.Solid);
    foreach (PdfPageObject pdfPageObject in square)
      this.NormalAppearance.Add(pdfPageObject);
    foreach (PdfPageObject pdfPageObject in textObjects)
      this.NormalAppearance.Add(pdfPageObject);
    if (pdfPathObjectList != null)
    {
      foreach (PdfPageObject pdfPageObject in pdfPathObjectList)
        this.NormalAppearance.Add(pdfPageObject);
    }
    FS_RECTF rectangle1 = this.Rectangle;
    float[] innerRectangle2 = this.InnerRectangle;
    this.GenerateAppearance(AppearanceStreamModes.Normal);
    if (pdfPathObjectList != null && pdfPathObjectList.Count > 0)
    {
      float[] innerRectangle3 = this.InnerRectangle;
      if (innerRectangle3 == null)
        return;
      FS_RECTF rectangle2 = this.Rectangle;
      if (innerRectangle3.Length != 0)
        innerRectangle3[0] = rectangle1.left + innerRectangle3[0] - rectangle2.left;
      if (innerRectangle3.Length > 1)
        innerRectangle3[1] = rectangle2.top - rectangle1.top + innerRectangle3[1];
      if (innerRectangle3.Length > 2)
        innerRectangle3[2] = rectangle2.right - rectangle1.right + innerRectangle3[2];
      if (innerRectangle3.Length > 3)
        innerRectangle3[3] = rectangle1.bottom + innerRectangle3[3] - rectangle2.bottom;
      this.InnerRectangle = innerRectangle3;
    }
    else
    {
      PdfTypeDictionary.Create(Pdfium.FPDFOBJ_GetDict(Pdfium.FPDFAnnot_GetAppearanceStream(this.Dictionary.Handle, AppearanceStreamModes.Normal))).SetAt("BBox", (PdfTypeBase) rectangle1.ToArray());
      this.Rectangle = rectangle1;
    }
  }

  private void ParseDefaultAppearance(
    string da,
    float opacity,
    out string fontName,
    out float fontSize,
    out FS_COLOR strokeColor,
    out FS_COLOR fillColor,
    out FS_MATRIX matrix)
  {
    float[] strokeColor1;
    float[] fillColor1;
    Pdfium.FPDFTOOLS_ParseDefaultAppearance(da, out strokeColor1, out fillColor1, out fontName, out fontSize, out matrix);
    strokeColor = strokeColor1 != null ? new FS_COLOR(opacity, new FS_COLOR(strokeColor1)) : new FS_COLOR(0);
    fillColor = strokeColor1 != null || fillColor1 != null ? (fillColor1 != null ? new FS_COLOR(opacity, new FS_COLOR(fillColor1)) : new FS_COLOR(0)) : new FS_COLOR((int) byte.MaxValue, 0, 0, 0);
    if (!((fontName ?? "") == ""))
      return;
    fontName = "Arial";
    fontSize = 12f;
  }
}
