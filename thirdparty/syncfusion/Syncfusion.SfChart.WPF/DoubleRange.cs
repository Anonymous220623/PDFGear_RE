// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DoubleRange
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public struct DoubleRange
{
  private static readonly DoubleRange c_empty = new DoubleRange(double.NaN, double.NaN);
  private bool m_isempty;
  private double m_start;
  private double m_end;

  public static DoubleRange Empty => DoubleRange.c_empty;

  public double Start => this.m_start;

  public double End => this.m_end;

  public double Delta => this.m_end - this.m_start;

  public double Median => (this.m_start + this.m_end) / 2.0;

  public bool IsEmpty => this.m_isempty;

  public DoubleRange(double start, double end)
  {
    this.m_isempty = double.IsNaN(start) || double.IsNaN(end);
    if (start > end)
    {
      this.m_start = end;
      this.m_end = start;
    }
    else
    {
      this.m_start = start;
      this.m_end = end;
    }
  }

  public static DoubleRange operator +(DoubleRange leftRange, DoubleRange rightRange)
  {
    return DoubleRange.Union(leftRange, rightRange);
  }

  public static DoubleRange operator +(DoubleRange range, double value)
  {
    return DoubleRange.Union(range, value);
  }

  public static bool operator >(DoubleRange range, double value) => range.m_start > value;

  public static bool operator >(DoubleRange range, DoubleRange value)
  {
    return range.m_start > value.m_start && range.m_end > value.m_end;
  }

  public static bool operator <(DoubleRange range, DoubleRange value)
  {
    return range.m_start < value.m_start && range.m_end < value.m_end;
  }

  public static bool operator <(DoubleRange range, double value) => range.m_end < value;

  public static bool operator ==(DoubleRange leftRange, DoubleRange rightRange)
  {
    return leftRange.Equals((object) rightRange);
  }

  public static bool operator !=(DoubleRange leftRange, DoubleRange rightRange)
  {
    return !leftRange.Equals((object) rightRange);
  }

  public static DoubleRange Union(params double[] values)
  {
    double start = double.MaxValue;
    double end = double.MinValue;
    foreach (double d in values)
    {
      if (double.IsNaN(d))
        start = d;
      else if (start > d)
        start = d;
      if (end < d)
        end = d;
    }
    return new DoubleRange(start, end);
  }

  public static DoubleRange Union(DoubleRange leftRange, DoubleRange rightRange)
  {
    if (leftRange.IsEmpty)
      return rightRange;
    return rightRange.IsEmpty ? leftRange : new DoubleRange(Math.Min(leftRange.m_start, rightRange.m_start), Math.Max(leftRange.m_end, rightRange.m_end));
  }

  public static DoubleRange Union(DoubleRange range, double value)
  {
    return range.IsEmpty ? new DoubleRange(value, value) : new DoubleRange(Math.Min(range.m_start, value), Math.Max(range.m_end, value));
  }

  public static DoubleRange Scale(DoubleRange range, double value)
  {
    return range.IsEmpty ? range : new DoubleRange(range.m_start - value * range.Delta, range.m_end + value * range.Delta);
  }

  public static DoubleRange Offset(DoubleRange range, double value)
  {
    return range.IsEmpty ? range : new DoubleRange(range.m_start + value, range.m_end + value);
  }

  public static bool Exclude(
    DoubleRange range,
    DoubleRange excluder,
    out DoubleRange leftRange,
    out DoubleRange rightRange)
  {
    leftRange = DoubleRange.Empty;
    rightRange = DoubleRange.Empty;
    if (!range.IsEmpty && !excluder.IsEmpty)
    {
      if (excluder.m_end < range.m_start)
        leftRange = excluder.m_end <= range.m_start ? excluder : new DoubleRange(excluder.m_start, range.m_start);
      if (excluder.m_end > range.m_end)
        rightRange = excluder.m_start >= range.m_end ? excluder : new DoubleRange(range.m_end, excluder.m_end);
    }
    return !leftRange.IsEmpty || !rightRange.IsEmpty;
  }

  public bool Intersects(DoubleRange range)
  {
    if (this.IsEmpty || this.IsEmpty)
      return false;
    return this.Inside(range.m_start) || this.Inside(range.m_end) || range.Inside(this.m_start) || range.Inside(this.m_end);
  }

  public bool Intersects(double start, double end) => this.Intersects(new DoubleRange(start, end));

  public bool Inside(double value) => !this.IsEmpty && value <= this.m_end && value >= this.m_start;

  public bool Inside(DoubleRange range)
  {
    return !this.IsEmpty && this.m_start <= range.m_start && this.m_end >= range.m_end;
  }

  public override bool Equals(object obj)
  {
    return obj is DoubleRange doubleRange && this.m_start == doubleRange.m_start && this.m_end == doubleRange.m_end;
  }

  public override int GetHashCode() => this.m_start.GetHashCode() ^ this.m_end.GetHashCode();
}
