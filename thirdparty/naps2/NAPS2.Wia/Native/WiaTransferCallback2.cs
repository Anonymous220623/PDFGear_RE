// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.WiaTransferCallback2
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

internal class WiaTransferCallback2 : IWiaTransferCallback
{
  private const int WIA_TRANSFER_MSG_STATUS = 1;
  private const int WIA_TRANSFER_MSG_END_OF_STREAM = 2;
  private const int WIA_TRANSFER_MSG_END_OF_TRANSFER = 3;
  private const int WIA_TRANSFER_MSG_DEVICE_STATUS = 5;
  private const int WIA_TRANSFER_MSG_NEW_PAGE = 6;
  private readonly Queue<IStream> _streams = new Queue<IStream>();
  private readonly TransferStatusCallback _statusCallback;

  internal WiaTransferCallback2(TransferStatusCallback statusCallback)
  {
    this._statusCallback = statusCallback;
  }

  public int TransferCallback(int lFlags, in WiaTransferParams pWiaTransferParams)
  {
    int num = 0;
    IStream stream = pWiaTransferParams.lMessage == 2 ? this._streams.Dequeue() : (IStream) null;
    if (!this._statusCallback(pWiaTransferParams.lMessage, pWiaTransferParams.lPercentComplete, pWiaTransferParams.ulTransferredBytes, (uint) pWiaTransferParams.hrErrorStatus, stream))
      num = 1;
    if (stream != null)
      Marshal.ReleaseComObject((object) stream);
    return num;
  }

  public void GetNextStream(
    int lFlags,
    string bstrItemName,
    string bstrFullItemName,
    out IStream ppDestination)
  {
    IStream memStream = Win32.SHCreateMemStream((byte[]) null, 0U);
    ppDestination = memStream;
    this._streams.Enqueue(memStream);
  }

  int IWiaTransferCallback.TransferCallback(int lFlags, in WiaTransferParams pWiaTransferParams)
  {
    return this.TransferCallback(lFlags, in pWiaTransferParams);
  }
}
