// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.SplitStatusImage
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfconverter.Models;

public class SplitStatusImage : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    SplitStatus splitStatus = (SplitStatus) value;
    string uriString = "";
    switch (splitStatus)
    {
      case SplitStatus.Init:
      case SplitStatus.Loading:
      case SplitStatus.Loaded:
      case SplitStatus.Spliting:
        uriString = "";
        break;
      case SplitStatus.LoadedFailed:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case SplitStatus.Unsupport:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case SplitStatus.Fail:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case SplitStatus.Succ:
        uriString = "pack://application:,,,/images/converted.png";
        break;
    }
    return string.IsNullOrWhiteSpace(uriString) ? (object) null : (object) new BitmapImage(new Uri(uriString));
  }

  object IValueConverter.ConvertBack(
    object value,
    Type targetType,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
