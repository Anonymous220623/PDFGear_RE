// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.VerticalLabelLayout
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

public class VerticalLabelLayout(ChartAxis axis, List<UIElement> elements) : AxisLabelLayout(axis, elements)
{
  private int currentPos;
  private Border currentBorder;
  private bool isOpposed;
  private double maxWidth;
  private double axisLineThickness;
  private double prevEnd;

  public override Size Measure(Size availableSize)
  {
    if (this.Axis == null || this.Children.Count <= 0)
      return new Size(0.0, availableSize.Height);
    this.AvailableSize = availableSize;
    base.Measure(availableSize);
    this.CalcBounds(availableSize.Height - this.Axis.GetActualPlotOffset());
    ChartAxisBase2D axis = this.Axis as ChartAxisBase2D;
    double val1 = this.LayoutElements();
    if (axis != null)
    {
      if (axis.MultiLevelLabels != null && axis.MultiLevelLabels.Count > 0 || axis.ShowLabelBorder && axis.LabelBorderWidth > 0.0)
        val1 += this.BorderPadding;
      if (axis.MultiLevelLabels != null && axis.MultiLevelLabels.Count == 0)
        val1 = Math.Max(val1, this.Axis.LabelExtent);
    }
    else
      val1 = Math.Max(val1, this.Axis.LabelExtent);
    return new Size(val1 + (this.Margin.Left + this.Margin.Right) * (double) this.RectssByRowsAndCols.Count, availableSize.Height);
  }

