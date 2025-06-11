// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ZoomGesture
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Shared;

public enum ZoomGesture
{
  None = 0,
  MouseWheelUp = 1,
  MouseWheelDown = 2,
  Ctrl = 4,
  Shift = 8,
  Alt = 16, // 0x00000010
  LeftClick = 32, // 0x00000020
  RightClick = 64, // 0x00000040
  LeftDoubleClick = 128, // 0x00000080
  RightDoubleClick = 256, // 0x00000100
  And = 512, // 0x00000200
}
