// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.OptionButtonShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class OptionButtonShapeSerializator : VmlTextBoxBaseSerializator
{
  protected override int ShapeInstance => 201;

  protected override string ShapeType => "Radio";

  protected override void SerializeClientDataAdditional(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    OptionButtonShapeImpl optionButtonShapeImpl = shape != null ? shape as OptionButtonShapeImpl : throw new ArgumentNullException(nameof (shape));
    writer.WriteElementString("AutoFill", "urn:schemas-microsoft-com:office:excel", "False");
    writer.WriteElementString("AutoLine", "urn:schemas-microsoft-com:office:excel", "False");
    writer.WriteElementString("FmlaMacro", "urn:schemas-microsoft-com:office:excel", optionButtonShapeImpl.OnAction);
    writer.WriteElementString("TextVAlign", "urn:schemas-microsoft-com:office:excel", "Center");
    if (!optionButtonShapeImpl.IsTextLocked)
      writer.WriteElementString("LockText", "urn:schemas-microsoft-com:office:excel", "False");
    int checkState = (int) optionButtonShapeImpl.CheckState;
    if (checkState != 0)
      writer.WriteElementString("Checked", "urn:schemas-microsoft-com:office:excel", checkState.ToString());
    if (optionButtonShapeImpl.LinkedCell != null && optionButtonShapeImpl.IsFirstButton)
    {
      string addressGlobal = optionButtonShapeImpl.LinkedCell.AddressGlobal;
      writer.WriteElementString("FmlaLink", "urn:schemas-microsoft-com:office:excel", addressGlobal);
    }
    if (!optionButtonShapeImpl.Display3DShading)
      writer.WriteElementString("NoThreeD", "urn:schemas-microsoft-com:office:excel", string.Empty);
    if (!optionButtonShapeImpl.IsFirstButton)
      return;
    writer.WriteElementString("FirstButton", "urn:schemas-microsoft-com:office:excel", string.Empty);
  }

  protected override void SerializeDiv(XmlWriter writer, ShapeImpl shape)
  {
    OptionButtonShapeImpl optionButtonShapeImpl = (OptionButtonShapeImpl) shape;
    string text = optionButtonShapeImpl.Text;
    IFont font = optionButtonShapeImpl.Workbook.CreateFont();
    font.Size -= 2.0;
    if (text == null || text.Length <= 0)
      return;
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

  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    TextBoxShapeBase textBox = shape != null ? shape as TextBoxShapeBase : throw new ArgumentNullException(nameof (shape));
    writer.WriteStartElement(nameof (shape), "urn:schemas-microsoft-com:vml");
    this.SerializeShapeNameAndType(writer, shape);
    this.SerializeShapeStyle(writer, shape);
    if (textBox.Fill.FillType == ExcelFillType.SolidColor)
    {
      if (ShapeSerializator.IsEmptyColor(textBox.Fill.ForeColor))
      {
        textBox.HasFill = false;
      }
      else
      {
        string hexColor = this.GenerateHexColor(textBox.Fill.ForeColor);
        writer.WriteAttributeString("fillcolor", hexColor);
      }
    }
    if (!textBox.HasFill)
      writer.WriteAttributeString("filled", "f");
    if (!textBox.HasLineFormat)
      writer.WriteAttributeString("stroked", "f");
    if (textBox.Line.ForeColor != ColorExtension.Empty)
    {
      string hexColor = this.GenerateHexColor(textBox.Line.ForeColor);
      writer.WriteAttributeString("strokecolor", hexColor);
    }
    if (textBox.Line.Weight > 0.0)
    {
      string str = textBox.Line.Weight.ToString() + "pt";
      writer.WriteAttributeString("strokeweight", str);
    }
    if (textBox.AlternativeText != null)
      writer.WriteAttributeString("alt", textBox.AlternativeText);
    this.SerializeShapeTagAttribute(writer, shape);
    writer.WriteAttributeString("insetmode", "urn:schemas-microsoft-com:office:office", "auto");
    if (textBox.HasFill)
      this.SerializeFill(writer, shape, holder, vmlRelations);
    if (textBox.HasLineFormat)
      this.SerializeLine(writer, textBox, holder.ParentHolder, vmlRelations);
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
}
