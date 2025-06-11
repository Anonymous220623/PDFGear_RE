// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfCheckBoxField
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Exceptions;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a check box that toggles between two states, on and off.
/// </summary>
public class PdfCheckBoxField : PdfButtonField
{
  /// <summary>
  /// Gets a collection of text strings representing the export value of each <see cref="T:Patagames.Pdf.Net.PdfControl" /> in this field.
  /// </summary>
  public ExportValuesCollection ExportValues { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfCheckBoxField" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The field's handle.</param>
  internal PdfCheckBoxField(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
    this.ExportValues = new ExportValuesCollection((PdfField) this);
  }

  /// <summary>
  /// Create new checkbox field and add it into interactive forms.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="name">The partial field name. Cannot contain a period.</param>
  /// <param name="parent">The parent field. Only non-terminal fields are accepted.</param>
  /// <param name="iconColor">The default checkbox icon color used when the control does not specify an icon color.</param>
  public PdfCheckBoxField(
    PdfInteractiveForms forms,
    string name = null,
    PdfField parent = null,
    FS_COLOR? iconColor = null)
    : base(forms, name, parent, iconColor.HasValue ? PdfFont.CreateStock(forms.FillForms.Document, FontStockNames.ZapfDingbats) : (PdfFont) null, captionColor: iconColor)
  {
    this.ExportValues = new ExportValuesCollection((PdfField) this);
  }

  /// <summary>Toggle Checkbox</summary>
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

  /// <summary>Toggle Checkbox</summary>
  /// <param name="check">True for check, False otherwise</param>
  public void Check(bool check)
  {
    foreach (PdfControl control in this.Controls)
    {
      if (control is PdfCheckBoxControl)
        (control as PdfCheckBoxControl).Check(check);
    }
  }
}
