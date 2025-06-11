// Decompiled with JetBrains decompiler
// Type: SoundTouch.IIR2Filter
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable enable
namespace SoundTouch;

internal class IIR2Filter
{
  private readonly double[] _coeffs;
  private readonly double[] _prev;

  public IIR2Filter(in Span<double> coeffs)
  {
    this._coeffs = new double[5];
    this._prev = new double[5];
    coeffs.CopyTo(Span<double>.op_Implicit(this._coeffs));
  }

  public float Update(float x)
  {
    this._prev[0] = (double) x;
    double num = (double) x * this._coeffs[0];
    for (int index = 4; index >= 1; --index)
    {
      num += this._coeffs[index] * this._prev[index];
      this._prev[index] = this._prev[index - 1];
    }
    this._prev[3] = num;
    return (float) num;
  }
}
