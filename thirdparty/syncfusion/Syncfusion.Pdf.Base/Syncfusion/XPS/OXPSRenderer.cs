// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

internal class OXPSRenderer : IDisposable
{
  private PdfGraphics m_graphics;
  private PdfPage m_page;
  private PdfUnitConvertor m_unitConvertor;
  private OXPSDocumentReader m_reader;
  private char[] m_commaSeparator = new char[1]{ ',' };
  private bool m_bStateChanged;
  private PdfTransformationMatrix currentMatrix;
  private OXPSCanvas m_canvas;
  private float m_locationY;
  private bool m_isLineClip;
  private XPSToPdfConverterSettings m_embedFont;
  private int count;
  private List<float> list = new List<float>();

  internal OXPSRenderer(PdfPage page, OXPSDocumentReader reader)
  {
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
    this.m_graphics = page.Graphics;
    this.m_unitConvertor = new PdfUnitConvertor(96f);
    this.m_reader = reader;
  }

  internal PdfGraphics Graphics => this.m_graphics;

  internal PrivateFontCollection PrivateFonts => PdfDocument.PrivateFonts;

  internal XPSToPdfConverterSettings EmbedFont
  {
    get => this.m_embedFont;
    set => this.m_embedFont = value;
  }

  internal void DrawGlyphs(OXPSGlyphs glyphs)
  {
    PdfGraphicsState state1 = this.m_graphics.Save();
    if (glyphs.Opacity < 1.0)
      this.Graphics.SetTransparency((float) glyphs.Opacity);
    if (glyphs.RenderTransform != null)
    {
      string renderTransform = glyphs.RenderTransform;
      if (this.count == 0 || glyphs.OriginX == 0.0 && glyphs.OriginY == 0.0)
      {
        this.ApplyRenderTransform(renderTransform);
      }
      else
      {
        float[] elements = this.ReadMatrix(renderTransform).Elements;
        glyphs.OriginX *= (double) elements[0];
        glyphs.OriginY *= (double) elements[3];
        glyphs.FontRenderingEmSize *= (double) elements[0];
      }
    }
    if (glyphs.GlyphsRenderTransform != null)
    {
      state1 = this.m_graphics.Save();
      this.ApplyRenderTransform(glyphs.GlyphsRenderTransform.MatrixTransform.Matrix);
    }
    this.m_graphics.m_isNormalRender = false;
    if (glyphs.UnicodeString != null && glyphs.FontRenderingEmSize > 0.0)
    {
      PointF pointF = new PointF(this.ConvertToPoints(glyphs.OriginX), this.ConvertToPoints(glyphs.OriginY));
      this.m_locationY = pointF.Y;
      PdfFont font = this.GetFont(glyphs);
      PdfStringFormat format1 = new PdfStringFormat();
      float ascent = font.Metrics.GetAscent(format1);
      float lineGap = font.Metrics.GetLineGap(format1);
      if (!glyphs.IsSideways)
        pointF.Y -= ascent + lineGap;
      PdfGraphicsState state2 = (PdfGraphicsState) null;
      if ((glyphs.StyleSimulations == OXPSStyleSimulations.ItalicSimulation || glyphs.StyleSimulations == OXPSStyleSimulations.BoldItalicSimulation) && !font.Metrics.PostScriptName.Contains("Italic"))
      {
        state2 = this.m_graphics.Save();
        this.m_graphics.TranslateTransform(pointF.X, pointF.Y);
        this.m_graphics.SkewTransform(0.0f, -10f);
        pointF = PointF.Empty;
      }
      PdfBrush brush = this.GetSolidBrush(glyphs.Fill);
      if (glyphs.Fill != null)
      {
        PdfColor pdfColor = new PdfColor(ColorTranslator.FromHtml(glyphs.Fill));
        if (pdfColor.A != byte.MaxValue)
          this.Graphics.SetTransparency((float) pdfColor.A / (float) byte.MaxValue);
      }
      PdfPen pen = (PdfPen) null;
      bool flag = false;
      if (glyphs.Fill == null && glyphs.GlyphsFill != null)
      {
        if (glyphs.GlyphsFill.Item is OXPSSolidColorBrush)
          brush = (PdfBrush) new PdfSolidBrush((PdfColor) this.FromHtml((glyphs.GlyphsFill.Item as OXPSSolidColorBrush).Color));
        else if (glyphs.GlyphsFill.Item is OXPSLinearGradientBrush)
          brush = (PdfBrush) this.ReadLinearGradientBrush(glyphs.GlyphsFill.Item as OXPSLinearGradientBrush);
        else if (glyphs.GlyphsFill.Item is OXPSRadialGradientBrush)
          brush = (PdfBrush) this.ReadRadialGradientBrush(glyphs.GlyphsFill.Item as OXPSRadialGradientBrush);
      }
      if ((glyphs.StyleSimulations == OXPSStyleSimulations.BoldSimulation || glyphs.StyleSimulations == OXPSStyleSimulations.BoldItalicSimulation) && !font.Metrics.PostScriptName.Contains("Bold"))
      {
        flag = true;
        pen = new PdfPen(brush);
        pen.Width = 0.3f;
      }
      if (glyphs.Clip != null)
      {
        PdfPath pathFromGeometry = this.GetPathFromGeometry(glyphs.Clip);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        for (int index = 0; index < pathFromGeometry.PathPoints.Length; ++index)
        {
          switch (index)
          {
            case 1:
              num1 = pathFromGeometry.PathPoints[index].X;
              break;
            case 2:
              num2 = pathFromGeometry.PathPoints[index].X;
              break;
            case 3:
              num3 = pathFromGeometry.PathPoints[index].X;
              break;
          }
        }
        if ((double) pathFromGeometry.PathPoints[0].X != (double) num1 && (double) num1 == (double) num2 && (double) num3 == (double) pathFromGeometry.PathPoints[0].X || !this.m_isLineClip)
          this.m_graphics.SetClip(pathFromGeometry);
      }
      PdfStringFormat format2 = new PdfStringFormat();
      format2.MeasureTrailingSpaces = true;
      PdfGraphicsState pdfGraphicsState;
      if (glyphs.Indices != null && glyphs.Indices.Contains(",") && !glyphs.Indices.Contains("(") && !glyphs.Indices.Contains(")") && !glyphs.Indices.Contains(".") && !glyphs.Indices.Contains("-") && !glyphs.Indices.Contains("E") && !glyphs.Indices.Contains("e") && ((double) this.m_locationY < (double) this.m_page.Size.Height || glyphs.RenderTransform != null))
      {
        string[] strArray = glyphs.Indices.Split(';');
        PdfTextElement pdfTextElement = new PdfTextElement();
        PdfLayoutResult pdfLayoutResult = (PdfLayoutResult) null;
        float width = 0.0f;
        if (strArray.Length > 0 && strArray.Length <= glyphs.UnicodeString.Length)
        {
          for (int index = 0; index < strArray.Length; ++index)
          {
            pdfTextElement.Text = glyphs.UnicodeString[index].ToString();
            pdfTextElement.Font = font;
            pdfTextElement.Brush = brush;
            if (flag)
              pdfTextElement.Pen = pen;
            pdfTextElement.StringFormat = format2;
            if (pdfLayoutResult != null)
              pointF.X = (double) width == 0.0 ? pdfLayoutResult.Bounds.Right : pdfLayoutResult.Bounds.Left + width;
            width = 0.0f;
            this.m_graphics.m_isNormalRender = false;
            if (strArray[index].Length > 0 && strArray[index].Contains(","))
            {
              strArray[index] = strArray[index].Split(',')[1];
              width = this.ConvertToPoints((double) int.Parse(strArray[index]) * glyphs.FontRenderingEmSize / 100.0);
            }
            if ((double) width > 0.0 && (double) width > (double) font.GetCharWidth(glyphs.UnicodeString[index], format2) && (double) this.m_locationY < (double) this.m_page.Size.Height)
              pdfLayoutResult = (PdfLayoutResult) pdfTextElement.Draw(this.m_page, pointF, width, new PdfLayoutFormat());
            else if ((double) this.m_locationY < (double) this.m_page.Size.Height)
            {
              pdfLayoutResult = pdfTextElement.Draw(this.m_page, pointF);
            }
            else
            {
              this.m_graphics.DrawString(pdfTextElement.Text, font, pen, brush, pointF, format2);
              pointF.X += width;
            }
          }
          if ((double) width > 0.0 && pdfLayoutResult != null)
            pointF.X = pdfLayoutResult.Bounds.X + width;
          else if (pdfLayoutResult != null)
            pointF.X = pdfLayoutResult.Bounds.Right;
          string s = glyphs.UnicodeString.Substring(strArray.Length);
          if (!flag)
            this.m_graphics.DrawString(s, font, brush, pointF, format2);
          else
            this.m_graphics.DrawString(s, font, pen, brush, pointF, format2);
          if (state2 != null)
          {
            this.m_graphics.Restore(state2);
            pdfGraphicsState = (PdfGraphicsState) null;
          }
          if (state1 == null)
            return;
          this.m_graphics.Restore(state1);
          return;
        }
      }
      if (glyphs.IsSideways)
      {
        this.m_graphics.RotateTransform(-90f);
        foreach (char ch in glyphs.UnicodeString.ToCharArray())
        {
          PdfStringLayoutResult stringLayoutResult = new PdfStringLayouter().Layout(ch.ToString(), font, format2, new SizeF());
          PointF point = pointF;
          this.m_graphics.m_isNormalRender = false;
          point.X = (float) (-(double) pointF.Y - (double) stringLayoutResult.ActualSize.Width / 2.0);
          point.Y = pointF.X;
          if (!flag)
            this.m_graphics.DrawString(ch.ToString(), font, brush, point, format2);
          else
            this.m_graphics.DrawString(ch.ToString(), font, pen, brush, point, format2);
          pointF.X += this.ConvertToPoints((double) stringLayoutResult.ActualSize.Height);
        }
        this.m_graphics.RotateTransform(90f);
      }
      else if (!flag)
      {
        if (glyphs.UnicodeString.StartsWith("{}"))
          this.m_graphics.DrawString(glyphs.UnicodeString.Substring(2), font, brush, pointF, format2);
        else
          this.m_graphics.DrawString(glyphs.UnicodeString, font, brush, pointF, format2);
      }
      else if (glyphs.UnicodeString.StartsWith("{}"))
        this.m_graphics.DrawString(glyphs.UnicodeString.Substring(2), font, pen, brush, pointF, format2);
      else
        this.m_graphics.DrawString(glyphs.UnicodeString, font, pen, brush, pointF, format2);
      if (state2 != null)
      {
        this.m_graphics.Restore(state2);
        pdfGraphicsState = (PdfGraphicsState) null;
      }
    }
    if (state1 == null)
      return;
    this.m_graphics.Restore(state1);
  }

