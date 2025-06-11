// Decompiled with JetBrains decompiler
// Type: pdfconverter.Convert.IsOutputInOneFileToBrowseBtnVisibilityConvert
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

public class IsOutputInOneFileToBrowseBtnVisibilityConvert : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      if ((bool) values[1])
        return (object) Visibility.Collapsed;
      return (ToPDFItemStatus) values[0] == ToPDFItemStatus.Succ && (WorkQueenState) values[2] == WorkQueenState.Succ ? (object) Visibility.Visible : (object) Visibility.Collapsed;
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
