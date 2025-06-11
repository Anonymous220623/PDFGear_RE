// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.RadialGradientBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlRoot("RadialGradientBrush", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[Serializable]
public class RadialGradientBrush
{
  private Syncfusion.XPS.Transform radialGradientBrushTransformField;
  private GradientStop[] radialGradientBrushGradientStopsField;
  private double opacityField;
  private string keyField;
  private ClrIntMode colorInterpolationModeField;
  private SpreadMethod spreadMethodField;
  private MappingMode mappingModeField;
  private string transformField;
  private string centerField;
  private string gradientOriginField;
  private double radiusXField;
  private double radiusYField;

  public RadialGradientBrush()
  {
    this.opacityField = 1.0;
    this.colorInterpolationModeField = ClrIntMode.SRgbLinearInterpolation;
    this.spreadMethodField = SpreadMethod.Pad;
    this.mappingModeField = MappingMode.Absolute;
  }

  [XmlElement("RadialGradientBrush.Transform")]
  public Syncfusion.XPS.Transform RadialGradientBrushTransform
  {
    get => this.radialGradientBrushTransformField;
    set => this.radialGradientBrushTransformField = value;
  }

  [XmlArrayItem("GradientStop", IsNullable = false)]
  [XmlArray("RadialGradientBrush.GradientStops")]
  public GradientStop[] RadialGradientBrushGradientStops
  {
    get => this.radialGradientBrushGradientStopsField;
    set => this.radialGradientBrushGradientStopsField = value;
  }

  [DefaultValue(1)]
  [XmlAttribute]
  public double Opacity
  {
    get => this.opacityField;
    set => this.opacityField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key")]
  public string Key
  {
    get => this.keyField;
    set => this.keyField = value;
  }

  [DefaultValue(ClrIntMode.SRgbLinearInterpolation)]
  [XmlAttribute]
  public ClrIntMode ColorInterpolationMode
  {
    get => this.colorInterpolationModeField;
    set => this.colorInterpolationModeField = value;
  }

  [XmlAttribute]
  [DefaultValue(SpreadMethod.Pad)]
  public SpreadMethod SpreadMethod
  {
    get => this.spreadMethodField;
    set => this.spreadMethodField = value;
  }

  [XmlAttribute]
  public MappingMode MappingMode
  {
    get => this.mappingModeField;
    set => this.mappingModeField = value;
  }

  [XmlAttribute]
  public string Transform
  {
    get => this.transformField;
    set => this.transformField = value;
  }

  [XmlAttribute]
  public string Center
  {
    get => this.centerField;
    set => this.centerField = value;
  }

  [XmlAttribute]
  public string GradientOrigin
  {
    get => this.gradientOriginField;
    set => this.gradientOriginField = value;
  }

  [XmlAttribute]
  public double RadiusX
  {
    get => this.radiusXField;
    set => this.radiusXField = value;
  }

  [XmlAttribute]
  public double RadiusY
  {
    get => this.radiusYField;
    set => this.radiusYField = value;
  }
}
