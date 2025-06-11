// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.RenderIntent
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the rendering intent</summary>
public enum RenderIntent
{
  /// <summary>
  /// Colors are represented with respect to the combination of the light source and the output medium’s white point (such as the color of unprinted paper).
  /// </summary>
  RelativeColorimetric,
  /// <summary>
  /// Colors are represented solely with respect to the light source; no correction is made for the output medium’s white point (such as the color of unprinted paper).
  /// </summary>
  AbsoluteColorimetric,
  /// <summary>
  /// Colors are represented in a manner that preserves or emphasizes saturation.
  /// </summary>
  Saturation,
  /// <summary>
  /// Colors are represented in a manner that provides a pleasing perceptual appearance.
  /// </summary>
  Perceptual,
}