  internal PdfPath GetPathFromGeometry(string pathData)
  {
    this.m_isLineClip = false;
    if (pathData == null)
      return (PdfPath) null;
    if (pathData.Contains("StaticResource"))
    {
      OXPSPathGeometry xpsPathGeometry = this.ReadStaticResource(pathData) as OXPSPathGeometry;
      if (xpsPathGeometry.Figures == null)
        return this.GetPathFromPathGeometry(xpsPathGeometry);
      pathData = xpsPathGeometry.Figures;
    }
    pathData = pathData.Trim();
    OXPSPathDataReader oxpsPathDataReader = new OXPSPathDataReader(pathData);
    oxpsPathDataReader.Position = 0;
    PointF pointF1 = PointF.Empty;
    PointF pointF2 = PointF.Empty;
    GraphicsPath graphicsPath = new GraphicsPath();
    char ch;
    PointF val1;
    float num1;
    float num2;
    while ((ch = oxpsPathDataReader.ReadSymbol()) != char.MinValue)
    {
      switch (ch)
      {
        case 'A':
          if (oxpsPathDataReader.TryReadPoint(out val1))
          {
            float rotationAngle;
            oxpsPathDataReader.TryReadFloat(out rotationAngle);
            float num3;
            oxpsPathDataReader.TryReadFloat(out num3);
            float num4;
            oxpsPathDataReader.TryReadFloat(out num4);
            PointF val2;
            oxpsPathDataReader.TryReadPoint(out val2);
            val1 = this.ConvertToPoints(val1);
            val2 = this.ConvertToPoints(val2);
            if (pointF2 != PointF.Empty)
              pointF1 = pointF2;
            List<PointF> arc = this.ComputeArc(pointF1, val2, val1.X, val1.Y, (double) rotationAngle, (double) num3 == 1.0, (double) num4 != 1.0);
            if (arc.Count == 0)
            {
              graphicsPath.AddLine(pointF1, val2);
              pointF1 = val2;
            }
            else if (arc.Count < 0)
              continue;
            graphicsPath.AddBeziers(arc.ToArray());
            pointF2 = pointF1 = arc[arc.Count - 1];
            arc.Clear();
            continue;
          }
          continue;
        case 'C':
          List<PointF> pointFList1 = new List<PointF>();
          if (pointF2 != PointF.Empty)
            pointFList1.Add(pointF2);
          else
            pointFList1.Add(pointF1);
          PointF[] val3 = (PointF[]) null;
          while (oxpsPathDataReader.TryReadPointM3(out val3))
          {
            foreach (PointF point in val3)
              pointFList1.Add(this.ConvertToPoints(point));
          }
          if ((pointFList1.Count - 1) % 3 == 0)
            graphicsPath.AddBeziers(pointFList1.ToArray());
          else
            graphicsPath.AddLines(pointFList1.ToArray());
          pointF2 = pointFList1[pointFList1.Count - 1];
          pointFList1.Clear();
          continue;
        case 'F':
          float num5;
          if (oxpsPathDataReader.TryReadFloat(out num5))
          {
            graphicsPath.FillMode = (double) num5 == 0.0 ? FillMode.Alternate : FillMode.Winding;
            continue;
          }
          continue;
        case 'H':
          if (oxpsPathDataReader.TryReadFloat(out num1))
          {
            float points = this.ConvertToPoints((double) num1);
            graphicsPath.AddLine(pointF1, new PointF(points, pointF1.Y));
            pointF1 = new PointF(points, pointF1.Y);
            continue;
          }
          continue;
        case 'L':
        case 'l':
          this.m_isLineClip = true;
          List<PointF> pointFList2 = new List<PointF>();
          PointF pt1 = !(pointF2 != PointF.Empty) ? pointF1 : pointF2;
          while (oxpsPathDataReader.TryReadPoint(out val1))
          {
            graphicsPath.AddLine(pt1, this.ConvertToPoints(val1));
            pt1 = pointF2 = this.ConvertToPoints(val1);
          }
          continue;
        case 'M':
          if (oxpsPathDataReader.TryReadPoint(out val1))
          {
            graphicsPath.StartFigure();
            pointF1 = this.ConvertToPoints(val1);
            pointF2 = PointF.Empty;
            continue;
          }
          continue;
        case 'V':
          if (oxpsPathDataReader.TryReadFloat(out num2))
          {
            float points = this.ConvertToPoints((double) num2);
            graphicsPath.AddLine(pointF1, new PointF(pointF1.X, points));
            pointF1 = new PointF(pointF1.X, points);
            continue;
          }
          continue;
        case 'Z':
        case 'z':
          if (oxpsPathDataReader.EOF)
          {
            graphicsPath.CloseAllFigures();
            continue;
          }
          graphicsPath.CloseFigure();
          continue;
        case 'h':
          if (oxpsPathDataReader.TryReadFloat(out num1))
          {
            float points = this.ConvertToPoints((double) num1);
            graphicsPath.AddLine(pointF1, new PointF(pointF1.X + points, pointF1.Y));
            pointF1 = new PointF(pointF1.X + points, pointF1.Y);
            continue;
          }
          continue;
        case 'v':
          if (oxpsPathDataReader.TryReadFloat(out num2))
          {
            float points = this.ConvertToPoints((double) num2);
            graphicsPath.AddLine(pointF1, new PointF(pointF1.X, pointF1.Y + points));
            pointF1 = new PointF(pointF1.X, pointF1.Y + points);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    if (graphicsPath.PathData.Points.LongLength <= 0L)
      return new PdfPath();
    PdfPath pathFromGeometry = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
    pathFromGeometry.FillMode = graphicsPath.FillMode == FillMode.Alternate ? PdfFillMode.Alternate : PdfFillMode.Winding;
    pathFromGeometry.CloseAllFigures();
    return pathFromGeometry;
  }

  private List<PointF> ComputeArc(
    PointF startPoint,
    PointF endPoint,
    float radiusX,
    float radiusY,
    double rotationAngle,
    bool isLargeArc,
    bool isCounterClockwise)
  {
    List<PointF> arc = new List<PointF>();
    arc.Add(startPoint);
    bool flag = false;
    Matrix matrix1 = new Matrix();
    double num1 = ((double) endPoint.X - (double) startPoint.X) / 2.0;
    double num2 = ((double) endPoint.Y - (double) startPoint.Y) / 2.0;
    PointF pointF1 = new PointF(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
    double num3 = Math.Sqrt((double) pointF1.X * (double) pointF1.X + (double) pointF1.Y * (double) pointF1.Y) / 2.0;
    rotationAngle = -rotationAngle * (Math.PI / 180.0);
    double num4 = Math.Cos(rotationAngle);
    double num5 = Math.Sin(rotationAngle);
    double num6 = num1 * num4 - num2 * num5;
    double num7 = num1 * num5 + num2 * num4;
    double num8 = num6 / (double) radiusX;
    double num9 = num7 / (double) radiusY;
    double d = num8 * num8 + num9 * num9;
    double num10;
    double num11;
    if (d > 1.0)
    {
      double num12 = Math.Sqrt(d);
      radiusX *= (float) num12;
      radiusY *= (float) num12;
      num11 = num10 = 0.0;
      flag = true;
      num8 /= num12;
      num9 /= num12;
    }
    else
    {
      double num13 = Math.Sqrt((1.0 - d) / d);
      if (isLargeArc == isCounterClockwise)
      {
        num11 = -num13 * num9;
        num10 = num13 * num8;
      }
      else
      {
        num11 = num13 * num9;
        num10 = -num13 * num8;
      }
    }
    PointF startPoint1 = new PointF((float) (-num8 - num11), (float) (-num9 - num10));
    PointF endPoint1 = new PointF((float) (num8 - num11), (float) (num9 - num10));
    double num14 = !flag ? num4 * (double) radiusX * num11 + num5 * (double) radiusX * num10 : 0.0;
    double num15 = !flag ? -num5 * (double) radiusY * num11 + num4 * (double) radiusY * num10 : 0.0;
    Matrix matrix2 = new Matrix((float) num4 * radiusX, (float) -num5 * radiusX, (float) num5 * radiusY, (float) num4 * radiusY, (float) num14 + (float) (((double) endPoint.X + (double) startPoint.X) / 2.0), (float) num15 + (float) (((double) endPoint.Y + (double) startPoint.Y) / 2.0));
    double cosArcAngle;
    double sinArcAngle;
    int lines;
    this.GetArcAngle(startPoint1, endPoint1, isLargeArc, isCounterClockwise, out cosArcAngle, out sinArcAngle, out lines);
    double num16 = this.GetBezierDistance(cosArcAngle, 1.0);
    if (isCounterClockwise)
      num16 = -num16;
    PointF pointF2 = new PointF((float) -num16 * startPoint1.Y, (float) num16 * startPoint1.X);
    PointF pointF3 = PointF.Empty;
    PointF pointF4 = PointF.Empty;
    PointF pointF5;
    for (int index = 1; index < lines; ++index)
    {
      PointF pointF6 = new PointF((float) ((double) startPoint1.X * cosArcAngle - (double) startPoint1.Y * sinArcAngle), (float) ((double) startPoint1.X * sinArcAngle + (double) startPoint1.Y * cosArcAngle));
      pointF5 = new PointF((float) -num16 * pointF6.Y, (float) num16 * pointF6.X);
      pointF3 = new PointF(startPoint1.X + pointF2.X, startPoint1.Y + pointF2.Y);
      pointF4 = new PointF(pointF6.X - pointF5.X, pointF6.Y - pointF5.Y);
      PointF[] pointFArray = new PointF[3]
      {
        pointF3,
        pointF4,
        pointF6
      };
      matrix2.TransformPoints(pointFArray);
      arc.AddRange((IEnumerable<PointF>) pointFArray);
      startPoint1 = pointF6;
      pointF2 = pointF5;
    }
    pointF5 = new PointF((float) -num16 * endPoint1.Y, (float) num16 * endPoint1.X);
    pointF3 = new PointF(startPoint1.X + pointF2.X, startPoint1.Y + pointF2.Y);
    pointF4 = new PointF(endPoint1.X - pointF5.X, endPoint1.Y - pointF5.Y);
    PointF[] pointFArray1 = new PointF[2]
    {
      pointF3,
      pointF4
    };
    matrix2.TransformPoints(pointFArray1);
    arc.AddRange((IEnumerable<PointF>) pointFArray1);
    arc.Add(new PointF(endPoint.X, endPoint.Y));
    return arc;
  }

  private void GetArcAngle(
    PointF startPoint,
    PointF endPoint,
    bool isLargeArc,
    bool isCounterClockwise,
    out double cosArcAngle,
    out double sinArcAngle,
    out int lines)
  {
    cosArcAngle = (double) startPoint.X * (double) endPoint.X + (double) startPoint.Y * (double) endPoint.Y;
    sinArcAngle = (double) startPoint.X * (double) endPoint.Y - (double) startPoint.Y * (double) endPoint.X;
    if (cosArcAngle >= 0.0)
    {
      if (isLargeArc)
      {
        lines = 4;
      }
      else
      {
        lines = 1;
        return;
      }
    }
    else
      lines = !isLargeArc ? 2 : 3;
    double num1 = Math.Atan2(sinArcAngle, cosArcAngle);
    if (!isCounterClockwise)
    {
      if (num1 < 0.0)
        num1 += 2.0 * Math.PI;
    }
    else if (num1 > 0.0)
      num1 -= 2.0 * Math.PI;
    double num2 = num1 / (double) lines;
    cosArcAngle = Math.Cos(num2);
    sinArcAngle = Math.Sin(num2);
  }

  private double GetBezierDistance(double dot, double radius)
  {
    double num1 = radius * radius;
    double bezierDistance = 0.0;
    double d1 = (num1 + dot) / 2.0;
    if (d1 < 0.0)
      return bezierDistance;
    double d2 = num1 - d1;
    if (d2 <= 0.0)
      return bezierDistance;
    double num2 = Math.Sqrt(d2);
    double num3 = Math.Sqrt(d1);
    return 4.0 * (radius - num3) / 3.0 > num2 * 1E-06 ? 4.0 * (radius - num3) / num2 / 3.0 : 0.0;
  }

  private void ThrowNotImplementedException()
  {
  }

  internal void DrawPath(OXPSPath path)
  {
    float[] numArray1 = (float[]) null;
    PdfPen pen = (PdfPen) null;
    PdfBrush brush1 = (PdfBrush) null;
    Image image1 = (Image) null;
    SizeF size1 = SizeF.Empty;
    PdfGraphicsState state1 = this.Graphics.Save();
    PdfPath path1 = this.GetPathFromGeometry(path.Data);
    if (path1 == null && path.PathData != null)
    {
      path1 = this.GetPathFromPathGeometry(path.PathData.PathGeometry);
      if (path.PathData.PathGeometry != null && path.PathData.PathGeometry.PathFigure != null)
      {
        if (path.PathData.PathGeometry.PathFigure.Length > 1)
        {
          string fill = path.Fill;
          foreach (OXPSPathFigure oxpsPathFigure in path.PathData.PathGeometry.PathFigure)
          {
            OXPSPath oxpsPath = new OXPSPath();
            OXPSPath path2 = path;
            path2.Fill = fill;
            path2.PathData.PathGeometry.PathFigure = new OXPSPathFigure[1];
            path2.PathData.PathGeometry.PathFigure[0] = oxpsPathFigure;
            this.DrawPath(path2);
          }
          return;
        }
        if (!path.PathData.PathGeometry.PathFigure[0].IsFilled)
          path.Fill = (string) null;
      }
    }
    if (path.RenderTransform != null)
    {
      string str = path.RenderTransform;
      if (str.Contains("StaticResource"))
        str = (this.ReadStaticResource(str) as OXPSMatrixTransform).Matrix;
      this.ApplyRenderTransform(str);
    }
    if (path.StrokeDashArray != null)
    {
      string[] val;
      new OXPSPathDataReader(path.StrokeDashArray).TryReadPositionArray(out val);
      numArray1 = new float[val.Length];
      for (int index = 0; index < val.Length; ++index)
        numArray1[index] = this.ConvertToPoints((double) OXPSRenderer.ParseFloat(val[index]));
    }
    if (path.Stroke != null)
    {
      if (path.Stroke.Contains("StaticResource"))
        path.PathStroke = this.ReadStaticResource(path.Stroke) as OXPSBrush;
      else if (path.StrokeThickness != 0.0)
      {
        pen = new PdfPen(new PdfColor(this.FromHtml(path.Stroke)), this.ConvertToPoints(path.StrokeThickness));
        pen.LineJoin = path.StrokeLineJoin == OXPSLineJoin.Bevel ? PdfLineJoin.Bevel : (path.StrokeLineJoin == OXPSLineJoin.Miter ? PdfLineJoin.Miter : PdfLineJoin.Round);
        if (numArray1 != null)
        {
          pen.DashStyle = PdfDashStyle.Dash;
          pen.DashPattern = numArray1;
        }
      }
    }
    PointF pointF;
    if (path.PathStroke != null)
    {
      if (path.PathStroke.Item is OXPSLinearGradientBrush)
        pen = new PdfPen((PdfBrush) this.ReadLinearGradientBrush(path.PathStroke.Item as OXPSLinearGradientBrush), (float) path.StrokeThickness);
      else if (path.PathStroke.Item is OXPSImageBrush)
      {
        OXPSImageBrush oxpsImageBrush = path.PathStroke.Item as OXPSImageBrush;
        if (!string.IsNullOrEmpty(oxpsImageBrush.Transform))
          this.ApplyRenderTransform(oxpsImageBrush.Transform);
        try
        {
          image1 = Image.FromStream(this.m_reader.ReadImage(oxpsImageBrush.ImageSource));
          SizeF size2 = new SizeF(this.ConvertToPoints((double) image1.Size.Width), this.ConvertToPoints((double) image1.Size.Height));
          if (path1 != null)
          {
            PointF point = path1.Points[1];
            double x1 = (double) point.X;
            point = path1.Points[0];
            double x2 = (double) point.X;
            double width = x1 - x2;
            point = path1.Points[2];
            double y1 = (double) point.Y;
            point = path1.Points[0];
            double y2 = (double) point.Y;
            double height = y1 - y2;
            size1 = new SizeF((float) width, (float) height);
          }
          PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(size2);
          if (oxpsImageBrush.TileMode != OXPSTileMode.None)
          {
            RectangleF rectangleF = RectangleF.Empty;
            RectangleF rectangle = RectangleF.Empty;
            PdfImage image2 = PdfImage.FromImage(image1);
            if (!string.IsNullOrEmpty(oxpsImageBrush.Viewport))
              rectangleF = this.StringToRectangleF(oxpsImageBrush.Viewport);
            if (!string.IsNullOrEmpty(oxpsImageBrush.Viewbox))
              rectangle = this.StringToRectangleF(oxpsImageBrush.Viewbox);
            if ((double) rectangleF.Width > 0.75 && (double) rectangleF.Height > 0.75)
            {
              PdfTilingBrush brush2 = new PdfTilingBrush(rectangle, this.m_page);
              brush2.Graphics.ScaleTransform(rectangleF.Width / rectangle.Width, rectangleF.Height / rectangle.Height);
              rectangleF.X -= rectangle.X;
              rectangleF.Y -= rectangle.Y;
              rectangleF.Width = (float) image2.Width;
              rectangleF.Height = (float) image2.Height;
              brush2.Graphics.DrawImage(image2, rectangle);
              this.Graphics.DrawRectangle(new PdfPen((PdfBrush) brush2, (float) path.StrokeThickness), new RectangleF(path1.Points[0], size1));
            }
            else
              this.Graphics.DrawImage(image2, new RectangleF(path1.Points[0], size1));
            image1 = (Image) null;
          }
        }
        catch (Exception ex)
        {
        }
      }
      else if (path.PathStroke.Item is OXPSSolidColorBrush)
        pen = new PdfPen((PdfBrush) new PdfSolidBrush((PdfColor) ColorTranslator.FromHtml((path.PathStroke.Item as OXPSSolidColorBrush).Color)), (float) path.StrokeThickness);
      else if (path.PathStroke.Item is OXPSRadialGradientBrush)
        pen = new PdfPen((PdfBrush) this.ReadRadialGradientBrush(path.PathStroke.Item as OXPSRadialGradientBrush), (float) path.StrokeThickness);
      else if (path.PathStroke.Item is OXPSVisualBrush)
      {
        OXPSVisualBrush xpsVisualBrush = path.PathStroke.Item as OXPSVisualBrush;
        if (xpsVisualBrush.VisualBrushVisual.Item is OXPSGlyphs)
        {
          OXPSGlyphs glyphs = xpsVisualBrush.VisualBrushVisual.Item as OXPSGlyphs;
          ref SizeF local = ref size1;
          double x3 = (double) path1.Points[1].X;
          pointF = path1.Points[0];
          double x4 = (double) pointF.X;
          double width = x3 - x4;
          pointF = path1.Points[2];
          double y3 = (double) pointF.Y;
          pointF = path1.Points[0];
          double y4 = (double) pointF.Y;
          double height = y3 - y4;
          local = new SizeF((float) width, (float) height);
          RectangleF rectangleF1 = this.StringToRectangleF(xpsVisualBrush.Viewport);
          RectangleF rectangleF2 = this.StringToRectangleF(xpsVisualBrush.Viewbox);
          PdfFont font1 = this.GetFont(glyphs);
          PdfTilingBrush brush3 = new PdfTilingBrush(new SizeF(rectangleF1.Width, rectangleF1.Height));
          PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) ColorTranslator.FromHtml(glyphs.Fill));
          brush3.Graphics.ScaleTransform(rectangleF1.Width / rectangleF2.Width, rectangleF1.Height / rectangleF2.Height);
          PdfGraphics graphics = brush3.Graphics;
          string unicodeString = glyphs.UnicodeString;
          PdfFont font2 = font1;
          PdfBrush brush4 = pdfBrush;
          pointF = new PointF();
          PointF point = pointF;
          graphics.DrawString(unicodeString, font2, brush4, point);
          this.Graphics.DrawRectangle(new PdfPen((PdfBrush) brush3, (float) path.StrokeThickness), new RectangleF(path1.Points[0], size1));
        }
        else
          this.ReadVisualBrush(xpsVisualBrush);
      }
    }
    if (path.Fill != null)
    {
      if (path.Fill.Contains("StaticResource"))
        path.PathFill = this.ReadStaticResource(path.Fill) as OXPSBrush;
      else if (path.Fill.Contains("icc"))
      {
        brush1 = (PdfBrush) null;
      }
      else
      {
        Color color = this.FromHtml(path.Fill);
        if (path.Fill.Contains("sc#"))
        {
          double alpha = color.A < byte.MaxValue ? (double) color.A / 256.0 : 1.0;
          if (alpha < 1.0)
            this.Graphics.SetTransparency((float) alpha);
        }
        if (color.A != (byte) 0)
          brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        if (color.A < byte.MaxValue && color.A > (byte) 0)
          this.Graphics.SetTransparency((float) color.A / (float) byte.MaxValue);
      }
    }
    if (path.PathFill != null)
    {
      if (path.PathFill.Item is OXPSImageBrush)
      {
        OXPSImageBrush oxpsImageBrush = path.PathFill.Item as OXPSImageBrush;
        ++this.count;
        if (!string.IsNullOrEmpty(oxpsImageBrush.Transform))
          this.ApplyRenderTransform(oxpsImageBrush.Transform);
        try
        {
          image1 = Image.FromStream(this.m_reader.ReadImage(oxpsImageBrush.ImageSource));
          SizeF size3 = new SizeF(this.ConvertToPoints((double) image1.Size.Width), this.ConvertToPoints((double) image1.Size.Height));
          if (path1 != null)
          {
            pointF = path1.Points[1];
            double x5 = (double) pointF.X;
            pointF = path1.Points[0];
            double x6 = (double) pointF.X;
            double width = x5 - x6;
            pointF = path1.Points[2];
            double y5 = (double) pointF.Y;
            pointF = path1.Points[0];
            double y6 = (double) pointF.Y;
            double height = y5 - y6;
            size1 = new SizeF((float) width, (float) height);
          }
          PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(size3);
          if (oxpsImageBrush.TileMode != OXPSTileMode.None)
          {
            RectangleF rectangle = RectangleF.Empty;
            RectangleF rectangleF = RectangleF.Empty;
            PdfImage image3 = PdfImage.FromImage(image1);
            if (!string.IsNullOrEmpty(oxpsImageBrush.Viewport))
              rectangle = this.StringToRectangleF(oxpsImageBrush.Viewport);
            if (!string.IsNullOrEmpty(oxpsImageBrush.Viewbox))
              rectangleF = this.StringToRectangleF(oxpsImageBrush.Viewbox);
            if ((double) this.Graphics.Matrix.Matrix.Elements[0] < 1.0)
              rectangle.Size = new SizeF(rectangle.Width * this.Graphics.Matrix.Matrix.Elements[0], rectangle.Height * this.Graphics.Matrix.Matrix.Elements[3]);
            if ((double) rectangle.Width > 0.75 && (double) rectangle.Height > 0.75)
            {
              PdfTilingBrush brush5;
              if (oxpsImageBrush.ImageBrushTransform != null)
              {
                float[] numArray2 = new float[6];
                string[] strArray = oxpsImageBrush.ImageBrushTransform.MatrixTransform.Matrix.Split(this.m_commaSeparator);
                for (int index = 0; index < numArray2.Length; ++index)
                  numArray2[index] = OXPSRenderer.ParseFloat(strArray[index]);
                if ((double) numArray2[0] > 0.0 && (double) numArray2[3] > 0.0)
                {
                  rectangle.Size = new SizeF(rectangle.Width * this.Graphics.Matrix.Matrix.Elements[0], rectangle.Height * this.Graphics.Matrix.Matrix.Elements[3]);
                  rectangleF.Size = new SizeF(rectangleF.Width * this.Graphics.Matrix.Matrix.Elements[0], rectangleF.Height * this.Graphics.Matrix.Matrix.Elements[3]);
                  brush5 = new PdfTilingBrush(rectangle, this.m_page);
                  Matrix matrix = new Matrix(numArray2[0], numArray2[1], numArray2[2], numArray2[3], numArray2[4], numArray2[5]);
                  brush5.TransformationMatrix = this.PrepareMatrix(matrix);
                }
                else
                  brush5 = new PdfTilingBrush(rectangle, this.m_page);
              }
              else
                brush5 = new PdfTilingBrush(rectangle, this.m_page);
              brush5.Graphics.ScaleTransform(rectangle.Width / rectangleF.Width, rectangle.Height / rectangleF.Height);
              rectangle.X -= rectangleF.X;
              rectangle.Y -= rectangleF.Y;
              rectangle.Width = (float) image3.Width;
              rectangle.Height = (float) image3.Height;
              brush5.Graphics.DrawImage(image3, rectangle);
              this.Graphics.DrawRectangle((PdfBrush) brush5, new RectangleF(path1.Points[0], size1));
            }
            else
              this.Graphics.DrawImage(image3, new RectangleF(path1.Points[0], size1));
            image1 = (Image) null;
          }
        }
        catch (Exception ex)
        {
        }
      }
      else if (path.PathFill.Item is OXPSSolidColorBrush)
        brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) ColorTranslator.FromHtml((path.PathFill.Item as SolidColorBrush).Color));
      else if (path.PathFill.Item is OXPSLinearGradientBrush)
        brush1 = (PdfBrush) this.ReadLinearGradientBrush(path.PathFill.Item as OXPSLinearGradientBrush);
      else if (path.PathFill.Item is OXPSRadialGradientBrush)
        brush1 = (PdfBrush) this.ReadRadialGradientBrush(path.PathFill.Item as OXPSRadialGradientBrush);
      else if (path.PathFill.Item is OXPSVisualBrush)
      {
        OXPSVisualBrush oxpsVisualBrush = path.PathFill.Item as OXPSVisualBrush;
        PdfGraphicsState state2 = this.Graphics.Save();
        RectangleF rectangleF3 = RectangleF.Empty;
        RectangleF rectangleF4 = RectangleF.Empty;
        if (!string.IsNullOrEmpty(oxpsVisualBrush.Viewport))
          rectangleF3 = this.StringToRectangleF(oxpsVisualBrush.Viewport);
        if (!string.IsNullOrEmpty(oxpsVisualBrush.Viewbox))
          rectangleF4 = this.StringToRectangleF(oxpsVisualBrush.Viewbox);
        this.Graphics.ScaleTransform(rectangleF3.Width / rectangleF4.Width, rectangleF3.Height / rectangleF4.Height);
        rectangleF3.X -= rectangleF4.X;
        rectangleF3.Y -= rectangleF4.Y;
        this.Graphics.TranslateTransform(rectangleF3.X, rectangleF3.Y);
        this.ReadVisualBrush(path.PathFill.Item as OXPSVisualBrush);
        this.Graphics.Restore(state2);
      }
    }
    if (path.Opacity < 1.0)
      this.Graphics.SetTransparency((float) path.Opacity);
    if (path.Clip != null)
      this.Graphics.SetClip(this.GetPathFromGeometry(path.Clip));
    int strokeLineJoin = (int) path.StrokeLineJoin;
    if (pen != null)
      pen.LineJoin = (PdfLineJoin) Enum.Parse(typeof (PdfLineJoin), Enum.GetName(typeof (OXPSLineJoin), (object) path.StrokeLineJoin));
    int strokeStartLineCap = (int) path.StrokeStartLineCap;
    if (pen != null && path.StrokeStartLineCap != OXPSLineCap.Triangle)
      pen.LineCap = (PdfLineCap) Enum.Parse(typeof (PdfLineCap), Enum.GetName(typeof (OXPSLineCap), (object) path.StrokeStartLineCap));
    if (path.FixedPageNavigateUri != null && this.currentMatrix != null)
    {
      PdfUriAnnotation annotation;
      if (path1 != null)
      {
        ref SizeF local = ref size1;
        pointF = path1.Points[2];
        double x7 = (double) pointF.X;
        pointF = path1.Points[0];
        double x8 = (double) pointF.X;
        double width = (x7 - x8) * (double) this.currentMatrix.Matrix.Elements[0];
        pointF = path1.Points[2];
        double y7 = (double) pointF.Y;
        pointF = path1.Points[0];
        double y8 = (double) pointF.Y;
        double height = (y7 - y8) * (double) this.currentMatrix.Matrix.Elements[3];
        local = new SizeF((float) width, (float) height);
        pointF = path1.Points[0];
        double x9 = (double) pointF.X * (double) this.currentMatrix.Matrix.Elements[0];
        pointF = path1.Points[0];
        double y9 = (double) pointF.Y * (double) this.currentMatrix.Matrix.Elements[3];
        annotation = new PdfUriAnnotation(new RectangleF(new PointF((float) x9, (float) y9), size1));
      }
      else
        annotation = new PdfUriAnnotation(new RectangleF());
      annotation.Uri = path.FixedPageNavigateUri;
      annotation.Color = new PdfColor(Color.Transparent);
      this.m_page.Annotations.Add((PdfAnnotation) annotation);
    }
    else if (path.PathFill != null && path.PathFill.Item is OXPSImageBrush && image1 != null && path1.PointCount > 0)
    {
      ImageBrush imageBrush = path.PathFill.Item as ImageBrush;
      float num = 0.0f;
      if (imageBrush.ImageBrushTransform != null)
        num = OXPSRenderer.ParseFloat(imageBrush.ImageBrushTransform.MatrixTransform.Matrix.Split(this.m_commaSeparator)[3]);
      if ((double) size1.Height < 0.0 && (double) num >= 0.0)
      {
        size1.Height = -size1.Height;
        PdfGraphics graphics = this.Graphics;
        PdfImage image4 = PdfImage.FromImage(image1);
        pointF = path1.Points[0];
        double x = (double) pointF.X;
        pointF = path1.Points[0];
        double y = (double) pointF.Y - (double) size1.Height;
        RectangleF rectangle = new RectangleF(new PointF((float) x, (float) y), size1);
        graphics.DrawImage(image4, rectangle);
      }
      else
      {
        PdfGraphics graphics = this.Graphics;
        PdfImage image5 = PdfImage.FromImage(image1);
        pointF = path1.Points[0];
        double x = (double) pointF.X;
        pointF = path1.Points[0];
        double y = (double) pointF.Y;
        RectangleF rectangle = new RectangleF(new PointF((float) x, (float) y), size1);
        graphics.DrawImage(image5, rectangle);
      }
      if (pen != null)
        this.Graphics.DrawPath(pen, path1);
    }
    else
      this.Graphics.DrawPath(pen, brush1, path1);
    this.Graphics.Restore(state1);
    image1?.Dispose();
  }

