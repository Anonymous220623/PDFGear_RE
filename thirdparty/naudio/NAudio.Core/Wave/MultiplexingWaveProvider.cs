// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.MultiplexingWaveProvider
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NAudio.Wave;

public class MultiplexingWaveProvider : IWaveProvider
{
  private readonly IList<IWaveProvider> inputs;
  private readonly int outputChannelCount;
  private readonly int inputChannelCount;
  private readonly List<int> mappings;
  private readonly int bytesPerSample;
  private byte[] inputBuffer;

  public MultiplexingWaveProvider(IEnumerable<IWaveProvider> inputs)
    : this(inputs, -1)
  {
  }

  public MultiplexingWaveProvider(IEnumerable<IWaveProvider> inputs, int numberOfOutputChannels)
  {
    this.inputs = (IList<IWaveProvider>) new List<IWaveProvider>(inputs);
    this.outputChannelCount = numberOfOutputChannels == -1 ? this.inputs.Sum<IWaveProvider>((Func<IWaveProvider, int>) (i => i.WaveFormat.Channels)) : numberOfOutputChannels;
    if (this.inputs.Count == 0)
      throw new ArgumentException("You must provide at least one input");
    if (this.outputChannelCount < 1)
      throw new ArgumentException("You must provide at least one output");
    foreach (IWaveProvider input in (IEnumerable<IWaveProvider>) this.inputs)
    {
      if (this.WaveFormat == null)
      {
        if (input.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
        {
          this.WaveFormat = new WaveFormat(input.WaveFormat.SampleRate, input.WaveFormat.BitsPerSample, this.outputChannelCount);
        }
        else
        {
          if (input.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            throw new ArgumentException("Only PCM and 32 bit float are supported");
          this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(input.WaveFormat.SampleRate, this.outputChannelCount);
        }
      }
      else
      {
        if (input.WaveFormat.BitsPerSample != this.WaveFormat.BitsPerSample)
          throw new ArgumentException("All inputs must have the same bit depth");
        if (input.WaveFormat.SampleRate != this.WaveFormat.SampleRate)
          throw new ArgumentException("All inputs must have the same sample rate");
      }
      this.inputChannelCount += input.WaveFormat.Channels;
    }
    this.bytesPerSample = this.WaveFormat.BitsPerSample / 8;
    this.mappings = new List<int>();
    for (int index = 0; index < this.outputChannelCount; ++index)
      this.mappings.Add(index % this.inputChannelCount);
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    int num1 = this.bytesPerSample * this.outputChannelCount;
    int num2 = count / num1;
    int num3 = 0;
    int val1 = 0;
    foreach (IWaveProvider input in (IEnumerable<IWaveProvider>) this.inputs)
    {
      int num4 = this.bytesPerSample * input.WaveFormat.Channels;
      int num5 = num2 * num4;
      this.inputBuffer = BufferHelpers.Ensure(this.inputBuffer, num5);
      int num6 = input.Read(this.inputBuffer, 0, num5);
      val1 = Math.Max(val1, num6 / num4);
      for (int index1 = 0; index1 < input.WaveFormat.Channels; ++index1)
      {
        int num7 = num3 + index1;
        for (int index2 = 0; index2 < this.outputChannelCount; ++index2)
        {
          if (this.mappings[index2] == num7)
          {
            int sourceIndex = index1 * this.bytesPerSample;
            int num8 = offset + index2 * this.bytesPerSample;
            int num9;
            for (num9 = 0; num9 < num2 && sourceIndex < num6; ++num9)
            {
              Array.Copy((Array) this.inputBuffer, sourceIndex, (Array) buffer, num8, this.bytesPerSample);
              num8 += num1;
              sourceIndex += num4;
            }
            for (; num9 < num2; ++num9)
            {
              Array.Clear((Array) buffer, num8, this.bytesPerSample);
              num8 += num1;
            }
          }
        }
      }
      num3 += input.WaveFormat.Channels;
    }
    return val1 * num1;
  }

  public WaveFormat WaveFormat { get; }

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
