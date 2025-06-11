// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.SampleProviders.MultiplexingSampleProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace NAudio.Wave.SampleProviders;

public class MultiplexingSampleProvider : ISampleProvider
{
  private readonly IList<ISampleProvider> inputs;
  private readonly WaveFormat waveFormat;
  private readonly int outputChannelCount;
  private readonly int inputChannelCount;
  private readonly List<int> mappings;
  private float[] inputBuffer;

  public MultiplexingSampleProvider(IEnumerable<ISampleProvider> inputs, int numberOfOutputChannels)
  {
    this.inputs = (IList<ISampleProvider>) new List<ISampleProvider>(inputs);
    this.outputChannelCount = numberOfOutputChannels;
    if (this.inputs.Count == 0)
      throw new ArgumentException("You must provide at least one input");
    if (numberOfOutputChannels < 1)
      throw new ArgumentException("You must provide at least one output");
    foreach (ISampleProvider input in (IEnumerable<ISampleProvider>) this.inputs)
    {
      if (this.waveFormat == null)
      {
        if (input.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
          throw new ArgumentException("Only 32 bit float is supported");
        this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(input.WaveFormat.SampleRate, numberOfOutputChannels);
      }
      else
      {
        if (input.WaveFormat.BitsPerSample != this.waveFormat.BitsPerSample)
          throw new ArgumentException("All inputs must have the same bit depth");
        if (input.WaveFormat.SampleRate != this.waveFormat.SampleRate)
          throw new ArgumentException("All inputs must have the same sample rate");
      }
      this.inputChannelCount += input.WaveFormat.Channels;
    }
    this.mappings = new List<int>();
    for (int index = 0; index < this.outputChannelCount; ++index)
      this.mappings.Add(index % this.inputChannelCount);
  }

  public int Read(float[] buffer, int offset, int count)
  {
    int num1 = count / this.outputChannelCount;
    int num2 = 0;
    int val1 = 0;
    foreach (ISampleProvider input in (IEnumerable<ISampleProvider>) this.inputs)
    {
      int num3 = num1 * input.WaveFormat.Channels;
      this.inputBuffer = BufferHelpers.Ensure(this.inputBuffer, num3);
      int num4 = input.Read(this.inputBuffer, 0, num3);
      val1 = Math.Max(val1, num4 / input.WaveFormat.Channels);
      for (int index1 = 0; index1 < input.WaveFormat.Channels; ++index1)
      {
        int num5 = num2 + index1;
        for (int index2 = 0; index2 < this.outputChannelCount; ++index2)
        {
          if (this.mappings[index2] == num5)
          {
            int index3 = index1;
            int index4 = offset + index2;
            int num6;
            for (num6 = 0; num6 < num1 && index3 < num4; ++num6)
            {
              buffer[index4] = this.inputBuffer[index3];
              index4 += this.outputChannelCount;
              index3 += input.WaveFormat.Channels;
            }
            for (; num6 < num1; ++num6)
            {
              buffer[index4] = 0.0f;
              index4 += this.outputChannelCount;
            }
          }
        }
      }
      num2 += input.WaveFormat.Channels;
    }
    return val1 * this.outputChannelCount;
  }

  public WaveFormat WaveFormat => this.waveFormat;

  public void ConnectInputToOutput(int inputChannel, int outputChannel)
  {
    if (inputChannel < 0 || inputChannel >= this.InputChannelCount)
      throw new ArgumentException("Invalid input channel");
    if (outputChannel < 0 || outputChannel >= this.OutputChannelCount)
      throw new ArgumentException("Invalid output channel");
    this.mappings[outputChannel] = inputChannel;
  }

  public int InputChannelCount => this.inputChannelCount;

  public int OutputChannelCount => this.outputChannelCount;
}
