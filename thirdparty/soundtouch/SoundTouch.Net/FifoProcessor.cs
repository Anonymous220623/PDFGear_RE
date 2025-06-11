// Decompiled with JetBrains decompiler
// Type: SoundTouch.FifoProcessor
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using SoundTouch.Assets;
using System;

#nullable enable
namespace SoundTouch;

public abstract class FifoProcessor : FifoSamplePipe
{
  protected FifoProcessor(FifoSamplePipe output)
  {
    this.Output = output ?? throw new ArgumentNullException(nameof (output));
  }

  public override int AvailableSamples => this.Output.AvailableSamples;

  public override bool IsEmpty => this.Output.IsEmpty;

  protected FifoSamplePipe Output { get; private protected set; }

  public override Span<float> PtrBegin() => this.Output.PtrBegin();

  public override int ReceiveSamples(in Span<float> output, int maxSamples)
  {
    return this.Output.ReceiveSamples(in output, maxSamples);
  }

  public override int ReceiveSamples(int maxSamples) => this.Output.ReceiveSamples(maxSamples);

  public override int AdjustAmountOfSamples(int numSamples)
  {
    return this.Output.AdjustAmountOfSamples(numSamples);
  }

  protected void SetOutPipe(FifoSamplePipe output)
  {
    if (output == null)
      throw new ArgumentNullException(nameof (output));
    this.Output = this.Output == null ? output : throw new InvalidOperationException(Strings.InvalidOperation_OutputPipeOverwrite);
  }
}
