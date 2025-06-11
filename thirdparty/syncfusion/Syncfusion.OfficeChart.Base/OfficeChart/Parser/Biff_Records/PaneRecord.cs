// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.PaneRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Pane)]
internal class PaneRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 10;
  [BiffRecordPos(0, 2)]
  private int m_iVertSplit;
  [BiffRecordPos(2, 2)]
  private int m_iHorizSplit;
  [BiffRecordPos(4, 2)]
  private int m_iFirstRow;
  [BiffRecordPos(6, 2)]
  private int m_iFirstColumn;
  [BiffRecordPos(8, 2)]
  private ushort m_usActivePane;

  public int VerticalSplit
  {
    get => this.m_iVertSplit;
    set => this.m_iVertSplit = value;
  }

  public int HorizontalSplit
  {
    get => this.m_iHorizSplit;
    set => this.m_iHorizSplit = value;
  }

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int FirstColumn
  {
    get => this.m_iFirstColumn;
    set => this.m_iFirstColumn = value;
  }

  public ushort ActivePane
  {
    get => this.m_usActivePane;
    set => this.m_usActivePane = value;
  }

  public override int MaximumRecordSize => 10;

  public override int MinimumRecordSize => 10;

  public PaneRecord()
  {
  }

  public PaneRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PaneRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_iVertSplit = (int) provider.ReadUInt16(iOffset);
    this.m_iHorizSplit = (int) provider.ReadUInt16(iOffset + 2);
    this.m_iFirstRow = (int) provider.ReadUInt16(iOffset + 4);
    this.m_iFirstColumn = (int) provider.ReadUInt16(iOffset + 6);
    this.m_usActivePane = provider.ReadUInt16(iOffset + 8);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, (ushort) this.m_iVertSplit);
    provider.WriteUInt16(iOffset + 2, (ushort) this.m_iHorizSplit);
    provider.WriteUInt16(iOffset + 4, (ushort) this.m_iFirstRow);
    provider.WriteUInt16(iOffset + 6, (ushort) this.m_iFirstColumn);
    provider.WriteUInt16(iOffset + 8, this.m_usActivePane);
    this.m_iLength = 10;
  }

  public override int GetStoreSize(OfficeVersion version) => 10;
}
