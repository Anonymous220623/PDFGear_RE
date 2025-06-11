// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Clipboard.DelimiterClipboardProvider
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Clipboard;

public class DelimiterClipboardProvider : ClipboardProvider
{
  public const string DEF_DEFAULT_FORMAT_NAME = "UnicodeText";
  public const string DEF_DEFAULT_COLUMN_DELIMITER = "\t";
  public const string DEF_DEFAULT_ROW_DELIMITER = "\r\n";
  private StringBuilder m_strCSVValue = new StringBuilder();
  private string m_strColDelim = "\t";
  private string m_strRowDelim = "\r\n";

  public DelimiterClipboardProvider()
    : this((IWorksheet) null)
  {
  }

  public DelimiterClipboardProvider(IWorksheet sheet)
    : this(sheet, (ClipboardProvider) null)
  {
  }

  public DelimiterClipboardProvider(IWorksheet sheet, ClipboardProvider next)
    : base(sheet, next)
  {
    this.FormatName = "UnicodeText";
  }

  public override IDataObject GetForClipboard()
  {
    this.FillData();
    return (IDataObject) new DataObject(this.FormatName, (object) this.m_strCSVValue.ToString());
  }

  public override IDataObject GetForClipboard(IRange range)
  {
    this.FillData(range);
    return (IDataObject) new DataObject(this.FormatName, (object) this.m_strCSVValue.ToString());
  }

  protected virtual void FillData() => this.FillData(this.Worksheet.UsedRange);

  protected virtual void FillData(IRange range)
  {
    this.m_strCSVValue.Length = 0;
    int column = range.Column;
    int lastColumn = range.LastColumn;
    int lastRow = range.LastRow;
    CellRecordCollection cellRecords = ((WorksheetImpl) this.Worksheet).CellRecords;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Worksheet.Application, this.Worksheet);
    for (int row = range.Row; row <= lastRow; ++row)
    {
      for (int iColumn = column; iColumn <= lastColumn; ++iColumn)
      {
        if (cellRecords.Contains(row, iColumn))
        {
          migrantRangeImpl.ResetRowColumn(row, iColumn);
          this.m_strCSVValue.Append(migrantRangeImpl.Value);
        }
        if (iColumn != lastColumn)
          this.m_strCSVValue.Append(this.ColumnDelimiter);
      }
      this.m_strCSVValue.Append(this.RowDelimiter);
    }
  }

  protected override IWorkbook ExtractWorkbook(IDataObject dataObject, IWorkbooks workbooks)
  {
    if (dataObject != null && dataObject.GetDataPresent(this.FormatName))
    {
      object data = dataObject.GetData(this.FormatName);
      switch (data)
      {
        case string _:
          return this.GetBookFromString((string) data, workbooks);
        case MemoryStream _:
          return this.GetBookFromStream((MemoryStream) data, workbooks);
      }
    }
    return (IWorkbook) null;
  }

  protected override void FillDataObject(IDataObject dataObject)
  {
    this.FillData();
    dataObject.SetData(this.FormatName, (object) this.m_strCSVValue.ToString());
  }

  protected override void FillDataObject(IDataObject dataObject, IRange range)
  {
    this.FillData(range);
    dataObject.SetData(this.FormatName, (object) this.m_strCSVValue.ToString());
  }

  private IWorksheet GetSheetFromString(string sheetData, IApplication application, object parent)
  {
    IWorksheet sheet = (IWorksheet) new WorksheetImpl(application, parent);
    this.FillSheet(sheet, sheetData);
    return sheet;
  }

  private IWorkbook GetBookFromString(string stringData, IWorkbooks workbooks)
  {
    switch (stringData)
    {
      case null:
        throw new ArgumentNullException(nameof (stringData));
      case "":
        throw new ArgumentException("stringData - string cannot be empty");
      default:
        IWorkbook bookFromString = workbooks != null ? workbooks.Create(1) : throw new ArgumentNullException(nameof (workbooks));
        this.FillSheet(bookFromString.Worksheets[0], stringData);
        return bookFromString;
    }
  }

  private IWorkbook GetBookFromStream(MemoryStream streamData, IWorkbooks workbooks)
  {
    if (streamData == null)
      throw new ArgumentNullException(nameof (streamData));
    if (workbooks == null)
      throw new ArgumentNullException(nameof (workbooks));
    return this.GetBookFromString(Encoding.Unicode.GetString(streamData.ToArray()), workbooks);
  }

  private void FillSheet(IWorksheet sheet, string sheetData)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    int num1 = 0;
    int iColumn = 1;
    int iRow = 1;
    int startIndex = 0;
    int num2 = sheetData.Length - this.ColumnDelimiter.Length;
    int num3 = sheetData.Length - this.RowDelimiter.Length;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(sheet.Application, sheet);
    while (num1 < sheetData.Length)
    {
      if (sheetData[num1] == '"' || sheetData[num1] == '\'')
        num1 = this.SkipString(sheetData, num1);
      else if (num1 <= num2 && sheetData.Substring(num1, this.ColumnDelimiter.Length) == this.ColumnDelimiter)
      {
        migrantRangeImpl.ResetRowColumn(iRow, iColumn);
        migrantRangeImpl.Value = sheetData.Substring(startIndex, num1 - startIndex);
        startIndex = num1 + this.ColumnDelimiter.Length;
        num1 = startIndex;
        ++iColumn;
      }
      else if (num1 <= num3 && sheetData.Substring(num1, this.RowDelimiter.Length) == this.RowDelimiter)
      {
        migrantRangeImpl.ResetRowColumn(iRow, iColumn);
        migrantRangeImpl.Value = sheetData.Substring(startIndex, num1 - startIndex);
        startIndex = num1 + this.RowDelimiter.Length;
        num1 = startIndex;
        ++iRow;
        iColumn = 1;
      }
      else
        ++num1;
    }
  }

  private int SkipString(string sheetData, int pos)
  {
    return sheetData.IndexOf(sheetData[pos], pos + 1) + 1;
  }

  public string ColumnDelimiter
  {
    get => this.m_strColDelim;
    set => this.m_strColDelim = value;
  }

  public string RowDelimiter
  {
    get => this.m_strRowDelim;
    set => this.m_strRowDelim = value;
  }
}
