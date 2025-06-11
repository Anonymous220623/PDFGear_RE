// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ThumbStyle
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ThumbStyle : DependencyObject
{
  public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(nameof (LineStyle), typeof (Style), typeof (ThumbStyle), new PropertyMetadata((object) null, new PropertyChangedCallback(ThumbStyle.OnPropertyChanged)));
  public static readonly DependencyProperty SymbolTemplateProperty = DependencyProperty.Register(nameof (SymbolTemplate), typeof (DataTemplate), typeof (ThumbStyle), new PropertyMetadata((object) null, new PropertyChangedCallback(ThumbStyle.OnPropertyChanged)));

  public Style LineStyle
  {
    get => (Style) this.GetValue(ThumbStyle.LineStyleProperty);
    set => this.SetValue(ThumbStyle.LineStyleProperty, (object) value);
  }

  public DataTemplate SymbolTemplate
  {
    get => (DataTemplate) this.GetValue(ThumbStyle.SymbolTemplateProperty);
    set => this.SetValue(ThumbStyle.SymbolTemplateProperty, (object) value);
  }

  internal SfDateTimeRangeNavigator Navigator { get; set; }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ThumbStyle thumbStyle = d as ThumbStyle;
    if (thumbStyle.Navigator == null)
      return;
    thumbStyle.Navigator.SetThumbStyle();
  }
}
