// Decompiled with JetBrains decompiler
// Type: SoundTouch.FifoSampleBuffer
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable enable
namespace SoundTouch;

public sealed class FifoSampleBuffer : FifoSamplePipe
{
  private float[]? _buffer;
  private int _sizeInBytes;
  private int _samplesInBuffer;
  private int _channels;
  private int _bufferPos;

  public FifoSampleBuffer(int numberOfChannels = 2)
  {
    if (numberOfChannels <= 0)
      throw new ArgumentOutOfRangeException(nameof (numberOfChannels));
    this._sizeInBytes = 0;
    this._buffer = (float[]) null;
    this._samplesInBuffer = 0;
    this._bufferPos = 0;
    this._channels = numberOfChannels;
    this.EnsureCapacity(32 /*0x20*/);
  }

  public override int AvailableSamples => this._samplesInBuffer;

  public override bool IsEmpty => this._samplesInBuffer == 0;

  public int Channels
  {
    get => this._channels;
    set
    {
      if (!FifoSamplePipe.VerifyNumberOfChannels(value))
        return;
      int num = this._channels * this._samplesInBuffer;
      this._channels = value;
      this._samplesInBuffer = num / this._channels;
    }
  }

  private int Capacity => this._sizeInBytes / (this._channels * 4);

  public override Span<float> PtrBegin()
  {
    if (this._buffer == null)
      throw new InvalidOperationException();
    return MemoryExtensions.AsSpan<float>(this._buffer, this._bufferPos * this._channels);
  }

  public Span<float> PtrEnd(int slackCapacity)
  {
    this.EnsureCapacity(this._samplesInBuffer + slackCapacity);
    return MemoryExtensions.AsSpan<float>(this._buffer).Slice(this._samplesInBuffer * this._channels);
  }

  public override void PutSamples(in ReadOnlySpan<float> samples, int numSamples)
  {
    Span<float> span = this.PtrEnd(numSamples);
    samples.Slice(0, numSamples * this._channels).CopyTo(span);
    this._samplesInBuffer += numSamples;
  }

  public void PutSamples(int numSamples)
  {
    this.EnsureCapacity(this._samplesInBuffer + numSamples);
    this._samplesInBuffer += numSamples;
  }

  public override int ReceiveSamples(in Span<float> output, int maxSamples)
  {
    int maxSamples1 = maxSamples > this._samplesInBuffer ? this._samplesInBuffer : maxSamples;
    Span<float> span = this.PtrBegin();
    span = span.Slice(0, this._channels * maxSamples1);
    span.CopyTo(output);
    return this.ReceiveSamples(maxSamples1);
  }

  public override int ReceiveSamples(int maxSamples)
  {
    if (maxSamples >= this._samplesInBuffer)
    {
      int samplesInBuffer = this._samplesInBuffer;
      this._samplesInBuffer = 0;
      return samplesInBuffer;
    }
    this._samplesInBuffer -= maxSamples;
    this._bufferPos += maxSamples;
    return maxSamples;
  }

  public override void Clear()
  {
    this._samplesInBuffer = 0;
    this._bufferPos = 0;
  }

  public override int AdjustAmountOfSamples(int numSamples)
  {
    if (numSamples < this._samplesInBuffer)
      this._samplesInBuffer = numSamples;
    return this._samplesInBuffer;
  }

  public void AddSilent(int numSamples)
  {
    Span<float> span = this.PtrEnd(numSamples);
    span = span.Slice(0, numSamples * this._channels);
    span.Clear();
    this._samplesInBuffer += numSamples;
  }

  private void Rewind()
  {
    if (this._buffer == null || this._bufferPos == 0)
      return;
    this.PtrBegin().Slice(0, this._channels * this._samplesInBuffer).CopyTo(Span<float>.op_Implicit(this._buffer));
    this._bufferPos = 0;
  }

  private void EnsureCapacity(int capacityRequirement)
  {
    if (capacityRequirement > this.Capacity)
    {
      this._sizeInBytes = (int) ((long) (capacityRequirement * this._channels * 4 + 4095 /*0x0FFF*/) & 4294963200L);
      float[] numArray = new float[this._sizeInBytes / 4 + 4];
      if (this._samplesInBuffer != 0)
        this.PtrBegin().CopyTo(Span<float>.op_Implicit(numArray));
      this._buffer = numArray;
      this._bufferPos = 0;
    }
    else
      this.Rewind();
  }
}
