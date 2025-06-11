// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.LinearGradientBrush
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
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("LinearGradientBrush", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[Serializable]
public class LinearGradientBrush
{
  private Syncfusion.XPS.Transform linearGradientBrushTransformField;
  private GradientStop[] linearGradientBrushGradientStopsField;
  private double opacityField;
  private string keyField;
  private ClrIntMode colorInterpolationModeField;
  private SpreadMethod spreadMethodField;
  private MappingMode mappingModeField;
  private string transformField;
  private string startPointField;
  private string endPointField;

  public LinearGradientBrush()
  {
    this.opacityField = 1.0;
    this.colorInterpolationModeField = ClrIntMode.SRgbLinearInterpolation;
    this.spreadMethodField = SpreadMethod.None;
    this.mappingModeField = MappingMode.Absolute;
  }

  [XmlElement("LinearGradientBrush.Transform")]
  public Syncfusion.XPS.Transform LinearGradientBrushTransform
  {
    get => this.linearGradientBrushTransformField;
    set => this.linearGradientBrushTransformField = value;
  }

  [XmlArray("LinearGradientBrush.GradientStops")]
  [XmlArrayItem("GradientStop", IsNullable = false)]
  public GradientStop[] LinearGradientBrushGradientStops
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
