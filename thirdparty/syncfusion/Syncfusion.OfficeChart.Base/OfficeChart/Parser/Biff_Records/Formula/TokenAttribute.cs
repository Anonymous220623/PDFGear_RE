// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.TokenAttribute
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class TokenAttribute : Attribute
{
  private FormulaToken m_Code;
  private string m_strOperationSymbol = string.Empty;
  private bool m_bPlaceAfter;

  private TokenAttribute()
  {
  }

  public TokenAttribute(FormulaToken Code) => this.m_Code = Code;

  public TokenAttribute(FormulaToken Code, string OperationSymbol)
  {
    this.m_Code = Code;
    this.m_strOperationSymbol = OperationSymbol;
  }

  public TokenAttribute(FormulaToken Code, string OperationSymbol, bool bPlaceAfter)
  {
    this.m_Code = Code;
    this.m_strOperationSymbol = OperationSymbol;
    this.m_bPlaceAfter = bPlaceAfter;
  }

  public FormulaToken FormulaType => this.m_Code;

  public string OperationSymbol => this.m_strOperationSymbol;

  public bool IsPlaceAfter => this.m_bPlaceAfter;
}
