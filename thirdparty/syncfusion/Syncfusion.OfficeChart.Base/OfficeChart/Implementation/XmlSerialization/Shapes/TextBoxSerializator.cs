// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes.TextBoxSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;

internal class TextBoxSerializator : DrawingShapeSerializator
{
  [SecuritySafeCritical]
  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (!(shape is TextBoxShapeImpl textBox))
      throw new ArgumentOutOfRangeException("textBox");
    FileDataHolder parentHolder = holder.ParentHolder;
    string str = shape.ParentShapes.Worksheet != null ? "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing" : "http://schemas.openxmlformats.org/drawingml/2006/chartDrawing";
    WorksheetImpl worksheet = shape.Worksheet as WorksheetImpl;
    if (shape.EnableAlternateContent)
    {
      writer.WriteStartElement("mc", "AlternateContent", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteStartElement("mc", "Choice", "http://schemas.openxmlformats.org/markup-compatibility/2006");
      writer.WriteAttributeString("xmlns", "a14", (string) null, "http://schemas.microsoft.com/office/drawing/2010/main");
      writer.WriteAttributeString("Requires", "a14");
    }
    if (worksheet != null)
    {
      writer.WriteStartElement("twoCellAnchor", str);
      writer.WriteAttributeString("editAs", DrawingShapeSerializator.GetEditAsValue(shape));
    }
    else
      writer.WriteStartElement("relSizeAnchor", str);
    this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, (WorksheetBaseImpl) worksheet, str);
    this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, (WorksheetBaseImpl) worksheet, str);
    if (shape.IsEquationShape)
    {
      Stream xmlDataStream = shape.XmlDataStream;
      xmlDataStream.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(xmlDataStream);
      writer.WriteNode(reader, false);
    }
    else
    {
      writer.WriteStartElement("sp", str);
      Excel2007Serializator.SerializeAttribute(writer, "fLocksText", textBox.IsTextLocked, true);
      Excel2007Serializator.SerializeAttribute(writer, "textlink", textBox.TextLink, (string) null);
      this.SerializeNonVisualProperties(writer, textBox, holder, str);
      this.SerializeShapeProperites(writer, textBox, parentHolder, holder.Relations, str);
      TextBoxSerializator.SerializeRichText(writer, str, (TextBoxShapeBase) textBox);
      writer.WriteEndElement();
    }
    if (worksheet != null)
      writer.WriteElementString("clientData", str, string.Empty);
    writer.WriteEndElement();
    if (!shape.EnableAlternateContent)
      return;
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  [SecurityCritical]
  private void SerializeShapeProperites(
    XmlWriter writer,
    TextBoxShapeImpl textBox,
    FileDataHolder holder,
    RelationCollection relations,
    string drawingsNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    writer.WriteStartElement("spPr", drawingsNamespace);
    Rectangle coordinates2007 = textBox.Coordinates2007;
    DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", coordinates2007.X, coordinates2007.Y, coordinates2007.Width, coordinates2007.Height, (IShape) textBox);
    this.SerializePresetGeometry(writer);
    this.SerializeFill(writer, (ShapeImpl) textBox, holder, relations);
    writer.WriteEndElement();
  }

  private void SerializeNonVisualProperties(
    XmlWriter writer,
    TextBoxShapeImpl textBox,
    WorksheetDataHolder holder,
    string drawingsNamespace)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    writer.WriteStartElement("nvSpPr", drawingsNamespace);
    this.SerializeNVCanvasProperties(writer, (ShapeImpl) textBox, holder, drawingsNamespace);
    writer.WriteStartElement("cNvSpPr", drawingsNamespace);
    writer.WriteAttributeString("txBox", "1");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeRichText(
    XmlWriter writer,
    string drawingsNamespace,
    TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    RichTextString textArea = textBox != null ? (RichTextString) textBox.RichText : throw new ArgumentNullException(nameof (textBox));
    writer.WriteStartElement("txBody", drawingsNamespace);
    TextBoxSerializator.SerializeBodyProperties(writer, textArea, textBox);
    TextBoxSerializator.SerializeListStyles(writer, textArea);
    TextBoxSerializator.SerializeParagraphs(writer, textArea, textBox);
    writer.WriteEndElement();
  }

  private static void SerializeBodyProperties(
    XmlWriter writer,
    RichTextString textArea,
    TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("bodyPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Dictionary<string, string> unknownBodyProperties = textBox.UnknownBodyProperties;
    if (unknownBodyProperties != null && unknownBodyProperties.Count > 0)
    {
      foreach (KeyValuePair<string, string> keyValuePair in unknownBodyProperties)
        writer.WriteAttributeString(keyValuePair.Key, keyValuePair.Value);
    }
    TextBoxSerializator.SerializeTextRotation(writer, textBox);
    TextBoxSerializator.SerializeAnchor(writer, textBox);
    writer.WriteEndElement();
  }

  private static void SerializeAnchor(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (textBox.VAlignment == OfficeCommentVAlign.Top)
      return;
    writer.WriteAttributeString("anchor", ((Excel2007CommentVAlign) textBox.VAlignment).ToString());
  }

  private static void SerializeTextRotation(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (textBox.TextRotation == OfficeTextRotation.LeftToRight)
      return;
    writer.WriteAttributeString("vert", ((Excel2007TextRotation) textBox.TextRotation).ToString());
  }

  private static void SerializeListStyles(XmlWriter writer, RichTextString textArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("lstStyle", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteEndElement();
  }

  private static void SerializeParagraphs(
    XmlWriter writer,
    RichTextString textArea,
    TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    TextWithFormat textWithFormat = textArea.TextObject;
    int startIndex = 0;
    WorkbookImpl workbook = (WorkbookImpl) textBox.Workbook;
    FontsCollection innerFonts = workbook.InnerFonts;
    string text = textWithFormat.Text;
    if (text == null || text.Length <= 0 || text == "\n")
      TextBoxSerializator.SerializeTextFeildElement(writer, textBox);
    if (textWithFormat.FormattingRunsCount > 0 && textWithFormat.GetPositionByIndex(0) != 0)
    {
      textWithFormat = textWithFormat.TypedClone();
      int defaultFontIndex = textArea.DefaultFontIndex;
      textWithFormat.FormattingRuns[0] = defaultFontIndex >= 0 ? defaultFontIndex : 0;
    }
    int formattingRunsCount = textWithFormat.FormattingRunsCount;
    for (int iIndex = 0; iIndex < formattingRunsCount; ++iIndex)
    {
      int fontByIndex = textWithFormat.GetFontByIndex(iIndex);
      int num = (iIndex != formattingRunsCount - 1 ? textWithFormat.GetPositionByIndex(iIndex + 1) : text.Length) - 1;
      IOfficeFont officeFont = innerFonts[fontByIndex];
      string[] strArray = text.Substring(startIndex, num - startIndex + 1).Split(new string[1]
      {
        "\n"
      }, StringSplitOptions.None);
      TextBoxSerializator.SerializeFormattingRunProperty(writer, (IWorkbook) workbook, officeFont);
      int index = 0;
      for (int length = strArray.Length; index < length; ++index)
      {
        TextBoxSerializator.SerializeFormattingRun(writer, officeFont, "rPr", (IWorkbook) workbook, strArray[index], textBox);
        if (index != length - 1)
        {
          TextBoxSerializator.SerializeParagraphRunProperites(writer, officeFont, "endParaRPr", (IWorkbook) workbook, false);
          writer.WriteEndElement();
          writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
        }
      }
      startIndex = num + 1;
    }
    int index1 = textArea.DefaultFontIndex;
    if (index1 < 0)
      index1 = 0;
    IOfficeFont officeFont1 = innerFonts[index1];
    if (formattingRunsCount == 0 && text != null && text.Length > 0)
      TextBoxSerializator.SerializeFormattingRun(writer, officeFont1, "rPr", (IWorkbook) workbook, text, textBox);
    TextBoxSerializator.SerializeParagraphRunProperites(writer, officeFont1, "endParaRPr", (IWorkbook) workbook, false);
    writer.WriteEndElement();
  }

  private static void SerializeTextFeildElement(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (!(textBox is TextBoxShapeImpl textBoxShapeImpl))
      return;
    string textLink = textBoxShapeImpl.TextLink;
    if (textLink == null || textLink.Length <= 0)
      return;
    string name = textLink.Substring(1, textLink.Length - 1);
    IWorkbook workbook = textBoxShapeImpl.Workbook;
    if (!(textBoxShapeImpl.Worksheet is IWorksheet worksheet))
      return;
    IRange range = worksheet.Range[name];
    IOfficeFont font = range.CellStyle.Font;
    string str1 = textBoxShapeImpl.FieldId;
    string str2 = textBoxShapeImpl.FieldType;
    if (str1 == null || str1.Length < 0)
      str1 = $"{{{Guid.NewGuid().ToString().ToUpper()}}}";
    if (str2 == null || str2.Length < 0)
      str2 = "TxLink";
    writer.WriteStartElement("fld", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("id", str1);
    writer.WriteAttributeString("type", str2);
    TextBoxSerializator.SerializeParagraphRunProperites(writer, font, "rPr", workbook, true);
    writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteString(range.DisplayText);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private static void SerializeFormattingRunProperty(
    XmlWriter writer,
    IWorkbook book,
    TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Excel2007CommentHAlign halignment = (Excel2007CommentHAlign) textBox.HAlignment;
    Excel2007CommentHAlign excel2007CommentHalign = Excel2007CommentHAlign.l;
    Excel2007Serializator.SerializeAttribute(writer, "algn", halignment.ToString(), excel2007CommentHalign.ToString());
    writer.WriteEndElement();
  }

  private static void SerializeFormattingRunProperty(
    XmlWriter writer,
    IWorkbook book,
    IOfficeFont font)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Excel2007CommentHAlign excel2007CommentHalign = Excel2007CommentHAlign.l;
    Excel2007Serializator.SerializeAttribute(writer, "algn", (font as FontImpl).ParaAlign.ToString(), excel2007CommentHalign.ToString());
    writer.WriteEndElement();
  }

  private static void SerializeFormattingRun(
    XmlWriter writer,
    IOfficeFont font,
    string tagName,
    IWorkbook book,
    string text,
    TextBoxShapeBase textBox)
  {
    if (text.Length <= 0)
      return;
    writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
    TextBoxSerializator.SerializeParagraphRunProperites(writer, font, tagName, book, false);
    writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteString(text);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeParagraphRunProperites(
    XmlWriter writer,
    IOfficeFont textArea,
    string mainTagName,
    IWorkbook book,
    bool isTextLink)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (mainTagName == null || mainTagName.Length == 0)
      throw new ArgumentException(nameof (mainTagName));
    writer.WriteStartElement(mainTagName, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("lang", CultureInfo.CurrentCulture.Name);
    string str1 = textArea.Bold ? "1" : "0";
    string str2 = textArea.Italic ? "1" : "0";
    writer.WriteAttributeString("b", str1);
    writer.WriteAttributeString("i", str2);
    if (textArea.Strikethrough)
      writer.WriteAttributeString("strike", "sngStrike");
    int num1 = (int) (textArea.Size * 100.0);
    writer.WriteAttributeString("sz", num1.ToString());
    if (textArea.Underline != OfficeUnderline.None)
    {
      string str3 = textArea.Underline == OfficeUnderline.Single ? "sng" : "dbl";
      writer.WriteAttributeString("u", str3);
    }
    int num2 = 0;
    if (textArea.Subscript)
      num2 = -25000;
    if (textArea.Superscript)
      num2 = 30000;
    if (num2 != 0)
      writer.WriteAttributeString("baseline", num2.ToString());
    writer.WriteStartElement("solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    ChartSerializatorCommon.SerializeRgbColor(writer, textArea.Color, book);
    writer.WriteEndElement();
    if (isTextLink)
    {
      writer.WriteStartElement("latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", textArea.FontName);
      writer.WriteEndElement();
    }
    else if ((textArea as FontImpl).showFontName)
    {
      writer.WriteStartElement("latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", textArea.FontName);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal static void SerializeParagraphsAutoShapes(
    XmlWriter writer,
    RichTextString textArea,
    WorkbookImpl book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    TextWithFormat textWithFormat = textArea.TextObject;
    int startIndex = 0;
    FontsCollection innerFonts = book.InnerFonts;
    if (textWithFormat.FormattingRunsCount > 0 && textWithFormat.GetPositionByIndex(0) != 0)
    {
      textWithFormat = textWithFormat.TypedClone();
      int defaultFontIndex = textArea.DefaultFontIndex;
      textWithFormat.FormattingRuns[0] = defaultFontIndex >= 0 ? defaultFontIndex : 0;
    }
    string text = textWithFormat.Text;
    int formattingRunsCount = textWithFormat.FormattingRunsCount;
    for (int iIndex = 0; iIndex < formattingRunsCount; ++iIndex)
    {
      int fontByIndex = textWithFormat.GetFontByIndex(iIndex);
      int num = (iIndex != formattingRunsCount - 1 ? textWithFormat.GetPositionByIndex(iIndex + 1) : text.Length) - 1;
      IOfficeFont officeFont = innerFonts[fontByIndex];
      string[] strArray = text.Substring(startIndex, num - startIndex + 1).Split('\n');
      int index = 0;
      for (int length = strArray.Length; index < length; ++index)
      {
        TextBoxSerializator.SerializeFormattingRun(writer, officeFont, "rPr", (IWorkbook) book, strArray[index], (TextBoxShapeBase) null);
        if (index != length - 1)
        {
          TextBoxSerializator.SerializeParagraphRunProperites(writer, officeFont, "endParaRPr", (IWorkbook) book, false);
          writer.WriteEndElement();
          writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
        }
      }
      startIndex = num + 1;
    }
    int index1 = textArea.DefaultFontIndex;
    if (index1 < 0)
      index1 = 0;
    IOfficeFont officeFont1 = innerFonts[index1];
    if (formattingRunsCount == 0 && text != null && text.Length > 0)
      TextBoxSerializator.SerializeFormattingRun(writer, officeFont1, "rPr", (IWorkbook) book, text, (TextBoxShapeBase) null);
    TextBoxSerializator.SerializeParagraphRunProperites(writer, officeFont1, "endParaRPr", (IWorkbook) book, false);
    writer.WriteEndElement();
  }
}
