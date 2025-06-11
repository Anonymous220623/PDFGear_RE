// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeNavigatorSelector
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class RangeNavigatorSelector : ResizableScrollBar
{
  public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register(nameof (OverlayBrush), typeof (Brush), typeof (ResizableScrollBar), new PropertyMetadata((object) new SolidColorBrush(Colors.Transparent)));

  public Brush OverlayBrush
  {
    get => (Brush) this.GetValue(RangeNavigatorSelector.OverlayBrushProperty);
    set => this.SetValue(RangeNavigatorSelector.OverlayBrushProperty, (object) value);
  }
}
