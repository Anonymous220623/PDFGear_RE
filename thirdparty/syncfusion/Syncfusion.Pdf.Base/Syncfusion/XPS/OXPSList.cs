// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSList
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
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure")]
[XmlRoot("ListStructure", Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure", IsNullable = false)]
[Serializable]
public class OXPSList
{
  private OXPSListItem[] itemsField;

  [XmlElement("ListItemStructure")]
  public OXPSListItem[] OXPSItems
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }
}
