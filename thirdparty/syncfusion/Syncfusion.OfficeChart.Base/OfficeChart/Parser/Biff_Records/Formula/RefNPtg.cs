// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.RefNPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Token(FormulaToken.tRefN1)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tRefN2)]
[Token(FormulaToken.tRefN3)]
internal class RefNPtg : RefPtg
{
  [Preserve]
  public RefNPtg()
  {
  }

  [Preserve]
  public RefNPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public RefNPtg(int iCellRow, int iCellColumn, string strRow, string strColumn, bool bR1C1)
  {
    this.SetCell(iCellRow, iCellColumn, strRow, strColumn, bR1C1);
    this.ColumnIndex -= iCellColumn;
    this.RowIndex -= iCellRow;
    this.IsRowIndexRelative = true;
    this.IsColumnIndexRelative = true;
  }

  public override Ptg ConvertSharedToken(IWorkbook parent, int iRow, int iColumn)
  {
    int num1 = parent.Version == OfficeVersion.Excel97to2003 ? this.ColumnIndex : this.ColumnIndex;
    int num2 = parent.Version == OfficeVersion.Excel97to2003 ? this.RowIndex : this.RowIndex;
    int iColIndex = this.IsColumnIndexRelative ? iColumn + num1 : num1;
    int iRowIndex = this.IsRowIndexRelative ? iRow + num2 : num2;
    if (parent.Version == OfficeVersion.Excel97to2003)
    {
      iColIndex = (int) (byte) iColIndex;
      iRowIndex = (int) (ushort) iRowIndex;
    }
    Ptg ptg = (Ptg) new RefPtg(iRowIndex, iColIndex, this.Options);
    int index = RefNPtg.CodeToIndex(this.TokenCode);
    ptg.TokenCode = RefPtg.IndexToCode(index);
    return ptg;
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    short num = 256 /*0x0100*/;
    short rowIndex = (short) this.RowIndex;
    short columnIndex = (short) this.ColumnIndex;
    if (columnIndex >= (short) byte.MaxValue)
      columnIndex -= num;
    int row = this.IsRowIndexRelative ? Math.Abs(iRow + (int) rowIndex - 1) : this.RowIndex;
    int column = this.IsColumnIndexRelative ? Math.Abs(iColumn + (int) columnIndex - 1) : this.ColumnIndex;
    return RefPtg.GetCellName(iRow, iColumn, row, column, this.IsRowIndexRelative, this.IsColumnIndexRelative, bR1C1);
  }

  public override Ptg Get3DToken(int iSheetReference)
  {
    FormulaToken code = Ref3DPtg.IndexToCode(RefNPtg.CodeToIndex(this.TokenCode));
    Ptg ptg = (Ptg) new Ref3DPtg(iSheetReference, this.RowIndex, this.ColumnIndex, this.Options);
    ptg.TokenCode = code;
    return ptg;
  }

  public new static int CodeToIndex(FormulaToken token)
  {
    switch (token)
    {
      case FormulaToken.tRefN1:
        return 1;
      case FormulaToken.tRefN2:
        return 2;
      case FormulaToken.tRefN3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public new static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tRefN1;
      case 2:
        return FormulaToken.tRefN2;
      case 3:
        return FormulaToken.tRefN3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }
}
