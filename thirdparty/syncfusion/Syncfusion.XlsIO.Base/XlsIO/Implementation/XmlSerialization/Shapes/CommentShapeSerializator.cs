// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.CommentShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class CommentShapeSerializator : VmlTextBoxBaseSerializator
{
  protected override int ShapeInstance => 202;

  protected override string ShapeType => "Note";

  protected override void PrepareStyleProperties(List<string> properties, ShapeImpl shape)
  {
    base.PrepareStyleProperties(properties, shape);
    CommentShapeImpl commentShapeImpl = shape as CommentShapeImpl;
    if (!commentShapeImpl.IsShapeVisible)
      properties.Add("visibility:hidden");
    properties.Add(commentShapeImpl.AutoSize ? "mso-fit-shape-to-text:t" : "mso-fit-shape-to-text:f");
  }

  protected override void SerializeClientDataAdditional(XmlWriter writer, ShapeImpl shape)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    base.SerializeClientDataAdditional(writer, shape);
    CommentShapeImpl commentShapeImpl = shape as CommentShapeImpl;
    writer.WriteElementString("Row", "urn:schemas-microsoft-com:office:excel", (commentShapeImpl.Row - 1).ToString());
    writer.WriteElementString("Column", "urn:schemas-microsoft-com:office:excel", (commentShapeImpl.Column - 1).ToString());
    if (!commentShapeImpl.IsVisible)
      return;
    writer.WriteElementString("Visible", "urn:schemas-microsoft-com:office:excel", (string) null);
  }

  protected override void SerializeShapeNodes(XmlWriter writer, ShapeImpl shape)
  {
    this.SerializeShadow(writer, shape);
  }

  protected override void SerializeShapeTypeSubNodes(XmlWriter writer)
  {
    writer.WriteStartElement("stroke", "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("joinstyle", "miter");
    writer.WriteEndElement();
    writer.WriteStartElement("path", "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("gradientshapeok", "t");
    writer.WriteAttributeString("connecttype", "urn:schemas-microsoft-com:office:office", "rect");
    writer.WriteEndElement();
  }
}
