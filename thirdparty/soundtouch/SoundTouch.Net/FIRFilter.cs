// Decompiled with JetBrains decompiler
// Type: SoundTouch.FirFilter
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using SoundTouch.Assets;
using System;

#nullable enable
namespace SoundTouch;

internal class FirFilter
{
  private float[]? _filterCoeffs;
  private float[]? _filterCoeffsStereo;

  public FirFilter()
  {
    this.Length = 0;
    this._filterCoeffs = (float[]) null;
    this._filterCoeffsStereo = (float[]) null;
  }

  public int Length { get; private set; }

  public virtual void SetCoefficients(in ReadOnlySpan<float> coeffs, int resultDivFactor)
  {
    if (coeffs.IsEmpty)
      throw new ArgumentException(Strings.Argument_EmptyCoefficients, nameof (coeffs));
    if (coeffs.Length % 8 != 0)
      throw new ArgumentException(Strings.Argument_CoefficientsFilterNotDivisible, nameof (coeffs));
    this.Length = coeffs.Length;
    double num = 1.0 / Math.Pow(2.0, (double) resultDivFactor);
    this._filterCoeffs = new float[this.Length];
    this._filterCoeffsStereo = new float[this.Length * 2];
    for (int index = 0; index < this.Length; ++index)
    {
      this._filterCoeffs[index] = coeffs[index] * (float) num;
      this._filterCoeffsStereo[2 * index] = coeffs[index] * (float) num;
      this._filterCoeffsStereo[2 * index + 1] = coeffs[index] * (float) num;
    }
  }

  public int Evaluate(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    int numSamples,
    int numChannels)
  {
    if (this.Length <= 0)
      throw new InvalidOperationException(Strings.InvalidOperation_CoefficientsNotInitialized);
    if (numSamples < this.Length)
      return 0;
    if (numChannels == 1)
      return this.EvaluateFilterMono(in dest, in src, numSamples);
    return numChannels == 2 ? this.EvaluateFilterStereo(in dest, in src, numSamples) : this.EvaluateFilterMulti(in dest, in src, numSamples, numChannels);
  }

  protected virtual int EvaluateFilterStereo(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    int numSamples)
  {
    if (this.Length <= 0 || this._filterCoeffsStereo == null)
      throw new InvalidOperationException(Strings.InvalidOperation_CoefficientsNotInitialized);
    int num1 = this.Length & -8;
    int num2 = 2 * (numSamples - num1);
    for (int index1 = 0; index1 < num2; index1 += 2)
    {
      double num3 = 0.0;
      double num4 = 0.0;
      ReadOnlySpan<float> readOnlySpan = src.Slice(index1);
      for (int index2 = 0; index2 < num1; ++index2)
      {
        num3 += (double) readOnlySpan[2 * index2] * (double) this._filterCoeffsStereo[2 * index2];
        num4 += (double) readOnlySpan[2 * index2 + 1] * (double) this._filterCoeffsStereo[2 * index2 + 1];
      }
      dest[index1] = (float) num3;
      dest[index1 + 1] = (float) num4;
    }
    return numSamples - num1;
  }

  protected virtual int EvaluateFilterMono(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    int numSamples)
  {
    if (this.Length <= 0 || this._filterCoeffs == null)
      throw new InvalidOperationException(Strings.InvalidOperation_CoefficientsNotInitialized);
    int num1 = this.Length & -8;
    int filterMono = numSamples - num1;
    for (int index1 = 0; index1 < filterMono; ++index1)
    {
      ReadOnlySpan<float> readOnlySpan = src.Slice(index1);
      double num2 = 0.0;
      for (int index2 = 0; index2 < num1; ++index2)
        num2 += (double) readOnlySpan[index2] * (double) this._filterCoeffs[index2];
      dest[index1] = (float) num2;
    }
    return filterMono;
  }

  protected virtual int EvaluateFilterMulti(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    int numSamples,
    int numChannels)
  {
    if (numChannels >= 16 /*0x10*/)
      throw new ArgumentOutOfRangeException(Strings.Argument_IllegalNumberOfChannels);
    if (this.Length <= 0 || this._filterCoeffs == null)
      throw new InvalidOperationException(Strings.InvalidOperation_CoefficientsNotInitialized);
    int num1 = this.Length & -8;
    int num2 = numChannels * (numSamples - num1);
    for (int index1 = 0; index1 < num2; index1 += numChannels)
    {
      Span<double> span = stackalloc double[16 /*0x10*/];
      for (int index2 = 0; index2 < numChannels; ++index2)
        span[index2] = 0.0;
      ReadOnlySpan<float> readOnlySpan = src.Slice(index1);
      for (int index3 = 0; index3 < num1; ++index3)
      {
        float filterCoeff = this._filterCoeffs[index3];
        for (int index4 = 0; index4 < numChannels; ++index4)
        {
          span[index4] += (double) readOnlySpan[0] * (double) filterCoeff;
          readOnlySpan = readOnlySpan.Slice(1);
        }
      }
      for (int index5 = 0; index5 < numChannels; ++index5)
        dest[index1 + index5] = (float) span[index5];
    }
    return numSamples - num1;
  }
}
