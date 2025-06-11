// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.StereoBalanceStrategy
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class StereoBalanceStrategy : IPanStrategy
{
  public StereoSamplePair GetMultipliers(float pan)
  {
    float num1 = (double) pan <= 0.0 ? 1f : (float) ((1.0 - (double) pan) / 2.0);
    float num2 = (double) pan >= 0.0 ? 1f : (float) (((double) pan + 1.0) / 2.0);
    return new StereoSamplePair()
    {
      Left = num1,
      Right = num2
    };
  }
}
