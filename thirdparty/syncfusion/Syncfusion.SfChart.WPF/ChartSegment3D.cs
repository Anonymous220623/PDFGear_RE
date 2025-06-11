// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartSegment3D : ChartSegment
{
  internal List<Polygon3D> Polygons = new List<Polygon3D>();
  protected internal double startDepth;
  protected internal double endDepth;

  public DoubleRange ZRange { get; set; }

  internal override UIElement CreateSegmentVisual(Size size)
  {
    if ((this.Series as ChartSeries3D).PrevSelectedIndex != this.Series.Segments.IndexOf((ChartSegment) this))
      this.BindProperties();
    return this.CreateVisual(size);
  }
}
