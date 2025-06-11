// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaDeviceInfo
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;

#nullable enable
namespace NAPS2.Wia;

public class WiaDeviceInfo : NativeWiaObject, IWiaDeviceProps
{
  protected internal WiaDeviceInfo(WiaVersion version, IntPtr propStorageHandle)
    : base(version)
  {
    this.Properties = new WiaPropertyCollection(version, propStorageHandle);
  }

  public WiaPropertyCollection Properties { get; }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (!disposing)
      return;
    this.Properties?.Dispose();
  }
}
