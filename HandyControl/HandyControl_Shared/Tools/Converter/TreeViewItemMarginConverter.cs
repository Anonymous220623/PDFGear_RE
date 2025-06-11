// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.TreeViewItemMarginConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Converter;

public class TreeViewItemMarginConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    double left = 0.0;
    UIElement reference = (UIElement) (value as TreeViewItem);
    while (reference != null && reference.GetType() != typeof (TreeView))
    {
      reference = (UIElement) VisualTreeHelper.GetParent((DependencyObject) reference);
      if (reference is TreeViewItem)
        left += 19.0;
    }
    return (object) new Thickness(left, 0.0, 0.0, 0.0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
