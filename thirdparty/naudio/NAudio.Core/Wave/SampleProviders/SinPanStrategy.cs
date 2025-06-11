// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SinPanStrategy
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SinPanStrategy : IPanStrategy
{
  private const float HalfPi = 1.57079637f;

  public StereoSamplePair GetMultipliers(float pan)
  {
    double num1 = (-(double) pan + 1.0) / 2.0;
    float num2 = (float) Math.Sin(num1 * 1.5707963705062866);
    float num3 = (float) Math.Cos(num1 * 1.5707963705062866);
    return new StereoSamplePair()
    {
      Left = num2,
      Right = num3
    };
  }
}
