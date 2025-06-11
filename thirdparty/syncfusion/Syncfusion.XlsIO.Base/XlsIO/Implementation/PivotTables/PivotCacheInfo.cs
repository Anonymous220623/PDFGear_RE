// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCacheInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotCacheInfo : ICloneable
{
  private List<BiffRecordRaw> m_records;

  private StreamIdRecord StreamIdRecord
  {
    get => this.m_records == null ? (StreamIdRecord) null : this.m_records[0] as StreamIdRecord;
  }

  public int StreamId
  {
    get
    {
      StreamIdRecord streamIdRecord = this.StreamIdRecord;
      return streamIdRecord == null ? -1 : (int) streamIdRecord.StreamId;
    }
    set => this.StreamIdRecord.StreamId = (ushort) value;
  }

  public PivotCacheInfo()
  {
  }

  public PivotCacheInfo(IList<BiffRecordRaw> data, int startIndex) => this.Parse(data, startIndex);

  public int Parse(IList<BiffRecordRaw> data, int startIndex)
  {
    this.m_records = new List<BiffRecordRaw>();
    BiffRecordRaw biffRecordRaw = data[startIndex];
    if (biffRecordRaw.TypeCode != TBIFFRecord.StreamId)
      throw new ArgumentOutOfRangeException();
    int count = data.Count;
    do
    {
      this.m_records.Add(biffRecordRaw);
      ++startIndex;
      if (startIndex < count)
        biffRecordRaw = data[startIndex];
      else
        break;
    }
    while (biffRecordRaw.TypeCode != TBIFFRecord.StreamId);
    return startIndex;
  }

  public void Serialize(OffsetArrayList records)
  {
    if (this.m_records == null || this.m_records.Count <= 0)
      return;
    records.AddList((IList) this.m_records);
  }

  public object Clone()
  {
    PivotCacheInfo pivotCacheInfo = (PivotCacheInfo) this.MemberwiseClone();
    pivotCacheInfo.m_records = CloneUtils.CloneCloneable<BiffRecordRaw>(this.m_records);
    return (object) pivotCacheInfo;
  }
}