  private void ReadVisualBrush(OXPSVisualBrush xpsVisualBrush)
  {
    if (xpsVisualBrush.Key != null && xpsVisualBrush.Key == "EmptyBrush")
      return;
    if (xpsVisualBrush.Transform != null)
      this.ApplyRenderTransform(xpsVisualBrush.Transform);
    object obj = (object) null;
    if (xpsVisualBrush.VisualBrushVisual != null)
      obj = xpsVisualBrush.VisualBrushVisual.Item;
    else if (!string.IsNullOrEmpty(xpsVisualBrush.Visual) && xpsVisualBrush.Visual.Contains("StaticResource"))
      obj = this.ReadStaticResource(xpsVisualBrush.Visual);
    switch (obj)
    {
      case OXPSCanvas _:
        this.DrawCanvas(obj as OXPSCanvas);
        break;
      case OXPSGlyphs _:
        this.DrawGlyphs(obj as OXPSGlyphs);
        break;
      case OXPSPath _:
        this.DrawPath(obj as OXPSPath);
        break;
    }
  }

  private PdfLinearGradientBrush ReadLinearGradientBrush(OXPSLinearGradientBrush gradientBrush)
  {
    PointF val;
    new OXPSPathDataReader(gradientBrush.StartPoint).TryReadPoint(out val);
    PointF point1 = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    new OXPSPathDataReader(gradientBrush.EndPoint).TryReadPoint(out val);
    PointF point2 = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    List<object> offSet;
    List<PdfColor> colors;
    this.PreProcessGradientStops(gradientBrush.LinearGradientBrushGradientStops, out offSet, out colors);
    float[] numArray = new float[offSet.Count];
    int index = 0;
    foreach (object obj in offSet)
    {
      numArray[index] = OXPSRenderer.ParseFloat(obj.ToString());
      ++index;
    }
    PdfLinearGradientBrush brush = new PdfLinearGradientBrush(point1, point2, colors[0], colors[colors.Count - 1]);
    if (gradientBrush.SpreadMethod == OXPSSpreadMethod.Pad)
      brush.Extend = PdfExtend.Both;
    else if (gradientBrush.SpreadMethod == OXPSSpreadMethod.Repeat)
      brush.Extend = PdfExtend.None;
    else if (gradientBrush.SpreadMethod == OXPSSpreadMethod.Reflect)
      brush.Extend = PdfExtend.None;
    PdfColorBlend pdfColorBlend = new PdfColorBlend((PdfBrush) brush);
    pdfColorBlend.Positions = numArray;
    pdfColorBlend.Colors = colors.ToArray();
    brush.InterpolationColors = pdfColorBlend;
    return brush;
  }

