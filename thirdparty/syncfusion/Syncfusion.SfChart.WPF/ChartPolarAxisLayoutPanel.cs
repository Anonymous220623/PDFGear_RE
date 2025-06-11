// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartPolarAxisLayoutPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartPolarAxisLayoutPanel : ILayoutCalculator
{
  private Size desiredSize;
  private Panel panel;
  private double radius;
  private bool isRadiusCalculating;

  public ChartPolarAxisLayoutPanel(Panel panel)
  {
    this.panel = panel != null ? panel : throw new ArgumentNullException();
  }

  public SfChart Area { get; set; }

  public Panel Panel => this.panel;

  public ChartAxisBase2D PolarAxis { get; set; }

  public ChartAxisBase2D CartesianAxis { get; set; }

  public double Radius
  {
    get => this.radius;
    set
    {
      if (this.radius == value)
        return;
      this.radius = value;
      if (this.isRadiusCalculating || this.Area == null || !(this.Area.GridLinesLayout is ChartPolarGridLinesPanel))
        return;
      this.CalculateSeriesRect(this.desiredSize);
      (this.Area.GridLinesLayout as ChartPolarGridLinesPanel).UpdateElements();
      (this.Area.GridLinesLayout as ChartPolarGridLinesPanel).Measure(this.desiredSize);
    }
  }

  public Size DesiredSize => this.desiredSize;

  public List<UIElement> Children
  {
    get
    {
      return this.panel != null ? this.panel.Children.Cast<UIElement>().ToList<UIElement>() : (List<UIElement>) null;
    }
  }

  public double Left { get; set; }

  public double Top { get; set; }

  public Size Measure(Size availableSize)
  {
    this.isRadiusCalculating = true;
    if (this.PolarAxis != null)
    {
      this.PolarAxis.ComputeDesiredSize(availableSize);
      if (this.PolarAxis.EnableScrollBar)
      {
        this.PolarAxis.DisableScrollbar = true;
        this.PolarAxis.EnableScrollBar = !this.PolarAxis.DisableScrollbar;
      }
    }
    if (this.CartesianAxis != null)
    {
      if (this.CartesianAxis.EnableScrollBar)
      {
        this.CartesianAxis.DisableScrollbar = true;
        this.CartesianAxis.EnableScrollBar = !this.CartesianAxis.DisableScrollbar;
      }
      this.CalculateSeriesRect(availableSize);
      if (this.CartesianAxis.PolarAngle == ChartPolarAngle.Rotate0 || this.CartesianAxis.PolarAngle == ChartPolarAngle.Rotate180)
        this.CartesianAxis.ComputeDesiredSize(new Size(Math.Min(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height) / 2.0, this.Area.SeriesClipRect.Height));
      else
        this.CartesianAxis.ComputeDesiredSize(new Size(this.Area.SeriesClipRect.Width, Math.Min(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height) / 2.0));
    }
    this.isRadiusCalculating = false;
    this.desiredSize = availableSize;
    return availableSize;
  }

  public void DetachElements()
  {
    if (this.CartesianAxis != null)
    {
      if (this.CartesianAxis.GridLinesRecycler != null)
        this.CartesianAxis.GridLinesRecycler.Clear();
      if (this.CartesianAxis.MinorGridLinesRecycler != null)
        this.CartesianAxis.MinorGridLinesRecycler.Clear();
    }
    this.panel.Children.Clear();
    this.panel = (Panel) null;
  }

  public Size Arrange(Size finalSize)
  {
    Rect rect = new Rect(0.0, 0.0, finalSize.Width, finalSize.Height);
    if (this.PolarAxis != null)
    {
      this.PolarAxis.ArrangeRect = rect;
      this.PolarAxis.Measure(new Size(rect.Width, rect.Height));
      this.PolarAxis.Arrange(rect);
      Canvas.SetLeft((UIElement) this.PolarAxis, rect.Left);
      Canvas.SetTop((UIElement) this.PolarAxis, rect.Top);
    }
    ChartAxis cartesianAxis = (ChartAxis) this.CartesianAxis;
    if (cartesianAxis != null)
    {
      this.CalculateCartesianArrangeRect(ChartLayoutUtils.GetCenter(rect), cartesianAxis);
      Rect finalRect = new Rect(this.CartesianAxis.ArrangeRect.Left, this.CartesianAxis.ArrangeRect.Top, this.CartesianAxis.ComputedDesiredSize.Width, this.CartesianAxis.ComputedDesiredSize.Height);
      if (this.CartesianAxis.PolarAngle != ChartPolarAngle.Rotate90 && (!cartesianAxis.OpposedPosition || this.CartesianAxis.PolarAngle == ChartPolarAngle.Rotate180))
        this.CartesianAxis.Measure(new Size(finalRect.Width, finalRect.Height));
      else
        this.CartesianAxis.Measure(new Size(finalRect.Left, finalRect.Height));
      this.CartesianAxis.Arrange(finalRect);
      Canvas.SetLeft((UIElement) this.CartesianAxis, this.CartesianAxis.ArrangeRect.Left);
      Canvas.SetTop((UIElement) this.CartesianAxis, this.CartesianAxis.ArrangeRect.Top);
      if (this.CartesianAxis.PolarAngle == ChartPolarAngle.Rotate90 || this.CartesianAxis.PolarAngle == ChartPolarAngle.Rotate180)
        this.Area.InternalSecondaryAxis.IsInversed = !this.Area.InternalSecondaryAxis.IsInversed;
    }
    return finalSize;
  }

  public void UpdateElements()
  {
    if (this.Area == null || this.Area.InternalPrimaryAxis == null)
      return;
    List<UIElement> uiElementList = new List<UIElement>();
    if (this.Children == null)
      return;
    this.PolarAxis = this.Area.InternalPrimaryAxis as ChartAxisBase2D;
    this.PolarAxis.AxisLayoutPanel = (ILayoutCalculator) this;
    ChartPolarAngle polarAngle = (this.Area.InternalSecondaryAxis as ChartAxisBase2D).PolarAngle;
    switch (polarAngle)
    {
      case ChartPolarAngle.Rotate0:
      case ChartPolarAngle.Rotate180:
        this.Area.InternalSecondaryAxis.Orientation = Orientation.Horizontal;
        if (this.Area.InternalSecondaryAxis is NumericalAxis internalSecondaryAxis)
        {
          internalSecondaryAxis.RangePadding = NumericalPadding.Round;
          break;
        }
        break;
      default:
        this.Area.InternalSecondaryAxis.Orientation = Orientation.Vertical;
        break;
    }
    if (this.Children.Count > 0 && (polarAngle == ChartPolarAngle.Rotate90 || polarAngle == ChartPolarAngle.Rotate180))
      this.Area.InternalSecondaryAxis.IsInversed = !this.Area.InternalSecondaryAxis.IsInversed;
    this.CartesianAxis = this.Area.InternalSecondaryAxis as ChartAxisBase2D;
    foreach (UIElement child in this.Children)
    {
      if (child is ChartAxis chartAxis && !this.Area.Axes.Contains(chartAxis))
        uiElementList.Add((UIElement) chartAxis);
    }
    foreach (UIElement element in uiElementList)
      this.panel.Children.Remove(element);
    uiElementList.Clear();
    List<UIElement> children = this.Children;
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Area.Axes)
    {
      if (!children.Contains((UIElement) ax))
        this.panel.Children.Add((UIElement) ax);
    }
  }

  private void CalculateSeriesRect(Size availableSize)
  {
    double num1 = Math.Max(availableSize.Width / 2.0 - this.Radius, 0.0);
    double num2 = Math.Max(availableSize.Height / 2.0 - this.Radius, 0.0);
    this.Area.AxisThickness = new Thickness().GetThickness(num1, num2, num1, num2);
    Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), availableSize), this.Area.AxisThickness);
    this.Area.SeriesClipRect = new Rect(rect.Left, rect.Top, rect.Width, rect.Height);
    this.Area.InternalCanvas.Clip = (Geometry) new RectangleGeometry()
    {
      Rect = new Rect(0.0, 0.0, this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height)
    };
  }

  private void CalculateCartesianArrangeRect(Point center, ChartAxis axis)
  {
    switch (this.CartesianAxis.PolarAngle)
    {
      case ChartPolarAngle.Rotate0:
        double y1 = axis.OpposedPosition ? center.Y - this.CartesianAxis.ComputedDesiredSize.Height : center.Y;
        this.CartesianAxis.ArrangeRect = new Rect(center.X, y1, this.radius, this.CartesianAxis.ComputedDesiredSize.Height);
        break;
      case ChartPolarAngle.Rotate90:
        this.CartesianAxis.ArrangeRect = new Rect(axis.OpposedPosition ? center.X : center.X - this.CartesianAxis.ComputedDesiredSize.Width, center.Y, this.CartesianAxis.DesiredSize.Width, this.radius);
        break;
      case ChartPolarAngle.Rotate180:
        double y2 = axis.OpposedPosition ? center.Y - this.CartesianAxis.ComputedDesiredSize.Height : center.Y;
        this.CartesianAxis.ArrangeRect = new Rect(center.X - this.radius, y2, this.radius, this.CartesianAxis.ComputedDesiredSize.Height);
        break;
      case ChartPolarAngle.Rotate270:
        this.CartesianAxis.ArrangeRect = new Rect(axis.OpposedPosition ? center.X : center.X - this.CartesianAxis.ComputedDesiredSize.Width, center.Y - this.radius, this.CartesianAxis.ComputedDesiredSize.Width, this.radius);
        break;
    }
  }
}
