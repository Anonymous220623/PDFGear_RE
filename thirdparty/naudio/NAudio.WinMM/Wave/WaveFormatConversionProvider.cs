// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFormatConversionProvider
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using NAudio.Wave.Compression;
using System;

#nullable disable
namespace NAudio.Wave;

public class WaveFormatConversionProvider : IWaveProvider, IDisposable
{
  private readonly AcmStream conversionStream;
  private readonly IWaveProvider sourceProvider;
  private readonly int preferredSourceReadSize;
  private int leftoverDestBytes;
  private int leftoverDestOffset;
  private int leftoverSourceBytes;
  private bool isDisposed;

  public WaveFormatConversionProvider(WaveFormat targetFormat, IWaveProvider sourceProvider)
  {
    this.sourceProvider = sourceProvider;
    this.WaveFormat = targetFormat;
    this.conversionStream = new AcmStream(sourceProvider.WaveFormat, targetFormat);
    this.preferredSourceReadSize = Math.Min(sourceProvider.WaveFormat.AverageBytesPerSecond, this.conversionStream.SourceBuffer.Length);
    this.preferredSourceReadSize -= this.preferredSourceReadSize % sourceProvider.WaveFormat.BlockAlign;
  }

  public WaveFormat WaveFormat { get; }

  public void Reposition()
  {
    this.leftoverDestBytes = 0;
    this.leftoverDestOffset = 0;
    this.leftoverSourceBytes = 0;
    this.conversionStream.Reposition();
  }

  public int Read(byte[] buffer, int offset, int count)
  {
    int num = 0;
    if (count % this.WaveFormat.BlockAlign != 0)
      count -= count % this.WaveFormat.BlockAlign;
    int length1;
    for (; num < count; num += length1)
    {
      int length2 = Math.Min(count - num, this.leftoverDestBytes);
      if (length2 > 0)
      {
        Array.Copy((Array) this.conversionStream.DestBuffer, this.leftoverDestOffset, (Array) buffer, offset + num, length2);
        this.leftoverDestOffset += length2;
        this.leftoverDestBytes -= length2;
        num += length2;
      }
      if (num < count)
      {
        int bytesToConvert = this.sourceProvider.Read(this.conversionStream.SourceBuffer, this.leftoverSourceBytes, Math.Min(this.preferredSourceReadSize, this.conversionStream.SourceBuffer.Length - this.leftoverSourceBytes)) + this.leftoverSourceBytes;
        if (bytesToConvert != 0)
        {
          int sourceBytesConverted;
          int val1 = this.conversionStream.Convert(bytesToConvert, out sourceBytesConverted);
          if (sourceBytesConverted != 0)
          {
            this.leftoverSourceBytes = bytesToConvert - sourceBytesConverted;
            if (this.leftoverSourceBytes > 0)
              Buffer.BlockCopy((Array) this.conversionStream.SourceBuffer, sourceBytesConverted, (Array) this.conversionStream.SourceBuffer, 0, this.leftoverSourceBytes);
            if (val1 > 0)
            {
              int val2 = count - num;
              length1 = Math.Min(val1, val2);
              if (length1 < val1)
              {
                this.leftoverDestBytes = val1 - length1;
                this.leftoverDestOffset = length1;
              }
              Array.Copy((Array) this.conversionStream.DestBuffer, 0, (Array) buffer, num + offset, length1);
            }
            else
              break;
          }
          else
            break;
        }
        else
          break;
      }
      else
        break;
    }
    return num;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.isDisposed)
      return;
    this.isDisposed = true;
    this.conversionStream?.Dispose();
  }

  public void Dispose()
  {
    GC.SuppressFinalize((object) this);
    this.Dispose(true);
  }

  ~WaveFormatConversionProvider() => this.Dispose(false);
}
