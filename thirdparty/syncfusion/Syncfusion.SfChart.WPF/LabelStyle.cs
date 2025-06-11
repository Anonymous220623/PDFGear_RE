// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LabelStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LabelStyle : DependencyObject
{
  public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof (FontFamily), typeof (FontFamily), typeof (LabelStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (SolidColorBrush), typeof (LabelStyle), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (LabelStyle), new PropertyMetadata((PropertyChangedCallback) null));

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(LabelStyle.FontFamilyProperty);
    set => this.SetValue(LabelStyle.FontFamilyProperty, (object) value);
  }

  public SolidColorBrush Foreground
  {
    get => (SolidColorBrush) this.GetValue(LabelStyle.ForegroundProperty);
    set => this.SetValue(LabelStyle.ForegroundProperty, (object) value);
  }

  public double FontSize
  {
    get => (double) this.GetValue(LabelStyle.FontSizeProperty);
    set => this.SetValue(LabelStyle.FontSizeProperty, (object) value);
  }
}
