// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ZoomIn
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ZoomIn : ZoomingToolBarItem
{
  public ZoomIn()
  {
    this.DataContext = (object) this;
    this.DefaultStyleKey = (object) typeof (ZoomIn);
    this.Tag = (object) nameof (ZoomIn);
    ToolTipService.SetToolTip((DependencyObject) this, (object) ChartLocalizationResourceAccessor.Instance.GetString(nameof (ZoomIn)));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.SetPressedState();
    if (this.Source.ChartArea.AreaType != ChartAreaType.CartesianAxes)
      return;
    if (this.Source.ChartArea != null)
    {
      bool flag = false;
      foreach (ChartAxisBase2D ax in (Collection<ChartAxis>) this.Source.ChartArea.Axes)
      {
        if (ax.RegisteredSeries.Count > 0 && ax.RegisteredSeries[0] is CartesianSeries && (ax.RegisteredSeries[0] as CartesianSeries).IsActualTransposed)
        {
          if (ax.Orientation.Equals((object) Orientation.Horizontal) && (this.Source.ZoomMode == ZoomMode.Y || this.Source.ZoomMode == ZoomMode.XY) || ax.Orientation.Equals((object) Orientation.Vertical) && (this.Source.ZoomMode == ZoomMode.X || this.Source.ZoomMode == ZoomMode.XY))
            flag = true;
        }
        else if (ax.Orientation.Equals((object) Orientation.Vertical) && (this.Source.ZoomMode == ZoomMode.Y || this.Source.ZoomMode == ZoomMode.XY) || ax.Orientation.Equals((object) Orientation.Horizontal) && (this.Source.ZoomMode == ZoomMode.X || this.Source.ZoomMode == ZoomMode.XY))
          flag = true;
        if (flag)
        {
          double num = 0.5;
          this.Source.Zoom(Math.Max(Math.Max(1.0 / ChartMath.MinMax(ax.ZoomFactor, 0.0, 1.0), 1.0) + 0.25, 1.0), num > 1.0 ? 1.0 : (num < 0.0 ? 0.0 : num), ax);
        }
        flag = false;
      }
    }
    e.Handled = true;
  }
}
