// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelExportDataTableOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelExportDataTableOptions
{
  None = 0,
  ColumnNames = 1,
  ComputedFormulaValues = 2,
  DetectColumnTypes = 4,
  DefaultStyleColumnTypes = 8,
  PreserveOleDate = 16, // 0x00000010
  ExportHiddenColumns = 32, // 0x00000020
  ExportHiddenRows = 64, // 0x00000040
  DetectMixedValueType = 132, // 0x00000084
  TrimColumnNames = 264, // 0x00000108
}
