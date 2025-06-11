// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.WIA_DEV_CAP
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable enable
namespace NAPS2.Wia.Native;

internal struct WIA_DEV_CAP
{
  public Guid guid;
  public uint ulFlags;
  [MarshalAs(UnmanagedType.BStr)]
  public string bstrName;
  [MarshalAs(UnmanagedType.BStr)]
  public string bstrDescription;
  [MarshalAs(UnmanagedType.BStr)]
  public string bstrIcon;
  [MarshalAs(UnmanagedType.BStr)]
  public string bstrCommandline;
}
