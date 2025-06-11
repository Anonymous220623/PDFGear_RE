// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Annotations.PdfWidgetAnnotation
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Annotations;

/// <summary>Represents Widget Annotation</summary>
/// <remarks>
/// Interactive forms use widget annotations to represent the appearance of fields and to manage user interactions.
/// <note type="note">This annotation is currently not supported by the SDK.</note>
/// </remarks>
public class PdfWidgetAnnotation : PdfAnnotation
{
  private PdfAction _action;
  private PdfMK _mk;
  private PdfBorderStyle _borderStyle;
  private PdfAnnotationLevelActions _aactions;

  /// <summary>
  /// Gets or sets the annotation’s highlighting mode, the visual effect to be used when the mouse button is pressed or held down inside its active area.
  /// </summary>
  public FormHighlightingMode HighlightingMode
  {
    get
    {
      FormHighlightingMode result;
      return !this.IsExists("H") || !Pdfium.GetEnumDescription<FormHighlightingMode>(this.Dictionary["H"].As<PdfTypeName>().Value, out result) ? FormHighlightingMode.Invert : result;
    }
    set
    {
      string enumDescription = Pdfium.GetEnumDescription((Enum) value);
      if ((enumDescription ?? "").Trim() == "" && this.Dictionary.ContainsKey("H"))
        this.Dictionary.Remove("H");
      this.Dictionary["H"] = (PdfTypeBase) PdfTypeName.Create(enumDescription);
    }
  }

  /// <summary>
  /// Gets or sets an appearance characteristics object to be used in constructing a dynamic appearance stream
  /// specifying the annotation’s visual presentation on the page.
  /// </summary>
  public PdfMK MK
  {
    get
    {
      if (!this.IsExists(nameof (MK)))
        return (PdfMK) null;
      if ((PdfWrapper) this._mk == (PdfWrapper) null || this._mk.Dictionary.IsDisposed)
        this._mk = new PdfMK(this.Page, this.Dictionary[nameof (MK)]);
      return this._mk;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey(nameof (MK)))
        this.Dictionary.Remove(nameof (MK));
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary[nameof (MK)] = (PdfTypeBase) value.Dictionary;
      this._mk = value;
    }
  }

  /// <summary>
  /// Gets or sets the <see cref="T:Patagames.Pdf.Net.PdfAction" /> object associated with this widget annotation or null if there is no action associated with the widget.
  /// </summary>
  public PdfAction Action
  {
    get
    {
      if (!this.Dictionary.ContainsKey("A"))
      {
        this._action = (PdfAction) null;
        return (PdfAction) null;
      }
      if (this._action == null || this.Dictionary["A"].Is<PdfTypeDictionary>() && this._action.Handle != this.Dictionary["A"].As<PdfTypeDictionary>().Handle)
        this._action = PdfAction.FromHandle(this.Page.Document, this.Dictionary["A"].As<PdfTypeDictionary>().Handle);
      return this._action;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("A"))
        this.Dictionary.Remove("A");
      else if (value != null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "action", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.Page.Document);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("A", list, (PdfTypeBase) value.Dictionary);
      }
      this._action = value;
    }
  }

  /// <summary>
  /// Gets or sets the additional-actions defining actions to be performed when the page is opened or closed.
  /// </summary>
  public PdfAnnotationLevelActions AdditionalActions
  {
    get
    {
      if (!this.Dictionary.ContainsKey("AA"))
      {
        this._aactions = (PdfAnnotationLevelActions) null;
        return (PdfAnnotationLevelActions) null;
      }
      if ((PdfWrapper) this._aactions == (PdfWrapper) null || this.Dictionary["AA"].Is<PdfTypeDictionary>() && this._aactions.Dictionary.Handle != this.Dictionary["AA"].As<PdfTypeDictionary>().Handle)
        this._aactions = new PdfAnnotationLevelActions(this.Page.Document, (PdfTypeBase) this.Dictionary["AA"].As<PdfTypeDictionary>());
      return this._aactions;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("AA"))
        this.Dictionary.Remove("AA");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
          throw new ArgumentException(string.Format(Error.err0067, (object) "additional actions", (object) "object"));
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.Page.Document);
        list.Add((PdfTypeBase) value.Dictionary);
        this.Dictionary.SetIndirectAt("AA", list, (PdfTypeBase) value.Dictionary);
      }
      this._aactions = value;
    }
  }

  /// <summary>
  /// Gets or sets a border style (see <see cref="T:Patagames.Pdf.Net.Wrappers.PdfBorderStyle" />) specifying the line width and dash pattern to be used in drawing the rectangle or ellipse.
  /// </summary>
  /// <remarks>
  /// <note type="note">The annotation dictionary’s AP entry, if present, takes precedence over the  <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Rectangle" /> and <see cref="P:Patagames.Pdf.Net.Annotations.PdfWidgetAnnotation.BorderStyle" /> properties.</note>
  /// <note type="note">If the value of BorderStyle property is null, the rectanle or ellipse are drawn as a solid line with a width of 1 point.</note>
  /// </remarks>
  public PdfBorderStyle BorderStyle
  {
    get
    {
      if (!this.IsExists("BS"))
        return (PdfBorderStyle) null;
      if ((PdfWrapper) this._borderStyle == (PdfWrapper) null || this._borderStyle.Dictionary.IsDisposed)
        this._borderStyle = new PdfBorderStyle(this.Dictionary["BS"]);
      return this._borderStyle;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("BS"))
        this.Dictionary.Remove("BS");
      else if ((PdfWrapper) value != (PdfWrapper) null)
        this.Dictionary["BS"] = (PdfTypeBase) value.Dictionary;
      this._borderStyle = value;
    }
  }

  /// <summary>
  /// Creates a new <see cref="T:Patagames.Pdf.Net.Annotations.PdfWidgetAnnotation" />.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  public PdfWidgetAnnotation(PdfPage page)
    : base(page)
  {
    this.Dictionary["Subtype"] = (PdfTypeBase) PdfTypeName.Create("Widget");
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Annotations.PdfWatermarkAnnotation" /> class based on the specified dictionary.
  /// </summary>
  /// <param name="page">The PDF page associated with annotation.</param>
  /// <param name="dictionary">The annotation dictionary or indirect dictionary.</param>
  public PdfWidgetAnnotation(PdfPage page, PdfTypeBase dictionary)
    : base(page, dictionary)
  {
  }
}
