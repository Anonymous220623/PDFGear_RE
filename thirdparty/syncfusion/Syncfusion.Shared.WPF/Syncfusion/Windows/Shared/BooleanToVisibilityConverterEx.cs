// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.BooleanToVisibilityConverterEx
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class BooleanToVisibilityConverterEx : IValueConverter
{
  private string direction = "inverse";

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    flag = false;
    if (!(value is bool flag) && value is bool? nullable)
      flag = nullable.HasValue && nullable.Value;
    if (TypeConverterHelper.ChanngeTypeToBool(parameter))
      flag = !flag;
    else if (parameter is string && this.direction == (string) parameter)
      flag = !flag;
    return (object) (Visibility) (flag ? 0 : 2);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    bool flag = false;
    if (value is Visibility visibility)
      flag = visibility == Visibility.Visible;
    if (TypeConverterHelper.ChanngeTypeToBool(parameter))
      flag = !flag;
    else if (parameter is string && this.direction == (string) parameter)
      flag = !flag;
    return (object) flag;
  }
}
