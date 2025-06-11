// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.WIA_DATA_TRANSFER_INFO
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.InteropServices;

#nullable disable
namespace NAPS2.Wia.Native;

internal struct WIA_DATA_TRANSFER_INFO
{
  public uint ulSize;
  public uint ulSection;
  public uint ulBufferSize;
  [MarshalAs(UnmanagedType.Bool)]
  public bool bDoubleBuffer;
  public uint ulReserved1;
  public uint ulReserved2;
  public uint ulReserved3;
}
