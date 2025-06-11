// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartWrapperRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartWrapper)]
[CLSCompliant(false)]
public class ChartWrapperRecord : BiffRecordRaw, ICloneable
{
  private const int DEF_RECORD_OFFSET = 4;
  private BiffRecordRaw m_record;

  public ChartWrapperRecord()
  {
  }

  public ChartWrapperRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartWrapperRecord(int iReserve)
    : base(iReserve)
  {
  }

  public BiffRecordRaw Record
  {
    get => this.m_record;
    set => this.m_record = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_record = BiffRecordFactory.GetRecord(provider, iOffset + 4, version);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    int storeSize = this.m_record.GetStoreSize(version);
    this.m_iLength = 4 + storeSize + 4;
    provider.WriteUInt16(iOffset + 4, (ushort) this.m_record.TypeCode);
    provider.WriteUInt16(iOffset + 4 + 2, (ushort) storeSize);
    this.m_record.InfillInternalData(provider, iOffset + 4 + 4, version);
    provider.WriteUInt16(iOffset, (ushort) this.TypeCode);
    provider.WriteUInt16(iOffset + 2, (ushort) 0);
  }

  public override int GetStoreSize(ExcelVersion version) => 8 + this.m_record.GetStoreSize(version);

  public new object Clone()
  {
    object obj = base.Clone();
    if (this.m_record != null)
      ((ChartWrapperRecord) obj).m_record = (BiffRecordRaw) this.m_record.Clone();
    return obj;
  }
}
