// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaDataTransfer
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("a6cef998-a5b0-11d2-a08f-00c04f72dc3c")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaDataTransfer
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int idtGetData(ref STGMEDIUM pMedium, IWiaDataCallback pIWiaDataCallback);

  [MethodImpl(MethodImplOptions.PreserveSig)]
  int idtGetBandedData(
    in WIA_DATA_TRANSFER_INFO pWiaDataTransInfo,
    IWiaDataCallback pIWiaDataCallback);

  void idtQueryGetData(in WIA_FORMAT_INFO pfe);

  IEnumWIA_FORMAT_INFO idtEnumWIA_FORMAT_INFO();

  void idtGetExtendedTransferInfo(
    out WIA_EXTENDED_TRANSFER_INFO pExtendedTransferInfo);
}
