// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfControl
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.AcroForms;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Encapsulates widget annotations to represent the appearance of fields and to manage user interactions.
/// </summary>
public class PdfControl
{
  private PdfInteractiveForms _forms;
  private PdfTypeDictionary _dictionary;
  private PdfWidgetAnnotation _widget;
  private PdfField _tmpFieldDoNotUseItAnywhereExceptInsideFieldProperty;
  private PdfBorderStyle _borderStyle;

  /// <summary>
  /// Gets the Pdfium SDK handle to which the control is bound.
  /// </summary>
  public IntPtr Handle { get; private set; }

  /// <summary>Gets the type of an underlying field.</summary>
  public FormFieldTypesEx Type
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetType(this.Handle);
      switch (this)
      {
        case PdfCheckBoxControl _:
          return FormFieldTypesEx.CheckBox;
        case PdfRadioButtonControl _:
          return FormFieldTypesEx.RadioButton;
        case PdfPushButtonControl _:
          return FormFieldTypesEx.PushButton;
        case PdfListBoxControl _:
          return FormFieldTypesEx.ListBox;
        case PdfComboBoxControl _:
          return FormFieldTypesEx.ComboBox;
        case PdfTextBoxControl _ when this.Field is PdfTextBoxField && (this.Field as PdfTextBoxField).IsRichText:
          return FormFieldTypesEx.RichText;
        case PdfTextBoxControl _ when this.Field is PdfTextBoxField && (this.Field as PdfTextBoxField).FileSelect:
          return FormFieldTypesEx.File;
        case PdfTextBoxControl _:
          return FormFieldTypesEx.Text;
        case PdfSignatureControl _:
          return FormFieldTypesEx.Sign;
        default:
          return FormFieldTypesEx.Unknown;
      }
    }
  }

  /// <summary>Gets an underlying field for this control.</summary>
  public PdfField Field
  {
    get
    {
      return this.Handle == IntPtr.Zero ? this._tmpFieldDoNotUseItAnywhereExceptInsideFieldProperty : this._forms.Fields.GetByHandle(Pdfium.FPDFFormControl_GetField(this.Handle));
    }
  }

  /// <summary>Gets or sets the rectangle of the control.</summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public FS_RECTF Rect
  {
    get
    {
      if (this.Handle == IntPtr.Zero && this.Dictionary != null)
        return this.Dictionary.GetRectBy(nameof (Rect));
      float left;
      float bottom;
      float right;
      float top;
      Pdfium.FPDFFormControl_GetRect(this.Handle, out left, out bottom, out right, out top);
      return new FS_RECTF()
      {
        left = left,
        bottom = bottom,
        right = right,
        top = top
      };
    }
    set
    {
      if (this.Dictionary == null)
        return;
      this.Dictionary.SetRectAt(nameof (Rect), value);
    }
  }

  /// <summary>Gets or sets the highlight mode for the control.</summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public FormHighlightingMode HiglightingMode
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetHighlightingMode(this.Handle);
      FormHighlightingMode result;
      return this.Dictionary != null && this.Dictionary.ContainsKey("H") && this.Dictionary["H"].Is<PdfTypeName>() && Pdfium.GetEnumDescription<FormHighlightingMode>(this.Dictionary["H"].As<PdfTypeName>().Value, out result) ? result : FormHighlightingMode.Invert;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      if ((value == FormHighlightingMode.Invert || (enumDescription ?? "").Trim() == "") && this.Dictionary.ContainsKey("H"))
        this.Dictionary.Remove("H");
      else
        this.Dictionary["H"] = (PdfTypeBase) PdfTypeName.Create(enumDescription);
    }
  }

  /// <summary>
  ///  Gets or sets a value indicating how the control is rotated counterclockwise relative to the page.
  /// </summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public PageRotate Rotation
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
      {
        int rotation = Pdfium.FPDFFormControl_GetRotation(this.Handle) / 90 % 4;
        if (rotation < 0)
          rotation = 4 + rotation;
        return (PageRotate) rotation;
      }
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().Rotation : PageRotate.Normal;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.Rotation = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>Gets the color type of the control's border.</summary>
  public ColorTypes BorderColorType
  {
    get
    {
      ColorTypes iColorType = ColorTypes.Unsupported;
      if (this.Handle != IntPtr.Zero)
        Pdfium.FPDFFormControl_GetBorderColor(this.Handle, out iColorType);
      else if (this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>())
      {
        PdfTypeDictionary pdfTypeDictionary = this.Dictionary["MK"].As<PdfTypeDictionary>();
        if (pdfTypeDictionary.ContainsKey("BC") && pdfTypeDictionary["BC"].Is<PdfTypeArray>())
        {
          switch (pdfTypeDictionary["BC"].As<PdfTypeArray>().Count)
          {
            case 1:
              iColorType = ColorTypes.DeviceGray;
              break;
            case 3:
              iColorType = ColorTypes.DeviceRGB;
              break;
            case 4:
              iColorType = ColorTypes.DeviceCMYK;
              break;
          }
        }
      }
      return iColorType;
    }
  }

  /// <summary>Gets or sets the color of the control's border.</summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public FS_COLOR BorderColor
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return new FS_COLOR(Pdfium.FPDFFormControl_GetBorderColor(this.Handle, out ColorTypes _));
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().BorderColor : new FS_COLOR(0);
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.BorderColor = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>
  /// Gets the color of the control's border as an array of color components.
  /// </summary>
  public float[] OriginalBorderColor
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetOriginalBorderColorEx(this.Handle, out ColorTypes _);
      if (this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>())
      {
        PdfTypeDictionary pdfTypeDictionary = this.Dictionary["MK"].As<PdfTypeDictionary>();
        if (pdfTypeDictionary.ContainsKey("BC") && pdfTypeDictionary["BC"].Is<PdfTypeArray>())
        {
          PdfTypeArray pdfTypeArray = pdfTypeDictionary["BC"].As<PdfTypeArray>();
          float[] originalBorderColor = new float[pdfTypeArray.Count];
          for (int index = 0; index < originalBorderColor.Length; ++index)
          {
            if (pdfTypeArray[index].Is<PdfTypeNumber>())
              originalBorderColor[index] = pdfTypeArray[index].As<PdfTypeNumber>().FloatValue;
          }
          return originalBorderColor;
        }
      }
      return new float[4];
    }
  }

  /// <summary>Gets the color type of the control's background.</summary>
  public ColorTypes BackgroundColorType
  {
    get
    {
      ColorTypes iColorType = ColorTypes.Unsupported;
      if (this.Handle != IntPtr.Zero)
        Pdfium.FPDFFormControl_GetBackgroundColor(this.Handle, out iColorType);
      else if (this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>())
      {
        PdfTypeDictionary pdfTypeDictionary = this.Dictionary["MK"].As<PdfTypeDictionary>();
        if (pdfTypeDictionary.ContainsKey("BG") && pdfTypeDictionary["BG"].Is<PdfTypeArray>())
        {
          switch (pdfTypeDictionary["BG"].As<PdfTypeArray>().Count)
          {
            case 1:
              iColorType = ColorTypes.DeviceGray;
              break;
            case 3:
              iColorType = ColorTypes.DeviceRGB;
              break;
            case 4:
              iColorType = ColorTypes.DeviceCMYK;
              break;
          }
        }
      }
      return iColorType;
    }
  }

  /// <summary>Gets or sets the color of the control's background.</summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public FS_COLOR BackgroundColor
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return new FS_COLOR(Pdfium.FPDFFormControl_GetBackgroundColor(this.Handle, out ColorTypes _));
      return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() ? this.GetMK().BackgroundColor : new FS_COLOR(0);
    }
    set
    {
      if (this.Dictionary == null)
        return;
      PdfMK mk = this.GetMK();
      mk.BackgroundColor = value;
      this.PostProcessMK(mk);
    }
  }

  /// <summary>
  /// Gets the color of the control's background as an array of color components.
  /// </summary>
  public float[] OriginalBackgroundColor
  {
    get
    {
      if (this.Handle != IntPtr.Zero)
        return Pdfium.FPDFFormControl_GetOriginalBackgroundColorEx(this.Handle, out ColorTypes _);
      if (this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>())
      {
        PdfTypeDictionary pdfTypeDictionary = this.Dictionary["MK"].As<PdfTypeDictionary>();
        if (pdfTypeDictionary.ContainsKey("BG") && pdfTypeDictionary["BG"].Is<PdfTypeArray>())
        {
          PdfTypeArray pdfTypeArray = pdfTypeDictionary["BG"].As<PdfTypeArray>();
          float[] originalBackgroundColor = new float[pdfTypeArray.Count];
          for (int index = 0; index < originalBackgroundColor.Length; ++index)
          {
            if (pdfTypeArray[index].Is<PdfTypeNumber>())
              originalBackgroundColor[index] = pdfTypeArray[index].As<PdfTypeNumber>().FloatValue;
          }
          return originalBackgroundColor;
        }
      }
      return new float[4];
    }
  }

  /// <summary>Gets or sets the fore color of the control.</summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public FS_COLOR Color
  {
    get
    {
      if (this.Dictionary == null)
        return FS_COLOR.Black;
      float[] fillColor;
      Pdfium.FPDFTOOLS_ParseDefaultAppearance(this.Field.GetDefaultAppearance(this.Dictionary, checkAcroFormsDictionary: false), out float[] _, out fillColor, out string _, out float _, out FS_MATRIX _);
      return fillColor != null ? new FS_COLOR(fillColor) : FS_COLOR.Black;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      this.Field.SetDefaultAppearance(this.Dictionary, (PdfFont) null, 0.0f, new FS_COLOR?(value), false);
    }
  }

  /// <summary>
  /// Gets or sets a border style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width, style and dash pattern to be used in drawing.
  /// </summary>
  /// <value>
  /// After changing this property, you must call the <see cref="M:Patagames.Pdf.Net.PdfControl.ResetAppearance" /> method for the changes to take effect.
  /// </value>
  public PdfBorderStyle BorderStyle
  {
    get
    {
      if (this.Dictionary == null || !this.Dictionary.ContainsKey("BS") || !this.Dictionary["BS"].Is<PdfTypeDictionary>())
        return (PdfBorderStyle) null;
      if ((PdfWrapper) this._borderStyle == (PdfWrapper) null || this._borderStyle.Dictionary.IsDisposed)
        this._borderStyle = new PdfBorderStyle(this.Dictionary["BS"]);
      return this._borderStyle;
    }
    set
    {
      if (this.Dictionary == null)
        return;
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("BS"))
        this.Dictionary.Remove("BS");
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary["BS"] = (PdfTypeBase) value.Dictionary;
      this._borderStyle = value;
    }
  }

  /// <summary>Gets a dictionary of widget annotation.</summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      if (this.Handle == IntPtr.Zero && this._dictionary != null)
        return this._dictionary;
      if (this.Handle == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary != null && !this._dictionary.IsDisposed)
        return this._dictionary;
      IntPtr widget = Pdfium.FPDFFormControl_GetWidget(this.Handle);
      if (widget == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      this._dictionary = new PdfTypeDictionary(widget);
      return this._dictionary;
    }
  }

  /// <summary>Initializes a new instance of the PdfControl class.</summary>
  /// <param name="forms">Interactive forms</param>
  /// <param name="handle">Handle ti the control wich will be assigned</param>
  internal PdfControl(PdfInteractiveForms forms, IntPtr handle)
  {
    this.Handle = handle;
    this._forms = forms;
    this._tmpFieldDoNotUseItAnywhereExceptInsideFieldProperty = (PdfField) null;
  }

  /// <summary>
  /// Create a new control and assign it to the specified <paramref name="field" />.
  /// </summary>
  /// <param name="field">The field to which this control is assigned./&gt;</param>
  /// <param name="page">The page that will host this control.</param>
  /// <param name="rect">The area on the <paramref name="page" /> where this control should be placed.</param>
  /// <param name="borderColor">The color of the control’s border.</param>
  /// <param name="backgroundColor">The color of the control’s background.</param>
  /// <param name="highlightingMode">The control’s highlighting mode, the visual effect to be used when the mouse button is pressed or held down inside its active area.</param>
  /// <param name="rotation">A value indicating how the pushbutton control is rotated counterclockwise relative to the <paramref name="page" />.</param>
  /// <param name="borderWidth">The border width in points.</param>
  /// <param name="borderStyle">The border style.</param>
  /// <param name="dashPattern">An array defining a pattern of dashes and gaps to be used in drawing a dashed border. The dash array is specified in the same format as in the line dash pattern parameter of the graphics state. The dash phase is not specified and is assumed to be 0. For example, a DashPattern property of[3 2] specifies a border drawn with 3-point dashes alternating with 2-point gaps.</param>
  protected PdfControl(
    PdfField field,
    PdfPage page,
    FS_RECTF rect,
    FS_COLOR borderColor,
    FS_COLOR backgroundColor,
    FormHighlightingMode highlightingMode,
    PageRotate rotation,
    float borderWidth,
    BorderStyles borderStyle,
    float[] dashPattern)
  {
    this._tmpFieldDoNotUseItAnywhereExceptInsideFieldProperty = field != null && page != null ? field : throw new ArgumentNullException(field == null ? nameof (field) : nameof (page));
    if (field.Dictionary.ContainsKey("Subtype") && field.Dictionary["Subtype"].Is<PdfTypeName>() && field.Dictionary["Subtype"].As<PdfTypeName>().Value == "Widget")
      throw new ArgumentException(Error.err0069);
    if (field.Dictionary.ContainsKey("Kids"))
    {
      PdfTypeArray pdfTypeArray = field.Dictionary["Kids"].As<PdfTypeArray>();
      if (pdfTypeArray.Count > 0)
      {
        PdfTypeDictionary pdfTypeDictionary = pdfTypeArray[0].As<PdfTypeDictionary>();
        if (!pdfTypeDictionary.ContainsKey("Subtype") || !pdfTypeDictionary["Subtype"].Is<PdfTypeName>() || pdfTypeDictionary["Subtype"].As<PdfTypeName>().Value != "Widget")
          throw new ArgumentException(Error.err0070);
      }
    }
    IntPtr document = Pdfium.FPDFInterForm_GetDocument(field.InterForms.Handle);
    IntPtr Handle = !(document == IntPtr.Zero) ? Pdfium.FPDFHOLDER_FromPdfDocument(document) : throw Pdfium.ProcessLastError();
    PdfIndirectList list = !(Handle == IntPtr.Zero) ? new PdfIndirectList(Handle) : throw Pdfium.ProcessLastError();
    this._widget = new PdfWidgetAnnotation(page);
    this._widget.Flags = AnnotationFlags.Print;
    list.Add((PdfTypeBase) this._widget.Dictionary);
    if (page.Annots == null)
      page.CreateAnnotations();
    page.Annots.Add((PdfAnnotation) this._widget);
    if (!field.Dictionary.ContainsKey("Kids"))
      field.Dictionary["Kids"] = (PdfTypeBase) PdfTypeArray.Create();
    field.Dictionary["Kids"].As<PdfTypeArray>().AddIndirect(list, (PdfTypeBase) this._widget.Dictionary);
    this._widget.Dictionary.SetIndirectAt("Parent", list, (PdfTypeBase) field.Dictionary);
    this._widget.Rectangle = rect;
    PdfMK pdfMk = new PdfMK(page);
    pdfMk.BackgroundColor = backgroundColor;
    pdfMk.BorderColor = borderColor;
    pdfMk.Rotation = rotation;
    this._widget.MK = pdfMk.Dictionary.Keys.Count <= 0 ? (PdfMK) null : pdfMk;
    this._borderStyle = new PdfBorderStyle();
    this._borderStyle.DashPattern = dashPattern;
    this._borderStyle.Style = borderStyle;
    this._borderStyle.Width = borderWidth;
    if (this._borderStyle.Dictionary.Keys.Count > 1)
      this._widget.BorderStyle = this._borderStyle;
    this._widget.HighlightingMode = highlightingMode;
    this._dictionary = this._widget.Dictionary;
    this._forms = field.InterForms;
    if (field.Handle != IntPtr.Zero)
    {
      Pdfium.FPDFInterForm_LoadField(this._forms.Handle, field.Dictionary.Handle);
      this.AfterLoadField();
    }
    else
      field.Controls.AddNewItem(this);
  }

  internal static PdfControl Create(PdfInteractiveForms forms, IntPtr handle)
  {
    switch (Pdfium.FPDFFormControl_GetType(handle))
    {
      case FormFieldTypesEx.PushButton:
        return (PdfControl) new PdfPushButtonControl(forms, handle);
      case FormFieldTypesEx.RadioButton:
        return (PdfControl) new PdfRadioButtonControl(forms, handle);
      case FormFieldTypesEx.CheckBox:
        return (PdfControl) new PdfCheckBoxControl(forms, handle);
      case FormFieldTypesEx.Text:
      case FormFieldTypesEx.RichText:
      case FormFieldTypesEx.File:
        return (PdfControl) new PdfTextBoxControl(forms, handle);
      case FormFieldTypesEx.ListBox:
        return (PdfControl) new PdfListBoxControl(forms, handle);
      case FormFieldTypesEx.ComboBox:
        return (PdfControl) new PdfComboBoxControl(forms, handle);
      case FormFieldTypesEx.Sign:
        return (PdfControl) new PdfSignatureControl(forms, handle);
      default:
        return new PdfControl(forms, handle);
    }
  }

  internal void UpdateHandle(IntPtr handle) => this.Handle = handle;

  internal void AfterLoadField()
  {
    if (this.Dictionary == null || this.Dictionary.IsDisposed)
      return;
    this.UpdateHandle(Pdfium.FPDFInterForm_GetControlByDict(this._forms.Handle, this.Dictionary.Handle));
    if (!((PdfWrapper) this._widget != (PdfWrapper) null) || this._widget.Page == null || !this._widget.Page.IsLoaded)
      return;
    this.ResetAppearance();
    this._widget.Page.ReloadPage();
  }

  internal void RemoveFromDom()
  {
    PdfTypeDictionary pageDictionary = this.GetPageDictionary(PdfRefObjectsCollection.FromPdfDocument(this._forms.FillForms.Document), (PdfTypeBase) this.Dictionary);
    if (pageDictionary != null && pageDictionary.ContainsKey("Annots") && pageDictionary["Annots"].Is<PdfTypeArray>())
    {
      PdfTypeArray pdfTypeArray = pageDictionary["Annots"].As<PdfTypeArray>();
      for (int index = 0; index < pdfTypeArray.Count; ++index)
      {
        if (Pdfium.FPDFARRAY_GetDirectObjectAt(pdfTypeArray.Handle, index) == this.Dictionary.Handle)
        {
          pdfTypeArray.RemoveAt(index);
          break;
        }
      }
    }
    this._widget = (PdfWidgetAnnotation) null;
    if (this.Field.Dictionary.Handle != this.Dictionary.Handle && this.Field.Dictionary.ContainsKey("Kids") && this.Field.Dictionary["Kids"].Is<PdfTypeArray>())
    {
      PdfTypeArray pdfTypeArray = this.Field.Dictionary["Kids"].As<PdfTypeArray>();
      for (int index = 0; index < pdfTypeArray.Count; ++index)
      {
        if (pdfTypeArray[index].Is<PdfTypeDictionary>() && !(pdfTypeArray[index].As<PdfTypeDictionary>().Handle != this.Dictionary.Handle))
        {
          pdfTypeArray.RemoveAt(index);
          break;
        }
      }
    }
    else if (this.Field.Dictionary.Handle == this.Dictionary.Handle)
    {
      string[] strArray = new string[19]
      {
        "Subtype",
        "H",
        "MK",
        "A",
        "AA",
        "BS",
        "Type",
        "Rect",
        "Contents",
        "P",
        "NM",
        "M",
        "F",
        "AP",
        "AS",
        "Border",
        "C",
        "StructParent",
        "OC"
      };
      foreach (string key in strArray)
      {
        if (this.Dictionary.ContainsKey(key))
          this.Dictionary.Remove(key);
      }
    }
    this.UpdateHandle(IntPtr.Zero);
  }

  /// <summary>
  /// Delete the existing appearance stream of this <see cref="T:Patagames.Pdf.Net.PdfControl" /> and build a new one.
  /// </summary>
  /// <param name="page">The page where the widget is located. If no page is specified, the widget is searched on all pages of the document.</param>
  public void RegenerateAppearance(PdfPage page = null)
  {
    if (page == null && (PdfWrapper) this.GetWidget(page) != (PdfWrapper) null)
      page = this.GetWidget().Page;
    if (page == null)
      return;
    this.ResetAppearance();
    IntPtr handle1 = this.Field.InterForms.FillForms.Document.Handle;
    IntPtr handle2 = page.Dictionary.Handle;
    IntPtr handle3 = this.Field.InterForms.FillForms.Handle;
    IntPtr page1 = Pdfium.FPDF_LoadPage(handle1, Pdfium.FPDF_GetPageIndexByDict(handle1, handle2));
    Pdfium.FORM_OnAfterLoadPage(page1, handle3);
    Pdfium.FORM_OnBeforeClosePage(page1, handle3);
    Pdfium.FPDF_ClosePage(page1);
  }

  /// <summary>
  /// Find the page on which the given control is located and return it in the form of the corresponding widget.
  /// </summary>
  /// <param name="page">The page where the widget is located. If no page is specified, the widget is searched on all pages of the document.</param>
  /// <returns>An instance of PdfWidgetAnnotation class that represents the found widget, or null if nothing was found.</returns>
  public PdfWidgetAnnotation GetWidget(PdfPage page = null)
  {
    if ((PdfWrapper) this._widget != (PdfWrapper) null)
      return this._widget;
    if (page != null)
    {
      this._widget = new PdfWidgetAnnotation(page, (PdfTypeBase) this.Dictionary);
      return this._widget;
    }
    using (PdfRefObjectsCollection refs = PdfRefObjectsCollection.FromPdfDocument(this._forms.FillForms.Document))
    {
      PdfTypeDictionary pageDictionary = this.GetPageDictionary(refs, (PdfTypeBase) this.Dictionary);
      if (pageDictionary == null)
        return (PdfWidgetAnnotation) null;
      int pageIndexByDict = Pdfium.FPDF_GetPageIndexByDict(this._forms.FillForms.Document.Handle, pageDictionary.Handle);
      if (pageIndexByDict < 0)
        return (PdfWidgetAnnotation) null;
      this._widget = new PdfWidgetAnnotation(this._forms.FillForms.Document.Pages[pageIndexByDict], (PdfTypeBase) this.Dictionary);
      return this._widget;
    }
  }

  /// <summary>
  /// Reset the appearance of the control and mark it to rebuild on next page load.
  /// </summary>
  public void ResetAppearance()
  {
    if (this.Dictionary == null || !this.Dictionary.ContainsKey("AP"))
      return;
    this.Dictionary.Remove("AP");
  }

  /// <summary>Gets whether the control has MK entry.</summary>
  /// <param name="entry">MK entry</param>
  /// <returns>True if entry exists, False otherwise</returns>
  /// <remarks>
  /// <para>The MK entry can be used to provide an appearance characteristics dictionary containing additional information for constructing the annotation’s appearance
  /// stream.</para>
  /// <para>
  /// MK entry can be one of the following:  "R", "BC", "BG", "CA", "RC", "AC", "I", "RI", "IX", "IF", "TP".
  /// </para>
  /// <para>
  /// See detils in a table 8.40 at <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF reference 1.7.pdf</a>
  /// </para>
  /// </remarks>
  public bool HasMKEntry(string entry)
  {
    if (this.Handle != IntPtr.Zero)
      return Pdfium.FPDFFormControl_HasMKEntry(this.Handle, entry);
    return this.Dictionary != null && this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>() && this.Dictionary["MK"].As<PdfTypeDictionary>().ContainsKey(entry);
  }

  /// <summary>Set the input focus to the specified control.</summary>
  /// <param name="page">The <see cref="T:Patagames.Pdf.Net.PdfPage" />.</param>
  public void SetFocus(PdfPage page)
  {
    Pdfium.FORM_SetFocusAnnot(this._forms._fillforms.Handle, page.Handle, this.Dictionary.Handle);
  }

  internal PdfMK GetMK()
  {
    PdfPage page = new PdfPage(this.Field.InterForms.FillForms.Document, -1);
    return !this.Dictionary.ContainsKey("MK") || !this.Dictionary["MK"].Is<PdfTypeDictionary>() ? new PdfMK(page) : new PdfMK(page, (PdfTypeBase) this.Dictionary["MK"].As<PdfTypeDictionary>());
  }

  internal void PostProcessMK(PdfMK mk)
  {
    if (mk.Dictionary.Count == 0 && this.Dictionary.ContainsKey("MK"))
    {
      this.Dictionary.Remove("MK");
    }
    else
    {
      if (mk.Dictionary.Count <= 0 || this.Dictionary.ContainsKey("MK") && this.Dictionary["MK"].Is<PdfTypeDictionary>())
        return;
      this.Dictionary["MK"] = (PdfTypeBase) mk.Dictionary;
    }
  }

  private PdfTypeDictionary GetPageDictionary(
    PdfRefObjectsCollection refs,
    PdfTypeBase obj,
    System.Collections.Generic.Dictionary<IntPtr, byte> ch = null)
  {
    if (obj == null)
      return (PdfTypeDictionary) null;
    if (ch != null && ch.ContainsKey(obj.Handle))
      return (PdfTypeDictionary) null;
    if (ch != null && obj.Is<PdfTypeDictionary>())
    {
      PdfTypeDictionary pdfTypeDictionary = obj.As<PdfTypeDictionary>();
      return pdfTypeDictionary.ContainsKey("Type") && pdfTypeDictionary["Type"].Is<PdfTypeName>() && pdfTypeDictionary["Type"].As<PdfTypeName>().Value == "Page" ? pdfTypeDictionary : (PdfTypeDictionary) null;
    }
    if (ch == null)
      ch = new System.Collections.Generic.Dictionary<IntPtr, byte>();
    ch.Add(obj.Handle, (byte) 1);
    if (obj.ObjectNumber == 0)
    {
      PdfTypeDictionary pageDictionary = this.GetPageDictionary(refs, PdfTypeBase.Create(Pdfium.FPDFOBJ_GetParentObj(obj.Handle)), ch);
      if (pageDictionary != null)
        return pageDictionary;
    }
    else
    {
      foreach (PdfTypeBase pdfTypeBase in refs.GetBy(obj.ObjectNumber).ReferredBy)
      {
        PdfTypeDictionary pageDictionary = this.GetPageDictionary(refs, pdfTypeBase, ch);
        if (pageDictionary != null)
          return pageDictionary;
      }
    }
    return (PdfTypeDictionary) null;
  }
}
