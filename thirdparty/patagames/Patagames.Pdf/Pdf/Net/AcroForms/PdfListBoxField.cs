// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfListBoxField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>Represents a scrollable list box.</summary>
public class PdfListBoxField : PdfChoiceField
{
  /// <summary>
  /// Gets or sets a flag indicating whether the field’s items may be selected simultaneously.
  /// </summary>
  /// <value>
  /// If true, more than one of the field’s items may be selected simultaneously; if false, no more than one item at a time may be selected.
  /// </value>
  public bool MultiSelect
  {
    get => (this.FlagsEx & FieldFlagsEx.MultiSelect) == FieldFlagsEx.MultiSelect;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.MultiSelect));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.MultiSelect));
    }
  }

  /// <summary>
  /// Gets or sets the zero-based index in the <see cref="P:Patagames.Pdf.Net.AcroForms.PdfChoiceField.Items" /> collection of the first item visible in the list.
  /// </summary>
  public int TopItem
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormField_GetTopVisibleIndex(this.Handle);
      if (this.Dictionary != null)
      {
        PdfTypeBase fieldAttribute = this.GetFieldAttribute("TI");
        if (fieldAttribute != null && fieldAttribute.Is<PdfTypeNumber>())
          return fieldAttribute.As<PdfTypeNumber>().IntValue;
      }
      return 0;
    }
    set
    {
      if (value == 0 && this.Dictionary.ContainsKey("TI"))
      {
        this.Dictionary.Remove("TI");
      }
      else
      {
        if (value == 0)
          return;
        this.Dictionary["TI"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfListBoxField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfListBoxField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create new listbox field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="defFont">Default font used for all controls assigned with this field.</param>
  /// <param name="fontSize">Default font's size.</param>
  /// <param name="color">The default text color used when no text color is specified in the control.</param>
  public PdfListBoxField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    PdfFont defFont = null,
    float fontSize = 0.0f,
    FS_COLOR? color = null)
    : base(forms, name, parent, defFont, fontSize, color)
  {
  }
}
