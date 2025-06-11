// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfComboBoxControl
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a widget annotations that are associated with <see cref="T:Patagames.Pdf.Net.AcroForms.PdfComboBoxField" /> fields.
/// </summary>
public class PdfComboBoxControl : PdfControl
{
  /// <summary>
  /// Gets or sets a code specifying the form of justification to be used in displaying the text
  /// </summary>
  public JustifyTypes Alignment
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetControlAlignment(this.Handle);
      if (this.Dictionary != null && this.Dictionary.ContainsKey("Q") && this.Dictionary["Q"].Is<PdfTypeNumber>())
      {
        int alignment = this.Dictionary["Q"].As<PdfTypeNumber>().IntValue;
        if (alignment < 0 || alignment > 2)
          alignment = 0;
        return (JustifyTypes) alignment;
      }
      return this.Field != null ? (this.Field as PdfComboBoxField).Alignment : JustifyTypes.Left;
    }
    set
    {
      if (value == JustifyTypes.Left && this.Dictionary.ContainsKey("Q"))
      {
        this.Dictionary.Remove("Q");
      }
      else
      {
        if (value == JustifyTypes.Left)
          return;
        this.Dictionary["Q"] = (PdfTypeBase) PdfTypeNumber.Create((int) value);
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfComboBoxControl" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The control's handle.</param>
  internal PdfComboBoxControl(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create a new combobox control and assign it to the specified <paramref name="field" />.
  /// </summary>
  /// <param name="field">The field to which this control is assigned./&gt;</param>
  /// <param name="page">The page that will host this control.</param>
  /// <param name="rect">The area on the <paramref name="page" /> where this control should be placed.</param>
  /// <param name="borderColor">The border color of this control.</param>
  /// <param name="backgroundColor">The background color of the control.</param>
  /// <param name="textColor">The color of the text inside this control.</param>
  /// <param name="rotation">A value indicating how the control is rotated counterclockwise relative to the <paramref name="page" />.</param>
  /// <param name="alignment">A code specifying the form of quadding (justification) to be used in displaying the text.</param>
  /// <param name="borderWidth">The border width in points.</param>
  /// <param name="borderStyle">The border style.</param>
  /// <param name="dashPattern">An array defining a pattern of dashes and gaps to be used in drawing a dashed border. The dash array is specified in the same format as in the line dash pattern parameter of the graphics state. The dash phase is not specified and is assumed to be 0. For example, a DashPattern property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point gaps.</param>
  public PdfComboBoxControl(
    PdfComboBoxField field,
    PdfPage page,
    FS_RECTF rect,
    FS_COLOR? borderColor = null,
    FS_COLOR? backgroundColor = null,
    FS_COLOR? textColor = null,
    PageRotate rotation = PageRotate.Normal,
    JustifyTypes alignment = JustifyTypes.Left,
    float borderWidth = 1f,
    BorderStyles borderStyle = BorderStyles.Solid,
    float[] dashPattern = null)
  {
    PdfComboBoxField field1 = field;
    PdfPage page1 = page;
    FS_RECTF rect1 = rect;
    FS_COLOR? nullable = borderColor;
    FS_COLOR borderColor1 = nullable ?? FS_COLOR.DarkGray;
    nullable = backgroundColor;
    FS_COLOR backgroundColor1 = nullable ?? FS_COLOR.White;
    int rotation1 = (int) rotation;
    double borderWidth1 = (double) borderWidth;
    int borderStyle1 = (int) borderStyle;
    float[] dashPattern1 = dashPattern;
    // ISSUE: explicit constructor call
    base.\u002Ector((PdfField) field1, page1, rect1, borderColor1, backgroundColor1, FormHighlightingMode.Invert, (PageRotate) rotation1, (float) borderWidth1, (BorderStyles) borderStyle1, dashPattern1);
    if (!textColor.HasValue)
    {
      float[] fillColor;
      Pdfium.FPDFTOOLS_ParseDefaultAppearance(field.GetDefaultAppearance(), out float[] _, out fillColor, out string _, out float _, out FS_MATRIX _);
      textColor = fillColor != null ? new FS_COLOR?(new FS_COLOR(fillColor)) : new FS_COLOR?(FS_COLOR.Black);
    }
    this.Alignment = alignment;
    if (!textColor.HasValue)
      return;
    PdfWidgetAnnotation widgetAnnotation = new PdfWidgetAnnotation(page, (PdfTypeBase) this.Dictionary);
    field.SetDefaultAppearance(widgetAnnotation.Dictionary, (PdfFont) null, 0.0f, textColor, false);
    widgetAnnotation.Color = textColor.Value;
  }
}
