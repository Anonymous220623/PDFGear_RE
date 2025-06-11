// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.EnableMultiVlaueConverter
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class EnableMultiVlaueConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue || values[3] == DependencyProperty.UnsetValue)
      return (object) true;
    if (!(bool) values[3])
      return (object) true;
    return (bool) values[0] || (bool) values[1] || (bool) values[2] ? (object) true : (object) false;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    return (object[]) targetTypes;
  }
}
