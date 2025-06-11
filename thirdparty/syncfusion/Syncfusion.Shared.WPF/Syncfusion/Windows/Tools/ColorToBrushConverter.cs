// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.ColorToBrushConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools;

[DesignTimeVisible(false)]
[ValueConversion(typeof (Color), typeof (Brush))]
public class ColorToBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) null;
    if (value == DependencyProperty.UnsetValue)
      return Binding.DoNothing;
    if (targetType != typeof (Brush))
      throw new InvalidOperationException("Only color to brush conversion is supported by ColorToBrushConverter converter.");
    return (object) new SolidColorBrush((Color) value);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new Exception("The method or operation is not implemented.");
  }
}
