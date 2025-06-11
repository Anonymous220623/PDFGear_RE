// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.UnaryOperationPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tPercent, "%", true)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tUnaryMinus, "-")]
[Token(FormulaToken.tUnaryPlus, "+")]
public class UnaryOperationPtg : OperationPtg
{
  private static readonly TokenAttribute[] s_arrAttributes;
  private static readonly Dictionary<string, TokenAttribute> NameToAttribute = new Dictionary<string, TokenAttribute>(3);

  [Preserve]
  static UnaryOperationPtg()
  {
    UnaryOperationPtg.s_arrAttributes = new TokenAttribute[3]
    {
      new TokenAttribute(FormulaToken.tUnaryMinus, "-"),
      new TokenAttribute(FormulaToken.tUnaryPlus, "+"),
      new TokenAttribute(FormulaToken.tPercent, "%", true)
    };
    int index = 0;
    for (int length = UnaryOperationPtg.s_arrAttributes.Length; index < length; ++index)
    {
      TokenAttribute arrAttribute = UnaryOperationPtg.s_arrAttributes[index];
      UnaryOperationPtg.NameToAttribute.Add(arrAttribute.OperationSymbol, arrAttribute);
    }
  }

  public static FormulaToken GetTokenId(string operationSign)
  {
    switch (operationSign)
    {
      case null:
        throw new ArgumentNullException(nameof (operationSign));
      case "":
        throw new ArgumentException("operationSign - string cannot be empty");
      default:
        return UnaryOperationPtg.NameToAttribute[operationSign].FormulaType;
    }
  }

  [Preserve]
  public UnaryOperationPtg()
  {
  }

  [Preserve]
  public UnaryOperationPtg(string strOperationSymbol)
  {
    TokenAttribute tokenAttribute;
    if (!UnaryOperationPtg.NameToAttribute.TryGetValue(strOperationSymbol, out tokenAttribute))
    {
      TokenAttribute[] attributes = this.Attributes;
      int num = attributes != null ? attributes.Length : throw new ArgumentNullException("Unknown operation");
      int index;
      for (index = 0; index < num; ++index)
      {
        tokenAttribute = attributes[index];
        if (tokenAttribute.OperationSymbol == strOperationSymbol)
          break;
      }
      if (index == num)
        throw new ArgumentNullException("Unknown operation.");
    }
    this.OperationSymbol = strOperationSymbol;
    this.TokenCode = tokenAttribute.FormulaType;
    this.IsPlaceAfter = tokenAttribute.IsPlaceAfter;
  }

  [Preserve]
  public UnaryOperationPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public override TOperation OperationType => TOperation.TYPE_UNARY;

  protected override TokenAttribute[] Attributes => UnaryOperationPtg.s_arrAttributes;

  public override int GetSize(ExcelVersion version) => 1;

  public override void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization)
  {
    if (operands == null)
      throw new ArgumentNullException(nameof (operands));
    FormulaUtil.PushOperandToStack(operands, this.ToString());
    string str1 = (string) operands.Pop();
    string str2 = (string) operands.Pop();
    if (this.IsPlaceAfter)
      operands.Push((object) (str2 + str1));
    else
      operands.Push((object) (str1 + str2));
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return this.OperationSymbol;
  }

  public override string[] GetOperands(string strFormula, ref int index, FormulaUtil formulaParser)
  {
    string[] operands = new string[1]
    {
      formulaParser.GetRightUnaryOperand(strFormula, index)
    };
    index += operands[0].Length + this.ToString().Length;
    return operands;
  }
}
