// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.DeviceN
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class DeviceN : Colorspace
{
  private Colorspace m_alternateColorspace;
  private Function m_function;

  internal override int Components => 1;

  internal Colorspace AlternateColorspace
  {
    get => this.m_alternateColorspace;
    set => this.m_alternateColorspace = value;
  }

  internal Function Function
  {
    get => this.m_function;
    set => this.m_function = value;
  }

  internal override Color GetColor(string[] pars)
  {
    return this.AlternateColorspace.GetColor(this.AlternateColorspace.ToParams(this.Function.ColorTransferFunction(DeviceN.ToDouble(pars))));
  }

  internal override Color GetColor(byte[] bytes, int offset)
  {
    return Colorspace.GetRgbColor(bytes, offset);
  }

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    return new Pen(this.GetColor(pars)).Brush;
  }

  internal void SetValue(PdfArray array)
  {
    this.m_alternateColorspace = this.GetColorspace(array);
    this.m_function = Function.CreateFunction((IPdfPrimitive) array);
  }

  private Colorspace GetColorspace(PdfArray array)
  {
    PdfName pdfName = array[2] as PdfName;
    if (pdfName != (PdfName) null)
      return Colorspace.CreateColorSpace(pdfName.Value);
    PdfReferenceHolder pdfReferenceHolder = array[2] as PdfReferenceHolder;
    return pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfArray array1 ? Colorspace.CreateColorSpace((array1[0] as PdfName).Value, (IPdfPrimitive) array1) : (Colorspace) new DeviceRGB();
  }

  private static double[] ToDouble(string[] pars)
  {
    float[] numArray1 = new float[pars.Length];
    double[] numArray2 = new double[pars.Length];
    for (int index = 0; index < pars.Length; ++index)
    {
      float.TryParse(pars[index], out numArray1[index]);
      numArray2[index] = (double) numArray1[index];
    }
    return numArray2;
  }
}
