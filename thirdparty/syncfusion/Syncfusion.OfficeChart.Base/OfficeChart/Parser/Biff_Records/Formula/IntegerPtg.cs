// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.IntegerPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tInteger)]
[CLSCompliant(false)]
[Preserve(AllMembers = true)]
internal class IntegerPtg : Ptg
{
  public ushort m_usValue;

  [Preserve]
  public IntegerPtg()
  {
  }

  [Preserve]
  public IntegerPtg(ushort value)
  {
    this.TokenCode = FormulaToken.tInteger;
    this.Value = value;
  }

  [Preserve]
  public IntegerPtg(string value)
    : this(ushort.Parse(value))
  {
  }

  [Preserve]
  public IntegerPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public ushort Value
  {
    get => this.m_usValue;
    set => this.m_usValue = value;
  }

  public override int GetSize(OfficeVersion version) => 3;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return this.m_usValue.ToString();
  }

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_usValue).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_usValue = provider.ReadUInt16(offset);
    offset += 2;
  }
}
