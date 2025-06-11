// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.MixingWaveProvider32
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NAudio.Wave;

public class MixingWaveProvider32 : IWaveProvider
{
  private List<IWaveProvider> inputs;
  private WaveFormat waveFormat;
  private int bytesPerSample;

  public MixingWaveProvider32()
  {
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    this.bytesPerSample = 4;
    this.inputs = new List<IWaveProvider>();
  }

  public MixingWaveProvider32(IEnumerable<IWaveProvider> inputs)
    : this()
  {
    foreach (IWaveProvider input in inputs)
      this.AddInputStream(input);
  }

  public void AddInputStream(IWaveProvider waveProvider)
  {
    if (waveProvider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Must be IEEE floating point", "waveProvider.WaveFormat");
    if (waveProvider.WaveFormat.BitsPerSample != 32 /*0x20*/)
      throw new ArgumentException("Only 32 bit audio currently supported", "waveProvider.WaveFormat");
    if (this.inputs.Count == 0)
      this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(waveProvider.WaveFormat.SampleRate, waveProvider.WaveFormat.Channels);
    else if (!waveProvider.WaveFormat.Equals((object) this.waveFormat))
      throw new ArgumentException("All incoming channels must have the same format", "waveProvider.WaveFormat");
    lock (this.inputs)
      this.inputs.Add(waveProvider);
  }

  public void RemoveInputStream(IWaveProvider waveProvider)
  {
    lock (this.inputs)
      this.inputs.Remove(waveProvider);
  }

  public int InputCount => this.inputs.Count;

  public int Read(byte[] buffer, int offset, int count)
  {
    if (count % this.bytesPerSample != 0)
      throw new ArgumentException("Must read an whole number of samples", nameof (count));
    Array.Clear((Array) buffer, offset, count);
    int val1 = 0;
    byte[] numArray = new byte[count];
    lock (this.inputs)
    {
      foreach (IWaveProvider input in this.inputs)
      {
        int num = input.Read(numArray, 0, count);
        val1 = Math.Max(val1, num);
        if (num > 0)
          MixingWaveProvider32.Sum32BitAudio(buffer, offset, numArray, num);
      }
    }
    return val1;
  }

  private static unsafe void Sum32BitAudio(
    byte[] destBuffer,
    int offset,
    byte[] sourceBuffer,
    int bytesRead)
  {
    fixed (byte* numPtr1 = &destBuffer[offset])
      fixed (byte* numPtr2 = &sourceBuffer[0])
      {
        float* numPtr3 = (float*) numPtr1;
        float* numPtr4 = (float*) numPtr2;
        int num = bytesRead / 4;
        for (int index = 0; index < num; ++index)
        {
          float* numPtr5 = numPtr3 + index;
          *numPtr5 = *numPtr5 + numPtr4[index];
        }
      }
  }

  public WaveFormat WaveFormat => this.waveFormat;
}
