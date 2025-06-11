// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Mp3FileReaderBase
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace NAudio.Wave;

public class Mp3FileReaderBase : WaveStream
{
  private readonly WaveFormat waveFormat;
  private Stream mp3Stream;
  private readonly long mp3DataLength;
  private readonly long dataStartPosition;
  private readonly XingHeader xingHeader;
  private readonly bool ownInputStream;
  private List<Mp3Index> tableOfContents;
  private int tocIndex;
  private long totalSamples;
  private readonly int bytesPerSample;
  private readonly int bytesPerDecodedFrame;
  private IMp3FrameDecompressor decompressor;
  private readonly byte[] decompressBuffer;
  private int decompressBufferOffset;
  private int decompressLeftovers;
  private bool repositionedFlag;
  private long position;
  private readonly object repositionLock = new object();

  public Mp3WaveFormat Mp3WaveFormat { get; private set; }

  public Mp3FileReaderBase(
    string mp3FileName,
    Mp3FileReaderBase.FrameDecompressorBuilder frameDecompressorBuilder)
    : this((Stream) File.OpenRead(mp3FileName), frameDecompressorBuilder, true)
  {
  }

  public Mp3FileReaderBase(
    Stream inputStream,
    Mp3FileReaderBase.FrameDecompressorBuilder frameDecompressorBuilder)
    : this(inputStream, frameDecompressorBuilder, false)
  {
  }

