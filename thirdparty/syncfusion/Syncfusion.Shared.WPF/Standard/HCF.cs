// Decompiled with JetBrains decompiler
// Type: Standard.HCF
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Standard;

[Flags]
internal enum HCF
{
  HIGHCONTRASTON = 1,
  AVAILABLE = 2,
  HOTKEYACTIVE = 4,
  CONFIRMHOTKEY = 8,
  HOTKEYSOUND = 16, // 0x00000010
  INDICATOR = 32, // 0x00000020
  HOTKEYAVAILABLE = 64, // 0x00000040
}
