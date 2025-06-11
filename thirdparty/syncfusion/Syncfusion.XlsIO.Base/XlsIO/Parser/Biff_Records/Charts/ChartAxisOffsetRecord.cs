// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAxisOffsetRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAxisOffset)]
[CLSCompliant(false)]
public class ChartAxisOffsetRecord : BiffRecordRaw
{
  public const int DEF_MIN_RECORD_SIZE = 10;
  public const int DEF_RECORD_SIZE = 12;
  [BiffRecordPos(4, 2)]
  private ushort m_usOffset;

  public ChartAxisOffsetRecord()
  {
  }

  public ChartAxisOffsetRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAxisOffsetRecord(int iReserve)
    : base(iReserve)
  {
  }

  public int Offset
  {
    get => (int) this.m_usOffset;
    set => this.m_usOffset = (ushort) value;
  }

  public override int MinimumRecordSize => 10;

  public override int MaximumRecordSize => 12;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    iOffset += 4;
    this.m_usOffset = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, (ushort) this.TypeCode);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOffset);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 2);
    iOffset += 2;
    provider.WriteUInt32(iOffset, 0U);
  }

  public override int GetStoreSize(ExcelVersion version) => 12;
}
