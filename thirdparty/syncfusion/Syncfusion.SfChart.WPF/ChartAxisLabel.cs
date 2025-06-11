// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisLabel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAxisLabel
{
  private LabelAlignment axisLabelAlignment;

  public ChartAxisLabel()
  {
  }

  public ChartAxisLabel(double position, object labelContent, double actualValue)
  {
    this.Position = position;
    this.LabelContent = labelContent;
    this.ActualValue = actualValue;
  }

  public ChartAxisLabel(double position, object labelContent)
  {
    this.Position = position;
    this.LabelContent = labelContent;
  }

  public object LabelContent { get; set; }

  public DataTemplate PrefixLabelTemplate { get; set; }

  public DataTemplate PostfixLabelTemplate { get; set; }

  public double Position { get; set; }

  internal double ActualValue { get; set; }

  internal object LabelStyle { get; set; }

  internal bool LabelContentChanged { get; set; }

  internal ChartAxis ChartAxis { get; set; }

  internal LabelAlignment AxisLabelAlignment
  {
    get => this.axisLabelAlignment;
    set
    {
      if (this.axisLabelAlignment == value)
        return;
      this.axisLabelAlignment = value;
    }
  }

  internal object GetContent() => this.LabelContent;
}
