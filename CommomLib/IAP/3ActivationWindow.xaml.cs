// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.BooleanToOpacityReverseConverter
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace CommomLib.IAP;

internal class BooleanToOpacityReverseConverter : IValueConverter
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
