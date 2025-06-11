// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Pattern
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Pattern : Colorspace
{
  private Pattern m_patterType;
  private Colorspace m_alternateColorspace;
  private PdfArray m_patternMatrix;
  private IPdfPrimitive m_patternDictioanry;
  private GraphicObjectData m_objects;
  private Bitmap m_brushImage;
  private Colorspace m_currentColorspace;
  private string[] m_dashedLine;
  private float m_lineCap;
  private PointF m_currentLocation = PointF.Empty;
  private bool m_isCurrentPositionChanged;
  private PathGeometry m_currentGeometry = new PathGeometry();
  private PathFigure m_currentPath;
  private Graphics m_brushGraphics;
  private bool m_isRectangle;
  private bool m_isCircle;
  private string m_rectangleWidth;
  private TilingPattern m_tilingPattern;

  internal Colorspace AlternateColorspace
  {
    get => this.m_alternateColorspace;
    set => this.m_alternateColorspace = value;
  }

  internal PdfArray PatternMatrix
  {
    get => this.m_patternMatrix;
    set => this.m_patternMatrix = value;
  }

  internal Pattern Type
  {
    get => this.m_patterType;
    set => this.m_patterType = value;
  }

  internal void SetValue(IPdfPrimitive array)
  {
  }

  private Pattern GetPatternType(IPdfPrimitive patternValue)
  {
    if (patternValue is PdfDictionary dictionary)
    {
      if (dictionary.Items.ContainsKey(new PdfName("PatternType")))
      {
        switch ((dictionary.Items[new PdfName("PatternType")] as PdfNumber).IntValue)
        {
          case 1:
            return this.Type = (Pattern) new TilingPattern(dictionary);
          case 2:
            return this.Type = (Pattern) new ShadingPattern(dictionary, this.m_isRectangle, this.m_isCircle, this.m_rectangleWidth);
        }
      }
      else if (dictionary.Items.ContainsKey(new PdfName("ShadingType")))
        return this.Type = (Pattern) new ShadingPattern(dictionary, this.m_isRectangle, this.m_isCircle, this.m_rectangleWidth);
    }
    return (Pattern) null;
  }

  private void SetPattern(string[] pars, PdfPageResources resource)
  {
    if (!resource.ContainsKey(pars[0].Replace("/", "")) || !(resource[pars[0].Replace("/", "")] is ExtendColorspace))
      return;
    this.m_patternDictioanry = (resource[pars[0].Replace("/", "")] as ExtendColorspace).ColorSpaceValueArray;
  }

  internal override Color GetColor(string[] pars) => Color.Gray;

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    this.SetPattern(pars, resource);
    switch (this.GetPatternType(this.m_patternDictioanry))
    {
      case ShadingPattern shadingPattern:
        return shadingPattern.CreateBrush();
      case TilingPattern tilingPattern:
        if (tilingPattern.Resources != null)
          this.GetTilingImage(tilingPattern);
        return tilingPattern.CreateBrush();
      default:
        return Brushes.Gray;
    }
  }

  internal override void SetOperatorValues(bool IsRectangle, bool IsCircle, string RectangleWidth)
  {
    this.m_isRectangle = IsRectangle;
    this.m_isCircle = IsCircle;
    this.m_rectangleWidth = RectangleWidth;
  }

  private PointF CurrentLocation
  {
    get => this.m_currentLocation;
    set
    {
      this.m_currentLocation = value;
      this.m_isCurrentPositionChanged = true;
    }
  }

  private System.Drawing.Drawing2D.Matrix CurrentMatrix
  {
    get => this.m_objects.drawing2dMatrixCTM;
    set => this.m_objects.drawing2dMatrixCTM = value;
  }

  private float MitterLength
  {
    get => this.m_objects.m_mitterLength;
    set => this.m_objects.m_mitterLength = value;
  }

  private Syncfusion.PdfViewer.Base.Matrix DocumentMatrix
  {
    get => this.m_objects.documentMatrix;
    set => this.m_objects.documentMatrix = value;
  }

  private System.Drawing.Drawing2D.Matrix Drawing2dMatrixCTM
  {
    get => this.m_objects.drawing2dMatrixCTM;
    set => this.m_objects.drawing2dMatrixCTM = value;
  }

  private PathFigure CurrentPath
  {
    get => this.m_currentPath;
    set => this.m_currentPath = value;
  }

  private Brush NonStrokingBrush
  {
    get => this.m_objects.NonStrokingBrush != null ? this.m_objects.NonStrokingBrush : (Brush) null;
  }

  private Brush StrokingBrush
  {
    get => this.m_objects.StrokingBrush != null ? this.m_objects.StrokingBrush : (Brush) null;
  }

  private void GetTilingImage(TilingPattern tilingPattern)
  {
    this.m_tilingPattern = tilingPattern;
    this.m_brushImage = tilingPattern.BoundingRectangle.Width >= 1 || tilingPattern.BoundingRectangle.Height >= 1 ? new Bitmap(tilingPattern.BoundingRectangle.Width, tilingPattern.BoundingRectangle.Height) : new Bitmap(2, 2);
    this.m_objects = new GraphicObjectData();
    this.m_brushGraphics = Graphics.FromImage((Image) this.m_brushImage);
    tilingPattern.TilingPatternMatrix = this.GetPatternMatrix(tilingPattern);
    this.m_objects.Ctm = Syncfusion.PdfViewer.Base.Matrix.Identity;
    char[] chArray = new char[6]
    {
      '(',
      ')',
      '[',
      ']',
      '<',
      '>'
    };
    PdfStream data = tilingPattern.Data;
    data.isSkip = true;
    data.Decompress();
    PdfRecordCollection recordCollection = new ContentParser(data.InternalStream.ToArray()).ReadContent();
    PageResourceLoader pageResourceLoader = new PageResourceLoader();
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    System.Drawing.Drawing2D.Matrix transform1 = this.m_brushGraphics.Transform;
    this.m_objects.documentMatrix = new Syncfusion.PdfViewer.Base.Matrix(4.0 / 3.0 * ((double) this.m_brushGraphics.DpiX / 96.0) * (double) transform1.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) this.m_brushGraphics.DpiX / 96.0) * (double) transform1.Elements[3], 0.0, (double) this.m_brushImage.Height * (double) transform1.Elements[3]);
    System.Drawing.Drawing2D.Matrix matrix1 = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    this.Drawing2dMatrixCTM = matrix1;
    this.CurrentMatrix = matrix1;
    if (recordCollection != null)
    {
      for (int index = 0; index < recordCollection.RecordCollection.Count; ++index)
      {
        string str = recordCollection.RecordCollection[index].OperatorName;
        string[] operands = recordCollection.RecordCollection[index].Operands;
        foreach (char ch in chArray)
        {
          if (str.Contains(ch.ToString()))
            str = str.Replace(ch.ToString(), "");
        }
        switch (str.Trim())
        {
          case "cs":
            this.SetStrokingColorspace(this.GetColorspace(operands));
            break;
          case "CS":
            this.SetNonStrokingColorspace(this.GetColorspace(operands));
            break;
          case "rg":
            this.SetStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
            break;
          case "RG":
            this.SetNonStrokingRGBColor(this.GetColor(operands, "nonstroking", "RGB"));
            break;
          case "d":
            if (operands[0] != "[]" && !operands[0].Contains("\n"))
            {
              this.m_dashedLine = operands;
              break;
            }
            break;
          case "w":
            this.MitterLength = float.Parse(operands[0]);
            break;
          case "W":
            Syncfusion.PdfViewer.Base.Matrix transform2 = new Syncfusion.PdfViewer.Base.Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
            System.Drawing.Drawing2D.Matrix matrix2 = new System.Drawing.Drawing2D.Matrix((float) Math.Round(transform2.M11, 5, MidpointRounding.ToEven), (float) Math.Round(transform2.M12, 5, MidpointRounding.ToEven), (float) Math.Round(transform2.M21, 5, MidpointRounding.ToEven), (float) Math.Round(transform2.M22, 5, MidpointRounding.ToEven), (float) Math.Round(transform2.OffsetX, 5, MidpointRounding.ToEven), (float) Math.Round(transform2.OffsetY, 5, MidpointRounding.ToEven));
            System.Drawing.Drawing2D.Matrix transform3 = this.m_brushGraphics.Transform;
            int pageUnit1 = (int) this.m_brushGraphics.PageUnit;
            this.m_brushGraphics.PageUnit = GraphicsUnit.Pixel;
            this.m_brushGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
            foreach (PathFigure figure in this.m_currentGeometry.Figures)
            {
              figure.IsClosed = true;
              figure.IsFilled = true;
            }
            this.m_currentGeometry.FillRule = FillRule.Nonzero;
            GraphicsPath geometry = this.GetGeometry(this.m_currentGeometry, transform2);
            if (geometry.PointCount != 0)
            {
              this.m_brushGraphics.SetClip(geometry, CombineMode.Intersect);
              break;
            }
            break;
          case "J":
            this.m_lineCap = float.Parse(operands[0]);
            break;
          case "m":
            this.BeginPath(operands);
            break;
          case "l":
            this.AddLine(operands);
            break;
          case "s":
          case "S":
            this.DrawPath();
            break;
          case "re":
            this.GetClipRectangle(operands);
            break;
          case "f":
            this.FillPath("Winding");
            this.CurrentLocation = PointF.Empty;
            break;
          case "k":
            this.SetStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
            break;
          case "K":
            this.SetNonStrokingCMYKColor(this.GetColor(operands, "nonstroking", "DeviceCMYK"));
            break;
          case "cm":
            this.CurrentMatrix = new System.Drawing.Drawing2D.Matrix(float.Parse(operands[0]), float.Parse(operands[1]), float.Parse(operands[2]), float.Parse(operands[3]), float.Parse(operands[4]), float.Parse(operands[5]));
            break;
          case "Do":
            PdfDictionary resources = tilingPattern.Resources;
            if (resources.ContainsKey("XObject"))
            {
              PdfPageResources pageResources = new PdfPageResources();
              PdfDictionary pdfDictionary2 = new PdfDictionary();
              if (resources["XObject"] is PdfDictionary)
                pdfDictionary2 = resources["XObject"] as PdfDictionary;
              else if (resources["XObject"] as PdfReferenceHolder != (PdfReferenceHolder) null && (resources["XObject"] as PdfReferenceHolder).Object is PdfDictionary)
                pdfDictionary2 = (resources["XObject"] as PdfReferenceHolder).Object as PdfDictionary;
              Dictionary<string, PdfMatrix> commonMatrix = new Dictionary<string, PdfMatrix>();
              PdfPageResources pdfPageResources = pageResourceLoader.UpdatePageResources(pageResources, pageResourceLoader.GetImageResources(resources, (PdfPageBase) null, ref commonMatrix));
              using (Dictionary<PdfName, IPdfPrimitive>.Enumerator enumerator = pdfDictionary2.Items.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  if (enumerator.Current.Key.Value.Contains(operands[0].Replace("/", "")) && pdfPageResources.ContainsKey(operands[0].Replace("/", "")) && pdfPageResources.Resources[operands[0].Replace("/", "")] is ImageStructure)
                  {
                    ImageStructure resource = pdfPageResources.Resources[operands[0].Replace("/", "")] as ImageStructure;
                    bool flag = false;
                    Bitmap bitmap;
                    if (tilingPattern.BoundingRectangle.Width < 1 && tilingPattern.BoundingRectangle.Height < 1)
                    {
                      bitmap = new Bitmap(2, 2);
                      flag = true;
                    }
                    else
                      bitmap = new Bitmap(tilingPattern.BoundingRectangle.Width, tilingPattern.BoundingRectangle.Height);
                    using (Graphics graphics = Graphics.FromImage((Image) bitmap))
                    {
                      Image embeddedImage = resource.EmbeddedImage;
                      System.Drawing.Drawing2D.Matrix transform4 = graphics.Transform;
                      int pageUnit2 = (int) graphics.PageUnit;
                      graphics.PageUnit = GraphicsUnit.Pixel;
                      graphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                      System.Drawing.Drawing2D.Matrix matrix3 = new System.Drawing.Drawing2D.Matrix(this.CurrentMatrix.Elements[0], this.CurrentMatrix.Elements[1], this.CurrentMatrix.Elements[2], this.CurrentMatrix.Elements[3], this.CurrentMatrix.Elements[4], this.CurrentMatrix.Elements[5]);
                      if (!flag)
                        graphics.Transform = this.Scale(1f, -1f, 0.0f, 1f);
                      if (embeddedImage != null)
                      {
                        graphics.DrawImage(embeddedImage, new Rectangle(0, 0, 1, 1));
                        this.m_brushImage = bitmap;
                      }
                      else
                        break;
                    }
                  }
                }
                break;
              }
            }
            break;
        }
      }
    }
    data.InternalStream.Dispose();
    tilingPattern.EmbeddedImage = (Image) this.m_brushImage;
  }

  private void SetStrokingCMYKColor(Color color)
  {
    this.m_objects.StrokingColorspace = (Colorspace) new DeviceCMYK();
    this.SetStrokingColor(color);
  }

  private void SetNonStrokingCMYKColor(Color color)
  {
    this.m_objects.NonStrokingColorspace = (Colorspace) new DeviceCMYK();
    this.SetNonStrokingColor(color);
  }

  private void FillPath(string mode)
  {
    Pen pen = (Pen) null;
    if (!(this.StrokingBrush is TextureBrush))
      pen = this.StrokingBrush == null ? new Pen(Color.Black) : new Pen(this.StrokingBrush);
    Syncfusion.PdfViewer.Base.Matrix matrix1 = new Syncfusion.PdfViewer.Base.Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
    System.Drawing.Drawing2D.Matrix matrix2 = new System.Drawing.Drawing2D.Matrix((float) matrix1.M11, (float) matrix1.M12, (float) matrix1.M21, (float) matrix1.M22, (float) matrix1.OffsetX, (float) matrix1.OffsetY);
    System.Drawing.Drawing2D.Matrix matrix3 = new System.Drawing.Drawing2D.Matrix();
    matrix3.Multiply(matrix2);
    System.Drawing.Drawing2D.Matrix transform = this.m_brushGraphics.Transform;
    GraphicsUnit pageUnit = this.m_brushGraphics.PageUnit;
    this.m_brushGraphics.PageUnit = GraphicsUnit.Pixel;
    this.m_brushGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    this.m_brushGraphics.Transform = matrix3;
    foreach (PathFigure figure in this.m_currentGeometry.Figures)
    {
      figure.IsClosed = true;
      figure.IsFilled = true;
    }
    this.m_currentGeometry.FillRule = mode == "Winding" ? FillRule.Nonzero : FillRule.EvenOdd;
    GraphicsPath geometry = this.GetGeometry(this.m_currentGeometry, new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
    geometry.FillMode = this.m_currentGeometry.FillRule == FillRule.Nonzero ? FillMode.Winding : FillMode.Alternate;
    this.m_brushGraphics.FillPath(pen.Brush, geometry);
    this.m_currentGeometry = new PathGeometry();
    this.m_currentPath = (PathFigure) null;
    this.m_brushGraphics.Transform = transform;
    this.m_brushGraphics.PageUnit = pageUnit;
  }

  private void GetClipRectangle(string[] rectangle)
  {
    float x = float.Parse(rectangle[0]);
    float y = float.Parse(rectangle[1]);
    float width = float.Parse(rectangle[2]);
    float height = float.Parse(rectangle[3]);
    this.BeginPath(x, y);
    this.AddLine(x + width, y);
    this.AddLine(x + width, y + height);
    this.AddLine(x, y + height);
    this.EndPath();
    RectangleF rectangleF = new RectangleF(x, y, width, height);
  }

  private void AddLine(float x, float y)
  {
    this.CurrentLocation = new PointF(x, y);
    this.m_currentPath.Segments.Add((PathSegment) new LineSegment()
    {
      Point = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation
    });
  }

  private void EndPath()
  {
    if (this.m_currentPath == null)
      return;
    this.m_currentPath.IsClosed = true;
  }

  private void DrawPath()
  {
    Pen pen = new Pen(this.NonStrokingBrush == null ? new Pen(Color.Black).Brush : this.NonStrokingBrush);
    System.Drawing.Drawing2D.Matrix transform = this.m_brushGraphics.Transform;
    GraphicsUnit pageUnit = this.m_brushGraphics.PageUnit;
    this.m_brushGraphics.PageUnit = GraphicsUnit.Pixel;
    this.m_brushGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    this.m_brushGraphics.Transform = this.CurrentMatrix;
    GraphicsPath geometry = this.GetGeometry(this.m_currentGeometry, new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
    if ((double) this.MitterLength != 0.0)
      pen.Width = this.MitterLength;
    if (this.m_dashedLine != null)
    {
      string str1 = this.m_dashedLine[0];
      string str2 = str1.Substring(1, str1.Length - 2).Trim();
      List<string> stringList = new List<string>();
      string str3 = str2;
      char[] chArray = new char[1]{ ' ' };
      foreach (string str4 in str3.Split(chArray))
      {
        if (str4 == "0")
          stringList.Add("0.000000001");
        else if (str4 != "")
          stringList.Add(str4);
      }
      float[] numArray = new float[stringList.Count];
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (stringList[index] != "")
          numArray[index] = float.Parse(stringList[index]);
      }
      if (numArray.Length > 0 && (double) this.MitterLength < (double) numArray[0] && (double) this.MitterLength != 0.0)
      {
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = numArray[index] / this.MitterLength;
      }
      if (numArray.Length > 0)
        pen.DashPattern = numArray;
      if ((double) this.m_lineCap == 1.0)
        pen.DashCap = DashCap.Round;
      this.m_dashedLine = (string[]) null;
    }
    this.m_currentGeometry.FillRule = FillRule.Nonzero;
    geometry.FillMode = FillMode.Alternate;
    this.m_brushGraphics.DrawPath(pen, geometry);
    this.m_currentGeometry = new PathGeometry();
    this.m_currentPath = (PathFigure) null;
    this.m_brushGraphics.Transform = transform;
    this.m_brushGraphics.PageUnit = pageUnit;
  }

  private void AddLine(string[] line)
  {
    this.CurrentLocation = new PointF(float.Parse(line[0]), float.Parse(line[1]));
    this.m_currentPath.Segments.Add((PathSegment) new LineSegment()
    {
      Point = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation
    });
  }

  private void BeginPath(string[] point)
  {
    this.CurrentLocation = new PointF(float.Parse(point[0]), float.Parse(point[1]));
    if (this.m_currentPath != null && this.m_currentPath.Segments.Count == 0)
      this.m_currentGeometry.Figures.Remove(this.CurrentPath);
    this.m_currentPath = new PathFigure();
    this.m_currentPath.StartPoint = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation;
    this.m_currentGeometry.Figures.Add(this.m_currentPath);
  }

  private void BeginPath(float x, float y)
  {
    this.CurrentLocation = new PointF(x, y);
    if (this.m_currentPath != null && this.m_currentPath.Segments.Count == 0)
      this.m_currentGeometry.Figures.Remove(this.CurrentPath);
    this.m_currentPath = new PathFigure();
    this.m_currentPath.StartPoint = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation;
    this.m_currentGeometry.Figures.Add(this.m_currentPath);
  }

  internal GraphicsPath GetGeometry(PathGeometry geometry, Syncfusion.PdfViewer.Base.Matrix transform)
  {
    System.Drawing.Drawing2D.Matrix transformationMatrix = PdfElementsRenderer.GetTransformationMatrix(transform);
    GraphicsPath graphicsPath = new GraphicsPath();
    foreach (PathFigure figure in geometry.Figures)
    {
      graphicsPath.StartFigure();
      PointF pointF = new PointF((float) figure.StartPoint.X, (float) figure.StartPoint.Y);
      foreach (PathSegment segment in figure.Segments)
      {
        if (segment is LineSegment)
        {
          LineSegment lineSegment = (LineSegment) segment;
          PointF[] pts = new PointF[2]
          {
            pointF,
            new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y)
          };
          transformationMatrix.TransformPoints(pts);
          graphicsPath.AddLine(pts[0], pts[1]);
          pointF = new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y);
        }
        else if (segment is BezierSegment)
        {
          BezierSegment bezierSegment = segment as BezierSegment;
          PointF[] pts = new PointF[4]
          {
            pointF,
            new PointF((float) bezierSegment.Point1.X, (float) bezierSegment.Point1.Y),
            new PointF((float) bezierSegment.Point2.X, (float) bezierSegment.Point2.Y),
            new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y)
          };
          transformationMatrix.TransformPoints(pts);
          graphicsPath.AddBezier(pts[0], pts[1], pts[2], pts[3]);
          pointF = new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y);
        }
      }
      if (figure.IsClosed)
        graphicsPath.CloseFigure();
    }
    return (GraphicsPath) graphicsPath.Clone();
  }

  private Color GetColor(string[] colorElement, string type, string colorSpace)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 1f;
    if (colorSpace == "RGB" && colorElement.Length == 3)
    {
      num1 = float.Parse(colorElement[0]);
      num2 = float.Parse(colorElement[1]);
      num3 = float.Parse(colorElement[2]);
    }
    else if (colorSpace == "DeviceCMYK" && colorElement.Length == 4)
    {
      float result1;
      float.TryParse(colorElement[0], out result1);
      float result2;
      float.TryParse(colorElement[1], out result2);
      float result3;
      float.TryParse(colorElement[2], out result3);
      float result4;
      float.TryParse(colorElement[3], out result4);
      return this.ConvertCMYKtoRGB(result1, result2, result3, result4);
    }
    return Color.FromArgb((int) (byte) ((double) num4 * (double) byte.MaxValue), (int) (byte) ((double) num1 * (double) byte.MaxValue), (int) (byte) ((double) num2 * (double) byte.MaxValue), (int) (byte) ((double) num3 * (double) byte.MaxValue));
  }

  private Color ConvertCMYKtoRGB(float c, float m, float y, float k)
  {
    float num1 = (float) ((double) byte.MaxValue * (1.0 - (double) c) * (1.0 - (double) k));
    float num2 = (float) ((double) byte.MaxValue * (1.0 - (double) m) * (1.0 - (double) k));
    float num3 = (float) ((double) byte.MaxValue * (1.0 - (double) y) * (1.0 - (double) k));
    return Color.FromArgb((int) byte.MaxValue, (double) num1 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num1 < 0.0 ? 0 : (int) num1), (double) num2 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num2 < 0.0 ? 0 : (int) num2), (double) num3 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num3 < 0.0 ? 0 : (int) num3));
  }

  private void SetStrokingRGBColor(Color color)
  {
    this.m_objects.StrokingColorspace = (Colorspace) new DeviceRGB();
    this.SetStrokingColor(color);
  }

  private void SetStrokingColor(Color color) => this.m_objects.StrokingBrush = new Pen(color).Brush;

  private void SetNonStrokingRGBColor(Color color)
  {
    this.m_objects.NonStrokingColorspace = (Colorspace) new DeviceRGB();
    this.SetNonStrokingColor(color);
  }

  private void SetNonStrokingColor(Color color)
  {
    this.m_objects.NonStrokingBrush = new Pen(color).Brush;
  }

  private void SetNonStrokingColorspace(Colorspace colorspace)
  {
    this.m_objects.NonStrokingColorspace = colorspace;
  }

  private void SetStrokingColorspace(Colorspace colorspace)
  {
    this.m_objects.StrokingColorspace = colorspace;
  }

  private Colorspace GetColorspace(string[] colorspaceelement)
  {
    if (Colorspace.IsColorSpace(colorspaceelement[0].Replace("/", "")))
      this.m_currentColorspace = Colorspace.CreateColorSpace(colorspaceelement[0].Replace("/", ""));
    else if (this.m_tilingPattern.Resources.ContainsKey(colorspaceelement[0].Replace("/", "")))
    {
      if (this.m_tilingPattern.Resources[colorspaceelement[0].Replace("/", "")] is ExtendColorspace)
      {
        ExtendColorspace resource = this.m_tilingPattern.Resources[colorspaceelement[0].Replace("/", "")] as ExtendColorspace;
        if (resource.ColorSpaceValueArray is PdfArray colorSpaceValueArray1)
          this.m_currentColorspace = Colorspace.CreateColorSpace((colorSpaceValueArray1[0] as PdfName).Value, (IPdfPrimitive) colorSpaceValueArray1);
        PdfName colorSpaceValueArray2 = resource.ColorSpaceValueArray as PdfName;
        if (colorSpaceValueArray2 != (PdfName) null)
          this.m_currentColorspace = Colorspace.CreateColorSpace(colorSpaceValueArray2.Value);
        if (resource.ColorSpaceValueArray is PdfDictionary colorSpaceValueArray3)
          this.m_currentColorspace = Colorspace.CreateColorSpace("Shading", (IPdfPrimitive) colorSpaceValueArray3);
      }
    }
    else if (this.m_tilingPattern.Resources.ContainsKey("ColorSpace"))
    {
      PdfDictionary resource = this.m_tilingPattern.Resources["ColorSpace"] as PdfDictionary;
      if (resource.ContainsKey(colorspaceelement[0].Replace("/", "")) && (object) (resource[colorspaceelement[0].Replace("/", "")] as PdfReferenceHolder) != null && (resource[colorspaceelement[0].Replace("/", "")] as PdfReferenceHolder).Object is PdfArray array)
        this.m_currentColorspace = Colorspace.CreateColorSpace((array[0] as PdfName).Value, (IPdfPrimitive) array);
    }
    return this.m_currentColorspace;
  }

  private System.Drawing.Drawing2D.Matrix Scale(
    float scaleX,
    float scaleY,
    float centerX,
    float centerY)
  {
    return this.Multiply(new System.Drawing.Drawing2D.Matrix(scaleX, 0.0f, 0.0f, scaleY, centerX, centerY), this.CurrentMatrix);
  }

  private System.Drawing.Drawing2D.Matrix Multiply(System.Drawing.Drawing2D.Matrix matrix1, System.Drawing.Drawing2D.Matrix matrix2)
  {
    return new System.Drawing.Drawing2D.Matrix((float) ((double) matrix1.Elements[0] * (double) matrix2.Elements[0] + (double) matrix1.Elements[1] * (double) matrix2.Elements[2]), (float) ((double) matrix1.Elements[0] * (double) matrix2.Elements[1] + (double) matrix1.Elements[1] * (double) matrix2.Elements[3]), (float) ((double) matrix1.Elements[2] * (double) matrix2.Elements[0] + (double) matrix1.Elements[3] * (double) matrix2.Elements[2]), (float) ((double) matrix1.Elements[2] * (double) matrix2.Elements[1] + (double) matrix1.Elements[3] * (double) matrix2.Elements[3]), (float) ((double) matrix1.OffsetX * (double) matrix2.Elements[0] + (double) matrix1.OffsetY * (double) matrix2.Elements[2]) + matrix2.OffsetX, (float) ((double) matrix1.OffsetX * (double) matrix2.Elements[1] + (double) matrix1.OffsetY * (double) matrix2.Elements[3]) + matrix2.OffsetY);
  }

  private System.Drawing.Drawing2D.Matrix GetPatternMatrix(TilingPattern tilingPattern)
  {
    return new System.Drawing.Drawing2D.Matrix((tilingPattern.PatternMatrix[0] as PdfNumber).FloatValue, (tilingPattern.PatternMatrix[1] as PdfNumber).FloatValue, (tilingPattern.PatternMatrix[2] as PdfNumber).FloatValue, (tilingPattern.PatternMatrix[3] as PdfNumber).FloatValue, (tilingPattern.PatternMatrix[4] as PdfNumber).FloatValue, (tilingPattern.PatternMatrix[5] as PdfNumber).FloatValue);
  }

  internal override Color GetColor(byte[] bytes, int offset)
  {
    return Colorspace.GetRgbColor(bytes, offset);
  }

  internal override int Components => 1;
}
