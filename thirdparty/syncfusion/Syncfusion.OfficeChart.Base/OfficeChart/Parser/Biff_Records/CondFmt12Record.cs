// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CondFmt12Record
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CondFMT12)]
internal class CondFmt12Record : BiffRecordRaw
{
  private const ushort DEF_MINIMUM_RECORD_SIZE = 26;
  private const int DEF_SUB_ITEM_SIZE = 8;
  private const ushort DEF_REDRAW_ON = 1;
  private const ushort DEF_REDRAW_OFF = 0;
  private FutureHeader m_header;
  private ushort m_attribute = 1;
  private TAddr m_addrEncloseRange = new TAddr();
  private ushort m_CF12Count;
  private bool m_NeedRedraw = true;
  private ushort m_index;
  private ushort m_usCellsCount;
  private List<Rectangle> m_arrCells = new List<Rectangle>();
  private bool m_isparsed;

  public ushort CF12RecordCount
  {
    get => this.m_CF12Count;
    set => this.m_CF12Count = value;
  }

  public bool NeedRedrawRule
  {
    get => this.m_NeedRedraw;
    set => this.m_NeedRedraw = value;
  }

  public ushort Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public TAddr EncloseRange
  {
    get => this.m_addrEncloseRange;
    set => this.m_addrEncloseRange = value;
  }

  public ushort CellsCount
  {
    get => this.m_usCellsCount;
    set => this.m_usCellsCount = value;
  }

  public List<Rectangle> CellList
  {
    get => this.m_arrCells;
    internal set => this.m_arrCells = value;
  }

  public bool IsParsed
  {
    get => this.m_isparsed;
    set => this.m_isparsed = value;
  }

  public CondFmt12Record()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2169;
  }

  public CondFmt12Record(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CondFmt12Record(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_isparsed = true;
    this.m_header.Type = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_attribute = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_addrEncloseRange = provider.ReadAddr(iOffset);
    iOffset += 8;
    this.m_CF12Count = provider.ReadUInt16(iOffset);
    iOffset += 2;
    ushort num = provider.ReadUInt16(iOffset);
    this.m_NeedRedraw = ((int) num & 1) == 1;
    this.m_index = (ushort) ((uint) num >> 1);
    iOffset += 2;
    this.m_addrEncloseRange = provider.ReadAddr(iOffset);
    iOffset += 8;
    this.ExtractCellsList(provider, ref iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_header.Type);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_attribute);
    iOffset += 2;
    provider.WriteAddr(iOffset, this.m_addrEncloseRange);
    iOffset += 8;
    provider.WriteUInt16(iOffset, this.m_CF12Count);
    iOffset += 2;
    int num = this.m_NeedRedraw ? (int) this.m_index << 1 | 1 : (int) this.m_index << 1;
    provider.WriteUInt16(iOffset, (ushort) num);
    iOffset += 2;
    provider.WriteAddr(iOffset, this.m_addrEncloseRange);
    iOffset += 8;
    provider.WriteUInt16(iOffset, this.m_usCellsCount);
    iOffset += 2;
    int index = 0;
    while (index < (int) this.m_usCellsCount)
    {
      Rectangle arrCell = this.m_arrCells[index];
      provider.WriteAddr(iOffset, arrCell);
      ++index;
      iOffset += 8;
    }
  }

  private void ExtractCellsList(DataProvider provider, ref int offset)
  {
    this.m_usCellsCount = provider.ReadUInt16(offset);
    offset += 2;
    int num = 0;
    while (num < (int) this.m_usCellsCount)
    {
      this.m_arrCells.Add(provider.ReadAddrAsRectangle(offset));
      ++num;
      offset += 8;
    }
  }

  public override int GetStoreSize(OfficeVersion version) => 26 + this.m_arrCells.Count * 8;

  public void AddCell(Rectangle addr)
  {
    this.m_arrCells.Add(addr);
    ++this.m_usCellsCount;
  }

  public override object Clone()
  {
    CondFmt12Record condFmt12Record = (CondFmt12Record) base.Clone();
    condFmt12Record.m_arrCells = new List<Rectangle>(this.m_arrCells.Count);
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      condFmt12Record.AddCell(this.m_arrCells[index]);
    return (object) condFmt12Record;
  }
}
