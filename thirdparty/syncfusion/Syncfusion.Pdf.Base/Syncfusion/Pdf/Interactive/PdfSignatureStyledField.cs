// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSignatureStyledField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfSignatureStyledField : PdfField
{
  private const byte ShadowShift = 64 /*0x40*/;
  internal int m_angle;
  private WidgetAnnotation m_widget;
  private PdfFieldActions m_actions;
  private PdfTemplate m_appearanceTemplate;
  private PdfBrush m_backBrush;
  private PdfPen m_borderPen;
  private PdfBrush m_shadowBrush;
  private bool m_visible = true;
  private string m_name;
  internal bool m_containsBW;
  internal bool m_containsBG;

  public PdfSignatureStyledField(PdfPageBase page, string name)
    : base(page, name)
  {
    this.m_name = name;
    this.AddAnnotationToPage(page, (PdfAnnotation) this.Widget);
  }

  internal PdfSignatureStyledField()
  {
  }

  public virtual RectangleF Bounds
  {
    get => this.m_widget.Bounds;
    set => this.m_widget.Bounds = value;
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

  public PointF Location
  {
    get => this.m_widget.Location;
    set => this.m_widget.AssignLocation(value);
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
      this.m_containsBG = true;
    }
  }

  public float BorderWidth
  {
    get => this.m_widget.WidgetBorder.Width;
    set
    {
      if ((double) this.m_widget.WidgetBorder.Width != (double) value)
      {
        this.m_widget.WidgetBorder.Width = value;
        this.CreateBorderPen();
      }
      this.m_containsBW = true;
    }
  }

  public PdfHighlightMode HighlightMode
  {
    get => this.m_widget.HighlightMode;
    set => this.m_widget.HighlightMode = value;
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
      widget.Dictionary.SetProperty("T", (IPdfPrimitive) new PdfString(this.m_name));
      pdfPage.Annotations.Add(widget);
    }
    else
    {
      PdfLoadedPage wrapper = page as PdfLoadedPage;
      PdfDictionary dictionary = wrapper.Dictionary;
      if (dictionary.ContainsKey("Annots"))
      {
        if (!(wrapper.CrossTable.GetObject(dictionary["Annots"]) is PdfArray primitive))
          primitive = new PdfArray();
      }
      else
        primitive = new PdfArray();
      widget.Dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
      if ((this as PdfSignatureField).m_fieldAutoNaming)
        widget.Dictionary.SetProperty("T", (IPdfPrimitive) new PdfString(this.m_name));
      else
        this.Dictionary.SetProperty("T", (IPdfPrimitive) new PdfString(this.m_name));
      primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) widget));
      page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    bool fieldAutoNaming = (this as PdfSignatureField).m_fieldAutoNaming;
    this.m_widget = new WidgetAnnotation();
    if (fieldAutoNaming)
    {
      this.CreateBorderPen();
      this.CreateBackBrush();
      this.Dictionary = this.m_widget.Dictionary;
      this.m_widget.m_signatureField = this as PdfSignatureField;
    }
    else
    {
      this.m_widget.Parent = (PdfField) this;
      this.Dictionary.SetProperty("Kids", (IPdfPrimitive) new PdfArray(new PdfArray()
      {
        (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_widget)
      }));
    }
    this.Widget.DefaultAppearance.FontName = "TiRo";
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
