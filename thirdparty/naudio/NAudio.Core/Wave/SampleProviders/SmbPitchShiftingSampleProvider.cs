// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SmbPitchShiftingSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Dsp;
using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SmbPitchShiftingSampleProvider : ISampleProvider
{
  private readonly ISampleProvider sourceStream;
  private readonly WaveFormat waveFormat;
  private float pitch = 1f;
  private readonly int fftSize;
  private readonly long osamp;
  private readonly SmbPitchShifter shifterLeft = new SmbPitchShifter();
  private readonly SmbPitchShifter shifterRight = new SmbPitchShifter();
  private const float LIM_THRESH = 0.95f;
  private const float LIM_RANGE = 0.0500000119f;
  private const float M_PI_2 = 1.57079637f;

  public SmbPitchShiftingSampleProvider(ISampleProvider sourceProvider)
    : this(sourceProvider, 4096 /*0x1000*/, 4L, 1f)
  {
  }

  public SmbPitchShiftingSampleProvider(
    ISampleProvider sourceProvider,
    int fftSize,
    long osamp,
    float initialPitch)
  {
    this.sourceStream = sourceProvider;
    this.waveFormat = sourceProvider.WaveFormat;
    this.fftSize = fftSize;
    this.osamp = osamp;
    this.PitchFactor = initialPitch;
  }

  public int Read(float[] buffer, int offset, int count)
  {
    int numSampsToProcess = this.sourceStream.Read(buffer, offset, count);
    if ((double) this.pitch == 1.0)
      return numSampsToProcess;
    if (this.waveFormat.Channels == 1)
    {
      float[] indata = new float[numSampsToProcess];
      int index1 = 0;
      for (int index2 = offset; index2 <= numSampsToProcess + offset - 1; ++index2)
      {
        indata[index1] = buffer[index2];
        ++index1;
      }
      this.shifterLeft.PitchShift(this.pitch, (long) numSampsToProcess, (long) this.fftSize, this.osamp, (float) this.waveFormat.SampleRate, indata);
      int index3 = 0;
      for (int index4 = offset; index4 <= numSampsToProcess + offset - 1; ++index4)
      {
        buffer[index4] = this.Limiter(indata[index3]);
        ++index3;
      }
      return numSampsToProcess;
    }
    if (this.waveFormat.Channels != 2)
      throw new Exception("Shifting of more than 2 channels is currently not supported.");
    float[] indata1 = new float[numSampsToProcess >> 1];
    float[] indata2 = new float[numSampsToProcess >> 1];
    int index5 = 0;
    for (int index6 = offset; index6 <= numSampsToProcess + offset - 1; index6 += 2)
    {
      indata1[index5] = buffer[index6];
      indata2[index5] = buffer[index6 + 1];
      ++index5;
    }
    this.shifterLeft.PitchShift(this.pitch, (long) (numSampsToProcess >> 1), (long) this.fftSize, this.osamp, (float) this.waveFormat.SampleRate, indata1);
    this.shifterRight.PitchShift(this.pitch, (long) (numSampsToProcess >> 1), (long) this.fftSize, this.osamp, (float) this.waveFormat.SampleRate, indata2);
    int index7 = 0;
    for (int index8 = offset; index8 <= numSampsToProcess + offset - 1; index8 += 2)
    {
      buffer[index8] = this.Limiter(indata1[index7]);
      buffer[index8 + 1] = this.Limiter(indata2[index7]);
      ++index7;
    }
    return numSampsToProcess;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public float PitchFactor
  {
    get => this.pitch;
    set => this.pitch = value;
  }

  private float Limiter(float sample)
  {
    return 0.949999988079071 >= (double) sample ? ((double) sample >= -0.949999988079071 ? sample : -(float) (Math.Atan(-((double) sample + 0.949999988079071) / 0.050000011920928955) / 1.5707963705062866 * 0.050000011920928955 + 0.949999988079071)) : (float) (Math.Atan(((double) sample - 0.949999988079071) / 0.050000011920928955) / 1.5707963705062866 * 0.050000011920928955 + 0.949999988079071);
  }
}
