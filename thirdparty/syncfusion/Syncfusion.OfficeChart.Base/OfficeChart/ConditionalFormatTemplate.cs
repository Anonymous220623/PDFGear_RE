// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ConditionalFormatTemplate
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal enum ConditionalFormatTemplate
{
  CellValue = 0,
  Formula = 1,
  ColorScale = 2,
  DataBar = 3,
  IconSet = 4,
  Filter = 5,
  UniqueValues = 7,
  ContainsText = 8,
  ContainsBlanks = 9,
  ContainsNoBlanks = 10, // 0x0000000A
  ContainsErrors = 11, // 0x0000000B
  ContainsNoErrors = 12, // 0x0000000C
  Today = 15, // 0x0000000F
  Tomorrow = 16, // 0x00000010
  Yesterday = 17, // 0x00000011
  Last7Days = 18, // 0x00000012
  LastMonth = 19, // 0x00000013
  NextMonth = 20, // 0x00000014
  ThisWeek = 21, // 0x00000015
  NextWeek = 22, // 0x00000016
  LastWeek = 23, // 0x00000017
  ThisMonth = 24, // 0x00000018
  AboveAverage = 25, // 0x00000019
  BelowAverage = 26, // 0x0000001A
  DuplicateValues = 27, // 0x0000001B
  AboveOrEqualToAverage = 29, // 0x0000001D
  BelowOrEqualToAverage = 30, // 0x0000001E
}
