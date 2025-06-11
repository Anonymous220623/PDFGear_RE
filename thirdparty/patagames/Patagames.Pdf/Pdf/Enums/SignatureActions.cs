// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.SignatureActions
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.ComponentModel;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// An enumeration which, in conjunction with <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Fields" />, indicates the set of fields that should be locked.
/// </summary>
public enum SignatureActions
{
  /// <summary>All fields in the document.</summary>
  [Description("All")] All,
  /// <summary>
  /// All fields specified in <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Fields" />.
  /// </summary>
  [Description("Include")] Include,
  /// <summary>
  /// All fields except those specified in <see cref="P:Patagames.Pdf.Net.Wrappers.PdfLock.Fields" />.
  /// </summary>
  [Description("Exclude")] Exclude,
}
