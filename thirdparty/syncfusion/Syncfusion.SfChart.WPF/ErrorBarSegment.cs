// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ErrorBarSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ErrorBarSegment : ChartSegment
{
  internal Line HorLine;
  internal Line VerLine;
  internal Line HorLeftCapLine;
  internal Line HorRightCapLine;
  internal Line VerTopCapLine;
  internal Line VerBottomCapLine;
  private Canvas _canvas;
  private ChartPoint _verToppoint;
  private ChartPoint _verBottompoint;
  private ChartPoint _horLeftpoint;
  private ChartPoint _horRightpoint;

  public ErrorBarSegment()
  {
  }

  [Obsolete("Use  ErrorBarSegment(ChartPoint hlpoint, ChartPoint hrpoint, ChartPoint vtpoint, ChartPoint vbpoint, ErrorBarSeries series, object item)")]
  public ErrorBarSegment(
    Point hlpoint,
    Point hrpoint,
    Point vtpoint,
    Point vbpoint,
    ErrorBarSeries series,
    object item)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = item;
  }

  public ErrorBarSegment(
    ChartPoint hlpoint,
    ChartPoint hrpoint,
    ChartPoint vtpoint,
    ChartPoint vbpoint,
    ErrorBarSeries series,
    object item)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = item;
  }

  [Obsolete("Use  SetData(ChartPoint hlpoint, ChartPoint hrpoint, ChartPoint vtpoint, ChartPoint vbpoint)")]
  public override void SetData(Point hlpoint, Point hrpoint, Point vtpoint, Point vbpoint)
  {
    this._horLeftpoint = new ChartPoint(hlpoint.X, hlpoint.Y);
    this._horRightpoint = new ChartPoint(hrpoint.X, hrpoint.Y);
    this._verToppoint = new ChartPoint(vtpoint.X, vtpoint.Y);
    this._verBottompoint = new ChartPoint(vbpoint.X, vbpoint.Y);
    switch ((this.Series as ErrorBarSeries).Mode)
    {
      case ErrorBarMode.Horizontal:
        this.XRange = new DoubleRange(ChartMath.Min(hlpoint.X, hrpoint.X), ChartMath.Max(hlpoint.X, hrpoint.X));
        this.YRange = DoubleRange.Empty;
        break;
      case ErrorBarMode.Vertical:
        this.YRange = new DoubleRange(vbpoint.Y, vtpoint.Y);
        this.XRange = DoubleRange.Empty;
        break;
      default:
        this.XRange = new DoubleRange(ChartMath.Min(hlpoint.X, hrpoint.X), ChartMath.Max(hlpoint.X, hrpoint.X));
        this.YRange = new DoubleRange(vbpoint.Y, vtpoint.Y);
        break;
    }
  }

  public override UIElement GetRenderedVisual() => (UIElement) this._canvas;

  public override void Update(IChartTransformer transformer)
  {
    ErrorBarSeries series = this.Series as ErrorBarSeries;
    if (transformer == null)
      return;
    this._canvas.Children.Clear();
    if (this.HorLine != null && series.Mode != ErrorBarMode.Vertical && !double.IsNaN(this._horLeftpoint.Y) && !double.IsNaN(this._horRightpoint.Y))
    {
      Point visible1 = transformer.TransformToVisible(this._horLeftpoint.X, this._horLeftpoint.Y);
      Point visible2 = transformer.TransformToVisible(this._horRightpoint.X, this._horRightpoint.Y);
      this.HorLine.X1 = visible1.X;
      this.HorLine.Y1 = visible1.Y;
      this.HorLine.X2 = visible2.X;
      this.HorLine.Y2 = visible2.Y;
      this._canvas.Children.Add((UIElement) this.HorLine);
      if (series.HorizontalCapLineStyle.Visibility == Visibility.Visible)
      {
        double num = series.HorizontalCapLineStyle.LineWidth / 2.0;
        if (series.IsTransposed)
        {
          this.HorLeftCapLine.X1 = this.HorLine.X1 - num;
          this.HorLeftCapLine.Y1 = this.HorLine.Y1;
          this.HorLeftCapLine.X2 = this.HorLine.X1 + num;
          this.HorLeftCapLine.Y2 = this.HorLine.Y1;
        }
        else
        {
          this.HorLeftCapLine.X1 = this.HorLine.X1;
          this.HorLeftCapLine.Y1 = this.HorLine.Y1 + num;
          this.HorLeftCapLine.X2 = this.HorLine.X1;
          this.HorLeftCapLine.Y2 = this.HorLine.Y1 - num;
        }
        this._canvas.Children.Add((UIElement) this.HorLeftCapLine);
        if (series.HorizontalDirection == ErrorBarDirection.Plus)
          this.HorLeftCapLine.Visibility = Visibility.Collapsed;
        else
          this.HorLeftCapLine.Visibility = Visibility.Visible;
        if (series.IsTransposed)
        {
          this.HorRightCapLine.X1 = this.HorLine.X2 - num;
          this.HorRightCapLine.Y1 = this.HorLine.Y2;
          this.HorRightCapLine.X2 = this.HorLine.X2 + num;
          this.HorRightCapLine.Y2 = this.HorLine.Y2;
        }
        else
        {
          this.HorRightCapLine.X1 = this.HorLine.X2;
          this.HorRightCapLine.Y1 = this.HorLine.Y2 + num;
          this.HorRightCapLine.X2 = this.HorLine.X2;
          this.HorRightCapLine.Y2 = this.HorLine.Y2 - num;
        }
        this._canvas.Children.Add((UIElement) this.HorRightCapLine);
        if (series.HorizontalDirection == ErrorBarDirection.Minus)
          this.HorRightCapLine.Visibility = Visibility.Collapsed;
        else
          this.HorRightCapLine.Visibility = Visibility.Visible;
      }
    }
    if (this.VerLine == null || series.Mode == ErrorBarMode.Horizontal || double.IsNaN(this._verToppoint.Y) || double.IsNaN(this._verBottompoint.Y))
      return;
    Point visible3 = transformer.TransformToVisible(this._verToppoint.X, this._verToppoint.Y);
    Point visible4 = transformer.TransformToVisible(this._verBottompoint.X, this._verBottompoint.Y);
    this.VerLine.X1 = visible3.X;
    this.VerLine.Y1 = visible3.Y;
    this.VerLine.X2 = visible4.X;
    this.VerLine.Y2 = visible4.Y;
    this._canvas.Children.Add((UIElement) this.VerLine);
    if (series.VerticalCapLineStyle.Visibility != Visibility.Visible)
      return;
    double num1 = series.VerticalCapLineStyle.LineWidth / 2.0;
    if (series.IsTransposed)
    {
      this.VerBottomCapLine.X1 = this.VerLine.X1;
      this.VerBottomCapLine.Y1 = this.VerLine.Y1 + num1;
      this.VerBottomCapLine.X2 = this.VerLine.X1;
      this.VerBottomCapLine.Y2 = this.VerLine.Y1 - num1;
    }
    else
    {
      this.VerBottomCapLine.X1 = this.VerLine.X1 - num1;
      this.VerBottomCapLine.Y1 = this.VerLine.Y1;
      this.VerBottomCapLine.X2 = this.VerLine.X1 + num1;
      this.VerBottomCapLine.Y2 = this.VerLine.Y1;
    }
    this._canvas.Children.Add((UIElement) this.VerBottomCapLine);
    if (series.VerticalDirection == ErrorBarDirection.Plus)
      this.VerBottomCapLine.Visibility = Visibility.Collapsed;
    else
      this.VerBottomCapLine.Visibility = Visibility.Visible;
    if (series.IsTransposed)
    {
      this.VerTopCapLine.X1 = this.VerLine.X2;
      this.VerTopCapLine.Y1 = this.VerLine.Y2 + num1;
      this.VerTopCapLine.X2 = this.VerLine.X2;
      this.VerTopCapLine.Y2 = this.VerLine.Y2 - num1;
    }
    else
    {
      this.VerTopCapLine.X1 = this.VerLine.X1 - num1;
      this.VerTopCapLine.Y1 = this.VerLine.Y2;
      this.VerTopCapLine.X2 = this.VerLine.X1 + num1;
      this.VerTopCapLine.Y2 = this.VerLine.Y2;
    }
    this._canvas.Children.Add((UIElement) this.VerTopCapLine);
    if (series.VerticalDirection == ErrorBarDirection.Minus)
      this.VerTopCapLine.Visibility = Visibility.Collapsed;
    else
      this.VerTopCapLine.Visibility = Visibility.Visible;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  public override void SetData(
    ChartPoint hlpoint,
    ChartPoint hrpoint,
    ChartPoint vtpoint,
    ChartPoint vbpoint)
  {
    this._horLeftpoint = hlpoint;
    this._horRightpoint = hrpoint;
    this._verToppoint = vtpoint;
    this._verBottompoint = vbpoint;
    switch ((this.Series as ErrorBarSeries).Mode)
    {
      case ErrorBarMode.Horizontal:
        this.XRange = new DoubleRange(ChartMath.Min(hlpoint.X, hrpoint.X), ChartMath.Max(hlpoint.X, hrpoint.X));
        this.YRange = DoubleRange.Empty;
        break;
      case ErrorBarMode.Vertical:
        this.YRange = new DoubleRange(vbpoint.Y, vtpoint.Y);
        this.XRange = DoubleRange.Empty;
        break;
      default:
        this.XRange = new DoubleRange(ChartMath.Min(hlpoint.X, hrpoint.X), ChartMath.Max(hlpoint.X, hrpoint.X));
        this.YRange = new DoubleRange(vbpoint.Y, vtpoint.Y);
        break;
    }
  }

  public override UIElement CreateVisual(Size size)
  {
    this._canvas = new Canvas();
    this.HorLine = new Line();
    this._canvas.Children.Add((UIElement) this.HorLine);
    this.HorLine.Tag = (object) this;
    this.HorLeftCapLine = new Line();
    this._canvas.Children.Add((UIElement) this.HorLeftCapLine);
    this.HorRightCapLine = new Line();
    this._canvas.Children.Add((UIElement) this.HorRightCapLine);
    this.HorRightCapLine.Tag = this.HorLeftCapLine.Tag = (object) this;
    this.VerLine = new Line();
    this._canvas.Children.Add((UIElement) this.VerLine);
    this.VerLine.Tag = (object) this;
    this.VerBottomCapLine = new Line();
    this._canvas.Children.Add((UIElement) this.VerBottomCapLine);
    this.VerTopCapLine = new Line();
    this._canvas.Children.Add((UIElement) this.VerTopCapLine);
    this.VerTopCapLine.Tag = this.VerBottomCapLine.Tag = (object) this;
    this.UpdateVisualBinding();
    return (UIElement) this._canvas;
  }

  internal Point DateTimeIntervalCalculation(double errorvalue, DateTimeIntervalType type)
  {
    ErrorBarSeries series = this.Series as ErrorBarSeries;
    DateTime date1 = Convert.ToDouble(this._horLeftpoint.X).FromOADate();
    DateTime date2 = Convert.ToDouble(this._horRightpoint.X).FromOADate();
    if (series.HorizontalDirection == ErrorBarDirection.Plus)
    {
      this._horLeftpoint.X = DateTimeAxisHelper.IncreaseInterval(date1, 0.0, type).ToOADate();
      this._horRightpoint.X = DateTimeAxisHelper.IncreaseInterval(date2, errorvalue, type).ToOADate();
    }
    else if (series.HorizontalDirection == ErrorBarDirection.Minus)
    {
      this._horLeftpoint.X = DateTimeAxisHelper.IncreaseInterval(date1, -errorvalue, type).ToOADate();
      this._horRightpoint.X = DateTimeAxisHelper.IncreaseInterval(date2, 0.0, type).ToOADate();
    }
    else
    {
      this._horLeftpoint.X = DateTimeAxisHelper.IncreaseInterval(date1, -errorvalue, type).ToOADate();
      this._horRightpoint.X = DateTimeAxisHelper.IncreaseInterval(date2, errorvalue, type).ToOADate();
    }
    return new Point(this._horLeftpoint.X, this._horRightpoint.X);
  }

  internal void UpdateVisualBinding()
  {
    ErrorBarSeries series = this.Series as ErrorBarSeries;
    this.SetVisualBindings((Shape) this.HorLine, (DependencyObject) series.HorizontalLineStyle);
    this.SetVisualBindings((Shape) this.HorLeftCapLine, (DependencyObject) series.HorizontalCapLineStyle);
    this.SetVisualBindings((Shape) this.HorRightCapLine, (DependencyObject) series.HorizontalCapLineStyle);
    this.SetVisualBindings((Shape) this.VerLine, (DependencyObject) series.VerticalLineStyle);
    this.SetVisualBindings((Shape) this.VerBottomCapLine, (DependencyObject) series.VerticalCapLineStyle);
    this.SetVisualBindings((Shape) this.VerTopCapLine, (DependencyObject) series.VerticalCapLineStyle);
  }

  private void SetVisualBindings(Shape element, DependencyObject linestyle)
  {
    if (element != this.HorLine && element != this.VerLine)
    {
      Binding binding = new Binding()
      {
        Source = (object) linestyle,
        Path = new PropertyPath("Visibility", new object[0])
      };
      element.SetBinding(UIElement.VisibilityProperty, (BindingBase) binding);
    }
    Binding binding1 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("Stroke", new object[0])
    };
    element.SetBinding(Shape.StrokeProperty, (BindingBase) binding1);
    Binding binding2 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("StrokeThickness", new object[0])
    };
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) binding2);
    Binding binding3 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("StrokeDashCap", new object[0])
    };
    element.SetBinding(Shape.StrokeDashCapProperty, (BindingBase) binding3);
    Binding binding4 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("StrokeEndLineCap", new object[0])
    };
    element.SetBinding(Shape.StrokeEndLineCapProperty, (BindingBase) binding4);
    Binding binding5 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("StrokeLineJoin", new object[0])
    };
    element.SetBinding(Shape.StrokeLineJoinProperty, (BindingBase) binding5);
    Binding binding6 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("StrokeMiterLimit", new object[0])
    };
    element.SetBinding(Shape.StrokeMiterLimitProperty, (BindingBase) binding6);
    Binding binding7 = new Binding()
    {
      Source = (object) linestyle,
      Path = new PropertyPath("StrokeDashOffset", new object[0])
    };
    element.SetBinding(Shape.StrokeDashOffsetProperty, (BindingBase) binding7);
    DoubleCollection strokeDashArray = (linestyle as LineStyle).StrokeDashArray;
    if (strokeDashArray == null || strokeDashArray.Count <= 0)
      return;
    DoubleCollection doubleCollection = new DoubleCollection();
    foreach (double num in strokeDashArray)
      doubleCollection.Add(num);
    element.StrokeDashArray = doubleCollection;
  }
}
