// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FontFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The value of the Flags entry in a font descriptor is an unsigned 32-bit integer containing flags specifying various characteristics of the font.
/// </summary>
/// <remarks>
/// The Nonsymbolic flag indicates that the font’s character
/// set is the Adobe standard Latin character set (or a subset of it) and that it uses the
/// standard names for those glyphs. This character set is shown in <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</a> in Section D.1, “Latin
/// Character Set and Encodings.” If the font contains any glyphs outside this set,
/// the Symbolic flag should be set and the Nonsymbolic flag clear. In other words,
/// any font whose character set is not a subset of the Adobe standard character set is
/// considered to be symbolic. This influences the font’s implicit base encoding and
/// may affect a consumer application’s font substitution strategies.
/// <note type="note">This classification of nonsymbolic and symbolic fonts is peculiar to PDF. A
/// font may contain additional characters that are used in Latin writing systems but
/// are outside the Adobe standard Latin character set; PDF considers such a font to be
/// symbolic. The use of two flags to represent a single binary choice is a historical accident.</note>
/// The ForceBold flag determines whether bold glyphs are painted with
/// extra pixels even at very small text sizes. Typically, when glyphs are painted at
/// small sizes on very low-resolution devices such as display screens, features of bold
/// glyphs may appear only 1 pixel wide. Because this is the minimum feature width
/// on a pixel-based device, ordinary (nonbold) glyphs also appear with 1-pixel-wide
/// features and therefore cannot be distinguished from bold glyphs. If the ForceBold
/// flag is set, features of bold glyphs may be thickened at small text sizes.
/// </remarks>
[Flags]
public enum FontFlags
{
  /// <summary>
  /// All glyphs have the same width (as opposed to proportional or variable-pitch fonts, which have different widths).
  /// </summary>
  PDFFONT_FIXEDPITCH = 1,
  /// <summary>
  /// Glyphs have serifs, which are short strokes drawn at an angle on the top and bottom of glyph stems. (Sans serif fonts do not have serifs.)
  /// </summary>
  PDFFONT_SERIF = 2,
  /// <summary>
  /// Font contains glyphs outside the Adobe standard Latin character set. This flag and the Nonsymbolic flag cannot both be set or both be clear (see below).
  /// </summary>
  PDFFONT_SYMBOLIC = 4,
  /// <summary>Glyphs resemble cursive handwriting.</summary>
  PDFFONT_SCRIPT = 8,
  /// <summary>
  /// Font uses the Adobe standard Latin character set or a subset of it (see below).
  /// </summary>
  PDFFONT_NONSYMBOLIC = 32, // 0x00000020
  /// <summary>
  /// Glyphs have dominant vertical strokes that are slanted.
  /// </summary>
  PDFFONT_ITALIC = 64, // 0x00000040
  /// <summary>
  /// Font contains no lowercase letters; typically used for display purposes, such as for titles or headlines.
  /// </summary>
  PDFFONT_ALLCAP = 65536, // 0x00010000
  /// <summary>
  /// Font contains both uppercase and lowercase letters. The uppercase letters are
  /// similar to those in the regular version of the same typeface family. The glyphs
  /// for the lowercase letters have the same shapes as the corresponding uppercase
  /// letters, but they are sized and their proportions adjusted so that they have the
  /// same size and stroke weight as lowercase glyphs in the same typeface family.
  /// </summary>
  PDFFONT_SMALLCAP = 131072, // 0x00020000
  /// <summary>
  /// The ForceBold flag determines whether bold glyphs are painted with extra pixels even at very small text sizes
  /// </summary>
  PDFFONT_FORCEBOLD = 262144, // 0x00040000
  /// <summary>External attributes</summary>
  PDFFONT_USEEXTERNATTR = 524288, // 0x00080000
}
