// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FontWeight
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Provides a set of static predefined FontWeight values.
/// </summary>
/// <remarks>
/// A font weight describes the relative weight of a font, in terms of the lightness or heaviness of the strokes. Weight differences are generally differentiated by an increased stroke or thickness that is associated with a given character in a font, as compared to a "normal" character from that same font.
/// </remarks>
public enum FontWeight
{
  /// <summary>A default weight is used.</summary>
  FW_DONTCARE = 0,
  /// <summary>Specifies a "Thin" font weight.</summary>
  FW_THIN = 100, // 0x00000064
  /// <summary>Specifies an "Extra-light" font weight.</summary>
  FW_EXTRALIGHT = 200, // 0x000000C8
  /// <summary>Specifies a "Light" font weight.</summary>
  FW_LIGHT = 300, // 0x0000012C
  /// <summary>Specifies a "Normal" font weight.</summary>
  FW_NORMAL = 400, // 0x00000190
  /// <summary>Specifies a "Medium" font weight.</summary>
  FW_MEDIUM = 500, // 0x000001F4
  /// <summary>Specifies a "Semi-bold" font weight.</summary>
  FW_SEMIBOLD = 600, // 0x00000258
  /// <summary>Specifies a "Bold" font weight.</summary>
  FW_BOLD = 700, // 0x000002BC
  /// <summary>Specifies an "Extra-bold" font weight.</summary>
  FW_EXTRABOLD = 800, // 0x00000320
  /// <summary>Specifies a "Heavy" font weight.</summary>
  FW_HEAVY = 900, // 0x00000384
}
