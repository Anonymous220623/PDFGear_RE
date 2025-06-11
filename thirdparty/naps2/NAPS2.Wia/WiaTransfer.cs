// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaTransfer
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia;

public class WiaTransfer : NativeWiaObject
{
  private const int MSG_STATUS = 1;
  private const int MSG_END_STREAM = 2;
  private const int MSG_END_TRANSFER = 3;
  private bool _cancel;

  protected internal WiaTransfer(WiaVersion version, IntPtr handle)
    : base(version, handle)
  {
  }

  public event EventHandler<WiaTransfer.ProgressEventArgs>? Progress;

  public event EventHandler<WiaTransfer.PageScannedEventArgs>? PageScanned;

  public event EventHandler? TransferComplete;

  public bool Download()
  {
    uint hresult = this.Version == WiaVersion.Wia10 ? NativeWiaMethods.Download1(this.Handle, new NAPS2.Wia.Native.TransferStatusCallback(this.TransferStatusCallback)) : NativeWiaMethods.Download2(this.Handle, new NAPS2.Wia.Native.TransferStatusCallback(this.TransferStatusCallback));
    if (hresult == 1U)
      return false;
    WiaException.Check(hresult);
    return true;
  }

  public void Cancel() => this._cancel = true;

  private bool TransferStatusCallback(
    int msgType,
    int percent,
    ulong bytesTransferred,
    uint hresult,
    IStream? stream)
  {
    switch (msgType)
    {
      case 1:
        EventHandler<WiaTransfer.ProgressEventArgs> progress = this.Progress;
        if (progress != null)
        {
          progress((object) this, new WiaTransfer.ProgressEventArgs(percent));
          break;
        }
        break;
      case 2:
        NativeStreamWrapper nativeStreamWrapper = new NativeStreamWrapper(stream);
        if (this._cancel)
        {
          nativeStreamWrapper.Dispose();
          break;
        }
        EventHandler<WiaTransfer.PageScannedEventArgs> pageScanned = this.PageScanned;
        if (pageScanned != null)
        {
          pageScanned((object) this, new WiaTransfer.PageScannedEventArgs((Stream) nativeStreamWrapper));
          break;
        }
        break;
      case 3:
        EventHandler transferComplete = this.TransferComplete;
        if (transferComplete != null)
        {
          transferComplete((object) this, EventArgs.Empty);
          break;
        }
        break;
    }
    return !this._cancel;
  }

  public class ProgressEventArgs : EventArgs
  {
    public ProgressEventArgs(int percent) => this.Percent = percent;

    public int Percent { get; }
  }

  public class PageScannedEventArgs : EventArgs
  {
    public PageScannedEventArgs(Stream stream) => this.Stream = stream;

    public Stream Stream { get; }
  }

  public class TransferErrorEventArgs : EventArgs
  {
    public TransferErrorEventArgs(uint errorCode) => this.ErrorCode = errorCode;

    public uint ErrorCode { get; }
  }
}
