// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.BitmapFormats
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Bitmap formats</summary>
public enum BitmapFormats
{
  FXDIB_Invalid = 0,
  FXDIB_1bppRgb = 1,
  FXDIB_8bppRgb = 8,
  FXDIB_Rgb = 24, // 0x00000018
  FXDIB_Rgb32 = 32, // 0x00000020
  FXDIB_1bppMask = 257, // 0x00000101
  FXDIB_8bppMask = 264, // 0x00000108
  FXDIB_8bppRgba = 520, // 0x00000208
  FXDIB_Rgba = 536, // 0x00000218
  FXDIB_Argb = 544, // 0x00000220
  FXDIB_1bppCmyk = 1025, // 0x00000401
  FXDIB_8bppCmyk = 1032, // 0x00000408
  FXDIB_Cmyk = 1056, // 0x00000420
  FXDIB_8bppCmyka = 1544, // 0x00000608
  FXDIB_Cmyka = 1568, // 0x00000620
}
