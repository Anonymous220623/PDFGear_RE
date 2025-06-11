// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.ConvertStatusToOperationSuccBtnVisibility
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Convert;

public class ConvertStatusToOperationSuccBtnVisibility : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (ToPDFItemStatus) value == ToPDFItemStatus.Succ ? (object) Visibility.Visible : (object) Visibility.Collapsed;
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
