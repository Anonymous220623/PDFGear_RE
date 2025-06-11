// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HiLoOpenCloseSegment
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

public class HiLoOpenCloseSegment : ChartSegment
{
  private Canvas canvas;
  private ChartPoint hipoint;
  private ChartPoint lowpoint;
  private ChartPoint sopoint;
  private ChartPoint eopoint;
  private ChartPoint scpoint;
  private ChartPoint ecpoint;
  private Line hiLoline;
  private Line closeLine;
  private Line openLine;
  private bool isBull;
  private Brush bullFillColor;
  private Brush bearFillColor;

  public HiLoOpenCloseSegment()
  {
  }

  [Obsolete("Use HiLoOpenCloseSegment(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartPoint point5, ChartPoint point6, bool isbull, HiLoOpenCloseSeries series, object item)")]
  public HiLoOpenCloseSegment(
    Point hghpoint,
    Point lowpoint,
    Point sopoint,
    Point eopoint,
    Point scpoint,
    Point ecpoint,
    bool isbull,
    HiLoOpenCloseSeries series,
    object item)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = item;
  }

  public HiLoOpenCloseSegment(
    ChartPoint highpoint,
    ChartPoint lowpoint,
    ChartPoint sopoint,
    ChartPoint eopoint,
    ChartPoint scpoint,
    ChartPoint ecpoint,
    bool isbull,
    HiLoOpenCloseSeries series,
    object item)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = item;
  }

  public Brush ActualInterior
  {
    get
    {
      if (this.Series.ActualArea.SelectedSeriesCollection.Contains(this.Series) && this.Series.ActualArea.GetEnableSeriesSelection() && this.Series.ActualArea.GetSeriesSelectionBrush(this.Series) != null)
        return this.Series.ActualArea.GetSeriesSelectionBrush(this.Series);
      if ((this.Series as ISegmentSelectable).SegmentSelectionBrush != null && this.IsSegmentSelected())
        return (this.Series as ISegmentSelectable).SegmentSelectionBrush;
      if (this.Series.Interior != null)
        return this.Series.Interior;
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
    }
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartPoint point5, ChartPoint point6, bool isbull)")]
  public override void SetData(
    Point hipoint,
    Point lopoint,
    Point sopoint,
    Point eopoint,
    Point scpoint,
    Point ecpoint,
    bool isbull)
  {
    this.hipoint.X = hipoint.X;
    this.hipoint.Y = hipoint.Y;
    this.lowpoint.X = lopoint.X;
    this.lowpoint.Y = lopoint.Y;
    this.sopoint.X = sopoint.X;
    this.sopoint.Y = sopoint.Y;
    this.eopoint.X = eopoint.X;
    this.eopoint.Y = eopoint.Y;
    this.scpoint.X = scpoint.X;
    this.scpoint.Y = scpoint.Y;
    this.ecpoint.X = ecpoint.X;
    this.ecpoint.Y = ecpoint.Y;
    this.IsBull = isbull;
    double[] numArray = this.AlignHiLoSegment(sopoint.Y, scpoint.Y, hipoint.Y, lopoint.Y);
    this.hipoint.Y = numArray[0];
    this.lowpoint.Y = numArray[1];
    this.XRange = new DoubleRange(ChartMath.Min(scpoint.X, ecpoint.X, sopoint.X, eopoint.X), ChartMath.Max(scpoint.X, ecpoint.X, sopoint.X, eopoint.X));
    this.YRange = new DoubleRange(lopoint.Y, hipoint.Y);
  }

  public override void SetData(
    ChartPoint hipoint,
    ChartPoint lopoint,
    ChartPoint sopoint,
    ChartPoint eopoint,
    ChartPoint scpoint,
    ChartPoint ecpoint,
    bool isbull)
  {
    this.hipoint = hipoint;
    this.lowpoint = lopoint;
    this.sopoint = sopoint;
    this.eopoint = eopoint;
    this.scpoint = scpoint;
    this.ecpoint = ecpoint;
    this.IsBull = isbull;
    double[] numArray = this.AlignHiLoSegment(sopoint.Y, scpoint.Y, hipoint.Y, lopoint.Y);
    this.hipoint.Y = numArray[0];
    this.lowpoint.Y = numArray[1];
    this.XRange = new DoubleRange(ChartMath.Min(scpoint.X, ecpoint.X, sopoint.X, eopoint.X), ChartMath.Max(scpoint.X, ecpoint.X, sopoint.X, eopoint.X));
    this.YRange = new DoubleRange(lopoint.Y, hipoint.Y);
  }

  public override UIElement CreateVisual(Size size)
  {
    this.canvas = new Canvas();
    this.hiLoline = new Line();
    this.SetVisualBindings((Shape) this.hiLoline);
    this.canvas.Children.Add((UIElement) this.hiLoline);
    this.openLine = new Line();
    this.SetVisualBindings((Shape) this.openLine);
    this.canvas.Children.Add((UIElement) this.openLine);
    this.closeLine = new Line();
    this.SetVisualBindings((Shape) this.closeLine);
    this.canvas.Children.Add((UIElement) this.closeLine);
    this.hiLoline.Tag = this.openLine.Tag = this.closeLine.Tag = (object) this;
    return (UIElement) this.canvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.canvas;

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num1 = isLogarithmic ? Math.Log(this.XRange.Start, newBase) : this.XRange.Start;
    double num2 = isLogarithmic ? Math.Log(this.XRange.End, newBase) : this.XRange.End;
    double num3 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num4 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    if (num1 <= num4 && num2 >= num3 || this.Series.ShowEmptyPoints)
    {
      Point visible1 = transformer.TransformToVisible(this.hipoint.X, this.hipoint.Y);
      Point visible2 = transformer.TransformToVisible(this.lowpoint.X, this.lowpoint.Y);
      Point visible3 = transformer.TransformToVisible(this.sopoint.X, this.sopoint.Y);
      Point visible4 = transformer.TransformToVisible(this.eopoint.X, this.eopoint.Y);
      Point visible5 = transformer.TransformToVisible(this.scpoint.X, this.scpoint.Y);
      Point visible6 = transformer.TransformToVisible(this.ecpoint.X, this.ecpoint.Y);
      if (!double.IsNaN(this.hipoint.Y) && !double.IsNaN(this.lowpoint.Y))
      {
        this.hiLoline.X1 = visible1.X;
        this.hiLoline.Y1 = visible1.Y;
        this.hiLoline.X2 = visible2.X;
        this.hiLoline.Y2 = visible2.Y;
      }
      else
        this.hiLoline.ClearUIValues();
      if (!double.IsNaN(this.sopoint.Y) && !double.IsNaN(this.eopoint.Y))
      {
        this.openLine.X1 = visible3.X;
        this.openLine.Y1 = visible3.Y;
        this.openLine.X2 = visible4.X;
        this.openLine.Y2 = visible4.Y;
      }
      else
        this.openLine.ClearUIValues();
      if (!double.IsNaN(this.scpoint.Y) && !double.IsNaN(this.ecpoint.Y))
      {
        this.closeLine.X1 = visible5.X;
        this.closeLine.Y1 = visible5.Y;
        this.closeLine.X2 = visible6.X;
        this.closeLine.Y2 = visible6.Y;
      }
      else
        this.closeLine.ClearUIValues();
    }
    else
    {
      this.hiLoline.ClearUIValues();
      this.openLine.ClearUIValues();
      this.closeLine.ClearUIValues();
    }
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ActualInterior", new object[0])
    });
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  protected override void OnPropertyChanged(string name)
  {
    if (name == "Interior")
      name = "ActualInterior";
    base.OnPropertyChanged(name);
  }

  internal override void Dispose()
  {
    if (this.canvas != null)
    {
      this.canvas.Children.Clear();
      this.canvas = (Canvas) null;
    }
    if (this.openLine != null)
    {
      this.openLine.Tag = (object) null;
      this.openLine = (Line) null;
    }
    if (this.closeLine != null)
    {
      this.closeLine.Tag = (object) null;
      this.closeLine = (Line) null;
    }
    if (this.hiLoline != null)
    {
      this.hiLoline.Tag = (object) null;
      this.hiLoline = (Line) null;
    }
    base.Dispose();
  }
}
