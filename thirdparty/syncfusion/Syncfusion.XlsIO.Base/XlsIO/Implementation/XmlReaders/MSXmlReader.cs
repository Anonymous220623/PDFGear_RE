// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.MSXmlReader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders;

public class MSXmlReader(IApplication application, object parent) : CommonObject(application, parent)
{
  private const string DEF_VERSION_STRING = "version=\"1.0\"";
  private const string DEF_XML_STRING = "xml";
  private const string DEF_APPLICATION_STRING = "progid=\"Excel.Sheet\"";
  private const string DEF_APPLICATION_STRING_SINGLE = "progid='Excel.Sheet'";
  private const string DEF_APPLICATION_NAME_STRING = "mso-application";
  private const string DEF_O_NAMESPACE = "urn:schemas-microsoft-com:office:office";
  private const string DEF_X_NAMESPACE = "urn:schemas-microsoft-com:office:excel";
  private const string DEF_SS_NAMESPACE = "urn:schemas-microsoft-com:office:spreadsheet";
  private const string DEF_HTML_NAMESPACE = "http://www.w3.org/TR/REC-html40";
  private const string DEF_SUBSCRIPT = "Subscript";
  private const string DEF_SUPERSCRIPT = "Superscript";
  private const string DEF_RTF_BOLD = "B";
  private const string DEF_RTF_ITALIC = "I";
  private const string DEF_RTF_UNDERLINE = "U";
  private const string DEF_RTF_STRIKETHROUGH = "S";
  private const string DEF_RTF_SPAN = "Span";
  private const string DEF_RTF_SUB = "Sub";
  private const string DEF_RTF_SUP = "Sup";
  private const string DEF_RTF_FONT = "Font";
  private const int DEF_SIZE_FONT = 10;
  private const string DEF_NONE = "None";
  private const string DefaultFontName = "Arial";
  private const string VersionAttribute = "version";
  private const string DefaultVersion = "1.0";
  private Dictionary<string, int> m_hashStyle = new Dictionary<string, int>();
  private WorkbookImpl m_parentBook;
  private List<string> m_arrNames = new List<string>();
  private Dictionary<long, MSXmlReader.FormulaData> m_hashFormula = new Dictionary<long, MSXmlReader.FormulaData>();
  private FormulaUtil m_formulaUtil;
  private static Dictionary<string, ExcelHAlign> m_hashHorizontalAll = new Dictionary<string, ExcelHAlign>(9);
  private static Dictionary<string, ExcelVAlign> m_hashVerticalAll = new Dictionary<string, ExcelVAlign>(7);
  private static Dictionary<string, string> m_hashNumberFormat = new Dictionary<string, string>(10);

  static MSXmlReader()
  {
    MSXmlReader.m_hashHorizontalAll.Add("Automatic", ExcelHAlign.HAlignGeneral);
    MSXmlReader.m_hashHorizontalAll.Add("Left", ExcelHAlign.HAlignLeft);
    MSXmlReader.m_hashHorizontalAll.Add("Center", ExcelHAlign.HAlignCenter);
    MSXmlReader.m_hashHorizontalAll.Add("Right", ExcelHAlign.HAlignRight);
    MSXmlReader.m_hashHorizontalAll.Add("Fill", ExcelHAlign.HAlignFill);
    MSXmlReader.m_hashHorizontalAll.Add("Justify", ExcelHAlign.HAlignJustify);
    MSXmlReader.m_hashHorizontalAll.Add("CenterAcrossSelection", ExcelHAlign.HAlignCenterAcrossSelection);
    MSXmlReader.m_hashHorizontalAll.Add("Distributed", ExcelHAlign.HAlignDistributed);
    MSXmlReader.m_hashHorizontalAll.Add("JustifyDistributed", ExcelHAlign.HAlignGeneral);
    MSXmlReader.m_hashVerticalAll.Add("Automatic", ExcelVAlign.VAlignBottom);
    MSXmlReader.m_hashVerticalAll.Add("Top", ExcelVAlign.VAlignTop);
    MSXmlReader.m_hashVerticalAll.Add("Bottom", ExcelVAlign.VAlignBottom);
    MSXmlReader.m_hashVerticalAll.Add("Center", ExcelVAlign.VAlignCenter);
    MSXmlReader.m_hashVerticalAll.Add("Justify", ExcelVAlign.VAlignJustify);
    MSXmlReader.m_hashVerticalAll.Add("Distributed", ExcelVAlign.VAlignDistributed);
    MSXmlReader.m_hashVerticalAll.Add("JustifyDistributed", ExcelVAlign.VAlignBottom);
    MSXmlReader.m_hashNumberFormat.Add("Fixed", "0.00");
    MSXmlReader.m_hashNumberFormat.Add("Standard", "#,##0.00");
    MSXmlReader.m_hashNumberFormat.Add("Percent", "0.00%");
    MSXmlReader.m_hashNumberFormat.Add("Scientific", "0.00E+0");
    MSXmlReader.m_hashNumberFormat.Add("Short Date", "m/d/yyyy");
    MSXmlReader.m_hashNumberFormat.Add("Medium Date", "d\\-mmm\\-yy");
    MSXmlReader.m_hashNumberFormat.Add("Medium Time", "h:mm AM/PM");
    MSXmlReader.m_hashNumberFormat.Add("Long Time", "h:mm:ss AM/PM");
    MSXmlReader.m_hashNumberFormat.Add("Short Time", "h:mm");
    MSXmlReader.m_hashNumberFormat.Add("General Date", "m/d/yy h:mm");
  }

