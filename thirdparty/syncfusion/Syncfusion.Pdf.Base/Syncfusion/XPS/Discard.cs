// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.Discard
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
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlRoot("Discard", Namespace = "http://schemas.microsoft.com/xps/2005/06/discard-control", IsNullable = false)]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/discard-control")]
[Serializable]
public class Discard
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
