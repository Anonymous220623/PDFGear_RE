// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.WiaTransferParams
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

#nullable disable
namespace NAPS2.Wia.Native;

internal struct WiaTransferParams
{
  public int lMessage;
  public int lPercentComplete;
  public ulong ulTransferredBytes;
  public int hrErrorStatus;
}
