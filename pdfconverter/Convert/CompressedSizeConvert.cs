// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.CompressedSizeConvert
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Convert;

public class CompressedSizeConvert : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    string str = "";
    try
    {
      str = int.Parse(values[1].ToString()) != 8 ? values[0].ToString() : "";
    }
    catch
    {
    }
    return (object) str;
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
