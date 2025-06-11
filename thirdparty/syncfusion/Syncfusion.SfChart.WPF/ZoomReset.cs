// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ZoomReset
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ZoomReset : ZoomingToolBarItem
{
  public ZoomReset()
  {
    this.DataContext = (object) this;
    this.DefaultStyleKey = (object) typeof (ZoomReset);
    this.Tag = (object) "Reset";
    ToolTipService.SetToolTip((DependencyObject) this, (object) ChartLocalizationResourceAccessor.Instance.GetString("Reset"));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.SetPressedState();
    this.Source.Reset();
    e.Handled = true;
  }
}
