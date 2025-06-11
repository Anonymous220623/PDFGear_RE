// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.GeneralTaskStatusToEnable
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using pdfconverter.Models;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Convert;

public class GeneralTaskStatusToEnable : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (WorkQueenState) value == WorkQueenState.Working ? (object) false : (object) true;
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
