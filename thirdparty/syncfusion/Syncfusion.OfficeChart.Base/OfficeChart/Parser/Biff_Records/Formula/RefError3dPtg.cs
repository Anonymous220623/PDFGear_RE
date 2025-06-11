// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.RefError3dPtg
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
[Token(FormulaToken.tRefErr3d1)]
[Token(FormulaToken.tRefErr3d2)]
[Token(FormulaToken.tRefErr3d3)]
internal class RefError3dPtg : Ref3DPtg, ISheetReference, IReference, IRangeGetter
{
  [Preserve]
  public RefError3dPtg()
  {
  }

  [Preserve]
  public RefError3dPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public RefError3dPtg(string strFormula, IWorkbook parent)
    : base(strFormula, parent)
  {
    this.TokenCode = FormulaToken.tRefErr3d1;
  }

  [Preserve]
  public RefError3dPtg(Ref3DPtg dataHolder)
    : base(dataHolder)
  {
  }

  public override string ToString() => $"RefErr3d ({this.RefIndex.ToString()}{base.ToString()})";

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    string sheetName = Ref3DPtg.GetSheetName(formulaUtil.ParentWorkbook, (int) this.RefIndex);
    return formulaUtil == null || sheetName == null ? "#REF!" : $"'{sheetName}'!{"#REF!"}";
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
        return FormulaToken.tRefErr3d1;
      case 2:
        return FormulaToken.tRefErr3d2;
      case 3:
        return FormulaToken.tRefErr3d3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public new IRange GetRange(IWorkbook book, IWorksheet sheet) => (IRange) null;
}
