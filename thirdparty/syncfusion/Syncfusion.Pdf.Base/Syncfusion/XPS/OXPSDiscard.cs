// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSDiscard
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
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/discard-control")]
[XmlRoot("Discard", Namespace = "http://schemas.openxps.org/oxps/v1.0/discard-control", IsNullable = false)]
[Serializable]
public class OXPSDiscard
{
  private string sentinelPageField;
  private string targetField;

  [XmlAttribute(DataType = "anyURI")]
  public string SentinelPage
  {
    get => this.sentinelPageField;
    set => this.sentinelPageField = value;
  }

  [XmlAttribute(DataType = "anyURI")]
  public string Target
  {
    get => this.targetField;
    set => this.targetField = value;
  }
}
