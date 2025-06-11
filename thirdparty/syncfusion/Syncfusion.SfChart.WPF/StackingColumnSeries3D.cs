// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingColumnSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StackingColumnSeries3D : StackingSeriesBase3D, ISegmentSpacing
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (StackingColumnSeries3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(StackingColumnSeries3D.OnSegmentSpacingChanged)));

  public double SegmentSpacing
  {
    get => (double) this.GetValue(StackingColumnSeries3D.SegmentSpacingProperty);
    set => this.SetValue(StackingColumnSeries3D.SegmentSpacingProperty, (object) value);
  }

  protected internal override bool IsSideBySide => true;

  protected override bool IsStacked => true;

  public override void CreateSegments()
  {
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double origin = this.ActualXAxis.Origin;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      origin = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    double num1 = sideBySideInfo.Delta / 2.0;
    StackingValues cumulativeStackValues = this.GetCumulativeStackValues((ChartSeriesBase) this);
    if (cumulativeStackValues == null)
      return;
    this.YRangeStartValues = cumulativeStackValues.StartValues;
    this.YRangeEndValues = cumulativeStackValues.EndValues;
    bool flag1 = this.ActualXAxis is CategoryAxis3D && !(this.ActualXAxis as CategoryAxis3D).IsIndexed;
    List<double> doubleList = flag1 ? this.GroupedXValuesIndexes : this.GetXValues();
    List<double> zvalues = this.GetZValues();
    bool flag2 = zvalues != null && zvalues.Count > 0;
    double num2 = (flag2 ? this.Area.ActualDepth : this.Area.Depth) / 4.0;
    double start = num2;
    double end = num2 * 3.0;
    DoubleRange zsbsInfo = DoubleRange.Empty;
    if (flag2)
      zsbsInfo = this.GetZSideBySideInfo((ChartSeriesBase) this);
    if (this.YRangeStartValues == null)
      this.YRangeStartValues = (IList<double>) doubleList.Select<double, double>((Func<double, double>) (val => origin)).ToList<double>();
    if (this.Area.VisibleSeries != null)
    {
      List<ChartSeriesBase> list = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series =>
      {
        switch (series)
        {
          case StackingColumnSeries3D _:
          case StackingColumn100Series3D _:
          case StackingBarSeries3D _:
            return true;
          default:
            return series is StackingBar100Series3D;
        }
      })).ToList<ChartSeriesBase>();
      if (list.Count > 0)
      {
        double num3 = (list[0] as StackingColumnSeries3D).SegmentSpacing;
        for (int index = 0; index < list.Count; ++index)
        {
          double segmentSpacing = (list[index] as StackingColumnSeries3D).SegmentSpacing;
          if ((num3 <= 0.0 || num3 > 1.0) && segmentSpacing > num3)
            num3 = segmentSpacing;
        }
        for (int index = 0; index < list.Count; ++index)
          (list[index] as StackingColumnSeries3D).SegmentSpacing = num3;
      }
    }
    if (doubleList == null)
      return;
    if (flag1)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      int index1 = 0;
      for (int index2 = 0; index2 < this.DistinctValuesIndexes.Count; ++index2)
      {
        for (int index3 = 0; index3 < this.DistinctValuesIndexes[(double) index2].Count; ++index3)
        {
          double x1 = (double) index2 + sideBySideInfo.Start;
          double x2 = (double) index2 + sideBySideInfo.End;
          double y2 = double.IsNaN(this.YRangeStartValues[index1]) ? origin : this.YRangeStartValues[index1];
          double y1 = double.IsNaN(this.YRangeEndValues[index1]) ? origin : this.YRangeEndValues[index1];
          double startDepth;
          double endDepth;
          if (flag2)
          {
            startDepth = zvalues[index2] + zsbsInfo.Start;
            endDepth = zvalues[index2] + zsbsInfo.End;
          }
          else
          {
            startDepth = start;
            endDepth = end;
          }
          ObservableCollection<ChartSegment> segments = this.Segments;
          StackingColumnSegment3D stackingColumnSegment3D1 = new StackingColumnSegment3D(x1, y1, x2, y2, startDepth, endDepth, (ChartSeriesBase) this);
          stackingColumnSegment3D1.XData = doubleList[index1];
          stackingColumnSegment3D1.YData = this.GroupedSeriesYValues[0][index1];
          stackingColumnSegment3D1.Item = this.GroupedActualData[index1];
          StackingColumnSegment3D stackingColumnSegment3D2 = stackingColumnSegment3D1;
          segments.Add((ChartSegment) stackingColumnSegment3D2);
          if (this.AdornmentsInfo != null)
          {
            double xadornmentAnglePosition = this.GetXAdornmentAnglePosition((double) index2, sideBySideInfo);
            double num4 = !flag2 ? this.GetZAdornmentAnglePosition(start, end) : this.GetZAdornmentAnglePosition(zvalues[index2], zsbsInfo);
            switch (this.adornmentInfo.GetAdornmentPosition())
            {
              case AdornmentsPosition.Top:
                this.AddColumnAdornments((double) index2, this.GroupedSeriesYValues[0][index1], xadornmentAnglePosition, y1, (double) index1, num1, num4);
                break;
              case AdornmentsPosition.Bottom:
                this.AddColumnAdornments((double) index2, this.GroupedSeriesYValues[0][index1], xadornmentAnglePosition, y2, (double) index1, num1, num4);
                break;
              default:
                this.AddColumnAdornments((double) index2, this.GroupedSeriesYValues[0][index1], xadornmentAnglePosition, y1 + (y2 - y1) / 2.0, (double) index1, num1, num4);
                break;
            }
            ++index1;
          }
        }
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      for (int index = 0; index < this.DataCount; ++index)
      {
        double x1 = doubleList[index] + sideBySideInfo.Start;
        double x2 = doubleList[index] + sideBySideInfo.End;
        double y2 = double.IsNaN(this.YRangeStartValues[index]) ? origin : this.YRangeStartValues[index];
        double y1 = double.IsNaN(this.YRangeEndValues[index]) ? origin : this.YRangeEndValues[index];
        double startDepth = flag2 ? zvalues[index] + zsbsInfo.Start : start;
        double endDepth = flag2 ? zvalues[index] + zsbsInfo.End : end;
        if (index < this.Segments.Count)
        {
          this.Segments[index].SetData(x1, y1, x2, y2, startDepth, endDepth);
          ((ColumnSegment3D) this.Segments[index]).YData = this.ActualSeriesYValues[0][index];
          ((ColumnSegment3D) this.Segments[index]).XData = doubleList[index];
          if (flag2)
            ((ColumnSegment3D) this.Segments[index]).ZData = zvalues[index];
          this.Segments[index].Item = this.ActualData[index];
        }
        else
        {
          ObservableCollection<ChartSegment> segments = this.Segments;
          StackingColumnSegment3D stackingColumnSegment3D3 = new StackingColumnSegment3D(x1, y1, x2, y2, startDepth, endDepth, (ChartSeriesBase) this);
          stackingColumnSegment3D3.XData = doubleList[index];
          stackingColumnSegment3D3.YData = this.ActualSeriesYValues[0][index];
          stackingColumnSegment3D3.ZData = flag2 ? zvalues[index] : 0.0;
          stackingColumnSegment3D3.Item = this.ActualData[index];
          StackingColumnSegment3D stackingColumnSegment3D4 = stackingColumnSegment3D3;
          segments.Add((ChartSegment) stackingColumnSegment3D4);
        }
        if (this.AdornmentsInfo != null)
        {
          double xadornmentAnglePosition = this.GetXAdornmentAnglePosition(doubleList[index], sideBySideInfo);
          double num5 = !flag2 ? this.GetZAdornmentAnglePosition(start, end) : this.GetZAdornmentAnglePosition(zvalues[index], zsbsInfo);
          switch (this.adornmentInfo.GetAdornmentPosition())
          {
            case AdornmentsPosition.Top:
              this.AddColumnAdornments(doubleList[index], this.YValues[index], xadornmentAnglePosition, y1, (double) index, num1, num5);
              continue;
            case AdornmentsPosition.Bottom:
              this.AddColumnAdornments(doubleList[index], this.YValues[index], xadornmentAnglePosition, y2, (double) index, num1, num5);
              continue;
            default:
              this.AddColumnAdornments(doubleList[index], this.YValues[index], xadornmentAnglePosition, y1 + (y2 - y1) / 2.0, (double) index, num1, num5);
              continue;
          }
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(doubleList, true);
  }

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double right, double left)
  {
    return StackingColumnSeries3D.CalculateSegmentSpacing(spacing, right, left);
  }

  internal override bool GetAnimationIsActive()
  {
    return this.AnimationStoryboard != null && this.AnimationStoryboard.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    this.canAnimate = false;
    if (this.Segments.Count <= 0)
      return;
    if (this.AnimationStoryboard != null)
    {
      this.AnimationStoryboard.Stop();
      if (!this.canAnimate)
      {
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    this.AnimationStoryboard = new Storyboard();
    int num = 0;
    double totalSeconds = this.AnimationDuration.TotalSeconds;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      if (!(segment is EmptyPointSegment))
      {
        StackingColumnSegment3D stackingColumnSegment3D = segment as StackingColumnSegment3D;
        double internalTop = stackingColumnSegment3D.InternalTop;
        double internalBottom = stackingColumnSegment3D.InternalBottom;
        if (!double.IsNaN(internalTop) && !double.IsNaN(internalBottom))
        {
          DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame1.Value = 0.0;
          SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
          keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
          element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
          SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame2.Value = internalTop;
          SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
          keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalSeconds));
          KeySpline keySpline1 = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          keyFrame2.KeySpline = keySpline1;
          element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
          Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) ColumnSegment3D.TopProperty));
          Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) stackingColumnSegment3D);
          this.AnimationStoryboard.Children.Add((Timeline) element1);
          DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame splineDoubleKeyFrame3 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame3.Value = 0.0;
          SplineDoubleKeyFrame keyFrame3 = splineDoubleKeyFrame3;
          keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
          element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
          SplineDoubleKeyFrame splineDoubleKeyFrame4 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame4.Value = internalBottom;
          SplineDoubleKeyFrame keyFrame4 = splineDoubleKeyFrame4;
          keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalSeconds));
          KeySpline keySpline2 = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          keyFrame4.KeySpline = keySpline2;
          element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
          Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) ColumnSegment3D.BottomProperty));
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) stackingColumnSegment3D);
          this.AnimationStoryboard.Children.Add((Timeline) element2);
          ++num;
        }
      }
    }
    this.AnimationStoryboard.Begin();
  }

  protected static double CalculateSegmentSpacing(double spacing, double right, double left)
  {
    double num = (right - left) * spacing / 2.0;
    left += num;
    right -= num;
    return left;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    StackingColumnSeries3D stackingColumnSeries3D = new StackingColumnSeries3D();
    stackingColumnSeries3D.SegmentSelectionBrush = this.SegmentSelectionBrush;
    stackingColumnSeries3D.SegmentSpacing = this.SegmentSpacing;
    stackingColumnSeries3D.SelectedIndex = this.SelectedIndex;
    return base.CloneSeries((DependencyObject) stackingColumnSeries3D);
  }

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    StackingColumnSeries3D stackingColumnSeries3D = d as StackingColumnSeries3D;
    if (stackingColumnSeries3D.Area == null)
      return;
    stackingColumnSeries3D.Area.ScheduleUpdate();
  }

  private void Storyboard_Completed(object sender, EventArgs e)
  {
    foreach (DependencyObject child in (sender as ClockGroup).Timeline.Children)
    {
      FrameworkElement target = Storyboard.GetTarget(child) as FrameworkElement;
      target.BeginAnimation(Canvas.TopProperty, (AnimationTimeline) null);
      target.BeginAnimation(Canvas.LeftProperty, (AnimationTimeline) null);
    }
  }
}
