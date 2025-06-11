// Decompiled with JetBrains decompiler
// Type: SoundTouch.AntiAliasFilter
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;
using System.Diagnostics;
using System.IO;

#nullable enable
namespace SoundTouch;

internal sealed class AntiAliasFilter
{
  private const double PI = 3.1415926535897931;
  private const double DOUBLE_PI = 6.2831853071795862;
  private readonly FirFilter _firFilter;
  private double _cutoffFreq;
  private int _length;

  public AntiAliasFilter(int length)
  {
    this._firFilter = new FirFilter();
    this._cutoffFreq = 0.5;
    this.Length = length;
  }

  public int Length
  {
    get => this._length;
    set
    {
      this._length = value;
      this.CalculateCoefficients();
    }
  }

  public void SetCutoffFreq(double newCutoffFreq)
  {
    this._cutoffFreq = newCutoffFreq;
    this.CalculateCoefficients();
  }

  public int Evaluate(
    Span<float> dest,
    in ReadOnlySpan<float> src,
    int numSamples,
    int numChannels)
  {
    return this._firFilter.Evaluate(in dest, in src, numSamples, numChannels);
  }

  public int Evaluate(in FifoSampleBuffer destinationBuffer, FifoSampleBuffer sourceBuffer)
  {
    if (sourceBuffer == null)
      throw new ArgumentNullException(nameof (sourceBuffer));
    if (destinationBuffer == null)
      throw new ArgumentNullException(nameof (destinationBuffer));
    int channels = sourceBuffer.Channels;
    int availableSamples = sourceBuffer.AvailableSamples;
    Span<float> span = sourceBuffer.PtrBegin();
    Span<float> dest = destinationBuffer.PtrEnd(availableSamples);
    int num = this._firFilter.Evaluate(in dest, Span<float>.op_Implicit(span), availableSamples, channels);
    sourceBuffer.ReceiveSamples(num);
    destinationBuffer.PutSamples(num);
    return num;
  }

  [Conditional("_DEBUG_SAVE_AAFILTER_COEFFICIENTS")]
  private static void DebugSaveAntiAliasFilterCoefficients(in ReadOnlySpan<float> coefficients)
  {
    using (FileStream fileStream = File.Open("aa_filter_coeffs.txt", FileMode.Truncate))
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
      {
        ReadOnlySpan<float> readOnlySpan = coefficients;
        for (int index = 0; index < readOnlySpan.Length; ++index)
        {
          float num = readOnlySpan[index];
          streamWriter.WriteLine(num);
        }
      }
    }
  }

  private void CalculateCoefficients()
  {
    Span<double> span1 = stackalloc double[this._length];
    Span<float> span2 = stackalloc float[this._length];
    double num1 = 2.0 * Math.PI * this._cutoffFreq;
    double num2 = 2.0 * Math.PI / (double) this._length;
    double num3 = 0.0;
    for (int index = 0; index < this._length; ++index)
    {
      double num4 = (double) index - (double) this._length / 2.0;
      double a = num4 * num1;
      double num5 = a == 0.0 ? 1.0 : Math.Sin(a) / a;
      double num6 = (0.54 + 0.46 * Math.Cos(num2 * num4)) * num5;
      span1[index] = num6;
      num3 += num6;
    }
    double num7 = 16384.0 / num3;
    for (int index = 0; index < this._length; ++index)
    {
      double num8 = span1[index] * num7;
      double num9 = num8 + (num8 >= 0.0 ? 0.5 : -0.5);
      span2[index] = (float) num9;
    }
    this._firFilter.SetCoefficients(Span<float>.op_Implicit(span2), 14);
  }
}
