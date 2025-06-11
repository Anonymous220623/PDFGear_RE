// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SelectionZoom
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SelectionZoom : ZoomingToolBarItem
{
  public SelectionZoom()
  {
    this.DataContext = (object) this;
    this.DefaultStyleKey = (object) typeof (SelectionZoom);
    this.Tag = (object) "Selection";
    ToolTipService.SetToolTip((DependencyObject) this, (object) ChartLocalizationResourceAccessor.Instance.GetString("BoxSelectionZoom"));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.SetPressedState();
    if (!this.Source.IsActive)
    {
      this.Source.InternalEnableSelectionZooming = false;
      foreach (ZoomingToolBarItem zoomingToolBarItem in (IEnumerable) this.Source.ChartZoomingToolBar.Items)
      {
        if (zoomingToolBarItem is ZoomPan)
        {
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.EnableColor;
          this.Source.EnablePanning = true;
        }
        if (zoomingToolBarItem is SelectionZoom)
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.DisableColor;
      }
      this.Source.IsActive = true;
    }
    else
    {
      this.Source.InternalEnableSelectionZooming = true;
      foreach (ZoomingToolBarItem zoomingToolBarItem in (IEnumerable) this.Source.ChartZoomingToolBar.Items)
      {
        if (zoomingToolBarItem is ZoomPan)
        {
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.DisableColor;
          this.Source.EnablePanning = false;
        }
        if (zoomingToolBarItem is SelectionZoom)
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.EnableColor;
      }
      this.Source.IsActive = false;
    }
  }
}
