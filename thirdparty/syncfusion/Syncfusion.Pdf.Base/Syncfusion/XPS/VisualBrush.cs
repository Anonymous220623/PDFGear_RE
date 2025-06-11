// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.VisualBrush
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
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("VisualBrush", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[Serializable]
public class VisualBrush
{
  private Syncfusion.XPS.Transform visualBrushTransformField;
  private Syncfusion.XPS.Visual visualBrushVisualField;
  private double opacityField;
  private string keyField;
  private string transformField;
  private string viewboxField;
  private string viewportField;
  private TileMode tileModeField;
  private ViewUnits viewboxUnitsField;
  private ViewUnits viewportUnitsField;
  private string visualField;

  public VisualBrush()
  {
    this.opacityField = 1.0;
    this.tileModeField = TileMode.None;
    this.viewboxUnitsField = ViewUnits.Absolute;
    this.viewportUnitsField = ViewUnits.Absolute;
  }

  [XmlElement("VisualBrush.Transform")]
  public Syncfusion.XPS.Transform VisualBrushTransform
  {
    get => this.visualBrushTransformField;
    set => this.visualBrushTransformField = value;
  }

  [XmlElement("VisualBrush.Visual")]
  public Syncfusion.XPS.Visual VisualBrushVisual
  {
    get => this.visualBrushVisualField;
    set => this.visualBrushVisualField = value;
  }

  [XmlAttribute]
  [DefaultValue(1)]
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

  [DefaultValue(TileMode.None)]
  [XmlAttribute]
  public TileMode TileMode
  {
    get => this.tileModeField;
    set => this.tileModeField = value;
  }

  [XmlAttribute]
  public ViewUnits ViewboxUnits
  {
    get => this.viewboxUnitsField;
    set => this.viewboxUnitsField = value;
  }

  [XmlAttribute]
  public ViewUnits ViewportUnits
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
