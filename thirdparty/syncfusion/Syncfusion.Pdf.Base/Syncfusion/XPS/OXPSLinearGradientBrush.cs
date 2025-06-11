// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSLinearGradientBrush
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

[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[XmlRoot("LinearGradientBrush", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[Serializable]
public class OXPSLinearGradientBrush
{
  private OXPSTransform linearGradientBrushTransformField;
  private OXPSGradientStop[] linearGradientBrushGradientStopsField;
  private double opacityField;
  private string keyField;
  private OXPSClrIntMode colorInterpolationModeField;
  private OXPSSpreadMethod spreadMethodField;
  private OXPSMappingMode mappingModeField;
  private string transformField;
  private string startPointField;
  private string endPointField;

  public OXPSLinearGradientBrush()
  {
    this.opacityField = 1.0;
    this.colorInterpolationModeField = OXPSClrIntMode.SRgbLinearInterpolation;
    this.spreadMethodField = OXPSSpreadMethod.None;
    this.mappingModeField = OXPSMappingMode.Absolute;
  }

  [XmlElement("LinearGradientBrush.Transform")]
  public OXPSTransform LinearGradientBrushTransform
  {
    get => this.linearGradientBrushTransformField;
    set => this.linearGradientBrushTransformField = value;
  }

  [XmlArray("LinearGradientBrush.GradientStops")]
  [XmlArrayItem("GradientStop", IsNullable = false)]
  public OXPSGradientStop[] LinearGradientBrushGradientStops
  {
    get => this.linearGradientBrushGradientStopsField;
    set => this.linearGradientBrushGradientStopsField = value;
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

  [XmlAttribute]
  [DefaultValue(OXPSClrIntMode.SRgbLinearInterpolation)]
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
  public string StartPoint
  {
    get => this.startPointField;
    set => this.startPointField = value;
  }

  [XmlAttribute]
  public string EndPoint
  {
    get => this.endPointField;
    set => this.endPointField = value;
  }
}
