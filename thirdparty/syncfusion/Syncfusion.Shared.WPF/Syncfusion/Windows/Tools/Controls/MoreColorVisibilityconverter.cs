// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.MoreColorVisibilityconverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class MoreColorVisibilityconverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
      return (object) Visibility.Visible;
    Visibility visibility1 = values[0] != null ? (Visibility) values[0] : Visibility.Collapsed;
    Visibility visibility2 = values[1] != null ? (Visibility) values[1] : Visibility.Collapsed;
    if (visibility1 != Visibility.Visible && visibility2 != Visibility.Visible)
      return (object) Visibility.Collapsed;
    return values[2] != null && values[2] != DependencyProperty.UnsetValue ? values[2] : (object) Visibility.Visible;
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
