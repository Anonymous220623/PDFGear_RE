// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCartesianAxisLayoutPanel
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

public class ChartCartesianAxisLayoutPanel : ILayoutCalculator
{
  private double left;
  private double bottom;
  private double right;
  private double top;
  private Size _desiredSize;
  private Panel panel;
  private List<double> leftSizes = new List<double>();
  private List<double> rightSizes = new List<double>();
  private List<double> topSizes = new List<double>();
  private List<double> bottomSizes = new List<double>();
  private List<double> leftLegendSizes = new List<double>();
  private List<double> rightLegendSizes = new List<double>();
  private List<double> topLegendSizes = new List<double>();
  private List<double> bottomLegendSizes = new List<double>();
  private Size currentAvalilableSize = Size.Empty;

  public ChartCartesianAxisLayoutPanel(Panel panel) => this.panel = panel;

  public ChartBase Area { get; set; }

  public Panel Panel => this.panel;

  public Size DesiredSize => this._desiredSize;

  public List<UIElement> Children
  {
    get
    {
      return this.panel != null ? this.panel.Children.Cast<UIElement>().ToList<UIElement>() : (List<UIElement>) null;
    }
  }

  double ILayoutCalculator.Left
  {
    get => this.Left;
    set => this.Left = value;
  }

  double ILayoutCalculator.Top
  {
    get => this.Top;
    set => this.Top = value;
  }

  protected double Left { get; set; }

  protected double Top { get; set; }

