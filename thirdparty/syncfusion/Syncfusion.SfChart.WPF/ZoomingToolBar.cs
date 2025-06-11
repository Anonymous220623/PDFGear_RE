// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ZoomingToolBar
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ZoomingToolBar : ChartToolBar
{
  private ChartZoomPanBehavior behavior;

  public ZoomingToolBar()
  {
    this.DefaultStyleKey = (object) typeof (ZoomingToolBar);
    this.Loaded += new RoutedEventHandler(this.ZoomingToolBar_Loaded);
  }

  internal ChartZoomPanBehavior ZoomBehavior
  {
    get => this.behavior;
    set => this.behavior = value;
  }

  public void ChangeBackground()
  {
    if (this.ZoomBehavior.ToolBarBackground == null)
      return;
    this.Background = (Brush) this.ZoomBehavior.ToolBarBackground;
  }

  internal void Dispose()
  {
    this.behavior = (ChartZoomPanBehavior) null;
    if (!(this.ItemsSource is List<ZoomingToolBarItem> itemsSource))
      return;
    foreach (ZoomingToolBarItem zoomingToolBarItem in itemsSource)
      zoomingToolBarItem.Dispose();
    itemsSource.Clear();
    this.ItemsSource = (IEnumerable) null;
  }

  internal void ChangedOrientation()
  {
    ItemsPresenter visualChild = ChartLayoutUtils.GetVisualChild<ItemsPresenter>((DependencyObject) this);
    if (visualChild == null || VisualTreeHelper.GetChildrenCount((DependencyObject) visualChild) <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) visualChild, 0) is StackPanel child))
      return;
    child.Orientation = this.ZoomBehavior.ToolBarOrientation;
    this.UpdateLayout();
    this.ZoomBehavior.OnLayoutUpdated();
  }

  protected internal void SetItemSource()
  {
    string[] strArray = new string[4]
    {
      "ZoomIn",
      "ZoomOut",
      "Reset",
      "SelectZooming"
    };
    List<ZoomingToolBarItem> zoomingToolBarItemList1 = new List<ZoomingToolBarItem>();
    IEnumerable<string> source = ((IEnumerable<string>) this.ZoomBehavior.ToolBarItems.ToString().Split(',')).Select<string, string>((Func<string, string>) (item => item.Trim()));
    if (source.Any<string>((Func<string, bool>) (item => item.Equals("All"))))
      source = (IEnumerable<string>) strArray;
    foreach (string str in source)
    {
      switch (str)
      {
        case "ZoomIn":
          List<ZoomingToolBarItem> zoomingToolBarItemList2 = zoomingToolBarItemList1;
          ZoomIn zoomIn1 = new ZoomIn();
          zoomIn1.Source = this.ZoomBehavior;
          ZoomIn zoomIn2 = zoomIn1;
          zoomingToolBarItemList2.Add((ZoomingToolBarItem) zoomIn2);
          continue;
        case "ZoomOut":
          List<ZoomingToolBarItem> zoomingToolBarItemList3 = zoomingToolBarItemList1;
          ZoomOut zoomOut1 = new ZoomOut();
          zoomOut1.Source = this.ZoomBehavior;
          zoomOut1.IsEnabled = false;
          ZoomOut zoomOut2 = zoomOut1;
          zoomingToolBarItemList3.Add((ZoomingToolBarItem) zoomOut2);
          continue;
        case "Reset":
          List<ZoomingToolBarItem> zoomingToolBarItemList4 = zoomingToolBarItemList1;
          ZoomReset zoomReset1 = new ZoomReset();
          zoomReset1.Source = this.ZoomBehavior;
          zoomReset1.IsEnabled = false;
          ZoomReset zoomReset2 = zoomReset1;
          zoomingToolBarItemList4.Add((ZoomingToolBarItem) zoomReset2);
          continue;
        case "SelectZooming":
          if (this.ZoomBehavior.EnableSelectionZooming)
          {
            List<ZoomingToolBarItem> zoomingToolBarItemList5 = zoomingToolBarItemList1;
            ZoomPan zoomPan1 = new ZoomPan();
            zoomPan1.Source = this.ZoomBehavior;
            ZoomPan zoomPan2 = zoomPan1;
            zoomingToolBarItemList5.Add((ZoomingToolBarItem) zoomPan2);
            List<ZoomingToolBarItem> zoomingToolBarItemList6 = zoomingToolBarItemList1;
            SelectionZoom selectionZoom1 = new SelectionZoom();
            selectionZoom1.Source = this.ZoomBehavior;
            SelectionZoom selectionZoom2 = selectionZoom1;
            zoomingToolBarItemList6.Add((ZoomingToolBarItem) selectionZoom2);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    this.ItemsSource = (IEnumerable) zoomingToolBarItemList1;
    this.ChangedOrientation();
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => e.Handled = true;

  private void ZoomingToolBar_Loaded(object sender, RoutedEventArgs e)
  {
    this.ChangeBackground();
    this.SetItemSource();
    if (this.ZoomBehavior.ChartArea == null || this.ZoomBehavior.ChartArea.Axes.Count <= 0)
      return;
    foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.ZoomBehavior.ChartArea.Axes)
    {
      if (ax.ZoomFactor < 1.0 || ax.ZoomPosition > 0.0)
        this.ZoomBehavior.ChartArea.ChangeToolBarState();
    }
  }
}
