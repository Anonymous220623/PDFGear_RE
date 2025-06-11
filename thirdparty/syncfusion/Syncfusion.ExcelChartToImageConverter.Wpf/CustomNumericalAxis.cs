// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelChartToImageConverter.CustomNumericalAxis
// Assembly: Syncfusion.ExcelChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 8A92A829-7139-4C93-8632-144655877EB3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System.Windows;

#nullable disable
namespace Syncfusion.ExcelChartToImageConverter;

internal class CustomNumericalAxis : NumericalAxis
{
  public static readonly DependencyProperty WidthExtProperty = DependencyProperty.Register(nameof (WidthExt), typeof (double), typeof (CustomNumericalAxis), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(CustomNumericalAxis.OnPropertyChanged)));
  public static readonly DependencyProperty HeightExtProperty = DependencyProperty.Register(nameof (HeightExt), typeof (double), typeof (CustomNumericalAxis), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(CustomNumericalAxis.OnPropertyChanged)));

  public double WidthExt
  {
    get => (double) this.GetValue(CustomNumericalAxis.WidthExtProperty);
    set => this.SetValue(CustomNumericalAxis.WidthExtProperty, (object) value);
  }

  public double HeightExt
  {
    get => (double) this.GetValue(CustomNumericalAxis.HeightExtProperty);
    set => this.SetValue(CustomNumericalAxis.HeightExtProperty, (object) value);
  }

  private static void OnPropertyChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
  }

  protected override void CalculateVisibleRange(Size avalableSize)
  {
    base.CalculateVisibleRange(avalableSize);
    this.HeightExt = avalableSize.Height * 0.98;
    this.WidthExt = avalableSize.Width * 0.98;
  }
}
