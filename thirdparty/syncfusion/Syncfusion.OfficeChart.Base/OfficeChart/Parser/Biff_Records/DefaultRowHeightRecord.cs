// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DefaultRowHeightRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.DefaultRowHeight)]
internal class DefaultRowHeightRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usOptionFlags;
  [BiffRecordPos(2, 2)]
  private ushort m_usRowHeigth = (ushort) byte.MaxValue;
  private bool m_customHeight;

  public ushort OptionFlags
  {
    get => this.m_usOptionFlags;
    set => this.m_usOptionFlags = value;
  }

  public ushort Height
  {
    get => this.m_usRowHeigth;
    set => this.m_usRowHeigth = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => 4;

  internal bool CustomHeight => this.m_customHeight;

  public DefaultRowHeightRecord()
  {
  }

  public DefaultRowHeightRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DefaultRowHeightRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usOptionFlags = provider.ReadUInt16(iOffset);
    this.m_customHeight = provider.ReadBit(iOffset, 0);
    iOffset += 2;
    this.m_usRowHeigth = provider.ReadUInt16(2);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = 4;
    provider.WriteUInt16(iOffset, this.m_usOptionFlags);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usRowHeigth);
  }
}
