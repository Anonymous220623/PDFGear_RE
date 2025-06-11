// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.TableCell
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
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlRoot("TableCellStructure", Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure", IsNullable = false)]
[Serializable]
public class TableCell
{
  private object[] itemsField;
  private ItemsChoiceType[] itemsElementNameField;
  private int rowSpanField;
  private int columnSpanField;

  public TableCell()
  {
    this.rowSpanField = 1;
    this.columnSpanField = 1;
  }

  [XmlChoiceIdentifier("ItemsElementName")]
  [XmlElement("FigureStructure", typeof (Figure))]
  [XmlElement("ListStructure", typeof (List))]
  [XmlElement("ParagraphStructure", typeof (Paragraph))]
  [XmlElement("TableStructure", typeof (Table))]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlElement("ItemsElementName")]
  [XmlIgnore]
  public ItemsChoiceType[] ItemsElementName
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
