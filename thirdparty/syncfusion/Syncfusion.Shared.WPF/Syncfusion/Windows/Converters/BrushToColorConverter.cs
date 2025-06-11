// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Converters.BrushToColorConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Converters;

public class BrushToColorConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo language)
  {
    if (!(value is SolidColorBrush solidColorBrush) && parameter != null && parameter.ToString() == "AccentBrushnull")
      return (object) new SolidColorBrush(Colors.Transparent);
    if (solidColorBrush == null && parameter != null && parameter.ToString() == "ContentBrushnull")
      return (object) Colors.Black;
    if (solidColorBrush != null && parameter != null && parameter.ToString() == "ContentBrushnull")
      return (object) solidColorBrush.Color;
    if (solidColorBrush != null && parameter != null && parameter.ToString() == "AccentBrushnull")
      return (object) new SolidColorBrush(Color.FromArgb(solidColorBrush.Color.A, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B));
    return solidColorBrush == null ? (object) Colors.Transparent : (object) Color.FromArgb(solidColorBrush.Color.A, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
  {
    return value != null ? (object) (Brush) new BrushConverter().ConvertFromString(value.ToString()) : (object) null;
  }
}
