// Decompiled with JetBrains decompiler
// Type: SoundTouch.InterpolateShannon
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable enable
namespace SoundTouch;

internal sealed class InterpolateShannon : TransposerBase
{
  private const double PI = 3.1415926535897931;
  private static readonly double[] _kaiser8 = new double[8]
  {
    0.41778693317814,
    0.64888025049173,
    0.83508562409944,
    0.93887857733412,
    0.93887857733412,
    0.83508562409944,
    0.64888025049173,
    0.41778693317814
  };
  private double _fract;

  public InterpolateShannon() => this._fract = 0.0;

  public override int Latency => 3;

  public override void ResetRegisters() => this._fract = 0.0;

  protected override int TransposeMono(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples)
  {
    ReadOnlySpan<float> readOnlySpan = src;
    int num1 = srcSamples - 8;
    int num2 = 0;
    int num3 = 0;
    int fract;
    for (; num2 < num1; num2 += fract)
    {
      double num4 = (double) readOnlySpan[0] * InterpolateShannon.Sinc(-3.0 - this._fract) * InterpolateShannon._kaiser8[0] + (double) readOnlySpan[1] * InterpolateShannon.Sinc(-2.0 - this._fract) * InterpolateShannon._kaiser8[1] + (double) readOnlySpan[2] * InterpolateShannon.Sinc(-1.0 - this._fract) * InterpolateShannon._kaiser8[2];
      double num5 = (this._fract >= 1E-06 ? num4 + (double) readOnlySpan[3] * InterpolateShannon.Sinc(-this._fract) * InterpolateShannon._kaiser8[3] : num4 + (double) readOnlySpan[3] * InterpolateShannon._kaiser8[3]) + (double) readOnlySpan[4] * InterpolateShannon.Sinc(1.0 - this._fract) * InterpolateShannon._kaiser8[4] + (double) readOnlySpan[5] * InterpolateShannon.Sinc(2.0 - this._fract) * InterpolateShannon._kaiser8[5] + (double) readOnlySpan[6] * InterpolateShannon.Sinc(3.0 - this._fract) * InterpolateShannon._kaiser8[6] + (double) readOnlySpan[7] * InterpolateShannon.Sinc(4.0 - this._fract) * InterpolateShannon._kaiser8[7];
      dest[num3] = (float) num5;
      ++num3;
      this._fract += this.Rate;
      fract = (int) this._fract;
      this._fract -= (double) fract;
      readOnlySpan = readOnlySpan.Slice(fract);
    }
    srcSamples = num2;
    return num3;
  }

  protected override int TransposeStereo(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples)
  {
    ReadOnlySpan<float> readOnlySpan = src;
    int num1 = srcSamples - 8;
    int num2 = 0;
    int num3 = 0;
    int fract;
    for (; num2 < num1; num2 += fract)
    {
      double num4 = InterpolateShannon.Sinc(-3.0 - this._fract) * InterpolateShannon._kaiser8[0];
      double num5 = (double) readOnlySpan[0] * num4;
      double num6 = (double) readOnlySpan[1] * num4;
      double num7 = InterpolateShannon.Sinc(-2.0 - this._fract) * InterpolateShannon._kaiser8[1];
      double num8 = num5 + (double) readOnlySpan[2] * num7;
      double num9 = num6 + (double) readOnlySpan[3] * num7;
      double num10 = InterpolateShannon.Sinc(-1.0 - this._fract) * InterpolateShannon._kaiser8[2];
      double num11 = num8 + (double) readOnlySpan[4] * num10;
      double num12 = num9 + (double) readOnlySpan[5] * num10;
      double num13 = InterpolateShannon._kaiser8[3] * (this._fract < 1E-05 ? 1.0 : InterpolateShannon.Sinc(-this._fract));
      double num14 = num11 + (double) readOnlySpan[6] * num13;
      double num15 = num12 + (double) readOnlySpan[7] * num13;
      double num16 = InterpolateShannon.Sinc(1.0 - this._fract) * InterpolateShannon._kaiser8[4];
      double num17 = num14 + (double) readOnlySpan[8] * num16;
      double num18 = num15 + (double) readOnlySpan[9] * num16;
      double num19 = InterpolateShannon.Sinc(2.0 - this._fract) * InterpolateShannon._kaiser8[5];
      double num20 = num17 + (double) readOnlySpan[10] * num19;
      double num21 = num18 + (double) readOnlySpan[11] * num19;
      double num22 = InterpolateShannon.Sinc(3.0 - this._fract) * InterpolateShannon._kaiser8[6];
      double num23 = num20 + (double) readOnlySpan[12] * num22;
      double num24 = num21 + (double) readOnlySpan[13] * num22;
      double num25 = InterpolateShannon.Sinc(4.0 - this._fract) * InterpolateShannon._kaiser8[7];
      double num26 = num23 + (double) readOnlySpan[14] * num25;
      double num27 = num24 + (double) readOnlySpan[15] * num25;
      dest[2 * num3] = (float) num26;
      dest[2 * num3 + 1] = (float) num27;
      ++num3;
      this._fract += this.Rate;
      fract = (int) this._fract;
      this._fract -= (double) fract;
      readOnlySpan = readOnlySpan.Slice(2 * fract);
    }
    srcSamples = num2;
    return num3;
  }

  protected override int TransposeMulti(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples)
  {
    throw new NotImplementedException();
  }

  private static double Sinc(double x) => Math.Sin(Math.PI * x) / (Math.PI * x);
}
