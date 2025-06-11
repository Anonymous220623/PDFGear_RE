// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.CommentShapeParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

internal class CommentShapeParser : VmlTextBoxBaseParser
{
  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    reader.Skip();
    return (ShapeImpl) (shapes.Application as ApplicationImpl).CreateCommentShapeImpl((object) shapes);
  }

  protected override void ParseUnknownClientDataTag(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      CommentShapeImpl commentShapeImpl = textBox as CommentShapeImpl;
      switch (reader.LocalName)
      {
        case "Row":
          commentShapeImpl.Row = reader.ReadElementContentAsInt() + 1;
          break;
        case "Column":
          commentShapeImpl.Column = reader.ReadElementContentAsInt() + 1;
          break;
        case "Visible":
          commentShapeImpl.IsVisible = VmlTextBoxBaseParser.ParseBoolOrEmpty(reader, true);
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  protected override void ParseStyle(
    TextBoxShapeBase textBox,
    Dictionary<string, string> styleProperties)
  {
    CommentShapeImpl comment = textBox as CommentShapeImpl;
    this.ParseVisibility(comment, styleProperties);
    this.ParseAutoSize(comment, styleProperties);
  }

  private void ParseVisibility(CommentShapeImpl comment, Dictionary<string, string> dictProperties)
  {
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    if (dictProperties == null)
      throw new ArgumentNullException(nameof (dictProperties));
    bool flag = true;
    string str;
    if (dictProperties.TryGetValue("visibility", out str))
      flag = str != "hidden";
    comment.IsShapeVisible = flag;
  }

  private void ParseAutoSize(CommentShapeImpl comment, Dictionary<string, string> dictProperties)
  {
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    if (dictProperties == null)
      throw new ArgumentNullException(nameof (dictProperties));
    string str;
    if (!dictProperties.TryGetValue("mso-fit-shape-to-text", out str))
      return;
    comment.AutoSize = str == "t";
  }

  protected override void RegisterShape(TextBoxShapeBase textBox)
  {
    base.RegisterShape(textBox);
    WorksheetImpl worksheet = (WorksheetImpl) textBox.Worksheet;
    if (!textBox.ValidComment)
      return;
    worksheet.InnerComments.AddComment(textBox as ICommentShape);
  }

  internal override ShapeImpl CreateShape(ShapeCollectionBase shapes)
  {
    return (ShapeImpl) (shapes.Application as ApplicationImpl).CreateCommentShapeImpl((object) shapes);
  }
}
