// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.BorderStyles
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the border style.</summary>
/// <remarks>
/// Other border styles may be defined in the future. Default value: <strong>Solid</strong>.
/// </remarks>
public enum BorderStyles
{
  /// <summary>A solid rectangle surrounding the annotation.</summary>
  [Description("S")] Solid,
  /// <summary>A dashed rectangle surrounding the annotation.</summary>
  [Description("D")] Dashed,
  /// <summary>
  /// A simulated embossed rectangle that appears to be raised above the surface of the page.
  /// </summary>
  [Description("B")] Beveled,
  /// <summary>
  /// A simulated engraved rectangle that appears to be recessed below the surface of the page.
  /// </summary>
  [Description("I")] Inset,
  /// <summary>
  /// A single line along the bottom of the annotation rectangle.
  /// </summary>
  [Description("U")] Underline,
}
