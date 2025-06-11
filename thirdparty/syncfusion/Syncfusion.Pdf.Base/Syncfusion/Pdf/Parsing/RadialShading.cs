// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.RadialShading
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

internal class RadialShading : GradientShading
{
  public RadialShading(PdfDictionary dictionary)
    : base(dictionary)
  {
  }

  public RadialShading()
  {
  }

  internal override Brush GetBrushColor(Syncfusion.PdfViewer.Base.Matrix transformMatrix)
  {
    double floatValue1 = (double) (this.Coordinate[0] as PdfNumber).FloatValue;
    double floatValue2 = (double) (this.Coordinate[1] as PdfNumber).FloatValue;
    double floatValue3 = (double) (this.Coordinate[2] as PdfNumber).FloatValue;
    double floatValue4 = (double) (this.Coordinate[3] as PdfNumber).FloatValue;
    double floatValue5 = (double) (this.Coordinate[4] as PdfNumber).FloatValue;
    double floatValue6 = (double) (this.Coordinate[5] as PdfNumber).FloatValue;
    double floatValue7 = (double) (this.Domain[0] as PdfNumber).FloatValue;
    double floatValue8 = (double) (this.Domain[1] as PdfNumber).FloatValue;
    if (this.Extented != null)
    {
      int num1 = (this.Extented[0] as PdfBoolean).Value ? 1 : 0;
      int num2 = (this.Extented[1] as PdfBoolean).Value ? 1 : 0;
    }
    RectangleF rect = floatValue3 == 0.0 ? new RectangleF((float) (floatValue4 - floatValue6), (float) (floatValue5 - floatValue6), (float) (floatValue6 * 2.0), (float) (floatValue6 * 2.0)) : new RectangleF((float) (floatValue1 - floatValue3), (float) (floatValue2 - floatValue3), (float) (floatValue3 * 2.0), (float) (floatValue3 * 2.0));
    rect = floatValue6 == 0.0 ? new RectangleF((float) (floatValue1 - floatValue3), (float) (floatValue2 - floatValue3), (float) (floatValue3 * 2.0), (float) (floatValue3 * 2.0)) : new RectangleF((float) (floatValue4 - floatValue6), (float) (floatValue5 - floatValue6), (float) (floatValue6 * 2.0), (float) (floatValue6 * 2.0));
    GraphicsPath path = new GraphicsPath();
    path.AddRectangle(rect);
    PathGradientBrush brushColor = new PathGradientBrush(path);
    double num3 = Math.Sqrt((floatValue1 - floatValue4) * (floatValue1 - floatValue4) + (floatValue2 - floatValue5) * (floatValue2 - floatValue5));
    double num4 = floatValue8 - floatValue7;
    double num5 = Math.Abs(floatValue8 - floatValue7);
    double num6 = 1.0 / (transformMatrix.Transform(num3 - num5) * 3.0);
    List<Color> colorList = new List<Color>();
    List<float> floatList = new List<float>();
    if (num6 <= 0.0)
      return Brushes.Gray;
    for (double data = floatValue7; data < floatValue8; data += num6)
    {
      colorList.Add(this.GetColor(data));
      floatList.Add(Convert.ToSingle((data - floatValue7) / num4));
    }
    Color[] array1 = colorList.ToArray();
    float[] array2 = floatList.ToArray();
    array2[floatList.Count - 1] = 1f;
    brushColor.InterpolationColors = new ColorBlend()
    {
      Colors = array1,
      Positions = array2
    };
    colorList.Clear();
    floatList.Clear();
    return (Brush) brushColor;
  }
}
