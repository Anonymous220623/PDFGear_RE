// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFont
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Drawing;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents the font object</summary>
/// <summary>Represents the font object</summary>
public class PdfFont
{
  private PdfTypeDictionary _dictionary;

  /// <summary>
  /// Get the special character bounding box of a font object.
  /// </summary>
  /// <param name="charCode">The character code.</param>
  /// <returns>Character bounding box of a font object</returns>
  public Rectangle GetCharBBoxEx(int charCode)
  {
    int left;
    int top;
    int right;
    int bottom;
    Pdfium.FPDFFont_GetCharBBox(this.Handle, charCode, out left, out top, out right, out bottom);
    return new Rectangle(left, top, right - left, bottom - top);
  }

  /// <summary>Gets the Pdfium SDK handle that the font is bound to</summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets font type</summary>
  public FontTypes FontType => Pdfium.FPDFFont_GetFontType(this.Handle);

  /// <summary>Gets font type name</summary>
  public string FontTypeName => Pdfium.FPDFFont_GetFontTypeName(this.Handle);

  /// <summary>Gets base font name</summary>
  public string BaseFontName => Pdfium.FPDFFont_GetBaseFont(this.Handle);

  /// <summary>Gets font flags</summary>
  public FontFlags Flags => Pdfium.FPDFFont_GetFlags(this.Handle);

  /// <summary>Specifies whether the font is VertWriting</summary>
  public bool IsVertWriting => Pdfium.FPDFFont_IsVertWriting(this.Handle);

  /// <summary>Specifies whether the font is ebedded</summary>
  public bool IsEmbedded => Pdfium.FPDFFont_IsEmbedded(this.Handle);

  /// <summary>Specifies whether the font is unicode compatible</summary>
  public bool IsUnicodeCompatible => Pdfium.FPDFFont_IsUnicodeCompatible(this.Handle);

  /// <summary>Specifies whether the font is a standard font.</summary>
  /// <remarks>See details in <see cref="T:Patagames.Pdf.Enums.FontStockNames" /> article.</remarks>
  public bool IsStandardFont => Pdfium.FPDFFont_IsStandardFont(this.Handle);

  /// <summary>Gets the ascent value in the font</summary>
  public int Ascent => Pdfium.FPDFFont_GetTypeAscent(this.Handle);

  /// <summary>Get the descent value in the font</summary>
  public int Descent => Pdfium.FPDFFont_GetTypeDescent(this.Handle);

  /// <summary>Get the italic angle value in the font</summary>
  public int ItalicAngel => Pdfium.FPDFFont_GetItalicAngle(this.Handle);

  /// <summary>Get the StemV value in the font</summary>
  public int StemV => Pdfium.FPDFFont_GetStemV(this.Handle);

  /// <summary>Get the Weight value in the font</summary>
  public int Weight => this.StemV >= 140 ? this.StemV * 4 + 140 : this.StemV * 5;

