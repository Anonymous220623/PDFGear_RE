// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaDevice
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;

#nullable enable
namespace NAPS2.Wia;

public class WiaDevice : WiaItemBase, IWiaDeviceProps
{
  protected internal WiaDevice(WiaVersion version, IntPtr handle)
    : base(version, handle)
  {
  }

  public WiaItem? PromptToConfigure(IntPtr parentWindowHandle = default (IntPtr))
  {
    if (this.Version == WiaVersion.Wia20)
      throw new InvalidOperationException("WIA 2.0 does not support PromptToConfigure. Use WiaDeviceManager.PromptForImage if you want to use the native WIA 2.0 UI.");
    IntPtr[] itemPtrs;
    uint hresult = NativeWiaMethods.ConfigureDevice1(this.Handle, parentWindowHandle, 0, 0, out int _, out itemPtrs);
    if (hresult == 1U)
      return (WiaItem) null;
    WiaException.Check(hresult);
    return itemPtrs == null ? (WiaItem) null : new WiaItem(this.Version, itemPtrs[0]);
  }
}
