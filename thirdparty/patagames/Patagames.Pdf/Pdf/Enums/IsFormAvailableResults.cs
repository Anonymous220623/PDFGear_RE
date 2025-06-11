// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.IsFormAvailableResults
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The result of the process which check availabilty the forms inside PDF.
/// </summary>
public enum IsFormAvailableResults
{
  /// <summary>
  /// error, the input parameter not correct, such as hints is null.
  /// </summary>
  Error = -1, // 0xFFFFFFFF
  /// <summary>data not available</summary>
  NotAvaialble = 0,
  /// <summary>data available</summary>
  Available = 1,
  /// <summary>No form data.</summary>
  NoFormData = 2,
}
