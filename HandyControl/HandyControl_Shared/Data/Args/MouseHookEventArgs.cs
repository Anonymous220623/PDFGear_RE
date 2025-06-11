// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.MouseHookEventArgs
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;

#nullable disable
namespace HandyControl.Data;

internal class MouseHookEventArgs : EventArgs
{
  public MouseHookMessageType MessageType { get; set; }

  public InteropValues.POINT Point { get; set; }
}
