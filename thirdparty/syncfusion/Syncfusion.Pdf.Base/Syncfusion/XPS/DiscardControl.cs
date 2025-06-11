// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.DiscardControl
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
[XmlRoot("DiscardControl", Namespace = "http://schemas.microsoft.com/xps/2005/06/discard-control", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/discard-control")]
[Serializable]
public class DiscardControl
{
  private Syncfusion.XPS.Discard[] discardField;

  [XmlElement("Discard")]
  public Syncfusion.XPS.Discard[] Discard
  {
    get => this.discardField;
    set => this.discardField = value;
  }
}
