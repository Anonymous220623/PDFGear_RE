// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.AnWTFilterInt
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class AnWTFilterInt : AnWTFilter
{
  public override int DataType => 3;

  public abstract void analyze_lpf(
    int[] inSig,
    int inOff,
    int inLen,
    int inStep,
    int[] lowSig,
    int lowOff,
    int lowStep,
    int[] highSig,
    int highOff,
    int highStep);

  public override void analyze_lpf(
    object inSig,
    int inOff,
    int inLen,
    int inStep,
    object lowSig,
    int lowOff,
    int lowStep,
    object highSig,
    int highOff,
    int highStep)
  {
    this.analyze_lpf((int[]) inSig, inOff, inLen, inStep, (int[]) lowSig, lowOff, lowStep, (int[]) highSig, highOff, highStep);
  }

  public abstract void analyze_hpf(
    int[] inSig,
    int inOff,
    int inLen,
    int inStep,
    int[] lowSig,
    int lowOff,
    int lowStep,
    int[] highSig,
    int highOff,
    int highStep);

  public override void analyze_hpf(
    object inSig,
    int inOff,
    int inLen,
    int inStep,
    object lowSig,
    int lowOff,
    int lowStep,
    object highSig,
    int highOff,
    int highStep)
  {
    this.analyze_hpf((int[]) inSig, inOff, inLen, inStep, (int[]) lowSig, lowOff, lowStep, (int[]) highSig, highOff, highStep);
  }
}
