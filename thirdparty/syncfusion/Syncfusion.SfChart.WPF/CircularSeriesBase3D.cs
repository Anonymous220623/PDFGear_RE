// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CircularSeriesBase3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class CircularSeriesBase3D : ChartSeries3D
{
  public static readonly DependencyProperty ConnectorTypeProperty = DependencyProperty.Register(nameof (ConnectorType), typeof (ConnectorMode), typeof (CircularSeriesBase3D), new PropertyMetadata((object) ConnectorMode.Line, new PropertyChangedCallback(CircularSeriesBase3D.OnPropertyChanged)));
  public static readonly DependencyProperty EnableSmartLabelsProperty = DependencyProperty.Register(nameof (EnableSmartLabels), typeof (bool), typeof (CircularSeriesBase3D), new PropertyMetadata((object) false, new PropertyChangedCallback(CircularSeriesBase3D.OnPropertyChanged)));
  public static readonly DependencyProperty CircleCoefficientProperty = DependencyProperty.Register(nameof (CircleCoefficient), typeof (double), typeof (CircularSeriesBase3D), new PropertyMetadata((object) 0.8, new PropertyChangedCallback(CircularSeriesBase3D.OnCircleCoefficientChanged)));
  public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register(nameof (LabelPosition), typeof (CircularSeriesLabelPosition), typeof (CircularSeriesBase3D), new PropertyMetadata((object) CircularSeriesLabelPosition.Inside, new PropertyChangedCallback(CircularSeriesBase3D.OnPropertyChanged)));
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (CircularSeriesBase3D), new PropertyMetadata((object) null, new PropertyChangedCallback(CircularSeriesBase3D.OnYPathChanged)));
  public static readonly DependencyProperty ExplodeRadiusProperty = DependencyProperty.Register(nameof (ExplodeRadius), typeof (double), typeof (CircularSeriesBase3D), new PropertyMetadata((object) 30.0, new PropertyChangedCallback(CircularSeriesBase3D.OnPropertyChanged)));
  public static readonly DependencyProperty ExplodeIndexProperty = DependencyProperty.Register(nameof (ExplodeIndex), typeof (int), typeof (CircularSeriesBase3D), new PropertyMetadata((object) -1, new PropertyChangedCallback(CircularSeriesBase3D.OnPropertyChanged)));
  public static readonly DependencyProperty ExplodeAllProperty = DependencyProperty.Register(nameof (ExplodeAll), typeof (bool), typeof (CircularSeriesBase3D), new PropertyMetadata((object) false, new PropertyChangedCallback(CircularSeriesBase3D.OnPropertyChanged)));
  public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), typeof (double), typeof (CircularSeriesBase3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(CircularSeriesBase3D.OnAngleChanged)));
  public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), typeof (double), typeof (CircularSeriesBase3D), new PropertyMetadata((object) 360.0, new PropertyChangedCallback(CircularSeriesBase3D.OnAngleChanged)));

  protected CircularSeriesBase3D()
  {
    this.InternalCircleCoefficient = 0.8;
    this.YValues = (IList<double>) new List<double>();
  }

  public ConnectorMode ConnectorType
  {
    get => (ConnectorMode) this.GetValue(CircularSeriesBase3D.ConnectorTypeProperty);
    set => this.SetValue(CircularSeriesBase3D.ConnectorTypeProperty, (object) value);
  }

  public bool EnableSmartLabels
  {
    get => (bool) this.GetValue(CircularSeriesBase3D.EnableSmartLabelsProperty);
    set => this.SetValue(CircularSeriesBase3D.EnableSmartLabelsProperty, (object) value);
  }

  public double CircleCoefficient
  {
    get => (double) this.GetValue(CircularSeriesBase3D.CircleCoefficientProperty);
    set => this.SetValue(CircularSeriesBase3D.CircleCoefficientProperty, (object) value);
  }

  public CircularSeriesLabelPosition LabelPosition
  {
    get => (CircularSeriesLabelPosition) this.GetValue(CircularSeriesBase3D.LabelPositionProperty);
    set => this.SetValue(CircularSeriesBase3D.LabelPositionProperty, (object) value);
  }

  public string YBindingPath
  {
    get => (string) this.GetValue(CircularSeriesBase3D.YBindingPathProperty);
    set => this.SetValue(CircularSeriesBase3D.YBindingPathProperty, (object) value);
  }

  public double ExplodeRadius
  {
    get => (double) this.GetValue(CircularSeriesBase3D.ExplodeRadiusProperty);
    set => this.SetValue(CircularSeriesBase3D.ExplodeRadiusProperty, (object) value);
  }

  public int ExplodeIndex
  {
    get => (int) this.GetValue(CircularSeriesBase3D.ExplodeIndexProperty);
    set => this.SetValue(CircularSeriesBase3D.ExplodeIndexProperty, (object) value);
  }

  public bool ExplodeAll
  {
    get => (bool) this.GetValue(CircularSeriesBase3D.ExplodeAllProperty);
    set => this.SetValue(CircularSeriesBase3D.ExplodeAllProperty, (object) value);
  }

  public double StartAngle
  {
    get => (double) this.GetValue(CircularSeriesBase3D.StartAngleProperty);
    set => this.SetValue(CircularSeriesBase3D.StartAngleProperty, (object) value);
  }

  public double EndAngle
  {
    get => (double) this.GetValue(CircularSeriesBase3D.EndAngleProperty);
    set => this.SetValue(CircularSeriesBase3D.EndAngleProperty, (object) value);
  }

  internal double InternalCircleCoefficient { get; set; }

  internal Point Center { get; set; }

  protected internal IList<double> YValues { get; set; }

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

  internal int GetCircularSeriesCount()
  {
    return this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is CircularSeriesBase3D)).ToList<ChartSeriesBase>().Count<ChartSeriesBase>();
  }

  internal int GetPieSeriesIndex()
  {
    ChartVisibleSeriesCollection visibleSeries = this.Area.VisibleSeries;
    Func<ChartSeriesBase, bool> predicate = (Func<ChartSeriesBase, bool>) (series => series is PieSeries3D);
    int num;
    return (num = visibleSeries.Where<ChartSeriesBase>(predicate).ToList<ChartSeriesBase>().IndexOf((ChartSeriesBase) this)) < 0 ? -1 : num;
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndex)
  {
    foreach (List<int> intList in emptyPointIndex)
    {
      foreach (int index in intList)
        this.YValues[index] = double.NaN;
    }
  }

  internal Point GetActualCenter(Point centerPoint, double radius)
  {
    if (this.Area != null && this.Area.Series.IndexOf((ChartSeries3D) this) > 0)
      return centerPoint;
    Point actualCenter = centerPoint;
    double startAngle = this.StartAngle;
    double endAngle = this.EndAngle;
    double[] source1 = new double[15]
    {
      -630.0,
      -540.0,
      -450.0,
      -360.0,
      -270.0,
      -180.0,
      -90.0,
      0.0,
      90.0,
      180.0,
      270.0,
      360.0,
      450.0,
      540.0,
      630.0
    };
    List<int> source2 = new List<int>();
    if (startAngle < endAngle)
    {
      for (int index = 0; index < ((IEnumerable<double>) source1).Count<double>(); ++index)
      {
        if (source1[index] > startAngle && source1[index] < endAngle)
          source2.Add(source1[index] % 360.0 < 0.0 ? (int) (source1[index] % 360.0 + 360.0) : (int) (source1[index] % 360.0));
      }
    }
    else
    {
      for (int index = 0; index < ((IEnumerable<double>) source1).Count<double>(); ++index)
      {
        if (source1[index] < startAngle && source1[index] > endAngle)
          source2.Add(source1[index] % 360.0 < 0.0 ? (int) (source1[index] % 360.0 + 360.0) : (int) (source1[index] % 360.0));
      }
    }
    double num1 = 2.0 * Math.PI * startAngle / 360.0;
    double num2 = 2.0 * Math.PI * endAngle / 360.0;
    Point point1 = new Point(centerPoint.X + radius * Math.Cos(num1), centerPoint.Y + radius * Math.Sin(num1));
    Point point2 = new Point(centerPoint.X + radius * Math.Cos(num2), centerPoint.Y + radius * Math.Sin(num2));
    switch (source2.Count)
    {
      case 0:
        double num3 = Math.Abs(centerPoint.X - point1.X) > Math.Abs(centerPoint.X - point2.X) ? point1.X : point2.X;
        double num4 = Math.Abs(centerPoint.Y - point1.Y) > Math.Abs(centerPoint.Y - point2.Y) ? point1.Y : point2.Y;
        Point point3 = new Point(Math.Abs(centerPoint.X + num3) / 2.0, Math.Abs(centerPoint.Y + num4) / 2.0);
        actualCenter.X = centerPoint.X + (centerPoint.X - point3.X);
        actualCenter.Y = centerPoint.Y + (centerPoint.Y - point3.Y);
        break;
      case 1:
        Point point4 = new Point();
        Point point5 = new Point();
        double num5 = 2.0 * Math.PI * (double) source2[0] / 360.0;
        Point point6 = new Point(centerPoint.X + radius * Math.Cos(num5), centerPoint.Y + radius * Math.Sin(num5));
        switch (source2.ElementAt<int>(0))
        {
          case 0:
          case 360:
            point4 = new Point(centerPoint.X, point2.Y);
            point5 = new Point(point6.X, point1.Y);
            break;
          case 90:
            point4 = new Point(point2.X, centerPoint.Y);
            point5 = new Point(point1.X, point6.Y);
            break;
          case 180:
            point4 = new Point(point6.X, point1.Y);
            point5 = new Point(centerPoint.X, point2.Y);
            break;
          case 270:
            point4 = new Point(point1.X, point6.Y);
            point5 = new Point(point2.X, centerPoint.Y);
            break;
        }
        Point point7 = new Point((point4.X + point5.X) / 2.0, (point4.Y + point5.Y) / 2.0);
        actualCenter.X = centerPoint.X + (centerPoint.X - point7.X >= radius ? 0.0 : centerPoint.X - point7.X);
        actualCenter.Y = centerPoint.Y + (centerPoint.Y - point7.Y >= radius ? 0.0 : centerPoint.Y - point7.Y);
        break;
      case 2:
        double num6 = 2.0 * Math.PI * (double) source2[0] / 360.0;
        double num7 = 2.0 * Math.PI * (double) source2[1] / 360.0;
        Point point8 = new Point(centerPoint.X + radius * Math.Cos(num7), centerPoint.Y + radius * Math.Sin(num7));
        Point point9 = new Point(centerPoint.X + radius * Math.Cos(num6), centerPoint.Y + radius * Math.Sin(num6));
        Point point10 = source2[0] == 0 && source2[1] == 90 || source2[0] == 180 && source2[1] == 270 ? new Point(point9.X, point8.Y) : new Point(point8.X, point9.Y);
        Point point11 = source2[0] == 0 || source2[0] == 180 ? new Point(CircularSeriesBase3D.GetMinMaxValue(point1, point2, source2[0]), CircularSeriesBase3D.GetMinMaxValue(point1, point2, source2[1])) : new Point(CircularSeriesBase3D.GetMinMaxValue(point1, point2, source2[1]), CircularSeriesBase3D.GetMinMaxValue(point1, point2, source2[0]));
        Point point12 = new Point(Math.Abs(point10.X - point11.X) / 2.0 >= radius ? 0.0 : (point10.X + point11.X) / 2.0, Math.Abs(point10.Y - point11.Y) / 2.0 >= radius ? 0.0 : (point10.Y + point11.Y) / 2.0);
        actualCenter.X = centerPoint.X + (point12.X == 0.0 ? 0.0 : (centerPoint.X - point12.X >= radius ? 0.0 : centerPoint.X - point12.X));
        actualCenter.Y = centerPoint.Y + (point12.Y == 0.0 ? 0.0 : (centerPoint.Y - point12.Y >= radius ? 0.0 : centerPoint.Y - point12.Y));
        break;
    }
    return actualCenter;
  }

  protected internal static double DegreeToRadianConverter(double degree)
  {
    return degree * Math.PI / 180.0;
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.YValues.Clear();
    this.Segments.Clear();
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    if (this.Area != null)
      this.Area.IsUpdateLegend = true;
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.YValues.Clear();
    this.Segments.Clear();
    if (this.Area != null)
      this.Area.IsUpdateLegend = true;
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (obj is CircularSeriesBase3D circularSeriesBase3D)
    {
      circularSeriesBase3D.YBindingPath = this.YBindingPath;
      circularSeriesBase3D.StartAngle = this.StartAngle;
      circularSeriesBase3D.EndAngle = this.EndAngle;
      circularSeriesBase3D.ExplodeAll = this.ExplodeAll;
      circularSeriesBase3D.ExplodeIndex = this.ExplodeIndex;
      circularSeriesBase3D.ExplodeRadius = this.ExplodeRadius;
      circularSeriesBase3D.LabelPosition = this.LabelPosition;
      circularSeriesBase3D.ConnectorType = this.ConnectorType;
      circularSeriesBase3D.EnableSmartLabels = this.EnableSmartLabels;
      circularSeriesBase3D.CircleCoefficient = this.CircleCoefficient;
    }
    return base.CloneSeries((DependencyObject) circularSeriesBase3D);
  }

  private static void OnCircleCoefficientChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(sender is CircularSeriesBase3D circularSeriesBase3D))
      return;
    circularSeriesBase3D.InternalCircleCoefficient = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    circularSeriesBase3D.UpdateArea();
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ChartSeriesBase) d).OnBindingPathChanged(e);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
  {
    if (!(d is CircularSeriesBase3D circularSeriesBase3D))
      return;
    circularSeriesBase3D.UpdateArea();
  }

  private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CircularSeriesBase3D circularSeriesBase3D))
      return;
    circularSeriesBase3D.UpdateArea();
  }

  private static double GetMinMaxValue(Point point1, Point point2, int degree)
  {
    double minMaxValue1 = Math.Min(point1.X, point2.Y);
    double minMaxValue2 = Math.Min(point1.Y, point2.Y);
    double minMaxValue3 = Math.Max(point1.X, point2.X);
    double minMaxValue4 = Math.Max(point1.Y, point2.Y);
    switch (degree)
    {
      case 0:
      case 360:
        return minMaxValue1;
      case 90:
        return minMaxValue2;
      case 180:
        return minMaxValue3;
      case 270:
        return minMaxValue4;
      default:
        return 0.0;
    }
  }
}
