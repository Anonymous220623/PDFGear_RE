// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FXDIBFormats
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>A number indicating for bitmap format</summary>
public enum FXDIBFormats
{
  /// <summary>Gray scale bitmap, one byte per pixel</summary>
  Bytes1 = 1,
  /// <summary>3 bytes per pixel, byte order: blue, green, red</summary>
  Bytes3 = 2,
  /// <summary>
  /// 4 bytes per pixel, byte order: blue, green, red, unused
  /// </summary>
  Bytes4 = 3,
  /// <summary>
  /// 4 bytes per pixel, byte order: blue, green, red, alpha
  /// </summary>
  Bytes4Alpha = 4,
}
