// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceAxisLabelLayout
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class SurfaceAxisLabelLayout
{
  private readonly List<UIElement> children;
  protected Thickness Margin = new Thickness(2.0, 2.0, 2.0, 2.0);

  protected List<Dictionary<int, Rect>> RectssByRowsAndCols { get; set; }

  protected List<Size> ComputedSizes { get; set; }

  protected List<Size> DesiredSizes { get; set; }

  protected SurfaceAxis Axis { get; set; }

  protected List<UIElement> Children => this.children;

  protected SurfaceAxisLabelLayout(SurfaceAxis axis, List<UIElement> elements)
  {
    this.Axis = axis;
    this.children = elements;
    this.DesiredSizes = new List<Size>();
  }

  protected bool IntersectsWith(Rect r1, Rect r2, int prevIndex, int currentIndex)
  {
    return r2.Left <= r1.Right && r2.Right >= r1.Left && r2.Top <= r1.Bottom && r2.Bottom >= r1.Top;
  }

  public static SurfaceAxisLabelLayout CreateAxisLayout(
    SurfaceAxis chartAxis,
    List<UIElement> elements)
  {
    return chartAxis.Orientation == Orientation.Horizontal ? (SurfaceAxisLabelLayout) new SurfaceHorizontalLabelLayout(chartAxis, elements) : (SurfaceAxisLabelLayout) new SurfaceVerticalLabelLayout(chartAxis, elements);
  }

  public virtual Size Measure(Size availableSize)
  {
    if (this.Axis != null && this.Children.Count > 0)
    {
      bool isContour = this.Axis.Area.IsContour;
      this.ComputedSizes = this.DesiredSizes;
      if (isContour)
        this.ComputedSizes = new List<Size>();
      this.AxisLabelsVisibilityBinding();
      foreach (FrameworkElement child in this.Children)
      {
        child.Measure(availableSize);
        this.DesiredSizes.Add(child.DesiredSize);
        if (isContour)
        {
          child.RenderTransformOrigin = new Point(0.5, 0.5);
          double angle = this.Axis == this.Axis.Area.InterernalZAxis ? 90.0 - this.Axis.Area.Rotate : -this.Axis.Area.Rotate;
          child.RenderTransform = (Transform) new RotateTransform()
          {
            Angle = angle
          };
          this.ComputedSizes.Add(SurfaceAxisLabelLayout.GetRotatedSize(angle, this.DesiredSizes.Last<Size>()));
        }
        else
          child.RenderTransform = (Transform) null;
      }
    }
    return new Size();
  }

  private static Size GetRotatedSize(double angle, Size size)
  {
    double num1 = 2.0 * Math.PI * angle / 360.0;
    double m12 = Math.Sin(num1);
    double num2 = Math.Cos(num1);
    Matrix matrix = new Matrix(num2, m12, -m12, num2, 0.0, 0.0);
    Point point1 = matrix.Transform(new Point(0.0, 0.0));
    Point point2 = matrix.Transform(new Point(size.Width, 0.0));
    Point point3 = matrix.Transform(new Point(0.0, size.Height));
    Point point4 = matrix.Transform(new Point(size.Width, size.Height));
    double num3 = Math.Min(Math.Min(point1.X, point2.X), Math.Min(point3.X, point4.X));
    double num4 = Math.Min(Math.Min(point1.Y, point2.Y), Math.Min(point3.Y, point4.Y));
    double num5 = Math.Max(Math.Max(point1.X, point2.X), Math.Max(point3.X, point4.X));
    double num6 = Math.Max(Math.Max(point1.Y, point2.Y), Math.Max(point3.Y, point4.Y));
    return new Size(num5 - num3, num6 - num4);
  }

  protected void InsertToRowOrColumn(int rowOrColIndex, int itemIndex, Rect rect)
  {
    if (this.RectssByRowsAndCols.Count <= rowOrColIndex)
    {
      this.RectssByRowsAndCols.Add(new Dictionary<int, Rect>());
      this.RectssByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
    }
    else
    {
      KeyValuePair<int, Rect> keyValuePair = this.RectssByRowsAndCols[rowOrColIndex].Last<KeyValuePair<int, Rect>>();
      if (this.IntersectsWith(keyValuePair.Value, rect, keyValuePair.Key, itemIndex))
        this.InsertToRowOrColumn(++rowOrColIndex, itemIndex, rect);
      else
        this.RectssByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
    }
  }

  protected virtual void CalcBounds(double size)
  {
  }

  public virtual void Arrange(Size finalSize)
  {
  }

  protected virtual double SurfaceLayoutElements()
  {
    int num1 = 1;
    int num2 = 0;
    for (; num1 < this.Children.Count; ++num1)
    {
      if (this.IntersectsWith(this.RectssByRowsAndCols[0][num2], this.RectssByRowsAndCols[0][num1], num2, num1))
        this.Children[num1].Visibility = Visibility.Collapsed;
      else
        num2 = num1;
    }
    return 0.0;
  }

  private void AxisLabelsVisibilityBinding()
  {
    foreach (FrameworkElement child in this.Children)
      child.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Axis,
        Path = new PropertyPath("Visibility", new object[0])
      });
  }
}
