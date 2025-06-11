// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Shapes.Arc
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Expression.Media;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Expression.Shapes;

public sealed class Arc : PrimitiveShape, IArcGeometrySourceParameters, IGeometrySourceParameters
{
  public static readonly DependencyProperty ArcThicknessProperty = DependencyProperty.Register(nameof (ArcThickness), typeof (double), typeof (Arc), (PropertyMetadata) new DrawingPropertyMetadata((object) 0.0, DrawingPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty ArcThicknessUnitProperty = DependencyProperty.Register(nameof (ArcThicknessUnit), typeof (UnitType), typeof (Arc), (PropertyMetadata) new DrawingPropertyMetadata((object) UnitType.Pixel, DrawingPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), typeof (double), typeof (Arc), (PropertyMetadata) new DrawingPropertyMetadata((object) 90.0, DrawingPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), typeof (double), typeof (Arc), (PropertyMetadata) new DrawingPropertyMetadata((object) 0.0, DrawingPropertyMetadataOptions.AffectsRender));

  public double ArcThickness
  {
    get => (double) this.GetValue(Arc.ArcThicknessProperty);
    set => this.SetValue(Arc.ArcThicknessProperty, (object) value);
  }

  public UnitType ArcThicknessUnit
  {
    get => (UnitType) this.GetValue(Arc.ArcThicknessUnitProperty);
    set => this.SetValue(Arc.ArcThicknessUnitProperty, (object) value);
  }

  public double EndAngle
  {
    get => (double) this.GetValue(Arc.EndAngleProperty);
    set => this.SetValue(Arc.EndAngleProperty, (object) value);
  }

  Stretch IGeometrySourceParameters.Stretch => this.Stretch;

  Brush IGeometrySourceParameters.Stroke => this.Stroke;

  double IGeometrySourceParameters.StrokeThickness => this.StrokeThickness;

  public double StartAngle
  {
    get => (double) this.GetValue(Arc.StartAngleProperty);
    set => this.SetValue(Arc.StartAngleProperty, (object) value);
  }

  protected override IGeometrySource CreateGeometrySource()
  {
    return (IGeometrySource) new ArcGeometrySource();
  }
}
