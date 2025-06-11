// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FontTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represent font types</summary>
/// <remarks>
/// <para>
/// <markup><b>Type 1 Fonts</b></markup>
/// </para>
/// <para>A Type 1 font program is a stylized PostScript program that describes glyph shapes.
/// It uses a compact encoding for the glyph descriptions, and it includes hint
/// information that enables high-quality rendering even at small sizes and low resolutions. Details on this format are provided in a separate book, Adobe Type 1 Font
/// Format. An alternative, more compact but functionally equivalent representation
/// of a Type 1 font program is documented in Adobe Technical Note #5176, The Compact Font Format Specification. </para>
/// <note type="note">Although a Type 1 font program uses PostScript language syntax, using it does not require a full PostScript interpreter; a specialized Type 1 font interpreter suffices</note>
/// 
/// <para>
/// <markup><b>TrueType Fonts</b></markup>
/// </para>
/// <para>The TrueType font format was developed by Apple Computer, Inc., and has been adopted as a standard font format for the Microsoft Windows operating system. Specifications for the TrueType font file format are available in Apple’s TrueType Reference Manual and Microsoft’s TrueType 1.0 Font Files Technical Specification.</para>
/// <note type="note">A TrueType font program can be embedded directly in a PDF file as a stream object. The Type 42 font format that is defined for PostScript does not apply to PDF. </note>
/// 
/// <para>
/// <markup><b>Type 3 Fonts</b></markup>
/// </para>
/// <para>Type 3 fonts differ from the other fonts supported by PDF. A Type 3 font dictionary
/// defines the font; font dictionaries for other fonts simply contain information
/// about the font and refer to a separate font program for the actual glyph descriptions.
/// In Type 3 fonts, glyphs are defined by streams of PDF graphics operators.
/// These streams are associated with character names. A separate encoding entry
/// maps character codes to the appropriate character names for the glyphs. </para>
/// <para>Type 3 fonts are more flexible than Type 1 fonts because the glyph descriptions
/// may contain arbitrary PDF graphics operators. However, Type 3 fonts have no
/// hinting mechanism for improving output at small sizes or low resolutions. </para>
/// 
/// <para>
/// <markup><b>CIDFonts</b></markup>
/// </para>
/// <para>A CIDFont program contains glyph descriptions that are accessed using a CID as the character selector. There are two types of CIDFonts:</para>
/// <list type="bullet">
/// <item>A Type 0 CIDFont contains glyph descriptions based on the Adobe Type 1 font format
/// <note type="note">The term “Type 0” when applied to a CIDFont has a different meaning than for a “Type 0 font”</note>
/// </item>
/// <item>A Type 2 CIDFont contains glyph descriptions based on the TrueType font format</item>
/// </list>
/// <para>A CIDFont dictionary is a PDF object that contains information about a CIDFont
/// program. Although its Type value is Font, a CIDFont is not actually a font. It does
/// not have an Encoding entry, it cannot be listed in the Font subdictionary of a resource
/// dictionary, and it cannot be used as the operand of the Tf operator. It is
/// used only as a descendant of a Type 0 font. The CMap in the Type 0 font is what
/// defines the encoding that maps character codes to CIDs in the CIDFont.</para>
/// </remarks>
public enum FontTypes
{
  /// <summary>
  /// A font that defines glyph shapes using Type 1 font technology
  /// </summary>
  PDFFONT_TYPE1 = 1,
  /// <summary>A font based on the TrueType font format.</summary>
  PDFFONT_TRUETYPE = 2,
  /// <summary>
  /// A font that defines glyphs with streams of PDF graphics operators
  /// </summary>
  PDFFONT_TYPE3 = 3,
  /// <summary>
  /// A CIDFont whose glyph descriptions are based on Type 1 or TrueType font technology
  /// </summary>
  PDFFONT_CIDFONT = 4,
}
