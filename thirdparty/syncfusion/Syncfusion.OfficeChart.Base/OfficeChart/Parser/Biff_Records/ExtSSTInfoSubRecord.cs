// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ExtSSTInfoSubRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.ExtSSTInfoSub)]
[CLSCompliant(false)]
internal class ExtSSTInfoSubRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 4, true)]
  private int m_iStreamPos;
  [BiffRecordPos(4, 2)]
  private ushort m_usBucketSSTOffset;
  [BiffRecordPos(6, 2)]
  private ushort m_usReserved;

  public int StreamPosition
  {
    get => this.m_iStreamPos;
    set => this.m_iStreamPos = value;
  }

  public ushort BucketSSTOffset
  {
    get => this.m_usBucketSSTOffset;
    set
    {
      this.m_usBucketSSTOffset = value <= (ushort) 8228 ? value : throw new ArgumentOutOfRangeException(nameof (BucketSSTOffset), "Bucket SST Offset cannot be larger then MAX record size. On each Continue Record offset must be started from zero.");
    }
  }

  public ushort Reserved => this.m_usReserved;

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public ExtSSTInfoSubRecord()
  {
  }

  public ExtSSTInfoSubRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExtSSTInfoSubRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_iStreamPos = provider.ReadInt32(iOffset);
    this.m_usBucketSSTOffset = provider.ReadUInt16(iOffset + 4);
    this.m_usReserved = provider.ReadUInt16(iOffset + 6);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteInt32(iOffset, this.m_iStreamPos);
    provider.WriteUInt16(iOffset + 4, this.m_usBucketSSTOffset);
    provider.WriteUInt16(iOffset + 6, this.m_usReserved);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(OfficeVersion version) => 8;
}
