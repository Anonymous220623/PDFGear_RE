// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CircularSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class CircularSeriesBase : AccumulationSeriesBase
{
  internal const double TotalArcLength = 6.2831853071795862;
  public static readonly DependencyProperty EnableSmartLabelsProperty = DependencyProperty.Register(nameof (EnableSmartLabels), typeof (bool), typeof (CircularSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(CircularSeriesBase.OnAdornmentPorpertyChanged)));
  public static readonly DependencyProperty ShowMarkerAtLineEndProperty = DependencyProperty.Register(nameof (ShowMarkerAtLineEnd), typeof (bool), typeof (CircularSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(CircularSeriesBase.OnAdornmentPorpertyChanged)));
  public static readonly DependencyProperty ConnectorTypeProperty = DependencyProperty.Register(nameof (ConnectorType), typeof (ConnectorMode), typeof (CircularSeriesBase), new PropertyMetadata((object) ConnectorMode.Line, new PropertyChangedCallback(CircularSeriesBase.OnAdornmentPorpertyChanged)));
  public static readonly DependencyProperty ConnectorLinePositionProperty = DependencyProperty.Register(nameof (ConnectorLinePosition), typeof (ConnectorLinePosition), typeof (CircularSeriesBase), new PropertyMetadata((object) ConnectorLinePosition.Center, new PropertyChangedCallback(CircularSeriesBase.OnAdornmentPorpertyChanged)));
  public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register(nameof (LabelPosition), typeof (CircularSeriesLabelPosition), typeof (CircularSeriesBase), new PropertyMetadata((object) CircularSeriesLabelPosition.Inside, new PropertyChangedCallback(CircularSeriesBase.OnAdornmentPorpertyChanged)));
  public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), typeof (double), typeof (CircularSeriesBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(CircularSeriesBase.OnStartAngleChanged)));
  public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), typeof (double), typeof (CircularSeriesBase), new PropertyMetadata((object) 360.0, new PropertyChangedCallback(CircularSeriesBase.OnStartAngleChanged)));
  public static readonly DependencyProperty ExplodeRadiusProperty = DependencyProperty.Register(nameof (ExplodeRadius), typeof (double), typeof (CircularSeriesBase), new PropertyMetadata((object) 30.0, new PropertyChangedCallback(CircularSeriesBase.OnExplodeRadiusChanged)));
  public static readonly DependencyProperty GroupModeProperty = DependencyProperty.Register(nameof (GroupMode), typeof (PieGroupMode), typeof (CircularSeriesBase), new PropertyMetadata((object) PieGroupMode.Value, new PropertyChangedCallback(CircularSeriesBase.OnGroupToPropertiesChanged)));
  public static readonly DependencyProperty GroupToProperty = DependencyProperty.Register(nameof (GroupTo), typeof (double), typeof (CircularSeriesBase), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(CircularSeriesBase.OnGroupToPropertiesChanged)));
  private string groupingLabel = "Others";
  private List<object> groupedData;

  public bool EnableSmartLabels
  {
    get => (bool) this.GetValue(CircularSeriesBase.EnableSmartLabelsProperty);
    set => this.SetValue(CircularSeriesBase.EnableSmartLabelsProperty, (object) value);
  }

  public bool ShowMarkerAtLineEnd
  {
    get => (bool) this.GetValue(CircularSeriesBase.ShowMarkerAtLineEndProperty);
    set => this.SetValue(CircularSeriesBase.ShowMarkerAtLineEndProperty, (object) value);
  }

  public ConnectorMode ConnectorType
  {
    get => (ConnectorMode) this.GetValue(CircularSeriesBase.ConnectorTypeProperty);
    set => this.SetValue(CircularSeriesBase.ConnectorTypeProperty, (object) value);
  }

  public ConnectorLinePosition ConnectorLinePosition
  {
    get => (ConnectorLinePosition) this.GetValue(CircularSeriesBase.ConnectorLinePositionProperty);
    set => this.SetValue(CircularSeriesBase.ConnectorLinePositionProperty, (object) value);
  }

  public CircularSeriesLabelPosition LabelPosition
  {
    get => (CircularSeriesLabelPosition) this.GetValue(CircularSeriesBase.LabelPositionProperty);
    set => this.SetValue(CircularSeriesBase.LabelPositionProperty, (object) value);
  }

  public double StartAngle
  {
    get => (double) this.GetValue(CircularSeriesBase.StartAngleProperty);
    set => this.SetValue(CircularSeriesBase.StartAngleProperty, (object) value);
  }

  public double EndAngle
  {
    get => (double) this.GetValue(CircularSeriesBase.EndAngleProperty);
    set => this.SetValue(CircularSeriesBase.EndAngleProperty, (object) value);
  }

  public double ExplodeRadius
  {
    get => (double) this.GetValue(CircularSeriesBase.ExplodeRadiusProperty);
    set => this.SetValue(CircularSeriesBase.ExplodeRadiusProperty, (object) value);
  }

  public PieGroupMode GroupMode
  {
    get => (PieGroupMode) this.GetValue(CircularSeriesBase.GroupModeProperty);
    set => this.SetValue(CircularSeriesBase.GroupModeProperty, (object) value);
  }

  public double GroupTo
  {
    get => (double) this.GetValue(CircularSeriesBase.GroupToProperty);
    set => this.SetValue(CircularSeriesBase.GroupToProperty, (object) value);
  }

  internal List<object> GroupedData
  {
    get => this.groupedData;
    set => this.groupedData = value;
  }

  internal double Radius { get; set; }

  internal Point Center { get; set; }

  internal string GroupingLabel
  {
    get => this.groupingLabel;
    set => this.groupingLabel = value;
  }

  internal override void ResetAdornmentAnimationState()
  {
    if (this.adornmentInfo == null)
      return;
    foreach (object child in this.AdornmentPresenter.Children)
      (child as FrameworkElement).ClearValue(UIElement.OpacityProperty);
  }

  internal void AnimateAdornments(Storyboard sb)
  {
    if (this.AdornmentsInfo == null)
      return;
    double totalSeconds = this.AnimationDuration.TotalSeconds;
    foreach (object child in this.AdornmentPresenter.Children)
    {
      DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
      SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
      keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
      keyFrame1.Value = 0.0;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
      SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
      keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalSeconds));
      keyFrame2.Value = 0.0;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
      SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
      keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalSeconds + 1.0));
      keyFrame3.Value = 1.0;
      element.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
      KeySpline keySpline = new KeySpline();
      keySpline.ControlPoint1 = new Point(0.64, 0.84);
      keySpline.ControlPoint2 = new Point(0.67, 0.95);
      Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath("Opacity", new object[0]));
      keyFrame3.KeySpline = keySpline;
      Storyboard.SetTarget((DependencyObject) element, (DependencyObject) (child as FrameworkElement));
      sb.Children.Add((Timeline) element);
    }
  }

  internal Point GetActualCenter(Point centerPoint, double radius)
  {
    if (this.Area != null && this.Area.Series.IndexOf((ChartSeries) this) > 0)
      return centerPoint;
    Point actualCenter = centerPoint;
    double startAngle = this.StartAngle;
    double endAngle = this.EndAngle;
    int length = (Math.Max(Math.Abs((int) startAngle / 90), Math.Abs((int) endAngle / 90)) + 1) * 2 + 1;
    double[] source1 = new double[length];
    int index1 = 0;
    for (int index2 = -(length / 2); index2 < length / 2 + 1; ++index2)
    {
      source1[index1] = (double) (index2 * 90);
      ++index1;
    }
    List<int> source2 = new List<int>();
    if (startAngle < endAngle)
    {
      for (int index3 = 0; index3 < ((IEnumerable<double>) source1).Count<double>(); ++index3)
      {
        if (source1[index3] > startAngle && source1[index3] < endAngle)
          source2.Add(source1[index3] % 360.0 < 0.0 ? (int) (source1[index3] % 360.0 + 360.0) : (int) (source1[index3] % 360.0));
      }
    }
    else
    {
      for (int index4 = 0; index4 < ((IEnumerable<double>) source1).Count<double>(); ++index4)
      {
        if (source1[index4] < startAngle && source1[index4] > endAngle)
          source2.Add(source1[index4] % 360.0 < 0.0 ? (int) (source1[index4] % 360.0 + 360.0) : (int) (source1[index4] % 360.0));
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
        Point point11 = source2[0] == 0 || source2[0] == 180 ? new Point(CircularSeriesBase.GetMinMaxValue(point1, point2, source2[0]), CircularSeriesBase.GetMinMaxValue(point1, point2, source2[1])) : new Point(CircularSeriesBase.GetMinMaxValue(point1, point2, source2[1]), CircularSeriesBase.GetMinMaxValue(point1, point2, source2[0]));
        Point point12 = new Point(Math.Abs(point10.X - point11.X) / 2.0 >= radius ? 0.0 : (point10.X + point11.X) / 2.0, Math.Abs(point10.Y - point11.Y) / 2.0 >= radius ? 0.0 : (point10.Y + point11.Y) / 2.0);
        actualCenter.X = centerPoint.X + (point12.X == 0.0 ? 0.0 : (centerPoint.X - point12.X >= radius ? 0.0 : centerPoint.X - point12.X));
        actualCenter.Y = centerPoint.Y + (point12.Y == 0.0 ? 0.0 : (centerPoint.Y - point12.Y >= radius ? 0.0 : centerPoint.Y - point12.Y));
        break;
    }
    return actualCenter;
  }

  internal Tuple<List<double>, List<object>> GetGroupToYValues()
  {
    List<double> doubleList = new List<double>();
    List<object> objectList = new List<object>();
    this.GroupedData = new List<object>();
    double num = 0.0;
    double sumOfYValues = this.YValues.Select<double, double>((Func<double, double>) (val => val <= 0.0 ? Math.Abs(double.IsNaN(val) ? 0.0 : val) : val)).Sum();
    for (int index = 0; index < this.DataCount; ++index)
    {
      double yvalue = this.YValues[index];
      if (this.GetGroupModeValue(yvalue, sumOfYValues) > this.GroupTo)
      {
        doubleList.Add(yvalue);
        objectList.Add(this.ActualData[index]);
      }
      else if (!double.IsNaN(yvalue))
      {
        num += yvalue;
        this.GroupedData.Add(this.ActualData[index]);
      }
      if (index == this.DataCount - 1 && this.GroupedData.Count > 0)
      {
        doubleList.Add(num);
        objectList.Add((object) this.GroupedData);
      }
    }
    return new Tuple<List<double>, List<object>>(doubleList, objectList);
  }

  internal double GetGroupModeValue(double yValue, double sumOfYValues)
  {
    double groupModeValue = 0.0;
    switch (this.GroupMode)
    {
      case PieGroupMode.Value:
        groupModeValue = yValue;
        break;
      case PieGroupMode.Percentage:
        groupModeValue = (double) ((float) Math.Floor(yValue / sumOfYValues * 100.0 * 100.0) / 100f);
        break;
      case PieGroupMode.Angle:
        double num = this.EndAngle - this.StartAngle;
        if (Math.Abs(Math.Round(num * 100.0) / 100.0) > 360.0)
          num %= 360.0;
        groupModeValue = Math.Abs(double.IsNaN(yValue) ? 0.0 : yValue) * (num / sumOfYValues);
        break;
    }
    return groupModeValue;
  }

  internal List<double> GetToggleYValues(List<double> groupToYValues)
  {
    List<double> toggleYvalues = new List<double>();
    for (int index = 0; index < groupToYValues.Count; ++index)
    {
      double groupToYvalue = groupToYValues[index];
      if (!this.ToggledLegendIndex.Contains(index))
        toggleYvalues.Add(groupToYvalue);
      else
        toggleYvalues.Add(double.NaN);
    }
    return toggleYvalues;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    CircularSeriesBase circularSeriesBase = obj as CircularSeriesBase;
    circularSeriesBase.LabelPosition = this.LabelPosition;
    circularSeriesBase.EnableSmartLabels = this.EnableSmartLabels;
    circularSeriesBase.ConnectorType = this.ConnectorType;
    circularSeriesBase.StartAngle = this.StartAngle;
    circularSeriesBase.EndAngle = this.EndAngle;
    circularSeriesBase.ExplodeRadius = this.ExplodeRadius;
    circularSeriesBase.GroupMode = this.GroupMode;
    circularSeriesBase.GroupTo = this.GroupTo;
    circularSeriesBase.GroupedData = this.GroupedData;
    return base.CloneSeries((DependencyObject) circularSeriesBase);
  }

  protected internal double DegreeToRadianConverter(double degree) => degree * Math.PI / 180.0;

  private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CircularSeriesBase circularSeriesBase))
      return;
    if (!double.IsNaN(circularSeriesBase.GroupTo))
      CircularSeriesBase.OnGroupToPropertiesChanged(d, e);
    else
      circularSeriesBase.UpdateArea();
  }

  private static void OnExplodeRadiusChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CircularSeriesBase circularSeriesBase))
      return;
    circularSeriesBase.SetExplodeRadius();
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

  private static void OnAdornmentPorpertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(d is CircularSeriesBase circularSeriesBase) || circularSeriesBase.adornmentInfo == null)
      return;
    circularSeriesBase.adornmentInfo.OnAdornmentPropertyChanged();
  }

  private static void OnGroupToPropertiesChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CircularSeriesBase circularSeriesBase) || circularSeriesBase.ActualArea == null)
      return;
    circularSeriesBase.ActualArea.IsUpdateLegend = true;
    circularSeriesBase.Segments.Clear();
    circularSeriesBase.UpdateArea();
  }
}
