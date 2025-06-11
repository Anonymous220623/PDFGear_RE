// Decompiled with JetBrains decompiler
// Type: SoundTouch.InterpolateCubic
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable enable
namespace SoundTouch;

internal sealed class InterpolateCubic : TransposerBase
{
  private static readonly float[] _coeffs = new float[16 /*0x10*/]
  {
    -0.5f,
    1f,
    -0.5f,
    0.0f,
    1.5f,
    -2.5f,
    0.0f,
    1f,
    -1.5f,
    2f,
    0.5f,
    0.0f,
    0.5f,
    -0.5f,
    0.0f,
    0.0f
  };
  private double _fract;

  public InterpolateCubic() => this._fract = 0.0;

  public override int Latency => 1;

  public override void ResetRegisters() => this._fract = 0.0;

  protected override int TransposeMono(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples)
  {
    ReadOnlySpan<float> readOnlySpan = src;
    int num1 = srcSamples - 4;
    int num2 = 0;
    int num3 = 0;
    int fract1;
    for (; num2 < num1; num2 += fract1)
    {
      float num4 = 1f;
      float fract2 = (float) this._fract;
      float num5 = fract2 * fract2;
      float num6 = num5 * fract2;
      double num7 = (double) InterpolateCubic._coeffs[0] * (double) num6 + (double) InterpolateCubic._coeffs[1] * (double) num5 + (double) InterpolateCubic._coeffs[2] * (double) fract2 + (double) InterpolateCubic._coeffs[3] * (double) num4;
      float num8 = (float) ((double) InterpolateCubic._coeffs[4] * (double) num6 + (double) InterpolateCubic._coeffs[5] * (double) num5 + (double) InterpolateCubic._coeffs[6] * (double) fract2 + (double) InterpolateCubic._coeffs[7] * (double) num4);
      float num9 = (float) ((double) InterpolateCubic._coeffs[8] * (double) num6 + (double) InterpolateCubic._coeffs[9] * (double) num5 + (double) InterpolateCubic._coeffs[10] * (double) fract2 + (double) InterpolateCubic._coeffs[11] * (double) num4);
      float num10 = (float) ((double) InterpolateCubic._coeffs[12] * (double) num6 + (double) InterpolateCubic._coeffs[13] * (double) num5 + (double) InterpolateCubic._coeffs[14] * (double) fract2 + (double) InterpolateCubic._coeffs[15] * (double) num4);
      double num11 = (double) readOnlySpan[0];
      float num12 = (float) (num7 * num11 + (double) num8 * (double) readOnlySpan[1] + (double) num9 * (double) readOnlySpan[2] + (double) num10 * (double) readOnlySpan[3]);
      dest[num3] = num12;
      ++num3;
      this._fract += this.Rate;
      fract1 = (int) this._fract;
      this._fract -= (double) fract1;
      readOnlySpan = readOnlySpan.Slice(fract1);
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
    int num1 = srcSamples - 4;
    int num2 = 0;
    int num3 = 0;
    int fract1;
    for (; num2 < num1; num2 += fract1)
    {
      float num4 = 1f;
      float fract2 = (float) this._fract;
      float num5 = fract2 * fract2;
      float num6 = num5 * fract2;
      double num7 = (double) InterpolateCubic._coeffs[0] * (double) num6 + (double) InterpolateCubic._coeffs[1] * (double) num5 + (double) InterpolateCubic._coeffs[2] * (double) fract2 + (double) InterpolateCubic._coeffs[3] * (double) num4;
      float num8 = (float) ((double) InterpolateCubic._coeffs[4] * (double) num6 + (double) InterpolateCubic._coeffs[5] * (double) num5 + (double) InterpolateCubic._coeffs[6] * (double) fract2 + (double) InterpolateCubic._coeffs[7] * (double) num4);
      float num9 = (float) ((double) InterpolateCubic._coeffs[8] * (double) num6 + (double) InterpolateCubic._coeffs[9] * (double) num5 + (double) InterpolateCubic._coeffs[10] * (double) fract2 + (double) InterpolateCubic._coeffs[11] * (double) num4);
      float num10 = (float) ((double) InterpolateCubic._coeffs[12] * (double) num6 + (double) InterpolateCubic._coeffs[13] * (double) num5 + (double) InterpolateCubic._coeffs[14] * (double) fract2 + (double) InterpolateCubic._coeffs[15] * (double) num4);
      float num11 = (float) (num7 * (double) readOnlySpan[0] + (double) num8 * (double) readOnlySpan[2] + (double) num9 * (double) readOnlySpan[4] + (double) num10 * (double) readOnlySpan[6]);
      float num12 = (float) (num7 * (double) readOnlySpan[1] + (double) num8 * (double) readOnlySpan[3] + (double) num9 * (double) readOnlySpan[5] + (double) num10 * (double) readOnlySpan[7]);
      dest[2 * num3] = num11;
      dest[2 * num3 + 1] = num12;
      ++num3;
      this._fract += this.Rate;
      fract1 = (int) this._fract;
      this._fract -= (double) fract1;
      readOnlySpan = readOnlySpan.Slice(2 * fract1);
    }
    srcSamples = num2;
    return num3;
  }

  protected override int TransposeMulti(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples)
  {
    ReadOnlySpan<float> readOnlySpan = src;
    Span<float> span = dest;
    int num1 = srcSamples - 4;
    int num2 = 0;
    int num3 = 0;
    int fract1;
    for (; num2 < num1; num2 += fract1)
    {
      float num4 = 1f;
      float fract2 = (float) this._fract;
      float num5 = fract2 * fract2;
      float num6 = num5 * fract2;
      float num7 = (float) ((double) InterpolateCubic._coeffs[0] * (double) num6 + (double) InterpolateCubic._coeffs[1] * (double) num5 + (double) InterpolateCubic._coeffs[2] * (double) fract2 + (double) InterpolateCubic._coeffs[3] * (double) num4);
      float num8 = (float) ((double) InterpolateCubic._coeffs[4] * (double) num6 + (double) InterpolateCubic._coeffs[5] * (double) num5 + (double) InterpolateCubic._coeffs[6] * (double) fract2 + (double) InterpolateCubic._coeffs[7] * (double) num4);
      float num9 = (float) ((double) InterpolateCubic._coeffs[8] * (double) num6 + (double) InterpolateCubic._coeffs[9] * (double) num5 + (double) InterpolateCubic._coeffs[10] * (double) fract2 + (double) InterpolateCubic._coeffs[11] * (double) num4);
      float num10 = (float) ((double) InterpolateCubic._coeffs[12] * (double) num6 + (double) InterpolateCubic._coeffs[13] * (double) num5 + (double) InterpolateCubic._coeffs[14] * (double) fract2 + (double) InterpolateCubic._coeffs[15] * (double) num4);
      for (int index = 0; index < this.NumberOfChannels; ++index)
      {
        float num11 = (float) ((double) num7 * (double) readOnlySpan[index] + (double) num8 * (double) readOnlySpan[index + this.NumberOfChannels] + (double) num9 * (double) readOnlySpan[index + 2 * this.NumberOfChannels] + (double) num10 * (double) readOnlySpan[index + 3 * this.NumberOfChannels]);
        span[0] = num11;
        span = span.Slice(1);
      }
      ++num3;
      this._fract += this.Rate;
      fract1 = (int) this._fract;
      this._fract -= (double) fract1;
      readOnlySpan = readOnlySpan.Slice(this.NumberOfChannels * fract1);
    }
    srcSamples = num2;
    return num3;
  }
}
