// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.TransferStatusCallback
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

internal delegate bool TransferStatusCallback(
  int msgType,
  int percent,
  ulong bytesTransferred,
  uint hresult,
  IStream? stream);