  private void ReadWorksheet(XmlReader reader, WorkbookImpl book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (!reader.MoveToAttribute("Name", "urn:schemas-microsoft-com:office:spreadsheet") || reader.Value == null || reader.Value.Length <= 0)
      throw new XmlReadingException("Worksheet", "Worksheet name attribute isn't specified.");
    WorksheetImpl sheet = (WorksheetImpl) book.Worksheets.Create(reader.Value);
    sheet.IsParsing = true;
    if (reader.MoveToAttribute("RightToLeft", "urn:schemas-microsoft-com:office:spreadsheet"))
      sheet.IsRightToLeft = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "Table" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        this.ReadTable(reader, sheet);
      if (reader.LocalName == "Names" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        this.ReadNames(reader, sheet.Names, sheet.Index + 1);
      if (reader.LocalName == "WorksheetOptions" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadWorksheetOptions(reader, sheet);
      if (reader.LocalName == "DataValidation")
        this.ReadDataValidation(reader, sheet);
      if (reader.LocalName == "ConditionalFormatting")
        this.ReadConditionalFormats(reader, sheet);
      reader.Skip();
    }
    IName name = sheet.Names["_FilterDatabase"];
    if (name != null && sheet != null && name.RefersToRange != null)
      sheet.AutoFilters.FilterRange = name.RefersToRange;
    sheet.IsParsing = false;
  }

  private void ReadTable(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("DefaultRowHeight", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      sheet.StandardHeight = XmlConvertExtension.ToDouble(reader.Value);
      sheet.StandardHeightFlag = true;
      sheet.CustomHeight = true;
    }
    if (reader.MoveToAttribute("DefaultColumnWidth", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      double pixels = this.Application.ConvertUnits(XmlConvertExtension.ToDouble(reader.Value), MeasureUnits.Point, MeasureUnits.Pixel);
      double columnWidth = sheet.PixelsToColumnWidth((int) pixels);
      sheet.StandardWidth = columnWidth;
    }
    if (reader.MoveToAttribute("StyleID", "urn:schemas-microsoft-com:office:spreadsheet"))
      sheet.DefaultXFIndex = this.m_hashStyle[reader.Value];
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    int iRowIndex = 0;
    int iColumnIndex = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (iRowIndex >= sheet.Workbook.MaxRowCount && sheet.Workbook.Version < ExcelVersion.Excel2007)
        sheet.Workbook.Version = ExcelVersion.Excel2007;
      if (reader.LocalName == "Row" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        iRowIndex = this.ReadRow(reader, sheet, iRowIndex);
      if (iColumnIndex >= sheet.Workbook.MaxColumnCount && sheet.Workbook.Version < ExcelVersion.Excel2007)
        sheet.Workbook.Version = ExcelVersion.Excel2007;
      if (reader.LocalName == "Column" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        iColumnIndex = this.ReadColumn(reader, sheet, iColumnIndex);
      reader.Skip();
    }
    if (sheet.DefaultXFIndex == -1)
      return;
    ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
    record.FirstColumn = (ushort) iColumnIndex;
    record.LastColumn = (ushort) sheet.Workbook.MaxColumnCount;
    record.ExtendedFormatIndex = (ushort) sheet.DefaultXFIndex;
    sheet.ParseColumnInfo(record, false);
    sheet.ParentWorkbook.AddUsedStyleIndex(sheet.DefaultXFIndex);
  }

  private int ReadRow(XmlReader reader, WorksheetImpl sheet, int iRowIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    bool flag1 = true;
    int num1 = 0;
    bool flag2 = false;
    double val1 = sheet.StandardHeight;
    int iStyleIndex = sheet.DefaultXFIndex == -1 ? sheet.ParentWorkbook.DefaultXFIndex : sheet.DefaultXFIndex;
    int iIndex = 0;
    iRowIndex = reader.MoveToAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet") ? XmlConvertExtension.ToInt32(reader.Value) : ++iRowIndex;
    if (reader.MoveToAttribute("Height", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      val1 = XmlConvertExtension.ToDouble(reader.Value);
      flag2 = true;
    }
    bool flag3 = reader.MoveToAttribute("Hidden", "urn:schemas-microsoft-com:office:spreadsheet") && XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("StyleID", "urn:schemas-microsoft-com:office:spreadsheet"))
      iStyleIndex = this.GetXFIndex(sheet, reader.Value);
    if (reader.MoveToAttribute("AutoFitHeight", "urn:schemas-microsoft-com:office:spreadsheet"))
      flag1 = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("Span", "urn:schemas-microsoft-com:office:spreadsheet"))
      num1 = XmlConvertExtension.ToInt32(reader.Value);
    int num2 = iRowIndex;
    for (int index = iRowIndex + num1; num2 <= index; ++num2)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) sheet, num2 - 1, true);
      val1 = Math.Min(val1, 409.5);
      row.Height = (ushort) (val1 * 20.0);
      if (sheet.CustomHeight || flag2 || val1 != sheet.StandardHeight)
        row.IsBadFontHeight = true;
      row.ExtendedFormatIndex = (ushort) iStyleIndex;
      row.IsHidden = flag3;
      if (sheet.FirstRow < 0 || sheet.FirstRow > num2)
        sheet.FirstRow = num2;
      if (sheet.LastRow < num2)
        sheet.LastRow = num2;
    }
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return iRowIndex + num1;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (iRowIndex >= sheet.Workbook.MaxRowCount && sheet.Workbook.Version < ExcelVersion.Excel2007)
        sheet.Workbook.Version = ExcelVersion.Excel2007;
      if (iIndex >= sheet.Workbook.MaxColumnCount && sheet.Workbook.Version < ExcelVersion.Excel2007)
        sheet.Workbook.Version = ExcelVersion.Excel2007;
      if (reader.LocalName == "Cell" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        iIndex = this.ReadCell(reader, sheet, iRowIndex, iIndex);
      reader.Skip();
    }
    RowStorage row1 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) sheet, iRowIndex - 1, true);
    if (!this.Application.SkipAutoFitRow && flag1 && !row1.IsBadFontHeight)
    {
      sheet.AutofitRow(iRowIndex);
      if (val1 != sheet.StandardHeight && val1 > (double) row1.Height / 20.0)
      {
        row1.Height = (ushort) (val1 * 20.0);
        row1.IsBadFontHeight = true;
      }
    }
    else if (sheet.CustomHeight || val1 != sheet.StandardHeight)
      row1.IsBadFontHeight = true;
    sheet.ParentWorkbook.AddUsedStyleIndex(iStyleIndex);
    return iRowIndex + num1;
  }

  private int ReadColumn(XmlReader reader, WorksheetImpl sheet, int iColumnIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    double num1 = sheet.StandardWidth * 256.0;
    bool flag1 = false;
    bool flag2 = false;
    int num2 = 0;
    int iStyleIndex = sheet.DefaultXFIndex == -1 ? sheet.ParentWorkbook.DefaultXFIndex : sheet.DefaultXFIndex;
    iColumnIndex = reader.MoveToAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet") ? XmlConvertExtension.ToInt32(reader.Value) : ++iColumnIndex;
    if (reader.MoveToAttribute("Width", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      double pixels = this.Application.ConvertUnits(XmlConvertExtension.ToDouble(reader.Value), MeasureUnits.Point, MeasureUnits.Pixel);
      num1 = sheet.PixelsToColumnWidth((int) pixels) * 256.0;
    }
    if (reader.MoveToAttribute("AutoFitWidth", "urn:schemas-microsoft-com:office:spreadsheet"))
      flag2 = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("Hidden", "urn:schemas-microsoft-com:office:spreadsheet"))
      flag1 = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("StyleID", "urn:schemas-microsoft-com:office:spreadsheet"))
      iStyleIndex = this.GetXFIndex(sheet, reader.Value);
    if (reader.MoveToAttribute("Span", "urn:schemas-microsoft-com:office:spreadsheet"))
      num2 = XmlConvertExtension.ToInt32(reader.Value);
    int index1 = iColumnIndex;
    for (int index2 = iColumnIndex + num2; index1 <= index2; ++index1)
    {
      ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = (ushort) (index1 - 1);
      record.LastColumn = (ushort) (index1 - 1);
      record.IsHidden = flag1;
      record.ExtendedFormatIndex = (ushort) iStyleIndex;
      record.ColumnWidth = (ushort) num1;
      sheet.ColumnInformation[index1] = record;
    }
    if (flag2 && num1 == sheet.StandardWidth)
      sheet.AutofitColumn(iColumnIndex);
    sheet.ParentWorkbook.AddUsedStyleIndex(iStyleIndex);
    reader.MoveToElement();
    return iColumnIndex + num2;
  }

  private int ReadCell(XmlReader reader, WorksheetImpl sheet, int iRowIndex, int iIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    iIndex = reader.MoveToAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet") ? XmlConvertExtension.ToInt32(reader.Value) : ++iIndex;
    int num1 = sheet.DefaultXFIndex == -1 ? sheet.ParentWorkbook.DefaultXFIndex : sheet.DefaultXFIndex;
    CellRecordCollection cellRecords = sheet.CellRecords;
    if (reader.MoveToAttribute("StyleID", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      num1 = this.GetXFIndex(sheet, reader.Value);
    }
    else
    {
      ColumnInfoRecord columnInfoRecord = sheet.ColumnInformation[iIndex];
      int xformatIndex = (sheet.GetDefaultRowStyle(iRowIndex) as ExtendedFormatWrapper).XFormatIndex;
      if (xformatIndex != num1)
        num1 = xformatIndex;
      else if (columnInfoRecord != null)
        num1 = (int) columnInfoRecord.ExtendedFormatIndex;
    }
    int num2 = this.ReadMerge(reader, sheet, iRowIndex - 1, iIndex - 1, num1);
    string strFormula = reader.MoveToAttribute("Formula", "urn:schemas-microsoft-com:office:spreadsheet") ? reader.Value : (string) null;
    this.ParseHyperlink(reader, sheet, iRowIndex, iIndex);
    reader.MoveToElement();
    WorkbookXmlSerializator.XmlSerializationCellType type = WorkbookXmlSerializator.XmlSerializationCellType.Number;
    TextWithFormat rtf = (TextWithFormat) null;
    string str = (string) null;
    ushort xfIndex = 0;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "Data" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          str = this.ReadData(reader, cellRecords, num1, out type, out rtf, out xfIndex);
        if (reader.LocalName == "Comment" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        {
          CommentShapeImpl comment = (CommentShapeImpl) sheet.InnerComments.AddComment(iRowIndex, iIndex);
          this.ReadComment(reader, comment, num1, out xfIndex);
        }
        reader.Skip();
      }
    }
    if (xfIndex > (ushort) 0 && (int) xfIndex != num1 && (strFormula != null || str != null))
      num1 = (int) xfIndex;
    if (strFormula != null)
      this.ParseFormula(sheet, iRowIndex, iIndex, strFormula, num1, str, type);
    else if (str != null)
      this.SetCellRecord(type, str, cellRecords, iRowIndex, iIndex, num1, rtf);
    else
      cellRecords.SetBlank(iRowIndex, iIndex, num1);
    sheet.ParentWorkbook.AddUsedStyleIndex(num1);
    return iIndex + num2;
  }

  private void ParseHyperlink(XmlReader reader, WorksheetImpl sheet, int row, int column)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    string str1 = reader.MoveToAttribute("HRef", "urn:schemas-microsoft-com:office:spreadsheet") ? reader.Value : (string) null;
    string str2 = reader.MoveToAttribute("HRefScreenTip", "urn:schemas-microsoft-com:office:excel") ? reader.Value : (string) null;
    if (str1 == null)
      return;
    IRange range = sheet[row, column];
    HyperLinkImpl hyperLinkImpl = (HyperLinkImpl) sheet.HyperLinks.Add(range);
    string str3;
    hyperLinkImpl.Type = FormulaUtil.IsCell3D(str1, false, out str3, out str3, out str3) || this.m_formulaUtil.IsCellRange3D(str1, false, out str3, out str3, out str3, out str3, out str3) ? ExcelHyperLinkType.Workbook : (!str1.StartsWith("\\\\") ? (str1.StartsWith("mailto") || str1.IndexOf("://") != -1 ? ExcelHyperLinkType.Url : ExcelHyperLinkType.File) : ExcelHyperLinkType.Unc);
    hyperLinkImpl.SetAddress(str1, false);
    hyperLinkImpl.ScreenTip = str2;
  }

  private string ReadData(
    XmlReader reader,
    CellRecordCollection cells,
    int iXFIndex,
    out WorkbookXmlSerializator.XmlSerializationCellType type,
    out TextWithFormat rtf,
    out ushort xfIndex)
  {
    xfIndex = (ushort) 0;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string str1 = (string) null;
    FontsCollection innerFonts = this.m_parentBook.InnerFonts;
    int fontIndex = this.m_parentBook.InnerExtFormats[iXFIndex].FontIndex;
    rtf = new TextWithFormat(fontIndex);
    string str2 = reader.MoveToAttribute("Type", "urn:schemas-microsoft-com:office:spreadsheet") ? reader.Value : throw new XmlReadingException("table", "Undefined cell type.");
    type = (WorkbookXmlSerializator.XmlSerializationCellType) Enum.Parse(typeof (WorkbookXmlSerializator.XmlSerializationCellType), str2, true);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
      str1 = this.ReadRTF(reader, iXFIndex, rtf, out xfIndex, false);
    return str1;
  }

  private int ReadMerge(XmlReader reader, WorksheetImpl sheet, int iRow, int iCol, int iXFIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    int num1 = 0;
    int num2 = 0;
    if (reader.MoveToAttribute("MergeAcross", "urn:schemas-microsoft-com:office:spreadsheet"))
      num1 = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("MergeDown", "urn:schemas-microsoft-com:office:spreadsheet"))
      num2 = XmlConvertExtension.ToInt32(reader.Value);
    if (num1 != 0 || num2 != 0)
    {
      CellRecordCollection cellRecords = sheet.CellRecords;
      sheet.MergeCells.AddMerge(iRow, iRow + num2, iCol, iCol + num1, ExcelMergeOperation.Leave);
      for (int index1 = iRow; index1 <= iRow + num2; ++index1)
      {
        for (int index2 = iCol; index2 <= iCol + num1; ++index2)
          cellRecords.SetBlank(index1 + 1, index2 + 1, iXFIndex);
      }
      cellRecords.Remove(iRow + 1, iCol + 1);
    }
    return num1;
  }

  private void ReadStyles(XmlReader reader, WorkbookImpl book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "Style" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        this.ReadStyle(reader, book);
      reader.Skip();
    }
  }

  private void ReadStyle(XmlReader reader, WorkbookImpl book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ExtendedFormatsCollection coll = book != null ? book.InnerExtFormats : throw new ArgumentNullException(nameof (book));
    string str = (string) null;
    string strName = (string) null;
    string strParent = (string) null;
    if (reader.MoveToAttribute("ID", "urn:schemas-microsoft-com:office:spreadsheet"))
      str = reader.Value;
    if (reader.MoveToAttribute("Parent", "urn:schemas-microsoft-com:office:spreadsheet"))
      strParent = reader.Value;
    if (reader.MoveToAttribute("Name", "urn:schemas-microsoft-com:office:spreadsheet"))
      strName = reader.Value;
    ExtendedFormatImpl extendedFormat = this.GetExtendedFormat(coll, str, strName, strParent);
    if (str == "Default")
      extendedFormat.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
    reader.MoveToElement();
    if (reader.IsEmptyElement)
    {
      this.AddXFToCollection(book, coll, extendedFormat, str, strName);
    }
    else
    {
      reader.Read();
      if (strName != null)
      {
        extendedFormat.IncludeAlignment = true;
        extendedFormat.IncludeFont = true;
        extendedFormat.IncludePatterns = true;
        extendedFormat.IncludeNumberFormat = true;
        extendedFormat.IncludeProtection = true;
        extendedFormat.IncludeBorder = true;
      }
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "Alignment" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadAlignment(reader, extendedFormat);
        if (reader.LocalName == "Font" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadFont(reader, extendedFormat, str);
        if (reader.LocalName == "Interior" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadInterior(reader, extendedFormat);
        if (reader.LocalName == "NumberFormat" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadNumberFormat(reader, extendedFormat);
        if (reader.LocalName == "Protection" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadProtection(reader, extendedFormat);
        if (reader.LocalName == "Borders" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadBorders(reader, extendedFormat);
        reader.Skip();
      }
      if (str != "Default")
        this.AddXFToCollection(book, coll, extendedFormat, str, strName);
      else
        extendedFormat.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
    }
  }

  private void ReadAlignment(XmlReader reader, ExtendedFormatImpl format)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.AttributeCount == 0)
      return;
    if (reader.MoveToAttribute("Rotate", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      double result = 0.0;
      double.TryParse(reader.Value, out result);
      format.Rotation = result < 0.0 ? (int) (90.0 - result) : (int) result;
    }
    if (reader.MoveToAttribute("WrapText", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.WrapText = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("Indent", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.IndentLevel = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("ReadingOrder", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.ReadingOrder = (ExcelReadingOrderType) Enum.Parse(typeof (ExcelReadingOrderType), reader.Value, true);
    if (reader.MoveToAttribute("ShrinkToFit", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.ShrinkToFit = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("VerticalText", "urn:schemas-microsoft-com:office:spreadsheet") && XmlConvertExtension.ToBoolean(reader.Value))
      format.Rotation = (int) byte.MaxValue;
    if (reader.MoveToAttribute("Horizontal", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.HorizontalAlignment = MSXmlReader.m_hashHorizontalAll[reader.Value];
    if (reader.MoveToAttribute("Vertical", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.VerticalAlignment = MSXmlReader.m_hashVerticalAll[reader.Value];
    reader.MoveToElement();
    if (format.XFType != ExtendedFormatRecord.TXFType.XF_CELL)
      return;
    format.IncludeAlignment = false;
  }

  private void ReadFont(XmlReader reader, ExtendedFormatImpl format, string strIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.AttributeCount == 0)
    {
      IFont font1 = (IFont) new FontImpl(this.Application, (object) format.Workbook.InnerFonts);
      font1.FontName = "Arial";
      font1.Size = 10.0;
      IFont font2 = format.Workbook.InnerFonts.Add(font1);
      format.FontIndex = ((IInternalFont) font2).Index;
    }
    else
    {
      IFont font3 = (IFont) new FontImpl(this.Application, (object) format.Workbook.InnerFonts);
      if (reader.MoveToAttribute("Bold", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.Bold = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("Color", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.RGBColor = this.GetColor(reader.Value);
      font3.FontName = !reader.MoveToAttribute("FontName", "urn:schemas-microsoft-com:office:spreadsheet") ? "Arial" : reader.Value;
      if (reader.MoveToAttribute("Italic", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.Italic = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("Outline", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.MacOSOutlineFont = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("Shadow", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.MacOSShadow = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("Size", "urn:schemas-microsoft-com:office:spreadsheet"))
      {
        FontImpl fontImpl = (FontImpl) font3;
        font3.Size = XmlConvertExtension.ToDouble(reader.Value) >= fontImpl.MIN_FONT_SIZE ? (XmlConvertExtension.ToDouble(reader.Value) <= fontImpl.MAX_FONT_SIZE ? XmlConvertExtension.ToDouble(reader.Value) : fontImpl.MAX_FONT_SIZE) : fontImpl.MIN_FONT_SIZE;
      }
      else
        font3.Size = 10.0;
      if (reader.MoveToAttribute("StrikeThrough", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.Strikethrough = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("Underline", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3.Underline = (ExcelUnderline) Enum.Parse(typeof (ExcelUnderline), reader.Value, true);
      if (reader.MoveToAttribute("VerticalAlign", "urn:schemas-microsoft-com:office:spreadsheet"))
        font3 = this.SetFontAllign(font3, reader.Value);
      if (format.Index == 0 && strIndex == "Default")
      {
        ((FontImpl) font3).CopyTo((FontImpl) format.Workbook.InnerFonts[0]);
        format.FontIndex = ((IInternalFont) format.Workbook.InnerFonts[0]).Index;
      }
      else
      {
        IFont font4 = format.Workbook.InnerFonts.Add(font3);
        format.FontIndex = ((IInternalFont) font4).Index;
      }
      reader.MoveToElement();
      if (format.XFType != ExtendedFormatRecord.TXFType.XF_CELL)
        return;
      format.IncludeFont = false;
    }
  }

  private void ReadInterior(XmlReader reader, ExtendedFormatImpl format)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.AttributeCount == 0)
    {
      format.FillPattern = ExcelPattern.None;
    }
    else
    {
      if (reader.MoveToAttribute("Color", "urn:schemas-microsoft-com:office:spreadsheet"))
        format.Color = this.GetColor(reader.Value);
      if (reader.MoveToAttribute("PatternColor", "urn:schemas-microsoft-com:office:spreadsheet"))
        format.PatternColor = this.GetColor(reader.Value);
      if (reader.MoveToAttribute("Pattern", "urn:schemas-microsoft-com:office:spreadsheet"))
        format.FillPattern = (ExcelPattern) Array.IndexOf<string>(WorkbookXmlSerializator.DEF_PATTERN_STRING, reader.Value);
      reader.MoveToElement();
      if (format.XFType != ExtendedFormatRecord.TXFType.XF_CELL)
        return;
      format.IncludePatterns = false;
    }
  }

  private void ReadNumberFormat(XmlReader reader, ExtendedFormatImpl format)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    format.NumberFormat = "General";
    if (reader.AttributeCount == 0)
      return;
    if (reader.MoveToAttribute("Format", "urn:schemas-microsoft-com:office:spreadsheet"))
    {
      string key = reader.Value;
      if (MSXmlReader.m_hashNumberFormat.ContainsKey(key))
        key = MSXmlReader.m_hashNumberFormat[key];
      format.NumberFormat = key;
    }
    if (format.XFType == ExtendedFormatRecord.TXFType.XF_CELL)
      format.IncludeNumberFormat = false;
    reader.MoveToElement();
  }

  private void ReadProtection(XmlReader reader, ExtendedFormatImpl format)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.AttributeCount == 0)
      return;
    format.IncludeProtection = true;
    if (reader.MoveToAttribute("Protected", "urn:schemas-microsoft-com:office:spreadsheet"))
      format.Locked = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("HideFormula", "urn:schemas-microsoft-com:office:excel"))
      format.FormulaHidden = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (format.XFType != ExtendedFormatRecord.TXFType.XF_CELL)
      return;
    format.IncludeProtection = false;
  }

  private void ReadBorders(XmlReader reader, ExtendedFormatImpl format)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.IsEmptyElement)
    {
      if (format.TopBorderLineStyle != ExcelLineStyle.None)
        format.TopBorderLineStyle = ExcelLineStyle.None;
      if (format.BottomBorderLineStyle != ExcelLineStyle.None)
        format.BottomBorderLineStyle = ExcelLineStyle.None;
      if (format.LeftBorderLineStyle != ExcelLineStyle.None)
        format.LeftBorderLineStyle = ExcelLineStyle.None;
      if (format.RightBorderLineStyle != ExcelLineStyle.None)
        format.RightBorderLineStyle = ExcelLineStyle.None;
      if (format.DiagonalDownBorderLineStyle != ExcelLineStyle.None)
        format.DiagonalDownBorderLineStyle = ExcelLineStyle.None;
      if (format.DiagonalUpBorderLineStyle == ExcelLineStyle.None)
        return;
      format.DiagonalUpBorderLineStyle = ExcelLineStyle.None;
    }
    else
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "Border" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
          this.ReadBorder(reader, format);
        reader.Skip();
      }
      if (format.XFType != ExtendedFormatRecord.TXFType.XF_CELL)
        return;
      format.IncludeBorder = false;
    }
  }

  private void ReadBorder(XmlReader reader, ExtendedFormatImpl format)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.AttributeCount == 0)
      return;
    format.IncludeBorder = true;
    reader.MoveToAttribute("Position", "urn:schemas-microsoft-com:office:spreadsheet");
    string str = reader.Value;
    string style = "None";
    string weight = "0";
    int Index = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_BORDER_POSITION_STRING, str);
    IBorder border = format.Borders[(ExcelBordersIndex) Index];
    border.LineStyle = ExcelLineStyle.None;
    if (reader.MoveToAttribute("Color", "urn:schemas-microsoft-com:office:spreadsheet"))
      border.ColorRGB = this.GetColor(reader.Value);
    if (reader.MoveToAttribute("LineStyle", "urn:schemas-microsoft-com:office:spreadsheet"))
      style = reader.Value;
    if (reader.MoveToAttribute("Weight", "urn:schemas-microsoft-com:office:spreadsheet"))
      weight = reader.Value;
    border.LineStyle = this.GetLineStyle(style, weight);
    reader.MoveToElement();
  }

  private void ReadWorksheetOptions(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "TabColorIndex" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.TabColor = (ExcelKnownColors) Enum.Parse(typeof (ExcelKnownColors), reader.Value, true);
        reader.Skip();
      }
      if (reader.LocalName == "PageSetup" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadPageSetup(reader, sheet);
      if (reader.LocalName == "ActivePane" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.ActivePane = (int) XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "Panes" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadPanes(reader, sheet);
      if (reader.LocalName == "SplitHorizontal" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.HorizontalSplit = (int) XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "TopRowBottomPane" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.FirstVisibleRow = (int) XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "TopRowVisible" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.WindowTwo.TopRow = XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "SplitVertical" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.VerticalSplit = (int) XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "LeftColumnRightPane" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.FirstVisibleColumn = (int) XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "FreezePanes" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        sheet.WindowTwo.IsFreezePanes = true;
      if (reader.LocalName == "FrozenNoSplit" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        sheet.WindowTwo.IsFreezePanesNoSplit = true;
      if (reader.LocalName == "Zoom" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        sheet.Zoom = XmlConvertExtension.ToInt32(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "Print" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadPrint(reader, sheet);
      if (reader.LocalName == "DoNotDisplayGridlines" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        sheet.IsGridLinesVisible = false;
      if (reader.LocalName == "Visible")
        this.ParseVisibility(reader, sheet);
      else
        reader.Skip();
    }
  }

  private void ReadDataValidation(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    DataValidationCollection dvCollection;
    if (sheet.DVTable.Count == 0)
    {
      DValRecord record = (DValRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DVal);
      dvCollection = sheet.DVTable.Add(record);
    }
    else
      dvCollection = sheet.DVTable[0];
    this.ParseDataValidations(reader, dvCollection, sheet);
  }

  private void ParseDataValidations(
    XmlReader reader,
    DataValidationCollection dvCollection,
    WorksheetImpl sheet)
  {
    DataValidationImpl dv = new DataValidationImpl(dvCollection);
    TAddr taddr = new TAddr();
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      reader.MoveToElement();
      switch (reader.LocalName)
      {
        case "Range":
          TAddr[] forDataValidation = this.GetRangesForDataValidation(reader.ReadElementContentAsString(), sheet);
          foreach (TAddr tAddr in forDataValidation)
          {
            taddr = tAddr;
            dv.AddRanges(tAddr);
          }
          if (forDataValidation.Length > 1)
          {
            for (int index = 0; index < forDataValidation.Length; ++index)
            {
              taddr.FirstRow = forDataValidation[index].FirstRow < taddr.FirstRow ? forDataValidation[index].FirstRow : taddr.FirstRow;
              taddr.FirstCol = forDataValidation[index].FirstCol < taddr.FirstCol ? forDataValidation[index].FirstCol : taddr.FirstCol;
            }
            continue;
          }
          continue;
        case "Type":
          dv.AllowType = this.ConvertDataValidationType(reader.ReadElementContentAsString());
          continue;
        case "Min":
          dv.SetFormulaValue(reader.ReadElementContentAsString(), this.m_formulaUtil, taddr, true);
          continue;
        case "Max":
          dv.SetFormulaValue(reader.ReadElementContentAsString(), this.m_formulaUtil, taddr, false);
          continue;
        case "InputTitle":
          dv.PromptBoxTitle = reader.ReadElementContentAsString();
          continue;
        case "InputMessage":
          dv.PromptBoxText = reader.ReadElementContentAsString();
          continue;
        case "ErrorStyle":
          dv.ErrorStyle = this.ConvertDataValidationErrorStyle(reader.ReadElementContentAsString());
          continue;
        case "ErrorMessage":
          dv.ErrorBoxText = reader.ReadElementContentAsString();
          continue;
        case "ErrorTitle":
          dv.ErrorBoxTitle = reader.ReadElementContentAsString();
          continue;
        case "Value":
          dv.SetFormulaValue(reader.ReadElementContentAsString(), this.m_formulaUtil, taddr, true);
          continue;
        case "Qualifier":
          dv.CompareOperator = (ExcelDataValidationComparisonOperator) Enum.Parse(typeof (ExcelDataValidationComparisonOperator), reader.ReadElementContentAsString(), true);
          continue;
        case "CellRangeList":
          reader.Skip();
          continue;
        default:
          reader.Skip();
          continue;
      }
    }
    dvCollection.Add(dv);
  }

  private ExcelDataType ConvertDataValidationType(string dataValidationType)
  {
    int num = dataValidationType != null && !(dataValidationType == string.Empty) ? Array.IndexOf<string>(WorkbookXmlSerializator.DEF_ALLOWTYPE_STRING, dataValidationType) : throw new ArgumentNullException("strErrorStyle");
    return num <= 0 ? ExcelDataType.Any : (ExcelDataType) num;
  }

  private ExcelErrorStyle ConvertDataValidationErrorStyle(string strErrorStyle)
  {
    int num = strErrorStyle != null && !(strErrorStyle == string.Empty) ? Array.IndexOf<string>(WorkbookXmlSerializator.DEF_ERRORSTYLE, strErrorStyle) : throw new ArgumentNullException(nameof (strErrorStyle));
    return num <= 0 ? ExcelErrorStyle.Stop : (ExcelErrorStyle) num;
  }

  private TAddr[] GetRangesForDataValidation(string strRange, WorksheetImpl sheet)
  {
    string[] strArray = strRange != null && !(strRange == string.Empty) ? strRange.Split(',') : throw new ArgumentNullException(nameof (strRange));
    List<TAddr> taddrList = new List<TAddr>();
    foreach (string strRange1 in strArray)
      taddrList.Add(this.GetRangeForDVOrAF(strRange1, sheet));
    return taddrList.ToArray();
  }

  private TAddr GetRangeForDVOrAF(string strRange, WorksheetImpl sheet)
  {
    string str = strRange != null && !(strRange == string.Empty) ? string.Empty : throw new ArgumentNullException(nameof (strRange));
    string empty = string.Empty;
    string strRow2 = string.Empty;
    string strColumn2 = string.Empty;
    TAddr rangeForDvOrAf = new TAddr();
    if (FormulaUtil.IsCell(strRange, true, out str, out empty))
    {
      int int32_1 = Convert.ToInt32(str.Remove(0, 1));
      int int32_2 = Convert.ToInt32(empty.Remove(0, 1));
      rangeForDvOrAf = new TAddr(int32_1 - 1, int32_2 - 1, int32_1 - 1, int32_2 - 1);
    }
    if (this.m_formulaUtil.IsCellRange(strRange, true, out str, out empty, out strRow2, out strColumn2))
    {
      int int32_3 = Convert.ToInt32(str.Remove(0, 1));
      int int32_4 = Convert.ToInt32(empty.Remove(0, 1));
      int int32_5 = Convert.ToInt32(strRow2.Remove(0, 1));
      int int32_6 = Convert.ToInt32(strColumn2.Remove(0, 1));
      rangeForDvOrAf = new TAddr(int32_3 - 1, int32_4 - 1, int32_5 - 1, int32_6 - 1);
      if ((int32_5 > sheet.Workbook.MaxRowCount || int32_6 > sheet.Workbook.MaxColumnCount) && sheet.Workbook.Version < ExcelVersion.Excel2007)
        sheet.Workbook.Version = ExcelVersion.Excel2007;
    }
    return rangeForDvOrAf;
  }

  private static SelectionRecord.TAddr[] ConvertToSelectionTAddr(TAddr[] tAddrArray)
  {
    SelectionRecord.TAddr[] selectionTaddr = new SelectionRecord.TAddr[tAddrArray.Length];
    for (int index = 0; index < tAddrArray.Length; ++index)
      selectionTaddr[index] = new SelectionRecord.TAddr(tAddrArray[index].FirstRow, tAddrArray[index].LastRow, tAddrArray[index].FirstCol, tAddrArray[index].LastCol);
    return selectionTaddr;
  }

  private void ReadConditionalFormats(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.NodeType == XmlNodeType.None)
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "ConditionalFormatting")
      {
        ConditionalFormats conditionalFormats = new ConditionalFormats(sheet.Application, (object) sheet.ConditionalFormats);
        if (this.ParseConditionalFormatting(reader, conditionalFormats, sheet) && conditionalFormats.Count != 0)
        {
          if (conditionalFormats.CondFMTRecord != null && conditionalFormats.CondFMTRecord.CellList.Count > 0 && conditionalFormats.CondFMTRecord.CFNumber == (ushort) 0)
            conditionalFormats.CondFMTRecord.CFNumber = (ushort) conditionalFormats.InnerList.Count;
          if (conditionalFormats.CondFMT12Record != null && conditionalFormats.CondFMT12Record.CellList.Count > 0 && conditionalFormats.CondFMT12Record.CF12RecordCount == (ushort) 0)
            conditionalFormats.CondFMT12Record.CF12RecordCount = (ushort) conditionalFormats.InnerList.Count;
          sheet.ConditionalFormats.Add(conditionalFormats);
        }
      }
      else
        reader.Skip();
    }
  }

  private bool ParseConditionalFormatting(
    XmlReader reader,
    ConditionalFormats conditionalFormats,
    WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (conditionalFormats == null)
      throw new ArgumentNullException(nameof (conditionalFormats));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return false;
    reader.Read();
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      reader.MoveToElement();
      if (reader.LocalName == "Range")
      {
        reader.Read();
        foreach (TAddr taddr in this.GetRangesForDataValidation(reader.Value, sheet))
        {
          Rectangle rectangle = taddr.GetRectangle();
          conditionalFormats.AddRange(rectangle);
          conditionalFormats.EnclosedRange = taddr;
        }
        reader.Skip();
      }
    }
    ConditionalFormats formats = conditionalFormats;
    this.ParseConditionalFormat(reader, formats);
    return true;
  }

  private void ParseConditionalFormat(XmlReader reader, ConditionalFormats formats)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (formats == null)
      throw new ArgumentNullException(nameof (formats));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "Condition")
      {
        ConditionalFormatImpl conditionalFormat = new ConditionalFormatImpl(formats.Application, (object) formats);
        this.ParseCondition(reader, conditionalFormat);
        formats.Add((IConditionalFormat) conditionalFormat);
      }
      else
        reader.Skip();
    }
  }

  private void ParseCondition(XmlReader reader, ConditionalFormatImpl conditionalFormat)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (conditionalFormat == null)
      throw new ArgumentNullException(nameof (conditionalFormat));
    if (reader.LocalName == "Condition")
    {
      reader.MoveToElement();
      reader.Read();
      bool flag = false;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        reader.MoveToElement();
        switch (reader.LocalName)
        {
          case "Qualifier":
            string strOperator = reader.ReadElementContentAsString();
            if (strOperator != null)
              conditionalFormat.Operator = this.ConvertCFOperator(strOperator);
            flag = true;
            continue;
          case "Value1":
            string str1 = reader.ReadElementContentAsString();
            if (str1 != null)
            {
              conditionalFormat.FirstFormulaR1C1 = str1;
              continue;
            }
            continue;
          case "Value2":
            string str2 = reader.ReadElementContentAsString();
            if (str2 != null)
            {
              conditionalFormat.SecondFormulaR1C1 = str2;
              continue;
            }
            continue;
          case "Format":
            if (reader.MoveToAttribute("Style"))
            {
              this.ParseConditionalFormatString(reader.Value, conditionalFormat);
            }
            else
            {
              reader.MoveToAttribute("Style", "urn:schemas-microsoft-com:office:excel");
              this.ParseConditionalFormatString(reader.Value, conditionalFormat);
            }
            reader.Read();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      if (!flag)
        conditionalFormat.FormatType = ExcelCFType.Formula;
    }
    reader.Read();
  }

  private void ParseConditionalFormatString(
    string formattingValues,
    ConditionalFormatImpl conditionalFormat)
  {
    if (formattingValues == null || formattingValues.Length == 0)
      throw new ArgumentNullException(nameof (formattingValues));
    if (conditionalFormat == null)
      throw new ArgumentNullException(nameof (conditionalFormat));
    string str1 = formattingValues;
    char[] chArray1 = new char[1]{ ";"[0] };
    foreach (string str2 in str1.Split(chArray1))
    {
      char[] chArray2 = new char[1]{ ":"[0] };
      string[] strArray1 = str2.Split(chArray2);
      for (int index = 0; index < strArray1.Length; ++index)
      {
        switch (strArray1[index])
        {
          case "color":
            conditionalFormat.IsFontFormatPresent = true;
            conditionalFormat.IsFontColorPresent = true;
            conditionalFormat.FontColorRGB = this.GetColor(strArray1[++index]);
            break;
          case "font-weight":
            conditionalFormat.IsBold = strArray1[++index] == "700";
            break;
          case "font-style":
            conditionalFormat.IsItalic = true;
            break;
          case "text-line-through":
            if (strArray1[++index] == "single")
            {
              conditionalFormat.IsStrikeThrough = true;
              break;
            }
            break;
          case "text-underline-style":
            conditionalFormat.Underline = (ExcelUnderline) Enum.Parse(typeof (ExcelUnderline), strArray1[++index], true);
            break;
          case "background":
            conditionalFormat.BackColorRGB = this.GetColor(strArray1[++index]);
            break;
          case "border-top":
            string[] borderSetting1 = this.GetBorderSetting(strArray1[++index]);
            conditionalFormat.TopBorderStyle = this.GetBroderLineStyle(borderSetting1);
            conditionalFormat.TopBorderColorRGB = this.GetColor(borderSetting1[0]);
            break;
          case "border-bottom":
            string[] borderSetting2 = this.GetBorderSetting(strArray1[++index]);
            conditionalFormat.BottomBorderStyle = this.GetBroderLineStyle(borderSetting2);
            conditionalFormat.BottomBorderColorRGB = this.GetColor(borderSetting2[0]);
            break;
          case "border-left":
            string[] borderSetting3 = this.GetBorderSetting(strArray1[++index]);
            conditionalFormat.LeftBorderStyle = this.GetBroderLineStyle(borderSetting3);
            conditionalFormat.LeftBorderColorRGB = this.GetColor(borderSetting3[0]);
            break;
          case "border-right":
            string[] borderSetting4 = this.GetBorderSetting(strArray1[++index]);
            conditionalFormat.RightBorderStyle = this.GetBroderLineStyle(borderSetting4);
            conditionalFormat.RightBorderColorRGB = this.GetColor(borderSetting4[0]);
            break;
          case "mso-pattern":
            string[] strArray2 = strArray1[++index].Split(' ');
            if (strArray2.Length > 1 && strArray2[1] != null && strArray2[1].Length > 0)
            {
              int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_PATTERN_STRING_CF, strArray2[1]);
              conditionalFormat.FillPattern = num > 0 ? (ExcelPattern) num : ExcelPattern.None;
              break;
            }
            if (strArray2.Length >= 1 && strArray2[0] != null && strArray2[0].Length > 0)
            {
              int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_PATTERN_STRING_CF, strArray2[0]);
              conditionalFormat.FillPattern = num > 0 ? (ExcelPattern) num : ExcelPattern.None;
              break;
            }
            break;
        }
      }
    }
  }

  private string[] GetBorderSetting(string borderSetting)
  {
    string[] strArray = borderSetting != null && borderSetting.Length != 0 ? borderSetting.Split(' ') : throw new ArgumentNullException(nameof (borderSetting));
    string[] borderSetting1 = new string[strArray.Length];
    int index1 = 0;
    int index2 = strArray.Length - 1;
    for (; index1 < strArray.Length; ++index1)
    {
      borderSetting1[index1] = strArray[index2];
      --index2;
    }
    return borderSetting1;
  }

  private ExcelLineStyle GetBroderLineStyle(string[] style)
  {
    if (style == null || style.Length == 0)
      throw new ArgumentNullException(nameof (style));
    string str = !(style[1] != "none") ? style[1] : $"{style[2]} {style[1]}";
    int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_BORDER_LINE_CF, str);
    return num <= 0 ? ExcelLineStyle.None : (ExcelLineStyle) num;
  }

  private ExcelComparisonOperator ConvertCFOperator(string strOperator)
  {
    int num = strOperator != null && strOperator.Length != 0 ? Array.IndexOf<string>(WorkbookXmlSerializator.DEF_COMPARISION_OPERATORS_PREF, strOperator) : throw new ArgumentNullException(nameof (strOperator));
    return num <= 0 ? ExcelComparisonOperator.None : (ExcelComparisonOperator) num;
  }

  private void ParseVisibility(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    string str = reader.ReadElementContentAsString();
    int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_VISIBLE_STRING, str);
    sheet.Visibility = num >= 0 ? (WorksheetVisibility) num : throw new XmlException();
  }

  private void ReadPageSetup(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if ((reader.LocalName == "Footer" || reader.LocalName == "Header") && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadHeaderFooter(reader, sheet);
      if (reader.LocalName == "Layout" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadLayout(reader, sheet);
      if (reader.LocalName == "PageMargins" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadPageMargins(reader, sheet);
      reader.Skip();
    }
  }

  private void ReadHeaderFooter(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    string localName = reader.LocalName;
    PageSetupImpl pageSetup = (PageSetupImpl) sheet.PageSetup;
    switch (localName)
    {
      case "Footer":
        if (reader.MoveToAttribute("Margin", "urn:schemas-microsoft-com:office:excel"))
          pageSetup.FooterMargin = XmlConvertExtension.ToDouble(reader.Value);
        if (!reader.MoveToAttribute("Data", "urn:schemas-microsoft-com:office:excel"))
          break;
        pageSetup.FullFooterString = reader.Value;
        break;
      case "Header":
        if (reader.MoveToAttribute("Margin", "urn:schemas-microsoft-com:office:excel"))
          pageSetup.HeaderMargin = XmlConvertExtension.ToDouble(reader.Value);
        if (!reader.MoveToAttribute("Data", "urn:schemas-microsoft-com:office:excel"))
          break;
        pageSetup.FullHeaderString = reader.Value;
        break;
    }
  }

  private void ReadLayout(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    IPageSetup pageSetup = sheet != null ? sheet.PageSetup : throw new ArgumentNullException(nameof (sheet));
    pageSetup.Orientation = ExcelPageOrientation.Portrait;
    if (reader.MoveToAttribute("StartPageNumber", "urn:schemas-microsoft-com:office:excel"))
    {
      pageSetup.AutoFirstPageNumber = false;
      uint int32 = (uint) XmlConvertExtension.ToInt32(reader.Value);
      pageSetup.FirstPageNumber = (short) int32;
    }
    if (reader.MoveToAttribute("Orientation", "urn:schemas-microsoft-com:office:excel"))
      pageSetup.Orientation = (ExcelPageOrientation) Enum.Parse(typeof (ExcelPageOrientation), reader.Value, true);
    if (reader.MoveToAttribute("CenterHorizontal", "urn:schemas-microsoft-com:office:excel"))
      pageSetup.CenterHorizontally = XmlConvertExtension.ToBoolean(reader.Value);
    if (!reader.MoveToAttribute("CenterVertical", "urn:schemas-microsoft-com:office:excel"))
      return;
    pageSetup.CenterVertically = XmlConvertExtension.ToBoolean(reader.Value);
  }

  private void ReadPageMargins(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    PageSetupImpl pageSetupImpl = sheet != null ? (PageSetupImpl) sheet.PageSetup : throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("Right", "urn:schemas-microsoft-com:office:excel"))
      pageSetupImpl.RightMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("Left", "urn:schemas-microsoft-com:office:excel"))
      pageSetupImpl.LeftMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("Bottom", "urn:schemas-microsoft-com:office:excel"))
      pageSetupImpl.BottomMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (!reader.MoveToAttribute("Top", "urn:schemas-microsoft-com:office:excel"))
      return;
    pageSetupImpl.TopMargin = XmlConvertExtension.ToDouble(reader.Value);
  }

  private void ReadPanes(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "Pane" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        this.ReadSelectionPane(reader, sheet);
      reader.Skip();
    }
  }

  private void ReadSelectionPane(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    SelectionRecord record = (SelectionRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Selection);
    TAddr[] tAddrArray = (TAddr[]) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "Number" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        record.Pane = XmlConvertExtension.ToByte(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "ActiveCol" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        record.ColumnActiveCell = XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "ActiveRow" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        record.RowActiveCell = XmlConvertExtension.ToUInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "RangeSelection" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        tAddrArray = this.GetRangesForDataValidation(reader.ReadElementContentAsString(), sheet);
      reader.Skip();
    }
    if (tAddrArray == null)
    {
      record.Addr = new SelectionRecord.TAddr[1]
      {
        new SelectionRecord.TAddr((int) record.RowActiveCell, (int) record.RowActiveCell, (int) record.ColumnActiveCell, (int) record.ColumnActiveCell)
      };
    }
    else
    {
      SelectionRecord.TAddr[] selectionTaddr = MSXmlReader.ConvertToSelectionTAddr(tAddrArray);
      record.Addr = selectionTaddr;
    }
    sheet.Selections.Add(record);
  }

  private void ReadPrint(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    PageSetupImpl pageSetup = (PageSetupImpl) sheet.PageSetup;
    pageSetup.PaperSize = ExcelPaperSize.PaperLetter;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "NumberofCopies" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        pageSetup.Copies = XmlConvertExtension.ToInt32(reader.Value);
        reader.Read();
      }
      if (reader.LocalName == "HorizontalResolution" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        pageSetup.PrintQuality = XmlConvertExtension.ToInt32(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "PaperSizeIndex" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        pageSetup.PaperSize = (ExcelPaperSize) XmlConvertExtension.ToInt16(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "FitWidth" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        pageSetup.FitToPagesWide = XmlConvertExtension.ToInt32(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "FitHeight" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        pageSetup.FitToPagesTall = XmlConvertExtension.ToInt32(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "Scale" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        pageSetup.Zoom = XmlConvertExtension.ToInt32(reader.Value);
        reader.Skip();
      }
      if (reader.LocalName == "Gridlines" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        pageSetup.PrintGridlines = true;
      if (reader.LocalName == "BlackAndWhite" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        pageSetup.BlackAndWhite = true;
      if (reader.LocalName == "DraftQuality" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        pageSetup.Draft = true;
      if (reader.LocalName == "RowColHeadings" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        pageSetup.PrintHeadings = true;
      if (reader.LocalName == "CommentsLayout" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_PRINT_LOCATION_STRING, reader.Value);
        if (num != -1)
          pageSetup.PrintComments = (ExcelPrintLocation) num;
        reader.Skip();
      }
      if (reader.LocalName == "PrintErrors" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.Read();
        int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_PRINT_ERROR_STRING, reader.Value);
        if (num != -1)
          pageSetup.PrintErrors = (ExcelPrintErrors) num;
        reader.Skip();
      }
      if (reader.LocalName == "LeftToRight" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
        sheet.PageSetup.Order = ExcelOrder.OverThenDown;
      reader.Skip();
    }
  }

  private void ReadNames(XmlReader reader, INames namesColl, int iSheetIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (namesColl == null)
      throw new ArgumentNullException(nameof (namesColl));
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "NamedRange" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        this.ReadName(reader, namesColl, iSheetIndex);
      reader.Skip();
    }
  }

  private void ReadName(XmlReader reader, INames namesColl, int sheetIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (namesColl == null)
      throw new ArgumentNullException(nameof (namesColl));
    if (sheetIndex < 0)
      throw new ArgumentNullException(nameof (sheetIndex));
    NameRecord record = (NameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Name);
    if (reader.MoveToAttribute("Hidden", "urn:schemas-microsoft-com:office:spreadsheet"))
      record.IsNameHidden = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToAttribute("Name", "urn:schemas-microsoft-com:office:spreadsheet");
    record.Name = reader.Value;
    reader.MoveToAttribute("RefersTo", "urn:schemas-microsoft-com:office:spreadsheet");
    this.m_arrNames.Add(reader.Value);
    record.IndexOrGlobal = (ushort) sheetIndex;
    NameImpl nameImpl = new NameImpl(this.Application, (object) namesColl, record);
    string strFormula = reader.Value;
    if (strFormula[0] == '=')
      strFormula = reader.Value.Substring(1);
    nameImpl.SetValue(this.m_formulaUtil.ParseString(strFormula, (IWorksheet) null, (Dictionary<string, string>) null, 0, 0, true));
    namesColl.Add((IName) nameImpl);
    reader.MoveToElement();
  }

  private void ReadComment(
    XmlReader reader,
    CommentShapeImpl comment,
    int iStyleIndex,
    out ushort xfIndex)
  {
    xfIndex = (ushort) 0;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    if (reader.MoveToAttribute("Author", "urn:schemas-microsoft-com:office:spreadsheet"))
      comment.Author = reader.Value;
    if (reader.MoveToAttribute("ShowAlways", "urn:schemas-microsoft-com:office:spreadsheet"))
      comment.IsShapeVisible = XmlConvertExtension.ToBoolean(reader.Value);
    comment.ShapeType = ExcelShapeType.Comment;
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "Data" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
      {
        TextWithFormat textObject = ((RichTextString) comment.RichText).TextObject;
        this.ReadCommentData(reader, iStyleIndex, textObject, out xfIndex);
      }
      reader.Skip();
    }
  }

  private void ReadCommentData(
    XmlReader reader,
    int iXFIndex,
    TextWithFormat rtf,
    out ushort xfIndex)
  {
    xfIndex = (ushort) 0;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    this.ReadRTF(reader, iXFIndex, rtf, out xfIndex, true);
  }

  public void FillWorkbook(XmlReader reader, WorkbookImpl book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    bool flag1;
    if (!reader.EOF)
    {
      reader.Read();
      flag1 = reader.NodeType != XmlNodeType.XmlDeclaration || !(reader.Name == "xml");
      if (reader.MoveToAttribute("version"))
        flag1 |= !(XmlConvert.DecodeName(reader.Value) == "1.0");
    }
    else
      flag1 = true;
    bool flag2;
    if (!reader.EOF)
    {
      reader.Read();
      if (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      if (reader.NodeType == XmlNodeType.Element)
      {
        flag2 = flag1 || reader.LocalName != "Workbook";
      }
      else
      {
        bool flag3 = flag1 || reader.NodeType != XmlNodeType.ProcessingInstruction || !(reader.Name == "mso-application");
        string str = XmlConvert.DecodeName(reader.Value);
        flag2 = flag3 || !(str == "progid=\"Excel.Sheet\"") && !(str == "progid='Excel.Sheet'");
      }
    }
    else
      flag2 = true;
    if (flag2)
      throw new XmlReadingException("strict", "Wrong file header. File is likely to be corrupt.");
    this.ReadWorkbook(reader, book);
  }

  private void ReadWorkbook(XmlReader reader, WorkbookImpl book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    this.m_parentBook = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_formulaUtil = new FormulaUtil(this.m_parentBook.Application, (object) this.m_parentBook, NumberFormatInfo.InvariantInfo, ',', ';');
    int content = (int) reader.MoveToContent();
    if (reader.LocalName != "Workbook" || reader.IsEmptyElement)
      throw new XmlReadingException(nameof (book), "Workbook node is empty.");
    reader.Read();
    bool flag1 = false;
    this.m_arrNames.Clear();
    int num1 = 0;
    int num2 = 0;
    bool flag2 = true;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "ExcelWorkbook" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
      {
        reader.MoveToElement();
        if (!reader.IsEmptyElement)
        {
          reader.Read();
          while (reader.NodeType != XmlNodeType.EndElement)
          {
            if (reader.LocalName == "ActiveSheet" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
            {
              reader.Read();
              num1 = XmlConvertExtension.ToInt32(reader.Value);
              reader.Skip();
            }
            if (reader.LocalName == "FirstVisibleSheet" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:excel")
            {
              reader.Read();
              num2 = XmlConvertExtension.ToInt32(reader.Value);
              reader.Skip();
            }
            if (reader.LocalName == "HideWorkbookTabs")
            {
              flag2 = false;
              reader.Skip();
            }
            reader.Skip();
          }
        }
      }
      if (reader.LocalName == "Worksheet" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
      {
        this.ReadWorksheet(reader, book);
        flag1 = true;
      }
      if (reader.LocalName == "Styles" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        this.ReadStyles(reader, book);
      if (reader.LocalName == "Names" && reader.NamespaceURI == "urn:schemas-microsoft-com:office:spreadsheet")
        this.ReadNames(reader, book.Names, 0);
      reader.Skip();
    }
    if (!flag1)
      throw new XmlReadingException(nameof (book), "Cannot find worksheet node.");
    this.ReparseNames(book);
    this.ReparseFormula(book);
    book.ActiveSheetIndex = num1;
    book.DisplayedTab = num2;
    book.ShowSheetTabs = flag2;
  }

  private ExtendedFormatImpl GetExtendedFormat(
    ExtendedFormatsCollection coll,
    string strIndex,
    string strName,
    string strParent)
  {
    switch (strIndex)
    {
      case null:
        throw new XmlReadingException("style", "File is corrupt - wrong style index.");
      case "Default":
        this.m_hashStyle.Add("Default", coll.ParentWorkbook.DefaultXFIndex);
        return coll[0];
      default:
        ExtendedFormatImpl extendedFormat;
        if (strParent != null)
        {
          int index = this.m_hashStyle[strParent];
          extendedFormat = (ExtendedFormatImpl) coll[index].Clone();
          extendedFormat.ParentIndex = index;
        }
        else if (strName == null)
        {
          extendedFormat = coll[0].CreateChildFormat(false);
        }
        else
        {
          extendedFormat = (ExtendedFormatImpl) coll[0].Clone();
          extendedFormat.ParentIndex = extendedFormat.Workbook.MaxXFCount;
        }
        extendedFormat.XFType = strName == null || strName == "Default" ? ExtendedFormatRecord.TXFType.XF_STYLE : ExtendedFormatRecord.TXFType.XF_CELL;
        return extendedFormat;
    }
  }

  private void AddXFToCollection(
    WorkbookImpl book,
    ExtendedFormatsCollection coll,
    ExtendedFormatImpl format,
    string strID,
    string strName)
  {
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (coll == null)
      throw new ArgumentNullException(nameof (coll));
    if (format.HasParent && format.XFType == ExtendedFormatRecord.TXFType.XF_CELL)
      format.ParentIndex = format.Workbook.MaxXFCount;
    if (strID != "Default")
    {
      format = coll.Add(format);
      this.m_hashStyle.Add(strID, format.XFormatIndex);
    }
    if (format.XFType != ExtendedFormatRecord.TXFType.XF_CELL)
      return;
    StylesCollection innerStyles = book.InnerStyles;
    if (innerStyles.ContainsName(strName))
    {
      StyleImpl styleImpl = (StyleImpl) innerStyles[strName];
      ExtendedFormatImpl twin = coll[styleImpl.Index];
      format.CopyTo(twin);
      ((ExtendedFormatWrapper) innerStyles[strName]).UpdateFont();
    }
    else
    {
      StyleRecord record = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
      record.ExtendedFormatIndex = (ushort) format.XFormatIndex;
      record.StyleName = strName;
      StyleImpl style = this.AppImplementation.CreateStyle(book, record);
      innerStyles.Add((IStyle) style);
    }
  }

  private Color GetColor(string strColor)
  {
    int num = strColor != null && strColor.Length != 0 ? strColor.IndexOf("#") : throw new ArgumentNullException(nameof (strColor));
    if (num == -1)
      return Color.FromName(strColor);
    strColor = strColor.Substring(num + 1);
    return ColorExtension.FromArgb(int.Parse(strColor, NumberStyles.HexNumber));
  }

  private IFont SetFontAllign(IFont font, string strAlling)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (strAlling == "Subscript")
      font.Subscript = true;
    if (strAlling == "Superscript")
      font.Superscript = true;
    return font;
  }

  private void SetCellRecord(
    WorkbookXmlSerializator.XmlSerializationCellType type,
    string strValue,
    CellRecordCollection cells,
    int iRow,
    int iColumn,
    int iXFIndex,
    TextWithFormat rtf)
  {
    switch (strValue)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (cells == null)
          throw new ArgumentNullException(nameof (cells));
        switch (type)
        {
          case WorkbookXmlSerializator.XmlSerializationCellType.Number:
            cells.SetNumberValue(iRow, iColumn, XmlConvertExtension.ToDouble(strValue), iXFIndex);
            return;
          case WorkbookXmlSerializator.XmlSerializationCellType.DateTime:
            double number = UtilityMethods.ConvertDateTimeToNumber(XmlConvertExtension.ToDateTime(strValue, XmlDateTimeSerializationMode.Unspecified));
            cells.SetNumberValue(iRow, iColumn, number, iXFIndex);
            return;
          case WorkbookXmlSerializator.XmlSerializationCellType.Boolean:
            cells.SetBooleanValue(iRow, iColumn, XmlConvertExtension.ToBoolean(strValue), iXFIndex);
            return;
          case WorkbookXmlSerializator.XmlSerializationCellType.Error:
            cells.SetErrorValue(iRow, iColumn, strValue, iXFIndex);
            return;
          default:
            cells.SetRTF(iRow, iColumn, iXFIndex, rtf);
            return;
        }
    }
  }

  private int GetXFIndex(WorksheetImpl sheet, string strValue)
  {
    if (strValue == null || strValue.Length == 0)
      throw new ArgumentNullException(nameof (strValue));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int xformatIndex = sheet.ParentWorkbook.InnerExtFormats[this.m_hashStyle[strValue]].CreateChildFormat().XFormatIndex;
    this.m_hashStyle[strValue] = xformatIndex;
    return xformatIndex;
  }

  private ExcelLineStyle GetLineStyle(string style, string weight)
  {
    if (style == null || style.Length == 0)
      throw new ArgumentNullException(nameof (style));
    if (weight == null || weight.Length == 0)
      throw new ArgumentNullException(nameof (weight));
    string empty = string.Empty;
    int result = 0;
    if ((int.TryParse(weight, out result) ? (result >= 0 ? 1 : 0) : 0) == 0 || Array.IndexOf<string>(WorkbookXmlSerializator.DEF_BORDER_LINE_TYPES, style) < 0)
      return ExcelLineStyle.None;
    string str1 = result != 0 ? (result <= 3 ? $"{weight} {style}" : "3 " + style) : "1 " + style;
    int num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_BORDER_LINE_TYPE_STRING, str1);
    for (int index = 3; num < 0 && index >= 1; --index)
    {
      string str2 = $"{index.ToString()} {style}";
      num = Array.IndexOf<string>(WorkbookXmlSerializator.DEF_BORDER_LINE_TYPE_STRING, str2);
    }
    return num <= 0 ? ExcelLineStyle.None : (ExcelLineStyle) num;
  }

  private void ReparseNames(WorkbookImpl book)
  {
    WorkbookNamesCollection workbookNamesCollection = book != null ? book.InnerNamesColection : throw new ArgumentNullException(nameof (book));
    int index = 0;
    for (int count = this.m_arrNames.Count; index < count; ++index)
    {
      NameRecord nameRecordByIndex = workbookNamesCollection.GetNameRecordByIndex(index);
      string strFormula = this.m_arrNames[index];
      if (strFormula[0] == '=')
        strFormula = strFormula.Substring(1);
      nameRecordByIndex.FormulaTokens = this.m_formulaUtil.ParseString(strFormula, (IWorksheet) null, (Dictionary<string, string>) null, 0, 0, true);
    }
  }

  private string ReadRTF(
    XmlReader reader,
    int iXFIndex,
    TextWithFormat rtf,
    out ushort xfIndex,
    bool isCommentRTF)
  {
    xfIndex = (ushort) 0;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Dictionary<FontRecord, FontImpl> dictionary = new Dictionary<FontRecord, FontImpl>();
    List<FontRecord> fontRecordList = new List<FontRecord>();
    FontsCollection innerFonts = this.m_parentBook.InnerFonts;
    int fontIndex = this.m_parentBook.InnerExtFormats[iXFIndex].FontIndex;
    FontImpl fontImpl1 = (FontImpl) innerFonts[fontIndex];
    FontImpl font1 = (FontImpl) null;
    StringBuilder stringBuilder = new StringBuilder();
    FontImpl fontImpl2 = fontImpl1.Clone((object) this.m_parentBook.InnerFonts);
    bool flag1 = false;
    if (reader.MoveToAttribute("xmlns") && reader.Value == "http://www.w3.org/TR/REC-html40")
      flag1 = true;
    reader.Read();
    bool isUpdated = false;
label_24:
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "Data")
    {
      isUpdated = false;
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (flag1)
          font1 = this.UpdateFont(reader, fontImpl2, reader.LocalName, false, ref isUpdated);
        else
          reader.Read();
      }
      if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.Whitespace)
      {
        if (font1 != null)
        {
          if (!fontRecordList.Contains(font1.Record))
          {
            fontRecordList.Add(font1.Record);
            innerFonts.ForceAdd(font1);
            dictionary.Add(font1.Record, font1);
            rtf.FormattingRuns.Add(stringBuilder.Length, font1.Index);
            stringBuilder.Append(reader.Value);
            rtf.DefaultFontIndex = font1.Index;
          }
          else
          {
            rtf.FormattingRuns.Add(stringBuilder.Length, dictionary[font1.Record].Index);
            stringBuilder.Append(reader.Value);
          }
          font1 = (FontImpl) null;
        }
        else
        {
          if (isCommentRTF)
          {
            FontImpl font2 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            if (!fontRecordList.Contains(font2.Record))
            {
              innerFonts.ForceAdd(font2);
              rtf.FormattingRuns.Add(stringBuilder.Length, font2.Index);
            }
            else
              rtf.FormattingRuns.Add(stringBuilder.Length, dictionary[font2.Record].Index);
          }
          else
            rtf.FormattingRuns.Add(stringBuilder.Length, (innerFonts[fontIndex] as FontImpl).Index);
          stringBuilder.Append(reader.Value);
        }
        reader.Read();
      }
      while (true)
      {
        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName != "Data")
          reader.Read();
        else
          goto label_24;
      }
    }
    bool flag2 = false;
    if (rtf.FormattingRuns.Count > 1)
    {
      flag2 = true;
      for (int index1 = 0; index1 < rtf.FormattingRuns.Count; ++index1)
      {
        int index2 = index1 + 1;
        if (index2 < rtf.FormattingRuns.Count && rtf.FormattingRuns.Values[index1] != rtf.FormattingRuns.Values[index2])
          flag2 = false;
        if (rtf.FormattingRuns.Values[index1] == fontIndex)
        {
          FontImpl font3 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
          font3.FontName = fontImpl2.FontName;
          font3.Size = fontImpl2.Size;
          if (!fontRecordList.Contains(font3.Record))
          {
            innerFonts.ForceAdd(font3);
            rtf.ReplaceFont(fontIndex, font3.Index);
          }
          else
            rtf.ReplaceFont(fontIndex, dictionary[font3.Record].Index);
          if (rtf.DefaultFontIndex == fontIndex)
            rtf.DefaultFontIndex = font3.Index;
        }
      }
    }
    if (!isCommentRTF && (rtf.FormattingRuns.Count == 1 && isUpdated || flag2))
    {
      ExtendedFormatImpl format = (ExtendedFormatImpl) this.m_parentBook.InnerExtFormats[iXFIndex].Clone();
      format.FontIndex = rtf.DefaultFontIndex;
      ExtendedFormatImpl extendedFormatImpl = this.m_parentBook.InnerExtFormats.Add(format);
      xfIndex = (ushort) extendedFormatImpl.Index;
      rtf.FormattingRuns.Clear();
    }
    string str = rtf.Text = stringBuilder.ToString();
    return stringBuilder.Length <= 0 ? (string) null : str;
  }

  private FontImpl UpdateFont(
    XmlReader reader,
    FontImpl fontImpl,
    string strNodeName,
    bool bEndElement,
    ref bool isUpdated)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    FontImpl fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl, false);
    string fontName = fontImpl1.FontName;
    double size = fontImpl1.Size;
    Color rgbColor = fontImpl1.RGBColor;
    while (reader.NodeType == XmlNodeType.Element && reader.NodeType != XmlNodeType.Text)
    {
      switch (reader.LocalName)
      {
        case "B":
          if (isUpdated)
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl1, false);
          }
          else
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            fontImpl1.FontName = fontName;
            fontImpl1.Size = size;
          }
          fontImpl1.Bold = !bEndElement;
          isUpdated = true;
          break;
        case "I":
          if (isUpdated)
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl1, false);
          }
          else
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            fontImpl1.FontName = fontName;
            fontImpl1.Size = size;
          }
          fontImpl1.Italic = !bEndElement;
          isUpdated = true;
          break;
        case "U":
          if (isUpdated)
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl1, false);
          }
          else
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            fontImpl1.FontName = fontName;
            fontImpl1.Size = size;
          }
          fontImpl1.Underline = reader.AttributeCount > 0 ? ExcelUnderline.Double : ExcelUnderline.Single;
          isUpdated = true;
          break;
        case "Sub":
          if (isUpdated)
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl1, false);
          }
          else
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            fontImpl1.FontName = fontName;
            fontImpl1.Size = size;
          }
          fontImpl1.Subscript = !bEndElement;
          isUpdated = true;
          break;
        case "Sup":
          if (isUpdated)
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl1, false);
          }
          else
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            fontImpl1.FontName = fontName;
            fontImpl1.Size = size;
          }
          fontImpl1.Superscript = !bEndElement;
          isUpdated = true;
          break;
        case "S":
          if (isUpdated)
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) fontImpl1, false);
          }
          else
          {
            fontImpl1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
            fontImpl1.FontName = fontName;
            fontImpl1.Size = size;
          }
          fontImpl1.Strikethrough = !bEndElement;
          isUpdated = true;
          break;
        case "Font":
          fontImpl1 = this.ReadRTFFont(reader, fontImpl1, ref isUpdated);
          break;
      }
      reader.Read();
    }
    return fontImpl1;
  }

  private FontImpl ReadRTFFont(XmlReader reader, FontImpl font, ref bool isUpdated)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string str = font != null ? font.FontName : throw new ArgumentNullException(nameof (font));
    double size = font.Size;
    Color color = font.RGBColor;
    FontImpl font1;
    if (isUpdated)
    {
      font1 = (FontImpl) this.m_parentBook.CreateFont((IFont) font, false);
    }
    else
    {
      font1 = (FontImpl) this.m_parentBook.CreateFont((IFont) null, false);
      font1.FontName = str;
      font1.Size = size;
    }
    bool flag = false;
    int i = 0;
    for (int attributeCount = reader.AttributeCount; i < attributeCount; ++i)
    {
      reader.MoveToAttribute(i);
      if (reader.LocalName == "Color")
      {
        color = this.GetColor(reader.Value);
        flag = true;
        isUpdated = true;
      }
      if (reader.LocalName == "Size")
      {
        font1.Size = XmlConvertExtension.ToDouble(reader.Value) >= font1.MIN_FONT_SIZE ? (XmlConvertExtension.ToDouble(reader.Value) <= font1.MAX_FONT_SIZE ? XmlConvertExtension.ToDouble(reader.Value) : font1.MAX_FONT_SIZE) : font1.MIN_FONT_SIZE;
        isUpdated = true;
      }
      if (reader.LocalName == "Face")
      {
        font1.FontName = reader.Value;
        isUpdated = true;
      }
    }
    reader.MoveToElement();
    if (flag)
      font1.RGBColor = color;
    font = font1;
    return font;
  }

  private void ParseFormula(
    WorksheetImpl sheet,
    int iRowIndex,
    int iCol,
    string strFormula,
    int iXFIndex,
    string cellValue,
    WorkbookXmlSerializator.XmlSerializationCellType cellType)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (strFormula == null || strFormula.Length == 0)
      throw new ArgumentNullException(nameof (strFormula));
    CellRecordCollection cellRecords = sheet.CellRecords;
    if (strFormula.IndexOf('!') != -1)
      this.m_hashFormula.Add(WorkbookXmlSerializator.GetUniqueID(sheet.Index, RangeImpl.GetCellIndex(iCol, iRowIndex)), new MSXmlReader.FormulaData(strFormula, cellValue, cellType, iXFIndex));
    else
      this.SetFormula(sheet, iRowIndex, iCol, strFormula, cellValue, cellType, iXFIndex);
  }

  private void SetFormula(
    WorksheetImpl sheet,
    int row,
    int column,
    string strFormula,
    string cellValue,
    WorkbookXmlSerializator.XmlSerializationCellType cellType,
    int iXFIndex)
  {
    CellRecordCollection cellRecords = sheet.CellRecords;
    FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
    if (strFormula.StartsWith("="))
      strFormula = UtilityMethods.RemoveFirstCharUnsafe(strFormula);
    record.ParsedExpression = this.m_formulaUtil.ParseString(strFormula, (IWorksheet) sheet, (Dictionary<string, string>) null, row - 1, column - 1, true);
    record.Row = row - 1;
    record.Column = column - 1;
    record.ExtendedFormatIndex = (ushort) iXFIndex;
    cellRecords.SetCellRecord(row, column, (ICellPositionFormat) record);
    this.SetFormulaValue(sheet, row, column, cellValue, cellType);
  }

  private void SetFormulaValue(
    WorksheetImpl sheet,
    int row,
    int column,
    string cellValue,
    WorkbookXmlSerializator.XmlSerializationCellType cellType)
  {
    if (cellValue == null)
      return;
    switch (cellType)
    {
      case WorkbookXmlSerializator.XmlSerializationCellType.Number:
        sheet.SetFormulaNumberValue(row, column, XmlConvertExtension.ToDouble(cellValue));
        break;
      case WorkbookXmlSerializator.XmlSerializationCellType.DateTime:
        DateTime dateTime = XmlConvertExtension.ToDateTime(cellValue, XmlDateTimeSerializationMode.Unspecified);
        sheet[row, column].FormulaDateTime = dateTime;
        break;
      case WorkbookXmlSerializator.XmlSerializationCellType.Boolean:
        sheet.SetFormulaBoolValue(row, column, XmlConvertExtension.ToBoolean(cellValue));
        break;
      case WorkbookXmlSerializator.XmlSerializationCellType.String:
        sheet.SetFormulaStringValue(row, column, cellValue);
        break;
      case WorkbookXmlSerializator.XmlSerializationCellType.Error:
        sheet.SetFormulaErrorValue(row, column, cellValue);
        break;
      default:
        throw new XmlException();
    }
  }

  private void ReparseFormula(WorkbookImpl book)
  {
    FormulaUtil formulaUtil = book != null ? book.FormulaUtil : throw new ArgumentNullException(nameof (book));
    WorksheetsCollection innerWorksheets = book.InnerWorksheets;
    formulaUtil.NumberFormat = NumberFormatInfo.InvariantInfo;
    foreach (KeyValuePair<long, MSXmlReader.FormulaData> keyValuePair in this.m_hashFormula)
    {
      long key = keyValuePair.Key;
      int sheetIndexByUniqueId = WorkbookXmlSerializator.GetSheetIndexByUniqueId(key);
      long cellIndexByUniqueId = WorkbookXmlSerializator.GetCellIndexByUniqueId(key);
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndexByUniqueId);
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndexByUniqueId);
      MSXmlReader.FormulaData formulaData = keyValuePair.Value;
      string strFormula = formulaData.Formula;
      if (strFormula[0] == '=')
        strFormula = UtilityMethods.RemoveFirstCharUnsafe(strFormula);
      this.SetFormula((WorksheetImpl) innerWorksheets[sheetIndexByUniqueId], rowFromCellIndex, columnFromCellIndex, strFormula, formulaData.Value, formulaData.ValueType, formulaData.XFIndex);
    }
    formulaUtil.NumberFormat = (NumberFormatInfo) null;
  }

  private class FormulaData
  {
    public string Formula;
    public string Value;
    public WorkbookXmlSerializator.XmlSerializationCellType ValueType;
    public int XFIndex;

    public FormulaData(
      string formula,
      string value,
      WorkbookXmlSerializator.XmlSerializationCellType type,
      int xfIndex)
    {
      this.Formula = formula;
      this.Value = value;
      this.ValueType = type;
      this.XFIndex = xfIndex;
    }
  }
}
