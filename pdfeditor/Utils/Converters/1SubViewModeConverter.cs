// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.SubViewModeContinuousConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class SubViewModeContinuousConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (SubViewModeContinuous) value == SubViewModeContinuous.Verticalcontinuous ? (object) true : (object) false;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (bool) value ? (object) SubViewModeContinuous.Verticalcontinuous : (object) SubViewModeContinuous.Discontinuous;
  }
}
