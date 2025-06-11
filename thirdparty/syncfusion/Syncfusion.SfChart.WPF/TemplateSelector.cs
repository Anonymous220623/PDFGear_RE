// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TemplateSelector
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class TemplateSelector : DependencyObject
{
  private double maximumY;
  private double minimumY;
  private SparklineBase sparkline;
  private int dataCount;
  private double minimumX;

  public double MaximumY
  {
    get => this.maximumY;
    internal set => this.maximumY = value;
  }

  public double MinimumY
  {
    get => this.minimumY;
    internal set => this.minimumY = value;
  }

  public SparklineBase Sparkline
  {
    get => this.sparkline;
    internal set => this.sparkline = value;
  }

  public int DataCount
  {
    get => this.dataCount;
    internal set => this.dataCount = value;
  }

  public double MinimumX
  {
    get => this.minimumX;
    internal set => this.minimumX = value;
  }

  internal void SetData(SparklineBase sparkline, int count)
  {
    this.minimumY = sparkline.minYValue;
    this.maximumY = sparkline.maxYValue;
    this.dataCount = count;
    this.sparkline = sparkline;
    this.minimumX = sparkline.minXValue;
  }

  protected internal virtual DataTemplate SelectTemplate(double x, double y) => (DataTemplate) null;
}
