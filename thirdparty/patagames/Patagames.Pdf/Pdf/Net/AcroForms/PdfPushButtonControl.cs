// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.PdfPushButtonControl
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using System;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a widget annotations that are associated with <see cref="T:Patagames.Pdf.Net.AcroForms.PdfPushButtonField" /> fields.
/// </summary>
public class PdfPushButtonControl : PdfControl
{
  /// <summary>
  /// Gets or sets the controls normal caption, displayed when it is not interacting with the user.
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

  /// <summary>
  /// Gets or sets the controls rollover caption, displayed when the user rolls the cursor into its active area without pressing the mouse button.
  /// </summary>
  public string RolloverCaption
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetRolloverCaption(this.Handle);
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().RolloverCaption : (string) null;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.RolloverCaption = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>
  /// Gets or sets the controls alternate (down) caption, displayed when the mouse button is pressed within its active area.
  /// </summary>
  public string DownCaption
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetDownCaption(this.Handle);
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().DownCaption : (string) null;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.DownCaption = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>
  /// Gets or sets a code indicating where to position the text of the widget annotation’s caption relative to its icon
  /// </summary>
  public TextPositions CaptionPosition
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetTextPosition(this.Handle);
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().CaptionPosition : TextPositions.TEXTPOS_CAPTION;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.CaptionPosition = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.AcroForms.PdfPushButtonControl" /> class.
  /// </summary>
  /// <param name="forms">Interactive forms.</param>
  /// <param name="handle">The control's handle.</param>
  internal PdfPushButtonControl(PdfInteractiveForms forms, IntPtr handle)
    : base(forms, handle)
  {
  }

  /// <summary>
  /// Create a new push button control and assign it to the specified <paramref name="field" />.
  /// </summary>
  /// <param name="field">The field to which this control is assigned./&gt;</param>
  /// <param name="page">The page that will host this control.</param>
  /// <param name="rect">The area on the <paramref name="page" /> where this control should be placed.</param>
  /// <param name="caption">The pushbutton controls’s caption.</param>
  /// <param name="action">An action to be performed when the button is clicked.</param>
  /// <param name="borderColor">The color of the pushbutton control’s border.</param>
  /// <param name="backgroundColor">The color of the pushbutton control’s background.</param>
  /// <param name="captionColor">The color of the pushbutton control’s caption.</param>
  /// <param name="font">The font used for drawing caption.</param>
  /// <param name="fontSize">The font's size.</param>
  /// <param name="icon">The pushbutton controls’s icon.</param>
  /// <param name="highlightingMode">The control’s highlighting mode, the visual effect to be used when the mouse button is pressed or held down inside its active area.</param>
  /// <param name="captionPos">A <see cref="T:Patagames.Pdf.Enums.TextPositions" /> enumeration indicating where to position the text of the pushbutton control’s caption relative to its icon.</param>
  /// <param name="iconMode">The circumstances under which the icon should be scaled inside the control rectangle.</param>
  /// <param name="iconScale">The type of scaling to use.</param>
  /// <param name="horizontalOffset">The number between 0.0 and 1.0 indicating the fraction of leftover space to allocate at the bottom of the icon. A value of 0.0 positions the icon at the bottom corner of the control rectangle. A value of 0.5 centers it within the horizontal direction of rectangle. Is used only if the <paramref name="iconScale" /> is <see cref="F:Patagames.Pdf.Enums.IconScaleTypes.Proportional" />.</param>
  /// <param name="verticalOffset">The number between 0.0 and 1.0 indicating the fraction of leftover space to allocate at the left of the icon. A value of 0.0 positions the icon at the left corner of the control rectangle. A value of 0.5 centers it within the vertical direction of rectangle. Is used only if the <paramref name="iconScale" /> is <see cref="F:Patagames.Pdf.Enums.IconScaleTypes.Proportional" />.</param>
  /// <param name="fitToBounds">A value indicating that the button appearance should be scaled to fit fully within the bounds of the annotation without taking into consideration the line width of the border.</param>
  /// <param name="rotation">A value indicating how the pushbutton control is rotated counterclockwise relative to the <paramref name="page" />.</param>
  /// <param name="borderWidth">The border width in points.</param>
  /// <param name="borderStyle">The border style.</param>
  /// <param name="dashPattern">An array defining a pattern of dashes and gaps to be used in drawing a dashed border. The dash array is specified in the same format as in the line dash pattern parameter of the graphics state. The dash phase is not specified and is assumed to be 0. For example, a DashPattern property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point gaps.</param>
  public PdfPushButtonControl(
    PdfPushButtonField field,
    PdfPage page,
    FS_RECTF rect,
    string caption,
    PdfAction action = null,
    FS_COLOR? borderColor = null,
    FS_COLOR? backgroundColor = null,
    FS_COLOR? captionColor = null,
    PdfFont font = null,
    float fontSize = 0.0f,
    PdfBitmap icon = null,
    FormHighlightingMode highlightingMode = FormHighlightingMode.Push,
    TextPositions captionPos = TextPositions.TEXTPOS_CAPTION,
    IconScaleModes iconMode = IconScaleModes.Always,
    IconScaleTypes iconScale = IconScaleTypes.Proportional,
    float horizontalOffset = 0.5f,
    float verticalOffset = 0.5f,
    bool fitToBounds = false,
    PageRotate rotation = PageRotate.Normal,
    float borderWidth = 1f,
    BorderStyles borderStyle = BorderStyles.Solid,
    float[] dashPattern = null)
  {
    PdfPushButtonField field1 = field;
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
    PdfWidgetAnnotation widgetAnnotation1 = new PdfWidgetAnnotation(page, (PdfTypeBase) this.Dictionary);
    PdfMK pdfMk = widgetAnnotation1.MK ?? new PdfMK(page);
    pdfMk.NormalCaption = caption;
    if (icon != null)
    {
      PdfImageObject pdfImageObject = PdfImageObject.Create(page.Document, icon, 0.0f, 0.0f);
      pdfMk.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      pdfMk.NormalIcon.Add((PdfPageObject) pdfImageObject);
      pdfMk.GenerateAppearance(AppearanceStreamModes.Normal);
    }
    PdfIconFit pdfIconFit = new PdfIconFit();
    pdfIconFit.ScaleType = iconScale;
    pdfIconFit.ScaleMode = iconMode;
    if ((double) verticalOffset < 0.5 || (double) verticalOffset > 0.5)
      pdfIconFit.VerticalOffset = verticalOffset;
    if ((double) horizontalOffset < 0.5 || (double) horizontalOffset > 0.5)
      pdfIconFit.HorizontalOffset = horizontalOffset;
    pdfIconFit.FitToBounds = fitToBounds;
    if (pdfIconFit.Dictionary.Keys.Count > 0)
      pdfMk.IconFit = pdfIconFit;
    pdfMk.CaptionPosition = captionPos;
    if ((PdfWrapper) widgetAnnotation1.MK == (PdfWrapper) null && pdfMk.Dictionary.Keys.Count > 0)
      widgetAnnotation1.MK = pdfMk;
    field.SetDefaultAppearance(widgetAnnotation1.Dictionary, font, fontSize, captionColor, false);
    if (captionColor.HasValue)
    {
      PdfWidgetAnnotation widgetAnnotation2 = widgetAnnotation1;
      nullable = captionColor;
      FS_COLOR fsColor = nullable ?? FS_COLOR.Black;
      widgetAnnotation2.Color = fsColor;
    }
    if (action == null)
      return;
    widgetAnnotation1.Action = action;
  }
}
