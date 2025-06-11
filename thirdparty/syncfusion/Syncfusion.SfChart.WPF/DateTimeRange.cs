// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DateTimeRange
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public struct DateTimeRange(DateTime rangeStart, DateTime rangeEnd) : IEquatable<DateTimeRange>
{
  private DateTime m_start = rangeStart;
  private DateTime m_end = rangeEnd;

  public bool IsEmpty => this.m_end <= this.m_start;

  public DateTime Start => this.m_start;

  public DateTime End => this.m_end;

  public override int GetHashCode() => base.GetHashCode();

  public override bool Equals(object obj) => obj is DateTimeRange other && this.Equals(other);

  public bool Equals(DateTimeRange other) => !(this.Start != other.Start) && this.End == other.End;

  public static bool operator ==(DateTimeRange point1, DateTimeRange point2)
  {
    return point1.Equals(point2);
  }

  public static bool operator !=(DateTimeRange point1, DateTimeRange point2)
  {
    return !point1.Equals(point2);
  }
}
