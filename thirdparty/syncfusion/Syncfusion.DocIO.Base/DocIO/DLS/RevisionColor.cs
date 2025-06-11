// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RevisionColor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

[Flags]
public enum RevisionColor
{
  ByAuthor = 0,
  Black = 1,
  Blue = 2,
  BrightGreen = Blue | Black, // 0x00000003
  DarkBlue = 4,
  DarkRed = DarkBlue | Black, // 0x00000005
  DarkYellow = DarkBlue | Blue, // 0x00000006
  Gray25 = DarkYellow | Black, // 0x00000007
  Gray50 = 8,
  Green = Gray50 | Black, // 0x00000009
  Pink = Gray50 | Blue, // 0x0000000A
  Red = Pink | Black, // 0x0000000B
  Teal = Gray50 | DarkBlue, // 0x0000000C
  Turquoise = Teal | Black, // 0x0000000D
  Violet = Teal | Blue, // 0x0000000E
  White = Violet | Black, // 0x0000000F
  Yellow = 16, // 0x00000010
  Auto = Yellow | Blue, // 0x00000012
  ClassicRed = Auto | Black, // 0x00000013
  ClassicBlue = Yellow | DarkBlue, // 0x00000014
}
