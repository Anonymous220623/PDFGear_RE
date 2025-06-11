// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.PivotCellType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

[Flags]
public enum PivotCellType
{
  ValueCell = 1,
  ExpanderCell = 2,
  HeaderCell = 4,
  TopLeftCell = 8,
  TotalCell = 16, // 0x00000010
  CalculationHeaderCell = 32, // 0x00000020
  RowHeaderCell = 64, // 0x00000040
  ColumnHeaderCell = 128, // 0x00000080
  GrandTotalCell = 256, // 0x00000100
}
