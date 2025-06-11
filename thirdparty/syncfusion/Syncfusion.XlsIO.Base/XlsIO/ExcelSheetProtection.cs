// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelSheetProtection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelSheetProtection
{
  None = 0,
  Objects = 1,
  Scenarios = 2,
  FormattingCells = 4,
  FormattingColumns = 8,
  FormattingRows = 16, // 0x00000010
  InsertingColumns = 32, // 0x00000020
  InsertingRows = 64, // 0x00000040
  InsertingHyperlinks = 128, // 0x00000080
  DeletingColumns = 256, // 0x00000100
  DeletingRows = 512, // 0x00000200
  LockedCells = 1024, // 0x00000400
  Sorting = 2048, // 0x00000800
  Filtering = 4096, // 0x00001000
  UsingPivotTables = 8192, // 0x00002000
  UnLockedCells = 16384, // 0x00004000
  Content = 32768, // 0x00008000
  All = Content | UnLockedCells | UsingPivotTables | Filtering | Sorting | LockedCells | DeletingRows | DeletingColumns | InsertingHyperlinks | InsertingRows | InsertingColumns | FormattingRows | FormattingColumns | FormattingCells | Scenarios | Objects, // 0x0000FFFF
}
