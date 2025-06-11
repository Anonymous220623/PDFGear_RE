// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Type2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Type2 : Function
{
  private PdfArray m_c0;
  private PdfArray m_c1;
  private PdfNumber m_n;
  private PdfDictionary m_functionResource;

  internal PdfDictionary FunctionResource
  {
    get => this.m_functionResource;
    set => this.m_functionResource = value;
  }

  internal PdfArray C0
  {
    get => this.m_c0;
    set => this.m_c0 = value;
  }

  internal PdfArray C1
  {
    get => this.m_c1;
    set => this.m_c1 = value;
  }

  internal PdfNumber N
  {
    get => this.m_n;
    set => this.m_n = value;
  }

  internal int ResultantValue => this.C0.Count;

  public Type2()
  {
  }

  public Type2(PdfDictionary functionDictionary)
    : base(functionDictionary)
  {
    this.m_functionResource = functionDictionary;
    this.m_c0 = this.m_functionResource.Items.ContainsKey(new PdfName(nameof (C0))) ? this.m_functionResource.Items[new PdfName(nameof (C0))] as PdfArray : new PdfArray(new double[1]);
    PdfArray pdfArray;
    if (!this.m_functionResource.Items.ContainsKey(new PdfName(nameof (C1))))
      pdfArray = new PdfArray(new double[1]{ 1.0 });
    else
      pdfArray = this.m_functionResource.Items[new PdfName(nameof (C1))] as PdfArray;
    this.m_c1 = pdfArray;
    this.m_n = this.m_functionResource.Items.ContainsKey(new PdfName(nameof (N))) ? this.m_functionResource.Items[new PdfName(nameof (N))] as PdfNumber : new PdfNumber(2.0);
  }

  protected override double[] PerformFunction(double[] inputData)
  {
    double[] numArray1 = new double[inputData.Length * this.ResultantValue];
    for (int index1 = 0; index1 < inputData.Length; ++index1)
    {
      double[] numArray2 = this.PerformFunctionSingleValue(inputData[index1]);
      for (int index2 = 0; index2 < numArray2.Length; ++index2)
        numArray1[index1 * this.ResultantValue + index2] = numArray2[index2];
    }
    return numArray1;
  }

  private double[] PerformFunctionSingleValue(double x)
  {
    double[] numArray = new double[this.ResultantValue];
    for (int index = 0; index < numArray.Length; ++index)
    {
      double floatValue1 = (double) (this.C0[index] as PdfNumber).FloatValue;
      double floatValue2 = (double) (this.C1[index] as PdfNumber).FloatValue;
      numArray[index] = this.CalculateExponentialInterpolation(floatValue1, floatValue2, x);
    }
    return numArray;
  }

  private double CalculateExponentialInterpolation(double c0, double c1, double x)
  {
    return c0 + Math.Pow(x, (double) this.N.FloatValue) * (c1 - c0);
  }
}
