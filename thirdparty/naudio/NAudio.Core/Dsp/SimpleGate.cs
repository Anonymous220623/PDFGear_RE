// Decompiled with JetBrains decompiler
// Type: NAudio.Dsp.SimpleGate
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Dsp;

internal class SimpleGate : AttRelEnvelope
{
  private double threshdB;
  private double thresh;
  private double env;

  public SimpleGate()
    : base(10.0, 10.0, 44100.0)
  {
    this.threshdB = 0.0;
    this.thresh = 1.0;
    this.env = 1E-25;
  }

  public void Process(ref double in1, ref double in2)
  {
    this.env = this.Run((Math.Max(Math.Abs(in1), Math.Abs(in2)) > this.thresh ? 1.0 : 0.0) + 1E-25, this.env);
    double num = this.env - 1E-25;
    in1 *= num;
    in2 *= num;
  }

  public double Threshold
  {
    get => this.threshdB;
    set
    {
      this.threshdB = value;
      this.thresh = Decibels.DecibelsToLinear(value);
    }
  }
}
