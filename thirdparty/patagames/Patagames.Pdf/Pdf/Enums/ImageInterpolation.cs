// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.ImageInterpolation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The <see cref="T:Patagames.Pdf.Enums.ImageInterpolation" /> enumeration specifies the algorithm that is used when images are scaled or rotated.
/// </summary>
public enum ImageInterpolation
{
  /// <summary>
  /// Specifies default mode. Depends on the size of the source and target images.
  /// </summary>
  Default = 0,
  /// <summary>Specifies low quality interpolation.</summary>
  DownSample = 4,
  /// <summary>
  /// Specifies bilinear interpolation. No prefiltering is done. This mode is not suitable for shrinking an image below 50 percent of its original size.
  /// </summary>
  Bilinear = 32, // 0x00000020
  /// <summary>
  /// Specifies bicubic interpolation. No prefiltering is done. This mode is not suitable for shrinking an image below 25 percent of its original size.
  /// </summary>
  Bicubic = 128, // 0x00000080
  /// <summary>No interpolation.</summary>
  NoSmooth = 256, // 0x00000100
}