  public override void Arrange(Size finalSize)
  {
    if (this.RectssByRowsAndCols == null)
      return;
    this.isOpposed = AxisLabelLayout.IsOpposed(this.Axis, this.Axis.OpposedPosition);
    bool flag1 = !double.IsNaN(this.Axis.LabelRotationAngle) && this.Axis.LabelRotationAngle != 0.0;
    double left = this.isOpposed ? this.Margin.Left : finalSize.Width - this.Margin.Right;
    this.axisLineThickness = (this.Axis.axisElementsPanel as ChartCartesianAxisElementsPanel).MainAxisLine.StrokeThickness;
    if (this.Axis.Area is SfChart3D)
    {
      ChartAxisBase3D axis = this.Axis as ChartAxisBase3D;
      Graphics3D graphics3D = (this.Axis.Area as SfChart3D).Graphics3D;
      foreach (Dictionary<int, Rect> rectssByRowsAndCol in this.RectssByRowsAndCols)
      {
        foreach (KeyValuePair<int, Rect> keyValuePair in rectssByRowsAndCol)
        {
          Size empty = Size.Empty;
          UIElement child = this.Children[keyValuePair.Key];
          Size desiredSize = (child as FrameworkElement).DesiredSize;
          double vz1 = 0.0;
          double num1 = 0.0;
          UIElementLeftShift leftShiftType1 = UIElementLeftShift.Default;
          double num2 = keyValuePair.Value.Top + this.Axis.GetActualPlotOffsetEnd() + this.Axis.ArrangeRect.Top;
          UIElementTopShift topShiftType1 = UIElementTopShift.TopHalfShift;
          double vy = num2 + this.DesiredSizes[keyValuePair.Key].Height / 2.0;
          if (flag1)
            vy += (desiredSize.Height - this.DesiredSizes[keyValuePair.Key].Height) / 2.0;
          if (axis.ShowAxisNextToOrigin)
          {
            SfChart3D area = this.Axis.Area as SfChart3D;
            double actualRotationAngle = area.ActualRotationAngle;
            bool isAxisOpposed = axis.OpposedPosition || actualRotationAngle >= 180.0 && actualRotationAngle < 360.0;
            bool flag2 = AxisLabelLayout.IsOpposed((ChartAxis) axis, isAxisOpposed);
            double num3 = flag2 ? this.Margin.Left : finalSize.Width - this.Margin.Right;
            UIElementLeftShift leftShiftType2 = UIElementLeftShift.LeftHalfShift;
            UIElementTopShift topShiftType2 = UIElementTopShift.TopHalfShift;
            if (actualRotationAngle >= 90.0 && actualRotationAngle < 270.0)
              vz1 = area.ActualDepth;
            double vx = num1 + (flag2 ? num3 + desiredSize.Width / 2.0 : num3 - desiredSize.Width / 2.0) + (this.Axis.ArrangeRect.Left + this.Left);
            if (flag1)
              vx += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
            graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(vx, vy, vz1), child, 10.0, 10.0, true, leftShiftType2, topShiftType2));
          }
          else
          {
            UIElement3D uiElement;
            switch (axis.AxisPosition3D)
            {
              case AxisPosition3D.DepthFrontLeft:
                double vz2;
                if (this.isOpposed)
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  vz2 = vz1 + (axis.AxisDepth + this.Left + this.Margin.Left);
                }
                else
                  vz2 = vz1 + (axis.AxisDepth + finalSize.Width + this.Left - this.Margin.Left);
                double vx1 = num1 + this.Axis.ArrangeRect.Left;
                if (flag1)
                  vz2 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx1, vy, vz2), child, 10.0, 10.0, false, leftShiftType1, topShiftType1);
                break;
              case AxisPosition3D.FrontRight:
                double num4;
                if (axis.GetLabelPosition() == AxisElementPosition.Inside)
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  num4 = num1 + (finalSize.Width - this.Margin.Left);
                }
                else
                  num4 = num1 + this.Margin.Left;
                double vx2 = num4 + (this.Axis.ArrangeRect.Left + this.Left);
                if (flag1)
                  vx2 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx2, vy, axis.AxisDepth), child, 10.0, 10.0, true, leftShiftType1, topShiftType1);
                break;
              case AxisPosition3D.DepthFrontRight:
                UIElementLeftShift leftShiftType3;
                double vz3;
                if (this.isOpposed)
                {
                  leftShiftType3 = UIElementLeftShift.RightHalfShift;
                  vz3 = vz1 + (axis.AxisDepth + this.Margin.Left - desiredSize.Width - finalSize.Width);
                }
                else
                {
                  leftShiftType3 = UIElementLeftShift.LeftShift;
                  vz3 = vz1 + (axis.AxisDepth - this.Left - this.Margin.Left);
                }
                double vx3 = num1 + axis.ArrangeRect.Left;
                if (flag1)
                  vz3 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx3, vy, vz3), child, 10.0, 10.0, false, leftShiftType3, topShiftType1);
                break;
              case AxisPosition3D.BackRight:
                double num5;
                if (this.isOpposed)
                {
                  num5 = num1 + (finalSize.Width - this.Margin.Left);
                }
                else
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  num5 = num1 + this.Margin.Left;
                }
                double vx4 = num5 + (this.Axis.ArrangeRect.Left + this.Left);
                if (flag1)
                  vx4 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx4, vy, axis.AxisDepth), child, 10.0, 10.0, true, leftShiftType1, topShiftType1);
                break;
              case AxisPosition3D.DepthBackRight:
                double vz4;
                if (this.isOpposed)
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  vz4 = vz1 + (axis.AxisDepth + this.Left + finalSize.Width - this.Margin.Left);
                }
                else
                  vz4 = vz1 + (axis.AxisDepth + this.Left + this.Margin.Left);
                double vx5 = num1 + this.Axis.ArrangeRect.Left;
                if (flag1)
                  vz4 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx5, vy, vz4), child, 10.0, 10.0, false, leftShiftType1, topShiftType1);
                break;
              case AxisPosition3D.BackLeft:
                double num6;
                if (this.isOpposed)
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  num6 = num1 + left;
                }
                else
                  num6 = num1 + this.DesiredSizes[keyValuePair.Key].Width + (finalSize.Width - desiredSize.Width - this.Margin.Left);
                double vx6 = num6 + (this.Axis.ArrangeRect.Left + this.Left);
                if (flag1)
                  vx6 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx6, vy, axis.AxisDepth), child, 10.0, 10.0, true, leftShiftType1, topShiftType1);
                break;
              case AxisPosition3D.DepthBackLeft:
                double vz5;
                if (this.isOpposed)
                {
                  vz5 = vz1 + (axis.AxisDepth - this.Left - this.Margin.Left);
                }
                else
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  vz5 = axis.AxisDepth - this.Left - left;
                }
                double vx7 = num1 + axis.ArrangeRect.Left;
                if (flag1)
                  vz5 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx7, vy, vz5), child, 10.0, 10.0, false, leftShiftType1, topShiftType1);
                break;
              default:
                double num7;
                if (this.isOpposed)
                {
                  num7 = num1 + left;
                }
                else
                {
                  leftShiftType1 = UIElementLeftShift.LeftShift;
                  num7 = num1 + left;
                }
                double vx8 = num7 + (this.Axis.ArrangeRect.Left + this.Left);
                if (flag1)
                  vx8 += (desiredSize.Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
                uiElement = Polygon3D.CreateUIElement(new Vector3D(vx8, vy, axis.AxisDepth), child, 10.0, 10.0, true, leftShiftType1, topShiftType1);
                break;
            }
            graphics3D.AddVisual((Polygon3D) uiElement);
          }
        }
        if (this.isOpposed)
          left += rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)) + this.Margin.Left;
        else
          left -= rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)) + this.Margin.Right;
      }
    }
    else
    {
      int row = 0;
      if (this.Axis is ChartAxisBase2D axis && axis.LabelBorderWidth > 0.0)
      {
        this.maxWidth = this.RectssByRowsAndCols.Select<Dictionary<int, Rect>, double>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)))).FirstOrDefault<double>();
        this.maxWidth = this.RectssByRowsAndCols.Count > 1 ? this.maxWidth : this.maxWidth + this.BorderPadding;
      }
      foreach (Dictionary<int, Rect> rectssByRowsAndCol in this.RectssByRowsAndCols)
      {
        foreach (KeyValuePair<int, Rect> keyValuePair in rectssByRowsAndCol)
        {
          UIElement child = this.Children[keyValuePair.Key];
          double length1 = this.isOpposed ? left : left - this.ComputedSizes[keyValuePair.Key].Width;
          if (this.Axis.TickLinesPosition == AxisElementPosition.Inside)
            length1 += this.Axis.OpposedPosition ? this.axisLineThickness : -this.axisLineThickness;
          double length2 = keyValuePair.Value.Top + this.Axis.GetActualPlotOffsetEnd();
          if (flag1)
          {
            length1 += (this.ComputedSizes[keyValuePair.Key].Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
            length2 += (this.ComputedSizes[keyValuePair.Key].Height - this.DesiredSizes[keyValuePair.Key].Height) / 2.0;
          }
          Canvas.SetLeft(child, length1);
          Canvas.SetTop(child, length2);
          if (axis != null && axis.ShowLabelBorder && axis.LabelBorderWidth > 0.0)
          {
            double val1;
            switch (axis)
            {
              case CategoryAxis _:
              case DateTimeCategoryAxis _:
                val1 = 5.0;
                break;
              default:
                val1 = (axis as RangeAxisBase).SmallTickLineSize;
                break;
            }
            double tickSize = Math.Max(val1, axis.TickLineSize);
            this.currentBorder = this.Borders[keyValuePair.Key];
            this.SetBorderThickness(rectssByRowsAndCol, axis);
            this.SetBorderPosition(rectssByRowsAndCol, row, axis, tickSize);
            this.SetBorderLeft(row, left, tickSize);
            ++this.currentPos;
          }
        }
        if (this.isOpposed)
          left += rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)) + this.Margin.Left;
        else
          left -= rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width)) + this.Margin.Right;
        this.currentPos = 0;
        ++row;
      }
      this.prevEnd = 0.0;
    }
  }

  protected override double LayoutElements()
  {
    if (this.Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Wrap)
    {
      this.CalculateWrapLabelRect();
      this.CalcBounds(this.AvailableSize.Height - this.Axis.GetActualPlotOffset());
    }
    base.LayoutElements();
    return this.RectssByRowsAndCols.Sum<Dictionary<int, Rect>>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width))));
  }

  protected override void CalcBounds(double availableHeight)
  {
    this.RectssByRowsAndCols = new List<Dictionary<int, Rect>>();
    this.RectssByRowsAndCols.Add(new Dictionary<int, Rect>());
    for (int index1 = 0; index1 < this.Children.Count; ++index1)
    {
      double y = 0.0;
      if (this.Axis is NumericalAxis axis && axis.BreakExistence())
      {
        for (int index2 = 0; index2 < axis.AxisRanges.Count; ++index2)
        {
          if (axis.AxisRanges[index2].Inside(this.Axis.VisibleLabels[index1].Position))
          {
            y = (1.0 - this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[index1].Position)) * availableHeight - this.ComputedSizes[index1].Height / 2.0;
            foreach (DoubleRange breakRange in axis.BreakRanges)
            {
              if (Math.Round(breakRange.Start, 6) == Convert.ToDouble(this.Axis.VisibleLabels[index1].GetContent()))
                y = axis.IsInversed ? y - this.ComputedSizes[index1].Height / 2.0 : y + this.ComputedSizes[index1].Height / 2.0;
              else if (Math.Round(breakRange.End, 6) == Convert.ToDouble(this.Axis.VisibleLabels[index1].GetContent()))
                y = axis.IsInversed ? y + this.ComputedSizes[index1].Height / 2.0 : y - this.ComputedSizes[index1].Height / 2.0;
            }
          }
        }
      }
      else
        y = (1.0 - this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[index1].Position)) * availableHeight - this.ComputedSizes[index1].Height / 2.0;
      LabelAlignment axisLabelAlignment = this.Axis.AxisLabelAlignment;
      if (this.Axis.VisibleLabels[index1].LabelStyle != null)
        axisLabelAlignment = this.Axis.VisibleLabels[index1].AxisLabelAlignment;
      switch (axisLabelAlignment)
      {
        case LabelAlignment.Far:
          y += this.ComputedSizes[index1].Height / 2.0;
          break;
        case LabelAlignment.Near:
          y -= this.ComputedSizes[index1].Height / 2.0;
          break;
      }
      this.RectssByRowsAndCols[0].Add(index1, new Rect(new Point(0.0, y), this.ComputedSizes[index1]));
    }
    if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
    {
      if (this.RectssByRowsAndCols[0][0].Bottom > availableHeight)
        this.RectssByRowsAndCols[0][0] = new Rect(0.0, availableHeight - this.ComputedSizes[0].Height, this.ComputedSizes[0].Width, this.ComputedSizes[0].Height);
      int num = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][num].Top >= 0.0)
        return;
      this.RectssByRowsAndCols[0][num] = new Rect(0.0, 0.0, this.ComputedSizes[num].Width, this.ComputedSizes[num].Height);
    }
    else
    {
      if (this.Axis.EdgeLabelsDrawingMode != EdgeLabelsDrawingMode.Hide)
        return;
      if (this.RectssByRowsAndCols[0][0].Bottom > availableHeight)
      {
        this.RectssByRowsAndCols[0][0] = new Rect(0.0, 0.0, 0.0, 0.0);
        this.Children[0].Visibility = Visibility.Collapsed;
      }
      int num = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][num].Top >= 0.0)
        return;
      this.RectssByRowsAndCols[0][num] = new Rect(0.0, 0.0, 0.0, 0.0);
      this.Children[num].Visibility = Visibility.Collapsed;
    }
  }

  protected override void CalculateActualPlotOffset(Size availableSize)
  {
    if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit)
    {
      double num1 = (1.0 - this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[0].Position)) * availableSize.Height - this.ComputedSizes[0].Height / 2.0;
      double num2 = 0.0;
      double num3 = 0.0;
      if (num1 + this.ComputedSizes[0].Height / 2.0 - this.Axis.PlotOffset > availableSize.Height)
        num2 = this.ComputedSizes[0].Height;
      int index = this.Children.Count - 1;
      if ((1.0 - this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[index].Position)) * availableSize.Height - this.ComputedSizes[index].Height / 2.0 - this.ComputedSizes[index].Height / 2.0 + this.Axis.PlotOffset > availableSize.Height)
        num3 = this.ComputedSizes[index].Height;
      this.Axis.ActualPlotOffset = Math.Max(Math.Max(num2 / 2.0, num3 / 2.0), this.Axis.PlotOffset);
    }
    else
      base.CalculateActualPlotOffset(availableSize);
  }

  private void SetBorderThickness(Dictionary<int, Rect> dictionary, ChartAxisBase2D axis)
  {
    bool isSidebySideSeries = this.CheckCartesianSeries();
    double labelBorderWidth = axis.LabelBorderWidth;
    if (this.CheckLabelPlacement(isSidebySideSeries))
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(0.0, labelBorderWidth, labelBorderWidth, labelBorderWidth) : new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, 0.0, labelBorderWidth);
    else if (this.currentPos == 0)
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(0.0, labelBorderWidth, labelBorderWidth, 0.0) : new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, 0.0, 0.0);
    else if (this.currentPos + 1 < dictionary.Count)
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(0.0, labelBorderWidth, labelBorderWidth, labelBorderWidth) : new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, 0.0, labelBorderWidth);
    else
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(0.0, 0.0, labelBorderWidth, labelBorderWidth) : new Thickness().GetThickness(labelBorderWidth, 0.0, 0.0, labelBorderWidth);
  }

  private double CalculatePoint(double value)
  {
    return this.Axis.GetActualPlotOffsetEnd() + Math.Round(this.Axis.RenderedRect.Height * (1.0 - this.Axis.ValueToCoefficient(value)));
  }

  private void SetBorderPosition(
    Dictionary<int, Rect> dictionary,
    int row,
    ChartAxisBase2D axis,
    double tickSize)
  {
    if (this.currentPos == 0)
      this.prevEnd = this.Axis.IsInversed ? this.CalculatePoint(this.Axis.VisibleRange.End) : (this.prevEnd = this.CalculatePoint(this.Axis.VisibleRange.Start));
    if (this.currentPos + 1 < dictionary.Count)
    {
      double point = this.CalculatePoint((this.Axis.VisibleLabels[dictionary.ElementAt<KeyValuePair<int, Rect>>(this.currentPos).Key].Position + this.Axis.VisibleLabels[dictionary.ElementAt<KeyValuePair<int, Rect>>(this.currentPos + 1).Key].Position) / 2.0);
      this.currentBorder.Height = this.prevEnd - point;
      this.currentBorder.Height += axis.LabelBorderWidth;
      Canvas.SetTop((UIElement) this.currentBorder, point - axis.LabelBorderWidth / 2.0);
      this.prevEnd = point;
    }
    else
    {
      double num = this.Axis.IsInversed ? this.CalculatePoint(this.Axis.VisibleRange.Start) : this.CalculatePoint(this.Axis.VisibleRange.End);
      this.currentBorder.Height = this.prevEnd - num;
      this.currentBorder.Height += axis.LabelBorderWidth;
      Canvas.SetTop((UIElement) this.currentBorder, num - axis.LabelBorderWidth / 2.0);
    }
    if (this.Axis.GetLabelPosition() == this.Axis.TickLinesPosition)
      this.currentBorder.Width = row == 0 ? this.maxWidth + tickSize + this.Margin.Left + this.Margin.Right : this.maxWidth + this.Margin.Left + this.Margin.Right;
    else
      this.currentBorder.Width = this.maxWidth + this.Margin.Left + this.Margin.Right;
  }

  private void SetBorderLeft(int row, double left, double tickSize)
  {
    Canvas.SetLeft((UIElement) this.currentBorder, this.RectssByRowsAndCols.Count <= 1 || row <= 0 ? (!this.isOpposed ? (this.Axis.GetLabelPosition() != this.Axis.TickLinesPosition ? left - this.currentBorder.Width + this.Margin.Right : left - this.currentBorder.Width + tickSize + this.Margin.Right) : (this.Axis.GetLabelPosition() != this.Axis.TickLinesPosition ? left - this.Margin.Right : left - (tickSize + this.Margin.Right))) : (!this.isOpposed ? left - this.currentBorder.Width : left));
  }
}
