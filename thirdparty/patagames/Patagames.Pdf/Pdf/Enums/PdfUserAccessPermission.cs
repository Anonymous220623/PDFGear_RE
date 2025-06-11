// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.PdfUserAccessPermission
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Specifies which operations are permitted when the document is opened with user access.
/// </summary>
/// <remarks>
/// <para>Bit 1–2 Reserved; must be 0.</para>
/// <para>Bit 7–8 Reserved; must be 1.</para>
/// <para>Bit 13–32 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Reserved; must be 1.</para>
/// <para>Please refer to <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference</a> for detailed description.</para>
/// </remarks>
/// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
[Flags]
public enum PdfUserAccessPermission : uint
{
  /// <summary>
  /// No permissions. Printing, modifying, annotating, extracting pages, etc. not allowed.
  /// </summary>
  NoPermissions = 4294963392, // 0xFFFFF0C0
  /// <summary>
  /// Represents reserved bits (PermitReserved | PermitReserved2)
  /// </summary>
  Reserved = NoPermissions, // 0xFFFFF0C0
  /// <summary>Permits everything. This is the default value.</summary>
  PermitAll = 4294967292, // 0xFFFFFFFC
  /// <summary>
  /// Bit 3 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 2) Print the document.
  /// (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Print the document (possibly not at the highest
  /// quality level, depending on whether bit 12 is also set).
  /// </summary>
  PermitPrint = 4,
  /// <summary>Bit 4 Modify the contents of the document by operations other than
  /// those controlled by bits 6, 9, and 11.
  /// </summary>
  PermitModifyDocument = 8,
  /// <summary>Bit 5 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 2) Copy or otherwise extract text and graphics from the
  /// document, including extracting text and graphics (in support of accessibility
  /// to users with disabilities or for other purposes).
  /// (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Copy or otherwise extract text and graphics
  /// from the document by operations other than that controlled by bit 10.
  /// </summary>
  PermitExtractContent = 16, // 0x00000010
  /// <summary>Bit 6 Add or modify text annotations, fill in interactive form fields, and,
  /// if bit 4 is also set, create or modify interactive form fields (including
  /// signature fields).
  /// </summary>
  PermitAnnotations = 32, // 0x00000020
  /// <summary>Bit 7-8 Reserver; must be 1</summary>
  PermitReserved = 192, // 0x000000C0
  /// <summary>9 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Fill in existing interactive form fields (including
  /// signature fields), even if bit 6 is clear.
  /// </summary>
  PermitFormsFill = 256, // 0x00000100
  /// <summary>Bit 10 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Extract text and graphics (in support of accessibility
  /// to users with disabilities or for other purposes).
  /// </summary>
  PermitAccessibilityExtractContent = 512, // 0x00000200
  /// <summary>Bit 11 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Assemble the document (insert, rotate, or delete
  /// pages and create bookmarks or thumbnail images), even if bit 4
  /// is clear.
  /// </summary>
  PermitAssembleDocument = 1024, // 0x00000400
  /// <summary>Bit 12 (<see cref="P:Patagames.Pdf.Net.PdfDocument.SecurityRevision" /> 3 or greater) Print the document to a representation from
  /// which a faithful digital copy of the PDF content could be generated.
  /// When this bit is clear (and bit 3 is set), printing is limited to a lowlevel
  /// representation of the appearance, possibly of degraded quality.
  /// </summary>
  PermitFullQualityPrint = 2048, // 0x00000800
  /// <summary>Bit 13-32. Reserved; must be 1</summary>
  PermitReserved2 = 4294963200, // 0xFFFFF000
}
