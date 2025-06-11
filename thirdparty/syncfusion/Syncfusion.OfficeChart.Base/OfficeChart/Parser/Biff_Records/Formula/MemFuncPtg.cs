// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.MemFuncPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tMemFunc2)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tMemFunc1)]
[Token(FormulaToken.tMemFunc3)]
[CLSCompliant(false)]
internal class MemFuncPtg : Ptg
{
  private const int SIZE = 3;
  private ushort m_usSize;
  private byte[] m_arrData = new byte[2];

  [Preserve]
  public MemFuncPtg()
  {
  }

  [Preserve]
  public MemFuncPtg(int size)
  {
    if (size < 0 || size > (int) ushort.MaxValue)
      throw new ArgumentOutOfRangeException(nameof (size), "Value cannot be less than 0 or greater than ushort.MaxValue");
    this.TokenCode = FormulaToken.tMemFunc1;
    this.m_usSize = (ushort) size;
  }

  [Preserve]
  public MemFuncPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  public ushort SubExpressionLength
  {
    get => this.m_usSize;
    set => this.m_usSize = value;
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
    return "( MemFunc not implemented ) Size is " + (object) this.m_usSize;
  }

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_usSize).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_usSize = provider.ReadUInt16(offset);
    offset += 2;
  }
}
