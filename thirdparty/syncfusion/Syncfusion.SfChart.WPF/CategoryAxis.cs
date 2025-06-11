// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CategoryAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class CategoryAxis : ChartAxisBase2D
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double?), typeof (CategoryAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(CategoryAxis.OnIntervalChanged)));
  public static readonly DependencyProperty LabelPlacementProperty = DependencyProperty.Register(nameof (LabelPlacement), typeof (LabelPlacement), typeof (CategoryAxis), new PropertyMetadata((object) LabelPlacement.OnTicks, new PropertyChangedCallback(CategoryAxis.OnIntervalChanged)));
  public static readonly DependencyProperty IsIndexedProperty = DependencyProperty.Register(nameof (IsIndexed), typeof (bool), typeof (CategoryAxis), new PropertyMetadata((object) true, new PropertyChangedCallback(CategoryAxis.OnIntervalChanged)));
  public static readonly DependencyProperty AggregateFunctionsProperty = DependencyProperty.Register(nameof (AggregateFunctions), typeof (AggregateFunctions), typeof (CategoryAxis), new PropertyMetadata((object) AggregateFunctions.None, new PropertyChangedCallback(CategoryAxis.OnIntervalChanged)));

  public double? Interval
  {
    get => (double?) this.GetValue(CategoryAxis.IntervalProperty);
    set => this.SetValue(CategoryAxis.IntervalProperty, (object) value);
  }

  public LabelPlacement LabelPlacement
  {
    get => (LabelPlacement) this.GetValue(CategoryAxis.LabelPlacementProperty);
    set => this.SetValue(CategoryAxis.LabelPlacementProperty, (object) value);
  }

  public bool IsIndexed
  {
    get => (bool) this.GetValue(CategoryAxis.IsIndexedProperty);
    set => this.SetValue(CategoryAxis.IsIndexedProperty, (object) value);
  }

  public AggregateFunctions AggregateFunctions
  {
    get => (AggregateFunctions) this.GetValue(CategoryAxis.AggregateFunctionsProperty);
    set => this.SetValue(CategoryAxis.AggregateFunctionsProperty, (object) value);
  }

  public override object GetLabelContent(double position)
  {
    return CategoryAxisHelper.GetLabelContent((ChartAxis) this, position);
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return CategoryAxisHelper.CalculateActualInterval((ChartAxis) this, range, availableSize, (object) this.Interval);
  }

  protected virtual void OnIntervalChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    return CategoryAxisHelper.ApplyRangePadding((ChartAxis) this, range, interval, this.LabelPlacement);
  }

  protected override void GenerateVisibleLabels()
  {
    this.SetRangeForAxisStyle();
    CategoryAxisHelper.GenerateVisibleLabels((ChartAxis) this, this.LabelPlacement);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    obj = (DependencyObject) new CategoryAxis()
    {
      Interval = this.Interval,
      LabelPlacement = this.LabelPlacement,
      IsIndexed = this.IsIndexed,
      AggregateFunctions = this.AggregateFunctions
    };
    return base.CloneAxis(obj);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as CategoryAxis).OnIntervalChanged(e);
  }
}
