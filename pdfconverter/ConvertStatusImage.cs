// Decompiled with JetBrains decompiler
// Type: pdfconverter.ConvertStatusImage
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfconverter;

public class ConvertStatusImage : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    FileCovertStatus fileCovertStatus = (FileCovertStatus) value;
    string uriString = "";
    switch (fileCovertStatus)
    {
      case FileCovertStatus.ConvertLoaded:
        uriString = "";
        break;
      case FileCovertStatus.ConvertLoadedFailed:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case FileCovertStatus.ConvertUnsupport:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case FileCovertStatus.ConvertFail:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case FileCovertStatus.ConvertSucc:
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
