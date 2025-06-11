// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaDevMgr
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("5eb2502a-8cf1-11d1-bf92-0060081ed811")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaDevMgr
{
  IEnumWIA_DEV_INFO EnumDeviceInfo(int lFlag);

  IWiaItem CreateDevice(string bstrDeviceID);

  IWiaItem SelectDeviceDlg(
    IntPtr hwndParent,
    int lDeviceType,
    int lFlags,
    ref string pbstrDeviceID);

  string SelectDeviceDlgID(IntPtr hwndParent, int lDeviceType, int lFlags);

  void GetImageDlg(
    IntPtr hwndParent,
    int lDeviceType,
    int lFlags,
    int lIntent,
    IWiaItem? pItemRoot,
    string bstrFilename,
    ref Guid pguidFormat);

  void RegisterEventCallbackProgram(
    int lFlags,
    string bstrDeviceID,
    in Guid pEventGUID,
    string bstrCommandline,
    string bstrName,
    string bstrDescription,
    string bstrIcon);

  void RegisterEventCallbackInterface(
    int lFlags,
    string bstrDeviceID,
    in Guid pEventGUID,
    IWiaEventCallback pIWiaEventCallback,
    [MarshalAs(UnmanagedType.IUnknown)] out object pEventObject);

  void RegisterEventCallbackCLSID(
    int lFlags,
    string bstrDeviceID,
    in Guid pEventGUID,
    in Guid pClsID,
    string bstrName,
    string bstrDescription,
    string bstrIcon);

  void AddDeviceDlg(IntPtr hwndParent, int lFlags);
}
