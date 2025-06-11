// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.SubmitFormFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// A set of flags specifying various characteristics of the action <see cref="T:Patagames.Pdf.Net.Actions.PdfSubmitFormAction" />
/// </summary>
[Flags]
public enum SubmitFormFlags
{
  /// <summary>No any flags are setted.</summary>
  None = 0,
  /// <summary>
  /// If clear, the <see cref="P:Patagames.Pdf.Net.Actions.PdfSubmitFormAction.Fields" /> property specifies which fields to
  /// include in the submission. (All descendants of the specified fields in the field hierarchy are submitted as well.)
  /// If set, the <see cref="P:Patagames.Pdf.Net.Actions.PdfSubmitFormAction.Fields" /> property tells which fields to exclude.
  /// All fields in the document’s interactive form are submitted except those listed in the
  /// <see cref="P:Patagames.Pdf.Net.Actions.PdfSubmitFormAction.Fields" /> property and those whose <see cref="F:Patagames.Pdf.Enums.FieldFlags.NoExport" /> flag is set.
  /// </summary>
  IncludeExclude = 1,
  /// <summary>
  /// If set, all fields designated by the Fields array and the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeExclude" /> flag are submitted,
  /// regardless of whether they have a value. For fields without a value, only the field name is transmitted.
  /// If clear, fields without a value are not submitted.
  /// </summary>
  IncludeNoValueFields = 2,
  /// <summary>
  /// Meaningful only if the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.SubmitPDF" /> and <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.XFDF" /> flags are clear.
  /// If set, field names and values are submitted in HTML Form format.
  /// If clear, they are submitted in Forms Data Format (FDF).
  /// </summary>
  ExportFormat = 4,
  /// <summary>
  /// If set, field names and values are submitted using an HTTP GET request.
  /// If clear, they are submitted using a POST request.
  /// This flag is meaningful only when the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flag is set; if <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> is clear,
  /// this flag must also be clear.
  /// </summary>
  GetMethod = 8,
  /// <summary>
  /// If set, the coordinates of the mouse click that caused the submitform action are transmitted as part of the form data.
  /// The coordinate values are relative to the upper-left corner of the field’s widget annotation rectangle.
  /// They are represented in the data in the format name.x = xval<![CDATA[&]]>name.y = yval where name is the field’s mapping name
  /// (TM in the field dictionary) if present; otherwise, name is the field name. If the value of the TM entry is a
  /// single space character, both the name and the dot following it are suppressed, resulting in the format x = xval<![CDATA[&]]>y = yval
  /// This flag is meaningful only when the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flag is set. If <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> is clear,
  /// this flag must also be clear.
  /// </summary>
  SubmitCoordinates = 16, // 0x00000010
  /// <summary>
  /// Meaningful only if the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.SubmitPDF" /> flags are clear. If set, field names and values are submitted as XFDF.
  /// </summary>
  XFDF = 32, // 0x00000020
  /// <summary>
  /// Meaningful only when the form is being submitted in Forms Data Format (that is, when both the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.XFDF" />
  /// and <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flags are clear). If set, the submitted FDF file includes the contents of all incremental
  /// updates to the underlying PDF document, as contained in the Differences entry in the FDF dictionary.
  /// If clear, the incremental updates are not included.
  /// </summary>
  IncludeAppendSaves = 64, // 0x00000040
  /// <summary>
  /// Meaningful only when the form is being submitted in Forms Data Format (that is, when both the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.XFDF" />
  /// and <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flags are clear). If set, the submitted FDF file includes all markup annotations
  /// in the underlying PDF document.
  /// If clear, markup annotations are not included.
  /// </summary>
  IncludeAnnotations = 128, // 0x00000080
  /// <summary>
  /// If set, the document is submitted as PDF, using the MIME content type application/pdf.
  /// If set, other flags are ignored except GetMethod.
  /// </summary>
  SubmitPDF = 256, // 0x00000100
  /// <summary>
  /// If set, any submitted field values representing dates are converted to the standard format.
  /// </summary>
  CanonicalFormat = 512, // 0x00000200
  /// <summary>
  /// Meaningful only when the form is being submitted in Forms Data Format (that is, when both the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.XFDF" />
  /// and <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flags are clear) and the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.IncludeAnnotations" /> flag is set.
  /// If set, it includes only those markup annotations whose <see cref="P:Patagames.Pdf.Net.Annotations.PdfMarkupAnnotation.Text" /> property matches the
  /// name of the current user, as determined by the remote server to which the form is being submitted.
  /// This allows multiple users to collaborate in annotating a single remote PDF document without affecting one another’s annotations.
  /// </summary>
  ExclNonUserAnnots = 1024, // 0x00000400
  /// <summary>
  /// Meaningful only when the form is being submitted in Forms Data Format (that is, when both the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.XFDF" />
  /// and <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flags are clear).
  /// If set, the submitted FDF excludes the F entry
  /// </summary>
  ExclFKey = 2048, // 0x00000800
  /// <summary>
  /// Meaningful only when the form is being submitted in Forms Data Format (that is, when both the <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.XFDF" />
  /// and <see cref="F:Patagames.Pdf.Enums.SubmitFormFlags.ExportFormat" /> flags are clear).
  /// If set, the F entry of the submitted FDF is a file specification containing an embedded file stream representing the
  /// PDF file from which the FDF is being submitted.
  /// </summary>
  EmbedForm = 4096, // 0x00001000
}
