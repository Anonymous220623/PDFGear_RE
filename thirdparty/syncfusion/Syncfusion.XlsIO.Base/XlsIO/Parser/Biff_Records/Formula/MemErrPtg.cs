// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.MemErrPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tMemErr1)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tMemErr2)]
[Token(FormulaToken.tMemErr3)]
public class MemErrPtg : Ptg
{
  private const int SIZE = 7;
  private byte[] m_arrData = new byte[6];

  [Preserve]
  public MemErrPtg()
  {
  }

  [Preserve]
  public MemErrPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return "(MemErr not implemented.)";
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    this.m_arrData.CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override int GetSize(ExcelVersion version) => 7;

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    provider.CopyTo(offset, this.m_arrData, 0, this.m_arrData.Length);
    offset += this.m_arrData.Length;
  }
}
