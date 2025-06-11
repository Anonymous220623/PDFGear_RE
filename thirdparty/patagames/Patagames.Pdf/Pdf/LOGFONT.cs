// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.LOGFONT
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// The LOGFONT structure defines the attributes of a font.
/// </summary>
public class LOGFONT
{
  /// <summary>
  /// The weight of the font in the range 0 through 1000. For example, 400 is normal and 700 is bold. If this value is zero, a default weight is used.
  /// </summary>
  public FontWeight lfWeight;
  /// <summary>An italic font if set to TRUE.</summary>
  public bool lfItalic;
  /// <summary>The character set.</summary>
  public FontCharSet lfCharSet;
  /// <summary>
  /// The pitch and family of the font. The two low-order bits specify the pitch of the font.
  /// </summary>
  public LOGFONT.FontPitchAndFamily lfPitchAndFamily;
  /// <summary>
  /// A string that specifies the typeface name of the font. The length of this string must not exceed 32 TCHAR values, including the terminating NULL.  If lfFaceName is an empty string, GDI uses the first font that matches the other specified attributes.
  /// </summary>
  public string lfFaceName;
  /// <summary>
  /// The output precision. The output precision defines how closely the output must match the requested font's height, width, character orientation, escapement, pitch, and font type.
  /// </summary>
  public LOGFONT.FontPrecision lfOutPrecision;
  /// <summary>
  /// The clipping precision. The clipping precision defines how to clip characters that are partially outside the clipping region.
  /// </summary>
  public LOGFONT.FontClipPrecision lfClipPrecision;
  /// <summary>
  /// The output quality. The output quality defines how carefully the graphics device interface (GDI) must attempt to match the logical-font attributes to those of an actual physical font.
  /// </summary>
  public LOGFONT.FontQuality lfQuality;

  /// <summary>The pitch and family of the font.</summary>
  /// <remarks>
  /// <para>The proper value can be obtained by using the Boolean OR operator to join one pitch constant with one family constant.</para>
  /// <para>Font families describe the look of a font in a general way. They are intended for specifying fonts when the exact typeface desired is not available. </para>
  /// </remarks>
  [Flags]
  public enum FontPitchAndFamily : byte
  {
    DEFAULT_PITCH = 0,
    FIXED_PITCH = 1,
    VARIABLE_PITCH = 2,
    /// <summary>Do not care or do not know</summary>
    FF_DONTCARE = 0,
    /// <summary>
    /// Fonts with variable stroke width (proportional) and with serifs. MS® Serif is an example.
    /// </summary>
    FF_ROMAN = 16, // 0x10
    /// <summary>
    /// Fonts with variable stroke width (proportional) and without serifs. MS® Sans Serif is an example.
    /// </summary>
    FF_SWISS = 32, // 0x20
    /// <summary>
    /// Fonts with constant stroke width (monospace), with or without serifs. Monospace fonts are usually modern. Pica, Elite, and Courer New® are examples.
    /// </summary>
    FF_MODERN = FF_SWISS | FF_ROMAN, // 0x30
    /// <summary>
    /// Fonts designed to look like handwriting. Script and Cursive are examples.
    /// </summary>
    FF_SCRIPT = 64, // 0x40
    /// <summary>Novelty fonts. Old English is an example.</summary>
    FF_DECORATIVE = FF_SCRIPT | FF_ROMAN, // 0x50
  }

  /// <summary>The output precision.</summary>
  /// <remarks>
  /// <para>The output precision defines how closely the output must match the requested font's height, width, character orientation, escapement, pitch, and font type. It can be one of the following values.</para>
  /// <para>Applications can use the OUT_DEVICE_PRECIS, OUT_RASTER_PRECIS, OUT_TT_PRECIS, and OUT_PS_ONLY_PRECIS values to control how the font mapper chooses a font when the operating system contains more than one font with a specified name. For example, if an operating system contains a font named Symbol in raster and TrueType form, specifying OUT_TT_PRECIS forces the font mapper to choose the TrueType version. Specifying OUT_TT_ONLY_PRECIS forces the font mapper to choose a TrueType font, even if it must substitute a TrueType font of another name.</para>
  /// </remarks>
  public enum FontPrecision : byte
  {
    /// <summary>Specifies the default font mapper behavior.</summary>
    OUT_DEFAULT_PRECIS,
    /// <summary>
    /// This value is not used by the font mapper, but it is returned when raster fonts are enumerated.
    /// </summary>
    OUT_STRING_PRECIS,
    /// <summary>Not used.</summary>
    OUT_CHARACTER_PRECIS,
    /// <summary>
    /// This value is not used by the font mapper, but it is returned when TrueType, other outline-based fonts, and vector fonts are enumerated.
    /// </summary>
    OUT_STROKE_PRECIS,
    /// <summary>
    /// Instructs the font mapper to choose a TrueType font when the system contains multiple fonts with the same name.
    /// </summary>
    OUT_TT_PRECIS,
    /// <summary>
    /// Instructs the font mapper to choose a Device font when the system contains multiple fonts with the same name.
    /// </summary>
    OUT_DEVICE_PRECIS,
    /// <summary>
    /// Instructs the font mapper to choose a raster font when the system contains multiple fonts with the same name.
    /// </summary>
    OUT_RASTER_PRECIS,
    /// <summary>
    /// Instructs the font mapper to choose from only TrueType fonts. If there are no TrueType fonts installed in the system, the font mapper returns to default behavior.
    /// </summary>
    OUT_TT_ONLY_PRECIS,
    /// <summary>
    /// This value instructs the font mapper to choose from TrueType and other outline-based fonts.
    /// </summary>
    OUT_OUTLINE_PRECIS,
    OUT_SCREEN_OUTLINE_PRECIS,
    OUT_PS_ONLY_PRECIS,
  }

