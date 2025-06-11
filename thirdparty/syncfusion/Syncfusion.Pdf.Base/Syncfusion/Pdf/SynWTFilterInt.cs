// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SynWTFilterInt
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class SynWTFilterInt : SynWTFilter
{
  public override int DataType => 3;

  public abstract void synthetize_lpf(
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
    int outStep);

  public override void synthetize_lpf(
    object lowSig,
    int lowOff,
    int lowLen,
    int lowStep,
    object highSig,
    int highOff,
    int highLen,
    int highStep,
    object outSig,
    int outOff,
    int outStep)
  {
    this.synthetize_lpf((int[]) lowSig, lowOff, lowLen, lowStep, (int[]) highSig, highOff, highLen, highStep, (int[]) outSig, outOff, outStep);
  }

  public abstract void synthetize_hpf(
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
    int outStep);

  public override void synthetize_hpf(
    object lowSig,
    int lowOff,
    int lowLen,
    int lowStep,
    object highSig,
    int highOff,
    int highLen,
    int highStep,
    object outSig,
    int outOff,
    int outStep)
  {
    this.synthetize_hpf((int[]) lowSig, lowOff, lowLen, lowStep, (int[]) highSig, highOff, highLen, highStep, (int[]) outSig, outOff, outStep);
  }
}
