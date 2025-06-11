// Decompiled with JetBrains decompiler
// Type: SoundTouch.TimeStretch
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using SoundTouch.Assets;
using System;

#nullable enable
namespace SoundTouch;

internal class TimeStretch : FifoProcessor
{
  private readonly FifoSampleBuffer _outputBuffer;
  private readonly FifoSampleBuffer _inputBuffer;
  private int _channels;
  private int _sampleReq;
  private int _overlapLength;
  private int _seekLength;
  private int _seekWindowLength;
  private int _overlapDividerBitsNorm;
  private int _sampleRate;
  private int _sequenceMs;
  private int _seekWindowMs;
  private int _overlapMs;
  private long _maxnorm;
  private float _maxnormf;
  private double _tempo;
  private double _nominalSkip;
  private double _skipFract;
  private bool _bQuickSeek;
  private bool _bAutoSeqSetting;
  private bool _bAutoSeekSetting;
  private bool _isBeginning;
  private float[] _pMidBuffer;

  public TimeStretch()
    : this(new FifoSampleBuffer())
  {
  }

  private TimeStretch(FifoSampleBuffer outputBuffer)
    : base((FifoSamplePipe) outputBuffer)
  {
    this._outputBuffer = outputBuffer;
    this._inputBuffer = new FifoSampleBuffer();
    this._bQuickSeek = false;
    this._channels = 2;
    this._pMidBuffer = Array.Empty<float>();
    this._overlapLength = 0;
    this._bAutoSeqSetting = true;
    this._bAutoSeekSetting = true;
    this._tempo = 1.0;
    this.SetParameters(44100, 0, 0, 8);
    this.SetTempo(1.0);
    this.Clear();
  }

  public FifoSamplePipe GetOutput() => (FifoSamplePipe) this._outputBuffer;

  public FifoSamplePipe GetInput() => (FifoSamplePipe) this._inputBuffer;

  public void SetTempo(double newTempo)
  {
    this._tempo = newTempo;
    this.CalcSeqParameters();
    this._nominalSkip = this._tempo * (double) (this._seekWindowLength - this._overlapLength);
    this._sampleReq = Math.Max((int) (this._nominalSkip + 0.5) + this._overlapLength, this._seekWindowLength) + this._seekLength;
  }

  public override void Clear()
  {
    this._outputBuffer.Clear();
    this.ClearInput();
  }

  public void ClearInput()
  {
    this._inputBuffer.Clear();
    this.ClearMidBuffer();
    this._isBeginning = true;
    this._maxnorm = 0L;
    this._maxnormf = 1E+08f;
    this._skipFract = 0.0;
  }

  public void SetChannels(int numChannels)
  {
    if (!FifoSamplePipe.VerifyNumberOfChannels(numChannels) || this._channels == numChannels)
      return;
    this._channels = numChannels;
    this._inputBuffer.Channels = this._channels;
    this._outputBuffer.Channels = this._channels;
    this._overlapLength = 0;
    this.SetParameters(this._sampleRate);
  }

  public void EnableQuickSeek(bool enable) => this._bQuickSeek = enable;

  public bool IsQuickSeekEnabled() => this._bQuickSeek;

  public void SetParameters(int sampleRate, int sequenceMS = -1, int seekwindowMS = -1, int overlapMS = -1)
  {
    if (sampleRate > 0)
      this._sampleRate = sampleRate <= 192000 ? sampleRate : throw new ArgumentException(Strings.Argument_ExcessiveSampleRate);
    if (overlapMS > 0)
      this._overlapMs = overlapMS;
    if (sequenceMS > 0)
    {
      this._sequenceMs = sequenceMS;
      this._bAutoSeqSetting = false;
    }
    else if (sequenceMS == 0)
      this._bAutoSeqSetting = true;
    if (seekwindowMS > 0)
    {
      this._seekWindowMs = seekwindowMS;
      this._bAutoSeekSetting = false;
    }
    else if (seekwindowMS == 0)
      this._bAutoSeekSetting = true;
    this.CalcSeqParameters();
    this.CalculateOverlapLength(this._overlapMs);
    this.SetTempo(this._tempo);
  }

