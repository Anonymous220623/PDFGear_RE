// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveInBuffer
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

public class WaveInBuffer : IDisposable
{
  private readonly WaveHeader header;
  private readonly int bufferSize;
  private readonly byte[] buffer;
  private GCHandle hBuffer;
  private IntPtr waveInHandle;
  private GCHandle hHeader;
  private GCHandle hThis;

  public WaveInBuffer(IntPtr waveInHandle, int bufferSize)
  {
    this.bufferSize = bufferSize;
    this.buffer = new byte[bufferSize];
    this.hBuffer = GCHandle.Alloc((object) this.buffer, GCHandleType.Pinned);
    this.waveInHandle = waveInHandle;
    this.header = new WaveHeader();
    this.hHeader = GCHandle.Alloc((object) this.header, GCHandleType.Pinned);
    this.header.dataBuffer = this.hBuffer.AddrOfPinnedObject();
    this.header.bufferLength = bufferSize;
    this.header.loops = 1;
    this.hThis = GCHandle.Alloc((object) this);
    this.header.userData = (IntPtr) this.hThis;
    MmException.Try(WaveInterop.waveInPrepareHeader(waveInHandle, this.header, Marshal.SizeOf<WaveHeader>(this.header)), "waveInPrepareHeader");
  }

  public void Reuse()
  {
    MmException.Try(WaveInterop.waveInUnprepareHeader(this.waveInHandle, this.header, Marshal.SizeOf<WaveHeader>(this.header)), "waveUnprepareHeader");
    MmException.Try(WaveInterop.waveInPrepareHeader(this.waveInHandle, this.header, Marshal.SizeOf<WaveHeader>(this.header)), "waveInPrepareHeader");
    MmException.Try(WaveInterop.waveInAddBuffer(this.waveInHandle, this.header, Marshal.SizeOf<WaveHeader>(this.header)), "waveInAddBuffer");
  }

  ~WaveInBuffer() => this.Dispose(false);

  public void Dispose()
  {
    GC.SuppressFinalize((object) this);
    this.Dispose(true);
  }

  protected void Dispose(bool disposing)
  {
    int num1 = disposing ? 1 : 0;
    if (this.waveInHandle != IntPtr.Zero)
    {
      int num2 = (int) WaveInterop.waveInUnprepareHeader(this.waveInHandle, this.header, Marshal.SizeOf<WaveHeader>(this.header));
      this.waveInHandle = IntPtr.Zero;
    }
    if (this.hHeader.IsAllocated)
      this.hHeader.Free();
    if (this.hBuffer.IsAllocated)
      this.hBuffer.Free();
    if (!this.hThis.IsAllocated)
      return;
    this.hThis.Free();
  }

  public byte[] Data => this.buffer;

  public bool Done => (this.header.flags & WaveHeaderFlags.Done) == WaveHeaderFlags.Done;

  public bool InQueue => (this.header.flags & WaveHeaderFlags.InQueue) == WaveHeaderFlags.InQueue;

  public int BytesRecorded => this.header.bytesRecorded;

  public int BufferSize => this.bufferSize;
}
