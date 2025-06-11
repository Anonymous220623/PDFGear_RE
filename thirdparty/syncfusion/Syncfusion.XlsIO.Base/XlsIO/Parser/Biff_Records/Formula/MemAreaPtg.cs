// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.MemAreaPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tMemArea1)]
[Token(FormulaToken.tMemArea2)]
[Token(FormulaToken.tMemArea3)]
public class MemAreaPtg : Ptg, IAdditionalData
{
  private const int DEF_RECT_SIZE = 8;
  private const int DEF_HEADER_SIZE = 7;
  private int m_iReserved;
  private ushort m_usSubExpressionLength;
  private Ptg[] m_arrSubexpression;
  private Rectangle[] m_arrRects;

  [Preserve]
  public MemAreaPtg()
  {
  }

  [Preserve]
  public MemAreaPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public MemAreaPtg(string strFormula)
  {
  }

  [CLSCompliant(false)]
  public ushort SubExpressionLength => this.m_usSubExpressionLength;

  public Ptg[] Subexpression => this.m_arrSubexpression;

  public Rectangle[] Rectangles => this.m_arrRects;

  public override int GetSize(ExcelVersion version) => (int) this.m_usSubExpressionLength + 7;

  public override byte[] ToByteArray(ExcelVersion version) => base.ToByteArray(version);

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return "MemArea";
  }

  public int ReadArray(DataProvider provider, int offset)
  {
    ushort length = provider.ReadUInt16(offset);
    offset += 2;
    this.m_arrRects = new Rectangle[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      ushort top = provider.ReadUInt16(offset);
      offset += 2;
      ushort bottom = provider.ReadUInt16(offset);
      offset += 2;
      ushort left = provider.ReadUInt16(offset);
      offset += 2;
      ushort right = provider.ReadUInt16(offset);
      offset += 2;
      this.m_arrRects[index] = Rectangle.FromLTRB((int) left, (int) top, (int) right, (int) bottom);
    }
    return offset;
  }

  public int AdditionalDataSize => 2 + (this.m_arrRects != null ? this.m_arrRects.Length : 0) * 8;

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_iReserved = provider.ReadInt32(offset);
    offset += 4;
    this.m_usSubExpressionLength = provider.ReadUInt16(offset);
    offset += 2;
    this.m_arrSubexpression = FormulaUtil.ParseExpression(provider, offset, (int) this.m_usSubExpressionLength, out int _, version);
    offset += (int) this.m_usSubExpressionLength;
  }
}
