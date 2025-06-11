// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeWorksheetCopyFlags
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum OfficeWorksheetCopyFlags
{
  None = 0,
  ClearBefore = 1,
  CopyNames = 2,
  CopyCells = 4,
  CopyRowHeight = 8,
  CopyColumnHeight = 16, // 0x00000010
  CopyOptions = 32, // 0x00000020
  CopyMerges = 64, // 0x00000040
  CopyShapes = 128, // 0x00000080
  CopyConditionlFormats = 256, // 0x00000100
  CopyAutoFilters = 512, // 0x00000200
  CopyDataValidations = 1024, // 0x00000400
  CopyPageSetup = 2048, // 0x00000800
  CopyTables = CopyPageSetup | CopyAutoFilters, // 0x00000A00
  CopyPivotTables = 4096, // 0x00001000
  CopyPalette = 8192, // 0x00002000
  CopyAll = CopyPalette | CopyPivotTables | CopyTables | CopyDataValidations | CopyConditionlFormats | CopyShapes | CopyMerges | CopyOptions | CopyColumnHeight | CopyRowHeight | CopyCells | CopyNames | ClearBefore, // 0x00003FFF
  CopyWithoutNames = CopyPivotTables | CopyTables | CopyDataValidations | CopyConditionlFormats | CopyShapes | CopyMerges | CopyOptions | CopyColumnHeight | CopyRowHeight | CopyCells | ClearBefore, // 0x00001FFD
}
