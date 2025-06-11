// Decompiled with JetBrains decompiler
// Type: Standard.NIF
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Standard;

[Flags]
internal enum NIF : uint
{
  MESSAGE = 1,
  ICON = 2,
  TIP = 4,
  STATE = 8,
  INFO = 16, // 0x00000010
  GUID = 32, // 0x00000020
  REALTIME = 64, // 0x00000040
  SHOWTIP = 128, // 0x00000080
  XP_MASK = GUID | INFO | STATE | ICON | MESSAGE, // 0x0000003B
  VISTA_MASK = XP_MASK | SHOWTIP | REALTIME, // 0x000000FB
}
