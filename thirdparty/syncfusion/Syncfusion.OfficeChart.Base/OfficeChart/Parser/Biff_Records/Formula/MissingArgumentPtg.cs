// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.MissingArgumentPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tMissingArgument)]
internal class MissingArgumentPtg : Ptg
{
  [Preserve]
  public MissingArgumentPtg() => this.TokenCode = FormulaToken.tMissingArgument;

  [Preserve]
  public MissingArgumentPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public MissingArgumentPtg(string strFormula)
  {
    if (strFormula != string.Empty)
      throw new ArgumentOutOfRangeException(nameof (strFormula), "should be empty string");
    this.TokenCode = FormulaToken.tMissingArgument;
  }

  public override int GetSize(OfficeVersion version) => 1;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return "";
  }
}
