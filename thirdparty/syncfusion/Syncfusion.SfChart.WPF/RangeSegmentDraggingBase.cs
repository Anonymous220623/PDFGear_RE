// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeSegmentDraggingBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class RangeSegmentDraggingBase : RangeSeriesBase
{
  public static readonly DependencyProperty EnableDragTooltipProperty = DependencyProperty.Register(nameof (EnableDragTooltip), typeof (bool), typeof (RangeSegmentDraggingBase), new PropertyMetadata((object) true));
  public static readonly DependencyProperty DragTooltipTemplateProperty = DependencyProperty.Register(nameof (DragTooltipTemplate), typeof (DataTemplate), typeof (RangeSegmentDraggingBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RoundToDecimalProperty = DependencyProperty.Register(nameof (RoundToDecimal), typeof (int), typeof (RangeSegmentDraggingBase), new PropertyMetadata((object) 0));
  public static readonly DependencyProperty SnapToPointProperty = DependencyProperty.Register(nameof (SnapToPoint), typeof (SnapToPoint), typeof (RangeSegmentDraggingBase), new PropertyMetadata((object) SnapToPoint.None));
  public static readonly DependencyProperty EnableSegmentDraggingProperty = DependencyProperty.Register(nameof (EnableSegmentDragging), typeof (bool), typeof (RangeSegmentDraggingBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty UpdateSourceProperty = DependencyProperty.Register(nameof (UpdateSource), typeof (bool), typeof (RangeSegmentDraggingBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty DragCancelKeyModifiersProperty = DependencyProperty.Register(nameof (DragCancelKeyModifiers), typeof (ModifierKeys), typeof (RangeSegmentDraggingBase), new PropertyMetadata((object) ModifierKeys.None));
  private ContentControl highTooltip;
  private ContentControl lowTooltip;
  private ChartDragPointinfo highDragInfo;
  private ChartDragPointinfo lowDragInfo;

  public event EventHandler<RangeSegmentEnterEventArgs> SegmentEnter;

  public event EventHandler<ChartDragStartEventArgs> DragStart;

  public event EventHandler<RangeDragEventArgs> DragDelta;

  public event EventHandler<RangeDragEndEventArgs> DragEnd;

  public event EventHandler<RangeDragEventArgs> PreviewDragEnd;

  public bool EnableDragTooltip
  {
    get => (bool) this.GetValue(RangeSegmentDraggingBase.EnableDragTooltipProperty);
    set => this.SetValue(RangeSegmentDraggingBase.EnableDragTooltipProperty, (object) value);
  }

  public DataTemplate DragTooltipTemplate
  {
    get => (DataTemplate) this.GetValue(RangeSegmentDraggingBase.DragTooltipTemplateProperty);
    set => this.SetValue(RangeSegmentDraggingBase.DragTooltipTemplateProperty, (object) value);
  }

  public int RoundToDecimal
  {
    get => (int) this.GetValue(RangeSegmentDraggingBase.RoundToDecimalProperty);
    set => this.SetValue(RangeSegmentDraggingBase.RoundToDecimalProperty, (object) value);
  }

  public SnapToPoint SnapToPoint
  {
    get => (SnapToPoint) this.GetValue(RangeSegmentDraggingBase.SnapToPointProperty);
    set => this.SetValue(RangeSegmentDraggingBase.SnapToPointProperty, (object) value);
  }

  public bool EnableSegmentDragging
  {
    get => (bool) this.GetValue(RangeSegmentDraggingBase.EnableSegmentDraggingProperty);
    set => this.SetValue(RangeSegmentDraggingBase.EnableSegmentDraggingProperty, (object) value);
  }

  public bool UpdateSource
  {
    get => (bool) this.GetValue(RangeSegmentDraggingBase.UpdateSourceProperty);
    set => this.SetValue(RangeSegmentDraggingBase.UpdateSourceProperty, (object) value);
  }

  public ModifierKeys DragCancelKeyModifiers
  {
    get => (ModifierKeys) this.GetValue(RangeSegmentDraggingBase.DragCancelKeyModifiersProperty);
    set => this.SetValue(RangeSegmentDraggingBase.DragCancelKeyModifiersProperty, (object) value);
  }

  protected int SegmentIndex { get; set; }

  protected ContentControl DragSpliterHigh { get; set; }

  protected ContentControl DragSpliterLow { get; set; }

  protected double DraggedValue { get; set; }

  internal virtual void ResetDragSpliter()
  {
    if (this.DragSpliterHigh != null)
    {
      this.SeriesPanel.Children.Remove((UIElement) this.DragSpliterHigh);
      this.DragSpliterHigh = (ContentControl) null;
    }
    if (this.DragSpliterLow == null)
      return;
    this.SeriesPanel.Children.Remove((UIElement) this.DragSpliterLow);
    this.DragSpliterLow = (ContentControl) null;
  }

  internal virtual void ResetDraggingElements(string reason, bool dragEndEvent)
  {
    this.KeyDown -= new KeyEventHandler(this.CoreWindow_KeyDown);
    this.UnHoldPanning(true);
    if (dragEndEvent)
      this.RaiseDragEnd(new RangeDragEndEventArgs());
    this.ResetSegmentDragTooltipInfo();
  }

  internal double GetSnapToPoint(double actualValue)
  {
    double snapToPoint = actualValue;
    switch (this.SnapToPoint)
    {
      case SnapToPoint.Round:
        snapToPoint = Math.Round(actualValue, this.RoundToDecimal);
        break;
      case SnapToPoint.Floor:
        snapToPoint = Math.Floor(actualValue);
        break;
      case SnapToPoint.Ceil:
        snapToPoint = Math.Ceiling(actualValue);
        break;
    }
    return snapToPoint;
  }

  internal void UnHoldPanning(bool value)
  {
    foreach (ChartZoomPanBehavior chartZoomPanBehavior in this.Area.Behaviors.OfType<ChartZoomPanBehavior>())
    {
      chartZoomPanBehavior.InternalEnablePanning = value;
      chartZoomPanBehavior.InternalEnableSelectionZooming = value;
    }
  }

  internal void UpdateSegmentDragValueToolTipHigh(
    Point pos,
    ChartSegment segment,
    double newValue,
    double offsetY)
  {
    if (!this.EnableDragTooltip)
      return;
    if (this.highTooltip == null)
    {
      ChartDragSegmentInfo chartDragSegmentInfo = new ChartDragSegmentInfo();
      chartDragSegmentInfo.PostfixLabelTemplate = this.ActualYAxis.PostfixLabelTemplate;
      chartDragSegmentInfo.PrefixLabelTemplate = this.ActualYAxis.PostfixLabelTemplate;
      this.highDragInfo = (ChartDragPointinfo) chartDragSegmentInfo;
      this.highTooltip = new ContentControl()
      {
        Content = (object) this.highDragInfo
      };
      this.SeriesPanel.Children.Add((UIElement) this.highTooltip);
      this.highTooltip.ContentTemplate = this.DragTooltipTemplate != null ? this.DragTooltipTemplate : (this.IsActualTransposed ? ChartDictionaries.GenericCommonDictionary[(object) "SegmentDragInfoOppRight"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "SegmentDragInfo"] as DataTemplate);
    }
    this.highDragInfo.Segment = segment;
    this.highDragInfo.Brush = segment.Interior;
    this.highDragInfo.ScreenCoordinates = pos;
    ((ChartDragSegmentInfo) this.highDragInfo).NewValue = (object) newValue;
    this.highDragInfo.Segment = segment;
    this.highTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.IsActualTransposed)
    {
      if (pos.X < 0.0)
      {
        Canvas.SetTop((UIElement) this.highTooltip, pos.Y - this.highTooltip.DesiredSize.Height / 2.0);
        Canvas.SetLeft((UIElement) this.highTooltip, 0.0);
      }
      else
      {
        Canvas.SetTop((UIElement) this.highTooltip, pos.Y - this.highTooltip.DesiredSize.Height / 2.0);
        Canvas.SetLeft((UIElement) this.highTooltip, pos.X);
      }
    }
    else
    {
      double length = pos.Y - this.highTooltip.DesiredSize.Height;
      if (length < 0.0)
      {
        Canvas.SetTop((UIElement) this.highTooltip, 0.0);
        Canvas.SetLeft((UIElement) this.highTooltip, pos.X - this.highTooltip.DesiredSize.Width / 2.0);
      }
      else
      {
        Canvas.SetTop((UIElement) this.highTooltip, length);
        Canvas.SetLeft((UIElement) this.highTooltip, pos.X - this.highTooltip.DesiredSize.Width / 2.0);
      }
    }
  }

  internal void UpdateSegmentDragValueToolTipLow(Point pos, ChartSegment segment, double newValue)
  {
    if (!this.EnableDragTooltip)
      return;
    if (this.lowTooltip == null)
    {
      ChartDragSegmentInfo chartDragSegmentInfo = new ChartDragSegmentInfo();
      chartDragSegmentInfo.PostfixLabelTemplate = this.ActualYAxis.PostfixLabelTemplate;
      chartDragSegmentInfo.PrefixLabelTemplate = this.ActualYAxis.PostfixLabelTemplate;
      this.lowDragInfo = (ChartDragPointinfo) chartDragSegmentInfo;
      this.lowTooltip = new ContentControl()
      {
        Content = (object) this.lowDragInfo
      };
      this.SeriesPanel.Children.Add((UIElement) this.lowTooltip);
      this.lowTooltip.ContentTemplate = this.DragTooltipTemplate != null ? this.DragTooltipTemplate : (this.IsActualTransposed ? ChartDictionaries.GenericCommonDictionary[(object) "SegmentDragInfoOppLeft"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "SegmentDragInfoOppBottom"] as DataTemplate);
    }
    this.lowDragInfo.Segment = segment;
    this.lowDragInfo.ScreenCoordinates = pos;
    this.lowDragInfo.Brush = segment.Interior;
    ((ChartDragSegmentInfo) this.lowDragInfo).NewValue = (object) newValue;
    this.lowDragInfo.Segment = segment;
    this.lowTooltip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    if (this.IsActualTransposed)
    {
      if (pos.X + this.lowTooltip.DesiredSize.Width > this.ActualWidth)
      {
        Canvas.SetTop((UIElement) this.lowTooltip, this.ActualWidth - this.lowTooltip.DesiredSize.Width);
        Canvas.SetLeft((UIElement) this.lowTooltip, pos.X - this.lowTooltip.DesiredSize.Height / 2.0);
      }
      else
      {
        Canvas.SetTop((UIElement) this.lowTooltip, pos.Y - this.lowTooltip.DesiredSize.Height / 2.0);
        Canvas.SetLeft((UIElement) this.lowTooltip, pos.X - this.lowTooltip.DesiredSize.Width);
      }
    }
    else if (pos.Y + this.lowTooltip.DesiredSize.Height > this.ActualHeight)
    {
      Canvas.SetTop((UIElement) this.lowTooltip, this.ActualHeight - this.lowTooltip.DesiredSize.Height);
      Canvas.SetLeft((UIElement) this.lowTooltip, pos.X - this.lowTooltip.DesiredSize.Width / 2.0);
    }
    else
    {
      Canvas.SetTop((UIElement) this.lowTooltip, pos.Y);
      Canvas.SetLeft((UIElement) this.lowTooltip, pos.X - this.lowTooltip.DesiredSize.Width / 2.0);
    }
  }

  protected virtual void UpdateDragSpliterHigh(Rectangle rect)
  {
    int index = this.Segments.IndexOf(rect.Tag as ChartSegment);
    RangeSegmentEnterEventArgs args = new RangeSegmentEnterEventArgs()
    {
      XValue = this.GetActualXValue(index),
      SegmentIndex = index,
      CanDrag = true,
      HighValue = (object) this.HighValues[index],
      LowValue = (object) this.LowValues[index]
    };
    this.RaiseDragEnter(args);
    if (!args.CanDrag)
      return;
    if (this.DragSpliterHigh == null)
    {
      this.DragSpliterHigh = new ContentControl();
      this.SeriesPanel.Children.Add((UIElement) this.DragSpliterHigh);
      if (this.IsActualTransposed)
        this.DragSpliterHigh.Template = ChartDictionaries.GenericCommonDictionary[(object) "DragSpliterLeft"] as ControlTemplate;
      else
        this.DragSpliterHigh.Template = ChartDictionaries.GenericCommonDictionary[(object) "DragSpliterTop"] as ControlTemplate;
    }
    double num1;
    double num2;
    double num3;
    double num4;
    if (this.IsActualTransposed)
    {
      num1 = Canvas.GetTop((UIElement) rect);
      double height = rect.Height;
      double num5 = height / 3.0;
      num2 = height - num5 * 2.0;
      this.DragSpliterHigh.Margin = new Thickness().GetThickness(0.0, num5, 0.0, num5);
      num3 = Canvas.GetLeft((UIElement) rect) + rect.Width - num5 / 2.0;
      num4 = num2 / 5.0;
    }
    else
    {
      double top = Canvas.GetTop((UIElement) rect);
      double width = rect.Width;
      double num6 = width / 3.0;
      num4 = width - num6 * 2.0;
      this.DragSpliterHigh.Margin = new Thickness().GetThickness(num6, 0.0, num6, 0.0);
      num3 = Canvas.GetLeft((UIElement) rect);
      num1 = top + 7.0;
      num2 = num4 / 5.0;
    }
    this.DragSpliterHigh.SetValue(Canvas.LeftProperty, (object) num3);
    this.DragSpliterHigh.SetValue(Canvas.TopProperty, (object) num1);
    this.DragSpliterHigh.Height = num2;
    this.DragSpliterHigh.Width = num4;
  }

  protected virtual void UpdateDragSpliterLow(Rectangle rect)
  {
    int index = this.Segments.IndexOf(rect.Tag as ChartSegment);
    RangeSegmentEnterEventArgs args = new RangeSegmentEnterEventArgs()
    {
      XValue = this.GetActualXValue(index),
      SegmentIndex = index,
      CanDrag = true,
      HighValue = (object) this.HighValues[index],
      LowValue = (object) this.LowValues[index]
    };
    this.RaiseDragEnter(args);
    if (!args.CanDrag)
      return;
    if (this.DragSpliterLow == null)
    {
      this.DragSpliterLow = new ContentControl();
      this.SeriesPanel.Children.Add((UIElement) this.DragSpliterLow);
      if (this.IsActualTransposed)
        this.DragSpliterLow.Template = ChartDictionaries.GenericCommonDictionary[(object) "DragSpliterLeft"] as ControlTemplate;
      else
        this.DragSpliterLow.Template = ChartDictionaries.GenericCommonDictionary[(object) "DragSpliterTop"] as ControlTemplate;
    }
    double num1;
    double num2;
    double num3;
    double num4;
    if (this.IsActualTransposed)
    {
      num1 = Canvas.GetTop((UIElement) rect);
      double height = rect.Height;
      double num5 = height / 3.0;
      num2 = height - num5 * 2.0;
      this.DragSpliterLow.Margin = new Thickness().GetThickness(0.0, num5, 0.0, num5);
      num3 = Canvas.GetLeft((UIElement) rect) + num5 / 3.0;
      num4 = num2 / 5.0;
    }
    else
    {
      double num6 = Canvas.GetTop((UIElement) rect) + rect.Height;
      double width = rect.Width;
      double num7 = width / 3.0;
      num4 = width - num7 * 2.0;
      this.DragSpliterLow.Margin = new Thickness().GetThickness(num7, 0.0, num7, 0.0);
      num3 = Canvas.GetLeft((UIElement) rect);
      num1 = num6 - 7.0;
      num2 = num4 / 5.0;
    }
    this.DragSpliterLow.SetValue(Canvas.LeftProperty, (object) num3);
    this.DragSpliterLow.SetValue(Canvas.TopProperty, (object) num1);
    this.DragSpliterLow.Height = num2;
    this.DragSpliterLow.Width = num4;
  }

  protected virtual void OnChartDragStart(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragDelta(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragEnd(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragEntered(Point mousePos, object originalSource)
  {
  }

  protected virtual void OnChartDragExited(Point mousePos, object originalSource)
  {
    this.ResetDragSpliter();
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (this.EnableSegmentDragging)
      this.OnChartDragDelta(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    base.OnMouseMove(e);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (!this.EnableSegmentDragging)
      return;
    this.SeriesPanel.CaptureMouse();
    this.OnChartDragStart(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    base.OnMouseLeftButtonDown(e);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.EnableSegmentDragging)
    {
      this.SeriesPanel.ReleaseMouseCapture();
      this.OnChartDragEnd(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    }
    base.OnMouseLeftButtonUp(e);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    if (this.EnableSegmentDragging)
      this.OnChartDragEntered(e.GetPosition((IInputElement) this.SeriesPanel), (object) e.MouseDevice.DirectlyOver);
    base.OnMouseEnter(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    if (this.EnableSegmentDragging)
      this.OnChartDragExited(e.GetPosition((IInputElement) this.SeriesPanel), e.OriginalSource);
    base.OnMouseLeave(e);
  }

  protected void UpdateUnderLayingModel(string path, int index, object updatedData)
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (!enumerator.MoveNext())
      return;
    PropertyInfo propertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, path);
    IPropertyAccessor propertyAccessor = (IPropertyAccessor) null;
    if (propertyInfo != (PropertyInfo) null)
      propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
    int num = 0;
    while (num != index)
    {
      ++num;
      if (!enumerator.MoveNext())
        return;
    }
    propertyAccessor.SetValue(enumerator.Current, updatedData);
  }

  protected void RaisePreviewEnd(RangeDragEventArgs args)
  {
    if (this.PreviewDragEnd == null)
      return;
    this.PreviewDragEnd((object) this, args);
  }

  protected void RaiseDragStart(ChartDragStartEventArgs args)
  {
    if (this.DragStart == null)
      return;
    this.DragStart((object) this, args);
  }

  protected void RaiseDragEnd(RangeDragEndEventArgs args)
  {
    if (this.DragEnd == null)
      return;
    this.DragEnd((object) this, args);
  }

  protected void RaiseDragDelta(RangeDragEventArgs args)
  {
    if (this.DragDelta == null)
      return;
    this.DragDelta((object) this, args);
  }

  protected void RaiseDragEnter(RangeSegmentEnterEventArgs args)
  {
    if (this.SegmentEnter == null)
      return;
    this.SegmentEnter((object) this, args);
  }

  private void ResetSegmentDragTooltipInfo()
  {
    if (this.highTooltip != null)
    {
      this.SeriesPanel.Children.Remove((UIElement) this.highTooltip);
      this.highTooltip = (ContentControl) null;
      this.highDragInfo = (ChartDragPointinfo) null;
    }
    if (this.lowTooltip == null)
      return;
    this.SeriesPanel.Children.Remove((UIElement) this.lowTooltip);
    this.lowTooltip = (ContentControl) null;
    this.lowDragInfo = (ChartDragPointinfo) null;
  }

  private void CoreWindow_KeyDown(object sender, KeyEventArgs e)
  {
    bool flag = false;
    if (e.Key == Key.Escape && Keyboard.Modifiers == this.DragCancelKeyModifiers)
      flag = true;
    if (!flag)
      return;
    Mouse.OverrideCursor = Cursors.Arrow;
    this.ResetDraggingElements("EscapeKey", true);
  }
}
