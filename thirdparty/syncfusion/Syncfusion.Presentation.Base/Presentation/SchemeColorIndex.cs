// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SchemeColorIndex
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation;

[Flags]
internal enum SchemeColorIndex : sbyte
{
  None = -1, // 0xFF
  Text1 = 0,
  Background1 = 1,
  Text2 = 2,
  Background2 = Text2 | Background1, // 0x03
  Accent1 = 4,
  Accent2 = Accent1 | Background1, // 0x05
  Accent3 = Accent1 | Text2, // 0x06
  Accent4 = Accent3 | Background1, // 0x07
  Accent5 = 8,
  Accent6 = Accent5 | Background1, // 0x09
  Hyperlink = Accent5 | Text2, // 0x0A
  FollowedHyperlink = Hyperlink | Background1, // 0x0B
}