  public void GetParameters(
    out int pSampleRate,
    out int pSequenceMs,
    out int pSeekWindowMs,
    out int pOverlapMs)
  {
    pSampleRate = this._sampleRate;
    pSequenceMs = this._bAutoSeqSetting ? 0 : this._sequenceMs;
    pSeekWindowMs = this._bAutoSeekSetting ? 0 : this._seekWindowMs;
    pOverlapMs = this._overlapMs;
  }

  public override void PutSamples(in ReadOnlySpan<float> samples, int numSamples)
  {
    this._inputBuffer.PutSamples(in samples, numSamples);
    this.ProcessSamples();
  }

  public int GetInputSampleReq() => (int) (this._nominalSkip + 0.5);

  public int GetOutputBatchSize() => this._seekWindowLength - this._overlapLength;

  public int GetLatency() => this._sampleReq;

  private static void ClearCrossCorrState()
  {
  }

  private void AcceptNewOverlapLength(int newOverlapLength)
  {
    if (newOverlapLength <= 0)
      throw new ArgumentOutOfRangeException(nameof (newOverlapLength));
    int overlapLength = this._overlapLength;
    this._overlapLength = newOverlapLength;
    if (this._overlapLength <= overlapLength)
      return;
    this._pMidBuffer = new float[this._overlapLength * this._channels];
    this.ClearMidBuffer();
  }

  private void CalculateOverlapLength(int overlapMs)
  {
    if (overlapMs <= 0)
      throw new ArgumentOutOfRangeException(nameof (overlapMs));
    int num = this._sampleRate * overlapMs / 1000;
    if (num < 16 /*0x10*/)
      num = 16 /*0x10*/;
    this.AcceptNewOverlapLength(num - num % 8);
  }

  private double CalcCrossCorr(
    in ReadOnlySpan<float> mixingPos,
    in ReadOnlySpan<float> compare,
    ref double anorm)
  {
    double num1;
    double num2 = num1 = 0.0;
    int num3 = this._channels * this._overlapLength & -8;
    for (int index = 0; index < num3; ++index)
    {
      num2 += (double) mixingPos[index] * (double) compare[index];
      num1 += (double) mixingPos[index] * (double) mixingPos[index];
    }
    anorm = num1;
    return num2 / Math.Sqrt(num1 < 1E-09 ? 1.0 : num1);
  }

  private unsafe double CalcCrossCorrAccumulate(
    in ReadOnlySpan<float> mixingPos,
    in ReadOnlySpan<float> compare,
    ref double norm)
  {
    double num1 = 0.0;
    fixed (float* numPtr = &mixingPos.GetPinnableReference())
    {
      for (int index = 1; index <= this._channels; ++index)
        norm -= (double) numPtr[-index] * (double) numPtr[-index];
    }
    int num2 = this._channels * this._overlapLength & -8;
    int num3;
    for (num3 = 0; num3 < num2; ++num3)
      num1 += (double) mixingPos[num3] * (double) compare[num3];
    for (int index = 0; index < this._channels; ++index)
    {
      --num3;
      norm += (double) mixingPos[num3] * (double) mixingPos[num3];
    }
    return num1 / Math.Sqrt(norm < 1E-09 ? 1.0 : norm);
  }

  private int SeekBestOverlapPositionFull(in ReadOnlySpan<float> refPos)
  {
    double num1 = 0.0;
    int num2 = 0;
    double num3 = (this.CalcCrossCorr(in refPos, ReadOnlySpan<float>.op_Implicit(this._pMidBuffer), ref num1) + 0.1) * 0.75;
    object obj = new object();
    for (int index = 1; index < this._seekLength; ++index)
    {
      double num4 = this.CalcCrossCorrAccumulate(refPos.Slice(this._channels * index), ReadOnlySpan<float>.op_Implicit(this._pMidBuffer), ref num1);
      double num5 = (double) (2 * index - this._seekLength) / (double) this._seekLength;
      double num6 = (num4 + 0.1) * (1.0 - 0.25 * num5 * num5);
      if (num6 > num3)
      {
        lock (obj)
        {
          if (num6 > num3)
          {
            num3 = num6;
            num2 = index;
          }
        }
      }
    }
    TimeStretch.ClearCrossCorrState();
    return num2;
  }

