// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.ListItem
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

[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlRoot("ListItemStructure", Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure", IsNullable = false)]
[DebuggerStepThrough]
[DesignerCategory("code")]
[Serializable]
public class ListItem
{
  private object[] itemsField;
  private string markerField;

  [XmlElement("TableStructure", typeof (Table))]
  [XmlElement("FigureStructure", typeof (Figure))]
  [XmlElement("ListStructure", typeof (List))]
  [XmlElement("ParagraphStructure", typeof (Paragraph))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlAttribute(DataType = "ID")]
  public string Marker
  {
    get => this.markerField;
    set => this.markerField = value;
  }
}
