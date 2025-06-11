// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.Path
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
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("Path", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[DebuggerStepThrough]
[Serializable]
public class Path
{
  private Transform pathRenderTransformField;
  private Geometry pathClipField;
  private Brush pathOpacityMaskField;
  private Brush pathFillField;
  private Brush pathStrokeField;
  private Geometry pathDataField;
  private string dataField;
  private string fillField;
  private string renderTransformField;
  private string clipField;
  private double opacityField;
  private string opacityMaskField;
  private string strokeField;
  private string strokeDashArrayField;
  private DashCap strokeDashCapField;
  private double strokeDashOffsetField;
  private LineCap strokeEndLineCapField;
  private LineCap strokeStartLineCapField;
  private LineJoin strokeLineJoinField;
  private double strokeMiterLimitField;
  private double strokeThicknessField;
  private string nameField;
  private string fixedPageNavigateUriField;
  private string langField;
  private string keyField;
  private string automationPropertiesNameField;
  private string automationPropertiesHelpTextField;
  private bool snapsToDevicePixelsField;
  private bool snapsToDevicePixelsFieldSpecified;

  public Path()
  {
    this.opacityField = 1.0;
    this.strokeDashCapField = DashCap.Flat;
    this.strokeDashOffsetField = 0.0;
    this.strokeEndLineCapField = LineCap.Flat;
    this.strokeStartLineCapField = LineCap.Flat;
    this.strokeLineJoinField = LineJoin.Miter;
    this.strokeMiterLimitField = 10.0;
    this.strokeThicknessField = 1.0;
  }

  [XmlElement("Path.RenderTransform")]
  public Transform PathRenderTransform
  {
    get => this.pathRenderTransformField;
    set => this.pathRenderTransformField = value;
  }

  [XmlElement("Path.Clip")]
  public Geometry PathClip
  {
    get => this.pathClipField;
    set => this.pathClipField = value;
  }

  [XmlElement("Path.OpacityMask")]
  public Brush PathOpacityMask
  {
    get => this.pathOpacityMaskField;
    set => this.pathOpacityMaskField = value;
  }

  [XmlElement("Path.Fill")]
  public Brush PathFill
  {
    get => this.pathFillField;
    set => this.pathFillField = value;
  }

  [XmlElement("Path.Stroke")]
  public Brush PathStroke
  {
    get => this.pathStrokeField;
    set => this.pathStrokeField = value;
  }

  [XmlElement("Path.Data")]
  public Geometry PathData
  {
    get => this.pathDataField;
    set => this.pathDataField = value;
  }

  [XmlAttribute]
  public string Data
  {
    get => this.dataField;
    set => this.dataField = value;
  }

  [XmlAttribute]
  public string Fill
  {
    get => this.fillField;
    set => this.fillField = value;
  }

  [XmlAttribute]
  public string RenderTransform
  {
    get => this.renderTransformField;
    set => this.renderTransformField = value;
  }

  [XmlAttribute]
  public string Clip
  {
    get => this.clipField;
    set => this.clipField = value;
  }

  [XmlAttribute]
  [DefaultValue(1)]
  public double Opacity
  {
    get => this.opacityField;
    set => this.opacityField = value;
  }

  [XmlAttribute]
  public string OpacityMask
  {
    get => this.opacityMaskField;
    set => this.opacityMaskField = value;
  }

  [XmlAttribute]
  public string Stroke
  {
    get => this.strokeField;
    set => this.strokeField = value;
  }

  [XmlAttribute]
  public string StrokeDashArray
  {
    get => this.strokeDashArrayField;
    set => this.strokeDashArrayField = value;
  }

  [DefaultValue(DashCap.Flat)]
  [XmlAttribute]
  public DashCap StrokeDashCap
  {
    get => this.strokeDashCapField;
    set => this.strokeDashCapField = value;
  }

  [XmlAttribute]
  [DefaultValue(0)]
  public double StrokeDashOffset
  {
    get => this.strokeDashOffsetField;
    set => this.strokeDashOffsetField = value;
  }

  [DefaultValue(LineCap.Flat)]
  [XmlAttribute]
  public LineCap StrokeEndLineCap
  {
    get => this.strokeEndLineCapField;
    set => this.strokeEndLineCapField = value;
  }

  [XmlAttribute]
  [DefaultValue(LineCap.Flat)]
  public LineCap StrokeStartLineCap
  {
    get => this.strokeStartLineCapField;
    set => this.strokeStartLineCapField = value;
  }

  [DefaultValue(LineJoin.Miter)]
  [XmlAttribute]
  public LineJoin StrokeLineJoin
  {
    get => this.strokeLineJoinField;
    set => this.strokeLineJoinField = value;
  }

  [XmlAttribute]
  [DefaultValue(10)]
  public double StrokeMiterLimit
  {
    get => this.strokeMiterLimitField;
    set => this.strokeMiterLimitField = value;
  }

  [XmlAttribute]
  [DefaultValue(1)]
  public double StrokeThickness
  {
    get => this.strokeThicknessField;
    set => this.strokeThicknessField = value;
  }

  [XmlAttribute(DataType = "ID")]
  public string Name
  {
    get => this.nameField;
    set => this.nameField = value;
  }

  [XmlAttribute("FixedPage.NavigateUri", DataType = "anyURI")]
  public string FixedPageNavigateUri
  {
    get => this.fixedPageNavigateUriField;
    set => this.fixedPageNavigateUriField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
  public string lang
  {
    get => this.langField;
    set => this.langField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key")]
  public string Key
  {
    get => this.keyField;
    set => this.keyField = value;
  }

  [XmlAttribute("AutomationProperties.Name")]
  public string AutomationPropertiesName
  {
    get => this.automationPropertiesNameField;
    set => this.automationPropertiesNameField = value;
  }

  [XmlAttribute("AutomationProperties.HelpText")]
  public string AutomationPropertiesHelpText
  {
    get => this.automationPropertiesHelpTextField;
    set => this.automationPropertiesHelpTextField = value;
  }

  [XmlAttribute]
  public bool SnapsToDevicePixels
  {
    get => this.snapsToDevicePixelsField;
    set => this.snapsToDevicePixelsField = value;
  }

  [XmlIgnore]
  public bool SnapsToDevicePixelsSpecified
  {
    get => this.snapsToDevicePixelsFieldSpecified;
    set => this.snapsToDevicePixelsFieldSpecified = value;
  }
}