  /// <summary>The clipping precision.</summary>
  /// <remarks>
  /// <para>The clipping precision defines how to clip characters that are partially outside the clipping region. It can be one or more of the following values.</para>
  /// </remarks>
  public enum FontClipPrecision : byte
  {
    /// <summary>Specifies default clipping behavior.</summary>
    CLIP_DEFAULT_PRECIS = 0,
    /// <summary>Not used.</summary>
    CLIP_CHARACTER_PRECIS = 1,
    /// <summary>
    /// Not used by the font mapper, but is returned when raster, vector, or True Type fonts are enumerated.
    /// </summary>
    CLIP_STROKE_PRECIS = 2,
    /// <summary>Not used.</summary>
    CLIP_MASK = 15, // 0x0F
    /// <summary>
    /// When this value is used, the rotation for all fonts depends on whether the orientation of the coordinate system is left-handed or right-handed. If not used, device fonts always rotate counterclockwise, but the rotation of other fonts is dependent on the orientation of the coordinate system. For more information about the orientation of coordinate systems, see the description of the nOrientation parameter.
    /// </summary>
    CLIP_LH_ANGLES = 16, // 0x10
    /// <summary>Not used.</summary>
    CLIP_TT_ALWAYS = 32, // 0x20
    /// <summary>
    /// Windows XP SP1: Turns off font association for the font. Note that this flag is not guaranteed to have any effect on any platform after Windows Server 2003.
    /// </summary>
    CLIP_DFA_DISABLE = 64, // 0x40
    /// <summary>
    /// You must specify this flag to use an embedded read-only font.
    /// </summary>
    CLIP_EMBEDDED = 128, // 0x80
  }

  /// <summary>The output quality.</summary>
  /// <remarks>
  /// <para>The output quality defines how carefully the graphics device interface (GDI) must attempt to match the logical-font attributes to those of an actual physical font. It can be one of the following values.</para>
  /// <para>If neither ANTIALIASED_QUALITY nor NONANTIALIASED_QUALITY is selected, the font is antialiased only if the user chooses smooth screen fonts in Control Panel.</para>
  /// </remarks>
  public enum FontQuality : byte
  {
    /// <summary>Appearance of the font does not matter.</summary>
    DEFAULT_QUALITY,
    /// <summary>
    /// Appearance of the font is less important than when PROOF_QUALITY is used. For GDI raster fonts, scaling is enabled, which means that more font sizes are available, but the quality may be lower. Bold, italic, underline, and strikeout fonts are synthesized if necessary.
    /// </summary>
    DRAFT_QUALITY,
    /// <summary>
    /// Character quality of the font is more important than exact matching of the logical-font attributes. For GDI raster fonts, scaling is disabled and the font closest in size is chosen. Although the chosen font size may not be mapped exactly when PROOF_QUALITY is used, the quality of the font is high and there is no distortion of appearance. Bold, italic, underline, and strikeout fonts are synthesized if necessary.
    /// </summary>
    PROOF_QUALITY,
    /// <summary>Font is never antialiased.</summary>
    NONANTIALIASED_QUALITY,
    /// <summary>
    /// Font is always antialiased if the font supports it and the size of the font is not too small or too large.
    /// </summary>
    ANTIALIASED_QUALITY,
    /// <summary>
    /// If set, text is rendered (when possible) using ClearType antialiasing method. See Remarks for more information.
    /// </summary>
    CLEARTYPE_QUALITY,
    CLEARTYPE_NATURAL_QUALITY,
  }
}