  public Size Measure(Size availableSize)
  {
    this.left = this.right = this.top = this.bottom = 0.0;
    if (this.Area.ColumnDefinitions.Count == 0)
      this.Area.ColumnDefinitions.Add(new ChartColumnDefinition());
    if (this.Area.RowDefinitions.Count == 0)
      this.Area.RowDefinitions.Add(new ChartRowDefinition());
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.Area.ColumnDefinitions)
      columnDefinition.Axis.Clear();
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.Area.RowDefinitions)
      rowDefinition.Axis.Clear();
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Area.Axes)
    {
      if (ax.DisableScrollbar && ax is ChartAxisBase2D chartAxisBase2D)
        chartAxisBase2D.EnableScrollBar = true;
      if (ax.Orientation == Orientation.Horizontal)
      {
        if (!(ax is ChartAxisBase3D axis3D) || !this.PreventAxisAddition(axis3D))
          this.Area.ColumnDefinitions[this.Area.GetActualColumn((UIElement) ax)].Axis.Add(ax);
      }
      else
        this.Area.RowDefinitions[this.Area.GetActualRow((UIElement) ax)].Axis.Add(ax);
    }
    if (this.Area != null)
      this.AxisSpanCalculation();
    this.leftSizes.Clear();
    this.rightSizes.Clear();
    this.topSizes.Clear();
    this.bottomSizes.Clear();
    this.MeasureAxis(availableSize, new Rect(new Point(0.0, 0.0), availableSize));
    this.Area.AxisThickness = new Thickness().GetThickness(this.left, this.top, this.right, this.bottom);
    this.UpdateArrangeRect(availableSize);
    Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), availableSize), this.Area.AxisThickness);
    this.currentAvalilableSize = availableSize;
    this.Area.SeriesClipRect = rect;
    foreach (ChartSeriesBase chartSeriesBase in this.Area.VisibleSeries.OfType<ISupportAxes>())
    {
      if (chartSeriesBase.ActualXAxis != null && chartSeriesBase.ActualYAxis != null)
      {
        ChartAxis chartAxis1 = chartSeriesBase.ActualXAxis.Orientation == Orientation.Horizontal ? chartSeriesBase.ActualXAxis : chartSeriesBase.ActualYAxis;
        ChartAxis chartAxis2 = chartSeriesBase.ActualYAxis.Orientation == Orientation.Vertical ? chartSeriesBase.ActualYAxis : chartSeriesBase.ActualXAxis;
        double x = chartAxis1.ArrangeRect.Left - rect.Left;
        double y = chartAxis2.ArrangeRect.Top - rect.Top;
        double width = chartAxis1.ArrangeRect.Width;
        double height = chartAxis2.ArrangeRect.Height;
        chartSeriesBase.Clip = (Geometry) new RectangleGeometry()
        {
          Rect = new Rect(x, y, width + 0.5, height + 0.5)
        };
      }
    }
    this._desiredSize = availableSize;
    return availableSize;
  }

  public void DetachElements()
  {
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Area.Axes)
    {
      if (ax.GridLinesRecycler != null)
        ax.GridLinesRecycler.Clear();
      if (ax.MinorGridLinesRecycler != null)
        ax.MinorGridLinesRecycler.Clear();
    }
    this.panel.Children.Clear();
    this.panel = (Panel) null;
  }

  public Size Arrange(Size finalSize)
  {
    this.ArrangeAxes();
    return finalSize;
  }

  public void UpdateElements()
  {
    List<UIElement> uiElementList = new List<UIElement>();
    if (this.Children == null)
      return;
    foreach (UIElement child in this.Children)
    {
      if (child is ChartAxis chartAxis && !this.Area.Axes.Contains(chartAxis))
      {
        if (chartAxis.GridLinesRecycler != null)
          chartAxis.GridLinesRecycler.Clear();
        if (chartAxis.MinorGridLinesRecycler != null)
          chartAxis.MinorGridLinesRecycler.Clear();
        uiElementList.Add((UIElement) chartAxis);
      }
    }
    foreach (UIElement element in uiElementList)
      this.panel.Children.Remove(element);
    uiElementList.Clear();
    List<UIElement> children = this.Children;
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Area.Axes)
    {
      ax.AxisLayoutPanel = (ILayoutCalculator) null;
      if (!children.Contains((UIElement) ax))
        this.panel.Children.Add((UIElement) ax);
      if (ax is ChartAxisBase3D axis3D)
        axis3D.IsManhattanAxis = ChartCartesianAxisLayoutPanel.IsDeclaredSeriesManhattan(ax.Area.VisibleSeries, axis3D);
    }
  }

  internal void UpdateLegendsArrangeRect()
  {
    this.leftLegendSizes.Clear();
    this.rightLegendSizes.Clear();
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.Area.RowDefinitions)
    {
      Size size = new Size(this.currentAvalilableSize.Width, rowDefinition.ComputedHeight);
      rowDefinition.MeasureLegends(size, this.leftLegendSizes, this.rightLegendSizes);
    }
    this.topLegendSizes.Clear();
    this.bottomLegendSizes.Clear();
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.Area.ColumnDefinitions)
    {
      Size size = new Size(columnDefinition.ComputedWidth, this.currentAvalilableSize.Height);
      columnDefinition.MeasureLegends(size, this.bottomLegendSizes, this.topLegendSizes);
    }
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.Area.RowDefinitions)
      rowDefinition.UpdateLegendArrangeRect(rowDefinition.ComputedTop, rowDefinition.ComputedHeight, this.Area.RootPanelDesiredSize.HasValue ? this.Area.RootPanelDesiredSize.Value.Width : 0.0, this.leftLegendSizes, this.rightLegendSizes);
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.Area.ColumnDefinitions)
      columnDefinition.UpdateLegendsArrangeRect(columnDefinition.ComputedLeft, columnDefinition.ComputedWidth, this.Area.RootPanelDesiredSize.HasValue ? this.Area.RootPanelDesiredSize.Value.Height : 0.0, this.bottomLegendSizes, this.topLegendSizes);
  }

  private static bool IsDeclaredSeriesManhattan(
    ChartVisibleSeriesCollection visibleSeries,
    ChartAxisBase3D axis3D)
  {
    return axis3D.IsZAxis && visibleSeries.Count > 0 && visibleSeries.All<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series is AreaSeries3D || series is LineSeries3D));
  }

  private bool PreventAxisAddition(ChartAxisBase3D axis3D)
  {
    return axis3D.IsManhattanAxis && this.Area.VisibleSeries != null && this.Area.VisibleSeries.Count == 1;
  }

  private void AxisSpanCalculation()
  {
    using (IEnumerator<ChartAxis> enumerator = this.Area.ColumnDefinitions.SelectMany((Func<ChartColumnDefinition, IEnumerable<ChartAxis>>) (column => (IEnumerable<ChartAxis>) column.Axis), (column, axis) => new
    {
      column = column,
      axis = axis
    }).Where(_param1 => this.Area.GetActualColumnSpan((UIElement) _param1.axis) > 1 && this.Area.GetActualColumn((UIElement) _param1.axis) == this.Area.ColumnDefinitions.IndexOf(_param1.column)).Select(_param0 => _param0.axis).DefaultIfEmpty<ChartAxis>().GetEnumerator())
    {
label_8:
      while (enumerator.MoveNext())
      {
        ChartAxis current = enumerator.Current;
        if (current != null)
        {
          int actualColumnSpan = this.Area.GetActualColumnSpan((UIElement) current);
          int actualColumn = this.Area.GetActualColumn((UIElement) current);
          int index1 = this.Area.ColumnDefinitions[actualColumn].Axis.IndexOf(current);
          int num = 1;
          int index2 = actualColumn + 1;
          while (true)
          {
            if (num < actualColumnSpan && index2 < this.Area.ColumnDefinitions.Count)
            {
              if (this.Area.ColumnDefinitions[index2].Axis.Count > index1)
                this.Area.ColumnDefinitions[index2].Axis.Insert(index1, current);
              ++num;
              ++index2;
            }
            else
              goto label_8;
          }
        }
        else
          break;
      }
    }
    using (IEnumerator<ChartAxis> enumerator = this.Area.RowDefinitions.SelectMany((Func<ChartRowDefinition, IEnumerable<ChartAxis>>) (row => (IEnumerable<ChartAxis>) row.Axis), (row, axis) => new
    {
      row = row,
      axis = axis
    }).Where(_param1 => this.Area.GetActualRowSpan((UIElement) _param1.axis) > 1 && this.Area.GetActualRow((UIElement) _param1.axis) == this.Area.RowDefinitions.IndexOf(_param1.row)).Select(_param0 => _param0.axis).DefaultIfEmpty<ChartAxis>().GetEnumerator())
    {
label_21:
      while (enumerator.MoveNext())
      {
        ChartAxis current = enumerator.Current;
        if (current == null)
          break;
        int actualRowSpan = this.Area.GetActualRowSpan((UIElement) current);
        int actualRow = this.Area.GetActualRow((UIElement) current);
        int index3 = this.Area.RowDefinitions[actualRow].Axis.IndexOf(current);
        int num = 1;
        int index4 = actualRow + 1;
        while (true)
        {
          if (num < actualRowSpan && index4 < this.Area.RowDefinitions.Count)
          {
            if (this.Area.RowDefinitions[index4].Axis.Count > index3)
              this.Area.RowDefinitions[index4].Axis.Insert(index3, current);
            ++num;
            ++index4;
          }
          else
            goto label_21;
        }
      }
    }
  }

  private void MeasureAxis(Size availableSize, Rect seriesClipRect)
  {
    bool flag = true;
    bool isFirstLayout = true;
    while (flag)
    {
      flag = false;
      this.leftSizes.Clear();
      this.rightSizes.Clear();
      this.CalcRowSize(seriesClipRect);
      foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.Area.RowDefinitions)
      {
        Size size = new Size(availableSize.Width, rowDefinition.ComputedHeight);
        rowDefinition.Measure(size, this.leftSizes, this.rightSizes, isFirstLayout);
      }
      this.left = this.leftSizes.Sum();
      this.right = this.rightSizes.Sum();
      this.top = this.topSizes.Count > 0 ? this.topSizes.Sum() : 0.0;
      this.bottom = this.bottomSizes.Count > 0 ? this.bottomSizes.Sum() : 0.0;
      Thickness thickness1 = new Thickness().GetThickness(this.left, this.top, this.right, this.bottom);
      Rect rect1 = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), availableSize), thickness1);
      if (Math.Abs(seriesClipRect.Width - rect1.Width) > 0.5 || isFirstLayout)
      {
        this.topSizes.Clear();
        this.bottomSizes.Clear();
        seriesClipRect = rect1;
        this.CalcColumnSize(seriesClipRect);
        foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.Area.ColumnDefinitions)
        {
          Size size = new Size(columnDefinition.ComputedWidth, availableSize.Height);
          columnDefinition.Measure(size, this.bottomSizes, this.topSizes);
          if (this.Area is SfChart3D)
          {
            if (columnDefinition.Axis.Any<ChartAxis>((Func<ChartAxis, bool>) (x => (x as ChartAxisBase3D).IsZAxis && !(x as ChartAxisBase3D).IsManhattanAxis)))
              (this.Area as SfChart3D).ActualDepth = columnDefinition.ComputedWidth;
            else
              (this.Area as SfChart3D).ActualDepth = (this.Area as SfChart3D).Depth;
          }
        }
        this.left = this.leftSizes.Sum();
        this.right = this.rightSizes.Sum();
        this.top = this.topSizes.Sum();
        this.bottom = this.bottomSizes.Sum();
        Thickness thickness2 = new Thickness().GetThickness(this.left, this.top, this.right, this.bottom);
        Rect rect2 = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), availableSize), thickness2);
        flag = Math.Abs(seriesClipRect.Height - rect2.Height) > 0.5;
        seriesClipRect = rect2;
      }
      isFirstLayout = false;
    }
  }

  private void CalcRowSize(Rect rect)
  {
    double top = rect.Top;
    double num1 = 0.0;
    double num2 = this.Area.RowDefinitions.Sum<ChartRowDefinition>((Func<ChartRowDefinition, double>) (rowDef => rowDef.Unit == ChartUnitType.Star ? rowDef.Height : 0.0));
    double num3 = this.Area.RowDefinitions.Sum<ChartRowDefinition>((Func<ChartRowDefinition, double>) (rowDef => rowDef.Unit == ChartUnitType.Pixels ? rowDef.Height : 0.0));
    double num4 = Math.Max(0.0, rect.Height - num3) / num2;
    for (int index = this.Area.RowDefinitions.Count - 1; index >= 0; --index)
    {
      ChartRowDefinition rowDefinition = this.Area.RowDefinitions[index];
      double val1 = rect.Height - num1;
      double d = rowDefinition.Unit != ChartUnitType.Star ? Math.Min(val1, rowDefinition.Height) : Math.Min(val1, rowDefinition.Height * num4);
      rowDefinition.ComputedHeight = d;
      rowDefinition.ComputedTop = top;
      num1 += double.IsNaN(d) ? 1.0 : d;
      rowDefinition.RowTop = top;
      top += double.IsNaN(d) ? 1.0 : d;
    }
  }

  private void CalcColumnSize(Rect rect)
  {
    double left = rect.Left;
    double num1 = 0.0;
    double num2 = this.Area.ColumnDefinitions.Sum<ChartColumnDefinition>((Func<ChartColumnDefinition, double>) (columnDef => columnDef.Unit == ChartUnitType.Star ? columnDef.Width : 0.0));
    double num3 = this.Area.ColumnDefinitions.Sum<ChartColumnDefinition>((Func<ChartColumnDefinition, double>) (columnDef => columnDef.Unit == ChartUnitType.Pixels ? columnDef.Width : 0.0));
    double num4 = Math.Max(0.0, rect.Width - num3) / num2;
    for (int index = 0; index < this.Area.ColumnDefinitions.Count; ++index)
    {
      ChartColumnDefinition columnDefinition = this.Area.ColumnDefinitions[index];
      double val1 = rect.Width - num1;
      double num5 = columnDefinition.Unit != ChartUnitType.Star ? Math.Min(val1, columnDefinition.Width) : Math.Min(val1, columnDefinition.Width * num4);
      columnDefinition.ComputedWidth = num5;
      columnDefinition.ComputedLeft = left;
      num1 += num5;
      left += num5;
    }
  }

  private void UpdateArrangeRect(Size availableSize)
  {
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.Area.RowDefinitions)
      rowDefinition.UpdateArrangeRect(rowDefinition.ComputedTop, rowDefinition.ComputedHeight, availableSize.Width, this.leftSizes, this.rightSizes);
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.Area.ColumnDefinitions)
      columnDefinition.UpdateArrangeRect(columnDefinition.ComputedLeft, columnDefinition.ComputedWidth, availableSize.Height, this.bottomSizes, this.topSizes);
  }

  private void ArrangeAxes()
  {
    if (this.Area.InternalPrimaryAxis == null || this.Area.InternalSecondaryAxis == null)
      return;
    foreach (ChartColumnDefinition columnDefinition in (Collection<ChartColumnDefinition>) this.Area.ColumnDefinitions)
      columnDefinition.Arrange();
    foreach (ChartRowDefinition rowDefinition in (Collection<ChartRowDefinition>) this.Area.RowDefinitions)
      rowDefinition.Arrange();
  }
}
