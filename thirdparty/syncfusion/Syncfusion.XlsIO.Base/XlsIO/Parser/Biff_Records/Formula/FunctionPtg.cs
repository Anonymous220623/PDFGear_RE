// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.FunctionPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tFunction1)]
[Token(FormulaToken.tFunction2)]
[Token(FormulaToken.tFunction3)]
public class FunctionPtg : OperationPtg
{
  public const string OperandsDelimiter = ",";
  private ExcelFunction m_FunctionIndex = ExcelFunction.NONE;
  private byte m_ArgumentsNumber;

  [Preserve]
  public FunctionPtg() => this.TokenCode = FormulaToken.tFunction2;

  [Preserve]
  public FunctionPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public FunctionPtg(ExcelFunction index)
  {
    this.m_FunctionIndex = index;
    this.TokenCode = FormulaToken.tFunction2;
    int num;
    if (!FormulaUtil.FunctionIdToParamCount.TryGetValue(index, out num))
      return;
    this.m_ArgumentsNumber = (byte) num;
  }

  [Preserve]
  public FunctionPtg(string strFunctionName)
    : this(FormulaUtil.FunctionAliasToId[strFunctionName])
  {
  }

  public ExcelFunction FunctionIndex
  {
    get => this.m_FunctionIndex;
    set => this.m_FunctionIndex = value;
  }

  public byte NumberOfArguments
  {
    get => this.m_ArgumentsNumber;
    set => this.m_ArgumentsNumber = value;
  }

  public override TOperation OperationType => TOperation.TYPE_FUNCTION;

  protected override TokenAttribute[] Attributes => (TokenAttribute[]) null;

  protected string[] GetOperands(
    string strFormula,
    ref int index,
    bool checkParamCount,
    FormulaUtil formulaParser)
  {
    List<string> stringList = new List<string>();
    int num = 0;
    strFormula = strFormula.Substring(index + 1, strFormula.Length - index - 2);
    index = -1;
    if (strFormula.Length > 0)
    {
      while (index < strFormula.Length)
      {
        string functionOperand = formulaParser.GetFunctionOperand(strFormula, index);
        stringList.Add(functionOperand);
        index += functionOperand.Length + 1;
        ++num;
      }
    }
    if (checkParamCount && num != (int) this.m_ArgumentsNumber)
      throw new ArgumentException("Too many or not enough arguments.");
    return stringList.ToArray();
  }

  public static FormulaToken IndexToCode(int index)
  {
    return Ptg.IndexToCode(FormulaToken.tFunction1, index);
  }

  public override int GetSize(ExcelVersion version) => 3;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    string str;
    if (!FormulaUtil.FunctionIdToAlias.TryGetValue(this.m_FunctionIndex, out str))
      str = ((Excel2007Function) this.m_FunctionIndex).ToString();
    if (isForSerialization && (FormulaUtil.IsExcel2010Function(this.m_FunctionIndex) || FormulaUtil.IsExcel2013Function(this.m_FunctionIndex) || FormulaUtil.IsExcel2016Function(this.m_FunctionIndex) || FormulaUtil.IsExcelOffice365Function(this.m_FunctionIndex)))
      str = "_xlfn." + str;
    return str;
  }

  public override void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization)
  {
    int num;
    if (FormulaUtil.FunctionIdToParamCount.TryGetValue(this.FunctionIndex, out int _) && FormulaUtil.MissingArgumentsCount.TryGetValue(this.FunctionIndex, out num))
      this.m_ArgumentsNumber = (byte) num;
    if (operands.Count < (int) this.m_ArgumentsNumber)
      throw new ArgumentOutOfRangeException("Not enough arguments.");
    string operand1 = this.ToString(formulaUtil, 0, 0, false, (NumberFormatInfo) null, isForSerialization);
    FormulaUtil.PushOperandToStack(operands, operand1);
    string str1 = (string) operands.Pop();
    string str2 = this.m_ArgumentsNumber > (byte) 0 ? operands.Pop().ToString() : string.Empty;
    string operandsSeparator = formulaUtil.OperandsSeparator;
    for (int index = 1; index < (int) this.m_ArgumentsNumber; ++index)
      str2 = operands.Pop().ToString() + operandsSeparator + str2;
    string operand2 = $"{str1}({str2})";
    FormulaUtil.PushOperandToStack(operands, operand2);
  }

  public override string[] GetOperands(string strFormula, ref int index, FormulaUtil formulaParser)
  {
    return this.GetOperands(strFormula, ref index, true, formulaParser);
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    byteArray[0] = (byte) this.TokenCode;
    BitConverter.GetBytes((ushort) this.m_FunctionIndex).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_FunctionIndex = (ExcelFunction) provider.ReadUInt16(offset);
    if (!FormulaUtil.FunctionIdToAlias.TryGetValue(this.m_FunctionIndex, out string _))
      throw new ArgumentNullException("Unknown function");
    int num;
    if (FormulaUtil.FunctionIdToParamCount.TryGetValue(this.m_FunctionIndex, out num))
      this.m_ArgumentsNumber = (byte) num;
    offset += 2;
  }
}
