// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.CellRangeListPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tCellRangeList, ",")]
internal class CellRangeListPtg : BinaryOperationPtg
{
  [Preserve]
  public CellRangeListPtg()
  {
  }

  [Preserve]
  public CellRangeListPtg(string operation)
  {
    switch (operation)
    {
      case null:
        throw new ArgumentNullException(nameof (operation));
      case "":
        throw new ArgumentException("operation - string cannot be empty");
      default:
        this.OperationSymbol = operation;
        this.TokenCode = FormulaToken.tCellRangeList;
        break;
    }
  }

  [Preserve]
  public CellRangeListPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public override string ToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.GetOperandsSeparator(formulaUtil);
  }
}
