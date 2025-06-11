// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartWrappedFrameFormatImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartWrappedFrameFormatImpl(IApplication application, object parent) : 
  ChartFrameFormatImpl(application, parent)
{
  [CLSCompliant(false)]
  protected override bool CheckBegin(BiffRecordRaw record)
  {
    record = this.UnwrapRecord(record);
    return base.CheckBegin(record);
  }

  [CLSCompliant(false)]
  protected override void ParseRecord(BiffRecordRaw record, ref int iBeginCounter)
  {
    record = this.UnwrapRecord(record);
    base.ParseRecord(record, ref iBeginCounter);
  }

  [CLSCompliant(false)]
  protected override BiffRecordRaw UnwrapRecord(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    return record.TypeCode == TBIFFRecord.ChartWrapper ? ((ChartWrapperRecord) record).Record : record;
  }

  [CLSCompliant(false)]
  protected override void SerializeRecord(IList<IBiffStorage> list, BiffRecordRaw record)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    ChartWrapperRecord record1 = (ChartWrapperRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartWrapper);
    record1.Record = record;
    list.Add((IBiffStorage) record1.Clone());
  }
}
