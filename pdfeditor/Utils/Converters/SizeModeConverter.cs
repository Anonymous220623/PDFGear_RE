// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.SizeModeConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class SizeModeConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    SizeModesWrap sizeModesWrap = (SizeModesWrap) value;
    string str = (string) parameter;
    if (sizeModesWrap == SizeModesWrap.ZoomActualSize && str == "ActualSize")
      return (object) true;
    if (sizeModesWrap == SizeModesWrap.FitToWidth && str == "FitToWidth")
      return (object) true;
    if (sizeModesWrap == SizeModesWrap.FitToHeight && str == "FitToHeight")
      return (object) true;
    return sizeModesWrap == SizeModesWrap.FitToSize && str == "FitToSize" ? (object) true : (object) false;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    int num = (bool) value ? 1 : 0;
    string str = (string) parameter;
    if (num != 0)
    {
      switch (str)
      {
        case "ActualSize":
          return (object) SizeModesWrap.ZoomActualSize;
        case "FitToWidth":
          return (object) SizeModesWrap.FitToWidth;
        case "FitToHeight":
          return (object) SizeModesWrap.FitToHeight;
        case "FitToSize":
          return (object) SizeModesWrap.FitToSize;
      }
    }
    return (object) null;
  }
}
