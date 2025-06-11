// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ZoomPan
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ZoomPan : ZoomingToolBarItem
{
  public ZoomPan()
  {
    this.DataContext = (object) this;
    this.DefaultStyleKey = (object) typeof (ZoomPan);
    this.Tag = (object) "Panning";
    ToolTipService.SetToolTip((DependencyObject) this, (object) ChartLocalizationResourceAccessor.Instance.GetString("Pan"));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.SetPressedState();
    if (this.Source.IsActive)
    {
      this.Source.EnablePanning = false;
      foreach (ZoomingToolBarItem zoomingToolBarItem in (IEnumerable) this.Source.ChartZoomingToolBar.Items)
      {
        if (zoomingToolBarItem is ZoomPan)
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.DisableColor;
        else if (zoomingToolBarItem is SelectionZoom)
        {
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.EnableColor;
          this.Source.InternalEnableSelectionZooming = true;
        }
      }
      this.Source.IsActive = false;
    }
    else
    {
      this.Source.EnablePanning = true;
      foreach (ZoomingToolBarItem zoomingToolBarItem in (IEnumerable) this.Source.ChartZoomingToolBar.Items)
      {
        if (zoomingToolBarItem is ZoomPan)
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.EnableColor;
        if (zoomingToolBarItem is SelectionZoom)
        {
          zoomingToolBarItem.IconBackground = zoomingToolBarItem.DisableColor;
          this.Source.InternalEnableSelectionZooming = false;
        }
      }
      this.Source.IsActive = true;
    }
    e.Handled = true;
  }
}
