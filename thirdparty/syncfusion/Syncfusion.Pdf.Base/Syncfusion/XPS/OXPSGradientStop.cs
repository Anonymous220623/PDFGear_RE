// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSGradientStop
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

[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlRoot("GradientStop", Namespace = "http://schemas.openxps.org/oxps/v1.0", IsNullable = false)]
[Serializable]
public class OXPSGradientStop
{
  private string colorField;
  private double offsetField;

  [XmlAttribute]
  public string Color
  {
    get => this.colorField;
    set => this.colorField = value;
  }

  [XmlAttribute]
  public double Offset
  {
    get => this.offsetField;
    set => this.offsetField = value;
  }
}
