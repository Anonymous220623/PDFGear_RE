// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SynWTFilterFloatLift9x7
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class SynWTFilterFloatLift9x7 : SynWTFilterFloat
{
  public const float ALPHA = -1.58613431f;
  public const float BETA = -0.0529801175f;
  public const float GAMMA = 0.8829111f;
  public const float DELTA = 0.443506867f;
  public const float KL = 0.8128931f;
  public const float KH = 1.23017406f;

  public override int AnLowNegSupport => 4;

  public override int AnLowPosSupport => 4;

  public override int AnHighNegSupport => 3;

  public override int AnHighPosSupport => 3;

  public override int SynLowNegSupport => 3;

  public override int SynLowPosSupport => 3;

  public override int SynHighNegSupport => 4;

  public override int SynHighPosSupport => 4;

  public override int ImplType => WaveletFilter_Fields.WT_FILTER_FLOAT_LIFT;

  public override bool Reversible => false;

  public override void synthetize_lpf(
    float[] lowSig,
    int lowOff,
    int lowLen,
    int lowStep,
    float[] highSig,
    int highOff,
    int highLen,
    int highStep,
    float[] outSig,
    int outOff,
    int outStep)
  {
    int num1 = lowLen + highLen;
    int num2 = 2 * outStep;
    int index1 = lowOff;
    int index2 = highOff;
    int index3 = outOff;
    outSig[index3] = num1 <= 1 ? lowSig[index1] : (float) ((double) lowSig[index1] / 0.8128930926322937 - 0.88701373338699341 * (double) highSig[index2] / 1.2301740646362305);
    int index4 = index1 + lowStep;
    int index5 = index2 + highStep;
    int index6 = index3 + num2;
    int num3 = 2;
    while (num3 < num1 - 1)
    {
      outSig[index6] = (float) ((double) lowSig[index4] / 0.8128930926322937 - 0.4435068666934967 * ((double) highSig[index5 - highStep] + (double) highSig[index5]) / 1.2301740646362305);
      num3 += 2;
      index6 += num2;
      index4 += lowStep;
      index5 += highStep;
    }
    if (num1 % 2 == 1 && num1 > 2)
      outSig[index6] = (float) ((double) lowSig[index4] / 0.8128930926322937 - 0.88701373338699341 * (double) highSig[index5 - highStep] / 1.2301740646362305);
    int num4 = lowOff;
    int index7 = highOff;
    int index8 = outOff + outStep;
    int num5 = 1;
    while (num5 < num1 - 1)
    {
      outSig[index8] = (float) ((double) highSig[index7] / 1.2301740646362305 - 0.8829110860824585 * ((double) outSig[index8 - outStep] + (double) outSig[index8 + outStep]));
      num5 += 2;
      index8 += num2;
      index7 += highStep;
      num4 += lowStep;
    }
    if (num1 % 2 == 0)
      outSig[index8] = (float) ((double) highSig[index7] / 1.2301740646362305 - 1.765822172164917 * (double) outSig[index8 - outStep]);
    int index9 = outOff;
    if (num1 > 1)
      outSig[index9] -= -0.105960235f * outSig[index9 + outStep];
    int index10 = index9 + num2;
    int num6 = 2;
    while (num6 < num1 - 1)
    {
      outSig[index10] -= (float) (-0.052980117499828339 * ((double) outSig[index10 - outStep] + (double) outSig[index10 + outStep]));
      num6 += 2;
      index10 += num2;
    }
    if (num1 % 2 == 1 && num1 > 2)
      outSig[index10] -= -0.105960235f * outSig[index10 - outStep];
    int index11 = outOff + outStep;
    int num7 = 1;
    while (num7 < num1 - 1)
    {
      outSig[index11] -= (float) (-1.5861343145370483 * ((double) outSig[index11 - outStep] + (double) outSig[index11 + outStep]));
      num7 += 2;
      index11 += num2;
    }
    if (num1 % 2 != 0)
      return;
    outSig[index11] -= -3.17226863f * outSig[index11 - outStep];
  }

  public override void synthetize_hpf(
    float[] lowSig,
    int lowOff,
    int lowLen,
    int lowStep,
    float[] highSig,
    int highOff,
    int highLen,
    int highStep,
    float[] outSig,
    int outOff,
    int outStep)
  {
    int num1 = lowLen + highLen;
    int num2 = 2 * outStep;
    int index1 = lowOff;
    int index2 = highOff;
    if (num1 != 1)
    {
      int num3 = num1 >> 1;
      for (int index3 = 0; index3 < num3; ++index3)
      {
        lowSig[index1] /= 0.8128931f;
        highSig[index2] /= 1.23017406f;
        index1 += lowStep;
        index2 += highStep;
      }
      if (num1 % 2 == 1)
        highSig[index2] /= 1.23017406f;
    }
    else
      highSig[highOff] /= 2f;
    int index4 = lowOff;
    int index5 = highOff;
    int index6 = outOff + outStep;
    for (int index7 = 1; index7 < num1 - 1; index7 += 2)
    {
      outSig[index6] = lowSig[index4] - (float) (0.4435068666934967 * ((double) highSig[index5] + (double) highSig[index5 + highStep]));
      index6 += num2;
      index4 += lowStep;
      index5 += highStep;
    }
    if (num1 % 2 == 0 && num1 > 1)
      outSig[index6] = lowSig[index4] - 0.887013733f * highSig[index5];
    int index8 = highOff;
    int index9 = outOff;
    outSig[index9] = num1 <= 1 ? highSig[index8] : highSig[index8] - 1.76582217f * outSig[index9 + outStep];
    int index10 = index9 + num2;
    int index11 = index8 + highStep;
    for (int index12 = 2; index12 < num1 - 1; index12 += 2)
    {
      outSig[index10] = highSig[index11] - (float) (0.8829110860824585 * ((double) outSig[index10 - outStep] + (double) outSig[index10 + outStep]));
      index10 += num2;
      index11 += highStep;
    }
    if (num1 % 2 == 1 && num1 > 1)
      outSig[index10] = highSig[index11] - 1.76582217f * outSig[index10 - outStep];
    int index13 = outOff + outStep;
    for (int index14 = 1; index14 < num1 - 1; index14 += 2)
    {
      outSig[index13] -= (float) (-0.052980117499828339 * ((double) outSig[index13 - outStep] + (double) outSig[index13 + outStep]));
      index13 += num2;
    }
    if (num1 % 2 == 0 && num1 > 1)
      outSig[index13] -= -0.105960235f * outSig[index13 - outStep];
    int index15 = outOff;
    if (num1 > 1)
      outSig[index15] -= -3.17226863f * outSig[index15 + outStep];
    int index16 = index15 + num2;
    for (int index17 = 2; index17 < num1 - 1; index17 += 2)
    {
      outSig[index16] -= (float) (-1.5861343145370483 * ((double) outSig[index16 - outStep] + (double) outSig[index16 + outStep]));
      index16 += num2;
    }
    if (num1 % 2 != 1 || num1 <= 1)
      return;
    outSig[index16] -= -3.17226863f * outSig[index16 - outStep];
  }

  public override bool isSameAsFullWT(int tailOvrlp, int headOvrlp, int inLen)
  {
    return inLen % 2 == 0 ? tailOvrlp >= 2 && headOvrlp >= 1 : tailOvrlp >= 2 && headOvrlp >= 2;
  }

  public override string ToString() => "w9x7 (lifting)";
}
