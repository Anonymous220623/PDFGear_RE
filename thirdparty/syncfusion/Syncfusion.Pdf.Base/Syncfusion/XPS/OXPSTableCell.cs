// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSTableCell
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
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure")]
[XmlRoot("TableCellStructure", Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure", IsNullable = false)]
[Serializable]
public class OXPSTableCell
{
  private object[] itemsField;
  private OXPSItemsChoiceType[] itemsElementNameField;
  private int rowSpanField;
  private int columnSpanField;

  public OXPSTableCell()
  {
    this.rowSpanField = 1;
    this.columnSpanField = 1;
  }

  [XmlElement("FigureStructure", typeof (OXPSFigure))]
  [XmlElement("ParagraphStructure", typeof (OXPSParagraph))]
  [XmlElement("ListStructure", typeof (OXPSList))]
  [XmlElement("TableStructure", typeof (OXPSTable))]
  [XmlChoiceIdentifier("ItemsElementName")]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlIgnore]
  [XmlElement("ItemsElementName")]
  public OXPSItemsChoiceType[] ItemsElementName
  {
    get => this.itemsElementNameField;
    set => this.itemsElementNameField = value;
  }

  [DefaultValue(1)]
  [XmlAttribute]
  public int RowSpan
  {
    get => this.rowSpanField;
    set => this.rowSpanField = value;
  }

  [XmlAttribute]
  [DefaultValue(1)]
  public int ColumnSpan
  {
    get => this.columnSpanField;
    set => this.columnSpanField = value;
  }
}
