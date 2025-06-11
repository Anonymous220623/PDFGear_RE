// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.DateTimeRangeComparer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Collections.Generic;

#nullable disable
namespace HandyControl.Tools;

public class DateTimeRangeComparer : IComparer<DateTimeRange>
{
  public int Compare(DateTimeRange x, DateTimeRange y)
  {
    if (x.Start > y.Start && x.End > y.End)
      return 1;
    return x.Start < y.Start && x.End < y.End ? -1 : 0;
  }
}
