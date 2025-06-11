// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.DLSXmlSerializator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Interfaces.XmlSerialization;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

[XmlSerializator(OfficeXmlSaveType.DLS)]
internal class DLSXmlSerializator : IXmlSerializator
{
  private const string DEF_DLS_START = "DLS";
  private const string DEF_PROTECTION_ATTRIBUTE = "ProtectionType";
  private const string DEF_PROTECTION_VALUE = "NoProtection";
  private const string DEF_STYLES_START = "styles";
  private const string DEF_STYLE_START = "style";
  private const string DEF_ID_ATTRIBUTE = "id";
  private const string DEF_NAME_ATTRIBUTE = "Name";
  private const string DEF_TYPE_ATTRIBUTE = "type";
  private const string DEF_SECTIONS_START = "sections";
  private const string DEF_SECTION_START = "section";
  private const string DEF_BREAK_CODE_ATTRIBUTE = "BreakCode";
  private const string DEF_PARAGRAPHS_START = "paragraphs";
  private const string DEF_PARAGRAPH_START = "paragraph";
  private const string DEF_ITEMS_START = "items";
  private const string DEF_ITEM_START = "item";
  private const string DEF_ROWS_START = "rows";
  private const string DEF_ROW_START = "row";
  private const string DEF_CELLS_START = "cells";
  private const string DEF_CELL_START = "cell";
  private const string DEF_WIDTH_ATTRIBUTE = "Width";
  private const string DEF_TEXT_RANGE_ATTRIBUTE = "TextRange";
  private const string DEF_TEXT_START = "text";
  private const string DEF_COLUMNS_COUNT_ATTRIBUTE = "ColumnsCount";
  private const string DEF_FORMAT_START = "format";
  private const string DEF_FONT_NAME_ATTRIBUTE = "FontName";
  private const string DEF_FONT_SIZE_ATTRIBUTE = "FontSize";
  private const string DEF_BOLD_ATTRIBUTE = "Bold";
  private const string DEF_ITALIC_ATTRIBUTE = "Italic";
  private const string DEF_UNDERLINE_ATTRIBUTE = "Underline";
  private const string DEF_TEXT_COLOR_ATTRIBUTE = "TextColor";
  private const string DEF_COLOR_PREFIX = "#";
  private const string DEF_UNDERLINE_NONE = "None";
  private const string DEF_UNDERLINE_SINGLE = "Single";
  private const string DEF_UNDERLINE_DOUBLE = "Double";
  private const string DEF_SUBCRIPT = "SubScript";
  private const string DEF_SUPSCRIPT = "SuperScript";
  private const string DEF_NO_SUBSUPERSCIRPT = "None";
  private const string DEF_SUBSUPERSCRIPT_ATTRIBUTE = "SubSuperScript";
  private const string DEF_STRIKEOUT_ATTRIBUTE = "Strike";
  private const string DEF_TABLE_FORMAT_START = "cell-format";
  private const string DEF_CHARACTER_FORMAT_START = "character-format";
  private const string DEF_BORDERS_START = "borders";
  private const string DEF_BORDER_START = "border";
  private const string DEF_COLOR_ATTRIBUTE = "Color";
  private const string DEF_LINE_WIDTH_ATTRIBUTE = "LineWidth";
  private const string DEF_BORDER_TYPE_ATTRIBUTE = "BorderType";
  private const string DEF_BORDER_WIDTH_NONE = "0";
  private const string DEF_BORDER_TYPE_SIGNLE = "Single";
  private const string DEF_BORDER_TYPE_DOUBLE = "Double";
  private const string DEF_BORDER_TYPE_DOT = "Dot";
  private const string DEF_BORDER_TYPE_DASH_SMALL = "DashSmallGap";
  private const string DEF_BORDER_TYPE_DOT_DASH = "DotDash";
  private const string DEF_BORDER_TYPE_DOT_DOT_DASH = "DotDotDash";
  private const string DEF_BORDER_TYPE_THICK = "Thick";
  private const string DEF_BORDER_TYPE_NONE = "None";
  private const string DEF_PAGE_SETTINGS_START = "page-setup";
  private const string DEF_PAGE_HEIGHT_ATTRIBUTE = "PageHeight";
  private const string DEF_PAGE_WIDTH_ATTRIBUTE = "PageWidth";
  private const string DEF_FOOTER_DISTANCE_ATTRIBUTE = "FooterDistance";
  private const string DEF_HEADER_DISTANCE_ATTRIBUTE = "HeaderDistance";
  private const string DEF_TOP_MARGIN_ATTRIBUTE = "TopMargin";
  private const string DEF_BOTTOM_MARGIN_ATTRIBUTE = "BottomMargin";
  private const string DEF_LEFT_MARGIN_ATTRIBUTE = "LeftMargin";
  private const string DEF_RIGHT_MARGIN_ATTRIBUTE = "RightMargin";
  private const string DEF_PAGE_BREAK_AFTER_ATTRIBUTE = "PageBreakAfter";
  private const string DEF_ORIENTATION_ATTRIBUTE = "Orientation";
  private const string DEF_PARAGRAPH_FORMAT_START = "paragraph-format";
  private const string DEF_HEADERS_FOOTERS_START = "headers-footers";
  private const string DEF_ITEM_TYPE_TABLE = "Table";
  private const string DEF_EVEN_FOOTER_START = "even-footer";
  private const string DEF_ODD_FOOTER_START = "odd-footer";
  private const string DEF_EVEN_HEADER_START = "even-header";
  private const string DEF_ODD_HEADER_START = "odd-header";
  private const string DEF_ROW_HEIGHT_ATTRIBUTE = "RowHeight";
  private const string DEF_TABLE_SHADOW_COLOR_ATTRIBUTE = "ShadingColor";
  private const int DEF_BORDER_WIDTH = 1;
  private const string DEF_HALIGNMENT_ATTRIBUTE = "HrAlignment";
  private const string DEF_VALIGNMENT_ATTRIBUTE = "VAlignment";
  private const string DEF_ALIGN_CENTER = "Center";
  private const string DEF_ALIGN_TOP = "Top";
  private const string DEF_ALIGN_BOTTOM = "Bottom";
  private const string DEF_ALIGN_MIDDLE = "Middle";
  private const string DEF_ALIGN_LEFT = "Left";
  private const string DEF_ALIGN_RIGHT = "Right";
  private const string DEF_ALIGN_JUSTIFY = "Justify";
  private static readonly string DEF_TRUE_STRING = bool.TrueString.ToLower();
  private static readonly OfficeBordersIndex[] DEF_DLS_BORDERS = new OfficeBordersIndex[4]
  {
    OfficeBordersIndex.EdgeBottom,
    OfficeBordersIndex.EdgeLeft,
    OfficeBordersIndex.EdgeRight,
    OfficeBordersIndex.EdgeTop
  };
  private static readonly string[] DEF_DLS_BORDER_NAMES = new string[4]
  {
    "Bottom",
    "Left",
    "Right",
    "Top"
  };
  private static readonly CultureInfo DLSCulture = CultureInfo.InvariantCulture;
  private static readonly string DEF_BORDER_WIDTH_HAIR = 0.25.ToString((IFormatProvider) DLSXmlSerializator.DLSCulture);
  private static readonly string DEF_BORDER_WIDTH_THIN = 0.5.ToString((IFormatProvider) DLSXmlSerializator.DLSCulture);
  private static readonly string DEF_BORDER_WIDTH_MEDIUM = 1.ToString((IFormatProvider) DLSXmlSerializator.DLSCulture);
  private static readonly string DEF_BORDER_WIDTH_THICK = 2.25.ToString((IFormatProvider) DLSXmlSerializator.DLSCulture);

