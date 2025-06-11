// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.IsCollatedToImageSourceConverterr
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class IsCollatedToImageSourceConverterr : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if ((bool) DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue)
      return (object) null;
    return !(value is bool flag) || !flag ? (object) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Printer/NotCollated.png")) : (object) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Printer/PageCollated.png"));
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
