// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.IntegerPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Token(FormulaToken.tInteger)]
[Preserve(AllMembers = true)]
public class IntegerPtg : Ptg
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
  public IntegerPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public ushort Value
  {
    get => this.m_usValue;
    set => this.m_usValue = value;
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
    return this.m_usValue.ToString();
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_usValue).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_usValue = provider.ReadUInt16(offset);
    offset += 2;
  }
}
