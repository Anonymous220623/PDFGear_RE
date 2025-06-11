// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListPatternType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public enum ListPatternType
{
  Arabic = 0,
  UpRoman = 1,
  LowRoman = 2,
  UpLetter = 3,
  LowLetter = 4,
  Ordinal = 5,
  Number = 6,
  OrdinalText = 7,
  KanjiDigit = 11, // 0x0000000B
  FarEast = 20, // 0x00000014
  LeadingZero = 22, // 0x00000016
  Bullet = 23, // 0x00000017
  ChineseCountingThousand = 39, // 0x00000027
  Special = 58, // 0x0000003A
  None = 255, // 0x000000FF
}
