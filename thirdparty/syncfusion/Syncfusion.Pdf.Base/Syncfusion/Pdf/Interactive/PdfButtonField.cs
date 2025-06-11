// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfButtonField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfButtonField : PdfAppearanceField
{
  private string m_text = string.Empty;

  public PdfButtonField(PdfPageBase page, string name)
    : base(page, name)
  {
    this.StringFormat.Alignment = PdfTextAlignment.Center;
    this.Widget.WidgetAppearance.NormalCaption = name;
    this.Widget.TextAlignment = PdfTextAlignment.Center;
  }

  internal PdfButtonField()
  {
  }

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  public string Text
  {
    get => this.m_text;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Text));
      if (!(this.m_text != value))
        return;
      this.m_text = value;
      this.Widget.WidgetAppearance.NormalCaption = this.m_text;
    }
  }

  public void AddPrintAction()
  {
    PdfDictionary primitive = new PdfDictionary();
    primitive.SetProperty("N", (IPdfPrimitive) new PdfName("Print"));
    primitive.SetProperty("S", (IPdfPrimitive) new PdfName("Named"));
    (((this.Dictionary["Kids"] as PdfArray)[0] as PdfReferenceHolder).Object as PdfDictionary).SetProperty("A", (IPdfPrimitive) primitive);
  }

  internal override void Draw()
  {
    base.Draw();
    if (this.Widget.ObtainAppearance() != null)
    {
      this.Page.Graphics.DrawPdfTemplate(this.Appearance.Normal, this.Location);
    }
    else
    {
      RectangleF bounds = this.Bounds with
      {
        Location = PointF.Empty
      };
      PdfFont font = this.Font ?? PdfDocument.DefaultFont;
      PaintParams paintParams = new PaintParams(bounds, this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
      PdfTemplate template = new PdfTemplate(bounds.Size);
      FieldPainter.DrawButton(template.Graphics, paintParams, this.Text, font, this.StringFormat);
      this.Page.Graphics.DrawPdfTemplate(template, this.Bounds.Location, bounds.Size);
    }
  }

  internal override void Save()
  {
    base.Save();
    if (this.Form == null || this.Form.NeedAppearances || this.Widget.Appearance.GetPressedTemplate() != null)
      return;
    this.DrawPressedAppearance(this.Widget.Appearance.Pressed);
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Btn"));
    this.BackColor = new PdfColor(byte.MaxValue, (byte) 211, (byte) 211, (byte) 211);
    this.Flags |= FieldFlags.PushButton;
  }

  protected override void DrawAppearance(PdfTemplate template)
  {
    base.DrawAppearance(template);
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    FieldPainter.DrawButton(template.Graphics, paintParams, this.Text, this.ObtainFont(), this.StringFormat);
  }

  protected void DrawPressedAppearance(PdfTemplate template)
  {
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    FieldPainter.DrawPressedButton(template.Graphics, paintParams, this.Text, this.ObtainFont(), this.StringFormat);
  }
}
