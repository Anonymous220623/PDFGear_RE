// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.BooleanArr2BooleanConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Tools.Converter;

public class BooleanArr2BooleanConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values == null)
      return ValueBoxes.FalseBox;
    List<bool> source = new List<bool>();
    foreach (object obj in values)
    {
      if (!(obj is bool flag))
        return ValueBoxes.FalseBox;
      source.Add(flag);
    }
    return ValueBoxes.BooleanBox(source.All<bool>((Func<bool, bool>) (item => item)));
  }

  public object[] ConvertBack(
    object value,
    Type[] targetTypes,
    object parameter,
    CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}
