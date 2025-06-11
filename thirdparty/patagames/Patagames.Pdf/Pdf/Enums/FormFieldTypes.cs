// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.FormFieldTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Form field Types</summary>
public enum FormFieldTypes
{
  /// <summary>-1 indicates no fields</summary>
  FPDF_FORMFIELD_NOFIELDS = -1, // 0xFFFFFFFF
  /// <summary>Unknown</summary>
  FPDF_FORMFIELD_UNKNOWN = 0,
  /// <summary>push button type.</summary>
  FPDF_FORMFIELD_PUSHBUTTON = 1,
  /// <summary>check box type.</summary>
  FPDF_FORMFIELD_CHECKBOX = 2,
  /// <summary>radio button type.</summary>
  FPDF_FORMFIELD_RADIOBUTTON = 3,
  /// <summary>combo box type.</summary>
  FPDF_FORMFIELD_COMBOBOX = 4,
  /// <summary>list box type.</summary>
  FPDF_FORMFIELD_LISTBOX = 5,
  /// <summary>text field type.</summary>
  FPDF_FORMFIELD_TEXTFIELD = 6,
  /// <summary>Signature</summary>
  FPDF_FORMFIELD_SIGNATURE = 7,
  /// <summary>The last elemеnt in this enumeration</summary>
  Last = 8,
}
