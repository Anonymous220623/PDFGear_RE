// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.GeometrySourceExtensions
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Media;

internal static class GeometrySourceExtensions
{
  public static GeometryEffect GetGeometryEffect(this IGeometrySourceParameters parameters)
  {
    if (parameters is DependencyObject dependencyObject)
    {
      GeometryEffect geometryEffect = GeometryEffect.GetGeometryEffect(dependencyObject);
      if (geometryEffect != null && dependencyObject.Equals((object) geometryEffect.Parent))
        return geometryEffect;
    }
    return (GeometryEffect) null;
  }

  public static double GetHalfStrokeThickness(this IGeometrySourceParameters parameter)
  {
    if (parameter.Stroke != null)
    {
      double strokeThickness = parameter.StrokeThickness;
      if (!double.IsNaN(strokeThickness) && !double.IsInfinity(strokeThickness))
        return Math.Abs(strokeThickness) / 2.0;
    }
    return 0.0;
  }
}
