// Decompiled with JetBrains decompiler
// Type: SoundTouch.RateTransposer
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System;

#nullable enable
namespace SoundTouch;

internal class RateTransposer : FifoProcessor
{
  private readonly AntiAliasFilter _pAAFilter;
  private readonly TransposerBase _transposer;
  private readonly FifoSampleBuffer _inputBuffer;
  private readonly FifoSampleBuffer _midBuffer;
  private readonly FifoSampleBuffer _outputBuffer;
  private bool _useAAFilter;

  public RateTransposer()
    : this(new FifoSampleBuffer())
  {
  }

  private RateTransposer(FifoSampleBuffer outputBuffer)
    : base((FifoSamplePipe) outputBuffer)
  {
    this._useAAFilter = true;
    this._inputBuffer = new FifoSampleBuffer();
    this._midBuffer = new FifoSampleBuffer();
    this._outputBuffer = outputBuffer;
    this._pAAFilter = new AntiAliasFilter(64 /*0x40*/);
    this._transposer = TransposerBase.CreateInstance();
    this.Clear();
  }

  public override bool IsEmpty => base.IsEmpty && this._inputBuffer.IsEmpty;

  public int Latency
  {
    get => this._transposer.Latency + (this._useAAFilter ? this._pAAFilter.Length / 2 : 0);
  }

  public FifoSamplePipe GetOutputBuffer() => (FifoSamplePipe) this._outputBuffer;

  public AntiAliasFilter GetAAFilter() => this._pAAFilter;

  public void EnableAAFilter(bool newMode)
  {
    this._useAAFilter = newMode;
    this.Clear();
  }

  public bool IsAAFilterEnabled() => this._useAAFilter;

  public virtual void SetRate(double newRate)
  {
    this._transposer.SetRate(newRate);
    this._pAAFilter.SetCutoffFreq(newRate > 1.0 ? 0.5 / newRate : 0.5 * newRate);
  }

  public void SetChannels(int channels)
  {
    if (!FifoSamplePipe.VerifyNumberOfChannels(channels) || this._transposer.NumberOfChannels == channels)
      return;
    this._transposer.SetChannels(channels);
    this._inputBuffer.Channels = channels;
    this._midBuffer.Channels = channels;
    this._outputBuffer.Channels = channels;
  }

  public override void PutSamples(in ReadOnlySpan<float> samples, int numSamples)
  {
    this.ProcessSamples(in samples, numSamples);
  }

  public override void Clear()
  {
    this._outputBuffer.Clear();
    this._midBuffer.Clear();
    this._inputBuffer.Clear();
    this._transposer.ResetRegisters();
    this._inputBuffer.AddSilent(this.Latency);
  }

  private void ProcessSamples(in ReadOnlySpan<float> src, int numSamples)
  {
    if (numSamples == 0)
      return;
    this._inputBuffer.PutSamples(in src, numSamples);
    if (!this._useAAFilter)
      this._transposer.Transpose(in this._outputBuffer, in this._inputBuffer);
    else if (this._transposer.Rate < 1.0)
    {
      this._transposer.Transpose(in this._midBuffer, in this._inputBuffer);
      this._pAAFilter.Evaluate(in this._outputBuffer, this._midBuffer);
    }
    else
    {
      this._pAAFilter.Evaluate(in this._midBuffer, this._inputBuffer);
      this._transposer.Transpose(in this._outputBuffer, in this._midBuffer);
    }
  }
}
