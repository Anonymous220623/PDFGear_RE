// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.PathGeometryHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class PathGeometryHelper
{
  public static bool IsStroked(this PathSegment pathSegment) => pathSegment.IsStroked;

  public static PathGeometry AsPathGeometry(this Geometry original)
  {
    if (original == null)
      return (PathGeometry) null;
    return !(original is PathGeometry pathGeometry) ? PathGeometry.CreateFromGeometry(original) : pathGeometry;
  }

  internal static Geometry FixPathGeometryBoundary(Geometry geometry) => geometry;
}
