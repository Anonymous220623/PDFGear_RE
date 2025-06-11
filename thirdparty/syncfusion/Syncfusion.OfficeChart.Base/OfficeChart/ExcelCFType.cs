// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ExcelCFType
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal enum ExcelCFType
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
}
