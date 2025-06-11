// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.DoublePtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tNumber)]
[Preserve(AllMembers = true)]
internal class DoublePtg : Ptg
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
  public DoublePtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public double Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  public override int GetSize(OfficeVersion version) => 9;

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

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_value).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_value = provider.ReadDouble(offset);
    offset += 8;
  }
}
