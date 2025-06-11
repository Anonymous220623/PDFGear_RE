// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisBase3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartAxisBase3D : ChartAxis
{
  private ChartCartesianAxisPanel3D axisPanel;

  public ChartAxisBase3D()
  {
    this.IsZAxis = false;
    this.IsManhattanAxis = false;
    this.DefaultStyleKey = (object) typeof (ChartAxisBase3D);
  }

  internal bool IsZAxis { get; set; }

  internal bool IsManhattanAxis { get; set; }

  internal AxisPosition3D AxisPosition3D { get; set; }

  internal double AxisDepth { get; set; }

  public override double CoefficientToValue(double value)
  {
    value = this.IsInversed ? 1.0 - value : value;
    return this.VisibleRange.Start + this.VisibleRange.Delta * value;
  }

  internal override void Dispose()
  {
    if (this.AssociatedAxes != null)
      this.AssociatedAxes.Clear();
    this.DisposePanels();
    this.DisposeEvents();
    this.LabelsSource = (object) null;
    this.Area = (ChartBase) null;
    this.PrefixLabelTemplate = (DataTemplate) null;
    this.PostfixLabelTemplate = (DataTemplate) null;
    this.LabelTemplate = (DataTemplate) null;
    base.Dispose();
  }

  private void DisposeEvents()
  {
    if (this.RegisteredSeries != null)
    {
      this.RegisteredSeries.CollectionChanged -= new NotifyCollectionChangedEventHandler(((ChartAxis) this).OnRegisteredSeriesCollectionChanged);
      this.RegisteredSeries.Clear();
    }
    if (this.ValueToCoefficientCalc != null)
    {
      foreach (Delegate invocation in this.ValueToCoefficientCalc.GetInvocationList())
      {
        ChartAxisBase3D chartAxisBase3D = this;
        chartAxisBase3D.ValueToCoefficientCalc = chartAxisBase3D.ValueToCoefficientCalc - (invocation as ChartAxis.ValueToCoefficientHandler);
      }
      this.ValueToCoefficientCalc = (ChartAxis.ValueToCoefficientHandler) null;
    }
    if (this.CoefficientToValueCalc == null)
      return;
    foreach (Delegate invocation in this.CoefficientToValueCalc.GetInvocationList())
    {
      ChartAxisBase3D chartAxisBase3D = this;
      chartAxisBase3D.CoefficientToValueCalc = chartAxisBase3D.CoefficientToValueCalc - (invocation as ChartAxis.ValueToCoefficientHandler);
    }
    this.CoefficientToValueCalc = (ChartAxis.ValueToCoefficientHandler) null;
  }

  internal void DisposePanels()
  {
    if (this.axisPanel != null)
    {
      if (this.axisPanel.LayoutCalc != null)
        this.axisPanel.LayoutCalc.Clear();
      this.axisPanel.Axis = (ChartAxisBase3D) null;
      this.axisPanel.HeaderContent = (FrameworkElement) null;
      this.axisPanel = (ChartCartesianAxisPanel3D) null;
    }
    if (this.axisLabelsPanel != null)
    {
      if (this.axisLabelsPanel is ChartCartesianAxisLabelsPanel axisLabelsPanel)
      {
        if (axisLabelsPanel.children != null)
          axisLabelsPanel.children.Clear();
        axisLabelsPanel.Dispose();
      }
      this.axisLabelsPanel = (ILayoutCalculator) null;
    }
    if (this.axisElementsPanel is ChartCartesianAxisElementsPanel axisElementsPanel)
    {
      if (this.axisElementsPanel.Children != null)
        this.axisElementsPanel.Children.Clear();
      axisElementsPanel.Dispose();
      this.axisElementsPanel = (ILayoutCalculator) null;
    }
    this.axisMultiLevelLabelsPanel?.Dispose();
    if (this.GridLinesRecycler != null)
    {
      foreach (Shape element in this.GridLinesRecycler)
        element.ClearUIValues();
      this.GridLinesRecycler.Clear();
      this.GridLinesRecycler = (UIElementsRecycler<Line>) null;
    }
    if (this.MinorGridLinesRecycler == null)
      return;
    foreach (Shape element in this.MinorGridLinesRecycler)
      element.ClearUIValues();
    this.MinorGridLinesRecycler.Clear();
    this.MinorGridLinesRecycler = (UIElementsRecycler<Line>) null;
  }

  internal override void CreateLineRecycler()
  {
    if (this.Area == null)
      return;
    this.GridLinesRecycler = new UIElementsRecycler<Line>((Panel) null);
    this.MinorGridLinesRecycler = new UIElementsRecycler<Line>((Panel) null);
  }

  internal override void ComputeDesiredSize(Size size)
  {
    this.AvailableSize = size;
    this.CalculateRangeAndInterval(size);
    if (this.Visibility != Visibility.Collapsed)
    {
      this.UpdatePanels();
      this.UpdateLabels();
      this.ComputedDesiredSize = this.axisPanel.ComputeSize(size);
    }
    else
    {
      this.ActualPlotOffset = this.PlotOffset;
      this.ActualPlotOffsetStart = this.PlotOffsetStart;
      this.ActualPlotOffsetEnd = this.PlotOffsetEnd;
      this.InsidePadding = 0.0;
      this.UpdateLabels();
      this.ComputedDesiredSize = this.Orientation == Orientation.Horizontal ? new Size(size.Width, 0.0) : new Size(0.0, size.Height);
    }
  }

  protected internal override void OnAxisBoundsChanged(ChartAxisBoundsEventArgs args)
  {
    base.OnAxisBoundsChanged(args);
    if (this.axisPanel == null)
      return;
    this.axisPanel.ArrangeElements(new Size(this.ArrangeRect.Width, this.ArrangeRect.Height));
  }

  private void UpdatePanels()
  {
    if (this.axisLabelsPanel != null)
      this.axisLabelsPanel.DetachElements();
    if (this.axisElementsPanel != null)
      this.axisElementsPanel.DetachElements();
    if (this.axisPanel != null)
      this.axisPanel.LayoutCalc.Clear();
    else
      this.axisPanel = new ChartCartesianAxisPanel3D()
      {
        Axis = this
      };
    this.axisLabelsPanel = (ILayoutCalculator) new ChartCartesianAxisLabelsPanel((Panel) null)
    {
      Axis = (ChartAxis) this
    };
    this.axisElementsPanel = (ILayoutCalculator) new ChartCartesianAxisElementsPanel((Panel) null)
    {
      Axis = (ChartAxis) this
    };
    if (this.headerContent == null)
    {
      ContentControl contentControl = new ContentControl();
      contentControl.Content = this.Header;
      contentControl.ContentTemplate = this.HeaderTemplate;
      contentControl.RenderTransformOrigin = new Point(0.5, 0.5);
      this.headerContent = contentControl;
      this.headerContent.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath("Visibility", new object[0])
      });
    }
    else
    {
      this.headerContent.Content = this.Header;
      this.headerContent.ContentTemplate = this.HeaderTemplate;
    }
    this.axisPanel.HeaderContent = (FrameworkElement) this.headerContent;
    if (this.HeaderTemplate == null && this.headerContent != null && this.HeaderStyle != null)
    {
      if (this.HeaderStyle.Foreground != null)
        this.headerContent.Foreground = (Brush) this.HeaderStyle.Foreground;
      if (this.HeaderStyle.FontSize != 0.0)
        this.headerContent.FontSize = this.HeaderStyle.FontSize;
      if (this.HeaderStyle.FontFamily != null)
        this.headerContent.FontFamily = this.HeaderStyle.FontFamily;
    }
    this.axisPanel.LayoutCalc.Add(this.axisLabelsPanel);
    this.axisPanel.LayoutCalc.Add(this.axisElementsPanel);
  }
}
