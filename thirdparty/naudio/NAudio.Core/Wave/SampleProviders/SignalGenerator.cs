// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.SignalGenerator
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class SignalGenerator : ISampleProvider
{
  private readonly WaveFormat waveFormat;
  private readonly Random random = new Random();
  private readonly double[] pinkNoiseBuffer = new double[7];
  private const double TwoPi = 6.2831853071795862;
  private int nSample;
  private double phi;

  public SignalGenerator()
    : this(44100, 2)
  {
  }

  public SignalGenerator(int sampleRate, int channel)
  {
    this.phi = 0.0;
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channel);
    this.Type = SignalGeneratorType.Sin;
    this.Frequency = 440.0;
    this.Gain = 1.0;
    this.PhaseReverse = new bool[channel];
    this.SweepLengthSecs = 2.0;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public double Frequency { get; set; }

  public double FrequencyLog => Math.Log(this.Frequency);

  public double FrequencyEnd { get; set; }

  public double FrequencyEndLog => Math.Log(this.FrequencyEnd);

  public double Gain { get; set; }

  public bool[] PhaseReverse { get; }

  public SignalGeneratorType Type { get; set; }

  public double SweepLengthSecs { get; set; }

  public int Read(float[] buffer, int offset, int count)
  {
    int num1 = offset;
    for (int index1 = 0; index1 < count / this.waveFormat.Channels; ++index1)
    {
      double num2;
      switch (this.Type)
      {
        case SignalGeneratorType.Pink:
          double num3 = this.NextRandomTwo();
          this.pinkNoiseBuffer[0] = 0.99886 * this.pinkNoiseBuffer[0] + num3 * 0.0555179;
          this.pinkNoiseBuffer[1] = 0.99332 * this.pinkNoiseBuffer[1] + num3 * 0.0750759;
          this.pinkNoiseBuffer[2] = 0.969 * this.pinkNoiseBuffer[2] + num3 * 0.153852;
          this.pinkNoiseBuffer[3] = 0.8665 * this.pinkNoiseBuffer[3] + num3 * 0.3104856;
          this.pinkNoiseBuffer[4] = 0.55 * this.pinkNoiseBuffer[4] + num3 * 0.5329522;
          this.pinkNoiseBuffer[5] = -476.0 / 625.0 * this.pinkNoiseBuffer[5] - num3 * 0.016898;
          double num4 = this.pinkNoiseBuffer[0] + this.pinkNoiseBuffer[1] + this.pinkNoiseBuffer[2] + this.pinkNoiseBuffer[3] + this.pinkNoiseBuffer[4] + this.pinkNoiseBuffer[5] + this.pinkNoiseBuffer[6] + num3 * 0.5362;
          this.pinkNoiseBuffer[6] = num3 * 0.115926;
          num2 = this.Gain * (num4 / 5.0);
          break;
        case SignalGeneratorType.White:
          num2 = this.Gain * this.NextRandomTwo();
          break;
        case SignalGeneratorType.Sweep:
          this.phi += 2.0 * Math.PI * Math.Exp(this.FrequencyLog + (double) this.nSample * (this.FrequencyEndLog - this.FrequencyLog) / (this.SweepLengthSecs * (double) this.waveFormat.SampleRate)) / (double) this.waveFormat.SampleRate;
          num2 = this.Gain * Math.Sin(this.phi);
          ++this.nSample;
          if ((double) this.nSample > this.SweepLengthSecs * (double) this.waveFormat.SampleRate)
          {
            this.nSample = 0;
            this.phi = 0.0;
            break;
          }
          break;
        case SignalGeneratorType.Sin:
          num2 = this.Gain * Math.Sin((double) this.nSample * (2.0 * Math.PI * this.Frequency / (double) this.waveFormat.SampleRate));
          ++this.nSample;
          break;
        case SignalGeneratorType.Square:
          num2 = (double) this.nSample * (2.0 * this.Frequency / (double) this.waveFormat.SampleRate) % 2.0 - 1.0 >= 0.0 ? this.Gain : -this.Gain;
          ++this.nSample;
          break;
        case SignalGeneratorType.Triangle:
          double num5 = 2.0 * ((double) this.nSample * (2.0 * this.Frequency / (double) this.waveFormat.SampleRate) % 2.0);
          if (num5 > 1.0)
            num5 = 2.0 - num5;
          if (num5 < -1.0)
            num5 = -2.0 - num5;
          num2 = num5 * this.Gain;
          ++this.nSample;
          break;
        case SignalGeneratorType.SawTooth:
          num2 = this.Gain * ((double) this.nSample * (2.0 * this.Frequency / (double) this.waveFormat.SampleRate) % 2.0 - 1.0);
          ++this.nSample;
          break;
        default:
          num2 = 0.0;
          break;
      }
      for (int index2 = 0; index2 < this.waveFormat.Channels; ++index2)
        buffer[num1++] = !this.PhaseReverse[index2] ? (float) num2 : (float) -num2;
    }
    return count;
  }

  private double NextRandomTwo() => 2.0 * this.random.NextDouble() - 1.0;
}
