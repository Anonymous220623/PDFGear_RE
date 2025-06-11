// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfTextBoxControl
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
/// Represents a widget annotations that are associated with <see cref="T:Patagames.Pdf.Net.AcroForms.PdfTextBoxField" /> fields.
/// </summary>
public class PdfTextBoxControl : PdfControl
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
      return this.Field != null ? (this.Field as PdfTextBoxField).Alignment : JustifyTypes.Left;
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
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfTextBoxControl" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The control's handle.</param>
  internal PdfTextBoxControl(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>Create new pushbutton field.</summary>
  /// <param name="field">The pushbutton field to which this control is assigned./&gt;</param>
  /// <param name="page">The page on which this control will be placed.</param>
  /// <param name="rect">The area on the page where the control should fit.</param>
  /// <param name="borderColor">The border color of this control.</param>
  /// <param name="backgroundColor">The background color of the control.</param>
  /// <param name="textColor">The color of the text inside this textbox control.</param>
  /// <param name="font">The font used for drawing textbox control's text.</param>
  /// <param name="fontSize">The <paramref name="font" /> size.</param>
  /// <param name="rotation">A value indicating how the control is rotated counterclockwise relative to the <paramref name="page" />.</param>
  /// <param name="alignment">A code specifying the form of quadding (justification) to be used in displaying the text.</param>
  /// <param name="borderWidth">The border width in points.</param>
  /// <param name="borderStyle">The border style.</param>
  /// <param name="dashPattern">An array defining a pattern of dashes and gaps to be used in drawing a dashed border. The dash array is specified in the same format as in the line dash pattern parameter of the graphics state. The dash phase is not specified and is assumed to be 0. For example, a DashPattern property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point gaps.</param>
  public PdfTextBoxControl(
    PdfTextBoxField field,
    PdfPage page,
    FS_RECTF rect,
    FS_COLOR? borderColor = null,
    FS_COLOR? backgroundColor = null,
    FS_COLOR? textColor = null,
    PdfFont font = null,
    float fontSize = 0.0f,
    PageRotate rotation = PageRotate.Normal,
    JustifyTypes alignment = JustifyTypes.Left,
    float borderWidth = 1f,
    BorderStyles borderStyle = BorderStyles.Solid,
    float[] dashPattern = null)
  {
    PdfTextBoxField field1 = field;
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
    this.Alignment = alignment;
    PdfWidgetAnnotation widgetAnnotation = new PdfWidgetAnnotation(page, (PdfTypeBase) this.Dictionary);
    field.SetDefaultAppearance(widgetAnnotation.Dictionary, font, fontSize, textColor, false);
    if (!textColor.HasValue)
      return;
    widgetAnnotation.Color = textColor.Value;
  }
}
