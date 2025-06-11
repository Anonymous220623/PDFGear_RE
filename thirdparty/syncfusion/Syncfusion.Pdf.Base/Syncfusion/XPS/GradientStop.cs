// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.GradientStop
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

[XmlRoot("GradientStop", Namespace = "http://schemas.microsoft.com/xps/2005/06", IsNullable = false)]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[Serializable]
public class GradientStop
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
