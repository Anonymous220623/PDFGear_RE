// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SymbolControl
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SymbolControl : Control
{
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (SymbolControl), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));

  public Brush Stroke
  {
    get => (Brush) this.GetValue(SymbolControl.StrokeProperty);
    set => this.SetValue(SymbolControl.StrokeProperty, (object) value);
  }
}
