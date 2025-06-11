// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaDevMgr2
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("79C07CF1-CBDD-41ee-8EC3-F00080CADA7A")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaDevMgr2
{
  IEnumWIA_DEV_INFO EnumDeviceInfo(int lFlags);

  IWiaItem2 CreateDevice(int lFlags, string bstrDeviceID);

  IWiaItem2 SelectDeviceDlg(
    IntPtr hwndParent,
    int lDeviceType,
    int lFlags,
    ref string pbstrDeviceID);

  string SelectDeviceDlgID(IntPtr hwndParent, int lDeviceType, int lFlags);

  void RegisterEventCallbackInterface(
    int lFlags,
    string bstrDeviceID,
    in Guid pEventGUID,
    IWiaEventCallback pIWiaEventCallback,
    [MarshalAs(UnmanagedType.IUnknown)] out object pEventObject);

  void RegisterEventCallbackProgram(
    int lFlags,
    string bstrDeviceID,
    in Guid pEventGUID,
    string bstrFullAppName,
    string bstrCommandLineArg,
    string bstrName,
    string bstrDescription,
    string bstrIcon);

  void RegisterEventCallbackCLSID(
    int lFlags,
    string bstrDeviceID,
    in Guid pEventGUID,
    in Guid pClsID,
    string bstrName,
    string bstrDescription,
    string bstrIcon);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  int GetImageDlg(
    int lFlags,
    string bstrDeviceID,
    IntPtr hwndParent,
    string bstrFolderName,
    string bstrFilename,
    ref int plNumFiles,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5, ArraySubType = UnmanagedType.BStr), In, Out] ref string[] ppbstrFilePaths,
    ref IWiaItem2? ppItem);
}
