// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextBody
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class TextBody
{
  private string attribute;
  private ShapeImplExt shape;

  public TextBody(ShapeImplExt shape, string attribute)
  {
    this.shape = shape;
    this.attribute = attribute;
  }

  private void TextBodyProperties(XmlWriter xmlTextWriter)
  {
    TextFrame textFrame = this.shape.TextFrame;
    xmlTextWriter.WriteStartElement("a", "bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textFrame.TextVertOverflowType != TextVertOverflowType.OverFlow)
      xmlTextWriter.WriteAttributeString("vertOverflow", Helper.GetVerticalFlowType(textFrame.TextVertOverflowType));
    if (textFrame.TextHorzOverflowType != TextHorzOverflowType.OverFlow)
      xmlTextWriter.WriteAttributeString("horzOverflow", Helper.GetHorizontalFlowType(textFrame.TextHorzOverflowType));
    string str = "square";
    if (!textFrame.WrapTextInShape)
      str = "none";
    xmlTextWriter.WriteAttributeString("wrap", str);
    if (!textFrame.IsAutoMargins)
    {
      xmlTextWriter.WriteAttributeString("lIns", Helper.ToString(textFrame.GetLeftMargin()));
      xmlTextWriter.WriteAttributeString("tIns", Helper.ToString(textFrame.GetTopMargin()));
      xmlTextWriter.WriteAttributeString("rIns", Helper.ToString(textFrame.GetRightMargin()));
      xmlTextWriter.WriteAttributeString("bIns", Helper.ToString(textFrame.GetBottomMargin()));
    }
    string anchor = "t";
    bool anchorPosition = textFrame.GetAnchorPosition(out anchor);
    if (textFrame.TextDirection != TextDirection.Horizontal)
    {
      string textDirection = textFrame.GetTextDirection(textFrame.TextDirection);
      if (textDirection != null)
        xmlTextWriter.WriteAttributeString("vert", textDirection);
    }
    xmlTextWriter.WriteAttributeString("anchor", anchor);
    if (anchorPosition)
      xmlTextWriter.WriteAttributeString("anchorCtr", "1");
    else
      xmlTextWriter.WriteAttributeString("anchorCtr", "0");
    if (textFrame.IsAutoSize)
      xmlTextWriter.WriteElementString("a:spAutoFit", (string) null);
    if (textFrame.Columns.Number > 0)
      xmlTextWriter.WriteAttributeString("numCol", Helper.ToString(textFrame.Columns.Number));
    int num = (int) ((double) textFrame.Columns.SpacingPt * 12700.0 + 0.5);
    if (num > 0)
      xmlTextWriter.WriteAttributeString("spcCol", Helper.ToString(num));
    xmlTextWriter.WriteEndElement();
  }

  private void TextParagraph(XmlWriter xmlTextWriter_0)
  {
    xmlTextWriter_0.WriteStartElement("a", "p", (string) null);
    xmlTextWriter_0.WriteStartElement("a", "r", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter_0.WriteStartElement("a", "rPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter_0.WriteAttributeString("lang", "en-US");
    xmlTextWriter_0.WriteAttributeString("sz", "1100");
    xmlTextWriter_0.WriteEndElement();
    xmlTextWriter_0.WriteStartElement("a", "t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    string text = this.shape.TextFrame.TextRange.Text;
    xmlTextWriter_0.WriteString(text);
    xmlTextWriter_0.WriteEndElement();
    xmlTextWriter_0.WriteEndElement();
    xmlTextWriter_0.WriteEndElement();
  }

  internal void Write(XmlWriter xmlTextWriter)
  {
    if (this.shape.Logger.GetPreservedItem(PreservedFlag.RichText))
    {
      xmlTextWriter.WriteStartElement(this.attribute, "txBody", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
      RichTextString richText = (RichTextString) this.shape.TextFrame.TextRange.RichText;
      this.TextBodyProperties(xmlTextWriter);
      TextBoxSerializator.SerializeParagraphsAutoShapes(xmlTextWriter, richText, this.shape.Worksheet.ParentWorkbook);
      xmlTextWriter.WriteEndElement();
    }
    else
    {
      Stream stream;
      if (!this.shape.PreservedElements.TryGetValue(nameof (TextBody), out stream) || stream == null || stream.Length <= 0L)
        return;
      stream.Position = 0L;
      ShapeParser.WriteNodeFromStream(xmlTextWriter, stream);
    }
  }
}
