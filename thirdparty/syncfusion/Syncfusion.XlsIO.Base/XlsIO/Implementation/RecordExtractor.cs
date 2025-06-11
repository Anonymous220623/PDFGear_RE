// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RecordExtractor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class RecordExtractor
{
  private Dictionary<int, BiffRecordRaw> m_dictRecords;

  public RecordExtractor() => this.m_dictRecords = new Dictionary<int, BiffRecordRaw>();

  public BiffRecordRaw GetRecord(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (provider == null)
      return (BiffRecordRaw) null;
    int recordType = (int) provider.ReadInt16(iOffset);
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
