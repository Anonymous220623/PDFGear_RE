// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.UnderlineStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public enum UnderlineStyle
{
  None = 0,
  Single = 1,
  Words = 2,
  Double = 3,
  Dotted = 4,
  [Obsolete("This enumeration option has been deprecated. On using this enumeration, None style will be set instead of DotDot.")] DotDot = 5,
  Thick = 6,
  Dash = 7,
  DotDash = 9,
  DotDotDash = 10, // 0x0000000A
  Wavy = 11, // 0x0000000B
  DottedHeavy = 20, // 0x00000014
  DashHeavy = 23, // 0x00000017
  DotDashHeavy = 25, // 0x00000019
  DotDotDashHeavy = 26, // 0x0000001A
  WavyHeavy = 27, // 0x0000001B
  DashLong = 39, // 0x00000027
  WavyDouble = 43, // 0x0000002B
  DashLongHeavy = 55, // 0x00000037
}
