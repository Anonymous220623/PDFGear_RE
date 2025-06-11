// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartEndDispUnitRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartEndDispUnit)]
public class ChartEndDispUnitRecord : BiffRecordRaw
{
  public const int DEF_MIN_RECORD_SIZE = 6;
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(4, 4, TFieldType.Bit)]
  private bool m_bIsShowLabel;

  public ChartEndDispUnitRecord()
  {
  }

  public ChartEndDispUnitRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartEndDispUnitRecord(int iReserve)
    : base(iReserve)
  {
  }

  public bool IsShowLabels
  {
    get => this.m_bIsShowLabel;
    set => this.m_bIsShowLabel = value;
  }

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 12;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    iOffset += 4;
    this.m_bIsShowLabel = provider.ReadBit(iOffset, 4);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, (ushort) this.TypeCode);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    int offset = iOffset;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 4;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 4;
    provider.WriteBit(offset, this.m_bIsShowLabel, 4);
  }

  public override int GetStoreSize(ExcelVersion version) => 12;
}
