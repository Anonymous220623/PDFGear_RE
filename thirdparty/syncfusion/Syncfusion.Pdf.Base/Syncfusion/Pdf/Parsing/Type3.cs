// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Type3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Type3 : Function
{
  private PdfArray m_encode;
  private PdfArray m_bounds;
  private PdfArray m_pdfFunction;

  internal PdfArray Encode
  {
    get => this.m_encode;
    set => this.m_encode = value;
  }

  internal PdfArray Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal PdfArray PdfFunction
  {
    get => this.m_pdfFunction;
    set => this.m_pdfFunction = value;
  }

  public Type3(PdfDictionary dic)
    : base(dic)
  {
    this.m_bounds = dic.Items.ContainsKey(new PdfName(nameof (Bounds))) ? dic.Items[new PdfName(nameof (Bounds))] as PdfArray : (PdfArray) null;
    this.m_encode = dic.Items.ContainsKey(new PdfName(nameof (Encode))) ? dic.Items[new PdfName(nameof (Encode))] as PdfArray : (PdfArray) null;
    this.m_pdfFunction = dic.Items.ContainsKey(new PdfName("Functions")) ? dic.Items[new PdfName("Functions")] as PdfArray : (PdfArray) null;
  }

  protected override double[] PerformFunction(double[] inputData)
  {
    double x = inputData[0];
    int index;
    Function function = this.FindFunction(x, out index);
    double xMin = index <= 0 ? (double) (this.Domain[0] as PdfNumber).FloatValue : (double) (this.Bounds[index - 1] as PdfNumber).FloatValue;
    double xMax = index >= this.PdfFunction.Count - 1 ? (double) (this.Domain[1] as PdfNumber).FloatValue : (double) (this.Bounds[index] as PdfNumber).FloatValue;
    double floatValue1 = (double) (this.Encode[2 * index] as PdfNumber).FloatValue;
    double floatValue2 = (double) (this.Encode[2 * index + 1] as PdfNumber).FloatValue;
    return function.ColorTransferFunction(new double[1]
    {
      Function.FindIntermediateData(x, xMin, xMax, floatValue1, floatValue2)
    });
  }

  private Function FindFunction(double x, out int index)
  {
    for (int index1 = 0; index1 < this.Bounds.Count; ++index1)
    {
      double floatValue = (double) (this.Bounds[index1] as PdfNumber).FloatValue;
      if (x < floatValue)
      {
        index = index1;
        return Function.CreateFunction(this.PdfFunction[index1]);
      }
    }
    index = this.PdfFunction.Count - 1;
    return Function.CreateFunction(this.PdfFunction[index]);
  }
}
