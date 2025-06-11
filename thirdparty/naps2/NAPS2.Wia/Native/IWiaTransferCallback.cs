// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaTransferCallback
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("27d4eaaf-28a6-4ca5-9aab-e678168b9527")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaTransferCallback
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int TransferCallback(int lFlags, in WiaTransferParams pWiaTransferParams);

  void GetNextStream(
    int lFlags,
    string bstrItemName,
    string bstrFullItemName,
    out IStream ppDestination);
}
