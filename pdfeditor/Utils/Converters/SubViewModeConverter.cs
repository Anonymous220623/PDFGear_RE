// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.SubViewModePageConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class SubViewModePageConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    SubViewModePage subViewModePage = (SubViewModePage) value;
    string str = (string) parameter;
    if (subViewModePage == SubViewModePage.SinglePage && str == "SingalPage")
      return (object) true;
    return subViewModePage == SubViewModePage.DoublePages && str == "DoublePages" ? (object) true : (object) false;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    int num = (bool) value ? 1 : 0;
    string str = (string) parameter;
    if (num != 0)
    {
      switch (str)
      {
        case "SingalPage":
          return (object) SubViewModePage.SinglePage;
        case "DoublePages":
          return (object) SubViewModePage.DoublePages;
      }
    }
    return (object) null;
  }
}
