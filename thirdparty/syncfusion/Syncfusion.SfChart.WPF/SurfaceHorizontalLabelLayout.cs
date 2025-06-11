// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceHorizontalLabelLayout
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SurfaceHorizontalLabelLayout(SurfaceAxis axis, List<UIElement> elements) : 
  SurfaceAxisLabelLayout(axis, elements)
{
  protected override double SurfaceLayoutElements()
  {
    base.SurfaceLayoutElements();
    return this.RectssByRowsAndCols.Sum<Dictionary<int, Rect>>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height))));
  }

  protected override void CalcBounds(double availableWidth)
  {
    this.RectssByRowsAndCols = new List<Dictionary<int, Rect>>();
    this.RectssByRowsAndCols.Add(new Dictionary<int, Rect>());
    if (this.Axis == null)
      return;
    for (int index = 0; index < this.Children.Count; ++index)
    {
      double x = this.Axis.ValueToCoefficient(this.Axis.VisibleLabels[index].Position) * availableWidth - this.ComputedSizes[index].Width / 2.0;
      this.RectssByRowsAndCols[0].Add(index, new Rect(new Point(x, 0.0), this.ComputedSizes[index]));
    }
    int num1 = this.Children.Count - 1;
    int num2 = this.Axis.IsInversed ? num1 : 0;
    int num3 = this.Axis.IsInversed ? 0 : num1;
    if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
    {
      if (this.RectssByRowsAndCols[0][num2].Left < 0.0)
      {
        this.RectssByRowsAndCols[0][num2] = new Rect(0.0, 0.0, this.ComputedSizes[num2].Width, this.ComputedSizes[num2].Height);
        this.Axis.LeftOffset = 0.0;
      }
      if (this.RectssByRowsAndCols[0][num3].Right <= availableWidth)
        return;
      double x = availableWidth - this.ComputedSizes[num3].Width;
      this.RectssByRowsAndCols[0][num3] = new Rect(x, 0.0, this.ComputedSizes[num3].Width, this.ComputedSizes[num3].Height);
      this.Axis.RightOffset = 0.0;
    }
    else if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
    {
      if (this.RectssByRowsAndCols[0][num2].Left < 0.0)
      {
        this.RectssByRowsAndCols[0][num2] = new Rect(0.0, 0.0, 0.0, 0.0);
        this.Children[num2].Visibility = Visibility.Collapsed;
        this.Axis.LeftOffset = 0.0;
      }
      if (this.RectssByRowsAndCols[0][num3].Right <= availableWidth)
        return;
      this.RectssByRowsAndCols[0][num3] = new Rect(0.0, 0.0, 0.0, 0.0);
      this.Children[num3].Visibility = Visibility.Collapsed;
      this.Axis.RightOffset = 0.0;
    }
    else
    {
      if (this.RectssByRowsAndCols[0][num2].Left < 0.0)
        this.Axis.LeftOffset = Math.Abs(this.RectssByRowsAndCols[0][num2].Left);
      if (this.RectssByRowsAndCols[0][num3].Right <= availableWidth)
        return;
      this.Axis.RightOffset = this.RectssByRowsAndCols[0][num3].Right - availableWidth;
    }
  }

  public override Size Measure(Size availableSize)
  {
    if (this.Axis == null || this.Children.Count <= 0)
      return new Size(availableSize.Width, 0.0);
    base.Measure(availableSize);
    this.CalcBounds(availableSize.Width);
    double height = this.SurfaceLayoutElements() + (this.Margin.Top + this.Margin.Bottom) * (double) this.RectssByRowsAndCols.Count;
    return new Size(availableSize.Width, height);
  }

  public override void Arrange(Size finalSize)
  {
    if (this.RectssByRowsAndCols == null)
      return;
    bool opposedPosition = this.Axis.OpposedPosition;
    bool isContour = this.Axis.Area.IsContour;
    double num = opposedPosition ? finalSize.Height - this.Margin.Bottom : this.Margin.Top;
    foreach (Dictionary<int, Rect> rectssByRowsAndCol in this.RectssByRowsAndCols)
    {
      foreach (KeyValuePair<int, Rect> keyValuePair in rectssByRowsAndCol)
      {
        UIElement child = this.Children[keyValuePair.Key];
        double length = opposedPosition ? num - this.ComputedSizes[keyValuePair.Key].Height : num;
        double left = keyValuePair.Value.Left;
        if (isContour)
        {
          length += (this.ComputedSizes[keyValuePair.Key].Height - this.DesiredSizes[keyValuePair.Key].Height) / 2.0;
          left += (this.ComputedSizes[keyValuePair.Key].Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
        }
        Canvas.SetLeft(child, left);
        Canvas.SetTop(child, length);
      }
      if (opposedPosition)
        num -= rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)) + this.Margin.Bottom;
      else
        num += rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)) + this.Margin.Top;
    }
  }
}
