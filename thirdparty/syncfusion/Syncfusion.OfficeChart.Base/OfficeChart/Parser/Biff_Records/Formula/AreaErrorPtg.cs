// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.AreaErrorPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[CLSCompliant(false)]
[Token(FormulaToken.tAreaErr1)]
[Token(FormulaToken.tAreaErr2)]
[Token(FormulaToken.tAreaErr3)]
internal class AreaErrorPtg : AreaPtg, IRangeGetter
{
  [Preserve]
  public AreaErrorPtg()
  {
  }

  [Preserve]
  public AreaErrorPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public AreaErrorPtg(AreaPtg area)
    : base(area)
  {
    this.TokenCode = area.TokenCode - 37 + 43;
  }

  [Preserve]
  public AreaErrorPtg(string value, IWorkbook book)
    : base(value, book)
  {
    this.TokenCode = FormulaToken.tAreaErr1;
  }

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

  public override int CodeToIndex() => AreaErrorPtg.CodeToIndex(this.TokenCode);

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
        return FormulaToken.tAreaErr1;
      case 2:
        return FormulaToken.tAreaErr2;
      case 3:
        return FormulaToken.tAreaErr3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public new static int CodeToIndex(FormulaToken code)
  {
    switch (code)
    {
      case FormulaToken.tAreaErr1:
        return 1;
      case FormulaToken.tAreaErr2:
        return 2;
      case FormulaToken.tAreaErr3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public new IRange GetRange(IWorkbook book, IWorksheet sheet) => (IRange) null;
}
