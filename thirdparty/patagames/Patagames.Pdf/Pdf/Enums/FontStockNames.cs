// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FontStockNames
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Represents standard Type 1 Fonts (Standard 14 Fonts). The PostScript names of 14 Type 1 fonts, known as the standard 14 fonts, are as follows:
/// </summary>
/// <remarks>
/// These fonts, or their font metrics and suitable substitution fonts, must be available
/// to the consumer application. The character sets and encodings for these
/// fonts are listed in Appendix D of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</a>.
/// The Adobe font metrics (AFM) files for the standard
/// 14 fonts are available from the ASN Web site. For
/// more information on font metrics, see Adobe Technical Note #5004, Adobe Font Metrics File Format Specification.
/// <para>
/// Ordinarily, a font dictionary that refers to one of the standard fonts should omit
/// the FirstChar, LastChar, Widths, and FontDescriptor entries. However, it is permissible
/// to override a standard font by including these entries and embedding the
/// font program in the PDF file. (See implementation note 62 in Appendix H in <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</a>.)
/// </para>
/// <note type="note">Beginning with PDF 1.5, the special treatment given to the standard 14 fonts
/// is deprecated. All fonts used in a PDF document should be represented using a complete
/// font descriptor. For backwards capability, viewer applications must still provide
/// the special treatment identified for the standard 14 fonts. </note>
/// </remarks>
public enum FontStockNames
{
  /// <summary>Synonym for Helvetica</summary>
  [Description("Helvetica")] Arial,
  /// <summary>Synonym for Helvetica−Bold</summary>
  [Description("Helvetica-Bold")] ArialBold,
  /// <summary>Synonym for Helvetica−BoldOblique</summary>
  [Description("Helvetica-BoldOblique")] ArialBoldOblique,
  /// <summary>Synonym for Helvetica−Oblique</summary>
  [Description("Helvetica-Oblique")] ArialOblique,
  /// <summary>Courier</summary>
  [Description("Courier")] Courier,
  /// <summary>Courier−Bold</summary>
  [Description("Courier-Bold")] CourierBold,
  /// <summary>Courier−BoldOblique</summary>
  [Description("Courier-BoldOblique")] CourierBoldOblique,
  /// <summary>Courier−Oblique</summary>
  [Description("Courier-Oblique")] CourierOblique,
  /// <summary>Helvetica</summary>
  [Description("Helvetica")] Helvetica,
  /// <summary>Helvetica−Bold</summary>
  [Description("Helvetica-Bold")] HelveticaBold,
  /// <summary>Helvetica−BoldOblique</summary>
  [Description("Helvetica-BoldOblique")] HelveticaBoldOblique,
  /// <summary>Helvetica−Oblique</summary>
  [Description("Helvetica-Oblique")] HelveticaOblique,
  /// <summary>Times−Roman</summary>
  [Description("Times-Roman")] TimesRoman,
  /// <summary>Times−Bold</summary>
  [Description("Times-Bold")] TimesBold,
  /// <summary>Times−BoldItalic</summary>
  [Description("Times-BoldItalic")] TimesBoldItalic,
  /// <summary>Times−Italic</summary>
  [Description("Times-Italic")] TimesItalic,
  /// <summary>Symbol</summary>
  [Description("Symbol")] Symbol,
  /// <summary>ZapfDingbats</summary>
  [Description("ZapfDingbats")] ZapfDingbats,
}
