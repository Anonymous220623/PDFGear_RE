// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Xlsb.XlsbDataHolder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Xlsb;

internal class XlsbDataHolder
{
  private const string ContentTypesItemName = "[Content_Types].xml";
  internal const string RelationsDirectory = "_rels";
  internal const string RelationExtension = ".rels";
  private const string TopRelationsPath = "_rels/.rels";
  private const string XmlExtension = "xml";
  private const string RelsExtension = "rels";
  internal const string BinaryExtension = "bin";
  private const string WorkbookPartName = "xl/workbook.bin";
  private const string CustomXmlPartName = "customXml/item{0}.xml";
  private const string SSTPartName = "/xl/sharedStrings.bin";
  private const string StylesPartName = "xl/styles.bin";
  private const string ThemesPartName = "xl/theme/theme1.xml";
  private const string DefaultWorksheetPathFormat = "xl/worksheets/sheet{0}.bin";
  internal const string ExtendedPropertiesPartName = "docProps/app.xml";
  internal const string CorePropertiesPartName = "docProps/core.xml";
  internal const string CustomPropertiesPartName = "docProps/custom.xml";
  private const string RelationIdFormat = "rId{0}";
  private readonly int[] DEF_NUMBERFORMAT_INDEXES = new int[8]
  {
    5,
    6,
    7,
    8,
    41,
    42,
    43,
    44
  };
  private WorkbookImpl m_book;
  private RelationCollection m_topRelations;
  private RelationCollection m_workbookRelations;
  private string m_strStylesRelationId;
  private string m_strSSTRelationId;
  private ZipArchive m_Archive = new ZipArchive();
  private IDictionary<string, string> m_dicDefaultTypes = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) System.StringComparer.OrdinalIgnoreCase);
  private IDictionary<string, string> m_dicOverriddenTypes = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) System.StringComparer.InvariantCultureIgnoreCase);
  private Dictionary<string, object> m_dictItemsToRemove = new Dictionary<string, object>();
  private Excel2007Parser m_parser;
  private string m_strSSTPartName;
  private string m_strStylesPartName;
  private List<int> m_arrCellFormats;

  internal Dictionary<string, object> ItemsToRemove => this.m_dictItemsToRemove;

  internal List<int> XFIndexes => this.m_arrCellFormats;

  internal Excel2007Parser Parser
  {
    get
    {
      if (this.m_parser == null)
        this.m_parser = new Excel2007Parser(this.m_book);
      return this.m_parser;
    }
  }

  internal WorkbookImpl Workbook => this.m_book;

  internal XlsbDataHolder()
  {
  }

  internal XlsbDataHolder(WorkbookImpl workbookImpl, ZipArchive archive)
  {
    this.m_book = workbookImpl;
    this.m_Archive = archive;
  }

  internal void ParseDocument(ref List<Color> themeColors, Stream stream)
  {
    this.m_book.Loading = true;
    bool throwOnUnknownNames = this.m_book.ThrowOnUnknownNames;
    this.m_book.ThrowOnUnknownNames = false;
    this.ParseContentType();
    this.m_topRelations = this.ParseRelations("_rels/.rels");
    this.m_dictItemsToRemove.Add("_rels/.rels", (object) null);
    this.ParseDocumentProperties();
    this.ParseWorkbook(themeColors);
    foreach (ZipArchiveItem zipArchiveItem in this.m_Archive.Items)
    {
      if (zipArchiveItem.ItemName.Contains("xl/tables/table"))
        this.m_Archive.RemoveItem(zipArchiveItem.ItemName);
      else if (zipArchiveItem.ItemName.Contains("xl/drawings/drawing") && !this.m_book.DataHolder.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
        this.m_Archive.RemoveItem(zipArchiveItem.ItemName);
      else if (zipArchiveItem.ItemName.Contains("xl/drawings/_rels/drawing") && !this.m_book.DataHolder.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
        this.m_Archive.RemoveItem(zipArchiveItem.ItemName);
      else if (zipArchiveItem.ItemName.Contains("xl/charts/chart") && !this.m_book.DataHolder.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
        this.m_Archive.RemoveItem(zipArchiveItem.ItemName);
      else if (zipArchiveItem.ItemName.Contains("xl/charts/_rels/chart") && !this.m_book.DataHolder.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
        this.m_Archive.RemoveItem(zipArchiveItem.ItemName);
      else if (zipArchiveItem.ItemName.Contains("xl/calcChain.bin"))
        this.m_Archive.RemoveItem(zipArchiveItem.ItemName);
    }
    foreach (string key in this.m_dictItemsToRemove.Keys)
      this.m_Archive.RemoveItem(key);
    this.m_dictItemsToRemove.Clear();
    this.m_book.ThrowOnUnknownNames = throwOnUnknownNames;
    this.m_book.Loading = false;
  }

  internal void ParseSheet(
    Stream stream,
    WorksheetImpl sheet,
    string strParentPath,
    List<int> xFIndexes,
    Dictionary<string, object> itemsToRemove,
    Dictionary<int, int> dictUpdateSSTIndexes)
  {
    if (ZipArchive.ReadInt16(stream) != (short) 385)
      return;
    ++stream.Position;
    XlsbRecords xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(stream);
    bool flag = true;
    for (; xlsbRecords != XlsbRecords.EndWorksheet; xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(stream))
    {
      switch (xlsbRecords)
      {
        case XlsbRecords.SheetViewsBegin:
          ++stream.Position;
          this.ParseSheetViews(sheet, stream);
          break;
        case XlsbRecords.BeginSheetData:
          this.ParseSheetData(stream, sheet, xFIndexes);
          flag = false;
          break;
        case XlsbRecords.SheetPr:
          int num = stream.ReadByte();
          stream.Position += (long) num;
          break;
        case XlsbRecords.SheetDimension:
          ++stream.Position;
          sheet.FirstRow = ZipArchive.ReadInt32(stream) + 1;
          sheet.LastRow = ZipArchive.ReadInt32(stream) + 1;
          sheet.FirstColumn = ZipArchive.ReadInt32(stream) + 1;
          sheet.LastColumn = ZipArchive.ReadInt32(stream) + 1;
          break;
        case XlsbRecords.ColumnsBegin:
          this.ParseColumns(stream, sheet, xFIndexes);
          break;
        case XlsbRecords.BeginSheetFormat:
          this.ParseSheetFormat(stream, sheet);
          break;
      }
      if (!flag)
        break;
    }
  }

  private void ParseSheetViews(WorksheetImpl sheet, Stream stream)
  {
    XlsbRecords xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(stream);
    for (; xlsbRecords != XlsbRecords.SheetViewEnd; xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(stream))
    {
      switch (xlsbRecords)
      {
        case XlsbRecords.SheetViewBegin:
          int num1 = stream.ReadByte();
          stream.Position += (long) num1;
          break;
        case XlsbRecords.SheetSelect:
          int num2 = stream.ReadByte();
          stream.Position += (long) num2;
          break;
      }
    }
    ++stream.Position;
    if (ZipArchive.ReadInt16(stream) != (short) 390)
      return;
    stream.Position += 16L /*0x10*/;
  }

  private void ParseSheetFormat(Stream stream, WorksheetImpl sheet)
  {
    ++stream.Position;
    stream.Position += 4L;
    sheet.DefaultColumnWidth = (double) ZipArchive.ReadInt16(stream);
    sheet.DefaultRowHeight = (int) ZipArchive.ReadInt16(stream);
    stream.Position += 4L;
  }

  private void ParseSheetData(Stream stream, WorksheetImpl sheet, List<int> xFIndexes)
  {
    ++stream.Position;
    if (ZipArchive.ReadInt16(stream) == (short) 1573)
      stream.Position += 13L;
    else
      stream.Position -= 2L;
    XlsbRecords xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(stream);
    while (true)
    {
      switch (xlsbRecords)
      {
        case XlsbRecords.EndSheetData:
          goto label_6;
        case XlsbRecords.Row:
          this.ParseRows(stream, sheet, xFIndexes);
          xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(stream);
          continue;
        default:
          goto label_7;
      }
    }
label_7:
    return;
label_6:
    ++stream.Position;
  }

  private void ParseRows(Stream stream, WorksheetImpl sheet, List<int> arrStyles)
  {
    int rowIndex = ZipArchive.ReadInt32(stream);
    RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) sheet, rowIndex, true);
    int num1 = ZipArchive.ReadInt32(stream);
    if (arrStyles.Count > num1)
      num1 = arrStyles[num1];
    this.m_book.AddUsedStyleIndex(num1);
    row.ExtendedFormatIndex = (ushort) num1;
    row.Height = (ushort) ZipArchive.ReadInt16(stream);
    ++stream.Position;
    byte btValue = (byte) stream.ReadByte();
    row.IsHidden = XlsbDataHolder.ReadBit(btValue, 4);
    row.IsBadFontHeight = XlsbDataHolder.ReadBit(btValue, 5);
    row.IsFormatted = XlsbDataHolder.ReadBit(btValue, 6);
    stream.Position += 5L;
    row.FirstColumn = ZipArchive.ReadInt32(stream);
    row.LastColumn = ZipArchive.ReadInt32(stream);
    this.ParseCells(stream, sheet, rowIndex, arrStyles);
    int num2 = (int) ZipArchive.ReadInt16(stream);
    while (stream.Position < stream.Length - 2L)
    {
      switch (num2)
      {
        case 402:
          stream.Position -= 2L;
          goto label_7;
        case 6400:
          goto label_7;
        default:
          num2 = (int) ZipArchive.ReadInt16(stream);
          continue;
      }
    }
label_7:
    if (num2 != 6400)
      return;
    stream.Position -= 2L;
  }

  private void ParseCells(Stream stream, WorksheetImpl sheet, int rowIndex, List<int> xFIndexes)
  {
    switch ((XlsbRecords) ZipArchive.ReadInt16(stream))
    {
      case XlsbRecords.EndSheetData:
      case XlsbRecords.Row:
        stream.Position -= 2L;
        break;
      case (XlsbRecords) 1573:
        stream.Position += 13L;
        break;
      default:
        stream.Position -= 2L;
        XlsbRecords xlsbRecords1 = (XlsbRecords) stream.ReadByte();
        int row = rowIndex;
        int defaultXfIndex = this.m_book.DefaultXFIndex;
        CellRecordCollection cellRecords = sheet.CellRecords;
        while (true)
        {
          switch (xlsbRecords1)
          {
            case XlsbRecords.Blank:
              ++stream.Position;
              int num1 = ZipArchive.ReadInt32(stream);
              int index1 = ZipArchive.ReadInt32(stream);
              int xFindex1 = xFIndexes[index1];
              this.m_book.AddUsedStyleIndex(xFindex1);
              cellRecords.SetBlank(row + 1, num1 + 1, xFindex1);
              break;
            case XlsbRecords.Number:
              ++stream.Position;
              int num2 = ZipArchive.ReadInt32(stream);
              int index2 = ZipArchive.ReadInt32(stream);
              int xFindex2 = xFIndexes[index2];
              this.m_book.AddUsedStyleIndex(xFindex2);
              long num3 = (long) ZipArchive.ReadInt32(stream);
              bool flag1 = (num3 & 2L) == 2L;
              bool flag2 = (num3 & 1L) == 1L;
              long num4 = num3 >> 2;
              double dValue1;
              if (flag1)
              {
                double num5 = (double) num4;
                dValue1 = flag2 ? num5 / 100.0 : num5;
              }
              else
              {
                double num6 = BitConverterGeneral.Int64BitsToDouble(num4 << 34);
                dValue1 = flag2 ? num6 / 100.0 : num6;
              }
              cellRecords.SetNumberValue(row + 1, num2 + 1, dValue1, xFindex2);
              break;
            case XlsbRecords.Error:
              ++stream.Position;
              int num7 = ZipArchive.ReadInt32(stream);
              int index3 = ZipArchive.ReadInt32(stream);
              int xFindex3 = xFIndexes[index3];
              this.m_book.AddUsedStyleIndex(xFindex3);
              string strValue1 = FormulaUtil.ErrorCodeToName[stream.ReadByte()];
              cellRecords.SetErrorValue(row + 1, num7 + 1, strValue1, xFindex3);
              break;
            case XlsbRecords.Boolean:
              ++stream.Position;
              int num8 = ZipArchive.ReadInt32(stream);
              int index4 = ZipArchive.ReadInt32(stream);
              int xFindex4 = xFIndexes[index4];
              this.m_book.AddUsedStyleIndex(xFindex4);
              bool bValue = stream.ReadByte() == 1;
              cellRecords.SetBooleanValue(row + 1, num8 + 1, bValue, xFindex4);
              break;
            case XlsbRecords.Double:
              ++stream.Position;
              int num9 = ZipArchive.ReadInt32(stream);
              int index5 = ZipArchive.ReadInt32(stream);
              int xFindex5 = xFIndexes[index5];
              this.m_book.AddUsedStyleIndex(xFindex5);
              byte[] buffer1 = new byte[8];
              stream.Read(buffer1, 0, 8);
              double dValue2 = BitConverterGeneral.Int64BitsToDouble(BitConverter.ToInt64(buffer1, 0));
              cellRecords.SetNumberValue(row + 1, num9 + 1, dValue2, xFindex5);
              break;
            case XlsbRecords.LabelText:
              ++stream.Position;
              int column = ZipArchive.ReadInt32(stream);
              int index6 = ZipArchive.ReadInt32(stream);
              int xFindex6 = xFIndexes[index6];
              this.m_book.AddUsedStyleIndex(xFindex6);
              byte[] numArray1 = new byte[ZipArchive.ReadInt32(stream) * 2];
              stream.Read(numArray1, 0, numArray1.Length);
              string strValue2 = Encoding.Unicode.GetString(numArray1, 0, numArray1.Length);
              cellRecords.SetNonSSTString(row, column, xFindex6, strValue2);
              break;
            case XlsbRecords.SSTItem:
              ++stream.Position;
              int num10 = ZipArchive.ReadInt32(stream);
              int index7 = ZipArchive.ReadInt32(stream);
              int xFindex7 = xFIndexes[index7];
              this.m_book.AddUsedStyleIndex(xFindex7);
              int iSSTIndex = ZipArchive.ReadInt32(stream);
              cellRecords.SetSingleStringValue(row + 1, num10 + 1, xFindex7, iSSTIndex);
              break;
            case XlsbRecords.FormulaStr:
              ++stream.Position;
              int num11 = ZipArchive.ReadInt32(stream);
              int index8 = ZipArchive.ReadInt32(stream);
              int xFindex8 = xFIndexes[index8];
              this.m_book.AddUsedStyleIndex(xFindex8);
              int num12 = ZipArchive.ReadInt32(stream);
              byte[] numArray2 = new byte[num12 * 2];
              stream.Read(numArray2, 0, num12 * 2);
              string str1 = Encoding.Unicode.GetString(numArray2, 0, num12 * 2);
              stream.Position += 2L;
              int count1 = ZipArchive.ReadInt32(stream);
              byte[] numArray3 = new byte[count1];
              stream.Read(numArray3, 0, count1);
              int num13 = ZipArchive.ReadInt32(stream);
              stream.Position += (long) num13;
              FormulaRecord record1 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
              record1.Row = row;
              record1.Column = num11;
              record1.ExtendedFormatIndex = (ushort) xFindex8;
              record1.CalculateOnOpen = true;
              record1.Expression = numArray3;
              ByteArrayDataProvider provider1 = new ByteArrayDataProvider(numArray3);
              record1.ParsedExpression = FormulaUtil.ParseExpression((DataProvider) provider1, numArray3.Length, ExcelVersion.Excel2007);
              cellRecords.SetCellRecord(row + 1, num11 + 1, (ICellPositionFormat) record1);
              sheet.SetFormulaStringValue(row + 1, num11 + 1, str1);
              break;
            case XlsbRecords.FormulaNum:
              ++stream.Position;
              int num14 = ZipArchive.ReadInt32(stream);
              int index9 = ZipArchive.ReadInt32(stream);
              int xFindex9 = xFIndexes[index9];
              this.m_book.AddUsedStyleIndex(xFindex9);
              byte[] buffer2 = new byte[8];
              stream.Read(buffer2, 0, 8);
              double num15 = BitConverterGeneral.Int64BitsToDouble(BitConverter.ToInt64(buffer2, 0));
              stream.Position += 2L;
              int count2 = ZipArchive.ReadInt32(stream);
              byte[] numArray4 = new byte[count2];
              stream.Read(numArray4, 0, count2);
              int num16 = ZipArchive.ReadInt32(stream);
              stream.Position += (long) num16;
              FormulaRecord record2 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
              record2.Row = row;
              record2.Column = num14;
              record2.ExtendedFormatIndex = (ushort) xFindex9;
              record2.Expression = numArray4;
              ByteArrayDataProvider provider2 = new ByteArrayDataProvider(numArray4);
              record2.ParsedExpression = FormulaUtil.ParseExpression((DataProvider) provider2, numArray4.Length, ExcelVersion.Excel2007);
              cellRecords.SetCellRecord(row + 1, num14 + 1, (ICellPositionFormat) record2);
              sheet.SetFormulaNumberValue(row + 1, num14 + 1, num15);
              break;
            case XlsbRecords.FormulaBool:
              ++stream.Position;
              int num17 = ZipArchive.ReadInt32(stream);
              int index10 = ZipArchive.ReadInt32(stream);
              int xFindex10 = xFIndexes[index10];
              this.m_book.AddUsedStyleIndex(xFindex10);
              int num18 = stream.ReadByte();
              stream.Position += 2L;
              int count3 = ZipArchive.ReadInt32(stream);
              byte[] numArray5 = new byte[count3];
              stream.Read(numArray5, 0, count3);
              int num19 = ZipArchive.ReadInt32(stream);
              stream.Position += (long) num19;
              FormulaRecord record3 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
              record3.Row = row;
              record3.Column = num17;
              record3.ExtendedFormatIndex = (ushort) xFindex10;
              record3.CalculateOnOpen = true;
              record3.Expression = numArray5;
              ByteArrayDataProvider provider3 = new ByteArrayDataProvider(numArray5);
              record3.ParsedExpression = FormulaUtil.ParseExpression((DataProvider) provider3, numArray5.Length, ExcelVersion.Excel2007);
              cellRecords.SetCellRecord(row + 1, num17 + 1, (ICellPositionFormat) record3);
              sheet.SetFormulaBoolValue(row + 1, num17 + 1, num18 == 1);
              break;
            case XlsbRecords.FormulaErr:
              ++stream.Position;
              int num20 = ZipArchive.ReadInt32(stream);
              int index11 = ZipArchive.ReadInt32(stream);
              int xFindex11 = xFIndexes[index11];
              this.m_book.AddUsedStyleIndex(xFindex11);
              byte key = (byte) stream.ReadByte();
              stream.Position += 2L;
              string str2 = FormulaUtil.ErrorCodeToName[(int) key];
              int count4 = ZipArchive.ReadInt32(stream);
              byte[] numArray6 = new byte[count4];
              stream.Read(numArray6, 0, count4);
              int num21 = ZipArchive.ReadInt32(stream);
              stream.Position += (long) num21;
              FormulaRecord record4 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
              record4.Row = row;
              record4.Column = num20;
              record4.ExtendedFormatIndex = (ushort) xFindex11;
              record4.CalculateOnOpen = true;
              record4.Expression = numArray6;
              ByteArrayDataProvider provider4 = new ByteArrayDataProvider(numArray6);
              record4.ParsedExpression = FormulaUtil.ParseExpression((DataProvider) provider4, numArray6.Length, ExcelVersion.Excel2007);
              cellRecords.SetCellRecord(row + 1, num20 + 1, (ICellPositionFormat) record4);
              sheet.SetFormulaErrorValue(row + 1, num20 + 1, str2);
              break;
            case (XlsbRecords) 146:
              goto label_4;
          }
          XlsbRecords xlsbRecords2 = (XlsbRecords) ZipArchive.ReadInt16(stream);
          if (xlsbRecords2 == (XlsbRecords) 939)
          {
            int num22 = stream.ReadByte();
            stream.Position += (long) num22;
            xlsbRecords2 = (XlsbRecords) ZipArchive.ReadInt16(stream);
          }
          if (xlsbRecords2 != (XlsbRecords) 1573)
          {
            if (xlsbRecords2 != XlsbRecords.Row)
            {
              if (xlsbRecords2 != XlsbRecords.EndSheetData)
              {
                stream.Position -= 2L;
                xlsbRecords1 = (XlsbRecords) stream.ReadByte();
              }
              else
                goto label_26;
            }
            else
              goto label_24;
          }
          else
            break;
        }
        stream.Position += 13L;
        break;
label_24:
        stream.Position -= 2L;
        break;
label_26:
        stream.Position -= 2L;
        break;
label_4:
        break;
    }
  }

  private void ParseColumns(Stream stream, WorksheetImpl sheet, List<int> xFIndexes)
  {
    ++stream.Position;
    while (ZipArchive.ReadInt16(stream) == (short) 4668)
      this.ParseColumn(stream, sheet, xFIndexes);
    stream.Position -= 2L;
    if (ZipArchive.ReadInt16(stream) != (short) 903)
      return;
    ++stream.Position;
  }

  private void ParseColumn(Stream stream, WorksheetImpl worksheet, List<int> xFIndexes)
  {
    ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
    record.FirstColumn = (ushort) ZipArchive.ReadInt32(stream);
    record.LastColumn = (ushort) ZipArchive.ReadInt32(stream);
    int num = (int) Math.Round((double) (ushort) ZipArchive.ReadInt32(stream));
    record.ColumnWidth = (ushort) num;
    int index = ZipArchive.ReadInt32(stream);
    int xFindex = xFIndexes[index];
    record.ExtendedFormatIndex = (ushort) xFindex;
    this.m_book.UpdateUsedStyleIndex(xFindex, this.m_book.MaxRowCount);
    if ((int) record.LastColumn != worksheet.Workbook.MaxColumnCount - 1)
    {
      if (worksheet.FirstColumn == int.MaxValue || worksheet.FirstColumn > (int) record.FirstColumn + 1)
        worksheet.FirstColumn = (int) record.FirstColumn + 1;
      if (worksheet.LastColumn == int.MaxValue || worksheet.LastColumn < (int) record.LastColumn + 1)
        worksheet.LastColumn = (int) record.LastColumn + 1;
    }
    stream.Position += 2L;
    worksheet.ParseColumnInfo(record, false);
  }

  private void ParseWorkbook(List<Color> themeColors)
  {
    string itemName = "xl/workbook.bin";
    if (itemName == null || itemName.Length == 0)
      throw new ArgumentOutOfRangeException("workbookItemName");
    if (itemName[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe(itemName);
    ZipArchiveItem zipArchiveItem1 = this.m_Archive[itemName];
    if (zipArchiveItem1 == null)
      throw new XmlException("Cannot locate workbook item: " + itemName);
    string correspondingRelations = FileDataHolder.GetCorrespondingRelations(itemName);
    this.m_workbookRelations = this.ParseRelations(correspondingRelations);
    this.m_dictItemsToRemove.Add(correspondingRelations, (object) null);
    Relation relationByContentType1 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", out this.m_strStylesRelationId);
    if (relationByContentType1 == null)
    {
      this.Workbook.InsertDefaultFonts();
      this.Workbook.InsertDefaultValues();
    }
    Relation relationByContentType2 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", out this.m_strSSTRelationId);
    MemoryStream memoryStream = new MemoryStream();
    string path;
    FileDataHolder.SeparateItemName(itemName, out path);
    Stream stream = (Stream) null;
    if (zipArchiveItem1 != null)
    {
      stream = zipArchiveItem1.DataStream;
      stream.Position = 0L;
    }
    this.ParseWorkbookPart(stream, this.m_workbookRelations, this, path);
    this.m_dictItemsToRemove.Add(zipArchiveItem1.ItemName, (object) null);
    string relationId = "";
    Relation relationByContentType3 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", out relationId);
    if (relationByContentType3 != null)
    {
      ZipArchiveItem zipArchiveItem2 = this.GetItem(relationByContentType3, path, out string _);
      if (zipArchiveItem2 != null)
      {
        themeColors = this.Parser.ParseThemes(FileDataHolder.CreateReader(zipArchiveItem2));
        if (themeColors != null)
          this.Workbook.m_isThemeColorsParsed = true;
      }
      else
        this.m_workbookRelations.Remove("xl/theme/theme1.xml");
    }
    int fullSize = this.m_book.TabSheets.Count + 4;
    int curPos1 = 1;
    ITabSheets objects = (ITabSheets) this.m_book.Objects;
    ApplicationImpl appImplementation = this.m_book.AppImplementation;
    appImplementation.RaiseProgressEvent((long) curPos1, (long) (objects.Count + 4));
    if (relationByContentType1 != null)
    {
      this.m_strStylesPartName = path + relationByContentType1.Target;
      this.m_arrCellFormats = this.ParseStyles(this.m_Archive[this.m_strStylesPartName].DataStream);
      this.m_dictItemsToRemove.Add(this.m_strStylesPartName, (object) null);
    }
    int curPos2 = curPos1 + 1;
    appImplementation.RaiseProgressEvent((long) curPos2, (long) fullSize);
    Dictionary<int, int> dictUpdatedSSTIndexes = (Dictionary<int, int>) null;
    if (relationByContentType2 != null)
    {
      this.m_strSSTPartName = relationByContentType2.Target[0] != '/' ? path + relationByContentType2.Target : relationByContentType2.Target;
      ZipArchiveItem zipArchiveItem3 = this.GetItem(relationByContentType2, path, out string _);
      if (zipArchiveItem3 != null)
      {
        dictUpdatedSSTIndexes = this.ParseSST(zipArchiveItem3.DataStream, false);
        this.m_dictItemsToRemove.Add(this.m_strSSTPartName, (object) null);
      }
    }
    int curPos3 = curPos2 + 1;
    appImplementation.RaiseProgressEvent((long) curPos3, (long) fullSize);
    this.ParseWorksheets(dictUpdatedSSTIndexes);
  }

  private void ParseWorksheets(Dictionary<int, int> dictUpdatedSSTIndexes)
  {
    ITabSheets objects = (ITabSheets) this.m_book.Objects;
    this.m_book.AppImplementation.IsFormulaParsed = false;
    int index = 0;
    for (int count = objects.Count; index < count; ++index)
    {
      if (objects[index] != null)
      {
        WorksheetBaseImpl worksheetBaseImpl = (WorksheetBaseImpl) objects[index];
        worksheetBaseImpl.ParseBinaryData(dictUpdatedSSTIndexes, this);
        worksheetBaseImpl.IsSaved = false;
      }
    }
  }

  internal Dictionary<int, int> ParseSST(Stream stream, bool parseOnDemand)
  {
    this.m_book.SSTStream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
    if (parseOnDemand)
      this.m_book.ParseOnDemand = parseOnDemand;
    this.m_book.SSTStream.Position = 0L;
    Dictionary<int, int> sst = new Dictionary<int, int>();
    SSTDictionary innerSst = this.m_book.InnerSST;
    stream.Position += 11L;
    int key = 0;
    for (XlsbRecords xlsbRecords = (XlsbRecords) stream.ReadByte(); xlsbRecords == XlsbRecords.StrRecord; xlsbRecords = (XlsbRecords) stream.ReadByte())
    {
      int stringItem = this.ParseStringItem(stream);
      if (key != stringItem)
        sst[key] = stringItem;
      ++key;
    }
    innerSst.UpdateRefCounts();
    return sst;
  }

  internal int ParseStringItem(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    stream.Position += 2L;
    SSTDictionary innerSst = this.m_book.InnerSST;
    int num = ZipArchive.ReadInt32(stream);
    byte[] numArray = new byte[num * 2];
    stream.Read(numArray, 0, num * 2);
    string key = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    return innerSst.AddIncrease((object) key, false);
  }

  internal ZipArchiveItem GetItem(Relation relation, string parentItemPath, out string strItemPath)
  {
    string str = relation != null ? relation.Target : throw new ArgumentNullException(nameof (relation));
    if (parentItemPath != null)
    {
      str = FileDataHolder.CombinePath(parentItemPath, str);
      str.Replace('\\', '/');
    }
    ZipArchiveItem zipArchiveItem = this.m_Archive[str];
    strItemPath = str;
    return zipArchiveItem;
  }

  private List<int> ParseStyles(Stream styleStream)
  {
    if (styleStream == null)
      throw new ArgumentNullException(nameof (styleStream));
    List<int> intList1 = (List<int>) null;
    List<BordersCollection> arrBorders = (List<BordersCollection>) null;
    List<FillImpl> arrFills = (List<FillImpl>) null;
    List<int> intList2 = (List<int>) null;
    List<int> styles = (List<int>) null;
    Dictionary<int, int> arrNumberFormatIndexes = (Dictionary<int, int>) null;
    XlsbRecords xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(styleStream);
    styleStream.ReadByte();
    if (xlsbRecords != XlsbRecords.EndStyleSheet)
    {
      for (bool flag = true; xlsbRecords != XlsbRecords.EndStyleSheet && flag; xlsbRecords = (XlsbRecords) ZipArchive.ReadInt16(styleStream))
      {
        switch (xlsbRecords)
        {
          case XlsbRecords.BeginFillCol:
            arrFills = this.ParseFills(styleStream);
            break;
          case XlsbRecords.BeginFontsCol:
            intList1 = this.ParseFonts(styleStream);
            break;
          case XlsbRecords.BeginBorderCol:
            arrBorders = this.ParseBorders(styleStream);
            break;
          case XlsbRecords.BeginNumFmtCol:
            arrNumberFormatIndexes = this.ParseNumberFormats(styleStream);
            break;
          case XlsbRecords.BeginCellXfs:
            styles = this.ParseCellFormats(styleStream, intList1, arrFills, arrBorders, intList2, arrNumberFormatIndexes);
            break;
          case XlsbRecords.BeginCellStyles:
            this.ParseStyles(styleStream, intList2);
            flag = false;
            break;
          case XlsbRecords.BeginCellStyleXfs:
            intList2 = this.ParseNamedStyles(styleStream, intList1, arrFills, arrBorders, arrNumberFormatIndexes);
            break;
        }
      }
    }
    this.m_book.ArrNewNumberFormatIndexes = arrNumberFormatIndexes;
    if (this.m_book.InnerStyles.Count == 0)
      this.m_book.PrepareStyles(false, new List<StyleRecord>(), (Dictionary<int, int>) null);
    ExtendedFormatWrapper innerStyle = (ExtendedFormatWrapper) this.m_book.InnerStyles["Normal"];
    for (int index = 0; index < styles.Count; ++index)
    {
      if (this.m_book.InnerExtFormats[styles[index]].ParentIndex == innerStyle.XFormatIndex)
      {
        this.m_book.DefaultXFIndex = this.m_book.InnerExtFormats[styles[index]].Index;
        break;
      }
    }
    return styles;
  }

  private List<int> ParseStyles(Stream styleStream, List<int> arrNamedStyleIndexes)
  {
    if (styleStream == null)
      throw new ArgumentNullException("reader");
    if (arrNamedStyleIndexes == null)
    {
      this.m_book.InsertDefaultFonts();
      this.m_book.InsertDefaultValues();
    }
    this.m_book.InnerStyles.Clear();
    List<int> validate = new List<int>();
    ++styleStream.Position;
    int num = ZipArchive.ReadInt32(styleStream);
    if (ZipArchive.ReadInt16(styleStream) == (short) 1573)
      styleStream.Position += 27L;
    else
      styleStream.Position -= 2L;
    while (num > 0)
    {
      if (styleStream.ReadByte() == 48 /*0x30*/)
      {
        this.ParseStyle(styleStream, arrNamedStyleIndexes, ref validate);
        --num;
      }
    }
    if (ZipArchive.ReadInt16(styleStream) == (short) 1260)
      ++styleStream.Position;
    return arrNamedStyleIndexes;
  }

  private void ParseStyle(Stream stream, List<int> arrNamedStyleIndexes, ref List<int> validate)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (arrNamedStyleIndexes == null)
      throw new ArgumentNullException(nameof (arrNamedStyleIndexes));
    StyleRecord record = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    ++stream.Position;
    int index = ZipArchive.ReadInt32(stream);
    record.ExtendedFormatIndex = (ushort) arrNamedStyleIndexes[index];
    record.DefXFIndex = this.m_book.Version != ExcelVersion.Excel97to2003 ? ushort.MaxValue : (ushort) 0;
    stream.Position += 2L;
    int num1 = (int) (byte) stream.ReadByte();
    record.BuildInOrNameLen = (byte) num1;
    record.OutlineStyleLevel = (byte) stream.ReadByte();
    int num2 = ZipArchive.ReadInt32(stream);
    byte[] numArray = new byte[num2 * 2];
    stream.Read(numArray, 0, num2 * 2);
    string str1 = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    if (str1.Length > (int) byte.MaxValue)
    {
      record.IsAsciiConverted = true;
      string str2 = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
      if (str2.Length > (int) byte.MaxValue)
      {
        string str3 = str2.Substring(0, (int) byte.MaxValue);
        record.StyleName = str3;
      }
      else
        record.StyleName = str2;
      record.StyleNameCache = str1;
    }
    else
      record.StyleName = str1;
    if (validate.Contains((int) record.ExtendedFormatIndex) && !record.IsBuildInStyle)
      return;
    validate.Add((int) record.ExtendedFormatIndex);
    this.m_book.InnerStyles.Add(record);
  }

  private List<int> ParseCellFormats(
    Stream stream,
    List<int> arrNewFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders,
    List<int> namedStyleIndexes,
    Dictionary<int, int> arrNumberFormatIndexes)
  {
    if (stream == null)
      throw new ArgumentNullException("reader");
    if (namedStyleIndexes == null)
    {
      this.m_book.InsertDefaultFonts();
      this.m_book.InsertDefaultValues();
    }
    if (arrNewFontIndexes == null)
      throw new ArgumentNullException(nameof (arrNewFontIndexes));
    if (arrFills == null)
      throw new ArgumentNullException(nameof (arrFills));
    if (arrBorders == null)
      throw new ArgumentNullException(nameof (arrBorders));
    List<int> cellFormats = new List<int>();
    ++stream.Position;
    for (int index = ZipArchive.ReadInt32(stream); index > 0; --index)
    {
      if (stream.ReadByte() == 47)
      {
        ExtendedFormatImpl extendedFormat = this.ParseExtendedFormat(stream, arrNewFontIndexes, arrFills, arrBorders, namedStyleIndexes, new bool?(false), arrNumberFormatIndexes, false);
        ExtendedFormatImpl extendedFormatImpl = namedStyleIndexes == null ? this.m_book.InnerExtFormats.Add(extendedFormat) : this.m_book.InnerExtFormats.Add(extendedFormat, namedStyleIndexes.Count);
        cellFormats.Add(extendedFormatImpl.Index);
      }
    }
    if (!arrBorders[0].IsEmptyBorder)
      arrBorders.RemoveAt(0);
    if (ZipArchive.ReadInt16(stream) == (short) 1258)
      ++stream.Position;
    return cellFormats;
  }

  private List<int> ParseNamedStyles(
    Stream stream,
    List<int> arrFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders,
    Dictionary<int, int> arrNumberFormatIndexes)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (arrFontIndexes == null)
      throw new ArgumentNullException(nameof (arrFontIndexes));
    if (arrFills == null)
      throw new ArgumentNullException(nameof (arrFills));
    if (arrBorders == null)
      throw new ArgumentNullException(nameof (arrBorders));
    List<int> namedStyles = new List<int>();
    ++stream.Position;
    for (int index = ZipArchive.ReadInt32(stream); index > 0; --index)
    {
      if (stream.ReadByte() == 47)
      {
        ExtendedFormatImpl extendedFormat = this.ParseExtendedFormat(stream, arrFontIndexes, arrFills, arrBorders, (List<int>) null, new bool?(), arrNumberFormatIndexes, true);
        extendedFormat.Record.ParentIndex = (ushort) this.m_book.MaxXFCount;
        ExtendedFormatImpl extendedFormatImpl = this.m_book.InnerExtFormats.ForceAdd(extendedFormat);
        namedStyles.Add(extendedFormatImpl.Index);
      }
    }
    if (ZipArchive.ReadInt16(stream) == (short) 1267)
      ++stream.Position;
    return namedStyles;
  }

  private ExtendedFormatImpl ParseExtendedFormat(
    Stream stream,
    List<int> arrFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders,
    List<int> namedStyleIndexes,
    bool? includeDefault,
    Dictionary<int, int> arrNumberFormatIndexes,
    bool isCellStyleXfs)
  {
    ExtendedFormatImpl extendedFormatImpl = new ExtendedFormatImpl(this.m_book.Application, (object) this.m_book);
    ExtendedFormatRecord record = extendedFormatImpl.Record;
    ExtendedXFRecord xfRecord = extendedFormatImpl.XFRecord;
    ++stream.Position;
    int index1 = (int) ZipArchive.ReadInt16(stream);
    if (!isCellStyleXfs)
    {
      if (namedStyleIndexes != null)
        record.ParentIndex = (ushort) namedStyleIndexes[index1];
      record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
    }
    else if (namedStyleIndexes != null)
    {
      record.ParentIndex = (ushort) namedStyleIndexes[0];
      record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
    }
    else if (!isCellStyleXfs)
    {
      record.ParentIndex = (ushort) 0;
      record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
    }
    else
    {
      record.ParentIndex = (ushort) this.m_book.MaxXFCount;
      record.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
    }
    int key = (int) ZipArchive.ReadInt16(stream);
    record.FormatIndex = arrNumberFormatIndexes == null || !arrNumberFormatIndexes.ContainsKey(key) ? (ushort) key : (ushort) arrNumberFormatIndexes[key];
    int index2 = (int) ZipArchive.ReadInt16(stream);
    record.FontIndex = (ushort) arrFontIndexes[index2];
    int index3 = (int) ZipArchive.ReadInt16(stream);
    record.FillIndex = (ushort) index3;
    Excel2007Parser.CopyFillSettings(arrFills[index3], extendedFormatImpl);
    int index4 = (int) ZipArchive.ReadInt16(stream);
    record.BorderIndex = (ushort) index4;
    if (index4 > 0)
      extendedFormatImpl.HasBorder = true;
    if (index4 == arrBorders.Count)
      index4 = arrBorders.Count - 1;
    if (index4 != -1)
    {
      BordersCollection arrBorder1 = arrBorders[index4];
      if (index4 == 0 && !arrBorder1.IsEmptyBorder)
      {
        for (int index5 = 1; index5 < arrBorders.Count; ++index5)
        {
          BordersCollection arrBorder2 = arrBorders[index5];
          if (arrBorder2.IsEmptyBorder)
          {
            Excel2007Parser.CopyBorderSettings(arrBorder2, extendedFormatImpl);
            break;
          }
        }
      }
      else
        Excel2007Parser.CopyBorderSettings(arrBorder1, extendedFormatImpl);
    }
    extendedFormatImpl.Rotation = stream.ReadByte();
    extendedFormatImpl.IndentLevel = stream.ReadByte();
    byte btValue1 = (byte) stream.ReadByte();
    extendedFormatImpl.HorizontalAlignment = (ExcelHAlign) XlsbDataHolder.ReadBits(btValue1, 0, 2);
    extendedFormatImpl.VerticalAlignment = (ExcelVAlign) XlsbDataHolder.ReadBits(btValue1, 3, 5);
    extendedFormatImpl.WrapText = XlsbDataHolder.ReadBit(btValue1, 6);
    extendedFormatImpl.JustifyLast = XlsbDataHolder.ReadBit(btValue1, 7);
    byte btValue2 = (byte) stream.ReadByte();
    extendedFormatImpl.ShrinkToFit = XlsbDataHolder.ReadBit(btValue2, 0);
    extendedFormatImpl.ReadingOrder = (ExcelReadingOrderType) XlsbDataHolder.ReadBits(btValue2, 2, 3);
    extendedFormatImpl.Locked = XlsbDataHolder.ReadBit(btValue2, 4);
    extendedFormatImpl.FormulaHidden = XlsbDataHolder.ReadBit(btValue2, 5);
    extendedFormatImpl.PivotButton = XlsbDataHolder.ReadBit(btValue2, 6);
    stream.Position += 2L;
    if (index3 > 0)
      extendedFormatImpl.IncludePatterns = true;
    if (index2 > 0)
      extendedFormatImpl.IncludeFont = true;
    if (key > 0)
      extendedFormatImpl.IncludeNumberFormat = true;
    if (index4 > 0)
      extendedFormatImpl.IncludeBorder = true;
    if (index3 > 0)
      extendedFormatImpl.IncludePatterns = true;
    return this.m_book.AddExtendedProperties(extendedFormatImpl);
  }

  private List<BordersCollection> ParseBorders(Stream styleStream)
  {
    ++styleStream.Position;
    int num = ZipArchive.ReadInt32(styleStream);
    List<BordersCollection> result = new List<BordersCollection>();
    for (int index = 0; index < num; ++index)
    {
      if (styleStream.ReadByte() == 46)
        this.ParseBorder(styleStream, result);
    }
    if (ZipArchive.ReadInt16(styleStream) == (short) 1254)
      ++styleStream.Position;
    return result;
  }

  private void ParseBorder(Stream styleStream, List<BordersCollection> result)
  {
    ++styleStream.Position;
    byte btValue = (byte) styleStream.ReadByte();
    bool flag1 = XlsbDataHolder.ReadBit(btValue, 0);
    bool flag2 = XlsbDataHolder.ReadBit(btValue, 1);
    BordersCollection bordersCollection = new BordersCollection(this.m_book.Application, (object) this.m_book, true);
    BorderSettingsHolder borderSettingsHolder1 = new BorderSettingsHolder();
    borderSettingsHolder1.LineStyle = (ExcelLineStyle) styleStream.ReadByte();
    ++styleStream.Position;
    this.ParseColor(styleStream, borderSettingsHolder1.ColorObject);
    bordersCollection.SetBorder(ExcelBordersIndex.EdgeTop, (IBorder) borderSettingsHolder1);
    BorderSettingsHolder borderSettingsHolder2 = new BorderSettingsHolder();
    borderSettingsHolder2.LineStyle = (ExcelLineStyle) styleStream.ReadByte();
    ++styleStream.Position;
    this.ParseColor(styleStream, borderSettingsHolder2.ColorObject);
    bordersCollection.SetBorder(ExcelBordersIndex.EdgeBottom, (IBorder) borderSettingsHolder2);
    BorderSettingsHolder borderSettingsHolder3 = new BorderSettingsHolder();
    borderSettingsHolder3.LineStyle = (ExcelLineStyle) styleStream.ReadByte();
    ++styleStream.Position;
    this.ParseColor(styleStream, borderSettingsHolder3.ColorObject);
    bordersCollection.SetBorder(ExcelBordersIndex.EdgeLeft, (IBorder) borderSettingsHolder3);
    BorderSettingsHolder borderSettingsHolder4 = new BorderSettingsHolder();
    borderSettingsHolder4.LineStyle = (ExcelLineStyle) styleStream.ReadByte();
    ++styleStream.Position;
    this.ParseColor(styleStream, borderSettingsHolder4.ColorObject);
    bordersCollection.SetBorder(ExcelBordersIndex.EdgeRight, (IBorder) borderSettingsHolder4);
    BorderSettingsHolder borderSettingsHolder5 = new BorderSettingsHolder();
    borderSettingsHolder5.LineStyle = (ExcelLineStyle) styleStream.ReadByte();
    ++styleStream.Position;
    this.ParseColor(styleStream, borderSettingsHolder5.ColorObject);
    if (flag1)
    {
      bordersCollection.SetBorder(ExcelBordersIndex.DiagonalDown, (IBorder) borderSettingsHolder5);
      bordersCollection[ExcelBordersIndex.DiagonalDown].ShowDiagonalLine = true;
    }
    if (flag2)
    {
      bordersCollection.SetBorder(ExcelBordersIndex.DiagonalUp, (IBorder) borderSettingsHolder5);
      bordersCollection[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine = true;
    }
    result.Add(bordersCollection);
  }

  private List<FillImpl> ParseFills(Stream styleStream)
  {
    ++styleStream.Position;
    int num = ZipArchive.ReadInt32(styleStream);
    List<FillImpl> result = new List<FillImpl>();
    for (int index = 0; index < num; ++index)
    {
      if (styleStream.ReadByte() == 45)
        this.ParseFill(styleStream, result);
    }
    if (ZipArchive.ReadInt16(styleStream) == (short) 1244)
      ++styleStream.Position;
    return result;
  }

  private void ParseFill(Stream styleStream, List<FillImpl> result)
  {
    ++styleStream.Position;
    FillImpl fillImpl = new FillImpl();
    fillImpl.Pattern = (ExcelPattern) ZipArchive.ReadInt32(styleStream);
    this.ParseColor(styleStream, fillImpl.ColorObject);
    this.ParseColor(styleStream, fillImpl.PatternColorObject);
    styleStream.Position += 44L;
    int num = ZipArchive.ReadInt32(styleStream);
    styleStream.Position += (long) (num * 16 /*0x10*/);
    result.Add(fillImpl);
  }

  private List<int> ParseFonts(Stream styleStream)
  {
    ++styleStream.Position;
    int num = ZipArchive.ReadInt32(styleStream);
    List<int> fontIndexes = new List<int>();
    for (int index = 0; index < num; ++index)
    {
      if (styleStream.ReadByte() == 43)
        this.ParseFont(styleStream, fontIndexes);
    }
    if (ZipArchive.ReadInt16(styleStream) == (short) 1252)
      ++styleStream.Position;
    return fontIndexes;
  }

  private int ParseFont(Stream stream, List<int> fontIndexes)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    FontImpl font = (FontImpl) this.m_book.CreateFont((IFont) null, false);
    if (this.m_book.InnerFonts.Count != 0)
    {
      font.FontName = this.m_book.InnerFonts[0].FontName;
      font.Size = this.m_book.InnerFonts[0].Size;
    }
    ++stream.Position;
    font.Size = FontImpl.SizeInPoints((int) ZipArchive.ReadInt16(stream));
    FontRecord.FontAttributes fontAttributes = (FontRecord.FontAttributes) ZipArchive.ReadInt16(stream);
    font.Italic = (fontAttributes & FontRecord.FontAttributes.Italic) != (FontRecord.FontAttributes) 0;
    font.Strikethrough = (fontAttributes & FontRecord.FontAttributes.Strikeout) != (FontRecord.FontAttributes) 0;
    font.MacOSOutlineFont = (fontAttributes & FontRecord.FontAttributes.MacOutline) != (FontRecord.FontAttributes) 0;
    font.MacOSShadow = (fontAttributes & FontRecord.FontAttributes.MacShadow) != (FontRecord.FontAttributes) 0;
    font.Record.BoldWeight = (ushort) ZipArchive.ReadInt16(stream);
    switch (ZipArchive.ReadInt16(stream))
    {
      case 1:
        font.Superscript = true;
        break;
      case 2:
        font.Subscript = true;
        break;
    }
    switch (stream.ReadByte())
    {
      case 1:
        font.Underline = ExcelUnderline.Single;
        break;
      case 2:
        font.Underline = ExcelUnderline.Double;
        break;
      case 33:
        font.Underline = ExcelUnderline.SingleAccounting;
        break;
      case 34:
        font.Underline = ExcelUnderline.DoubleAccounting;
        break;
      default:
        font.Underline = ExcelUnderline.None;
        break;
    }
    font.Family = (byte) stream.ReadByte();
    font.CharSet = (byte) stream.ReadByte();
    ++stream.Position;
    this.ParseColor(stream, font.ColorObject);
    switch ((byte) stream.ReadByte())
    {
      case 1:
        font.Scheme = "major";
        break;
      case 2:
        font.Scheme = "minor";
        break;
    }
    int num = ZipArchive.ReadInt32(stream);
    byte[] numArray = new byte[num * 2];
    stream.Read(numArray, 0, num * 2);
    font.FontName = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    FontImpl fontImpl = (FontImpl) this.m_book.InnerFonts.Add((IFont) font);
    fontIndexes?.Add(fontImpl.Index);
    return fontImpl.Index;
  }

  private Dictionary<int, int> ParseNumberFormats(Stream styleStream)
  {
    ++styleStream.Position;
    int num = ZipArchive.ReadInt32(styleStream);
    Dictionary<int, int> result = new Dictionary<int, int>();
    for (int index = 0; index < num; ++index)
    {
      if (styleStream.ReadByte() == 44)
        this.ParseNumberFormat(styleStream, result);
    }
    if (ZipArchive.ReadInt16(styleStream) == (short) 1256)
      ++styleStream.Position;
    return result;
  }

  private void ParseNumberFormat(Stream stream, Dictionary<int, int> result)
  {
    if (stream == null)
      throw new ArgumentNullException("reader");
    ++stream.Position;
    int key = (int) ZipArchive.ReadInt16(stream);
    int num = ZipArchive.ReadInt32(stream);
    byte[] numArray = new byte[num * 2];
    stream.Read(numArray, 0, num * 2);
    string customizedString = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    if (!(customizedString != string.Empty))
      return;
    int formatId = key;
    if (!this.m_book.InnerFormats.ContainsFormat(customizedString))
    {
      customizedString = this.m_book.InnerFormats.GetCustomizedString(customizedString);
      if (Array.IndexOf<int>(this.DEF_NUMBERFORMAT_INDEXES, key) < 0)
        formatId = 163 + this.m_book.InnerFormats.Count - 36 + 1;
    }
    else
      formatId = this.m_book.InnerFormats[customizedString].Index;
    if (!result.ContainsKey(key))
      result.Add(key, formatId);
    this.m_book.InnerFormats.Add(formatId, customizedString);
    this.m_book.InnerFormats[customizedString].isUsed = true;
    this.m_book.InnerFormats.HasNumberFormats = true;
  }

  private void ParseWorkbookPart(
    Stream stream,
    RelationCollection relations,
    XlsbDataHolder xlsbDataHolder,
    string strWorkbookPath)
  {
    if (ZipArchive.ReadInt16(stream) == (short) 387)
    {
      int num = stream.ReadByte();
      while (num != 143)
        num = stream.ReadByte();
      if (num == 143)
      {
        stream.Position += 2L;
        this.ParseSheetsOptions(stream, relations, this.m_book.DataHolder, strWorkbookPath);
        ++stream.Position;
      }
      if (ZipArchive.ReadInt16(stream) == (short) 737)
        this.ParseExternalReferences(stream);
      else
        stream.Position -= 2L;
      for (int index = stream.ReadByte(); index == 39; index = stream.ReadByte())
        this.ParseNamedRange(stream);
    }
    if (this.m_book == null)
      return;
    this.m_book.ActiveSheetIndex = 0;
  }

  private void ParseNamedRange(Stream stream)
  {
    ++stream.Position;
    bool flag1 = XlsbDataHolder.ReadBit((byte) stream.ReadByte(), 0);
    stream.Position += 4L;
    int index = ZipArchive.ReadInt32(stream);
    bool flag2 = index == -1;
    byte[] numArray1 = new byte[ZipArchive.ReadInt32(stream) * 2];
    stream.Read(numArray1, 0, numArray1.Length);
    string name1 = Encoding.Unicode.GetString(numArray1, 0, numArray1.Length);
    WorksheetImpl worksheetImpl = flag2 ? this.m_book.Objects[index] as WorksheetImpl : (WorksheetImpl) null;
    bool flag3 = flag2 || worksheetImpl != null;
    IName name2 = !flag3 || worksheetImpl == null ? this.m_book.Names.Add(name1) : worksheetImpl.Names.Add(name1);
    if (flag3 && worksheetImpl == null)
      (name2 as NameImpl).SheetIndex = index;
    int finalOffset = ZipArchive.ReadInt32(stream);
    byte[] numArray2 = new byte[finalOffset];
    stream.Read(numArray2, 0, finalOffset);
    ByteArrayDataProvider provider = new ByteArrayDataProvider(numArray2);
    if (finalOffset > 0)
      (name2 as NameImpl).Record.FormulaTokens = FormulaUtil.ParseExpression((DataProvider) provider, 0, finalOffset, out finalOffset, ExcelVersion.Excel2007);
    finalOffset = ZipArchive.ReadInt32(stream);
    ZipArchive.ReadInt32(stream);
    name2.Visible = !flag1;
  }

  private void ParseExternalReferences(Stream stream)
  {
    ++stream.Position;
    if (ZipArchive.ReadInt16(stream) == (short) 741)
    {
      ++stream.Position;
      if (ZipArchive.ReadInt16(stream) == (short) 746)
      {
        ++stream.Position;
        for (int index = ZipArchive.ReadInt32(stream); index > 0; --index)
        {
          ZipArchive.ReadInt32(stream);
          this.m_book.ExternSheet.AddReference(this.m_book.InsertSelfSupbook(), ZipArchive.ReadInt32(stream), ZipArchive.ReadInt32(stream));
        }
      }
    }
    stream.Position += 3L;
  }

  private void ParseSheetsOptions(
    Stream stream,
    RelationCollection relations,
    FileDataHolder holder,
    string bookPath)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    this.m_book.Objects.Clear();
    this.m_book.InnerWorksheets.Clear();
    int num1 = 0;
    int num2;
    for (num2 = (int) ZipArchive.ReadInt16(stream); num2 != 412; num2 = (int) ZipArchive.ReadInt16(stream))
    {
      int num3 = stream.ReadByte();
      while (num3 != 143)
        num3 = stream.ReadByte();
      stream.Position += 2L;
    }
    for (; num2 == 412; num2 = (int) ZipArchive.ReadInt16(stream))
    {
      ++stream.Position;
      this.ParseWorkbookSheetEntry(stream, relations, holder, bookPath, ++num1);
    }
  }

  private void ParseWorkbookSheetEntry(
    Stream stream,
    RelationCollection relations,
    FileDataHolder holder,
    string bookPath,
    int sheetRelationIdCount)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    string strVisibility = (string) null;
    switch (ZipArchive.ReadInt32(stream))
    {
      case 0:
        strVisibility = "visible";
        break;
      case 1:
        strVisibility = "hidden";
        break;
      case 2:
        strVisibility = "veryHidden";
        break;
    }
    int num1 = ZipArchive.ReadInt32(stream);
    stream.Position += 4L;
    byte[] numArray1 = new byte[8];
    stream.Read(numArray1, 0, 8);
    string id1 = Encoding.Unicode.GetString(numArray1, 0, numArray1.Length);
    int num2 = ZipArchive.ReadInt32(stream);
    byte[] numArray2 = new byte[num2 * 2];
    stream.Read(numArray2, 0, num2 * 2);
    string str = Encoding.Unicode.GetString(numArray2, 0, numArray2.Length);
    if (id1 == "" && strVisibility == "veryHidden")
      return;
    Relation relation = relations[id1];
    if (relation == null)
    {
      relation = new Relation($"worksheets/sheet{sheetRelationIdCount}.xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet");
      string id2 = $"rId{relations.Count}";
      relations[id2] = relation;
      id1 = id2;
    }
    WorksheetBaseImpl sheet;
    switch (relation.Type)
    {
      case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet":
        sheet = (WorksheetBaseImpl) this.m_book.InnerWorksheets.Add(str);
        break;
      case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartsheet":
        sheet = (WorksheetBaseImpl) this.m_book.InnerCharts.Add(str);
        sheet.PageSetupBase.Orientation = ExcelPageOrientation.Landscape;
        break;
      case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/dialogsheet":
        DialogSheet dialogSheet = new DialogSheet(str)
        {
          DataHolder = new WorksheetDataHolder(holder, relation, bookPath)
        };
        dialogSheet.DataHolder.RelationId = id1;
        dialogSheet.DataHolder.SheetId = num1.ToString();
        this.m_book.InnerDialogs.Add(dialogSheet);
        relations.Remove(id1);
        this.m_book.Objects.InnerList.Add((object) dialogSheet);
        return;
      case "http://schemas.microsoft.com/office/2006/relationships/xlMacrosheet":
      case "http://schemas.microsoft.com/office/2006/relationships/xlIntlMacrosheet":
        MacroSheet macroSheet = new MacroSheet(str, relation.Type == "http://schemas.microsoft.com/office/2006/relationships/xlIntlMacrosheet")
        {
          DataHolder = new WorksheetDataHolder(holder, relation, bookPath)
        };
        macroSheet.DataHolder.RelationId = id1;
        macroSheet.DataHolder.SheetId = num1.ToString();
        this.m_book.InnerMacros.Add(macroSheet);
        relations.Remove(id1);
        this.m_book.Objects.InnerList.Add((object) macroSheet);
        return;
      default:
        throw new XmlException("Unknown part type: " + relation.Type);
    }
    sheet.DataHolder = new WorksheetDataHolder(holder, relation, bookPath);
    sheet.m_dataHolder.RelationId = id1;
    sheet.m_dataHolder.SheetId = num1.ToString();
    sheet.IsSaved = true;
    this.Parser.SetVisibilityState(sheet, strVisibility);
    relations.Remove(id1);
  }

  internal void ParseDocumentProperties()
  {
    this.ParseArchiveItemByContentType("application/vnd.openxmlformats-package.core-properties+xml");
    this.ParseArchiveItemByContentType("application/vnd.openxmlformats-officedocument.extended-properties+xml");
    this.ParseArchiveItemByContentType("application/vnd.openxmlformats-officedocument.custom-properties+xml");
  }

  internal void ParseArchiveItemByContentType(string strContentType)
  {
    string strItemName;
    XmlReader readerByContentType = this.GetXmlReaderByContentType(strContentType, out strItemName);
    if (readerByContentType == null)
      return;
    switch (strContentType)
    {
      case "application/vnd.openxmlformats-package.core-properties+xml":
        this.Parser.ParseDocumentCoreProperties(readerByContentType);
        this.m_topRelations.RemoveByContentType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
        break;
      case "application/vnd.openxmlformats-officedocument.extended-properties+xml":
        this.Parser.ParseExtendedProperties(readerByContentType);
        this.m_topRelations.RemoveByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
        break;
      case "application/vnd.openxmlformats-officedocument.custom-properties+xml":
        this.Parser.ParseCustomProperties(readerByContentType);
        this.m_topRelations.RemoveByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
        break;
      default:
        throw new ArgumentException(nameof (strContentType));
    }
    this.m_Archive.RemoveItem(strItemName);
  }

  internal XmlReader GetXmlReaderByContentType(string strContentType, out string strItemName)
  {
    string str = this.FindItemByContent(strContentType);
    if (str == null)
    {
      strItemName = string.Empty;
      return (XmlReader) null;
    }
    this.m_book.DataHolder.OverriddenContentTypes.Remove(str);
    if (str.StartsWith("/"))
      str = UtilityMethods.RemoveFirstCharUnsafe(str);
    ZipArchiveItem zipArchiveItem = this.m_Archive[str];
    strItemName = zipArchiveItem.ItemName;
    Stream dataStream = zipArchiveItem.DataStream;
    dataStream.Position = 0L;
    return UtilityMethods.CreateReader(dataStream);
  }

  private string FindItemByContent(string contentType)
  {
    return this.FindItemByContentInOverride(contentType) ?? this.FindItemByContentInDefault(contentType);
  }

  private string FindItemByContentInDefault(string contentType)
  {
    string contentInDefault = (string) null;
    foreach (KeyValuePair<string, string> dicDefaultType in (IEnumerable<KeyValuePair<string, string>>) this.m_dicDefaultTypes)
    {
      if (dicDefaultType.Value == contentType)
      {
        string key1 = dicDefaultType.Key;
        int index = 0;
        for (int count = this.m_Archive.Count; index < count; ++index)
        {
          string key2 = this.m_Archive[index].ItemName;
          if (key2[0] != '/')
            key2 = '/'.ToString() + key2;
          if (key2.EndsWith(key1) && !this.m_book.DataHolder.OverriddenContentTypes.ContainsKey(key2))
          {
            contentInDefault = key2;
            break;
          }
        }
        break;
      }
    }
    return contentInDefault;
  }

  private string FindItemByContentInOverride(string contentType)
  {
    string contentInOverride = (string) null;
    foreach (KeyValuePair<string, string> overriddenContentType in (IEnumerable<KeyValuePair<string, string>>) this.m_book.DataHolder.OverriddenContentTypes)
    {
      if (overriddenContentType.Value == contentType)
      {
        contentInOverride = overriddenContentType.Key;
        break;
      }
    }
    return contentInOverride;
  }

  private RelationCollection ParseRelations(string topRelationsPath)
  {
    return this.Parser.ParseRelations(UtilityMethods.CreateReader(this.m_Archive[topRelationsPath].DataStream));
  }

  private void ParseContentType()
  {
    this.m_dicDefaultTypes.Clear();
    this.m_book.DataHolder.OverriddenContentTypes.Clear();
    this.Parser.ParseContentTypes(UtilityMethods.CreateReader((this.m_Archive["[Content_Types].xml"] ?? throw new NotSupportedException("File cannot be opened - format is not supported")).DataStream), this.m_book.DataHolder.DefaultContentTypes, this.m_book.DataHolder.OverriddenContentTypes);
    this.m_dictItemsToRemove.Add("[Content_Types].xml", (object) null);
  }

  private void ParseColor(Stream stream, ColorObject color)
  {
    int num = stream.ReadByte();
    ColorType colorType = ColorType.None;
    if (num >> 1 == 1)
      colorType = ColorType.Indexed;
    else if (num >> 1 == 2)
      colorType = ColorType.RGB;
    else if (num >> 1 == 3)
      colorType = ColorType.Theme;
    int themeIndex = stream.ReadByte();
    int dTintValue = (int) ZipArchive.ReadInt16(stream);
    int red = stream.ReadByte();
    int green = stream.ReadByte();
    int blue = stream.ReadByte();
    int alpha = stream.ReadByte();
    switch (colorType)
    {
      case ColorType.Indexed:
        ExcelKnownColors int32 = (ExcelKnownColors) Convert.ToInt32(themeIndex);
        color.SetIndexed(int32, true, this.m_book);
        break;
      case ColorType.RGB:
        Color rgb = Color.FromArgb((int) (byte) alpha, (int) (byte) red, (int) (byte) green, (int) (byte) blue);
        color.SetRGB(rgb, (IWorkbook) this.m_book, (double) dTintValue);
        color.ColorType = ColorType.RGB;
        break;
      case ColorType.Theme:
        color.SetTheme(themeIndex, (IWorkbook) this.m_book, (double) dTintValue);
        break;
    }
  }

  private void SerializeColor(Stream stream, ColorObject color)
  {
    switch (color.ColorType)
    {
      case ColorType.Indexed:
        if (color.Value != (int) short.MaxValue)
        {
          byte num = this.WriteBits(this.WriteBit((byte) 0, true, 0), (byte) 1, 1, 7);
          stream.WriteByte(num);
          break;
        }
        stream.WriteByte((byte) 1);
        break;
      case ColorType.RGB:
        stream.WriteByte((byte) 5);
        break;
      case ColorType.Theme:
        stream.WriteByte((byte) 7);
        break;
      default:
        stream.WriteByte((byte) 0);
        break;
    }
    stream.WriteByte((byte) color.Value);
    stream.Write(BitConverter.GetBytes(Convert.ToInt16(color.Tint)), 0, 2);
    stream.WriteByte(color.GetRGB((IWorkbook) this.m_book).R);
    stream.WriteByte(color.GetRGB((IWorkbook) this.m_book).G);
    stream.WriteByte(color.GetRGB((IWorkbook) this.m_book).B);
    stream.WriteByte(color.GetRGB((IWorkbook) this.m_book).A);
  }

  internal void Serialize(string fileName, WorkbookImpl book)
  {
    this.m_book = book;
    this.m_Archive = this.m_book.DataHolder.Archive;
    this.SaveDocument(fileName);
  }

  private void SaveDocument(string fileName)
  {
    using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
      this.SaveDocument((Stream) fileStream);
  }

  private void EmptyArchive()
  {
    if (this.m_Archive == null)
      return;
    while (this.m_Archive.Items.Length > 0)
      this.m_Archive.RemoveAt(0);
  }

  private void SaveDocument(Stream stream)
  {
    this.EmptyArchive();
    this.SaveDocument();
    this.m_Archive.Save(stream, true);
  }

  private void SaveDocument()
  {
    this.SaveWorkbook();
    this.SaveDocumentProperties();
    this.SaveContentTypes();
    this.SaveTopLevelRelations();
  }

  private void SaveTopLevelRelations()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    new Excel2007Serializator(this.m_book).SerializeRelations(writer, this.m_topRelations, (WorksheetDataHolder) null);
    writer.Flush();
    memoryStream.Flush();
    ZipArchiveItem zipArchiveItem = this.m_Archive["_rels/.rels"];
    if (zipArchiveItem != null)
      zipArchiveItem.Update((Stream) memoryStream, true);
    else
      this.m_Archive.AddItem("_rels/.rels", (Stream) memoryStream, false, FileAttributes.Archive);
  }

  private void SaveContentTypes()
  {
    this.FillDefaultContentTypes();
    this.SaveArchiveItem("[Content_Types].xml");
  }

  private void SaveDocumentProperties()
  {
    this.SaveArchiveItemRelationContentType("docProps/app.xml", "application/vnd.openxmlformats-officedocument.extended-properties+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
    this.SaveArchiveItemRelationContentType("docProps/core.xml", "application/vnd.openxmlformats-package.core-properties+xml", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
  }

  private void SaveArchiveItemRelationContentType(
    string partName,
    string contentType,
    string relationType)
  {
    this.m_book.DataHolder.OverriddenContentTypes["/" + partName] = contentType;
    string relationId;
    this.m_topRelations.FindRelationByContentType(relationType, out relationId);
    if (relationId == null)
      relationId = this.m_topRelations.GenerateRelationId();
    this.m_topRelations[relationId] = new Relation(partName, relationType);
    this.SaveArchiveItem(partName);
  }

  private void SaveArchiveItem(string strItemPartName)
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    Excel2007Serializator excel2007Serializator = new Excel2007Serializator(this.m_book);
    switch (strItemPartName)
    {
      case "docProps/app.xml":
        excel2007Serializator.SerializeExtendedProperties(writer);
        break;
      case "docProps/core.xml":
        excel2007Serializator.SerializeCoreProperties(writer);
        break;
      case "[Content_Types].xml":
        excel2007Serializator.SerializeContentTypes(writer, this.m_dicDefaultTypes, this.m_book.DataHolder.OverriddenContentTypes);
        break;
      case "docProps/custom.xml":
        excel2007Serializator.SerializeCustomProperties(writer);
        break;
      default:
        throw new ArgumentException(nameof (strItemPartName));
    }
    writer.Flush();
    ZipArchiveItem zipArchiveItem = this.m_Archive[strItemPartName];
    if (zipArchiveItem != null)
      zipArchiveItem.Update((Stream) memoryStream, true);
    else
      this.m_Archive.AddItem(strItemPartName, (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private void FillDefaultContentTypes()
  {
    this.m_dicDefaultTypes["bin"] = "application/vnd.ms-excel.sheet.binary.macroEnabled.main";
    this.m_dicDefaultTypes["xml"] = "application/xml";
    this.m_dicDefaultTypes["rels"] = "application/vnd.openxmlformats-package.relationships+xml";
  }

  private void SaveWorkbook()
  {
    if (this.m_topRelations == null)
    {
      this.m_topRelations = new RelationCollection();
      this.m_topRelations[FileDataHolder.GetRelationId(1)] = new Relation("xl/workbook.bin", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
    }
    this.m_book.DataHolder.OverriddenContentTypes.Clear();
    if (this.m_workbookRelations == null)
      this.m_workbookRelations = new RelationCollection();
    Dictionary<int, int> xfIndexes = this.SaveStyles();
    this.SaveSharedStrings();
    this.SaveSheets(xfIndexes, this.m_workbookRelations);
    this.SaveWorkbookPart();
    string path;
    FileDataHolder.SeparateItemName("xl/workbook.bin", out path);
    this.m_strStylesRelationId = FileDataHolder.AddRelation(this.m_workbookRelations, "xl/styles.bin", path, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", this.m_strStylesRelationId);
    if (this.m_book.InnerSST.ActiveCount > 0)
      this.m_strSSTRelationId = FileDataHolder.AddRelation(this.m_workbookRelations, "/xl/sharedStrings.bin", path, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", this.m_strSSTRelationId);
    this.SaveRelations("xl/workbook.bin", this.m_workbookRelations);
  }

  internal void SaveRelations(string parentPartName, RelationCollection relations)
  {
    if (relations == null || relations.Count == 0)
      return;
    string itemName = parentPartName != null && parentPartName.Length != 0 ? FileDataHolder.GetCorrespondingRelations(parentPartName) : throw new ArgumentOutOfRangeException(nameof (parentPartName));
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
    new Excel2007Serializator(this.m_book).SerializeRelations(writer, relations, (WorksheetDataHolder) null);
    writer.Flush();
    newDataStream.Flush();
    this.m_Archive.UpdateItem(itemName, (Stream) newDataStream, false, FileAttributes.Archive);
  }

  private void SaveSharedStrings()
  {
    this.m_book.DataHolder.OverriddenContentTypes[new AddSlashPreprocessor().PreprocessName("/xl/sharedStrings.bin")] = "application/vnd.ms-excel.sharedStrings";
    ZippedContentStream stream = new ZippedContentStream(new ZipArchive.CompressorCreator(this.m_book.AppImplementation.CreateCompressor));
    SSTDictionary innerSst = this.m_book.InnerSST;
    stream.Write(BitConverter.GetBytes(415), 0, 2);
    int count = innerSst.Count;
    int labelSstCount = innerSst.GetLabelSSTCount();
    stream.WriteByte((byte) 8);
    stream.Write(BitConverter.GetBytes(count), 0, 4);
    stream.Write(BitConverter.GetBytes(labelSstCount), 0, 4);
    for (int index = 0; index < count; ++index)
    {
      object sstContentByIndex = innerSst.GetSSTContentByIndex(index);
      this.SerializeStringItem((Stream) stream, sstContentByIndex);
    }
    stream.Write(BitConverter.GetBytes(416), 0, 2);
    stream.WriteByte((byte) 0);
    string itemName = "/xl/sharedStrings.bin";
    if ("/xl/sharedStrings.bin"[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe("/xl/sharedStrings.bin");
    (this.m_Archive[itemName] ?? this.m_Archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive)).Update(stream);
  }

  private void SerializeStringItem(Stream stream, object objTextOrString)
  {
    stream.Write(BitConverter.GetBytes(19), 0, 1);
    byte[] bytes = Encoding.Unicode.GetBytes(objTextOrString.ToString());
    stream.WriteByte((byte) (bytes.Length + 4 + 1));
    stream.WriteByte((byte) 0);
    stream.Write(BitConverter.GetBytes(bytes.Length / 2), 0, 4);
    stream.Write(bytes, 0, bytes.Length);
  }

  private Dictionary<int, int> SaveStyles()
  {
    this.m_book.DataHolder.OverriddenContentTypes[new AddSlashPreprocessor().PreprocessName("xl/styles.bin")] = "application/vnd.ms-excel.styles";
    MemoryStream memoryStream = new MemoryStream();
    memoryStream.Write(BitConverter.GetBytes(662), 0, 2);
    memoryStream.WriteByte((byte) 0);
    this.SerializeNumberFormats((Stream) memoryStream);
    this.SerializeFonts(memoryStream);
    int[] numArray1 = this.SerializeFills(memoryStream);
    int[] numArray2 = this.SerializeBorders(memoryStream);
    int count = this.m_book.InnerExtFormats.Count;
    Dictionary<int, int> dictionary1 = this.SerializeNamedStyleXFs(memoryStream, numArray1, numArray2);
    Dictionary<int, int> dictionary2 = this.SerializeNotNamedXFs(memoryStream, numArray1, numArray2, dictionary1);
    this.SerializeStyles(memoryStream, dictionary1);
    memoryStream.Write(BitConverter.GetBytes(663), 0, 2);
    memoryStream.WriteByte((byte) 0);
    this.m_Archive.AddItem("xl/styles.bin", (Stream) memoryStream, false, FileAttributes.Archive);
    return dictionary2;
  }

  private void SerializeStyles(MemoryStream stream, Dictionary<int, int> hashNamedStyleIndexes)
  {
    stream.Write(BitConverter.GetBytes(1259), 0, 2);
    stream.WriteByte((byte) 4);
    StylesCollection innerStyles = this.m_book.InnerStyles;
    int count = innerStyles.Count;
    long position1 = stream.Position;
    long position2 = stream.Position;
    stream.Position += 4L;
    int num = 0;
    for (int i = 0; i < count; ++i)
    {
      StyleImpl style = (StyleImpl) innerStyles[i];
      StyleExtRecord styleExt = style.StyleExt;
      if (styleExt == null || (style.BuiltIn || !styleExt.IsBuildInStyle) && !styleExt.IsHidden)
      {
        this.SerializeStyle((Stream) stream, style, hashNamedStyleIndexes);
        ++num;
        position2 = stream.Position;
      }
    }
    stream.Position = position1;
    stream.Write(BitConverter.GetBytes(num), 0, 4);
    stream.Position = position2;
    stream.Write(BitConverter.GetBytes(1260), 0, 2);
    stream.WriteByte((byte) 0);
  }

  private void SerializeStyle(
    Stream stream,
    StyleImpl style,
    Dictionary<int, int> hashNewParentIndexes)
  {
    stream.Write(BitConverter.GetBytes(48 /*0x30*/), 0, 1);
    byte[] bytes = Encoding.Unicode.GetBytes(style.Name);
    int num1 = bytes.Length + 12;
    stream.WriteByte((byte) num1);
    int hashNewParentIndex = hashNewParentIndexes[style.XFormatIndex];
    stream.Write(BitConverter.GetBytes(hashNewParentIndex), 0, 4);
    StyleRecord record = style.Record;
    int num2 = 1;
    if (!style.BuiltIn)
      num2 = 0;
    stream.Write(BitConverter.GetBytes(num2), 0, 2);
    stream.WriteByte(record.BuildInOrNameLen);
    stream.WriteByte(record.OutlineStyleLevel);
    stream.Write(BitConverter.GetBytes(bytes.Length / 2), 0, 4);
    stream.Write(bytes, 0, bytes.Length);
  }

  private Dictionary<int, int> SerializeNotNamedXFs(
    MemoryStream stream,
    int[] arrFillIndexes,
    int[] arrBorderIndexes,
    Dictionary<int, int> hashNewParentIndexes)
  {
    stream.Write(BitConverter.GetBytes(1257), 0, 2);
    stream.WriteByte((byte) 4);
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count1 = innerExtFormats.Count;
    int count2 = this.m_book.InnerStyles.Count;
    stream.Write(BitConverter.GetBytes(count1), 0, 4);
    int num = 0;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    for (int index = 0; index < count1; ++index)
    {
      ExtendedFormatImpl format = innerExtFormats[index];
      if (!dictionary.ContainsKey(format.Index))
      {
        dictionary.Add(format.Index, num);
        this.SerializeExtendedFormat(stream, arrFillIndexes, arrBorderIndexes, format, false);
        ++num;
      }
    }
    stream.Write(BitConverter.GetBytes(1258), 0, 2);
    stream.WriteByte((byte) 0);
    return dictionary;
  }

  private Dictionary<int, int> SerializeNamedStyleXFs(
    MemoryStream stream,
    int[] fills,
    int[] borders)
  {
    if (stream == null)
      throw new ArgumentNullException("writer");
    if (fills == null)
      throw new ArgumentNullException("arrFillIndexes");
    if (borders == null)
      throw new ArgumentNullException("arrBorderIndexes");
    stream.Write(BitConverter.GetBytes(1266), 0, 2);
    stream.WriteByte((byte) 4);
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count1 = innerExtFormats.Count;
    stream.Write(BitConverter.GetBytes(count1), 0, 4);
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int num = 0;
    int index = 0;
    for (int count2 = innerExtFormats.Count; index < count2; ++index)
    {
      ExtendedFormatImpl format = innerExtFormats[index];
      if (!dictionary.ContainsKey(format.Index))
      {
        this.SerializeExtendedFormat(stream, fills, borders, format, true);
        dictionary.Add(format.Index, num);
        ++num;
      }
    }
    stream.Write(BitConverter.GetBytes(1267), 0, 2);
    stream.WriteByte((byte) 0);
    return dictionary;
  }

  private void SerializeExtendedFormat(
    MemoryStream stream,
    int[] arrFillIndexes,
    int[] arrBorderIndexes,
    ExtendedFormatImpl format,
    bool isCellStyleXf)
  {
    if (stream == null)
      throw new ArgumentNullException("writer");
    if (arrFillIndexes == null)
      throw new ArgumentNullException(nameof (arrFillIndexes));
    if (arrBorderIndexes == null)
      throw new ArgumentNullException(nameof (arrBorderIndexes));
    int index = format != null ? format.Index : throw new ArgumentNullException(nameof (format));
    stream.Write(BitConverter.GetBytes(47), 0, 1);
    stream.WriteByte((byte) 16 /*0x10*/);
    if (isCellStyleXf)
      stream.Write(BitConverter.GetBytes((int) ushort.MaxValue), 0, 2);
    else
      stream.Write(BitConverter.GetBytes(0), 0, 2);
    stream.Write(BitConverter.GetBytes(format.NumberFormatIndex), 0, 2);
    stream.Write(BitConverter.GetBytes(format.FontIndex), 0, 2);
    stream.Write(BitConverter.GetBytes(arrFillIndexes[index]), 0, 2);
    stream.Write(BitConverter.GetBytes(arrBorderIndexes[index]), 0, 2);
    stream.WriteByte((byte) format.Rotation);
    stream.WriteByte((byte) format.IndentLevel);
    byte num1 = this.WriteBit(this.WriteBit(this.WriteBits(this.WriteBits((byte) 0, (byte) format.HorizontalAlignment, 0, 2), (byte) format.VerticalAlignment, 3, 5), format.WrapText, 6), format.JustifyLast, 7);
    stream.WriteByte(num1);
    byte num2 = this.WriteBit(this.WriteBit(this.WriteBit(this.WriteBits(this.WriteBit((byte) 0, format.ShrinkToFit, 0), (byte) format.ReadingOrder, 2, 3), format.Locked, 4), format.FormulaHidden, 5), format.PivotButton, 6);
    stream.WriteByte(num2);
    stream.Write(BitConverter.GetBytes(0), 0, 2);
  }

  private int[] SerializeBorders(MemoryStream stream)
  {
    Dictionary<BordersCollection, int> dictionary = new Dictionary<BordersCollection, int>();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count = innerExtFormats.Count;
    int[] numArray = new int[count];
    BordersCollection[] bordersCollectionArray = new BordersCollection[count];
    int index1 = -1;
    for (int index2 = 0; index2 < count; ++index2)
    {
      BordersCollection key;
      if (index1 == -1)
      {
        WorkbookImpl book = this.m_book;
        BordersCollection bordersCollection = new BordersCollection(book.Application, (object) book, true);
        ExtendedFormatWrapper impl = new ExtendedFormatWrapper(book, 0);
        bordersCollection.InnerList.Clear();
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.DiagonalDown));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.DiagonalUp));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeBottom));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeLeft));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeRight));
        bordersCollection.InnerList.Add((IBorder) new BorderImpl(book.Application, (object) book, (IInternalExtendedFormat) impl, ExcelBordersIndex.EdgeTop));
        key = bordersCollection;
      }
      else
        key = (BordersCollection) innerExtFormats[index2].Borders;
      if (dictionary.ContainsKey(key))
      {
        numArray[index2] = dictionary[key];
      }
      else
      {
        index1 = dictionary.Count;
        dictionary.Add(key, index1);
        bordersCollectionArray[index1] = key;
        numArray[index2] = index1;
        if (index1 == 0)
          --index2;
      }
    }
    stream.Write(BitConverter.GetBytes(1253), 0, 2);
    stream.WriteByte((byte) 4);
    stream.Write(BitConverter.GetBytes(index1 + 1), 0, 4);
    for (int index3 = 0; index3 <= index1; ++index3)
      this.SerializeBordersCollection(stream, bordersCollectionArray[index3]);
    stream.Write(BitConverter.GetBytes(1254), 0, 2);
    stream.WriteByte((byte) 0);
    return numArray;
  }

  private void SerializeBordersCollection(MemoryStream stream, BordersCollection bordersCollection)
  {
    stream.Write(BitConverter.GetBytes(46), 0, 1);
    stream.WriteByte((byte) 51);
    byte num = this.WriteBit(this.WriteBit((byte) 0, bordersCollection[ExcelBordersIndex.DiagonalDown].ShowDiagonalLine, 0), bordersCollection[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine, 1);
    stream.WriteByte(num);
    this.SerializeBorder((Stream) stream, bordersCollection[ExcelBordersIndex.EdgeTop]);
    this.SerializeBorder((Stream) stream, bordersCollection[ExcelBordersIndex.EdgeBottom]);
    this.SerializeBorder((Stream) stream, bordersCollection[ExcelBordersIndex.EdgeLeft]);
    this.SerializeBorder((Stream) stream, bordersCollection[ExcelBordersIndex.EdgeRight]);
    if (bordersCollection[ExcelBordersIndex.DiagonalUp].ShowDiagonalLine)
      this.SerializeBorder((Stream) stream, bordersCollection[ExcelBordersIndex.DiagonalUp]);
    else
      this.SerializeBorder((Stream) stream, bordersCollection[ExcelBordersIndex.DiagonalDown]);
  }

  private void SerializeBorder(Stream stream, IBorder border)
  {
    stream.WriteByte((byte) border.LineStyle);
    stream.WriteByte((byte) 0);
    this.SerializeColor(stream, border.ColorObject);
  }

  private int[] SerializeFills(MemoryStream stream)
  {
    Dictionary<FillImpl, int> dictionary = new Dictionary<FillImpl, int>();
    if (this.m_book.m_bisUnusedXFRemoved)
      this.m_book.m_bisUnusedXFRemoved = false;
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int count = innerExtFormats.Count;
    int[] numArray = new int[count];
    FillImpl[] array = new FillImpl[count];
    int index1 = -1;
    for (int index2 = 0; index2 < count; ++index2)
    {
      FillImpl key;
      switch (index1)
      {
        case -1:
          key = new FillImpl();
          key.Pattern = ExcelPattern.None;
          key.PatternColorObject.SetIndexed(ExcelKnownColors.BlackCustom);
          key.ColorObject.SetIndexed(ExcelKnownColors.None);
          break;
        case 0:
          key = new FillImpl();
          key.Pattern = ExcelPattern.Percent10;
          key.PatternColorObject.SetIndexed(ExcelKnownColors.BlackCustom);
          key.ColorObject.SetIndexed(ExcelKnownColors.None);
          break;
        default:
          key = new FillImpl(innerExtFormats[index2]);
          break;
      }
      if (dictionary.ContainsKey(key))
      {
        numArray[index2] = dictionary[key];
      }
      else
      {
        index1 = dictionary.Count;
        dictionary.Add(key, index1);
        if (index1 >= array.Length)
          Array.Resize<FillImpl>(ref array, index1 + 1);
        array[index1] = key;
        numArray[index2] = index1;
        if (index1 == 0 || index1 == 1)
          --index2;
      }
    }
    stream.Write(BitConverter.GetBytes(1243), 0, 2);
    stream.WriteByte((byte) 4);
    stream.Write(BitConverter.GetBytes(index1 + 1), 0, 4);
    for (int index3 = 0; index3 <= index1; ++index3)
      this.SerializeFill(stream, array[index3]);
    stream.Write(BitConverter.GetBytes(1244), 0, 2);
    stream.WriteByte((byte) 0);
    return numArray;
  }

  private void SerializeFill(MemoryStream stream, FillImpl fillImpl)
  {
    stream.Write(BitConverter.GetBytes(45), 0, 1);
    stream.WriteByte((byte) 68);
    stream.Write(BitConverter.GetBytes((int) fillImpl.Pattern), 0, 4);
    this.SerializeColor((Stream) stream, fillImpl.ColorObject);
    this.SerializeColor((Stream) stream, fillImpl.PatternColorObject);
    for (int index = 0; index <= 47; ++index)
      stream.WriteByte((byte) 0);
  }

  private void SerializeFonts(MemoryStream stream)
  {
    FontsCollection innerFonts = this.m_book.InnerFonts;
    int count = innerFonts.Count;
    stream.Write(BitConverter.GetBytes(1251), 0, 2);
    stream.WriteByte((byte) 4);
    stream.Write(BitConverter.GetBytes(count), 0, 4);
    for (int index = 0; index < count; ++index)
    {
      IFont font = innerFonts[index];
      this.SerializeFont(stream, font);
    }
    stream.Write(BitConverter.GetBytes(1252), 0, 2);
    stream.WriteByte((byte) 0);
  }

  private void SerializeNumberFormats(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    List<FormatRecord> usedFormats = this.m_book.InnerFormats.GetUsedFormats(ExcelVersion.Excel2007);
    int count = usedFormats.Count;
    if (count == 0)
      return;
    stream.Write(BitConverter.GetBytes(1255), 0, 2);
    stream.WriteByte((byte) 4);
    stream.Write(BitConverter.GetBytes(count), 0, 4);
    for (int index = 0; index < count; ++index)
      this.SerializeNumberFormat(stream, usedFormats[index]);
    stream.Write(BitConverter.GetBytes(1256), 0, 2);
    stream.WriteByte((byte) 0);
  }

  private void SerializeNumberFormat(Stream stream, FormatRecord formatRecord)
  {
    stream.Write(BitConverter.GetBytes(44), 0, 1);
    byte[] bytes = Encoding.Unicode.GetBytes(formatRecord.FormatString);
    stream.WriteByte((byte) (bytes.Length + 4 + 2));
    stream.Write(BitConverter.GetBytes(formatRecord.Index), 0, 2);
    stream.Write(BitConverter.GetBytes(bytes.Length / 2), 0, 4);
    stream.Write(bytes, 0, bytes.Length);
  }

  private void SerializeFont(MemoryStream stream, IFont font)
  {
    stream.Write(BitConverter.GetBytes(43), 0, 1);
    int num1 = 21;
    byte[] bytes = Encoding.Unicode.GetBytes(font.FontName);
    int num2 = num1 + bytes.Length + 4;
    stream.WriteByte((byte) num2);
    stream.Write(BitConverter.GetBytes((font as FontImpl).Record.FontHeight), 0, 2);
    FontRecord.FontAttributes fontAttributes = (FontRecord.FontAttributes) 0;
    if (font.Italic)
      fontAttributes |= FontRecord.FontAttributes.Italic;
    if (font.Strikethrough)
      fontAttributes |= FontRecord.FontAttributes.Strikeout;
    if (font.MacOSOutlineFont)
      fontAttributes |= FontRecord.FontAttributes.MacOutline;
    if (font.MacOSShadow)
      fontAttributes |= FontRecord.FontAttributes.MacShadow;
    stream.Write(BitConverter.GetBytes((int) fontAttributes), 0, 2);
    stream.Write(BitConverter.GetBytes((font as FontImpl).Record.BoldWeight), 0, 2);
    if (font.Superscript)
      stream.Write(BitConverter.GetBytes(1), 0, 2);
    else if (font.Subscript)
      stream.Write(BitConverter.GetBytes(2), 0, 2);
    else
      stream.Write(BitConverter.GetBytes(0), 0, 2);
    stream.WriteByte((byte) font.Underline);
    stream.WriteByte(((FontImpl) font).Family);
    stream.WriteByte(((FontImpl) font).CharSet);
    stream.WriteByte((byte) 0);
    this.SerializeColor((Stream) stream, (font as FontImpl).ColorObject);
    stream.WriteByte((byte) 0);
    stream.Write(BitConverter.GetBytes(bytes.Length / 2), 0, 4);
    stream.Write(bytes, 0, bytes.Length);
  }

  private void SaveSheets(Dictionary<int, int> xfIndexes, RelationCollection relations)
  {
    WorkbookObjectsCollection objects = this.m_book.Objects;
    int index = 0;
    for (int count = objects.Count; index < count; ++index)
    {
      WorksheetBaseImpl sheet = (WorksheetBaseImpl) objects[index];
      if (sheet != null)
      {
        string str = $"xl/worksheets/sheet{sheet.Index + 1}.bin";
        this.m_book.DataHolder.OverriddenContentTypes["/" + str] = "application/vnd.ms-excel.worksheet";
        this.m_book.DataHolder.Serializator.Worksheet = sheet as WorksheetImpl;
        this.UpdateArchiveItem((WorksheetImpl) sheet, str);
        this.SaveSheet((WorksheetImpl) sheet, str, xfIndexes, relations);
      }
    }
  }

  private void SaveSheet(
    WorksheetImpl sheet,
    string strItemName,
    Dictionary<int, int> xfIndexes,
    RelationCollection relations)
  {
    ZippedContentStream stream = new ZippedContentStream(new ZipArchive.CompressorCreator(sheet.AppImplementation.CreateCompressor));
    stream.Write(BitConverter.GetBytes(385), 0, 2);
    stream.WriteByte((byte) 0);
    stream.Write(BitConverter.GetBytes(997), 0, 2);
    stream.WriteByte((byte) 12);
    stream.Write(BitConverter.GetBytes(uint.MaxValue), 0, 4);
    stream.Write(BitConverter.GetBytes(sheet.DefaultColumnWidth > 0.0 ? (int) sheet.DefaultColumnWidth : 8), 0, 2);
    stream.Write(BitConverter.GetBytes(sheet.DefaultRowHeight), 0, 2);
    stream.Write(BitConverter.GetBytes(0), 0, 4);
    this.SerializeColumns((Stream) stream, sheet, xfIndexes);
    this.SerializeSheetData((Stream) stream, sheet, xfIndexes);
    stream.Write(BitConverter.GetBytes(386), 0, 2);
    stream.WriteByte((byte) 0);
    string itemName = strItemName;
    string type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
    string str = "xl/";
    if (strItemName.StartsWith(str))
      strItemName = strItemName.Substring(str.Length);
    string relationId = sheet.m_dataHolder.RelationId;
    if (relationId == null)
    {
      relationId = relations.GenerateRelationId();
      sheet.m_dataHolder.RelationId = relationId;
    }
    Relation relation = new Relation(strItemName, type);
    relations.RemoveRelationByTarget(strItemName);
    relations[relationId] = relation;
    this.m_Archive[itemName].Update(stream);
  }

  private void SerializeColumns(Stream stream, WorksheetImpl sheet, Dictionary<int, int> dicStyles)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (dicStyles == null)
      throw new ArgumentNullException(nameof (dicStyles));
    ColumnInfoRecord[] columnInformation = sheet.ColumnInformation;
    bool flag = true;
    double standardWidth = sheet.StandardWidth;
    int index = 1;
    for (int maxColumnCount = this.m_book.MaxColumnCount; index <= maxColumnCount; ++index)
    {
      ColumnInfoRecord columnInfo = columnInformation[index];
      if (columnInfo != null)
      {
        if (flag)
        {
          stream.Write(BitConverter.GetBytes(902), 0, 2);
          stream.WriteByte((byte) 0);
        }
        index = this.SerializeColumn(stream, columnInfo, dicStyles, standardWidth, sheet);
        flag = false;
      }
    }
    if (flag)
      return;
    stream.Write(BitConverter.GetBytes(903), 0, 2);
    stream.WriteByte((byte) 0);
  }

  private int SerializeColumn(
    Stream stream,
    ColumnInfoRecord columnInfo,
    Dictionary<int, int> dicStyles,
    double defaultWidth,
    WorksheetImpl sheet)
  {
    if (columnInfo == null)
      return int.MaxValue;
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (dicStyles == null)
      throw new ArgumentNullException(nameof (dicStyles));
    int sameColumns = Excel2007Serializator.FindSameColumns(sheet, (int) columnInfo.FirstColumn + 1, this.m_book);
    stream.Write(BitConverter.GetBytes(4668), 0, 2);
    stream.Write(BitConverter.GetBytes((int) columnInfo.FirstColumn), 0, 4);
    stream.Write(BitConverter.GetBytes(sameColumns - 1), 0, 4);
    double columnWidth = (double) columnInfo.ColumnWidth;
    if (columnWidth > (double) (sheet.MaxColumnWidth * 256 /*0x0100*/))
      stream.Write(BitConverter.GetBytes(sheet.MaxColumnWidth), 0, 4);
    else
      stream.Write(BitConverter.GetBytes(Convert.ToInt32(columnWidth + 256.0)), 0, 4);
    int extendedFormatIndex = (int) columnInfo.ExtendedFormatIndex;
    int num = 0;
    if (!dicStyles.TryGetValue(extendedFormatIndex, out num))
      num = extendedFormatIndex;
    stream.Write(BitConverter.GetBytes(num), 0, 4);
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    return sameColumns;
  }

  private void UpdateArchiveItem(WorksheetImpl sheet, string itemName)
  {
    if (sheet.m_dataHolder == null)
    {
      this.m_Archive.RemoveItem(itemName);
      ZipArchiveItem zipArchiveItem = this.m_Archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive);
      sheet.m_dataHolder = new WorksheetDataHolder(this.m_book.DataHolder, zipArchiveItem);
    }
    else
    {
      ZipArchiveItem archiveItem = sheet.m_dataHolder.ArchiveItem;
      if (archiveItem != null && !(archiveItem.ItemName != itemName))
        return;
      if (this.m_Archive.Find(itemName) >= 0)
        this.m_Archive.UpdateItem(itemName, (Stream) null, false, FileAttributes.Archive);
      else
        this.m_Archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive);
      sheet.m_dataHolder.ArchiveItem = this.m_Archive[itemName];
    }
  }

  private void SerializeSheetData(
    Stream stream,
    WorksheetImpl sheet,
    Dictionary<int, int> xfIndexes)
  {
    stream.Write(BitConverter.GetBytes(401), 0, 2);
    stream.WriteByte((byte) 0);
    CellRecordCollection cellRecords = sheet.CellRecords;
    int firstRow = cellRecords.FirstRow;
    for (int lastRow = cellRecords.LastRow; firstRow <= lastRow; ++firstRow)
    {
      if (cellRecords.ContainsRow(firstRow - 1))
      {
        RowStorage row = cellRecords.Table.Rows[firstRow - 1];
        this.SerializeRow(stream, row, cellRecords, firstRow, xfIndexes);
      }
    }
    stream.Write(BitConverter.GetBytes(402), 0, 2);
    stream.WriteByte((byte) 0);
  }

  private void SerializeRow(
    Stream stream,
    RowStorage row,
    CellRecordCollection cells,
    int rowIndex,
    Dictionary<int, int> xfIndexes)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    if (cells == null)
      throw new ArgumentNullException(nameof (cells));
    stream.Write(BitConverter.GetBytes(6400), 0, 2);
    stream.Write(BitConverter.GetBytes(rowIndex - 1), 0, 4);
    int key = (int) row.ExtendedFormatIndex;
    if (xfIndexes != null && xfIndexes.Count > 0)
    {
      if (xfIndexes.ContainsKey(key))
        key = xfIndexes[key];
      else
        --key;
    }
    stream.Write(BitConverter.GetBytes(key), 0, 4);
    stream.Write(BitConverter.GetBytes(row.Height), 0, 2);
    stream.WriteByte((byte) 0);
    byte num = this.WriteBit(this.WriteBit(this.WriteBit((byte) 0, row.IsHidden, 4), row.IsBadFontHeight, 5), row.IsFormatted, 6);
    stream.WriteByte(num);
    stream.WriteByte((byte) 0);
    stream.Write(BitConverter.GetBytes(1), 0, 4);
    stream.Write(BitConverter.GetBytes(row.FirstColumn > -1 ? row.FirstColumn : 0), 0, 4);
    stream.Write(BitConverter.GetBytes(row.LastColumn > -1 ? row.LastColumn : 0), 0, 4);
    RowStorageEnumerator enumerator = row.GetEnumerator(new RecordExtractor()) as RowStorageEnumerator;
    while (enumerator.MoveNext())
    {
      BiffRecordRaw current = enumerator.Current as BiffRecordRaw;
      this.SerializeCell(stream, current, enumerator, cells, xfIndexes);
    }
  }

  internal static bool ReadBit(byte btValue, int iBit)
  {
    if (iBit < 0 || iBit > 7)
      throw new ArgumentOutOfRangeException(nameof (iBit), "Bit Position cannot be less than 0 or greater than 7.");
    return ((int) btValue & 1 << iBit) == 1 << iBit;
  }

  internal static byte ReadBits(byte btValue, int startBit, int endBit)
  {
    if (startBit < 0 || endBit > 7)
      throw new ArgumentOutOfRangeException("iBit", "Bit Position cannot be less than 0 or greater than 7.");
    string str = "";
    for (; startBit <= endBit; ++startBit)
      str += (string) (object) (byte) (XlsbDataHolder.ReadBit(btValue, startBit) ? 1 : 0);
    return Convert.ToByte(str, 2);
  }

  internal byte WriteBits(byte actualValue, byte writeVal, int startBit, int endBit)
  {
    if (startBit < 0 || endBit > 7)
      throw new ArgumentOutOfRangeException("iBit", "Bit Position cannot be less than 0 or greater than 7.");
    for (; startBit <= endBit && writeVal != (byte) 0; ++startBit)
    {
      bool flag = (int) writeVal % 2 != 0;
      writeVal /= (byte) 2;
      actualValue = this.WriteBit(actualValue, flag, startBit);
    }
    return actualValue;
  }

  internal byte WriteBit(byte val, bool value, int bitPos)
  {
    if (bitPos < 0 || bitPos > 7)
      throw new ArgumentOutOfRangeException(nameof (bitPos), "Bit Position can be zero or greater than 7.");
    if (value)
      val |= (byte) (1 << bitPos);
    else
      val &= (byte) ~(1 << bitPos);
    return val;
  }

  private void SerializeCell(
    Stream stream,
    BiffRecordRaw record,
    RowStorageEnumerator rowStorageEnumerator,
    CellRecordCollection cells,
    Dictionary<int, int> xfIndexes)
  {
    ICellPositionFormat cellPositionFormat = record as ICellPositionFormat;
    int num = (int) cellPositionFormat.ExtendedFormatIndex;
    if (xfIndexes != null && xfIndexes.Count > 0)
    {
      if (xfIndexes.ContainsKey(num))
        num = xfIndexes[num];
      else
        --num;
    }
    string cellName = RangeImpl.GetCellName(cellPositionFormat.Column + 1, cellPositionFormat.Row + 1);
    double result;
    if (record.TypeCode == TBIFFRecord.Formula && cells.Sheet != null && cells.Sheet.CalcEngine != null && (record as FormulaRecord).Formula.Length == 1 && double.TryParse(cells.Sheet.CalcEngine.PullUpdatedValue(cellName), out result))
      (record as FormulaRecord).Value = result;
    if (record.TypeCode == TBIFFRecord.Formula)
    {
      Excel2007Serializator.CellType cellDataType = Excel2007Serializator.GetCellDataType(record, out string _);
      FormulaRecord formulaRecord = (FormulaRecord) record;
      ArrayRecord arrayRecord;
      if ((arrayRecord = rowStorageEnumerator.GetArrayRecord()) != null)
        this.SerializeArrayFormula(stream, arrayRecord);
      else
        this.SerializeSimpleFormula(stream, formulaRecord, cellDataType, rowStorageEnumerator, cellPositionFormat.Row, cellPositionFormat.Column, num);
    }
    else
    {
      switch (record.TypeCode)
      {
        case TBIFFRecord.LabelSST:
          stream.WriteByte((byte) 7);
          stream.WriteByte((byte) 12);
          stream.Write(BitConverter.GetBytes(cellPositionFormat.Column), 0, 4);
          stream.Write(BitConverter.GetBytes(Convert.ToInt32((byte) num)), 0, 3);
          stream.WriteByte((byte) 0);
          stream.Write(BitConverter.GetBytes((record as LabelSSTRecord).SSTIndex), 0, 4);
          break;
        case TBIFFRecord.Blank:
          stream.WriteByte((byte) 1);
          stream.WriteByte((byte) 8);
          stream.Write(BitConverter.GetBytes(cellPositionFormat.Column), 0, 4);
          stream.Write(BitConverter.GetBytes(Convert.ToInt32((byte) num)), 0, 3);
          stream.WriteByte((byte) 0);
          break;
        case TBIFFRecord.Number:
        case TBIFFRecord.RK:
          stream.WriteByte((byte) 5);
          stream.WriteByte((byte) 16 /*0x10*/);
          stream.Write(BitConverter.GetBytes(cellPositionFormat.Column), 0, 4);
          stream.Write(BitConverter.GetBytes(Convert.ToInt32((byte) num)), 0, 3);
          stream.WriteByte((byte) 0);
          stream.Write(BitConverter.GetBytes(((IDoubleValue) record).DoubleValue), 0, 8);
          break;
        case TBIFFRecord.BoolErr:
          BoolErrRecord boolErrRecord = (BoolErrRecord) record;
          if (!boolErrRecord.IsErrorCode)
          {
            stream.WriteByte((byte) 4);
            stream.WriteByte((byte) 9);
            stream.Write(BitConverter.GetBytes(cellPositionFormat.Column), 0, 4);
            if (!boolErrRecord.IsErrorCode)
            {
              if (boolErrRecord.BoolOrError == (byte) 1)
              {
                stream.Write(BitConverter.GetBytes(Convert.ToInt32((byte) num)), 0, 4);
                stream.WriteByte((byte) 1);
                break;
              }
              stream.Write(BitConverter.GetBytes(Convert.ToInt32((byte) num)), 0, 4);
              stream.WriteByte((byte) 0);
              break;
            }
            stream.Write(BitConverter.GetBytes(0), 0, 4);
            stream.WriteByte((byte) 0);
            break;
          }
          stream.WriteByte((byte) 3);
          stream.WriteByte((byte) 9);
          stream.Write(BitConverter.GetBytes(cellPositionFormat.Column), 0, 4);
          stream.Write(BitConverter.GetBytes(Convert.ToInt32((byte) num)), 0, 3);
          stream.WriteByte((byte) 0);
          stream.WriteByte(boolErrRecord.BoolOrError);
          break;
      }
    }
  }

  private void SerializeSimpleFormula(
    Stream stream,
    FormulaRecord formulaRecord,
    Excel2007Serializator.CellType cellType,
    RowStorageEnumerator rowStorageEnumerator,
    int row,
    int col,
    int xfIndex)
  {
    byte[] expression = formulaRecord.Expression;
    int num1 = 18 + expression.Length;
    if (cellType == Excel2007Serializator.CellType.str)
    {
      byte[] bytes = Encoding.Unicode.GetBytes(rowStorageEnumerator.GetFormulaStringValue());
      num1 += bytes.Length + 4;
    }
    switch (cellType)
    {
      case Excel2007Serializator.CellType.b:
        stream.WriteByte((byte) 10);
        int num2 = num1 + 1;
        stream.WriteByte((byte) num2);
        break;
      case Excel2007Serializator.CellType.e:
        stream.WriteByte((byte) 11);
        int num3 = num1 + 1;
        stream.WriteByte((byte) num3);
        break;
      case Excel2007Serializator.CellType.n:
        stream.WriteByte((byte) 9);
        int num4 = num1 + 8;
        stream.WriteByte((byte) num4);
        break;
      case Excel2007Serializator.CellType.str:
        stream.WriteByte((byte) 8);
        stream.WriteByte((byte) num1);
        break;
    }
    stream.Write(BitConverter.GetBytes(col), 0, 4);
    stream.Write(BitConverter.GetBytes(xfIndex), 0, 4);
    switch (cellType)
    {
      case Excel2007Serializator.CellType.b:
        stream.WriteByte(formulaRecord.BooleanValue ? (byte) 1 : (byte) 0);
        break;
      case Excel2007Serializator.CellType.e:
        stream.WriteByte(formulaRecord.ErrorValue);
        break;
      case Excel2007Serializator.CellType.n:
        if (!double.IsNaN(formulaRecord.DoubleValue))
        {
          stream.Write(BitConverter.GetBytes(formulaRecord.DoubleValue), 0, 8);
          break;
        }
        stream.Write(BitConverter.GetBytes(Convert.ToInt64(0)), 0, 8);
        break;
      case Excel2007Serializator.CellType.str:
        string formulaStringValue = rowStorageEnumerator.GetFormulaStringValue();
        byte[] bytes1 = Encoding.Unicode.GetBytes(formulaStringValue);
        stream.Write(BitConverter.GetBytes(formulaStringValue.Length), 0, 4);
        stream.Write(bytes1, 0, bytes1.Length);
        break;
    }
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
    stream.Write(BitConverter.GetBytes(expression.Length), 0, 4);
    stream.Write(expression, 0, expression.Length);
    stream.Write(BitConverter.GetBytes(0), 0, 4);
  }

  private void SerializeArrayFormula(Stream stream, ArrayRecord arrayRecord)
  {
  }

  private void SaveWorkbookPart()
  {
    MemoryStream newDataStream = new MemoryStream();
    newDataStream.Write(BitConverter.GetBytes(387), 0, 2);
    newDataStream.WriteByte((byte) 0);
    newDataStream.Write(BitConverter.GetBytes(399), 0, 2);
    newDataStream.WriteByte((byte) 0);
    foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.m_book.Worksheets)
    {
      WorksheetDataHolder dataHolder = ((WorksheetBaseImpl) worksheet).m_dataHolder;
      string sheetId = dataHolder?.SheetId;
      if (sheetId == null)
      {
        sheetId = this.GenerateSheetId();
        if (dataHolder != null)
          dataHolder.SheetId = sheetId;
      }
      newDataStream.Write(BitConverter.GetBytes(412), 0, 2);
      newDataStream.WriteByte((byte) (24 + Encoding.Unicode.GetBytes(worksheet.Name).Length));
      switch (worksheet.Visibility)
      {
        case WorksheetVisibility.Hidden:
          newDataStream.Write(BitConverter.GetBytes(1), 0, 4);
          break;
        case WorksheetVisibility.StrongHidden:
          newDataStream.Write(BitConverter.GetBytes(2), 0, 4);
          break;
        default:
          newDataStream.Write(BitConverter.GetBytes(0), 0, 4);
          break;
      }
      newDataStream.Write(BitConverter.GetBytes(int.Parse(sheetId)), 0, 4);
      byte[] bytes1 = Encoding.Unicode.GetBytes(((WorksheetBaseImpl) worksheet).DataHolder.RelationId);
      newDataStream.Write(BitConverter.GetBytes(bytes1.Length / 2), 0, 4);
      newDataStream.Write(bytes1, 0, bytes1.Length);
      byte[] bytes2 = Encoding.Unicode.GetBytes(worksheet.Name);
      newDataStream.Write(BitConverter.GetBytes(bytes2.Length / 2), 0, 4);
      newDataStream.Write(bytes2, 0, bytes2.Length);
    }
    newDataStream.Write(BitConverter.GetBytes(400), 0, 2);
    newDataStream.WriteByte((byte) 0);
    int refCount = (int) this.m_book.ExternSheet.RefCount;
    if (refCount > 0)
    {
      newDataStream.Write(BitConverter.GetBytes(737), 0, 2);
      newDataStream.WriteByte((byte) 0);
      newDataStream.Write(BitConverter.GetBytes(741), 0, 2);
      newDataStream.WriteByte((byte) 0);
      newDataStream.Write(BitConverter.GetBytes(746), 0, 2);
      int num = refCount * 12;
      newDataStream.WriteByte((byte) (num + 4));
      newDataStream.Write(BitConverter.GetBytes(refCount), 0, 4);
      for (int index = 0; index < refCount; ++index)
      {
        ExternSheetRecord.TREF tref = this.m_book.ExternSheet.Refs[index];
        newDataStream.Write(BitConverter.GetBytes((int) tref.SupBookIndex), 0, 4);
        newDataStream.Write(BitConverter.GetBytes((int) tref.FirstSheet), 0, 4);
        newDataStream.Write(BitConverter.GetBytes((int) tref.LastSheet), 0, 4);
      }
      newDataStream.Write(BitConverter.GetBytes(738), 0, 2);
      newDataStream.WriteByte((byte) 0);
    }
    this.SerializeNamedRanges((Stream) newDataStream);
    newDataStream.Write(BitConverter.GetBytes(388), 0, 2);
    newDataStream.WriteByte((byte) 0);
    string itemName = "xl/workbook.bin";
    if ("xl/workbook.bin"[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe("/xl/sharedStrings.bin");
    (this.m_Archive[itemName] ?? this.m_Archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive)).Update((Stream) newDataStream, false);
  }

  private void SerializeNamedRanges(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    WorkbookNamesCollection innerNamesColection = this.m_book.InnerNamesColection;
    if ((innerNamesColection != null ? innerNamesColection.Count : 0) <= 0)
      return;
    foreach (NameImpl name in (CollectionBase<IName>) innerNamesColection)
    {
      if (name != null && !name.Record.IsFunctionOrCommandMacro && !name.m_isTableNamedRange && !name.m_isFormulaNamedRange && !name.IsDeleted)
        this.SerializeNamedRange(stream, name);
    }
  }

  private void SerializeNamedRange(Stream stream, NameImpl name)
  {
    stream.WriteByte((byte) 39);
    long position1 = stream.Position;
    ++stream.Position;
    byte num = this.WriteBit((byte) 0, !name.Visible, 0);
    stream.WriteByte(num);
    stream.Position += 4L;
    if (name.IsLocal)
    {
      WorksheetImpl worksheet = name.Worksheet;
      if (worksheet != null)
      {
        int localSheetIndex = this.GetLocalSheetIndex(worksheet);
        stream.Write(BitConverter.GetBytes(localSheetIndex), 0, 4);
      }
      else
        stream.Write(BitConverter.GetBytes(uint.MaxValue), 0, 4);
    }
    else if (name != null && name.IsQueryTableRange)
      stream.Write(BitConverter.GetBytes(name.SheetIndex), 0, 4);
    else
      stream.Write(BitConverter.GetBytes(uint.MaxValue), 0, 4);
    byte[] bytes = Encoding.Unicode.GetBytes(name.Name);
    stream.Write(BitConverter.GetBytes(bytes.Length / 2), 0, 4);
    stream.Write(bytes, 0, bytes.Length);
    Ptg[] formulaTokens = name.NameRecord.FormulaTokens;
    int formulaLen = 0;
    byte[] byteArray = FormulaUtil.PtgArrayToByteArray(formulaTokens, out formulaLen, ExcelVersion.Excel2007);
    stream.Write(BitConverter.GetBytes(formulaLen), 0, 4);
    stream.Write(byteArray, 0, byteArray.Length);
    stream.Write(BitConverter.GetBytes(0), 0, 4);
    stream.Write(BitConverter.GetBytes(uint.MaxValue), 0, 4);
    long position2 = stream.Position;
    stream.Position = position1;
    stream.WriteByte((byte) ((ulong) (position2 - position1) - 1UL));
    stream.Position = position2;
  }

  private int GetLocalSheetIndex(WorksheetImpl sheet)
  {
    int num = -1;
    ITabSheets tabSheets = this.m_book.TabSheets;
    for (int index = 0; index < tabSheets.Count; ++index)
    {
      if (tabSheets[index] != null && tabSheets[index].Name == sheet.Name)
      {
        num = index;
        break;
      }
    }
    return num != -1 ? num : throw new ArgumentException("Invalid Sheet");
  }

  internal void Serialize(Stream filestream, WorkbookImpl book)
  {
    this.m_book = book;
    this.m_Archive = this.m_book.DataHolder.Archive;
    this.SaveDocument(filestream);
  }

  private string GenerateSheetId()
  {
    WorkbookObjectsCollection objects = this.m_book.Objects;
    int num = 0;
    int index = 0;
    for (int count = objects.Count; index < count; ++index)
    {
      WorksheetDataHolder dataHolder = ((WorksheetBaseImpl) objects[index]).DataHolder;
      if (dataHolder != null)
      {
        string sheetId = dataHolder.SheetId;
        int result;
        if (sheetId != null && int.TryParse(sheetId, out result) && result > num)
          num = result;
      }
    }
    return (num + 1).ToString();
  }
}
