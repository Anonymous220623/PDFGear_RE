// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DValRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.DVal)]
internal class DValRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bPromtBoxVisible;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bPromtBoxPosFixed;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bDataCached;
  [BiffRecordPos(2, 4, true)]
  private int m_iPromtBoxHPos;
  [BiffRecordPos(6, 4, true)]
  private int m_iPromtBoxVPos;
  [BiffRecordPos(10, 4)]
  private uint m_uiObjectId = uint.MaxValue;
  [BiffRecordPos(14, 4)]
  private uint m_uiDVNumber;

  public ushort Options => this.m_usOptions;

  public bool IsPromtBoxVisible
  {
    get => this.m_bPromtBoxVisible;
    set => this.m_bPromtBoxVisible = value;
  }

  public bool IsPromtBoxPosFixed
  {
    get => this.m_bPromtBoxPosFixed;
    set => this.m_bPromtBoxPosFixed = value;
  }

  public bool IsDataCached
  {
    get => this.m_bDataCached;
    set => this.m_bDataCached = value;
  }

  public int PromtBoxHPos
  {
    get => this.m_iPromtBoxHPos;
    set => this.m_iPromtBoxHPos = value;
  }

  public int PromtBoxVPos
  {
    get => this.m_iPromtBoxVPos;
    set => this.m_iPromtBoxVPos = value;
  }

  public uint ObjectId
  {
    get => this.m_uiObjectId;
    set => this.m_uiObjectId = value;
  }

  public uint DVNumber
  {
    get => this.m_uiDVNumber;
    set => this.m_uiDVNumber = value;
  }

  public override int MinimumRecordSize => 18;

  public override int MaximumRecordSize => 18;

  public DValRecord()
  {
  }

  public DValRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DValRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bPromtBoxVisible = provider.ReadBit(iOffset, 0);
    this.m_bPromtBoxPosFixed = provider.ReadBit(iOffset, 1);
    this.m_bDataCached = provider.ReadBit(iOffset, 2);
    iOffset += 2;
    this.m_iPromtBoxHPos = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iPromtBoxVPos = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_uiObjectId = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiDVNumber = provider.ReadUInt32(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bPromtBoxVisible, 0);
    provider.WriteBit(iOffset, this.m_bPromtBoxPosFixed, 1);
    provider.WriteBit(iOffset, this.m_bDataCached, 2);
    iOffset += 2;
    provider.WriteInt32(iOffset, this.m_iPromtBoxHPos);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iPromtBoxVPos);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiObjectId);
    iOffset += 4;
    provider.WriteUInt32(iOffset, this.m_uiDVNumber);
  }

  public override bool Equals(object obj)
  {
    return obj is DValRecord dvalRecord && dvalRecord.IsPromtBoxVisible == this.IsPromtBoxVisible && dvalRecord.IsPromtBoxPosFixed == this.IsPromtBoxPosFixed && dvalRecord.IsDataCached == this.IsDataCached && dvalRecord.PromtBoxHPos == this.PromtBoxHPos && dvalRecord.PromtBoxVPos == this.PromtBoxVPos && (int) dvalRecord.ObjectId == (int) this.ObjectId;
  }

  public override int GetHashCode()
  {
    return this.m_bDataCached.GetHashCode() + this.m_bPromtBoxPosFixed.GetHashCode() + this.m_bPromtBoxPosFixed.GetHashCode() + this.m_iPromtBoxHPos.GetHashCode() + this.m_iPromtBoxVPos.GetHashCode() + this.m_uiObjectId.GetHashCode();
  }
}
