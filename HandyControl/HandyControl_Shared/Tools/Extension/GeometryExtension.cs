// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.GeometryExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Drawing;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class GeometryExtension
{
  public static double GetTotalLength(this Geometry geometry)
  {
    if (geometry == null)
      return 0.0;
    PathGeometry fromGeometry = PathGeometry.CreateFromGeometry(geometry);
    Point point;
    fromGeometry.GetPointAtFractionLength(0.0001, out point, out Point _);
    return (fromGeometry.Figures[0].StartPoint - point).Length * 10000.0;
  }

  public static double GetTotalLength(this Geometry geometry, Size size, double strokeThickness = 1.0)
  {
    if (geometry == null || MathHelper.IsVerySmall(size.Width) || MathHelper.IsVerySmall(size.Height))
      return 0.0;
    double totalLength = geometry.GetTotalLength();
    double num = Math.Min(geometry.Bounds.Width / size.Width, geometry.Bounds.Height / size.Height);
    return MathHelper.IsVerySmall(num) || MathHelper.IsVerySmall(strokeThickness) ? 0.0 : totalLength / num / strokeThickness;
  }
}
