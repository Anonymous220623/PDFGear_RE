// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.BooleanPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tBoolean)]
internal class BooleanPtg : Ptg
{
  private bool m_bValue;

  [Preserve]
  public BooleanPtg()
  {
  }

  [Preserve]
  public BooleanPtg(bool value)
  {
    this.TokenCode = FormulaToken.tBoolean;
    this.Value = value;
  }

  [Preserve]
  public BooleanPtg(string value)
    : this(bool.Parse(value))
  {
  }

  [Preserve]
  public BooleanPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public bool Value
  {
    get => this.m_bValue;
    set => this.m_bValue = value;
  }

  public override int GetSize(OfficeVersion version) => 2;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return this.m_bValue.ToString();
  }

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_bValue).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_bValue = provider.ReadBoolean(offset++);
  }
}