  private PdfRadialGradientBrush ReadRadialGradientBrush(OXPSRadialGradientBrush xpsRadialBrush)
  {
    float radiusX = (float) xpsRadialBrush.RadiusX;
    float radiusY = (float) xpsRadialBrush.RadiusY;
    PointF val;
    new OXPSPathDataReader(xpsRadialBrush.GradientOrigin).TryReadPoint(out val);
    PointF centreEnd = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    new OXPSPathDataReader(xpsRadialBrush.Center).TryReadPoint(out val);
    PointF centreStart = new PointF(this.ConvertToPoints((double) val.X), this.ConvertToPoints((double) val.Y));
    List<object> offSet;
    List<PdfColor> colors;
    this.PreProcessGradientStops(xpsRadialBrush.RadialGradientBrushGradientStops, out offSet, out colors);
    float[] numArray = new float[offSet.Count];
    int index = 0;
    foreach (object obj in offSet)
    {
      numArray[index] = OXPSRenderer.ParseFloat(obj.ToString());
      ++index;
    }
    PdfRadialGradientBrush brush = new PdfRadialGradientBrush(centreStart, radiusX, centreEnd, radiusY, colors[0], colors[colors.Count - 1]);
    if (xpsRadialBrush.SpreadMethod == OXPSSpreadMethod.Pad)
      brush.Extend = PdfExtend.Both;
    else if (xpsRadialBrush.SpreadMethod == OXPSSpreadMethod.Repeat)
      brush.Extend = PdfExtend.None;
    else if (xpsRadialBrush.SpreadMethod == OXPSSpreadMethod.Reflect)
      brush.Extend = PdfExtend.None;
    PdfColorBlend pdfColorBlend = new PdfColorBlend((PdfBrush) brush);
    pdfColorBlend.Positions = numArray;
    pdfColorBlend.Colors = colors.ToArray();
    brush.InterpolationColors = pdfColorBlend;
    return brush;
  }

