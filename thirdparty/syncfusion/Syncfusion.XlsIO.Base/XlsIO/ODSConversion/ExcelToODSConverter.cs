// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODSConversion.ExcelToODSConverter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.ODF.Base;
using Syncfusion.XlsIO.ODF.Base.ODFImplementation;
using Syncfusion.XlsIO.ODF.Base.ODFSerialization;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.ODSConversion;

internal class ExcelToODSConverter
{
  private const string DEF_RANGE_REF = ".";
  private const string DefaultStyleName = "Default";
  private const int DEF_ROTATION_ANGLE = 90;
  private WorkbookImpl m_book;
  private ODFWriter m_writer;
  private List<string> pageNames;
  private RecordExtractor m_recordExtractor = new RecordExtractor();

  public ExcelToODSConverter(WorkbookImpl book)
  {
    if (book.AppImplementation.EvalExpired)
      book.AddWatermark((IWorkbook) book);
    this.m_book = book;
    this.m_writer = new ODFWriter();
    this.pageNames = new List<string>();
  }

  internal void ConvertFonts()
  {
    List<FontFace> fonts = (List<FontFace>) null;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    int count = innerFonts.Count;
    if (count > 0)
      fonts = new List<FontFace>();
    for (int index = 0; index < count; ++index)
    {
      FontFace fontFace = new FontFace(innerFonts[index].FontName);
      if (!fonts.Contains(fontFace))
        fonts.Add(fontFace);
    }
    this.m_writer.SerializeFontFaceDecls(fonts);
    if (fonts == null)
      return;
    for (int index = 0; index < fonts.Count; ++index)
      fonts[index] = (FontFace) null;
    fonts.Clear();
  }

  internal void ConvertToODF(string fileName)
  {
    this.m_writer.SerializeMimeType();
    this.m_writer.SerializeDocumentManifest();
    this.m_writer.SerializeMetaData();
    this.MapDocumentStyles();
    this.MapContent();
    this.m_writer.SaveDocument(fileName);
  }

  internal void ConvertToODF(Stream stream)
  {
    this.m_writer.SerializeMimeType();
    this.m_writer.SerializeDocumentManifest();
    this.m_writer.SerializeMetaData();
    this.MapDocumentStyles();
    this.MapContent();
    this.m_writer.SaveDocument(stream);
  }

  internal void MapDocumentStyles()
  {
    MemoryStream stream = this.m_writer.SerializeStyleStart();
    this.ConvertFonts();
    this.ConvertDataStyles();
    this.ConvertAutomaticAndMasterStyles();
    this.m_writer.SerializeStylesEnd(stream);
  }

  internal void ConvertDataStyles()
  {
    ODFStyleCollection cellStyles = new ODFStyleCollection();
    this.m_writer.SerializeDataStylesStart();
    this.ConvertNumberStyles();
    ODFStyleCollection styles = this.ConvertCellStyles(cellStyles);
    this.m_writer.SerializeDataStyles(styles);
    this.m_writer.SerializeDefaultGraphicStyle();
    this.m_writer.SerializeEnd();
    styles.Dispose();
  }

  internal void ConvertContentAutoStyles()
  {
    ODFStyleCollection tableStyles = new ODFStyleCollection();
    for (int index = 0; index < this.m_book.InnerExtFormats.Count; ++index)
    {
      ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[index];
      if (innerExtFormat.HasParent)
      {
        ODFStyle cellStyle = new ODFStyle();
        cellStyle.DataStyleName = "N" + (object) innerExtFormat.NumberFormatIndex;
        cellStyle.Family = ODFFontFamily.Table_Cell;
        StyleImpl byXfIndex = this.m_book.InnerStyles.GetByXFIndex(innerExtFormat.ParentIndex);
        cellStyle.ParentStyleName = byXfIndex == null || byXfIndex.Name == "Normal" ? "Default" : byXfIndex.Name;
        ODFStyle style = this.ConvertCellStyle((IExtendedFormat) innerExtFormat, cellStyle);
        tableStyles.Add(style, innerExtFormat.XFormatIndex);
      }
    }
    this.ConvertTableData(tableStyles);
  }

  private void ConvertTableProperties(WorksheetImpl sheet, OTableProperties tblProp)
  {
    tblProp.Display = sheet.IsVisible;
    tblProp.WritingMode = sheet.IsRightToLeft ? WritingMode.TBLR : WritingMode.LRTB;
  }

  internal void MapContent()
  {
    MemoryStream stream = this.m_writer.SerializeContentNameSpace();
    this.ConvertFonts();
    this.ConvertContentAutoStyles();
    this.m_writer.SerializeContentEnd(stream);
  }

