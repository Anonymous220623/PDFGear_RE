// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.TextBoxSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class TextBoxSerializator : DrawingShapeSerializator
{
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
    TextBoxShapeImpl textBoxShapeImpl = shape as TextBoxShapeImpl;
    bool flag = false;
    if (textBoxShapeImpl == null)
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
    else if (!(shape.Workbook as WorkbookImpl).IsCreated && shape.ClientAnchor.OneCellAnchor)
    {
      flag = true;
      writer.WriteStartElement("oneCellAnchor", str);
    }
    else if (worksheet != null)
    {
      writer.WriteStartElement("twoCellAnchor", str);
      writer.WriteAttributeString("editAs", DrawingShapeSerializator.GetEditAsValue(shape));
    }
    else
      writer.WriteStartElement("relSizeAnchor", str);
    this.SerializeAnchorPoint(writer, "from", shape.LeftColumn, shape.LeftColumnOffset, shape.TopRow, shape.TopRowOffset, (WorksheetBaseImpl) worksheet, str);
    if (!flag)
      this.SerializeAnchorPoint(writer, "to", shape.RightColumn, shape.RightColumnOffset, shape.BottomRow, shape.BottomRowOffset, (WorksheetBaseImpl) worksheet, str);
    else
      this.SerializeExtent(writer, shape);
    this.SerializeTextBox(writer, shape, holder, str);
    if (worksheet != null)
    {
      writer.WriteStartElement("clientData", str);
      Excel2007Serializator.SerializeAttribute(writer, "fLocksWithSheet", textBoxShapeImpl.LockWithSheet, true);
      Excel2007Serializator.SerializeAttribute(writer, "fPrintsWithSheet", textBoxShapeImpl.PrintWithSheet, true);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    if (!shape.EnableAlternateContent)
      return;
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void SerializeTextBox(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    string DrawingsNamespace)
  {
    TextBoxShapeImpl textBox = shape as TextBoxShapeImpl;
    if (shape.IsEquationShape)
    {
      Stream xmlDataStream = shape.XmlDataStream;
      xmlDataStream.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(xmlDataStream);
      writer.WriteNode(reader, false);
    }
    else
    {
      writer.WriteStartElement("sp", DrawingsNamespace);
      Excel2007Serializator.SerializeAttribute(writer, "fLocksText", textBox.LocksText, true);
      Excel2007Serializator.SerializeAttribute(writer, "macro", textBox.OnAction, (string) null);
      Excel2007Serializator.SerializeAttribute(writer, "textlink", textBox.TextLink, (string) null);
      this.SerializeNonVisualProperties(writer, textBox, holder, DrawingsNamespace);
      this.SerializeShapeProperites(writer, textBox, holder.ParentHolder, holder.DrawingsRelations, DrawingsNamespace);
      if (textBox.StyleStream != null)
      {
        Stream styleStream = textBox.StyleStream;
        styleStream.Position = 0L;
        XmlReader reader = UtilityMethods.CreateReader(styleStream);
        writer.WriteNode(reader, false);
      }
      TextBoxSerializator.SerializeRichText(writer, DrawingsNamespace, (TextBoxShapeBase) textBox);
      writer.WriteEndElement();
    }
  }

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
    if (textBox.Group != null)
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", (int) textBox.ShapeFrame.OffsetX, (int) textBox.ShapeFrame.OffsetY, (int) textBox.ShapeFrame.OffsetCX, (int) textBox.ShapeFrame.OffsetCY, (IShape) textBox);
    else
      DrawingShapeSerializator.SerializeForm(writer, "http://schemas.openxmlformats.org/drawingml/2006/main", "http://schemas.openxmlformats.org/drawingml/2006/main", coordinates2007.X, coordinates2007.Y, coordinates2007.Width, coordinates2007.Height, (IShape) textBox);
    this.SerializePresetGeometry(writer, textBox.PresetGeometry);
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
    if (textBox.NoChangeAspect)
    {
      writer.WriteStartElement("spLocks", drawingsNamespace);
      Excel2007Serializator.SerializeAttribute(writer, "noChangeAspect", textBox.NoChangeAspect, false);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeExtent(XmlWriter writer, ShapeImpl shape)
  {
    writer.WriteStartElement("ext", "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    int num1 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Width, MeasureUnits.EMU);
    writer.WriteAttributeString("cx", num1.ToString());
    int num2 = (int) ApplicationImpl.ConvertFromPixel((double) shape.Height, MeasureUnits.EMU);
    writer.WriteAttributeString("cy", num2.ToString());
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
      {
        if (!keyValuePair.Key.Equals("lIns") && !keyValuePair.Key.Equals("tIns") && !keyValuePair.Key.Equals("rIns"))
          writer.WriteAttributeString(keyValuePair.Key, keyValuePair.Value);
      }
    }
    if (textBox is TextBoxShapeImpl && !(textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.IsAutoMargins)
    {
      writer.WriteAttributeString("lIns", (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.GetLeftMargin().ToString());
      writer.WriteAttributeString("rIns", (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.GetRightMargin().ToString());
      writer.WriteAttributeString("tIns", (textBox as TextBoxShapeImpl).TextBodyPropertiesHolder.GetTopMargin().ToString());
    }
    TextBoxSerializator.SerializeTextRotation(writer, textBox);
    TextBoxSerializator.SerializeAnchor(writer, textBox);
    if (textBox is TextBoxShapeImpl textBoxShapeImpl && textBoxShapeImpl.IsAutoSize)
    {
      writer.WriteStartElement("spAutoFit", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeAnchor(XmlWriter writer, TextBoxShapeBase textBox)
  {
    TextBoxShapeImpl textBoxShapeImpl = textBox as TextBoxShapeImpl;
    string str = textBoxShapeImpl == null || !textBoxShapeImpl.IsCreated ? ((Excel2007CommentVAlign) textBox.VAlignment).ToString() : ((Excel2007CommentVAlign) TextBoxSerializator.GetVerticalAnchorPosition(textBox.TextRotation, textBox.VAlignment, textBox.HAlignment)).ToString();
    if (!(str != "t"))
      return;
    writer.WriteAttributeString("anchor", str);
  }

  private static void SerializeTextRotation(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (textBox.TextRotation == ExcelTextRotation.LeftToRight)
      return;
    writer.WriteAttributeString("vert", ((Excel2007TextRotation) textBox.TextRotation).ToString());
  }

  private static void SerializeListStyles(XmlWriter writer, RichTextString textArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    if (textArea.LevelStyleStream == null)
      return;
    Stream levelStyleStream = textArea.LevelStyleStream;
    levelStyleStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(levelStyleStream);
    writer.WriteNode(reader, false);
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
    bool flag1 = false;
    writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
    TextWithFormat textWithFormat = textArea.TextObject;
    int startIndex = 0;
    WorkbookImpl workbook = (WorkbookImpl) textBox.Workbook;
    FontsCollection innerFonts = workbook.InnerFonts;
    string text = textWithFormat.Text;
    int index1 = textArea.DefaultFontIndex;
    if (index1 < 0 || index1 >= workbook.InnerFonts.Count)
      index1 = 0;
    TextBoxShapeImpl textBoxShapeImpl = textBox as TextBoxShapeImpl;
    IFont font = innerFonts[index1];
    if (textBoxShapeImpl != null && textBoxShapeImpl.IsCreated)
    {
      if (textBox.HAlignment != ExcelCommentHAlign.Left || textBox.TextRotation == ExcelTextRotation.Clockwise || textBox.TextRotation == ExcelTextRotation.CounterClockwise)
        (font as FontImpl).ParaAlign = (Excel2007CommentHAlign) TextBoxSerializator.GetHorizontalAnchorPostion(textBox.TextRotation, textBox.VAlignment, textBox.HAlignment);
    }
    else
      (font as FontImpl).ParaAlign = (Excel2007CommentHAlign) textBox.HAlignment;
    TextBoxSerializator.SerializeFormattingRunProperty(writer, (IWorkbook) workbook, font, textArea);
    bool flag2 = false;
    if (textBoxShapeImpl != null)
    {
      flag2 = textBoxShapeImpl.FldElementStream != null && !string.IsNullOrEmpty(textBoxShapeImpl.TextLink);
      if (textBoxShapeImpl.FldElementStream != null && (!textBoxShapeImpl.IsFldText || flag2))
        Excel2007Serializator.SerializeStream(writer, textBoxShapeImpl.FldElementStream, "root");
      else if (text == null || text.Length <= 0 || text == "\n")
        TextBoxSerializator.SerializeTextFeildElement(writer, textBox);
    }
    if (textWithFormat.FormattingRunsCount > 0 && textWithFormat.GetPositionByIndex(0) != 0)
    {
      textWithFormat = textWithFormat.TypedClone();
      textWithFormat.FormattingRuns[0] = index1;
    }
    if (flag2)
    {
      writer.WriteEndElement();
    }
    else
    {
      int formattingRunsCount = textWithFormat.FormattingRunsCount;
      for (int iIndex = 0; iIndex < formattingRunsCount; ++iIndex)
      {
        int fontByIndex = textWithFormat.GetFontByIndex(iIndex);
        int num = (iIndex != formattingRunsCount - 1 ? textWithFormat.GetPositionByIndex(iIndex + 1) : text.Length) - 1;
        font = innerFonts[fontByIndex];
        string str = text.Substring(startIndex, num - startIndex + 1);
        string[] separator;
        if (textBoxShapeImpl != null && textBoxShapeImpl.IsCreated)
          separator = new string[1]{ "\r\n" };
        else
          separator = new string[1]{ "\n" };
        string[] strArray = str.Split(separator, StringSplitOptions.None);
        if (textArea.Bullet == null || iIndex + 1 != formattingRunsCount || !(strArray[0] == "\n"))
        {
          int index2 = 0;
          for (int length = strArray.Length; index2 < length; ++index2)
          {
            TextBoxSerializator.SerializeFormattingRun(writer, font, "rPr", (IWorkbook) workbook, strArray[index2], textBox);
            if (index2 != length - 1)
            {
              flag1 = true;
              TextBoxSerializator.SerializeParagraphRunProperites(writer, font, "endParaRPr", (IWorkbook) workbook, false, textArea);
              if (iIndex != formattingRunsCount - 1 || index2 + 1 != length - 1 || strArray[length - 1].Length != 0)
              {
                flag1 = false;
                writer.WriteEndElement();
                writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
                if (textBoxShapeImpl.IsCreated && (textBox.HAlignment != ExcelCommentHAlign.Left || textBox.TextRotation == ExcelTextRotation.Clockwise || textBox.TextRotation == ExcelTextRotation.CounterClockwise))
                  (font as FontImpl).ParaAlign = (Excel2007CommentHAlign) TextBoxSerializator.GetHorizontalAnchorPostion(textBox.TextRotation, textBox.VAlignment, textBox.HAlignment);
                else if (textBox.HAlignment != ExcelCommentHAlign.Left)
                  (font as FontImpl).ParaAlign = (Excel2007CommentHAlign) textBox.HAlignment;
                TextBoxSerializator.SerializeFormattingRunProperty(writer, (IWorkbook) workbook, font, textArea);
              }
            }
          }
          startIndex = num + 1;
        }
      }
      if (formattingRunsCount == 0 && text != null && text.Length > 0)
        TextBoxSerializator.SerializeFormattingRun(writer, font, "rPr", (IWorkbook) workbook, text, textBox);
      if (!flag1)
        TextBoxSerializator.SerializeParagraphRunProperites(writer, font, "endParaRPr", (IWorkbook) workbook, false, textArea);
      writer.WriteEndElement();
    }
  }

  private static void SerializeBulletFormat(XmlWriter writer, BulletImpl bulletImpl)
  {
    if (bulletImpl.Type == BulletType.None)
      return;
    writer.WriteStartElement("buFont", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("typeface", bulletImpl.TypeFace);
    writer.WriteAttributeString("panose", bulletImpl.Panose);
    writer.WriteAttributeString("pitchFamily", bulletImpl.PitchFamily.ToString());
    writer.WriteAttributeString("charset", bulletImpl.CharSet.ToString());
    writer.WriteEndElement();
    writer.WriteStartElement("buChar", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("char", bulletImpl.BulletChar);
    writer.WriteEndElement();
  }

  internal static ExcelCommentVAlign GetVerticalAnchorPosition(
    ExcelTextRotation textDirection,
    ExcelCommentVAlign verticalAlignment,
    ExcelCommentHAlign horizontalAlignment)
  {
    switch (textDirection)
    {
      case ExcelTextRotation.CounterClockwise:
        switch (horizontalAlignment)
        {
          case ExcelCommentHAlign.Left:
            return ExcelCommentVAlign.Top;
          case ExcelCommentHAlign.Center:
            return ExcelCommentVAlign.Center;
          case ExcelCommentHAlign.Right:
            return ExcelCommentVAlign.Bottom;
        }
        break;
      case ExcelTextRotation.Clockwise:
        switch (horizontalAlignment)
        {
          case ExcelCommentHAlign.Left:
            return ExcelCommentVAlign.Bottom;
          case ExcelCommentHAlign.Center:
            return ExcelCommentVAlign.Center;
          case ExcelCommentHAlign.Right:
            return ExcelCommentVAlign.Top;
        }
        break;
    }
    return verticalAlignment;
  }

  internal static ExcelCommentHAlign GetHorizontalAnchorPostion(
    ExcelTextRotation textDirection,
    ExcelCommentVAlign verticalAlignment,
    ExcelCommentHAlign horizontalAlignment)
  {
    switch (textDirection)
    {
      case ExcelTextRotation.CounterClockwise:
        switch (verticalAlignment)
        {
          case ExcelCommentVAlign.Top:
            return ExcelCommentHAlign.Left;
          case ExcelCommentVAlign.Center:
            return ExcelCommentHAlign.Center;
          case ExcelCommentVAlign.Bottom:
            return ExcelCommentHAlign.Right;
        }
        break;
      case ExcelTextRotation.Clockwise:
        switch (verticalAlignment)
        {
          case ExcelCommentVAlign.Top:
            return ExcelCommentHAlign.Right;
          case ExcelCommentVAlign.Center:
            return ExcelCommentHAlign.Center;
          case ExcelCommentVAlign.Bottom:
            return ExcelCommentHAlign.Left;
        }
        break;
    }
    return horizontalAlignment;
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
    IFont font = range.CellStyle.Font;
    string str1 = textBoxShapeImpl.FieldId;
    string str2 = textBoxShapeImpl.FieldType;
    if (worksheet.CalcEngine == null)
      worksheet.EnableSheetCalculations();
    if (str1 == null || str1.Length < 0)
      str1 = $"{{{Guid.NewGuid().ToString().ToUpper()}}}";
    if (str2 == null || str2.Length < 0)
      str2 = "TxLink";
    writer.WriteStartElement("fld", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("id", str1);
    writer.WriteAttributeString("type", str2);
    RichTextString richText = textBox.RichText as RichTextString;
    TextBoxSerializator.SerializeParagraphRunProperites(writer, font, "rPr", workbook, true, richText);
    writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteString(range.DisplayText);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal static void SerializeParagraphProperties(XmlWriter writer, TextFrame textFrame)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textFrame == null)
      throw new ArgumentNullException(nameof (textFrame));
    string align = "l";
    textFrame.TextBodyProperties.GetHorizontalAnchorPostion(textFrame.TextDirection, textFrame.VerticalAlignment, textFrame.HorizontalAlignment, out align);
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("algn", align);
    if (textFrame.TextRange != null && textFrame.TextRange.RichText != null && (textFrame.TextRange.RichText as RichTextString).Bullet != null)
    {
      writer.WriteAttributeString("marL", "171450");
      writer.WriteAttributeString("indent", "-171450");
      TextBoxSerializator.SerializeBulletFormat(writer, (textFrame.TextRange.RichText as RichTextString).Bullet);
    }
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
    Excel2007CommentHAlign excel2007CommentHalign1 = textBox != null ? (Excel2007CommentHAlign) textBox.HAlignment : throw new ArgumentNullException(nameof (textBox));
    Excel2007CommentHAlign excel2007CommentHalign2 = Excel2007CommentHAlign.l;
    if (excel2007CommentHalign1 == excel2007CommentHalign2)
      return;
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    Excel2007Serializator.SerializeAttribute(writer, "algn", excel2007CommentHalign1.ToString(), excel2007CommentHalign2.ToString());
    writer.WriteEndElement();
  }

  private static void SerializeFormattingRunProperty(
    XmlWriter writer,
    IWorkbook book,
    IFont font,
    RichTextString textArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    Excel2007CommentHAlign excel2007CommentHalign = Excel2007CommentHAlign.l;
    if (!((font as FontImpl).ParaAlign.ToString() != excel2007CommentHalign.ToString()) && textArea.Bullet == null)
      return;
    writer.WriteStartElement("pPr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    if (textArea.Bullet != null)
    {
      writer.WriteAttributeString("marL", "171450");
      writer.WriteAttributeString("indent", "-171450");
    }
    if ((font as FontImpl).ParaAlign.ToString() != excel2007CommentHalign.ToString())
      Excel2007Serializator.SerializeAttribute(writer, "algn", (font as FontImpl).ParaAlign.ToString(), excel2007CommentHalign.ToString());
    if (textArea.Bullet != null)
      TextBoxSerializator.SerializeBulletFormat(writer, textArea.Bullet);
    writer.WriteEndElement();
  }

  private static void SerializeFormattingRun(
    XmlWriter writer,
    IFont font,
    string tagName,
    IWorkbook book,
    string text,
    TextBoxShapeBase textBox)
  {
    if (text.Length <= 0)
      return;
    writer.WriteStartElement("r", "http://schemas.openxmlformats.org/drawingml/2006/main");
    RichTextString rtfStringArea = (RichTextString) null;
    if (textBox != null)
      rtfStringArea = textBox.RichText as RichTextString;
    TextBoxSerializator.SerializeParagraphRunProperites(writer, font, tagName, book, false, rtfStringArea);
    writer.WriteStartElement("t", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteString(text);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  public static void SerializeParagraphRunProperites(
    XmlWriter writer,
    IFont font,
    string mainTagName,
    IWorkbook book,
    bool isTextLink,
    RichTextString rtfStringArea)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (font == null)
      throw new ArgumentNullException("textArea");
    if (mainTagName == null || mainTagName.Length == 0)
      throw new ArgumentException(nameof (mainTagName));
    writer.WriteStartElement(mainTagName, "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("lang", CultureInfo.CurrentCulture.Name);
    string str1 = font.Bold ? "1" : "0";
    string str2 = font.Italic ? "1" : "0";
    writer.WriteAttributeString("b", str1);
    writer.WriteAttributeString("i", str2);
    if (font.Strikethrough)
    {
      if (rtfStringArea != null && rtfStringArea.IsDoubleStrike)
        writer.WriteAttributeString("strike", "dblStrike");
      else
        writer.WriteAttributeString("strike", "sngStrike");
    }
    int num1 = (int) (font.Size * 100.0);
    writer.WriteAttributeString("sz", num1.ToString());
    if (font.Underline != ExcelUnderline.None)
    {
      string str3 = font.Underline == ExcelUnderline.Single ? "sng" : "dbl";
      writer.WriteAttributeString("u", str3);
    }
    int num2 = 0;
    if (font.Subscript)
      num2 = -25000;
    if (font.Superscript)
      num2 = 30000;
    if (num2 != 0)
      writer.WriteAttributeString("baseline", num2.ToString());
    if (rtfStringArea != null)
    {
      if (rtfStringArea.IsNormalizeHeights.HasValue)
        writer.WriteAttributeString("normalizeH", rtfStringArea.IsNormalizeHeights.Value ? "1" : "0");
      if (rtfStringArea.IsCapsUsed)
      {
        if (rtfStringArea.CapitalizationType == TextCapsType.All)
          writer.WriteAttributeString("cap", "all");
        else if (rtfStringArea.CapitalizationType == TextCapsType.Small)
          writer.WriteAttributeString("cap", "small");
        else
          writer.WriteAttributeString("cap", "none");
      }
    }
    writer.WriteStartElement("solidFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    ChartSerializatorCommon.SerializeRgbColor(writer, font.RGBColor);
    writer.WriteEndElement();
    if (isTextLink)
    {
      writer.WriteStartElement("latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", font.FontName);
      writer.WriteEndElement();
    }
    else if ((font as FontImpl).showFontName)
    {
      writer.WriteStartElement("latin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("typeface", font.FontName);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal static void SerializeParagraphsAutoShapes(
    XmlWriter writer,
    RichTextString textArea,
    WorkbookImpl book,
    TextFrame textFrame)
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
    TextBoxSerializator.SerializeParagraphProperties(writer, textFrame);
    string text = textWithFormat.Text;
    int formattingRunsCount = textWithFormat.FormattingRunsCount;
    for (int iIndex = 0; iIndex < formattingRunsCount; ++iIndex)
    {
      int fontByIndex = textWithFormat.GetFontByIndex(iIndex);
      int num = (iIndex != formattingRunsCount - 1 ? textWithFormat.GetPositionByIndex(iIndex + 1) : text.Length) - 1;
      IFont font = innerFonts[fontByIndex];
      string[] strArray = text.Substring(startIndex, num - startIndex + 1).Split('\n');
      int index = 0;
      for (int length = strArray.Length; index < length; ++index)
      {
        TextBoxSerializator.SerializeFormattingRun(writer, font, "rPr", (IWorkbook) book, strArray[index], (TextBoxShapeBase) null);
        if (index != length - 1)
        {
          TextBoxSerializator.SerializeParagraphRunProperites(writer, font, "endParaRPr", (IWorkbook) book, false, textArea);
          writer.WriteEndElement();
          writer.WriteStartElement("p", "http://schemas.openxmlformats.org/drawingml/2006/main");
          TextBoxSerializator.SerializeParagraphProperties(writer, textFrame);
        }
      }
      startIndex = num + 1;
    }
    int index1 = textArea.DefaultFontIndex;
    if (index1 < 0)
      index1 = 0;
    IFont font1 = innerFonts[index1];
    if (formattingRunsCount == 0 && text != null && text.Length > 0)
      TextBoxSerializator.SerializeFormattingRun(writer, font1, "rPr", (IWorkbook) book, text, (TextBoxShapeBase) null);
    TextBoxSerializator.SerializeParagraphRunProperites(writer, font1, "endParaRPr", (IWorkbook) book, false, textArea);
    writer.WriteEndElement();
  }
}
