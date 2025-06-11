// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XyDataSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class XyDataSeries3D : CartesianSeries3D
{
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (XyDataSeries3D), new PropertyMetadata((object) null, new PropertyChangedCallback(XyDataSeries3D.OnYBindingPathChanged)));

  protected XyDataSeries3D() => this.YValues = (IList<double>) new List<double>();

  public string YBindingPath
  {
    get => (string) this.GetValue(XyDataSeries3D.YBindingPathProperty);
    set => this.SetValue(XyDataSeries3D.YBindingPathProperty, (object) value);
  }

  protected internal IList<double> YValues { get; set; }

  public double GetPointToValue(ChartAxis axis, Point point)
  {
    if (axis == null)
      return double.NaN;
    return axis.Orientation == Orientation.Horizontal ? axis.CoefficientToValueCalc((point.X - (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)) / axis.RenderedRect.Width) : axis.CoefficientToValueCalc(1.0 - (point.Y - (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top)) / axis.RenderedRect.Height);
  }

  internal void OnLabelPropertyChanged()
  {
    ChartBase actualArea = this.ActualArea;
    if (actualArea == null || actualArea.InternalDepthAxis == null || !(actualArea.InternalDepthAxis as ChartAxisBase3D).IsManhattanAxis)
      return;
    actualArea.ScheduleUpdate();
  }

  internal DoubleRange GetSegmentDepth(double depth)
  {
    double num1 = depth;
    double start;
    double end;
    if (this.Area.SideBySideSeriesPlacement && this.IsSideBySide)
    {
      double num2 = num1 / 4.0;
      start = num2;
      end = num2 * 3.0;
    }
    else
    {
      int num3 = this.Area.VisibleSeries.IndexOf((ChartSeriesBase) this);
      int count = this.Area.VisibleSeries.Count;
      double num4 = num1 / (double) (count * 2 + count + 1);
      start = num4 + num4 * (double) num3 * 3.0;
      end = start + num4 * 2.0;
    }
    return new DoubleRange(start, end);
  }

  internal override void ValidateYValues()
  {
    foreach (double yvalue in (IEnumerable<double>) this.YValues)
    {
      if (double.IsNaN(yvalue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.YValues);
        break;
      }
    }
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndex)
  {
    foreach (List<int> intList in emptyPointIndex)
    {
      foreach (int index in intList)
        this.YValues[index] = double.NaN;
    }
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.YValues.Clear();
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.YValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (obj is XyDataSeries xyDataSeries)
      xyDataSeries.YBindingPath = this.YBindingPath;
    return base.CloneSeries(obj);
  }

  private static void OnYBindingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is XyDataSeries3D xyDataSeries3D))
      return;
    xyDataSeries3D.OnBindingPathChanged(e);
  }
}
