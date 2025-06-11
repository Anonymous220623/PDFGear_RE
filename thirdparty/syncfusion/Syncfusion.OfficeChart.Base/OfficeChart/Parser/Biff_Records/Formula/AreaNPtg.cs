// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.AreaNPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[CLSCompliant(false)]
[Token(FormulaToken.tAreaN1)]
[Token(FormulaToken.tAreaN2)]
[Token(FormulaToken.tAreaN3)]
internal class AreaNPtg : AreaPtg
{
  [Preserve]
  public AreaNPtg()
  {
  }

  [Preserve]
  public AreaNPtg(string strFormula, IWorkbook book)
    : base(strFormula, book)
  {
  }

  [Preserve]
  public AreaNPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public short FirstColumn
  {
    get => (short) (ushort) base.FirstColumn;
    set => this.FirstColumn = (int) (ushort) value;
  }

  public short LastColumn
  {
    get => (short) (ushort) base.LastColumn;
    set => this.LastColumn = (int) (ushort) value;
  }

  public override Ptg ConvertSharedToken(IWorkbook parent, int iRow, int iColumn)
  {
    bool flag1 = this.IsWholeRows(parent);
    bool flag2 = this.IsWholeColumns(parent);
    int iFirstCol = !this.IsFirstColumnRelative || flag1 ? (int) this.FirstColumn : iColumn + (int) this.FirstColumn;
    int iFirstRow = !this.IsFirstRowRelative || flag2 ? this.FirstRow : iRow + this.FirstRow;
    int iLastCol = !this.IsLastColumnRelative || flag1 ? (int) this.LastColumn : iColumn + (int) this.LastColumn;
    int iLastRow = !this.IsLastRowRelative || flag2 ? this.LastRow : iRow + this.LastRow;
    if (parent.Version == OfficeVersion.Excel97to2003)
    {
      iFirstCol = (int) (byte) iFirstCol;
      iLastCol = (int) (byte) iLastCol;
      iFirstRow = (int) (ushort) iFirstRow;
      iLastRow = (int) (ushort) iLastRow;
    }
    Ptg ptg = (Ptg) new AreaPtg(iFirstRow, iFirstCol, iLastRow, iLastCol, this.FirstOptions, this.LastOptions);
    int index = AreaNPtg.CodeToIndex(this.TokenCode);
    ptg.TokenCode = AreaPtg.IndexToCode(index);
    return ptg;
  }

  public new static int CodeToIndex(FormulaToken token)
  {
    switch (token)
    {
      case FormulaToken.tAreaN1:
        return 1;
      case FormulaToken.tAreaN2:
        return 2;
      case FormulaToken.tAreaN3:
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
        return FormulaToken.tAreaN1;
      case 2:
        return FormulaToken.tAreaN2;
      case 3:
        return FormulaToken.tAreaN3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    WorkbookImpl parentWorkbook = formulaUtil != null ? (WorkbookImpl) formulaUtil.ParentWorkbook : (WorkbookImpl) null;
    bool flag1 = this.IsWholeRows((IWorkbook) parentWorkbook);
    bool flag2 = this.IsWholeColumns((IWorkbook) parentWorkbook);
    if (flag1 && bR1C1)
      return $"{RefPtg.GetR1C1Name(iRow, "R", this.FirstRow, this.IsFirstRowRelative)}:{RefPtg.GetR1C1Name(iRow, "R", this.LastRow, this.IsFirstRowRelative)}";
    if (flag2 && bR1C1)
      return $"{RefPtg.GetR1C1Name(iColumn, "C", (int) this.FirstColumn, this.IsFirstColumnRelative)}:{RefPtg.GetR1C1Name(iColumn, "C", (int) this.LastColumn, this.IsFirstColumnRelative)}";
    if (flag1)
    {
      (this.FirstRow + 1).ToString();
      return $"${(this.FirstRow + 1).ToString()}:${(this.LastRow + 1).ToString()}";
    }
    if (flag2)
    {
      RangeImpl.GetColumnName((int) this.FirstColumn + 1);
      return $"${RangeImpl.GetColumnName((int) this.FirstColumn + 1)}:${RangeImpl.GetColumnName((int) this.LastColumn + 1)}";
    }
    int row1 = this.IsFirstRowRelative ? this.GetUpdatedRowIndex(iRow, this.FirstRow, true) : this.FirstRow;
    int row2 = this.IsLastRowRelative ? this.GetUpdatedRowIndex(iRow, this.LastRow, false) : this.LastRow;
    int column1 = this.IsFirstColumnRelative ? this.GetUpdatedColumnIndex(iColumn, (int) this.FirstColumn, true) : (int) this.FirstColumn;
    int column2 = this.IsLastColumnRelative ? this.GetUpdatedColumnIndex(iColumn, (int) this.LastColumn, false) : (int) this.LastColumn;
    if ((int) this.FirstColumn == (int) this.LastColumn)
      column2 = column1;
    if (this.FirstRow == this.LastRow)
      row2 = row1;
    return $"{RefPtg.GetCellName(iRow, iColumn, row1, column1, this.IsFirstRowRelative, this.IsFirstColumnRelative, bR1C1)}:{RefPtg.GetCellName(iRow, iColumn, row2, column2, this.IsLastRowRelative, this.IsLastColumnRelative, bR1C1)}";
  }

  private int GetUpdatedRowIndex(int iRow, int row, bool isFirst)
  {
    int num = 0;
    return iRow == 0 && row == 0 ? num : (!(isFirst ? row <= iRow || iRow >= row || iRow == 0 : iRow <= row || row >= iRow || row == 0) ? iRow - (65536 /*0x010000*/ - row) : row + iRow - 1);
  }

  private int GetUpdatedColumnIndex(int iColumn, int column, bool isFirst)
  {
    int num = 0;
    return iColumn == 0 && column == 0 ? num : (!(isFirst ? column <= iColumn || iColumn >= column || iColumn == 0 : (iColumn <= column || column >= iColumn) && column == 0) ? column + iColumn - 256 /*0x0100*/ - 1 : column + iColumn - 1);
  }
}
