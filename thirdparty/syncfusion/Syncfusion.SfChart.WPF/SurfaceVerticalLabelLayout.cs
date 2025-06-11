// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceVerticalLabelLayout
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

public class SurfaceVerticalLabelLayout(SurfaceAxis axis, List<UIElement> elements) : 
  SurfaceAxisLabelLayout(axis, elements)
{
  protected override double SurfaceLayoutElements()
  {
    base.SurfaceLayoutElements();
    return this.RectssByRowsAndCols.Sum<Dictionary<int, Rect>>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width))));
  }

  protected override void CalcBounds(double availableHeight)
  {
    this.RectssByRowsAndCols = new List<Dictionary<int, Rect>>();
    this.RectssByRowsAndCols.Add(new Dictionary<int, Rect>());
    for (int index = 0; index < this.Children.Count; ++index)
    {
      double y = (1.0 - this.Axis.ValueToCoefficient(this.Axis.VisibleLabels[index].Position)) * availableHeight - this.ComputedSizes[index].Height / 2.0;
      this.RectssByRowsAndCols[0].Add(index, new Rect(new Point(0.0, y), this.ComputedSizes[index]));
    }
    if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
    {
      if (this.RectssByRowsAndCols[0][0].Bottom > availableHeight)
      {
        this.RectssByRowsAndCols[0][0] = new Rect(0.0, availableHeight - this.ComputedSizes[0].Height, this.ComputedSizes[0].Width, this.ComputedSizes[0].Height);
        this.Axis.LeftOffset = 0.0;
      }
      int num = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][num].Top >= 0.0)
        return;
      this.RectssByRowsAndCols[0][num] = new Rect(0.0, 0.0, this.ComputedSizes[num].Width, this.ComputedSizes[num].Height);
      this.Axis.RightOffset = 0.0;
    }
    else if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
    {
      if (this.RectssByRowsAndCols[0][0].Bottom > availableHeight)
      {
        this.RectssByRowsAndCols[0][0] = new Rect(0.0, 0.0, 0.0, 0.0);
        this.Children[0].Visibility = Visibility.Collapsed;
        this.Axis.LeftOffset = 0.0;
      }
      int num = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][num].Top >= 0.0)
        return;
      this.RectssByRowsAndCols[0][num] = new Rect(0.0, 0.0, 0.0, 0.0);
      this.Children[num].Visibility = Visibility.Collapsed;
      this.Axis.RightOffset = 0.0;
    }
    else
    {
      if (this.RectssByRowsAndCols[0][0].Bottom > availableHeight)
        this.Axis.LeftOffset = this.RectssByRowsAndCols[0][0].Bottom - availableHeight;
      int key = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][key].Top >= 0.0)
        return;
      this.Axis.RightOffset = Math.Abs(this.RectssByRowsAndCols[0][key].Top);
    }
  }

  public override Size Measure(Size availableSize)
  {
    if (this.Axis == null || this.Children.Count <= 0)
      return new Size(0.0, availableSize.Height);
    base.Measure(availableSize);
    this.CalcBounds(availableSize.Height);
    return new Size(this.SurfaceLayoutElements() + (this.Margin.Left + this.Margin.Right) * (double) this.RectssByRowsAndCols.Count, availableSize.Height);
  }

  public override void Arrange(Size finalSize)
  {
    if (this.RectssByRowsAndCols == null)
      return;
    bool opposedPosition = this.Axis.OpposedPosition;
    bool isContour = this.Axis.Area.IsContour;
    double num = opposedPosition ? this.Margin.Left : finalSize.Width - this.Margin.Right;
    foreach (Dictionary<int, Rect> rectssByRowsAndCol in this.RectssByRowsAndCols)
    {
      foreach (KeyValuePair<int, Rect> keyValuePair in rectssByRowsAndCol)
      {
        UIElement child = this.Children[keyValuePair.Key];
        double length = opposedPosition ? num : num - this.ComputedSizes[keyValuePair.Key].Width;
        double top = keyValuePair.Value.Top;
        if (isContour)
        {
          length += (this.ComputedSizes[keyValuePair.Key].Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
          top += (this.ComputedSizes[keyValuePair.Key].Height - this.DesiredSizes[keyValuePair.Key].Height) / 2.0;
        }
        Canvas.SetLeft(child, length);
        Canvas.SetTop(child, top);
      }
      if (opposedPosition)
        num += rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)) + this.Margin.Left;
      else
        num -= rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)) + this.Margin.Right;
    }
  }
}
