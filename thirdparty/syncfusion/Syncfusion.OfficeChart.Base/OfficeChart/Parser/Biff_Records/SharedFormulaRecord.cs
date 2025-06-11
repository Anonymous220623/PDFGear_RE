// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.SharedFormulaRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.SharedFormula2)]
internal class SharedFormulaRecord : BiffRecordRaw, ISharedFormula
{
  private const int DEF_FIXED_SIZE = 10;
  [BiffRecordPos(0, 2)]
  private int m_iFirstRow;
  [BiffRecordPos(2, 2)]
  private int m_iLastRow;
  [BiffRecordPos(4, 1)]
  private int m_iFirstColumn;
  [BiffRecordPos(5, 1)]
  private int m_iLastColumn;
  [BiffRecordPos(6, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(8, 2)]
  private ushort m_usExpressionLen;
  private byte[] m_arrExpression;
  private Ptg[] m_arrFormula;

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int LastRow
  {
    get => this.m_iLastRow;
    set => this.m_iLastRow = value;
  }

  public int FirstColumn
  {
    get => this.m_iFirstColumn;
    set => this.m_iFirstColumn = value;
  }

  public int LastColumn
  {
    get => this.m_iLastColumn;
    set => this.m_iLastColumn = value;
  }

  public ushort ExpressionLen => this.m_usExpressionLen;

  public byte[] Expression
  {
    get => this.m_arrExpression;
    set
    {
      this.m_arrExpression = value;
      this.m_usExpressionLen = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public Ptg[] Formula
  {
    get => this.m_arrFormula;
    set => this.m_arrFormula = value;
  }

  public ushort Reserved => this.m_usReserved;

  public override int MinimumRecordSize => 8;

  public SharedFormulaRecord()
  {
  }

  public SharedFormulaRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SharedFormulaRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    iOffset = ArrayRecord.ParseDimensions((ISharedFormula) this, provider, iOffset, version);
    this.m_usReserved = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usExpressionLen = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_arrExpression = new byte[(int) this.m_usExpressionLen];
    provider.ReadArray(iOffset, this.m_arrExpression);
    this.m_arrFormula = FormulaUtil.ParseExpression(provider, iOffset, (int) this.m_usExpressionLen, out int _, version);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    iOffset = ArrayRecord.SerializeDimensions((ISharedFormula) this, provider, iOffset, version);
    provider.WriteUInt16(iOffset, this.m_usReserved);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usExpressionLen);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_arrExpression, 0, (int) this.m_usExpressionLen);
    this.m_iLength = iOffset + (int) this.m_usExpressionLen;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 10 + (int) this.m_usExpressionLen;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 10;
    return storeSize;
  }
}
