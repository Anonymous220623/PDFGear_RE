// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelCFType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public enum ExcelCFType
{
  CellValue = 1,
  Formula = 2,
  ColorScale = 3,
  DataBar = 4,
  IconSet = 6,
  Blank = 7,
  NoBlank = 8,
  SpecificText = 9,
  ContainsErrors = 10, // 0x0000000A
  NotContainsErrors = 11, // 0x0000000B
  TimePeriod = 12, // 0x0000000C
  Duplicate = 13, // 0x0000000D
  Unique = 14, // 0x0000000E
  TopBottom = 15, // 0x0000000F
  AboveBelowAverage = 16, // 0x00000010
}
