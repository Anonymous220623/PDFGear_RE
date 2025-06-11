// Decompiled with JetBrains decompiler
// Type: SoundTouch.TransposerBase
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable enable
namespace SoundTouch;

internal abstract class TransposerBase
{
  private static TransposerBase.Algorithm _algorithm = TransposerBase.Algorithm.Cubic;

  protected TransposerBase()
  {
    this.NumberOfChannels = 0;
    this.Rate = 1.0;
  }

  public double Rate { get; private set; }

  public int NumberOfChannels { get; private set; }

  public virtual int Latency => 0;

  public static TransposerBase CreateInstance()
  {
    switch (TransposerBase._algorithm)
    {
      case TransposerBase.Algorithm.Linear:
        return (TransposerBase) new InterpolateLinearFloat();
      case TransposerBase.Algorithm.Cubic:
        return (TransposerBase) new InterpolateCubic();
      case TransposerBase.Algorithm.Shannon:
        return (TransposerBase) new InterpolateShannon();
      default:
        throw new NotSupportedException();
    }
  }

  public static void SetAlgorithm(TransposerBase.Algorithm a) => TransposerBase._algorithm = a;

  public virtual int Transpose(in FifoSampleBuffer dest, in FifoSampleBuffer src)
  {
    int availableSamples = src.AvailableSamples;
    int slackCapacity = (int) ((double) availableSamples / this.Rate) + 8;
    Span<float> span = src.PtrBegin();
    Span<float> dest1 = dest.PtrEnd(slackCapacity);
    int numSamples = this.NumberOfChannels != 1 ? (this.NumberOfChannels != 2 ? this.TransposeMulti(in dest1, Span<float>.op_Implicit(span), ref availableSamples) : this.TransposeStereo(in dest1, Span<float>.op_Implicit(span), ref availableSamples)) : this.TransposeMono(in dest1, Span<float>.op_Implicit(span), ref availableSamples);
    dest.PutSamples(numSamples);
    src.ReceiveSamples(availableSamples);
    return numSamples;
  }

  public virtual void SetRate(double newRate) => this.Rate = newRate;

  public virtual void SetChannels(int channels)
  {
    this.NumberOfChannels = channels;
    this.ResetRegisters();
  }

  public abstract void ResetRegisters();

  protected abstract int TransposeMono(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples);

  protected abstract int TransposeStereo(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples);

  protected abstract int TransposeMulti(
    in Span<float> dest,
    in ReadOnlySpan<float> src,
    ref int srcSamples);

  public enum Algorithm
  {
    Linear,
    Cubic,
    Shannon,
  }
}
