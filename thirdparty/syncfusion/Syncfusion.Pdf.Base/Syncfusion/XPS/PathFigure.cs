// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.PathFigure
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

[DesignerCategory("code")]
[XmlRoot("PathFigure", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[Serializable]
public class PathFigure
{
  private object[] itemsField;
  private bool isClosedField;
  private string startPointField;
  private bool isFilledField;

  public PathFigure()
  {
    this.isClosedField = false;
    this.isFilledField = true;
  }

  [XmlElement("ArcSegment", typeof (ArcSegment))]
  [XmlElement("PolyBezierSegment", typeof (PolyBezierSegment))]
  [XmlElement("PolyLineSegment", typeof (PolyLineSegment))]
  [XmlElement("PolyQuadraticBezierSegment", typeof (PolyQuadraticBezierSegment))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [DefaultValue(false)]
  [XmlAttribute]
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

  [XmlAttribute]
  [DefaultValue(true)]
  public bool IsFilled
  {
    get => this.isFilledField;
    set => this.isFilledField = value;
  }
}
