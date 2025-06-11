// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSVisualBrush
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
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[XmlRoot("VisualBrush", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[Serializable]
public class OXPSVisualBrush
{
  private OXPSTransform visualBrushTransformField;
  private OXPSVisual visualBrushVisualField;
  private double opacityField;
  private string keyField;
  private string transformField;
  private string viewboxField;
  private string viewportField;
  private OXPSTileMode tileModeField;
  private OXPSViewUnits viewboxUnitsField;
  private OXPSViewUnits viewportUnitsField;
  private string visualField;

  public OXPSVisualBrush()
  {
    this.opacityField = 1.0;
    this.tileModeField = OXPSTileMode.None;
    this.viewboxUnitsField = OXPSViewUnits.Absolute;
    this.viewportUnitsField = OXPSViewUnits.Absolute;
  }

  [XmlElement("VisualBrush.Transform")]
  public OXPSTransform VisualBrushTransform
  {
    get => this.visualBrushTransformField;
    set => this.visualBrushTransformField = value;
  }

  [XmlElement("VisualBrush.Visual")]
  public OXPSVisual VisualBrushVisual
  {
    get => this.visualBrushVisualField;
    set => this.visualBrushVisualField = value;
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
  public string Transform
  {
    get => this.transformField;
    set => this.transformField = value;
  }

  [XmlAttribute]
  public string Viewbox
  {
    get => this.viewboxField;
    set => this.viewboxField = value;
  }

  [XmlAttribute]
  public string Viewport
  {
    get => this.viewportField;
    set => this.viewportField = value;
  }

  [DefaultValue(OXPSTileMode.None)]
  [XmlAttribute]
  public OXPSTileMode TileMode
  {
    get => this.tileModeField;
    set => this.tileModeField = value;
  }

  [XmlAttribute]
  public OXPSViewUnits ViewboxUnits
  {
    get => this.viewboxUnitsField;
    set => this.viewboxUnitsField = value;
  }

  [XmlAttribute]
  public OXPSViewUnits ViewportUnits
  {
    get => this.viewportUnitsField;
    set => this.viewportUnitsField = value;
  }

  [XmlAttribute]
  public string Visual
  {
    get => this.visualField;
    set => this.visualField = value;
  }
}
