// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.PathGeometry
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
[XmlRoot("PathGeometry", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[Serializable]
public class PathGeometry
{
  private Syncfusion.XPS.Transform pathGeometryTransformField;
  private Syncfusion.XPS.PathFigure[] pathFigureField;
  private string figuresField;
  private FillRule fillRuleField;
  private string transformField;
  private string keyField;

  public PathGeometry() => this.fillRuleField = FillRule.EvenOdd;

  [XmlElement("PathGeometry.Transform")]
  public Syncfusion.XPS.Transform PathGeometryTransform
  {
    get => this.pathGeometryTransformField;
    set => this.pathGeometryTransformField = value;
  }

  [XmlElement("PathFigure")]
  public Syncfusion.XPS.PathFigure[] PathFigure
  {
    get => this.pathFigureField;
    set => this.pathFigureField = value;
  }

  [XmlAttribute]
  public string Figures
  {
    get => this.figuresField;
    set => this.figuresField = value;
  }

  [DefaultValue(FillRule.EvenOdd)]
  [XmlAttribute]
  public FillRule FillRule
  {
    get => this.fillRuleField;
    set => this.fillRuleField = value;
  }

  [XmlAttribute]
  public string Transform
  {
    get => this.transformField;
    set => this.transformField = value;
  }

  [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key")]
  public string Key
  {
    get => this.keyField;
    set => this.keyField = value;
  }
}
