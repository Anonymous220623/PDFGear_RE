// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.RecoverStatusImage
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFLauncher.Models;

public class RecoverStatusImage : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    int num = (int) value;
    string uriString = "";
    if (num == 1)
      uriString = "pack://application:,,,/images/convering.png";
    if (num == 2)
      uriString = "pack://application:,,,/images/converted.png";
    return string.IsNullOrWhiteSpace(uriString) ? (object) null : (object) new BitmapImage(new Uri(uriString));
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
