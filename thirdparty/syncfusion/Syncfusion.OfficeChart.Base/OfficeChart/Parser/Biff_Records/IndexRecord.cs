// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.IndexRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Index)]
[CLSCompliant(false)]
internal class IndexRecord : BiffRecordRaw
{
  private const int DEF_FIXED_SIZE = 16 /*0x10*/;
  private const int DEF_SUB_ITEM_SIZE = 4;
  [BiffRecordPos(0, 4, true)]
  private int m_iReserved0;
  [BiffRecordPos(4, 4, true)]
  private int m_iFirstRow;
  [BiffRecordPos(8, 4, true)]
  private int m_iLastRowAdd1;
  [BiffRecordPos(12, 4, true)]
  private int m_iReserved1;
  private int[] m_arrDbCells;
  private List<DBCellRecord> m_arrDBCellRecords;

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int LastRow
  {
    get => this.m_iLastRowAdd1;
    set => this.m_iLastRowAdd1 = value;
  }

  public int[] DbCells
  {
    get => this.m_arrDbCells;
    set
    {
      if (value == null)
        throw new ArgumentNullException();
      this.m_arrDbCells = value.Length <= 2048 /*0x0800*/ ? value : throw new ArgumentOutOfRangeException("Worksheet cannot contain more than 2048 DBCells.");
    }
  }

  public int Reserved0 => this.m_iReserved0;

  public int Reserved1 => this.m_iReserved1;

  public override int MinimumRecordSize => 16 /*0x10*/;

  internal List<DBCellRecord> DbCellRecords
  {
    get => this.m_arrDBCellRecords;
    set => this.m_arrDBCellRecords = value;
  }

  public IndexRecord()
  {
  }

  public IndexRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public IndexRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_iReserved0 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iFirstRow = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iLastRowAdd1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iReserved1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.InternalDataIntegrityCheck();
    this.m_arrDbCells = new int[(this.m_iLength - 16 /*0x10*/) / 4];
    int index = 0;
    while (iOffset < this.m_iLength)
    {
      this.m_arrDbCells[index] = provider.ReadInt32(iOffset);
      iOffset += 4;
      ++index;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteInt32(iOffset, this.m_iReserved0);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iFirstRow);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iLastRowAdd1);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iReserved1);
    iOffset += 4;
    if (this.m_arrDbCells == null)
      return;
    int length = this.m_arrDbCells.Length;
    for (int index = 0; index < length; ++index)
    {
      provider.WriteInt32(iOffset, this.m_arrDbCells[index]);
      iOffset += 4;
    }
  }

  private void InternalDataIntegrityCheck()
  {
    if (this.m_iLength % 4 != 0)
      throw new WrongBiffRecordDataException(nameof (IndexRecord));
  }

  public void UpdateOffsets()
  {
    if (this.m_arrDBCellRecords == null)
      return;
    int index = 0;
    for (int count = this.m_arrDBCellRecords.Count; index < count; ++index)
    {
      long streamPos = this.m_arrDBCellRecords[index].StreamPos;
      this.m_arrDbCells[index] = (int) streamPos;
    }
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 16 /*0x10*/ + (this.m_arrDbCells != null ? this.m_arrDbCells.Length : 0) * 4;
  }
}
