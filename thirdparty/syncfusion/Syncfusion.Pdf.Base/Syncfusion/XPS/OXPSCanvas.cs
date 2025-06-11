// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSCanvas
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

[XmlRoot("Canvas", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[Serializable]
public class OXPSCanvas
{
  private OXPSResources canvasResourcesField;
  private OXPSTransform canvasRenderTransformField;
  private OXPSGeometry canvasClipField;
  private OXPSBrush canvasOpacityMaskField;
  private object[] itemsField;
  private string renderTransformField;
  private string clipField;
  private double opacityField;
  private string opacityMaskField;
  private string nameField;
  private OXPSEdgeMode renderOptionsEdgeModeField;
  private bool renderOptionsEdgeModeFieldSpecified;
  private string fixedPageNavigateUriField;
  private string langField;
  private string keyField;
  private string automationPropertiesNameField;
  private string automationPropertiesHelpTextField;
  internal object m_parent;

  public OXPSCanvas() => this.opacityField = 1.0;

  [XmlElement("Canvas.Resources")]
  public OXPSResources CanvasResources
  {
    get => this.canvasResourcesField;
    set => this.canvasResourcesField = value;
  }

  [XmlElement("Canvas.RenderTransform")]
  public OXPSTransform CanvasRenderTransform
  {
    get => this.canvasRenderTransformField;
    set => this.canvasRenderTransformField = value;
  }

  [XmlElement("Canvas.Clip")]
  public OXPSGeometry CanvasClip
  {
    get => this.canvasClipField;
    set => this.canvasClipField = value;
  }

  [XmlElement("Canvas.OpacityMask")]
  public OXPSBrush CanvasOpacityMask
  {
    get => this.canvasOpacityMaskField;
    set => this.canvasOpacityMaskField = value;
  }

  [XmlElement("Canvas", typeof (OXPSCanvas))]
  [XmlElement("Glyphs", typeof (OXPSGlyphs))]
  [XmlElement("Path", typeof (OXPSPath))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
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

  [XmlAttribute(DataType = "ID")]
  public string Name
  {
    get => this.nameField;
    set => this.nameField = value;
  }

  [XmlAttribute("RenderOptions.EdgeMode")]
  public OXPSEdgeMode RenderOptionsEdgeMode
  {
    get => this.renderOptionsEdgeModeField;
    set => this.renderOptionsEdgeModeField = value;
  }

  [XmlIgnore]
  public bool RenderOptionsEdgeModeSpecified
  {
    get => this.renderOptionsEdgeModeFieldSpecified;
    set => this.renderOptionsEdgeModeFieldSpecified = value;
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

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.openxps.org/oxps/v1.0/resourcedictionary-key")]
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
}
