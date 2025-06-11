// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PieSeries
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
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PieSeries : CircularSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty PieCoefficientProperty = DependencyProperty.Register(nameof (PieCoefficient), typeof (double), typeof (PieSeries), new PropertyMetadata((object) 0.8, new PropertyChangedCallback(PieSeries.OnPieCoefficientPropertyChanged)));
  internal double InternalPieCoefficient = 0.8;
  private Storyboard sb;
  private double grandTotal;
  private double ARCLENGTH;
  private double arcStartAngle;
  private double arcEndAngle;
  private int animateCount;

  public PieSeries() => this.DefaultStyleKey = (object) typeof (PieSeries);

  public double PieCoefficient
  {
    get => (double) this.GetValue(PieSeries.PieCoefficientProperty);
    set => this.SetValue(PieSeries.PieCoefficientProperty, (object) value);
  }

  public override void CreateSegments()
  {
    List<object> actualData = this.ActualData;
    IList<double> source;
    List<double> xValues;
    if (double.IsNaN(this.GroupTo))
    {
      source = this.ToggledLegendIndex.Count <= 0 ? this.YValues : (IList<double>) this.GetYValues();
      xValues = this.GetXValues();
    }
    else
    {
      if (this.Adornments != null)
        this.Adornments.Clear();
      if (this.Segments != null)
        this.Segments.Clear();
      double sumOfYValues = this.YValues.Select<double, double>((Func<double, double>) (val => val <= 0.0 ? Math.Abs(double.IsNaN(val) ? 0.0 : val) : val)).Sum();
      double xIndexValues = 0.0;
      xValues = this.YValues.Where<double>((Func<double, bool>) (val => this.GetGroupModeValue(val, sumOfYValues) > this.GroupTo)).Select<double, double>((Func<double, double>) (val => xIndexValues++)).ToList<double>();
      if (this.YValues.Count > xValues.Count)
        xValues.Add(xIndexValues);
      Tuple<List<double>, List<object>> groupToYvalues = this.GetGroupToYValues();
      actualData = groupToYvalues.Item2;
      source = this.ToggledLegendIndex.Count <= 0 ? (IList<double>) groupToYvalues.Item1 : (IList<double>) this.GetToggleYValues(groupToYvalues.Item1);
    }
    this.ClearUnUsedAdornments(this.DataCount);
    this.ClearUnUsedSegments(this.DataCount);
    int explodeIndex = this.ExplodeIndex;
    bool explodeAll = this.ExplodeAll;
    this.arcStartAngle = this.DegreeToRadianConverter(this.StartAngle);
    this.arcEndAngle = this.DegreeToRadianConverter(this.EndAngle);
    if (this.arcStartAngle == this.arcEndAngle)
      this.Segments.Clear();
    this.ARCLENGTH = this.arcEndAngle - this.arcStartAngle;
    if (Math.Abs(Math.Round(this.ARCLENGTH, 2)) > 2.0 * Math.PI)
      this.ARCLENGTH %= 2.0 * Math.PI;
    if (xValues == null)
      return;
    this.grandTotal = source.Select<double, double>((Func<double, double>) (val => val <= 0.0 ? Math.Abs(double.IsNaN(val) ? 0.0 : val) : val)).Sum();
    for (int index = 0; index < xValues.Count; ++index)
    {
      this.arcEndAngle = this.grandTotal == 0.0 ? 0.0 : Math.Abs(double.IsNaN(source[index]) ? 0.0 : source[index]) * (this.ARCLENGTH / this.grandTotal);
      if (index < this.Segments.Count)
      {
        (this.Segments[index] as PieSegment).SetData(this.arcStartAngle, this.arcStartAngle + this.arcEndAngle, this, actualData[index]);
        (this.Segments[index] as PieSegment).XData = xValues[index];
        (this.Segments[index] as PieSegment).YData = !double.IsNaN(this.GroupTo) ? Math.Abs(source[index]) : Math.Abs(this.YValues[index]);
        (this.Segments[index] as PieSegment).AngleOfSlice = (2.0 * this.arcStartAngle + this.arcEndAngle) / 2.0;
        (this.Segments[index] as PieSegment).IsExploded = explodeAll || explodeIndex == index;
        (this.Segments[index] as PieSegment).Item = actualData[index];
        if (this.SegmentColorPath != null && !this.Segments[index].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index].IsSelectedSegment)
          this.Segments[index].Interior = this.Interior != null ? this.Interior : this.ColorValues[index];
        if (this.ToggledLegendIndex.Contains(index))
          this.Segments[index].IsSegmentVisible = false;
        else
          this.Segments[index].IsSegmentVisible = true;
      }
      else if (this.CreateSegment() is PieSegment segment)
      {
        segment.SetData(this.arcStartAngle, this.arcStartAngle + this.arcEndAngle, this, actualData[index]);
        segment.XData = xValues[index];
        segment.YData = !double.IsNaN(this.GroupTo) ? Math.Abs(source[index]) : Math.Abs(this.YValues[index]);
        segment.AngleOfSlice = (2.0 * this.arcStartAngle + this.arcEndAngle) / 2.0;
        segment.IsExploded = explodeAll || explodeIndex == index;
        segment.Item = actualData[index];
        if (this.ToggledLegendIndex.Contains(index))
          segment.IsSegmentVisible = false;
        else
          segment.IsSegmentVisible = true;
        this.Segments.Add((ChartSegment) segment);
      }
      if (this.AdornmentsInfo != null)
        this.AddPieAdornments(xValues[index], source[index], this.arcStartAngle, this.arcStartAngle + this.arcEndAngle, index);
      this.arcStartAngle += this.arcEndAngle;
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, false);
  }

  internal override void UpdateEmptyPointSegments(List<double> xValues, bool isSidebySideSeries)
  {
    if (this.EmptyPointIndexes == null)
      return;
    foreach (int index in this.EmptyPointIndexes[0])
    {
      PieSegment segment = this.Segments[index] as PieSegment;
      bool isExploded = segment.IsExploded;
      this.Segments[index].IsEmptySegmentInterior = true;
      (this.Segments[index] as PieSegment).AngleOfSlice = segment.AngleOfSlice;
      (this.Segments[index] as PieSegment).IsExploded = isExploded;
      if (this.Adornments.Count > 0)
        this.Adornments[index].IsEmptySegmentInterior = true;
    }
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    if (this.Segments.Count <= 0)
      return;
    double radianConverter = this.DegreeToRadianConverter(this.StartAngle);
    if (this.sb != null && this.animateCount <= this.Segments.Count)
      this.sb = new Storyboard();
    else if (this.sb != null)
    {
      this.sb.Stop();
      if (!this.canAnimate)
      {
        foreach (PieSegment segment in (Collection<ChartSegment>) this.Segments)
        {
          if (segment.EndAngle - segment.StartAngle != 0.0)
          {
            (segment.GetRenderedVisual() as FrameworkElement).ClearValue(UIElement.RenderTransformProperty);
            segment.ActualStartAngle = segment.StartAngle;
            segment.ActualEndAngle = segment.EndAngle;
          }
        }
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    else
      this.sb = new Storyboard();
    this.AnimateAdornments(this.sb);
    foreach (PieSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      ++this.animateCount;
      double startAngle = segment.StartAngle;
      double endAngle = segment.EndAngle;
      if (endAngle - startAngle != 0.0)
      {
        FrameworkElement renderedVisual = segment.GetRenderedVisual() as FrameworkElement;
        renderedVisual.Width = this.DesiredSize.Width;
        renderedVisual.Height = this.DesiredSize.Height;
        renderedVisual.RenderTransform = (Transform) new ScaleTransform();
        renderedVisual.RenderTransformOrigin = new Point(0.5, 0.5);
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame1.Value = radianConverter;
        SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame2.Value = startAngle;
        SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
        keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
        keyFrame2.KeySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) PieSegment.ActualStartAngleProperty));
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) segment);
        this.sb.Children.Add((Timeline) element1);
        DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame3 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame3.Value = radianConverter;
        SplineDoubleKeyFrame keyFrame3 = splineDoubleKeyFrame3;
        keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
        SplineDoubleKeyFrame splineDoubleKeyFrame4 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame4.Value = endAngle;
        SplineDoubleKeyFrame keyFrame4 = splineDoubleKeyFrame4;
        keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(this.AnimationDuration);
        KeySpline keySpline1 = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        keyFrame4.KeySpline = keySpline1;
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) PieSegment.ActualEndAngleProperty));
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) segment);
        this.sb.Children.Add((Timeline) element2);
        DoubleAnimationUsingKeyFrames element3 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame splineDoubleKeyFrame5 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame5.Value = 0.0;
        SplineDoubleKeyFrame keyFrame5 = splineDoubleKeyFrame5;
        keyFrame5.KeyTime = keyFrame5.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        element3.KeyFrames.Add((DoubleKeyFrame) keyFrame5);
        SplineDoubleKeyFrame splineDoubleKeyFrame6 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame6.Value = 1.0;
        SplineDoubleKeyFrame keyFrame6 = splineDoubleKeyFrame6;
        keyFrame6.KeyTime = keyFrame6.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
        KeySpline keySpline2 = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        keyFrame6.KeySpline = keySpline2;
        element3.KeyFrames.Add((DoubleKeyFrame) keyFrame6);
        Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", new object[0]));
        Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) renderedVisual);
        this.sb.Children.Add((Timeline) element3);
        DoubleAnimationUsingKeyFrames element4 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame keyFrame7 = new SplineDoubleKeyFrame();
        keyFrame7.KeyTime = keyFrame7.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        keyFrame7.Value = 0.0;
        element4.KeyFrames.Add((DoubleKeyFrame) keyFrame7);
        SplineDoubleKeyFrame splineDoubleKeyFrame7 = new SplineDoubleKeyFrame();
        splineDoubleKeyFrame7.Value = 1.0;
        SplineDoubleKeyFrame keyFrame8 = splineDoubleKeyFrame7;
        keyFrame8.KeyTime = keyFrame8.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
        KeySpline keySpline3 = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        keyFrame8.KeySpline = keySpline3;
        element4.KeyFrames.Add((DoubleKeyFrame) keyFrame8);
        Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", new object[0]));
        Storyboard.SetTarget((DependencyObject) element4, (DependencyObject) renderedVisual);
        this.sb.Children.Add((Timeline) element4);
      }
    }
    this.sb.Begin();
  }

  internal override void Dispose()
  {
    if (this.sb != null)
    {
      this.sb.Stop();
      this.sb.Children.Clear();
      this.sb = (Storyboard) null;
    }
    base.Dispose();
  }

  internal int GetPieSeriesCount()
  {
    return this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is PieSeries)).ToList<ChartSeriesBase>().Count;
  }

  protected internal override IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreateSimple(size);
    return this.ChartTransformer;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new PieSegment();

  protected override ChartAdornment CreateAdornment(
    AdornmentSeries series,
    double xVal,
    double yVal,
    double angle,
    double radius)
  {
    ChartPieAdornment adornment = new ChartPieAdornment(xVal, yVal, angle, radius, series);
    adornment.SetValues(xVal, yVal, angle, radius, series);
    return (ChartAdornment) adornment;
  }

  protected override void SetExplodeIndex(int i)
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (PieSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      int num = !double.IsNaN(this.GroupTo) ? this.Segments.IndexOf((ChartSegment) segment) : this.ActualData.IndexOf(segment.Item);
      if (i == num)
      {
        segment.IsExploded = !segment.IsExploded;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
      else if (i == -1)
      {
        segment.IsExploded = false;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
    }
  }

  protected override void SetExplodeRadius()
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      this.UpdateSegments(this.Segments.IndexOf(segment), NotifyCollectionChangedAction.Replace);
  }

  protected override void SetExplodeAll()
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (PieSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      int index = this.Segments.IndexOf((ChartSegment) segment);
      segment.IsExploded = true;
      this.UpdateSegments(index, NotifyCollectionChangedAction.Replace);
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new PieSeries()
    {
      PieCoefficient = this.PieCoefficient
    });
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (oldValue != null)
      this.animateCount = 0;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    PieSegment toolTipTag = this.ToolTipTag as PieSegment;
    Point dataPointPosition = new Point();
    double angleOfSlice = toolTipTag.AngleOfSlice;
    double num = toolTipTag.IsExploded ? this.ExplodeRadius + this.Radius / 2.0 : this.Radius / 2.0;
    dataPointPosition.X = this.Center.X + Math.Cos(angleOfSlice) * num;
    dataPointPosition.Y = this.Center.Y + Math.Sin(angleOfSlice) * num;
    return dataPointPosition;
  }

  private static void OnPieCoefficientPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PieSeries pieSeries = d as PieSeries;
    pieSeries.InternalPieCoefficient = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    pieSeries.UpdateArea();
  }

  private void AddPieAdornments(
    double x,
    double y,
    double startAngle,
    double endAngle,
    int index)
  {
    double xPos = (startAngle + endAngle) / 2.0;
    if (this.Area == null || !this.Area.RootPanelDesiredSize.HasValue)
      return;
    Size size = this.Area.RootPanelDesiredSize.Value;
    double height = this.Area.RootPanelDesiredSize.Value.Height;
    double width = this.Area.RootPanelDesiredSize.Value.Width;
    List<ChartSeriesBase> list = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is PieSeries)).ToList<ChartSeriesBase>();
    int num1 = list.Count<ChartSeriesBase>();
    double num2 = (double) list.IndexOf((ChartSeriesBase) this);
    double num3 = Math.Min(width, height) / 2.0 / (double) num1;
    double yPos = num3 * (num2 + 1.0) - num3 * (1.0 - this.InternalPieCoefficient);
    if (index < this.Adornments.Count)
      (this.Adornments[index] as ChartPieAdornment).SetData(x, y, xPos, yPos);
    else
      this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, x, y, xPos, yPos));
    this.Adornments[index].Item = !double.IsNaN(this.GroupTo) ? this.Segments[index].Item : this.ActualData[index];
  }
}
