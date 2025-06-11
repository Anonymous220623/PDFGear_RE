// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FinancialTechnicalIndicator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class FinancialTechnicalIndicator : ChartSeries, ISupportAxes2D, ISupportAxes
{
  public static readonly DependencyProperty ShowTrackballInfoProperty = DependencyProperty.Register(nameof (ShowTrackballInfo), typeof (bool), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) false));
  public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (ChartAxisBase2D), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnXAxisChanged)));
  public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (RangeAxisBase), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnYAxisChanged)));
  public static readonly DependencyProperty IsTransposedProperty = DependencyProperty.Register(nameof (IsTransposed), typeof (bool), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) false, new PropertyChangedCallback(FinancialTechnicalIndicator.OnTransposeChanged)));
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnCustomTemplateChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (FinancialTechnicalIndicator), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SeriesNameProperty = DependencyProperty.Register(nameof (SeriesName), typeof (string), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty HighProperty = DependencyProperty.Register(nameof (High), typeof (string), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnYPathChanged)));
  public static readonly DependencyProperty LowProperty = DependencyProperty.Register(nameof (Low), typeof (string), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnYPathChanged)));
  public static readonly DependencyProperty OpenProperty = DependencyProperty.Register(nameof (Open), typeof (string), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnYPathChanged)));
  public static readonly DependencyProperty CloseProperty = DependencyProperty.Register(nameof (Close), typeof (string), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnYPathChanged)));
  public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(nameof (Volume), typeof (string), typeof (FinancialTechnicalIndicator), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialTechnicalIndicator.OnYPathChanged)));

  public DoubleRange XRange { get; internal set; }

  public DoubleRange YRange { get; internal set; }

  public bool ShowTrackballInfo
  {
    get => (bool) this.GetValue(FinancialTechnicalIndicator.ShowTrackballInfoProperty);
    set => this.SetValue(FinancialTechnicalIndicator.ShowTrackballInfoProperty, (object) value);
  }

  public ChartAxisBase2D XAxis
  {
    get => (ChartAxisBase2D) this.GetValue(FinancialTechnicalIndicator.XAxisProperty);
    set => this.SetValue(FinancialTechnicalIndicator.XAxisProperty, (object) value);
  }

  public RangeAxisBase YAxis
  {
    get => (RangeAxisBase) this.GetValue(FinancialTechnicalIndicator.YAxisProperty);
    set => this.SetValue(FinancialTechnicalIndicator.YAxisProperty, (object) value);
  }

  ChartAxis ISupportAxes.ActualXAxis => this.ActualXAxis;

  ChartAxis ISupportAxes.ActualYAxis => this.ActualYAxis;

  public bool IsTransposed
  {
    get => (bool) this.GetValue(FinancialTechnicalIndicator.IsTransposedProperty);
    set => this.SetValue(FinancialTechnicalIndicator.IsTransposedProperty, (object) value);
  }

  private static void OnTransposeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as FinancialTechnicalIndicator).OnTransposeChanged(Convert.ToBoolean(e.NewValue));
  }

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(FinancialTechnicalIndicator.CustomTemplateProperty);
    set => this.SetValue(FinancialTechnicalIndicator.CustomTemplateProperty, (object) value);
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FinancialTechnicalIndicator technicalIndicator = d as FinancialTechnicalIndicator;
    if (technicalIndicator.Area == null)
      return;
    technicalIndicator.Segments.Clear();
    technicalIndicator.Area.ScheduleUpdate();
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(FinancialTechnicalIndicator.StrokeDashArrayProperty);
    set => this.SetValue(FinancialTechnicalIndicator.StrokeDashArrayProperty, (object) value);
  }

  public string SeriesName
  {
    get => (string) this.GetValue(FinancialTechnicalIndicator.SeriesNameProperty);
    set => this.SetValue(FinancialTechnicalIndicator.SeriesNameProperty, (object) value);
  }

  public string High
  {
    get => (string) this.GetValue(FinancialTechnicalIndicator.HighProperty);
    set => this.SetValue(FinancialTechnicalIndicator.HighProperty, (object) value);
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as FinancialTechnicalIndicator).OnBindingPathChanged(e);
  }

  public string Low
  {
    get => (string) this.GetValue(FinancialTechnicalIndicator.LowProperty);
    set => this.SetValue(FinancialTechnicalIndicator.LowProperty, (object) value);
  }

  public string Open
  {
    get => (string) this.GetValue(FinancialTechnicalIndicator.OpenProperty);
    set => this.SetValue(FinancialTechnicalIndicator.OpenProperty, (object) value);
  }

  public string Close
  {
    get => (string) this.GetValue(FinancialTechnicalIndicator.CloseProperty);
    set => this.SetValue(FinancialTechnicalIndicator.CloseProperty, (object) value);
  }

  public string Volume
  {
    get => (string) this.GetValue(FinancialTechnicalIndicator.VolumeProperty);
    set => this.SetValue(FinancialTechnicalIndicator.VolumeProperty, (object) value);
  }

  public override void CreateSegments()
  {
  }

  private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FinancialTechnicalIndicator technicalIndicator))
      return;
    technicalIndicator.OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FinancialTechnicalIndicator technicalIndicator))
      return;
    technicalIndicator.OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  internal virtual void SetIndicatorInfo(
    ChartPointInfo info,
    List<double> yValue,
    bool seriesPalette)
  {
    if (yValue.Count <= 0)
      return;
    info.SignalLine = double.IsNaN(yValue[0]) ? "null" : Math.Round(yValue[0], 2).ToString();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.ActualXValues = (IEnumerable) null;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (this.XAxis != null && this.Area != null && this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis.AssociatedAxes.Contains((ChartAxis) this.XAxis))
    {
      this.Area.InternalSecondaryAxis.AssociatedAxes.Remove((ChartAxis) this.XAxis);
      if (this.XAxis.AssociatedAxes.Contains(this.Area.InternalSecondaryAxis))
        this.XAxis.AssociatedAxes.Remove(this.Area.InternalSecondaryAxis);
    }
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis))
        this.Area.Axes.Remove(oldAxis);
    }
    else if (this.ActualArea != null && this.ActualArea.InternalSecondaryAxis != null && this.ActualArea.InternalSecondaryAxis.RegisteredSeries.Contains((ISupportAxes) this))
      this.ActualArea.InternalSecondaryAxis.RegisteredSeries.Remove((ISupportAxes) this);
    if (newAxis != null && !newAxis.RegisteredSeries.Contains((ISupportAxes) this) && this.Area != null && !this.Area.Axes.Contains(newAxis))
    {
      this.Area.Axes.Add(newAxis);
      newAxis.Area = (ChartBase) this.Area;
      newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (newAxis != null)
      newAxis.Orientation = this.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (this.YAxis != null && this.Area != null && this.Area.InternalPrimaryAxis != null && this.Area.InternalPrimaryAxis.AssociatedAxes.Contains((ChartAxis) this.YAxis))
    {
      this.Area.InternalPrimaryAxis.AssociatedAxes.Remove((ChartAxis) this.YAxis);
      if (this.YAxis.AssociatedAxes.Contains(this.Area.InternalPrimaryAxis))
        this.YAxis.AssociatedAxes.Remove(this.Area.InternalPrimaryAxis);
    }
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis))
        this.Area.Axes.Remove(oldAxis);
    }
    else if (this.ActualArea != null && this.ActualArea.InternalPrimaryAxis != null && this.ActualArea.InternalPrimaryAxis.RegisteredSeries.Contains((ISupportAxes) this))
      this.ActualArea.InternalPrimaryAxis.RegisteredSeries.Remove((ISupportAxes) this);
    if (newAxis != null && this.Area != null && !this.Area.Axes.Contains(newAxis))
    {
      this.Area.Axes.Add(newAxis);
      newAxis.Area = (ChartBase) this.Area;
      newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (newAxis != null)
      newAxis.Orientation = this.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
  }

  protected internal virtual void SetSeriesItemSource(ChartSeriesBase series)
  {
  }

  protected internal override void GeneratePoints() => throw new NotImplementedException();

  internal static void ComputeMovingAverage(
    int len,
    List<double> xValues,
    IList<double> yValues,
    List<double> xPoints,
    List<double> yPoints)
  {
    double num = 0.0;
    if (len > yValues.Count - 1)
      return;
    xPoints.Clear();
    yPoints.Clear();
    double yValue = yValues[len - 1];
    double xValue = xValues[len - 1];
    int count = xValues.Count;
    for (int index = 0; index < count; ++index)
    {
      xPoints.Add(0.0);
      yPoints.Add(0.0);
      if (index >= len - 1 && index < count)
      {
        if (index - len >= 0)
          num += yValues[index] - yValues[index - len];
        else
          num += yValues[index];
        xPoints[index] = xValues[index];
        yPoints[index] = num / (double) len;
      }
      else
      {
        if (index < len - 1)
          num += yValues[index];
        xPoints[index] = xValue;
        yPoints[index] = yValue;
      }
    }
  }

  internal static void ComputeMovingAverage(
    double len,
    List<double> xValues,
    IList<double> yValues,
    List<double> xPoints,
    List<double> yPoints)
  {
    xPoints.Clear();
    yPoints.Clear();
    double num1 = 0.0;
    int num2 = xValues.Count<double>();
    int num3 = (int) len;
    for (int index = 0; index < num2; ++index)
    {
      xPoints.Add(0.0);
      yPoints.Add(0.0);
      if (index >= num3 - 1 && index < num2)
      {
        if ((double) index - len >= 0.0)
          num1 += yValues[index] - yValues[index - num3];
        else
          num1 += yValues[index];
        xPoints[index] = xValues[index];
        yPoints[index] = num1 / len;
      }
      else if ((double) index < len - 1.0)
        num1 += yValues[index];
    }
  }

  internal override void UpdateRange()
  {
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      this.XRange += segment.XRange;
      this.YRange += segment.YRange;
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    FinancialTechnicalIndicator technicalIndicator = obj as FinancialTechnicalIndicator;
    if (this.XAxis != null && this.XAxis != this.Area.InternalPrimaryAxis)
      technicalIndicator.XAxis = (ChartAxisBase2D) this.XAxis.Clone();
    if (this.YAxis != null && this.YAxis != this.Area.InternalSecondaryAxis)
      technicalIndicator.YAxis = (RangeAxisBase) this.YAxis.Clone();
    technicalIndicator.High = this.High;
    technicalIndicator.Low = this.Low;
    technicalIndicator.Open = this.Open;
    technicalIndicator.Close = this.Close;
    technicalIndicator.Volume = this.Volume;
    return base.CloneSeries((DependencyObject) technicalIndicator);
  }
}
