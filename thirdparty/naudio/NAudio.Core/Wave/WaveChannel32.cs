// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveChannel32
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Wave.SampleProviders;
using System;

#nullable disable
namespace NAudio.Wave;

public class WaveChannel32 : WaveStream, ISampleNotifier
{
  private WaveStream sourceStream;
  private readonly WaveFormat waveFormat;
  private readonly long length;
  private readonly int destBytesPerSample;
  private readonly int sourceBytesPerSample;
  private volatile float volume;
  private volatile float pan;
  private long position;
  private readonly ISampleChunkConverter sampleProvider;
  private readonly object lockObject = new object();
  private SampleEventArgs sampleEventArgs = new SampleEventArgs(0.0f, 0.0f);

  public WaveChannel32(WaveStream sourceStream, float volume, float pan)
  {
    this.PadWithZeroes = true;
    ISampleChunkConverter[] sampleChunkConverterArray = new ISampleChunkConverter[8]
    {
      (ISampleChunkConverter) new Mono8SampleChunkConverter(),
      (ISampleChunkConverter) new Stereo8SampleChunkConverter(),
      (ISampleChunkConverter) new Mono16SampleChunkConverter(),
      (ISampleChunkConverter) new Stereo16SampleChunkConverter(),
      (ISampleChunkConverter) new Mono24SampleChunkConverter(),
      (ISampleChunkConverter) new Stereo24SampleChunkConverter(),
      (ISampleChunkConverter) new MonoFloatSampleChunkConverter(),
      (ISampleChunkConverter) new StereoFloatSampleChunkConverter()
    };
    foreach (ISampleChunkConverter sampleChunkConverter in sampleChunkConverterArray)
    {
      if (sampleChunkConverter.Supports(sourceStream.WaveFormat))
      {
        this.sampleProvider = sampleChunkConverter;
        break;
      }
    }
    if (this.sampleProvider == null)
      throw new ArgumentException("Unsupported sourceStream format");
    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sourceStream.WaveFormat.SampleRate, 2);
    this.destBytesPerSample = 8;
    this.sourceStream = sourceStream;
    this.volume = volume;
    this.pan = pan;
    this.sourceBytesPerSample = sourceStream.WaveFormat.Channels * sourceStream.WaveFormat.BitsPerSample / 8;
    this.length = this.SourceToDest(sourceStream.Length);
    this.position = 0L;
  }

  private long SourceToDest(long sourceBytes)
  {
    return sourceBytes / (long) this.sourceBytesPerSample * (long) this.destBytesPerSample;
  }

  private long DestToSource(long destBytes)
  {
    return destBytes / (long) this.destBytesPerSample * (long) this.sourceBytesPerSample;
  }

  public WaveChannel32(WaveStream sourceStream)
    : this(sourceStream, 1f, 0.0f)
  {
  }

  public override int BlockAlign => (int) this.SourceToDest((long) this.sourceStream.BlockAlign);

  public override long Length => this.length;

  public override long Position
  {
    get => this.position;
    set
    {
      lock (this.lockObject)
      {
        value -= value % (long) this.BlockAlign;
        if (value < 0L)
          this.sourceStream.Position = 0L;
        else
          this.sourceStream.Position = this.DestToSource(value);
        this.position = this.SourceToDest(this.sourceStream.Position);
      }
    }
  }

  public override int Read(byte[] destBuffer, int offset, int numBytes)
  {
    lock (this.lockObject)
    {
      int num1 = 0;
      WaveBuffer waveBuffer = new WaveBuffer(destBuffer);
      if (this.position < 0L)
      {
        num1 = (int) Math.Min((long) numBytes, -this.position);
        for (int index = 0; index < num1; ++index)
          destBuffer[index + offset] = (byte) 0;
      }
      if (num1 < numBytes)
      {
        this.sampleProvider.LoadNextChunk((IWaveProvider) this.sourceStream, (numBytes - num1) / 8);
        int num2 = offset / 4 + num1 / 4;
        float sampleLeft;
        float sampleRight;
        while (this.sampleProvider.GetNextSample(out sampleLeft, out sampleRight) && num1 < numBytes)
        {
          sampleLeft = (double) this.pan <= 0.0 ? sampleLeft : (float) ((double) sampleLeft * (1.0 - (double) this.pan) / 2.0);
          float num3 = (double) this.pan >= 0.0 ? sampleRight : (float) ((double) sampleRight * ((double) this.pan + 1.0) / 2.0);
          sampleLeft *= this.volume;
          sampleRight = num3 * this.volume;
          float[] floatBuffer1 = waveBuffer.FloatBuffer;
          int index1 = num2;
          int num4 = index1 + 1;
          double num5 = (double) sampleLeft;
          floatBuffer1[index1] = (float) num5;
          float[] floatBuffer2 = waveBuffer.FloatBuffer;
          int index2 = num4;
          num2 = index2 + 1;
          double num6 = (double) sampleRight;
          floatBuffer2[index2] = (float) num6;
          num1 += 8;
          if (this.Sample != null)
            this.RaiseSample(sampleLeft, sampleRight);
        }
      }
      if (this.PadWithZeroes && num1 < numBytes)
      {
        Array.Clear((Array) destBuffer, offset + num1, numBytes - num1);
        num1 = numBytes;
      }
      this.position += (long) num1;
      return num1;
    }
  }

  public bool PadWithZeroes { get; set; }

  public override WaveFormat WaveFormat => this.waveFormat;

  public float Volume
  {
    get => this.volume;
    set => this.volume = value;
  }

  public float Pan
  {
    get => this.pan;
    set => this.pan = value;
  }

  public override bool HasData(int count)
  {
    return this.sourceStream.HasData(count) && this.position + (long) count >= 0L && this.position < this.length && (double) this.volume != 0.0;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.sourceStream != null)
    {
      this.sourceStream.Dispose();
      this.sourceStream = (WaveStream) null;
    }
    base.Dispose(disposing);
  }

  public event EventHandler<SampleEventArgs> Sample;

  private void RaiseSample(float left, float right)
  {
    this.sampleEventArgs.Left = left;
    this.sampleEventArgs.Right = right;
    this.Sample((object) this, this.sampleEventArgs);
  }
}
