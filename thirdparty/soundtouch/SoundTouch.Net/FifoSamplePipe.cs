// Decompiled with JetBrains decompiler
// Type: SoundTouch.FifoSamplePipe
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using SoundTouch.Assets;
using System;

#nullable enable
namespace SoundTouch;

public abstract class FifoSamplePipe
{
  public abstract int AvailableSamples { get; }

  public abstract bool IsEmpty { get; }

  public abstract Span<float> PtrBegin();

  public abstract void PutSamples(in ReadOnlySpan<float> samples, int numSamples);

  public void MoveSamples(in FifoSamplePipe other)
  {
    if (other == null)
      throw new ArgumentNullException(nameof (other));
    int availableSamples = other.AvailableSamples;
    this.PutSamples(Span<float>.op_Implicit(other.PtrBegin()), availableSamples);
    other.ReceiveSamples(availableSamples);
  }

  public abstract int ReceiveSamples(in Span<float> output, int maxSamples);

  public abstract int ReceiveSamples(int maxSamples);

  public abstract void Clear();

  public abstract int AdjustAmountOfSamples(int numSamples);

  protected static bool VerifyNumberOfChannels(int nChannels)
  {
    if (nChannels > 0 && nChannels <= 16 /*0x10*/)
      return true;
    throw new ArgumentException(Strings.Argument_IllegalNumberOfChannels);
  }
}
