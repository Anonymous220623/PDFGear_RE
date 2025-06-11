// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.EnvelopeGenerator
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Dsp;

public class EnvelopeGenerator
{
  private EnvelopeGenerator.EnvelopeState state;
  private float output;
  private float attackRate;
  private float decayRate;
  private float releaseRate;
  private float attackCoef;
  private float decayCoef;
  private float releaseCoef;
  private float sustainLevel;
  private float targetRatioAttack;
  private float targetRatioDecayRelease;
  private float attackBase;
  private float decayBase;
  private float releaseBase;

  public EnvelopeGenerator()
  {
    this.Reset();
    this.AttackRate = 0.0f;
    this.DecayRate = 0.0f;
    this.ReleaseRate = 0.0f;
    this.SustainLevel = 1f;
    this.SetTargetRatioAttack(0.3f);
    this.SetTargetRatioDecayRelease(0.0001f);
  }

  public float AttackRate
  {
    get => this.attackRate;
    set
    {
      this.attackRate = value;
      this.attackCoef = EnvelopeGenerator.CalcCoef(value, this.targetRatioAttack);
      this.attackBase = (float) ((1.0 + (double) this.targetRatioAttack) * (1.0 - (double) this.attackCoef));
    }
  }

  public float DecayRate
  {
    get => this.decayRate;
    set
    {
      this.decayRate = value;
      this.decayCoef = EnvelopeGenerator.CalcCoef(value, this.targetRatioDecayRelease);
      this.decayBase = (float) (((double) this.sustainLevel - (double) this.targetRatioDecayRelease) * (1.0 - (double) this.decayCoef));
    }
  }

  public float ReleaseRate
  {
    get => this.releaseRate;
    set
    {
      this.releaseRate = value;
      this.releaseCoef = EnvelopeGenerator.CalcCoef(value, this.targetRatioDecayRelease);
      this.releaseBase = (float) (-(double) this.targetRatioDecayRelease * (1.0 - (double) this.releaseCoef));
    }
  }

  private static float CalcCoef(float rate, float targetRatio)
  {
    return (float) Math.Exp(-Math.Log((1.0 + (double) targetRatio) / (double) targetRatio) / (double) rate);
  }

  public float SustainLevel
  {
    get => this.sustainLevel;
    set
    {
      this.sustainLevel = value;
      this.decayBase = (float) (((double) this.sustainLevel - (double) this.targetRatioDecayRelease) * (1.0 - (double) this.decayCoef));
    }
  }

  private void SetTargetRatioAttack(float targetRatio)
  {
    if ((double) targetRatio < 9.9999997171806854E-10)
      targetRatio = 1E-09f;
    this.targetRatioAttack = targetRatio;
    this.attackBase = (float) ((1.0 + (double) this.targetRatioAttack) * (1.0 - (double) this.attackCoef));
  }

  private void SetTargetRatioDecayRelease(float targetRatio)
  {
    if ((double) targetRatio < 9.9999997171806854E-10)
      targetRatio = 1E-09f;
    this.targetRatioDecayRelease = targetRatio;
    this.decayBase = (float) (((double) this.sustainLevel - (double) this.targetRatioDecayRelease) * (1.0 - (double) this.decayCoef));
    this.releaseBase = (float) (-(double) this.targetRatioDecayRelease * (1.0 - (double) this.releaseCoef));
  }

  public float Process()
  {
    switch (this.state)
    {
      case EnvelopeGenerator.EnvelopeState.Attack:
        this.output = this.attackBase + this.output * this.attackCoef;
        if ((double) this.output >= 1.0)
        {
          this.output = 1f;
          this.state = EnvelopeGenerator.EnvelopeState.Decay;
          break;
        }
        break;
      case EnvelopeGenerator.EnvelopeState.Decay:
        this.output = this.decayBase + this.output * this.decayCoef;
        if ((double) this.output <= (double) this.sustainLevel)
        {
          this.output = this.sustainLevel;
          this.state = EnvelopeGenerator.EnvelopeState.Sustain;
          break;
        }
        break;
      case EnvelopeGenerator.EnvelopeState.Release:
        this.output = this.releaseBase + this.output * this.releaseCoef;
        if ((double) this.output <= 0.0)
        {
          this.output = 0.0f;
          this.state = EnvelopeGenerator.EnvelopeState.Idle;
          break;
        }
        break;
    }
    return this.output;
  }

  public void Gate(bool gate)
  {
    if (gate)
    {
      this.state = EnvelopeGenerator.EnvelopeState.Attack;
    }
    else
    {
      if (this.state == EnvelopeGenerator.EnvelopeState.Idle)
        return;
      this.state = EnvelopeGenerator.EnvelopeState.Release;
    }
  }

  public EnvelopeGenerator.EnvelopeState State => this.state;

  public void Reset()
  {
    this.state = EnvelopeGenerator.EnvelopeState.Idle;
    this.output = 0.0f;
  }

  public float GetOutput() => this.output;

  public enum EnvelopeState
  {
    Idle,
    Attack,
    Decay,
    Sustain,
    Release,
  }
}
