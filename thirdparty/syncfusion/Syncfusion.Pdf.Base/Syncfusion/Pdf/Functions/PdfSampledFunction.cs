// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Functions.PdfSampledFunction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Functions;

internal class PdfSampledFunction : PdfFunction
{
  internal PdfSampledFunction(float[] domain, float[] range, int[] sizes, byte[] samples)
    : this()
  {
    this.CheckParams(domain, range, sizes, (Array) samples);
    this.SetDomainAndRange(domain, range);
    this.SetSizeAndValues(sizes, samples);
  }

  internal PdfSampledFunction(float[] domain, float[] range, int[] sizes, int[] samples)
    : this()
  {
    this.CheckParams(domain, range, sizes, (Array) samples);
    this.SetDomainAndRange(domain, range);
    this.SetSizeAndValues(sizes, samples);
  }

  internal PdfSampledFunction(
    float[] domain,
    float[] range,
    int[] sizes,
    float[] samples,
    int bps)
    : this()
  {
    this.CheckParams(domain, range, sizes, (Array) samples);
    PdfDictionary dictionary = this.Dictionary;
  }

  private PdfSampledFunction()
    : base((PdfDictionary) new PdfStream())
  {
    this.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(0));
  }

  private void CheckParams(float[] domain, float[] range, int[] sizes, Array samples)
  {
    if (domain == null)
      throw new ArgumentNullException(nameof (domain));
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (samples == null)
      throw new ArgumentNullException(nameof (samples));
    int length1 = range.Length;
    int length2 = domain.Length;
    int length3 = samples.Length;
    if (length2 <= 0)
      throw new ArgumentException("The array has no enough elements", nameof (domain));
    if (length1 <= 0)
      throw new ArgumentException("The array has no enough elements", nameof (range));
    double num = (double) (length1 * length2 / 4);
    if ((double) length3 < num)
      throw new ArgumentException("There is no enough samples", nameof (samples));
  }

  private void SetDomainAndRange(float[] domain, float[] range)
  {
    this.Domain = new PdfArray(domain);
    this.Range = new PdfArray(range);
  }

  private void SetSizeAndValues(int[] sizes, byte[] samples)
  {
    PdfStream dictionary = this.Dictionary as PdfStream;
    this.Dictionary.SetProperty("Size", (IPdfPrimitive) new PdfArray(sizes));
    this.Dictionary.SetProperty("BitsPerSample", (IPdfPrimitive) new PdfNumber(8));
    dictionary.Write(samples);
  }

  private void SetSizeAndValues(int[] sizes, int[] samples)
  {
    PdfStream dictionary = this.Dictionary as PdfStream;
    this.Dictionary.SetProperty("Size", (IPdfPrimitive) new PdfArray(sizes));
    this.Dictionary.SetProperty("BitsPerSample", (IPdfPrimitive) new PdfNumber(32 /*0x20*/));
    byte[] data = new byte[samples.Length * 4];
    int index = 0;
    foreach (int sample in samples)
    {
      BitConverter.GetBytes(sample).CopyTo((Array) data, index);
      index += 4;
    }
    dictionary.Write(data);
  }
}
