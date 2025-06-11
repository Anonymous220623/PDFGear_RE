// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.RandomBrushConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class RandomBrushConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo language)
  {
    return value == null || !(value is Brush) ? (object) new SolidColorBrush(Colors.Transparent) : (object) this.GetRandomBrush(value as Brush);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
  {
    throw new NotImplementedException();
  }

  private Brush GetRandomBrush(Brush brush)
  {
    switch (new Random().Next(2))
    {
      case 0:
        brush.Opacity = 1.0;
        break;
      case 1:
        brush.Opacity = 0.7;
        break;
      default:
        brush.Opacity = 0.3;
        break;
    }
    return brush;
  }
}
