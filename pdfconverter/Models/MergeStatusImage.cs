// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.MergeStatusImage
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfconverter.Models;

public class MergeStatusImage : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    MergeStatus mergeStatus = (MergeStatus) value;
    string uriString = "";
    switch (mergeStatus)
    {
      case MergeStatus.Init:
      case MergeStatus.Loading:
      case MergeStatus.Loaded:
      case MergeStatus.Merging:
        uriString = "";
        break;
      case MergeStatus.LoadedFailed:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case MergeStatus.Unsupport:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case MergeStatus.Fail:
        uriString = "pack://application:,,,/images/warning.png";
        break;
      case MergeStatus.Succ:
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
