// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotFormatRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotFormat)]
public class PivotFormatRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usType;
  [BiffRecordPos(2, 2)]
  private ushort m_usDataSize;

  public PivotFormatRecord()
  {
  }

  public PivotFormatRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotFormatRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Type
  {
    get => this.m_usType;
    set => this.m_usType = value;
  }

  public ushort DataSize
  {
    get => this.m_usDataSize;
    set => this.m_usDataSize = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usType = provider.ReadUInt16(iOffset);
    this.m_usDataSize = provider.ReadUInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usType);
    provider.WriteUInt16(iOffset + 2, this.m_usDataSize);
    this.m_iLength = 4;
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
