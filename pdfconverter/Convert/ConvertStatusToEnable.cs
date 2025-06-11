// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.ConvertStatusToEnable
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Models;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Convert;

public class ConvertStatusToEnable : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch ((ToPDFItemStatus) value)
    {
      case ToPDFItemStatus.LoadedFailed:
      case ToPDFItemStatus.Unsupport:
      case ToPDFItemStatus.Working:
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
