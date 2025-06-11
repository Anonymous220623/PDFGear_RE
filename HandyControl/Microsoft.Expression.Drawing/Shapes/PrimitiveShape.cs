// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Shapes.PrimitiveShape
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Expression.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Expression.Shapes;

public abstract class PrimitiveShape : Shape, IGeometrySourceParameters, IShape
{
  private IGeometrySource _geometrySource;

  static PrimitiveShape()
  {
    Shape.StretchProperty.OverrideMetadata(typeof (PrimitiveShape), (PropertyMetadata) new DrawingPropertyMetadata((object) Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));
    Shape.StrokeThicknessProperty.OverrideMetadata(typeof (PrimitiveShape), (PropertyMetadata) new DrawingPropertyMetadata(ValueBoxes.Double1Box, DrawingPropertyMetadataOptions.AffectsRender));
  }

  protected sealed override Geometry DefiningGeometry
  {
    get => this.GeometrySource.Geometry ?? Geometry.Empty;
  }

  private IGeometrySource GeometrySource
  {
    get => this._geometrySource ?? (this._geometrySource = this.CreateGeometrySource());
  }

  Stretch IGeometrySourceParameters.Stretch => this.Stretch;

  Brush IGeometrySourceParameters.Stroke => this.Stroke;

  double IGeometrySourceParameters.StrokeThickness => this.StrokeThickness;

  public event EventHandler RenderedGeometryChanged;

  public void InvalidateGeometry(InvalidateGeometryReasons reasons)
  {
    if (!this.GeometrySource.InvalidateGeometry(reasons))
      return;
    this.InvalidateArrange();
  }

  public Thickness GeometryMargin
  {
    get => this.GeometrySource.LogicalBounds.Subtract(this.RenderedGeometry.Bounds);
  }

  Brush IShape.Fill
  {
    get => this.Fill;
    set => this.Fill = value;
  }

  Stretch IShape.Stretch
  {
    get => this.Stretch;
    set => this.Stretch = value;
  }

  Brush IShape.Stroke
  {
    get => this.Stroke;
    set => this.Stroke = value;
  }

  double IShape.StrokeThickness
  {
    get => this.StrokeThickness;
    set => this.StrokeThickness = value;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    if (this.GeometrySource.UpdateGeometry((IGeometrySourceParameters) this, finalSize.Bounds()))
      this.RealizeGeometry();
    base.ArrangeOverride(finalSize);
    return finalSize;
  }

  protected abstract IGeometrySource CreateGeometrySource();

  protected override Size MeasureOverride(Size availableSize)
  {
    return new Size(this.StrokeThickness, this.StrokeThickness);
  }

  private void RealizeGeometry()
  {
    EventHandler renderedGeometryChanged = this.RenderedGeometryChanged;
    if (renderedGeometryChanged == null)
      return;
    renderedGeometryChanged((object) this, EventArgs.Empty);
  }
}
