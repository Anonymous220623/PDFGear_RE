// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BubbleSegment
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

public class BubbleSegment : ChartSegment
{
  private double segmentRadius;
  private double size;
  private Ellipse ellipseSegment;
  private double xPos;
  private double yPos;
  private ContentControl control;
  private DataTemplate customTemplate;

  public BubbleSegment()
  {
  }

  public BubbleSegment(double xPos, double yPos, double size, BubbleSeries series)
  {
    this.segmentRadius = size;
    this.customTemplate = series.CustomTemplate;
  }

  public double XData { get; internal set; }

  public double YData { get; internal set; }

  public double Size
  {
    get => this.size;
    set
    {
      this.size = value;
      this.OnPropertyChanged(nameof (Size));
    }
  }

  public double SegmentRadius
  {
    get => this.segmentRadius;
    set
    {
      this.segmentRadius = value;
      this.OnPropertyChanged(nameof (SegmentRadius));
    }
  }

  public double RectX
  {
    get => this.xPos;
    set
    {
      this.xPos = value;
      this.OnPropertyChanged(nameof (RectX));
    }
  }

  public double RectY
  {
    get => this.yPos;
    set
    {
      this.yPos = value;
      this.OnPropertyChanged(nameof (RectY));
    }
  }

  public override void SetData(params double[] Values)
  {
    this.XData = Values[0];
    this.YData = Values[1];
    this.xPos = Values[0];
    this.yPos = Values[1];
    this.XRange = new DoubleRange(this.xPos, this.xPos);
    this.YRange = new DoubleRange(this.yPos, this.yPos);
    if (!(this.Series is BubbleSeries series))
      return;
    this.customTemplate = series.CustomTemplate;
  }

  public override UIElement CreateVisual(System.Windows.Size size)
  {
    if (this.customTemplate == null)
    {
      this.ellipseSegment = new Ellipse();
      this.SetVisualBindings((Shape) this.ellipseSegment);
      this.ellipseSegment.Tag = (object) this;
      return (UIElement) this.ellipseSegment;
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
    return this.customTemplate == null ? (UIElement) this.ellipseSegment : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    double num3 = isLogarithmic ? Math.Log(this.xPos, newBase) : this.xPos;
    if (num3 >= num1 && num3 <= num2 && (!double.IsNaN(this.yPos) || this.Series.ShowEmptyPoints))
    {
      Point visible = transformer.TransformToVisible(this.xPos, this.yPos);
      if (this.ellipseSegment != null)
      {
        this.ellipseSegment.Visibility = Visibility.Visible;
        this.ellipseSegment.Height = this.ellipseSegment.Width = 2.0 * this.segmentRadius;
        this.ellipseSegment.SetValue(Canvas.LeftProperty, (object) (visible.X - this.segmentRadius));
        this.ellipseSegment.SetValue(Canvas.TopProperty, (object) (visible.Y - this.segmentRadius));
      }
      else
      {
        this.control.Visibility = Visibility.Visible;
        this.RectX = visible.X - this.segmentRadius;
        this.RectY = visible.Y - this.segmentRadius;
        this.Size = this.SegmentRadius = 2.0 * this.segmentRadius;
      }
    }
    else if (this.customTemplate == null)
      this.ellipseSegment.Visibility = Visibility.Collapsed;
    else
      this.control.Visibility = Visibility.Collapsed;
  }

  public override void OnSizeChanged(System.Windows.Size size)
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
    if (this.ellipseSegment != null)
    {
      this.ellipseSegment.Tag = (object) null;
      this.ellipseSegment = (Ellipse) null;
    }
    base.Dispose();
  }
}
