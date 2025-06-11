// Decompiled with JetBrains decompiler
// Type: pdfconverter.FileSelectIsEnabled
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter;

public class FileSelectIsEnabled : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch ((FileCovertStatus) value)
    {
      case FileCovertStatus.ConvertLoadedFailed:
      case FileCovertStatus.ConvertUnsupport:
        return (object) false;
      default:
        return (object) true;
    }
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
