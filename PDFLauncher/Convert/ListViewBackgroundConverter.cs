// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Convert.ListViewBackgroundConverter
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace PDFLauncher.Convert;

public class ListViewBackgroundConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    ListViewItem container = (ListViewItem) value;
    int num = (ItemsControl.ItemsControlFromItemContainer((DependencyObject) container) as ListView).ItemContainerGenerator.IndexFromContainer((DependencyObject) container);
    BrushConverter brushConverter = new BrushConverter();
    return num % 2 == 0 ? (object) (Brush) brushConverter.ConvertFromString("#FFFFFF") : (object) (Brush) brushConverter.ConvertFromString("#F5F5F5");
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ((Brush) value).ToString();
  }
}
