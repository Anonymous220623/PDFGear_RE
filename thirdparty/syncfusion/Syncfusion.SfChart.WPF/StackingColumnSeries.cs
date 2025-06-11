// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingColumnSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class StackingColumnSeries : StackingSeriesBase, ISegmentSelectable, ISegmentSpacing
{
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (StackingColumnSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StackingColumnSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (StackingColumnSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(StackingColumnSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (StackingColumnSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(StackingColumnSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (StackingColumnSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(StackingColumnSeries.OnCustomTemplateChanged)));
  private Storyboard sb;

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(StackingColumnSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(StackingColumnSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(StackingColumnSeries.SegmentSpacingProperty);
    set => this.SetValue(StackingColumnSeries.SegmentSpacingProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(StackingColumnSeries.SelectedIndexProperty);
    set => this.SetValue(StackingColumnSeries.SelectedIndexProperty, (object) value);
  }

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(StackingColumnSeries.CustomTemplateProperty);
    set => this.SetValue(StackingColumnSeries.CustomTemplateProperty, (object) value);
  }

  protected internal override bool IsSideBySide => true;

  protected override bool IsStacked => true;

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double Origin = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      Origin = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    double num1 = sideBySideInfo.Delta / 2.0;
    List<double> doubleList = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    StackingValues cumulativeStackValues = this.GetCumulativeStackValues((ChartSeriesBase) this);
    if (cumulativeStackValues == null)
      return;
    this.YRangeStartValues = cumulativeStackValues.StartValues;
    this.YRangeEndValues = cumulativeStackValues.EndValues;
    if (this.YRangeStartValues == null)
      this.YRangeStartValues = (IList<double>) doubleList.Select<double, double>((Func<double, double>) (val => Origin)).ToList<double>();
    if (doubleList == null)
      return;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      int index1 = 0;
      List<string> distinctXvalues = this.GetDistinctXValues();
      int index2 = 0;
      List<string> xvalues = this.XValues as List<string>;
      for (int key1 = 0; key1 < this.DistinctValuesIndexes.Count; ++key1)
      {
        for (int index3 = 0; index3 < this.DistinctValuesIndexes[(double) key1].Count; ++index3)
        {
          int key2 = distinctXvalues.IndexOf(xvalues[index2]);
          if (index3 < doubleList.Count)
          {
            int index4 = this.DistinctValuesIndexes[(double) key2][index3];
            double num2 = (double) key2 + sideBySideInfo.Start;
            double num3 = (double) key2 + sideBySideInfo.End;
            double num4 = double.IsNaN(this.YRangeStartValues[index1]) ? Origin : this.YRangeStartValues[index1];
            double num5 = double.IsNaN(this.YRangeEndValues[index1]) ? Origin : this.YRangeEndValues[index1];
            if (this.CreateSegment() is StackingColumnSegment segment)
            {
              segment.Series = (ChartSeriesBase) this;
              segment.SetData(num2, num5, num3, num4);
              segment.customTemplate = this.CustomTemplate;
              segment.XData = doubleList[index1];
              segment.YData = this.GroupedSeriesYValues[0][index4];
              segment.Item = this.GroupedActualData[index1];
              this.Segments.Add((ChartSegment) segment);
              ++index2;
            }
            if (this.AdornmentsInfo != null)
            {
              switch (this.adornmentInfo.GetAdornmentPosition())
              {
                case AdornmentsPosition.Top:
                  this.AddColumnAdornments((double) key1, this.GroupedSeriesYValues[0][index4], num2, num5, (double) index1, num1);
                  break;
                case AdornmentsPosition.Bottom:
                  this.AddColumnAdornments((double) key1, this.GroupedSeriesYValues[0][index4], num2, num4, (double) index1, num1);
                  break;
                default:
                  this.AddColumnAdornments((double) key1, this.GroupedSeriesYValues[0][index4], num2, num5 + (num4 - num5) / 2.0, (double) index1, num1);
                  break;
              }
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
        double num6 = doubleList[index] + sideBySideInfo.Start;
        double num7 = doubleList[index] + sideBySideInfo.End;
        double num8 = double.IsNaN(this.YRangeStartValues[index]) ? Origin : this.YRangeStartValues[index];
        double num9 = double.IsNaN(this.YRangeEndValues[index]) ? Origin : this.YRangeEndValues[index];
        if (index < this.Segments.Count)
        {
          this.Segments[index].Item = this.ActualData[index];
          (this.Segments[index] as StackingColumnSegment).XData = doubleList[index];
          (this.Segments[index] as StackingColumnSegment).YData = this.YValues[index];
          this.Segments[index].SetData(num6, num9, num7, num8);
          if (this.SegmentColorPath != null && !this.Segments[index].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index].IsSelectedSegment)
            this.Segments[index].Interior = this.Interior != null ? this.Interior : this.ColorValues[index];
        }
        else if (this.CreateSegment() is StackingColumnSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(num6, num9, num7, num8);
          segment.customTemplate = this.CustomTemplate;
          segment.XData = doubleList[index];
          segment.YData = this.ActualSeriesYValues[0][index];
          segment.Item = this.ActualData[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
        {
          switch (this.adornmentInfo.GetAdornmentPosition())
          {
            case AdornmentsPosition.Top:
              this.AddColumnAdornments(doubleList[index], this.YValues[index], num6, num9, (double) index, num1);
              continue;
            case AdornmentsPosition.Bottom:
              this.AddColumnAdornments(doubleList[index], this.YValues[index], num6, num8, (double) index, num1);
              continue;
            default:
              this.AddColumnAdornments(doubleList[index], this.YValues[index], num6, num9 + (num8 - num9) / 2.0, (double) index, num1);
              continue;
          }
        }
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    if (this is StackingColumn100Series)
    {
      List<int> emptyPointIndex = this.EmptyPointIndexes[0];
      if (this.EmptyPointStyle == EmptyPointStyle.Symbol || this.EmptyPointStyle == EmptyPointStyle.SymbolAndInterior)
      {
        foreach (int index in emptyPointIndex)
          this.ActualSeriesYValues[0][index] = double.IsNaN(this.YRangeEndValues[index]) ? 0.0 : this.YRangeEndValues[index];
      }
      this.UpdateEmptyPointSegments(doubleList, true);
      this.ReValidateYValues(this.EmptyPointIndexes);
      this.ValidateYValues();
    }
    else
      this.UpdateEmptyPointSegments(doubleList, true);
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    int index = 0;
    if (this.sb != null)
    {
      this.sb.Stop();
      if (!this.canAnimate)
      {
        this.ResetAdornmentAnimationState();
        return;
      }
    }
    this.sb = new Storyboard();
    object parameter = this.IsActualTransposed ? (object) Canvas.LeftProperty : (object) Canvas.TopProperty;
    string path1 = this.IsActualTransposed ? "(UIElement.RenderTransform).(ScaleTransform.ScaleX)" : "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
    string path2 = this.IsActualTransposed ? "(UIElement.RenderTransform).(TranslateTransform.X)" : "(UIElement.RenderTransform).(TranslateTransform.Y)";
    double point = this.Area.ValueToPoint(this.Area.InternalSecondaryAxis, 0.0);
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      FrameworkElement renderedVisual = (FrameworkElement) segment.GetRenderedVisual();
      if (renderedVisual == null)
        return;
      double d;
      if (this.ShowEmptyPoints)
      {
        d = this.IsActualTransposed ? renderedVisual.Width : renderedVisual.Height;
      }
      else
      {
        ColumnSegment columnSegment = segment as ColumnSegment;
        d = this.IsActualTransposed ? columnSegment.Width : columnSegment.Height;
      }
      if (!double.IsNaN(d))
      {
        renderedVisual.RenderTransform = (Transform) new ScaleTransform();
        double num = this.IsActualTransposed ? Canvas.GetLeft((UIElement) renderedVisual) : Canvas.GetTop((UIElement) renderedVisual);
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        keyFrame1.Value = point;
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
        keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
        keyFrame2.Value = num;
        keyFrame2.KeySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(parameter));
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) renderedVisual);
        this.sb.Children.Add((Timeline) element1);
        DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
        keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        keyFrame3.Value = 0.0;
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
        SplineDoubleKeyFrame keyFrame4 = new SplineDoubleKeyFrame();
        keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(this.AnimationDuration);
        keyFrame4.KeySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
        keyFrame4.Value = 1.0;
        Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath(path1, new object[0]));
        Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) renderedVisual);
        this.sb.Children.Add((Timeline) element2);
        if (this.AdornmentsInfo != null && this.AdornmentsInfo.ShowLabel)
        {
          FrameworkElement labelPresenter = this.AdornmentsInfo.LabelPresenters[index];
          labelPresenter.RenderTransform = (Transform) new TranslateTransform();
          DoubleAnimationUsingKeyFrames element3 = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame keyFrame5 = new SplineDoubleKeyFrame();
          keyFrame5.KeyTime = keyFrame5.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
          keyFrame5.Value = this.YValues[index] > 0.0 ? d * 10.0 / 100.0 : -(d * 10.0) / 100.0;
          element3.KeyFrames.Add((DoubleKeyFrame) keyFrame5);
          SplineDoubleKeyFrame keyFrame6 = new SplineDoubleKeyFrame();
          keyFrame6.KeyTime = keyFrame6.KeyTime.GetKeyTime(this.AnimationDuration);
          keyFrame6.KeySpline = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          element3.KeyFrames.Add((DoubleKeyFrame) keyFrame6);
          keyFrame6.Value = 0.0;
          Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath(path2, new object[0]));
          Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) labelPresenter);
          this.sb.Children.Add((Timeline) element3);
          labelPresenter.Opacity = 0.0;
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(0.0);
          doubleAnimation.To = new double?(1.0);
          doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
          DoubleAnimation element4 = doubleAnimation;
          element4.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 20.0 / 100.0));
          Storyboard.SetTarget((DependencyObject) element4, (DependencyObject) labelPresenter);
          Storyboard.SetTargetProperty((DependencyObject) element4, new PropertyPath((object) UIElement.OpacityProperty));
          this.sb.Children.Add((Timeline) element4);
        }
        this.sb.Completed += new EventHandler(this.sb_Completed);
        ++index;
      }
    }
    this.sb.Begin();
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    StackingColumnSegment toolTipTag = this.ToolTipTag as StackingColumnSegment;
    Point dataPointPosition = new Point();
    Point point = !this.IsActualTransposed ? this.ChartTransformer.TransformToVisible(toolTipTag.XRange.Median, toolTipTag.YRange.End) : this.ChartTransformer.TransformToVisible(this.ActualXAxis.IsInversed ? toolTipTag.XRange.Start : toolTipTag.XRange.End, toolTipTag.YRange.Median);
    dataPointPosition.X = point.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = point.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new StackingColumnSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new StackingColumnSeries()
    {
      CustomTemplate = this.CustomTemplate,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      SegmentSpacing = this.SegmentSpacing
    });
  }

  protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    double num = (Right - Left) * spacing / 2.0;
    Left += num;
    Right -= num;
    return Left;
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as StackingColumnSeries).UpdateArea();
  }

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    StackingColumnSeries stackingColumnSeries = d as StackingColumnSeries;
    if (stackingColumnSeries.Area == null)
      return;
    stackingColumnSeries.Area.ScheduleUpdate();
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries chartSeries = d as ChartSeries;
    chartSeries.OnPropertyChanged("SelectedIndex");
    if (chartSeries.ActualArea == null || chartSeries.ActualArea.SelectionBehaviour == null)
      return;
    if (chartSeries.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
    {
      chartSeries.SelectedIndexChanged((int) e.NewValue, (int) e.OldValue);
    }
    else
    {
      if ((int) e.NewValue == -1)
        return;
      chartSeries.SelectedSegmentsIndexes.Add((int) e.NewValue);
    }
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    StackingColumnSeries stackingColumnSeries = d as StackingColumnSeries;
    if (stackingColumnSeries.Area == null)
      return;
    stackingColumnSeries.Segments.Clear();
    stackingColumnSeries.Area.ScheduleUpdate();
  }

  private void sb_Completed(object sender, EventArgs e)
  {
    foreach (DependencyObject child in (sender as ClockGroup).Timeline.Children)
    {
      FrameworkElement target = Storyboard.GetTarget(child) as FrameworkElement;
      target.BeginAnimation(Canvas.TopProperty, (AnimationTimeline) null);
      target.BeginAnimation(Canvas.LeftProperty, (AnimationTimeline) null);
    }
  }
}
