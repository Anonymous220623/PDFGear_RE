// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.StreamIdRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.StreamId)]
[CLSCompliant(false)]
public class StreamIdRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usStreamId;

  public StreamIdRecord()
  {
  }

  public StreamIdRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public StreamIdRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort StreamId
  {
    get => this.m_usStreamId;
    set => this.m_usStreamId = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usStreamId = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usStreamId);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
