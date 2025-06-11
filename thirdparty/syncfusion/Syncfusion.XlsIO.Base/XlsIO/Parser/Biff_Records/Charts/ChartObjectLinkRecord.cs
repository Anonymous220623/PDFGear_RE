// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartObjectLinkRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartObjectLink)]
[CLSCompliant(false)]
public class ChartObjectLinkRecord : BiffRecordRaw
{
  public const int DEF_RECORD_SIZE = 6;
  [BiffRecordPos(0, 2)]
  private ushort m_usLinkObject;
  [BiffRecordPos(2, 2)]
  private ushort m_usLinkIndex1;
  [BiffRecordPos(4, 2)]
  private ushort m_usLinkIndex2;

  public ExcelObjectTextLink LinkObject
  {
    get => (ExcelObjectTextLink) this.m_usLinkObject;
    set => this.m_usLinkObject = (ushort) value;
  }

  public ushort SeriesNumber
  {
    get => this.m_usLinkIndex1;
    set => this.m_usLinkIndex1 = value;
  }

  public ushort DataPointNumber
  {
    get => this.m_usLinkIndex2;
    set => this.m_usLinkIndex2 = value;
  }

  public override int MinimumRecordSize => 6;

  public override int MaximumRecordSize => 6;

  public ChartObjectLinkRecord()
  {
  }

  public ChartObjectLinkRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartObjectLinkRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usLinkObject = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLinkIndex1 = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLinkIndex2 = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usLinkObject);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLinkIndex1);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLinkIndex2);
  }

  public override int GetStoreSize(ExcelVersion version) => 6;
}
