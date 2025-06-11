// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.ImageBrush
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
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[XmlRoot("ImageBrush", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[DesignerCategory("code")]
[Serializable]
public class ImageBrush
{
  private Syncfusion.XPS.Transform imageBrushTransformField;
  private double opacityField;
  private string keyField;
  private string transformField;
  private string viewboxField;
  private string viewportField;
  private TileMode tileModeField;
  private ViewUnits viewboxUnitsField;
  private ViewUnits viewportUnitsField;
  private string imageSourceField;

  public ImageBrush()
  {
    this.opacityField = 1.0;
    this.tileModeField = TileMode.None;
    this.viewboxUnitsField = ViewUnits.Absolute;
    this.viewportUnitsField = ViewUnits.Absolute;
  }

  [XmlElement("ImageBrush.Transform")]
  public Syncfusion.XPS.Transform ImageBrushTransform
  {
    get => this.imageBrushTransformField;
    set => this.imageBrushTransformField = value;
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
  public string ImageSource
  {
    get => this.imageSourceField;
    set => this.imageSourceField = value;
  }
}
