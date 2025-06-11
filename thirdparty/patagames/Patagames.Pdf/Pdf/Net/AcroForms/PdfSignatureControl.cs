// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfSignatureControl
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
/// Represents a widget annotations that are associated with <see cref="T:Patagames.Pdf.Net.AcroForms.PdfSignatureField" /> fields.
/// </summary>
public class PdfSignatureControl : PdfControl
{
  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfSignatureControl" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The control's handle.</param>
  internal PdfSignatureControl(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create a new checkbox control and assign it to the specified <paramref name="field" />.
  /// </summary>
  /// <param name="field">The field to which this control is assigned./&gt;</param>
  /// <param name="page">The page that will host this control.</param>
  /// <param name="rect">The area on the <paramref name="page" /> where this control should be placed.</param>
  /// <param name="borderColor">The border color of this control.</param>
  /// <param name="rotation">A value indicating how the control is rotated counterclockwise relative to the <paramref name="page" />.</param>
  /// <param name="borderWidth">The border width in points.</param>
  /// <param name="dashPattern">An array defining a pattern of dashes and gaps to be used in drawing a dashed border. The dash array is specified in the same format as in the line dash pattern parameter of the graphics state. The dash phase is not specified and is assumed to be 0. For example, a DashPattern property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point gaps.</param>
  public PdfSignatureControl(
    PdfSignatureField field,
    PdfPage page,
    FS_RECTF rect,
    FS_COLOR? borderColor = null,
    PageRotate rotation = PageRotate.Normal,
    float borderWidth = 1f,
    float[] dashPattern = null)
    : base((PdfField) field, page, rect, borderColor ?? FS_COLOR.DarkGray, new FS_COLOR(0), FormHighlightingMode.Invert, rotation, borderWidth, dashPattern == null ? BorderStyles.Solid : BorderStyles.Dashed, dashPattern)
  {
    new PdfWidgetAnnotation(page, (PdfTypeBase) this.Dictionary).Color = this.BorderColor;
  }
}
