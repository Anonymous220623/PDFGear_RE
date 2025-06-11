// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IWiaDataCallback
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

[Guid("a558a866-a5b0-11d2-a08f-00c04f72dc3c")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IWiaDataCallback
{
  [MethodImpl(MethodImplOptions.PreserveSig)]
  int BandedDataCallback(
    int lMessage,
    int lStatus,
    int lPercentComplete,
    int lOffset,
    int lLength,
    int lReserved,
    int lResLength,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] pbBuffer);
}