  protected Mp3FileReaderBase(
    Stream inputStream,
    Mp3FileReaderBase.FrameDecompressorBuilder frameDecompressorBuilder,
    bool ownInputStream)
  {
    if (inputStream == null)
      throw new ArgumentNullException(nameof (inputStream));
    if (frameDecompressorBuilder == null)
      throw new ArgumentNullException(nameof (frameDecompressorBuilder));
    this.ownInputStream = ownInputStream;
    try
    {
      this.mp3Stream = inputStream;
      this.Id3v2Tag = Id3v2Tag.ReadTag(this.mp3Stream);
      this.dataStartPosition = this.mp3Stream.Position;
      Mp3Frame frame = Mp3Frame.LoadFromStream(this.mp3Stream);
      double bitRate1 = frame != null ? (double) frame.BitRate : throw new InvalidDataException("Invalid MP3 file - no MP3 Frames Detected");
      this.xingHeader = XingHeader.LoadXingHeader(frame);
      if (this.xingHeader != null)
        this.dataStartPosition = this.mp3Stream.Position;
      Mp3Frame mp3Frame = Mp3Frame.LoadFromStream(this.mp3Stream);
      if (mp3Frame != null && (mp3Frame.SampleRate != frame.SampleRate || mp3Frame.ChannelMode != frame.ChannelMode))
      {
        this.dataStartPosition = mp3Frame.FileOffset;
        frame = mp3Frame;
      }
      this.mp3DataLength = this.mp3Stream.Length - this.dataStartPosition;
      this.mp3Stream.Position = this.mp3Stream.Length - 128L /*0x80*/;
      byte[] buffer = new byte[128 /*0x80*/];
      this.mp3Stream.Read(buffer, 0, 128 /*0x80*/);
      if (buffer[0] == (byte) 84 && buffer[1] == (byte) 65 && buffer[2] == (byte) 71)
      {
        this.Id3v1Tag = buffer;
        this.mp3DataLength -= 128L /*0x80*/;
      }
      this.mp3Stream.Position = this.dataStartPosition;
      this.Mp3WaveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, (int) bitRate1);
      this.CreateTableOfContents();
      this.tocIndex = 0;
      double bitRate2 = (double) this.mp3DataLength * 8.0 / this.TotalSeconds();
      this.mp3Stream.Position = this.dataStartPosition;
      this.Mp3WaveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, (int) bitRate2);
      this.decompressor = frameDecompressorBuilder((WaveFormat) this.Mp3WaveFormat);
      this.waveFormat = this.decompressor.OutputFormat;
      this.bytesPerSample = this.decompressor.OutputFormat.BitsPerSample / 8 * this.decompressor.OutputFormat.Channels;
      this.bytesPerDecodedFrame = 1152 * this.bytesPerSample;
      this.decompressBuffer = new byte[this.bytesPerDecodedFrame * 2];
    }
    catch (Exception ex)
    {
      if (ownInputStream)
        inputStream.Dispose();
      throw;
    }
  }

  private void CreateTableOfContents()
  {
    try
    {
      this.tableOfContents = new List<Mp3Index>((int) (this.mp3DataLength / 400L));
      Mp3Frame frame;
      do
      {
        Mp3Index mp3Index = new Mp3Index();
        mp3Index.FilePosition = this.mp3Stream.Position;
        mp3Index.SamplePosition = this.totalSamples;
        frame = this.ReadNextFrame(false);
        if (frame != null)
        {
          this.ValidateFrameFormat(frame);
          this.totalSamples += (long) frame.SampleCount;
          mp3Index.SampleCount = frame.SampleCount;
          mp3Index.ByteCount = (int) (this.mp3Stream.Position - mp3Index.FilePosition);
          this.tableOfContents.Add(mp3Index);
        }
      }
      while (frame != null);
    }
    catch (EndOfStreamException ex)
    {
    }
  }

  private void ValidateFrameFormat(Mp3Frame frame)
  {
    if (frame.SampleRate != this.Mp3WaveFormat.SampleRate)
      throw new InvalidOperationException($"Got a frame at sample rate {frame.SampleRate}, in an MP3 with sample rate {this.Mp3WaveFormat.SampleRate}. Mp3FileReader does not support sample rate changes.");
    if ((frame.ChannelMode == ChannelMode.Mono ? 1 : 2) != this.Mp3WaveFormat.Channels)
      throw new InvalidOperationException($"Got a frame with channel mode {frame.ChannelMode}, in an MP3 with {this.Mp3WaveFormat.Channels} channels. Mp3FileReader does not support changes to channel count.");
  }

  private double TotalSeconds()
  {
    return (double) this.totalSamples / (double) this.Mp3WaveFormat.SampleRate;
  }

  public Id3v2Tag Id3v2Tag { get; }

  public byte[] Id3v1Tag { get; }

  public Mp3Frame ReadNextFrame()
  {
    Mp3Frame mp3Frame = this.ReadNextFrame(true);
    if (mp3Frame != null)
      this.position += (long) (mp3Frame.SampleCount * this.bytesPerSample);
    return mp3Frame;
  }

  private Mp3Frame ReadNextFrame(bool readData)
  {
    Mp3Frame mp3Frame = (Mp3Frame) null;
    try
    {
      mp3Frame = Mp3Frame.LoadFromStream(this.mp3Stream, readData);
      if (mp3Frame != null)
        ++this.tocIndex;
    }
    catch (EndOfStreamException ex)
    {
    }
    return mp3Frame;
  }

  public override long Length => this.totalSamples * (long) this.bytesPerSample;

  public override WaveFormat WaveFormat => this.waveFormat;

  public override long Position
  {
    get => this.position;
    set
    {
      lock (this.repositionLock)
      {
        value = Math.Max(Math.Min(value, this.Length), 0L);
        long num1 = value / (long) this.bytesPerSample;
        Mp3Index mp3Index = (Mp3Index) null;
        for (int index = 0; index < this.tableOfContents.Count; ++index)
        {
          if (this.tableOfContents[index].SamplePosition + (long) this.tableOfContents[index].SampleCount > num1)
          {
            mp3Index = this.tableOfContents[index];
            this.tocIndex = index;
            break;
          }
        }
        this.decompressBufferOffset = 0;
        this.decompressLeftovers = 0;
        this.repositionedFlag = true;
        if (mp3Index != null)
        {
          this.mp3Stream.Position = mp3Index.FilePosition;
          long num2 = num1 - mp3Index.SamplePosition;
          if (num2 > 0L)
            this.decompressBufferOffset = (int) num2 * this.bytesPerSample;
        }
        else
          this.mp3Stream.Position = this.mp3DataLength + this.dataStartPosition;
        this.position = value;
      }
    }
  }

  public override int Read(byte[] sampleBuffer, int offset, int numBytes)
  {
    int num1 = 0;
    lock (this.repositionLock)
    {
      if (this.decompressLeftovers != 0)
      {
        int length = Math.Min(this.decompressLeftovers, numBytes);
        Array.Copy((Array) this.decompressBuffer, this.decompressBufferOffset, (Array) sampleBuffer, offset, length);
        this.decompressLeftovers -= length;
        if (this.decompressLeftovers == 0)
          this.decompressBufferOffset = 0;
        else
          this.decompressBufferOffset += length;
        num1 += length;
        offset += length;
      }
      int tocIndex = this.tocIndex;
      if (this.repositionedFlag)
      {
        this.decompressor.Reset();
        this.tocIndex = Math.Max(0, this.tocIndex - 3);
        this.mp3Stream.Position = this.tableOfContents[this.tocIndex].FilePosition;
        this.repositionedFlag = false;
      }
      while (num1 < numBytes)
      {
        Mp3Frame frame = this.ReadNextFrame(true);
        if (frame != null)
        {
          int num2 = this.decompressor.DecompressFrame(frame, this.decompressBuffer, 0);
          if (this.tocIndex > tocIndex && num2 != 0)
          {
            if (this.tocIndex == tocIndex + 1 && num2 == this.bytesPerDecodedFrame * 2)
            {
              Array.Copy((Array) this.decompressBuffer, this.bytesPerDecodedFrame, (Array) this.decompressBuffer, 0, this.bytesPerDecodedFrame);
              num2 = this.bytesPerDecodedFrame;
            }
            int length = Math.Min(num2 - this.decompressBufferOffset, numBytes - num1);
            Array.Copy((Array) this.decompressBuffer, this.decompressBufferOffset, (Array) sampleBuffer, offset, length);
            if (length + this.decompressBufferOffset < num2)
            {
              this.decompressBufferOffset = length + this.decompressBufferOffset;
              this.decompressLeftovers = num2 - this.decompressBufferOffset;
            }
            else
            {
              this.decompressBufferOffset = 0;
              this.decompressLeftovers = 0;
            }
            offset += length;
            num1 += length;
          }
        }
        else
          break;
      }
    }
    this.position += (long) num1;
    return num1;
  }

  public XingHeader XingHeader => this.xingHeader;

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      if (this.mp3Stream != null)
      {
        if (this.ownInputStream)
          this.mp3Stream.Dispose();
        this.mp3Stream = (Stream) null;
      }
      if (this.decompressor != null)
      {
        this.decompressor.Dispose();
        this.decompressor = (IMp3FrameDecompressor) null;
      }
    }
    base.Dispose(disposing);
  }

  public delegate IMp3FrameDecompressor FrameDecompressorBuilder(WaveFormat mp3Format);
}
