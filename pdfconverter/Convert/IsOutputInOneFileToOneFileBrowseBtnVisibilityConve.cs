// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.IsOutputInOneFileToOneFileBrowseBtnVisibilityConvert
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

internal class IsOutputInOneFileToOneFileBrowseBtnVisibilityConvert : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      return (bool) values[1] && (WorkQueenState) values[0] == WorkQueenState.Succ ? (object) Visibility.Visible : (object) Visibility.Collapsed;
    }
    catch
    {
      return (object) Visibility.Collapsed;
    }
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
