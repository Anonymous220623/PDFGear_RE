// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfRadioButtonField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents radio button fields which contain a set of related buttons that can each be on or off.
/// Typically, at most one radio button in a set may be on at any given time, and selecting any one of the buttons automatically deselects all the others.
/// </summary>
public class PdfRadioButtonField : PdfButtonField
{
  /// <summary>
  /// Gets a collection of text strings representing the export value of each <see cref="T:Patagames.Pdf.Net.PdfControl" /> in this field.
  /// </summary>
  public ExportValuesCollection ExportValues { get; private set; }

  /// <summary>
  /// Gets or sets a value indicating whether one radio button must be selected at all times.
  /// </summary>
  /// <value>
  /// If true, exactly one radio button must be selected at all times; clicking the currently selected button has no effect.
  /// If false, clicking the selected button deselects it, leaving no button selected.
  /// </value>
  public bool NoToggleToOff
  {
    get => (this.FlagsEx & FieldFlagsEx.NoToggleToOff) == FieldFlagsEx.NoToggleToOff;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.NoToggleToOff));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.NoToggleToOff));
    }
  }

  /// <summary>
  /// Gets or sets a value indicating whether radio buttons with the same export value will turn off and on together.
  /// </summary>
  /// <remarks>
  /// If true, a group of radio buttons within a radio button field that
  /// use the same value for the on state will turn on and off in unison;
  /// that is if one is checked, they are all checked.
  /// If false, the buttons are mutually exclusive.
  /// </remarks>
  public bool RadiosInUnison
  {
    get => (this.FlagsEx & FieldFlagsEx.RadiosInUnison) == FieldFlagsEx.RadiosInUnison;
    set
    {
      if (value)
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx | FieldFlagsEx.RadiosInUnison));
      else
        this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create((int) (this.FlagsEx & ~FieldFlagsEx.RadiosInUnison));
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfRadioButtonField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfRadioButtonField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
    this.ExportValues = new ExportValuesCollection((PdfField) this);
  }

  /// <summary>
  /// Create new radiobutton field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="iconColor">The default radiobutton icon color used when the control does not specify an icon color.</param>
  public PdfRadioButtonField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    FS_COLOR? iconColor = null)
    : base(forms, name, parent, iconColor.HasValue ? PdfFont.CreateStock(forms.FillForms.Document, FontStockNames.ZapfDingbats) : (PdfFont) null, captionColor: iconColor)
  {
    this.Dictionary["Ff"] = (PdfTypeBase) PdfTypeNumber.Create(32768 /*0x8000*/);
    this.NoToggleToOff = true;
    this.ExportValues = new ExportValuesCollection((PdfField) this);
  }

  /// <summary>Toggle Radiobutton</summary>
  /// <param name="index">Zerobased index of field's control</param>
  /// <param name="check">True for check, False otherwise</param>
  /// <returns>True for success, False otherwise.</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Generated for newly created fields that have not yet been added to the DOM.</exception>
  public bool Check(int index, bool check)
  {
    if (this.Handle == IntPtr.Zero)
      throw new DomException((PdfField) this);
    return Pdfium.FPDFFormField_CheckControl(this.Handle, index, check, this.IsNotify);
  }
}
