// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.IsNullOrEmptyConverter
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace CommomLib.Controls;

internal class IsNullOrEmptyConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value == null)
      return (object) true;
    return value is string str && string.IsNullOrEmpty(str) ? (object) true : (object) false;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
