// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.EnvelopeDetector
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Dsp;

internal class EnvelopeDetector
{
  private double sampleRate;
  private double ms;
  private double coeff;

  public EnvelopeDetector()
    : this(1.0, 44100.0)
  {
  }

  public EnvelopeDetector(double ms, double sampleRate)
  {
    this.sampleRate = sampleRate;
    this.ms = ms;
    this.SetCoef();
  }

  public double TimeConstant
  {
    get => this.ms;
    set
    {
      this.ms = value;
      this.SetCoef();
    }
  }

  public double SampleRate
  {
    get => this.sampleRate;
    set
    {
      this.sampleRate = value;
      this.SetCoef();
    }
  }

  public double Run(double inValue, double state) => inValue + this.coeff * (state - inValue);

  private void SetCoef() => this.coeff = Math.Exp(-1.0 / (0.001 * this.ms * this.sampleRate));
}
