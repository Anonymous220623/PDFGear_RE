// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmStreamHeader
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

internal class AcmStreamHeader : IDisposable
{
  private AcmStreamHeaderStruct streamHeader;
  private GCHandle hSourceBuffer;
  private GCHandle hDestBuffer;
  private IntPtr streamHandle;
  private bool firstTime;
  private bool disposed;

  public AcmStreamHeader(IntPtr streamHandle, int sourceBufferLength, int destBufferLength)
  {
    this.streamHeader = new AcmStreamHeaderStruct();
    this.SourceBuffer = new byte[sourceBufferLength];
    this.hSourceBuffer = GCHandle.Alloc((object) this.SourceBuffer, GCHandleType.Pinned);
    this.DestBuffer = new byte[destBufferLength];
    this.hDestBuffer = GCHandle.Alloc((object) this.DestBuffer, GCHandleType.Pinned);
    this.streamHandle = streamHandle;
    this.firstTime = true;
  }

  private void Prepare()
  {
    this.streamHeader.cbStruct = Marshal.SizeOf<AcmStreamHeaderStruct>(this.streamHeader);
    this.streamHeader.sourceBufferLength = this.SourceBuffer.Length;
    this.streamHeader.sourceBufferPointer = this.hSourceBuffer.AddrOfPinnedObject();
    this.streamHeader.destBufferLength = this.DestBuffer.Length;
    this.streamHeader.destBufferPointer = this.hDestBuffer.AddrOfPinnedObject();
    MmException.Try(AcmInterop.acmStreamPrepareHeader(this.streamHandle, this.streamHeader, 0), "acmStreamPrepareHeader");
  }

  private void Unprepare()
  {
    this.streamHeader.sourceBufferLength = this.SourceBuffer.Length;
    this.streamHeader.sourceBufferPointer = this.hSourceBuffer.AddrOfPinnedObject();
    this.streamHeader.destBufferLength = this.DestBuffer.Length;
    this.streamHeader.destBufferPointer = this.hDestBuffer.AddrOfPinnedObject();
    MmResult result = AcmInterop.acmStreamUnprepareHeader(this.streamHandle, this.streamHeader, 0);
    if (result != MmResult.NoError)
      throw new MmException(result, "acmStreamUnprepareHeader");
  }

  public void Reposition() => this.firstTime = true;

  public int Convert(int bytesToConvert, out int sourceBytesConverted)
  {
    this.Prepare();
    try
    {
      this.streamHeader.sourceBufferLength = bytesToConvert;
      this.streamHeader.sourceBufferLengthUsed = bytesToConvert;
      MmException.Try(AcmInterop.acmStreamConvert(this.streamHandle, this.streamHeader, this.firstTime ? AcmStreamConvertFlags.BlockAlign | AcmStreamConvertFlags.Start : AcmStreamConvertFlags.BlockAlign), "acmStreamConvert");
      this.firstTime = false;
      sourceBytesConverted = this.streamHeader.sourceBufferLengthUsed;
    }
    finally
    {
      this.Unprepare();
    }
    return this.streamHeader.destBufferLengthUsed;
  }

  public byte[] SourceBuffer { get; private set; }

  public byte[] DestBuffer { get; private set; }

  public void Dispose()
  {
    GC.SuppressFinalize((object) this);
    this.Dispose(true);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!this.disposed)
    {
      this.SourceBuffer = (byte[]) null;
      this.DestBuffer = (byte[]) null;
      this.hSourceBuffer.Free();
      this.hDestBuffer.Free();
    }
    this.disposed = true;
  }

  ~AcmStreamHeader() => this.Dispose(false);
}
