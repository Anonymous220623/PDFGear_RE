// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.WiaTransferCallback1
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

internal class WiaTransferCallback1 : IWiaDataCallback
{
  private const int IT_MSG_DATA_HEADER = 1;
  private const int IT_MSG_DATA = 2;
  private const int IT_MSG_STATUS = 3;
  private const int IT_MSG_TERMINATION = 4;
  private const int IT_MSG_NEW_PAGE = 5;
  private const int IT_MSG_FILE_PREVIEW_DATA = 6;
  private const int IT_MSG_FILE_PREVIEW_DATA_HEADER = 7;
  private const int STREAM_SEEK_SET = 0;
  private const int STREAM_SEEK_CUR = 1;
  private const int STREAM_SEEK_END = 2;
  private IStream? _stream;
  private readonly TransferStatusCallback _statusCallback;

  internal WiaTransferCallback1(TransferStatusCallback statusCallback)
  {
    this._statusCallback = statusCallback;
  }

  [MethodImpl(MethodImplOptions.PreserveSig)]
  public int BandedDataCallback(
    int lMessage,
    int lStatus,
    int lPercentComplete,
    int lOffset,
    int lLength,
    int lReserved,
    int lResLength,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] pbBuffer)
  {
    uint num1 = 0;
    IStream stream = (IStream) null;
    switch (lMessage)
    {
      case 2:
        if (this._stream == null)
        {
          this._stream = Win32.SHCreateMemStream((byte[]) null, 0U);
          byte[] numArray = new byte[14];
          numArray[0] = (byte) 66;
          numArray[1] = (byte) 77;
          byte[] pv = numArray;
          this._stream.Write(pv, pv.Length, IntPtr.Zero);
        }
        this._stream.Write(pbBuffer, lLength, IntPtr.Zero);
        break;
      case 4:
        stream = this._stream;
        break;
      case 5:
        stream = this._stream;
        this._stream = (IStream) null;
        break;
    }
    if (stream != null)
    {
      stream.Seek(14L, 0, IntPtr.Zero);
      byte[] pv = new byte[4];
      stream.Read(pv, 4, IntPtr.Zero);
      int num2 = BitConverter.ToInt32(pv, 0) + 14;
      stream.Seek(2L, 0, IntPtr.Zero);
      System.Runtime.InteropServices.ComTypes.STATSTG pstatstg;
      stream.Stat(out pstatstg, 1);
      byte[] bytes1 = BitConverter.GetBytes(pstatstg.cbSize);
      stream.Write(bytes1, 4, IntPtr.Zero);
      stream.Seek(10L, 0, IntPtr.Zero);
      byte[] bytes2 = BitConverter.GetBytes(num2);
      stream.Write(bytes2, 4, IntPtr.Zero);
      stream.Seek(0L, 0, IntPtr.Zero);
      int num3 = this._statusCallback(2, lPercentComplete, (ulong) (lOffset + lLength), (uint) lStatus, stream) ? 1 : 0;
    }
    if (!this._statusCallback(lMessage == 4 ? 3 : 1, lPercentComplete, (ulong) (lOffset + lLength), (uint) lStatus, (IStream) null))
      num1 = 1U;
    return (int) num1;
  }
}
