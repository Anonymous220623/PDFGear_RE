// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SynWTFilterIntLift5x3
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class SynWTFilterIntLift5x3 : SynWTFilterInt
{
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

  public override void synthetize_lpf(
    int[] lowSig,
    int lowOff,
    int lowLen,
    int lowStep,
    int[] highSig,
    int highOff,
    int highLen,
    int highStep,
    int[] outSig,
    int outOff,
    int outStep)
  {
    int num1 = lowLen + highLen;
    int num2 = 2 * outStep;
    int index1 = lowOff;
    int index2 = highOff;
    int index3 = outOff;
    outSig[index3] = num1 <= 1 ? lowSig[index1] : lowSig[index1] - (highSig[index2] + 1 >> 1);
    int index4 = index1 + lowStep;
    int index5 = index2 + highStep;
    int index6 = index3 + num2;
    for (int index7 = 2; index7 < num1 - 1; index7 += 2)
    {
      outSig[index6] = lowSig[index4] - (highSig[index5 - highStep] + highSig[index5] + 2 >> 2);
      index4 += lowStep;
      index5 += highStep;
      index6 += num2;
    }
    if (num1 % 2 == 1 && num1 > 2)
      outSig[index6] = lowSig[index4] - (2 * highSig[index5 - highStep] + 2 >> 2);
    int index8 = highOff;
    int index9 = outOff + outStep;
    for (int index10 = 1; index10 < num1 - 1; index10 += 2)
    {
      outSig[index9] = highSig[index8] + (outSig[index9 - outStep] + outSig[index9 + outStep] >> 1);
      index8 += highStep;
      index9 += num2;
    }
    if (num1 % 2 != 0 || num1 <= 1)
      return;
    outSig[index9] = highSig[index8] + outSig[index9 - outStep];
  }

  public override void synthetize_hpf(
    int[] lowSig,
    int lowOff,
    int lowLen,
    int lowStep,
    int[] highSig,
    int highOff,
    int highLen,
    int highStep,
    int[] outSig,
    int outOff,
    int outStep)
  {
    int num1 = lowLen + highLen;
    int num2 = 2 * outStep;
    int index1 = lowOff;
    int index2 = highOff;
    int index3 = outOff + outStep;
    for (int index4 = 1; index4 < num1 - 1; index4 += 2)
    {
      outSig[index3] = lowSig[index1] - (highSig[index2] + highSig[index2 + highStep] + 2 >> 2);
      index1 += lowStep;
      index2 += highStep;
      index3 += num2;
    }
    if (num1 > 1 && num1 % 2 == 0)
      outSig[index3] = lowSig[index1] - (2 * highSig[index2] + 2 >> 2);
    int index5 = highOff;
    int index6 = outOff;
    outSig[index6] = num1 <= 1 ? highSig[index5] >> 1 : highSig[index5] + outSig[index6 + outStep];
    int index7 = index5 + highStep;
    int index8 = index6 + num2;
    for (int index9 = 2; index9 < num1 - 1; index9 += 2)
    {
      outSig[index8] = highSig[index7] + (outSig[index8 - outStep] + outSig[index8 + outStep] >> 1);
      index7 += highStep;
      index8 += num2;
    }
    if (num1 % 2 != 1 || num1 <= 1)
      return;
    outSig[index8] = highSig[index7] + outSig[index8 - outStep];
  }

  public override bool isSameAsFullWT(int tailOvrlp, int headOvrlp, int inLen)
  {
    return inLen % 2 == 0 ? tailOvrlp >= 2 && headOvrlp >= 1 : tailOvrlp >= 2 && headOvrlp >= 2;
  }

  public override string ToString() => "w5x3 (lifting)";
}
