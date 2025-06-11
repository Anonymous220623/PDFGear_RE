// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSquareMeasurementAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfSquareMeasurementAnnotation(RectangleF rectangle) : PdfAnnotation(rectangle)
{
  private LineBorder m_border = new LineBorder();
  private string m_unitString;
  private PdfMeasurementUnit m_measurementUnit;
  private PdfFont m_font;
  private float m_borderWidth;

  public PdfMeasurementUnit Unit
  {
    get => this.m_measurementUnit;
    set => this.m_measurementUnit = value;
  }

  public LineBorder Border
  {
    get => this.m_border;
    set => this.m_border = value;
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
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Square"));
  }

  private float CalculateAreaOfSquare()
  {
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    float areaOfSquare;
    if ((double) this.Bounds.Width == (double) this.Bounds.Height)
    {
      float num = pdfUnitConvertor.ConvertUnits(this.Bounds.Width, PdfGraphicsUnit.Point, this.GetEqualPdfGraphicsUnit(this.Unit, out this.m_unitString));
      areaOfSquare = num * num;
    }
    else
      areaOfSquare = pdfUnitConvertor.ConvertUnits(this.Bounds.Width, PdfGraphicsUnit.Point, this.GetEqualPdfGraphicsUnit(this.Unit, out this.m_unitString)) * pdfUnitConvertor.ConvertUnits(this.Bounds.Height, PdfGraphicsUnit.Point, this.GetEqualPdfGraphicsUnit(this.Unit, out this.m_unitString));
    return areaOfSquare;
  }

  protected override void Save()
  {
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    if (this.Dictionary.Items.ContainsKey(new PdfName("AP")))
      return;
    float areaOfSquare = this.CalculateAreaOfSquare();
    SizeF sizeF = this.Font.MeasureString($"{areaOfSquare.ToString("0.00")} sq {this.m_unitString}");
    PdfPen pen = new PdfPen(this.Color, this.m_borderWidth);
    if (this.Dictionary.Items.ContainsKey(new PdfName("AP")))
      return;
    PdfBrush brush1 = (PdfBrush) null;
    if (this.InnerColor.A != (byte) 0)
      brush1 = (PdfBrush) new PdfSolidBrush(this.InnerColor);
    PdfBrush brush2 = (PdfBrush) new PdfSolidBrush(this.Color);
    if (this.Page != null)
    {
      PdfSection section = this.Page.Section;
    }
    RectangleF rect = new RectangleF(this.Bounds.X, this.Bounds.Bottom, this.Bounds.Width, this.Bounds.Height);
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
        float num1 = this.m_borderWidth / 2f;
        paintParams.BorderPen = pen;
        paintParams.BackBrush = brush1;
        paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
        RectangleF rectangleF = new RectangleF(rect.X, -rect.Y - rect.Height, rect.Width, rect.Height);
        paintParams.Bounds = new RectangleF(rect.X, -rect.Y, rect.Width, -rect.Height);
        rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
        if ((double) this.Opacity < 1.0)
        {
          PdfGraphicsState state = graphics.Save();
          graphics.SetTransparency(this.Opacity);
          FieldPainter.DrawRectangleAnnotation(graphics, paintParams, rectangleF.X + num1, rectangleF.Y + num1, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
          graphics.TranslateTransform(rect.X, -rect.Y);
          float offsetX = (float) ((double) rect.Width / 2.0 - (double) sizeF.Width / 2.0);
          float num2 = (float) ((double) rect.Height / 2.0 - (double) sizeF.Height / 2.0);
          graphics.TranslateTransform(offsetX, -num2 - this.Font.Height);
          graphics.DrawString($"{areaOfSquare.ToString("0.00")} sq {this.m_unitString}", this.Font, paintParams.ForeBrush, 0.0f, 0.0f);
          graphics.Restore(state);
        }
        else
        {
          FieldPainter.DrawRectangleAnnotation(graphics, paintParams, rectangleF.X + num1, rectangleF.Y + num1, rectangleF.Width - this.m_borderWidth, rectangleF.Height - this.m_borderWidth);
          graphics.Save();
          graphics.TranslateTransform(rect.X, -rect.Y);
          float offsetX = (float) ((double) rect.Width / 2.0 - (double) sizeF.Width / 2.0);
          float num3 = (float) ((double) rect.Height / 2.0 - (double) sizeF.Height / 2.0);
          graphics.TranslateTransform(offsetX, -num3 - this.Font.Height);
          graphics.DrawString($"{areaOfSquare.ToString("0.00")} sq {this.m_unitString}", this.Font, paintParams.ForeBrush, 0.0f, 0.0f);
          graphics.Restore();
        }
      }
      base.Save();
      if (!this.Dictionary.Items.ContainsKey(new PdfName("Measure")))
        this.Dictionary.Items.Add(new PdfName("Measure"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.CreateMeasureDictioanry(this.m_unitString)));
      this.Dictionary.SetProperty("DS", (IPdfPrimitive) new PdfString($"font:{this.Font.Name} {this.Font.Size}pt; color:{this.ColorToHex((System.Drawing.Color) this.Color)}"));
      this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
      this.Dictionary.SetProperty("Contents", (IPdfPrimitive) new PdfString($"{this.Text}\n{areaOfSquare.ToString("0.00")} sq {this.m_unitString}"));
      this.Dictionary.SetProperty("Subj", (IPdfPrimitive) new PdfString("Area Measurement"));
      this.Dictionary.SetProperty("MeasurementTypes", (IPdfPrimitive) new PdfNumber(129));
      this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Polygon"));
      this.Dictionary.SetProperty("IT", (IPdfPrimitive) new PdfName("PolygonDimension"));
      PdfArray pdfArray = this.Dictionary["Rect"] as PdfArray;
      float[] numArray = new float[pdfArray.Elements.Count];
      int index = 0;
      foreach (PdfNumber pdfNumber in pdfArray)
      {
        numArray[index] = pdfNumber.FloatValue;
        ++index;
      }
      float[] array = new float[numArray.Length * 2];
      array[0] = numArray[0];
      array[1] = numArray[3];
      array[2] = numArray[0];
      array[3] = numArray[1];
      array[4] = numArray[2];
      array[5] = numArray[1];
      array[6] = numArray[2];
      array[7] = numArray[3];
      this.Dictionary.SetProperty("Vertices", (IPdfPrimitive) new PdfArray(array));
    }
    else
    {
      if (this.Page != null)
      {
        PdfGraphicsState state = this.Page.Graphics.Save();
        this.Page.Graphics.SetTransparency(this.Opacity);
        this.Page.Graphics.DrawRectangle(pen, brush1, this.Bounds);
        PointF pointF = new PointF(this.Bounds.X + this.Bounds.Width / 2f, this.Bounds.Y + this.Bounds.Height / 2f);
        this.Page.Graphics.DrawString($"{areaOfSquare.ToString("0.00")} sq {this.m_unitString}", this.Font, brush2, new PointF(pointF.X - sizeF.Width / 2f, pointF.Y - sizeF.Height / 2f));
        this.Page.Graphics.Restore(state);
      }
      else if (this.LoadedPage != null)
      {
        PdfGraphicsState state = this.LoadedPage.Graphics.Save();
        this.LoadedPage.Graphics.SetTransparency(this.Opacity);
        this.LoadedPage.Graphics.DrawRectangle(pen, brush1, this.Bounds);
        PointF pointF = new PointF(this.Bounds.X + this.Bounds.Width / 2f, this.Bounds.Y + this.Bounds.Height / 2f);
        this.LoadedPage.Graphics.DrawString($"{areaOfSquare.ToString("0.00")} sq {this.m_unitString}", this.Font, brush2, new PointF(pointF.X - sizeF.Width / 2f, pointF.Y - sizeF.Height / 2f));
        this.LoadedPage.Graphics.Restore(state);
      }
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
  }
}
