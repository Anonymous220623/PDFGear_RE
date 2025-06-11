// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveMixerStream32
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NAudio.Wave;

public class WaveMixerStream32 : WaveStream
{
  private readonly List<WaveStream> inputStreams;
  private readonly object inputsLock;
  private WaveFormat waveFormat;
  private long length;
  private long position;
  private readonly int bytesPerSample;

  public WaveMixerStream32()
  {
    this.AutoStop = true;
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
    this.bytesPerSample = 4;
    this.inputStreams = new List<WaveStream>();
    this.inputsLock = new object();
  }

  public WaveMixerStream32(IEnumerable<WaveStream> inputStreams, bool autoStop)
    : this()
  {
    this.AutoStop = autoStop;
    foreach (WaveStream inputStream in inputStreams)
      this.AddInputStream(inputStream);
  }

  public void AddInputStream(WaveStream waveStream)
  {
    if (waveStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
      throw new ArgumentException("Must be IEEE floating point", nameof (waveStream));
    if (waveStream.WaveFormat.BitsPerSample != 32 /*0x20*/)
      throw new ArgumentException("Only 32 bit audio currently supported", nameof (waveStream));
    if (this.inputStreams.Count == 0)
      this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(waveStream.WaveFormat.SampleRate, waveStream.WaveFormat.Channels);
    else if (!waveStream.WaveFormat.Equals((object) this.waveFormat))
      throw new ArgumentException("All incoming channels must have the same format", nameof (waveStream));
    lock (this.inputsLock)
    {
      this.inputStreams.Add(waveStream);
      this.length = Math.Max(this.length, waveStream.Length);
      waveStream.Position = this.Position;
    }
  }

  public void RemoveInputStream(WaveStream waveStream)
  {
    lock (this.inputsLock)
    {
      if (!this.inputStreams.Remove(waveStream))
        return;
      long val1 = 0;
      foreach (WaveStream inputStream in this.inputStreams)
        val1 = Math.Max(val1, inputStream.Length);
      this.length = val1;
    }
  }

  public int InputCount => this.inputStreams.Count;

  public bool AutoStop { get; set; }

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (this.AutoStop && this.position + (long) count > this.length)
      count = (int) (this.length - this.position);
    if (count % this.bytesPerSample != 0)
      throw new ArgumentException("Must read an whole number of samples", nameof (count));
    Array.Clear((Array) buffer, offset, count);
    int val1 = 0;
    byte[] numArray = new byte[count];
    lock (this.inputsLock)
    {
      foreach (WaveStream inputStream in this.inputStreams)
      {
        if (inputStream.HasData(count))
        {
          int num = inputStream.Read(numArray, 0, count);
          val1 = Math.Max(val1, num);
          if (num > 0)
            WaveMixerStream32.Sum32BitAudio(buffer, offset, numArray, num);
        }
        else
        {
          val1 = Math.Max(val1, count);
          inputStream.Position += (long) count;
        }
      }
    }
    this.position += (long) count;
    return count;
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

  public override int BlockAlign => this.waveFormat.BlockAlign;

  public override long Length => this.length;

  public override long Position
  {
    get => this.position;
    set
    {
      lock (this.inputsLock)
      {
        value = Math.Min(value, this.Length);
        foreach (WaveStream inputStream in this.inputStreams)
          inputStream.Position = Math.Min(value, inputStream.Length);
        this.position = value;
      }
    }
  }

  public override WaveFormat WaveFormat => this.waveFormat;

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      lock (this.inputsLock)
      {
        foreach (Stream inputStream in this.inputStreams)
          inputStream.Dispose();
      }
    }
    base.Dispose(disposing);
  }
}
