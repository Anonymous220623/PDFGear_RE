// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColumnSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ColumnSeries3D : XyzDataSeries3D, ISegmentSpacing
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (ColumnSeries3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ColumnSeries3D.OnSegmentSpacingChanged)));

  public double SegmentSpacing
  {
    get => (double) this.GetValue(ColumnSeries3D.SegmentSpacingProperty);
    set => this.SetValue(ColumnSeries3D.SegmentSpacingProperty, (object) value);
  }

  protected internal override bool IsSideBySide => true;

  public override void CreateSegments()
  {
    bool flag1 = this.ActualXAxis is CategoryAxis3D && !(this.ActualXAxis as CategoryAxis3D).IsIndexed;
    List<double> xValues = !flag1 ? this.GetXValues() : this.GroupedXValuesIndexes;
    List<double> zvalues = this.GetZValues();
    bool flag2 = zvalues != null && zvalues.Count > 0;
    if (xValues == null)
      return;
    SfChart3D actualArea = this.ActualArea as SfChart3D;
    DoubleRange segmentDepth = this.GetSegmentDepth(flag2 ? actualArea.ActualDepth : actualArea.Depth);
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    DoubleRange zsbsInfo = DoubleRange.Empty;
    if (flag2)
      zsbsInfo = this.GetZSideBySideInfo((ChartSeriesBase) this);
    double num1 = sideBySideInfo.Delta / 2.0;
    double start1 = segmentDepth.Start;
    double end1 = segmentDepth.End;
    int index1 = 0;
    if (flag1 && this.GroupedSeriesYValues[0] != null && this.GroupedSeriesYValues[0].Count > 0)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      this.GroupedActualData.Clear();
      for (int index2 = 0; index2 < this.DistinctValuesIndexes.Count; ++index2)
      {
        List<List<double>> list = this.DistinctValuesIndexes[(double) index2].Select<int, List<double>>((Func<int, List<double>>) (index => new List<double>()
        {
          this.GroupedSeriesYValues[0][index],
          (double) index
        })).ToList<List<double>>();
        double num2 = flag2 ? zsbsInfo.Start : segmentDepth.Start;
        double num3 = zsbsInfo.Delta / (double) list.Count;
        double num4 = num3 * 0.75;
        double num5 = num3 * 0.25;
        for (int index3 = 0; index3 < list.Count; ++index3)
        {
          double num6 = list[index3][0];
          double x1 = (double) index2 + sideBySideInfo.Start;
          double x2 = (double) index2 + sideBySideInfo.End;
          double y1 = num6;
          double y2 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
          double start2 = 0.0;
          double end2 = 0.0;
          this.GroupedActualData.Add(this.ActualData[(int) list[index3][1]]);
          double num7;
          double num8;
          if (list.Count > 1)
          {
            if (flag2)
            {
              double num9 = num2;
              double num10 = num9 + num4;
              num2 = num10 + num5;
              start2 = num9;
              end2 = num10;
              num7 = zvalues[index2] + num9;
              num8 = zvalues[index2] + num10;
            }
            else
            {
              int count = list.Count;
              double num11 = segmentDepth.End / (double) (count * 2 + count + 1);
              num7 = num2 + num11;
              num8 = num7 + segmentDepth.End / (double) (count * 2);
              num2 = num8;
            }
          }
          else if (flag2)
          {
            start2 = zsbsInfo.Start;
            end2 = zsbsInfo.End;
            num7 = zvalues[index2] + zsbsInfo.Start;
            num8 = zvalues[index2] + zsbsInfo.End;
          }
          else
          {
            num7 = segmentDepth.Start;
            num8 = segmentDepth.End;
          }
          ObservableCollection<ChartSegment> segments = this.Segments;
          ColumnSegment3D columnSegment3D1 = new ColumnSegment3D(x1, y1, x2, y2, num7, num8, (ChartSeriesBase) this);
          columnSegment3D1.XData = xValues[index3];
          columnSegment3D1.YData = num6;
          columnSegment3D1.Item = this.GroupedActualData[index1];
          ColumnSegment3D columnSegment3D2 = columnSegment3D1;
          segments.Add((ChartSegment) columnSegment3D2);
          if (this.AdornmentsInfo != null)
          {
            double xadornmentAnglePosition = this.GetXAdornmentAnglePosition((double) index2, sideBySideInfo);
            double num12 = !flag2 ? this.GetZAdornmentAnglePosition(num7, num8) : this.GetZAdornmentAnglePosition(zvalues[index2], new DoubleRange(start2, end2));
            switch (this.adornmentInfo.GetAdornmentPosition())
            {
              case AdornmentsPosition.Top:
                this.AddColumnAdornments((double) index2, num6, xadornmentAnglePosition, y1, (double) index1, num1, num12);
                break;
              case AdornmentsPosition.Bottom:
                this.AddColumnAdornments((double) index2, num6, xadornmentAnglePosition, y2, (double) index1, num1, num12);
                break;
              default:
                this.AddColumnAdornments((double) index2, num6, xadornmentAnglePosition, y1 + (y2 - y1) / 2.0, (double) index1, num1, num12);
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
      for (int index4 = 0; index4 < this.DataCount; ++index4)
      {
        if (index4 < this.DataCount)
        {
          double x1 = xValues[index4] + sideBySideInfo.Start;
          double x2 = xValues[index4] + sideBySideInfo.End;
          double yvalue = this.YValues[index4];
          double y2 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
          double startDepth;
          double endDepth;
          if (flag2)
          {
            startDepth = zvalues[index4] + zsbsInfo.Start;
            endDepth = zvalues[index4] + zsbsInfo.End;
          }
          else
          {
            startDepth = segmentDepth.Start;
            endDepth = segmentDepth.End;
          }
          if (index4 < this.Segments.Count)
          {
            this.Segments[index4].SetData(x1, yvalue, x2, y2, startDepth, endDepth);
            ((ColumnSegment3D) this.Segments[index4]).YData = this.YValues[index4];
            ((ColumnSegment3D) this.Segments[index4]).Plans = (Polygon3D[]) null;
            ((ColumnSegment3D) this.Segments[index4]).XData = xValues[index4];
            if (flag2)
              ((ColumnSegment3D) this.Segments[index4]).ZData = zvalues[index4];
            this.Segments[index4].Item = this.ActualData[index4];
          }
          else
          {
            ObservableCollection<ChartSegment> segments = this.Segments;
            ColumnSegment3D columnSegment3D3 = new ColumnSegment3D(x1, yvalue, x2, y2, startDepth, endDepth, (ChartSeriesBase) this);
            columnSegment3D3.XData = xValues[index4];
            columnSegment3D3.YData = this.YValues[index4];
            columnSegment3D3.ZData = flag2 ? zvalues[index4] : 0.0;
            columnSegment3D3.Item = this.ActualData[index4];
            ColumnSegment3D columnSegment3D4 = columnSegment3D3;
            segments.Add((ChartSegment) columnSegment3D4);
          }
          if (this.AdornmentsInfo != null)
          {
            double xadornmentAnglePosition = this.GetXAdornmentAnglePosition(xValues[index4], sideBySideInfo);
            double num13 = !flag2 ? this.GetZAdornmentAnglePosition(start1, end1) : this.GetZAdornmentAnglePosition(zvalues[index4], zsbsInfo);
            switch (this.adornmentInfo.GetAdornmentPosition())
            {
              case AdornmentsPosition.Top:
                this.AddColumnAdornments(xValues[index4], this.YValues[index4], xadornmentAnglePosition, yvalue, (double) index4, num1, num13);
                continue;
              case AdornmentsPosition.Bottom:
                this.AddColumnAdornments(xValues[index4], this.YValues[index4], xadornmentAnglePosition, y2, (double) index4, num1, num13);
                continue;
              default:
                this.AddColumnAdornments(xValues[index4], this.YValues[index4], xadornmentAnglePosition, yvalue + (y2 - yvalue) / 2.0, (double) index4, num1, num13);
                continue;
            }
          }
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, true);
  }

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double right, double left)
  {
    return ColumnSeries3D.CalculateSegmentSpacing(spacing, right, left);
  }

  internal override void OnTransposeChanged(bool val) => this.IsActualTransposed = val;

  internal override bool GetAnimationIsActive()
  {
    return this.AnimationStoryboard != null && this.AnimationStoryboard.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
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
    double end = this.ActualYAxis.VisibleRange.End;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      if (!(segment is EmptyPointSegment))
      {
        ColumnSegment3D columnSegment3D = segment as ColumnSegment3D;
        double d = end < columnSegment3D.Top ? end : columnSegment3D.Top;
        if (!double.IsNaN(d))
        {
          DoubleAnimationUsingKeyFrames element = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame1.Value = columnSegment3D.Bottom;
          SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
          keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
          element.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
          SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame2.Value = d;
          SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
          keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
          KeySpline keySpline = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          keyFrame2.KeySpline = keySpline;
          element.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
          Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath((object) ColumnSegment3D.TopProperty));
          Storyboard.SetTarget((DependencyObject) element, (DependencyObject) columnSegment3D);
          this.AnimationStoryboard.Children.Add((Timeline) element);
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
    ColumnSeries3D columnSeries3D = new ColumnSeries3D();
    columnSeries3D.SegmentSelectionBrush = this.SegmentSelectionBrush;
    columnSeries3D.SelectedIndex = this.SelectedIndex;
    columnSeries3D.SegmentSpacing = this.SegmentSpacing;
    return base.CloneSeries((DependencyObject) columnSeries3D);
  }

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ColumnSeries3D columnSeries3D = d as ColumnSeries3D;
    if (columnSeries3D.Area == null)
      return;
    columnSeries3D.Area.ScheduleUpdate();
  }
}
