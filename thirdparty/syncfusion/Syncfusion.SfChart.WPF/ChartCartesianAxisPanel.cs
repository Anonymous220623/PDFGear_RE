// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCartesianAxisPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCartesianAxisPanel : Canvas
{
  public ChartCartesianAxisPanel() => this.LayoutCalc = new List<ILayoutCalculator>();

  internal ChartAxisBase2D Axis { get; set; }

  internal List<ILayoutCalculator> LayoutCalc { get; set; }

  internal Size ComputeSize(Size availableSize)
  {
    Size size = Size.Empty;
    if (this.Axis.AxisLayoutPanel is ChartPolarAxisLayoutPanel)
    {
      foreach (UIElement child in this.Children)
        child.Measure(availableSize);
      this.Children[0].Visibility = Visibility.Collapsed;
      foreach (ILayoutCalculator layoutCalculator in this.LayoutCalc)
      {
        layoutCalculator.Measure(availableSize);
        size = layoutCalculator.DesiredSize;
        (this.Axis.AxisLayoutPanel as ChartPolarAxisLayoutPanel).Radius = (layoutCalculator as ChartCircularAxisPanel).Radius;
      }
    }
    else
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double width = 0.0;
      double height = 0.0;
      if (this.Axis.headerContent != null)
      {
        this.Axis.headerContent.HorizontalAlignment = HorizontalAlignment.Center;
        this.Axis.headerContent.VerticalAlignment = VerticalAlignment.Center;
        if (this.Axis.Orientation == Orientation.Vertical)
        {
          double num3 = (this.Axis.OpposedPosition ? 1.0 : -1.0) * 90.0;
          this.Axis.headerContent.RenderTransform = (Transform) new RotateTransform()
          {
            Angle = num3
          };
        }
        else
          this.Axis.headerContent.RenderTransform = (Transform) null;
      }
      foreach (UIElement child in this.Children)
      {
        bool flag = this.Axis.headerContent == child && this.Axis.Orientation == Orientation.Vertical;
        Size availableSize1 = availableSize;
        if (flag)
        {
          availableSize1.Width = Math.Max(availableSize.Width, child.DesiredSize.Width);
          availableSize1.Height = Math.Max(availableSize.Height, child.DesiredSize.Height);
        }
        child.Measure(availableSize1);
        if (!(child is SfChartResizableBar) || !this.Axis.EnableTouchMode)
        {
          width += flag ? child.DesiredSize.Height : child.DesiredSize.Width;
          height += flag ? child.DesiredSize.Width : child.DesiredSize.Height;
        }
      }
      foreach (ILayoutCalculator layoutCalculator in this.LayoutCalc)
      {
        layoutCalculator.Measure(availableSize);
        if (layoutCalculator is ChartCartesianAxisLabelsPanel && this.Axis.GetLabelPosition() == AxisElementPosition.Inside || layoutCalculator is ChartCartesianAxisElementsPanel && this.Axis.TickLinesPosition == AxisElementPosition.Inside)
        {
          num1 += layoutCalculator.DesiredSize.Width;
          num2 += layoutCalculator.DesiredSize.Height;
        }
        width += layoutCalculator.DesiredSize.Width;
        height += layoutCalculator.DesiredSize.Height;
      }
      if (this.Axis.MultiLevelLabels != null && this.Axis.MultiLevelLabels.Count > 0 && this.Axis.axisMultiLevelLabelsPanel != null)
      {
        this.Axis.axisMultiLevelLabelsPanel.Measure(availableSize);
        if (this.Axis.GetLabelPosition() == AxisElementPosition.Inside)
        {
          num1 += this.Axis.axisMultiLevelLabelsPanel.DesiredSize.Width;
          num2 += this.Axis.axisMultiLevelLabelsPanel.DesiredSize.Height;
        }
        width += this.Axis.axisMultiLevelLabelsPanel.DesiredSize.Width;
        height += this.Axis.axisMultiLevelLabelsPanel.DesiredSize.Height;
      }
      if (this.Axis.Orientation == Orientation.Vertical)
      {
        this.Axis.InsidePadding = num1;
        size = new Size(width, availableSize.Height);
      }
      else
      {
        this.Axis.InsidePadding = num2;
        size = new Size(availableSize.Width, height);
      }
    }
    return ChartLayoutUtils.CheckSize(size);
  }

  internal void ArrangeElements(Size finalSize)
  {
    if (this.Axis.AxisLayoutPanel is ChartPolarAxisLayoutPanel)
    {
      foreach (UIElement child in this.Children)
      {
        child.Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
        Canvas.SetLeft(child, 0.0);
        Canvas.SetTop(child, 0.0);
      }
      foreach (ILayoutCalculator layoutCalculator in this.LayoutCalc)
      {
        layoutCalculator.Arrange(finalSize);
        (this.Axis.AxisLayoutPanel as ChartPolarAxisLayoutPanel).Radius = (layoutCalculator as ChartCircularAxisPanel).Radius;
      }
    }
    else
      this.ArrangeCartesianElements(finalSize);
  }

  private void ArrangeCartesianElements(Size finalSize)
  {
    if (this.Axis == null)
      return;
    foreach (UIElement child in this.Children)
    {
      Canvas.SetLeft(child, 0.0);
      Canvas.SetTop(child, 0.0);
    }
    UIElement child1 = this.Children[0];
    ILayoutCalculator labelsPanel = this.LayoutCalc.Count > 0 ? this.LayoutCalc[0] : (ILayoutCalculator) null;
    ILayoutCalculator layoutCalculator1 = this.LayoutCalc.Count > 1 ? this.LayoutCalc[1] : (ILayoutCalculator) null;
    MultiLevelLabelsPanel levelLabelsPanel = this.Axis.axisMultiLevelLabelsPanel;
    SfChartResizableBar chartResizableBar = this.Children.OfType<SfChartResizableBar>().FirstOrDefault<SfChartResizableBar>();
    List<UIElement> uiElementList = new List<UIElement>();
    List<Size> sizeList = new List<Size>();
    bool flag1 = this.Axis.Orientation == Orientation.Vertical;
    bool flag2 = this.Axis.OpposedPosition ^ flag1;
    double num1 = 2.0;
    if (chartResizableBar != null)
    {
      uiElementList.Add((UIElement) chartResizableBar);
      sizeList.Add(chartResizableBar.DesiredSize);
    }
    if (layoutCalculator1 != null)
    {
      if (this.Axis.TickLinesPosition == AxisElementPosition.Inside)
      {
        uiElementList.Insert(0, (UIElement) layoutCalculator1.Panel);
        sizeList.Insert(0, layoutCalculator1.DesiredSize);
      }
      else
      {
        uiElementList.Add((UIElement) layoutCalculator1.Panel);
        sizeList.Add(layoutCalculator1.DesiredSize);
      }
    }
    if (labelsPanel != null)
    {
      if (this.Axis.GetLabelPosition() == AxisElementPosition.Inside)
      {
        uiElementList.Insert(0, (UIElement) labelsPanel.Panel);
        sizeList.Insert(0, labelsPanel.DesiredSize);
      }
      else
      {
        uiElementList.Add((UIElement) labelsPanel.Panel);
        sizeList.Add(labelsPanel.DesiredSize);
      }
    }
    if (this.Axis.MultiLevelLabels != null && this.Axis.MultiLevelLabels.Count > 0 && levelLabelsPanel != null)
    {
      if (this.Axis.GetLabelPosition() == AxisElementPosition.Inside)
      {
        uiElementList.Insert(0, (UIElement) levelLabelsPanel.Panel);
        sizeList.Insert(0, levelLabelsPanel.DesiredSize);
      }
      else
      {
        uiElementList.Add((UIElement) levelLabelsPanel.Panel);
        sizeList.Add(levelLabelsPanel.DesiredSize);
      }
    }
    if (child1 != null)
    {
      uiElementList.Add(child1);
      Size size = child1.DesiredSize;
      if (flag1 && child1.GetType() == typeof (ContentControl))
        size = new Size(size.Height, size.Width);
      sizeList.Add(size);
    }
    if (flag2)
    {
      uiElementList.Reverse();
      sizeList.Reverse();
    }
    double length1 = 0.0;
    for (int index = 0; index < uiElementList.Count; ++index)
    {
      UIElement element = uiElementList[index];
      bool flag3 = element is SfChartResizableBar;
      if (flag1)
      {
        if (element == child1)
        {
          double length2 = length1 - (element.DesiredSize.Width - sizeList[index].Width) / 2.0;
          double height = element.DesiredSize.Height;
          double width = element.DesiredSize.Width;
          Canvas.SetTop(element, (finalSize.Height - element.DesiredSize.Height) / 2.0);
          if (this.Axis.TickLinesPosition == AxisElementPosition.Inside)
          {
            double num2 = layoutCalculator1.DesiredSize.Width - this.Axis.TickLineSize;
            double length3 = this.Axis.OpposedPosition ? length2 + num2 : length2 - num2;
            Canvas.SetLeft(element, length3);
          }
          if (this.Axis.ShowAxisNextToOrigin && this.Axis.HeaderPosition == AxisHeaderPosition.Far)
          {
            ChartAxis chartAxis = (ChartAxis) null;
            double num3 = 0.0;
            if (this.Axis.Area.InternalSecondaryAxis == this.Axis)
            {
              num3 = this.Axis.Area.InternalPrimaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
              chartAxis = this.Axis.Area.InternalPrimaryAxis;
            }
            else if (this.Axis.Area.InternalSecondaryAxis.Orientation == Orientation.Horizontal && this.Axis.Area.InternalPrimaryAxis == this.Axis)
            {
              num3 = this.Axis.Area.InternalSecondaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
              chartAxis = this.Axis.Area.InternalSecondaryAxis;
            }
            if (0.0 < num3 && 1.0 > num3 && chartAxis != null)
            {
              if (this.Axis.OpposedPosition)
              {
                Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), this.Axis.AvailableSize), this.Axis.Area.AxisThickness);
                double num4 = (1.0 - num3) * (rect.Width - chartAxis.GetActualPlotOffset()) + chartAxis.GetActualPlotOffsetStart() + this.Axis.InsidePadding;
                if (num4 > finalSize.Width - height)
                {
                  if (chartAxis.AxisLayoutPanel is ChartPolarAxisLayoutPanel)
                    Canvas.SetLeft(element, length2);
                  else
                    Canvas.SetLeft(element, num4 - (width - height) / 2.0 + num1);
                }
                else
                  Canvas.SetLeft(element, length2);
              }
              else if (this.Axis.ArrangeRect.Left + finalSize.Width - height > finalSize.Width - height)
              {
                if (chartAxis.AxisLayoutPanel is ChartPolarAxisLayoutPanel)
                  Canvas.SetLeft(element, length2);
                else
                  Canvas.SetLeft(element, -(this.Axis.ArrangeRect.Left + (width - height) / 2.0));
              }
              else
                Canvas.SetLeft(element, length2);
            }
            else
              Canvas.SetLeft(element, length2);
          }
          else
            Canvas.SetLeft(element, length2);
        }
        else
        {
          if (flag3 && this.Axis.EnableTouchMode)
          {
            Canvas.SetLeft(element, length1 - sizeList[index].Width / 2.0);
            continue;
          }
          if (flag3 && !this.Axis.EnableTouchMode && this.Axis.TickLinesPosition == AxisElementPosition.Inside)
          {
            double num5 = layoutCalculator1.DesiredSize.Width - this.Axis.TickLineSize;
            double length4 = this.Axis.OpposedPosition ? length1 + num5 : length1 - num5;
            Canvas.SetLeft(element, length4);
          }
          else
            Canvas.SetLeft(element, length1);
        }
        length1 += sizeList[index].Width;
      }
      else
      {
        if (element == child1)
        {
          Canvas.SetLeft(element, (finalSize.Width - sizeList[index].Width) / 2.0);
          double height = element.DesiredSize.Height;
          if (this.Axis.TickLinesPosition == AxisElementPosition.Inside)
          {
            double num6 = layoutCalculator1.DesiredSize.Height - this.Axis.TickLineSize;
            double length5 = this.Axis.OpposedPosition ? length1 - num6 : length1 + num6;
            Canvas.SetTop(element, length5);
          }
          if (this.Axis.ShowAxisNextToOrigin && this.Axis.HeaderPosition == AxisHeaderPosition.Far)
          {
            ChartAxis chartAxis = (ChartAxis) null;
            double num7 = 0.0;
            if (this.Axis.Area.InternalPrimaryAxis == this.Axis)
            {
              num7 = this.Axis.Area.InternalSecondaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
              chartAxis = this.Axis.Area.InternalSecondaryAxis;
            }
            else if (this.Axis.Area.InternalPrimaryAxis.Orientation == Orientation.Vertical && this.Axis.Area.InternalSecondaryAxis == this.Axis)
            {
              num7 = this.Axis.Area.InternalPrimaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
              chartAxis = this.Axis.Area.InternalPrimaryAxis;
            }
            if (0.0 < num7 && 1.0 > num7 && chartAxis != null)
            {
              Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), this.Axis.AvailableSize), this.Axis.Area.AxisThickness);
              double num8 = num7 * (rect.Height - chartAxis.GetActualPlotOffset()) + chartAxis.GetActualPlotOffsetEnd() + this.Axis.InsidePadding;
              if (this.Axis.OpposedPosition)
              {
                if (this.Axis.ArrangeRect.Top + finalSize.Height - height > finalSize.Height - height)
                  Canvas.SetTop(element, -this.Axis.ArrangeRect.Top - num1);
                else
                  Canvas.SetTop(element, length1);
              }
              else if (num8 > finalSize.Height - height)
                Canvas.SetTop(element, num8 + num1);
              else
                Canvas.SetTop(element, length1);
            }
            else
              Canvas.SetTop(element, length1);
          }
          else
            Canvas.SetTop(element, length1);
        }
        else
        {
          if (flag3 && this.Axis.EnableTouchMode)
          {
            Canvas.SetTop(element, length1 - sizeList[index].Height / 2.0);
            continue;
          }
          if (flag3 && !this.Axis.EnableTouchMode && this.Axis.TickLinesPosition == AxisElementPosition.Inside)
          {
            double num9 = layoutCalculator1.DesiredSize.Height - this.Axis.TickLineSize;
            double length6 = this.Axis.OpposedPosition ? length1 - num9 : length1 + num9;
            Canvas.SetTop(element, length6);
          }
          else
            Canvas.SetTop(element, length1);
        }
        length1 += sizeList[index].Height;
      }
    }
    foreach (ILayoutCalculator layoutCalculator2 in this.LayoutCalc)
      layoutCalculator2.Arrange(layoutCalculator2.DesiredSize);
    foreach (UIElement element in uiElementList)
    {
      if ((element as FrameworkElement).Name == "axisLabelsPanel")
      {
        this.SetLabelsPanelBounds(element, labelsPanel);
        break;
      }
    }
    if (this.Axis.MultiLevelLabels == null || this.Axis.MultiLevelLabels.Count <= 0 || levelLabelsPanel == null)
      return;
    levelLabelsPanel.Arrange(levelLabelsPanel.DesiredSize);
  }

  private void SetLabelsPanelBounds(UIElement element, ILayoutCalculator labelsPanel)
  {
    double left1 = Canvas.GetLeft(element);
    double top1 = Canvas.GetTop(element);
    double width = labelsPanel.DesiredSize.Width;
    double height = labelsPanel.DesiredSize.Height;
    ChartCartesianAxisLabelsPanel cartesianAxisLabelsPanel = labelsPanel as ChartCartesianAxisLabelsPanel;
    if (this.Axis.Orientation == Orientation.Horizontal)
    {
      double left2 = left1 + this.Axis.ArrangeRect.Left;
      double top2 = top1 + this.Axis.ArrangeRect.Top;
      cartesianAxisLabelsPanel.SetOffsetValues(left2, top2, width, height);
    }
    else
    {
      double left3 = left1 + this.Axis.ArrangeRect.Left;
      double top3 = top1 + this.Axis.ArrangeRect.Top;
      cartesianAxisLabelsPanel.SetOffsetValues(left3, top3, width, height);
    }
  }
}
