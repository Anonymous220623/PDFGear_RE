// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.SynWTFilter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal abstract class SynWTFilter : WaveletFilter
{
  public abstract int AnHighPosSupport { get; }

  public abstract int AnLowNegSupport { get; }

  public abstract int AnLowPosSupport { get; }

  public abstract bool Reversible { get; }

  public abstract int ImplType { get; }

  public abstract int SynHighNegSupport { get; }

  public abstract int SynHighPosSupport { get; }

  public abstract int AnHighNegSupport { get; }

  public abstract int DataType { get; }

  public abstract int SynLowNegSupport { get; }

  public abstract int SynLowPosSupport { get; }

  public abstract void synthetize_lpf(
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
    int outStep);

  public abstract void synthetize_hpf(
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
    int outStep);

  public abstract bool isSameAsFullWT(int param1, int param2, int param3);
}
