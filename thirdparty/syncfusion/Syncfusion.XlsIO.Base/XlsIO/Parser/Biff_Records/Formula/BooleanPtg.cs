// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.BooleanPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tBoolean)]
[Preserve(AllMembers = true)]
public class BooleanPtg : Ptg
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
  public BooleanPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public bool Value
  {
    get => this.m_bValue;
    set => this.m_bValue = value;
  }

  public override int GetSize(ExcelVersion version) => 2;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return this.m_bValue.ToString().ToUpper();
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_bValue).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_bValue = provider.ReadBoolean(offset++);
  }
}
