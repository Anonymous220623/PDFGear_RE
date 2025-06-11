// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.HitTestResults
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Shared;

internal enum HitTestResults
{
  Error = -2, // 0xFFFFFFFE
  Transparent = -1, // 0xFFFFFFFF
  Nowhere = 0,
  Client = 1,
  Caption = 2,
  Sysmenu = 3,
  GrowBox = 4,
  Menu = 5,
  HScroll = 6,
  VScroll = 7,
  ButtonMin = 8,
  ButtonMax = 9,
  BorderLeft = 10, // 0x0000000A
  BorderRight = 11, // 0x0000000B
  BorderTop = 12, // 0x0000000C
  CornerLeftTop = 13, // 0x0000000D
  CornerTopRight = 14, // 0x0000000E
  BorderBottom = 15, // 0x0000000F
  CornerTopLeft = 16, // 0x00000010
  CornerBottonRight = 17, // 0x00000011
  Border = 18, // 0x00000012
  Object = 19, // 0x00000013
  Close = 20, // 0x00000014
  Help = 21, // 0x00000015
}
