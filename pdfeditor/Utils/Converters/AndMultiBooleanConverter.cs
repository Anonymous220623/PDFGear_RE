// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.AndMultiBooleanConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

internal class AndMultiBooleanConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    bool flag1 = false;
    for (int index = 0; index < values.Length; ++index)
    {
      if (values[index].GetType() == typeof (bool))
      {
        bool flag2 = (bool) values[index];
        if (index == 0)
          flag1 = flag2;
        else
          flag1 &= flag2;
      }
      if (!flag1)
        break;
    }
    return (object) flag1;
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    return (object[]) null;
  }
}
