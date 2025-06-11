// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfStyledField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfStyledField : PdfField
{
  private const byte ShadowShift = 64 /*0x40*/;
  internal int m_angle;
  internal WidgetAnnotation m_widget;
  private PdfFont m_font;
  private PdfFieldActions m_actions;
  private PdfTemplate m_appearanceTemplate;
  private PdfBrush m_backBrush;
  private PdfColor m_backRectColor = PdfColor.Empty;
  private PdfBrush m_foreBrush;
  private PdfPen m_borderPen;
  private PdfStringFormat m_stringFormat;
  private PdfBrush m_shadowBrush;
  private bool m_visible = true;
  private PdfFormFieldVisibility m_visibility;
  internal PdfArray m_array = new PdfArray();
  internal List<PdfField> fieldItems = new List<PdfField>();
  internal bool m_isBCSet;

  public PdfStyledField(PdfPageBase page, string name)
    : base(page, name)
  {
  }

  internal PdfStyledField()
  {
  }

  public virtual RectangleF Bounds
  {
    get => this.m_widget.Bounds;
    set => this.m_widget.Bounds = value;
  }

  public PdfFormFieldVisibility Visibility
  {
    get => this.m_visibility;
    set
    {
      this.m_visibility = value;
      this.SetVisibility();
    }
  }

  public PointF Location
  {
    get => this.m_widget.Location;
    set => this.m_widget.AssignLocation(value);
  }

  public new int RotationAngle
  {
    get => this.m_widget.WidgetAppearance.RotationAngle;
    set
    {
      this.m_angle = value;
      int num = 360;
      if (this.m_angle >= 360)
        this.m_angle %= num;
      if (this.m_angle < 45)
        this.m_angle = 0;
      else if (this.m_angle >= 45 && this.m_angle < 135)
        this.m_angle = 90;
      else if (this.m_angle >= 135 && this.m_angle < 225)
        this.m_angle = 180;
      else if (this.m_angle >= 225 && this.m_angle < 315)
        this.m_angle = 270;
      this.m_widget.WidgetAppearance.RotationAngle = this.m_angle;
    }
  }

  public SizeF Size
  {
    get => this.m_widget.Size;
    set => this.m_widget.AssignSize(value);
  }

  public PdfColor BorderColor
  {
    get => this.m_widget.WidgetAppearance.BorderColor;
    set
    {
      this.m_widget.WidgetAppearance.BorderColor = value;
      this.CreateBorderPen();
    }
  }

  public PdfColor BackColor
  {
    get => this.m_widget.WidgetAppearance.BackColor;
    set
    {
      this.m_widget.WidgetAppearance.BackColor = value;
      this.CreateBackBrush();
      this.m_isBCSet = true;
    }
  }

  internal PdfColor BackRectColor
  {
    get => this.m_backRectColor;
    set => this.m_backRectColor = value;
  }

  public PdfColor ForeColor
  {
    get => this.m_widget.DefaultAppearance.ForeColor;
    set
    {
      this.m_widget.DefaultAppearance.ForeColor = value;
      this.m_foreBrush = (PdfBrush) new PdfSolidBrush(value);
    }
  }

  public float BorderWidth
  {
    get => this.m_widget.WidgetBorder.Width;
    set
    {
      if ((double) this.m_widget.WidgetBorder.Width == (double) value)
        return;
      this.m_widget.WidgetBorder.Width = value;
      if ((double) value == 0.0)
        this.m_widget.WidgetAppearance.BorderColor = new PdfColor(byte.MaxValue, byte.MaxValue, byte.MaxValue);
      else
        this.CreateBorderPen();
    }
  }

  public PdfHighlightMode HighlightMode
  {
    get => this.m_widget.HighlightMode;
    set => this.m_widget.HighlightMode = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Font));
      if (this.m_font == value)
        return;
      this.m_font = value;
      this.DefineDefaultAppearance();
    }
  }

  public PdfTextAlignment TextAlignment
  {
    get => this.Widget.TextAlignment;
    set
    {
      if (this.Widget.TextAlignment == value)
        return;
      this.Widget.TextAlignment = value;
      this.m_stringFormat = new PdfStringFormat(value, PdfVerticalAlignment.Middle);
    }
  }

  public PdfFieldActions Actions
  {
    get
    {
      if (this.m_actions == null)
      {
        this.m_actions = new PdfFieldActions(this.Widget.Actions);
        this.Dictionary.SetProperty("AA", (IPdfWrapper) this.m_actions);
      }
      return this.m_actions;
    }
  }

  public PdfBorderStyle BorderStyle
  {
    get => this.Widget.WidgetBorder.Style;
    set
    {
      this.Widget.WidgetBorder.Style = value;
      this.CreateBorderPen();
    }
  }

  public bool Visible
  {
    get => this.m_visible;
    set
    {
      if (this.m_visible == value || value)
        return;
      this.m_visible = value;
      this.m_widget.AnnotationFlags = PdfAnnotationFlags.Hidden;
    }
  }

  internal PdfBrush ShadowBrush => this.m_shadowBrush;

  internal WidgetAnnotation Widget => this.m_widget;

  internal PdfTemplate AppearanceTemplate => this.m_appearanceTemplate;

  internal PdfBrush BackBrush => this.m_backBrush;

  internal PdfPen BorderPen => this.m_borderPen;

  internal PdfBrush ForeBrush => this.m_foreBrush;

  internal PdfStringFormat StringFormat
  {
    get
    {
      if (this.m_stringFormat != null && (this.ComplexScript || this.Form != null && this.Form.ComplexScript))
        this.m_stringFormat.ComplexScript = true;
      return this.m_stringFormat;
    }
  }

  private void SetVisibility()
  {
    switch (this.m_visibility)
    {
      case PdfFormFieldVisibility.Visible:
        this.m_widget.AnnotationFlags = PdfAnnotationFlags.Print;
        break;
      case PdfFormFieldVisibility.Hidden:
        this.m_widget.AnnotationFlags = PdfAnnotationFlags.Hidden;
        break;
      case PdfFormFieldVisibility.VisibleNotPrintable:
        this.m_widget.AnnotationFlags = PdfAnnotationFlags.Default;
        break;
      case PdfFormFieldVisibility.HiddenPrintable:
        this.m_widget.AnnotationFlags = PdfAnnotationFlags.Print | PdfAnnotationFlags.NoView;
        break;
    }
  }

  internal override void Draw()
  {
    this.RemoveAnnoationFromPage(this.Page, (PdfAnnotation) this.Widget);
  }

  internal void RemoveAnnoationFromPage(PdfPageBase page, PdfAnnotation widget)
  {
    if (page is PdfPage pdfPage)
    {
      pdfPage.Annotations.Remove(widget);
    }
    else
    {
      PdfLoadedPage wrapper = page as PdfLoadedPage;
      PdfDictionary dictionary = wrapper.Dictionary;
      PdfArray primitive = !dictionary.ContainsKey("Annots") ? new PdfArray() : wrapper.CrossTable.GetObject(dictionary["Annots"]) as PdfArray;
      widget.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      primitive.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) widget));
      page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }

  internal void AddAnnotationToPage(PdfPageBase page, PdfAnnotation widget)
  {
    if (page is PdfPage pdfPage)
    {
      pdfPage.Annotations.Add(widget);
    }
    else
    {
      PdfLoadedPage wrapper = page as PdfLoadedPage;
      PdfDictionary dictionary = wrapper.Dictionary;
      PdfArray primitive = !dictionary.ContainsKey("Annots") ? new PdfArray() : wrapper.CrossTable.GetObject(dictionary["Annots"]) as PdfArray;
      widget.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) widget));
      page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }

  protected PdfFont ObtainFont() => this.m_font == null ? PdfDocument.DefaultFont : this.m_font;

  protected override void Initialize()
  {
    base.Initialize();
    this.m_widget = new WidgetAnnotation();
    this.m_widget.Parent = (PdfField) this;
    this.m_foreBrush = (PdfBrush) new PdfSolidBrush(this.m_widget.DefaultAppearance.ForeColor);
    this.m_stringFormat = new PdfStringFormat(this.Widget.TextAlignment, PdfVerticalAlignment.Middle);
    this.CreateBorderPen();
    this.CreateBackBrush();
    this.Dictionary.SetProperty("Kids", (IPdfPrimitive) new PdfArray(new PdfArray()
    {
      (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_widget)
    }));
    this.Widget.DefaultAppearance.FontName = "TiRo";
  }

  protected override void DefineDefaultAppearance()
  {
    if (this.Form != null && this.m_font != null)
    {
      this.m_widget.DefaultAppearance.FontName = this.Form.Resources.GetName((IPdfWrapper) this.m_font).Value;
      this.m_widget.DefaultAppearance.FontSize = this.m_font.Size;
    }
    else
    {
      if (this.m_font == null)
        return;
      this.Widget.DefaultAppearance.FontName = this.m_font.Name;
      this.Widget.DefaultAppearance.FontSize = this.m_font.Size;
    }
  }

  private void CreateBorderPen()
  {
    float width = this.m_widget.WidgetBorder.Width;
    this.m_borderPen = new PdfPen(this.m_widget.WidgetAppearance.BorderColor, width);
    if (this.Widget.WidgetBorder.Style != PdfBorderStyle.Dashed)
      return;
    this.m_borderPen.DashStyle = PdfDashStyle.Custom;
    this.m_borderPen.DashPattern = new float[1]
    {
      3f / width
    };
  }

  private void CreateBackBrush()
  {
    this.m_backBrush = (PdfBrush) new PdfSolidBrush(this.m_widget.WidgetAppearance.BackColor);
    PdfColor color = new PdfColor(this.m_widget.WidgetAppearance.BackColor);
    color.R = (int) color.R - 64 /*0x40*/ >= 0 ? (byte) ((int) color.R - 64 /*0x40*/) : (byte) 0;
    color.G = (int) color.G - 64 /*0x40*/ >= 0 ? (byte) ((int) color.G - 64 /*0x40*/) : (byte) 0;
    color.B = (int) color.B - 64 /*0x40*/ >= 0 ? (byte) ((int) color.B - 64 /*0x40*/) : (byte) 0;
    this.m_shadowBrush = (PdfBrush) new PdfSolidBrush(color);
  }
}
