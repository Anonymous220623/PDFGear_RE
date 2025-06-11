// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSSpotLocationType
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

[DebuggerStepThrough]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/signature-definitions")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[Serializable]
public class OXPSSpotLocationType
{
  private string pageURIField;
  private double startXField;
  private double startYField;

  [XmlAttribute(DataType = "anyURI")]
  public string PageURI
  {
    get => this.pageURIField;
    set => this.pageURIField = value;
  }

  [XmlAttribute]
  public double StartX
  {
    get => this.startXField;
    set => this.startXField = value;
  }

  [XmlAttribute]
  public double StartY
  {
    get => this.startYField;
    set => this.startYField = value;
  }
}
