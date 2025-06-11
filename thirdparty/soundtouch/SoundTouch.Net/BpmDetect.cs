// Decompiled with JetBrains decompiler
// Type: SoundTouch.BpmDetect
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using SoundTouch.Assets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable enable
namespace SoundTouch;

public sealed class BpmDetect
{
  private const int MIN_BPM = 45;
  private const int MAX_BPM_RANGE = 200;
  private const int MAX_BPM_VALID = 190;
  private const int INPUT_BLOCK_SIZE = 2048 /*0x0800*/;
  private const int DECIMATED_BLOCK_SIZE = 256 /*0x0100*/;
  private const int TARGET_SRATE = 1000;
  private const int XCORR_UPDATE_SEQUENCE = 200;
  private const int MOVING_AVERAGE_N = 15;
  private const double XCORR_DECAY_TIME_CONSTANT = 30.0;
  private const int OVERLAP_FACTOR = 4;
  private const double TWOPI = 6.2831853071795862;
  private static readonly double[] _LPF_coeffs = new double[5]
  {
    0.00996655391939,
    -0.01944529148401,
    0.00996655391939,
    1.96867605796247,
    -0.96916387431724
  };
  private readonly float[] _xcorr;
  private readonly int _decimateBy;
  private readonly int _windowLen;
  private readonly int _channels;
  private readonly int _sampleRate;
  private readonly int _windowStart;
  private readonly float[] _hamw;
  private readonly float[] _hamw2;
  private readonly float[] _beatcorr_ringbuff;
  private readonly FifoSampleBuffer _buffer;
  private readonly List<Beat> _beats;
  private readonly IIR2Filter _beat_lpf;
  private int _decimateCount;
  private double _decimateSum;
  private int _pos;
  private int _peakPos;
  private int _beatcorr_ringbuffpos;
  private int _init_scaler;
  private float _peakVal;

  public BpmDetect(int numChannels, int sampleRate)
  {
    this._beat_lpf = new IIR2Filter(Span<double>.op_Implicit(BpmDetect._LPF_coeffs));
    this._beats = new List<Beat>(250);
    this._sampleRate = sampleRate;
    this._channels = numChannels;
    this._decimateSum = 0.0;
    this._decimateCount = 0;
    this._decimateBy = sampleRate / 1000;
    if (this._decimateBy <= 0 || this._decimateBy * 256 /*0x0100*/ < 2048 /*0x0800*/)
      throw new ArgumentOutOfRangeException(nameof (sampleRate), Strings.Argument_SampleRateTooSmall);
    this._windowLen = 60 * sampleRate / (this._decimateBy * 45);
    this._windowStart = 60 * sampleRate / (this._decimateBy * 200);
    this._xcorr = new float[this._windowLen];
    this._pos = 0;
    this._peakPos = 0;
    this._peakVal = 0.0f;
    this._init_scaler = 1;
    this._beatcorr_ringbuffpos = 0;
    this._beatcorr_ringbuff = new float[this._windowLen];
    this._buffer = new FifoSampleBuffer() { Channels = 1 };
    this._buffer.Clear();
    this._hamw = new float[200];
    Span<float> w = Span<float>.op_Implicit(this._hamw);
    BpmDetect.Hamming(in w);
    this._hamw2 = new float[100];
    w = Span<float>.op_Implicit(this._hamw2);
    BpmDetect.Hamming(in w);
  }

  public void InputSamples(ReadOnlySpan<float> samples, int numSamples)
  {
    Span<float> dest = stackalloc float[256 /*0x0100*/];
    while (numSamples > 0)
    {
      int numSamples1 = numSamples > 2048 /*0x0800*/ ? 2048 /*0x0800*/ : numSamples;
      int numSamples2 = this.Decimate(in dest, samples, numSamples1);
      samples = samples.Slice(numSamples1 * this._channels);
      numSamples -= numSamples1;
      this._buffer.PutSamples(Span<float>.op_Implicit(dest), numSamples2);
    }
    int num = Math.Max(this._windowLen + 200, 400);
    while (this._buffer.AvailableSamples >= num)
    {
      this.UpdateXCorr(200);
      this.UpdateBeatPos(100);
      this._buffer.ReceiveSamples(50);
    }
  }

