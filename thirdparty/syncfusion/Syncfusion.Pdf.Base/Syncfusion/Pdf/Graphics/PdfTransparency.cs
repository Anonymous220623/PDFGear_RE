// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTransparency
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PdfTransparency : IPdfWrapper
{
  private PdfDictionary m_dictionary = new PdfDictionary();

  public float Stroke => this.GetNumber("CA");

  public float Fill => this.GetNumber("ca");

  public PdfBlendMode Mode
  {
    get => (PdfBlendMode) Enum.Parse(typeof (PdfBlendMode), this.GetName("ca"), true);
  }

  public PdfTransparency(float stroke, float fill, PdfBlendMode mode)
  {
    if ((double) stroke < 0.0)
      throw new ArgumentOutOfRangeException(nameof (stroke), "The value can't be less then zero.");
    if ((double) fill < 0.0)
      throw new ArgumentOutOfRangeException(nameof (fill), "The value can't be less then zero.");
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1A)
    {
      stroke = (double) stroke == 0.0 ? 1f : stroke;
      fill = (double) fill == 0.0 ? 1f : fill;
      mode = mode != PdfBlendMode.Normal ? PdfBlendMode.Normal : mode;
    }
    this.m_dictionary.SetNumber("CA", stroke);
    this.m_dictionary.SetNumber("ca", fill);
    this.m_dictionary.SetName("BM", mode.ToString());
  }

  public override bool Equals(object obj)
  {
    bool flag = false;
    if (obj != null && obj is PdfTransparency pdfTransparency)
      flag = true & (double) pdfTransparency.Stroke != (double) this.Stroke & (double) pdfTransparency.Fill != (double) this.Fill & pdfTransparency.Mode != this.Mode;
    return flag;
  }

  public override int GetHashCode() => base.GetHashCode();

  private float GetNumber(string keyName)
  {
    float number = 0.0f;
    if (this.m_dictionary[keyName] is PdfNumber pdfNumber)
      number = pdfNumber.FloatValue;
    return number;
  }

  private string GetName(string keyName)
  {
    string name = (string) null;
    PdfName pdfName = this.m_dictionary[keyName] as PdfName;
    if (pdfName != (PdfName) null)
      name = pdfName.Value;
    return name;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
