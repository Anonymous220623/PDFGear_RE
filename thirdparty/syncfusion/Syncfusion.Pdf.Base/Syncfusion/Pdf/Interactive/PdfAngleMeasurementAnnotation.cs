// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAngleMeasurementAnnotation
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

public class PdfAngleMeasurementAnnotation : PdfAnnotation
{
  private LineBorder m_border = new LineBorder();
  internal PdfArray m_linePoints;
  private int m_lineExtension;
  private PdfFont m_font;
  private PointF m_firstIntersectionPoint = PointF.Empty;
  private PointF m_secondIntersectionPoint = PointF.Empty;
  private PointF[] m_pointArray;
  private float m_startAngle;
  private float m_sweepAngle;
  private float m_radius;
  private float m_angle;
  private float m_borderWidth;

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

  internal float Angle
  {
    get => this.m_angle;
    set => this.m_angle = value;
  }

  public PdfAngleMeasurementAnnotation(PointF[] points)
  {
    this.m_pointArray = points.Length <= 6 ? points : throw new PdfException("points length should not be greater than 3 ");
    int[] array = new int[points.Length * 2];
    int index1 = 0;
    for (int index2 = 0; index2 < points.Length; ++index2)
    {
      array[index1] = (int) points[index2].X;
      array[index1 + 1] = (int) points[index2].Y;
      index1 += 2;
    }
    this.m_linePoints = new PdfArray(array);
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("PolyLine"));
  }

  private RectangleF GetBoundsValue()
  {
    PointF[] linePoints = this.ObtainLinePoints();
    for (int index = 0; index < linePoints.Length; ++index)
      linePoints[index].Y = -linePoints[index].Y;
    byte[] types = new byte[3]
    {
      (byte) 0,
      (byte) 1,
      (byte) 1
    };
    this.Bounds = new GraphicsPath(linePoints, types).GetBounds();
    return this.Bounds;
  }

  private PointF[] ObtainLinePoints()
  {
    PointF[] linePoints = (PointF[]) null;
    if (this.m_linePoints != null)
    {
      if (this.Page != null)
      {
        double height1 = (double) this.Page.Size.Height;
      }
      else
      {
        double height2 = (double) this.LoadedPage.Size.Height;
      }
      float[] numArray = new float[this.m_linePoints.Count];
      int index1 = 0;
      foreach (PdfNumber linePoint in this.m_linePoints)
      {
        numArray[index1] = linePoint.FloatValue;
        ++index1;
      }
      linePoints = new PointF[numArray.Length / 2];
      int index2 = 0;
      for (int index3 = 0; index3 < numArray.Length; index3 += 2)
      {
        linePoints[index2] = new PointF(numArray[index3], -numArray[index3 + 1]);
        ++index2;
      }
    }
    return linePoints;
  }

  private double CalculateAngle()
  {
    PointF[] pts = (PointF[]) null;
    if (this.m_linePoints != null)
    {
      float[] numArray = new float[this.m_linePoints.Count];
      int index1 = 0;
      foreach (PdfNumber linePoint in this.m_linePoints)
      {
        numArray[index1] = linePoint.FloatValue;
        ++index1;
      }
      pts = new PointF[numArray.Length / 2];
      int index2 = 0;
      for (int index3 = 0; index3 < numArray.Length; index3 += 2)
      {
        pts[index2] = new PointF(numArray[index3], numArray[index3 + 1]);
        ++index2;
      }
    }
    PointF point1_1 = pts[0];
    PointF point2 = pts[1];
    PointF point1_2 = pts[2];
    this.m_radius = Math.Min((float) Math.Sqrt(Math.Pow((double) point2.X - (double) point1_1.X, 2.0) + Math.Pow((double) point2.Y - (double) point1_1.Y, 2.0)), (float) Math.Sqrt(Math.Pow((double) point2.X - (double) point1_2.X, 2.0) + Math.Pow((double) point2.Y - (double) point1_2.Y, 2.0))) / 4f;
    byte[] types = new byte[pts.Length];
    types[0] = (byte) 0;
    types[1] = (byte) 1;
    types[2] = (byte) 1;
    GraphicsPath graphicsPath = new GraphicsPath(pts, types);
    PointF intersection1_1;
    PointF intersection2_1;
    this.FindLineCircleIntersectionPoints(point2.X, point2.Y, this.m_radius, point1_1, point2, out intersection1_1, out intersection2_1);
    if (graphicsPath.IsVisible(intersection1_1))
      this.m_firstIntersectionPoint = intersection1_1;
    else if (graphicsPath.IsVisible(intersection2_1))
      this.m_firstIntersectionPoint = intersection2_1;
    if ((double) this.m_firstIntersectionPoint.X == 0.0 && (double) this.m_firstIntersectionPoint.Y == 0.0)
      this.m_firstIntersectionPoint = intersection2_1;
    PointF intersection1_2 = PointF.Empty;
    PointF intersection2_2 = PointF.Empty;
    this.FindLineCircleIntersectionPoints(point2.X, point2.Y, this.m_radius, point1_2, point2, out intersection1_2, out intersection2_2);
    if (graphicsPath.IsVisible(intersection1_2) && (int) this.m_firstIntersectionPoint.X != (int) intersection1_2.X && (int) this.m_firstIntersectionPoint.Y != (int) intersection1_2.Y)
      this.m_secondIntersectionPoint = intersection1_2;
    else if (graphicsPath.IsVisible(intersection2_2) && (int) this.m_firstIntersectionPoint.X != (int) intersection2_2.X && (int) this.m_firstIntersectionPoint.Y != (int) intersection2_2.Y)
      this.m_secondIntersectionPoint = intersection2_2;
    if (this.m_secondIntersectionPoint.IsEmpty)
      this.m_secondIntersectionPoint = intersection2_2;
    float x1 = this.m_firstIntersectionPoint.X - point2.X;
    float num1 = (float) (Math.Atan2((double) (this.m_firstIntersectionPoint.Y - point2.Y), (double) x1) * (180.0 / Math.PI));
    float x2 = this.m_secondIntersectionPoint.X - point2.X;
    float num2 = (float) (Math.Atan2((double) (this.m_secondIntersectionPoint.Y - point2.Y), (double) x2) * (180.0 / Math.PI));
    double num3;
    if ((double) num1 <= 0.0)
    {
      num3 = -(double) num1;
    }
    else
    {
      float num4 = (float) (num3 = 360.0 - (double) num1);
    }
    float num5 = (float) num3;
    double num6;
    if ((double) num2 <= 0.0)
    {
      num6 = -(double) num2;
    }
    else
    {
      float num7 = (float) (num6 = 360.0 - (double) num2);
    }
    float num8 = (float) num6;
    if ((double) num5 == 180.0 && (double) num8 == 0.0)
    {
      this.m_startAngle = num5;
      this.m_sweepAngle = 180f;
    }
    else if ((double) num5 == 0.0 && (double) num8 == 180.0)
    {
      this.m_startAngle = num8;
      this.m_sweepAngle = 180f;
    }
    else if ((double) num5 < 180.0)
    {
      if ((double) num5 > (double) num8)
      {
        this.m_startAngle = num8;
        this.m_sweepAngle = num5 - num8;
      }
      else if ((double) num5 + 180.0 < (double) num8)
      {
        this.m_startAngle = num8;
        this.m_sweepAngle = 360f - num8 + num5;
      }
      else
      {
        this.m_startAngle = num5;
        this.m_sweepAngle = num8 - num5;
      }
    }
    else if ((double) num5 < (double) num8)
    {
      this.m_startAngle = num5;
      this.m_sweepAngle = num8 - num5;
    }
    else if ((double) num5 - 180.0 > (double) num8)
    {
      this.m_startAngle = num5;
      this.m_sweepAngle = 360f - num5 + num8;
    }
    else
    {
      this.m_startAngle = num8;
      this.m_sweepAngle = num5 - num8;
    }
    double y1 = (double) point1_2.X - (double) point2.X;
    double x3 = (double) point1_2.Y - (double) point2.Y;
    double y2 = (double) point1_1.X - (double) point2.X;
    double x4 = (double) point1_1.Y - (double) point2.Y;
    Math.Atan2(y1, x3);
    Math.Atan2(y2, x4);
    return Math.Atan2(y1, x3) - Math.Atan2(y2, x4);
  }

  private int FindLineCircleIntersectionPoints(
    float centerX,
    float centerY,
    float radius,
    PointF point1,
    PointF point2,
    out PointF intersection1,
    out PointF intersection2)
  {
    float num1 = point2.X - point1.X;
    float num2 = point2.Y - point1.Y;
    float num3 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    float num4 = (float) (2.0 * ((double) num1 * ((double) point1.X - (double) centerX) + (double) num2 * ((double) point1.Y - (double) centerY)));
    float num5 = (float) (((double) point1.X - (double) centerX) * ((double) point1.X - (double) centerX) + ((double) point1.Y - (double) centerY) * ((double) point1.Y - (double) centerY) - (double) radius * (double) radius);
    float d = (float) ((double) num4 * (double) num4 - 4.0 * (double) num3 * (double) num5);
    if ((double) num3 <= 1E-07 || (double) d < 0.0)
    {
      intersection1 = new PointF(float.NaN, float.NaN);
      intersection2 = new PointF(float.NaN, float.NaN);
      return 0;
    }
    if ((double) d == 0.0)
    {
      float num6 = (float) (-(double) num4 / (2.0 * (double) num3));
      intersection1 = new PointF(point1.X + num6 * num1, point1.Y + num6 * num2);
      intersection2 = new PointF(float.NaN, float.NaN);
      return 1;
    }
    float num7 = (float) ((-(double) num4 + Math.Sqrt((double) d)) / (2.0 * (double) num3));
    intersection1 = new PointF(point1.X + num7 * num1, point1.Y + num7 * num2);
    float num8 = (float) ((-(double) num4 - Math.Sqrt((double) d)) / (2.0 * (double) num3));
    intersection2 = new PointF(point1.X + num8 * num1, point1.Y + num8 * num2);
    return 2;
  }

  protected override void Save()
  {
    this.m_borderWidth = this.Border.BorderWidth != 1 ? (float) this.Border.BorderWidth : this.Border.BorderLineWidth;
    if (this.Dictionary.Items.ContainsKey(new PdfName("AP")))
      return;
    base.Save();
    int num1 = (int) (this.CalculateAngle() * (180.0 / Math.PI));
    if (num1 < 0)
      num1 = -num1;
    if (num1 > 180)
      num1 = 360 - num1;
    if (!this.Dictionary.Items.ContainsKey(new PdfName("C")))
    {
      PdfColorSpace colorSpace = PdfColorSpace.RGB;
      if (this.Page != null)
        colorSpace = this.Page.Section.Parent.Document.ColorSpace;
      this.Dictionary.SetProperty("C", (IPdfPrimitive) this.Color.ToArray(colorSpace));
    }
    this.Dictionary.SetProperty("Vertices", (IPdfPrimitive) new PdfArray(this.m_linePoints));
    this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
    if (!this.Dictionary.Items.ContainsKey(new PdfName("MeasurementTypes")))
      this.Dictionary.Items.Add(new PdfName("MeasurementTypes"), (IPdfPrimitive) new PdfNumber(1152));
    this.Dictionary.SetProperty("DS", (IPdfPrimitive) new PdfString($"font:{this.Font.Name} {this.Font.Size}pt; color:{ColorTranslator.ToHtml((System.Drawing.Color) this.Color)}"));
    if (!this.Dictionary.Items.ContainsKey(new PdfName("Subj")))
      this.Dictionary.Items.Add(new PdfName("Subj"), (IPdfPrimitive) new PdfString("Angle Measurement"));
    this.Dictionary[new PdfName("Contents")] = (IPdfPrimitive) new PdfString($"{this.Text}\n{num1.ToString("0.00")}°");
    if (!this.Dictionary.Items.ContainsKey(new PdfName("IT")))
      this.Dictionary.Items.Add(new PdfName("IT"), (IPdfPrimitive) new PdfName("PolyLineAngle"));
    PdfDictionary pdfDictionary = new PdfDictionary();
    PdfArray pdfArray1 = new PdfArray();
    PdfArray pdfArray2 = new PdfArray();
    PdfArray pdfArray3 = new PdfArray();
    PdfArray pdfArray4 = new PdfArray();
    PdfArray pdfArray5 = new PdfArray();
    pdfDictionary.Items.Add(new PdfName("D"), (IPdfPrimitive) pdfArray1);
    pdfDictionary.Items.Add(new PdfName("T"), (IPdfPrimitive) pdfArray2);
    pdfDictionary.Items.Add(new PdfName("A"), (IPdfPrimitive) pdfArray3);
    pdfDictionary.Items.Add(new PdfName("X"), (IPdfPrimitive) pdfArray4);
    pdfDictionary.Items.Add(new PdfName("V"), (IPdfPrimitive) pdfArray5);
    pdfDictionary.Items.Add(new PdfName("Type"), (IPdfPrimitive) new PdfName("Measure"));
    pdfDictionary.Items.Add(new PdfName("R"), (IPdfPrimitive) new PdfString("1 in = 1 in"));
    pdfDictionary.Items.Add(new PdfName("Subtype"), (IPdfPrimitive) new PdfName("RL"));
    pdfDictionary.Items.Add(new PdfName("TargetUnitConversion"), (IPdfPrimitive) new PdfNumber(0.1388889));
    pdfArray1.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString("in")
        },
        {
          new PdfName("Type"),
          (IPdfPrimitive) new PdfName("NumberFormat")
        },
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        }
      }
    });
    pdfArray2.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString("°")
        },
        {
          new PdfName("Type"),
          (IPdfPrimitive) new PdfName("NumberFormat")
        },
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("FD"),
          (IPdfPrimitive) new PdfBoolean(true)
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        }
      }
    });
    pdfArray3.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString("sq in")
        },
        {
          new PdfName("Type"),
          (IPdfPrimitive) new PdfName("NumberFormat")
        },
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("FD"),
          (IPdfPrimitive) new PdfBoolean(true)
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        }
      }
    });
    pdfArray5.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString("cu in")
        },
        {
          new PdfName("Type"),
          (IPdfPrimitive) new PdfName("NumberFormat")
        },
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("FD"),
          (IPdfPrimitive) new PdfBoolean(true)
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        }
      }
    });
    pdfArray4.Add((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("U"),
          (IPdfPrimitive) new PdfString("in")
        },
        {
          new PdfName("Type"),
          (IPdfPrimitive) new PdfName("NumberFormat")
        },
        {
          new PdfName("C"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("D"),
          (IPdfPrimitive) new PdfNumber(1)
        },
        {
          new PdfName("SS"),
          (IPdfPrimitive) new PdfString("")
        }
      }
    });
    this.Dictionary["Measure"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    RectangleF empty = RectangleF.Empty;
    this.GetBoundsValue();
    RectangleF rectangleF = RectangleF.Empty;
    PointF[] linePoints = this.ObtainLinePoints();
    byte[] pathTypes = new byte[linePoints.Length];
    pathTypes[0] = (byte) 0;
    for (int index = 1; index < linePoints.Length; ++index)
      pathTypes[index] = (byte) 1;
    GraphicsPath graphicsPath1 = new GraphicsPath();
    graphicsPath1.AddRectangle(new RectangleF(linePoints[1].X - this.m_radius, (float) -((double) linePoints[1].Y + (double) this.m_radius), 2f * this.m_radius, 2f * this.m_radius));
    graphicsPath1.GetBounds();
    SizeF sizeF = this.Font.MeasureString(num1.ToString() + "°");
    PointF pointF1 = new PointF((float) (((double) this.m_firstIntersectionPoint.X + (double) this.m_secondIntersectionPoint.X) / 2.0), (float) (((double) this.m_firstIntersectionPoint.Y + (double) this.m_secondIntersectionPoint.Y) / 2.0));
    PointF pointF2 = new PointF(linePoints[1].X, -linePoints[1].Y);
    float x1 = linePoints[1].X + this.m_radius * (float) Math.Cos(((double) this.m_startAngle + (double) this.m_sweepAngle / 2.0) * (Math.PI / 180.0));
    float y = linePoints[1].Y + this.m_radius * (float) Math.Sin(((double) this.m_startAngle + (double) this.m_sweepAngle / 2.0) * (Math.PI / 180.0));
    PointF pointF3 = new PointF(pointF1.X, pointF1.Y);
    float x2 = pointF3.X - pointF2.X;
    float num2 = (float) (Math.Atan2((double) (pointF3.Y - pointF2.Y), (double) x2) * (180.0 / Math.PI));
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    if ((double) num2 > 0.0)
    {
      if ((double) num2 < 45.0)
        flag2 = true;
      else if ((double) num2 >= 45.0 && (double) num2 < 135.0)
        flag3 = true;
      else
        flag1 = true;
    }
    else
    {
      float num3 = -num2;
      if ((double) num3 == 0.0)
      {
        GraphicsPath graphicsPath2 = new GraphicsPath();
        graphicsPath2.AddRectangle(this.Bounds);
        if (!graphicsPath2.IsVisible(new PointF(x1, y)))
        {
          if ((double) this.m_startAngle + (double) this.m_sweepAngle == 360.0 || (double) this.m_startAngle + (double) this.m_sweepAngle == 180.0)
          {
            rectangleF = new RectangleF(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, (float) ((double) this.Bounds.Height + (double) this.m_radius + (double) this.Font.Height + 4.0));
            flag3 = true;
          }
          else if (((double) this.m_startAngle + (double) this.m_sweepAngle) % 360.0 == 45.0)
          {
            rectangleF = new RectangleF(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
            flag2 = true;
          }
          else
          {
            rectangleF = (double) this.Bounds.X <= (double) x1 - (double) sizeF.Width - 2.0 ? this.Bounds : new RectangleF(this.Bounds.X - (float) ((double) this.m_radius + (double) sizeF.Width + 4.0), this.Bounds.Y, (float) ((double) this.Bounds.Width + (double) this.m_radius + (double) sizeF.Width + 4.0), this.Bounds.Height);
            flag1 = true;
          }
        }
      }
      else if ((double) num3 < 45.0)
        flag2 = true;
      else if ((double) num3 < 45.0 || (double) num3 >= 135.0)
        flag1 = true;
    }
    if (rectangleF.IsEmpty)
      rectangleF = new RectangleF(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
    PdfPath path = new PdfPath(linePoints, pathTypes);
    this.Dictionary["Rect"] = (IPdfPrimitive) PdfArray.FromRectangle(rectangleF);
    this.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
    this.Appearance.Normal = new PdfTemplate(rectangleF);
    PdfTemplate normal = this.Appearance.Normal;
    PaintParams paintParams = new PaintParams();
    normal.m_writeTransformation = false;
    PdfGraphics graphics = this.Appearance.Normal.Graphics;
    PdfPen pen = new PdfPen(this.Color, this.m_borderWidth);
    if (this.Border.BorderStyle == PdfBorderStyle.Dashed)
      pen.DashStyle = PdfDashStyle.Dash;
    PdfBrush brush = (PdfBrush) new PdfSolidBrush(this.Color);
    graphics.Save();
    if ((double) this.Opacity < 1.0)
      graphics.SetTransparency(this.Opacity);
    graphics.DrawPath(pen, path);
    graphics.DrawArc(pen, new RectangleF(linePoints[1].X - this.m_radius, linePoints[1].Y - this.m_radius, 2f * this.m_radius, 2f * this.m_radius), this.m_startAngle, this.m_sweepAngle);
    if (flag3)
      graphics.DrawString(num1.ToString() + "°", this.Font, brush, new PointF(x1 - sizeF.Width / 2f, (float) -(-(double) y + (double) this.Font.Height + 2.0)));
    else if (flag2)
      graphics.DrawString(num1.ToString() + "°", this.Font, brush, new PointF(x1 + 2f, (float) -(-(double) y + (double) this.Font.Height / 2.0)));
    else if (flag1)
      graphics.DrawString(num1.ToString() + "°", this.Font, brush, new PointF((float) ((double) x1 - (double) sizeF.Width - 2.0), (float) -(-(double) y + (double) this.Font.Height / 2.0)));
    else
      graphics.DrawString(num1.ToString() + "°", this.Font, brush, new PointF(x1 - sizeF.Width / 2f, y + 2f));
    graphics.Restore();
    if (!this.Flatten)
      return;
    if (this.Page != null)
    {
      PdfDictionary content = (PdfDictionary) normal.m_content;
      this.Page.Document.SetWaterMarkResources(normal.m_resources, this.Page.GetResources());
      content.Items.Clear();
      this.Page.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (content as PdfStream)));
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
    else
    {
      PdfDictionary content = (PdfDictionary) normal.m_content;
      this.Page.Document.SetWaterMarkResources(normal.m_resources, this.LoadedPage.GetResources());
      content.Items.Clear();
      this.LoadedPage.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (content as PdfStream)));
      this.RemoveAnnoationFromPage((PdfPageBase) this.Page, (PdfAnnotation) this);
    }
  }
}
