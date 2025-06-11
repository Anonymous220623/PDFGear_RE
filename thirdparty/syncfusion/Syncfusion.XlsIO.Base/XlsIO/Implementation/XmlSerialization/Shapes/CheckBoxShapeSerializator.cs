// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.CheckBoxShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class CheckBoxShapeSerializator : VmlTextBoxBaseSerializator
{
  protected override int ShapeInstance => 201;

  protected override string ShapeType => "Checkbox";

  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    CheckBoxShapeImpl checkBoxShapeImpl = shape != null ? shape as CheckBoxShapeImpl : throw new ArgumentNullException(nameof (shape));
    writer.WriteStartElement(nameof (shape), "urn:schemas-microsoft-com:vml");
    this.SerializeShapeNameAndType(writer, shape);
    this.SerializeShapeStyle(writer, shape);
    if (!checkBoxShapeImpl.HasFill)
    {
      writer.WriteAttributeString("filled", "f");
      string hexColor = this.GenerateHexColor(checkBoxShapeImpl.Fill.BackColor);
      writer.WriteAttributeString("fillcolor", hexColor);
    }
    else if (checkBoxShapeImpl.Fill.FillType == ExcelFillType.SolidColor)
    {
      if (ShapeSerializator.IsEmptyColor(checkBoxShapeImpl.Fill.ForeColor))
      {
        checkBoxShapeImpl.HasFill = false;
      }
      else
      {
        string hexColor = this.GenerateHexColor(checkBoxShapeImpl.Fill.ForeColor);
        writer.WriteAttributeString("fillcolor", hexColor);
      }
    }
    if (!checkBoxShapeImpl.HasLineFormat)
    {
      writer.WriteAttributeString("stroked", "f");
    }
    else
    {
      if (checkBoxShapeImpl.Line.ForeColor != ColorExtension.Empty)
      {
        string hexColor = this.GenerateHexColor(checkBoxShapeImpl.Line.ForeColor);
        writer.WriteAttributeString("strokecolor", hexColor);
      }
      if (checkBoxShapeImpl.Line.Weight > 0.0)
      {
        string str = checkBoxShapeImpl.Line.Weight.ToString() + "pt";
        writer.WriteAttributeString("strokeweight", str);
      }
    }
    if (checkBoxShapeImpl.AlternativeText != null)
      writer.WriteAttributeString("alt", checkBoxShapeImpl.AlternativeText);
    this.SerializeShapeTagAttribute(writer, shape);
    writer.WriteAttributeString("insetmode", "urn:schemas-microsoft-com:office:office", "auto");
    if (checkBoxShapeImpl.HasFill && (ShapeSerializator.IsEmptyColor(checkBoxShapeImpl.Fill.ForeColor) || checkBoxShapeImpl.Fill.FillType != ExcelFillType.SolidColor) || !ShapeSerializator.IsEmptyColor(checkBoxShapeImpl.Fill.ForeColor))
      this.SerializeFill(writer, (ShapeImpl) checkBoxShapeImpl, holder, vmlRelations);
    if (checkBoxShapeImpl.HasLineFormat)
      this.SerializeLine(writer, (TextBoxShapeBase) checkBoxShapeImpl, holder.ParentHolder, vmlRelations);
    writer.WriteStartElement("textbox", "urn:schemas-microsoft-com:vml");
    this.SerializeTextBoxStyle(writer, shape);
    writer.WriteStartElement("div");
    writer.WriteAttributeString("style", "text-align:left");
    this.SerializeDiv(writer, shape);
    writer.WriteEndElement();
    writer.WriteEndElement();
    this.SerializeClientData(writer, shape, this.ShapeType);
    writer.WriteEndElement();
  }

  protected override void SerializeClientDataAdditional(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    CheckBoxShapeImpl checkBoxShapeImpl = shape != null ? shape as CheckBoxShapeImpl : throw new ArgumentNullException(nameof (shape));
    writer.WriteElementString("AutoFill", "urn:schemas-microsoft-com:office:excel", "False");
    writer.WriteElementString("AutoLine", "urn:schemas-microsoft-com:office:excel", "False");
    writer.WriteElementString("FmlaMacro", "urn:schemas-microsoft-com:office:excel", checkBoxShapeImpl.OnAction);
    writer.WriteElementString("TextVAlign", "urn:schemas-microsoft-com:office:excel", "Center");
    int checkState = (int) checkBoxShapeImpl.CheckState;
    if (checkState != 0)
      writer.WriteElementString("Checked", "urn:schemas-microsoft-com:office:excel", checkState.ToString());
    if (!checkBoxShapeImpl.Display3DShading)
      writer.WriteElementString("NoThreeD", "urn:schemas-microsoft-com:office:excel", string.Empty);
    if (checkBoxShapeImpl.LinkedCell == null)
      return;
    string addressGlobal = checkBoxShapeImpl.LinkedCell.AddressGlobal;
    writer.WriteElementString("FmlaLink", "urn:schemas-microsoft-com:office:excel", addressGlobal);
  }

  protected override void SerializeDiv(XmlWriter writer, ShapeImpl shape)
  {
    CheckBoxShapeImpl checkBoxShapeImpl = (CheckBoxShapeImpl) shape;
    string text = checkBoxShapeImpl.RichText.Text;
    if (text == null || text.Length <= 0)
      return;
    IFont font = checkBoxShapeImpl.Workbook.CreateFont();
    font.Size -= 2.0;
    writer.WriteStartElement("font");
    writer.WriteAttributeString("face", font.FontName);
    writer.WriteAttributeString("size", (font.Size * 20.0).ToString());
    writer.WriteAttributeString("color", "auto");
    writer.WriteString(text);
    writer.WriteEndElement();
  }

  protected override void SerializeShapeTypeSubNodes(XmlWriter writer)
  {
    writer.WriteStartElement("stroke", "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("joinstyle", "miter");
    writer.WriteEndElement();
    writer.WriteStartElement("path", "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("shadowok", "f");
    writer.WriteAttributeString("extrusionok", "urn:schemas-microsoft-com:office:office", "f");
    writer.WriteAttributeString("strokeok", "f");
    writer.WriteAttributeString("fillok", "f");
    writer.WriteAttributeString("connecttype", "urn:schemas-microsoft-com:office:office", "rect");
    writer.WriteEndElement();
    writer.WriteStartElement("lock", "urn:schemas-microsoft-com:office:office");
    writer.WriteAttributeString("ext", "urn:schemas-microsoft-com:vml", "edit");
    writer.WriteAttributeString("shapetype", "t");
    writer.WriteEndElement();
  }
}
