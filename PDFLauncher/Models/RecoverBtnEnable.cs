// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Models.RecoverBtnEnable
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace PDFLauncher.Models;

public class RecoverBtnEnable : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    string str = values != null && values.Length != 0 ? values[0].ToString().Trim() : throw new ArgumentNullException("values can not be null");
    int num = int.Parse(values[1].ToString());
    return str.Length > 0 && num > 0 ? (object) true : (object) false;
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
