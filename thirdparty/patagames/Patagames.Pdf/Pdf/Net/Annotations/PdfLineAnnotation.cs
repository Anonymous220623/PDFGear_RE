// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfLineAnnotation
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

/// <summary>Represents a line annotation</summary>
/// <remarks>
/// A line annotation (PDF 1.3) displays a single straight line on the page.
/// When opened, it displays a pop-up window containing the text of the associated note.
/// <para></para>
/// 
/// <list type="table">
/// <item><term><img src="../Media/Figure8.5LeaderLines.png" /></term></item>
/// <listheader>
/// <term>FIGURE 8.5 Leader lines</term>
/// </listheader>
/// </list>
/// 
/// <para></para>
/// <para>Figure 8.6 illustrates the effect of including a caption to a line annotation, which is specified by setting <see cref="P:Patagames.Pdf.Net.Annotations.PdfLineAnnotation.Cap" /> property to true.</para>
/// <list type="table">
/// <item><term><img src="../Media/Figure8.6Caption.png" /></term></item>
/// <listheader>
/// <term>FIGURE 8.6 Lines with captions appearing as part of the line</term>
/// </listheader>
/// </list>
/// 
/// <para></para>
/// <para>Figure 8.7 illustrates the effect of applying a caption to a line annotation that has a leader offset.</para>
/// <list type="table">
/// <item><term><img src="../Media/Figure8.7CaptionOffset.png" /></term></item>
/// <listheader>
/// <term>FIGURE 8.7 Line with a caption appearing as part of the offset</term>
/// </listheader>
/// </list>
/// </remarks>
public class PdfLineAnnotation : PdfMarkupAnnotation
{
  private PdfLinePointCollection<PdfLineAnnotation> _line;
  private PdfLineEndingCollection _lineEnding;
  private PdfBorderStyle _lineStyle;

