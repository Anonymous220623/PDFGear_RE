// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.MissingArgumentPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tMissingArgument)]
[Preserve(AllMembers = true)]
public class MissingArgumentPtg : Ptg
{
  [Preserve]
  public MissingArgumentPtg() => this.TokenCode = FormulaToken.tMissingArgument;

  [Preserve]
  public MissingArgumentPtg(DataProvider provider, int offset, ExcelVersion version)
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

  public override int GetSize(ExcelVersion version) => 1;

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
