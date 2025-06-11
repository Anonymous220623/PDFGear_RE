// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.SelectionRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Selection)]
[CLSCompliant(false)]
internal class SelectionRecord : BiffRecordRaw, ICloneable
{
  private const int DEF_FIXED_SIZE = 9;
  private const int DEF_SUB_ITEM_SIZE = 6;
  [BiffRecordPos(0, 1)]
  private byte m_Pane = 3;
  [BiffRecordPos(1, 2)]
  private ushort m_usRowActiveCell;
  [BiffRecordPos(3, 2)]
  private ushort m_usColActiveCell;
  [BiffRecordPos(5, 2)]
  private ushort m_usRefActiveCell;
  [BiffRecordPos(7, 2)]
  private ushort m_usNumRefs = 1;
  private List<SelectionRecord.TAddr> m_arrAddr = new List<SelectionRecord.TAddr>((IEnumerable<SelectionRecord.TAddr>) new SelectionRecord.TAddr[1]
  {
    new SelectionRecord.TAddr()
  });

  public byte Pane
  {
    get => this.m_Pane;
    set => this.m_Pane = value;
  }

  public ushort RowActiveCell
  {
    get => this.m_usRowActiveCell;
    set => this.m_usRowActiveCell = value;
  }

  public ushort ColumnActiveCell
  {
    get => this.m_usColActiveCell;
    set => this.m_usColActiveCell = value;
  }

  public ushort RefActiveCell
  {
    get => this.m_usRefActiveCell;
    set => this.m_usRefActiveCell = value;
  }

  public ushort NumRefs => this.m_usNumRefs;

  public override int MinimumRecordSize => 9;

  public SelectionRecord.TAddr[] Addr
  {
    get => this.m_arrAddr.ToArray();
    set
    {
      this.m_arrAddr = new List<SelectionRecord.TAddr>((IEnumerable<SelectionRecord.TAddr>) value);
      this.m_usNumRefs = (ushort) this.m_arrAddr.Count;
    }
  }

  public void SetSelection(int iIndex, SelectionRecord.TAddr addr)
  {
    if (iIndex >= (int) this.NumRefs || iIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    this.m_arrAddr[iIndex] = addr;
  }

  public SelectionRecord()
  {
  }

  public SelectionRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SelectionRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_Pane = provider.ReadByte(iOffset);
    this.m_usRowActiveCell = provider.ReadUInt16(iOffset + 1);
    this.m_usColActiveCell = provider.ReadUInt16(iOffset + 3);
    this.m_usRefActiveCell = provider.ReadUInt16(iOffset + 5);
    this.m_usNumRefs = provider.ReadUInt16(iOffset + 7);
    if (this.m_iLength < 9 + (int) this.m_usNumRefs * 6)
      throw new WrongBiffRecordDataException("Data length does not fit to number of refernces.");
    SelectionRecord.TAddr taddr = new SelectionRecord.TAddr();
    int num1 = 9;
    this.m_arrAddr.Clear();
    int num2 = 0;
    while (num2 < (int) this.m_usNumRefs)
    {
      taddr.m_usFirstRow = provider.ReadUInt16(iOffset + num1);
      taddr.m_usLastRow = provider.ReadUInt16(iOffset + num1 + 2);
      taddr.m_FirstCol = provider.ReadByte(iOffset + num1 + 4);
      taddr.m_LastCol = provider.ReadByte(iOffset + num1 + 5);
      this.m_arrAddr.Add(taddr);
      ++num2;
      num1 += 6;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 9;
    provider.WriteByte(iOffset, this.m_Pane);
    provider.WriteUInt16(iOffset + 1, this.m_usRowActiveCell);
    provider.WriteUInt16(iOffset + 3, this.m_usColActiveCell);
    provider.WriteUInt16(iOffset + 5, this.m_usRefActiveCell);
    provider.WriteUInt16(iOffset + 7, this.m_usNumRefs);
    int index = 0;
    while (index < (int) this.m_usNumRefs)
    {
      SelectionRecord.TAddr taddr = this.m_arrAddr[index];
      provider.WriteUInt16(iOffset + this.m_iLength, taddr.m_usFirstRow);
      provider.WriteUInt16(iOffset + this.m_iLength + 2, taddr.m_usLastRow);
      provider.WriteByte(iOffset + this.m_iLength + 4, taddr.m_FirstCol);
      provider.WriteByte(iOffset + this.m_iLength + 5, taddr.m_LastCol);
      ++index;
      this.m_iLength += 6;
    }
  }

  public override int GetStoreSize(OfficeVersion version) => 9 + this.m_arrAddr.Count * 6;

  public new object Clone()
  {
    SelectionRecord selectionRecord = (SelectionRecord) this.MemberwiseClone();
    selectionRecord.m_arrAddr = new List<SelectionRecord.TAddr>((IEnumerable<SelectionRecord.TAddr>) this.m_arrAddr);
    return (object) selectionRecord;
  }

  public struct TAddr(ushort FirstRow, ushort LastRow, byte FirstCol, byte LastCol)
  {
    public ushort m_usFirstRow = FirstRow;
    public ushort m_usLastRow = LastRow;
    public byte m_FirstCol = FirstCol;
    public byte m_LastCol = LastCol;

    public override string ToString()
    {
      return $"firstRow: {this.m_usFirstRow}, lastRow: {this.m_usLastRow}, firstColumn: {this.m_FirstCol}, lastColumn: {this.m_LastCol}";
    }
  }
}