  public void Serialize(XmlWriter writer, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    writer.WriteStartElement("DLS");
    writer.WriteAttributeString("ProtectionType", "NoProtection");
    this.SerializeStyles(writer, book);
    this.SerializeDocumentProperties(writer, book);
    this.SerializeSections(writer, book);
    writer.WriteEndElement();
  }

  private void SerializeStyles(XmlWriter writer, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    writer.WriteStartElement("styles");
    writer.WriteStartElement("style");
    writer.WriteAttributeString("id", "0");
    writer.WriteAttributeString("Name", "Normal");
    writer.WriteAttributeString("type", "ParagraphStyle");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeDocumentProperties(XmlWriter writer, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
  }

  private void SerializeSections(XmlWriter writer, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    writer.WriteStartElement("sections");
    IWorksheets worksheets = book.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
      this.SerializeWorksheet(writer, book.Worksheets[Index]);
    writer.WriteEndElement();
  }

  private void SerializeWorksheet(XmlWriter writer, IWorksheet sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (((WorksheetImpl) sheet).IsEmpty)
      return;
    writer.WriteStartElement("section");
    writer.WriteAttributeString("BreakCode", "NewPage");
    this.SerializePageSettings(writer, (PageSetupImpl) sheet.PageSetup);
    double dPageWidth = this.SerializeParagraphs(writer, sheet);
    this.SerializeHeaderFooter(writer, sheet, dPageWidth);
    writer.WriteEndElement();
  }

  private void SerializePageSettings(XmlWriter writer, PageSetupImpl pageSetup)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    writer.WriteStartElement("page-setup");
    this.WriteAttribute(writer, "PageHeight", pageSetup.PageHeight);
    this.WriteAttribute(writer, "PageWidth", pageSetup.PageWidth);
    this.WriteAttribute(writer, "FooterDistance", pageSetup.FooterMargin, MeasureUnits.Inch);
    this.WriteAttribute(writer, "HeaderDistance", pageSetup.FooterMargin, MeasureUnits.Inch);
    this.WriteAttribute(writer, "TopMargin", pageSetup.FooterMargin, MeasureUnits.Inch);
    this.WriteAttribute(writer, "BottomMargin", pageSetup.BottomMargin, MeasureUnits.Inch);
    this.WriteAttribute(writer, "LeftMargin", pageSetup.LeftMargin, MeasureUnits.Inch);
    this.WriteAttribute(writer, "RightMargin", pageSetup.RightMargin, MeasureUnits.Inch);
    writer.WriteAttributeString("Orientation", pageSetup.Orientation.ToString());
    writer.WriteEndElement();
  }

