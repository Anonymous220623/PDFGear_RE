// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColumnSeries
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
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ColumnSeries : XySegmentDraggingBase, ISegmentSelectable, ISegmentSpacing
{
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (ColumnSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ColumnSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (ColumnSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(ColumnSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (ColumnSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ColumnSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (ColumnSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ColumnSeries.OnCustomTemplateChanged)));
  private bool hasTemplate;
  private double initialHeight;
  private Rectangle previewRect;
  private bool isDragged;
  private Storyboard sb;

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(ColumnSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(ColumnSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(ColumnSeries.SelectedIndexProperty);
    set => this.SetValue(ColumnSeries.SelectedIndexProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(ColumnSeries.SegmentSpacingProperty);
    set => this.SetValue(ColumnSeries.SegmentSpacingProperty, (object) value);
  }

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(ColumnSeries.CustomTemplateProperty);
    set => this.SetValue(ColumnSeries.CustomTemplateProperty, (object) value);
  }

  protected internal override bool IsSideBySide => true;

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    double num1 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      num1 = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    if (xValues == null)
      return;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double num2 = sideBySideInfo.Delta / 2.0;
    int index1 = 0;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      this.GroupedActualData.Clear();
      for (int key = 0; key < this.DistinctValuesIndexes.Count; ++key)
      {
        List<List<double>> list = this.DistinctValuesIndexes[(double) key].Where<int>((Func<int, bool>) (index => this.GroupedSeriesYValues[0].Count > index)).Select<int, List<double>>((Func<int, List<double>>) (index => new List<double>()
        {
          this.GroupedSeriesYValues[0][index],
          (double) index
        })).OrderByDescending<List<double>, double>((Func<List<double>, double>) (val => val[0])).ToList<List<double>>();
        for (int index2 = 0; index2 < list.Count; ++index2)
        {
          double yValue = list[index2][0];
          double num3 = (double) key + sideBySideInfo.Start;
          double num4 = (double) key + sideBySideInfo.End;
          double num5 = yValue;
          double num6 = num1;
          this.GroupedActualData.Add(this.ActualData[(int) list[index2][1]]);
          this.CreateSegment(new double[4]
          {
            num3,
            num5,
            num4,
            num6
          }, xValues[index2], yValue, this.GroupedActualData[index1]);
          if (this.AdornmentsInfo != null)
          {
            switch (this.adornmentInfo.GetAdornmentPosition())
            {
              case AdornmentsPosition.Top:
                this.AddColumnAdornments((double) key, yValue, num3, num5, (double) index1, num2);
                break;
              case AdornmentsPosition.Bottom:
                this.AddColumnAdornments((double) key, yValue, num3, num6, (double) index1, num2);
                break;
              default:
                this.AddColumnAdornments((double) key, yValue, num3, num5 + (num6 - num5) / 2.0, (double) index1, num2);
                break;
            }
          }
          ++index1;
        }
      }
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      this.ClearUnUsedAdornments(this.DataCount);
      double start = sideBySideInfo.Start;
      double end = sideBySideInfo.End;
      List<int> intList = new List<int>();
      foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeries>) this.Area.Series)
        intList.Add(chartSeriesBase.DataCount);
      for (int index3 = 0; index3 < this.DataCount; ++index3)
      {
        if (index3 < this.DataCount)
        {
          double num7 = xValues[index3] + start;
          double num8 = xValues[index3] + end;
          double yvalue = this.YValues[index3];
          double num9 = num1;
          if (index3 < this.Segments.Count)
          {
            this.Segments[index3].SetData(num7, yvalue, num8, num9);
            (this.Segments[index3] as ColumnSegment).XData = xValues[index3];
            (this.Segments[index3] as ColumnSegment).YData = this.YValues[index3];
            (this.Segments[index3] as ColumnSegment).Item = this.ActualData[index3];
            if (this.SegmentColorPath != null && !this.Segments[index3].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index3].IsSelectedSegment)
              this.Segments[index3].Interior = this.Interior != null ? this.Interior : this.ColorValues[index3];
          }
          else
            this.CreateSegment(new double[4]
            {
              num7,
              yvalue,
              num8,
              num9
            }, xValues[index3], this.YValues[index3], this.ActualData[index3]);
          if (this.AdornmentsInfo != null)
          {
            switch (this.adornmentInfo.GetAdornmentPosition())
            {
              case AdornmentsPosition.Top:
                this.AddColumnAdornments(xValues[index3], this.YValues[index3], num7, yvalue, (double) index3, num2);
                continue;
              case AdornmentsPosition.Bottom:
                this.AddColumnAdornments(xValues[index3], this.YValues[index3], num7, num9, (double) index3, num2);
                continue;
              default:
                this.AddColumnAdornments(xValues[index3], this.YValues[index3], num7, yvalue + (num9 - yvalue) / 2.0, (double) index3, num2);
                continue;
            }
          }
        }
      }
      if (!this.ShowEmptyPoints)
        return;
      this.UpdateEmptyPointSegments(xValues, true);
    }
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
      double d = !(segment is EmptyPointSegment) || (segment as EmptyPointSegment).IsEmptySegmentInterior && this.EmptyPointStyle != EmptyPointStyle.SymbolAndInterior ? (this.IsActualTransposed ? ((ColumnSegment) segment).Width : ((ColumnSegment) segment).Height) : (this.IsActualTransposed ? ((EmptyPointSegment) segment).EmptyPointSymbolWidth : ((EmptyPointSegment) segment).EmptyPointSymbolHeight);
      if (!double.IsNaN(d) && !double.IsNaN(this.YValues[index1]))
      {
        if (renderedVisual == null)
          return;
        ScaleTransform scaleTransform = new ScaleTransform();
        renderedVisual.RenderTransform = (Transform) scaleTransform;
        if (this.CustomTemplate != null)
          scaleTransform.CenterY = segment.rect.Y + d;
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
          doubleAnimation.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 20.0 / 100.0));
          doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 80.0 / 100.0));
          DoubleAnimation element3 = doubleAnimation;
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

  internal override void ActivateDragging(Point mousePos, object element)
  {
    try
    {
      if (this.previewRect != null)
        return;
      if (this.CustomTemplate != null)
      {
        this.hasTemplate = true;
        base.ActivateDragging(mousePos, element);
        if (this.SegmentIndex >= 0)
          ;
      }
      else
      {
        if (!(element is Rectangle element1) || !(element1.Tag is ColumnSegment))
          return;
        base.ActivateDragging(mousePos, element);
        if (this.SegmentIndex < 0)
          return;
        this.initialHeight = Canvas.GetTop((UIElement) element1);
        SolidColorBrush fill = element1.Fill as SolidColorBrush;
        Rectangle rectangle = new Rectangle();
        rectangle.Fill = fill != null ? (Brush) new SolidColorBrush(Color.FromArgb(fill.Color.A, (byte) ((double) fill.Color.R * 0.6), (byte) ((double) fill.Color.G * 0.6), (byte) ((double) fill.Color.B * 0.6))) : element1.Fill;
        rectangle.Opacity = 0.5;
        rectangle.Stroke = element1.Stroke;
        rectangle.StrokeThickness = element1.StrokeThickness;
        this.previewRect = rectangle;
        this.previewRect.SetValue(Canvas.LeftProperty, (object) Canvas.GetLeft((UIElement) element1));
        this.previewRect.SetValue(Canvas.TopProperty, (object) this.initialHeight);
        this.previewRect.Height = element1.ActualHeight;
        this.previewRect.Width = element1.ActualWidth;
        this.SeriesPanel.Children.Add((UIElement) this.previewRect);
      }
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ColumnSegment toolTipTag = this.ToolTipTag as ColumnSegment;
    Point dataPointPosition = new Point();
    Point point = !this.IsActualTransposed ? this.ChartTransformer.TransformToVisible(toolTipTag.XRange.Median, toolTipTag.YData) : this.ChartTransformer.TransformToVisible(this.ActualXAxis.IsInversed ? toolTipTag.XRange.Start : toolTipTag.XRange.End, toolTipTag.YRange.Median);
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new ColumnSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new ColumnSeries()
    {
      CustomTemplate = this.CustomTemplate,
      SegmentSpacing = this.SegmentSpacing,
      SelectedIndex = this.SelectedIndex,
      SegmentSelectionBrush = this.SegmentSelectionBrush
    });
  }

  protected override void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    this.isDragged = false;
    this.hasTemplate = false;
    base.ResetDraggingElements(reason, dragEndEvent);
    if (this.previewRect == null)
      return;
    this.SeriesPanel.Children.Remove((UIElement) this.previewRect);
    this.previewRect = (Rectangle) null;
  }

  protected override void OnChartDragStart(Point mousePos, object originalSource)
  {
    this.ActivateDragging(mousePos, originalSource);
  }

  protected override void OnChartDragDelta(Point mousePos, object originalSource)
  {
    this.SegmentPreview(mousePos);
  }

  protected override void OnChartDragEnd(Point mousePos, object originalSource)
  {
    if (this.isDragged)
      this.UpdateDraggedSource();
    this.ResetDraggingElements("", false);
  }

  protected override void OnChartDragEntered(Point mousePos, object originalSource)
  {
    if (!this.EnableSegmentDragging)
      return;
    FrameworkElement rect = originalSource as FrameworkElement;
    ColumnSegment indexSegment = (ColumnSegment) null;
    if (rect != null)
    {
      if (rect.Tag is ColumnSegment)
        indexSegment = rect.Tag as ColumnSegment;
      else if (rect.TemplatedParent is ContentPresenter templatedParent && templatedParent.Content is ColumnSegment)
        indexSegment = templatedParent.Content as ColumnSegment;
    }
    if (indexSegment == null)
      return;
    double yvalue = this.YValues[this.Segments.IndexOf((ChartSegment) indexSegment)];
    if (this.IsActualTransposed)
      this.UpdateDragSpliter(rect, (ChartSegment) indexSegment, yvalue < 0.0 ? "Left" : "Right");
    else
      this.UpdateDragSpliter(rect, (ChartSegment) indexSegment, yvalue < 0.0 ? "Bottom" : "Top");
    base.OnChartDragEntered(mousePos, originalSource);
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
    (d as ColumnSeries).UpdateArea();
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

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ColumnSeries columnSeries = d as ColumnSeries;
    if (columnSeries.Area == null)
      return;
    columnSeries.Area.ScheduleUpdate();
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ColumnSeries columnSeries = d as ColumnSeries;
    if (columnSeries.Area == null)
      return;
    columnSeries.Segments.Clear();
    columnSeries.Area.ScheduleUpdate();
  }

  private void CreateSegment(double[] values, double xValue, double yValue, object actualData)
  {
    if (!(this.CreateSegment() is ColumnSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.SetData(values);
    segment.customTemplate = this.CustomTemplate;
    segment.XData = xValue;
    segment.YData = yValue;
    segment.Item = actualData;
    this.Segments.Add((ChartSegment) segment);
  }

  private void UpdateDraggedSource()
  {
    try
    {
      this.DraggedValue = this.GetSnapToPoint(this.DraggedValue);
      double yvalue = this.YValues[this.SegmentIndex];
      XyPreviewEndEventArgs args = new XyPreviewEndEventArgs()
      {
        BaseYValue = yvalue,
        NewYValue = this.DraggedValue
      };
      this.RaisePreviewEnd(args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("CaptureReleased", false);
      }
      else
      {
        this.YValues[this.SegmentIndex] = this.DraggedValue;
        if (this.UpdateSource && !this.IsSortData)
          this.UpdateUnderLayingModel(this.YBindingPath, this.SegmentIndex, (object) this.DraggedValue);
        this.UpdateArea();
        this.ResetDraggingElements("CaptureReleased", false);
        this.isDragged = false;
        this.RaiseDragEnd(new ChartDragEndEventArgs()
        {
          BaseYValue = yvalue,
          NewYValue = this.DraggedValue
        });
      }
    }
    catch
    {
      this.ResetDraggingElements("CaptureReleased", true);
    }
  }

  private void SegmentPreview(Point mousePos)
  {
    try
    {
      if (this.previewRect == null && !this.hasTemplate)
        return;
      this.DraggedValue = this.Area.PointToValue(this.ActualYAxis, mousePos);
      XySegmentDragEventArgs segmentDragEventArgs = new XySegmentDragEventArgs();
      segmentDragEventArgs.NewYValue = this.DraggedValue;
      segmentDragEventArgs.BaseYValue = this.YValues[this.SegmentIndex];
      segmentDragEventArgs.Segment = this.Segments[this.SegmentIndex];
      segmentDragEventArgs.Delta = this.GetActualDelta();
      XySegmentDragEventArgs args = segmentDragEventArgs;
      this.prevDraggedValue = this.DraggedValue;
      this.RaiseDragDelta((Syncfusion.UI.Xaml.Charts.DragDelta) args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("Cancel", true);
      }
      else
      {
        if (this.CustomTemplate != null)
        {
          ColumnSegment segment = this.Segments[this.SegmentIndex] as ColumnSegment;
          if (!this.IsActualTransposed)
          {
            double rectY = segment.RectY;
            double y = rectY > mousePos.Y ? mousePos.Y : rectY;
            this.UpdateSegmentDragValueToolTip(new Point(segment.RectX + segment.Width / 2.0, y), this.Segments[this.SegmentIndex], 0.0, this.DraggedValue, segment.Width / 2.0, 0.0);
          }
          else
          {
            double num = segment.RectX + segment.Width;
            this.UpdateSegmentDragValueToolTip(new Point(num < mousePos.X ? mousePos.X : num, segment.RectY + segment.Height / 2.0), this.Segments[this.SegmentIndex], 0.0, this.DraggedValue, 0.0, segment.Height / 2.0);
          }
        }
        else if (!this.IsActualTransposed)
        {
          double y1 = mousePos.Y;
          double num = Canvas.GetTop((UIElement) this.previewRect) - y1;
          if (y1 > this.Area.ValueToPoint(this.ActualYAxis, this.ActualXAxis.Origin))
          {
            this.previewRect.Height = Math.Abs(num);
          }
          else
          {
            this.previewRect.SetValue(Canvas.TopProperty, (object) y1);
            this.previewRect.Height += num;
          }
          double top = Canvas.GetTop(this.Segments[this.SegmentIndex].GetRenderedVisual());
          double y2 = top > mousePos.Y ? mousePos.Y : top + 20.0;
          this.UpdateSegmentDragValueToolTip(new Point(Canvas.GetLeft((UIElement) this.previewRect) + this.previewRect.Width / 2.0, y2), this.Segments[this.SegmentIndex], 0.0, this.DraggedValue, this.previewRect.Width / 2.0, 0.0);
        }
        else
        {
          double x = mousePos.X;
          double num1 = Canvas.GetLeft((UIElement) this.previewRect) - x;
          if (x > this.Area.ValueToPoint(this.ActualYAxis, this.ActualXAxis.Origin))
          {
            this.previewRect.SetValue(Canvas.LeftProperty, (object) Canvas.GetLeft((UIElement) this.previewRect));
            this.previewRect.Width = Math.Abs(num1);
          }
          else
          {
            this.previewRect.SetValue(Canvas.LeftProperty, (object) x);
            this.previewRect.Width += num1;
          }
          Rectangle renderedVisual = this.Segments[this.SegmentIndex].GetRenderedVisual() as Rectangle;
          double num2 = Canvas.GetLeft((UIElement) renderedVisual) + renderedVisual.Width;
          this.UpdateSegmentDragValueToolTip(new Point(num2 < mousePos.X ? mousePos.X : num2 + 20.0, Canvas.GetTop((UIElement) this.previewRect) + this.previewRect.Height / 2.0), this.Segments[this.SegmentIndex], 0.0, this.DraggedValue, 0.0, this.previewRect.Height / 2.0);
        }
        this.ResetDragSpliter();
        this.isDragged = true;
      }
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }
}
