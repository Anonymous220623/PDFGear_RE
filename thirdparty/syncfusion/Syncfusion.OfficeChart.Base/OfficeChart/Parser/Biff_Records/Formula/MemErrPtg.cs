// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.MemErrPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tMemErr3)]
[Token(FormulaToken.tMemErr1)]
[Token(FormulaToken.tMemErr2)]
internal class MemErrPtg : Ptg
{
  private const int SIZE = 7;
  private byte[] m_arrData = new byte[6];

  [Preserve]
  public MemErrPtg()
  {
  }

  [Preserve]
  public MemErrPtg(DataProvider provider, int offset, OfficeVersion version)
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

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    this.m_arrData.CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override int GetSize(OfficeVersion version) => 7;

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    provider.CopyTo(offset, this.m_arrData, 0, this.m_arrData.Length);
    offset += this.m_arrData.Length;
  }
}
