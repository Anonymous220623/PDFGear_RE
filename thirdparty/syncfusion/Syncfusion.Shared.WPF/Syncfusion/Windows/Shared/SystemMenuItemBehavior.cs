// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SystemMenuItemBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Syncfusion.Windows.Shared;

[CLSCompliant(false)]
[Flags]
public enum SystemMenuItemBehavior : uint
{
  DOES_NOT_EXIST = 4294967295, // 0xFFFFFFFF
  ENABLED = 0,
  BYCOMMAND = 0,
  GRAYED = 1,
  DISABLED = 2,
}
