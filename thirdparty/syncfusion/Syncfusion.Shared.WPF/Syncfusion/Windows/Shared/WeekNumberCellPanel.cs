// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.WeekNumberCellPanel
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class WeekNumberCellPanel : Cell
{
  public static readonly DependencyProperty WeekNumberProperty = DependencyProperty.Register(nameof (WeekNumber), typeof (string), typeof (WeekNumberCellPanel));

  static WeekNumberCellPanel()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (WeekNumberCellPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (WeekNumberCellPanel)));
    UIElement.FocusableProperty.OverrideMetadata(typeof (WeekNumberCellPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    Border.CornerRadiusProperty.AddOwner(typeof (WeekNumberCellPanel));
  }

  public string WeekNumber
  {
    get => (string) this.GetValue(WeekNumberCellPanel.WeekNumberProperty);
    set => this.SetValue(WeekNumberCellPanel.WeekNumberProperty, (object) value);
  }
}
