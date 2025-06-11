// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.LabColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class LabColor : CIEBasedColorspace
{
  private PdfArray m_whitePoint;
  private PdfArray m_blackPoint;
  private PdfArray m_range;

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

  internal PdfArray Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  internal override Color GetColor(string[] pars)
  {
    double[] numArray = this.LabColorTransferFunction(pars);
    Color baseColor = Color.FromArgb((int) byte.MaxValue, (int) (byte) (numArray[0] * (double) byte.MaxValue), (int) (byte) (numArray[1] * (double) byte.MaxValue), (int) (byte) (numArray[2] * (double) byte.MaxValue));
    return (double) baseColor.GetBrightness() == 0.0 ? Color.FromArgb((int) byte.MaxValue, baseColor) : Color.FromArgb((int) (byte) ((double) byte.MaxValue * (double) baseColor.GetBrightness()), baseColor);
  }

  internal override Color GetColor(byte[] bytes, int offset)
  {
    return Colorspace.GetRgbColor(bytes, offset);
  }

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    return new Pen(this.GetColor(pars)).Brush;
  }

  private double[] LabColorTransferFunction(string[] pars)
  {
    float result1;
    float.TryParse(pars[0], out result1);
    float result2;
    float.TryParse(pars[1], out result2);
    float result3;
    float.TryParse(pars[2], out result3);
    double num1 = LabColor.CorrectRange(new double[2]
    {
      (double) (this.m_range[0] as PdfNumber).FloatValue,
      (double) (this.m_range[1] as PdfNumber).FloatValue
    }, (double) result2);
    double num2 = LabColor.CorrectRange(new double[2]
    {
      (double) (this.m_range[2] as PdfNumber).FloatValue,
      (double) (this.m_range[3] as PdfNumber).FloatValue
    }, (double) result3);
    double x1 = ((double) result1 + 16.0) / 116.0 + num1 / 500.0;
    double x2 = ((double) result1 + 16.0) / 116.0;
    double x3 = ((double) result1 + 16.0) / 116.0 - num2 / 200.0;
    return this.XYZtoRGB((double) (this.WhitePoint[0] as PdfNumber).FloatValue * LabColor.GammaFunction(x1), (double) (this.WhitePoint[1] as PdfNumber).FloatValue * LabColor.GammaFunction(x2), (double) (this.WhitePoint[2] as PdfNumber).FloatValue * LabColor.GammaFunction(x3), (double) (this.WhitePoint[2] as PdfNumber).FloatValue);
  }

  private double GetCompare(double f, double min, double max)
  {
    if (f <= min)
      return min;
    return f >= max ? max : f;
  }

  private static double GammaFunction(double x)
  {
    return x < 0.0 ? (x - 0.0) * (108.0 / 841.0) : x * x * x;
  }

  private static double CorrectRange(double[] range, double value)
  {
    double num = range[1] - range[0];
    return (value - range[0]) / num * 200.0 - 100.0;
  }

  private double[] XYZtoRGB(double x, double y, double z, double whitepointZ)
  {
    double[] numArray = new double[3];
    if (whitepointZ < 1.0)
    {
      numArray[0] = x * 3.1339 + y * -1.617 + z * -0.4906;
      numArray[1] = x * -0.9785 + y * 1.916 + z * 0.0333;
      numArray[2] = x * 0.072 + y * -0.229 + z * 1.4057;
    }
    else
    {
      numArray[0] = (x * 3.240449 + y * -1.537136 + z * -0.498531) * 0.830026;
      numArray[1] = (x * -0.969265 + y * 1.876011 + z * 0.041556) * 1.05452;
      numArray[2] = (x * 0.055643 + y * -0.204026 + z * 1.057229) * 1.1002999544143677;
    }
    return numArray;
  }

  private double ColorTransferFunction(double value)
  {
    return value > 0.0031308 ? 1.055 * Math.Pow(value, 5.0 / 12.0) - 0.055 : value * 12.92;
  }

  internal void SetValue(PdfArray labColorspaceArray)
  {
    if ((object) (labColorspaceArray[1] as PdfReferenceHolder) != null)
    {
      PdfReferenceHolder labColorspace = labColorspaceArray[1] as PdfReferenceHolder;
      if (!(labColorspace != (PdfReferenceHolder) null) || !(labColorspace.Object is PdfDictionary))
        return;
      PdfDictionary pdfDictionary = labColorspace.Object as PdfDictionary;
      if (pdfDictionary.ContainsKey("WhitePoint"))
        this.m_whitePoint = pdfDictionary["WhitePoint"] as PdfArray;
      if (pdfDictionary.ContainsKey("BlackPoint"))
        this.m_blackPoint = pdfDictionary["BlackPoint"] as PdfArray;
      if (!pdfDictionary.ContainsKey("Range"))
        return;
      this.m_range = pdfDictionary["Range"] as PdfArray;
    }
    else
    {
      if (!(labColorspaceArray[1] is PdfDictionary))
        return;
      PdfDictionary labColorspace = labColorspaceArray[1] as PdfDictionary;
      if (labColorspace.ContainsKey("WhitePoint"))
        this.m_whitePoint = labColorspace["WhitePoint"] as PdfArray;
      if (labColorspace.ContainsKey("BlackPoint"))
        this.m_blackPoint = labColorspace["BlackPoint"] as PdfArray;
      if (!labColorspace.ContainsKey("Range"))
        return;
      this.m_range = labColorspace["Range"] as PdfArray;
    }
  }
}