  /// <summary>Gets the font dictionary</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      IntPtr fontDict = Pdfium.FPDFFont_GetFontDict(this.Handle);
      if (fontDict == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary == null || this._dictionary.IsDisposed || this._dictionary.Handle != fontDict)
        this._dictionary = new PdfTypeDictionary(fontDict);
      return this._dictionary;
    }
  }

  /// <summary>Get a special string width of a font object.</summary>
  /// <param name="text">Text the width of which is should be obtained.</param>
  /// <returns>String width of a font object</returns>
  public int GetStringWidth(string text) => Pdfium.FPDFFont_GetStringWidth(this.Handle, text);

  /// <summary>Get a special character width of a font object.</summary>
  /// <param name="charCode">The character code.</param>
  /// <returns>Character width of a font object.</returns>
  public int GetCharFontWidth(int charCode) => Pdfium.FPDFFont_GetCharWidthF(this.Handle, charCode);

  /// <summary>Get a special character width of a font object.</summary>
  /// <param name="charCode">The character code.</param>
  /// <param name="isVert">Gets the flag indicating whether a given symbol is vertically inscribed.</param>
  /// <returns>Character width of a font object.</returns>
  public int GetCharTypeWidth(int charCode, out bool isVert)
  {
    return Pdfium.FPDFFont_GetCharTypeWidth(this.Handle, charCode, out isVert);
  }

  /// <summary>
  /// Get the special character bounding box of a font object.
  /// </summary>
  /// <param name="charCode">The character code.</param>
  /// <returns>Character bounding box of a font object</returns>
  public FX_RECT GetCharBBox(int charCode)
  {
    int left;
    int top;
    int right;
    int bottom;
    Pdfium.FPDFFont_GetCharBBox(this.Handle, charCode, out left, out top, out right, out bottom);
    return new FX_RECT()
    {
      left = left,
      top = bottom,
      right = right,
      bottom = top
    };
  }

  /// <summary>Convert given char code to unicode</summary>
  /// <param name="charCode">Char code</param>
  /// <returns>Unicode</returns>
  public char ToUnicode(int charCode) => Pdfium.FPDFFont_UnicodeFromCharCode(this.Handle, charCode);

  /// <summary>Convert given unicode to char code</summary>
  /// <param name="unicode">Unicode</param>
  /// <returns>Char code </returns>
  public int ToCharCode(char unicode) => Pdfium.FPDFFont_CharCodeFromUnicode(this.Handle, unicode);

  internal PdfFont(IntPtr handle) => this.Handle = handle;

  /// <summary>
  /// Create new instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class from native handle.
  /// </summary>
  /// <param name="handle"></param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  public static PdfFont CreateFromHandle(IntPtr handle) => new PdfFont(handle);

  /// <summary>
  /// Creates a standard type 1 font with the specified typeface name and Windows ANSI encoding.
  /// </summary>
  /// <param name="doc">Document object.</param>
  /// <param name="fontName">A string that specifies the typeface name of the font.</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  /// <remarks>See detailed information about standard fonts in <see cref="T:Patagames.Pdf.Enums.FontStockNames" />.
  /// <para>Possible values of <paramref name="fontName" /> are: "Arial", "Arial,Bold", "Arial,BoldItalic", "Arial,Italic", "Arial-Bold", "Arial-BoldItalic", "Arial-BoldItalicMT", "Arial-BoldMT", "Arial-Italic", "Arial-ItalicMT", "ArialBold", "ArialBoldItalic", "ArialItalic", "ArialMT", "ArialMT,Bold", "ArialMT,BoldItalic", "ArialMT,Italic", "ArialRoundedMTBold", "Courier", "Courier,Bold", "Courier,BoldItalic", "Courier,Italic", "Courier-Bold", "Courier-BoldOblique", "Courier-Oblique", "CourierBold", "CourierBoldItalic", "CourierItalic", "CourierNew", "CourierNew,Bold", "CourierNew,BoldItalic", "CourierNew,Italic", "CourierNew-Bold", "CourierNew-BoldItalic", "CourierNew-Italic", "CourierNewBold", "CourierNewBoldItalic", "CourierNewItalic", "CourierNewPS-BoldItalicMT", "CourierNewPS-BoldMT", "CourierNewPS-ItalicMT", "CourierNewPSMT", "CourierStd", "CourierStd-Bold", "CourierStd-BoldOblique", "CourierStd-Oblique", "Helvetica", "Helvetica,Bold", "Helvetica,BoldItalic", "Helvetica,Italic", "Helvetica-Bold", "Helvetica-BoldItalic", "Helvetica-BoldOblique", "Helvetica-Italic", "Helvetica-Oblique", "HelveticaBold", "HelveticaBoldItalic", "HelveticaItalic", "Symbol", "SymbolMT", "Times-Bold", "Times-BoldItalic", "Times-Italic", "Times-Roman", "TimesBold", "TimesBoldItalic", "TimesItalic", "TimesNewRoman", "TimesNewRoman,Bold", "TimesNewRoman,BoldItalic", "TimesNewRoman,Italic", "TimesNewRoman-Bold", "TimesNewRoman-BoldItalic", "TimesNewRoman-Italic", "TimesNewRomanBold", "TimesNewRomanBoldItalic", "TimesNewRomanItalic", "TimesNewRomanPS", "TimesNewRomanPS-Bold", "TimesNewRomanPS-BoldItalic", "TimesNewRomanPS-BoldItalicMT", "TimesNewRomanPS-BoldMT", "TimesNewRomanPS-Italic", "TimesNewRomanPS-ItalicMT", "TimesNewRomanPSMT", "TimesNewRomanPSMT,Bold", "TimesNewRomanPSMT,BoldItalic", "TimesNewRomanPSMT,Italic", "ZapfDingbats".</para>
  /// </remarks>
  public static PdfFont CreateStock(PdfDocument doc, string fontName)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    if ((fontName ?? "").Trim() == "")
      throw new ArgumentException(Error.err0016);
    IntPtr stockFont = Pdfium.FPDF_GetStockFont(doc.Handle, fontName);
    return !(stockFont == IntPtr.Zero) ? new PdfFont(stockFont) : throw new FontNotFoundException();
  }

  /// <summary>
  /// Creates a standard type 1 font with the specified typeface name and Windows ANSI encoding.
  /// </summary>
  /// <param name="doc">Document object.</param>
  /// <param name="fontName">A value that specifies the typeface name of the font.</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  /// <remarks>See detailed information about standard fonts in <see cref="T:Patagames.Pdf.Enums.FontStockNames" /></remarks>
  public static PdfFont CreateStock(PdfDocument doc, FontStockNames fontName)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    IntPtr stockFont = Pdfium.FPDF_GetStockFont(doc.Handle, fontName);
    return !(stockFont == IntPtr.Zero) ? new PdfFont(stockFont) : throw new FontNotFoundException();
  }

  /// <summary>
  /// Creates a standard type font with the specified typeface name and encoding.
  /// </summary>
  /// <param name="doc">A document object.</param>
  /// <param name="fontName">A value that specifies the typeface name of the font.</param>
  /// <param name="encoding">Encoding</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  /// <remarks>
  /// <para>Possible values of <paramref name="encoding" /> are:</para>
  /// <list type="table">
  /// <listheader>
  /// <term>Name</term><term>Value</term><description>Description</description>
  /// </listheader>
  /// <item><term>WinAnsiEncoding</term><term>1</term><description>Windows Code Page 1252, often called the “Windows ANSI” encoding.This is the standard Windows encoding for Latin text in Western writing systems.PDF has a predefined encoding named WinAnsiEncoding that can be used with both Type 1 and TrueType fonts.</description></item>
  /// <item><term>MacRomanEncoding</term><term>2</term><description>Mac OS standard encoding for Latin text in Western writing systems. PDF has a predefined encoding named MacRomanEncoding that can be used with both Type 1 and TrueType fonts.</description></item>
  /// <item><term>MacExpertEncoding</term><term>3</term><description>An encoding for use with expert fonts—ones containing the expert character set.PDF has a predefined encoding named MacExpertEncoding.Despite its name, it is not a platform-specific encoding; however, only certain fonts have the appropriate character set for use with this encoding.No such fonts are among the standard 14 predefined fonts.</description></item>
  /// <item><term>StandardEncoding</term><term>4</term><description>Adobe standard Latin-text encoding. This is the built-in encoding defined in Type 1 Latin-text font programs(but generally not in TrueType font programs). PDF does not have a predefined encoding named StandardEncoding.However, it is useful to describe this encoding, since a font’s built-in encoding can be used as the base encoding from which differences are specified in an encoding dictionary.</description></item>
  /// <item><term>AdobeSymbolEncoding</term><term>5</term><description></description></item>
  /// <item><term>ZapDingBatsEncoding</term><term>6</term><description></description></item>
  /// <item><term>PDFDocEncoding</term><term>7</term><description>Encoding for text strings in a PDF document outside the document’s content streams. This is one of two encodings (the other being Unicode) that can be used to represent text strings; see Section, “Text String Type.” PDF does not have a predefined encoding named PDFDocEncoding; it is not customary to use this encoding to show text from fonts.</description></item>
  /// <item><term>MSSymbolEncoding</term><term>8</term><description></description></item>
  /// </list>
  /// <para>Possible values of <paramref name="fontName" /> are: "Arial", "Arial,Bold", "Arial,BoldItalic", "Arial,Italic", "Arial-Bold", "Arial-BoldItalic", "Arial-BoldItalicMT", "Arial-BoldMT", "Arial-Italic", "Arial-ItalicMT", "ArialBold", "ArialBoldItalic", "ArialItalic", "ArialMT", "ArialMT,Bold", "ArialMT,BoldItalic", "ArialMT,Italic", "ArialRoundedMTBold", "Courier", "Courier,Bold", "Courier,BoldItalic", "Courier,Italic", "Courier-Bold", "Courier-BoldOblique", "Courier-Oblique", "CourierBold", "CourierBoldItalic", "CourierItalic", "CourierNew", "CourierNew,Bold", "CourierNew,BoldItalic", "CourierNew,Italic", "CourierNew-Bold", "CourierNew-BoldItalic", "CourierNew-Italic", "CourierNewBold", "CourierNewBoldItalic", "CourierNewItalic", "CourierNewPS-BoldItalicMT", "CourierNewPS-BoldMT", "CourierNewPS-ItalicMT", "CourierNewPSMT", "CourierStd", "CourierStd-Bold", "CourierStd-BoldOblique", "CourierStd-Oblique", "Helvetica", "Helvetica,Bold", "Helvetica,BoldItalic", "Helvetica,Italic", "Helvetica-Bold", "Helvetica-BoldItalic", "Helvetica-BoldOblique", "Helvetica-Italic", "Helvetica-Oblique", "HelveticaBold", "HelveticaBoldItalic", "HelveticaItalic", "Symbol", "SymbolMT", "Times-Bold", "Times-BoldItalic", "Times-Italic", "Times-Roman", "TimesBold", "TimesBoldItalic", "TimesItalic", "TimesNewRoman", "TimesNewRoman,Bold", "TimesNewRoman,BoldItalic", "TimesNewRoman,Italic", "TimesNewRoman-Bold", "TimesNewRoman-BoldItalic", "TimesNewRoman-Italic", "TimesNewRomanBold", "TimesNewRomanBoldItalic", "TimesNewRomanItalic", "TimesNewRomanPS", "TimesNewRomanPS-Bold", "TimesNewRomanPS-BoldItalic", "TimesNewRomanPS-BoldItalicMT", "TimesNewRomanPS-BoldMT", "TimesNewRomanPS-Italic", "TimesNewRomanPS-ItalicMT", "TimesNewRomanPSMT", "TimesNewRomanPSMT,Bold", "TimesNewRomanPSMT,BoldItalic", "TimesNewRomanPSMT,Italic", "ZapfDingbats".</para>
  /// </remarks>
  public static PdfFont CreateStandardFont(PdfDocument doc, string fontName, int encoding)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    if ((fontName ?? "").Trim() == "")
      throw new ArgumentException(Error.err0016);
    IntPtr handle = Pdfium.FPDF_AddStandardFont(doc.Handle, fontName, encoding);
    return !(handle == IntPtr.Zero) ? new PdfFont(handle) : throw new FontNotFoundException();
  }

  /// <summary>
  /// Creates a standard type font with the specified typeface name and encoding.
  /// </summary>
  /// <param name="doc">A document object.</param>
  /// <param name="fontName">A value that specifies the typeface name of the font.</param>
  /// <param name="encoding">Encoding</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  /// <remarks>
  /// <para>Possible values of <paramref name="encoding" /> are:</para>
  /// <list type="table">
  /// <listheader>
  /// <term>Name</term><term>Value</term><description>Description</description>
  /// </listheader>
  /// <item><term>WinAnsiEncoding</term><term>1</term><description>Windows Code Page 1252, often called the “Windows ANSI” encoding.This is the standard Windows encoding for Latin text in Western writing systems.PDF has a predefined encoding named WinAnsiEncoding that can be used with both Type 1 and TrueType fonts.</description></item>
  /// <item><term>MacRomanEncoding</term><term>2</term><description>Mac OS standard encoding for Latin text in Western writing systems. PDF has a predefined encoding named MacRomanEncoding that can be used with both Type 1 and TrueType fonts.</description></item>
  /// <item><term>MacExpertEncoding</term><term>3</term><description>An encoding for use with expert fonts—ones containing the expert character set.PDF has a predefined encoding named MacExpertEncoding.Despite its name, it is not a platform-specific encoding; however, only certain fonts have the appropriate character set for use with this encoding.No such fonts are among the standard 14 predefined fonts.</description></item>
  /// <item><term>StandardEncoding</term><term>4</term><description>Adobe standard Latin-text encoding. This is the built-in encoding defined in Type 1 Latin-text font programs(but generally not in TrueType font programs). PDF does not have a predefined encoding named StandardEncoding.However, it is useful to describe this encoding, since a font’s built-in encoding can be used as the base encoding from which differences are specified in an encoding dictionary.</description></item>
  /// <item><term>AdobeSymbolEncoding</term><term>5</term><description></description></item>
  /// <item><term>ZapDingBatsEncoding</term><term>6</term><description></description></item>
  /// <item><term>PDFDocEncoding</term><term>7</term><description>Encoding for text strings in a PDF document outside the document’s content streams. This is one of two encodings (the other being Unicode) that can be used to represent text strings; see Section, “Text String Type.” PDF does not have a predefined encoding named PDFDocEncoding; it is not customary to use this encoding to show text from fonts.</description></item>
  /// <item><term>MSSymbolEncoding</term><term>8</term><description></description></item>
  /// </list>
  /// </remarks>
  public static PdfFont CreateStandardFont(PdfDocument doc, FontStockNames fontName, int encoding)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    IntPtr handle = Pdfium.FPDF_AddStandardFont(doc.Handle, fontName, encoding);
    return !(handle == IntPtr.Zero) ? new PdfFont(handle) : throw new FontNotFoundException();
  }

  /// <summary>
  /// Creates a font that has the specified characteristics.
  /// </summary>
  /// <param name="doc">A document object.</param>
  /// <param name="logfont">The <see cref="T:Patagames.Pdf.LOGFONT" /> structure defines the attributes of a font. Please refer <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd145037(v=vs.85).aspx">MSDN</a> for more details.</param>
  /// <param name="bVert">The font is vertical</param>
  /// <param name="bTranslateName">Translate names for CJK fonts</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  public static PdfFont CreateWindowsFont(
    PdfDocument doc,
    LOGFONT logfont,
    bool bVert,
    bool bTranslateName)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    if (logfont == null)
      throw new ArgumentNullException(nameof (logfont));
    IntPtr handle = Pdfium.FPDF_AddWindowsFont(doc.Handle, logfont, bVert, bTranslateName);
    return !(handle == IntPtr.Zero) ? new PdfFont(handle) : throw new FontNotFoundException();
  }

  /// <summary>
  /// Creates a built in font that has the specified characteristics.
  /// </summary>
  /// <param name="doc">PDF document object.</param>
  /// <param name="content">A byte array containing the font program that should be embedded.</param>
  /// <param name="charSet">The character set.</param>
  /// <param name="bVert">The font is vertical.</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  /// <remarks>
  /// <para>A font program can be embedded in a PDF file as data contained in a byte array.</para>
  /// <para>Font programs are subject to copyright, and the copyright owner may impose
  /// conditions under which a font program can be used.These permissions are recorded either in the font program or as part of a separate license.One of the conditions may be that the font program cannot be embedded, in which case it
  /// should not be incorporated into a PDF file.A font program may allow embedding for the sole purpose of viewing and printing the document but not for creating new or modified text that uses the font(in either the same document or other
  /// documents). The latter operation would require the user performing the operation to have a licensed copy of the font program, not a copy extracted from the
  /// PDF file.In the absence of explicit information to the contrary, a PDF consumer
  /// should assume that any embedded font programs are to be used only to view and
  /// print the document and not for any other purposes.</para>
  /// <para>
  /// The following TrueType tables are always required: “head,” “hhea,” “loca,” “maxp,” “cvt ,”
  /// “prep,” “glyf,” “hmtx,” and “fpgm.”
  /// </para>
  /// <para>
  /// Beginning with PDF 1.6, font programs may be embedded using the OpenType
  /// format, which is an extension of the TrueType format that allows inclusion of font
  /// programs using the Compact Font Format(CFF). It also allows inclusion of data
  /// to describe glyph substitutions, kerning, and baseline adjustments.In addition to
  /// rendering glyphs, applications can use the data in OpenType fonts to do advanced
  /// line layout, automatically substitute ligatures, provide selections of alternate
  /// glyphs to users, and handle complicated writing scripts.
  /// </para>
  /// <para>
  /// Like TrueType, OpenType font programs contain a number of tables, as defined
  /// in the OpenType Font Specification. For OpenType fonts
  /// based on TrueType, the “glyf ” table contains the glyph descriptions.For OpenType fonts based on CFF, the “CFF” table is a complete font program containing
  /// the glyph descriptions. These tables, as well as the “cmap” table, are required to be
  /// present when embedding fonts.In addition, for OpenType fonts based on TrueType, the “head,” “hhea,” “loca,” “maxp,” “cvt ,” “prep,” “hmtx,” and “fpgm” tables
  /// are required.
  /// </para>
  /// <note type="note">
  /// Note: Other tables, such as those used for advanced line layout, need not be present;
  /// however, their absence may prevent editing of text containing the font.
  /// </note>
  /// <para>An embedded font program may contain only the subset of glyphs that are used in the PDF document.</para>
  /// </remarks>
  public static PdfFont CreateEmbeddedFont(
    PdfDocument doc,
    byte[] content,
    FontCharSet charSet,
    bool bVert)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    if (content == null)
      throw new ArgumentNullException(nameof (content));
    IntPtr embeddedFont = Pdfium.FPDF_CreateEmbeddedFont(doc.Handle, content, charSet, bVert);
    return !(embeddedFont == IntPtr.Zero) ? new PdfFont(embeddedFont) : throw new FontNotFoundException();
  }

  /// <summary>Creates a font with the specified characteristics.</summary>
  /// <param name="doc">PDF document object.</param>
  /// <param name="faceName">A string that specifies the typeface name of the font. The length of this string must not exceed 32 TCHAR values, including the terminating NULL.  If lfFaceName is an empty string, GDI uses the first font that matches the other specified attributes.</param>
  /// <param name="bTrueType"></param>
  /// <param name="flags">The value of the <see cref="T:Patagames.Pdf.Enums.FontFlags" /> entry.</param>
  /// <param name="weight">The weight of the font.</param>
  /// <param name="italicAngel">The angle, expressed in degrees counterclockwise from the vertical, of the dominant vertical strokes of the font. (For example, the 9-o’clock position is 90 degrees, and the 3-o’clock position is –90 degrees.) The value is negative for fonts that slope to the right, as almost all italic fonts do.</param>
  /// <param name="charSet">The character set.</param>
  /// <param name="bVert">The font is vertical.</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  /// <example>
  /// <code language="c#" source="..\PdfiumExamples\Examples.cs" region="Create font example"></code>
  /// </example>
  public static PdfFont CreateFont(
    PdfDocument doc,
    string faceName,
    bool bTrueType,
    FontFlags flags,
    FontWeight weight,
    int italicAngel,
    FontCharSet charSet,
    bool bVert)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    IntPtr font = Pdfium.FPDF_CreateFont(doc.Handle, faceName ?? "", bTrueType, flags, weight, italicAngel, charSet, bVert);
    return !(font == IntPtr.Zero) ? new PdfFont(font) : throw new FontNotFoundException();
  }

  /// <summary>Creates a font with the specified characteristics.</summary>
  /// <param name="doc">PDF document object.</param>
  /// <param name="faceName">A string that specifies the typeface name of the font. The length of this string must not exceed 32 TCHAR values, including the terminating NULL.  If lfFaceName is an empty string, GDI uses the first font that matches the other specified attributes.</param>
  /// <param name="charSet">Sets a value that specifies the character set.</param>
  /// <param name="isBold">Sets a value that indicates whether this font should be a bold.</param>
  /// <param name="isItalic">Sets a value that indicates whether this font should have italic style applied.</param>
  /// <param name="isVert">The font is vertical.</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  public static PdfFont CreateFont(
    PdfDocument doc,
    string faceName,
    FontCharSet charSet = FontCharSet.DEFAULT_CHARSET,
    bool isBold = false,
    bool isItalic = false,
    bool isVert = false)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    faceName += isBold | isItalic ? "," : "";
    faceName += isBold ? "Bold" : "";
    faceName += isItalic ? "Italic" : "";
    return PdfFont.CreateFont(doc, faceName, true, (FontFlags) 0, FontWeight.FW_DONTCARE, 0, charSet, isVert);
  }

  /// <summary>Creates a font with the specified characteristics.</summary>
  /// <param name="doc">PDF document object.</param>
  /// <param name="dict">A font dictionary.</param>
  /// <returns>An instance of <see cref="T:Patagames.Pdf.Net.PdfFont" /> class.</returns>
  public static PdfFont CreateFont(PdfDocument doc, PdfTypeDictionary dict)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc), Error.err0014);
    if (doc.Handle == IntPtr.Zero)
      throw new ArgumentException(Error.err0015);
    if (dict == null)
      throw new ArgumentNullException(nameof (dict));
    IntPtr fontFromDict = Pdfium.FPDF_CreateFontFromDict(doc.Handle, dict.Handle);
    return !(fontFromDict == IntPtr.Zero) ? new PdfFont(fontFromDict) : throw new FontNotFoundException();
  }
}
