// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.SplitStatusToOperationBtnVisibility
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Models;

public class SplitStatusToOperationBtnVisibility : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    switch ((SplitStatus) value)
    {
      case SplitStatus.Init:
      case SplitStatus.Loading:
      case SplitStatus.Spliting:
        return (object) Visibility.Collapsed;
      default:
        return (object) Visibility.Visible;
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