  private int SeekBestOverlapPositionQuick(in ReadOnlySpan<float> refPos)
  {
    double anorm = 0.0;
    float num1;
    float num2 = num1 = float.MinValue;
    int num3;
    int num4 = num3 = 8;
    for (int index = 16 /*0x10*/; index < this._seekLength - 8 - 1; index += 16 /*0x10*/)
    {
      float num5 = (float) this.CalcCrossCorr(refPos.Slice(this._channels * index), ReadOnlySpan<float>.op_Implicit(this._pMidBuffer), ref anorm);
      float num6 = (float) (2 * index - this._seekLength - 1) / (float) this._seekLength;
      float num7 = (float) (((double) num5 + 0.10000000149011612) * (1.0 - 0.25 * (double) num6 * (double) num6));
      if ((double) num7 > (double) num2)
      {
        num1 = num2;
        num3 = num4;
        num2 = num7;
        num4 = index;
      }
      else if ((double) num7 > (double) num1)
      {
        num1 = num7;
        num3 = index;
      }
    }
    int num8 = Math.Min(num4 + 8 + 1, this._seekLength);
    for (int index = num4 - 8; index < num8; ++index)
    {
      if (index != num4)
      {
        float num9 = (float) this.CalcCrossCorr(refPos.Slice(this._channels * index), ReadOnlySpan<float>.op_Implicit(this._pMidBuffer), ref anorm);
        float num10 = (float) (2 * index - this._seekLength - 1) / (float) this._seekLength;
        float num11 = (float) (((double) num9 + 0.10000000149011612) * (1.0 - 0.25 * (double) num10 * (double) num10));
        if ((double) num11 > (double) num2)
        {
          num2 = num11;
          num4 = index;
        }
      }
    }
    int num12 = Math.Min(num3 + 8 + 1, this._seekLength);
    for (int index = num3 - 8; index < num12; ++index)
    {
      if (index != num3)
      {
        float num13 = (float) this.CalcCrossCorr(refPos.Slice(this._channels * index), ReadOnlySpan<float>.op_Implicit(this._pMidBuffer), ref anorm);
        float num14 = (float) (2 * index - this._seekLength - 1) / (float) this._seekLength;
        float num15 = (float) (((double) num13 + 0.10000000149011612) * (1.0 - 0.25 * (double) num14 * (double) num14));
        if ((double) num15 > (double) num2)
        {
          num2 = num15;
          num4 = index;
        }
      }
    }
    TimeStretch.ClearCrossCorrState();
    return num4;
  }

  private int SeekBestOverlapPosition(in ReadOnlySpan<float> refPos)
  {
    return this._bQuickSeek ? this.SeekBestOverlapPositionQuick(in refPos) : this.SeekBestOverlapPositionFull(in refPos);
  }

  private void OverlapStereo(in Span<float> output, in ReadOnlySpan<float> input)
  {
    float num1 = 1f / (float) this._overlapLength;
    float num2 = 0.0f;
    float num3 = 1f;
    for (int index = 0; index < 2 * this._overlapLength; index += 2)
    {
      output[index] = (float) ((double) input[index] * (double) num2 + (double) this._pMidBuffer[index] * (double) num3);
      output[index + 1] = (float) ((double) input[index + 1] * (double) num2 + (double) this._pMidBuffer[index + 1] * (double) num3);
      num2 += num1;
      num3 -= num1;
    }
  }

  private void OverlapMono(in Span<float> output, in ReadOnlySpan<float> input)
  {
    float num = 0.0f;
    float overlapLength = (float) this._overlapLength;
    for (int index = 0; index < this._overlapLength; ++index)
    {
      output[index] = (float) ((double) input[index] * (double) num + (double) this._pMidBuffer[index] * (double) overlapLength) / (float) this._overlapLength;
      ++num;
      --overlapLength;
    }
  }

  private void OverlapMulti(in Span<float> output, in ReadOnlySpan<float> input)
  {
    float num1 = 1f / (float) this._overlapLength;
    float num2 = 0.0f;
    float num3 = 1f;
    int index1 = 0;
    for (int index2 = 0; index2 < this._overlapLength; ++index2)
    {
      for (int index3 = 0; index3 < this._channels; ++index3)
      {
        output[index1] = (float) ((double) input[index1] * (double) num2 + (double) this._pMidBuffer[index1] * (double) num3);
        ++index1;
      }
      num2 += num1;
      num3 -= num1;
    }
  }

