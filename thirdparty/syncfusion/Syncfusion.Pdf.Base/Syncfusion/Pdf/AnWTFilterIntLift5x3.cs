// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.AnWTFilterIntLift5x3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class AnWTFilterIntLift5x3 : AnWTFilterInt
{
  private static readonly float[] LPSynthesisFilter = new float[3]
  {
    0.5f,
    1f,
    0.5f
  };
  private static readonly float[] HPSynthesisFilter = new float[5]
  {
    -0.125f,
    -0.25f,
    0.75f,
    -0.25f,
    -0.125f
  };

  public override int AnLowNegSupport => 2;

  public override int AnLowPosSupport => 2;

  public override int AnHighNegSupport => 1;

  public override int AnHighPosSupport => 1;

  public override int SynLowNegSupport => 1;

  public override int SynLowPosSupport => 1;

  public override int SynHighNegSupport => 2;

  public override int SynHighPosSupport => 2;

  public override int ImplType => WaveletFilter_Fields.WT_FILTER_INT_LIFT;

  public override bool Reversible => true;

  public override int FilterType => 1;

  public override void analyze_lpf(
    int[] inSig,
    int inOff,
    int inLen,
    int inStep,
    int[] lowSig,
    int lowOff,
    int lowStep,
    int[] highSig,
    int highOff,
    int highStep)
  {
    int num = 2 * inStep;
    int index1 = inOff + inStep;
    int index2 = highOff;
    for (int index3 = 1; index3 < inLen - 1; index3 += 2)
    {
      highSig[index2] = inSig[index1] - (inSig[index1 - inStep] + inSig[index1 + inStep] >> 1);
      index1 += num;
      index2 += highStep;
    }
    if (inLen % 2 == 0)
      highSig[index2] = inSig[index1] - (2 * inSig[index1 - inStep] >> 1);
    int index4 = inOff;
    int index5 = lowOff;
    int index6 = highOff;
    lowSig[index5] = inLen <= 1 ? inSig[index4] : inSig[index4] + (highSig[index6] + 1 >> 1);
    int index7 = index4 + num;
    int index8 = index5 + lowStep;
    int index9 = index6 + highStep;
    for (int index10 = 2; index10 < inLen - 1; index10 += 2)
    {
      lowSig[index8] = inSig[index7] + (highSig[index9 - highStep] + highSig[index9] + 2 >> 2);
      index7 += num;
      index8 += lowStep;
      index9 += highStep;
    }
    if (inLen % 2 != 1 || inLen <= 2)
      return;
    lowSig[index8] = inSig[index7] + (2 * highSig[index9 - highStep] + 2 >> 2);
  }

  public override void analyze_hpf(
    int[] inSig,
    int inOff,
    int inLen,
    int inStep,
    int[] lowSig,
    int lowOff,
    int lowStep,
    int[] highSig,
    int highOff,
    int highStep)
  {
    int num = 2 * inStep;
    int index1 = inOff;
    int index2 = highOff;
    highSig[index2] = inLen <= 1 ? inSig[index1] << 1 : inSig[index1] - inSig[index1 + inStep];
    int index3 = index1 + num;
    int index4 = index2 + highStep;
    if (inLen > 3)
    {
      for (int index5 = 2; index5 < inLen - 1; index5 += 2)
      {
        highSig[index4] = inSig[index3] - (inSig[index3 - inStep] + inSig[index3 + inStep] >> 1);
        index3 += num;
        index4 += highStep;
      }
    }
    if (inLen % 2 == 1 && inLen > 1)
      highSig[index4] = inSig[index3] - inSig[index3 - inStep];
    int index6 = inOff + inStep;
    int index7 = lowOff;
    int index8 = highOff;
    for (int index9 = 1; index9 < inLen - 1; index9 += 2)
    {
      lowSig[index7] = inSig[index6] + (highSig[index8] + highSig[index8 + highStep] + 2 >> 2);
      index6 += num;
      index7 += lowStep;
      index8 += highStep;
    }
    if (inLen <= 1 || inLen % 2 != 0)
      return;
    lowSig[index7] = inSig[index6] + (2 * highSig[index8] + 2 >> 2);
  }

  public override float[] getLPSynthesisFilter() => AnWTFilterIntLift5x3.LPSynthesisFilter;

  public override float[] getHPSynthesisFilter() => AnWTFilterIntLift5x3.HPSynthesisFilter;

  public override bool isSameAsFullWT(int tailOvrlp, int headOvrlp, int inLen)
  {
    return inLen % 2 == 0 ? tailOvrlp >= 2 && headOvrlp >= 1 : tailOvrlp >= 2 && headOvrlp >= 2;
  }

  public override bool Equals(object obj) => obj == this || obj is AnWTFilterIntLift5x3;

  public override string ToString() => "w5x3";

  public override int GetHashCode() => base.GetHashCode();
}
