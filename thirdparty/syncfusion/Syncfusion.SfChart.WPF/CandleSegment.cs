// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CandleSegment
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

public class CandleSegment : ChartSegment
{
  private bool isBull;
  private bool isFill;
  private ChartPoint cdpBottomLeft;
  private ChartPoint cdpRightTop;
  private ChartPoint hiPoint;
  private ChartPoint loPoint;
  private ChartPoint hiPoint1;
  private ChartPoint loPoint1;
  private Line hiLoLine;
  private Line hiLoLine1;
  private Line openCloseLine;
  private Canvas segmentCanvas;
  private Rectangle columnSegment;
  private Brush bullFillColor;
  private Brush bearFillColor;

  public CandleSegment()
  {
  }

  public CandleSegment(
    Point cdpBottomLeft,
    Point cdpRightTop,
    Point hipoint,
    Point lopoint,
    bool isbull,
    CandleSeries series,
    object item)
  {
  }

  public Brush ActualInterior
  {
    get
    {
      if (this.Series.ActualArea.SelectedSeriesCollection.Contains(this.Series) && this.Series.ActualArea.GetEnableSeriesSelection() && this.Series.ActualArea.GetSeriesSelectionBrush(this.Series) != null)
        return this.Series.ActualArea.GetSeriesSelectionBrush(this.Series);
      if ((this.Series as ISegmentSelectable).SegmentSelectionBrush != null && this.IsSegmentSelected())
        return (this.Series as ISegmentSelectable).SegmentSelectionBrush;
      if (this.isFill || (this.Series as CandleSeries).ComparisonMode == FinancialPrice.None)
      {
        if (this.Series.Interior != null)
          return this.Series.Interior;
        return !this.IsBull ? this.BearFillColor : this.BullFillColor;
      }
      return this.Series.Interior == null ? (Brush) new SolidColorBrush(Colors.Transparent) : this.Interior;
    }
  }

  public Brush ActualStroke
  {
    get
    {
      CandleSeries series = this.Series as CandleSeries;
      if (series.Stroke != null)
        return series.Stroke;
      return !this.IsBull ? this.BearFillColor : this.BullFillColor;
    }
  }

  public Brush BearFillColor
  {
    get => this.bearFillColor != null ? this.bearFillColor : this.Interior;
    set
    {
      if (this.bearFillColor == value)
        return;
      this.bearFillColor = value;
      this.OnPropertyChanged("ActualInterior");
    }
  }

  public Brush BullFillColor
  {
    get => this.bullFillColor != null ? this.bullFillColor : this.Interior;
    set
    {
      if (this.bullFillColor == value)
        return;
      this.bullFillColor = value;
      this.OnPropertyChanged("ActualInterior");
    }
  }

  public double High { get; set; }

  public double Low { get; set; }

  public double Open { get; set; }

  public double Close { get; set; }

  private bool IsBull
  {
    get => this.isBull;
    set
    {
      if (this.isBull == value)
        return;
      this.isBull = value;
      this.OnPropertyChanged("ActualInterior");
      this.OnPropertyChanged("ActualStroke");
    }
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3,ChartPoint point4,bool isBull)")]
  public override void SetData(
    Point BottomLeft,
    Point RightTop,
    Point hipoint,
    Point loPoint,
    bool isBull)
  {
    this.cdpBottomLeft.X = BottomLeft.X;
    this.cdpBottomLeft.Y = BottomLeft.Y;
    this.cdpRightTop.X = RightTop.X;
    this.cdpRightTop.Y = RightTop.Y;
    this.hiPoint.X = hipoint.X;
    this.hiPoint.Y = hipoint.Y;
    this.loPoint.X = loPoint.X;
    this.loPoint.Y = loPoint.Y;
    this.isFill = this.cdpRightTop.Y < this.cdpBottomLeft.Y;
    if (!this.isFill)
    {
      this.hiPoint1 = new ChartPoint(this.hiPoint.X, this.cdpRightTop.Y);
      this.loPoint1 = new ChartPoint(loPoint.X, this.cdpBottomLeft.Y);
    }
    else
    {
      this.hiPoint1 = new ChartPoint(this.hiPoint.X, this.cdpBottomLeft.Y);
      this.loPoint1 = new ChartPoint(loPoint.X, this.cdpRightTop.Y);
    }
    this.IsBull = isBull;
    double[] numArray = this.AlignHiLoSegment(this.cdpBottomLeft.Y, this.cdpRightTop.Y, this.hiPoint.Y, loPoint.Y);
    this.hiPoint.Y = numArray[0];
    this.loPoint.Y = numArray[1];
    this.XRange = DoubleRange.Union(BottomLeft.X, RightTop.X, hipoint.X, loPoint.X);
    if (!double.IsNaN(BottomLeft.Y) || !double.IsNaN(RightTop.Y) || !double.IsNaN(hipoint.Y) || !double.IsNaN(loPoint.Y))
      this.YRange = DoubleRange.Union(BottomLeft.Y, RightTop.Y, hipoint.Y, loPoint.Y);
    else
      this.YRange = DoubleRange.Empty;
  }