  private void ConvertNumberStyles()
  {
    int count = this.m_book.InnerExtFormats.Count;
    List<int> intList = new List<int>();
    for (int index = 0; index < count; ++index)
    {
      int formatIndex = (int) this.m_book.InnerExtFormats[index].Record.FormatIndex;
      if (!intList.Contains(formatIndex))
      {
        string currencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
        FormatImpl innerFormat = this.m_book.InnerFormats[formatIndex];
        intList.Add(innerFormat.Index);
        if (innerFormat != null)
        {
          switch (innerFormat.FormatType)
          {
            case ExcelFormatType.General:
              NumberStyle style = new NumberStyle();
              style.Name = "N" + (object) innerFormat.Index;
              style.Number.MinIntegerDigits = 1;
              this.m_writer.SerializeGeneralStyle(style);
              style.Dispose();
              continue;
            case ExcelFormatType.Text:
              TextStyle nFormat1 = new TextStyle();
              nFormat1.Name = "N" + (object) innerFormat.Index;
              nFormat1.TextContent = true;
              this.m_writer.SerializeNumberStyle((DataStyle) nFormat1);
              nFormat1.Dispose1();
              continue;
            case ExcelFormatType.Number:
              if (innerFormat.FormatString.Contains("%"))
              {
                PercentageStyle nFormat2 = new PercentageStyle();
                nFormat2.Name = "N" + (object) innerFormat.Index;
                nFormat2.Number.DecimalPlaces = innerFormat.DecimalPlaces;
                nFormat2.Number.MinIntegerDigits = innerFormat.NoOfSignificantDigits;
                this.m_writer.SerializeNumberStyle((DataStyle) nFormat2);
                nFormat2.Dispose();
                continue;
              }
              if (innerFormat.FormatString.Contains(currencySymbol))
              {
                CurrencyStyle nFormat3 = new CurrencyStyle();
                nFormat3.Name = "N" + (object) innerFormat.Index;
                nFormat3.CurrencySymbol.Data = currencySymbol;
                nFormat3.Number.MinIntegerDigits = innerFormat.NoOfSignificantDigits;
                nFormat3.Number.DecimalPlaces = innerFormat.DecimalPlaces;
                if (innerFormat.IsThousandSeparator)
                  nFormat3.Number.Grouping = innerFormat.IsThousandSeparator;
                this.m_writer.SerializeNumberStyle((DataStyle) nFormat3);
                nFormat3.Dispose();
                continue;
              }
              NumberStyle nFormat4 = new NumberStyle();
              nFormat4.Name = "N" + (object) innerFormat.Index;
              if (innerFormat.IsScientific)
              {
                nFormat4.ScientificNumber.MinIntegerDigits = innerFormat.NoOfSignificantDigits;
                nFormat4.ScientificNumber.DecimalPlaces = innerFormat.DecimalPlaces;
                nFormat4.ScientificNumber.MinExponentDigits = innerFormat.MinExponentDigits;
              }
              else if (innerFormat.IsFraction)
              {
                nFormat4.Fraction.MinIntegerDigits = innerFormat.NoOfSignificantDigits;
                if (innerFormat.FractionBase != 0)
                  nFormat4.Fraction.DenominatorValue = innerFormat.FractionBase;
                nFormat4.Fraction.MinDenominatorDigits = innerFormat.DenumaratorLen;
                nFormat4.Fraction.MinNumeratorDigits = innerFormat.NumeratorLen;
              }
              else
              {
                nFormat4.Number.MinIntegerDigits = innerFormat.NoOfSignificantDigits;
                nFormat4.Number.DecimalPlaces = innerFormat.DecimalPlaces;
                if (innerFormat.IsThousandSeparator)
                  nFormat4.Number.Grouping = innerFormat.IsThousandSeparator;
              }
              this.m_writer.SerializeNumberStyle((DataStyle) nFormat4);
              nFormat4.Dispose();
              continue;
            case ExcelFormatType.DateTime:
              DateStyle nFormat5 = new DateStyle();
              nFormat5.Name = "N" + (object) innerFormat.Index;
              this.m_writer.SerializeNumberStyle((DataStyle) nFormat5);
              nFormat5.Dispose();
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  private ODFStyleCollection ConvertCellStyles(ODFStyleCollection cellStyles)
  {
    IStyles styles = this.m_book.Styles;
    for (int Index = 0; Index < styles.Count; ++Index)
    {
      ODFStyle cellStyle = new ODFStyle();
      IStyle curStyle = styles[Index];
      cellStyle.Name = this.ProcessName(curStyle.Name);
      cellStyle.DataStyleName = "N" + (object) curStyle.NumberFormatIndex;
      cellStyle.Family = ODFFontFamily.Table_Cell;
      ODFStyle style = this.ConvertCellStyle((IExtendedFormat) curStyle, cellStyle);
      cellStyles.Add(style);
    }
    return cellStyles;
  }

  private ODFStyle ConvertCellStyle(IExtendedFormat curStyle, ODFStyle cellStyle)
  {
    this.ConvertTableCellProperties(curStyle, cellStyle);
    this.ConvertParagraphProperties(curStyle, cellStyle);
    this.ConvertTextPropeties(curStyle, cellStyle);
    return cellStyle;
  }

  private void ConvertParagraphProperties(IExtendedFormat format, ODFStyle cellStyle)
  {
    bool flag = format.HorizontalAlignment != ExcelHAlign.HAlignGeneral;
    if (!format.IncludeAlignment || !flag)
      return;
    ODFParagraphProperties paragraphProperties = new ODFParagraphProperties();
    paragraphProperties.TextAlign = this.MapHAlign(format.HorizontalAlignment);
    if (format.HorizontalAlignment == ExcelHAlign.HAlignFill)
      cellStyle.TableCellProperties.RepeatContent = true;
    cellStyle.ParagraphProperties = paragraphProperties;
  }

  private TextAlign MapHAlign(ExcelHAlign halign)
  {
    switch (halign)
    {
      case ExcelHAlign.HAlignLeft:
      case ExcelHAlign.HAlignFill:
        return TextAlign.start;
      case ExcelHAlign.HAlignCenter:
        return TextAlign.center;
      case ExcelHAlign.HAlignRight:
        return TextAlign.end;
      case ExcelHAlign.HAlignJustify:
        return TextAlign.justify;
      default:
        return TextAlign.left;
    }
  }

  private TextAlign MapHAlign(ExcelCommentHAlign halign)
  {
    switch (halign)
    {
      case ExcelCommentHAlign.Left:
        return TextAlign.start;
      case ExcelCommentHAlign.Center:
        return TextAlign.center;
      case ExcelCommentHAlign.Right:
        return TextAlign.end;
      case ExcelCommentHAlign.Justified:
        return TextAlign.justify;
      default:
        return TextAlign.left;
    }
  }

  private string ProcessName(string styleName)
  {
    string str = styleName;
    if (styleName.Equals("Normal"))
      str = "Default";
    else if (styleName.Contains(" "))
      str = styleName.Replace(" ", "_");
    return str;
  }

  private OTableColumn ConvertColumnStyle(
    ColumnInfoRecord XlColumn,
    OTableColumn column,
    ODFStyleCollection styles)
  {
    ODFStyle odfStyle = (ODFStyle) null;
    styles.DictStyles.TryGetValue(XlColumn.ExtendedFormatIndex.ToString(), out odfStyle);
    if (odfStyle != null)
      column.DefaultCellStyleName = odfStyle.Name;
    ODFStyle style = new ODFStyle();
    style.Family = ODFFontFamily.Table_Column;
    style.TableColumnProperties = this.ConvertTableColumnProperties(XlColumn);
    column.OutlineLevel = (int) XlColumn.OutlineLevel;
    column.StyleName = styles.Add(style);
    return column;
  }

  private OTableRow ConvertRowStyle(
    RowStorage row,
    OTableRow tableRow,
    ODFStyleCollection tableStyles)
  {
    ODFStyle odfStyle = (ODFStyle) null;
    tableStyles.DictStyles.TryGetValue(row.ExtendedFormatIndex.ToString(), out odfStyle);
    if (odfStyle != null)
      tableRow.DefaultCellStyleName = odfStyle.Name;
    ODFStyle style = new ODFStyle();
    style.Family = ODFFontFamily.Table_Row;
    style.TableRowProperties = this.ConvertTableRowProperties(row);
    tableRow.OutlineLevel = (int) row.OutlineLevel;
    tableRow.OutlineLevel = (int) row.OutlineLevel;
    if (row.IsHidden)
      tableRow.IsCollapsed = row.IsHidden;
    tableRow.StyleName = tableStyles.Add(style);
    return tableRow;
  }

  internal OTable ConvertTableStyle(WorksheetImpl sheet, OTable table, ODFStyleCollection styles)
  {
    ODFStyle style = new ODFStyle();
    style.Family = ODFFontFamily.Table;
    this.ConvertTableProperties(sheet, style.TableProperties = new OTableProperties());
    table.StyleName = styles.Add(style);
    style.MasterPageName = this.pageNames[sheet.Index] != null ? this.pageNames[sheet.Index] : this.pageNames[0];
    return table;
  }

  private OTableColumnProperties ConvertTableColumnProperties(ColumnInfoRecord column)
  {
    return new OTableColumnProperties()
    {
      ColumnWidth = (double) column.ColumnWidth / 256.0 * 0.211666667
    };
  }

  private OTableRowProperties ConvertTableRowProperties(RowStorage row)
  {
    return new OTableRowProperties()
    {
      RowHeight = (double) row.Height / 20.0
    };
  }

  private void ConvertTableCellProperties(IExtendedFormat format, ODFStyle cellStyle)
  {
    OTableCellProperties otableCellProperties = (OTableCellProperties) null;
    if (format.IncludeAlignment || format.IncludeBorder || format.IncludePatterns)
      otableCellProperties = cellStyle.TableCellProperties = new OTableCellProperties();
    if (format.IncludeAlignment)
    {
      otableCellProperties.VerticalAlign = format.VerticalAlignment == ExcelVAlign.VAlignBottom ? new VerticalAlign?(VerticalAlign.automatic) : new VerticalAlign?((VerticalAlign) format.VerticalAlignment);
      if (format.WrapText)
        otableCellProperties.Wrap = format.WrapText;
      if (format.ShrinkToFit)
        otableCellProperties.ShrinkToFit = format.ShrinkToFit;
    }
    if (format.IncludeBorder)
    {
      if (format.Borders.LineStyle != ExcelLineStyle.None)
      {
        ODFBorder border = otableCellProperties.Border = new ODFBorder();
        border.LineStyle = this.MapBorders(border, format.Borders.LineStyle);
        border.LineColor = format.Borders.ColorRGB;
      }
      else
      {
        ExcelLineStyle lineStyle1 = format.Borders[ExcelBordersIndex.EdgeLeft].LineStyle;
        ExcelLineStyle lineStyle2 = format.Borders[ExcelBordersIndex.EdgeRight].LineStyle;
        ExcelLineStyle lineStyle3 = format.Borders[ExcelBordersIndex.EdgeTop].LineStyle;
        ExcelLineStyle lineStyle4 = format.Borders[ExcelBordersIndex.EdgeBottom].LineStyle;
        ExcelLineStyle lineStyle5 = format.Borders[ExcelBordersIndex.DiagonalDown].LineStyle;
        ExcelLineStyle lineStyle6 = format.Borders[ExcelBordersIndex.DiagonalUp].LineStyle;
        if (lineStyle1 != ExcelLineStyle.None)
        {
          ODFBorder border = otableCellProperties.BorderLeft = new ODFBorder();
          border.LineStyle = this.MapBorders(border, format.Borders[ExcelBordersIndex.EdgeLeft].LineStyle);
          border.LineColor = format.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB;
        }
        if (lineStyle2 != ExcelLineStyle.None)
        {
          ODFBorder border = otableCellProperties.BorderRight = new ODFBorder();
          border.LineStyle = this.MapBorders(border, format.Borders[ExcelBordersIndex.EdgeRight].LineStyle);
          border.LineColor = format.Borders[ExcelBordersIndex.EdgeRight].ColorRGB;
        }
        if (lineStyle4 != ExcelLineStyle.None)
        {
          ODFBorder border = otableCellProperties.BorderBottom = new ODFBorder();
          border.LineStyle = this.MapBorders(border, format.Borders[ExcelBordersIndex.EdgeBottom].LineStyle);
          border.LineColor = format.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB;
        }
        if (lineStyle3 != ExcelLineStyle.None)
        {
          ODFBorder border = otableCellProperties.BorderTop = new ODFBorder();
          border.LineStyle = this.MapBorders(border, format.Borders[ExcelBordersIndex.EdgeTop].LineStyle);
          border.LineColor = format.Borders[ExcelBordersIndex.EdgeTop].ColorRGB;
        }
        if (lineStyle5 != ExcelLineStyle.None)
        {
          ODFBorder border = otableCellProperties.DiagonalLeft = new ODFBorder();
          border.LineStyle = this.MapBorders(border, format.Borders[ExcelBordersIndex.DiagonalDown].LineStyle);
          border.LineColor = format.Borders[ExcelBordersIndex.DiagonalDown].ColorRGB;
        }
        if (lineStyle6 != ExcelLineStyle.None)
        {
          ODFBorder border = otableCellProperties.DiagonalRight = new ODFBorder();
          border.LineStyle = this.MapBorders(border, format.Borders[ExcelBordersIndex.DiagonalUp].LineStyle);
          border.LineColor = format.Borders[ExcelBordersIndex.DiagonalUp].ColorRGB;
        }
      }
    }
    if (format.IncludePatterns)
      otableCellProperties.BackColor = format.ColorIndex == ExcelKnownColors.None ? Color.Transparent : format.Color;
    int rotation = format.Rotation;
    if (rotation == 0)
      return;
    if (rotation > 0 && rotation <= 90)
      otableCellProperties.RotationAngle = rotation;
    else if (rotation > 90 && rotation <= 180)
    {
      otableCellProperties.RotationAngle = 360 + (90 - rotation);
    }
    else
    {
      if (rotation != (int) byte.MaxValue)
        return;
      otableCellProperties.Direction = PageOrder.ttb;
    }
  }

  private void ConvertTextPropeties(IExtendedFormat format, ODFStyle cellStyle)
  {
    TextProperties txtProp = (TextProperties) null;
    if (format.IncludeFont || format.IncludeAlignment)
      txtProp = cellStyle.Textproperties = new TextProperties();
    if (!format.IncludeFont)
      return;
    txtProp.FontName = format.Font.FontName;
    txtProp.FontSize = format.Font.Size;
    if (format.IncludePatterns || !format.Font.IsAutoColor || format.Font.Color != ExcelKnownColors.Custom0)
      txtProp.Color = format.Font.RGBColor;
    if (format.Font.Strikethrough)
    {
      txtProp.LinethroughType = LineType.single;
      txtProp.LinethroughStyle = BorderLineStyle.solid;
    }
    if (format.Font.Superscript)
      txtProp.TextPosition = TextPosition.Super.ToString().ToLower();
    else if (format.Font.Subscript)
      txtProp.TextPosition = TextPosition.Sub.ToString().ToLower();
    txtProp.FontStyle = format.Font.Italic ? ODFFontStyle.italic : ODFFontStyle.normal;
    txtProp.FontWeight = format.Font.Bold ? FontWeight.bold : FontWeight.normal;
    if (format.Font.Underline == ExcelUnderline.None)
      return;
    this.MapUnderlineStyle(txtProp, format.Font.Underline);
  }

  private void MapUnderlineStyle(TextProperties txtProp, ExcelUnderline underLine)
  {
    switch (underLine)
    {
      case ExcelUnderline.Single:
        txtProp.TextUnderlineType = LineType.single;
        txtProp.TextUnderlineStyle = BorderLineStyle.solid;
        break;
      case ExcelUnderline.Double:
        txtProp.TextUnderlineType = LineType.Double;
        txtProp.TextUnderlineStyle = BorderLineStyle.solid;
        break;
    }
  }

  private void ConvertAutomaticAndMasterStyles()
  {
    PageLayoutCollection layouts = new PageLayoutCollection();
    MasterPageCollection mPages = new MasterPageCollection();
    for (int Index = 0; Index < this.m_book.Worksheets.Count; ++Index)
    {
      PageLayout layout = new PageLayout();
      IPageSetup pageSetup = this.m_book.Worksheets[Index].PageSetup;
      this.ConvertPageLayouts(pageSetup, layout);
      this.ConvertHeaderFooterStyles(pageSetup, layout);
      string str = layouts.Add(layout);
      this.pageNames.Add(mPages.Add(new MasterPage()
      {
        PageLayoutName = str
      }));
    }
    this.m_writer.SerializeAutomaticStyles(layouts);
    this.m_writer.SerializeMasterStylesStart();
    this.m_writer.SerializeMasterStyles(mPages);
    this.m_writer.SerializeEnd();
    layouts.Dispose();
    mPages.Dispose();
  }

  private void ConvertPageLayouts(IPageSetup setup, PageLayout layout)
  {
    this.ConvertPageLayoutProperties(setup, layout);
  }

  private void GetColumns(WorksheetImpl sheet, OTable table, ODFStyleCollection tableStyles)
  {
    ColumnInfoRecord[] columnInformation = sheet.ColumnInformation;
    int num = sheet.LastColumn > this.m_book.MaxColumnCount ? this.m_book.MaxColumnCount : sheet.LastColumn;
    int index1 = 1;
    for (int index2 = num; index1 <= index2; ++index1)
    {
      ColumnInfoRecord XlColumn = columnInformation[index1];
      OTableColumn column = new OTableColumn();
      if (XlColumn != null)
        column = this.ConvertColumnStyle(XlColumn, column, tableStyles);
      if (!string.IsNullOrEmpty(column.DefaultCellStyleName))
        table.HasDefaultColumnStyle = true;
      table.Columns.Add(column);
    }
  }

  private void ConvertTableData(ODFStyleCollection tableStyles)
  {
    List<OTable> tables = new List<OTable>();
    for (int Index = 0; Index < this.m_book.Worksheets.Count; ++Index)
    {
      if (this.m_book.Worksheets[Index] is WorksheetImpl worksheet)
      {
        OTable table = new OTable();
        table.Name = worksheet.Name.Replace(" ", "_");
        CellRecordCollection cellRecords = worksheet.CellRecords;
        if (cellRecords.LastColumn < int.MaxValue)
          this.GetColumns(worksheet, table, tableStyles);
        int num1 = cellRecords.LastColumn > this.m_book.MaxColumnCount ? this.m_book.MaxColumnCount : worksheet.LastColumn;
        int num2 = this.UpdateCommentRange((IWorksheet) worksheet);
        if (num2 > 0 && num2 > num1)
          num1 = num2;
        if (cellRecords.LastRow > 0)
        {
          this.LoadDefaultRowStyle(tableStyles, worksheet);
          int num3 = 1;
          for (int lastRow = cellRecords.LastRow; num3 <= lastRow; ++num3)
          {
            RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) worksheet, num3 - 1, true);
            OTableRow tableRow = new OTableRow();
            int num4 = 1;
            for (int index = num1; num4 <= index; ++num4)
            {
              long cellIndex = RangeImpl.GetCellIndex(num4, num3);
              ICellPositionFormat cellRecord = cellRecords.GetCellRecord(cellIndex);
              OTableCell cell = new OTableCell();
              IComment innerComment = (IComment) worksheet.InnerComments[num3, num4];
              if (cellRecord != null)
              {
                this.GetCellType(cellIndex, cell, worksheet, cellRecords, row);
                this.MapCellName(cellRecord, tableStyles, cell, worksheet);
              }
              else
              {
                cell.IsBlank = true;
                ++cell.ColumnsRepeated;
                cell.StyleName = string.IsNullOrEmpty(tableRow.DefaultCellStyleName) ? "CE1" : tableRow.DefaultCellStyleName;
              }
              this.GetComment(worksheet, innerComment, cell, tableStyles);
              tableRow.Cells.Add(cell);
            }
            tableRow.RepeatedRowColumns = 1048576 /*0x100000*/ - num3;
            OTableRow otableRow = this.ConvertRowStyle(row, tableRow, tableStyles);
            table.Rows.Add(otableRow);
          }
        }
        else
          this.LoadDefaults(worksheet, tableStyles, table);
        this.ConvertNamedRanges(table);
        tables.Add(this.ConvertTableStyle(worksheet, table, tableStyles));
      }
    }
    this.m_writer.SerializeContentAutoStyles(tableStyles);
    tableStyles.Dispose();
    tableStyles = (ODFStyleCollection) null;
    this.SerializeExcelBody(tables);
  }

  private void GetComment(
    WorksheetImpl sheet,
    IComment comment,
    OTableCell cell,
    ODFStyleCollection styles)
  {
    if (comment == null)
      return;
    string text1 = (string) null;
    Annotation annotation = cell.Comment = new Annotation();
    annotation.Creator = comment.Author;
    annotation.Display = (comment as ShapeImpl).IsShapeVisible;
    CommentShapeImpl commentShapeImpl = comment as CommentShapeImpl;
    int num1 = this.m_book.AppImplementation.GetdpiY();
    annotation.X = (float) commentShapeImpl.LeftColumnOffset / (float) num1;
    annotation.Y = (float) commentShapeImpl.TopRowOffset / (float) num1;
    annotation.Height = (float) commentShapeImpl.Height / (float) num1;
    annotation.Width = (float) commentShapeImpl.Width / (float) num1;
    annotation.StyleName = styles.Add(this.FillCommentShapeStyle(commentShapeImpl));
    TextWithFormat textObject = (comment.RichText as RichTextString).TextObject;
    OParagraph para = new OParagraph();
    for (int iIndex = 0; iIndex < textObject.FormattingRunsCount; ++iIndex)
    {
      bool flag = false;
      int positionByIndex = textObject.GetPositionByIndex(iIndex);
      int num2 = iIndex + 1 >= textObject.FormattingRunsCount ? textObject.Text.Length : textObject.GetPositionByIndex(iIndex + 1);
      if (num2 > positionByIndex)
        text1 = comment.Text.Substring(positionByIndex, num2 - positionByIndex);
      string styleName = styles.Add(this.GetSpanTextProperties(textObject.GetFontByIndex(iIndex), (IFont) (comment.RichText as RichTextString).DefaultFont));
      if (text1.Contains('\n'.ToString()))
        flag = true;
      if (flag)
      {
        string text2 = (string) null;
        for (int index = 0; index < text1.Length; ++index)
        {
          if (text1[index] == '\n')
          {
            if (text2 != null)
            {
              this.AddCommentText(text2, styleName, para);
              text2 = (string) null;
            }
            para.StyleName = styles.Add(this.ConvertParagraphProperties(commentShapeImpl));
            annotation.Paras.Add(para);
            para = new OParagraph();
          }
          else
            text2 += (string) (object) text1[index];
        }
        if (text2 != null)
          this.AddCommentText(text2, styleName, para);
      }
      else
        this.AddCommentText(text1, styleName, para);
    }
    para.StyleName = styles.Add(this.ConvertParagraphProperties(commentShapeImpl));
    if (textObject.FormattingRunsCount == 0)
      para.TextInput = textObject.Text;
    annotation.Paras.Add(para);
  }

  private ODFStyle FillCommentShapeStyle(CommentShapeImpl commentShape)
  {
    ODFStyle odfStyle = new ODFStyle();
    odfStyle.Family = ODFFontFamily.Graphic;
    GraphicProperties graphicProperties = odfStyle.GraphicProperties = new GraphicProperties();
    graphicProperties.Fill = this.MapGraphicFillType(commentShape.Fill.FillType);
    graphicProperties.FillColor = commentShape.FillColor;
    graphicProperties.Stroke = Stroke.Solid;
    graphicProperties.StrokeColor = commentShape.Line.ForeColor;
    return odfStyle;
  }

  private void AddCommentText(string text, string styleName, OParagraph para)
  {
    OParagraphItem oparagraphItem = new OParagraphItem();
    ODFStyle odfStyle = new ODFStyle();
    oparagraphItem.StyleName = styleName;
    oparagraphItem.Text = text;
    para.OParagraphItemCollection.Add(oparagraphItem);
  }

  private FillType MapGraphicFillType(ExcelFillType type)
  {
    switch (type)
    {
      case ExcelFillType.SolidColor:
        return FillType.Solid;
      case ExcelFillType.Texture:
        return FillType.Hatch;
      case ExcelFillType.Picture:
        return FillType.Bitmap;
      case ExcelFillType.Gradient:
        return FillType.Gradient;
      default:
        return FillType.None;
    }
  }

  private ODFStyle GetSpanTextProperties(int index, IFont defaultFont)
  {
    ODFStyle spanTextProperties = new ODFStyle();
    spanTextProperties.Family = ODFFontFamily.Text;
    TextProperties txtProp = new TextProperties();
    IFont font = index < this.m_book.InnerFonts.Count ? this.m_book.InnerFonts[index] : defaultFont;
    txtProp.FontName = font.FontName;
    txtProp.FontSize = font.Size;
    txtProp.Color = (font as FontImpl).ColorObject.ColorType != ColorType.Indexed || font.Color > ExcelKnownColors.Dark_teal && font.Color != (ExcelKnownColors) 32767 /*0x7FFF*/ ? defaultFont.RGBColor : font.RGBColor;
    if (font.Strikethrough)
    {
      txtProp.LinethroughType = LineType.single;
      txtProp.LinethroughStyle = BorderLineStyle.solid;
    }
    if (font.Superscript)
      txtProp.TextPosition = TextPosition.Super.ToString().ToLower();
    else if (font.Subscript)
      txtProp.TextPosition = TextPosition.Sub.ToString().ToLower();
    txtProp.FontStyle = font.Italic ? ODFFontStyle.italic : ODFFontStyle.normal;
    txtProp.FontWeight = font.Bold ? FontWeight.bold : FontWeight.normal;
    if (font.Underline != ExcelUnderline.None)
      this.MapUnderlineStyle(txtProp, font.Underline);
    spanTextProperties.Textproperties = txtProp;
    return spanTextProperties;
  }

  private ODFStyle ConvertParagraphProperties(CommentShapeImpl shape)
  {
    ODFStyle odfStyle = new ODFStyle();
    (odfStyle.ParagraphProperties = new ODFParagraphProperties()).TextAlign = this.MapHAlign(shape.HAlignment);
    return odfStyle;
  }

  private void GetCellType(
    long cellIndex,
    OTableCell cell,
    WorksheetImpl sheet,
    CellRecordCollection cells,
    RowStorage row)
  {
    ICellPositionFormat cellRecord = cells.GetCellRecord(cellIndex);
    if (cellRecord == null)
      return;
    OParagraph oparagraph = new OParagraph();
    switch (cellRecord.TypeCode)
    {
      case TBIFFRecord.Formula:
        string strToken = cells.GetFormula(cellIndex).Remove(0, 1);
        if (!FormulaParser.IsTableRange(ref strToken, this.m_book, RangeImpl.GetRowFromCellIndex(cellIndex) + 1, RangeImpl.GetColumnFromCellIndex(cellIndex) + 1))
        {
          this.m_book.ThrowOnUnknownNames = false;
          string odfFormula = this.GenerateODFFormula(this.m_book.FormulaUtil.ParseString(strToken), strToken, this.m_book.FormulaUtil, sheet);
          cell.TableFormula = odfFormula;
          this.m_book.ThrowOnUnknownNames = true;
        }
        FormulaRecord record1 = (FormulaRecord) this.GetRecord(row, cellRecord.Column);
        if (record1.IsError)
        {
          string str = FormulaUtil.ErrorCodeToName[(int) record1.ErrorValue];
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Float;
          oparagraph.TextInput = str.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        if (record1.IsBool)
        {
          string str = record1.BooleanValue.ToString();
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Boolean;
          cell.TableFormula = str;
          cell.BooleanValue = Convert.ToBoolean(str);
          oparagraph.TextInput = str.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        if (record1.HasString)
        {
          string formulaStringValue = cells.GetFormulaStringValue(cellIndex);
          if (string.IsNullOrEmpty(formulaStringValue))
            break;
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.String;
          cell.Value = (object) formulaStringValue;
          oparagraph.TextInput = formulaStringValue;
          cell.Paragraph = oparagraph;
          break;
        }
        string str1 = cells.GetFormulaNumberValue(cellIndex).ToString();
        cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Float;
        cell.Value = (object) str1;
        oparagraph.TextInput = str1;
        cell.Paragraph = oparagraph;
        break;
      case TBIFFRecord.LabelSST:
      case TBIFFRecord.String:
        cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.String;
        cell.Value = (object) cells.GetText(cellIndex);
        if (cell.Value == null)
          break;
        oparagraph.TextInput = cell.Value.ToString();
        cell.Paragraph = oparagraph;
        break;
      case TBIFFRecord.Blank:
        cell.IsBlank = true;
        ++cell.ColumnsRepeated;
        break;
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        string numberFormat = cells.GetCellFormatting(cellIndex).NumberFormat;
        double numberValue = row.GetNumberValue(cellRecord.Column, sheet.Index);
        if (numberFormat.Contains("%"))
        {
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Percentage;
          cell.Value = (object) numberValue;
          oparagraph.TextInput = numberValue.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        if (numberFormat.Contains(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol) && string.IsNullOrEmpty(this.CheckForCulturePattern(numberFormat)))
        {
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Currency;
          cell.Value = (object) numberValue;
          oparagraph.TextInput = numberValue.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        FormatImpl innerFormat = this.m_book.InnerFormats[cells.GetCellFormatting(cellIndex).NumberFormatIndex];
        if (innerFormat.IsDateFormat(numberValue))
        {
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Date;
          cell.DateValue = DateTime.FromOADate(numberValue);
          oparagraph.TextInput = cell.DateValue.ToShortDateString();
          cell.Paragraph = oparagraph;
          break;
        }
        if (innerFormat.IsTimeFormat(numberValue))
        {
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Time;
          cell.TimeValue = TimeSpan.FromDays(numberValue);
          oparagraph.TextInput = cell.TimeValue.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        if (innerFormat.FormatType == ExcelFormatType.DateTime)
        {
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Date;
          cell.DateValue = DateTime.FromOADate(numberValue);
          oparagraph.TextInput = cell.DateValue.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Float;
        cell.Value = (object) numberValue;
        oparagraph.TextInput = numberValue.ToString();
        cell.Paragraph = oparagraph;
        break;
      case TBIFFRecord.BoolErr:
        BoolErrRecord record2 = (BoolErrRecord) this.GetRecord(row, cellRecord.Column);
        if (record2.IsErrorCode)
        {
          string str2 = FormulaUtil.ErrorCodeToName[(int) record2.BoolOrError];
          cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Float;
          cell.TableFormula = str2;
          oparagraph.TextInput = str2.ToString();
          cell.Paragraph = oparagraph;
          break;
        }
        cell.Type = Syncfusion.XlsIO.ODF.Base.CellValueType.Boolean;
        cell.BooleanValue = Convert.ToBoolean(record2.BoolOrError);
        oparagraph.TextInput = cell.BooleanValue.ToString();
        cell.Paragraph = oparagraph;
        break;
    }
  }

  private BiffRecordRaw GetRecord(RowStorage row, int colIndex)
  {
    BiffRecordRaw record = (BiffRecordRaw) null;
    RowStorageEnumerator enumerator = row.GetEnumerator(this.m_recordExtractor) as RowStorageEnumerator;
    while (enumerator.MoveNext())
    {
      if (enumerator.ColumnIndex == colIndex)
        record = enumerator.Current as BiffRecordRaw;
    }
    return record;
  }

  private void ConvertNamedRanges(OTable table)
  {
    for (int index = 0; index < this.m_book.Names.Count; ++index)
    {
      NameImpl nameImpl = this.m_book.InnerNamesColection[index] as NameImpl;
      IWorksheet worksheet = this.m_book.Worksheets[nameImpl.SheetIndex];
      if (nameImpl.RefersToRange != null && !nameImpl.Name.StartsWith("_xlnm"))
      {
        NamedRange namedRange = new NamedRange(nameImpl.Name);
        string cellNameWithDollars = RangeImpl.GetCellNameWithDollars(nameImpl.RefersToRange.Column, nameImpl.RefersToRange.Row);
        string str = worksheet == null ? "$A$1" : RangeImpl.GetCellNameWithDollars(worksheet.FirstVisibleColumn + 1, worksheet.FirstVisibleRow + 1);
        namedRange.CellRangeAddress = $"{worksheet.Name.Replace(" ", "_")}.{cellNameWithDollars}";
        namedRange.BaseCellAddress = $"{worksheet.Name.Replace(" ", "_")}.{str}";
        table.Expressions = new NamedExpressions();
        table.Expressions.NamedRanges.Add(namedRange);
      }
    }
  }

  private string GenerateODFFormula(
    Ptg[] ptgs,
    string formula,
    FormulaUtil util,
    WorksheetImpl sheet)
  {
    StringBuilder builder = new StringBuilder();
    for (int index = 0; index < ptgs.Length; ++index)
    {
      Ptg ptg = ptgs[index];
      switch (ptg.TokenCode)
      {
        case FormulaToken.tName1:
        case FormulaToken.tNameX1:
        case FormulaToken.tName2:
        case FormulaToken.tNameX2:
        case FormulaToken.tName3:
        case FormulaToken.tNameX3:
          NameImpl nameImpl = ptg.TokenCode == FormulaToken.tName1 || ptg.TokenCode == FormulaToken.tName2 || ptg.TokenCode == FormulaToken.tName3 ? this.m_book.InnerNamesColection[(ptg as NamePtg).ExternNameIndexInt - 1] as NameImpl : this.m_book.InnerNamesColection[(int) (ptg as NameXPtg).NameIndex - 1] as NameImpl;
          if (nameImpl != null)
          {
            string name = nameImpl.Name;
            if (formula.Contains(name))
              formula = formula.Replace(name, "$$" + name);
            if (formula.Contains("!"))
            {
              formula = formula.Replace("!", ".");
              break;
            }
            break;
          }
          break;
        case FormulaToken.tRef1:
        case FormulaToken.tRef2:
          RefPtg refPtg = ptg as RefPtg;
          string replacableString1 = $"[.{refPtg.ToString()}]";
          formula.IndexOf(refPtg.ToString());
          if (formula.Contains(refPtg.ToString()) && !formula.Contains(replacableString1))
          {
            builder = this.ProcessFormula(ref formula, refPtg.ToString(), replacableString1, builder);
            break;
          }
          break;
        case FormulaToken.tArea1:
        case FormulaToken.tArea2:
        case FormulaToken.tArea3:
          AreaPtg areaPtg = ptg as AreaPtg;
          if (areaPtg.IsWholeColumns((IWorkbook) this.m_book))
          {
            string columnName = RangeImpl.GetColumnName(areaPtg.FirstColumn + 1);
            if (!areaPtg.IsFirstColumnRelative)
            {
              string orgString = $"${columnName}:${columnName}";
              string replacableString2 = $"[.${columnName}:.${columnName}]";
              builder = this.ProcessFormula(ref formula, orgString, replacableString2, builder);
              break;
            }
            string orgString1 = $"{columnName}:{columnName}";
            string replacableString3 = $"[.{columnName}:.{columnName}]";
            builder = this.ProcessFormula(ref formula, orgString1, replacableString3, builder);
            break;
          }
          if (areaPtg.IsWholeRows((IWorkbook) this.m_book))
          {
            string str = (areaPtg.FirstRow + 1).ToString();
            if (!areaPtg.IsFirstRowRelative)
            {
              string orgString = $"${str}:${str}";
              string replacableString4 = $"[.${str}:.${str}]";
              builder = this.ProcessFormula(ref formula, orgString, replacableString4, builder);
              break;
            }
            string orgString2 = $"{str}:{str}";
            string replacableString5 = $"[.{str}:.{str}]";
            builder = this.ProcessFormula(ref formula, orgString2, replacableString5, builder);
            break;
          }
          if (areaPtg.IsFirstColumnRelative)
          {
            string columnName1 = RangeImpl.GetColumnName(areaPtg.FirstColumn + 1);
            string str1 = areaPtg.IsFirstColumnRelative ? columnName1 : "$" + columnName1;
            string str2 = (areaPtg.FirstRow + 1).ToString();
            string str3 = areaPtg.IsFirstRowRelative ? str2 : "$" + str2;
            string columnName2 = RangeImpl.GetColumnName(areaPtg.LastColumn + 1);
            string str4 = areaPtg.IsLastColumnRelative ? columnName2 : "$" + columnName2;
            string str5 = (areaPtg.LastRow + 1).ToString();
            string str6 = areaPtg.IsLastRowRelative ? str5 : "$" + str5;
            string orgString = $"{str1}{str3}:{str4}{str6}";
            string replacableString6 = $"[.{str1}{str3}:.{str4}{str6}]";
            builder = this.ProcessFormula(ref formula, orgString, replacableString6, builder);
            break;
          }
          break;
        case FormulaToken.tRef3d1:
        case FormulaToken.tRef3d2:
          Ref3DPtg ref3Dptg = ptg as Ref3DPtg;
          string str7 = Ref3DPtg.GetSheetName((IWorkbook) this.m_book, (int) ref3Dptg.RefIndex);
          string cellName = RangeImpl.GetCellName(ref3Dptg.ColumnIndex + 1, ref3Dptg.RowIndex + 1);
          string orgString3 = $"{str7}!{cellName}";
          string orgString4 = $"'{str7}'!{cellName}";
          if (str7.Contains(" "))
            str7 = str7.Replace(" ", "_");
          string replacableString7 = $"[{str7}.{cellName}]";
          string replacableString8 = $"['{str7}'.{cellName}]";
          if (formula.Contains(orgString3))
          {
            builder = this.ProcessFormula(ref formula, orgString3, replacableString7, builder);
            break;
          }
          if (formula.Contains(orgString4))
          {
            builder = this.ProcessFormula(ref formula, orgString4, replacableString8, builder);
            break;
          }
          break;
      }
      if (formula.Contains(util.OperandsSeparator))
        formula = formula.Replace(util.OperandsSeparator, util.ArrayRowSeparator);
    }
    builder.Append(formula);
    return builder.ToString();
  }

  private StringBuilder ProcessFormula(
    ref string formula,
    string orgString,
    string replacableString,
    StringBuilder builder)
  {
    int num = formula.IndexOf(orgString);
    string str = formula.Substring(0, num + orgString.Length).Replace(orgString, replacableString);
    builder.Append(str);
    int startIndex = num + orgString.Length;
    formula = formula.Substring(startIndex, formula.Length - startIndex);
    return builder;
  }

  private string CheckForCulturePattern(string formatString)
  {
    Match match = new Regex("\\[\\$(?<Character>.?)\\-(?<LocaleID>[0-9A-Za-z]+)\\]", RegexOptions.Compiled).Match(formatString);
    return match.Success ? match.Value : string.Empty;
  }

  private void LoadDefaults(WorksheetImpl sheet, ODFStyleCollection styles, OTable table)
  {
    OTableColumn otableColumn = new OTableColumn();
    this.LoadDefaultColumnStyle(styles, sheet);
    table.Columns.Add(otableColumn);
    this.LoadDefaultRowStyle(styles, sheet);
  }

  private void LoadDefaultRowStyle(ODFStyleCollection tableStyles, WorksheetImpl sheet)
  {
    ODFStyle style = new ODFStyle();
    style.Family = ODFFontFamily.Table_Row;
    style.TableRowProperties = new OTableRowProperties();
    style.TableRowProperties.RowHeight = sheet.StandardHeight;
    style.isDefault = true;
    tableStyles.Add(style);
  }

  private void LoadDefaultColumnStyle(ODFStyleCollection tableStyles, WorksheetImpl sheet)
  {
    ODFStyle style = new ODFStyle();
    style.Family = ODFFontFamily.Table_Column;
    style.TableColumnProperties = new OTableColumnProperties();
    style.TableColumnProperties.ColumnWidth = sheet.StandardWidth;
    style.isDefault = true;
    tableStyles.Add(style);
  }

  private void SerializeExcelBody(List<OTable> tables)
  {
    this.m_writer.SerializeBodyStart();
    this.m_writer.SerializeExcelBody(tables);
    this.m_writer.SerializeEnd();
  }

  private void MapCellName(
    ICellPositionFormat cellRecord,
    ODFStyleCollection inlineStyles,
    OTableCell cell,
    WorksheetImpl sheet)
  {
    int extendedFormatIndex = (int) cellRecord.ExtendedFormatIndex;
    ODFStyle odfStyle = (ODFStyle) null;
    inlineStyles.DictStyles.TryGetValue(extendedFormatIndex.ToString(), out odfStyle);
    if (odfStyle == null)
      return;
    cell.StyleName = odfStyle.Name;
    if (!(odfStyle.ParentStyleName == "Hyperlink"))
      return;
    OParagraph paragraph = cell.Paragraph;
    if (paragraph == null)
      return;
    paragraph.Anchor = new Anchor();
    IHyperLink hyperlink = sheet[cellRecord.Row + 1, cellRecord.Column + 1].Hyperlinks[0];
    if (hyperlink.Type == ExcelHyperLinkType.Workbook)
    {
      if (hyperlink.Address.Contains("!"))
        paragraph.Anchor.href = "#" + hyperlink.Address.Replace("!", ".").Replace("'", string.Empty).Replace(" ", "_");
    }
    else if (hyperlink.Type == ExcelHyperLinkType.File)
    {
      string str = hyperlink.Address;
      if (this.m_book.FullOutputFileName != null)
        str = new Uri(this.m_book.FullOutputFileName).MakeRelativeUri(new Uri(Path.Combine(Path.GetDirectoryName(this.m_book.FullFileName), hyperlink.Address))).OriginalString;
      paragraph.Anchor.href = "../" + str.Replace("\\", "/");
    }
    else
      paragraph.Anchor.href = hyperlink.Address;
    paragraph.Anchor.Name = hyperlink.Name;
    cell.Paragraph = paragraph;
  }

  private void ConvertPageLayoutProperties(IPageSetup setup, PageLayout layout)
  {
    layout.PageLayoutProperties.MarginTop = setup.HeaderMargin;
    layout.PageLayoutProperties.MarginBottom = setup.FooterMargin;
    layout.PageLayoutProperties.MarginLeft = setup.LeftMargin;
    layout.PageLayoutProperties.MarginRight = setup.RightMargin;
    layout.PageLayoutProperties.PageOrientation = setup.Orientation == ExcelPageOrientation.Portrait ? PrintOrientation.portrait : PrintOrientation.landscape;
    layout.PageLayoutProperties.PrintPageOrder = (PageOrder) setup.Order;
    layout.PageLayoutProperties.FirstPageNumber = setup.FirstPageNumber.ToString();
    layout.PageLayoutProperties.ScaleTo = setup.Zoom.ToString();
    if (setup.CenterHorizontally)
      layout.PageLayoutProperties.TableCentering |= TableCentering.Horizontal;
    if (!setup.CenterVertically)
      return;
    layout.PageLayoutProperties.TableCentering |= TableCentering.Vertical;
  }

  private void ConvertHeaderFooterStyles(IPageSetup setup, PageLayout layout)
  {
    HeaderFooterProperties footerProperties1 = new HeaderFooterProperties();
    footerProperties1.MarginLeft = setup.LeftMargin;
    footerProperties1.MarginRight = setup.RightMargin;
    footerProperties1.MinHeight = setup.TopMargin - setup.HeaderMargin;
    layout.HeaderStyle.HeaderFooterproperties = footerProperties1;
    layout.HeaderStyle.IsHeader = true;
    HeaderFooterProperties footerProperties2 = new HeaderFooterProperties();
    footerProperties2.MarginLeft = setup.LeftMargin;
    footerProperties2.MarginRight = setup.RightMargin;
    footerProperties2.MinHeight = setup.BottomMargin - setup.FooterMargin;
    layout.FooterStyle.HeaderFooterproperties = footerProperties2;
  }

  private void LoadMasterPage(IPageSetup setup, MasterPage page)
  {
    if (setup.DifferentOddAndEvenPagesHF)
      this.LoadHeaderFooter(setup, page);
    else
      this.LoadHeaderFooter(setup, page);
  }

  private void LoadHeaderFooter(IPageSetup setup, MasterPage page)
  {
    string fullHeaderString = (setup as PageSetupImpl).FullHeaderString;
    string fullFooterString = (setup as PageSetupImpl).FullFooterString;
    if (!string.IsNullOrEmpty(fullHeaderString))
    {
      page.Header = new HeaderFooterContent();
      if (!string.IsNullOrEmpty(setup.LeftHeader))
        page.Header.Display = true;
      if (!string.IsNullOrEmpty(setup.CenterHeader))
        page.Header.Display = true;
      string.IsNullOrEmpty(setup.RightHeader);
    }
    if (string.IsNullOrEmpty(fullFooterString))
      return;
    page.Footer = new HeaderFooterContent();
    if (!string.IsNullOrEmpty(setup.LeftFooter))
      page.Header.Display = true;
    if (!string.IsNullOrEmpty(setup.CenterFooter))
    {
      page.Header = new HeaderFooterContent();
      page.Header.Display = true;
    }
    if (string.IsNullOrEmpty(setup.RightFooter))
      return;
    page.Header.Display = true;
  }

  internal BorderLineStyle MapBorders(ODFBorder border, ExcelLineStyle style)
  {
    if (style != ExcelLineStyle.None)
      border.LineWidth = "thin";
    switch (style)
    {
      case ExcelLineStyle.None:
        return BorderLineStyle.none;
      case ExcelLineStyle.Thin:
      case ExcelLineStyle.Hair:
        border.LineWidth = "thin";
        return BorderLineStyle.solid;
      case ExcelLineStyle.Medium:
        border.LineWidth = "2pt";
        return BorderLineStyle.solid;
      case ExcelLineStyle.Dashed:
        border.LineWidth = "thin";
        return BorderLineStyle.dashed;
      case ExcelLineStyle.Dotted:
        border.LineWidth = "thin";
        return BorderLineStyle.dotted;
      case ExcelLineStyle.Thick:
        border.LineWidth = "thick";
        return BorderLineStyle.solid;
      case ExcelLineStyle.Double:
        return BorderLineStyle.Double;
      case ExcelLineStyle.Medium_dashed:
        border.LineWidth = "2pt";
        return BorderLineStyle.dashed;
      default:
        return BorderLineStyle.solid;
    }
  }

  private int UpdateCommentRange(IWorksheet sheet)
  {
    if (sheet.Comments.Count == 0)
      return -1;
    int num = 1;
    foreach (IComment comment in (IEnumerable) sheet.Comments)
    {
      if (num < comment.Column)
        num = comment.Column;
    }
    return num;
  }

  internal void Dispose()
  {
    if (this.m_writer != null)
      this.m_writer.Dispose();
    if (this.pageNames == null)
      return;
    this.pageNames.Clear();
    this.pageNames = (List<string>) null;
  }
}
