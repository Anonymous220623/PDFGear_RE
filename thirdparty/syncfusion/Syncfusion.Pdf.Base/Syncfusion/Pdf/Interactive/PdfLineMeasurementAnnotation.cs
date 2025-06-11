// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLineMeasurementAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLineMeasurementAnnotation : PdfAnnotation
{
  private LineBorder m_lineBorder = new LineBorder();
  internal PdfArray m_linePoints;
  private int m_leaderLineExt;
  private int m_leaderLine;
  private bool m_lineCaption = true;
  private PdfLineIntent m_lineIntent;
  public PdfLineCaptionType m_captionType;
  private PdfMeasurementUnit m_measurementUnit;
  private PdfFont m_font;
  private int[] m_points;
  private string m_unitString;
  internal PdfArray m_lineStyle;
  private int m_leaderOffset;
  private PdfLineEndingStyle m_beginLine;
  private PdfLineEndingStyle m_endLine;
  private float m_borderWidth;

  public bool LineCaption
  {
    get => this.m_lineCaption;
    set => this.m_lineCaption = value;
  }

  public int LeaderLine
  {
    get => this.m_leaderLine;
    set
    {
      if (this.m_leaderLineExt == 0)
        return;
      this.m_leaderLine = value;
    }
  }

  public int LeaderLineExt
  {
    get => this.m_leaderLineExt;
    set => this.m_leaderLineExt = value;
  }

  public LineBorder lineBorder
  {
    get => this.m_lineBorder;
    set => this.m_lineBorder = value;
  }

  public PdfLineCaptionType CaptionType
  {
    get => this.m_captionType;
    set => this.m_captionType = value;
  }

  public PdfLineIntent LineIntent
  {
    get => this.m_lineIntent;
    set => this.m_lineIntent = value;
  }

  public PdfColor InnerLineColor
  {
    get => this.InnerColor;
    set => this.InnerColor = value;
  }

  public PdfFont Font
  {
    get
    {
      if (this.m_font == null)
        this.m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 6f, PdfFontStyle.Regular);
      return this.m_font;
    }
    set => this.m_font = value != null ? value : throw new ArgumentNullException(nameof (Font));
  }

  public PdfColor BackColor
  {
    get => this.Color;
    set => this.Color = value;
  }

  public int[] LinePoints
  {
    get => this.m_points;
    set
    {
      this.m_points = value;
      this.m_linePoints = new PdfArray(this.m_points);
    }
  }

  public PdfMeasurementUnit Unit
  {
    get => this.m_measurementUnit;
    set => this.m_measurementUnit = value;
  }

  public int LeaderOffset
  {
    get => this.m_leaderOffset;
    set => this.m_leaderOffset = value;
  }

  public PdfLineEndingStyle BeginLineStyle
  {
    get => this.m_beginLine;
    set
    {
      if (this.m_beginLine == value)
        return;
      this.m_beginLine = value;
    }
  }

  public PdfLineEndingStyle EndLineStyle
  {
    get => this.m_endLine;
    set
    {
      if (this.m_endLine == value)
        return;
      this.m_endLine = value;
    }
  }

  public PdfLineMeasurementAnnotation(int[] linePoints)
  {
    this.m_linePoints = linePoints.Length <= 4 ? new PdfArray(linePoints) : throw new PdfException("LineMeasurement annotation shoule not 2 points");
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Line"));
  }

  private float[] ObtainLinePoints()
  {
    float[] linePoints = (float[]) null;
    if (this.m_linePoints != null)
    {
      linePoints = new float[this.m_linePoints.Count];
      int index = 0;
      foreach (PdfNumber linePoint in this.m_linePoints)
      {
        linePoints[index] = (float) linePoint.IntValue;
        ++index;
      }
    }
    return linePoints;
  }

  private float ConvertToUnit()
  {
    float[] linePoints = this.ObtainLinePoints();
    PointF[] pointFArray = new PointF[linePoints.Length / 2];
    int index1 = 0;
    for (int index2 = 0; index2 < linePoints.Length; index2 += 2)
    {
      pointFArray[index1] = new PointF(linePoints[index2], linePoints[index2 + 1]);
      ++index1;
    }
    return new PdfUnitConvertor().ConvertUnits((float) Math.Sqrt(Math.Pow((double) pointFArray[1].X - (double) pointFArray[0].X, 2.0) + Math.Pow((double) pointFArray[1].Y - (double) pointFArray[0].Y, 2.0)), PdfGraphicsUnit.Point, this.GetEqualPdfGraphicsUnit(this.Unit, out this.m_unitString));
  }

  private RectangleF ObtainLineBounds()
  {
    RectangleF lineBounds = this.Bounds;
    float[] linePoints = this.ObtainLinePoints();
    if (linePoints != null && linePoints.Length == 4)
    {
      PdfPath pdfPath = new PdfPath();
      PdfArray lineStyle = new PdfArray();
      lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
      lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
      lineBounds = this.CalculateLineBounds(linePoints, this.m_leaderLineExt, this.m_leaderLine, this.m_leaderOffset, lineStyle, (double) this.m_borderWidth);
      lineBounds = new RectangleF(lineBounds.X - 8f, lineBounds.Y - 8f, lineBounds.Width + 16f, lineBounds.Height + 16f);
    }
    return lineBounds;
  }

  protected override void Save()
  {
    this.m_borderWidth = this.lineBorder.BorderWidth != 1 ? (float) this.lineBorder.BorderWidth : this.lineBorder.BorderLineWidth;
    if (this.Dictionary.Items.ContainsKey(new PdfName("AP")))
      return;
    RectangleF rectangleF1 = RectangleF.Empty;
    float unit = this.ConvertToUnit();
    float[] linePoints1 = this.ObtainLinePoints();
    PointF[] pts = new PointF[linePoints1.Length / 2];
    int index1 = 0;
    for (int index2 = 0; index2 < linePoints1.Length; index2 += 2)
    {
      pts[index1] = new PointF(linePoints1[index2], linePoints1[index2 + 1]);
      ++index1;
    }
    byte[] types = new byte[pts.Length];
    types[0] = (byte) 0;
    types[1] = (byte) 1;
    this.Bounds = new GraphicsPath(pts, types).GetBounds();
    PdfTemplate pdfTemplate = (PdfTemplate) null;
    int length1 = this.LeaderLine >= 0 ? this.m_leaderLine : -this.m_leaderLine;
    this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
    if (this.Dictionary["AP"] != null)
    {
      rectangleF1 = this.ObtainLineBounds();
      this.Appearance.Normal = new PdfTemplate(rectangleF1);
      pdfTemplate = this.Appearance.Normal;
      PaintParams paintParams = new PaintParams();
      pdfTemplate.m_writeTransformation = false;
      PdfGraphics graphics = this.Appearance.Normal.Graphics;
      PdfPen pdfPen1 = new PdfPen(this.BackColor, this.m_borderWidth);
      PdfBrush m_backBrush = (PdfBrush) new PdfSolidBrush(this.InnerLineColor);
      paintParams.BorderPen = pdfPen1;
      paintParams.ForeBrush = (PdfBrush) new PdfSolidBrush(this.Color);
      RectangleF rectangleF2 = new RectangleF(rectangleF1.X, -rectangleF1.Y - rectangleF1.Height, rectangleF1.Width, rectangleF1.Height);
      float[] linePoints2 = this.ObtainLinePoints();
      if (linePoints2 != null && linePoints2.Length == 4)
      {
        float x1 = linePoints2[0];
        float y1 = linePoints2[1];
        float x2 = linePoints2[2];
        float y2 = linePoints2[3];
        PdfPen pdfPen2 = new PdfPen(this.Color, this.m_borderWidth);
        if (this.lineBorder.BorderStyle == PdfBorderStyle.Dashed)
          pdfPen2.DashStyle = PdfDashStyle.Dash;
        else if (this.lineBorder.BorderStyle == PdfBorderStyle.Dot)
          pdfPen2.DashStyle = PdfDashStyle.Dot;
        PdfSolidBrush brush = new PdfSolidBrush(this.Color);
        PdfStringFormat pdfStringFormat = new PdfStringFormat()
        {
          Alignment = PdfTextAlignment.Center,
          LineAlignment = PdfVerticalAlignment.Middle
        };
        SizeF sizeF = this.Font.MeasureString($"{unit.ToString("0.00")} {this.m_unitString}");
        double angle = (double) x2 - (double) x1 != 0.0 ? this.GetAngle(x1, y1, x2, y2) : ((double) y2 <= (double) y1 ? 270.0 : 90.0);
        graphics.Save();
        if ((double) this.Opacity < 1.0)
          graphics.SetTransparency(this.Opacity);
        float[] numArray1 = new float[2]{ x1, y1 };
        float[] numArray2 = new float[2]{ x2, y2 };
        double num1 = this.m_leaderLine >= 0 ? angle : angle + 180.0;
        float[] axisValue1 = this.GetAxisValue(numArray1, num1 + 90.0, (double) (length1 + this.m_leaderOffset));
        float[] axisValue2 = this.GetAxisValue(numArray2, num1 + 90.0, (double) (length1 + this.m_leaderOffset));
        double num2 = Math.Sqrt(Math.Pow((double) axisValue2[0] - (double) axisValue1[0], 2.0) + Math.Pow((double) axisValue2[1] - (double) axisValue1[1], 2.0));
        double length2 = num2 / 2.0 - ((double) sizeF.Width / 2.0 + (double) this.m_borderWidth);
        float[] axisValue3 = this.GetAxisValue(axisValue1, angle, length2);
        float[] axisValue4 = this.GetAxisValue(axisValue2, angle + 180.0, length2);
        float[] numArray3 = this.BeginLineStyle == PdfLineEndingStyle.OpenArrow || this.BeginLineStyle == PdfLineEndingStyle.ClosedArrow ? this.GetAxisValue(axisValue1, angle, (double) this.m_borderWidth) : axisValue1;
        float[] numArray4 = this.EndLineStyle == PdfLineEndingStyle.OpenArrow || this.EndLineStyle == PdfLineEndingStyle.ClosedArrow ? this.GetAxisValue(axisValue2, angle, -(double) this.m_borderWidth) : axisValue2;
        string str = this.m_captionType.ToString();
        if (str == "Top")
        {
          graphics.DrawLine(pdfPen2, numArray3[0], -numArray3[1], numArray4[0], -numArray4[1]);
        }
        else
        {
          graphics.DrawLine(pdfPen2, numArray3[0], -numArray3[1], axisValue3[0], -axisValue3[1]);
          graphics.DrawLine(pdfPen2, numArray4[0], -numArray4[1], axisValue4[0], -axisValue4[1]);
        }
        PdfArray lineStyle = new PdfArray();
        lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
        lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
        double borderWidth = (double) this.m_borderWidth;
        this.SetLineEndingStyles(axisValue1, axisValue2, graphics, angle, pdfPen2, m_backBrush, lineStyle, borderWidth);
        float[] axisValue5 = this.GetAxisValue(axisValue1, num1 + 90.0, (double) this.m_leaderLineExt);
        graphics.DrawLine(pdfPen2, axisValue1[0], -axisValue1[1], axisValue5[0], -axisValue5[1]);
        float[] axisValue6 = this.GetAxisValue(axisValue2, num1 + 90.0, (double) this.m_leaderLineExt);
        graphics.DrawLine(pdfPen2, axisValue2[0], -axisValue2[1], axisValue6[0], -axisValue6[1]);
        float[] axisValue7 = this.GetAxisValue(axisValue1, num1 - 90.0, (double) length1);
        graphics.DrawLine(pdfPen2, axisValue1[0], -axisValue1[1], axisValue7[0], -axisValue7[1]);
        float[] axisValue8 = this.GetAxisValue(axisValue2, num1 - 90.0, (double) length1);
        graphics.DrawLine(pdfPen2, axisValue2[0], -axisValue2[1], axisValue8[0], -axisValue8[1]);
        double length3 = num2 / 2.0;
        float[] axisValue9 = this.GetAxisValue(axisValue1, angle, length3);
        float[] numArray5 = new float[2];
        float[] numArray6 = !(str == "Top") ? this.GetAxisValue(axisValue9, angle + 90.0, (double) this.Font.Height / 2.0) : this.GetAxisValue(axisValue9, angle + 90.0, (double) this.Font.Height);
        graphics.TranslateTransform(numArray6[0], -numArray6[1]);
        graphics.RotateTransform(this.CalculateAngle(new PointF(linePoints2[0], linePoints2[1]), new PointF(linePoints2[2], linePoints2[3])));
        graphics.DrawString($"{unit.ToString("0.00")} {this.m_unitString}", this.Font, (PdfBrush) brush, new PointF((float) (-(double) sizeF.Width / 2.0), 0.0f));
        graphics.Restore();
      }
    }
    base.Save();
    this.Dictionary.SetProperty("DS", (IPdfPrimitive) new PdfString($"font:{this.Font.Name} {this.Font.Size}pt; color:{this.ColorToHex((System.Drawing.Color) this.Color)}"));
    PdfDictionary measureDictioanry = this.CreateMeasureDictioanry(this.m_unitString);
    if (!this.Dictionary.Items.ContainsKey(new PdfName("Measure")))
      this.Dictionary.Items.Add(new PdfName("Measure"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) measureDictioanry));
    this.m_lineStyle = new PdfArray();
    this.m_lineStyle.Insert(0, (IPdfPrimitive) new PdfName((Enum) this.BeginLineStyle));
    this.m_lineStyle.Insert(1, (IPdfPrimitive) new PdfName((Enum) this.EndLineStyle));
    this.Dictionary.SetProperty("LE", (IPdfPrimitive) this.m_lineStyle);
    if (this.m_linePoints == null)
      throw new PdfException("LinePoints cannot be null");
    this.Dictionary.SetProperty("L", (IPdfPrimitive) this.m_linePoints);
    if (this.m_lineBorder.DashArray == 0)
    {
      if (this.m_lineBorder.BorderStyle == PdfBorderStyle.Dashed)
        this.m_lineBorder.DashArray = 4;
      else if (this.m_lineBorder.BorderStyle == PdfBorderStyle.Dot)
        this.m_lineBorder.DashArray = 2;
    }
    this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_lineBorder);
    if (this.InnerLineColor.A != (byte) 0)
      this.Dictionary.SetProperty("IC", (IPdfPrimitive) this.InnerLineColor.ToArray());
    this.Dictionary["C"] = (IPdfPrimitive) this.Color.ToArray();
    this.Dictionary.SetProperty("Contents", (IPdfPrimitive) new PdfString($"{this.Text}\n{unit.ToString("0.00")} {this.m_unitString}"));
    this.Dictionary.SetProperty("IT", (IPdfPrimitive) new PdfName("LineDimension"));
    this.Dictionary.SetProperty("LLE", (IPdfPrimitive) new PdfNumber(this.m_leaderLineExt));
    this.Dictionary.SetProperty("LLO", (IPdfPrimitive) new PdfNumber(this.m_leaderOffset));
    this.Dictionary.SetProperty("LL", (IPdfPrimitive) new PdfNumber(this.m_leaderLine));
    this.Dictionary.SetProperty("CP", (IPdfPrimitive) new PdfName((Enum) this.m_captionType));
    this.Dictionary.SetProperty("Cap", (IPdfPrimitive) new PdfBoolean(this.LineCaption));
    this.Dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangleF1));
    if (!this.Flatten)
      return;
    if (this.Page != null)
    {
      PdfDictionary content = (PdfDictionary) pdfTemplate.m_content;
      this.Page.Document.SetWaterMarkResources(pdfTemplate.m_resources, this.Page.GetResources());
      content.Items.Clear();
      this.Page.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (content as PdfStream)));
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
    else
    {
      PdfDictionary content = (PdfDictionary) pdfTemplate.m_content;
      this.Page.Document.SetWaterMarkResources(pdfTemplate.m_resources, this.LoadedPage.GetResources());
      content.Items.Clear();
      this.LoadedPage.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (content as PdfStream)));
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
  }

  private float CalculateAngle(PointF startPoint, PointF endPoint)
  {
    return -(float) (Math.Atan2((double) (endPoint.Y - startPoint.Y), (double) (endPoint.X - startPoint.X)) * (180.0 / Math.PI));
  }
}
