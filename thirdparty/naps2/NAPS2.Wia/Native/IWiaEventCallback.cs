// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaEventCallback
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("ae6287b0-0084-11d2-973b-00a0c9068f2e")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaEventCallback
{
  void ImageEventCallback(
    in Guid pEventGUID,
    string bstrEventDescription,
    string bstrDeviceID,
    string bstrDeviceDescription,
    uint dwDeviceType,
    string bstrFullItemName,
    ref uint pulEventType,
    uint ulReserved);
}
