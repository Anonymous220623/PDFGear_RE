// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.WaterfallSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class WaterfallSeries : XyDataSeries, ISegmentSelectable, ISegmentSpacing
{
  public static readonly DependencyProperty AllowAutoSumProperty = DependencyProperty.Register(nameof (AllowAutoSum), typeof (bool), typeof (WaterfallSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(WaterfallSeries.OnAllowAutoSum)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (WaterfallSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(WaterfallSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (WaterfallSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(WaterfallSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (WaterfallSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(WaterfallSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty ShowConnectorProperty = DependencyProperty.Register(nameof (ShowConnector), typeof (bool), typeof (WaterfallSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(WaterfallSeries.OnShowConnectorChanged)));
  public static readonly DependencyProperty SummaryBindingPathProperty = DependencyProperty.Register(nameof (SummaryBindingPath), typeof (string), typeof (WaterfallSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(WaterfallSeries.OnSummaryBindingPathChanged)));
  public static readonly DependencyProperty ConnectorLineStyleProperty = DependencyProperty.Register(nameof (ConnectorLineStyle), typeof (Style), typeof (WaterfallSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(WaterfallSeries.OnConnectorLineStyleChanged)));
  public static readonly DependencyProperty NegativeSegmentBrushProperty = DependencyProperty.Register(nameof (NegativeSegmentBrush), typeof (Brush), typeof (WaterfallSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(WaterfallSeries.OnNegativeSegmentBrushChanged)));
  public static readonly DependencyProperty SummarySegmentBrushProperty = DependencyProperty.Register(nameof (SummarySegmentBrush), typeof (Brush), typeof (WaterfallSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(WaterfallSeries.OnSummarySegmentBrushChanged)));
  private Storyboard sb;

  public event EventHandler<WaterfallSegmentCreatedEventArgs> SegmentCreated;

  public bool AllowAutoSum
  {
    get => (bool) this.GetValue(WaterfallSeries.AllowAutoSumProperty);
    set => this.SetValue(WaterfallSeries.AllowAutoSumProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(WaterfallSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(WaterfallSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(WaterfallSeries.SelectedIndexProperty);
    set => this.SetValue(WaterfallSeries.SelectedIndexProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(WaterfallSeries.SegmentSpacingProperty);
    set => this.SetValue(WaterfallSeries.SegmentSpacingProperty, (object) value);
  }

  public bool ShowConnector
  {
    get => (bool) this.GetValue(WaterfallSeries.ShowConnectorProperty);
    set => this.SetValue(WaterfallSeries.ShowConnectorProperty, (object) value);
  }

  public string SummaryBindingPath
  {
    get => (string) this.GetValue(WaterfallSeries.SummaryBindingPathProperty);
    set => this.SetValue(WaterfallSeries.SummaryBindingPathProperty, (object) value);
  }

  public Style ConnectorLineStyle
  {
    get => (Style) this.GetValue(WaterfallSeries.ConnectorLineStyleProperty);
    set => this.SetValue(WaterfallSeries.ConnectorLineStyleProperty, (object) value);
  }

  public Brush NegativeSegmentBrush
  {
    get => (Brush) this.GetValue(WaterfallSeries.NegativeSegmentBrushProperty);
    set => this.SetValue(WaterfallSeries.NegativeSegmentBrushProperty, (object) value);
  }

  public Brush SummarySegmentBrush
  {
    get => (Brush) this.GetValue(WaterfallSeries.SummarySegmentBrushProperty);
    set => this.SetValue(WaterfallSeries.SummarySegmentBrushProperty, (object) value);
  }

  internal List<bool> SummaryValues { get; set; }

  protected internal override bool IsSideBySide => true;

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    List<double> xvalues = this.GetXValues();
    double num = this.GetSideBySideInfo((ChartSeriesBase) this).Delta / 2.0;
    double origin = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      origin = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    if (xvalues != null)
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (index < this.DataCount)
        {
          double x1;
          double x2;
          double y1;
          double y2;
          this.OnCalculateSegmentValues(out x1, out x2, out y1, out y2, index, origin, xvalues[index]);
          bool isSum1 = false;
          if (index < this.Segments.Count)
          {
            segment = this.Segments[index] as WaterfallSegment;
            segment.SetData(x1, y1, x2, y2);
            segment.XData = xvalues[index];
            segment.YData = this.YValues[index];
            segment.Item = this.ActualData[index];
            if (segment.SegmentType == WaterfallSegmentType.Sum)
              isSum1 = true;
            this.OnUpdateSumSegmentValues(segment, index, isSum1, origin);
            segment.BindProperties();
          }
          else if (this.CreateSegment() is WaterfallSegment segment)
          {
            segment.XData = xvalues[index];
            segment.YData = this.YValues[index];
            segment.Item = this.ActualData[index];
            segment.Series = (ChartSeriesBase) this;
            segment.SetData(x1, y1, x2, y2);
            bool isSum2 = this.RaiseSegmentCreatedEvent(segment, index);
            this.OnUpdateSumSegmentValues(segment, index, isSum2, origin);
            this.Segments.Add((ChartSegment) segment);
          }
          if (this.AdornmentsInfo != null)
          {
            AdornmentsPosition adornmentPosition = this.adornmentInfo.GetAdornmentPosition();
            if (segment.SegmentType == WaterfallSegmentType.Sum)
            {
              if (this.Segments.IndexOf((ChartSegment) segment) > 0 && this.AllowAutoSum)
              {
                if (adornmentPosition == AdornmentsPosition.TopAndBottom)
                  this.AddColumnAdornments(xvalues[index], segment.WaterfallSum, x1, segment.WaterfallSum / 2.0, (double) index, num);
                else if (segment.WaterfallSum >= 0.0)
                  this.AddColumnAdornments(xvalues[index], segment.WaterfallSum, x1, segment.Top, (double) index, num);
                else
                  this.AddColumnAdornments(xvalues[index], segment.WaterfallSum, x1, segment.Bottom, (double) index, num);
              }
              else if (adornmentPosition == AdornmentsPosition.TopAndBottom)
                this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, y1 + (y2 - y1) / 2.0, (double) index, num);
              else if (this.YValues[index] >= 0.0)
                this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, segment.Top, (double) index, num);
              else
                this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, segment.Bottom, (double) index, num);
            }
            else
            {
              switch (adornmentPosition)
              {
                case AdornmentsPosition.Top:
                  if (segment.SegmentType == WaterfallSegmentType.Positive)
                  {
                    this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, y1, (double) index, num);
                    continue;
                  }
                  this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, y2, (double) index, num);
                  continue;
                case AdornmentsPosition.Bottom:
                  if (segment.SegmentType == WaterfallSegmentType.Positive)
                  {
                    this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, y2, (double) index, num);
                    continue;
                  }
                  this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, y1, (double) index, num);
                  continue;
                default:
                  this.AddColumnAdornments(xvalues[index], this.YValues[index], x1, y1 + (y2 - y1) / 2.0, (double) index, num);
                  continue;
              }
            }
          }
        }
      }
    }
    this.ActualArea.IsUpdateLegend = true;
  }

  internal override void UpdateTooltip(object source)
  {
    if (!this.ShowTooltip || !(source is FrameworkElement element))
      return;
    object tooltipTag = this.GetTooltipTag(element);
    if (tooltipTag is WaterfallSegment waterfallSegment && waterfallSegment.SegmentType == WaterfallSegmentType.Sum && this.Segments.IndexOf((ChartSegment) waterfallSegment) != 0)
    {
      ChartDataPointInfo customTag = new ChartDataPointInfo();
      customTag.Series = (ChartSeriesBase) this;
      customTag.XData = waterfallSegment.XData;
      customTag.Item = waterfallSegment.Item;
      customTag.YData = this.Segments.IndexOf((ChartSegment) waterfallSegment) <= 0 || !this.AllowAutoSum ? waterfallSegment.YData : waterfallSegment.WaterfallSum;
      this.UpdateSeriesTooltip((object) customTag);
    }
    else
      this.UpdateSeriesTooltip(tooltipTag);
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    int index1 = 0;
    int index2 = 0;
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
    string path1 = this.IsActualTransposed ? "(UIElement.RenderTransform).(ScaleTransform.ScaleX)" : "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
    string path2 = this.IsActualTransposed ? "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)" : "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)";
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      FrameworkElement renderedVisual = (FrameworkElement) segment.GetRenderedVisual();
      double d = !(segment is EmptyPointSegment) || (segment as EmptyPointSegment).IsEmptySegmentInterior && this.EmptyPointStyle != EmptyPointStyle.SymbolAndInterior ? (this.IsActualTransposed ? ((WaterfallSegment) segment).Width : ((WaterfallSegment) segment).Height) : (this.IsActualTransposed ? ((EmptyPointSegment) segment).EmptyPointSymbolWidth : ((EmptyPointSegment) segment).EmptyPointSymbolHeight);
      if (!double.IsNaN(d) && !double.IsNaN(this.YValues[index1]))
      {
        if (renderedVisual == null)
          return;
        renderedVisual.RenderTransform = (Transform) new ScaleTransform();
        if (this.YValues[index1] < 0.0 && this.IsActualTransposed)
          renderedVisual.RenderTransformOrigin = new Point(1.0, 1.0);
        else if (this.YValues[index1] > 0.0 && !this.IsActualTransposed)
          renderedVisual.RenderTransformOrigin = new Point(1.0, 1.0);
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0.0));
        keyFrame1.Value = 0.0;
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
        keyFrame2.KeyTime = keyFrame2.KeyTime.GetKeyTime(this.AnimationDuration);
        keyFrame2.KeySpline = new KeySpline()
        {
          ControlPoint1 = new Point(0.64, 0.84),
          ControlPoint2 = new Point(0.67, 0.95)
        };
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame2);
        keyFrame2.Value = 1.0;
        Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath(path1, new object[0]));
        Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) renderedVisual);
        this.sb.Children.Add((Timeline) element1);
        if (this.AdornmentsInfo != null && this.AdornmentsInfo.ShowLabel)
        {
          FrameworkElement labelPresenter = this.AdornmentsInfo.LabelPresenters[index2];
          TransformGroup renderTransform = labelPresenter.RenderTransform as TransformGroup;
          TranslateTransform translateTransform = new TranslateTransform();
          if (renderTransform != null)
          {
            if (renderTransform.Children.Count > 0 && renderTransform.Children[0] is TranslateTransform)
              renderTransform.Children[0] = (Transform) translateTransform;
            else
              renderTransform.Children.Insert(0, (Transform) translateTransform);
          }
          DoubleAnimationUsingKeyFrames element2 = new DoubleAnimationUsingKeyFrames();
          SplineDoubleKeyFrame keyFrame3 = new SplineDoubleKeyFrame();
          keyFrame3.KeyTime = keyFrame3.KeyTime.GetKeyTime(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
          keyFrame3.Value = this.YValues[index1] > 0.0 ? d * 10.0 / 100.0 : -(d * 10.0) / 100.0;
          element2.KeyFrames.Add((DoubleKeyFrame) keyFrame3);
          SplineDoubleKeyFrame keyFrame4 = new SplineDoubleKeyFrame();
          keyFrame4.KeyTime = keyFrame4.KeyTime.GetKeyTime(this.AnimationDuration);
          keyFrame4.KeySpline = new KeySpline()
          {
            ControlPoint1 = new Point(0.64, 0.84),
            ControlPoint2 = new Point(0.67, 0.95)
          };
          element2.KeyFrames.Add((DoubleKeyFrame) keyFrame4);
          keyFrame4.Value = 0.0;
          Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath(path2, new object[0]));
          Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) labelPresenter);
          this.sb.Children.Add((Timeline) element2);
          labelPresenter.Opacity = 0.0;
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(0.0);
          doubleAnimation.To = new double?(1.0);
          doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
          DoubleAnimation element3 = doubleAnimation;
          element2.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 20.0 / 100.0));
          Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) labelPresenter);
          Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
          this.sb.Children.Add((Timeline) element3);
          ++index2;
        }
      }
      ++index1;
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

  internal bool RaiseSegmentCreatedEvent(WaterfallSegment segment, int index)
  {
    WaterfallSegmentCreatedEventArgs args = new WaterfallSegmentCreatedEventArgs();
    args.Segment = (ChartSegment) segment;
    args.Index = index;
    this.OnSegmentCreated(args);
    return args.IsSummary;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    WaterfallSegment toolTipTag1 = this.ToolTipTag as WaterfallSegment;
    Point dataPointPosition = new Point();
    if (toolTipTag1 != null)
    {
      Point visible = this.ChartTransformer.TransformToVisible(toolTipTag1.XData, toolTipTag1.Top);
      dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
      dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    }
    else
    {
      ChartDataPointInfo toolTipTag2 = this.ToolTipTag as ChartDataPointInfo;
      Point visible = this.ChartTransformer.TransformToVisible(toolTipTag2.XData, toolTipTag2.YData);
      dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
      dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    }
    return dataPointPosition;
  }

  protected internal virtual void OnSegmentCreated(WaterfallSegmentCreatedEventArgs args)
  {
    if (this.SegmentCreated == null)
      return;
    this.SegmentCreated((object) this, args);
  }

  protected internal override void GeneratePoints()
  {
    if (this.YBindingPath != null)
      this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    this.GetSummaryValues();
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new WaterfallSegment();

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    base.OnBindingPathChanged(args);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.GetSummaryValues();
  }

  protected override void SetIndividualPoint(int index, object obj, bool replace)
  {
    if (this.SummaryBindingPath != null)
    {
      object arrayPropertyValue = this.GetArrayPropertyValue(obj, new string[1]
      {
        this.SummaryBindingPath
      });
      if (replace && this.SummaryValues.Count > index)
        this.SummaryValues[index] = Convert.ToBoolean(arrayPropertyValue);
      else
        this.SummaryValues.Insert(index, Convert.ToBoolean(arrayPropertyValue));
    }
    base.SetIndividualPoint(index, obj, replace);
    if (this.SummaryValues == null || index >= this.Segments.Count || index >= this.YValues.Count)
      return;
    WaterfallSegment segment = this.Segments[index] as WaterfallSegment;
    segment.SegmentType = this.SummaryValues[index] ? WaterfallSegmentType.Sum : (this.YValues[index] >= 0.0 ? WaterfallSegmentType.Positive : WaterfallSegmentType.Negative);
    segment.BindProperties();
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new WaterfallSeries()
    {
      SegmentSpacing = this.SegmentSpacing,
      SelectedIndex = this.SelectedIndex,
      SegmentSelectionBrush = this.SegmentSelectionBrush
    });
  }

  protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    double num = (Right - Left) * spacing / 2.0;
    Left += num;
    Right -= num;
    return Left;
  }

  private static void OnAllowAutoSum(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as WaterfallSeries).UpdateArea();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as WaterfallSeries).UpdateArea();
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries chartSeries = d as ChartSeries;
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

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    WaterfallSeries waterfallSeries = d as WaterfallSeries;
    if (waterfallSeries.Area == null)
      return;
    waterfallSeries.Area.ScheduleUpdate();
  }

  private static void OnShowConnectorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is WaterfallSeries waterfallSeries) || waterfallSeries.Area == null)
      return;
    waterfallSeries.Area.ScheduleUpdate();
  }

  private static void OnNegativeSegmentBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    WaterfallSeries.OnUpdateSegmentandAdornmentInterior(d as WaterfallSeries);
  }

  private static void OnSummarySegmentBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    WaterfallSeries.OnUpdateSegmentandAdornmentInterior(d as WaterfallSeries);
  }

  private static void OnConnectorLineStyleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    WaterfallSeries waterfallSeries = d as WaterfallSeries;
    if (e.NewValue == null)
      return;
    foreach (WaterfallSegment segment in (Collection<ChartSegment>) waterfallSeries.Segments)
      segment.LineSegment.Style = e.NewValue as Style;
  }

  private static void OnSummaryBindingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as WaterfallSeries).OnBindingPathChanged(e);
  }

  private static void OnUpdateSegmentandAdornmentInterior(WaterfallSeries series)
  {
    if (series.Area == null)
      return;
    foreach (ChartSegment segment in (Collection<ChartSegment>) series.Segments)
      segment.BindProperties();
    foreach (ChartAdornment adornment1 in (Collection<ChartAdornment>) series.Adornments)
    {
      ChartAdornment adornment = adornment1;
      if (series.Segments.FirstOrDefault<ChartSegment>((System.Func<ChartSegment, bool>) (seg => seg.Item == adornment.Item)) is WaterfallSegment segment)
        adornment.BindWaterfallSegmentInterior(segment);
    }
  }

  private void OnCalculateSegmentValues(
    out double x1,
    out double x2,
    out double y1,
    out double y2,
    int i,
    double origin,
    double xVal)
  {
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    x1 = xVal + sideBySideInfo.Start;
    x2 = xVal + sideBySideInfo.End;
    if (i == 0)
    {
      if (this.YValues[i] >= 0.0)
      {
        y1 = this.YValues[i];
        y2 = origin;
      }
      else if (double.IsNaN(this.YValues[i]))
      {
        y2 = origin;
        y1 = origin;
      }
      else
      {
        y2 = this.YValues[i];
        y1 = origin;
      }
    }
    else
    {
      WaterfallSegment segment = this.Segments[i - 1] as WaterfallSegment;
      if (this.YValues[i] >= 0.0)
      {
        if (this.YValues[i - 1] >= 0.0 || segment.SegmentType == WaterfallSegmentType.Sum)
        {
          y1 = this.YValues[i] + segment.Top;
          y2 = segment.Top;
        }
        else if (double.IsNaN(this.YValues[i - 1]))
        {
          y1 = this.YValues[i] == 0.0 ? segment.Bottom : segment.Bottom + this.YValues[i];
          y2 = segment.Bottom;
        }
        else
        {
          y1 = this.YValues[i] + segment.Bottom;
          y2 = segment.Bottom;
        }
      }
      else if (double.IsNaN(this.YValues[i]))
      {
        if (this.YValues[i - 1] >= 0.0 || segment.SegmentType == WaterfallSegmentType.Sum)
          y1 = y2 = segment.Top;
        else
          y1 = y2 = segment.Bottom;
      }
      else if (this.YValues[i - 1] >= 0.0 || segment.SegmentType == WaterfallSegmentType.Sum)
      {
        y1 = segment.Top;
        y2 = this.YValues[i] + segment.Top;
      }
      else
      {
        y1 = segment.Bottom;
        y2 = this.YValues[i] + segment.Bottom;
      }
    }
  }

  private void OnUpdateSumSegmentValues(
    WaterfallSegment segment,
    int i,
    bool isSum,
    double origin)
  {
    double num = !double.IsNaN(segment.YData) ? segment.YData : 0.0;
    if (i - 1 >= 0)
      segment.PreviousWaterfallSegment = this.Segments[i - 1] as WaterfallSegment;
    if (isSum || this.SummaryValues != null && i < this.SummaryValues.Count && this.SummaryValues[i])
    {
      segment.SegmentType = WaterfallSegmentType.Sum;
      segment.WaterfallSum = segment.PreviousWaterfallSegment == null ? num : segment.PreviousWaterfallSegment.Sum;
      if (i - 1 >= 0 && this.AllowAutoSum)
      {
        segment.Bottom = origin;
        segment.Top = segment.WaterfallSum;
      }
      else if (this.YValues[i] >= 0.0)
      {
        segment.Bottom = origin;
        segment.Top = this.YValues[i];
      }
      else if (double.IsNaN(this.YValues[i]))
      {
        segment.Bottom = segment.Top = origin;
      }
      else
      {
        segment.Top = origin;
        segment.Bottom = this.YValues[i];
      }
      segment.YRange = new DoubleRange(segment.Top, segment.Bottom);
    }
    else
      segment.SegmentType = this.YValues[i] >= 0.0 ? WaterfallSegmentType.Positive : WaterfallSegmentType.Negative;
    if (!this.AllowAutoSum && segment.SegmentType == WaterfallSegmentType.Sum)
      segment.Sum = num;
    else if (segment.PreviousWaterfallSegment != null && segment.SegmentType != WaterfallSegmentType.Sum)
      segment.Sum = num + segment.PreviousWaterfallSegment.Sum;
    else if (segment.PreviousWaterfallSegment != null)
      segment.Sum = segment.PreviousWaterfallSegment.Sum;
    else
      segment.Sum = num;
  }

  private void GetSummaryValues()
  {
    if (this.ItemsSource == null || string.IsNullOrEmpty(this.SummaryBindingPath))
      return;
    DataTable itemsSource = this.ItemsSource as DataTable;
    if (this.SummaryValues != null)
      this.SummaryValues.Clear();
    else
      this.SummaryValues = new List<bool>();
    IList<bool> summaryValues = (IList<bool>) this.SummaryValues;
    if (itemsSource == null)
    {
      IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
      IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
      if (!enumerator.MoveNext())
        return;
      PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.SummaryBindingPath);
      if (propertyInfo != (PropertyInfo) null)
        propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
      if (propertyAccessor == null)
        return;
      System.Func<object, object> getMethod = propertyAccessor.GetMethod;
      if (getMethod(enumerator.Current) != null)
      {
        if (getMethod(enumerator.Current).GetType().IsArray)
          return;
      }
      try
      {
        do
        {
          object obj = getMethod(enumerator.Current);
          summaryValues.Add(Convert.ToBoolean(obj));
        }
        while (enumerator.MoveNext());
      }
      catch
      {
      }
    }
    else
    {
      IEnumerator enumerator = itemsSource.Rows.GetEnumerator();
      if (!itemsSource.Columns.Contains(this.SummaryBindingPath) || !enumerator.MoveNext())
        return;
      do
      {
        object obj = (enumerator.Current as DataRow)[this.SummaryBindingPath];
        summaryValues.Add((bool) obj);
      }
      while (enumerator.MoveNext());
    }
  }
}
