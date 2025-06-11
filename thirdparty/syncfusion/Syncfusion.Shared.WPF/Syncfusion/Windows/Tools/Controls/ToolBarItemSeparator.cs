// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarItemSeparator
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class ToolBarItemSeparator : Control
{
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (System.Windows.Controls.Orientation), typeof (ToolBarItemSeparator), new PropertyMetadata((object) System.Windows.Controls.Orientation.Horizontal));

  public ToolBarItemSeparator() => this.DefaultStyleKey = (object) typeof (ToolBarItemSeparator);

  public System.Windows.Controls.Orientation Orientation
  {
    get => (System.Windows.Controls.Orientation) this.GetValue(ToolBarItemSeparator.OrientationProperty);
    set => this.SetValue(ToolBarItemSeparator.OrientationProperty, (object) value);
  }
}
