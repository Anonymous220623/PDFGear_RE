// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartWrappedFrameFormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartWrappedFrameFormatImpl(IApplication application, object parent) : 
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
