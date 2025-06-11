// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.CornerRadiusSplitConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class CornerRadiusSplitConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (!(value is CornerRadius cornerRadius) || !(parameter is string str))
      return value;
    char[] chArray = new char[1]{ ',' };
    string[] strArray = str.Split(chArray);
    return strArray.Length != 4 ? (object) cornerRadius : (object) new CornerRadius(strArray[0].Equals("1") ? cornerRadius.TopLeft : 0.0, strArray[1].Equals("1") ? cornerRadius.TopRight : 0.0, strArray[2].Equals("1") ? cornerRadius.BottomRight : 0.0, strArray[3].Equals("1") ? cornerRadius.BottomLeft : 0.0);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
