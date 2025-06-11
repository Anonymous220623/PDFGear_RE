// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelIgnoreError
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelIgnoreError
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
