// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCrossHairBehavior
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCrossHairBehavior : ChartBehavior
{
  public static readonly DependencyProperty VerticalAxisLabelAlignmentProperty = DependencyProperty.Register(nameof (VerticalAxisLabelAlignment), typeof (ChartAlignment), typeof (ChartCrossHairBehavior), new PropertyMetadata((object) ChartAlignment.Center));
  public static readonly DependencyProperty HorizontalAxisLabelAlignmentProperty = DependencyProperty.Register(nameof (HorizontalAxisLabelAlignment), typeof (ChartAlignment), typeof (ChartCrossHairBehavior), new PropertyMetadata((object) ChartAlignment.Center));
  public static readonly DependencyProperty HorizontalLineStyleProperty = DependencyProperty.Register(nameof (HorizontalLineStyle), typeof (Style), typeof (ChartCrossHairBehavior), new PropertyMetadata(ChartDictionaries.GenericCommonDictionary[(object) "trackBallLineStyle"]));
  public static readonly DependencyProperty VerticalLineStyleProperty = DependencyProperty.Register(nameof (VerticalLineStyle), typeof (Style), typeof (ChartCrossHairBehavior), new PropertyMetadata(ChartDictionaries.GenericCommonDictionary[(object) "trackBallLineStyle"]));
  protected internal Point CurrentPoint;
  private Line verticalLine;
  private Line horizontalLine;
  private bool isActivated;
  private List<ContentControl> labelElements;
  private ObservableCollection<ChartPointInfo> pointInfos;
  private List<FrameworkElement> elements;
  private string labelXValue;
  private string labelYValue;

  public ChartCrossHairBehavior()
  {
    this.elements = new List<FrameworkElement>();
    this.verticalLine = new Line();
    this.horizontalLine = new Line();
    this.labelElements = new List<ContentControl>();
    this.pointInfos = new ObservableCollection<ChartPointInfo>();
  }

  public ChartAlignment VerticalAxisLabelAlignment
  {
    get
    {
      return (ChartAlignment) this.GetValue(ChartCrossHairBehavior.VerticalAxisLabelAlignmentProperty);
    }
    set => this.SetValue(ChartCrossHairBehavior.VerticalAxisLabelAlignmentProperty, (object) value);
  }

  public ChartAlignment HorizontalAxisLabelAlignment
  {
    get
    {
      return (ChartAlignment) this.GetValue(ChartCrossHairBehavior.HorizontalAxisLabelAlignmentProperty);
    }
    set
    {
      this.SetValue(ChartCrossHairBehavior.HorizontalAxisLabelAlignmentProperty, (object) value);
    }
  }

  public ObservableCollection<ChartPointInfo> PointInfos
  {
    get => this.pointInfos;
    internal set => this.pointInfos = value;
  }

  public Style HorizontalLineStyle
  {
    get => (Style) this.GetValue(ChartCrossHairBehavior.HorizontalLineStyleProperty);
    set => this.SetValue(ChartCrossHairBehavior.HorizontalLineStyleProperty, (object) value);
  }

  public Style VerticalLineStyle
  {
    get => (Style) this.GetValue(ChartCrossHairBehavior.VerticalLineStyleProperty);
    set => this.SetValue(ChartCrossHairBehavior.VerticalLineStyleProperty, (object) value);
  }

  protected internal bool IsActivated
  {
    get => this.isActivated;
    set
    {
      this.isActivated = value;
      this.Activate(this.isActivated);
    }
  }

  protected internal override void DetachElements()
  {
    if (this.AdorningCanvas == null)
      return;
    foreach (UIElement element in this.elements)
      this.AdorningCanvas.Children.Remove(element);
    this.elements.Clear();
  }

  protected internal override void OnSizeChanged(SizeChangedEventArgs e)
  {
    if (this.ChartArea == null || string.IsNullOrEmpty(this.labelXValue) || string.IsNullOrEmpty(this.labelYValue))
      return;
    double point1 = this.ChartArea.ValueToPoint(this.ChartArea.InternalSecondaryAxis, Convert.ToDouble(this.labelYValue));
    double point2 = this.ChartArea.ValueToPoint(this.ChartArea.InternalPrimaryAxis, Convert.ToDouble(this.labelXValue));
    if (double.IsNaN(point1) || double.IsNaN(point2))
      return;
    foreach (UIElement labelElement in this.labelElements)
      this.DetachElement(labelElement);
    this.labelElements.Clear();
    this.pointInfos.Clear();
    this.elements.Clear();
    this.SetPosition(new Point(point2, point1));
  }

  protected internal override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.IsActivated = false;
  }

  protected internal override void OnMouseMove(MouseEventArgs e)
  {
    if (Mouse.LeftButton != MouseButtonState.Pressed)
      this.IsActivated = true;
    if (this.ChartArea == null || this.ChartArea.AreaType != ChartAreaType.CartesianAxes || !this.IsActivated)
      return;
    this.CurrentPoint = e.GetPosition((IInputElement) this.AdorningCanvas);
    if (this.ChartArea.SeriesClipRect.Contains(this.CurrentPoint))
    {
      foreach (UIElement labelElement in this.labelElements)
        this.DetachElement(labelElement);
      this.labelElements.Clear();
      this.pointInfos.Clear();
      this.elements.Clear();
      this.SetPosition(this.CurrentPoint);
    }
    else
      this.IsActivated = false;
  }

  protected internal override void OnMouseLeave(MouseEventArgs e)
  {
    if (!this.IsActivated)
      return;
    this.IsActivated = false;
    if (this.ChartArea == null)
      return;
    this.ChartArea.HoldUpdate = false;
  }

  protected internal override void OnLayoutUpdated()
  {
    if (this.ChartArea == null || !this.IsActivated)
      return;
    foreach (UIElement labelElement in this.labelElements)
      this.DetachElement(labelElement);
    this.labelElements.Clear();
    this.pointInfos.Clear();
    if (this.ChartArea.SeriesClipRect.Contains(this.CurrentPoint))
    {
      this.SetPosition(this.CurrentPoint);
    }
    else
    {
      foreach (UIElement element in this.elements)
        element.Visibility = Visibility.Collapsed;
    }
  }

  protected internal override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (this.ChartArea == null || !this.IsActivated)
      return;
    this.IsActivated = false;
    if (this.ChartArea == null)
      return;
    this.ChartArea.HoldUpdate = false;
  }

  protected internal override void AlignDefaultLabel(
    ChartAlignment verticalAlignemnt,
    ChartAlignment horizontalAlignment,
    double x,
    double y,
    ContentControl control)
  {
    switch (horizontalAlignment)
    {
      case ChartAlignment.Near:
        x -= control.DesiredSize.Width;
        if (control != null)
        {
          (control.Content as ChartPointInfo).X = x;
          break;
        }
        break;
      case ChartAlignment.Center:
        x -= control.DesiredSize.Width / 2.0;
        if (control != null)
        {
          (control.Content as ChartPointInfo).X = x;
          break;
        }
        break;
    }
    switch (verticalAlignemnt)
    {
      case ChartAlignment.Near:
        y -= control.DesiredSize.Height;
        if (control != null)
        {
          (control.Content as ChartPointInfo).Y = y;
          break;
        }
        break;
      case ChartAlignment.Center:
        y -= control.DesiredSize.Height / 2.0;
        if (control != null)
        {
          (control.Content as ChartPointInfo).Y = y;
          break;
        }
        break;
    }
    Canvas.SetLeft((UIElement) control, x);
    Canvas.SetTop((UIElement) control, y);
  }

  protected internal virtual void SetPosition(Point point)
  {
    if (this.AdorningCanvas == null || double.IsNaN(point.X) || double.IsNaN(point.Y) || !this.IsActivated)
      return;
    double left = this.ChartArea.SeriesClipRect.Left;
    double top = this.ChartArea.SeriesClipRect.Top;
    double x = point.X;
    double y = point.Y;
    foreach (UIElement element in this.elements)
      element.Visibility = Visibility.Visible;
    this.verticalLine.X1 = this.verticalLine.X2 = x > this.ChartArea.SeriesClipRect.Right ? this.ChartArea.SeriesClipRect.Right : x;
    this.verticalLine.Y1 = top;
    this.verticalLine.Y2 = this.ChartArea.SeriesClipRect.Height + top;
    this.elements.Add((FrameworkElement) this.verticalLine);
    this.horizontalLine.Y1 = this.horizontalLine.Y2 = y;
    this.horizontalLine.X1 = left;
    this.horizontalLine.X2 = left + this.ChartArea.SeriesClipRect.Width;
    this.elements.Add((FrameworkElement) this.horizontalLine);
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.ChartArea.Axes)
    {
      if (ax.RenderedRect.Left <= point.X && ax.RenderedRect.Right >= point.X || ax.RenderedRect.Top <= point.Y && ax.RenderedRect.Bottom >= point.Y)
      {
        double num1 = this.ChartArea.PointToValue(ax, new Point(point.X - left, point.Y - top));
        if (!double.IsNaN(num1))
        {
          ChartPointInfo pointInfo = new ChartPointInfo();
          pointInfo.Axis = ax;
          bool flag = ax is DateTimeAxis;
          if (ax.Orientation == Orientation.Horizontal)
          {
            if (this.ChartArea.VisibleSeries.Count > 0 && this.ChartArea.VisibleSeries[0].IsIndexed && !this.ChartArea.VisibleSeries[0].IsActualTransposed)
            {
              pointInfo.ValueX = ax.GetLabelContent((double) (int) Math.Round(num1)).ToString();
              double num2 = this.ChartArea.ValueToPoint(ax, Math.Round(num1)) + left;
              ChartPointInfo chartPointInfo = pointInfo;
              Line verticalLine = this.verticalLine;
              double num3;
              this.verticalLine.X2 = num3 = num2 > this.ChartArea.SeriesClipRect.Right ? this.ChartArea.SeriesClipRect.Right : (num2 < this.ChartArea.SeriesClipRect.Left ? this.ChartArea.SeriesClipRect.Left : num2);
              double num4;
              double num5 = num4 = num3;
              verticalLine.X1 = num4;
              double num6 = num5;
              chartPointInfo.X = num6;
              pointInfo.BaseX = pointInfo.X;
            }
            else
            {
              pointInfo.ValueX = flag ? ax.GetLabelContent(num1).ToString() : ax.GetLabelContent(Math.Round(num1, 2)).ToString();
              pointInfo.X = point.X;
              pointInfo.BaseX = pointInfo.X;
            }
            this.labelXValue = num1.ToString();
          }
          else
          {
            if (this.ChartArea.VisibleSeries.Count > 0 && this.ChartArea.VisibleSeries[0].IsIndexed && this.ChartArea.VisibleSeries[0].IsActualTransposed)
            {
              pointInfo.ValueY = ax.GetLabelContent((double) (int) Math.Round(num1)).ToString();
              double num7 = this.ChartArea.ValueToPoint(ax, Math.Round(num1)) + top;
              ChartPointInfo chartPointInfo = pointInfo;
              Line horizontalLine = this.horizontalLine;
              double num8;
              this.horizontalLine.Y2 = num8 = num7 > this.ChartArea.SeriesClipRect.Bottom ? this.ChartArea.SeriesClipRect.Bottom : (num7 < this.ChartArea.SeriesClipRect.Top ? this.ChartArea.SeriesClipRect.Top : num7);
              double num9;
              double num10 = num9 = num8;
              horizontalLine.Y1 = num9;
              double num11 = num10;
              chartPointInfo.Y = num11;
              pointInfo.BaseY = pointInfo.Y;
            }
            else
            {
              pointInfo.ValueY = flag ? ax.GetLabelContent(num1).ToString() : ax.GetLabelContent(Math.Round(num1, 2)).ToString();
              pointInfo.Y = point.Y;
              pointInfo.BaseY = pointInfo.Y;
            }
            this.labelYValue = num1.ToString();
          }
          this.GenerateLabel(pointInfo, ax);
        }
      }
    }
  }

  protected override void AttachElements()
  {
    this.verticalLine.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("VerticalLineStyle", new object[0])
    });
    this.horizontalLine = new Line();
    this.horizontalLine.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("HorizontalLineStyle", new object[0])
    });
    if (this.AdorningCanvas != null && !this.AdorningCanvas.Children.Contains((UIElement) this.verticalLine))
    {
      this.AdorningCanvas.Children.Add((UIElement) this.verticalLine);
      this.elements.Add((FrameworkElement) this.verticalLine);
    }
    if (this.AdorningCanvas != null && !this.AdorningCanvas.Children.Contains((UIElement) this.horizontalLine))
    {
      this.AdorningCanvas.Children.Add((UIElement) this.horizontalLine);
      this.elements.Add((FrameworkElement) this.horizontalLine);
    }
    this.IsActivated = false;
  }

  protected override DependencyObject CloneBehavior(DependencyObject obj)
  {
    return base.CloneBehavior((DependencyObject) new ChartCrossHairBehavior()
    {
      CurrentPoint = this.CurrentPoint,
      HorizontalAxisLabelAlignment = this.HorizontalAxisLabelAlignment,
      VerticalAxisLabelAlignment = this.VerticalAxisLabelAlignment,
      HorizontalLineStyle = this.HorizontalLineStyle,
      VerticalLineStyle = this.VerticalLineStyle
    });
  }

  protected virtual void GenerateLabel(ChartPointInfo pointInfo, ChartAxis axis)
  {
    if (!axis.GetTrackballInfo())
      return;
    double num = 0.0;
    this.chartAxis = axis;
    DataTemplate template = axis.CrosshairLabelTemplate ?? ChartDictionaries.GenericCommonDictionary[(object) "axisCrosshairLabel"] as DataTemplate;
    ChartAxisBase2D chartAxisBase2D = axis as ChartAxisBase2D;
    if (chartAxisBase2D.EnableScrollBar && !chartAxisBase2D.EnableTouchMode)
      num = axis.Orientation != Orientation.Vertical ? chartAxisBase2D.sfChartResizableBar.DesiredSize.Height : chartAxisBase2D.sfChartResizableBar.DesiredSize.Width;
    if (axis.Orientation == Orientation.Vertical)
    {
      pointInfo.X = axis.OpposedPosition ? axis.ArrangeRect.Left + num : axis.ArrangeRect.Right - num;
      this.AddLabel(pointInfo, this.VerticalAxisLabelAlignment, ChartCrossHairBehavior.GetChartAlignment(axis.OpposedPosition, ChartAlignment.Near), template);
    }
    else
    {
      pointInfo.Y = axis.OpposedPosition ? axis.ArrangeRect.Bottom - num : axis.ArrangeRect.Top + num;
      this.AddLabel(pointInfo, ChartCrossHairBehavior.GetChartAlignment(axis.OpposedPosition, ChartAlignment.Far), this.HorizontalAxisLabelAlignment, template);
    }
  }

  protected virtual void AddLabel(
    object obj,
    ChartAlignment verticalAlignemnt,
    ChartAlignment horizontalAlignment,
    DataTemplate labelTemplate,
    double x,
    double y)
  {
    ContentControl contentControl = new ContentControl();
    contentControl.Content = obj;
    contentControl.ContentTemplate = labelTemplate;
    this.AddElement((UIElement) contentControl);
    this.labelElements.Add(contentControl);
    contentControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    this.AlignAxisToolTipPolygon(contentControl, verticalAlignemnt, horizontalAlignment, x, y, (ChartBehavior) this);
  }

  protected void AddLabel(
    ChartPointInfo obj,
    ChartAlignment verticalAlignment,
    ChartAlignment horizontalAlignment,
    DataTemplate template)
  {
    if (obj == null || template == null)
      return;
    this.AddLabel((object) obj, verticalAlignment, horizontalAlignment, template, obj.X, obj.Y);
  }

  protected void AddElement(UIElement element)
  {
    if (this.AdorningCanvas.Children.Contains(element))
      return;
    this.AdorningCanvas.Children.Add(element);
    this.elements.Add(element as FrameworkElement);
  }

  private static ChartAlignment GetChartAlignment(bool isOpposed, ChartAlignment alignment)
  {
    if (!isOpposed)
      return alignment;
    if (alignment == ChartAlignment.Near)
      return ChartAlignment.Far;
    return alignment == ChartAlignment.Far ? ChartAlignment.Near : ChartAlignment.Center;
  }

  private void Activate(bool activate)
  {
    foreach (UIElement element in this.elements)
      element.Visibility = activate ? Visibility.Visible : Visibility.Collapsed;
  }
}
