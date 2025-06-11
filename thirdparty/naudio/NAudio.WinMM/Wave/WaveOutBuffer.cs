// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveOutBuffer
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

public class WaveOutBuffer : IDisposable
{
  private readonly WaveHeader header;
  private readonly int bufferSize;
  private readonly byte[] buffer;
  private readonly IWaveProvider waveStream;
  private readonly object waveOutLock;
  private GCHandle hBuffer;
  private IntPtr hWaveOut;
  private GCHandle hHeader;
  private GCHandle hThis;

  public WaveOutBuffer(
    IntPtr hWaveOut,
    int bufferSize,
    IWaveProvider bufferFillStream,
    object waveOutLock)
  {
    this.bufferSize = bufferSize;
    this.buffer = new byte[bufferSize];
    this.hBuffer = GCHandle.Alloc((object) this.buffer, GCHandleType.Pinned);
    this.hWaveOut = hWaveOut;
    this.waveStream = bufferFillStream;
    this.waveOutLock = waveOutLock;
    this.header = new WaveHeader();
    this.hHeader = GCHandle.Alloc((object) this.header, GCHandleType.Pinned);
    this.header.dataBuffer = this.hBuffer.AddrOfPinnedObject();
    this.header.bufferLength = bufferSize;
    this.header.loops = 1;
    this.hThis = GCHandle.Alloc((object) this);
    this.header.userData = (IntPtr) this.hThis;
    lock (waveOutLock)
      MmException.Try(WaveInterop.waveOutPrepareHeader(hWaveOut, this.header, Marshal.SizeOf<WaveHeader>(this.header)), "waveOutPrepareHeader");
  }

  ~WaveOutBuffer() => this.Dispose(false);

  public void Dispose()
  {
    GC.SuppressFinalize((object) this);
    this.Dispose(true);
  }

  protected void Dispose(bool disposing)
  {
    int num1 = disposing ? 1 : 0;
    if (this.hHeader.IsAllocated)
      this.hHeader.Free();
    if (this.hBuffer.IsAllocated)
      this.hBuffer.Free();
    if (this.hThis.IsAllocated)
      this.hThis.Free();
    if (!(this.hWaveOut != IntPtr.Zero))
      return;
    lock (this.waveOutLock)
    {
      int num2 = (int) WaveInterop.waveOutUnprepareHeader(this.hWaveOut, this.header, Marshal.SizeOf<WaveHeader>(this.header));
    }
    this.hWaveOut = IntPtr.Zero;
  }

  public bool OnDone()
  {
    int num;
    lock (this.waveStream)
      num = this.waveStream.Read(this.buffer, 0, this.buffer.Length);
    if (num == 0)
      return false;
    for (int index = num; index < this.buffer.Length; ++index)
      this.buffer[index] = (byte) 0;
    this.WriteToWaveOut();
    return true;
  }

  public bool InQueue => (this.header.flags & WaveHeaderFlags.InQueue) == WaveHeaderFlags.InQueue;

  public int BufferSize => this.bufferSize;

  private void WriteToWaveOut()
  {
    MmResult result;
    lock (this.waveOutLock)
      result = WaveInterop.waveOutWrite(this.hWaveOut, this.header, Marshal.SizeOf<WaveHeader>(this.header));
    if (result != MmResult.NoError)
      throw new MmException(result, "waveOutWrite");
    GC.KeepAlive((object) this);
  }
}
