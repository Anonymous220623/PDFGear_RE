// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HorizontalLabelLayout
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

public class HorizontalLabelLayout(ChartAxis axis, List<UIElement> elements) : AxisLabelLayout(axis, elements)
{
  private int currentPos;
  private Border currentBorder;
  private bool isOpposed;
  private double maxHeight;
  private double axisLineThickness;
  private double prevEnd;

  public override Size Measure(Size availableSize)
  {
    if (this.Axis == null || this.Children.Count <= 0)
      return new Size(availableSize.Width, 0.0);
    this.AvailableSize = availableSize;
    base.Measure(availableSize);
    this.CalcBounds(availableSize.Width - this.Axis.GetActualPlotOffset());
    if (this.Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Auto && this.Axis.LabelRotationAngle == 0.0 && this.AngleForAutoRotate != 90.0)
    {
      int num1 = 0;
      double num2 = 0.0;
      for (int index = 1; index < this.Children.Count; ++index)
      {
        if (this.IntersectsWith(this.RectssByRowsAndCols[0][num1], this.RectssByRowsAndCols[0][index], num1, index))
          num2 = this.AngleForAutoRotate == 45.0 ? 90.0 : 45.0;
        else
          num1 = index;
      }
      if (num2 != 0.0)
      {
        this.AngleForAutoRotate = num2;
        this.Measure(availableSize);
      }
    }
    ChartAxisBase2D axis = this.Axis as ChartAxisBase2D;
    double val1 = this.LayoutElements();
    if (axis != null)
    {
      if (axis.ShowLabelBorder && axis.LabelBorderWidth > 0.0 || axis.MultiLevelLabels != null && axis.MultiLevelLabels.Count > 0)
        val1 += this.BorderPadding;
      if (axis.MultiLevelLabels != null && axis.MultiLevelLabels.Count == 0)
        val1 = Math.Max(val1, this.Axis.LabelExtent);
    }
    else
      val1 = Math.Max(val1, this.Axis.LabelExtent);
    double height = val1 + (this.Margin.Top + this.Margin.Bottom) * (double) this.RectssByRowsAndCols.Count;
    return new Size(availableSize.Width, height);
  }

