// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DBCellRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.DBCell)]
[CLSCompliant(false)]
internal class DBCellRecord : BiffRecordRaw
{
  private const int DEF_FIXED_SIZE = 4;
  private const int DEF_SUB_ITEM_SIZE = 2;
  [BiffRecordPos(0, 4, false)]
  private int m_iRowOffset;
  private ushort[] m_arrCellOffset = new ushort[1];

  public int RowOffset
  {
    get => this.m_iRowOffset;
    set => this.m_iRowOffset = value;
  }

  public ushort[] CellOffsets
  {
    get => this.m_arrCellOffset;
    set => this.m_arrCellOffset = value;
  }

  public override int MinimumRecordSize => 4;

  public DBCellRecord()
  {
  }

  public DBCellRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DBCellRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_iRowOffset = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.InternalDataIntegrityCheck();
    int length = (this.m_iLength - 4) / 2;
    this.m_arrCellOffset = new ushort[length];
    int index = 0;
    while (index < length)
    {
      this.m_arrCellOffset[index] = provider.ReadUInt16(iOffset);
      ++index;
      iOffset += 2;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    int length = this.m_arrCellOffset.Length;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteInt32(iOffset, this.m_iRowOffset);
    iOffset += 4;
    for (int index = 0; index < length; ++index)
    {
      provider.WriteUInt16(iOffset, this.m_arrCellOffset[index]);
      iOffset += 2;
    }
  }

  private void InternalDataIntegrityCheck()
  {
    if ((this.Length - 2) % 2 != 0)
      throw new WrongBiffRecordDataException(nameof (DBCellRecord));
  }

  public override int GetStoreSize(OfficeVersion version) => 4 + this.m_arrCellOffset.Length * 2;
}
