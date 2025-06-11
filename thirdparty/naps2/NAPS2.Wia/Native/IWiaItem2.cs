// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaItem2
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("6CBA0075-1287-407d-9B77-CF0E030435CC")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaItem2
{
  IWiaItem2 CreateChildItem(int lItemFlags, int lCreationFlags, string bstrItemName);

  void DeleteItem(int lFlags);

  IEnumWiaItem2 EnumChildItems(IntPtr pCategoryGUID);

  IWiaItem2 FindItemByName(int lFlags, string bstrFullItemName);

  void GetItemCategory(out Guid pItemCategoryGUID);

  int GetItemType();

  void DeviceDlg(
    int lFlags,
    IntPtr hwndParent,
    string bstrFolderName,
    string bstrFilename,
    out int plNumFiles,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4, ArraySubType = UnmanagedType.LPWStr), Out] string[] ppbstrFilePaths,
    ref IWiaItem2 ppItem);

  void DeviceCommand(int lFlags, in Guid pCmdGUID, out IWiaItem2 ppIWiaItem2);

  IEnumWIA_DEV_CAPS EnumDeviceCapabilities(int lFlags);

  bool CheckExtension(int lFlags, string bstrName, in Guid riidExtensionInterface);

  void GetExtension(int lFlags, string bstrName, in Guid riidExtensionInterface, [MarshalAs(UnmanagedType.IUnknown)] out object ppOut);

  IWiaItem2 GetParentItem();

  IWiaItem2 GetRootItem();

  IWiaPreview GetPreviewComponent(int lFlags);

  IEnumWIA_DEV_CAPS EnumRegisterEventInfo(int lFlags, in Guid pEventGUID);

  void Diagnostic(uint ulSize, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer);
}
