// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ScrollBarVisibilityToVisibilityConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class ScrollBarVisibilityToVisibilityConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if ((ScrollBarVisibility) values[0] == ScrollBarVisibility.Visible)
      return (object) Visibility.Visible;
    if ((ScrollBarVisibility) values[0] == ScrollBarVisibility.Auto)
      return (object) (Visibility) ((int) values[1] > 1 ? 0 : 1);
    return (ScrollBarVisibility) values[0] == ScrollBarVisibility.Hidden ? (object) Visibility.Hidden : (object) Visibility.Collapsed;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