  private PdfPath GetPathFromPathGeometry(OXPSPathGeometry xpsPathGeometry)
  {
    PdfPath fromPathGeometry = (PdfPath) null;
    if (xpsPathGeometry == null)
      return fromPathGeometry;
    if (xpsPathGeometry.Figures != null)
      fromPathGeometry = this.GetPathFromGeometry(xpsPathGeometry.Figures);
    else if (xpsPathGeometry.PathFigure != null)
    {
      OXPSPathFigure[] pathFigure = xpsPathGeometry.PathFigure;
      GraphicsPath graphicsPath = new GraphicsPath();
      for (int index = 0; index < pathFigure.Length; ++index)
      {
        OXPSPathFigure oxpsPathFigure = pathFigure[index];
        PointF pt1 = this.StringToPointF(oxpsPathFigure.StartPoint);
        PointF pointF1 = pt1;
        foreach (object obj in oxpsPathFigure.Items)
        {
          switch (obj)
          {
            case OXPSPolyLineSegment _:
              string points = (obj as OXPSPolyLineSegment).Points;
              char[] chArray = new char[1]{ ' ' };
              foreach (string point in points.Split(chArray))
              {
                PointF pointF2 = this.StringToPointF(point);
                graphicsPath.AddLine(pointF1, pointF2);
                pointF1 = pointF2;
              }
              break;
            case OXPSArcSegment _:
              OXPSArcSegment oxpsArcSegment = obj as OXPSArcSegment;
              PointF pointF3 = this.StringToPointF(oxpsArcSegment.Point);
              PointF pointF4 = this.StringToPointF(oxpsArcSegment.Size);
              bool isCounterClockwise = false;
              if (oxpsArcSegment.SweepDirection == OXPSSweepDirection.Clockwise)
                isCounterClockwise = false;
              else if (oxpsArcSegment.SweepDirection == OXPSSweepDirection.Counterclockwise)
                isCounterClockwise = true;
              List<PointF> arc = this.ComputeArc(pointF1, pointF3, pointF4.X, pointF4.Y, oxpsArcSegment.RotationAngle, oxpsArcSegment.IsLargeArc, isCounterClockwise);
              if (arc.Count == 0)
              {
                if (oxpsArcSegment.IsStroked)
                  graphicsPath.AddLine(pt1, pointF3);
                pt1 = pointF3;
                break;
              }
              if (arc.Count > 0)
              {
                if (oxpsArcSegment.IsStroked)
                  graphicsPath.AddBeziers(arc.ToArray());
                pointF1 = arc[arc.Count - 1];
                arc.Clear();
                break;
              }
              break;
            case OXPSPolyBezierSegment _:
              OXPSPolyBezierSegment polyBezierSegment = obj as OXPSPolyBezierSegment;
              List<PointF> pointFList = new List<PointF>();
              string[] strArray = polyBezierSegment.Points.Split(' ');
              if (pointF1 != PointF.Empty)
                pointFList.Add(pointF1);
              else
                pointFList.Add(pt1);
              foreach (string point in strArray)
                pointFList.Add(this.StringToPointF(point));
              if (polyBezierSegment.IsStroked)
                graphicsPath.AddBeziers(pointFList.ToArray());
              pointF1 = pointFList[pointFList.Count - 1];
              pointFList.Clear();
              break;
          }
        }
        if (oxpsPathFigure.IsClosed)
        {
          graphicsPath.CloseFigure();
          graphicsPath.StartFigure();
          PointF empty = PointF.Empty;
        }
      }
      if (graphicsPath.PointCount > 0)
      {
        fromPathGeometry = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
        fromPathGeometry.FillMode = graphicsPath.FillMode == FillMode.Alternate ? PdfFillMode.Alternate : PdfFillMode.Winding;
        fromPathGeometry.CloseAllFigures();
      }
      else if (fromPathGeometry == null)
        fromPathGeometry = new PdfPath();
    }
    return fromPathGeometry;
  }

