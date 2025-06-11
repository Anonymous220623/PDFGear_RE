// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.TextBlockExt
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class TextBlockExt : TextBlock
{
  public static readonly DependencyProperty InlineListProperty = DependencyProperty.Register(nameof (InlineList), typeof (ObservableCollection<Inline>), typeof (TextBlockExt), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(TextBlockExt.OnPropertyChanged)));

  public ObservableCollection<Inline> InlineList
  {
    get => (ObservableCollection<Inline>) this.GetValue(TextBlockExt.InlineListProperty);
    set => this.SetValue(TextBlockExt.InlineListProperty, (object) value);
  }

  private static void OnPropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(sender is TextBlockExt))
      return;
    ((TextBlock) sender).Inlines.AddRange((IEnumerable) e.NewValue);
  }
}
