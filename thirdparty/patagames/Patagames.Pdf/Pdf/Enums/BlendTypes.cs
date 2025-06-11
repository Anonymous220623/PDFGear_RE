// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.BlendTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents the blend mode to be used in the transparent imaging model.
/// </summary>
/// <remarks>
/// <para>Photoshop’s hue, saturation, color, and luminosity blend modes are based on a color space with dimensions that the article HSL and HSV calls hue, chroma, and luma.</para>
/// <para>Because these blend modes are based on a color space which is much closer than RGB to perceptually relevant dimensions, it can be used to correct the color of an image without altering perceived lightness, and to manipulate lightness contrast without changing the hue or chroma. The Luminosity mode is commonly used for image sharpening, because human vision is much more sensitive to fine-scale lightness contrast than color contrast.</para>
/// </remarks>
public enum BlendTypes
{
  /// <summary>Unsupported blend mode.</summary>
  FXDIB_BLEND_UNSUPPORTED = -1, // 0xFFFFFFFF
  /// <summary>
  /// The foreground color is fully opaque, except for objects for which the opacity is set to less than 100 percent.
  /// </summary>
  FXDIB_BLEND_NORMAL = 0,
  /// <summary>
  /// The <strong>Multiply</strong> blend mode always creates a darker color (except when the foreground or background color is white, in which case no change occurs). <strong>Multiply</strong> is like overprinting two inks. For example, setting a yellow box to <strong>Multiply</strong> and putting the box over a blue area would give you green.
  /// </summary>
  FXDIB_BLEND_MULTIPLY = 1,
  /// <summary>
  /// The <strong>Screen</strong> blend mode (which some other applications call "light") is like projecting two or more colored lights at a white wall. The result is almost always lighter than the original colors. If you position a red path, a green path, and a blue path on top of each other and set them to the <strong>Screen</strong> blend mode, the result is white.
  /// </summary>
  FXDIB_BLEND_SCREEN = 2,
  /// <summary>
  /// Overlay combines Multiply and Screen blend modes. The parts of the top layer where the base layer is light become lighter, the parts where the base layer is dark become darker. Areas where the top layer are mid grey are unaffected.
  /// </summary>
  FXDIB_BLEND_OVERLAY = 3,
  /// <summary>
  /// This mode compares the color channels of the foreground and background colors and uses the darker of the two.
  /// </summary>
  FXDIB_BLEND_DARKEN = 4,
  /// <summary>
  /// The <strong>Lighten</strong> blend mode (which some applications call "brighten") compares the color channels of the foreground and background colors and uses the lighter of the two.
  /// </summary>
  FXDIB_BLEND_LIGHTEN = 5,
  /// <summary>
  /// In <strong>Color Dodge</strong> mode, the color channels of the background color are brightened based on the color channels in the foreground color. If the foreground color is black, then the mode has no effect. Anything brighter than black dodges ("lightens") the background. The result is often lighter than the <strong>Screen</strong> blend mode.
  /// </summary>
  FXDIB_BLEND_COLORDODGE = 6,
  /// <summary>
  /// Here, the background colors are darkened based on the foreground colors. A black foreground color gives you black, and a white foreground color has no effect.
  /// </summary>
  FXDIB_BLEND_COLORBURN = 7,
  /// <summary>
  /// Hard Light combines Multiply and Screen blend modes. Equivalent to Overlay, but with the bottom and top images swapped.
  /// </summary>
  FXDIB_BLEND_HARDLIGHT = 8,
  /// <summary>
  /// This is a softer version of Hard Light. Applying pure black or white does not result in pure black or white.
  /// </summary>
  FXDIB_BLEND_SOFTLIGHT = 9,
  /// <summary>
  /// In the <strong>Difference</strong> blend mode, Microsoft Expression Design mathematically subtracts each color channel of the foreground object from the color channel of the background color. If two colors are identical, the result is black. If the two colors are on exactly opposite sides of the color spectrum (such as red and cyan), the result is white. This blend mode produces interesting but sometimes unexpected results, especially with soft-edge paths in bright colors.
  /// </summary>
  FXDIB_BLEND_DIFFERENCE = 10, // 0x0000000A
  /// <summary>
  ///  The foreground object acts as an eraser through all objects below it on the same layer. Any object on a different layer underneath shows through.
  /// </summary>
  FXDIB_BLEND_EXCLUSION = 11, // 0x0000000B
  /// <summary>
  /// The <strong>Hue</strong> blend mode preserves the luma and chroma of the bottom layer, while adopting the hue of the top layer.
  /// </summary>
  FXDIB_BLEND_HUE = 21, // 0x00000015
  /// <summary>Non separable blend mode.</summary>
  FXDIB_BLEND_NONSEPARABLE = 21, // 0x00000015
  /// <summary>
  /// The <strong>Saturation</strong> blend mode preserves the luma and hue of the bottom layer, while adopting the chroma of the top layer.
  /// </summary>
  FXDIB_BLEND_SATURATION = 22, // 0x00000016
  /// <summary>
  /// The <strong>Color</strong> blend mode preserves the luma of the bottom layer, while adopting the hue and chroma of the top layer.
  /// </summary>
  FXDIB_BLEND_COLOR = 23, // 0x00000017
  /// <summary>
  /// The <strong>Luminosity</strong> blend mode preserves the hue and chroma of the bottom layer, while adopting the luma of the top layer.
  /// </summary>
  FXDIB_BLEND_LUMINOSITY = 24, // 0x00000018
}
