// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Converters.EraserTypeToBoolConverter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace pdfeditor.Utils.Converters;

public class EraserTypeToBoolConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ((ToolbarSettingInkEraserModel.EraserType) value == ToolbarSettingInkEraserModel.EraserType.Whole);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (object) ToolbarSettingInkEraserModel.EraserType.Whole;
  }
}
