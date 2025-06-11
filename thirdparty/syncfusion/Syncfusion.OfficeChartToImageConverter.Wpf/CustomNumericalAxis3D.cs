// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChartToImageConverter.CustomNumericalAxis3D
// Assembly: Syncfusion.OfficeChartToImageConverter.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 82053128-0A33-4E43-8DD1-E8016B1463BC
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChartToImageConverter.Wpf.dll

using Syncfusion.UI.Xaml.Charts;
using System.Windows;

#nullable disable
namespace Syncfusion.OfficeChartToImageConverter;

internal class CustomNumericalAxis3D : NumericalAxis3D
{
  public static readonly DependencyProperty WidthExtProperty = DependencyProperty.Register(nameof (WidthExt), typeof (double), typeof (CustomNumericalAxis3D), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(CustomNumericalAxis3D.OnPropertyChanged)));
  public static readonly DependencyProperty HeightExtProperty = DependencyProperty.Register(nameof (HeightExt), typeof (double), typeof (CustomNumericalAxis3D), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(CustomNumericalAxis3D.OnPropertyChanged)));

  public double WidthExt
  {
    get => (double) this.GetValue(CustomNumericalAxis3D.WidthExtProperty);
    set => this.SetValue(CustomNumericalAxis3D.WidthExtProperty, (object) value);
  }

  public double HeightExt
  {
    get => (double) this.GetValue(CustomNumericalAxis3D.HeightExtProperty);
    set => this.SetValue(CustomNumericalAxis3D.HeightExtProperty, (object) value);
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
