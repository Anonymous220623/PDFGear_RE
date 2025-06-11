// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ScatterSegment
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

public class ScatterSegment : ChartSegment
{
  internal DataTemplate CustomTemplate;
  protected Ellipse EllipseSegment;
  private double yData;
  private double xData;
  private double scatterWidth;
  private double scatterHeight;
  private double xPos;
  private double yPos;
  private ContentControl control;

  public ScatterSegment()
  {
  }

  public ScatterSegment(double xpos, double ypos, ScatterSeries series)
  {
    this.CustomTemplate = series.CustomTemplate;
  }

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

  public double ScatterWidth
  {
    get => this.scatterWidth;
    set
    {
      if (this.scatterWidth == value)
        return;
      this.scatterWidth = value;
      this.OnPropertyChanged(nameof (ScatterWidth));
    }
  }

  public double ScatterHeight
  {
    get => this.scatterHeight;
    set
    {
      if (this.scatterHeight == value)
        return;
      this.scatterHeight = value;
      this.OnPropertyChanged(nameof (ScatterHeight));
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
    if (!double.IsNaN(this.xPos))
      this.XRange = DoubleRange.Union(this.xPos);
    else
      this.XRange = DoubleRange.Empty;
    if (!double.IsNaN(this.yPos))
      this.YRange = DoubleRange.Union(this.yPos);
    else
      this.YRange = DoubleRange.Empty;
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.CustomTemplate == null)
    {
      this.EllipseSegment = new Ellipse();
      Binding binding = new Binding();
      binding.Source = (object) this;
      binding.Path = new PropertyPath("ScatterWidth", new object[0]);
      this.EllipseSegment.Tag = (object) this;
      this.EllipseSegment.SetBinding(FrameworkElement.WidthProperty, (BindingBase) binding);
      this.EllipseSegment.SetBinding(FrameworkElement.HeightProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("ScatterHeight", new object[0])
      });
      this.EllipseSegment.Tag = (object) this;
      this.SetVisualBindings((Shape) this.EllipseSegment);
      return (UIElement) this.EllipseSegment;
    }
    ContentControl contentControl = new ContentControl();
    contentControl.Content = (object) this;
    contentControl.Tag = (object) this;
    contentControl.ContentTemplate = this.CustomTemplate;
    this.control = contentControl;
    return (UIElement) this.control;
  }

  public override UIElement GetRenderedVisual()
  {
    return this.CustomTemplate == null ? (UIElement) this.EllipseSegment : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer)
    {
      double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
      double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
      double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
      double num3 = isLogarithmic ? Math.Log(this.xPos, newBase) : this.xPos;
      if (num3 >= num1 && num3 <= num2 && (!double.IsNaN(this.YData) || this.Series.ShowEmptyPoints))
      {
        Point visible = transformer.TransformToVisible(this.xPos, this.yPos);
        if (this is EmptyPointSegment emptyPointSegment)
        {
          this.ScatterHeight = emptyPointSegment.EmptyPointSymbolHeight;
          this.ScatterWidth = emptyPointSegment.EmptyPointSymbolWidth;
        }
        else if (this.Series is ScatterSeries series)
        {
          this.ScatterHeight = series.ScatterHeight;
          this.ScatterWidth = series.ScatterWidth;
        }
        if (this.EllipseSegment != null)
        {
          this.EllipseSegment.SetValue(Canvas.LeftProperty, (object) (visible.X - this.ScatterWidth / 2.0));
          this.EllipseSegment.SetValue(Canvas.TopProperty, (object) (visible.Y - this.ScatterHeight / 2.0));
        }
        else
        {
          this.control.Visibility = Visibility.Visible;
          this.RectX = visible.X - this.ScatterWidth / 2.0;
          this.RectY = visible.Y - this.ScatterHeight / 2.0;
        }
      }
      else if (this.CustomTemplate == null)
      {
        this.ScatterHeight = 0.0;
        this.ScatterWidth = 0.0;
      }
      else
        this.control.Visibility = Visibility.Collapsed;
    }
    else
    {
      this.ScatterWidth = 0.0;
      this.ScatterHeight = 0.0;
    }
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
    if (this.EllipseSegment != null)
    {
      this.EllipseSegment.Tag = (object) null;
      this.EllipseSegment = (Ellipse) null;
    }
    if (this.control != null)
    {
      this.control.Tag = (object) null;
      this.control = (ContentControl) null;
    }
    base.Dispose();
  }
}
