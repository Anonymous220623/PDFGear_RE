// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.MemFuncPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tMemFunc1)]
[Token(FormulaToken.tMemFunc2)]
[Token(FormulaToken.tMemFunc3)]
[CLSCompliant(false)]
public class MemFuncPtg : Ptg
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
  public MemFuncPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  public ushort SubExpressionLength
  {
    get => this.m_usSize;
    set => this.m_usSize = value;
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
    return "( MemFunc not implemented ) Size is " + (object) this.m_usSize;
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_usSize).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_usSize = provider.ReadUInt16(offset);
    offset += 2;
  }
}
