// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.AnnotationIntent
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the intent of the markup annotation.</summary>
public enum AnnotationIntent
{
  /// <summary>Unknown intent name.</summary>
  Unknown,
  /// <summary>The intent is not set.</summary>
  None,
  /// <summary>
  /// The annotation is intended to function as a callout. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" />
  /// </summary>
  [Description("FreeTextCallout")] FreeTextCallout,
  /// <summary>
  /// The annotation is intended to function as a click-to-type or typewriter object. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfFreeTextAnnotation" />
  /// </summary>
  [Description("FreeTextTypeWriter")] FreeTextTypeWriter,
  /// <summary>
  /// The annotation is intended to function as an arrow. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />
  /// </summary>
  [Description("LineArrow")] LineArrow,
  /// <summary>
  /// The annotation is intended to function as a dimension line. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfLineAnnotation" />
  /// </summary>
  [Description("LineDimension")] LineDimension,
  /// <summary>
  /// The annotation is intended to function as a cloud object. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" />
  /// </summary>
  [Description("PolygonCloud")] PolygonCloud,
  /// <summary>
  /// The polyline annotation is intended to function as a dimension. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" />
  /// </summary>
  [Description("PolyLineDimension")] PolyLineDimension,
  /// <summary>
  /// The polygon annotation is intended to function as a dimension. Applied to <see cref="T:Patagames.Pdf.Net.Annotations.PdfPolygonalChainAnnotation" />
  /// </summary>
  [Description("PolygonDimension")] PolygonDimension,
}
