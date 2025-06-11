// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.BinaryOperationPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tLessThan, "<")]
[Token(FormulaToken.tGreater, ">")]
[Token(FormulaToken.tConcat, "&")]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tAdd, "+")]
[Token(FormulaToken.tDiv, "/")]
[Token(FormulaToken.tMul, "*")]
[Token(FormulaToken.tSub, "-")]
[Token(FormulaToken.tPower, "^")]
[Token(FormulaToken.tGreaterEqual, ">=")]
[Token(FormulaToken.tLessEqual, "<=")]
[Token(FormulaToken.tCellRange, ":")]
[Token(FormulaToken.tNotEqual, "<>")]
[Token(FormulaToken.tEqual, "=")]
[Token(FormulaToken.tCellRangeIntersection, " ")]
public class BinaryOperationPtg : OperationPtg
{
  private static readonly Dictionary<string, FormulaToken> NameToId = new Dictionary<string, FormulaToken>(16 /*0x10*/);
  private static readonly Dictionary<FormulaToken, string> IdToName = new Dictionary<FormulaToken, string>(16 /*0x10*/);
  private static readonly TokenAttribute[] s_arrAttributes = new TokenAttribute[14]
  {
    new TokenAttribute(FormulaToken.tAdd, "+"),
    new TokenAttribute(FormulaToken.tDiv, "/"),
    new TokenAttribute(FormulaToken.tMul, "*"),
    new TokenAttribute(FormulaToken.tSub, "-"),
    new TokenAttribute(FormulaToken.tPower, "^"),
    new TokenAttribute(FormulaToken.tConcat, "&"),
    new TokenAttribute(FormulaToken.tLessThan, "<"),
    new TokenAttribute(FormulaToken.tLessEqual, "<="),
    new TokenAttribute(FormulaToken.tEqual, "="),
    new TokenAttribute(FormulaToken.tNotEqual, "<>"),
    new TokenAttribute(FormulaToken.tGreater, ">"),
    new TokenAttribute(FormulaToken.tGreaterEqual, ">="),
    new TokenAttribute(FormulaToken.tCellRangeIntersection, " "),
    new TokenAttribute(FormulaToken.tCellRange, ":")
  };

  [Preserve]
  static BinaryOperationPtg()
  {
    int index = 0;
    for (int length = BinaryOperationPtg.s_arrAttributes.Length; index < length; ++index)
    {
      TokenAttribute arrAttribute = BinaryOperationPtg.s_arrAttributes[index];
      FormulaToken formulaType = arrAttribute.FormulaType;
      string operationSymbol = arrAttribute.OperationSymbol;
      BinaryOperationPtg.NameToId.Add(operationSymbol, formulaType);
      BinaryOperationPtg.IdToName.Add(formulaType, operationSymbol);
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
        return BinaryOperationPtg.NameToId[operationSign];
    }
  }

  public static string GetTokenString(FormulaToken token) => BinaryOperationPtg.IdToName[token];

  [Preserve]
  public BinaryOperationPtg()
  {
  }

  [Preserve]
  public BinaryOperationPtg(string operation)
  {
    switch (operation)
    {
      case null:
        throw new ArgumentNullException(nameof (operation));
      case "":
        throw new ArgumentException("operation - string cannot be empty");
      default:
        this.OperationSymbol = BinaryOperationPtg.NameToId.ContainsKey(operation) ? operation : throw new ArgumentException(nameof (operation), "Unknown operation symbol");
        this.TokenCode = BinaryOperationPtg.GetTokenId(operation);
        break;
    }
  }

  [Preserve]
  public BinaryOperationPtg(FormulaToken operation)
  {
    this.OperationSymbol = BinaryOperationPtg.IdToName.ContainsKey(operation) ? BinaryOperationPtg.GetTokenString(operation) : throw new ArgumentException(nameof (operation), "Unknown operation symbol");
    this.TokenCode = operation;
  }

  [Preserve]
  public BinaryOperationPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public override int NumberOfOperands => 2;

  public override TOperation OperationType => TOperation.TYPE_BINARY;

  protected override TokenAttribute[] Attributes => BinaryOperationPtg.s_arrAttributes;

  public override void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization)
  {
    FormulaUtil.PushOperandToStack(operands, this.ToString(formulaUtil));
    string str1 = (string) operands.Pop();
    string str2 = (string) operands.Pop();
    string str3 = (string) operands.Pop();
    operands.Push((object) (str3 + str1 + str2));
  }

  public override string[] GetOperands(string strFormula, ref int index, FormulaUtil formulaParser)
  {
    index += this.OperationSymbol.Length;
    string rightBinaryOperand = formulaParser.GetRightBinaryOperand(strFormula, index, this.ToString());
    index += rightBinaryOperand.Length;
    return new string[1]{ rightBinaryOperand };
  }

  public override int GetSize(ExcelVersion version) => 1;
}
