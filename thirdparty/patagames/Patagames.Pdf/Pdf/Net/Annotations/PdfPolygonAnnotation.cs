// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfPolygonAnnotation
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

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents the polygon annotation</summary>
/// <remarks>
/// Please find details in the remarks section of <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" />
/// </remarks>
public class PdfPolygonAnnotation : PdfPolygonalChainAnnotation
{
  private PdfBorderEffect _borderEffect;

  /// <summary>
  /// Gets or sets a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderEffect" /> describing an effect applied to the line described by the <see cref="P:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation.LineStyle" /> property.
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
  /// Gets or sets a name describing the intent of the polygon annotation.
  /// </summary>
  /// <remarks>
  /// Valid values are <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.PolygonCloud" />, which means that the annotation is intended to function as a cloud object,
  /// and <see cref="F:Patagames.Pdf.Enums.AnnotationIntent.PolygonDimension" />, (PDF 1.7), which indicates that the polygon annotation is intended to function as a dimension.
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
        case AnnotationIntent.PolygonCloud:
        case AnnotationIntent.PolygonDimension:
          return internalIntent;
        default:
          throw new PdfParserException(string.Format(Error.err0045, (object) "IT"));
      }
    }
    set
    {
      this.InternalIntent = value == AnnotationIntent.None || value == AnnotationIntent.PolygonCloud || value == AnnotationIntent.PolygonDimension ? value : throw new ArgumentException(string.Format(Error.err0047, (object) nameof (Intent), (object) "are None, PolygonCloud and PolygonDimension"));
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfPolygonAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Polygon");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfPolygonAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
