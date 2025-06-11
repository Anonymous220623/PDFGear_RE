// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmStream
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

public class AcmStream : IDisposable
{
  private IntPtr streamHandle;
  private IntPtr driverHandle;
  private AcmStreamHeader streamHeader;
  private readonly WaveFormat sourceFormat;

  public AcmStream(WaveFormat sourceFormat, WaveFormat destFormat)
  {
    try
    {
      this.streamHandle = IntPtr.Zero;
      this.sourceFormat = sourceFormat;
      int num1 = Math.Max(65536 /*0x010000*/, sourceFormat.AverageBytesPerSecond);
      int num2 = num1 - num1 % sourceFormat.BlockAlign;
      IntPtr ptr1 = WaveFormat.MarshalToPtr(sourceFormat);
      IntPtr ptr2 = WaveFormat.MarshalToPtr(destFormat);
      try
      {
        MmException.Try(AcmInterop.acmStreamOpen2(out this.streamHandle, IntPtr.Zero, ptr1, ptr2, (WaveFilter) null, IntPtr.Zero, IntPtr.Zero, AcmStreamOpenFlags.NonRealTime), "acmStreamOpen");
      }
      finally
      {
        Marshal.FreeHGlobal(ptr1);
        Marshal.FreeHGlobal(ptr2);
      }
      int dest = this.SourceToDest(num2);
      this.streamHeader = new AcmStreamHeader(this.streamHandle, num2, dest);
      this.driverHandle = IntPtr.Zero;
    }
    catch
    {
      this.Dispose();
      throw;
    }
  }

  public AcmStream(IntPtr driverId, WaveFormat sourceFormat, WaveFilter waveFilter)
  {
    int num1 = Math.Max(16384 /*0x4000*/, sourceFormat.AverageBytesPerSecond);
    this.sourceFormat = sourceFormat;
    int num2 = num1 - num1 % sourceFormat.BlockAlign;
    MmException.Try(AcmInterop.acmDriverOpen(out this.driverHandle, driverId, 0), "acmDriverOpen");
    IntPtr ptr = WaveFormat.MarshalToPtr(sourceFormat);
    try
    {
      MmException.Try(AcmInterop.acmStreamOpen2(out this.streamHandle, this.driverHandle, ptr, ptr, waveFilter, IntPtr.Zero, IntPtr.Zero, AcmStreamOpenFlags.NonRealTime), "acmStreamOpen");
    }
    finally
    {
      Marshal.FreeHGlobal(ptr);
    }
    this.streamHeader = new AcmStreamHeader(this.streamHandle, num2, this.SourceToDest(num2));
  }

  public int SourceToDest(int source)
  {
    if (source == 0)
      return 0;
    int outputBufferSize;
    MmException.Try(AcmInterop.acmStreamSize(this.streamHandle, source, out outputBufferSize, AcmStreamSizeFlags.Source), "acmStreamSize");
    return outputBufferSize;
  }

  public int DestToSource(int dest)
  {
    if (dest == 0)
      return 0;
    int outputBufferSize;
    MmException.Try(AcmInterop.acmStreamSize(this.streamHandle, dest, out outputBufferSize, AcmStreamSizeFlags.Destination), "acmStreamSize");
    return outputBufferSize;
  }

  public static WaveFormat SuggestPcmFormat(WaveFormat compressedFormat)
  {
    WaveFormat waveFormat = new WaveFormat(compressedFormat.SampleRate, 16 /*0x10*/, compressedFormat.Channels);
    IntPtr ptr1 = WaveFormat.MarshalToPtr(waveFormat);
    IntPtr ptr2 = WaveFormat.MarshalToPtr(compressedFormat);
    try
    {
      int result = (int) AcmInterop.acmFormatSuggest2(IntPtr.Zero, ptr2, ptr1, Marshal.SizeOf<WaveFormat>(waveFormat), AcmFormatSuggestFlags.FormatTag);
      waveFormat = WaveFormat.MarshalFromPtr(ptr1);
      MmException.Try((MmResult) result, "acmFormatSuggest");
    }
    finally
    {
      Marshal.FreeHGlobal(ptr1);
      Marshal.FreeHGlobal(ptr2);
    }
    return waveFormat;
  }

  public byte[] SourceBuffer => this.streamHeader.SourceBuffer;

  public byte[] DestBuffer => this.streamHeader.DestBuffer;

  public void Reposition() => this.streamHeader.Reposition();

  public int Convert(int bytesToConvert, out int sourceBytesConverted)
  {
    if (bytesToConvert % this.sourceFormat.BlockAlign != 0)
      bytesToConvert -= bytesToConvert % this.sourceFormat.BlockAlign;
    return this.streamHeader.Convert(bytesToConvert, out sourceBytesConverted);
  }

  [Obsolete("Call the version returning sourceBytesConverted instead")]
  public int Convert(int bytesToConvert)
  {
    int sourceBytesConverted;
    int num = this.Convert(bytesToConvert, out sourceBytesConverted);
    if (sourceBytesConverted == bytesToConvert)
      return num;
    throw new MmException(MmResult.NotSupported, "AcmStreamHeader.Convert didn't convert everything");
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (disposing && this.streamHeader != null)
    {
      this.streamHeader.Dispose();
      this.streamHeader = (AcmStreamHeader) null;
    }
    if (this.streamHandle != IntPtr.Zero)
    {
      MmResult result = AcmInterop.acmStreamClose(this.streamHandle, 0);
      this.streamHandle = IntPtr.Zero;
      if (result != MmResult.NoError)
        throw new MmException(result, "acmStreamClose");
    }
    if (!(this.driverHandle != IntPtr.Zero))
      return;
    int num = (int) AcmInterop.acmDriverClose(this.driverHandle, 0);
    this.driverHandle = IntPtr.Zero;
  }

  ~AcmStream() => this.Dispose(false);
}
