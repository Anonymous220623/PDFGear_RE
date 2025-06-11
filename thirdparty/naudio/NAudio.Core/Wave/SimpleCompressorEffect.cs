// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SimpleCompressorEffect
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Dsp;

#nullable disable
namespace NAudio.Wave;

public class SimpleCompressorEffect : ISampleProvider
{
  private readonly ISampleProvider sourceStream;
  private readonly SimpleCompressor simpleCompressor;
  private readonly int channels;
  private readonly object lockObject = new object();

  public SimpleCompressorEffect(ISampleProvider sourceStream)
  {
    this.sourceStream = sourceStream;
    this.channels = sourceStream.WaveFormat.Channels;
    this.simpleCompressor = new SimpleCompressor(5.0, 10.0, (double) sourceStream.WaveFormat.SampleRate);
    this.simpleCompressor.Threshold = 16.0;
    this.simpleCompressor.Ratio = 6.0;
    this.simpleCompressor.MakeUpGain = 16.0;
  }

  public double MakeUpGain
  {
    get => this.simpleCompressor.MakeUpGain;
    set
    {
      lock (this.lockObject)
        this.simpleCompressor.MakeUpGain = value;
    }
  }

  public double Threshold
  {
    get => this.simpleCompressor.Threshold;
    set
    {
      lock (this.lockObject)
        this.simpleCompressor.Threshold = value;
    }
  }

  public double Ratio
  {
    get => this.simpleCompressor.Ratio;
    set
    {
      lock (this.lockObject)
        this.simpleCompressor.Ratio = value;
    }
  }

  public double Attack
  {
    get => this.simpleCompressor.Attack;
    set
    {
      lock (this.lockObject)
        this.simpleCompressor.Attack = value;
    }
  }

  public double Release
  {
    get => this.simpleCompressor.Release;
    set
    {
      lock (this.lockObject)
        this.simpleCompressor.Release = value;
    }
  }

  public bool Enabled { get; set; }

  public WaveFormat WaveFormat => this.sourceStream.WaveFormat;

  public int Read(float[] array, int offset, int count)
  {
    lock (this.lockObject)
    {
      int num = this.sourceStream.Read(array, offset, count);
      if (this.Enabled)
      {
        for (int index = 0; index < num; index += this.channels)
        {
          double in1 = (double) array[offset + index];
          double in2 = this.channels == 1 ? 0.0 : (double) array[offset + index + 1];
          this.simpleCompressor.Process(ref in1, ref in2);
          array[offset + index] = (float) in1;
          if (this.channels > 1)
            array[offset + index + 1] = (float) in2;
        }
      }
      return num;
    }
  }
}