  public float GetBpm()
  {
    PeakFinder peakFinder = new PeakFinder();
    this.RemoveBias();
    double num1 = 60.0 * ((double) this._sampleRate / (double) this._decimateBy);
    Span<float> dest = stackalloc float[this._windowLen];
    BpmDetect.MAFilter(in dest, ReadOnlySpan<float>.op_Implicit(this._xcorr), this._windowStart, this._windowLen, 15);
    double num2 = peakFinder.DetectPeak(Span<float>.op_Implicit(dest), this._windowStart, this._windowLen);
    if (num2 < 1E-09)
      return 0.0f;
    float num3 = (float) (num1 / num2);
    return (double) num3 < 45.0 || (double) num3 > 190.0 ? 0.0f : num3;
  }

  public int GetBeats(Span<float> pos, Span<float> strength)
  {
    int count = this._beats.Count;
    if (pos.Length == 0 || strength.Length == 0)
      return count;
    int num = Math.Min(pos.Length, strength.Length);
    for (int index = 0; index < count && index < num; ++index)
    {
      ref float local1 = ref pos[index];
      Beat beat = this._beats[index];
      double position = (double) beat.Position;
      local1 = (float) position;
      ref float local2 = ref strength[index];
      beat = this._beats[index];
      double strength1 = (double) beat.Strength;
      local2 = (float) strength1;
    }
    return count;
  }

  private static void Hamming(in Span<float> w)
  {
    int length = w.Length;
    for (int index = 0; index < length; ++index)
      w[index] = (float) (0.54 - 0.46 * Math.Cos(2.0 * Math.PI * (double) index / (double) (length - 1)));
  }

  private static void MAFilter(
    in Span<float> dest,
    in ReadOnlySpan<float> source,
    int start,
    int end,
    int n)
  {
    for (int index1 = start; index1 < end; ++index1)
    {
      int num1 = index1 - n / 2;
      int num2 = index1 + n / 2 + 1;
      if (num1 < start)
        num1 = start;
      if (num2 > end)
        num2 = end;
      double num3 = 0.0;
      for (int index2 = num1; index2 < num2; ++index2)
        num3 += (double) source[index2];
      dest[index1] = (float) num3 / (float) (num2 - num1);
    }
  }

  [Conditional("_CREATE_BPM_DEBUG_FILE")]
  private static void SaveDebugData(
    in string name,
    in ReadOnlySpan<float> data,
    int minpos,
    int maxpos,
    double coeff)
  {
    using (StreamWriter streamWriter = new StreamWriter(name, false))
    {
      Console.Error.WriteLine();
      Console.Error.WriteLine($"Writing BPM debug data into file {name}.");
      for (int index = minpos; index < maxpos; ++index)
        streamWriter.WriteLine("{0}\t{1:.0}\t{2}", (object) index, (object) (coeff / (double) index), (object) data[index]);
    }
  }

  [Conditional("_CREATE_BPM_DEBUG_FILE")]
  private static void SaveDebugBeatPos(in string name, in List<Beat> beats)
  {
    using (StreamWriter streamWriter = new StreamWriter(name, false))
    {
      Console.Error.WriteLine();
      Console.Error.WriteLine($"Writing BPM debug data into file {name}.");
      foreach (Beat beat in beats)
        streamWriter.WriteLine("{0}\t{1}", (object) beat.Position, (object) beat.Strength);
    }
  }

  private void UpdateXCorr(int process_samples)
  {
    ReadOnlySpan<float> readOnlySpan = Span<float>.op_Implicit(this._buffer.PtrBegin());
    float num1 = (float) Math.Pow(0.5, 1.0 / (30000.0 / (double) process_samples));
    Span<float> span = stackalloc float[200];
    for (int index = 0; index < process_samples; ++index)
      span[index] = this._hamw[index] * this._hamw[index] * readOnlySpan[index];
    for (int windowStart = this._windowStart; windowStart < this._windowLen; ++windowStart)
    {
      float num2 = 0.0f;
      for (int index = 0; index < process_samples; ++index)
        num2 += span[index] * readOnlySpan[index + windowStart];
      this._xcorr[windowStart] *= num1;
      this._xcorr[windowStart] += Math.Abs(num2);
    }
  }

