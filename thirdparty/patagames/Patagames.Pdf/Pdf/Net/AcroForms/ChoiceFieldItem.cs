// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.ChoiceFieldItem
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents an item in a <see cref="T:Patagames.Pdf.Net.AcroForms.PdfListBoxField" /> or <see cref="T:Patagames.Pdf.Net.AcroForms.PdfComboBoxField" />.
/// </summary>
public struct ChoiceFieldItem
{
  /// <summary>
  /// Gets the text to be displayed as the name of the <see cref="T:Patagames.Pdf.Net.AcroForms.ChoiceFieldItem" />.
  /// </summary>
  public string Name { get; }

  /// <summary>Gets the items’s export value.</summary>
  public string Value { get; }

  /// <summary>Create new item without export value.</summary>
  /// <param name="name">The text to be displayed as the name of the item.</param>
  public ChoiceFieldItem(string name)
  {
    this.Name = name;
    this.Value = (string) null;
  }

  /// <summary>Create new item.</summary>
  /// <param name="name">The text to be displayed as the name of the item.</param>
  /// <param name="value">The items’s export value.&gt;</param>
  public ChoiceFieldItem(string name, string value)
  {
    this.Name = name;
    this.Value = value;
  }
}
