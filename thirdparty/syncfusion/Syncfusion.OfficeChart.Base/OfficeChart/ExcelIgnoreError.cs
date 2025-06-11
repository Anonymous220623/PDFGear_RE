// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ExcelIgnoreError
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

[Flags]
internal enum ExcelIgnoreError
{
  None = 0,
  EvaluateToError = 1,
  EmptyCellReferences = 2,
  NumberAsText = 4,
  OmittedCells = 8,
  InconsistentFormula = 16, // 0x00000010
  TextDate = 32, // 0x00000020
  UnlockedFormulaCells = 64, // 0x00000040
  All = UnlockedFormulaCells | TextDate | InconsistentFormula | OmittedCells | NumberAsText | EmptyCellReferences | EvaluateToError, // 0x0000007F
}
