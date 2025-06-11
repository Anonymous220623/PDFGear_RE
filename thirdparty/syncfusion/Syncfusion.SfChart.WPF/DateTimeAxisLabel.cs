// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DateTimeAxisLabel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DateTimeAxisLabel : ChartAxisLabel
{
  public DateTimeAxisLabel()
  {
  }

  public DateTimeAxisLabel(double position, object labelContent, double actualValue)
    : base(position, labelContent, actualValue)
  {
  }

  public DateTimeAxisLabel(double position, object labelContent)
    : base(position, labelContent)
  {
  }

  public DateTimeIntervalType IntervalType { get; internal set; }

  public bool IsTransition { get; internal set; }
}