  private PointF StringToPointF(string point)
  {
    string[] strArray = point.Split(',');
    return this.ConvertToPoints(new PointF(OXPSRenderer.ParseFloat(strArray[0]), OXPSRenderer.ParseFloat(strArray[1])));
  }

  private RectangleF StringToRectangleF(string rect)
  {
    string[] strArray = rect.Split(',');
    PointF point1 = new PointF(OXPSRenderer.ParseFloat(strArray[0]), OXPSRenderer.ParseFloat(strArray[1]));
    PointF point2 = new PointF(OXPSRenderer.ParseFloat(strArray[2]), OXPSRenderer.ParseFloat(strArray[3]));
    PointF points1 = this.ConvertToPoints(point1);
    PointF points2 = this.ConvertToPoints(point2);
    SizeF size = new SizeF(points2.X, points2.Y);
    return new RectangleF(points1, size);
  }

  private void PreProcessGradientStops(
    OXPSGradientStop[] stops,
    out List<object> offSet,
    out List<PdfColor> colors)
  {
    offSet = new List<object>();
    colors = new List<PdfColor>();
    PdfColor empty = PdfColor.Empty;
    foreach (OXPSGradientStop stop in stops)
    {
      float offset = (float) stop.Offset;
      PdfColor pdfColor = new PdfColor(this.FromHtml(stop.Color));
      int num1 = offSet.IndexOf((object) offset);
      int num2 = offSet.LastIndexOf((object) offset);
      if (num1 == -1 && num1 == num2)
      {
        offSet.Add((object) offset);
        offSet.Sort();
        int index = offSet.IndexOf((object) offset);
        colors.Insert(index, pdfColor);
      }
      else if (num1 > -1 && num1 == num2)
      {
        offSet.Add((object) offset);
        offSet.Sort();
        int index = offSet.LastIndexOf((object) offset);
        colors.Insert(index, pdfColor);
      }
      else if (num1 > -1 && num1 != num2)
      {
        int index = offSet.LastIndexOf((object) offset);
        offSet.RemoveAt(index);
        colors.RemoveAt(index);
        offSet.Insert(index, (object) offset);
        colors.Insert(index, pdfColor);
      }
    }
    if (!offSet.Contains((object) 0.0f))
    {
      offSet.FindLast(new Predicate<object>(OXPSRenderer.LowerLeastGradient));
      offSet.Find(new Predicate<object>(OXPSRenderer.LowerMostGradient));
      if ((double) (float) offSet[0] > 0.0)
      {
        offSet.Insert(0, (object) 0.0f);
        PdfColor pdfColor = colors[0];
        colors.Insert(0, pdfColor);
      }
      object last1 = offSet.FindLast(new Predicate<object>(OXPSRenderer.LowerLeastGradient));
      object obj1 = offSet.Find(new Predicate<object>(OXPSRenderer.LowerMostGradient));
      if (last1 != null && obj1 != null)
      {
        PdfColor color1 = colors[offSet.IndexOf(last1)];
        PdfColor color2 = colors[offSet.IndexOf(obj1)];
        PdfColor pdfColor1 = colors[offSet.IndexOf(last1)];
        PdfColor pdfColor2 = PdfBlendBase.Interpolate(0.5, color1, color2, PdfColorSpace.RGB);
        for (; last1 != null; last1 = offSet.Find(new Predicate<object>(OXPSRenderer.LowerLeastGradient)))
        {
          int index = offSet.IndexOf(last1);
          offSet.RemoveAt(index);
          colors.RemoveAt(index);
        }
        offSet.Insert(0, (object) 0.0f);
        colors.Insert(0, pdfColor2);
      }
      object last2 = offSet.FindLast(new Predicate<object>(OXPSRenderer.LowerLeastGradient));
      object obj2 = offSet.Find(new Predicate<object>(OXPSRenderer.LowerMostGradient));
      if (last2 != null && obj2 == null)
      {
        PdfColor pdfColor = colors[colors.Count - 1];
        for (; last2 != null; last2 = offSet.Find(new Predicate<object>(OXPSRenderer.LowerLeastGradient)))
        {
          int index = offSet.IndexOf(last2);
          offSet.RemoveAt(index);
          colors.RemoveAt(index);
        }
        offSet.Insert(0, (object) 0.0f);
        colors.Insert(0, pdfColor);
      }
    }
    if (offSet.Contains((object) 1f))
      return;
    offSet.FindLast(new Predicate<object>(OXPSRenderer.HigherLeastGradient));
    offSet.Find(new Predicate<object>(OXPSRenderer.HigherMostGradient));
    if ((double) (float) offSet[offSet.Count - 1] < 1.0)
    {
      int count = offSet.Count;
      offSet.Add((object) 1f);
      PdfColor pdfColor = colors[count - 1];
      colors.Add(pdfColor);
    }
    object last3 = offSet.FindLast(new Predicate<object>(OXPSRenderer.HigherLeastGradient));
    object obj3 = offSet.Find(new Predicate<object>(OXPSRenderer.HigherMostGradient));
    if (obj3 != null && last3 != null)
    {
      PdfColor color1 = colors[offSet.IndexOf(last3)];
      PdfColor color2 = colors[offSet.IndexOf(obj3)];
      PdfColor pdfColor3 = colors[offSet.IndexOf(obj3)];
      PdfColor pdfColor4 = PdfBlendBase.Interpolate(0.5, color1, color2, PdfColorSpace.RGB);
      for (; obj3 != null; obj3 = offSet.Find(new Predicate<object>(OXPSRenderer.HigherMostGradient)))
      {
        int index = offSet.IndexOf(obj3);
        offSet.RemoveAt(index);
        colors.RemoveAt(index);
      }
      offSet.Add((object) 1f);
      colors.Add(pdfColor4);
    }
    object last4 = offSet.FindLast(new Predicate<object>(OXPSRenderer.HigherLeastGradient));
    object obj4 = offSet.Find(new Predicate<object>(OXPSRenderer.HigherMostGradient));
    if (obj4 == null || last4 != null)
      return;
    PdfColor pdfColor5 = colors[0];
    for (; obj4 != null; obj4 = offSet.Find(new Predicate<object>(OXPSRenderer.HigherMostGradient)))
    {
      int index = offSet.IndexOf(obj4);
      offSet.RemoveAt(index);
      colors.RemoveAt(index);
    }
    offSet.Add((object) 1f);
    colors.Add(pdfColor5);
  }

