// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaItem
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("4db1ad10-3391-11d2-9a33-00c04fa36145")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaItem
{
  int GetItemType();

  void AnalyzeItem(int lFlags);

  IEnumWiaItem EnumChildItems();

  void DeleteItem(int lFlags);

  IWiaItem CreateChildItem(int lFlags, string bstrItemName, string bstrFullItemName);

  IEnumWIA_DEV_CAPS EnumRegisterEventInfo(int lFlags, in Guid pEventGUID);

  IWiaItem FindItemByName(int lFlags, string bstrFullItemName);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  int DeviceDlg(
    IntPtr hwndParent,
    int lFlags,
    int lIntent,
    out int plItemCount,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] out IWiaItem[]? ppIWiaItem);

  IWiaItem DeviceCommand(int lFlags, in Guid pCmdGUID);

  IWiaItem GetRootItem();

  IEnumWIA_DEV_CAPS EnumDeviceCapabilities(int lFlags);

  string DumpItemData();

  string DumpDrvItemData();

  string DumpTreeItemData();

  void Diagnostic(uint ulSize, [MarshalAs(UnmanagedType.LPArray), Out] byte[] pBuffer);
}
