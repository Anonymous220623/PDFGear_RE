// Decompiled with JetBrains decompiler
// Type: pdfconverter.Models.MergeFileItemMoveUpBtnIsEnabledConverter
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfconverter.Models;

public class MergeFileItemMoveUpBtnIsEnabledConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      if (values[0] is MergeFileItem mergeFileItem)
      {
        if (values[1] is MergeFileItemCollection fileItemCollection)
          return fileItemCollection.IndexOf(mergeFileItem) == 0 ? (object) false : (object) true;
      }
    }
    catch
    {
    }
    return (object) true;
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
