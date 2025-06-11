// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.FunctionVarPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tFunctionVar3)]
[Token(FormulaToken.tFunctionVar2)]
[CLSCompliant(false)]
[Token(FormulaToken.tFunctionVar1)]
[Preserve(AllMembers = true)]
public class FunctionVarPtg : FunctionPtg
{
  [Preserve]
  public FunctionVarPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public FunctionVarPtg(ExcelFunction funcIndex)
    : base(funcIndex)
  {
    this.TokenCode = FormulaToken.tFunctionVar2;
  }

  [Preserve]
  public FunctionVarPtg(string strFunctionName)
    : base(strFunctionName)
  {
    this.TokenCode = FormulaToken.tFunctionVar2;
  }

  [Preserve]
  public FunctionVarPtg() => this.TokenCode = FormulaToken.tFunctionVar2;

  public override int GetSize(ExcelVersion version) => base.GetSize(version) + 1;

  public override string[] GetOperands(string strFormula, ref int index, FormulaUtil formulaParser)
  {
    string[] operands = this.GetOperands(strFormula, ref index, false, formulaParser);
    if (this.FunctionIndex != ExcelFunction.CustomFunction)
      this.NumberOfArguments = (byte) operands.Length;
    else
      this.NumberOfArguments = (byte) (operands.Length + 1);
    return operands;
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    byteArray[1] = this.NumberOfArguments;
    BitConverter.GetBytes((ushort) this.FunctionIndex).CopyTo((Array) byteArray, 2);
    return byteArray;
  }

  public override void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization)
  {
    if (operands == null)
      throw new ArgumentNullException(nameof (operands));
    if (operands.Count < (int) this.NumberOfArguments)
      throw new ArgumentException("Not enough elements in stack");
    if (this.FunctionIndex == ExcelFunction.CustomFunction)
    {
      string operandsSeparator = formulaUtil.OperandsSeparator;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("(");
      int num1 = 1;
      for (int index = (int) this.NumberOfArguments - 1; num1 <= index; ++num1)
      {
        string str = Convert.ToString(operands.Pop());
        stringBuilder.Insert(1, str);
        if (num1 != index)
          stringBuilder.Insert(1, operandsSeparator);
      }
      stringBuilder.Append(")");
      string str1 = (string) operands.Pop();
      int length = str1.Length;
      if (str1[length - 1] == '\'')
      {
        int num2 = str1.LastIndexOf('\'', length - 2);
        if (num2 >= 0)
          str1 = str1.Substring(num2 + 1, length - num2 - 2);
      }
      stringBuilder.Insert(0, str1);
      string str2 = stringBuilder.ToString();
      operands.Push((object) str2);
    }
    else
      base.PushResultToStack(formulaUtil, operands, isForSerialization);
  }

  public new static FormulaToken IndexToCode(int index)
  {
    return Ptg.IndexToCode(FormulaToken.tFunctionVar1, index);
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.NumberOfArguments = provider.ReadByte(offset++);
    this.FunctionIndex = (ExcelFunction) provider.ReadUInt16(offset);
    offset += 2;
  }
}
