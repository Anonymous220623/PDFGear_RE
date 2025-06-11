// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.TokenAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class TokenAttribute : Attribute
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
