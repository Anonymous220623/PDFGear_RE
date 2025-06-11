// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.WaterfallSegment
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

public class WaterfallSegment : ChartSegment
{
  internal WaterfallSegment PreviousWaterfallSegment;
  protected internal double Left;
  protected internal double Top;
  protected internal double Bottom;
  protected internal double Right;
  protected internal Line LineSegment;
  protected Rectangle WaterfallRectSegment;
  private double rectX;
  private double rectY;
  private double width;
  private double height;
  private Canvas segmentCanvas;
  private WaterfallSegmentType segmentType;

  public WaterfallSegment()
  {
  }

  public WaterfallSegment(double x1, double y1, double x2, double y2, WaterfallSeries series)
  {
    this.Series = (ChartSeriesBase) series;
    this.SetData(x1, y1, x2, y2);
  }

  public WaterfallSegment(double x1, double y1, double x2, double y2)
  {
    this.SetData(x1, y1, x2, y2);
  }

  public double XData { get; internal set; }

  public double YData { get; internal set; }

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

  internal double WaterfallSum { get; set; }

  internal double Sum { get; set; }

  internal WaterfallSegmentType SegmentType
  {
    get => this.segmentType;
    set
    {
      this.segmentType = value;
      this.OnPropertyChanged(nameof (SegmentType));
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
    this.segmentCanvas = new Canvas();
    this.WaterfallRectSegment = new Rectangle();
    this.SetVisualBindings((Shape) this.WaterfallRectSegment);
    Panel.SetZIndex((UIElement) this.WaterfallRectSegment, 1);
    this.segmentCanvas.Children.Add((UIElement) this.WaterfallRectSegment);
    this.WaterfallRectSegment.Tag = (object) this;
    this.LineSegment = new Line();
    this.LineSegment.Style = (this.Series as WaterfallSeries).ConnectorLineStyle ?? ChartDictionaries.GenericCommonDictionary[(object) "defaultWaterfallConnectorStyle"] as Style;
    this.SetVisualBindings((Shape) this.LineSegment);
    Panel.SetZIndex((UIElement) this.LineSegment, 0);
    this.segmentCanvas.Children.Add((UIElement) this.LineSegment);
    this.LineSegment.Tag = (object) this;
    return (UIElement) this.segmentCanvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentCanvas;

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
    if (!double.IsNaN(this.Top) && !double.IsNaN(this.Bottom) && num1 <= num4 && num2 >= num3)
    {
      this.rect = this.CalculateSegmentRect(transformer);
      this.segmentCanvas.Visibility = Visibility.Visible;
      if (this.WaterfallRectSegment == null)
        return;
      this.WaterfallRectSegment.SetValue(Canvas.LeftProperty, (object) this.rect.X);
      this.WaterfallRectSegment.SetValue(Canvas.TopProperty, (object) this.rect.Y);
      this.WaterfallRectSegment.Visibility = Visibility.Visible;
      this.Width = this.WaterfallRectSegment.Width = this.rect.Width;
      this.Height = this.WaterfallRectSegment.Height = this.rect.Height;
      this.UpdateConnectorLine();
    }
    else
      this.segmentCanvas.Visibility = Visibility.Collapsed;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    if (element is Line)
    {
      element.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("ShowConnector", new object[0]),
        Converter = (IValueConverter) new BooleanToVisibilityConverter(),
        Mode = BindingMode.TwoWay
      });
    }
    else
    {
      base.SetVisualBindings(element);
      element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("Stroke", new object[0])
      });
    }
  }

  private void UpdateConnectorLine()
  {
    if (this.PreviousWaterfallSegment != null && (this.Series as WaterfallSeries).ShowConnector)
    {
      Rect rect = this.PreviousWaterfallSegment.rect;
      this.LineSegment.Visibility = Visibility.Visible;
      bool flag = false;
      int num1 = this.Series.Segments.IndexOf((ChartSegment) this);
      if (num1 == this.Series.Segments.Count - 1)
      {
        if (this.Series.IsActualTransposed && this.rect.Width == 0.0)
        {
          if (this.Series.ActualXAxis.IsInversed)
            this.rect.Y += this.rect.Height;
          else
            this.rect.Y -= this.rect.Height;
        }
        else if (!this.Series.IsActualTransposed && this.rect.Height == 0.0)
        {
          if (this.Series.ActualXAxis.IsInversed)
            this.rect.X -= this.rect.Width;
          else
            this.rect.X += this.rect.Width;
        }
      }
      if (this.PreviousWaterfallSegment.segmentType == WaterfallSegmentType.Sum)
      {
        double num2 = num1 == 0 ? this.PreviousWaterfallSegment.YData : this.PreviousWaterfallSegment.WaterfallSum;
        if (num2 < 0.0 && !this.Series.ActualYAxis.IsInversed || num2 > 0.0 && this.Series.ActualYAxis.IsInversed)
          flag = true;
      }
      else if (!this.Series.ActualYAxis.IsInversed)
      {
        if (this.PreviousWaterfallSegment.YData < 0.0)
          flag = true;
      }
      else if (this.Series.ActualYAxis.IsInversed && (this.PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive || this.PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Sum))
        flag = true;
      if (this.Series.IsActualTransposed)
      {
        if (this.Series.ActualXAxis.IsInversed)
        {
          this.LineSegment.Y1 = rect.Width == 0.0 ? rect.Y : rect.Y + rect.Height;
          this.LineSegment.Y2 = this.rect.Y;
        }
        else
        {
          this.LineSegment.Y1 = rect.Width != 0.0 ? rect.Y : rect.Y + rect.Height;
          this.LineSegment.Y2 = this.rect.Y + this.rect.Height;
        }
        if (flag)
        {
          this.LineSegment.X1 = rect.X;
          this.LineSegment.X2 = this.GetCurrentSegmentXValue();
        }
        else
        {
          this.LineSegment.X1 = rect.X + rect.Width;
          this.LineSegment.X2 = this.GetCurrentSegmentXValue();
        }
      }
      else
      {
        if (this.Series.ActualXAxis.IsInversed)
        {
          this.LineSegment.X1 = rect.Height != 0.0 ? rect.X : rect.X + rect.Width;
          this.LineSegment.X2 = this.rect.X + this.rect.Width;
        }
        else
        {
          this.LineSegment.X1 = rect.Height == 0.0 ? rect.X : rect.X + rect.Width;
          this.LineSegment.X2 = this.rect.X;
        }
        if (flag)
        {
          this.LineSegment.Y1 = rect.Y + rect.Height;
          this.LineSegment.Y2 = this.GetCurrentSegmentYValue();
        }
        else
        {
          this.LineSegment.Y1 = rect.Y;
          this.LineSegment.Y2 = this.GetCurrentSegmentYValue();
        }
      }
    }
    else
    {
      if ((this.Series as WaterfallSeries).ShowConnector)
        return;
      this.LineSegment.Visibility = Visibility.Collapsed;
    }
  }

  private double GetCurrentSegmentXValue()
  {
    return !this.Series.ActualYAxis.IsInversed ? (this.SegmentType == WaterfallSegmentType.Negative || this.SegmentType == WaterfallSegmentType.Sum && this.WaterfallSum >= 0.0 && (this.Series as WaterfallSeries).AllowAutoSum || this.SegmentType == WaterfallSegmentType.Sum && this.YData >= 0.0 && !(this.Series as WaterfallSeries).AllowAutoSum ? this.rect.X + this.rect.Width : this.rect.X) : (this.SegmentType == WaterfallSegmentType.Negative || this.SegmentType == WaterfallSegmentType.Sum && this.WaterfallSum >= 0.0 && (this.Series as WaterfallSeries).AllowAutoSum || this.SegmentType == WaterfallSegmentType.Sum && this.YData >= 0.0 && !(this.Series as WaterfallSeries).AllowAutoSum ? this.rect.X : this.rect.X + this.rect.Width);
  }

  private double GetCurrentSegmentYValue()
  {
    return !this.Series.ActualYAxis.IsInversed ? (this.SegmentType == WaterfallSegmentType.Negative || this.SegmentType == WaterfallSegmentType.Sum && this.WaterfallSum >= 0.0 && (this.Series as WaterfallSeries).AllowAutoSum || this.SegmentType == WaterfallSegmentType.Sum && this.YData >= 0.0 && !(this.Series as WaterfallSeries).AllowAutoSum ? this.rect.Y : this.rect.Y + this.rect.Height) : (this.SegmentType == WaterfallSegmentType.Negative || this.SegmentType == WaterfallSegmentType.Sum && this.WaterfallSum >= 0.0 && (this.Series as WaterfallSeries).AllowAutoSum || this.SegmentType == WaterfallSegmentType.Sum && this.YData >= 0.0 && !(this.Series as WaterfallSeries).AllowAutoSum ? this.rect.Y + this.rect.Height : this.rect.Y);
  }

  private Rect CalculateSegmentRect(IChartTransformer transformer)
  {
    double spacing = this.Series is HistogramSeries ? 0.0 : (this.Series as ISegmentSpacing).SegmentSpacing;
    Rect segmentRect = new Rect(transformer.TransformToVisible(this.Left, this.Top), transformer.TransformToVisible(this.Right, this.Bottom));
    if (spacing > 0.0 && spacing <= 1.0)
    {
      if (this.Series.IsActualTransposed)
      {
        double segmentSpacing = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, segmentRect.Bottom, segmentRect.Top);
        segmentRect.Y = segmentSpacing;
        this.Height = segmentRect.Height = (1.0 - spacing) * segmentRect.Height;
      }
      else
      {
        double segmentSpacing = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, segmentRect.Right, segmentRect.Left);
        segmentRect.X = segmentSpacing;
        this.Width = segmentRect.Width = (1.0 - spacing) * segmentRect.Width;
      }
    }
    return segmentRect;
  }
}
