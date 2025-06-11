// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.DrawControlMode2BooleanConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.Screenshots;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

internal class DrawControlMode2BooleanConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ($"{value}" == $"{parameter}");
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    DrawControlMode result;
    if (((!(value is bool flag) ? 0 : 1) & (flag ? 1 : 0)) != 0)
      Enum.TryParse<DrawControlMode>(parameter?.ToString(), true, out result);
    else
      result = DrawControlMode.None;
    return (object) result;
  }
}
