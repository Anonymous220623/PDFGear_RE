// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.DateTimeRange
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Data;

public struct DateTimeRange : IValueRange<DateTime>
{
  public DateTimeRange(DateTime start)
  {
    this.Start = start;
    this.End = start;
  }

  public DateTimeRange(DateTime start, DateTime end)
  {
    this.Start = start;
    this.End = end;
  }

  public DateTime Start { get; set; }

  public DateTime End { get; set; }

  public double TotalMilliseconds => (this.End - this.Start).TotalMilliseconds;
}
