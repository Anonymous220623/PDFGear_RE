// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.CalRGB
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class CalRGB : CIEBasedColorspace
{
  private PdfArray m_whitePoint;
  private PdfArray m_blackPoint;
  private PdfArray m_gamma;
  private ColorspaceMatrix m_matrix;

  internal override int Components => 3;

  internal override PdfArray WhitePoint
  {
    get => this.m_whitePoint;
    set => this.m_whitePoint = value;
  }

  internal override PdfArray BlackPoint
  {
    get
    {
      if (this.m_blackPoint == null)
        this.m_blackPoint = new PdfArray(new PdfArray()
        {
          (IPdfPrimitive) new PdfNumber(0),
          (IPdfPrimitive) new PdfNumber(0),
          (IPdfPrimitive) new PdfNumber(0)
        });
      return this.m_blackPoint;
    }
    set => this.m_blackPoint = value;
  }

  internal PdfArray Gamma
  {
    get => this.m_gamma;
    set => this.m_gamma = value;
  }

  internal ColorspaceMatrix Matrix
  {
    get => this.m_matrix;
    set => this.m_matrix = value;
  }

  internal override Color GetColor(string[] pars)
  {
    return Colorspace.GetRgbColor(this.ColorTransferFunction(pars));
  }

  internal override Color GetColor(byte[] bytes, int offset)
  {
    return Colorspace.GetRgbColor(bytes, offset);
  }

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    return new Pen(this.GetColor(pars)).Brush;
  }

  private double[] ColorTransferFunction(string[] values)
  {
    double[,] numArray1 = new double[3, 3]
    {
      {
        3.240449,
        -1.537136,
        -0.498531
      },
      {
        -0.969265,
        1.876011,
        0.041556
      },
      {
        0.055643,
        -0.204026,
        1.057229
      }
    };
    float result1;
    float.TryParse(values[0], out result1);
    float result2;
    float.TryParse(values[1], out result2);
    float result3;
    float.TryParse(values[2], out result3);
    double num1 = Math.Pow((double) result1, (double) (this.m_gamma[0] as PdfNumber).FloatValue);
    double num2 = Math.Pow((double) result2, (double) (this.m_gamma[1] as PdfNumber).FloatValue);
    double num3 = Math.Pow((double) result3, (double) (this.m_gamma[2] as PdfNumber).FloatValue);
    double[] numArray2 = new double[3];
    double num4 = this.Matrix.Xa * num1 + this.Matrix.Xb * num2 + this.Matrix.Xc * num3;
    double num5 = this.Matrix.Ya * num1 + this.Matrix.Yb * num2 + this.Matrix.Yc * num3;
    double num6 = this.Matrix.Za * num1 + this.Matrix.Zb * num2 + this.Matrix.Zc * num3;
    double num7 = numArray1[0, 0] * num4 + numArray1[0, 1] * num5 + numArray1[0, 2] * num6;
    double num8 = numArray1[1, 0] * num4 + numArray1[1, 1] * num5 + numArray1[1, 2] * num6;
    double num9 = numArray1[2, 0] * num4 + numArray1[2, 1] * num5 + numArray1[2, 2] * num6;
    double num10 = 1.0 / (numArray1[0, 0] * (double) (this.WhitePoint[0] as PdfNumber).FloatValue + numArray1[0, 1] * (double) (this.WhitePoint[1] as PdfNumber).FloatValue + numArray1[0, 2] * (double) (this.WhitePoint[2] as PdfNumber).FloatValue);
    double num11 = 1.0 / (numArray1[1, 0] * (double) (this.WhitePoint[0] as PdfNumber).FloatValue + numArray1[1, 1] * (double) (this.WhitePoint[1] as PdfNumber).FloatValue + numArray1[1, 2] * (double) (this.WhitePoint[2] as PdfNumber).FloatValue);
    double num12 = 1.0 / (numArray1[2, 0] * (double) (this.WhitePoint[0] as PdfNumber).FloatValue + numArray1[2, 1] * (double) (this.WhitePoint[1] as PdfNumber).FloatValue + numArray1[2, 2] * (double) (this.WhitePoint[2] as PdfNumber).FloatValue);
    double num13 = Math.Pow(this.ColorTransferFunction(num7 * num10), 0.5);
    double num14 = Math.Pow(this.ColorTransferFunction(num8 * num11), 0.5);
    double num15 = Math.Pow(this.ColorTransferFunction(num9 * num12), 0.5);
    numArray2[0] = num13;
    numArray2[1] = num14;
    numArray2[2] = num15;
    return numArray2;
  }

  internal void SetValue(PdfArray colorspaceArray)
  {
    if ((object) (colorspaceArray[1] as PdfReferenceHolder) != null)
    {
      PdfReferenceHolder colorspace = colorspaceArray[1] as PdfReferenceHolder;
      if (!(colorspace != (PdfReferenceHolder) null) || !(colorspace.Object is PdfDictionary))
        return;
      PdfDictionary pdfDictionary = colorspace.Object as PdfDictionary;
      if (pdfDictionary.ContainsKey("WhitePoint"))
        this.m_whitePoint = pdfDictionary["WhitePoint"] as PdfArray;
      if (pdfDictionary.ContainsKey("BlackPoint"))
        this.m_blackPoint = pdfDictionary["BlackPoint"] as PdfArray;
      if (pdfDictionary.ContainsKey("Gamma"))
        this.m_gamma = pdfDictionary["Gamma"] as PdfArray;
      if (!pdfDictionary.ContainsKey("Matrix"))
        return;
      this.m_matrix = new ColorspaceMatrix(pdfDictionary["Matrix"] as PdfArray);
    }
    else
    {
      if (!(colorspaceArray[1] is PdfDictionary))
        return;
      PdfDictionary colorspace = colorspaceArray[1] as PdfDictionary;
      if (colorspace.ContainsKey("WhitePoint"))
        this.m_whitePoint = colorspace["WhitePoint"] as PdfArray;
      if (colorspace.ContainsKey("BlackPoint"))
        this.m_blackPoint = colorspace["BlackPoint"] as PdfArray;
      if (colorspace.ContainsKey("Gamma"))
        this.m_gamma = colorspace["Gamma"] as PdfArray;
      if (!colorspace.ContainsKey("Matrix"))
        return;
      this.m_matrix = new ColorspaceMatrix(colorspace["Matrix"] as PdfArray);
    }
  }

  private double[] FromXYZ(double x, double y, double z, double whitepointZ)
  {
    double[] numArray = new double[3];
    if (whitepointZ < 1.0)
    {
      double num1 = numArray[0] = x * 3.1339 + y * -1.617 + z * -0.4906;
      double num2 = numArray[1] = x * -0.9785 + y * 1.916 + z * 0.0333;
      double num3 = numArray[2] = x * 0.072 + y * -0.229 + z * 1.4057;
      double num4 = num1 / (num1 + num2 + num3);
      double num5 = num2 / (num1 + num2 + num3);
    }
    else
    {
      double num6 = numArray[0] = x * 3.2406 + y * -1.5372 + z * -0.4986;
      double num7 = numArray[1] = x * -0.9689 + y * 1.8758 + z * 0.0415;
      double num8 = numArray[2] = x * 0.0557 + y * -0.204 + z * 1.057;
      double num9 = num6 / (num6 + num7 + num8);
      double num10 = num7 / (num6 + num7 + num8);
    }
    return numArray;
  }

  private double ColorTransferFunction(double value)
  {
    return value <= 0.0031308 ? value / 12.92 : 1.055 * Math.Pow(value, 5.0 / 12.0) - 0.055;
  }

  private double ExtractColorValues(double colorValue) => Math.Min(1.0, Math.Max(0.0, colorValue));
}
