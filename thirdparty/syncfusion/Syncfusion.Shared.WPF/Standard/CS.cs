﻿// Decompiled with JetBrains decompiler
// Type: Standard.CS
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Standard;

[Flags]
internal enum CS : uint
{
  VREDRAW = 1,
  HREDRAW = 2,
  DBLCLKS = 8,
  OWNDC = 32, // 0x00000020
  CLASSDC = 64, // 0x00000040
  PARENTDC = 128, // 0x00000080
  NOCLOSE = 512, // 0x00000200
  SAVEBITS = 2048, // 0x00000800
  BYTEALIGNCLIENT = 4096, // 0x00001000
  BYTEALIGNWINDOW = 8192, // 0x00002000
  GLOBALCLASS = 16384, // 0x00004000
  IME = 65536, // 0x00010000
  DROPSHADOW = 131072, // 0x00020000
}
