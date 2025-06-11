// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.TableRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Table)]
internal class TableRecord : BiffRecordRaw
{
  public const ushort OperationModeBitMask = 12;
  public const int OperationModeStartBit = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usFirstRow;
  [BiffRecordPos(2, 2)]
  private ushort m_usLastRow;
  [BiffRecordPos(4, 1)]
  private byte m_FirstCol;
  [BiffRecordPos(5, 1)]
  private byte m_LastCol;
  [BiffRecordPos(6, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(6, 0, TFieldType.Bit)]
  private bool m_bRecalculate;
  [BiffRecordPos(6, 1, TFieldType.Bit)]
  private bool m_bCalculateOnOpen;
  [BiffRecordPos(8, 2)]
  private ushort m_usInputCellRow;
  [BiffRecordPos(10, 2)]
  private ushort m_usInputCellCol;
  [BiffRecordPos(12, 2)]
  private ushort m_usInputCellRowForCol;
  [BiffRecordPos(14, 2)]
  private ushort m_usInputCellColForCol;

  public ushort FirstRow
  {
    get => this.m_usFirstRow;
    set => this.m_usFirstRow = value;
  }

  public ushort LastRow
  {
    get => this.m_usLastRow;
    set => this.m_usLastRow = value;
  }

  public byte FirstCol
  {
    get => this.m_FirstCol;
    set => this.m_FirstCol = value;
  }

  public byte LastCol
  {
    get => this.m_LastCol;
    set => this.m_LastCol = value;
  }

  public bool IsRecalculate
  {
    get => this.m_bRecalculate;
    set => this.m_bRecalculate = value;
  }

  public bool IsCalculateOnOpen
  {
    get => this.m_bCalculateOnOpen;
    set => this.m_bCalculateOnOpen = value;
  }

  public ushort OperationMode
  {
    get => (ushort) ((uint) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 12) >> 2);
    set
    {
      if (value > (ushort) 4)
        throw new ArgumentOutOfRangeException();
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 12, (ushort) ((uint) value << 2));
    }
  }

  public ushort InputCellRow
  {
    get => this.m_usInputCellRow;
    set => this.m_usInputCellRow = value;
  }

  public ushort InputCellColumn
  {
    get => this.m_usInputCellCol;
    set => this.m_usInputCellCol = value;
  }

  public ushort InputCellRowForColumn
  {
    get => this.m_usInputCellRowForCol;
    set => this.m_usInputCellRowForCol = value;
  }

  public ushort InputCellColumnForColumn
  {
    get => this.m_usInputCellColForCol;
    set => this.m_usInputCellColForCol = value;
  }

  public override int MinimumRecordSize => 16 /*0x10*/;

  public override int MaximumRecordSize => 16 /*0x10*/;

  public TableRecord()
  {
  }

  public TableRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public TableRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usFirstRow = provider.ReadUInt16(iOffset);
    this.m_usLastRow = provider.ReadUInt16(iOffset + 2);
    this.m_FirstCol = provider.ReadByte(iOffset + 4);
    this.m_LastCol = provider.ReadByte(iOffset + 5);
    this.m_usOptions = provider.ReadUInt16(iOffset + 6);
    this.m_bRecalculate = provider.ReadBit(iOffset + 6, 0);
    this.m_bCalculateOnOpen = provider.ReadBit(iOffset + 6, 1);
    this.m_usInputCellRow = provider.ReadUInt16(iOffset + 8);
    this.m_usInputCellCol = provider.ReadUInt16(iOffset + 10);
    this.m_usInputCellRowForCol = provider.ReadUInt16(iOffset + 12);
    this.m_usInputCellColForCol = provider.ReadUInt16(iOffset + 14);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFirstRow);
    provider.WriteUInt16(iOffset + 2, this.m_usLastRow);
    provider.WriteByte(iOffset + 4, this.m_FirstCol);
    provider.WriteByte(iOffset + 5, this.m_LastCol);
    provider.WriteUInt16(iOffset + 6, this.m_usOptions);
    provider.WriteBit(iOffset + 6, this.m_bRecalculate, 0);
    provider.WriteBit(iOffset + 6, this.m_bCalculateOnOpen, 1);
    provider.WriteUInt16(iOffset + 8, this.m_usInputCellRow);
    provider.WriteUInt16(iOffset + 10, this.m_usInputCellCol);
    provider.WriteUInt16(iOffset + 12, this.m_usInputCellRowForCol);
    provider.WriteUInt16(iOffset + 14, this.m_usInputCellColForCol);
    this.m_iLength = this.MinimumRecordSize;
  }
}
