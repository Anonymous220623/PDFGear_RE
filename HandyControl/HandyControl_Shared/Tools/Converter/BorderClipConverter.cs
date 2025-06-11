// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.BorderClipConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Converter;

public class BorderClipConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 3 || !(values[0] is double x) || !(values[1] is double y) || !(values[2] is CornerRadius cornerRadius))
      return DependencyProperty.UnsetValue;
    if (x < double.Epsilon || y < double.Epsilon)
      return (object) Geometry.Empty;
    PathGeometry pathGeometry = new PathGeometry()
    {
      Figures = new PathFigureCollection()
      {
        new PathFigure(new Point(cornerRadius.TopLeft, 0.0), (IEnumerable<PathSegment>) new PathSegment[8]
        {
          (PathSegment) new LineSegment(new Point(x - cornerRadius.TopRight, 0.0), false),
          (PathSegment) new ArcSegment(new Point(x, cornerRadius.TopRight), new Size(cornerRadius.TopRight, cornerRadius.TopRight), 90.0, false, SweepDirection.Clockwise, false),
          (PathSegment) new LineSegment(new Point(x, y - cornerRadius.BottomRight), false),
          (PathSegment) new ArcSegment(new Point(x - cornerRadius.BottomRight, y), new Size(cornerRadius.BottomRight, cornerRadius.BottomRight), 90.0, false, SweepDirection.Clockwise, false),
          (PathSegment) new LineSegment(new Point(cornerRadius.BottomLeft, y), false),
          (PathSegment) new ArcSegment(new Point(0.0, y - cornerRadius.BottomLeft), new Size(cornerRadius.BottomLeft, cornerRadius.BottomLeft), 90.0, false, SweepDirection.Clockwise, false),
          (PathSegment) new LineSegment(new Point(0.0, cornerRadius.TopLeft), false),
          (PathSegment) new ArcSegment(new Point(cornerRadius.TopLeft, 0.0), new Size(cornerRadius.TopLeft, cornerRadius.TopLeft), 90.0, false, SweepDirection.Clockwise, false)
        }, false)
      }
    };
    pathGeometry.Freeze();
    return (object) pathGeometry;
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
