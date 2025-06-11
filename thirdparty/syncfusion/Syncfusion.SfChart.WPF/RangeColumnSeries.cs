// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeColumnSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RangeColumnSeries : RangeSegmentDraggingBase, ISegmentSelectable, ISegmentSpacing
{
  public static readonly DependencyProperty CustomTemplateProperty = DependencyProperty.Register(nameof (CustomTemplate), typeof (DataTemplate), typeof (RangeColumnSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeColumnSeries.OnCustomTemplateChanged)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (RangeColumnSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(RangeColumnSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (RangeColumnSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeColumnSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (RangeColumnSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(RangeColumnSeries.OnSelectedIndexChanged)));
  private double initialHeight;
  private double initialValue;
  private int draggingMode;
  private Rectangle previewRect;
  private RangeColumnSegment selectedSegment;
  private bool dragged;
  private Storyboard sb;

  public DataTemplate CustomTemplate
  {
    get => (DataTemplate) this.GetValue(RangeColumnSeries.CustomTemplateProperty);
    set => this.SetValue(RangeColumnSeries.CustomTemplateProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(RangeColumnSeries.SegmentSpacingProperty);
    set => this.SetValue(RangeColumnSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(RangeColumnSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(RangeColumnSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(RangeColumnSeries.SelectedIndexProperty);
    set => this.SetValue(RangeColumnSeries.SelectedIndexProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired
  {
    get => !string.IsNullOrEmpty(this.High) && !string.IsNullOrEmpty(this.Low);
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
    if (xValues == null)
      return;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (index < xValues.Count && this.GroupedSeriesYValues[0].Count > index)
        {
          double num1 = xValues[index] + sideBySideInfo.Start;
          double num2 = xValues[index] + sideBySideInfo.End;
          double high = this.GroupedSeriesYValues[0][index];
          double low = this.IsMultipleYPathRequired ? this.GroupedSeriesYValues[1][index] : 0.0;
          if (this.CreateSegment() is RangeColumnSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.SetData(num1, high, num2, low);
            segment.customTemplate = this.CustomTemplate;
            segment.Item = this.ActualData[index];
            segment.High = this.GroupedSeriesYValues[0][index];
            segment.Low = this.IsMultipleYPathRequired ? this.GroupedSeriesYValues[1][index] : 0.0;
            this.Segments.Add((ChartSegment) segment);
          }
          if (this.AdornmentsInfo != null)
            this.AddAdornments(xValues[index], sideBySideInfo.Start + sideBySideInfo.Delta / 2.0, high, low, index);
        }
      }
    }
    else
    {
      if (this.Segments.Count > this.DataCount)
        this.ClearUnUsedSegments(this.DataCount);
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 2);
        else
          this.ClearUnUsedAdornments(this.DataCount);
      }
      for (int index = 0; index < this.DataCount; ++index)
      {
        double num3 = xValues[index] + sideBySideInfo.Start;
        double num4 = xValues[index] + sideBySideInfo.End;
        double highValue = this.HighValues[index];
        double low = this.IsMultipleYPathRequired ? this.LowValues[index] : 0.0;
        if (index < this.Segments.Count)
        {
          this.Segments[index].Item = this.ActualData[index];
          this.Segments[index].SetData(num3, highValue, num4, low);
          (this.Segments[index] as RangeColumnSegment).High = highValue;
          (this.Segments[index] as RangeColumnSegment).Low = low;
          if (this.SegmentColorPath != null && !this.Segments[index].IsEmptySegmentInterior && this.ColorValues.Count > 0 && !this.Segments[index].IsSelectedSegment)
            this.Segments[index].Interior = this.Interior != null ? this.Interior : this.ColorValues[index];
        }
        else if (this.CreateSegment() is RangeColumnSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(num3, highValue, num4, low);
          segment.customTemplate = this.CustomTemplate;
          segment.Item = this.ActualData[index];
          segment.High = highValue;
          segment.Low = low;
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], sideBySideInfo.Start + sideBySideInfo.Delta / 2.0, highValue, low, index);
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, true);
  }

  internal override bool GetAnimationIsActive()
  {
    return this.sb != null && this.sb.GetCurrentState() == ClockState.Active;
  }

  internal override void Animate()
  {
    int index1 = 0;
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
      renderedVisual.RenderTransform = (Transform) new ScaleTransform();
      double d = !(segment is EmptyPointSegment) || (segment as EmptyPointSegment).IsEmptySegmentInterior && this.EmptyPointStyle != EmptyPointStyle.SymbolAndInterior ? (this.IsActualTransposed ? ((ColumnSegment) segment).Width : ((ColumnSegment) segment).Height) : (this.IsActualTransposed ? ((EmptyPointSegment) segment).EmptyPointSymbolWidth : ((EmptyPointSegment) segment).EmptyPointSymbolHeight);
      if (!double.IsNaN(d))
      {
        renderedVisual.RenderTransformOrigin = new Point(0.5, 0.5);
        DoubleAnimationUsingKeyFrames element1 = new DoubleAnimationUsingKeyFrames();
        SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
        keyFrame1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
        keyFrame1.Value = 0.0;
        element1.KeyFrames.Add((DoubleKeyFrame) keyFrame1);
        SplineDoubleKeyFrame keyFrame2 = new SplineDoubleKeyFrame();
        keyFrame2.KeyTime = KeyTime.FromTimeSpan(this.AnimationDuration);
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
          AdornmentsPosition adornmentPosition = this.adornmentInfo.GetAdornmentPosition();
          for (int index2 = adornmentPosition == AdornmentsPosition.TopAndBottom ? 0 : 1; index2 < 2; ++index2)
          {
            FrameworkElement labelPresenter = this.AdornmentsInfo.LabelPresenters[index1];
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
            switch (adornmentPosition)
            {
              case AdornmentsPosition.Top:
                keyFrame3.Value = d * 10.0 / 100.0;
                break;
              case AdornmentsPosition.Bottom:
                keyFrame3.Value = -(d * 10.0) / 100.0;
                break;
              default:
                keyFrame3.Value = index1 % 2 == 0 ? d * 10.0 / 100.0 : -(d * 10.0) / 100.0;
                break;
            }
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
            element3.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(this.AnimationDuration.TotalSeconds * 20.0 / 100.0));
            Storyboard.SetTarget((DependencyObject) element3, (DependencyObject) labelPresenter);
            Storyboard.SetTargetProperty((DependencyObject) element3, new PropertyPath((object) UIElement.OpacityProperty));
            this.sb.Children.Add((Timeline) element3);
            ++index1;
          }
        }
      }
    }
    this.sb.Begin();
  }

  internal override void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    base.ResetDraggingElements(reason, dragEndEvent);
    if (this.SeriesPanel.Children.Contains((UIElement) this.previewRect))
      this.SeriesPanel.Children.Remove((UIElement) this.previewRect);
    this.previewRect = (Rectangle) null;
    this.draggingMode = -1;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    RangeColumnSegment toolTipTag = this.ToolTipTag as RangeColumnSegment;
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new RangeColumnSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new RangeColumnSeries()
    {
      SelectedIndex = this.SelectedIndex,
      SegmentSpacing = this.SegmentSpacing,
      SegmentSelectionBrush = this.SegmentSelectionBrush
    });
  }

  protected override void OnChartDragStart(Point mousePos, object originalSource)
  {
    if (this.EnableSegmentDragging)
      this.ActivateDragging(mousePos, originalSource);
    base.OnChartDragStart(mousePos, originalSource);
  }

  protected override void OnChartDragEnd(Point mousePos, object originalSource)
  {
    if (this.dragged)
      this.UpdateDraggedSource();
    this.ResetDraggingElements("", false);
    base.OnChartDragEnd(mousePos, originalSource);
  }

  protected override void OnChartDragEntered(Point mousePos, object originalSource)
  {
    if (originalSource is Shape shape && shape.Tag is RangeColumnSegment && originalSource is Rectangle rect)
    {
      this.UpdateDragSpliterHigh(rect);
      this.UpdateDragSpliterLow(rect);
    }
    base.OnChartDragEntered(mousePos, originalSource);
  }

  protected override void OnChartDragDelta(Point mousePos, object originalSource)
  {
    if (this.EnableSegmentDragging)
      this.SegmentPreview(mousePos);
    base.OnChartDragDelta(mousePos, originalSource);
  }

  protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    double num = (Right - Left) * spacing / 2.0;
    Left += num;
    Right -= num;
    return Left;
  }

  private static void OnCustomTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    RangeColumnSeries rangeColumnSeries = d as RangeColumnSeries;
    if (rangeColumnSeries.Area == null)
      return;
    rangeColumnSeries.Segments.Clear();
    rangeColumnSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    RangeColumnSeries rangeColumnSeries = d as RangeColumnSeries;
    if (rangeColumnSeries.Area == null)
      return;
    rangeColumnSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as RangeColumnSeries).UpdateArea();
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

  private int GetSegmentMousePosition(RangeColumnSegment rangeColumnSegment, Point mousePos)
  {
    double high = rangeColumnSegment.High;
    double low = rangeColumnSegment.Low;
    double num1 = this.Area.PointToValue(this.ActualYAxis, mousePos);
    double num2 = Math.Abs(high - low) * 25.0 / 100.0;
    if (low < high)
    {
      double num3 = high - num2;
      double num4 = low + num2;
      if (num1 > num3 && high > num1)
        return 1;
      return num1 < num4 && num1 > low ? 2 : 3;
    }
    double num5 = high + num2;
    double num6 = low - num2;
    if (num1 < num5 && high < num1)
      return 1;
    return num1 > num6 && num1 < low ? 2 : 3;
  }

  private void SegmentPreview(Point mousePos)
  {
    try
    {
      if (this.previewRect == null)
        return;
      this.DraggedValue = this.Area.PointToValue(this.ActualYAxis, mousePos);
      double num1 = this.IsActualTransposed ? mousePos.X : mousePos.Y;
      Rectangle renderedVisual = this.selectedSegment.GetRenderedVisual() as Rectangle;
      double num2 = 0.0;
      double num3 = 0.0;
      double newValue1 = double.NaN;
      double newValue2 = double.NaN;
      this.ResetDragSpliter();
      this.dragged = true;
      if (this.IsActualTransposed)
      {
        switch (this.draggingMode)
        {
          case 1:
            double left1 = Canvas.GetLeft((UIElement) renderedVisual);
            double num4 = left1 - num1;
            if (num1 < left1)
            {
              this.previewRect.SetValue(Canvas.LeftProperty, (object) (left1 - num4));
              this.previewRect.Width = Math.Abs(num4);
            }
            else
            {
              this.previewRect.SetValue(Canvas.LeftProperty, (object) left1);
              this.previewRect.Width = Math.Abs(num4);
            }
            newValue1 = this.DraggedValue;
            newValue2 = double.NaN;
            this.UpdateSegmentDragValueToolTipHigh(new Point(mousePos.X, Canvas.GetTop((UIElement) this.previewRect) + this.previewRect.Height / 2.0), this.Segments[this.SegmentIndex], this.DraggedValue, this.previewRect.Height / 2.0);
            break;
          case 2:
            if (renderedVisual != null)
              num3 = Canvas.GetLeft((UIElement) renderedVisual) + renderedVisual.ActualWidth;
            double num5 = num3 - num1;
            if (num1 < num3)
            {
              this.previewRect.SetValue(Canvas.LeftProperty, (object) (num3 - num5));
              this.previewRect.Width = Math.Abs(num5);
            }
            else
            {
              this.previewRect.SetValue(Canvas.LeftProperty, (object) num3);
              this.previewRect.Width = Math.Abs(num5);
            }
            newValue2 = this.DraggedValue;
            newValue1 = double.NaN;
            this.UpdateSegmentDragValueToolTipLow(new Point(mousePos.X, Canvas.GetTop((UIElement) this.previewRect) + this.previewRect.Height / 2.0), this.Segments[this.SegmentIndex], this.DraggedValue);
            break;
          case 3:
            double left2 = Canvas.GetLeft((UIElement) this.previewRect);
            double num6 = mousePos.X - this.initialHeight;
            this.previewRect.SetValue(Canvas.LeftProperty, (object) (left2 + num6));
            this.initialHeight = mousePos.X;
            newValue1 = this.HighValues[this.SegmentIndex] + this.DraggedValue - this.initialValue;
            newValue2 = this.LowValues[this.SegmentIndex] + this.DraggedValue - this.initialValue;
            this.UpdateSegmentDragValueToolTipHigh(new Point(Canvas.GetLeft((UIElement) this.previewRect) + this.previewRect.Width, Canvas.GetTop((UIElement) this.previewRect) + this.previewRect.Height / 2.0), this.Segments[this.SegmentIndex], newValue1, this.previewRect.Width / 2.0);
            this.UpdateSegmentDragValueToolTipLow(new Point(Canvas.GetLeft((UIElement) this.previewRect), Canvas.GetTop((UIElement) this.previewRect) + this.previewRect.Height / 2.0), this.Segments[this.SegmentIndex], newValue2);
            break;
        }
      }
      else
      {
        switch (this.draggingMode)
        {
          case 1:
            if (renderedVisual != null)
              num2 = Canvas.GetTop((UIElement) renderedVisual) + renderedVisual.ActualHeight;
            double num7 = num2 - num1;
            if (num1 < num2)
            {
              this.previewRect.SetValue(Canvas.TopProperty, (object) (num2 - num7));
              this.previewRect.Height = Math.Abs(num7);
            }
            else
            {
              this.previewRect.SetValue(Canvas.TopProperty, (object) num2);
              this.previewRect.Height = Math.Abs(num7);
            }
            newValue1 = this.DraggedValue;
            newValue2 = double.NaN;
            this.UpdateSegmentDragValueToolTipHigh(new Point(Canvas.GetLeft((UIElement) this.previewRect) + this.previewRect.Width / 2.0, mousePos.Y), this.Segments[this.SegmentIndex], this.DraggedValue, this.previewRect.Width / 2.0);
            break;
          case 2:
            double top1 = Canvas.GetTop((UIElement) renderedVisual);
            double num8 = top1 - num1;
            if (num1 < top1)
            {
              this.previewRect.SetValue(Canvas.TopProperty, (object) (top1 - num8));
              this.previewRect.Height = Math.Abs(num8);
            }
            else
            {
              this.previewRect.SetValue(Canvas.TopProperty, (object) top1);
              this.previewRect.Height = Math.Abs(num8);
            }
            newValue2 = this.DraggedValue;
            newValue1 = double.NaN;
            this.UpdateSegmentDragValueToolTipLow(new Point(Canvas.GetLeft((UIElement) this.previewRect) + this.previewRect.Width / 2.0, mousePos.Y), this.Segments[this.SegmentIndex], this.DraggedValue);
            break;
          case 3:
            double top2 = Canvas.GetTop((UIElement) this.previewRect);
            double num9 = this.initialHeight - mousePos.Y;
            this.previewRect.SetValue(Canvas.TopProperty, (object) (top2 - num9));
            this.initialHeight = mousePos.Y;
            newValue1 = this.HighValues[this.SegmentIndex] + this.DraggedValue - this.initialValue;
            newValue2 = this.LowValues[this.SegmentIndex] + this.DraggedValue - this.initialValue;
            this.UpdateSegmentDragValueToolTipHigh(new Point(Canvas.GetLeft((UIElement) this.previewRect) + this.previewRect.Width / 2.0, Canvas.GetTop((UIElement) this.previewRect)), this.Segments[this.SegmentIndex], newValue1, this.previewRect.Width / 2.0);
            this.UpdateSegmentDragValueToolTipLow(new Point(Canvas.GetLeft((UIElement) this.previewRect) + this.previewRect.Width / 2.0, Canvas.GetTop((UIElement) this.previewRect) + this.previewRect.Height), this.Segments[this.SegmentIndex], newValue2);
            break;
        }
      }
      RangeDragEventArgs rangeDragEventArgs = new RangeDragEventArgs();
      rangeDragEventArgs.NewHighValue = newValue1;
      rangeDragEventArgs.NewLowValue = newValue2;
      rangeDragEventArgs.BaseHighValue = this.HighValues[this.SegmentIndex];
      rangeDragEventArgs.BaseLowValue = this.LowValues[this.SegmentIndex];
      RangeDragEventArgs args = rangeDragEventArgs;
      this.RaiseDragDelta(args);
      if (!args.Cancel)
        return;
      this.ResetDraggingElements("Cancel", true);
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }

  private void ActivateDragging(Point mousePos, object element)
  {
    try
    {
      if (this.previewRect != null || !(element is Rectangle element1) || !this.EnableSegmentDragging || !(element1.Tag is RangeColumnSegment tag))
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
      this.SegmentIndex = this.Segments.IndexOf(element1.Tag as ChartSegment);
      this.draggingMode = this.GetSegmentMousePosition(tag, mousePos);
      this.selectedSegment = tag;
      this.initialHeight = this.IsActualTransposed ? mousePos.X : mousePos.Y;
      this.initialValue = this.Area.PointToValue(this.ActualYAxis, mousePos);
      ChartDragStartEventArgs args = new ChartDragStartEventArgs()
      {
        BaseXValue = this.GetActualXValue(this.SegmentIndex)
      };
      this.RaiseDragStart(args);
      if (args.Cancel)
        this.ResetDraggingElements("Cancel", true);
      Keyboard.Focus((IInputElement) this);
      this.UnHoldPanning(false);
    }
    catch
    {
      this.ResetDraggingElements("Exception", true);
    }
  }

  private void UpdateDraggedSource()
  {
    try
    {
      double num1 = this.HighValues[this.SegmentIndex];
      double num2 = this.LowValues[this.SegmentIndex];
      double num3 = num1;
      double num4 = num2;
      this.DraggedValue = this.GetSnapToPoint(this.DraggedValue);
      double snapToPoint = this.GetSnapToPoint(this.DraggedValue - this.initialValue);
      switch (this.draggingMode)
      {
        case 1:
          num1 = this.DraggedValue;
          break;
        case 2:
          num2 = this.DraggedValue;
          break;
        case 3:
          num1 = this.GetSnapToPoint(this.HighValues[this.SegmentIndex] + snapToPoint);
          num2 = this.GetSnapToPoint(this.LowValues[this.SegmentIndex] + snapToPoint);
          break;
      }
      RangeDragEventArgs rangeDragEventArgs = new RangeDragEventArgs();
      rangeDragEventArgs.BaseHighValue = num3;
      rangeDragEventArgs.BaseLowValue = num4;
      rangeDragEventArgs.NewHighValue = num1;
      rangeDragEventArgs.NewLowValue = num2;
      RangeDragEventArgs args = rangeDragEventArgs;
      this.RaisePreviewEnd(args);
      if (args.Cancel)
      {
        this.ResetDraggingElements("", false);
      }
      else
      {
        this.HighValues[this.SegmentIndex] = num1;
        this.LowValues[this.SegmentIndex] = num2;
        if (this.UpdateSource && !this.IsSortData)
        {
          this.UpdateUnderLayingModel(this.Low, this.SegmentIndex, (object) this.LowValues[this.SegmentIndex]);
          this.UpdateUnderLayingModel(this.High, this.SegmentIndex, (object) this.HighValues[this.SegmentIndex]);
        }
        this.dragged = false;
        this.UpdateArea();
        this.RaiseDragEnd(new RangeDragEndEventArgs()
        {
          BaseHighValue = num3,
          BaseLowValue = num4,
          NewHighValue = num1,
          NewLowValue = num2
        });
      }
    }
    catch
    {
      this.ResetDraggingElements("", false);
    }
  }
}
