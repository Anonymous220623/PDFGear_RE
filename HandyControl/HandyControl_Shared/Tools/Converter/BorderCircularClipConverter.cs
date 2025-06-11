// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.BorderCircularClipConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Converter;

public class BorderCircularClipConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 3 || !(values[0] is double width) || !(values[1] is double height) || !(values[2] is CornerRadius cornerRadius))
      return DependencyProperty.UnsetValue;
    if (width < double.Epsilon || height < double.Epsilon)
      return (object) Geometry.Empty;
    RectangleGeometry rectangleGeometry = new RectangleGeometry(new Rect(0.0, 0.0, width, height), cornerRadius.TopLeft, cornerRadius.TopLeft);
    rectangleGeometry.Freeze();
    return (object) rectangleGeometry;
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
