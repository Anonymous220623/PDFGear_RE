// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaPreview
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("95C2B4FD-33F2-4d86-AD40-9431F0DF08F7")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaPreview
{
  void GetNewPreview(int lFlags, IWiaItem2 pWiaItem2, IWiaTransferCallback pWiaTransferCallback);

  void UpdatePreview(
    int lFlags,
    IWiaItem2 pChildWiaItem2,
    IWiaTransferCallback pWiaTransferCallback);

  void DetectRegions(int lFlags);

  void Clear();
}
