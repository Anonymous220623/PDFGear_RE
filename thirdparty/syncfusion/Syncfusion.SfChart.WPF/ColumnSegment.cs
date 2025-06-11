// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColumnSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ColumnSegment : ChartSegment
{
  internal DataTemplate customTemplate;
  protected double Left;
  protected double Top;
  protected double Bottom;
  protected double Right;
  protected Rectangle RectSegment;
  protected ContentControl control;
  private double rectX;
  private double rectY;
  private double width;
  private double height;
  private double yData;
  private double xData;

  public ColumnSegment()
  {
  }

  public ColumnSegment(double x1, double y1, double x2, double y2, ColumnSeries series)
  {
    this.Series = (ChartSeriesBase) series;
    this.customTemplate = series.CustomTemplate;
    this.SetData(x1, y1, x2, y2);
  }

  public ColumnSegment(double x1, double y1, double x2, double y2) => this.SetData(x1, y1, x2, y2);

  public double XData
  {
    get => this.xData;
    internal set
    {
      this.xData = value;
      this.OnPropertyChanged(nameof (XData));
    }
  }

  public double YData
  {
    get => this.yData;
    internal set
    {
      this.yData = value;
      this.OnPropertyChanged(nameof (YData));
    }
  }

  public double Width
  {
    get => this.width;
    set
    {
      this.width = value;
      this.OnPropertyChanged(nameof (Width));
    }
  }

  public double Height
  {
    get => this.height;
    set
    {
      this.height = value;
      this.OnPropertyChanged(nameof (Height));
    }
  }

  public double RectX
  {
    get => this.rectX;
    set
    {
      this.rectX = value;
      this.OnPropertyChanged(nameof (RectX));
    }
  }

  public double RectY
  {
    get => this.rectY;
    set
    {
      this.rectY = value;
      this.OnPropertyChanged(nameof (RectY));
    }
  }

  public override void SetData(params double[] Values)
  {
    this.Left = Values[0];
    this.Top = Values[1];
    this.Right = Values[2];
    this.Bottom = Values[3];
    this.XRange = new DoubleRange(this.Left, this.Right);
    this.YRange = new DoubleRange(this.Top, this.Bottom);
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.customTemplate == null)
    {
      this.RectSegment = new Rectangle();
      this.SetVisualBindings((Shape) this.RectSegment);
      this.RectSegment.Tag = (object) this;
      return (UIElement) this.RectSegment;
    }
    ContentControl contentControl = new ContentControl();
    contentControl.Content = (object) this;
    contentControl.Tag = (object) this;
    contentControl.ContentTemplate = this.customTemplate;
    this.control = contentControl;
    return (UIElement) this.control;
  }

  public override UIElement GetRenderedVisual()
  {
    return this.customTemplate == null ? (UIElement) this.RectSegment : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num1 = isLogarithmic ? Math.Log(this.Left, newBase) : this.Left;
    double num2 = isLogarithmic ? Math.Log(this.Right, newBase) : this.Right;
    double num3 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num4 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    if (!double.IsNaN(this.Top) && !double.IsNaN(this.Bottom) && num1 <= num4 && num2 >= num3 && (!double.IsNaN(this.YData) || this.Series.ShowEmptyPoints))
    {
      double spacing = this.Series is HistogramSeries ? 0.0 : (this.Series as ISegmentSpacing).SegmentSpacing;
      this.rect = new Rect(transformer.TransformToVisible(this.Left, this.Top), transformer.TransformToVisible(this.Right, this.Bottom));
      if (spacing > 0.0 && spacing <= 1.0)
      {
        if (this.Series.IsActualTransposed)
        {
          this.rect.Y = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, this.rect.Bottom, this.rect.Top);
          this.Height = this.rect.Height = (1.0 - spacing) * this.rect.Height;
        }
        else
        {
          this.rect.X = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, this.rect.Right, this.rect.Left);
          this.Width = this.rect.Width = (1.0 - spacing) * this.rect.Width;
        }
      }
      if (this.RectSegment != null)
      {
        this.RectSegment.SetValue(Canvas.LeftProperty, (object) this.rect.X);
        this.RectSegment.SetValue(Canvas.TopProperty, (object) this.rect.Y);
        this.RectSegment.Visibility = Visibility.Visible;
        this.Width = this.RectSegment.Width = this.rect.Width;
        this.Height = this.RectSegment.Height = this.rect.Height;
      }
      else
      {
        this.control.Visibility = Visibility.Visible;
        this.RectX = this.rect.X;
        this.RectY = this.rect.Y;
        this.Width = this.rect.Width;
        this.Height = this.rect.Height;
      }
    }
    else if (this.customTemplate == null)
      this.RectSegment.Visibility = Visibility.Collapsed;
    else
      this.control.Visibility = Visibility.Collapsed;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    base.SetVisualBindings(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
  }

  internal override void Dispose()
  {
    if (this.RectSegment != null)
    {
      this.RectSegment.Tag = (object) null;
      this.RectSegment = (Rectangle) null;
    }
    base.Dispose();
  }
}