  private int Decimate(in Span<float> dest, ReadOnlySpan<float> src, int numSamples)
  {
    int num1 = 0;
    for (int index = 0; index < numSamples; ++index)
    {
      int num2;
      for (num2 = 0; num2 < this._channels; ++num2)
        this._decimateSum += (double) src[num2];
      src = src.Slice(num2);
      ++this._decimateCount;
      if (this._decimateCount >= this._decimateBy)
      {
        double num3 = this._decimateSum / (double) (this._decimateBy * this._channels);
        this._decimateSum = 0.0;
        this._decimateCount = 0;
        dest[num1] = (float) num3;
        ++num1;
      }
    }
    return num1;
  }

  private void RemoveBias()
  {
    double num1 = 0.0;
    for (int windowStart = this._windowStart; windowStart < this._windowLen; ++windowStart)
      num1 += (double) this._xcorr[windowStart];
    double num2 = num1 / (double) (this._windowLen - this._windowStart);
    double num3 = 0.5 * (double) (this._windowLen - 1 + this._windowStart);
    double num4 = 0.0;
    double num5 = 0.0;
    for (int windowStart = this._windowStart; windowStart < this._windowLen; ++windowStart)
    {
      double num6 = (double) this._xcorr[windowStart] - num2;
      double num7 = (double) windowStart - num3;
      num4 += num6 * num7;
      num5 += num7 * num7;
    }
    double num8 = num4 / num5;
    float maxValue = float.MaxValue;
    for (int windowStart = this._windowStart; windowStart < this._windowLen; ++windowStart)
    {
      this._xcorr[windowStart] -= (float) num8 * (float) windowStart;
      if ((double) this._xcorr[windowStart] < (double) maxValue)
        maxValue = this._xcorr[windowStart];
    }
    for (int windowStart = this._windowStart; windowStart < this._windowLen; ++windowStart)
      this._xcorr[windowStart] -= maxValue;
  }

  private void UpdateBeatPos(int process_samples)
  {
    ReadOnlySpan<float> readOnlySpan = Span<float>.op_Implicit(this._buffer.PtrBegin());
    double num1 = (double) this._decimateBy / (double) this._sampleRate;
    int num2 = (int) (0.12 / num1 + 0.5);
    Span<float> span = stackalloc float[100];
    for (int index = 0; index < process_samples; ++index)
      span[index] = this._hamw2[index] * this._hamw2[index] * readOnlySpan[index];
    for (int windowStart = this._windowStart; windowStart < this._windowLen; ++windowStart)
    {
      float num3 = 0.0f;
      for (int index = 0; index < process_samples; ++index)
        num3 += span[index] * readOnlySpan[windowStart + index];
      this._beatcorr_ringbuff[(this._beatcorr_ringbuffpos + windowStart) % this._windowLen] += (double) num3 > 0.0 ? num3 : 0.0f;
    }
    float num4 = (float) this._windowLen / (float) (50 * this._init_scaler);
    if ((double) num4 > 1.0)
      ++this._init_scaler;
    else
      num4 = 1f;
    for (int index = 0; index < 50; ++index)
    {
      float x = this._beatcorr_ringbuff[this._beatcorr_ringbuffpos];
      float num5 = x - this._beat_lpf.Update(x);
      if ((double) num5 > (double) this._peakVal)
      {
        this._peakVal = num5;
        this._peakPos = this._pos;
      }
      if (this._pos > this._peakPos + num2)
      {
        this._peakPos += 50;
        if ((double) this._peakVal > 0.0)
          this._beats.Add(new Beat((float) this._peakPos * (float) num1, this._peakVal * num4));
        this._peakVal = 0.0f;
        this._peakPos = this._pos;
      }
      this._beatcorr_ringbuff[this._beatcorr_ringbuffpos] = 0.0f;
      ++this._pos;
      this._beatcorr_ringbuffpos = (this._beatcorr_ringbuffpos + 1) % this._windowLen;
    }
  }
}
