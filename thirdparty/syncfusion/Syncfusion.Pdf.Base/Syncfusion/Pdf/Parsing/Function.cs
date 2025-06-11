// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Function
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class Function
{
  private PdfArray m_domain;
  private PdfArray m_range;
  private PdfDictionary m_functionDictionary;

  internal PdfArray Domain
  {
    get => this.m_domain;
    set => this.m_domain = value;
  }

  internal PdfArray Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  public Function()
  {
  }

  public Function(PdfDictionary dictionary)
  {
    this.m_functionDictionary = dictionary;
    this.m_domain = dictionary.Items.ContainsKey(new PdfName(nameof (Domain))) ? dictionary.Items[new PdfName(nameof (Domain))] as PdfArray : (PdfArray) null;
    this.m_range = dictionary.Items.ContainsKey(new PdfName(nameof (Range))) ? dictionary.Items[new PdfName(nameof (Range))] as PdfArray : (PdfArray) null;
  }

  internal static Function CreateFunction(IPdfPrimitive array)
  {
    PdfDictionary function = Function.GetFunction(array);
    if (function != null && function.Items.ContainsKey(new PdfName("FunctionType")))
    {
      switch ((function.Items[new PdfName("FunctionType")] as PdfNumber).IntValue)
      {
        case 0:
          return (Function) new Type0(function);
        case 2:
          return (Function) new Type2(function);
        case 3:
          return (Function) new Type3(function);
        case 4:
          return (Function) new Type4(function);
      }
    }
    return (Function) null;
  }

  internal static Function CreateFunction(PdfDictionary array)
  {
    if (array.Items.ContainsKey(new PdfName("FunctionType")))
    {
      switch ((array.Items[new PdfName("FunctionType")] as PdfNumber).IntValue)
      {
        case 0:
          return (Function) new Type0(array);
        case 2:
          return (Function) new Type2(array);
        case 3:
          return (Function) new Type3(array);
        case 4:
          return (Function) new Type4(array);
      }
    }
    return (Function) null;
  }

  private static PdfDictionary GetFunction(IPdfPrimitive function)
  {
    PdfDictionary pdfDictionary;
    if (function is PdfArray pdfArray)
    {
      if (pdfArray[3] is PdfDictionary function1)
        return function1;
      PdfReferenceHolder pdfReferenceHolder = pdfArray[3] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        return pdfDictionary = pdfReferenceHolder.Object as PdfDictionary;
    }
    PdfReferenceHolder pdfReferenceHolder1 = function as PdfReferenceHolder;
    return pdfReferenceHolder1 != (PdfReferenceHolder) null ? (pdfDictionary = pdfReferenceHolder1.Object as PdfDictionary) : function as PdfDictionary;
  }

  internal double[] ColorTransferFunction(double[] inputValues)
  {
    return this.ExtractOutputData(this.PerformFunction(this.ExtractInputData(inputValues)));
  }

  protected abstract double[] PerformFunction(double[] clippedInputValues);

  private double[] ExtractInputData(double[] inputValues)
  {
    double[] inputData = new double[inputValues.Length];
    for (int index = 0; index < inputValues.Length; ++index)
    {
      double floatValue1 = (double) (this.Domain[2 * index] as PdfNumber).FloatValue;
      double floatValue2 = (double) (this.Domain[2 * index + 1] as PdfNumber).FloatValue;
      inputData[index] = Function.ExtractData(inputValues[index], floatValue1, floatValue2);
    }
    return inputData;
  }

  private double[] ExtractOutputData(double[] outputValues)
  {
    if (this.Range == null)
      return outputValues;
    double[] outputData = new double[outputValues.Length];
    for (int index = 0; index < outputValues.Length; ++index)
    {
      double floatValue1 = (double) (this.Range[2 * index] as PdfNumber).FloatValue;
      double floatValue2 = (double) (this.Range[2 * index + 1] as PdfNumber).FloatValue;
      outputData[index] = Function.ExtractData(outputValues[index], floatValue1, floatValue2);
    }
    return outputData;
  }

  internal static double FindIntermediateData(
    double x,
    double xMin,
    double xMax,
    double yMin,
    double yMax)
  {
    return yMin + (x - xMin) * ((yMax - yMin) / (xMax - xMin));
  }

  internal static double ExtractData(double value, double min, double max)
  {
    if (value < min)
      return min;
    return value > max ? max : value;
  }
}