  /// <summary>
  /// Gets or sets a collection of the <see cref="T:Patagames.Pdf.FS_POINTF" /> specifying the starting and ending coordinates of the line in default user space.
  /// </summary>
  /// <remarks>
  /// <note type="note">If the value of the <see cref="P:Patagames.Pdf.Net.Annotations.PdfLineAnnotation.LeaderLineLenght" /> property is non zero, this value represents the endpoints of the leader lines rather than the endpoints of the line itself;
  /// see Figure 8.5 in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" /> reference.</note>
  /// </remarks>
  public PdfLinePointCollection<PdfLineAnnotation> Line
  {
    get
    {
      if (!this.IsExists("L"))
        return (PdfLinePointCollection<PdfLineAnnotation>) null;
      if (this._line == null || this._line.IsDisposed)
        this._line = new PdfLinePointCollection<PdfLineAnnotation>(this.Dictionary["L"].As<PdfTypeArray>());
      return this._line;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("L"))
        this.Dictionary.Remove("L");
      else if (value != null)
        this.Dictionary["L"] = (PdfTypeBase) value.LinePoints;
      this._line = value;
    }
  }

  /// <summary>
  /// Gets or sets a list of line ending styles to be used in drawing the <see cref="P:Patagames.Pdf.Net.Annotations.PdfLineAnnotation.Line" />.
  /// </summary>
  /// <remarks>
  /// The first and second elements of the collection specify the line ending styles for the endpoints defined, respectively, by the
  /// first and second point in the <see cref="P:Patagames.Pdf.Net.Annotations.PdfLineAnnotation.Line" /> collection.
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
  /// Gets or sets a line style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width and dash pattern to be used in drawing the line.
  /// </summary>
  /// <remarks>
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over LineStyle property.</note>
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
      this._lineStyle = (PdfBorderStyle) null;
    }
  }

  /// <summary>
  /// Gets or sets the interior color with which to fill the annotation’s line endings.
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
  /// Gets or sets a name describing the intent of the line annotation.
  /// </summary>
  /// <remarks>
  /// Valid values are <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.LineArrow" />, which means that the annotation is intended to function as an arrow, and <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.LineDimension" />, which means that the annotation is intended to function as a dimension line.
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
        case AnnotationIntent.LineArrow:
        case AnnotationIntent.LineDimension:
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
        case AnnotationIntent.LineArrow:
        case AnnotationIntent.LineDimension:
          this.InternalIntent = value;
          break;
        default:
          throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Intent), (object) "are None, LineArrow and LineDimension"));
      }
    }
  }

  /// <summary>
  /// The length of leader lines in default user space that extend from each endpoint of the line perpendicular to the line itself, as shown in Figure 8.5 in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" /> reference.
  /// </summary>
  /// <remarks>
  /// A positive value means that the leader lines appear in the direction that is clockwise when traversing
  /// the line from its starting point to its ending point(as specified by <see cref="P:Patagames.Pdf.Net.Annotations.PdfLineAnnotation.Line" />);
  /// a negative value indicates the opposite direction.
  /// <para>Default value: <strong>0</strong> (no leader lines).</para>
  /// </remarks>
  public float LeaderLineLenght
  {
    get => !this.IsExists("LL") ? 0.0f : this.Dictionary["LL"].As<PdfTypeNumber>().FloatValue;
    set
    {
      if (value.Equals(0.0f) && this.Dictionary.ContainsKey("LL"))
      {
        this.Dictionary.Remove("LL");
      }
      else
      {
        if (value.Equals(0.0f))
          return;
        this.Dictionary["LL"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>
  /// A non-negative number representing the length of leader line extensions that extend from the line
  /// proper 180 degrees from the leader lines, as shown in Figure 8.5 in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" /> reference.
  /// </summary>
  /// <remarks>Default value: <strong>0</strong> (no leader line extensions).</remarks>
  public float LeaderLineExtension
  {
    get => !this.IsExists("LLE") ? 0.0f : this.Dictionary["LLE"].As<PdfTypeNumber>().FloatValue;
    set
    {
      if (value.Equals(0.0f) && this.Dictionary.ContainsKey("LLE"))
      {
        this.Dictionary.Remove("LLE");
      }
      else
      {
        if (value.Equals(0.0f))
          return;
        this.Dictionary["LLE"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or set a flag specifying whether line has a caption.
  /// </summary>
  /// <remarks>
  /// If true, the text specified by the <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Contents" /> or <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.RichText" /> properties
  /// should be replicated as a caption in the appearance of the line, as shown in Figure 8.6 and Figure 8.7 in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" /> reference.
  /// The text should be rendered in a manner appropriate to the content, taking into account factors
  /// such as writing direction.
  /// <para>Default value: <strong>false</strong>.</para>
  /// </remarks>
  public bool Cap
  {
    get => this.IsExists(nameof (Cap)) && this.Dictionary[nameof (Cap)].As<PdfTypeBoolean>().Value;
    set => this.Dictionary[nameof (Cap)] = (PdfTypeBase) PdfTypeBoolean.Create(value);
  }

  /// <summary>
  /// Gets or sets a non-negative number representing the length of the leader line offset, which is the amount of empty space between the endpoints of the annotation and the beginning of the leader lines.
  /// </summary>
  public float LeaderLineOffset
  {
    get => !this.IsExists("LLO") ? 0.0f : this.Dictionary["LLO"].As<PdfTypeNumber>().FloatValue;
    set
    {
      if (value.Equals(0.0f) && this.Dictionary.ContainsKey("LLO"))
      {
        this.Dictionary.Remove("LLO");
      }
      else
      {
        if (value.Equals(0.0f))
          return;
        this.Dictionary["LLO"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets a value describing the annotation’s caption positioning.
  /// </summary>
  public CaptionPositions CaptionPosition
  {
    get
    {
      if (!this.IsExists("CP"))
        return CaptionPositions.Inline;
      CaptionPositions result;
      if (Pdfium.GetEnumDescription<CaptionPositions>(this.Dictionary["CP"].As<PdfTypeName>().Value, out result))
        return result;
      throw new PdfParserException(string.Format(Error.err0045, (object) "CP"));
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      this.Dictionary["CP"] = !((enumDescription ?? "").Trim() == "") ? (PdfTypeBase) PdfTypeName.Create(enumDescription) : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (CaptionPosition), (object) "are CaptionPositions.Inline or CaptionPositions.Top"));
    }
  }

  /// <summary>
  /// Gets or sets the value specifying the offset of the caption text from its normal position.
  /// </summary>
  /// <remarks>
  /// The horizontal component of the <see cref="T:Patagames.Pdf.FS_SIZEF" /> structure is the horizontal offset along the
  /// annotation line from its midpoint, with a positive value indicating offset to the right and
  /// a negative value indicating offset to the left.
  /// The vertical component is the vertical offset perpendicular to the annotation
  /// line, with a positive value indicating a shift up and a negative value indicating a shift down.
  /// <para>Default value: <strong>[0, 0]</strong> (no offset from normal positioning)</para>
  /// </remarks>
  public FS_SIZEF CaptionOffset
  {
    get
    {
      if (!this.IsExists("CO"))
        return new FS_SIZEF();
      PdfTypeArray pdfTypeArray = this.Dictionary["CO"].As<PdfTypeArray>();
      float height;
      float width = height = 0.0f;
      int count = pdfTypeArray.Count;
      if (count > 0)
        width = pdfTypeArray[0].As<PdfTypeNumber>().FloatValue;
      if (count > 1)
        height = pdfTypeArray[1].As<PdfTypeNumber>().FloatValue;
      return new FS_SIZEF(width, height);
    }
    set
    {
      if (value == new FS_SIZEF() && this.Dictionary.ContainsKey("CO"))
      {
        this.Dictionary.Remove("CO");
      }
      else
      {
        if (!(value != new FS_SIZEF()))
          return;
        PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
        pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value.Width));
        pdfTypeArray.Add((PdfTypeBase) PdfTypeNumber.Create(value.Height));
        this.Dictionary["CO"] = (PdfTypeBase) pdfTypeArray;
      }
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfLineAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create(nameof (Line));
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfLineAnnotation(PdfPage page, PdfTypeBase dictionary)
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
    FS_COLOR fsColor = new FS_COLOR(this.Opacity, this.Color);
    if (this.Line.Count < 2)
      return;
    PdfFont font = (PdfFont) null;
    if (this.Cap && (this.Contents ?? "").Trim() != "")
      font = PdfFont.CreateStock(this.Page.Document, FontStockNames.Arial);
    foreach (PdfPageObject pdfPageObject in AnnotDrawing.CreateLine((IList<FS_POINTF>) this.Line, new FS_COLOR(this.Opacity, this.Color), new FS_COLOR(this.Opacity, this.InteriorColor), this.LineEnding, this.LeaderLineLenght, this.LeaderLineExtension, this.LeaderLineOffset, this.Contents, this.Cap, this.CaptionOffset, this.CaptionPosition, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Width : 1f, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.DashPattern : (float[]) null, (PdfWrapper) this.LineStyle != (PdfWrapper) null ? this.LineStyle.Style : BorderStyles.Solid, font))
      this.NormalAppearance.Add(pdfPageObject);
    this.GenerateAppearance(AppearanceStreamModes.Normal);
  }
}
