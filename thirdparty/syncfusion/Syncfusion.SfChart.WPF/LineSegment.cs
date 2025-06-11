// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LineSegment
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

public class LineSegment : ChartSegment
{
  private bool isSegmentUpdated;
  private Line line;
  private DataTemplate CustomTemplate;
  private double x1;
  private double x2;
  private double y1;
  private double y2;
  private double _yData;
  private double _y1Data;
  private double _xData;
  private double _x1Data;
  private ContentControl control;

  public LineSegment()
  {
  }

  public LineSegment(
    double x1,
    double y1,
    double x2,
    double y2,
    AdornmentSeries lineSeries,
    object item)
  {
    this.Series = (ChartSeriesBase) lineSeries;
    this.Item = item;
    this.SetData(x1, y1, x2, y2);
    if (!(lineSeries is LineSeries lineSeries1))
      return;
    this.CustomTemplate = lineSeries1.CustomTemplate;
  }

  public LineSegment(double x1, double y1, double X2, double Y2, object item)
  {
  }

  public double X1Value { get; set; }

  public double Y1Value { get; set; }

  public double X2Value { get; set; }

  public double Y2Value { get; set; }

  public double X1
  {
    get => this.x1;
    set
    {
      this.x1 = value;
      this.OnPropertyChanged(nameof (X1));
    }
  }

  public double X2
  {
    get => this.x2;
    set
    {
      this.x2 = value;
      this.OnPropertyChanged(nameof (X2));
    }
  }

  public double Y1
  {
    get => this.y1;
    set
    {
      this.y1 = value;
      this.OnPropertyChanged(nameof (Y1));
    }
  }

  public double Y2
  {
    get => this.y2;
    set
    {
      this.y2 = value;
      this.OnPropertyChanged(nameof (Y2));
    }
  }

  public double X1Data
  {
    get => this._x1Data;
    set
    {
      this._x1Data = value;
      this.OnPropertyChanged(nameof (X1Data));
    }
  }

  public double XData
  {
    get => this._xData;
    set
    {
      this._xData = value;
      this.OnPropertyChanged(nameof (XData));
    }
  }

  public double YData
  {
    get => this._yData;
    set
    {
      this._yData = value;
      this.OnPropertyChanged(nameof (YData));
    }
  }

  public double Y1Data
  {
    get => this._y1Data;
    set
    {
      this._y1Data = value;
      this.OnPropertyChanged(nameof (Y1Data));
    }
  }

  public override void SetData(params double[] Values)
  {
    this.X1Value = Values[0];
    this.Y1Value = Values[1];
    this.X2Value = Values[2];
    this.Y2Value = Values[3];
    this.XData = Values[0];
    this.X1Data = Values[2];
    this.YData = Values[1];
    this.Y1Data = Values[3];
    this.XRange = new DoubleRange(this.X1Value, this.X2Value);
    this.YRange = new DoubleRange(this.Y1Value, this.Y2Value);
    if (!(this.Series is LineSeries series))
      return;
    this.CustomTemplate = series.CustomTemplate;
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.CustomTemplate == null)
    {
      this.line = new Line();
      this.line.DataContext = (object) this;
      this.SetVisualBindings((Shape) this.line);
      this.line.Tag = (object) this;
      this.line.StrokeEndLineCap = PenLineCap.Round;
      this.line.StrokeStartLineCap = PenLineCap.Round;
      return (UIElement) this.line;
    }
    this.control = new ContentControl();
    this.control.Content = (object) this;
    this.control.Tag = (object) this;
    this.control.ContentTemplate = this.CustomTemplate;
    return (UIElement) this.control;
  }

  public override UIElement GetRenderedVisual()
  {
    return this.CustomTemplate == null ? (UIElement) this.line : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer)
    {
      if (this.isSegmentUpdated)
        this.Series.SeriesRootPanel.Clip = (Geometry) null;
      double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
      double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
      double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
      double num3 = isLogarithmic ? Math.Log(this.X1Value, newBase) : this.X1Value;
      double num4 = isLogarithmic ? Math.Log(this.X2Value, newBase) : this.X2Value;
      if (num3 <= num2 && num4 >= num1 && (!double.IsNaN(this.Y1Value) && !double.IsNaN(this.Y2Value) || this.Series.ShowEmptyPoints))
      {
        Point visible1 = transformer.TransformToVisible(this.X1Value, this.Y1Value);
        Point visible2 = transformer.TransformToVisible(this.X2Value, this.Y2Value);
        if (this.line != null)
        {
          this.line.X1 = visible1.X;
          this.line.Y1 = visible1.Y;
          this.line.X2 = visible2.X;
          this.line.Y2 = visible2.Y;
          this.line.Visibility = Visibility.Visible;
        }
        else
        {
          this.control.Visibility = Visibility.Visible;
          this.X1 = visible1.X;
          this.Y1 = visible1.Y;
          this.X2 = visible2.X;
          this.Y2 = visible2.Y;
        }
      }
      else if (this.CustomTemplate == null)
      {
        this.line.ClearUIValues();
        this.line.Visibility = Visibility.Collapsed;
      }
      else
        this.control.Visibility = Visibility.Collapsed;
      this.isSegmentUpdated = true;
    }
    else
    {
      ChartTransform.ChartPolarTransformer polarTransformer = transformer as ChartTransform.ChartPolarTransformer;
      if (this.Series.ShowEmptyPoints || !double.IsNaN(this.Y1Value) && !double.IsNaN(this.Y2Value))
      {
        Point visible3 = polarTransformer.TransformToVisible(this.X1Value, this.Y1Value);
        Point visible4 = polarTransformer.TransformToVisible(this.X2Value, this.Y2Value);
        if (this.line != null)
        {
          this.line.Visibility = Visibility.Visible;
          this.line.X1 = visible3.X;
          this.line.Y1 = visible3.Y;
          this.line.X2 = visible4.X;
          this.line.Y2 = visible4.Y;
        }
        else
        {
          this.X1 = visible3.X;
          this.Y1 = visible3.Y;
          this.X2 = visible4.X;
          this.Y2 = visible4.Y;
        }
      }
      else
      {
        if (this.line == null)
          return;
        this.line.Visibility = Visibility.Collapsed;
      }
    }
  }

  protected override void SetVisualBindings(Shape element)
  {
    base.SetVisualBindings(element);
    if (this.Series is LineSeries || this.Series is StackingLineSeries)
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeDashArrayProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("StrokeDashArray", new object[0])
      });
    if (!(this.Series is PolarRadarSeriesBase series))
      return;
    DoubleCollection strokeDashArray = series.StrokeDashArray;
    DoubleCollection doubleCollection = new DoubleCollection();
    if (strokeDashArray == null || strokeDashArray.Count <= 0)
      return;
    foreach (double num in strokeDashArray)
      doubleCollection.Add(num);
    element.StrokeDashArray = doubleCollection;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal override void Dispose()
  {
    if (this.line != null)
    {
      this.line.Tag = (object) null;
      this.line = (Line) null;
    }
    base.Dispose();
  }
}
