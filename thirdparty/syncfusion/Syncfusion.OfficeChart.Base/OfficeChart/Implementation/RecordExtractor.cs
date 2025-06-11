// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RecordExtractor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RecordExtractor
{
  private Dictionary<int, BiffRecordRaw> m_dictRecords;

  public RecordExtractor() => this.m_dictRecords = new Dictionary<int, BiffRecordRaw>();

  public BiffRecordRaw GetRecord(DataProvider provider, int iOffset, OfficeVersion version)
  {
    int recordType = provider != null ? (int) provider.ReadInt16(iOffset) : throw new ArgumentNullException(nameof (provider));
    iOffset += 2;
    BiffRecordRaw record = this.GetRecord(recordType);
    int iLength = (int) provider.ReadInt16(iOffset);
    record.Length = iLength;
    iOffset += 2;
    record.ParseStructure(provider, iOffset, iLength, version);
    return record;
  }

  public BiffRecordRaw GetRecord(int recordType)
  {
    BiffRecordRaw record;
    if (!this.m_dictRecords.TryGetValue(recordType, out record))
    {
      record = BiffRecordFactory.GetRecord(recordType);
      this.m_dictRecords.Add(recordType, record);
    }
    return record;
  }
}
