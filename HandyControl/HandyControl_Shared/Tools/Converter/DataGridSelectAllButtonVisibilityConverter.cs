// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.DataGridSelectAllButtonVisibilityConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class DataGridSelectAllButtonVisibilityConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    int? length = values?.Length;
    return length.HasValue && length.GetValueOrDefault() == 2 && values[0] is DataGridHeadersVisibility.All && values[1] is bool flag ? (object) (Visibility) (flag ? 0 : 1) : (object) Visibility.Collapsed;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
