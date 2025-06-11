// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.ColorTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>the colorspace in which the color is defined</summary>
public enum ColorTypes
{
  /// <summary>
  /// Please use <see cref="F:Patagames.Pdf.Enums.ColorTypes.Transparent" /> instead.
  /// </summary>
  [Obsolete("Please use Transparent instead.", false)] COLORTYPE_TRANSPARENT = 0,
  /// <summary>No color; transparent</summary>
  Transparent = 0,
  /// <summary>Unsupported Color Space</summary>
  Unsupported = 0,
  /// <summary>
  /// Please use <see cref="F:Patagames.Pdf.Enums.ColorTypes.DeviceGray" /> instead.
  /// </summary>
  [Obsolete("Please use DeviceGray instead.", false)] COLORTYPE_GRAY = 1,
  /// <summary>
  /// Black, white, and intermediate shades of gray are special cases of full color.
  /// </summary>
  /// <remarks>A grayscale value is represented by a single number in the range 0.0 to 1.0, where 0.0 corresponds to black, 1.0 to white, and intermediate values to different gray levels.</remarks>
  DeviceGray = 1,
  /// <summary>
  /// Please use <see cref="F:Patagames.Pdf.Enums.ColorTypes.DeviceRGB" /> instead.
  /// </summary>
  [Obsolete("Please use DeviceRGB instead.", false)] COLORTYPE_RGB = 2,
  /// <summary>
  /// Colors in the DeviceRGB color space are specified according to the additive RGB (red-green-blue) color model, in which color values are defined by three components representing the intensities of the additive primary colorants red, green, and blue.
  /// </summary>
  /// <remarks>
  /// Each component is specified by a number in the range 0.0 to 1.0, where 0.0 denotes the complete absence of a primary component and 1.0 denotes maximum intensity. If all three components have equal intensity, the perceived result theoretically is a pure gray on the scale from black to white. If the intensities are not all equal, the result is some color other than a pure gray.
  /// </remarks>
  DeviceRGB = 2,
  /// <summary>
  /// Please use <see cref="F:Patagames.Pdf.Enums.ColorTypes.DeviceCMYK" /> instead.
  /// </summary>
  [Obsolete("Please use DeviceCMYK instead.", false)] COLORTYPE_CMYK = 3,
  /// <summary>
  /// The DeviceCMYK color space allows colors to be specified according to the subtractive CMYK (cyan-magenta-yellow-black) model typical of printers and other paper-based output devices.
  /// </summary>
  DeviceCMYK = 3,
  /// <summary>
  /// A CalGray color space (PDF 1.1) is a special case of a single-component CIEbased color space, known as a CIE-based A color space.
  /// </summary>
  CalGray = 4,
  /// <summary>
  /// A CalRGB color space is a CIE-based ABC color space with only one transformation stage instead of two.
  /// </summary>
  /// <remarks>
  /// In this type of space, A, B, and C represent calibrated red, green, and blue color values. These three color components must be in the range 0.0 to 1.0; component values falling outside that range are adjusted to the nearest valid value without error indication.
  /// </remarks>
  CalRGB = 5,
  /// <summary>
  /// A Lab color space is a CIE-based ABC color space with two transformation stages.
  /// </summary>
  /// <remarks>
  /// In this type of space, A, B, and C represent the L*, a*, and b* components of a CIE 1976 L*a*b* space. The range of the first (L*) component is always 0 to 100; the ranges of the second and third (a* and b*) components are defined by the Range entry in the color space dictionary.
  /// </remarks>
  Lab = 6,
  /// <summary>
  /// ICCBased color spaces are based on a cross-platform color profile as defined by the International Color Consortium (ICC).
  /// </summary>
  ICCBased = 7,
  /// <summary>
  /// A Separation color space provides a means for specifying the use of additional colorants or for isolating the control of individual color components of a device color space for a subtractive device.
  /// </summary>
  Separation = 8,
  /// <summary>
  /// DeviceN color spaces can contain an arbitrary number of color components.
  /// </summary>
  DeviceN = 9,
  /// <summary>
  /// An Indexed color space allows a PDF content stream to use small integers as indices into a color map or color table of arbitrary colors in some other space.
  /// </summary>
  Indexed = 10, // 0x0000000A
  /// <summary>
  /// A Pattern color space enables a PDF content stream to paint an area with a pattern rather than a single color.
  /// </summary>
  /// <remarks>
  /// The pattern may be either a tiling pattern (type 1) or a shading pattern (type 2).
  /// </remarks>
  Pattern = 11, // 0x0000000B
}
