// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.ThicknessListBoxEllipseSizeConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.Screenshots;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

internal class ThicknessListBoxEllipseSizeConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    double num = (double) value;
    int index = Array.IndexOf<double>(DrawSettingConstants.Thicknesses, num);
    if (index < 0 || index > ((IEnumerable<double>) DrawSettingConstants.ThicknessListBoxEllipseSize).Count<double>() - 1)
      index = 0;
    return (object) DrawSettingConstants.ThicknessListBoxEllipseSize[index];
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    double num = (double) value;
    int index = Array.IndexOf<double>(DrawSettingConstants.ThicknessListBoxEllipseSize, num);
    if (index < 0 || index > ((IEnumerable<double>) DrawSettingConstants.Thicknesses).Count<double>() - 1)
      index = 0;
    return (object) DrawSettingConstants.Thicknesses[index];
  }
}
