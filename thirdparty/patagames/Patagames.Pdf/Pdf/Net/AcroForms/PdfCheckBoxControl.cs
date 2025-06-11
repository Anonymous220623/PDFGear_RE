// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfCheckBoxControl
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a widget annotations that are associated with <see cref="T:Patagames.Pdf.Net.AcroForms.PdfCheckBoxField" /> fields.
/// </summary>
public class PdfCheckBoxControl : PdfControl
{
  /// <summary>
  /// Get a string describing the check status of the Checkbox.
  /// </summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Thrown by newly created Controls that have not yet been added to the DOM.</exception>
  public string ExportValue
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormControl_GetExportValue(this.Handle) : throw new DomException((PdfControl) this);
    }
  }

  /// <summary>
  /// The controls normal caption, displayed when it is not interacting with the user.
  /// </summary>
  public string NormalCaption
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetNormalCaption(this.Handle);
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().NormalCaption : (string) null;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.NormalCaption = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>Gets whether the Checkbox is checked.</summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Thrown by newly created Controls that have not yet been added to the DOM.</exception>
  public bool IsChecked
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormControl_IsChecked(this.Handle) : throw new DomException((PdfControl) this);
    }
  }

  /// <summary>Get a default state of the checkbox.</summary>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Thrown by newly created Controls that have not yet been added to the DOM.</exception>
  public bool IsDefaultChecked
  {
    get
    {
      return !(this.Handle == IntPtr.Zero) ? Pdfium.FPDFFormControl_IsDefaultChecked(this.Handle) : throw new DomException((PdfControl) this);
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfCheckBoxControl" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The control's handle.</param>
  internal PdfCheckBoxControl(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create a new checkbox control and assign it to the specified <paramref name="field" />.
  /// </summary>
  /// <param name="field">The field to which this control is assigned./&gt;</param>
  /// <param name="page">The page that will host this control.</param>
  /// <param name="rect">The area on the <paramref name="page" /> where this control should be placed.</param>
  /// <param name="icon">The appearance of an icon that indicates the checked state of this control.</param>
  /// <param name="borderColor">The border color of this control.</param>
  /// <param name="backgroundColor">The background color of the control.</param>
  /// <param name="iconColor">The color of the control’s icon.</param>
  /// <param name="highlightingMode">The control’s highlighting mode, the visual effect to be used when the mouse button is pressed or held down inside its active area.</param>
  /// <param name="rotation">A value indicating how the control is rotated counterclockwise relative to the <paramref name="page" />.</param>
  /// <param name="borderWidth">The border width in points.</param>
  /// <param name="borderStyle">The border style.</param>
  /// <param name="dashPattern">An array defining a pattern of dashes and gaps to be used in drawing a dashed border. The dash array is specified in the same format as in the line dash pattern parameter of the graphics state. The dash phase is not specified and is assumed to be 0. For example, a DashPattern property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point gaps.</param>
  public PdfCheckBoxControl(
    PdfCheckBoxField field,
    PdfPage page,
    FS_RECTF rect,
    CheckboxCaptions icon = CheckboxCaptions.Check,
    FS_COLOR? borderColor = null,
    FS_COLOR? backgroundColor = null,
    FS_COLOR? iconColor = null,
    FormHighlightingMode highlightingMode = FormHighlightingMode.Invert,
    PageRotate rotation = PageRotate.Normal,
    float borderWidth = 1f,
    BorderStyles borderStyle = BorderStyles.Solid,
    float[] dashPattern = null)
  {
    PdfCheckBoxField field1 = field;
    PdfPage page1 = page;
    FS_RECTF rect1 = rect;
    FS_COLOR? nullable = borderColor;
    FS_COLOR borderColor1 = nullable ?? FS_COLOR.DarkGray;
    nullable = backgroundColor;
    FS_COLOR backgroundColor1 = nullable ?? FS_COLOR.LightGray;
    int num = (int) highlightingMode;
    int rotation1 = (int) rotation;
    double borderWidth1 = (double) borderWidth;
    int borderStyle1 = (int) borderStyle;
    float[] dashPattern1 = dashPattern;
    // ISSUE: explicit constructor call
    base.\u002Ector((PdfField) field1, page1, rect1, borderColor1, backgroundColor1, (FormHighlightingMode) num, (PageRotate) rotation1, (float) borderWidth1, (BorderStyles) borderStyle1, dashPattern1);
    if (!iconColor.HasValue)
    {
      float[] fillColor;
      Pdfium.FPDFTOOLS_ParseDefaultAppearance(field.GetDefaultAppearance(), out float[] _, out fillColor, out string _, out float _, out FS_MATRIX _);
      iconColor = fillColor != null ? new FS_COLOR?(new FS_COLOR(fillColor)) : new FS_COLOR?(FS_COLOR.Black);
    }
    PdfWidgetAnnotation widgetAnnotation = new PdfWidgetAnnotation(page, (PdfTypeBase) this.Dictionary);
    PdfMK pdfMk = widgetAnnotation.MK ?? new PdfMK(page);
    pdfMk.NormalCaption = Pdfium.GetEnumDescription((Enum) icon);
    if ((PdfWrapper) widgetAnnotation.MK == (PdfWrapper) null && pdfMk.Dictionary.Keys.Count > 0)
      widgetAnnotation.MK = pdfMk;
    if (iconColor.HasValue)
    {
      field.SetDefaultAppearance(widgetAnnotation.Dictionary, PdfFont.CreateStock(page.Document, FontStockNames.ZapfDingbats), 0.0f, iconColor, false);
      widgetAnnotation.Color = iconColor.Value;
    }
    field.Dictionary["V"] = (PdfTypeBase) PdfTypeName.Create("Off");
  }

  /// <summary>Toggle Checkbox</summary>
  /// <param name="check">True for check, False otherwise</param>
  /// <returns>True for success, False otherwise.</returns>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.DomException">Thrown if the underlying field has not yet been added to the DOM.</exception>
  public bool Check(bool check)
  {
    int index = this.Field.Controls.IndexOf((PdfControl) this);
    return index >= 0 && (this.Field as PdfCheckBoxField).Check(index, check);
  }
}
