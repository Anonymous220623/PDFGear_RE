// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableParts
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

[Flags]
public enum PivotTableParts
{
  WholeTable = 1,
  PageFieldsLabels = 2,
  PageFieldsValues = 4,
  FirstColumnStripe = 8,
  SecondColumnStripe = 16, // 0x00000010
  FirstRowStripe = 32, // 0x00000020
  SecondRowStripe = 64, // 0x00000040
  FirstColumn = 128, // 0x00000080
  HeaderRow = 256, // 0x00000100
  FirstHeaderCell = 512, // 0x00000200
  SubtotalColumn1 = 1024, // 0x00000400
  SubtotalColumn2 = 2048, // 0x00000800
  SubtotalColumn3 = 4096, // 0x00001000
  BlankRow = 8192, // 0x00002000
  SubtotalRow1 = 16384, // 0x00004000
  SubtotalRow2 = 32768, // 0x00008000
  SubtotalRow3 = 65536, // 0x00010000
  ColumnSubHeading1 = 131072, // 0x00020000
  ColumnSubHeading2 = 262144, // 0x00040000
  ColumnSubHeading3 = 524288, // 0x00080000
  RowSubHeading1 = 1048576, // 0x00100000
  RowSubHeading2 = 2097152, // 0x00200000
  RowSubHeading3 = 4194304, // 0x00400000
  GrandTotalColumn = 8388608, // 0x00800000
  GrandTotalRow = 16777216, // 0x01000000
  None = 33554432, // 0x02000000
}
