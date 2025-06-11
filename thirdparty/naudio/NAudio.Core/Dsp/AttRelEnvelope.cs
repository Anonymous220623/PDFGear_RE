// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.AttRelEnvelope
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Dsp;

internal class AttRelEnvelope
{
  protected const double DC_OFFSET = 1E-25;
  private readonly EnvelopeDetector attack;
  private readonly EnvelopeDetector release;

  public AttRelEnvelope(double attackMilliseconds, double releaseMilliseconds, double sampleRate)
  {
    this.attack = new EnvelopeDetector(attackMilliseconds, sampleRate);
    this.release = new EnvelopeDetector(releaseMilliseconds, sampleRate);
  }

  public double Attack
  {
    get => this.attack.TimeConstant;
    set => this.attack.TimeConstant = value;
  }

  public double Release
  {
    get => this.release.TimeConstant;
    set => this.release.TimeConstant = value;
  }

  public double SampleRate
  {
    get => this.attack.SampleRate;
    set => this.attack.SampleRate = this.release.SampleRate = value;
  }

  public double Run(double inValue, double state)
  {
    return inValue <= state ? this.release.Run(inValue, state) : this.attack.Run(inValue, state);
  }
}
