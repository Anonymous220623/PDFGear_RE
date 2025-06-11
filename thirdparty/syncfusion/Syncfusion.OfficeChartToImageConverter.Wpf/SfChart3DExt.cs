// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.SfChart3DExt
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class SfChart3DExt : SfChart3D
{
  private Canvas customCanvas { get; set; }

  internal Canvas AdornmentPresenter { get; set; }

  private System.Windows.Size ChartAreaSize { get; set; }

  protected override void OnRootPanelSizeChanged(System.Windows.Size size)
  {
    this.ChartAreaSize = size;
    base.OnRootPanelSizeChanged(size);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.AdornmentPresenter == null)
    {
      this.AdornmentPresenter = new Canvas();
      (this.GetTemplateChild("Part_LayoutRoot") as ChartRootPanel).Children.Add((UIElement) this.AdornmentPresenter);
    }
    if (this.customCanvas != null)
      return;
    Grid parent = (this.GetTemplateChild("Part_DockPanel") as ChartDockPanel).Parent as Grid;
    this.customCanvas = new Canvas();
    parent.Children.Add((UIElement) this.customCanvas);
  }

  internal void AddTextBlockInCustomCanvas(
    Border textBlock,
    RectangleF rectPosition,
    bool canOverlap)
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
      Canvas.SetLeft((UIElement) textBlock, (double) rectPosition.X);
      Canvas.SetTop((UIElement) textBlock, (double) rectPosition.Y);
    }
  }

  internal void TrySetManualAxisTextElements(
    Dictionary<ChartAxis, Tuple<Border, PointF>> axisBorders,
    bool isDisplayUnit)
  {
    if (axisBorders.Count == 0 || this.customCanvas == null)
      return;
    foreach (KeyValuePair<ChartAxis, Tuple<Border, PointF>> axisBorder in axisBorders)
    {
      Border element = axisBorder.Value.Item1;
      PointF pointF = axisBorder.Value.Item2;
      if ((double) pointF.X < 0.0)
        pointF.X = 0.0f;
      if ((double) pointF.Y < 0.0)
        pointF.Y = 0.0f;
      this.customCanvas.Children.Add((UIElement) element);
      Canvas.SetLeft((UIElement) element, (double) pointF.X);
      Canvas.SetTop((UIElement) element, (double) pointF.Y);
    }
  }

  private void CustomChart_LayoutUpdated(object sender, EventArgs e)
  {
    if (this.AdornmentPresenter == null || this.AdornmentPresenter.Children.Count != 0)
      return;
    object templateChild = (object) this.GetTemplateChild("PART_3DPanel");
    if (templateChild == null || !(templateChild is Canvas))
      return;
    Canvas canvas = templateChild as Canvas;
    for (int index = 0; index < canvas.Children.Count; ++index)
    {
      UIElement child = canvas.Children[index];
      if (child is ContentControl && (child as ContentControl).Content is ChartAdornment3D)
      {
        canvas.Children.Remove(child);
        this.AdornmentPresenter.Children.Add(child);
      }
    }
  }

  internal void AddHandlerLayoutUpdate()
  {
    this.LayoutUpdated += new EventHandler(this.CustomChart_LayoutUpdated);
  }
}
