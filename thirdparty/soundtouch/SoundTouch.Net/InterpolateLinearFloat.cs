// Decompiled with JetBrains decompiler
// Type: SoundTouch.InterpolateLinearFloat
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable disable
namespace SoundTouch;

internal sealed class InterpolateLinearFloat : TransposerBase
{
  private double _fract;

  public InterpolateLinearFloat()
  {
    this.ResetRegisters();
    this.SetRate(1.0);
  }

  public override int Latency => 0;

  public override void ResetRegisters() => this._fract = 0.0;

  protected override int TransposeMono(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples)
  {
    ReadOnlySpan<float> readOnlySpan = src;
    int num1 = srcSamples - 1;
    int num2 = 0;
    int num3 = 0;
    int fract;
    for (; num2 < num1; num2 += fract)
    {
      double num4 = (1.0 - this._fract) * (double) readOnlySpan[0] + this._fract * (double) readOnlySpan[1];
      dest[num3] = (float) num4;
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
    int num1 = srcSamples - 1;
    int num2 = 0;
    int num3 = 0;
    int fract;
    for (; num2 < num1; num2 += fract)
    {
      double num4 = (1.0 - this._fract) * (double) readOnlySpan[0] + this._fract * (double) readOnlySpan[2];
      double num5 = (1.0 - this._fract) * (double) readOnlySpan[1] + this._fract * (double) readOnlySpan[3];
      dest[2 * num3] = (float) num4;
      dest[2 * num3 + 1] = (float) num5;
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
    ReadOnlySpan<float> readOnlySpan = src;
    Span<float> span = dest;
    int num1 = srcSamples - 1;
    int num2 = 0;
    int num3 = 0;
    while (num2 < num1)
    {
      float num4 = (float) (1.0 - this._fract);
      float fract1 = (float) this._fract;
      for (int index = 0; index < this.NumberOfChannels; ++index)
      {
        float num5 = (float) ((double) num4 * (double) readOnlySpan[index] + (double) fract1 * (double) readOnlySpan[index + this.NumberOfChannels]);
        span[0] = num5;
        span = span.Slice(1);
      }
      ++num3;
      this._fract += this.Rate;
      int fract2 = (int) this._fract;
      this._fract -= (double) fract2;
      num2 += fract2;
      readOnlySpan = readOnlySpan.Slice(fract2 * this.NumberOfChannels);
    }
    srcSamples = num2;
    return num3;
  }
}
