// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaItem
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;

#nullable enable
namespace NAPS2.Wia;

public class WiaItem : WiaItemBase
{
  protected internal WiaItem(WiaVersion version, IntPtr handle)
    : base(version, handle)
  {
  }

  public WiaTransfer StartTransfer()
  {
    IntPtr transfer;
    WiaException.Check(this.Version == WiaVersion.Wia10 ? NativeWiaMethods.StartTransfer1(this.Handle, out transfer) : NativeWiaMethods.StartTransfer2(this.Handle, out transfer));
    return new WiaTransfer(this.Version, transfer);
  }
}
