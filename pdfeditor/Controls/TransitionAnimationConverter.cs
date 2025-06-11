// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.TransitionAnimationConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Controls;

public class TransitionAnimationConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length == 2)
    {
      try
      {
        double num = System.Convert.ToDouble(values[0], (IFormatProvider) culture);
        return (object) (System.Convert.ToDouble(values[1], (IFormatProvider) culture) * num);
      }
      catch
      {
      }
    }
    return (object) 0.0;
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
