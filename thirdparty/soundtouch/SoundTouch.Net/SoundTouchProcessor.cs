// Decompiled with JetBrains decompiler
// Type: SoundTouch.SoundTouchProcessor
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using SoundTouch.Assets;
using System;

#nullable enable
namespace SoundTouch;

public sealed class SoundTouchProcessor : FifoProcessor
{
  internal const int SOUNDTOUCH_MAX_CHANNELS = 16 /*0x10*/;
  private readonly RateTransposer _rateTransposer;
  private readonly TimeStretch _stretch;
  private bool _isSampleRateSet;
  private double _rate;
  private double _tempo;
  private double _pitch;
  private double _samplesExpectedOut;
  private long _samplesOutput;
  private int _channels;
  private double _effectiveRate;
  private double _effectiveTempo;

  public SoundTouchProcessor()
    : this(new TimeStretch())
  {
  }

  private SoundTouchProcessor(TimeStretch stretch)
    : base((FifoSamplePipe) stretch)
  {
    this._rateTransposer = new RateTransposer();
    this._stretch = stretch;
    this._effectiveRate = this._effectiveTempo = 0.0;
    this._pitch = this._rate = this._tempo = 1.0;
    this.CalcEffectiveRateAndTempo();
    this._samplesExpectedOut = 0.0;
    this._samplesOutput = 0L;
    this._channels = 0;
    this._isSampleRateSet = false;
  }

  public static string VersionString => GitVersionInformation.InformationalVersion;

  public static Version Version => new Version(GitVersionInformation.AssemblySemFileVer);

  public int UnprocessedSampleCount
  {
    get
    {
      if (this._stretch != null)
      {
        FifoSamplePipe input = this._stretch.GetInput();
        if (input != null)
          return input.AvailableSamples;
      }
      return 0;
    }
  }

  public int Channels
  {
    get => this._channels;
    set
    {
      if (!FifoSamplePipe.VerifyNumberOfChannels(value))
        return;
      this._channels = value;
      this._rateTransposer.SetChannels(value);
      this._stretch.SetChannels(value);
    }
  }

  public double Rate
  {
    get => this._rate;
    set
    {
      this._rate = value;
      this.CalcEffectiveRateAndTempo();
    }
  }

  public double Tempo
  {
    get => this._tempo;
    set
    {
      this._tempo = value;
      this.CalcEffectiveRateAndTempo();
    }
  }

  public double RateChange
  {
    get => 100.0 * (this._rate - 1.0);
    set
    {
      this._rate = 1.0 + 0.01 * value;
      this.CalcEffectiveRateAndTempo();
    }
  }

  public double TempoChange
  {
    get => 100.0 * (this._tempo - 1.0);
    set
    {
      this._tempo = 1.0 + 0.01 * value;
      this.CalcEffectiveRateAndTempo();
    }
  }

  public int SampleRate
  {
    get
    {
      int pSampleRate;
      this._stretch.GetParameters(out pSampleRate, out int _, out int _, out int _);
      return pSampleRate;
    }
    set
    {
      this._stretch.SetParameters(value);
      this._isSampleRateSet = true;
    }
  }

  public double Pitch
  {
    get => this._pitch;
    set
    {
      this._pitch = value;
      this.CalcEffectiveRateAndTempo();
    }
  }

  public double PitchOctaves
  {
    get => Math.Log10(this._pitch) / 0.301029995664;
    set
    {
      this._pitch = Math.Exp(0.69314718056 * value);
      this.CalcEffectiveRateAndTempo();
    }
  }

  public double PitchSemiTones
  {
    get => this.PitchOctaves * 12.0;
    set => this.PitchOctaves = value / 12.0;
  }

  public double GetInputOutputSampleRatio() => 1.0 / (this._effectiveTempo * this._effectiveRate);

  public void Flush()
  {
    Span<float> span = stackalloc float[128 /*0x80*/ * this._channels];
    int numSamples = (int) ((long) (this._samplesExpectedOut + 0.5) - this._samplesOutput);
    if (numSamples < 0)
      numSamples = 0;
    for (int index = 0; numSamples > this.AvailableSamples && index < 200; ++index)
      this.PutSamples(Span<float>.op_Implicit(span), 128 /*0x80*/);
    this.AdjustAmountOfSamples(numSamples);
    this._stretch.ClearInput();
  }

  public override void PutSamples(in ReadOnlySpan<float> samples, int numSamples)
  {
    if (!this._isSampleRateSet)
      throw new InvalidOperationException(Strings.InvalidOperation_SampleRateUndefined);
    if (this._channels == 0)
      throw new InvalidOperationException(Strings.InvalidOperation_ChannelsUndefined);
    this._samplesExpectedOut += (double) numSamples / (this._effectiveRate * this._effectiveTempo);
    if (this._effectiveRate <= 1.0)
    {
      this._rateTransposer.PutSamples(in samples, numSamples);
      this._stretch.MoveSamples((FifoSamplePipe) this._rateTransposer);
    }
    else
    {
      this._stretch.PutSamples(in samples, numSamples);
      this._rateTransposer.MoveSamples((FifoSamplePipe) this._stretch);
    }
  }

