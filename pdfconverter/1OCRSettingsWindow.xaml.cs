// Decompiled with JetBrains decompiler
// Type: pdfconverter.TestConverter
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace pdfconverter;

public class TestConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (Visibility) (values.Length < 2 || !object.Equals(values[0], values[1]) ? 2 : 0);
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