  private static bool LowerLeastGradient(object p) => (double) (float) p < 0.0;

  private static bool LowerMostGradient(object p) => (double) (float) p > 0.0;

  private static bool HigherLeastGradient(object p) => (double) (float) p < 1.0;

  private static bool HigherMostGradient(object p) => (double) (float) p > 1.0;

  private object ReadStaticResource(string resourceName)
  {
    resourceName = resourceName.Trim('{', '}').Replace("StaticResource ", string.Empty);
    object obj = (object) null;
    int index = this.m_page.Section.Parent.IndexOf(this.m_page.Section);
    object[] collection = (object[]) null;
    if (this.m_canvas != null)
    {
      collection = this.GetResourceCollection(this.m_canvas.CanvasResources);
      obj = this.GetResource(collection, resourceName);
    }
    if (this.m_canvas != null && obj == null)
    {
      OXPSCanvas oxpsCanvas = this.m_canvas;
      while (oxpsCanvas.m_parent != null)
      {
        oxpsCanvas = oxpsCanvas.m_parent as OXPSCanvas;
        collection = this.GetResourceCollection(oxpsCanvas.CanvasResources);
        obj = this.GetResource(collection, resourceName);
        if (obj != null)
          break;
      }
    }
    if (collection == null && this.m_reader.Pages[index].FixedPageResources != null)
      obj = this.GetResource(this.GetResourceCollection(this.m_reader.Pages[index].FixedPageResources), resourceName);
    return obj;
  }

  private object[] GetResourceCollection(OXPSResources resources)
  {
    if (resources != null)
    {
      OXPSResourceDictionary resourceDictionary1 = resources.ResourceDictionary;
      if (resourceDictionary1 != null && resourceDictionary1.Items == null && resourceDictionary1.Source != null && resourceDictionary1.Source != string.Empty)
      {
        OXPSResourceDictionary resourceDictionary2 = (OXPSResourceDictionary) new XmlSerializer(typeof (OXPSResourceDictionary)).Deserialize((TextReader) new StreamReader(this.m_reader.ReadResource(resourceDictionary1.Source)));
        resources.ResourceDictionary = resourceDictionary2;
      }
    }
    return resources == null || resources.ResourceDictionary == null ? (object[]) null : resources.ResourceDictionary.Items;
  }

