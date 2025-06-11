// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.AreaError3DPtg
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
[Token(FormulaToken.tAreaErr3d1)]
[Token(FormulaToken.tAreaErr3d2)]
[Token(FormulaToken.tAreaErr3d3)]
[CLSCompliant(false)]
public class AreaError3DPtg : Area3DPtg, IRangeGetter
{
  [Preserve]
  public AreaError3DPtg()
  {
  }

  [Preserve]
  public AreaError3DPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public AreaError3DPtg(Area3DPtg ptg)
    : base(ptg)
  {
    this.TokenCode = ptg.TokenCode - 59 + 61;
  }

  [Preserve]
  public AreaError3DPtg(string value, IWorkbook book)
    : base(value, book)
  {
    this.TokenCode = FormulaToken.tAreaErr3d1;
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    IWorkbook parentWorkbook = formulaUtil?.ParentWorkbook;
    string sheetName = Ref3DPtg.GetSheetName(parentWorkbook, (int) this.RefIndex);
    return parentWorkbook == null ? "#REF!" : $"'{sheetName}'!{"#REF!"}";
  }

  public override int CodeToIndex() => AreaError3DPtg.CodeToIndex(this.TokenCode);

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
    return (Ptg) this.Clone();
  }

  public new static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tAreaErr3d1;
      case 2:
        return FormulaToken.tAreaErr3d2;
      case 3:
        return FormulaToken.tAreaErr3d3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index), "Must be less than 4 and greater than than 0.");
    }
  }

  public new static int CodeToIndex(FormulaToken code)
  {
    switch (code)
    {
      case FormulaToken.tAreaErr3d1:
        return 1;
      case FormulaToken.tAreaErr3d2:
        return 2;
      case FormulaToken.tAreaErr3d3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public new IRange GetRange(IWorkbook book, IWorksheet sheet) => (IRange) null;
}
