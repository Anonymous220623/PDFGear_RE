// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.SmbPitchShifter
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Dsp;

public class SmbPitchShifter
{
  private static int MAX_FRAME_LENGTH = 16000;
  private float[] gInFIFO = new float[SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gOutFIFO = new float[SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gFFTworksp = new float[2 * SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gLastPhase = new float[SmbPitchShifter.MAX_FRAME_LENGTH / 2 + 1];
  private float[] gSumPhase = new float[SmbPitchShifter.MAX_FRAME_LENGTH / 2 + 1];
  private float[] gOutputAccum = new float[2 * SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gAnaFreq = new float[SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gAnaMagn = new float[SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gSynFreq = new float[SmbPitchShifter.MAX_FRAME_LENGTH];
  private float[] gSynMagn = new float[SmbPitchShifter.MAX_FRAME_LENGTH];
  private long gRover;

  public void PitchShift(
    float pitchShift,
    long numSampsToProcess,
    float sampleRate,
    float[] indata)
  {
    this.PitchShift(pitchShift, numSampsToProcess, 2048L /*0x0800*/, 10L, sampleRate, indata);
  }

  public void PitchShift(
    float pitchShift,
    long numSampsToProcess,
    long fftFrameSize,
    long osamp,
    float sampleRate,
    float[] indata)
  {
    float[] numArray = indata;
    long num1 = fftFrameSize / 2L;
    long num2 = fftFrameSize / osamp;
    double num3 = (double) sampleRate / (double) fftFrameSize;
    double num4 = 2.0 * Math.PI * (double) num2 / (double) fftFrameSize;
    long num5 = fftFrameSize - num2;
    if (this.gRover == 0L)
      this.gRover = num5;
    for (long index1 = 0; index1 < numSampsToProcess; ++index1)
    {
      this.gInFIFO[this.gRover] = indata[index1];
      numArray[index1] = this.gOutFIFO[this.gRover - num5];
      ++this.gRover;
      if (this.gRover >= fftFrameSize)
      {
        this.gRover = num5;
        for (long index2 = 0; index2 < fftFrameSize; ++index2)
        {
          double num6 = -0.5 * Math.Cos(2.0 * Math.PI * (double) index2 / (double) fftFrameSize) + 0.5;
          this.gFFTworksp[2L * index2] = this.gInFIFO[index2] * (float) num6;
          this.gFFTworksp[2L * index2 + 1L] = 0.0f;
        }
        this.ShortTimeFourierTransform(this.gFFTworksp, fftFrameSize, -1L);
        for (long index3 = 0; index3 <= num1; ++index3)
        {
          double x = (double) this.gFFTworksp[2L * index3];
          double y = (double) this.gFFTworksp[2L * index3 + 1L];
          double num7 = 2.0 * Math.Sqrt(x * x + y * y);
          double num8 = Math.Atan2(y, x);
          double num9 = num8 - (double) this.gLastPhase[index3];
          this.gLastPhase[index3] = (float) num8;
          double num10 = num9 - (double) index3 * num4;
          long num11 = (long) (num10 / Math.PI);
          long num12 = num11 < 0L ? num11 - (num11 & 1L) : num11 + (num11 & 1L);
          double num13 = num10 - Math.PI * (double) num12;
          double num14 = (double) osamp * num13 / (2.0 * Math.PI);
          double num15 = (double) index3 * num3 + num14 * num3;
          this.gAnaMagn[index3] = (float) num7;
          this.gAnaFreq[index3] = (float) num15;
        }
        for (int index4 = 0; (long) index4 < fftFrameSize; ++index4)
        {
          this.gSynMagn[index4] = 0.0f;
          this.gSynFreq[index4] = 0.0f;
        }
        for (long index5 = 0; index5 <= num1; ++index5)
        {
          long index6 = (long) ((double) index5 * (double) pitchShift);
          if (index6 <= num1)
          {
            this.gSynMagn[index6] += this.gAnaMagn[index5];
            this.gSynFreq[index6] = this.gAnaFreq[index5] * pitchShift;
          }
        }
        for (long index7 = 0; index7 <= num1; ++index7)
        {
          double num16 = (double) this.gSynMagn[index7];
          double num17 = 2.0 * Math.PI * (((double) this.gSynFreq[index7] - (double) index7 * num3) / num3) / (double) osamp + (double) index7 * num4;
          this.gSumPhase[index7] += (float) num17;
          double num18 = (double) this.gSumPhase[index7];
          this.gFFTworksp[2L * index7] = (float) (num16 * Math.Cos(num18));
          this.gFFTworksp[2L * index7 + 1L] = (float) (num16 * Math.Sin(num18));
        }
        for (long index8 = fftFrameSize + 2L; index8 < 2L * fftFrameSize; ++index8)
          this.gFFTworksp[index8] = 0.0f;
        this.ShortTimeFourierTransform(this.gFFTworksp, fftFrameSize, 1L);
        for (long index9 = 0; index9 < fftFrameSize; ++index9)
        {
          double num19 = -0.5 * Math.Cos(2.0 * Math.PI * (double) index9 / (double) fftFrameSize) + 0.5;
          this.gOutputAccum[index9] += (float) (2.0 * num19) * this.gFFTworksp[2L * index9] / (float) (num1 * osamp);
        }
        for (long index10 = 0; index10 < num2; ++index10)
          this.gOutFIFO[index10] = this.gOutputAccum[index10];
        for (long index11 = 0; index11 < fftFrameSize; ++index11)
          this.gOutputAccum[index11] = this.gOutputAccum[index11 + num2];
        for (long index12 = 0; index12 < num5; ++index12)
          this.gInFIFO[index12] = this.gInFIFO[index12 + num2];
      }
    }
  }

  public void ShortTimeFourierTransform(float[] fftBuffer, long fftFrameSize, long sign)
  {
    for (long index1 = 2; index1 < 2L * fftFrameSize - 2L; index1 += 2L)
    {
      long num1 = 2;
      long index2 = 0;
      for (; num1 < 2L * fftFrameSize; num1 <<= 1)
      {
        if ((index1 & num1) != 0L)
          ++index2;
        index2 <<= 1;
      }
      if (index1 < index2)
      {
        float num2 = fftBuffer[index1];
        fftBuffer[index1] = fftBuffer[index2];
        fftBuffer[index2] = num2;
        float num3 = fftBuffer[index1 + 1L];
        fftBuffer[index1 + 1L] = fftBuffer[index2 + 1L];
        fftBuffer[index2 + 1L] = num3;
      }
    }
    long num4 = (long) (Math.Log((double) fftFrameSize) / Math.Log(2.0) + 0.5);
    long num5 = 0;
    long num6 = 2;
    for (; num5 < num4; ++num5)
    {
      num6 <<= 1;
      long num7 = num6 >> 1;
      float num8 = 1f;
      float num9 = 0.0f;
      float num10 = 3.14159274f / (float) (num7 >> 1);
      float num11 = (float) Math.Cos((double) num10);
      float num12 = (float) sign * (float) Math.Sin((double) num10);
      for (long index3 = 0; index3 < num7; index3 += 2L)
      {
        for (long index4 = index3; index4 < 2L * fftFrameSize; index4 += num6)
        {
          float num13 = (float) ((double) fftBuffer[index4 + num7] * (double) num8 - (double) fftBuffer[index4 + num7 + 1L] * (double) num9);
          float num14 = (float) ((double) fftBuffer[index4 + num7] * (double) num9 + (double) fftBuffer[index4 + num7 + 1L] * (double) num8);
          fftBuffer[index4 + num7] = fftBuffer[index4] - num13;
          fftBuffer[index4 + num7 + 1L] = fftBuffer[index4 + 1L] - num14;
          fftBuffer[index4] += num13;
          fftBuffer[index4 + 1L] += num14;
        }
        float num15 = (float) ((double) num8 * (double) num11 - (double) num9 * (double) num12);
        num9 = (float) ((double) num8 * (double) num12 + (double) num9 * (double) num11);
        num8 = num15;
      }
    }
  }
}
