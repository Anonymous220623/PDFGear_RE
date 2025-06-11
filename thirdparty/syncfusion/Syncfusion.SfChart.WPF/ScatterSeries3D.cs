// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ScatterSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ScatterSeries3D : XyzDataSeries3D
{
  public static readonly DependencyProperty ScatterWidthProperty = DependencyProperty.Register(nameof (ScatterWidth), typeof (double), typeof (ScatterSeries3D), new PropertyMetadata((object) 20.0, new PropertyChangedCallback(ScatterSeries3D.OnSizeChanged)));
  public static readonly DependencyProperty ScatterHeightProperty = DependencyProperty.Register(nameof (ScatterHeight), typeof (double), typeof (ScatterSeries3D), new PropertyMetadata((object) 20.0, new PropertyChangedCallback(ScatterSeries3D.OnSizeChanged)));

  public ScatterSegment3D Segment { get; set; }

  public double ScatterWidth
  {
    get => (double) this.GetValue(ScatterSeries3D.ScatterWidthProperty);
    set => this.SetValue(ScatterSeries3D.ScatterWidthProperty, (object) value);
  }

  public double ScatterHeight
  {
    get => (double) this.GetValue(ScatterSeries3D.ScatterHeightProperty);
    set => this.SetValue(ScatterSeries3D.ScatterHeightProperty, (object) value);
  }

  public override void CreateSegments()
  {
    bool flag1 = this.ActualXAxis is CategoryAxis3D && !(this.ActualXAxis as CategoryAxis3D).IsIndexed;
    List<double> xValues = !flag1 ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    List<double> zvalues = this.GetZValues();
    bool flag2 = zvalues != null && zvalues.Count > 0;
    SfChart3D actualArea = this.ActualArea as SfChart3D;
    DoubleRange segmentDepth = this.GetSegmentDepth(flag2 ? actualArea.ActualDepth : actualArea.Depth);
    if (flag1)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (index < xValues.Count)
        {
          double start;
          double end;
          if (flag2)
          {
            start = zvalues[index];
            end = zvalues[index];
          }
          else
          {
            start = segmentDepth.Start;
            end = segmentDepth.End;
          }
          if (index < xValues.Count)
          {
            ObservableCollection<ChartSegment> segments = this.Segments;
            ScatterSegment3D scatterSegment3D1 = new ScatterSegment3D(xValues[index], this.GroupedSeriesYValues[0][index], start, end, this);
            scatterSegment3D1.XData = xValues[index];
            scatterSegment3D1.YData = this.GroupedSeriesYValues[0][index];
            scatterSegment3D1.Item = this.ActualData[index];
            ScatterSegment3D scatterSegment3D2 = scatterSegment3D1;
            segments.Add((ChartSegment) scatterSegment3D2);
            if (this.AdornmentsInfo != null)
              this.AddAdornments(xValues[index], this.GroupedSeriesYValues[0][index], index, start);
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
        double start;
        double end;
        if (flag2)
        {
          start = zvalues[index];
          end = zvalues[index];
        }
        else
        {
          start = segmentDepth.Start;
          end = segmentDepth.End;
        }
        if (index < this.DataCount)
        {
          if (index < this.Segments.Count)
          {
            (this.Segments[index] as ScatterSegment3D).SetData(xValues[index], this.YValues[index], start, end);
            (this.Segments[index] as ScatterSegment3D).YData = this.YValues[index];
            (this.Segments[index] as ScatterSegment3D).XData = xValues[index];
            (this.Segments[index] as ScatterSegment3D).ZData = flag2 ? zvalues[index] : 0.0;
            (this.Segments[index] as ScatterSegment3D).Item = this.ActualData[index];
            ((ScatterSegment3D) this.Segments[index]).Plans = (Polygon3D[]) null;
          }
          else
          {
            ObservableCollection<ChartSegment> segments = this.Segments;
            ScatterSegment3D scatterSegment3D3 = new ScatterSegment3D(xValues[index], this.YValues[index], start, end, this);
            scatterSegment3D3.XData = xValues[index];
            scatterSegment3D3.YData = this.YValues[index];
            scatterSegment3D3.ZData = flag2 ? zvalues[index] : 0.0;
            scatterSegment3D3.Item = this.ActualData[index];
            ScatterSegment3D scatterSegment3D4 = scatterSegment3D3;
            segments.Add((ChartSegment) scatterSegment3D4);
          }
          if (this.AdornmentsInfo != null)
            this.AddAdornments(xValues[index], this.YValues[index], index, start);
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, false);
  }

  internal override bool GetAnimationIsActive()
  {
    return this.AnimationStoryboard != null && this.AnimationStoryboard.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    int index = 0;
    Random random = new Random();
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
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      if (!(segment is EmptyPointSegment))
      {
        ScatterSegment3D scatterSegment3D = segment as ScatterSegment3D;
        if (!double.IsNaN(scatterSegment3D.Y))
        {
          TimeSpan timeSpan = TimeSpan.FromMilliseconds((double) ((this.Segments.IndexOf((ChartSegment) scatterSegment3D) == 0 ? 0 : random.Next(1, 20)) * 20));
          DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame splineDoubleKeyFrame1 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame1.Value = 0.0;
          SplineDoubleKeyFrame keyFrame1 = splineDoubleKeyFrame1;
          keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
          element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
          SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame2.Value = scatterSegment3D.ScatterHeight;
          SplineDoubleKeyFrame keyFrame2 = splineDoubleKeyFrame2;
          keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
          KeySpline keySpline1 = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          keyFrame2.KeySpline = keySpline1;
          element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
          Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath((object) ScatterSegment3D.ScatterHeightProperty));
          Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) scatterSegment3D);
          this.AnimationStoryboard.Children.Add((Timeline) element1);
          DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame splineDoubleKeyFrame3 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame3.Value = 0.0;
          SplineDoubleKeyFrame keyFrame3 = splineDoubleKeyFrame3;
          keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
          element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
          SplineDoubleKeyFrame splineDoubleKeyFrame4 = new SplineDoubleKeyFrame();
          splineDoubleKeyFrame4.Value = scatterSegment3D.ScatterWidth;
          SplineDoubleKeyFrame keyFrame4 = splineDoubleKeyFrame4;
          keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(this.AnimationDuration);
          KeySpline keySpline2 = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          keyFrame4.KeySpline = keySpline2;
          element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
          Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath((object) ScatterSegment3D.ScatterWidthProperty));
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) scatterSegment3D);
          this.AnimationStoryboard.Children.Add((Timeline) element2);
          if (this.AdornmentsInfo != null && this.AdornmentsInfo.ShowLabel)
          {
            UIElement labelPresenter = (UIElement) this.AdornmentsInfo.LabelPresenters[index];
            labelPresenter.Opacity = 0.0;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.To = new double?(1.0);
            doubleAnimation.From = new double?(0.0);
            doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(timeSpan.TotalSeconds + (double) (timeSpan.Seconds * 90 / 100)));
            DoubleAnimation element3 = doubleAnimation;
            element3.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 50.0 / 100.0));
            Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
            Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) labelPresenter);
            this.AnimationStoryboard.Children.Add((Timeline) element3);
          }
          ++index;
        }
      }
    }
    this.AnimationStoryboard.Begin();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    ScatterSeries3D scatterSeries3D = new ScatterSeries3D();
    scatterSeries3D.SegmentSelectionBrush = this.SegmentSelectionBrush;
    scatterSeries3D.SelectedIndex = this.SelectedIndex;
    scatterSeries3D.ScatterHeight = this.ScatterHeight;
    scatterSeries3D.ScatterWidth = this.ScatterWidth;
    return base.CloneSeries((DependencyObject) scatterSeries3D);
  }

  private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ScatterSeries3D scatterSeries3D = d as ScatterSeries3D;
    if (scatterSeries3D.ActualArea == null)
      return;
    (scatterSeries3D.ActualArea as SfChart3D).ScheduleUpdate();
  }
}
