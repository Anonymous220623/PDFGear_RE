// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.ParenthesesPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tParentheses, "(")]
public class ParenthesesPtg : UnaryOperationPtg
{
  private static readonly TokenAttribute[] s_arrAttributes = new TokenAttribute[1]
  {
    new TokenAttribute(FormulaToken.tParentheses, "(")
  };

  [Preserve]
  static ParenthesesPtg()
  {
  }

  [Preserve]
  public ParenthesesPtg()
    : base("(")
  {
    this.TokenCode = FormulaToken.tParentheses;
  }

  [Preserve]
  public ParenthesesPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public ParenthesesPtg(string strFormula)
    : base("(")
  {
    if (strFormula != "(")
      throw new ArgumentOutOfRangeException(nameof (strFormula));
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return "()";
  }

  public override void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization)
  {
    object obj1 = operands.Pop();
    object obj2 = (object) (obj1 as AttrPtg);
    if (obj2 != null)
      obj1 = operands.Pop();
    else
      obj2 = (object) string.Empty;
    string str = $"{obj2.ToString()}({obj1.ToString()})";
    operands.Push((object) str);
  }

  public override string[] GetOperands(string strFormula, ref int index, FormulaUtil formulaParser)
  {
    int correspondingBracket = FormulaUtil.FindCorrespondingBracket(strFormula, index);
    index = correspondingBracket + 1;
    return new string[1]
    {
      strFormula.Substring(1, correspondingBracket - 1)
    };
  }

  public override ExcelParseFormulaOptions UpdateParseOptions(ExcelParseFormulaOptions options)
  {
    return options;
  }

  protected override TokenAttribute[] Attributes => ParenthesesPtg.s_arrAttributes;
}
