// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSRadialGradientBrush
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

[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlRoot("RadialGradientBrush", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[Serializable]
public class OXPSRadialGradientBrush
{
  private OXPSTransform radialGradientBrushTransformField;
  private OXPSGradientStop[] radialGradientBrushGradientStopsField;
  private double opacityField;
  private string keyField;
  private OXPSClrIntMode colorInterpolationModeField;
  private OXPSSpreadMethod spreadMethodField;
  private OXPSMappingMode mappingModeField;
  private string transformField;
  private string centerField;
  private string gradientOriginField;
  private double radiusXField;
  private double radiusYField;

  public OXPSRadialGradientBrush()
  {
    this.opacityField = 1.0;
    this.colorInterpolationModeField = OXPSClrIntMode.SRgbLinearInterpolation;
    this.spreadMethodField = OXPSSpreadMethod.Pad;
    this.mappingModeField = OXPSMappingMode.Absolute;
  }

  [XmlElement("RadialGradientBrush.Transform")]
  public OXPSTransform RadialGradientBrushTransform
  {
    get => this.radialGradientBrushTransformField;
    set => this.radialGradientBrushTransformField = value;
  }

  [XmlArrayItem("GradientStop", IsNullable = false)]
  [XmlArray("RadialGradientBrush.GradientStops")]
  public OXPSGradientStop[] RadialGradientBrushGradientStops
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

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.openxps.org/oxps/v1.0/resourcedictionary-key")]
  public string Key
  {
    get => this.keyField;
    set => this.keyField = value;
  }

  [DefaultValue(OXPSClrIntMode.SRgbLinearInterpolation)]
  [XmlAttribute]
  public OXPSClrIntMode ColorInterpolationMode
  {
    get => this.colorInterpolationModeField;
    set => this.colorInterpolationModeField = value;
  }

  [XmlAttribute]
  [DefaultValue(OXPSSpreadMethod.Pad)]
  public OXPSSpreadMethod SpreadMethod
  {
    get => this.spreadMethodField;
    set => this.spreadMethodField = value;
  }

  [XmlAttribute]
  public OXPSMappingMode MappingMode
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
