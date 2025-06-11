// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfComboBoxField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a combo box that consisting of a drop-down list optionally accompanied by an editable text box in which the user can type a value other than the predefined choices.
/// </summary>
public class PdfComboBoxField : PdfChoiceField
{
  /// <summary>
  /// Gets or sets a flag indicating whether the combo box includes an editable text box as well as a dropdown list.
  /// </summary>
  /// <value>
  /// If true, the combo box includes an editable text box as well as a dropdown list;
  /// if false, it includes only a drop-down list.
  /// </value>
  public bool Edit
  {
    get => (this.FlagsEx & FieldFlagsEx.Edit) == FieldFlagsEx.Edit;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.Edit));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.Edit));
    }
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the text entered in the field should be spell-checked or not.
  /// </summary>
  /// <value>
  /// If true, text entered in the field is not spell-checked.
  /// This flag is meaningful only if the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfComboBoxField.Edit" /> property is true.
  /// </value>
  public bool DoNotSpellCheck
  {
    get => (this.FlagsEx & FieldFlagsEx.DoNotSpellCheck) == FieldFlagsEx.DoNotSpellCheck;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.DoNotSpellCheck));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.DoNotSpellCheck));
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfComboBoxField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfComboBoxField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create new combobox field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="defFont">Default font used for all controls assigned with this field.</param>
  /// <param name="fontSize">Default font's size.</param>
  /// <param name="color">The default text color used when no text color is specified in the control.</param>
  public PdfComboBoxField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    PdfFont defFont = null,
    float fontSize = 0.0f,
    FS_COLOR? color = null)
    : base(forms, name, parent, defFont, fontSize, color)
  {
    this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create(131072 /*0x020000*/);
  }
}
