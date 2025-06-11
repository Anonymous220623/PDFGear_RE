// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.AxialShading
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class AxialShading : GradientShading
{
  private bool m_isRectangle;
  private bool m_isCircle;
  private float m_rectangleWidth;
  internal RectangleF ShadingRectangle = RectangleF.Empty;

  public AxialShading(PdfDictionary dictionary)
    : base(dictionary)
  {
  }

  public AxialShading()
  {
  }

  internal override void SetOperatorValues(bool IsRectangle, bool IsCircle, string RectangleWidth)
  {
    this.m_isRectangle = IsRectangle;
    this.m_isCircle = IsCircle;
    if (RectangleWidth == null)
      return;
    this.m_rectangleWidth = float.Parse(RectangleWidth);
  }

  internal override Brush GetBrushColor(Syncfusion.PdfViewer.Base.Matrix transformMatrix)
  {
    try
    {
      double x1;
      double y1;
      double x2;
      double y2;
      if (this.m_isRectangle)
      {
        double floatValue1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue;
        double floatValue2 = (double) (this.Coordinate[1] as PdfNumber).FloatValue;
        double floatValue3 = (double) (this.Coordinate[2] as PdfNumber).FloatValue;
        double floatValue4 = (double) (this.Coordinate[3] as PdfNumber).FloatValue;
        if (floatValue1 < 0.0 && floatValue3 > 0.0 && floatValue2 == 0.0 && floatValue4 == 0.0)
        {
          double rectangleWidth = (double) this.m_rectangleWidth;
          x1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue * transformMatrix.M11;
          double num = x1;
          y1 = (double) (this.Coordinate[1] as PdfNumber).FloatValue * transformMatrix.M22;
          x2 = (double) (this.Coordinate[2] as PdfNumber).FloatValue * transformMatrix.M11;
          y2 = (double) (this.Coordinate[3] as PdfNumber).FloatValue * transformMatrix.M22;
          if (x1 < 0.0 && x2 > 0.0 && rectangleWidth > num)
            x2 = rectangleWidth;
          else if (rectangleWidth > num)
          {
            x1 = rectangleWidth;
          }
          else
          {
            x1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue;
            y1 = (double) (this.Coordinate[1] as PdfNumber).FloatValue;
            x2 = (double) (this.Coordinate[2] as PdfNumber).FloatValue;
            y2 = (double) (this.Coordinate[3] as PdfNumber).FloatValue;
          }
        }
        else
        {
          x1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue;
          y1 = (double) (this.Coordinate[1] as PdfNumber).FloatValue;
          x2 = (double) (this.Coordinate[2] as PdfNumber).FloatValue;
          y2 = (double) (this.Coordinate[3] as PdfNumber).FloatValue;
        }
      }
      else if (this.m_isCircle)
      {
        if (transformMatrix.M21 > 0.0 && transformMatrix.M12 < 0.0 && transformMatrix.M11 < 0.0 && transformMatrix.M22 < 0.0)
        {
          double num1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue * transformMatrix.M11;
          x1 = num1 + num1 * transformMatrix.M21;
          double num2 = (double) (this.Coordinate[1] as PdfNumber).FloatValue * transformMatrix.M22;
          y1 = num2 + num2 * transformMatrix.M12;
          double num3 = (double) (this.Coordinate[2] as PdfNumber).FloatValue * transformMatrix.M11 + (double) (this.Coordinate[2] as PdfNumber).FloatValue * transformMatrix.M11;
          x2 = num3 + num3 * transformMatrix.M21;
          double num4 = (double) (this.Coordinate[3] as PdfNumber).FloatValue * transformMatrix.M22 + (double) (this.Coordinate[3] as PdfNumber).FloatValue * transformMatrix.M22;
          y2 = num4 + num4 * transformMatrix.M12;
        }
        else
        {
          x1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue;
          y1 = (double) (this.Coordinate[1] as PdfNumber).FloatValue;
          x2 = (double) (this.Coordinate[2] as PdfNumber).FloatValue;
          y2 = (double) (this.Coordinate[3] as PdfNumber).FloatValue;
        }
      }
      else
      {
        x1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue;
        y1 = (double) (this.Coordinate[1] as PdfNumber).FloatValue;
        x2 = (double) (this.Coordinate[2] as PdfNumber).FloatValue;
        y2 = (double) (this.Coordinate[3] as PdfNumber).FloatValue;
      }
      double floatValue5 = (double) (this.Domain[0] as PdfNumber).FloatValue;
      double floatValue6 = (double) (this.Domain[1] as PdfNumber).FloatValue;
      if (this.Extented != null)
      {
        int num5 = (this.Extented[0] as PdfBoolean).Value ? 1 : 0;
        int num6 = (this.Extented[1] as PdfBoolean).Value ? 1 : 0;
      }
      Color color1 = this.GetColor(floatValue5);
      Color color2 = this.GetColor(floatValue6);
      if (this.AlternateColorspace is DeviceN)
      {
        DeviceN alternateColorspace1 = this.AlternateColorspace as DeviceN;
        if (alternateColorspace1.AlternateColorspace is ICCBased alternateColorspace2)
        {
          if ((transformMatrix.M11 != 0.0 || transformMatrix.M22 != 0.0) && alternateColorspace2.Profile.AlternateColorspace is DeviceCMYK)
          {
            double x3 = (double) (this.Coordinate[0] as PdfNumber).FloatValue * transformMatrix.M11;
            double y3 = (double) (this.Coordinate[1] as PdfNumber).FloatValue * transformMatrix.M22;
            double x4 = (double) (this.Coordinate[2] as PdfNumber).FloatValue * transformMatrix.M11;
            double y4 = (double) (this.Coordinate[3] as PdfNumber).FloatValue * transformMatrix.M22;
            if (x4 > 0.0)
              this.ShadingRectangle = new LinearGradientBrush(new PointF((float) x3, (float) y3), new PointF((float) x4, (float) y4), color1, color2).Rectangle;
            x1 = x3 / transformMatrix.M11;
            y1 = y3 / transformMatrix.M22;
            x2 = x4 / transformMatrix.M11;
            y2 = y4 / transformMatrix.M22;
          }
        }
        else if ((transformMatrix.M11 != 0.0 || transformMatrix.M22 != 0.0) && alternateColorspace1.AlternateColorspace is DeviceCMYK)
        {
          double x5 = (double) (this.Coordinate[0] as PdfNumber).FloatValue * transformMatrix.M11;
          double y5 = (double) (this.Coordinate[1] as PdfNumber).FloatValue * transformMatrix.M22;
          double x6 = (double) (this.Coordinate[2] as PdfNumber).FloatValue * transformMatrix.M11;
          double y6 = (double) (this.Coordinate[3] as PdfNumber).FloatValue * transformMatrix.M22;
          if (x6 > 0.0)
            this.ShadingRectangle = new LinearGradientBrush(new PointF((float) x5, (float) y5), new PointF((float) x6, (float) y6), color1, color2).Rectangle;
          x1 = x5 / transformMatrix.M11;
          y1 = y5 / transformMatrix.M22;
          x2 = x6 / transformMatrix.M11;
          y2 = y6 / transformMatrix.M22;
        }
      }
      if (this.AlternateColorspace is DeviceRGB && (transformMatrix.M11 != 0.0 || transformMatrix.M22 != 0.0) && (double) (this.Coordinate[1] as PdfNumber).FloatValue > 0.0 && (double) (this.Coordinate[0] as PdfNumber).FloatValue == 0.0)
      {
        x1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue * transformMatrix.M11;
        y1 = (double) (this.Coordinate[1] as PdfNumber).FloatValue * transformMatrix.M22;
        x2 = (double) (this.Coordinate[2] as PdfNumber).FloatValue * transformMatrix.M11;
        y2 = (double) (this.Coordinate[3] as PdfNumber).FloatValue * transformMatrix.M22;
      }
      LinearGradientBrush brushColor = new LinearGradientBrush(new PointF((float) x1, (float) y1), new PointF((float) x2, (float) y2), color1, color2);
      Type0 function = this.Function as Type0;
      bool flag = false;
      if (function != null)
        flag = (function.Size[0] as PdfNumber).IntValue <= 2;
      Color[] colorArray;
      float[] numArray;
      if (!flag)
      {
        double d = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        double num7 = floatValue6 - floatValue5;
        double num8 = 1.0 / (transformMatrix.Transform(d) * 3.0);
        List<Color> colorList = new List<Color>();
        List<float> floatList = new List<float>();
        for (double data = floatValue5; data < floatValue6; data += num8)
        {
          colorList.Add(this.GetColor(data));
          floatList.Add(Convert.ToSingle((data - floatValue5) / num7));
        }
        colorArray = colorList.ToArray();
        numArray = floatList.ToArray();
        numArray[floatList.Count - 1] = 1f;
        colorList.Clear();
        floatList.Clear();
      }
      else
      {
        Color color3 = this.ExecuteLinearInterpolation(color1, color2, 0.5f);
        colorArray = new Color[3]{ color1, color3, color2 };
        numArray = new float[3]{ 0.0f, 0.5f, 1f };
      }
      brushColor.InterpolationColors = new ColorBlend()
      {
        Colors = colorArray,
        Positions = numArray
      };
      return (Brush) brushColor;
    }
    catch
    {
      return Brushes.Transparent;
    }
  }

  private Color ExecuteLinearInterpolation(Color color1, Color color2, float factor)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) ((1.0 - (double) factor) * (double) color1.R + (double) factor * (double) color2.R), (int) ((1.0 - (double) factor) * (double) color1.G + (double) factor * (double) color2.G), (int) ((1.0 - (double) factor) * (double) color1.B + (double) factor * (double) color2.B));
  }
}
