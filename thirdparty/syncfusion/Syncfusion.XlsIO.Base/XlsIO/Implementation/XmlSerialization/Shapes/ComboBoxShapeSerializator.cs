// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.ComboBoxShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class ComboBoxShapeSerializator : ShapeSerializator
{
  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if ((shape as ComboBoxShapeImpl).ComboType == ExcelComboType.AutoFilter)
      return;
    writer.WriteStartElement(nameof (shape), "urn:schemas-microsoft-com:vml");
    string str1 = '#'.ToString() + $"_x0000_t{shape.InnerSpRecord.Instance}";
    string str2 = $"_x0000_s{shape.ShapeId}";
    writer.WriteAttributeString("id", str2);
    writer.WriteAttributeString("type", str1);
    string str3 = $"{$"margin-left:{shape.Left.ToString()}pt"};{$"margin-top:{shape.Top.ToString()}pt"};{$"width:{shape.Width.ToString()}pt"};{$"height:{shape.Height.ToString()}pt"}";
    if (!shape.IsShapeVisible)
      str3 = $"{str3};{"visibility"}:{"hidden"}";
    string str4 = $"position:absolute;{str3};z-index:1";
    writer.WriteAttributeString("style", str4);
    this.SerializeClientData(writer, shape as ComboBoxShapeImpl);
    writer.WriteEndElement();
  }

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    writer.WriteStartElement("shapetype", "urn:schemas-microsoft-com:vml");
    string str = $"_x0000_t{201}";
    writer.WriteAttributeString("id", str);
    writer.WriteAttributeString("coordsize", "21600,21600");
    writer.WriteAttributeString("spt", "urn:schemas-microsoft-com:office:office", 201.ToString());
    writer.WriteAttributeString("path", "m,l,21600r21600,l21600,xe");
    writer.WriteEndElement();
  }

  private void SerializeClientData(XmlWriter writer, ComboBoxShapeImpl comboBox)
  {
    writer.WriteStartElement("ClientData", "urn:schemas-microsoft-com:office:excel");
    writer.WriteAttributeString("ObjectType", "Drop");
    writer.WriteElementString("SizeWithCells", "urn:schemas-microsoft-com:office:excel", (!comboBox.IsSizeWithCell).ToString());
    string anchorValue = ShapeSerializator.GetAnchorValue((ShapeImpl) comboBox);
    writer.WriteElementString("Anchor", "urn:schemas-microsoft-com:office:excel", anchorValue);
    writer.WriteElementString("AutoLine", "urn:schemas-microsoft-com:office:excel", "False");
    if (comboBox.OnAction != null && comboBox.OnAction.Length > 0)
      writer.WriteElementString("FmlaMacro", "urn:schemas-microsoft-com:office:excel", comboBox.OnAction);
    IRange linkedCell = comboBox.LinkedCell;
    if (linkedCell != null)
      writer.WriteElementString("FmlaLink", "urn:schemas-microsoft-com:office:excel", linkedCell.AddressGlobal);
    writer.WriteElementString("Val", "urn:schemas-microsoft-com:office:excel", "2");
    writer.WriteElementString("Min", "urn:schemas-microsoft-com:office:excel", "0");
    writer.WriteElementString("Max", "urn:schemas-microsoft-com:office:excel", "2");
    writer.WriteElementString("Inc", "urn:schemas-microsoft-com:office:excel", "1");
    writer.WriteElementString("Page", "urn:schemas-microsoft-com:office:excel", "8");
    writer.WriteElementString("Dx", "urn:schemas-microsoft-com:office:excel", "15");
    IRange listFillRange = comboBox.ListFillRange;
    if (listFillRange != null)
      writer.WriteElementString("FmlaRange", "urn:schemas-microsoft-com:office:excel", listFillRange.AddressGlobal);
    writer.WriteElementString("Sel", "urn:schemas-microsoft-com:office:excel", comboBox.SelectedIndex.ToString());
    if (!comboBox.Display3DShading)
      writer.WriteElementString("NoThreeD2", "urn:schemas-microsoft-com:office:excel", string.Empty);
    writer.WriteElementString("SelType", "urn:schemas-microsoft-com:office:excel", Vml.SelectionTypes.Single.ToString());
    writer.WriteElementString("LCT", "urn:schemas-microsoft-com:office:excel", "Normal");
    writer.WriteElementString("DropStyle", "urn:schemas-microsoft-com:office:excel", Vml.DropStyles.Combo.ToString());
    writer.WriteElementString("DropLines", "urn:schemas-microsoft-com:office:excel", comboBox.DropDownLines.ToString());
    writer.WriteEndElement();
  }
}
