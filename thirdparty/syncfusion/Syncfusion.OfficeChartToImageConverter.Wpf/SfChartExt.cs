// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.SfChartExt
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class SfChartExt : SfChart
{
  private System.Windows.Size m_plotAreaSize = System.Windows.Size.Empty;

  private Canvas customCanvas { get; set; }

  internal Canvas AdornmentPresenter { get; set; }

  internal System.Windows.Size ChartAreaSize { get; private set; }

  private double[] TLBRValue { get; set; }

  internal void SetMargin(
    Thickness marginThickness,
    bool isInnerLayout,
    bool isLegendOutside,
    bool isLegendManual,
    int legPosition)
  {
    ChartRootPanel templateChild = this.GetTemplateChild("LayoutRoot") as ChartRootPanel;
    ChartLegend legend;
    if (this.Legend is ChartLegendCollection)
    {
      legend = (this.Legend as ChartLegendCollection)[0];
      ChartLegend chartLegend = (this.Legend as ChartLegendCollection)[1];
    }
    else
      legend = this.Legend as ChartLegend;
    Rect rect = (Rect) legend.GetType().GetProperty("ArrangeRect", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty).GetValue((object) legend, (object[]) null);
    double actualWidth = this.ActualWidth;
    double left = this.Margin.Left;
    double right = this.Margin.Right;
    double width1 = rect.Width;
    this.TLBRValue = new double[4];
    if (isInnerLayout || (isLegendManual ? 1 : (legPosition == -1 ? 1 : 0)) != 0)
      this.UpdateTLBR();
    if (isInnerLayout)
    {
      marginThickness.Left -= this.TLBRValue[1];
      marginThickness.Right -= this.TLBRValue[3];
      marginThickness.Top -= this.TLBRValue[0];
      marginThickness.Bottom -= this.TLBRValue[2];
    }
    if (legPosition != -1 && isLegendOutside && isLegendManual && legend != null)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.FontFamily = legend.FontFamily;
      textBlock.FontSize = legend.FontSize;
      textBlock.FontWeight = legend.FontWeight;
      textBlock.FontStyle = legend.FontStyle;
      System.Windows.Size size = new System.Windows.Size();
      for (int index = 0; index < legend.Items.Count; ++index)
      {
        if (legend.Items[index] is LegendItem legendItem && legendItem.VisibilityOnLegend == Visibility.Visible)
        {
          textBlock.Text = legendItem.Label;
          textBlock.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
          if (size.Width < textBlock.DesiredSize.Width)
            size.Width = textBlock.DesiredSize.Width;
          if (size.Height < textBlock.DesiredSize.Height)
            size.Height = textBlock.DesiredSize.Height;
        }
      }
      if (size.Width > this.Width * 0.375)
        size.Width = this.Width * 0.375;
      switch (legPosition)
      {
        case 0:
          marginThickness.Bottom += size.Height + 10.0;
          break;
        case 1:
          marginThickness.Right += size.Width + 10.0 + legend.IconWidth;
          marginThickness.Top += size.Height + 10.0;
          break;
        case 2:
          marginThickness.Top += size.Height + 10.0;
          break;
        case 3:
          marginThickness.Right += size.Width + 10.0 + legend.IconWidth;
          break;
        case 4:
          marginThickness.Left += size.Width + 10.0 + legend.IconWidth;
          break;
      }
    }
    if (legend != null && legPosition == -1 && !isLegendManual)
    {
      double width2 = this.ChartAreaSize.Width;
      double height1 = this.ChartAreaSize.Height;
      ChartDock dockPosition = legend.DockPosition;
      legend.DockPosition = ChartDock.Left;
      double width3 = legend.DesiredSize.Width;
      double height2 = legend.DesiredSize.Height;
      double top1 = -(this.TLBRValue[0] / 2.0) + 5.0;
      Thickness thickness = new Thickness();
      switch (dockPosition)
      {
        case ChartDock.Left:
          double top2 = top1 + (height1 - height2) / 2.0;
          thickness = new Thickness(10.0, top2, 0.0, top2 - 10.0);
          break;
        case ChartDock.Top:
          double num1 = (width2 - width3) / 2.0;
          thickness = new Thickness(num1, top1, num1, 0.0);
          break;
        case ChartDock.Right:
          if (legend.VerticalAlignment == VerticalAlignment.Top && legend.HorizontalAlignment == HorizontalAlignment.Right)
          {
            thickness = new Thickness(width2 - width3 - 10.0, top1, 10.0, 0.0);
            break;
          }
          double top3 = top1 + (height1 - height2) / 2.0;
          thickness = new Thickness(width2 - width3 - 10.0, top3, 10.0, top3 - 10.0);
          break;
        case ChartDock.Bottom:
          double num2 = (width2 - width3) / 2.0;
          double top4 = top1 + (height1 - height2);
          thickness = new Thickness(num2, top4, num2, 10.0);
          break;
      }
      legend.LegendPosition = LegendPosition.Inside;
      legend.VerticalAlignment = VerticalAlignment.Top;
      legend.HorizontalAlignment = HorizontalAlignment.Left;
      legend.Margin = thickness;
    }
    if (marginThickness.Left < 0.0)
      marginThickness.Left = 0.0;
    if (marginThickness.Right < 0.0)
      marginThickness.Right = 0.0;
    if (marginThickness.Top < 0.0)
      marginThickness.Top = 0.0;
    if (marginThickness.Bottom < 0.0)
      marginThickness.Bottom = 0.0;
    templateChild.Margin = marginThickness;
  }

  internal System.Windows.Size PlotAreaSize
  {
    get
    {
      if (this.m_plotAreaSize.IsEmpty)
      {
        if (this.GetTemplateChild("InternalCanvas") is Grid templateChild)
        {
          double height = this.ChartAreaSize.Height - (templateChild.Margin.Bottom + templateChild.Margin.Top);
          this.m_plotAreaSize = new System.Windows.Size(this.ChartAreaSize.Width - (templateChild.Margin.Right + templateChild.Margin.Left), height);
        }
        else
          this.m_plotAreaSize = this.ChartAreaSize;
      }
      return this.m_plotAreaSize;
    }
  }

  private void UpdateTLBR()
  {
    this.TLBRValue = new double[4];
    Grid templateChild1 = this.GetTemplateChild("InternalCanvas") as Grid;
    Canvas templateChild2 = this.GetTemplateChild("PART_chartAxisPanel") as Canvas;
    double top = templateChild1.Margin.Top;
    double left = templateChild1.Margin.Left;
    double num1 = this.ChartAreaSize.Height - templateChild1.Margin.Bottom;
    double num2 = this.ChartAreaSize.Width - templateChild1.Margin.Right;
    foreach (UIElement child in templateChild2.Children)
    {
      if (child is ChartAxis chartAxis && chartAxis.Visibility == Visibility.Visible)
      {
        Rect arrangeRect = chartAxis.ArrangeRect;
        if (arrangeRect.Left < left && arrangeRect.Right <= left)
          this.TLBRValue[1] = arrangeRect.Width;
        else if (arrangeRect.Left >= num2 && arrangeRect.Right > num2)
          this.TLBRValue[3] = arrangeRect.Width;
        else if (arrangeRect.Top < top && arrangeRect.Bottom <= top)
          this.TLBRValue[0] = arrangeRect.Height;
        else if (arrangeRect.Top >= num1 && arrangeRect.Bottom > num1)
          this.TLBRValue[2] = arrangeRect.Height;
      }
    }
  }

  protected override void OnRootPanelSizeChanged(System.Windows.Size size)
  {
    this.ChartAreaSize = size;
    base.OnRootPanelSizeChanged(size);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    Grid parent = (this.GetTemplateChild("Part_DockPanel") as ChartDockPanel).Parent as Grid;
    if (this.AdornmentPresenter == null)
    {
      this.AdornmentPresenter = new Canvas();
      (this.GetTemplateChild("LayoutRoot") as ChartRootPanel).Children.Add((UIElement) this.AdornmentPresenter);
    }
    if (this.customCanvas != null)
      return;
    this.customCanvas = new Canvas();
    parent.Children.Add((UIElement) this.customCanvas);
  }

  internal void AddTextBlockInCustomCanvas(
    Border textBlock,
    RectangleF rectPosition,
    bool canOverlap,
    bool isNotManualLayout)
  {
    if (this.customCanvas == null)
      return;
    this.customCanvas.Children.Add((UIElement) textBlock);
    if (((double) rectPosition.X < 0.0 || (double) rectPosition.Y < 0.0) && canOverlap)
    {
      textBlock.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
      Canvas.SetLeft((UIElement) textBlock, (this.ChartAreaSize.Width - textBlock.DesiredSize.Width) / 2.0);
      Canvas.SetTop((UIElement) textBlock, 10.0);
    }
    else
    {
      textBlock.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
      if (textBlock.DesiredSize.Width > this.DesiredSize.Width)
        textBlock.Width = this.DesiredSize.Width - (double) rectPosition.X;
      Canvas.SetLeft((UIElement) textBlock, (double) rectPosition.X);
      Canvas.SetTop((UIElement) textBlock, (double) rectPosition.Y);
    }
  }

  internal void TrySetManualAxisTextElements(
    Dictionary<ChartAxis, Tuple<Border, PointF>> axisBorders,
    bool isDisplayUnit)
  {
    if (axisBorders.Count == 0)
      return;
    Grid templateChild1 = this.GetTemplateChild("InternalCanvas") as Grid;
    ChartRootPanel templateChild2 = this.GetTemplateChild("LayoutRoot") as ChartRootPanel;
    double top = templateChild1.Margin.Top;
    double left = templateChild1.Margin.Left;
    double width1 = this.Width;
    double height1 = this.Height;
    double num1 = this.ChartAreaSize.Height - templateChild1.Margin.Bottom;
    double num2 = this.ChartAreaSize.Width - templateChild1.Margin.Right;
    double num3 = this.Height - this.ChartAreaSize.Height;
    double num4 = this.Width - this.ChartAreaSize.Width;
    Thickness margin = templateChild2.Margin;
    double num5 = isDisplayUnit ? 30.0 : 2.0;
    double num6 = this.TLBRValue[0] + margin.Top;
    double num7 = this.TLBRValue[2] + margin.Bottom;
    double num8 = this.TLBRValue[1] + margin.Left;
    double num9 = this.TLBRValue[3] + margin.Right;
    if (this.customCanvas == null)
      return;
    foreach (KeyValuePair<ChartAxis, Tuple<Border, PointF>> axisBorder in axisBorders)
    {
      Rect arrangeRect = axisBorder.Key.ArrangeRect;
      Border element = axisBorder.Value.Item1;
      PointF pointF = axisBorder.Value.Item2;
      element.Measure(new System.Windows.Size(double.MaxValue, double.MaxValue));
      double height2 = element.DesiredSize.Height;
      double width2 = element.DesiredSize.Width;
      bool flag = false;
      if ((double) pointF.X < 0.0 || (double) pointF.Y < 0.0)
      {
        if (arrangeRect.Left < left && arrangeRect.Right <= left)
        {
          pointF.X = margin.Left >= width2 ? (float) (margin.Left - width2) : 0.0f;
          if (height2 * 0.7 > this.ChartAreaSize.Height - (num6 + num7))
          {
            pointF.Y = (float) (num6 + num3 / 2.0);
            (element.Child as TextBlock).TextWrapping = TextWrapping.Wrap;
            element.Width = height1 - num6;
            element.Height = height2 / element.Width * width2;
            pointF.X = (float) (margin.Left - element.Height);
            flag = true;
          }
          else
            pointF.Y = (float) (num6 + num3 / 2.0 + (this.ChartAreaSize.Height - (height2 + num6 + num7)) / num5);
        }
        else if (arrangeRect.Left >= num2 && arrangeRect.Right > num2)
        {
          pointF.X = margin.Right >= width2 ? (float) (width1 - margin.Right) : (float) (width1 - width2);
          if (height2 * 0.7 > this.ChartAreaSize.Height - (num6 + num7))
          {
            pointF.Y = (float) (num6 + num3 / 2.0);
            (element.Child as TextBlock).TextWrapping = TextWrapping.Wrap;
            element.Width = height1 - num6;
            element.Height = height2 / element.Width * width2;
            pointF.X = (float) (width1 - margin.Right);
            flag = true;
          }
          else
            pointF.Y = (float) (num6 + num3 / 2.0 + (this.ChartAreaSize.Height - (height2 + num6 + num7)) / num5);
        }
        else if (arrangeRect.Top < top && arrangeRect.Bottom <= top)
        {
          pointF.Y = margin.Top >= height2 ? (float) (margin.Top - height2) : 0.0f;
          if (width2 * 0.7 > this.ChartAreaSize.Width - (num8 + num9))
          {
            pointF.X = (float) (num8 + num4 / 2.0);
            (element.Child as TextBlock).TextWrapping = TextWrapping.Wrap;
            element.Width = width1 - num8;
            element.Height = width2 / element.Width * height2;
            pointF.Y = (float) (margin.Top - element.Height);
            flag = true;
          }
          else
            pointF.X = (float) (num8 + num4 / 2.0 + (this.ChartAreaSize.Width - (width2 + num8 + num9)) / num5);
        }
        else if (arrangeRect.Top >= num1 && arrangeRect.Bottom > num1)
        {
          pointF.Y = margin.Bottom >= height2 ? (float) (height1 - margin.Bottom) : (float) (height1 - height2);
          if (width2 * 0.7 > this.ChartAreaSize.Width - (num8 + num9))
          {
            pointF.X = (float) (num8 + num4 / 2.0);
            (element.Child as TextBlock).TextWrapping = TextWrapping.Wrap;
            element.Width = width1 - num8;
            element.Height = width2 / element.Width * height2;
            pointF.Y = (float) (height1 - margin.Bottom);
            flag = true;
          }
          else
            pointF.X = (float) (num8 + num4 / 2.0 + (this.ChartAreaSize.Width - (width2 + num8 + num9)) / num5);
        }
      }
      if ((double) pointF.X >= 0.0 && (double) pointF.Y >= 0.0 || flag)
      {
        this.customCanvas.Children.Add((UIElement) element);
        Canvas.SetLeft((UIElement) element, (double) pointF.X);
        Canvas.SetTop((UIElement) element, (double) pointF.Y);
      }
    }
  }

  private void Chart_LayoutUpdated(object sender, EventArgs e)
  {
    ChartRootPanel templateChild = this.GetTemplateChild("LayoutRoot") as ChartRootPanel;
    SfChartExt sfChartExt = this;
    ChartLegend chartLegend = !(sfChartExt.Legend is ChartLegendCollection) ? sfChartExt.Legend as ChartLegend : (sfChartExt.Legend as ChartLegendCollection)[0];
    if (this.TLBRValue != null && this.TLBRValue.Length > 3 && this.TLBRValue[3] == 0.0)
      this.UpdateTLBR();
    if (templateChild != null && chartLegend != null && this.TLBRValue[3] == 0.0)
    {
      Rect rect = (Rect) sfChartExt.Legend.GetType().GetProperty("ArrangeRect", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty).GetValue((object) chartLegend, (object[]) null);
      double num = this.ChartAreaSize.Width - sfChartExt.Margin.Left - sfChartExt.Margin.Right - rect.Width;
      chartLegend.Margin = new Thickness(-num + chartLegend.Margin.Left + templateChild.Margin.Left + templateChild.Margin.Right, chartLegend.Margin.Top, chartLegend.Margin.Right, chartLegend.Margin.Bottom);
    }
    this.LayoutUpdated -= new EventHandler(this.Chart_LayoutUpdated);
  }

  internal void AddHandlerLayoutUpdate(bool isAdornmentUpdate)
  {
    if (isAdornmentUpdate)
      this.LayoutUpdated += new EventHandler(this.UpdateAdornmentsOnLayouting);
    else
      this.LayoutUpdated += new EventHandler(this.Chart_LayoutUpdated);
  }

  private void UpdateAdornmentsOnLayouting(object sender, EventArgs e)
  {
    DependencyObject templateChild = this.GetTemplateChild("InternalCanvas");
    if (this.AdornmentPresenter == null || templateChild == null)
      return;
    this.AdornmentPresenter.Margin = (Thickness) templateChild.GetValue(FrameworkElement.MarginProperty);
    foreach (ChartSeriesBase chartSeriesBase in (Collection<ChartSeriesBase>) this.VisibleSeries)
    {
      ChartAdornmentPresenter adornmentPresenter = chartSeriesBase.GetType().GetProperty("AdornmentPresenter", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetProperty).GetValue((object) chartSeriesBase, (object[]) null) as ChartAdornmentPresenter;
      for (int index = 0; index < adornmentPresenter.Children.Count; ++index)
      {
        UIElement child = adornmentPresenter.Children[index];
        switch (child)
        {
          case TextBlock _:
          case ContentControl _:
            adornmentPresenter.Children.Remove(child);
            this.AdornmentPresenter.Children.Add(child);
            break;
        }
      }
    }
  }
}
