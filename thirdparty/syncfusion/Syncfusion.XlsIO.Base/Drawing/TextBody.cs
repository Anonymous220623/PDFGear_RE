// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextBody
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class TextBody
{
  private string attribute;
  private ShapeImplExt shape;
  private string nameSpace;

  public TextBody(ShapeImplExt shape, string attribute)
  {
    this.shape = shape;
    this.attribute = attribute;
    this.nameSpace = attribute == "xdr" ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
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
      xmlTextWriter.WriteStartElement(this.attribute, "txBody", this.nameSpace);
      RichTextString richText = (RichTextString) this.shape.TextFrame.TextRange.RichText;
      this.shape.TextFrame.TextBodyProperties.SerialzieTextBodyProperties(xmlTextWriter, "a", "http://schemas.openxmlformats.org/drawingml/2006/main");
      WorksheetImpl worksheet = this.shape.Worksheet;
      WorksheetBaseImpl parentSheet = this.shape.ParentSheet;
      if (worksheet != null)
        TextBoxSerializator.SerializeParagraphsAutoShapes(xmlTextWriter, richText, worksheet.ParentWorkbook, this.shape.TextFrame);
      else if (parentSheet != null)
        TextBoxSerializator.SerializeParagraphsAutoShapes(xmlTextWriter, richText, parentSheet.ParentWorkbook, this.shape.TextFrame);
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
