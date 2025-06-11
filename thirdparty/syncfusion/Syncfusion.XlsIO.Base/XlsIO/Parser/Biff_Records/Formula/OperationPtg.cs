// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.OperationPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
public abstract class OperationPtg : Ptg
{
  private const string DEFAULT_ARGUMENTS_SEPARATOR = ",";
  private string m_strOperationSymbol = string.Empty;
  private bool m_bPlaceAfter;

  [Preserve]
  public OperationPtg()
  {
  }

  [Preserve]
  protected OperationPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public override bool IsOperation => true;

  public abstract TOperation OperationType { get; }

  public virtual int NumberOfOperands
  {
    get
    {
      switch (this.OperationType)
      {
        case TOperation.TYPE_UNARY:
          return 2;
        case TOperation.TYPE_BINARY:
          return 1;
        default:
          return 0;
      }
    }
  }

  public string OperationSymbol
  {
    get => this.m_strOperationSymbol;
    set => this.m_strOperationSymbol = value;
  }

  public bool IsPlaceAfter
  {
    get => this.m_bPlaceAfter;
    set => this.m_bPlaceAfter = value;
  }

  protected abstract TokenAttribute[] Attributes { get; }

  public virtual void PushResultToStack(Stack<object> operands)
  {
    this.PushResultToStack((FormulaUtil) null, operands, false);
  }

  public abstract void PushResultToStack(
    FormulaUtil formulaUtil,
    Stack<object> operands,
    bool isForSerialization);

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return this.m_strOperationSymbol;
  }

  public abstract string[] GetOperands(string strFormula, ref int index, FormulaUtil formulaParser);

  public virtual ExcelParseFormulaOptions UpdateParseOptions(ExcelParseFormulaOptions options)
  {
    return options | ExcelParseFormulaOptions.ParseComplexOperand;
  }

  protected string GetOperandsSeparator(FormulaUtil formulaUtil)
  {
    return formulaUtil != null ? formulaUtil.OperandsSeparator : ",";
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    TokenAttribute[] attributes = this.Attributes;
    if (attributes == null)
      return;
    int index = 0;
    for (int length = attributes.Length; index < length; ++index)
    {
      TokenAttribute tokenAttribute = attributes[index];
      if (tokenAttribute.FormulaType == this.TokenCode)
      {
        this.m_strOperationSymbol = tokenAttribute.OperationSymbol;
        this.m_bPlaceAfter = tokenAttribute.IsPlaceAfter;
        break;
      }
    }
  }
}
