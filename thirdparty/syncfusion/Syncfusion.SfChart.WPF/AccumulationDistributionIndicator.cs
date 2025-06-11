// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AccumulationDistributionIndicator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class AccumulationDistributionIndicator : FinancialTechnicalIndicator
{
  private IList<double> _closeValues = (IList<double>) new List<double>();
  private IList<double> _highValues = (IList<double>) new List<double>();
  private IList<double> _lowValues = (IList<double>) new List<double>();
  private IList<double> _volumeValues = (IList<double>) new List<double>();
  private List<double> _xValues;
  private readonly List<double> _xPoints = new List<double>();
  private readonly List<double> _yPoints = new List<double>();
  private TechnicalIndicatorSegment _fastLineSegment;
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (AccumulationDistributionIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(AccumulationDistributionIndicator.OnColorChanged)));

  internal override bool IsMultipleYPathRequired => true;

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(AccumulationDistributionIndicator.SignalLineColorProperty);
    set => this.SetValue(AccumulationDistributionIndicator.SignalLineColorProperty, (object) value);
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is AccumulationDistributionIndicator distributionIndicator))
      return;
    distributionIndicator.UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this._fastLineSegment = (TechnicalIndicatorSegment) null;
    this._closeValues.Clear();
    this._highValues.Clear();
    this._lowValues.Clear();
    this._volumeValues.Clear();
    this.GeneratePoints(new string[4]
    {
      this.High,
      this.Low,
      this.Close,
      this.Volume
    }, this._highValues, this._lowValues, this._closeValues, this._volumeValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this._fastLineSegment = (TechnicalIndicatorSegment) null;
    this._closeValues.Clear();
    this._highValues.Clear();
    this._lowValues.Clear();
    this._volumeValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 3)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this._highValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this._lowValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[1]);
    this._closeValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[2]);
    this._volumeValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[3]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[4]
    {
      this.High,
      this.Low,
      this.Close,
      this.Volume
    }, this._highValues, this._lowValues, this._closeValues, this._volumeValues);
  }

  public override void CreateSegments()
  {
    this._xValues = this.GetXValues();
    this.AddPoints(this._xPoints, this._yPoints);
    if (this._fastLineSegment == null)
    {
      this._fastLineSegment = new TechnicalIndicatorSegment(this._xPoints, this._yPoints, this.SignalLineColor, (ChartSeriesBase) this);
      this._fastLineSegment.SetValues(this._xPoints, this._yPoints, this.SignalLineColor, (ChartSeriesBase) this);
      this.Segments.Add((ChartSegment) this._fastLineSegment);
    }
    else
    {
      this._fastLineSegment.SetData((IList<double>) this._xPoints, (IList<double>) this._yPoints, this.SignalLineColor);
      this._fastLineSegment.SetRange();
    }
  }

  private void AddPoints(List<double> xPoints, List<double> yPoints)
  {
    xPoints.Clear();
    yPoints.Clear();
    double num = 0.0;
    for (int index = 0; index < this.DataCount; ++index)
    {
      double closeValue = this._closeValues[index];
      if (closeValue != 0.0)
        num += this._volumeValues[index] * ((closeValue - this._lowValues[index] - (this._highValues[index] - closeValue)) / (this._highValues[index] - this._lowValues[index]));
      xPoints.Add(this._xValues[index]);
      yPoints.Add(num);
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new AccumulationDistributionIndicator()
    {
      SignalLineColor = this.SignalLineColor
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
