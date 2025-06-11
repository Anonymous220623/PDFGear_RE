// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaTransfer
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("c39d6942-2f4e-4d04-92fe-4ef4d3a1de5a")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaTransfer
{
  void Download(int lFlags, IWiaTransferCallback pIWiaTransferCallback);

  void Upload(int lFlags, IStream pSource, IWiaTransferCallback pIWiaTransferCallback);

  void Cancel();

  IEnumWIA_FORMAT_INFO EnumWIA_FORMAT_INFO();
}
