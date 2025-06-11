// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.IStreamPointerRead
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAPS2.Wia.Native;

[Guid("0000000c-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IStreamPointerRead
{
  unsafe void Read(byte* pv, int cb, IntPtr pcbRead);
}
