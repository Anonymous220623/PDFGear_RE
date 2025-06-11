// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSPathFigure
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

[XmlRoot("PathFigure", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[Serializable]
public class OXPSPathFigure
{
  private object[] itemsField;
  private bool isClosedField;
  private string startPointField;
  private bool isFilledField;

  public OXPSPathFigure()
  {
    this.isClosedField = false;
    this.isFilledField = true;
  }

  [XmlElement("PolyQuadraticBezierSegment", typeof (OXPSPolyQuadraticBezierSegment))]
  [XmlElement("ArcSegment", typeof (OXPSArcSegment))]
  [XmlElement("PolyBezierSegment", typeof (OXPSPolyBezierSegment))]
  [XmlElement("PolyLineSegment", typeof (OXPSPolyLineSegment))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlAttribute]
  [DefaultValue(false)]
  public bool IsClosed
  {
    get => this.isClosedField;
    set => this.isClosedField = value;
  }

  [XmlAttribute]
  public string StartPoint
  {
    get => this.startPointField;
    set => this.startPointField = value;
  }

  [DefaultValue(true)]
  [XmlAttribute]
  public bool IsFilled
  {
    get => this.isFilledField;
    set => this.isFilledField = value;
  }
}
