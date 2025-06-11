// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.DocumentTags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The tag for the meta data.<markup><br /></markup>
/// Currently, It can be Title, Author, Subject
/// Keywords, Creator, Producer, CreationDate, ModDate or  "Trapped".<markup><br /></markup>
/// For detailed explanation of these tags and their respective values, please refer
/// to <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</a>, section 10.2.1, "Document Information Dictionary".
/// </summary>
/// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
public enum DocumentTags
{
  /// <summary>(Optional; PDF 1.1) The document’s title.</summary>
  [Description("Title")] Title,
  /// <summary>
  /// (Optional) The name of the person who created the document.
  /// </summary>
  [Description("Author")] Author,
  /// <summary>(Optional; PDF 1.1) The subject of the document.</summary>
  [Description("Subject")] Subject,
  /// <summary>
  /// (Optional; PDF 1.1) Keywords associated with the document.
  /// </summary>
  [Description("Keywords")] Keywords,
  /// <summary>
  /// (Optional) If the document was converted to PDF from another format, the name of the application (for example, Adobe FrameMaker®) that created the original document from which it was converted.
  /// </summary>
  [Description("Creator")] Creator,
  /// <summary>
  /// (Optional) If the document was converted to PDF from another format, the name of the application (for example, Acrobat Distiller) that converted it to PDF.
  /// </summary>
  [Description("Producer")] Producer,
  /// <summary>
  /// (Optional) The date and time the document was created, in human-readable form (see Section 3.8.3, “Dates”).
  /// </summary>
  [Description("CreationDate")] CreationDate,
  /// <summary>
  /// (Required if PieceInfo is present in the document catalog; otherwise optional; PDF 1.1) The date and time the document was most recently modified, in human-readable form (see Section 3.8.3, “Dates”).
  /// </summary>
  [Description("ModDate")] ModificationDate,
  /// <summary>
  /// (Optional; PDF 1.3) A name object indicating whether the document has
  /// been modified to include trapping information (see Section 10.10.5, “Trapping Support”):
  /// True The document has been fully trapped; no further trapping is
  /// needed. (This is the name True, not the boolean value true.)
  /// False The document has not yet been trapped; any desired
  /// trapping must still be done. (This is the name False, not the
  /// boolean value false.)
  /// Unknown Either it is unknown whether the document has been
  /// trapped or it has been partly but not yet fully trapped; some
  /// additional trapping may still be needed.
  /// Default value: Unknown.
  /// The value of this entry may be set automatically by the software creating the
  /// document’s trapping information, or it may be known only to a human operator
  /// and entered manually.
  /// </summary>
  [Description("Trapped")] Trapped,
}
