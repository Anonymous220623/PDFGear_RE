// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.IShape
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Media;

public interface IShape
{
  event EventHandler RenderedGeometryChanged;

  void InvalidateGeometry(InvalidateGeometryReasons reasons);

  Brush Fill { get; set; }

  Thickness GeometryMargin { get; }

  Geometry RenderedGeometry { get; }

  Stretch Stretch { get; set; }

  Brush Stroke { get; set; }

  double StrokeThickness { get; set; }
}
