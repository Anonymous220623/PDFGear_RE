// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.RefErrorPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tRefErr1)]
[ErrorCode("#REF!", 23)]
[Token(FormulaToken.tRefErr2)]
[Token(FormulaToken.tRefErr3)]
public class RefErrorPtg : RefPtg, IRangeGetter
{
  public const string ReferenceError = "#REF!";

  [Preserve]
  static RefErrorPtg()
  {
  }

  [Preserve]
  public RefErrorPtg()
  {
  }

  [Preserve]
  public RefErrorPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public RefErrorPtg(string errorName)
    : base("A1")
  {
    this.TokenCode = FormulaToken.tRefErr2;
  }

  [Preserve]
  public RefErrorPtg(string errorName, IWorkbook book)
    : this(errorName)
  {
  }

  [Preserve]
  public RefErrorPtg(RefPtg dataHolder)
    : base(dataHolder)
  {
    this.TokenCode = RefErrorPtg.IndexToCode(RefPtg.CodeToIndex(dataHolder.TokenCode));
  }

  public override string ToString() => $"RefErr ({base.ToString()})";

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return "#REF!";
  }

  public override Ptg Offset(
    int iCurSheetIndex,
    int iTokenRow,
    int iTokenColumn,
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    Rectangle rectDest,
    out bool bChanged,
    WorkbookImpl book)
  {
    bChanged = false;
    return (Ptg) this;
  }

  public new static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tRefErr1;
      case 2:
        return FormulaToken.tRefErr2;
      case 3:
        return FormulaToken.tRefErr3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public override Ptg ConvertPtgToNPtg(IWorkbook parent, int iRow, int iColumn)
  {
    int num1 = this.IsColumnIndexRelative ? this.ColumnIndex - iColumn : this.ColumnIndex;
    int num2 = this.IsRowIndexRelative ? this.RowIndex - iRow : this.RowIndex;
    RefErrorPtg ptg = (RefErrorPtg) FormulaUtil.CreatePtg(RefErrorPtg.IndexToCode(this.CodeToIndex()));
    if (parent.Version == ExcelVersion.Excel97to2003)
    {
      ptg.RowIndex = num2;
      ptg.ColumnIndex = num1;
    }
    else
    {
      RefPtg refPtg = (RefPtg) ptg;
      refPtg.RowIndex = num2;
      refPtg.ColumnIndex = num1;
    }
    ptg.Options = this.Options;
    return (Ptg) ptg;
  }

  public new IRange GetRange(IWorkbook book, IWorksheet sheet) => (IRange) null;
}
