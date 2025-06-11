// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.FdfFieldItem
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>A structure that represents a field name-value pair.</summary>
/// <summary>
/// Initialize new instance of the <see cref="T:Patagames.Pdf.Net.FdfFieldItem" /> structure.
/// </summary>
/// <param name="name">The <see cref="P:Patagames.Pdf.Net.PdfField.FullName" /> of the field in the <see cref="T:Patagames.Pdf.Net.FdfDocument" /></param>
/// .
///             <param name="values">An array of values of the field.</param>
public struct FdfFieldItem(string name, string[] values)
{
  /// <summary>
  /// Gets the <see cref="P:Patagames.Pdf.Net.PdfField.FullName" /> of the field in the <see cref="T:Patagames.Pdf.Net.FdfDocument" />.
  /// </summary>
  public string Name { get; private set; } = name;

  /// <summary>Gets an array of values of the field.</summary>
  public string[] Values { get; private set; } = values;
}
