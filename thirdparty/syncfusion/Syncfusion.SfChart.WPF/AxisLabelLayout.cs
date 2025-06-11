// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AxisLabelLayout
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class AxisLabelLayout
{
  private readonly List<UIElement> children;
  protected double BorderPadding = 10.0;
  protected double AngleForAutoRotate;
  protected Thickness Margin = new Thickness().GetThickness(2.0, 2.0, 2.0, 2.0);

  protected AxisLabelLayout(ChartAxis axis, List<UIElement> elements)
  {
    this.Axis = axis;
    this.children = elements;
    this.DesiredSizes = new List<Size>();
  }

  internal double Left { get; set; }

  internal double Top { get; set; }

  internal List<Border> Borders { get; set; }

  internal Size AvailableSize { get; set; }

  protected internal List<Dictionary<int, Rect>> RectssByRowsAndCols { get; set; }

  protected List<Size> ComputedSizes { get; set; }

  protected List<Size> DesiredSizes { get; set; }

  protected ChartAxis Axis { get; set; }

  protected List<UIElement> Children => this.children;

  public static AxisLabelLayout CreateAxisLayout(ChartAxis chartAxis, List<UIElement> elements)
  {
    return chartAxis.Orientation == Orientation.Horizontal ? (AxisLabelLayout) new HorizontalLabelLayout(chartAxis, elements) : (AxisLabelLayout) new VerticalLabelLayout(chartAxis, elements);
  }

  public virtual Size Measure(Size availableSize)
  {
    if (this.Axis != null && this.Children.Count > 0)
    {
      bool flag = !double.IsNaN(this.Axis.LabelRotationAngle) && this.Axis.LabelRotationAngle != 0.0 || this.AngleForAutoRotate != 0.0;
      this.ComputedSizes = this.DesiredSizes;
      if (flag)
        this.ComputedSizes = new List<Size>();
      this.AxisLabelsVisibilityBinding();
      Size availableSize1 = new Size();
      foreach (FrameworkElement child in this.Children)
      {
        availableSize1.Width = Math.Max(availableSize.Width, child.DesiredSize.Width);
        availableSize1.Height = Math.Max(availableSize.Height, child.DesiredSize.Height);
        child.Measure(availableSize1);
        this.DesiredSizes.Add(child.DesiredSize);
        if (flag)
        {
          int num1 = this.Children.IndexOf((UIElement) child);
          double num2;
          double angle;
          if (this.AngleForAutoRotate != 0.0)
          {
            angle = num2 = this.AngleForAutoRotate;
          }
          else
          {
            angle = this.Axis.LabelRotationAngle;
            num2 = Math.Abs(this.Axis.LabelRotationAngle);
          }
          if (angle < -360.0 || angle > 360.0)
            angle %= 360.0;
          child.RenderTransformOrigin = new Point(0.5, 0.5);
          RotateTransform rotateTransform = new RotateTransform()
          {
            Angle = angle
          };
          double num3 = angle;
          if (!this.Axis.OpposedPosition && this.Axis.GetLabelPosition() == AxisElementPosition.Outside || this.Axis.OpposedPosition && this.Axis.GetLabelPosition() == AxisElementPosition.Inside)
          {
            if (this.Axis.Orientation == Orientation.Horizontal)
            {
              if (num3 > 180.0 && num3 < 360.0 || num3 < 0.0 && num3 > -180.0)
                num3 -= 180.0;
            }
            else if (num2 > 0.0 && num2 < 90.0 || num2 > 270.0 && num2 < 360.0)
              num3 += 180.0;
          }
          else if (this.Axis.Orientation == Orientation.Horizontal)
          {
            if (num3 > 0.0 && num3 < 180.0 || num3 < -180.0 && num3 > -360.0)
              num3 += 180.0;
          }
          else if (num2 > 90.0 && num2 < 180.0 || num2 > 180.0 && num2 < 270.0)
            num3 += 180.0;
          double num4 = Math.PI * num3 / 180.0;
          double num5 = child.DesiredSize.Width / 2.0;
          double num6 = Math.Sin(num4) * num5;
          double num7 = Math.Cos(num4) * num5;
          if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift && (num1 == 0 || num1 == this.Children.Count - 1))
          {
            num7 = 0.0;
            num6 = 0.0;
          }
          TranslateTransform translateTransform;
          if (this.Axis.Orientation == Orientation.Horizontal)
          {
            if (num2 >= 0.0 && num2 < 1.0 || num2 > 359.0 && num2 <= 360.0 || num2 > 179.0 && num2 < 181.0 || this.Axis is CategoryAxis && (this.Axis as CategoryAxis).LabelPlacement == LabelPlacement.BetweenTicks)
              translateTransform = new TranslateTransform();
            else
              translateTransform = new TranslateTransform()
              {
                X = num7
              };
          }
          else if (num2 > 89.0 && num2 < 91.0 || num2 > 269.0 && num2 < 271.0)
            translateTransform = new TranslateTransform();
          else
            translateTransform = new TranslateTransform()
            {
              Y = num6
            };
          child.RenderTransform = (Transform) new TransformGroup()
          {
            Children = {
              (Transform) rotateTransform,
              (Transform) translateTransform
            }
          };
          this.ComputedSizes.Add(AxisLabelLayout.GetRotatedSize(angle, this.DesiredSizes.Last<Size>()));
        }
        else
          child.RenderTransform = (Transform) null;
      }
      this.CalculateActualPlotOffset(availableSize);
    }
    return new Size();
  }

  public virtual void Arrange(Size finalSize)
  {
  }

  protected bool CheckCartesianSeries()
  {
    return this.Axis.RegisteredSeries != null && this.Axis.RegisteredSeries.Count > 0 && this.Axis.RegisteredSeries.Any<ISupportAxes>((Func<ISupportAxes, bool>) (series => series is CartesianSeries && (series as CartesianSeries).IsSideBySide));
  }

  protected bool CheckLabelPlacement(bool isSidebySideSeries)
  {
    CategoryAxis axis1 = this.Axis as CategoryAxis;
    if (!(this.Axis is ChartAxisBase2D axis2) || axis2.IsZoomed || this.RectssByRowsAndCols.Count != 1)
      return false;
    if (axis1 != null && axis1.LabelPlacement == LabelPlacement.BetweenTicks)
      return true;
    return !this.Axis.IsSecondaryAxis && isSidebySideSeries;
  }

  protected bool IntersectsWith(Rect r1, Rect r2, int prevIndex, int currentIndex)
  {
    double angle = this.AngleForAutoRotate == 45.0 ? 45.0 : this.Axis.LabelRotationAngle;
    if (angle != 0.0)
      return AxisLabelLayout.IntersectsWith(this.GetRotatedPoints(r1, prevIndex, angle), this.GetRotatedPoints(r2, currentIndex, angle));
    return r2.Left <= r1.Right && r2.Right >= r1.Left && r2.Top <= r1.Bottom && r2.Bottom >= r1.Top;
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

  protected static bool IsOpposed(ChartAxis axis, bool isAxisOpposed)
  {
    if (axis == null)
      return false;
    if (isAxisOpposed && axis.GetLabelPosition() == AxisElementPosition.Outside)
      return true;
    return !isAxisOpposed && axis.GetLabelPosition() == AxisElementPosition.Inside;
  }

  protected virtual double LayoutElements()
  {
    int num1 = 1;
    int num2 = 0;
    if (this.Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Hide || this.AngleForAutoRotate == 90.0)
    {
      for (; num1 < this.Children.Count; ++num1)
      {
        if (this.IntersectsWith(this.RectssByRowsAndCols[0][num2], this.RectssByRowsAndCols[0][num1], num2, num1))
        {
          if (num1 == this.Children.Count - 1 && (this.Axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || this.Axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && this.Axis is ChartAxisBase2D && !(this.Axis as ChartAxisBase2D).IsZoomed))
            this.Children[num2].Visibility = Visibility.Collapsed;
          else if (this.Axis is NumericalAxis axis && axis.BreakExistence())
          {
            double previousContent = Convert.ToDouble(axis.VisibleLabels[num2].GetContent());
            double currentContent = Convert.ToDouble(axis.VisibleLabels[num1].GetContent());
            if (axis.BreakRanges.Any<DoubleRange>((Func<DoubleRange, bool>) (item => item.Start == previousContent || item.End == previousContent)))
            {
              if (axis.BreakRanges.Any<DoubleRange>((Func<DoubleRange, bool>) (item => item.Start == currentContent || item.End == currentContent)))
                this.Children[num1 + 1].Visibility = Visibility.Collapsed;
              else
                this.Children[num1].Visibility = Visibility.Collapsed;
            }
            else
              this.Children[num2].Visibility = Visibility.Collapsed;
          }
          else
            this.Children[num1].Visibility = Visibility.Collapsed;
        }
        else
          num2 = num1;
      }
    }
    else if (this.Axis.GetLabelIntersection() == AxisLabelsIntersectAction.MultipleRows || this.Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Wrap)
    {
      int num3 = 1;
      int num4 = 0;
      for (; num3 < this.Children.Count; ++num3)
      {
        if (this.IntersectsWith(this.RectssByRowsAndCols[0][num4], this.RectssByRowsAndCols[0][num3], num4, num3))
        {
          Rect rect = this.RectssByRowsAndCols[0][num3];
          this.RectssByRowsAndCols[0].Remove(num3);
          this.InsertToRowOrColumn(1, num3, rect);
        }
        else
          num4 = num3;
      }
    }
    return 0.0;
  }

  protected virtual void CalculateActualPlotOffset(Size availableSize)
  {
    this.Axis.ActualPlotOffset = this.Axis.PlotOffset < 0.0 ? 0.0 : this.Axis.PlotOffset;
    this.Axis.ActualPlotOffsetStart = this.Axis.PlotOffsetStart < 0.0 ? 0.0 : this.Axis.PlotOffsetStart;
    this.Axis.ActualPlotOffsetEnd = this.Axis.PlotOffsetEnd < 0.0 ? 0.0 : this.Axis.PlotOffsetEnd;
  }

  private static bool DoLinesIntersect(Point point11, Point point12, Point point21, Point point22)
  {
    double num1 = (point22.Y - point21.Y) * (point12.X - point11.X) - (point22.X - point21.X) * (point12.Y - point11.Y);
    double num2 = (point22.X - point21.X) * (point11.Y - point21.Y) - (point22.Y - point21.Y) * (point11.X - point21.X);
    double num3 = (point12.X - point11.X) * (point11.Y - point21.Y) - (point12.Y - point11.Y) * (point11.X - point21.X);
    if (num1 == 0.0)
      return false;
    double num4 = num2 / num1;
    double num5 = num3 / num1;
    return num4 >= 0.0 && num4 <= 1.0 && num5 >= 0.0 && num5 <= 1.0;
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

  private static List<Point> GetRotatedPoints(
    double angle,
    Rect rect,
    double translateX,
    double translateY)
  {
    double num1 = 2.0 * Math.PI * angle / 360.0;
    double m12 = Math.Sin(num1);
    double num2 = Math.Cos(num1);
    Matrix matrix = new Matrix(num2, m12, -m12, num2, translateX, translateY);
    return new List<Point>()
    {
      matrix.Transform(new Point(rect.Left, rect.Top)),
      matrix.Transform(new Point(rect.Right, rect.Top)),
      matrix.Transform(new Point(rect.Right, rect.Bottom)),
      matrix.Transform(new Point(rect.Left, rect.Bottom))
    };
  }

  private static bool IntersectsWith(List<Point> shape1Points, List<Point> shape2Points)
  {
    for (int index1 = 0; index1 < shape1Points.Count; ++index1)
    {
      Point shape1Point1 = shape1Points[index1];
      int index2 = index1 == shape1Points.Count - 1 ? 0 : index1 + 1;
      Point shape1Point2 = shape1Points[index2];
      for (int index3 = 0; index3 < shape2Points.Count; ++index3)
      {
        Point shape2Point1 = shape2Points[index3];
        int index4 = index3 == shape2Points.Count - 1 ? 0 : index3 + 1;
        Point shape2Point2 = shape2Points[index4];
        if (AxisLabelLayout.DoLinesIntersect(shape1Point1, shape1Point2, shape2Point1, shape2Point2))
          return true;
      }
    }
    return false;
  }

  private List<Point> GetRotatedPoints(Rect rect, int index, double angle)
  {
    double num1 = rect.Left + (this.ComputedSizes[index].Width - this.DesiredSizes[index].Width) / 2.0;
    double num2 = rect.Top + (this.ComputedSizes[index].Height - this.DesiredSizes[index].Height) / 2.0;
    double num3 = this.DesiredSizes[index].Width / 2.0;
    double num4 = this.DesiredSizes[index].Height / 2.0;
    rect = new Rect(-num3, -num4, this.DesiredSizes[index].Width, this.DesiredSizes[index].Height);
    double translateX = num1 + num3;
    double translateY = num2 + num4;
    return AxisLabelLayout.GetRotatedPoints(angle, rect, translateX, translateY);
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

  internal void CalculateWrapLabelRect()
  {
    int num1 = 0;
    int num2 = 1;
    List<Dictionary<int, Rect>> rectssByRowsAndCols = this.RectssByRowsAndCols;
    ObservableCollection<ChartAxisLabel> visibleLabels = this.Axis.VisibleLabels;
    for (int count = visibleLabels.Count; num2 < count; ++num2)
    {
      Rect r1 = rectssByRowsAndCols[0][num1];
      Rect r2 = rectssByRowsAndCols[0][num2];
      if (this.IntersectsWith(r1, r2, num1, num2))
      {
        double num3 = r2.Left - r1.Left - (this.Margin.Left + this.Margin.Right);
        bool flag = AxisLabelLayout.LabelContainWrapWidth(visibleLabels[num1].GetContent().ToString(), num3);
        if (flag || flag && num2 == count - 1)
        {
          Size availableSize1 = new Size(num3, double.MaxValue);
          this.Children[num1].Measure(availableSize1);
          this.ComputedSizes[num1] = this.Children[num1].DesiredSize;
          this.RectssByRowsAndCols[0].Remove(num1);
          this.RectssByRowsAndCols[0].Add(num1, new Rect(r1.X, r1.Y, this.ComputedSizes[num1].Width, this.ComputedSizes[num1].Height));
          if (num2 == count - 1)
          {
            double num4 = r2.Left + (r1.Right - r2.Left);
            double num5 = r2.Right - num4 - (this.Margin.Left + this.Margin.Right);
            if (AxisLabelLayout.LabelContainWrapWidth(visibleLabels[num2].GetContent().ToString(), num5))
            {
              Size availableSize2 = new Size(num5, double.MaxValue);
              this.Children[num2].Measure(availableSize2);
              this.ComputedSizes[num2] = this.Children[num2].DesiredSize;
              this.RectssByRowsAndCols[0].Remove(num2);
              this.RectssByRowsAndCols[0].Add(num2, new Rect(r2.X, r2.Y, this.ComputedSizes[num2].Width, this.ComputedSizes[num2].Height));
            }
          }
        }
      }
      num1 = num2;
    }
  }

  private static bool LabelContainWrapWidth(string label, double wrapWidth)
  {
    string str1 = label;
    char[] chArray = new char[1]{ ' ' };
    foreach (string str2 in str1.Split(chArray))
    {
      TextBlock textBlock = new TextBlock() { Text = str2 };
      textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      if (textBlock.DesiredSize.Width > wrapWidth)
        return false;
    }
    return true;
  }
}
