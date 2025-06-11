// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XySeriesDraggingBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class XySeriesDraggingBase : XySegmentDraggingBase
{
  public static readonly DependencyProperty EnableSeriesDraggingProperty = DependencyProperty.Register(nameof (EnableSeriesDragging), typeof (bool), typeof (XySeriesDraggingBase), new PropertyMetadata((object) false, new PropertyChangedCallback(XySeriesDraggingBase.OnEnableDraggingChanged)));

  public bool EnableSeriesDragging
  {
    get => (bool) this.GetValue(XySeriesDraggingBase.EnableSeriesDraggingProperty);
    set => this.SetValue(XySeriesDraggingBase.EnableSeriesDraggingProperty, (object) value);
  }

  internal Ellipse DraggingPointIndicator { get; set; }

  internal UIElement PreviewSeries { get; set; }

  internal ChartSegment DraggingSegment { get; set; }

  internal virtual void UpdatePreivewSeriesDragging(Point mousePos)
  {
  }

  internal virtual void UpdatePreviewSegmentDragging(Point mousePos)
  {
  }

  internal override void ActivateDragging(Point mousePos, object element)
  {
    Keyboard.Focus((IInputElement) this);
    this.KeyDown += new KeyEventHandler(((XySegmentDraggingBase) this).CoreWindow_KeyDown);
    this.FindNearestChartPoint(mousePos, out double _, out double _, out double _);
    this.delta = 0.0;
    this.SegmentIndex = this.NearestSegmentIndex;
    if (this.SegmentIndex < 0)
      return;
    ChartDragStartEventArgs args = new ChartDragStartEventArgs()
    {
      BaseXValue = this.GetActualXValue(this.SegmentIndex)
    };
    if (this.EmptyPointIndexes != null)
    {
      List<int> emptyPointIndex = this.EmptyPointIndexes[0];
      for (int index = 0; index < emptyPointIndex.Count; ++index)
      {
        if (this.SegmentIndex == emptyPointIndex[index])
        {
          args.EmptyPoint = true;
          break;
        }
      }
    }
    this.RaiseDragStart(args);
    if (args.Cancel)
    {
      this.ResetDraggingElements("Cancel", true);
      this.SegmentIndex = -1;
    }
    else
    {
      foreach (ChartZoomPanBehavior chartZoomPanBehavior in this.Area.Behaviors.OfType<ChartZoomPanBehavior>())
      {
        chartZoomPanBehavior.InternalEnablePanning = false;
        chartZoomPanBehavior.InternalEnableSelectionZooming = false;
      }
    }
  }

  internal void UpdateSeriesDragValueToolTip(
    Point pos,
    Brush brush,
    double newValue,
    double baseValue,
    double offsetX)
  {
    if (!this.EnableDragTooltip)
      return;
    if (this.Tooltip == null)
    {
      this.DragInfo = (ChartDragPointinfo) new ChartDragSeriesInfo();
      this.Tooltip = new ContentControl();
      this.Tooltip.Content = (object) this.DragInfo;
      this.Tooltip.ContentTemplate = this.IsActualTransposed ? ChartDictionaries.GenericCommonDictionary[(object) "SeriesDragInfoHorizontal"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "SeriesDragInfoVertical"] as DataTemplate;
      this.SeriesPanel.Children.Add((UIElement) this.Tooltip);
    }
    if (this.IsActualTransposed)
    {
      double num = 50.0;
      double logPoint = this.Area.ValueToLogPoint(this.ActualXAxis, baseValue);
      ((ChartDragSeriesInfo) this.DragInfo).OffsetY = this.Tooltip.Width = Math.Abs(this.Area.ValueToLogPoint(this.ActualXAxis, newValue + baseValue) - logPoint);
      this.DragInfo.IsNegative = newValue >= 0.0;
      this.DragInfo.Delta = newValue;
      this.DragInfo.Brush = brush;
      if (this.DragInfo.IsNegative)
        Canvas.SetLeft((UIElement) this.Tooltip, offsetX);
      else
        Canvas.SetLeft((UIElement) this.Tooltip, offsetX - this.Tooltip.Width);
      Canvas.SetTop((UIElement) this.Tooltip, logPoint - num);
    }
    else
    {
      double logPoint1 = this.Area.ValueToLogPoint(this.ActualYAxis, baseValue);
      double logPoint2 = this.Area.ValueToLogPoint(this.ActualYAxis, newValue + baseValue);
      ((ChartDragSeriesInfo) this.DragInfo).OffsetY = this.Tooltip.Height = Math.Abs(logPoint1 - logPoint2);
      this.DragInfo.IsNegative = newValue <= 0.0;
      this.DragInfo.Delta = newValue;
      this.DragInfo.Brush = brush;
      if (this.DragInfo.IsNegative)
        Canvas.SetTop((UIElement) this.Tooltip, logPoint1);
      else
        Canvas.SetTop((UIElement) this.Tooltip, logPoint1 - this.Tooltip.Height);
      Canvas.SetLeft((UIElement) this.Tooltip, offsetX);
    }
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (this.EnableSeriesDragging)
      this.OnChartDragDelta(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    else
      base.OnMouseMove(e);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.EnableSeriesDragging)
    {
      this.SeriesPanel.CaptureMouse();
      this.OnChartDragStart(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    }
    else
      base.OnMouseLeftButtonDown(e);
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    this.SeriesPanel.ReleaseMouseCapture();
    if (this.EnableSeriesDragging)
      this.OnChartDragEnd(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    else
      base.OnMouseLeftButtonUp(e);
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    if (this.EnableSeriesDragging)
      this.OnChartDragEntered(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    else
      base.OnMouseEnter(e);
    base.OnMouseEnter(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.EnableSeriesDragging)
      this.OnChartDragExited(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    else
      base.OnMouseLeave(e);
    base.OnMouseLeave(e);
  }

  protected override void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    this.ResetDraggingindicators();
    base.ResetDraggingElements(reason, dragEndEvent);
  }

  protected override void OnChartDragDelta(Point mousePos, object originalSource)
  {
    if (this.PreviewSeries != null)
    {
      this.ResetDraggingindicators();
      this.UpdatePreivewSeriesDragging(mousePos);
    }
    else
    {
      if (this.DraggingSegment == null)
        return;
      this.ResetDraggingindicators();
      this.UpdatePreviewSegmentDragging(mousePos);
    }
  }

  protected override void OnChartDragEntered(Point mousePos, object originalSource)
  {
    FrameworkElement frameworkElement = originalSource as FrameworkElement;
    if ((this.EnableSegmentDragging || this.EnableSeriesDragging) && frameworkElement != null && (frameworkElement.Tag is ChartSegment || frameworkElement.DataContext is ChartAdornmentContainer))
      this.UpdatePreviewIndicatorPosition(mousePos);
    base.OnChartDragEntered(mousePos, originalSource);
  }

  protected override void OnChartDragExited(Point mousePos, object originalSource)
  {
    if (this.EnableSegmentDragging || this.EnableSeriesDragging)
      this.ResetDraggingindicators();
    base.OnChartDragExited(mousePos, originalSource);
  }

  protected void UpdateUnderLayingModel(string path, IList<double> updatedDatas)
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, path);
    IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
    if (propertyInfo != (PropertyInfo) null)
      propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
    int index = 0;
    do
    {
      propertyAccessor.SetValue(enumerator.Current, (object) updatedDatas[index]);
      ++index;
    }
    while (enumerator.MoveNext());
  }

  private static void OnEnableDraggingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue)
      return;
    ((XySegmentDraggingBase) d).ResetDraggingElements("OnPropertyChanged", false);
  }

  private void AddSegmentIndicator()
  {
    Ellipse ellipse = new Ellipse();
    ellipse.Height = 15.0;
    ellipse.Width = 15.0;
    ellipse.Fill = this.Segments[this.SegmentIndex == 0 ? 0 : this.SegmentIndex - 1].Interior;
    this.DraggingPointIndicator = ellipse;
    this.SeriesPanel.Children.Add((UIElement) this.DraggingPointIndicator);
  }

  private void UpdatePreviewIndicatorPosition(Point mousePos)
  {
    if (this.PreviewSeries != null || this.DraggingSegment != null || !this.EnableSegmentDragging)
      return;
    double x;
    double y;
    this.FindNearestChartPoint(mousePos, out x, out y, out double _);
    if (double.IsNaN(y))
      return;
    int segmentIndex = this.SegmentIndex;
    this.SegmentIndex = this.NearestSegmentIndex;
    if (segmentIndex != this.SegmentIndex)
      this.prevDraggedValue = 0.0;
    XySegmentEnterEventArgs args = new XySegmentEnterEventArgs()
    {
      XValue = this.GetActualXValue(this.SegmentIndex),
      SegmentIndex = this.SegmentIndex,
      CanDrag = true,
      YValue = (object) this.YValues[this.SegmentIndex]
    };
    this.RaiseDragEnter(args);
    if (!args.CanDrag)
      return;
    double logPoint1 = this.Area.ValueToLogPoint(this.ActualYAxis, y);
    double logPoint2 = this.Area.ValueToLogPoint(this.ActualXAxis, x);
    if (this.AdornmentsInfo == null)
    {
      if (this.DraggingPointIndicator == null)
        this.AddSegmentIndicator();
      if (this.IsActualTransposed)
      {
        Canvas.SetTop((UIElement) this.DraggingPointIndicator, logPoint2 - this.DraggingPointIndicator.Width / 2.0);
        Canvas.SetLeft((UIElement) this.DraggingPointIndicator, logPoint1 - this.DraggingPointIndicator.Height / 2.0);
      }
      else
      {
        Canvas.SetLeft((UIElement) this.DraggingPointIndicator, logPoint2 - this.DraggingPointIndicator.Width / 2.0);
        Canvas.SetTop((UIElement) this.DraggingPointIndicator, logPoint1 - this.DraggingPointIndicator.Height / 2.0);
      }
      this.DraggingPointIndicator.Tag = Math.Abs(this.Segments.Count - this.SegmentIndex) > 0 ? (object) this.Segments[this.SegmentIndex] : (object) this.Segments[this.SegmentIndex - 1];
      this.AddAnimationEllipse(ChartDictionaries.GenericSymbolDictionary[(object) "AnimationEllipse"] as ControlTemplate, this.DraggingPointIndicator.Height, this.DraggingPointIndicator.Width, logPoint2, logPoint1, (object) null, false);
    }
    else
    {
      if (this.AdornmentsInfo == null || this.AdornmentsInfo.Symbol == ChartSymbol.Custom)
        return;
      this.AddAnimationEllipse(ChartDictionaries.GenericSymbolDictionary[(object) ("Animation" + this.AdornmentsInfo.Symbol.ToString())] as ControlTemplate, this.AdornmentsInfo.SymbolHeight, this.AdornmentsInfo.SymbolWidth, logPoint2, logPoint1, (object) null, true);
    }
  }

  private void ResetDraggingindicators()
  {
    if (this.DraggingPointIndicator != null)
    {
      this.SeriesPanel.Children.Remove((UIElement) this.DraggingPointIndicator);
      this.DraggingPointIndicator = (Ellipse) null;
    }
    if (this.AnimationElement != null && this.SeriesPanel.Children.Contains((UIElement) this.AnimationElement))
    {
      this.SeriesPanel.Children.Remove((UIElement) this.AnimationElement);
      this.AnimationElement = (FrameworkElement) null;
    }
    this.DraggingPointIndicator = (Ellipse) null;
  }
}
