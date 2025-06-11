// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.VmlTextBoxBaseSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal abstract class VmlTextBoxBaseSerializator : ShapeSerializator
{
  internal const string DEF_SHAPE_STYLE = "position:absolute; margin-left:59.25pt;margin-top:1.5pt;width:108pt;height:59.25pt;z-index:1;";

  protected abstract int ShapeInstance { get; }

  protected abstract string ShapeType { get; }

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("shapetype", "urn:schemas-microsoft-com:vml");
    string str = $"_x0000_t{this.ShapeInstance}";
    writer.WriteAttributeString("id", str);
    writer.WriteAttributeString("coordsize", "21600,21600");
    writer.WriteAttributeString("spt", "urn:schemas-microsoft-com:office:office", this.ShapeInstance.ToString());
    writer.WriteAttributeString("path", "m,l,21600r21600,l21600,xe");
    this.SerializeShapeTypeSubNodes(writer);
    writer.WriteEndElement();
  }

  protected virtual void SerializeShapeTypeSubNodes(XmlWriter writer)
  {
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
    string str1 = '#'.ToString() + $"_x0000_t{shape.InnerSpRecord.Instance}";
    string str2 = $"_x0000_s{shape.ShapeId}";
    writer.WriteAttributeString("id", str2);
    writer.WriteAttributeString("type", str1);
    this.SerializeShapeStyle(writer, shape);
    if (!textBox.HasFill)
      writer.WriteAttributeString("filled", "f");
    else if (textBox.FillColor != ColorExtension.Empty)
    {
      string hexColor = this.GenerateHexColor(textBox.Fill.ForeColor);
      writer.WriteAttributeString("fillcolor", hexColor);
    }
    if (!textBox.HasLineFormat)
    {
      writer.WriteAttributeString("stroked", "f");
      if (textBox.Line.BackColor != ColorExtension.Empty)
      {
        string hexColor = this.GenerateHexColor(textBox.Line.BackColor);
        writer.WriteAttributeString("strokecolor", hexColor);
      }
      if (textBox.Line.Weight > 0.0)
      {
        string str3 = textBox.Line.Weight.ToString() + "pt";
        writer.WriteAttributeString("strokeweight", str3);
      }
    }
    this.SerializeShapeTagAttribute(writer, shape);
    writer.WriteAttributeString("insetmode", "urn:schemas-microsoft-com:office:office", "auto");
    if (textBox.HasFill)
      this.SerializeFill(writer, shape, holder, vmlRelations);
    if (textBox.HasLineFormat)
      this.SerializeLine(writer, textBox, holder.ParentHolder, vmlRelations);
    this.SerializeShapeNodes(writer, shape);
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

  protected virtual void SerializeShapeTagAttribute(XmlWriter writer, ShapeImpl shape)
  {
  }

  protected virtual void SerializeShapeNodes(XmlWriter writer, ShapeImpl shape)
  {
  }

  protected void SerializeShadow(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    writer.WriteStartElement("shadow", "urn:schemas-microsoft-com:vml");
    if (shape.Workbook.Version < ExcelVersion.Excel2013)
      writer.WriteAttributeString("on", "t");
    writer.WriteAttributeString("color", "black");
    writer.WriteAttributeString("obscured", "t");
    writer.WriteEndElement();
  }

  protected virtual void SerializeDiv(XmlWriter writer, ShapeImpl shape)
  {
  }

  protected void SerializeTextBoxStyle(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    List<string> properties = new List<string>();
    this.PrepareStyleProperties(properties, shape);
    string str = UtilityMethods.Join(";", properties);
    writer.WriteAttributeString("style", str);
  }

  protected void SerializeShapeStyle(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    List<string> stringList = new List<string>();
    this.PrepareStyleProperties(stringList, shape);
    TextBoxShapeBase textBoxShapeBase = shape as TextBoxShapeBase;
    CommentShapeImpl commentShapeImpl = shape as CommentShapeImpl;
    string str = textBoxShapeBase != null && !textBoxShapeBase.IsShapeVisible || commentShapeImpl != null && !commentShapeImpl.IsShapeVisible ? "visibility:hidden" : "visibility:visible";
    if (shape.StyleProperties.Count == 0)
      writer.WriteAttributeString("style", "position:absolute; margin-left:59.25pt;margin-top:1.5pt;width:108pt;height:59.25pt;z-index:1;" + str);
    else
      VmlTextBoxBaseSerializator.SerializeStyle(writer, stringList);
  }

  public static void SerializeStyle(XmlWriter writer, List<string> styleProperties)
  {
    if (styleProperties == null || styleProperties.Count <= 0)
      return;
    string str = UtilityMethods.Join(";", styleProperties);
    writer.WriteAttributeString("style", str);
  }

  protected virtual void PrepareStyleProperties(List<string> properties, ShapeImpl shape)
  {
    if (shape.StyleProperties.Count > 0)
    {
      foreach (KeyValuePair<string, string> styleProperty in shape.StyleProperties)
        properties.Add(styleProperty.Key + (object) ':' + styleProperty.Value);
    }
    else
    {
      properties.Add("mso-direction-alt:auto");
      if (!(shape is TextBoxShapeBase textBoxShapeBase))
        return;
      switch (textBoxShapeBase.TextRotation)
      {
        case ExcelTextRotation.TopToBottom:
          properties.Add($"layout-flow{(object) ':'}vertical");
          properties.Add($"mso-layout-flow-alt{(object) ':'}top-to-bottom");
          break;
        case ExcelTextRotation.CounterClockwise:
          properties.Add($"layout-flow{(object) ':'}vertical");
          properties.Add($"mso-layout-flow-alt{(object) ':'}bottom-to-top");
          break;
        case ExcelTextRotation.Clockwise:
          properties.Add($"layout-flow{(object) ':'}vertical");
          break;
      }
      if (textBoxShapeBase.IsShapeVisible)
        return;
      properties.Add($"visibility{(object) ':'}hidden");
    }
  }

  protected override void SerializeClientDataAdditional(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    TextBoxShapeBase textBoxShapeBase = shape != null ? shape as TextBoxShapeBase : throw new ArgumentNullException(nameof (shape));
    if (textBoxShapeBase.HAlignment != ExcelCommentHAlign.Left)
      writer.WriteElementString("TextHAlign", "urn:schemas-microsoft-com:office:excel", textBoxShapeBase.HAlignment.ToString());
    if (textBoxShapeBase.VAlignment != ExcelCommentVAlign.Top)
      writer.WriteElementString("TextVAlign", "urn:schemas-microsoft-com:office:excel", textBoxShapeBase.VAlignment.ToString());
    if (!textBoxShapeBase.IsTextLocked)
      writer.WriteElementString("LockText", "urn:schemas-microsoft-com:office:excel", "False");
    base.SerializeClientDataAdditional(writer, shape);
  }

  protected void SerializeShapeNameAndType(XmlWriter writer, ShapeImpl shape)
  {
    string str1 = '#'.ToString() + $"_x0000_t{shape.InnerSpRecord.Instance}";
    string str2 = $"_x0000_s{shape.ShapeId}";
    if (!string.IsNullOrEmpty(shape.Name))
    {
      writer.WriteAttributeString("id", shape.Name);
      writer.WriteAttributeString("spid", "urn:schemas-microsoft-com:office:office", str2);
    }
    else
      writer.WriteAttributeString("id", str2);
    writer.WriteAttributeString("type", str1);
  }
}