  public override void SetData(
    ChartPoint BottomLeft,
    ChartPoint RightTop,
    ChartPoint hipoint,
    ChartPoint loPoint,
    bool isBull)
  {
    this.cdpBottomLeft = BottomLeft;
    this.cdpRightTop = RightTop;
    this.hiPoint = hipoint;
    this.loPoint = loPoint;
    this.isFill = this.cdpRightTop.Y < this.cdpBottomLeft.Y;
    if (!this.isFill)
    {
      this.hiPoint1 = new ChartPoint(this.hiPoint.X, this.cdpRightTop.Y);
      this.loPoint1 = new ChartPoint(loPoint.X, this.cdpBottomLeft.Y);
    }
    else
    {
      this.hiPoint1 = new ChartPoint(this.hiPoint.X, this.cdpBottomLeft.Y);
      this.loPoint1 = new ChartPoint(loPoint.X, this.cdpRightTop.Y);
    }
    this.IsBull = isBull;
    double[] numArray = this.AlignHiLoSegment(this.cdpBottomLeft.Y, this.cdpRightTop.Y, this.hiPoint.Y, loPoint.Y);
    this.hiPoint.Y = numArray[0];
    this.loPoint.Y = numArray[1];
    this.XRange = DoubleRange.Union(BottomLeft.X, RightTop.X, hipoint.X, loPoint.X);
    if (!double.IsNaN(BottomLeft.Y) || !double.IsNaN(RightTop.Y) || !double.IsNaN(hipoint.Y) || !double.IsNaN(loPoint.Y))
      this.YRange = DoubleRange.Union(BottomLeft.Y, RightTop.Y, hipoint.Y, loPoint.Y);
    else
      this.YRange = DoubleRange.Empty;
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segmentCanvas = new Canvas();
    this.columnSegment = new Rectangle();
    this.SetVisualBindings((Shape) this.columnSegment);
    Panel.SetZIndex((UIElement) this.columnSegment, 1);
    this.segmentCanvas.Children.Add((UIElement) this.columnSegment);
    this.columnSegment.Tag = (object) this;
    this.hiLoLine = new Line();
    this.SetVisualBindings((Shape) this.hiLoLine);
    Panel.SetZIndex((UIElement) this.hiLoLine, 0);
    this.segmentCanvas.Children.Add((UIElement) this.hiLoLine);
    this.hiLoLine.Tag = (object) this;
    this.hiLoLine1 = new Line();
    this.SetVisualBindings((Shape) this.hiLoLine1);
    Panel.SetZIndex((UIElement) this.hiLoLine1, 0);
    this.segmentCanvas.Children.Add((UIElement) this.hiLoLine1);
    this.hiLoLine1.Tag = (object) this;
    this.openCloseLine = new Line();
    this.SetVisualBindings((Shape) this.openCloseLine);
    this.segmentCanvas.Children.Add((UIElement) this.openCloseLine);
    this.openCloseLine.Tag = (object) this;
    return (UIElement) this.segmentCanvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentCanvas;

  public override void Update(IChartTransformer transformer)
  {
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num3 = isLogarithmic ? Math.Log(this.cdpBottomLeft.X, newBase) : this.cdpBottomLeft.X;
    double num4 = isLogarithmic ? Math.Log(this.cdpRightTop.X, newBase) : this.cdpRightTop.X;
    if (num3 <= num2 && num4 >= num1 || this.Series.ShowEmptyPoints)
    {
      double segmentSpacing = (this.Series as ISegmentSpacing).SegmentSpacing;
      this.columnSegment.Visibility = Visibility.Visible;
      this.hiLoLine.Visibility = Visibility.Visible;
      this.hiLoLine1.Visibility = Visibility.Visible;
      this.openCloseLine.Visibility = Visibility.Visible;
      this.rect = new Rect(transformer.TransformToVisible(this.cdpBottomLeft.X, this.cdpBottomLeft.Y), transformer.TransformToVisible(this.cdpRightTop.X, this.cdpRightTop.Y));
      if (segmentSpacing > 0.0 && segmentSpacing <= 1.0)
      {
        if (this.Series.IsActualTransposed)
        {
          this.rect.Y = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing, this.rect.Bottom, this.rect.Top);
          this.columnSegment.Height = this.rect.Height = (1.0 - segmentSpacing) * this.rect.Height;
        }
        else
        {
          this.rect.X = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing, this.rect.Right, this.rect.Left);
          this.columnSegment.Width = this.rect.Width = (1.0 - segmentSpacing) * this.rect.Width;
        }
      }
      if (this.cdpBottomLeft.Y == this.cdpRightTop.Y)
      {
        if (!double.IsNaN(this.cdpBottomLeft.Y) && !double.IsNaN(this.cdpRightTop.Y))
        {
          this.columnSegment.Visibility = Visibility.Collapsed;
          this.openCloseLine.X1 = this.rect.Left;
          this.openCloseLine.Y1 = this.rect.Top;
          this.openCloseLine.X2 = this.rect.Right;
          this.openCloseLine.Y2 = this.rect.Bottom;
        }
        else
        {
          this.openCloseLine.Visibility = Visibility.Collapsed;
          this.openCloseLine.ClearUIValues();
        }
      }
      else if (!double.IsNaN(this.cdpBottomLeft.Y) && !double.IsNaN(this.cdpRightTop.Y))
      {
        this.openCloseLine.Visibility = Visibility.Collapsed;
        this.columnSegment.SetValue(Canvas.LeftProperty, (object) this.rect.X);
        this.columnSegment.SetValue(Canvas.TopProperty, (object) this.rect.Y);
        this.columnSegment.Width = this.rect.Width;
        this.columnSegment.Height = this.rect.Height;
      }
      else
      {
        this.columnSegment.Visibility = Visibility.Collapsed;
        this.columnSegment.ClearUIValues();
      }
      Point visible1 = transformer.TransformToVisible(this.hiPoint.X, this.hiPoint.Y);
      Point visible2 = transformer.TransformToVisible(this.hiPoint1.X, this.hiPoint1.Y);
      Point visible3 = transformer.TransformToVisible(this.loPoint.X, this.loPoint.Y);
      Point visible4 = transformer.TransformToVisible(this.loPoint1.X, this.loPoint1.Y);
      if (!double.IsNaN(visible1.Y) && !double.IsNaN(visible2.Y) && !double.IsNaN(visible1.X) && !double.IsNaN(visible2.X) && !double.IsNaN(visible3.Y) && !double.IsNaN(visible4.Y) && !double.IsNaN(visible3.X) && !double.IsNaN(visible4.X))
      {
        this.hiLoLine.X1 = visible1.X;
        this.hiLoLine.X2 = visible2.X;
        this.hiLoLine.Y1 = visible1.Y;
        this.hiLoLine.Y2 = visible2.Y;
        this.hiLoLine1.X1 = visible3.X;
        this.hiLoLine1.X2 = visible4.X;
        this.hiLoLine1.Y1 = visible3.Y;
        this.hiLoLine1.Y2 = visible4.Y;
        if (this.rect.Contains(visible1))
          this.hiLoLine.Visibility = Visibility.Collapsed;
        if (!this.rect.Contains(visible3))
          return;
        this.hiLoLine1.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.hiLoLine.Visibility = Visibility.Collapsed;
        this.hiLoLine.ClearUIValues();
        this.hiLoLine1.Visibility = Visibility.Collapsed;
        this.hiLoLine1.ClearUIValues();
      }
    }
    else
    {
      this.columnSegment.ClearUIValues();
      this.hiLoLine.ClearUIValues();
      this.hiLoLine1.ClearUIValues();
      this.openCloseLine.ClearUIValues();
      this.columnSegment.Visibility = Visibility.Collapsed;
      this.hiLoLine.Visibility = Visibility.Collapsed;
      this.hiLoLine1.Visibility = Visibility.Collapsed;
      this.openCloseLine.Visibility = Visibility.Collapsed;
    }
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ActualInterior", new object[0])
    });
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ActualStroke", new object[0])
    });
  }

  protected override void OnPropertyChanged(string name)
  {
    switch (name)
    {
      case "Interior":
        name = "ActualInterior";
        break;
      case "Stroke":
        name = "ActualStroke";
        break;
    }
    base.OnPropertyChanged(name);
  }

  internal override void Dispose()
  {
    if (this.segmentCanvas != null)
    {
      this.segmentCanvas.Children.Clear();
      this.segmentCanvas = (Canvas) null;
    }
    if (this.columnSegment != null)
    {
      this.columnSegment.Tag = (object) null;
      this.columnSegment = (Rectangle) null;
    }
    if (this.hiLoLine != null)
    {
      this.hiLoLine.Tag = (object) null;
      this.hiLoLine = (Line) null;
    }
    if (this.hiLoLine1 != null)
    {
      this.hiLoLine1.Tag = (object) null;
      this.hiLoLine1 = (Line) null;
    }
    if (this.openCloseLine != null)
    {
      this.openCloseLine.Tag = (object) null;
      this.openCloseLine = (Line) null;
    }
    base.Dispose();
  }
}
