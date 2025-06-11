// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSArcSegment
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

[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlRoot("ArcSegment", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[Serializable]
public class OXPSArcSegment
{
  private string pointField;
  private string sizeField;
  private double rotationAngleField;
  private bool isLargeArcField;
  private OXPSSweepDirection sweepDirectionField;
  private bool isStrokedField;

  public OXPSArcSegment() => this.isStrokedField = true;

  [XmlAttribute]
  public string Point
  {
    get => this.pointField;
    set => this.pointField = value;
  }

  [XmlAttribute]
  public string Size
  {
    get => this.sizeField;
    set => this.sizeField = value;
  }

  [XmlAttribute]
  public double RotationAngle
  {
    get => this.rotationAngleField;
    set => this.rotationAngleField = value;
  }

  [XmlAttribute]
  public bool IsLargeArc
  {
    get => this.isLargeArcField;
    set => this.isLargeArcField = value;
  }

  [XmlAttribute]
  public OXPSSweepDirection SweepDirection
  {
    get => this.sweepDirectionField;
    set => this.sweepDirectionField = value;
  }

  [XmlAttribute]
  [DefaultValue(true)]
  public bool IsStroked
  {
    get => this.isStrokedField;
    set => this.isStrokedField = value;
  }
}
