// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Functions.PdfExponentialInterpolationFunction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Functions;

public class PdfExponentialInterpolationFunction : PdfFunction
{
  protected float[] m_c0;
  protected float[] m_c1;
  private float m_interpolationExp;

  public PdfExponentialInterpolationFunction(bool Init)
    : base(new PdfDictionary())
  {
    this.m_interpolationExp = 1f;
    this.Domain = new PdfArray(new float[2]{ 0.0f, 1f });
    this.Range = new PdfArray(new float[8]
    {
      0.0f,
      1f,
      0.0f,
      1f,
      0.0f,
      1f,
      0.0f,
      1f
    });
    this.m_interpolationExp = 1f;
    this.C0 = new float[4];
  }

  internal PdfExponentialInterpolationFunction()
    : base(new PdfDictionary())
  {
  }

  public float[] C0
  {
    get => this.m_c0;
    set => this.m_c0 = value;
  }

  public float[] C1
  {
    get => this.m_c1;
    set => this.m_c1 = value;
  }

  public float Exponent
  {
    get => this.m_interpolationExp;
    set => this.m_interpolationExp = value;
  }

  internal float[] InterpolationExponent(float[] singleArray1)
  {
    int length = this.Range.Count / 2;
    float[] numArray = new float[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = this.C0[index] + (float) Math.Pow((double) singleArray1[0], (double) this.m_interpolationExp) * (this.C1[index] - this.C0[index]);
    return numArray;
  }
}
