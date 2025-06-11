// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfPolylineAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents the polyline annotation</summary>
/// <remarks>
/// Please find details in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" />
/// </remarks>
public class PdfPolylineAnnotation : PdfPolygonalChainAnnotation
{
  private PdfLineEndingCollection _lineEnding;

  /// <summary>
  /// Gets or sets a list of line ending styles to be used in drawing the polugon or polyline/&gt;.
  /// </summary>
  /// <remarks>
  /// The first and second elements of the array specify
  /// the line ending styles for the endpoints defined, respectively, by the first and last
  /// pairs of coordinates in the <see cref="P:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation.Vertices" /> colection.
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
  /// Gets or sets a name describing the intent of the polyline annotation.
  /// </summary>
  /// <remarks>
  /// Valid value is <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.PolyLineDimension" />, (PDF 1.7), which indicates that the polyline annotation is intended to function as a dimension
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
        case AnnotationIntent.PolyLineDimension:
          return internalIntent;
        default:
          throw new PdfParserException(string.Format(Error.err0045, (object) "IT"));
      }
    }
    set
    {
      this.InternalIntent = value == AnnotationIntent.None || value == AnnotationIntent.PolyLineDimension ? value : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Intent), (object) "are None and PolyLineDimension"));
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolylineAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfPolylineAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("PolyLine");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolylineAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfPolylineAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
