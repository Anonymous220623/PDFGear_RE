// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ActualRangeChangedEventArgs
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ActualRangeChangedEventArgs : EventArgs
{
  private readonly ChartAxis axis;
  private object actualMinimum;
  private object actualMaximum;

  public ActualRangeChangedEventArgs(ChartAxis axis) => this.axis = axis;

  public bool IsScrolling { get; set; }

  public object ActualMinimum
  {
    get => this.actualMinimum;
    set => this.actualMinimum = this.GetConvertedValue(value);
  }

  public object ActualMaximum
  {
    get => this.actualMaximum;
    set => this.actualMaximum = this.GetConvertedValue(value);
  }

  public object VisibleMinimum { get; set; }

  public object VisibleMaximum { get; set; }

  public double ActualInterval { get; internal set; }

  internal DoubleRange GetVisibleRange()
  {
    if (this.VisibleMinimum == null && this.VisibleMaximum == null)
      return DoubleRange.Empty;
    double end;
    double start;
    if (this.VisibleMaximum == null)
    {
      end = this.ToDouble(this.ActualMaximum);
      start = this.ToDouble(this.VisibleMinimum);
    }
    else if (this.VisibleMinimum == null)
    {
      end = this.ToDouble(this.VisibleMaximum);
      start = this.ToDouble(this.ActualMinimum);
    }
    else
    {
      end = this.ToDouble(this.VisibleMaximum);
      start = this.ToDouble(this.VisibleMinimum);
    }
    DoubleRange actualRange = this.GetActualRange();
    if (start < actualRange.Start)
      start = actualRange.Start;
    if (end > actualRange.End)
      end = actualRange.End;
    if (start == end)
      ++end;
    return new DoubleRange(start, end);
  }

  internal DoubleRange GetActualRange()
  {
    return new DoubleRange(this.ToDouble(this.ActualMinimum), this.ToDouble(this.ActualMaximum));
  }

  private object GetConvertedValue(object actualValue)
  {
    if (!(actualValue is double))
      return actualValue;
    if (this.axis is DateTimeAxis)
      return (object) ((double) actualValue).FromOADate();
    if (this.axis is TimeSpanAxis)
      return (object) TimeSpan.FromMilliseconds((double) actualValue);
    return this.axis is LogarithmicAxis ? (object) Math.Pow((this.axis as LogarithmicAxis).LogarithmicBase, (double) actualValue) : actualValue;
  }

  private double ToDouble(object actualValue)
  {
    if (actualValue is DateTime dateTime)
      return dateTime.ToOADate();
    if (actualValue is TimeSpan timeSpan)
      return timeSpan.TotalMilliseconds;
    return this.axis is LogarithmicAxis ? Math.Log((double) actualValue, (this.axis as LogarithmicAxis).LogarithmicBase) : Convert.ToDouble(actualValue);
  }
}
