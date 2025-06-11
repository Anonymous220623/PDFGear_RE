// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartDragTooltipStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartDragTooltipStyle : DependencyObject
{
  public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof (FontFamily), typeof (FontFamily), typeof (ChartDragTooltipStyle), new PropertyMetadata(TextBlock.FontFamilyProperty.GetMetadata(typeof (TextBlock)).DefaultValue));
  public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof (FontSize), typeof (double), typeof (ChartDragTooltipStyle), new PropertyMetadata((object) 20.0));
  public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register(nameof (FontStyle), typeof (FontStyle), typeof (ChartDragTooltipStyle), new PropertyMetadata(TextBlock.FontStyleProperty.GetMetadata(typeof (TextBlock)).DefaultValue));
  public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (ChartDragTooltipStyle), new PropertyMetadata((object) new SolidColorBrush(Colors.White)));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (ChartDragTooltipStyle), new PropertyMetadata((PropertyChangedCallback) null));

  public FontFamily FontFamily
  {
    get => (FontFamily) this.GetValue(ChartDragTooltipStyle.FontFamilyProperty);
    set => this.SetValue(ChartDragTooltipStyle.FontFamilyProperty, (object) value);
  }

  public double FontSize
  {
    get => (double) this.GetValue(ChartDragTooltipStyle.FontSizeProperty);
    set => this.SetValue(ChartDragTooltipStyle.FontSizeProperty, (object) value);
  }

  public FontStyle FontStyle
  {
    get => (FontStyle) this.GetValue(ChartDragTooltipStyle.FontStyleProperty);
    set => this.SetValue(ChartDragTooltipStyle.FontStyleProperty, (object) value);
  }

  public Brush Foreground
  {
    get => (Brush) this.GetValue(ChartDragTooltipStyle.ForegroundProperty);
    set => this.SetValue(ChartDragTooltipStyle.ForegroundProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(ChartDragTooltipStyle.BackgroundProperty);
    set => this.SetValue(ChartDragTooltipStyle.BackgroundProperty, (object) value);
  }
}
