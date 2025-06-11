// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSTable
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
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure")]
[XmlRoot("TableStructure", Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure", IsNullable = false)]
[Serializable]
public class OXPSTable
{
  private OXPSTableRowGroup[] itemsField;

  [XmlElement("TableRowGroupStructure")]
  public OXPSTableRowGroup[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }
}
