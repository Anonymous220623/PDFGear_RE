// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CondFMTRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CondFMT)]
public class CondFMTRecord : BiffRecordRaw, ICloneable
{
  private const ushort DEF_MINIMUM_RECORD_SIZE = 14;
  private const int DEF_FIXED_SIZE = 14;
  private const int DEF_SUB_ITEM_SIZE = 8;
  private const ushort DEF_REDRAW_ON = 1;
  private const ushort DEF_REDRAW_OFF = 0;
  [BiffRecordPos(0, 2)]
  private ushort m_usCFNumber;
  [BiffRecordPos(2, 0, TFieldType.Bit)]
  private bool m_usNeedRecalc;
  private ushort m_index;
  private TAddr m_addrEncloseRange = new TAddr();
  private ushort m_usCellsCount;
  private List<Rectangle> m_arrCells = new List<Rectangle>();
  private bool m_isparsed;

  public ushort CFNumber
  {
    get => this.m_usCFNumber;
    set => this.m_usCFNumber = value;
  }

  public bool NeedRecalc
  {
    get => !this.m_usNeedRecalc;
    set => this.m_usNeedRecalc = value;
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
    set => this.m_usCellsCount = (ushort) this.m_arrCells.Count;
  }

  public List<Rectangle> CellList
  {
    get => this.m_arrCells;
    internal set => this.m_arrCells = value;
  }

  public override int MinimumRecordSize => 14;

  public bool IsParsed
  {
    get => this.m_isparsed;
    set => this.m_isparsed = value;
  }

  public CondFMTRecord()
  {
  }

  public CondFMTRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CondFMTRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_isparsed = true;
    this.m_usCFNumber = provider.ReadUInt16(iOffset);
    iOffset += 2;
    ushort num = provider.ReadUInt16(iOffset);
    this.m_usNeedRecalc = ((int) num & 1) == 1;
    this.m_index = (ushort) ((uint) num >> 1);
    iOffset += 2;
    this.m_addrEncloseRange = provider.ReadAddr(iOffset);
    iOffset += 8;
    this.ExtractCellsList(provider, ref iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usCellsCount = (ushort) this.m_arrCells.Count;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usCFNumber);
    iOffset += 2;
    int num = this.m_usNeedRecalc ? (int) this.m_index << 1 | 1 : (int) this.m_index << 1;
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

  public override int GetStoreSize(ExcelVersion version) => 14 + this.m_arrCells.Count * 8;

  public void AddCell(Rectangle addr)
  {
    this.m_arrCells.Add(addr);
    ++this.m_usCellsCount;
  }

  public override object Clone()
  {
    CondFMTRecord condFmtRecord = (CondFMTRecord) base.Clone();
    condFmtRecord.m_arrCells = new List<Rectangle>(this.m_arrCells.Count);
    int index = 0;
    for (int count = this.m_arrCells.Count; index < count; ++index)
      condFmtRecord.AddCell(this.m_arrCells[index]);
    return (object) condFmtRecord;
  }
}
