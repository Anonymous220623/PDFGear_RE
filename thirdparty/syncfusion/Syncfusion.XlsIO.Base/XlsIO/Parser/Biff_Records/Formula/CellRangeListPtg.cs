// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.CellRangeListPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tCellRangeList, ",")]
[Preserve(AllMembers = true)]
public class CellRangeListPtg : BinaryOperationPtg
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
  public CellRangeListPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public override string ToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.GetOperandsSeparator(formulaUtil);
  }
}