  public override int ReceiveSamples(in Span<float> output, int maxSamples)
  {
    int samples = base.ReceiveSamples(in output, maxSamples);
    this._samplesOutput += (long) samples;
    return samples;
  }

  public override int ReceiveSamples(int maxSamples)
  {
    int samples = base.ReceiveSamples(maxSamples);
    this._samplesOutput += (long) samples;
    return samples;
  }

  public override void Clear()
  {
    this._samplesExpectedOut = 0.0;
    this._samplesOutput = 0L;
    this._rateTransposer.Clear();
    this._stretch.Clear();
  }

  public bool SetSetting(SettingId settingId, int value)
  {
    int pSampleRate;
    int pSequenceMs;
    int pSeekWindowMs;
    int pOverlapMs;
    this._stretch.GetParameters(out pSampleRate, out pSequenceMs, out pSeekWindowMs, out pOverlapMs);
    switch (settingId)
    {
      case SettingId.UseAntiAliasFilter:
        this._rateTransposer.EnableAAFilter(value != 0);
        return true;
      case SettingId.AntiAliasFilterLength:
        this._rateTransposer.GetAAFilter().Length = value;
        return true;
      case SettingId.UseQuickSeek:
        this._stretch.EnableQuickSeek(value != 0);
        return true;
      case SettingId.SequenceDurationMs:
        this._stretch.SetParameters(pSampleRate, value, pSeekWindowMs, pOverlapMs);
        return true;
      case SettingId.SeekWindowDurationMs:
        this._stretch.SetParameters(pSampleRate, pSequenceMs, value, pOverlapMs);
        return true;
      case SettingId.OverlapDurationMs:
        this._stretch.SetParameters(pSampleRate, pSequenceMs, pSeekWindowMs, value);
        return true;
      default:
        return false;
    }
  }

  public int GetSetting(SettingId settingId)
  {
    int num1;
    int num2;
    int num3;
    switch (settingId)
    {
      case SettingId.UseAntiAliasFilter:
        return !this._rateTransposer.IsAAFilterEnabled() ? 0 : 1;
      case SettingId.AntiAliasFilterLength:
        return this._rateTransposer.GetAAFilter().Length;
      case SettingId.UseQuickSeek:
        return !this._stretch.IsQuickSeekEnabled() ? 0 : 1;
      case SettingId.SequenceDurationMs:
        int pSequenceMs;
        this._stretch.GetParameters(out num1, out pSequenceMs, out num2, out num3);
        return pSequenceMs;
      case SettingId.SeekWindowDurationMs:
        int pSeekWindowMs;
        this._stretch.GetParameters(out num3, out num2, out pSeekWindowMs, out num1);
        return pSeekWindowMs;
      case SettingId.OverlapDurationMs:
        int pOverlapMs;
        this._stretch.GetParameters(out num1, out num2, out num3, out pOverlapMs);
        return pOverlapMs;
      case SettingId.NominalInputSequence:
        int inputSampleReq = this._stretch.GetInputSampleReq();
        return this._effectiveRate <= 1.0 ? (int) ((double) inputSampleReq * this._effectiveRate + 0.5) : inputSampleReq;
      case SettingId.NominalOutputSequence:
        int outputBatchSize = this._stretch.GetOutputBatchSize();
        return this._effectiveRate > 1.0 ? (int) ((double) outputBatchSize / this._effectiveRate + 0.5) : outputBatchSize;
      case SettingId.InitialLatency:
        double latency1 = (double) this._stretch.GetLatency();
        int latency2 = this._rateTransposer.Latency;
        return (int) ((this._effectiveRate > 1.0 ? latency1 + (double) latency2 / this._effectiveRate : (latency1 + (double) latency2) * this._effectiveRate) + 0.5);
      default:
        return 0;
    }
  }

  private static bool IsDoubleEqual(double a, double b) => Math.Abs(a - b) < double.Epsilon;

  private void CalcEffectiveRateAndTempo()
  {
    double effectiveTempo = this._effectiveTempo;
    double effectiveRate = this._effectiveRate;
    this._effectiveTempo = this._tempo / this._pitch;
    this._effectiveRate = this._pitch * this._rate;
    if (!SoundTouchProcessor.IsDoubleEqual(this._effectiveRate, effectiveRate))
      this._rateTransposer.SetRate(this._effectiveRate);
    if (!SoundTouchProcessor.IsDoubleEqual(this._effectiveTempo, effectiveTempo))
      this._stretch.SetTempo(this._effectiveTempo);
    if (this.Output == null)
      throw new InvalidOperationException(Strings.InvalidOperation_OutputUndefined);
    if (this._effectiveRate <= 1.0)
    {
      if (this.Output == this._stretch)
        return;
      this._stretch.GetOutput().MoveSamples(this.Output);
      this.Output = (FifoSamplePipe) this._stretch;
    }
    else
    {
      if (this.Output == this._rateTransposer)
        return;
      this._rateTransposer.GetOutputBuffer().MoveSamples(this.Output);
      this._rateTransposer.MoveSamples(this._stretch.GetInput());
      this.Output = (FifoSamplePipe) this._rateTransposer;
    }
  }
}
