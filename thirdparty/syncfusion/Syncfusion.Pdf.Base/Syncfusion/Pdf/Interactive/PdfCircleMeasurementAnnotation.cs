// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfCircleMeasurementAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfCircleMeasurementAnnotation(RectangleF rectangle) : PdfAnnotation(rectangle)
{
  private LineBorder m_border = new LineBorder();
  private PdfMeasurementUnit m_measurementUnit;
  private PdfCircleMeasurementType m_type;
  private PdfFont m_font;
  private string m_unitString;
  private float m_borderWidth;

  public LineBorder Border
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  public PdfMeasurementUnit Unit
  {
    get => this.m_measurementUnit;
    set => this.m_measurementUnit = value;
  }

  public PdfCircleMeasurementType MeasurementType
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public PdfFont Font
  {
    get
    {
      if (this.m_font == null)
        this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 8f, PdfFontStyle.Regular);
      return this.m_font;
    }
    set => this.m_font = value != null ? value : throw new ArgumentNullException(nameof (Font));
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Circle"));
  }

  private float ConvertToUnit()
  {
    float unit = new PdfUnitConvertor().ConvertUnits(this.Bounds.Width / 2f, PdfGraphicsUnit.Point, this.GetEqualPdfGraphicsUnit(this.Unit, out this.m_unitString));
    if (this.MeasurementType == PdfCircleMeasurementType.Diameter)
      unit = 2f * unit;
    return unit;
  }

  protected override void Save()
  {
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    if (this.Dictionary.Items.ContainsKey(new PdfName("AP")))
      return;
    float unit = this.ConvertToUnit();
    SizeF sizeF = this.Font.MeasureString($"{unit.ToString("0.00")} {this.m_unitString}");
    PdfSection pdfSection = (PdfSection) null;
    if (this.Page != null)
      pdfSection = this.Page.Section;
    PdfPen pen = new PdfPen(this.Color, this.m_borderWidth);
    PdfBrush brush1 = (PdfBrush) null;
    if (this.InnerColor.A != (byte) 0)
      brush1 = (PdfBrush) new PdfSolidBrush(this.InnerColor);
    PdfBrush brush2 = (PdfBrush) new PdfSolidBrush(this.Color);
    RectangleF rect = new RectangleF(this.Bounds.X, this.Bounds.Bottom, this.Bounds.Width, this.Bounds.Height);
    if (this.Page != null)
      rect.Location = pdfSection.PointToNativePdf(this.Page, this.Bounds.Location);
    if (!this.Flatten)
    {
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
      if (this.Dictionary["AP"] != null)
      {
        rect.Y -= rect.Height;
        this.Appearance.Normal = new PdfTemplate(rect);
        PdfTemplate normal = this.Appearance.Normal;
        PaintParams paintParams = new PaintParams();
        normal.m_writeTransformation = false;
        PdfGraphics graphics = this.Appearance.Normal.Graphics;
        float num1 = (float) this.Border.BorderWidth / 2f;
        paintParams.BorderPen = pen;
        paintParams.BackBrush = brush1;
        paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
        RectangleF rectangleF = new RectangleF(rect.X, -rect.Y - rect.Height, rect.Width, rect.Height);
        paintParams.Bounds = new RectangleF(rect.X, -rect.Y, rect.Width, -rect.Height);
        rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
        graphics.Save();
        if ((double) this.Opacity < 1.0)
          graphics.SetTransparency(this.Opacity);
        FieldPainter.DrawEllipseAnnotation(graphics, paintParams, rectangleF.X + num1, rectangleF.Y + num1, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
        if (this.MeasurementType == PdfCircleMeasurementType.Diameter)
        {
          graphics.Save();
          graphics.TranslateTransform(rect.X, -rect.Y);
          float offsetX = (float) ((double) rect.Width / 2.0 - (double) sizeF.Width / 2.0);
          graphics.DrawLine(paintParams.BorderPen, new PointF(0.0f, (float) (-(double) rect.Height / 2.0)), new PointF(rect.Right, (float) (-(double) rect.Height / 2.0)));
          graphics.TranslateTransform(offsetX, (float) -((double) rect.Height / 2.0) - this.Font.Height);
          graphics.DrawString($"{unit.ToString("0.00")} {this.m_unitString}", this.Font, paintParams.ForeBrush, 0.0f, 0.0f);
          graphics.Restore();
        }
        else
        {
          graphics.Save();
          graphics.TranslateTransform(rect.X, -rect.Y);
          float offsetX = (float) ((double) rect.Width / 2.0 + ((double) rect.Width / 4.0 - (double) sizeF.Width / 2.0));
          double num2 = (double) rect.Height / 2.0;
          double num3 = (double) sizeF.Height / 2.0;
          graphics.DrawLine(paintParams.BorderPen, new PointF(rect.Width / 2f, (float) (-(double) rect.Height / 2.0)), new PointF(rect.Right, (float) (-(double) rect.Height / 2.0)));
          graphics.TranslateTransform(offsetX, (float) -((double) rect.Height / 2.0) - this.Font.Height);
          graphics.DrawString($"{unit.ToString("0.00")} {this.m_unitString}", this.Font, paintParams.ForeBrush, 0.0f, 0.0f);
          graphics.Restore();
        }
        graphics.Restore();
      }
      base.Save();
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
      if (!this.Dictionary.Items.ContainsKey(new PdfName("Measure")))
        this.Dictionary.Items.Add(new PdfName("Measure"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.CreateMeasureDictioanry(this.m_unitString)));
      this.Dictionary.SetProperty(new PdfName("Contents"), (IPdfPrimitive) new PdfString($"{this.Text}\n{unit.ToString("0.00")} {this.m_unitString}"));
      this.Dictionary.SetProperty("DS", (IPdfPrimitive) new PdfString($"font:{this.Font.Name} {this.Font.Size}pt; color:{this.ColorToHex((System.Drawing.Color) this.Color)}"));
    }
    else
    {
      if (this.Page != null)
      {
        PdfGraphicsState state = this.Page.Graphics.Save();
        this.Page.Graphics.SetTransparency(this.Opacity);
        this.Page.Graphics.DrawEllipse(pen, brush1, this.Bounds);
        PointF pointF = new PointF(this.Bounds.X + this.Bounds.Width / 2f, this.Bounds.Y + this.Bounds.Height / 2f);
        if (this.MeasurementType == PdfCircleMeasurementType.Radius)
        {
          this.Page.Graphics.DrawLine(pen, new PointF(pointF.X, pointF.Y), new PointF(this.Bounds.Right, pointF.Y));
          pointF = new PointF(pointF.X + this.Bounds.Width / 4f, pointF.Y);
        }
        else
          this.Page.Graphics.DrawLine(pen, new PointF(this.Bounds.X, pointF.Y), new PointF(this.Bounds.Right, pointF.Y));
        this.Page.Graphics.DrawString($"{unit.ToString("0.00")} {this.m_unitString}", this.Font, brush2, new PointF(pointF.X - sizeF.Width / 2f, pointF.Y - sizeF.Height));
        this.Page.Graphics.Restore(state);
      }
      else if (this.LoadedPage != null)
      {
        PdfGraphicsState state = this.LoadedPage.Graphics.Save();
        this.LoadedPage.Graphics.SetTransparency(this.Opacity);
        this.LoadedPage.Graphics.DrawEllipse(pen, brush1, this.Bounds);
        PointF pointF = new PointF(this.Bounds.X + this.Bounds.Width / 2f, this.Bounds.Y + this.Bounds.Height / 2f);
        if (this.MeasurementType == PdfCircleMeasurementType.Radius)
        {
          this.LoadedPage.Graphics.DrawLine(pen, new PointF(pointF.X, pointF.Y), new PointF(this.Bounds.Right, pointF.Y));
          pointF = new PointF(pointF.X + this.Bounds.Width / 4f, pointF.Y);
        }
        else
          this.LoadedPage.Graphics.DrawLine(pen, new PointF(this.Bounds.X, pointF.Y), new PointF(this.Bounds.Right, pointF.Y));
        this.LoadedPage.Graphics.DrawString($"{unit.ToString("0.00")} {this.m_unitString}", this.Font, brush2, new PointF(pointF.X - sizeF.Width / 2f, pointF.Y - sizeF.Height));
        this.LoadedPage.Graphics.Restore(state);
      }
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
  }
}
