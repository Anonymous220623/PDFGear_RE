// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.CalGray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class CalGray : CIEBasedColorspace
{
  private PdfArray m_whitePoint;
  private PdfArray m_blackPoint;
  private PdfNumber m_gamma;

  internal override int Components => 1;

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

  internal PdfNumber Gamma
  {
    get => this.m_gamma;
    set => this.m_gamma = value;
  }

  internal override Color GetColor(string[] pars)
  {
    return Colorspace.GetRgbColor(this.ColorTransferFunction(pars));
  }

  internal override Color GetColor(byte[] bytes, int offset)
  {
    return Colorspace.GetGrayColor(bytes, offset);
  }

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    return new Pen(this.GetColor(pars)).Brush;
  }

  private double[] ColorTransferFunction(string[] values)
  {
    float floatValue = (this.BlackPoint[1] as PdfNumber).FloatValue;
    float result;
    float.TryParse(values[0], out result);
    float num1 = (float) Math.Pow((double) result, (double) this.m_gamma.FloatValue);
    double num2 = Math.Max(116.0 * Math.Pow((double) floatValue + ((double) (this.m_whitePoint[1] as PdfNumber).FloatValue - (double) floatValue) * (double) num1, 1.0 / 3.0) - 16.0, 0.0) / 100.0;
    return new double[3]{ num2, num2, num2 };
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
      if (!pdfDictionary.ContainsKey("Gamma"))
        return;
      this.m_gamma = pdfDictionary["Gamma"] as PdfNumber;
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
      if (!colorspace.ContainsKey("Gamma"))
        return;
      this.m_gamma = colorspace["Gamma"] as PdfNumber;
    }
  }
}
