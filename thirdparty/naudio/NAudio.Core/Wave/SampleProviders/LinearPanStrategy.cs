// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.LinearPanStrategy
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class LinearPanStrategy : IPanStrategy
{
  public StereoSamplePair GetMultipliers(float pan)
  {
    float num1 = (float) ((-(double) pan + 1.0) / 2.0);
    float num2 = num1;
    float num3 = 1f - num1;
    return new StereoSamplePair()
    {
      Left = num2,
      Right = num3
    };
  }
}
