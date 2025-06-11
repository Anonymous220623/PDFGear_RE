// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.PanningSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class PanningSampleProvider : ISampleProvider
{
  private readonly ISampleProvider source;
  private float pan;
  private float leftMultiplier;
  private float rightMultiplier;
  private readonly WaveFormat waveFormat;
  private float[] sourceBuffer;
  private IPanStrategy panStrategy;

  public PanningSampleProvider(ISampleProvider source)
  {
    if (source.WaveFormat.Channels != 1)
      throw new ArgumentException("Source sample provider must be mono");
    this.source = source;
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(source.WaveFormat.SampleRate, 2);
    this.panStrategy = (IPanStrategy) new SinPanStrategy();
  }

  public float Pan
  {
    get => this.pan;
    set
    {
      this.pan = (double) value >= -1.0 && (double) value <= 1.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Pan must be in the range -1 to 1");
      this.UpdateMultipliers();
    }
  }

  public IPanStrategy PanStrategy
  {
    get => this.panStrategy;
    set
    {
      this.panStrategy = value;
      this.UpdateMultipliers();
    }
  }

  private void UpdateMultipliers()
  {
    StereoSamplePair multipliers = this.panStrategy.GetMultipliers(this.Pan);
    this.leftMultiplier = multipliers.Left;
    this.rightMultiplier = multipliers.Right;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public int Read(float[] buffer, int offset, int count)
  {
    int num1 = count / 2;
    this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num1);
    int num2 = this.source.Read(this.sourceBuffer, 0, num1);
    int num3 = offset;
    for (int index1 = 0; index1 < num2; ++index1)
    {
      float[] numArray1 = buffer;
      int index2 = num3;
      int num4 = index2 + 1;
      double num5 = (double) this.leftMultiplier * (double) this.sourceBuffer[index1];
      numArray1[index2] = (float) num5;
      float[] numArray2 = buffer;
      int index3 = num4;
      num3 = index3 + 1;
      double num6 = (double) this.rightMultiplier * (double) this.sourceBuffer[index1];
      numArray2[index3] = (float) num6;
    }
    return num2 * 2;
  }
}
