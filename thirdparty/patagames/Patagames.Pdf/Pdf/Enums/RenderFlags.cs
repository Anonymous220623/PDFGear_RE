// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.RenderFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Page rendering flags. They can be combined with bit OR.
/// </summary>
[Flags]
public enum RenderFlags
{
  /// <summary>None</summary>
  FPDF_NONE = 0,
  /// <summary>Set if annotations are to be rendered.</summary>
  FPDF_ANNOT = 1,
  /// <summary>
  /// Set if using text rendering optimized for LCD display.
  /// </summary>
  FPDF_LCD_TEXT = 2,
  /// <summary>
  /// Don't use the native text output available on some platforms
  /// </summary>
  FPDF_NO_NATIVETEXT = 4,
  /// <summary>Grayscale output.</summary>
  FPDF_GRAYSCALE = 8,
  /// <summary>
  /// Set if you want to get some debug info. Please discuss with Foxit first if you need to collect debug info.
  /// </summary>
  FPDF_DEBUG_INFO = 128, // 0x00000080
  /// <summary>Set if you don't want to catch exception.</summary>
  FPDF_NO_CATCH = 256, // 0x00000100
  /// <summary>Limit image cache size.</summary>
  FPDF_RENDER_LIMITEDIMAGECACHE = 512, // 0x00000200
  /// <summary>Always use halftone for image stretching.</summary>
  FPDF_RENDER_FORCEHALFTONE = 1024, // 0x00000400
  /// <summary>Render for printing.</summary>
  FPDF_PRINTING = 2048, // 0x00000800
  /// <summary>
  /// set whether render in a reverse Byte order, this flag only enable when render to a bitmap.
  /// </summary>
  FPDF_REVERSE_BYTE_ORDER = 16, // 0x00000010
  /// <summary>Render page as a thumbnail</summary>
  FPDF_THUMBNAIL = 32768, // 0x00008000
  /// <summary>Render page as a thumbnail (high quality)</summary>
  [Obsolete("Please use FPDF_THUMBNAIL instead.", false)] FPDF_HQTHUMBNAIL = FPDF_THUMBNAIL, // 0x00008000
  /// <summary>Set to disable anti-aliasing on text.</summary>
  FPDF_RENDER_NO_SMOOTHTEXT = 4096, // 0x00001000
  /// <summary>Set to disable anti-aliasing on images.</summary>
  FPDF_RENDER_NO_SMOOTHIMAGE = 8192, // 0x00002000
  /// <summary>Set to disable anti-aliasing on paths.</summary>
  FPDF_RENDER_NO_SMOOTHPATH = 16384, // 0x00004000
}