  private void ClearMidBuffer()
  {
    Span<float> span = MemoryExtensions.AsSpan<float>(this._pMidBuffer);
    span = span.Slice(0, this._channels * this._overlapLength);
    span.Clear();
  }

  private void Overlap(in Span<float> output, in ReadOnlySpan<float> input, int ovlPos)
  {
    if (this._channels == 1)
      this.OverlapMono(in output, input.Slice(ovlPos));
    else if (this._channels == 2)
      this.OverlapStereo(in output, input.Slice(2 * ovlPos));
    else
      this.OverlapMulti(in output, input.Slice(this._channels * ovlPos));
  }

  private void CalcSeqParameters()
  {
    if (this._bAutoSeqSetting)
      this._sequenceMs = (int) (CHECK_LIMITS(320.0 / 3.0 + -100.0 / 3.0 * this._tempo, 40.0, 90.0) + 0.5);
    if (this._bAutoSeekSetting)
      this._seekWindowMs = (int) (CHECK_LIMITS(65.0 / 3.0 + -10.0 / 3.0 * this._tempo, 15.0, 20.0) + 0.5);
    this._seekWindowLength = this._sampleRate * this._sequenceMs / 1000;
    if (this._seekWindowLength < 2 * this._overlapLength)
      this._seekWindowLength = 2 * this._overlapLength;
    this._seekLength = this._sampleRate * this._seekWindowMs / 1000;

    static double CHECK_LIMITS(double x, double mi, double ma) => x >= mi ? (x <= ma ? x : ma) : mi;
  }

  private void AdaptNormalizer()
  {
    if (this._maxnorm > 1000L || (double) this._maxnormf > 40000000.0)
    {
      this._maxnormf = (float) (0.89999997615814209 * (double) this._maxnormf + 0.10000000149011612 * (double) this._maxnorm);
      if (this._maxnorm > 800000000L && this._overlapDividerBitsNorm < 16 /*0x10*/)
      {
        ++this._overlapDividerBitsNorm;
        if (this._maxnorm > 1600000000L)
          ++this._overlapDividerBitsNorm;
      }
      else if ((double) this._maxnormf < 1000000.0 && this._overlapDividerBitsNorm > 0)
        --this._overlapDividerBitsNorm;
    }
    this._maxnorm = 0L;
  }

  private void ProcessSamples()
  {
    int num1 = 0;
    while (this._inputBuffer.AvailableSamples >= this._sampleReq)
    {
      Span<float> output;
      if (!this._isBeginning)
      {
        int ovlPos = this.SeekBestOverlapPosition(Span<float>.op_Implicit(this._inputBuffer.PtrBegin()));
        output = this._outputBuffer.PtrEnd(this._overlapLength);
        this.Overlap(in output, Span<float>.op_Implicit(this._inputBuffer.PtrBegin()), ovlPos);
        this._outputBuffer.PutSamples(this._overlapLength);
        num1 = ovlPos + this._overlapLength;
      }
      else
      {
        this._isBeginning = false;
        this._skipFract -= (double) (int) (this._tempo * (double) this._overlapLength + 0.5 * (double) this._seekLength + 0.5);
        if (this._skipFract <= -this._nominalSkip)
          this._skipFract = -this._nominalSkip;
      }
      if (this._inputBuffer.AvailableSamples >= num1 + this._seekWindowLength - this._overlapLength)
      {
        int num2 = this._seekWindowLength - 2 * this._overlapLength;
        FifoSampleBuffer outputBuffer = this._outputBuffer;
        output = this._inputBuffer.PtrBegin();
        // ISSUE: explicit reference operation
        ref ReadOnlySpan<float> local = @Span<float>.op_Implicit(output.Slice(this._channels * num1));
        int numSamples = num2;
        outputBuffer.PutSamples(in local, numSamples);
        output = this._inputBuffer.PtrBegin();
        output = output.Slice(this._channels * (num1 + num2), this._channels * this._overlapLength);
        output.CopyTo(Span<float>.op_Implicit(this._pMidBuffer));
        this._skipFract += this._nominalSkip;
        int skipFract = (int) this._skipFract;
        this._skipFract -= (double) skipFract;
        this._inputBuffer.ReceiveSamples(skipFract);
      }
    }
  }
}
