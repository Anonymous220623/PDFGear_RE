// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.RecordsPosComparer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

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
