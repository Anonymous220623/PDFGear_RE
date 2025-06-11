// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DCONRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.DCON)]
internal class DCONRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 2, true)]
  private short m_sFuncIndex;
  [BiffRecordPos(2, 2, true)]
  private short m_sLeftColumn;
  [BiffRecordPos(4, 2, true)]
  private short m_sTopRow;
  [BiffRecordPos(6, 2, true)]
  private short m_sLinkSource;

  public DCONRecord()
  {
  }

  public DCONRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DCONRecord(int iReserve)
    : base(iReserve)
  {
  }

  public short FuncIndex
  {
    get => this.m_sFuncIndex;
    set => this.m_sFuncIndex = value;
  }

  public bool IsLeftColumn
  {
    get => this.m_sLeftColumn == (short) 1;
    set => this.m_sLeftColumn = value ? (short) 1 : (short) 0;
  }

  public bool IsTopRow
  {
    get => this.m_sTopRow == (short) 1;
    set => this.m_sTopRow = value ? (short) 1 : (short) 0;
  }

  public bool IsLinkSource
  {
    get => this.m_sLinkSource == (short) 1;
    set => this.m_sLinkSource = value ? (short) 1 : (short) 0;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_sFuncIndex = provider.ReadInt16(iOffset);
    this.m_sLeftColumn = provider.ReadInt16(iOffset + 2);
    this.m_sTopRow = provider.ReadInt16(iOffset + 4);
    this.m_sLinkSource = provider.ReadInt16(iOffset + 6);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteInt16(iOffset, this.m_sFuncIndex);
    provider.WriteInt16(iOffset + 2, this.m_sLeftColumn);
    provider.WriteInt16(iOffset + 4, this.m_sTopRow);
    provider.WriteInt16(iOffset + 6, this.m_sLinkSource);
  }
}
