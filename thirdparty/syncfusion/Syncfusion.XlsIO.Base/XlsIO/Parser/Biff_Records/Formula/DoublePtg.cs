// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.DoublePtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tNumber)]
[Preserve(AllMembers = true)]
public class DoublePtg : Ptg
{
  private double m_value;

  [Preserve]
  public DoublePtg()
  {
  }

  [Preserve]
  public DoublePtg(double value)
  {
    this.TokenCode = FormulaToken.tNumber;
    this.Value = value;
  }

  [Preserve]
  public DoublePtg(string value)
    : this(value, (NumberFormatInfo) null)
  {
  }

  [Preserve]
  public DoublePtg(string value, NumberFormatInfo numberInfo)
  {
    double num = numberInfo == null ? double.Parse(value) : double.Parse(value, (IFormatProvider) numberInfo);
    this.TokenCode = FormulaToken.tNumber;
    this.Value = num;
  }

  [Preserve]
  public DoublePtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public double Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  public override int GetSize(ExcelVersion version) => 9;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return numberFormat == null ? this.m_value.ToString() : this.m_value.ToString((IFormatProvider) numberFormat);
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberInfo)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1, numberInfo, false);
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_value).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_value = provider.ReadDouble(offset);
    offset += 8;
  }
}
