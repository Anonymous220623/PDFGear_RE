// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.IsStringConverter
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace CommomLib.Controls;

internal class IsStringConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) (bool) (value == null ? 1 : (value is string ? 1 : 0));
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
