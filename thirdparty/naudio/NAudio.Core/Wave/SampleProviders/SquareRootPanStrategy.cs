// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SquareRootPanStrategy
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SquareRootPanStrategy : IPanStrategy
{
  public StereoSamplePair GetMultipliers(float pan)
  {
    float d = (float) ((-(double) pan + 1.0) / 2.0);
    float num1 = (float) Math.Sqrt((double) d);
    float num2 = (float) Math.Sqrt(1.0 - (double) d);
    return new StereoSamplePair()
    {
      Left = num1,
      Right = num2
    };
  }
}