  private object GetResource(object[] collection, string resourceName)
  {
    object resource1 = (object) null;
    OXPSBrush resource2 = new OXPSBrush();
    OXPSPathGeometry oxpsPathGeometry = new OXPSPathGeometry();
    if (collection != null && collection.Length > 0)
    {
      for (int index = 0; index < collection.Length; ++index)
      {
        resource1 = collection[index];
        switch (resource1)
        {
          case OXPSImageBrush _ when (resource1 as OXPSImageBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case OXPSVisualBrush _ when (resource1 as OXPSVisualBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case OXPSLinearGradientBrush _ when (resource1 as OXPSLinearGradientBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case OXPSRadialGradientBrush _ when (resource1 as OXPSRadialGradientBrush).Key == resourceName:
            resource2.Item = resource1;
            return (object) resource2;
          case OXPSPathGeometry _ when (resource1 as OXPSPathGeometry).Key == resourceName:
            return (object) (resource1 as OXPSPathGeometry);
          case OXPSMatrixTransform _ when (resource1 as OXPSMatrixTransform).Key == resourceName:
            return (object) (resource1 as OXPSMatrixTransform);
          case OXPSVisualBrush _ when (resource1 as OXPSVisualBrush).Key == resourceName:
            return (object) (resource1 as OXPSVisualBrush);
          default:
            continue;
        }
      }
    }
    return resource1;
  }

  internal void DrawCanvas(OXPSCanvas canvas)
  {
    PdfGraphicsState state = this.Graphics.Save();
    this.InitializeCanvas(canvas);
    if (this.m_canvas != null)
      canvas.m_parent = (object) this.m_canvas;
    this.m_canvas = canvas;
    if (canvas.Opacity < 1.0)
      this.Graphics.SetTransparency((float) canvas.Opacity);
    if (canvas.Items != null)
    {
      float opacity = (float) canvas.Opacity;
      foreach (object obj in canvas.Items)
      {
        switch (obj)
        {
          case OXPSCanvas _:
            this.list.Add(opacity);
            this.DrawCanvas((OXPSCanvas) obj);
            this.list.Remove(opacity);
            break;
          case OXPSPath _:
            float num1 = 1f;
            for (int index = 0; index < this.list.Count; ++index)
              num1 = this.list[index] * num1;
            this.m_canvas.Opacity = (double) num1 * (double) opacity;
            this.Graphics.SetTransparency((float) canvas.Opacity);
            this.DrawPath((OXPSPath) obj);
            break;
          case OXPSGlyphs _:
            float num2 = 1f;
            for (int index = 0; index < this.list.Count; ++index)
              num2 = this.list[index] * num2;
            this.m_canvas.Opacity = (double) num2 * (double) opacity;
            this.Graphics.SetTransparency((float) canvas.Opacity);
            this.DrawGlyphs((OXPSGlyphs) obj);
            break;
          default:
            throw new NotImplementedException(obj.GetType().ToString());
        }
      }
    }
    this.m_canvas = (OXPSCanvas) this.m_canvas.m_parent;
    this.Graphics.Restore(state);
  }

  internal void InitializeCanvas(OXPSCanvas canvas)
  {
    if (canvas == null)
      throw new ArgumentNullException("Canvas");
    if (canvas.CanvasRenderTransform != null)
      this.ApplyRenderTransform(canvas.CanvasRenderTransform.MatrixTransform.Matrix);
    if (canvas.RenderTransform != null)
      this.ApplyRenderTransform(canvas.RenderTransform);
    if (canvas.Clip == null)
      return;
    this.Graphics.SetClip(this.GetPathFromGeometry(canvas.Clip));
  }

  internal void ApplyRenderTransform(string data, PdfGraphics graphics)
  {
    this.Graphics.MultiplyTransform(this.PrepareMatrix(this.ReadMatrix(data)));
  }

  internal void ApplyRenderTransform(string data)
  {
    PdfTransformationMatrix matrix = this.PrepareMatrix(!data.Contains("StaticResource") ? this.ReadMatrix(data) : this.ReadMatrix((this.ReadStaticResource(data) as OXPSMatrixTransform).Matrix));
    this.Graphics.Matrix.Multiply(matrix);
    this.Graphics.MultiplyTransform(matrix);
    this.currentMatrix = matrix;
  }

  private PdfTransformationMatrix PrepareMatrix(Matrix matrix)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    PdfTransformationMatrix matrix1 = new PdfTransformationMatrix();
    matrix1.Matrix = matrix;
    transformationMatrix.Scale(1f, -1f);
    transformationMatrix.Multiply(matrix1);
    transformationMatrix.Scale(1f, -1f);
    return transformationMatrix;
  }

  private Matrix ReadMatrix(string data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    float[] numArray = new float[6];
    string[] strArray = data.Split(this.m_commaSeparator);
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = OXPSRenderer.ParseFloat(strArray[index]);
    return new Matrix(numArray[0], numArray[1], numArray[2], numArray[3], this.ConvertToPoints((double) numArray[4]), this.ConvertToPoints((double) numArray[5]));
  }

  private PdfFont GetFont(OXPSGlyphs glyphs)
  {
    PdfTrueTypeFont font1 = (PdfTrueTypeFont) null;
    float points = this.ConvertToPoints(glyphs.FontRenderingEmSize);
    int deviceFontStyle = (int) this.GetDeviceFontStyle(glyphs.StyleSimulations);
    if (glyphs.UnicodeString != null)
      PdfString.IsUnicode(glyphs.UnicodeString);
    List<FontFamily> fontFamilyList = new List<FontFamily>((IEnumerable<FontFamily>) FontFamily.Families);
    if (glyphs.FontUri != null)
    {
      if (this.m_reader.Fonts.ContainsKey(glyphs.FontUri))
      {
        font1 = new PdfTrueTypeFont(this.m_reader.Fonts[glyphs.FontUri], this.m_embedFont.EmbedCompleteFont, points);
      }
      else
      {
        Stream stream = this.m_reader.ReadFont(glyphs.FontUri);
        stream.Position = 0L;
        TtfReader ttfReader = new TtfReader(new BinaryReader(stream));
        stream.Position = 0L;
        PdfTrueTypeFont font2 = new PdfTrueTypeFont(stream, points, this.m_embedFont.EmbedCompleteFont);
        this.m_reader.Fonts.Add(glyphs.FontUri, font2);
        return (PdfFont) font2;
      }
    }
    else if (glyphs.DeviceFontName != null)
      font1 = new PdfTrueTypeFont(new Font(glyphs.DeviceFontName, points, this.GetDeviceFontStyle(glyphs.StyleSimulations)), this.m_embedFont.EmbedCompleteFont);
    return (PdfFont) font1;
  }

  private PdfBrush GetSolidBrush(string color)
  {
    if (color == null)
      return (PdfBrush) null;
    if (!color.Contains("StaticResource"))
      return (PdfBrush) new PdfSolidBrush(new PdfColor(ColorTranslator.FromHtml(color)));
    PdfBrush solidBrush = PdfBrushes.Transparent;
    OXPSBrush oxpsBrush = this.ReadStaticResource(color) as OXPSBrush;
    if (oxpsBrush.Item is OXPSSolidColorBrush)
      solidBrush = (PdfBrush) new PdfSolidBrush((PdfColor) this.FromHtml((oxpsBrush.Item as OXPSSolidColorBrush).Color));
    else if (oxpsBrush.Item is OXPSLinearGradientBrush)
      solidBrush = (PdfBrush) this.ReadLinearGradientBrush(oxpsBrush.Item as OXPSLinearGradientBrush);
    else if (oxpsBrush.Item is OXPSRadialGradientBrush)
      solidBrush = (PdfBrush) this.ReadRadialGradientBrush(oxpsBrush.Item as OXPSRadialGradientBrush);
    return solidBrush;
  }

  private FontStyle GetDeviceFontStyle(OXPSStyleSimulations style)
  {
    FontStyle deviceFontStyle = FontStyle.Regular;
    switch (style)
    {
      case OXPSStyleSimulations.ItalicSimulation:
        deviceFontStyle = FontStyle.Italic;
        break;
      case OXPSStyleSimulations.BoldSimulation:
        deviceFontStyle = FontStyle.Bold;
        break;
      case OXPSStyleSimulations.BoldItalicSimulation:
        deviceFontStyle = FontStyle.Bold | FontStyle.Italic;
        break;
    }
    return deviceFontStyle;
  }

  private FontStyle GetFontStyle(PdfFontStyle style)
  {
    FontStyle fontStyle = FontStyle.Regular;
    if ((style & PdfFontStyle.Bold) == PdfFontStyle.Bold)
      fontStyle |= FontStyle.Bold;
    if ((style & PdfFontStyle.Italic) == PdfFontStyle.Italic)
      fontStyle |= FontStyle.Italic;
    if ((style & PdfFontStyle.Strikeout) == PdfFontStyle.Strikeout)
      fontStyle |= FontStyle.Strikeout;
    if ((style & PdfFontStyle.Underline) == PdfFontStyle.Underline)
      fontStyle |= FontStyle.Underline;
    return fontStyle;
  }

  private float ConvertToPoints(double value)
  {
    return this.m_unitConvertor.ConvertFromPixels((float) value, PdfGraphicsUnit.Point);
  }

  private PointF ConvertToPoints(PointF point)
  {
    return new PointF(this.ConvertToPoints((double) point.X), this.ConvertToPoints((double) point.Y));
  }

  private int Find(string fontName)
  {
    for (int index = 0; index < this.PrivateFonts.Families.Length; ++index)
    {
      if (this.PrivateFonts.Families[index].Name == fontName)
        return index;
    }
    return 0;
  }

  private Color FromHtml(string colorString)
  {
    string[] strArray1 = colorString.Replace("sc#", string.Empty).Split(',');
    Color empty = Color.Empty;
    Color color;
    if (strArray1 != null && strArray1.Length > 1)
    {
      int num1 = 0;
      double[] numArray1 = new double[4]
      {
        strArray1.Length == 4 ? OXPSRenderer.ParseDouble(strArray1[num1++]) : 1.0,
        0.0,
        0.0,
        0.0
      };
      double[] numArray2 = numArray1;
      string[] strArray2 = strArray1;
      int index1 = num1;
      int num2 = index1 + 1;
      double num3 = OXPSRenderer.ParseDouble(strArray2[index1]);
      numArray2[1] = num3;
      double[] numArray3 = numArray1;
      string[] strArray3 = strArray1;
      int index2 = num2;
      int num4 = index2 + 1;
      double num5 = OXPSRenderer.ParseDouble(strArray3[index2]);
      numArray3[2] = num5;
      double[] numArray4 = numArray1;
      string[] strArray4 = strArray1;
      int index3 = num4;
      int num6 = index3 + 1;
      double num7 = OXPSRenderer.ParseDouble(strArray4[index3]);
      numArray4[3] = num7;
      color = Color.FromArgb(numArray1[0] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[0] * 256.0), numArray1[1] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[1] * 256.0), numArray1[2] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[2] * 256.0), numArray1[3] == 1.0 ? (int) byte.MaxValue : (int) (numArray1[3] * 256.0));
    }
    else
      color = ColorTranslator.FromHtml(colorString);
    return color;
  }

  internal static float ParseFloat(string f)
  {
    float result = 0.0f;
    if (CultureInfo.CurrentCulture.ToString() == "pt-BR")
      f = f.Replace(',', '.');
    else if (CultureInfo.CurrentCulture.ToString() == "fi-FI")
      f = f.Replace(',', '.');
    if (float.TryParse(f, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      result = float.Parse(f, (IFormatProvider) CultureInfo.InvariantCulture);
    return result;
  }

  internal static double ParseDouble(string f)
  {
    double result = 0.0;
    if (double.TryParse(f, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      result = double.Parse(f, (IFormatProvider) CultureInfo.InvariantCulture);
    return result;
  }

  public void Dispose()
  {
  }
}
