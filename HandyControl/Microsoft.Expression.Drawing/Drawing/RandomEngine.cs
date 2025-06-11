// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.RandomEngine
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal class RandomEngine
{
  private double? _anotherSample;
  private Random _random;

  public RandomEngine(long seed) => this.Initialize(seed);

  private double Gaussian()
  {
    if (this._anotherSample.HasValue)
    {
      double num = this._anotherSample.Value;
      this._anotherSample = new double?();
      return num;
    }
    double num1;
    double num2;
    double d;
    do
    {
      num1 = 2.0 * this.Uniform() - 1.0;
      num2 = 2.0 * this.Uniform() - 1.0;
      d = num1 * num1 + num2 * num2;
    }
    while (d >= 1.0);
    double num3 = Math.Sqrt(-2.0 * Math.Log(d) / d);
    this._anotherSample = new double?(num1 * num3);
    return num2 * num3;
  }

  private void Initialize(long seed) => this._random = new Random((int) seed);

  public double NextGaussian(double mean, double variance) => this.Gaussian() * variance + mean;

  public double NextUniform(double min, double max) => this.Uniform() * (max - min) + min;

  private double Uniform() => this._random.NextDouble();
}
