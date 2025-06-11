// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.RecordsPosComparer
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class RecordsPosComparer : IComparer, IComparer<BiffRecordPosAttribute>
{
  public int Compare(object x, object y)
  {
    BiffRecordPosAttribute recordPosAttribute1 = (BiffRecordPosAttribute) x;
    BiffRecordPosAttribute recordPosAttribute2 = (BiffRecordPosAttribute) y;
    int num = recordPosAttribute1.Position.CompareTo(recordPosAttribute2.Position);
    if (num == 0 && (recordPosAttribute1.IsBit || recordPosAttribute2.IsBit))
    {
      if (recordPosAttribute1.IsBit && !recordPosAttribute2.IsBit)
        return 1;
      if (!recordPosAttribute1.IsBit && recordPosAttribute2.IsBit)
        return -1;
      if (recordPosAttribute1.IsBit && recordPosAttribute2.IsBit)
        return recordPosAttribute1.SizeOrBitPosition.CompareTo(recordPosAttribute2.SizeOrBitPosition);
    }
    return num;
  }

  public int Compare(BiffRecordPosAttribute x, BiffRecordPosAttribute y)
  {
    int num = x.Position.CompareTo(y.Position);
    if (num == 0 && (x.IsBit || y.IsBit))
    {
      if (x.IsBit && !y.IsBit)
        return 1;
      if (!x.IsBit && y.IsBit)
        return -1;
      if (x.IsBit && y.IsBit)
        return x.SizeOrBitPosition.CompareTo(y.SizeOrBitPosition);
    }
    return num;
  }
}
