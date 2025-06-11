// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.RefNPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tRefN1)]
[Preserve(AllMembers = true)]
[CLSCompliant(false)]
[Token(FormulaToken.tRefN2)]
[Token(FormulaToken.tRefN3)]
public class RefNPtg : RefPtg
{
  [Preserve]
  public RefNPtg()
  {
  }

  [Preserve]
  public RefNPtg(DataProvider provider, int offset, ExcelVersion version)
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
    int num1 = parent.Version == ExcelVersion.Excel97to2003 ? this.ColumnIndex : this.ColumnIndex;
    int num2 = parent.Version == ExcelVersion.Excel97to2003 ? this.RowIndex : this.RowIndex;
    int iColIndex = this.IsColumnIndexRelative ? iColumn + num1 : num1;
    int iRowIndex = this.IsRowIndexRelative ? iRow + num2 : num2;
    if (parent.Version == ExcelVersion.Excel97to2003)
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
    IWorkbook parentWorkbook = formulaUtil != null ? formulaUtil.ParentWorkbook : (IWorkbook) null;
    int row = this.IsRowIndexRelative ? (parentWorkbook == null || Math.Abs(iRow + this.RowIndex) <= parentWorkbook.MaxRowCount ? Math.Abs(iRow + this.RowIndex - 1) : Math.Abs(iRow + this.RowIndex - parentWorkbook.MaxRowCount - 1)) : this.RowIndex;
    if (bR1C1 && this.IsRowIndexRelative)
      ++row;
    int column = this.IsColumnIndexRelative ? (parentWorkbook == null || Math.Abs(iColumn + this.ColumnIndex) <= parentWorkbook.MaxColumnCount ? Math.Abs(iColumn + this.ColumnIndex - 1) : Math.Abs(iColumn + this.ColumnIndex - parentWorkbook.MaxColumnCount - 1)) : this.ColumnIndex;
    if (bR1C1 && this.IsColumnIndexRelative)
      ++column;
    return RefPtg.GetCellName(iRow, iColumn, row, column, this.IsRowIndexRelative, this.IsColumnIndexRelative, bR1C1);
  }

  public override Ptg Get3DToken(int iSheetReference)
  {
    FormulaToken code = RefN3DPtg.IndexToCode(RefNPtg.CodeToIndex(this.TokenCode));
    Ptg ptg = (Ptg) new RefN3DPtg(iSheetReference, this.RowIndex, this.ColumnIndex, this.Options);
    ptg.TokenCode = code;
    return ptg;
  }

  internal FormulaToken GetErrorCode()
  {
    return RefErrorPtg.IndexToCode(RefNPtg.CodeToIndex(this.TokenCode));
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
