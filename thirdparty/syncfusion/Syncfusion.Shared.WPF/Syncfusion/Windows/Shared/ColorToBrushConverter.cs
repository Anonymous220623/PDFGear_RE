// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ColorToBrushConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[ValueConversion(typeof (Color), typeof (Brush))]
public class ColorToBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) null;
    return value is Color color ? (object) new SolidColorBrush(color) : throw new ArgumentException("Value of Color type is expected.", nameof (value));
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) null;
    return value is SolidColorBrush ? (object) ((SolidColorBrush) value).Color : throw new ArgumentException("Value of SolidColorBrush type is expected.", nameof (value));
  }
}