  public override void Arrange(Size finalSize)
  {
    if (this.RectssByRowsAndCols == null)
      return;
    this.isOpposed = false;
    bool flag1 = !double.IsNaN(this.Axis.LabelRotationAngle) && this.Axis.LabelRotationAngle != 0.0 || this.AngleForAutoRotate != 0.0;
    this.axisLineThickness = (this.Axis.axisElementsPanel as ChartCartesianAxisElementsPanel).MainAxisLine.StrokeThickness;
    if (this.Axis.Area is SfChart3D)
    {
      this.isOpposed = AxisLabelLayout.IsOpposed(this.Axis, this.Axis.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x.Orientation == Orientation.Horizontal && x.OpposedPosition)).Any<ChartAxis>());
      double num1 = !this.isOpposed || this.Axis.GetLabelPosition() != AxisElementPosition.Outside ? this.Axis.ArrangeRect.Top + this.Top : this.Axis.ArrangeRect.Bottom;
      double left = this.Axis.ArrangeRect.Left;
      Graphics3D graphics3D = (this.Axis.Area as SfChart3D).Graphics3D;
      foreach (Dictionary<int, Rect> rectssByRowsAndCol in this.RectssByRowsAndCols)
      {
        foreach (KeyValuePair<int, Rect> keyValuePair in rectssByRowsAndCol)
        {
          ChartAxisBase3D axis = this.Axis as ChartAxisBase3D;
          UIElement child = this.Children[keyValuePair.Key];
          double vy = this.isOpposed ? num1 - keyValuePair.Value.Bottom - this.DesiredSizes[keyValuePair.Key].Height : num1;
          double num2 = axis.IsManhattanAxis ? keyValuePair.Value.Left : keyValuePair.Value.Left + this.Axis.GetActualPlotOffsetStart();
          SfChart3D area = this.Axis.Area as SfChart3D;
          double actualRotationAngle = area.ActualRotationAngle;
          double actualTiltAngle = area.ActualTiltAngle;
          double axisDepth = axis.AxisDepth;
          double num3 = num2 + child.DesiredSize.Width / 2.0;
          if (flag1)
          {
            vy += (this.ComputedSizes[keyValuePair.Key].Height - this.DesiredSizes[keyValuePair.Key].Height) / 2.0;
            num3 += (this.ComputedSizes[keyValuePair.Key].Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
          }
          UIElementLeftShift leftElementShift = UIElementLeftShift.Default;
          UIElementTopShift topElementShift = UIElementTopShift.Default;
          this.GetLeftandShift(ref leftElementShift, ref topElementShift, this.isOpposed, actualRotationAngle, actualTiltAngle);
          double num4 = 0.0;
          bool flag2 = this.Axis.Area.Axes.Any<ChartAxis>((Func<ChartAxis, bool>) (x => x.Orientation == Orientation.Vertical && x.OpposedPosition));
          if (axis.IsZAxis)
          {
            if (flag2)
              num4 -= axis.TickLineSize / 2.0;
            else if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 180.0)
              num4 += axis.TickLineSize / 2.0;
            else if (actualRotationAngle >= 180.0 && actualRotationAngle < 225.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
              num4 -= axis.TickLineSize / 2.0;
          }
          else
          {
            num3 += left;
            if (flag2)
              axisDepth -= axis.TickLineSize / 2.0;
            else if (actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
            {
              if (actualRotationAngle >= 45.0 && actualRotationAngle < 90.0 || actualRotationAngle >= 270.0 && actualRotationAngle < 315.0)
                axisDepth -= axis.TickLineSize / 2.0;
              else if (actualRotationAngle >= 90.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 270.0)
                axisDepth += axis.TickLineSize / 2.0;
            }
          }
          if (axis.IsZAxis)
            graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(left + num4, vy, num3), child, 10.0, 10.0, false, leftElementShift, topElementShift));
          else
            graphics3D.AddVisual((Polygon3D) Polygon3D.CreateUIElement(new Vector3D(num3, vy, axisDepth), child, 10.0, 10.0, true, leftElementShift, topElementShift));
        }
        if (this.isOpposed)
          num1 -= rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)) + this.Margin.Bottom;
        else
          num1 += rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)) + this.Margin.Top;
      }
    }
    else
    {
      int row = 0;
      ChartAxisBase2D axis = this.Axis as ChartAxisBase2D;
      this.isOpposed = AxisLabelLayout.IsOpposed(this.Axis, this.Axis.OpposedPosition);
      double top = this.isOpposed ? finalSize.Height - this.Margin.Bottom : this.Margin.Top;
      if (axis != null && axis.LabelBorderWidth > 0.0)
      {
        this.maxHeight = this.RectssByRowsAndCols.Select<Dictionary<int, Rect>, double>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)))).FirstOrDefault<double>();
        this.maxHeight = this.RectssByRowsAndCols.Count > 1 ? this.maxHeight + this.Margin.Top : this.maxHeight + this.BorderPadding;
      }
      foreach (Dictionary<int, Rect> rectssByRowsAndCol in this.RectssByRowsAndCols)
      {
        foreach (KeyValuePair<int, Rect> keyValuePair in rectssByRowsAndCol)
        {
          UIElement child = this.Children[keyValuePair.Key];
          double length1 = this.isOpposed ? top - this.ComputedSizes[keyValuePair.Key].Height : top;
          if (this.Axis.TickLinesPosition == AxisElementPosition.Inside)
            length1 += this.Axis.OpposedPosition ? -this.axisLineThickness : this.axisLineThickness;
          double length2 = keyValuePair.Value.Left + this.Axis.GetActualPlotOffsetStart();
          if (flag1)
          {
            length1 += (this.ComputedSizes[keyValuePair.Key].Height - this.DesiredSizes[keyValuePair.Key].Height) / 2.0;
            length2 += (this.ComputedSizes[keyValuePair.Key].Width - this.DesiredSizes[keyValuePair.Key].Width) / 2.0;
          }
          Canvas.SetLeft(child, length2);
          Canvas.SetTop(child, length1);
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
            this.SetBorderTop(row, top, tickSize);
            ++this.currentPos;
          }
        }
        if (this.isOpposed)
          top -= rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)) + this.Margin.Bottom;
        else
          top += rectssByRowsAndCol.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height)) + this.Margin.Top;
        this.currentPos = 0;
        ++row;
      }
      this.prevEnd = 0.0;
    }
  }

  protected override void CalculateActualPlotOffset(Size availableSize)
  {
    if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit)
    {
      double num1 = this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[0].Position) * availableSize.Width - this.ComputedSizes[0].Width / 2.0;
      double num2 = 0.0;
      double num3 = 0.0;
      if (num1 - this.ComputedSizes[0].Width / 2.0 + this.Axis.PlotOffset < 0.0)
        num2 = this.ComputedSizes[0].Width;
      int index = this.Children.Count - 1;
      if (num1 + this.ComputedSizes[index].Width / 2.0 - this.Axis.PlotOffset < availableSize.Width)
        num3 = this.ComputedSizes[index].Width;
      this.Axis.ActualPlotOffset = Math.Max(Math.Max(num2 / 2.0, num3 / 2.0), this.Axis.PlotOffset);
    }
    else
      base.CalculateActualPlotOffset(availableSize);
  }

  protected override double LayoutElements()
  {
    if (this.Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Wrap)
    {
      this.CalculateWrapLabelRect();
      this.CalcBounds(this.AvailableSize.Width - this.Axis.GetActualPlotOffset());
    }
    base.LayoutElements();
    return this.RectssByRowsAndCols.Sum<Dictionary<int, Rect>>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Height))));
  }

  protected override void CalcBounds(double availableWidth)
  {
    this.RectssByRowsAndCols = new List<Dictionary<int, Rect>>();
    this.RectssByRowsAndCols.Add(new Dictionary<int, Rect>());
    for (int index1 = 0; index1 < this.Children.Count; ++index1)
    {
      double x = 0.0;
      ChartAxisBase3D axis1 = this.Axis as ChartAxisBase3D;
      NumericalAxis axis2 = this.Axis as NumericalAxis;
      if (axis1 != null && axis1.IsManhattanAxis && (axis1.RegisteredSeries[index1] as ChartSeries3D).Segments != null && (axis1.RegisteredSeries[index1] as ChartSeries3D).Segments.Count > 0)
      {
        ChartSegment3D segment = (axis1.RegisteredSeries[index1] as ChartSeries3D).Segments[0] as ChartSegment3D;
        x = segment.startDepth + (segment.endDepth - segment.startDepth) / 2.0 - this.ComputedSizes[index1].Width / 2.0;
      }
      else if (axis2 != null && axis2.BreakExistence())
      {
        for (int index2 = 0; index2 < axis2.AxisRanges.Count; ++index2)
        {
          if (axis2.AxisRanges[index2].Inside(this.Axis.VisibleLabels[index1].Position))
          {
            x = this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[index1].Position) * availableWidth - this.ComputedSizes[index1].Width / 2.0;
            foreach (DoubleRange breakRange in axis2.BreakRanges)
            {
              if (Math.Round(breakRange.Start, 6) == Convert.ToDouble(this.Axis.VisibleLabels[index1].GetContent()))
                x = axis2.IsInversed ? x + this.ComputedSizes[index1].Width / 2.0 : x - this.ComputedSizes[index1].Width / 2.0;
              else if (Math.Round(breakRange.End, 6) == Convert.ToDouble(this.Axis.VisibleLabels[index1].GetContent()))
                x = axis2.IsInversed ? x - this.ComputedSizes[index1].Width / 2.0 : x + this.ComputedSizes[index1].Width / 2.0;
            }
          }
        }
      }
      else
        x = this.Axis.ValueToCoefficientCalc(this.Axis.VisibleLabels[index1].Position) * availableWidth - this.ComputedSizes[index1].Width / 2.0;
      LabelAlignment axisLabelAlignment = this.Axis.AxisLabelAlignment;
      if (this.Axis.VisibleLabels[index1].LabelStyle != null)
        axisLabelAlignment = this.Axis.VisibleLabels[index1].AxisLabelAlignment;
      switch (axisLabelAlignment)
      {
        case LabelAlignment.Far:
          x += this.ComputedSizes[index1].Width / 2.0;
          break;
        case LabelAlignment.Near:
          x -= this.ComputedSizes[index1].Width / 2.0;
          break;
      }
      this.RectssByRowsAndCols[0].Add(index1, new Rect(new Point(x, 0.0), this.ComputedSizes[index1]));
    }
    if (this.Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
    {
      if (this.RectssByRowsAndCols[0][0].Left < 0.0)
        this.RectssByRowsAndCols[0][0] = new Rect(0.0, 0.0, this.ComputedSizes[0].Width, this.ComputedSizes[0].Height);
      int num = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][num].Right <= availableWidth)
        return;
      double x = availableWidth - this.ComputedSizes[this.Children.Count - 1].Width;
      this.RectssByRowsAndCols[0][num] = new Rect(x, 0.0, this.ComputedSizes[num].Width, this.ComputedSizes[num].Height);
    }
    else
    {
      if (this.Axis.EdgeLabelsDrawingMode != EdgeLabelsDrawingMode.Hide)
        return;
      if (this.RectssByRowsAndCols[0][0].Left < 0.0)
      {
        this.RectssByRowsAndCols[0][0] = new Rect(0.0, 0.0, 0.0, 0.0);
        this.Children[0].Visibility = Visibility.Collapsed;
      }
      int num = this.Children.Count - 1;
      if (this.RectssByRowsAndCols[0][num].Right <= availableWidth)
        return;
      this.RectssByRowsAndCols[0][num] = new Rect(0.0, 0.0, 0.0, 0.0);
      this.Children[num].Visibility = Visibility.Collapsed;
    }
  }

  private static void PositionLabelsBack(
    bool isOpposed,
    ref UIElementLeftShift leftElementShift,
    ref UIElementTopShift topElementShift,
    double actualTiltAngle)
  {
    leftElementShift = UIElementLeftShift.LeftHalfShift;
    if (isOpposed)
    {
      if (actualTiltAngle < 45.0 || actualTiltAngle >= 315.0)
        return;
      topElementShift = UIElementTopShift.Default;
    }
    else
    {
      if (actualTiltAngle < 45.0 || actualTiltAngle >= 315.0)
        return;
      topElementShift = UIElementTopShift.TopShift;
    }
  }

  private static void PositionLabelsRight(
    ref UIElementLeftShift leftElementShift,
    ref UIElementTopShift topElementShift,
    double actualTiltAngle)
  {
    leftElementShift = UIElementLeftShift.Default;
    if (actualTiltAngle < 45.0 || actualTiltAngle >= 315.0)
      return;
    topElementShift = UIElementTopShift.TopHalfShift;
  }

  private static void PositionLabelsLeft(
    ref UIElementLeftShift leftElementShift,
    ref UIElementTopShift topElementShift,
    double actualTiltAngle)
  {
    leftElementShift = UIElementLeftShift.LeftShift;
    if (actualTiltAngle < 45.0 || actualTiltAngle >= 315.0)
      return;
    topElementShift = UIElementTopShift.TopHalfShift;
  }

  private static void PositionLabelsFront(
    bool isOpposed,
    ref UIElementLeftShift leftElementShift,
    ref UIElementTopShift topElementShift,
    double actualTiltAngle)
  {
    leftElementShift = UIElementLeftShift.LeftHalfShift;
    if (!isOpposed || actualTiltAngle < 45.0 || actualTiltAngle >= 315.0)
      return;
    topElementShift = UIElementTopShift.TopShift;
  }

  private double CalculatePoint(double value)
  {
    return this.Axis.GetActualPlotOffsetStart() + Math.Round(this.Axis.RenderedRect.Width * this.Axis.ValueToCoefficient(value));
  }

  private void SetBorderPosition(
    Dictionary<int, Rect> dictionary,
    int row,
    ChartAxisBase2D axis,
    double tickSize)
  {
    if (this.currentPos == 0)
      this.prevEnd = this.Axis.IsInversed ? this.CalculatePoint(this.Axis.VisibleRange.End) : this.CalculatePoint(this.Axis.VisibleRange.Start);
    if (this.currentPos + 1 < dictionary.Count)
    {
      double point = this.CalculatePoint((this.Axis.VisibleLabels[dictionary.ElementAt<KeyValuePair<int, Rect>>(this.currentPos).Key].Position + this.Axis.VisibleLabels[dictionary.ElementAt<KeyValuePair<int, Rect>>(this.currentPos + 1).Key].Position) / 2.0);
      this.currentBorder.Width = point - this.prevEnd;
      this.currentBorder.Width += axis.LabelBorderWidth;
      Canvas.SetLeft((UIElement) this.currentBorder, this.prevEnd - axis.LabelBorderWidth / 2.0);
      this.prevEnd = point;
    }
    else
    {
      this.currentBorder.Width = this.Axis.IsInversed ? this.CalculatePoint(this.Axis.VisibleRange.Start) : this.CalculatePoint(this.Axis.VisibleRange.End);
      this.currentBorder.Width -= this.prevEnd;
      this.currentBorder.Width += axis.LabelBorderWidth;
      Canvas.SetLeft((UIElement) this.currentBorder, this.prevEnd - axis.LabelBorderWidth / 2.0);
    }
    if (this.Axis.GetLabelPosition() == this.Axis.TickLinesPosition)
      this.currentBorder.Height = row == 0 ? this.maxHeight + tickSize + this.Margin.Top + this.Margin.Bottom : this.maxHeight + this.Margin.Top + this.Margin.Bottom;
    else
      this.currentBorder.Height = this.maxHeight + this.Margin.Top + this.Margin.Bottom;
  }

  private void SetBorderThickness(Dictionary<int, Rect> dictionary, ChartAxisBase2D axis)
  {
    bool isSidebySideSeries = this.CheckCartesianSeries();
    double labelBorderWidth = axis.LabelBorderWidth;
    if (this.CheckLabelPlacement(isSidebySideSeries))
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, labelBorderWidth, 0.0) : new Thickness().GetThickness(labelBorderWidth, 0.0, labelBorderWidth, labelBorderWidth);
    else if (this.currentPos == 0)
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(0.0, labelBorderWidth, labelBorderWidth, 0.0) : new Thickness().GetThickness(0.0, 0.0, labelBorderWidth, labelBorderWidth);
    else if (this.currentPos + 1 < dictionary.Count)
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, labelBorderWidth, 0.0) : new Thickness().GetThickness(labelBorderWidth, 0.0, labelBorderWidth, labelBorderWidth);
    else
      this.currentBorder.BorderThickness = this.isOpposed ? new Thickness().GetThickness(labelBorderWidth, labelBorderWidth, 0.0, 0.0) : new Thickness().GetThickness(labelBorderWidth, 0.0, 0.0, labelBorderWidth);
  }

  private void SetBorderTop(int row, double top, double tickSize)
  {
    Canvas.SetTop((UIElement) this.currentBorder, this.RectssByRowsAndCols.Count <= 1 || row <= 0 ? (!this.isOpposed ? (this.Axis.GetLabelPosition() != this.Axis.TickLinesPosition ? top - this.Margin.Top : top - (tickSize + this.Margin.Top)) : (this.Axis.GetLabelPosition() != this.Axis.TickLinesPosition ? top - this.currentBorder.Height + this.Margin.Bottom : top - this.currentBorder.Height + this.Margin.Bottom + tickSize)) : (!this.isOpposed ? top : top - this.currentBorder.Height));
  }

  private void GetLeftandShift(
    ref UIElementLeftShift leftElementShift,
    ref UIElementTopShift topElementShift,
    bool isOpposed,
    double actualRotationAngle,
    double actualTiltAngle)
  {
    bool flag = this.Axis.Area.Axes.Any<ChartAxis>((Func<ChartAxis, bool>) (x => x.Orientation == Orientation.Vertical && x.OpposedPosition));
    if ((this.Axis as ChartAxisBase3D).IsZAxis)
    {
      if (flag)
      {
        if (actualRotationAngle < 45.0 || actualRotationAngle >= 315.0)
          HorizontalLabelLayout.PositionLabelsLeft(ref leftElementShift, ref topElementShift, actualTiltAngle);
        else if (actualRotationAngle >= 45.0 && actualRotationAngle < 135.0)
          HorizontalLabelLayout.PositionLabelsBack(isOpposed, ref leftElementShift, ref topElementShift, actualTiltAngle);
        else if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
        {
          HorizontalLabelLayout.PositionLabelsRight(ref leftElementShift, ref topElementShift, actualTiltAngle);
        }
        else
        {
          if (actualRotationAngle < 225.0 || actualRotationAngle >= 315.0)
            return;
          HorizontalLabelLayout.PositionLabelsFront(isOpposed, ref leftElementShift, ref topElementShift, actualTiltAngle);
        }
      }
      else if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 180.0 && actualRotationAngle < 225.0)
        HorizontalLabelLayout.PositionLabelsRight(ref leftElementShift, ref topElementShift, actualTiltAngle);
      else if (actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 315.0)
      {
        HorizontalLabelLayout.PositionLabelsFront(isOpposed, ref leftElementShift, ref topElementShift, actualTiltAngle);
      }
      else
      {
        if ((actualRotationAngle < 135.0 || actualRotationAngle >= 180.0) && (actualRotationAngle < 315.0 || actualRotationAngle >= 360.0))
          return;
        HorizontalLabelLayout.PositionLabelsLeft(ref leftElementShift, ref topElementShift, actualTiltAngle);
      }
    }
    else if (flag)
    {
      if (actualRotationAngle < 45.0 || actualRotationAngle >= 315.0)
        HorizontalLabelLayout.PositionLabelsFront(isOpposed, ref leftElementShift, ref topElementShift, actualTiltAngle);
      else if (actualRotationAngle >= 45.0 && actualRotationAngle < 135.0)
        HorizontalLabelLayout.PositionLabelsLeft(ref leftElementShift, ref topElementShift, actualTiltAngle);
      else if (actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
      {
        HorizontalLabelLayout.PositionLabelsBack(isOpposed, ref leftElementShift, ref topElementShift, actualTiltAngle);
      }
      else
      {
        if (actualRotationAngle < 225.0 || actualRotationAngle >= 315.0)
          return;
        HorizontalLabelLayout.PositionLabelsRight(ref leftElementShift, ref topElementShift, actualTiltAngle);
      }
    }
    else if (actualRotationAngle < 45.0 || actualRotationAngle >= 315.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
      HorizontalLabelLayout.PositionLabelsFront(isOpposed, ref leftElementShift, ref topElementShift, actualTiltAngle);
    else if (actualRotationAngle >= 45.0 && actualRotationAngle < 90.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 270.0)
    {
      HorizontalLabelLayout.PositionLabelsLeft(ref leftElementShift, ref topElementShift, actualTiltAngle);
    }
    else
    {
      if ((actualRotationAngle < 90.0 || actualRotationAngle >= 135.0) && (actualRotationAngle < 270.0 || actualRotationAngle >= 315.0))
        return;
      HorizontalLabelLayout.PositionLabelsRight(ref leftElementShift, ref topElementShift, actualTiltAngle);
    }
  }
}
