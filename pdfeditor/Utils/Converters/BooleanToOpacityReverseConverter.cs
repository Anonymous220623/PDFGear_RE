// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.BooleanToOpacityReverseConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class BooleanToOpacityReverseConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return value is bool flag && flag ? (object) 0.0 : (object) 1.0;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    try
    {
      return (object) (System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture) == 0.0);
    }
    catch
    {
    }
    return (object) true;
  }
}