  private double SerializeParagraphs(XmlWriter writer, IWorksheet sheet)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("paragraphs");
    INames names = sheet.Names;
    IRange range1 = names.Contains(PageSetupImpl.DEF_AREA_XlS) ? names[PageSetupImpl.DEF_AREA_XlS].RefersToRange : sheet.UsedRange;
    SizeF pageSize;
    SizeF headingsSize;
    this.MeasurePageArea(range1, out pageSize, out headingsSize);
    OfficeOrder order = sheet.PageSetup.Order;
    if (range1 is RangesCollection)
    {
      RangesCollection rangesCollection = (RangesCollection) range1;
      int index = 0;
      for (int count = rangesCollection.Count; index < count; ++index)
      {
        IRange range2 = rangesCollection[index];
        this.SerializeRange(writer, range2, pageSize, headingsSize, order);
      }
    }
    else
      this.SerializeRange(writer, range1, pageSize, headingsSize, order);
    writer.WriteEndElement();
    return (double) pageSize.Width;
  }

  private void MeasurePageArea(IRange printArea, out SizeF pageSize, out SizeF headingsSize)
  {
    pageSize = new SizeF(0.0f, 0.0f);
    headingsSize = new SizeF(0.0f, 0.0f);
    IWorksheet worksheet = printArea != null ? printArea.Worksheet : throw new ArgumentNullException(nameof (printArea));
    PageSetupImpl pageSetup = (PageSetupImpl) worksheet.PageSetup;
    double num1 = ApplicationImpl.ConvertUnitsStatic(pageSetup.LeftMargin + pageSetup.RightMargin, MeasureUnits.Inch, MeasureUnits.Point);
    pageSize.Width = (float) (pageSetup.PageWidth - num1);
    double num2 = ApplicationImpl.ConvertUnitsStatic(pageSetup.TopMargin + pageSetup.BottomMargin, MeasureUnits.Inch, MeasureUnits.Point);
    pageSize.Height = (float) (pageSetup.PageHeight - num2);
    int order = (int) pageSetup.Order;
    if (!pageSetup.PrintHeadings)
      return;
    FontImpl wrapped = ((FontWrapper) worksheet.Workbook.Styles["Normal"].Font).Wrapped;
    string strValue = printArea.LastRow.ToString();
    headingsSize.Width = wrapped.MeasureString(strValue).Width;
    headingsSize.Width = (float) ApplicationImpl.ConvertUnitsStatic((double) headingsSize.Width, MeasureUnits.Pixel, MeasureUnits.Point);
    headingsSize.Height = (float) worksheet.StandardHeight;
    pageSize.Width -= headingsSize.Width;
    pageSize.Height -= headingsSize.Height;
  }

  private void SerializeRange(
    XmlWriter writer,
    IRange range,
    SizeF pageSize,
    SizeF headingsSize,
    OfficeOrder pageOrder)
  {
    int iFirstRow = -1;
    int iFirstCol = -1;
    int iLastRow = -1;
    int iLastCol = -1;
    int iRowId = -1;
    IWorksheet worksheet = range.Worksheet;
    IPageSetup pageSetup = worksheet.PageSetup;
    int num1 = 0;
    while (this.FillNextPage(range, ref iFirstRow, ref iFirstCol, ref iLastRow, ref iLastCol, pageSize, pageOrder))
    {
      int num2 = iLastCol - iFirstCol + 1;
      if ((double) headingsSize.Width > 0.0)
        ++num2;
      writer.WriteStartElement("paragraph");
      writer.WriteAttributeString("id", num1.ToString());
      writer.WriteStartElement("paragraph-format");
      this.SerializeBoolAttribute(writer, "PageBreakAfter", true);
      writer.WriteEndElement();
      writer.WriteStartElement("items");
      writer.WriteStartElement("item");
      writer.WriteAttributeString("id", "0");
      writer.WriteAttributeString("type", "Table");
      writer.WriteAttributeString("ColumnsCount", num2.ToString());
      this.WriteEmptyBorders(writer);
      writer.WriteStartElement("rows");
      if ((double) headingsSize.Height > 0.0)
        iRowId = this.SerializeHeadingsRow(writer, worksheet, headingsSize, iFirstCol, iLastCol, iRowId);
      for (int iRow = iFirstRow; iRow <= iLastRow; ++iRow)
        iRowId = this.SerializeRow(writer, worksheet, iRow, iFirstCol, iLastCol, iRowId, headingsSize);
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
      ++num1;
    }
  }

  private void WriteEmptyBorders(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("cell-format");
    writer.WriteStartElement("borders");
    int index = 0;
    for (int length = DLSXmlSerializator.DEF_DLS_BORDER_NAMES.Length; index < length; ++index)
    {
      string localName = DLSXmlSerializator.DEF_DLS_BORDER_NAMES[index];
      writer.WriteStartElement(localName);
      writer.WriteAttributeString("BorderType", "None");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void WriteHeadingsBorders(XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("cell-format");
    writer.WriteStartElement("borders");
    int index = 0;
    for (int length = DLSXmlSerializator.DEF_DLS_BORDER_NAMES.Length; index < length; ++index)
    {
      string localName = DLSXmlSerializator.DEF_DLS_BORDER_NAMES[index];
      writer.WriteStartElement(localName);
      writer.WriteAttributeString("BorderType", "Single");
      writer.WriteAttributeString("Width", "1");
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private bool FillNextPage(
    IRange range,
    ref int iFirstRow,
    ref int iFirstCol,
    ref int iLastRow,
    ref int iLastCol,
    SizeF pageSize,
    OfficeOrder pageOrder)
  {
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (iLastRow == range.LastRow && iLastCol == range.LastColumn)
      return false;
    if (iFirstRow == -1)
    {
      iFirstRow = range.Row;
      iFirstCol = range.Column;
    }
    else if (pageOrder == OfficeOrder.DownThenOver)
    {
      if (iLastRow == range.LastRow)
      {
        iFirstRow = range.Row;
        iFirstCol = iLastCol + 1;
      }
      else
        iFirstRow = iLastRow + 1;
    }
    else if (iLastCol == range.LastColumn)
    {
      iFirstCol = range.Column;
      iFirstRow = iLastRow + 1;
    }
    else
      iFirstCol = iLastCol + 1;
    iLastRow = Math.Min(this.GetMaxRow(range.Worksheet, iFirstRow, (double) pageSize.Height), range.LastRow);
    iLastCol = Math.Min(this.GetMaxColumn(range.Worksheet, iFirstCol, (double) pageSize.Width), range.LastColumn);
    return true;
  }

  private int GetMaxColumn(IWorksheet sheet, int iFirstColumn, double dPageWidth)
  {
    if (dPageWidth <= 0.0)
      throw new ArgumentOutOfRangeException(nameof (dPageWidth));
    if (iFirstColumn <= 0)
      throw new ArgumentOutOfRangeException(nameof (iFirstColumn));
    double num = 0.0;
    int iColumn = iFirstColumn - 1;
    int maxColumn = iFirstColumn;
    double columnWidth;
    for (; num <= dPageWidth; num += columnWidth)
    {
      maxColumn = iColumn;
      ++iColumn;
      if (iColumn <= sheet.Workbook.MaxColumnCount)
        columnWidth = this.GetColumnWidth(sheet, iColumn);
      else
        break;
    }
    return maxColumn;
  }

  private int GetMaxRow(IWorksheet sheet, int iFirstRow, double dPageHeight)
  {
    if (dPageHeight <= 0.0)
      throw new ArgumentOutOfRangeException(nameof (dPageHeight));
    if (iFirstRow <= 0)
      throw new ArgumentOutOfRangeException(nameof (iFirstRow));
    double num1 = 0.0;
    int iRowIndex = iFirstRow - 1;
    int maxRow = iFirstRow;
    int maxRowCount = sheet.Workbook.MaxRowCount;
    double num2;
    for (; num1 <= dPageHeight; num1 += num2)
    {
      maxRow = iRowIndex;
      ++iRowIndex;
      if (iRowIndex <= maxRowCount)
        num2 = ApplicationImpl.ConvertUnitsStatic((double) sheet.GetRowHeightInPixels(iRowIndex), MeasureUnits.Pixel, MeasureUnits.Point);
      else
        break;
    }
    return maxRow;
  }

  private int SerializeRow(
    XmlWriter writer,
    IWorksheet sheet,
    int iRow,
    int iFirstCol,
    int iLastCol,
    int iRowId,
    SizeF headingsSize)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    double num1 = sheet != null ? (double) sheet.GetRowHeightInPixels(iRow) : throw new ArgumentNullException(nameof (sheet));
    if (num1 > 0.0)
    {
      ++iRowId;
      double num2 = ApplicationImpl.ConvertUnitsStatic(num1, MeasureUnits.Pixel, MeasureUnits.Point);
      writer.WriteStartElement("row");
      writer.WriteAttributeString("id", iRowId.ToString());
      this.WriteAttribute(writer, "RowHeight", num2);
      writer.WriteStartElement("cells");
      int iCellId = 0;
      float width = headingsSize.Width;
      if ((double) width > 0.0)
      {
        this.SerializeCell(writer, (double) width, iRow.ToString(), iCellId);
        ++iCellId;
      }
      int iColumn = iFirstCol;
      while (iColumn <= iLastCol)
      {
        this.SerializeCell(writer, sheet, iRow, iColumn, iCellId);
        ++iColumn;
        ++iCellId;
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    return iRowId;
  }

  private int SerializeHeadingsRow(
    XmlWriter writer,
    IWorksheet sheet,
    SizeF headingsSize,
    int iFirstColumn,
    int iLastColumn,
    int iRowId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    float height = headingsSize.Height;
    float width = headingsSize.Width;
    if ((double) height > 0.0)
    {
      ++iRowId;
      float num = (float) ApplicationImpl.ConvertUnitsStatic((double) height, MeasureUnits.Pixel, MeasureUnits.Point);
      writer.WriteStartElement("row");
      writer.WriteAttributeString("id", iRowId.ToString());
      this.WriteAttribute(writer, "RowHeight", (double) num);
      writer.WriteStartElement("cells");
      int iCellId = 0;
      if ((double) width > 0.0)
      {
        this.SerializeCell(writer, (double) width, string.Empty, iCellId);
        ++iCellId;
      }
      int iColumn = iFirstColumn;
      while (iColumn <= iLastColumn)
      {
        string columnName = RangeImpl.GetColumnName(iColumn);
        double columnWidth = this.GetColumnWidth(sheet, iColumn);
        this.SerializeCell(writer, columnWidth, columnName, iCellId);
        ++iColumn;
        ++iCellId;
      }
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    return iRowId;
  }

  private void SerializeCell(
    XmlWriter writer,
    IWorksheet sheet,
    int iRow,
    int iColumn,
    int iCellId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorksheetImpl worksheetImpl = sheet != null ? (WorksheetImpl) sheet : throw new ArgumentNullException(nameof (sheet));
    int xfIndex = worksheetImpl.GetXFIndex(iRow, iColumn);
    ExtendedFormatImpl innerExtFormat = worksheetImpl.ParentWorkbook.InnerExtFormats[xfIndex];
    double columnWidth = this.GetColumnWidth(sheet, iColumn);
    writer.WriteStartElement("cell");
    writer.WriteAttributeString("id", iCellId.ToString());
    this.WriteAttribute(writer, "Width", columnWidth);
    if (sheet.Contains(iRow, iColumn))
    {
      long cellIndex = RangeImpl.GetCellIndex(iColumn, iRow);
      RichTextString rtfString = worksheetImpl.CellRecords.GetRTFString(cellIndex, false);
      this.SerializeRichTextString(writer, rtfString, innerExtFormat);
    }
    this.SerializeTableFormat(writer, innerExtFormat);
    writer.WriteEndElement();
  }

  private void SerializeCell(XmlWriter writer, double dWidth, string strCellValue, int iCellId)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (strCellValue == null)
      throw new ArgumentNullException(nameof (strCellValue));
    writer.WriteStartElement("cell");
    writer.WriteAttributeString("id", iCellId.ToString());
    this.WriteAttribute(writer, "Width", dWidth);
    writer.WriteStartElement("paragraphs");
    writer.WriteStartElement("paragraph");
    writer.WriteStartElement("paragraph-format");
    writer.WriteAttributeString("HrAlignment", "Center");
    writer.WriteEndElement();
    writer.WriteStartElement("items");
    writer.WriteStartElement("item");
    writer.WriteAttributeString("type", "TextRange");
    writer.WriteStartElement("text");
    writer.WriteString(strCellValue);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    this.WriteHeadingsBorders(writer);
    writer.WriteEndElement();
  }

  private void SerializeTableFormat(XmlWriter writer, ExtendedFormatImpl xFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (xFormat == null)
      throw new ArgumentNullException(nameof (xFormat));
    writer.WriteStartElement("cell-format");
    string valignment = this.GetVAlignment(xFormat.VerticalAlignment);
    this.WriteAttribute(writer, "VAlignment", valignment);
    if (!xFormat.IsDefaultColor)
    {
      string colorString = this.GetColorString(xFormat.Color);
      writer.WriteAttributeString("ShadingColor", colorString);
    }
    this.SerializeBorders(writer, xFormat);
    writer.WriteEndElement();
  }

  private void SerializeBorders(XmlWriter writer, ExtendedFormatImpl xFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (xFormat == null)
      throw new ArgumentNullException(nameof (xFormat));
    writer.WriteStartElement("borders");
    BorderImpl borderImpl = (BorderImpl) null;
    int index = 0;
    for (int length = DLSXmlSerializator.DEF_DLS_BORDERS.Length; index < length; ++index)
    {
      OfficeBordersIndex borderIndex = DLSXmlSerializator.DEF_DLS_BORDERS[index];
      if (borderImpl == null)
        borderImpl = new BorderImpl(xFormat.Application, (object) xFormat, (IInternalExtendedFormat) xFormat, borderIndex);
      else
        borderImpl.BorderIndex = borderIndex;
      writer.WriteStartElement(DLSXmlSerializator.DEF_DLS_BORDER_NAMES[index]);
      if (borderImpl.LineStyle != OfficeLineStyle.None)
      {
        writer.WriteAttributeString("Color", this.GetColorString(borderImpl.ColorRGB));
        writer.WriteAttributeString("LineWidth", this.GetLineWidth((IBorder) borderImpl));
      }
      writer.WriteAttributeString("BorderType", this.GetBorderType((IBorder) borderImpl));
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private string GetVAlignment(OfficeVAlign align)
  {
    switch (align)
    {
      case OfficeVAlign.VAlignTop:
        return "Top";
      case OfficeVAlign.VAlignCenter:
      case OfficeVAlign.VAlignDistributed:
        return "Middle";
      case OfficeVAlign.VAlignBottom:
        return "Bottom";
      case OfficeVAlign.VAlignJustify:
        return (string) null;
      default:
        throw new ArgumentOutOfRangeException(nameof (align));
    }
  }

  private string GetHAlignment(OfficeHAlign align)
  {
    switch (align)
    {
      case OfficeHAlign.HAlignLeft:
        return "Left";
      case OfficeHAlign.HAlignCenter:
      case OfficeHAlign.HAlignCenterAcrossSelection:
        return "Center";
      case OfficeHAlign.HAlignRight:
        return "Right";
      case OfficeHAlign.HAlignJustify:
        return "Justify";
      default:
        return (string) null;
    }
  }

  private string GetLineWidth(IBorder border)
  {
    switch (border.LineStyle)
    {
      case OfficeLineStyle.Thin:
      case OfficeLineStyle.Dashed:
      case OfficeLineStyle.Dotted:
      case OfficeLineStyle.Double:
      case OfficeLineStyle.Dash_dot:
      case OfficeLineStyle.Dash_dot_dot:
      case OfficeLineStyle.Slanted_dash_dot:
        return DLSXmlSerializator.DEF_BORDER_WIDTH_THIN;
      case OfficeLineStyle.Medium:
      case OfficeLineStyle.Medium_dashed:
      case OfficeLineStyle.Medium_dash_dot:
      case OfficeLineStyle.Medium_dash_dot_dot:
        return DLSXmlSerializator.DEF_BORDER_WIDTH_MEDIUM;
      case OfficeLineStyle.Thick:
        return DLSXmlSerializator.DEF_BORDER_WIDTH_THICK;
      case OfficeLineStyle.Hair:
        return DLSXmlSerializator.DEF_BORDER_WIDTH_HAIR;
      default:
        return "0";
    }
  }

  private string GetBorderType(IBorder border)
  {
    switch (border.LineStyle)
    {
      case OfficeLineStyle.Thin:
      case OfficeLineStyle.Medium:
      case OfficeLineStyle.Hair:
        return "Single";
      case OfficeLineStyle.Dashed:
      case OfficeLineStyle.Medium_dashed:
        return "DashSmallGap";
      case OfficeLineStyle.Dotted:
        return "Dot";
      case OfficeLineStyle.Thick:
        return "Thick";
      case OfficeLineStyle.Double:
        return "Double";
      case OfficeLineStyle.Dash_dot:
      case OfficeLineStyle.Medium_dash_dot:
      case OfficeLineStyle.Slanted_dash_dot:
        return "DotDash";
      case OfficeLineStyle.Dash_dot_dot:
      case OfficeLineStyle.Medium_dash_dot_dot:
        return "DotDotDash";
      default:
        return "None";
    }
  }

  private void SerializeRichTextString(
    XmlWriter writer,
    RichTextString rtfString,
    ExtendedFormatImpl xFormat)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (rtfString == null)
      return;
    writer.WriteStartElement("paragraphs");
    writer.WriteStartElement("paragraph");
    writer.WriteAttributeString("id", "0");
    writer.WriteStartElement("paragraph-format");
    string halignment = this.GetHAlignment(xFormat.HorizontalAlignment);
    this.WriteAttribute(writer, "HrAlignment", halignment);
    writer.WriteEndElement();
    writer.WriteStartElement("items");
    TextWithFormat textObject = rtfString.TextObject;
    int formattingRunsCount = textObject.FormattingRunsCount;
    string text = textObject.Text;
    FontsCollection innerFonts = rtfString.Workbook.InnerFonts;
    int id = 0;
    if (formattingRunsCount > 0)
    {
      int length = textObject.Text.Length;
      int iIndex = 0;
      int positionByIndex1 = textObject.GetPositionByIndex(0);
      if (positionByIndex1 != 0)
      {
        string strText = text.Substring(0, positionByIndex1);
        this.WriteText(writer, strText, (IOfficeFont) rtfString.DefaultFont, id);
        ++id;
      }
      while (iIndex < formattingRunsCount)
      {
        int fontByIndex = textObject.GetFontByIndex(iIndex);
        int positionByIndex2 = textObject.GetPositionByIndex(iIndex);
        int num = iIndex != formattingRunsCount - 1 ? textObject.GetPositionByIndex(iIndex + 1) : length;
        string strText = text.Substring(positionByIndex2, num - positionByIndex2);
        IOfficeFont font = innerFonts[fontByIndex];
        this.WriteText(writer, strText, font, id);
        ++iIndex;
        ++id;
      }
    }
    else
      this.WriteText(writer, text, (IOfficeFont) rtfString.DefaultFont, id);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void WriteText(XmlWriter writer, string strText, IOfficeFont font, int id)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (strText == null)
      throw new ArgumentNullException(nameof (strText));
    writer.WriteStartElement("item");
    writer.WriteAttributeString(nameof (id), id.ToString());
    writer.WriteAttributeString("type", "TextRange");
    if (font != null && strText.Length > 0)
      this.WriteFont(writer, font);
    writer.WriteStartElement("text");
    writer.WriteString(strText);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void WriteFont(XmlWriter writer, IOfficeFont font)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    writer.WriteStartElement("character-format");
    writer.WriteAttributeString("FontName", font.FontName.ToString());
    writer.WriteAttributeString("FontSize", font.Size.ToString());
    writer.WriteAttributeString("TextColor", this.GetColorString(font.RGBColor));
    this.SerializeBoolAttribute(writer, "Bold", font.Bold);
    this.SerializeBoolAttribute(writer, "Italic", font.Italic);
    writer.WriteAttributeString("Underline", this.GetUnderlineString(font.Underline));
    writer.WriteAttributeString("SubSuperScript", this.GetSubSuperScript(font));
    this.SerializeBoolAttribute(writer, "Strike", font.Strikethrough);
    writer.WriteEndElement();
  }

  private void SerializeBoolAttribute(XmlWriter writer, string strAttributeName, bool bValue)
  {
    if (!bValue)
      return;
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (strAttributeName)
    {
      case null:
        throw new ArgumentNullException(nameof (strAttributeName));
      case "":
        throw new ArgumentException("strAttributeName - string cannot be empty.");
      default:
        writer.WriteAttributeString(strAttributeName, DLSXmlSerializator.DEF_TRUE_STRING);
        break;
    }
  }

  private string GetColorString(Color color) => "#" + color.ToArgb().ToString("X");

  private string GetUnderlineString(OfficeUnderline underline)
  {
    switch (underline)
    {
      case OfficeUnderline.Single:
      case OfficeUnderline.SingleAccounting:
        return "Single";
      case OfficeUnderline.Double:
      case OfficeUnderline.DoubleAccounting:
        return "Double";
      default:
        return "None";
    }
  }

  private string GetSubSuperScript(IOfficeFont font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (font.Subscript)
      return "SubScript";
    return font.Superscript ? "SuperScript" : "None";
  }

  private void WriteAttribute(
    XmlWriter writer,
    string strAttributeName,
    double value,
    MeasureUnits units)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (strAttributeName)
    {
      case null:
        throw new ArgumentNullException(nameof (strAttributeName));
      case "":
        throw new ArgumentException("strAttributeName - string cannot be empty.");
      default:
        value = ApplicationImpl.ConvertUnitsStatic(value, units, MeasureUnits.Point);
        string str = value.ToString((IFormatProvider) DLSXmlSerializator.DLSCulture);
        writer.WriteAttributeString(strAttributeName, str);
        break;
    }
  }

  private void WriteAttribute(XmlWriter writer, string strAttributeName, string strValue)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (strAttributeName)
    {
      case null:
        throw new ArgumentNullException(nameof (strAttributeName));
      case "":
        throw new ArgumentException("strAttributeName - string cannot be empty.");
      default:
        switch (strValue)
        {
          case null:
            return;
          case "":
            return;
          default:
            writer.WriteAttributeString(strAttributeName, strValue);
            return;
        }
    }
  }

  private void WriteAttribute(XmlWriter writer, string strAttributeName, double value)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (strAttributeName)
    {
      case null:
        throw new ArgumentNullException(nameof (strAttributeName));
      case "":
        throw new ArgumentException("strAttributeName - string cannot be empty.");
      default:
        string str = value.ToString((IFormatProvider) DLSXmlSerializator.DLSCulture);
        writer.WriteAttributeString(strAttributeName, str);
        break;
    }
  }

  private void SerializeHeaderFooter(XmlWriter writer, IWorksheet sheet, double dPageWidth)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    writer.WriteStartElement("headers-footers");
    IPageSetup pageSetup = sheet.PageSetup;
    string[] arrValues = new string[3]
    {
      pageSetup.LeftHeader,
      pageSetup.RightHeader,
      pageSetup.CenterHeader
    };
    writer.WriteStartElement("odd-header");
    this.SerializeHeaderFooter(writer, arrValues, dPageWidth);
    writer.WriteEndElement();
    arrValues[0] = pageSetup.LeftFooter;
    arrValues[1] = pageSetup.RightFooter;
    arrValues[2] = pageSetup.CenterFooter;
    writer.WriteStartElement("odd-footer");
    this.SerializeHeaderFooter(writer, arrValues, dPageWidth);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeHeaderFooter(XmlWriter writer, string[] arrValues, double dPageWidth)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int num1 = arrValues != null ? arrValues.Length : throw new ArgumentNullException(nameof (arrValues));
    if (num1 == 0)
      return;
    int num2 = 0;
    for (int index = 0; index < num1; ++index)
      num2 += arrValues[index].Length;
    if (num2 == 0)
      return;
    writer.WriteStartElement("paragraphs");
    writer.WriteStartElement("paragraph");
    writer.WriteStartElement("items");
    writer.WriteStartElement("item");
    writer.WriteAttributeString("id", "0");
    writer.WriteAttributeString("type", "Table");
    writer.WriteAttributeString("ColumnsCount", num1.ToString());
    this.WriteEmptyBorders(writer);
    writer.WriteStartElement("rows");
    writer.WriteStartElement("row");
    writer.WriteAttributeString("id", "0");
    writer.WriteStartElement("cells");
    double num3 = dPageWidth / (double) num1;
    for (int index = 0; index < num1; ++index)
    {
      writer.WriteStartElement("cell");
      writer.WriteAttributeString("id", index.ToString());
      this.WriteAttribute(writer, "Width", num3);
      writer.WriteStartElement("paragraphs");
      writer.WriteStartElement("paragraph");
      writer.WriteAttributeString("id", "0");
      writer.WriteStartElement("items");
      writer.WriteStartElement("item");
      writer.WriteAttributeString("id", "0");
      writer.WriteAttributeString("type", "TextRange");
      writer.WriteStartElement("text");
      writer.WriteString(arrValues[index]);
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private double GetColumnWidth(IWorksheet sheet, int iColumn)
  {
    return ApplicationImpl.ConvertUnitsStatic((double) sheet.GetColumnWidthInPixels(iColumn), MeasureUnits.Pixel, MeasureUnits.Point) + 1.0;
  }
}
