// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.RangeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.FormatParser;
using Syncfusion.XlsIO.FormatParser.FormatTokens;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class RangeImpl : 
  IReparse,
  ICombinedRange,
  IRange,
  IParentApplication,
  IEnumerable<IRange>,
  IEnumerable,
  ICellPositionFormat,
  INativePTG,
  IDisposable
{
  public const string DEF_DATE_FORMAT = "mm/dd/yyyy";
  public const string DEF_TIME_FORMAT = "h:mm:ss";
  public const string DEF_UK_DATETIME_FORMAT = "dd/MM/yyyy HH:mm";
  public const int DEF_DATETIME_INDEX = 22;
  private const double DEF_OLE_DOUBLE = 2958465.9999999884;
  private const double DEF_MAX_DOUBLE = 2958466.0;
  public const string DEF_NUMBER_FORMAT = "0.00";
  public const string DEF_TEXT_FORMAT = "@";
  public const string DEF_GENERAL_FORMAT = "General";
  internal const string DEF_FORMULAARRAY_FORMAT = "{{{0}}}";
  private const string DEF_SINGLECELL_ERROR = "This method should be called for single cells only.";
  public const string DEF_DEFAULT_STYLE = "Normal";
  internal const int DEF_NORMAL_STYLE_INDEX = 15;
  private const bool DEF_WRAPTEXT_VALUE = false;
  private const string DEF_EMPTY_DIGIT = " ";
  private const string DEF_OPEN_BRACE = "(";
  internal const string DEF_EQUIVALENT = "=";
  internal const string DEF_AMPERSAND = "&";
  private const char DEF_CELL_NAME_SEPARATER = '$';
  private const char DEF_R1C1_COLUMN = 'C';
  private const char DEF_R1C1_ROW = 'R';
  private const char DEF_R1C1_OPENBRACKET = '[';
  private const char DEF_R1C1_CLOSEBRACKET = ']';
  private const string DEF_R1C1_FORMAT = "R{0}C{1}";
  private const long DEF_MIN_OADATE = 31241376000000000;
  private const int DEF_AUTOFORMAT_NUMBER_INDEX = 0;
  private const int DEF_AUTOFORMAT_NUMBER_INDEX_1 = 43;
  private const int DEF_AUTOFORMAT_NUMBER_INDEX_2 = 44;
  private const int ColumnBitsInCellIndex = 32 /*0x20*/;
  private const int ArrayFormulaXFFlag = -1;
  private const int FormulaLengthXls = 1024 /*0x0400*/;
  private const int FormulaLengthXlsX = 8192 /*0x2000*/;
  internal const char SingleQuote = '\'';
  private const string NEW_LINE = "\n";
  private const string DEF_PERCENTAGE_FORMAT = "0%";
  private const string DEF_DECIMAL_PERCENTAGE_FORMAT = "0.00%";
  private const string DEF_EXPONENTIAL_FORMAT = "0.00E+00";
  private const string DEF_CULTUREINFO_TIMETOKEN = "tt";
  private const string DEF_TIMETOKEN_FORMAT = "AM/PM";
  protected const RegexOptions DEF_OPTIONS = RegexOptions.Compiled;
  internal const string UKCultureName = "cy-GB";
  private static readonly RangeImpl.TCellType[] DEF_DATETIMECELLTYPES = new RangeImpl.TCellType[3]
  {
    RangeImpl.TCellType.RK,
    RangeImpl.TCellType.Number,
    RangeImpl.TCellType.Formula
  };
  private static readonly ExcelAutoFormat[] DEF_AUTOFORMAT_RIGHT = new ExcelAutoFormat[7]
  {
    ExcelAutoFormat.Classic_2,
    ExcelAutoFormat.Classic_3,
    ExcelAutoFormat.Accounting_1,
    ExcelAutoFormat.Accounting_2,
    ExcelAutoFormat.Accounting_3,
    ExcelAutoFormat.Colorful_2,
    ExcelAutoFormat.Colorful_3
  };
  private static readonly ExcelAutoFormat[] DEF_AUTOFORMAT_NUMBER = new ExcelAutoFormat[4]
  {
    ExcelAutoFormat.Accounting_1,
    ExcelAutoFormat.Accounting_2,
    ExcelAutoFormat.Accounting_3,
    ExcelAutoFormat.Accounting_4
  };
  internal static readonly DateTime DEF_MIN_DATETIME = new DateTime(1900, 1, 1, 0, 0, 0, 0);
  private static readonly long MinAllowedDateTicks = new DateTime(1900, 1, 1, 0, 0, 0, 0).Ticks;
  private static readonly ExcelLineStyle[] ThinBorders = new ExcelLineStyle[3]
  {
    ExcelLineStyle.None,
    ExcelLineStyle.Hair,
    ExcelLineStyle.Thin
  };
  private string[] DEF_DATETIME_FORMULA = new string[4]
  {
    "TIME",
    "DATE",
    "TODAY",
    "NOW"
  };
  private static string[] DEF_DATE_SEPARATOR = new string[4]
  {
    "/",
    "-",
    ". ",
    "."
  };
  private static readonly Regex MonthRegex = new Regex("[Mm]{3,}", RegexOptions.Compiled);
  private static readonly Regex DayRegex = new Regex("[Dd]+", RegexOptions.Compiled);
  private static readonly Regex YearRegex = new Regex("[yY]+", RegexOptions.Compiled);
  private HtmlStringParser m_htmlStringParser;
  private WorksheetImpl m_worksheet;
  private WorkbookImpl m_book;
  protected int m_iLeftColumn;
  protected int m_iRightColumn;
  protected int m_iTopRow;
  protected int m_iBottomRow;
  private bool m_bIsNumReference;
  private bool m_bIsMultiReference;
  private bool m_bIsStringReference;
  private List<IRange> m_cells;
  protected Syncfusion.XlsIO.Implementation.CellStyle m_style;
  private bool m_bCells;
  protected DataValidationWrapper m_dataValidation;
  [ThreadStatic]
  private static string m_dateSeperator = (string) null;
  [ThreadStatic]
  private static string m_timeSeparator = (string) null;
  protected IRTFWrapper m_rtfString;
  private char[] unnecessaryChar = new char[3]
  {
    '_',
    '?',
    '*'
  };
  private string[] osCultureSpecficFormats = new string[3]
  {
    "m/d/yyyy",
    "m/d/yy h:mm",
    "m/d/yyyy\\ h:mm"
  };
  private string[] floatNumberStyleCultures = new string[5]
  {
    "de-AT",
    "de-DE",
    "de-CH",
    "de-LI",
    "de-LU"
  };
  private bool m_isEntireRow;
  private bool m_isEntireColumn;
  private bool m_hasDefaultFormat = true;
  internal bool updateCellValue = true;
  private OutlineWrapperUtility m_outlineWrapperUtility;
  private int m_noOfSubtotals;
  private bool m_bIsAbsolute;
  private StringFormat _stringformat;
  internal bool m_bAutofitText;

  public string HtmlString
  {
    get
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.m_worksheet.Application, (IWorksheet) this.m_worksheet);
      migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
      string htmlString = (migrantRangeImpl.RichText as RichTextString).PrepareHtml(this.CellStyle.Font, (IRange) migrantRangeImpl);
      for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
      {
        for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
          string str = (migrantRangeImpl.RichText as RichTextString).PrepareHtml(migrantRangeImpl.CellStyle.Font, (IRange) migrantRangeImpl);
          if (htmlString != str)
            return (string) null;
        }
      }
      return htmlString;
    }
    set
    {
      if (this.m_htmlStringParser == null)
        this.m_htmlStringParser = new HtmlStringParser();
      this.m_htmlStringParser.ParseHtml(value, this.RichText);
    }
  }

  internal string[] DefaultStyleNames => this.m_book.AppImplementation.DefaultStyleNames;

  public string Address
  {
    get
    {
      this.CheckDisposed();
      return $"{this.m_worksheet.QuotedName}!{this.AddressLocal}";
    }
  }

  public string AddressLocal
  {
    get
    {
      this.CheckDisposed();
      return RangeImpl.GetAddressLocal(this.FirstRow, this.FirstColumn, this.LastRow, this.LastColumn);
    }
  }

  public string AddressR1C1
  {
    get
    {
      this.CheckDisposed();
      return $"{this.m_worksheet.QuotedName}!{this.AddressR1C1Local}";
    }
  }

  public string AddressR1C1Local
  {
    get
    {
      this.CheckDisposed();
      string addressR1C1Local = $"R{this.Row}C{this.Column}";
      if (!this.IsSingleCell)
        addressR1C1Local = $"{addressR1C1Local}:{$"R{this.LastRow}C{this.LastColumn}"}";
      return addressR1C1Local;
    }
  }

  public bool Boolean
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (!this.m_worksheet.GetBoolean(row, column))
            return false;
        }
      }
      return true;
    }
    set
    {
      this.CheckDisposed();
      this.TryRemoveFormulaArrays();
      if (this.IsSingleCell)
      {
        if (this.Boolean != value)
          this.OnCellValueChanged((object) this.Boolean, (object) value, (IRange) this);
        this.SetBoolean(value);
        this.SetChanged();
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.Boolean = value;
          }
        }
      }
    }
  }

  public IBorders Borders
  {
    get
    {
      this.CheckDisposed();
      return this.CellStyle.Borders;
    }
  }

  public IRange[] Cells
  {
    get
    {
      this.CheckDisposed();
      if (this.m_cells == null && !this.m_bCells)
        this.InfillCells();
      return this.m_cells != null ? this.m_cells.ToArray() : throw new ArgumentNullException();
    }
  }

  public int Column
  {
    get
    {
      this.CheckDisposed();
      return this.FirstColumn;
    }
  }

  public int ColumnGroupLevel
  {
    get
    {
      this.CheckDisposed();
      int firstColumn = this.FirstColumn;
      int lastColumn = this.LastColumn;
      int columnGroupLevel;
      if (firstColumn == lastColumn)
      {
        ColumnInfoRecord columnInfoRecord = this.m_worksheet.ColumnInformation[firstColumn];
        columnGroupLevel = columnInfoRecord != null ? (int) columnInfoRecord.OutlineLevel : 0;
      }
      else
      {
        int firstRow = this.FirstRow;
        columnGroupLevel = this.m_worksheet[firstRow, firstColumn].ColumnGroupLevel;
        for (int column = firstColumn + 1; column <= lastColumn; ++column)
        {
          if (columnGroupLevel != this.m_worksheet[firstRow, column].ColumnGroupLevel)
            return -1;
        }
      }
      return columnGroupLevel;
    }
  }

  public double ColumnWidth
  {
    get
    {
      this.CheckDisposed();
      double columnWidth;
      if (this.m_iLeftColumn == this.m_iRightColumn)
      {
        columnWidth = this.m_worksheet.InnerGetColumnWidth(this.m_iLeftColumn);
      }
      else
      {
        columnWidth = this.m_worksheet.InnerGetColumnWidth(this.m_iLeftColumn);
        for (int iColumn = this.m_iLeftColumn + 1; iColumn <= this.m_iRightColumn; ++iColumn)
        {
          if (columnWidth != this.m_worksheet.InnerGetColumnWidth(iColumn))
          {
            columnWidth = double.MinValue;
            break;
          }
        }
      }
      return columnWidth;
    }
    set
    {
      this.CheckDisposed();
      if (value < 0.0 || value > (double) byte.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (ColumnWidth), "Column Width cannot be larger then 255 or zeroless");
      int firstColumn = this.FirstColumn;
      for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
        this.m_worksheet.SetColumnWidth(firstColumn, value);
    }
  }

  public int Count
  {
    get
    {
      this.CheckDisposed();
      return (this.LastColumn - this.FirstColumn + 1) * (this.LastRow - this.FirstRow + 1);
    }
  }

  public bool HasDataValidation
  {
    get
    {
      bool hasDataValidation;
      if (this.IsSingleCell)
      {
        hasDataValidation = this.FindDataValidation() != null;
      }
      else
      {
        hasDataValidation = true;
        IMigrantRange migrantRange = (IMigrantRange) new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int iTopRow = this.m_iTopRow; iTopRow <= this.m_iBottomRow; ++iTopRow)
        {
          for (int iLeftColumn = this.m_iLeftColumn; iLeftColumn <= this.m_iRightColumn; ++iLeftColumn)
          {
            migrantRange.ResetRowColumn(iTopRow, iLeftColumn);
            if (!migrantRange.HasDataValidation)
              hasDataValidation = false;
          }
        }
      }
      return hasDataValidation;
    }
  }

  public bool HasConditionFormats
  {
    get
    {
      bool conditionFormats;
      if (this.IsSingleCell)
      {
        conditionFormats = this.m_worksheet.ConditionalFormats.Find(this.GetRectangles()) != null;
      }
      else
      {
        conditionFormats = true;
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int iTopRow = this.m_iTopRow; iTopRow <= this.m_iBottomRow; ++iTopRow)
        {
          for (int iLeftColumn = this.m_iLeftColumn; iLeftColumn <= this.m_iRightColumn; ++iLeftColumn)
          {
            migrantRangeImpl.ResetRowColumn(iTopRow, iLeftColumn);
            if (!migrantRangeImpl.HasConditionFormats)
              conditionFormats = false;
          }
        }
      }
      return conditionFormats;
    }
  }

  public DateTime DateTime
  {
    get
    {
      this.CheckDisposed();
      double dNumber = this.m_worksheet.GetCellType(this.Row, this.Column, true) != (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula) ? this.m_worksheet.GetNumber(this.Row, this.Column) : this.m_worksheet.GetFormulaNumberValue(this.Row, this.Column);
      string text = this.m_worksheet.GetText(this.Row, this.Column);
      if (dNumber < 0.0 || this.InnerNumberFormat.GetFormatType(dNumber) != ExcelFormatType.DateTime || !string.IsNullOrEmpty(text) && DateTime.TryParse(text, out DateTime _))
        return Convert.ToDateTime(text);
      if (dNumber == double.NaN || this.InnerNumberFormat.GetFormatType(dNumber) != ExcelFormatType.DateTime || dNumber > DateTime.MaxValue.ToOADate() || dNumber < DateTime.MinValue.ToOADate())
        return DateTime.MinValue;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double num = this.m_worksheet.GetCellType(row, column, true) != (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula) ? this.m_worksheet.GetNumber(row, column) : this.m_worksheet.GetFormulaNumberValue(row, column);
          if (num == double.NaN || dNumber != num || this.InnerNumberFormat.GetFormatType(num) != ExcelFormatType.DateTime)
            return DateTime.MinValue;
        }
      }
      return UtilityMethods.ConvertNumberToDateTime(dNumber, this.m_book.Date1904);
    }
    set
    {
      this.CheckDisposed();
      if (this.m_book.Date1904)
        value = DateTime.FromOADate(value.ToOADate() - 1462.0);
      if (this.IsSingleCell)
      {
        this.FormatType = ExcelFormatType.DateTime;
        DateTime dateTime = this.DateTime;
        int num = dateTime != value ? 1 : 0;
        this.OnCellValueChanged((object) dateTime, (object) value, (IRange) this);
        this.SetDateTime(value);
        this.SetChanged();
      }
      else
      {
        this.TryRemoveFormulaArrays();
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.DateTime = value;
          }
        }
      }
    }
  }

  public string DisplayText => this.GetDisplayText(this.Row, this.Column);

  private string GetCultureFormat(string result, double dNumber, FormatImpl numberFormat)
  {
    if (numberFormat.FormatType == ExcelFormatType.DateTime && this.CheckOSSpecificDateFormats(numberFormat) && result != string.Empty && dNumber < CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime.ToOADate() && dNumber >= 0.0)
    {
      CultureInfo cultureInfo = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
      DateTime dt = DateTime.FromOADate(dNumber);
      result = !this.HasFormulaErrorValue ? (numberFormat.IsTimeFormat(dNumber) ? dt.ToLongTimeString() : (numberFormat.IsDateFormat(dNumber) ? dt.ToString("d", (IFormatProvider) cultureInfo) : dt.ToString((IFormatProvider) cultureInfo))) : this.FormulaErrorValue;
      if (numberFormat.Index == 22)
        result = this.GetCultureDateTime(cultureInfo, dt);
    }
    return result;
  }

  private string GetCultureDateTime(CultureInfo culture, DateTime dt)
  {
    switch (culture.Name)
    {
      case "en-GB":
        return dt.ToString("dd/MM/yyyy HH:mm", (IFormatProvider) culture);
      default:
        return dt.ToString((IFormatProvider) culture);
    }
  }

  private bool CheckOSSpecificDateFormats(FormatImpl InnerNumberFormat)
  {
    if (InnerNumberFormat == null)
      throw new ArgumentNullException(nameof (InnerNumberFormat));
    return Array.IndexOf<string>(this.osCultureSpecficFormats, InnerNumberFormat.FormatString) >= 0 && Thread.CurrentThread.CurrentCulture.Name != "en-US";
  }

  public IRange End
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? (IRange) this : this.m_worksheet.InnerGetCell(this.LastColumn, this.LastRow);
    }
  }

  public bool IsEntireRow
  {
    get
    {
      if (this.m_isEntireRow)
        return true;
      return this.FirstColumn == 1 && this.LastColumn == this.Workbook.MaxColumnCount;
    }
    set => this.m_isEntireRow = value;
  }

  public bool IsEntireColumn
  {
    get => this.m_isEntireColumn;
    set => this.m_isEntireColumn = value;
  }

  public IRange EntireColumn
  {
    get
    {
      this.CheckDisposed();
      RangeImpl entireColumn = this.Worksheet[1, this.FirstColumn, this.m_book.MaxRowCount, this.LastColumn] as RangeImpl;
      entireColumn.IsEntireColumn = true;
      return (IRange) entireColumn;
    }
  }

  public IRange EntireRow
  {
    get
    {
      this.CheckDisposed();
      RangeImpl entireRow = this.Worksheet[this.FirstRow, 1, this.LastRow, this.m_book.MaxColumnCount] as RangeImpl;
      entireRow.IsEntireRow = true;
      return (IRange) entireRow;
    }
  }

  public string Error
  {
    get
    {
      this.CheckDisposed();
      string error = this.m_worksheet.GetError(this.Row, this.Column);
      if (error == null)
        return (string) null;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (error != this.m_worksheet.GetError(row, column))
            return (string) null;
        }
      }
      return error;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.SetError(value);
        this.SetChanged();
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.Error = value;
          }
        }
      }
    }
  }

  public string Formula
  {
    get
    {
      this.CheckDisposed();
      string formula1 = this.GetFormula(this.Row, this.Column);
      if (!this.IsSingleCell && formula1 != null)
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            string formula2 = this.GetFormula(row, column);
            if (formula1 != formula2)
            {
              formula1 = (string) null;
              break;
            }
          }
        }
      }
      return formula1;
    }
    set
    {
      if (value == null || !(value != string.Empty))
        return;
      if (this.Workbook.Version == ExcelVersion.Excel97to2003 && value.Length > 1024 /*0x0400*/)
        throw new ArgumentException("The formula is too long. Length should not be longer than 1024");
      if (value.Length > 8192 /*0x2000*/)
        throw new ArgumentException("The formula is too long. Length should not be longer than 8192");
      this.CheckDisposed();
      this.TryRemoveFormulaArrays();
      if (value[0] != '=')
        value = '='.ToString() + value;
      this.Value = value;
      if (this.Formula != null && this.HasFormulaDateTime)
        this.FormatType = ExcelFormatType.DateTime;
      if (!(this.Worksheet as WorksheetImpl).FormulaValues.ContainsKey(RangeImpl.GetCellIndex(this.Column, this.Row)))
        return;
      (this.Worksheet as WorksheetImpl).FormulaValues.Remove(RangeImpl.GetCellIndex(this.Column, this.Row));
    }
  }

  public string FormulaArray
  {
    get
    {
      this.CheckDisposed();
      string formulaArray = this.GetFormulaArray(false);
      if (formulaArray != null && formulaArray.Trim().StartsWith("=_xlfn."))
        formulaArray = formulaArray.Trim().Replace("_xlfn.", "");
      return formulaArray;
    }
    set
    {
      this.CheckDisposed();
      this.SetFormulaArray(value, false);
    }
  }

  public string FormulaStringValue
  {
    get
    {
      this.CheckDisposed();
      RangeImpl.UpdateCellValue(this.Parent, this.Column, this.Row, this.updateCellValue);
      string formulaStringValue = this.m_worksheet.GetFormulaStringValue(this.Row, this.Column);
      if (!this.IsSingleCell && formulaStringValue != null)
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            if (formulaStringValue != this.m_worksheet.GetFormulaStringValue(row, column))
              return (string) null;
          }
        }
      }
      return formulaStringValue;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.m_worksheet.CellRecords.SetStringValue(this.CellIndex, value);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.FormulaStringValue = value;
          }
        }
      }
    }
  }

  public double FormulaNumberValue
  {
    get
    {
      this.CheckDisposed();
      double formulaNumberValue1 = this.m_worksheet.GetFormulaNumberValue(this.Row, this.Column);
      if (!this.IsSingleCell && !double.IsNaN(formulaNumberValue1))
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            double formulaNumberValue2 = this.m_worksheet.GetFormulaNumberValue(row, column);
            if (formulaNumberValue1 != formulaNumberValue2)
              return double.NaN;
          }
        }
      }
      return formulaNumberValue1;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        if (!(this.Record is FormulaRecord record))
          throw new NotSupportedException("This property is only for formula ranges");
        record.Value = value;
        this.Record = (BiffRecordRaw) record;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.FormulaNumberValue = value;
          }
        }
      }
    }
  }

  public bool FormulaBoolValue
  {
    get
    {
      this.CheckDisposed();
      RangeImpl.UpdateCellValue(this.Parent, this.Column, this.Row, this.updateCellValue);
      bool formulaBoolValue = this.m_worksheet.GetFormulaBoolValue(this.Row, this.Column);
      if (!this.IsSingleCell && formulaBoolValue)
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            if (formulaBoolValue != this.m_worksheet.GetFormulaBoolValue(row, column))
              return false;
          }
        }
      }
      return formulaBoolValue;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        if (!(this.Record is FormulaRecord record))
          throw new NotSupportedException("This property is only for formula ranges");
        record.BooleanValue = value;
        this.Record = (BiffRecordRaw) record;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.FormulaBoolValue = value;
          }
        }
      }
    }
  }

  public string FormulaErrorValue
  {
    get
    {
      this.CheckDisposed();
      string formulaErrorValue = this.m_worksheet.GetFormulaErrorValue(this.Row, this.Column);
      if (!this.IsSingleCell && formulaErrorValue != null)
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            if (formulaErrorValue != this.m_worksheet.GetFormulaErrorValue(row, column))
              return (string) null;
          }
        }
      }
      return formulaErrorValue;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        if (!(this.Record is FormulaRecord record))
          throw new NotSupportedException("This property is only for formula ranges");
        int num = this.GetErrorCodeByString(value);
        if (num == -1)
          num = 0;
        record.ErrorValue = (byte) num;
        this.Record = (BiffRecordRaw) record;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.FormulaErrorValue = value;
          }
        }
      }
    }
  }

  public object FormulaValue
  {
    get
    {
      object formulaValue;
      if (this.HasFormula)
      {
        string formulaStringValue = this.FormulaStringValue;
        formulaValue = formulaStringValue == null ? (!this.HasFormulaDateTime ? (!this.HasFormulaBoolValue ? (!this.HasFormulaErrorValue ? (object) this.FormulaNumberValue : (object) this.FormulaErrorValue) : (object) this.FormulaBoolValue) : (object) this.FormulaDateTime) : (object) formulaStringValue;
      }
      else
        formulaValue = (object) null;
      return formulaValue;
    }
  }

  public bool FormulaHidden
  {
    get
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
        return this.CellStyle.FormulaHidden;
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      bool formulaHidden = this.m_worksheet[this.FirstRow, this.FirstColumn].FormulaHidden;
      if (formulaHidden)
      {
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow && formulaHidden; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn && formulaHidden; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            formulaHidden = migrantRangeImpl.FormulaHidden;
          }
        }
      }
      return formulaHidden;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.CellStyle.FormulaHidden = value;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.FormulaHidden = value;
          }
        }
      }
    }
  }

  public DateTime FormulaDateTime
  {
    get
    {
      this.CheckDisposed();
      double formulaNumberValue1 = this.m_worksheet.GetFormulaNumberValue(this.Row, this.Column);
      if (formulaNumberValue1 == double.NaN || this.InnerNumberFormat.GetFormatType(formulaNumberValue1) != ExcelFormatType.DateTime)
        return DateTime.MinValue;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double formulaNumberValue2 = this.m_worksheet.GetFormulaNumberValue(row, column);
          if (formulaNumberValue2 == double.NaN || formulaNumberValue1 != formulaNumberValue2 || this.InnerNumberFormat.GetFormatType(formulaNumberValue2) != ExcelFormatType.DateTime)
            return DateTime.MinValue;
        }
      }
      return UtilityMethods.ConvertNumberToDateTime(formulaNumberValue1, this.m_book.Date1904);
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        if (this.CellType != RangeImpl.TCellType.Formula)
          throw new NotSupportedException("This property is only for formula ranges");
        this.FormatType = ExcelFormatType.DateTime;
        this.m_worksheet.SetFormulaNumberValue(this.Row, this.Column, value.ToOADate());
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.FormulaDateTime = value;
          }
        }
      }
    }
  }

  public string FormulaR1C1
  {
    get
    {
      this.CheckDisposed();
      string formulaR1C1_1;
      if (this.IsSingleCell)
      {
        formulaR1C1_1 = this.HasFormulaArray ? this.GetFormulaArray(true) : this.m_worksheet.GetFormula(this.Row, this.Column, true);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
        formulaR1C1_1 = migrantRangeImpl.FormulaR1C1;
        if (formulaR1C1_1 != null)
        {
          int row = this.Row;
          for (int lastRow = this.LastRow; row <= lastRow; ++row)
          {
            int column = this.Column;
            for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
            {
              migrantRangeImpl.ResetRowColumn(row, column);
              string formulaR1C1_2 = migrantRangeImpl.FormulaR1C1;
              if (formulaR1C1_1 != formulaR1C1_2)
              {
                formulaR1C1_1 = (string) null;
                break;
              }
            }
          }
        }
      }
      if (formulaR1C1_1 != null && formulaR1C1_1.Contains("_xlfn."))
        formulaR1C1_1 = formulaR1C1_1.Replace("_xlfn.", "");
      return formulaR1C1_1;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value[0] == '=')
        value = value.Substring(1);
      for (int iTopRow = this.m_iTopRow; iTopRow <= this.m_iBottomRow; ++iTopRow)
      {
        for (int iLeftColumn = this.m_iLeftColumn; iLeftColumn <= this.m_iRightColumn; ++iLeftColumn)
          this.m_worksheet.SetFormula(iTopRow, iLeftColumn, value, true);
      }
      this.SetChanged();
    }
  }

  public string FormulaArrayR1C1
  {
    get
    {
      this.CheckDisposed();
      return $"{{{this.GetFormulaArray(true)}}}";
    }
    set
    {
      this.CheckDisposed();
      this.SetFormulaArray(value, true);
    }
  }

  public bool HasFormula
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Formula)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasFormulaArray
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (!this.m_worksheet.HasArrayFormulaRecord(row, column))
            return false;
        }
      }
      return true;
    }
  }

  public ExcelHAlign HorizontalAlignment
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.CellStyle.HorizontalAlignment : ExcelHAlign.HAlignGeneral;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.CellStyle.HorizontalAlignment = value;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.HorizontalAlignment = value;
          }
        }
      }
    }
  }

  public IHyperLinks Hyperlinks
  {
    get
    {
      return (IHyperLinks) ((HyperLinksCollection) this.m_worksheet.HyperLinks).GetRangeHyperlinks((IRange) this);
    }
  }

  public int IndentLevel
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.CellStyle.IndentLevel : int.MinValue;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.CellStyle.IndentLevel = (int) (ushort) value;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.IndentLevel = value;
          }
        }
      }
      if (value <= 0 || !this.Boolean && this.FormatType != ExcelFormatType.Number)
        return;
      this.AutofitColumns();
    }
  }

  public bool IsBoolean
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Boolean)
            return false;
        }
      }
      return true;
    }
  }

  public bool IsError
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Error)
            return false;
        }
      }
      return true;
    }
  }

  public bool IsGroupedByColumn
  {
    get
    {
      this.CheckDisposed();
      int firstColumn = this.FirstColumn;
      int lastColumn = this.LastColumn;
      if (firstColumn == lastColumn)
      {
        ColumnInfoRecord columnInfoRecord = this.m_worksheet.ColumnInformation[firstColumn];
        return columnInfoRecord != null && columnInfoRecord.OutlineLevel != (ushort) 0;
      }
      int firstRow = this.FirstRow;
      for (int column = firstColumn; column <= lastColumn; ++column)
      {
        if (!this.m_worksheet[firstRow, column].IsGroupedByColumn)
          return false;
      }
      return true;
    }
  }

  public bool IsGroupedByRow
  {
    get
    {
      this.CheckDisposed();
      int firstRow = this.FirstRow;
      int lastRow = this.LastRow;
      if (firstRow == lastRow)
      {
        IOutline rowOutline = WorksheetHelper.GetRowOutline((IInternalWorksheet) this.m_worksheet, firstRow);
        return rowOutline != null && rowOutline.OutlineLevel != (ushort) 0;
      }
      int firstColumn = this.FirstColumn;
      for (int row = firstRow; row <= lastRow; ++row)
      {
        if (!this.m_worksheet[row, firstColumn].IsGroupedByRow)
          return false;
      }
      return true;
    }
  }

  public int LastColumn
  {
    [DebuggerStepThrough] get => this.m_iRightColumn;
    set
    {
      if (value < 1 || value > this.m_book.MaxColumnCount)
        throw new ArgumentOutOfRangeException("FirstRow");
      if (value == this.LastColumn)
        return;
      this.m_iRightColumn = value;
      this.OnLastColumnChanged();
    }
  }

  public int LastRow
  {
    [DebuggerStepThrough] get => this.m_iBottomRow;
    set
    {
      if (value < 1 || value > this.m_book.MaxRowCount)
        throw new ArgumentOutOfRangeException("FirstRow");
      if (value == this.LastRow)
        return;
      this.m_iBottomRow = value;
      this.OnLastRowChanged();
    }
  }

  public double Number
  {
    get
    {
      this.CheckDisposed();
      double number = this.m_worksheet.GetNumber(this.Row, this.Column);
      if (number == double.NaN)
        return number;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (number != this.m_worksheet.GetNumber(row, column) && double.IsNaN(number))
            return double.NaN;
        }
      }
      return number;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        if (BitConverter.DoubleToInt64Bits(value) == BitConverter.DoubleToInt64Bits(-0.0))
          value = 0.0;
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
          double number = this.Number;
          if (number != value)
            this.OnCellValueChanged((object) number, (object) value, (IRange) this);
          if (this.m_rtfString == null)
            this.CreateRichTextString();
          this.m_rtfString.BeginUpdate();
          this.m_rtfString.Text = "#N/A";
          if (this.NumberFormat != "General")
            this.NumberFormat = "@";
          this.m_rtfString.ClearFormatting();
          this.m_rtfString.EndUpdate();
          this.SetChanged();
        }
        else
        {
          double number = this.Number;
          if (number != value)
            this.OnCellValueChanged((object) number, (object) value, (IRange) this);
          this.SetNumberAndFormat(value, true);
          this.SetChanged();
        }
      }
      else
      {
        this.TryRemoveFormulaArrays();
        int firstRow = this.FirstRow;
        for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
        {
          int firstColumn = this.FirstColumn;
          for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
            this.m_worksheet[firstRow, firstColumn].Number = value;
        }
      }
    }
  }

  public string NumberFormat
  {
    get
    {
      this.CheckDisposed();
      string inputFormat = (string) null;
      if (this.IsSingleCell)
        inputFormat = this.GetNumberFormat();
      if (inputFormat == null)
        inputFormat = RangeImpl.GetNumberFormat((IList) this.CellsList);
      if (inputFormat != null && inputFormat != string.Empty && !this.Workbook.Loading && !this.Workbook.Saving)
      {
        if (inputFormat.Contains("\\"))
          inputFormat = this.CheckAndGetDateUncustomizedString(inputFormat);
        string input = this.CheckForAccountingString(inputFormat);
        inputFormat = FormatParserImpl.NumberFormatRegex.IsMatch(input) ? FormatParserImpl.NumberFormatRegex.Replace(input, string.Empty) : input;
      }
      return inputFormat;
    }
    set
    {
      this.CheckDisposed();
      value = AmPmToken.CheckAndApplyAMPM(value);
      if (this.IsSingleCell)
      {
        this.CellStyle.NumberFormat = value;
        this.SetChanged();
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.NumberFormat = value;
          }
        }
      }
      this.m_hasDefaultFormat = false;
    }
  }

  public int Row
  {
    get
    {
      this.CheckDisposed();
      return this.FirstRow;
    }
  }

  public int RowGroupLevel
  {
    get
    {
      this.CheckDisposed();
      int firstRow = this.FirstRow;
      int lastRow = this.LastRow;
      if (firstRow == lastRow)
      {
        IOutline row = (IOutline) WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, firstRow - 1, false);
        return row == null ? 0 : (int) row.OutlineLevel;
      }
      int firstColumn = this.FirstColumn;
      int rowGroupLevel = this.m_worksheet[firstRow, firstColumn].RowGroupLevel;
      for (int row = firstRow + 1; row <= lastRow; ++row)
      {
        if (rowGroupLevel != this.m_worksheet[row, firstColumn].RowGroupLevel)
          return -1;
      }
      return rowGroupLevel;
    }
  }

  public double RowHeight
  {
    get
    {
      this.CheckDisposed();
      double rowHeight;
      if (this.m_iTopRow == this.m_iBottomRow)
      {
        rowHeight = this.m_worksheet.GetRowHeight(this.Row);
      }
      else
      {
        rowHeight = this.m_worksheet.GetRowHeight(this.m_iTopRow);
        for (int iRow = this.m_iTopRow + 1; iRow <= this.m_iBottomRow; ++iRow)
        {
          if (rowHeight != this.m_worksheet.GetRowHeight(iRow))
          {
            rowHeight = double.MinValue;
            break;
          }
        }
      }
      return rowHeight;
    }
    set
    {
      this.CheckDisposed();
      this.SetRowHeight(value, true, true);
    }
  }

  public IRange[] Rows
  {
    get
    {
      this.CheckDisposed();
      int length = this.FirstColumn == 0 || this.LastColumn == 0 || this.LastRow == 0 ? 0 : this.LastRow - this.FirstRow + 1;
      IRange[] rows = new IRange[length];
      if (length > 0)
      {
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
          rows[firstRow - this.FirstRow] = this.m_worksheet.Range[firstRow, this.FirstColumn, firstRow, this.LastColumn];
      }
      return rows;
    }
  }

  public IRange[] Columns
  {
    get
    {
      this.CheckDisposed();
      if (this.FirstColumn == 0 || this.FirstColumn > this.m_book.MaxColumnCount)
        return new IRange[0];
      IRange[] columns = new IRange[this.LastColumn - this.FirstColumn + 1];
      for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
        columns[firstColumn - this.FirstColumn] = this.m_worksheet.Range[this.FirstRow, firstColumn, this.LastRow, firstColumn];
      return columns;
    }
  }

  private IStyle CreateStyleForEntireRowEntireColumn()
  {
    List<IRange> LstRange = new List<IRange>();
    if (this.m_style != null)
      return (IStyle) this.m_style;
    this.CreateStyle();
    LstRange.Add((IRange) this);
    if (this.IsEntireRow)
    {
      for (int row = this.Row; row <= this.LastRow; ++row)
      {
        for (int column = this.Column; column <= this.LastColumn; ++column)
        {
          if (this.m_worksheet.CellRecords.GetCellRecord(row, column) != null)
          {
            RangeImpl rangeImpl = this.m_worksheet[row, column] as RangeImpl;
            LstRange.Add((IRange) rangeImpl);
          }
        }
      }
    }
    if (this.IsEntireColumn)
    {
      for (int column = this.Column; column <= this.LastColumn; ++column)
      {
        for (int row = this.Row; row <= this.LastRow; ++row)
        {
          if (this.m_worksheet.CellRecords.GetCellRecord(row, column) != null)
          {
            RangeImpl rangeImpl = this.m_worksheet[row, column] as RangeImpl;
            LstRange.Add((IRange) rangeImpl);
          }
        }
      }
    }
    return LstRange.Count == 1 ? (IStyle) this.m_style : (IStyle) new StyleArrayWrapper(this.Application, LstRange, (IWorksheet) this.m_worksheet);
  }

  public IStyle CellStyle
  {
    get
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        if (this.m_style == null)
          this.CreateStyle();
        (this.m_style.Font as FontWrapper).Range = (IRange) this;
        return (IStyle) this.m_style;
      }
      return this.IsEntireRow || this.IsEntireColumn ? this.CreateStyleForEntireRowEntireColumn() : (IStyle) new StyleArrayWrapper((IRange) this);
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.SetChanged();
        RangeImpl.TCellType cellType = this.CellType;
        this.ExtendedFormatIndex = !(value is ExtendedFormatWrapper) ? (ushort) ((ExtendedFormatWrapper) this.m_book.Styles[value == null ? this.DefaultStyleNames[0] : value.Name]).Wrapped.Index : (ushort) (value as ExtendedFormatWrapper).Wrapped.Index;
        BiffRecordRaw record = this.Record;
        if (record != null && record.TypeCode == TBIFFRecord.Formula || record == null)
        {
          string old = this.Value;
          this.OnValueChanged(old, old);
        }
        this.OnStyleChanged(cellType);
        this.m_book.AddUsedStyleIndex((int) this.ExtendedFormatIndex);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        Dictionary<int, int> dictionary = new Dictionary<int, int>((this.LastRow - this.FirstRow + 1) * (this.LastColumn - this.FirstColumn + 1));
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            ushort xfIndex = (ushort) this.m_worksheet.GetXFIndex(firstRow, firstColumn);
            int num;
            if (dictionary.TryGetValue((int) xfIndex, out num))
            {
              BiffRecordRaw record1 = (BiffRecordRaw) this.m_worksheet.GetRecord(firstRow, firstColumn);
              RangeImpl.TCellType oldType = record1 != null ? (RangeImpl.TCellType) record1.TypeCode : RangeImpl.TCellType.Blank;
              this.m_worksheet.CellRecords.SetCellStyle(firstRow, firstColumn, num);
              BiffRecordRaw record2 = (BiffRecordRaw) this.m_worksheet.GetRecord(firstRow, firstColumn);
              if (record2 != null && record2.TypeCode == TBIFFRecord.Formula || record2 == null)
              {
                string old = migrantRangeImpl.Value;
                this.OnValueChanged(old, old);
              }
              this.OnStyleChanged(oldType);
              this.m_book.AddUsedStyleIndex(num);
            }
            else
            {
              migrantRangeImpl.CellStyle = value;
              dictionary.Add((int) xfIndex, (int) migrantRangeImpl.ExtendedFormatIndex);
            }
          }
        }
      }
    }
  }

  public string CellStyleName
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.GetStyleName() : RangeImpl.GetCellStyleName((IList<IRange>) this.CellsList);
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        RangeImpl.TCellType cellType = this.CellType;
        if (value == null)
        {
          string defaultStyleName = this.DefaultStyleNames[0];
        }
        this.ChangeStyleName(value);
        BiffRecordRaw record = this.Record;
        if (record != null && record.TypeCode == TBIFFRecord.Formula || record == null)
        {
          string old = this.Value;
          this.OnValueChanged(old, old);
        }
        this.OnStyleChanged(cellType);
        this.SetChanged();
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        Dictionary<int, int> dictionary = new Dictionary<int, int>((this.LastRow - this.FirstRow + 1) * (this.LastColumn - this.FirstColumn + 1));
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            ushort xfIndex = (ushort) this.m_worksheet.GetXFIndex(firstRow, firstColumn);
            int iXFIndex;
            if (dictionary.TryGetValue((int) xfIndex, out iXFIndex))
            {
              BiffRecordRaw record1 = (BiffRecordRaw) this.m_worksheet.GetRecord(firstRow, firstColumn);
              RangeImpl.TCellType oldType = record1 != null ? (RangeImpl.TCellType) record1.TypeCode : RangeImpl.TCellType.Blank;
              this.m_worksheet.CellRecords.SetCellStyle(firstRow, firstColumn, iXFIndex);
              BiffRecordRaw record2 = (BiffRecordRaw) this.m_worksheet.GetRecord(firstRow, firstColumn);
              if (record2 != null && record2.TypeCode == TBIFFRecord.Formula || record2 == null)
              {
                string old = migrantRangeImpl.Value;
                this.OnValueChanged(old, old);
              }
              this.OnStyleChanged(oldType);
            }
            else
            {
              migrantRangeImpl.CellStyleName = value;
              dictionary.Add((int) xfIndex, (int) migrantRangeImpl.ExtendedFormatIndex);
            }
          }
        }
      }
    }
  }

  public BuiltInStyles? BuiltInStyle
  {
    get
    {
      return new BuiltInStyles?((BuiltInStyles) Array.IndexOf<string>(this.DefaultStyleNames, this.CellStyleName));
    }
    set => this.CellStyleName = this.DefaultStyleNames[(int) value.Value];
  }

  public string Text
  {
    get
    {
      this.CheckDisposed();
      string text = this.m_worksheet.GetText(this.Row, this.Column);
      if (text == null)
        return (string) null;
      if (!this.IsMerged)
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            if (text != this.m_worksheet.GetText(row, column))
              return (string) null;
          }
        }
      }
      if (this.ExtendedFormat.IsFirstSymbolApostrophe)
        text = "'" + text;
      return text;
    }
    set
    {
      this.CheckDisposed();
      if (value == null)
        value = string.Empty;
      this.TryRemoveFormulaArrays();
      if (this.IsSingleCell)
      {
        if (value.Length == 0)
        {
          this.Value = value;
        }
        else
        {
          if (this.Text != value)
            this.OnCellValueChanged((object) this.Text, (object) value, (IRange) this);
          value = this.CheckApostrophe(value);
          if (this.m_rtfString == null)
            this.CreateRichTextString();
          this.m_rtfString.BeginUpdate();
          this.m_rtfString.Text = value;
          if (this.NumberFormat != "General" && this.FormatType != ExcelFormatType.Unknown)
            this.NumberFormat = "@";
          this.m_rtfString.EndUpdate();
          this.SetChanged();
        }
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.Text = value;
          }
        }
        if (this.m_rtfString == null)
          this.CreateRichTextString();
        this.m_rtfString.BeginUpdate();
        this.m_rtfString.Text = value;
        this.m_rtfString.EndUpdate();
      }
      if (!value.Contains(Environment.NewLine) && !value.Contains("\n") && !value.Contains("_x000a_"))
        return;
      this.WrapText = true;
    }
  }

  public TimeSpan TimeSpan
  {
    get
    {
      this.CheckDisposed();
      double number1 = this.m_worksheet.GetNumber(this.Row, this.Column);
      if (number1 == double.NaN || this.InnerNumberFormat.GetFormatType(number1) != ExcelFormatType.DateTime)
        return TimeSpan.MinValue;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double number2 = this.m_worksheet.GetNumber(row, column);
          if (number2 == double.NaN || number1 != number2 || this.InnerNumberFormat.GetFormatType(number2) != ExcelFormatType.DateTime)
            return TimeSpan.MinValue;
        }
      }
      return number1 < 2958466.0 ? TimeSpan.FromDays(number1) : TimeSpan.FromDays(2958465.9999999884);
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.FormatType = ExcelFormatType.DateTime;
        TimeSpan timeSpan = this.TimeSpan;
        if (timeSpan != value)
          this.OnCellValueChanged((object) timeSpan, (object) value, (IRange) this);
        this.SetTimeSpan(value);
        this.SetChanged();
      }
      else
      {
        this.TryRemoveFormulaArrays();
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.TimeSpan = value;
          }
        }
      }
    }
  }

  public string Value
  {
    get
    {
      this.CheckDisposed();
      string str1 = (string) null;
      if (this.IsSingleCell)
      {
        if (this.m_book != null)
        {
          ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
          str1 = this.m_worksheet.GetValue(this.Record as ICellPositionFormat, false);
        }
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
        str1 = migrantRangeImpl.Value;
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            migrantRangeImpl.ResetRowColumn(row, column);
            string str2 = migrantRangeImpl.Value;
            if (str1 != str2)
            {
              str1 = (string) null;
              break;
            }
          }
        }
      }
      return str1;
    }
    set
    {
      this.CheckDisposed();
      this.TryRemoveFormulaArrays();
      if (this.IsSingleCell)
      {
        string old = this.Value;
        if (value != old)
          this.OnValueChanged(old, value);
      }
      else
      {
        WorksheetImpl worksheet = this.Worksheet as WorksheetImpl;
        string strCellsRange = this.AddressLocal;
        bool flag = false;
        FileDataHolder fileDataHolder = this.Workbook.DataHolder ?? new FileDataHolder(this.m_book);
        if (value != null && value != string.Empty && value[0].ToString() == "=" && value.Length > 1 && value[1].ToString() != "&")
          flag = true;
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            if (this.Workbook.Application.EnableIncrementalFormula && flag)
            {
              fileDataHolder.Parser.SetSharedFormula(worksheet, value, strCellsRange, worksheet.m_sharedFormulaGroupIndex, firstRow, firstColumn, (int) this.ExtendedFormatIndex, false);
              strCellsRange = (string) null;
            }
            else
              migrantRangeImpl.Value = value;
          }
        }
        if (this.Workbook.Application.EnableIncrementalFormula && flag)
          ++worksheet.m_sharedFormulaGroupIndex;
      }
      if (value == null || !value.Contains(Environment.NewLine) && !value.Contains("\n"))
        return;
      this.WrapText = true;
    }
  }

  public string CalculatedValue
  {
    get
    {
      if (this.Formula == null)
        return this.Value;
      return this.Parent is IWorksheet && ((IWorksheet) this.Parent).CalcEngine != null ? ((IWorksheet) this.Parent).CalcEngine.PullUpdatedValue(RangeInfo.GetAlphaLabel(this.Column) + this.Row.ToString()) : (string) null;
    }
  }

  public object Value2
  {
    get
    {
      this.CheckDisposed();
      return this.TryCreateValue2() ?? (object) this.Value;
    }
    set
    {
      this.CheckDisposed();
      this.TryRemoveFormulaArrays();
      if (this.IsSingleCell)
      {
        this.SetSingleCellValue2(value);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.Value2 = value;
          }
        }
      }
    }
  }

  internal bool IsNumReference
  {
    get => this.m_bIsNumReference;
    set => this.m_bIsNumReference = value;
  }

  internal bool IsStringReference
  {
    get => this.m_bIsStringReference;
    set => this.m_bIsStringReference = value;
  }

  internal bool IsMultiReference
  {
    get => this.m_bIsMultiReference;
    set => this.m_bIsMultiReference = value;
  }

  public ExcelVAlign VerticalAlignment
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.CellStyle.VerticalAlignment : ExcelVAlign.VAlignBottom;
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.CellStyle.VerticalAlignment = value;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.VerticalAlignment = value;
          }
        }
      }
    }
  }

  public IWorksheet Worksheet
  {
    get
    {
      this.CheckDisposed();
      return (IWorksheet) this.m_worksheet;
    }
  }

  public IRange this[int row, int column]
  {
    get
    {
      this.CheckDisposed();
      if (this.m_book.Application.RangeIndexerMode == ExcelRangeIndexerMode.Relative && !this.m_bIsAbsolute)
      {
        row = this.m_iTopRow + row - 1;
        column = this.m_iLeftColumn + column - 1;
      }
      this.CheckRange(row, column);
      return this.m_worksheet.InnerGetCell(column, row);
    }
    set
    {
      this.CheckDisposed();
      this.CheckRange(row, column);
      this.m_worksheet.InnerSetCell(column, row, (RangeImpl) value);
      this.SetChanged();
    }
  }

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get
    {
      this.CheckDisposed();
      if (this.m_book.Application.RangeIndexerMode == ExcelRangeIndexerMode.Relative && !this.m_bIsAbsolute)
      {
        row = this.m_iTopRow + row - 1;
        column = this.m_iLeftColumn + column - 1;
        lastRow = this.m_iTopRow + lastRow - 1;
        lastColumn = this.m_iLeftColumn + lastColumn - 1;
      }
      row = this.NormalizeRowIndex(row, column, lastColumn);
      lastRow = this.NormalizeRowIndex(lastRow, column, lastColumn);
      column = this.NormalizeColumnIndex(column, row, lastRow);
      lastColumn = this.NormalizeColumnIndex(lastColumn, row, lastRow);
      this.CheckRange(row, column);
      this.CheckRange(lastRow, lastColumn);
      return row != lastRow || column != lastColumn ? (IRange) this.AppImplementation.CreateRange(this.Parent, column, row, lastColumn, lastRow) : this.m_worksheet[row, column];
    }
  }

  public IRange this[string name] => this[name, false];

  public IRange this[string name, bool IsR1C1Notation]
  {
    get
    {
      this.CheckDisposed();
      string worksheetName = RangeImpl.GetWorksheetName(ref name);
      if (worksheetName != null && this.m_worksheet.Name != worksheetName)
        return this.FindWorksheet(worksheetName).Range[name];
      IName name1 = this.m_worksheet.Names[name];
      if (name1 != null)
        return name1.RefersToRange;
      IName name2 = this.m_book.Names[name];
      if (name2 != null)
        return name2.RefersToRange;
      IRange intersect;
      if (this.m_worksheet.TryGetIntersectRange(name, out intersect))
        return intersect;
      name = name.ToUpper();
      if (IsR1C1Notation)
        return this.ParseR1C1Reference(name);
      int iFirstRow;
      int iFirstColumn;
      int iLastRow;
      int iLastColumn;
      int rangeString = RangeImpl.ParseRangeString(name, (IWorkbook) this.Workbook, out iFirstRow, out iFirstColumn, out iLastRow, out iLastColumn);
      if (rangeString == 1)
        return this.m_worksheet[iFirstRow, iFirstColumn];
      if (rangeString >= 2)
        return this.m_worksheet[iFirstRow, iFirstColumn, iLastRow, iLastColumn];
      throw new ArgumentException();
    }
  }

  public IConditionalFormats ConditionalFormats
  {
    get
    {
      this.m_worksheet.ParseSheetCF();
      this.CheckDisposed();
      return (IConditionalFormats) this.AppImplementation.CreateCondFormatCollectionWrapper((ICombinedRange) this);
    }
  }

  public IDataValidation DataValidation
  {
    get
    {
      this.CheckDisposed();
      if (!this.IsSingleCell)
        return (IDataValidation) this.AppImplementation.CreateDataValidationArrayImpl((IRange) this);
      if (this.m_dataValidation == null)
        this.m_dataValidation = this.AppImplementation.CreateDataValidationWrapper(this, this.FindDataValidation());
      return (IDataValidation) this.m_dataValidation;
    }
  }

  public bool HasFormulaBoolValue
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          WorksheetImpl.TRangeValueType cellType = this.m_worksheet.GetCellType(row, column, true);
          if ((cellType & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula || (cellType & WorksheetImpl.TRangeValueType.Boolean) != WorksheetImpl.TRangeValueType.Boolean)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasFormulaErrorValue
  {
    get
    {
      this.CheckDisposed();
      RangeImpl.UpdateCellValue(this.Parent, this.Column, this.Row, this.updateCellValue);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          WorksheetImpl.TRangeValueType cellType = this.m_worksheet.GetCellType(row, column, true);
          if ((cellType & WorksheetImpl.TRangeValueType.Formula) != WorksheetImpl.TRangeValueType.Formula || (cellType & WorksheetImpl.TRangeValueType.Error) != WorksheetImpl.TRangeValueType.Error)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasFormulaDateTime
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double formulaNumberValue = this.m_worksheet.GetFormulaNumberValue(row, column);
          if (!double.IsNaN(formulaNumberValue))
          {
            switch (this.InnerNumberFormat.GetFormatType(formulaNumberValue))
            {
              case ExcelFormatType.General:
                if (Array.IndexOf<string>(this.DEF_DATETIME_FORMULA, RangeImpl.GetFormula(this.Formula)) != -1)
                  continue;
                break;
              case ExcelFormatType.DateTime:
                continue;
            }
          }
          return false;
        }
      }
      return true;
    }
  }

  public bool HasFormulaNumberValue
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double formulaNumberValue = this.m_worksheet.GetFormulaNumberValue(row, column);
          if (double.IsNaN(formulaNumberValue) || this.InnerNumberFormat.GetFormatType(formulaNumberValue) == ExcelFormatType.DateTime)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasFormulaStringValue
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet[row, column].FormulaStringValue == null)
            return false;
        }
      }
      return true;
    }
  }

  public bool IsBlank
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Blank)
            return false;
        }
      }
      return true;
    }
  }

  public bool IsBlankorHasStyle
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Blank || this.Worksheet[row, column].HasStyle && this.Worksheet[row, column].CellStyle.FillPattern != ExcelPattern.None || this.Worksheet[row, column].CellStyle.HasBorder)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasBoolean
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Boolean)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasDateTime
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double number = this.m_worksheet.GetNumber(row, column);
          if (double.IsNaN(number) || this.InnerNumberFormat.GetFormatType(number) != ExcelFormatType.DateTime)
            return false;
        }
      }
      return true;
    }
  }

  public bool HasNumber
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double number = this.m_worksheet.GetNumber(row, column);
          if (!double.IsNaN(number))
          {
            ExcelFormatType formatType = this.InnerNumberFormat.GetFormatType(number);
            if (formatType == ExcelFormatType.Unknown && number == 0.0)
              formatType = this.InnerNumberFormat.GetFormatType(1.0);
            if (formatType != ExcelFormatType.DateTime && formatType != ExcelFormatType.Text)
              return true;
          }
        }
      }
      return false;
    }
  }

  public bool HasString
  {
    get
    {
      this.CheckDisposed();
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.String)
            return false;
        }
      }
      return true;
    }
  }

  public ICommentShape Comment
  {
    get
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
        return this.m_worksheet.InnerComments[this.FirstRow, this.FirstColumn];
      CommentsCollection innerComments = this.m_worksheet.InnerComments;
      foreach (IRange cell in this.Cells)
      {
        if (innerComments[cell.Row, cell.Column] != null)
          return (ICommentShape) ((ApplicationImpl) this.Application).CreateCommentsRange((IRange) this);
      }
      return (ICommentShape) null;
    }
  }

  public IRichTextString RichText
  {
    get
    {
      this.CheckDisposed();
      if (this.m_rtfString == null)
        this.CreateRichTextString();
      if (this.m_rtfString is RichTextString)
        (this.m_rtfString as RichTextString).m_formatType = this.FormatType;
      return (IRichTextString) this.m_rtfString;
    }
  }

  public bool HasRichText
  {
    get
    {
      this.CheckDisposed();
      if (!this.IsSingleCell || !this.HasString)
        return false;
      TextWithFormat textWithFormat = this.m_worksheet.GetTextWithFormat(RangeImpl.GetCellIndex(this.m_iLeftColumn, this.m_iTopRow));
      return textWithFormat != null && textWithFormat.FormattingRunsCount > 0;
    }
  }

  public bool IsMerged
  {
    get
    {
      this.CheckDisposed();
      Rectangle rect1 = new Rectangle(this.FirstColumn - 1, this.FirstRow - 1, 0, 0);
      if (this.IsSingleCell)
        return this.m_worksheet.MergeCells[rect1] != null;
      Rectangle rect2 = new Rectangle(this.LastColumn - 1, this.LastRow - 1, 0, 0);
      MergeCellsRecord.MergedRegion mergeCell1 = this.m_worksheet.MergeCells[rect1];
      MergeCellsRecord.MergedRegion mergeCell2 = this.m_worksheet.MergeCells[rect2];
      return mergeCell1 != null && mergeCell1.Equals((object) mergeCell2);
    }
  }

  public IRange MergeArea
  {
    get
    {
      this.CheckDisposed();
      MergeCellsRecord.MergedRegion parentMergeRegion = this.ParentMergeRegion;
      return parentMergeRegion == null ? (IRange) null : this.m_worksheet[parentMergeRegion.RowFrom + 1, parentMergeRegion.ColumnFrom + 1, parentMergeRegion.RowTo + 1, parentMergeRegion.ColumnTo + 1];
    }
  }

  public bool IsInitialized
  {
    get
    {
      this.CheckDisposed();
      return !this.IsBlank || this.HasStyle;
    }
  }

  public bool HasStyle
  {
    get
    {
      this.CheckDisposed();
      if (this.IsSingleCell || this.IsEntireRow || this.IsEntireColumn)
      {
        bool flag1 = this.m_style != null && this.m_style.IsInitialized;
        int extendedFormatIndex = (int) this.ExtendedFormatIndex;
        bool flag2 = extendedFormatIndex != 0 && extendedFormatIndex != this.m_book.DefaultXFIndex;
        return flag1 || flag2;
      }
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.HasStyle)
            return true;
        }
      }
      return false;
    }
  }

  public bool WrapText
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.GetWrapText() : RangeImpl.GetWrapText((IList) this.CellsList);
    }
    set
    {
      this.CheckDisposed();
      if (this.IsSingleCell)
      {
        this.CellStyle.WrapText = value;
        RowStorage row = WorksheetHelper.GetOrCreateRow(this.Worksheet as IInternalWorksheet, this.Row - 1, false);
        if (row != null && !this.Workbook.Loading)
          row.IsWrapText = value;
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.CellStyle.WrapText = value;
          }
          RowStorage row = WorksheetHelper.GetOrCreateRow(this.Worksheet as IInternalWorksheet, firstRow - 1, false);
          if (row != null && !row.IsBadFontHeight && !this.Workbook.Loading)
            row.IsWrapText = value;
        }
      }
      this.SetChanged();
    }
  }

  public ExcelIgnoreError IgnoreErrorOptions
  {
    get
    {
      ErrorIndicatorImpl errorIndicatorImpl = this.m_worksheet.ErrorIndicators.Find(this.GetRectangles());
      return errorIndicatorImpl == null ? ExcelIgnoreError.None : errorIndicatorImpl.IgnoreOptions;
    }
    set
    {
      ErrorIndicatorsCollection errorIndicators = this.m_worksheet.ErrorIndicators;
      Rectangle rect = Rectangle.FromLTRB(this.Column - 1, this.Row - 1, this.LastColumn - 1, this.LastRow - 1);
      if (value == ExcelIgnoreError.None)
      {
        errorIndicators.Remove(new Rectangle[1]{ rect });
      }
      else
      {
        ErrorIndicatorImpl errorIndicator = new ErrorIndicatorImpl(rect, value);
        errorIndicators.Add(errorIndicator);
      }
    }
  }

  public bool HasExternalFormula
  {
    get
    {
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (!this.m_worksheet.IsExternalFormula(row, column))
            return false;
        }
      }
      return true;
    }
  }

  public bool? IsStringsPreserved
  {
    get => this.m_worksheet.GetStringPreservedValue((ICombinedRange) this);
    set => this.m_worksheet.SetStringPreservedValue((ICombinedRange) this, value);
  }

  internal StringFormat StringFormt
  {
    get
    {
      if (this._stringformat == null)
      {
        this._stringformat = new StringFormat(StringFormat.GenericTypographic);
        this._stringformat.FormatFlags &= ~StringFormatFlags.LineLimit;
        this._stringformat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        this._stringformat.FormatFlags |= StringFormatFlags.NoClip;
        this._stringformat.Trimming = StringTrimming.Character;
      }
      return this._stringformat;
    }
  }

  internal bool IsAbsolute
  {
    get => this.m_bIsAbsolute;
    set => this.m_bIsAbsolute = value;
  }

  public IApplication Application => this.m_worksheet.Application;

  public object Parent => (object) this.m_worksheet;

  private ApplicationImpl AppImplementation => this.m_worksheet.AppImplementation;

  public string AddressGlobal
  {
    get => $"{this.m_worksheet.QuotedName}!{this.AddressGlobalWithoutSheetName}";
  }

  public string AddressGlobalWithoutSheetName
  {
    get
    {
      string empty = string.Empty;
      string cellNameWithDollars1 = RangeImpl.GetCellNameWithDollars(this.FirstColumn, this.FirstRow);
      if (this.IsSingleCell)
        return empty + cellNameWithDollars1;
      string cellNameWithDollars2 = RangeImpl.GetCellNameWithDollars(this.LastColumn, this.LastRow);
      return $"{empty}{cellNameWithDollars1}:{cellNameWithDollars2}";
    }
  }

  internal List<IRange> CellsList
  {
    get
    {
      this.CheckDisposed();
      if (this.m_cells == null && !this.m_bCells)
        this.InfillCells();
      return this.m_cells != null ? this.m_cells : throw new ArgumentNullException();
    }
  }

  protected internal bool IsSingleCell
  {
    get => this.m_iLeftColumn == this.m_iRightColumn && this.m_iTopRow == this.m_iBottomRow;
  }

  protected internal int FirstRow
  {
    [DebuggerStepThrough] get => this.m_iTopRow;
    set
    {
      if (value == 0 || value > this.m_book.MaxRowCount)
        throw new ArgumentOutOfRangeException(nameof (FirstRow));
      if (value == this.FirstRow)
        return;
      this.m_iTopRow = value;
      this.OnFirstRowChanged();
    }
  }

  protected internal int FirstColumn
  {
    [DebuggerStepThrough] get => this.m_iLeftColumn;
    set
    {
      if (value < 1 || value > this.m_book.MaxColumnCount)
        throw new ArgumentOutOfRangeException("FirstRow", "Value was out of range.");
      if (value == this.FirstColumn)
        return;
      this.m_iLeftColumn = value;
      this.OnFirstColumnChanged();
    }
  }

  protected internal string CellName
  {
    get
    {
      return this.IsSingleCell ? RangeImpl.GetCellName(this.FirstColumn, this.FirstRow) : (string) null;
    }
  }

  protected internal long CellIndex
  {
    get => this.IsSingleCell ? RangeImpl.GetCellIndex(this.FirstColumn, this.FirstRow) : -1L;
  }

  protected internal RangeImpl.TCellType CellType
  {
    get
    {
      return this.Record != null ? (RangeImpl.TCellType) this.Record.TypeCode : RangeImpl.TCellType.Blank;
    }
  }

  [CLSCompliant(false)]
  protected internal ushort StyleXFIndex
  {
    get
    {
      if (this.IsSingleCell)
      {
        if (this.m_style != null)
          return (ushort) this.m_style.Wrapped.Index;
        if (this.Record != null)
          return ((ICellPositionFormat) this.Record).ExtendedFormatIndex;
      }
      return (ushort) this.m_book.DefaultXFIndex;
    }
  }

  [CLSCompliant(false)]
  public ushort ExtendedFormatIndex
  {
    get
    {
      if (this.IsEntireRow)
        return (ushort) this.m_worksheet.GetXFIndex(this.m_iTopRow);
      return this.IsEntireColumn ? (ushort) this.m_worksheet.GetColumnXFIndex(this.m_iLeftColumn) : (ushort) this.m_worksheet.GetXFIndex(this.m_iTopRow, this.m_iLeftColumn);
    }
    set
    {
      if (!this.IsSingleCell && !this.IsEntireRow && !this.IsEntireColumn)
        throw new ArgumentException("This property can be used only for single cell not a range");
      this.SetXFormatIndex((int) value);
    }
  }

  [CLSCompliant(false)]
  protected internal MulRKRecord.RkRec RKSubRecord
  {
    get
    {
      if (this.CellType != RangeImpl.TCellType.RK)
        throw new ArgumentException("This property can be accessed only when range represent RKRecord");
      return new MulRKRecord.RkRec(this.StyleXFIndex, RKRecord.ConvertToRKNumber(((RKRecord) this.Record).RKNumber));
    }
  }

  protected internal WorkbookImpl Workbook => this.m_book;

  private MergeCellsRecord.MergedRegion ParentMergeRegion
  {
    get
    {
      Rectangle rect1 = new Rectangle(this.FirstColumn - 1, this.FirstRow - 1, 0, 0);
      if (this.IsSingleCell)
        return this.m_worksheet.MergeCells[rect1];
      Rectangle rect2 = new Rectangle(this.LastColumn - 1, this.LastRow - 1, 0, 0);
      MergeCellsRecord.MergedRegion mergeCell1 = this.m_worksheet.MergeCells[rect1];
      MergeCellsRecord.MergedRegion mergeCell2 = this.m_worksheet.MergeCells[rect2];
      return !MergeCellsRecord.MergedRegion.Equals(mergeCell1, mergeCell2) ? (MergeCellsRecord.MergedRegion) null : mergeCell1;
    }
  }

  protected internal WorksheetImpl InnerWorksheet => this.m_worksheet;

  [CLSCompliant(false)]
  protected internal BiffRecordRaw Record
  {
    get => (BiffRecordRaw) this.m_worksheet.GetRecord(this.FirstRow, this.FirstColumn);
    set => this.m_worksheet.CellRecords.AddRecord(value, false);
  }

  public Dictionary<ArrayRecord, object> FormulaArrays
  {
    get
    {
      Dictionary<ArrayRecord, object> formulaArrays = (Dictionary<ArrayRecord, object>) null;
      if (this.IsSingleCell)
      {
        ArrayRecord arrayRecord = this.m_worksheet.CellRecords.GetArrayRecord(this.m_iTopRow, this.m_iLeftColumn);
        if (arrayRecord != null)
        {
          formulaArrays = new Dictionary<ArrayRecord, object>();
          formulaArrays[arrayRecord] = (object) null;
        }
      }
      else
      {
        formulaArrays = new Dictionary<ArrayRecord, object>();
        Dictionary<long, object> dictionary = new Dictionary<long, object>();
        CellRecordCollection cellRecords = this.m_worksheet.CellRecords;
        int firstRow = this.FirstRow;
        for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
        {
          int firstColumn = this.FirstColumn;
          for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
          {
            ArrayRecord arrayRecord = cellRecords.GetArrayRecord(firstRow, firstColumn);
            if (arrayRecord != null)
            {
              long cellIndex = RangeImpl.GetCellIndex(arrayRecord.FirstColumn, arrayRecord.FirstRow);
              if (!dictionary.ContainsKey(cellIndex))
              {
                formulaArrays[arrayRecord] = (object) null;
                dictionary.Add(cellIndex, (object) null);
              }
            }
          }
        }
        dictionary.Clear();
      }
      return formulaArrays;
    }
  }

  public bool AreFormulaArraysNotSeparated
  {
    get => this.GetAreArrayFormulasNotSeparated((Dictionary<ArrayRecord, object>) null);
  }

  protected internal bool CheckFormulaArraysNotSeparated(ICollection<ArrayRecord> colFormulas)
  {
    if (colFormulas == null)
      throw new ArgumentNullException(nameof (colFormulas));
    int firstRow = this.FirstRow;
    int firstColumn = this.FirstColumn;
    int lastRow = this.LastRow;
    int lastColumn = this.LastColumn;
    foreach (ArrayRecord colFormula in (IEnumerable<ArrayRecord>) colFormulas)
    {
      if (colFormula.FirstRow + 1 < firstRow || colFormula.LastRow + 1 > lastRow || colFormula.FirstColumn + 1 < firstColumn || colFormula.LastColumn + 1 > lastColumn)
        return false;
    }
    return true;
  }

  public int CellsCount
  {
    get => (this.LastRow - this.FirstRow + 1) * (this.LastColumn - this.FirstColumn + 1);
  }

  public FormatImpl InnerNumberFormat
  {
    get
    {
      int num = this.m_book.GetExtFormat((int) this.ExtendedFormatIndex).NumberFormatIndex;
      if (this.m_book.InnerFormats.Count > 14 && !this.m_book.InnerFormats.Contains(num))
        num = 14;
      return this.m_book.InnerFormats[num];
    }
  }

  public string AddressGlobal2007 => this.AddressGlobal;

  internal RowStorage RowStorage
  {
    get
    {
      return WorksheetHelper.GetOrCreateRow(this.Worksheet as IInternalWorksheet, this.Row - 1, false);
    }
  }

  internal static string DateSeperator
  {
    get
    {
      if (RangeImpl.m_dateSeperator == null)
        RangeImpl.m_dateSeperator = RangeImpl.GetDateSeperator();
      return RangeImpl.m_dateSeperator;
    }
  }

  internal static string TimeSeparator
  {
    get
    {
      if (RangeImpl.m_timeSeparator == null)
        RangeImpl.m_timeSeparator = RangeImpl.GetTimeSeperator();
      return RangeImpl.m_timeSeparator;
    }
  }

  protected int CurrentStyleNumber(string pre) => this.m_book.CurrentStyleNumber(pre);

  protected void OnLastColumnChanged()
  {
  }

  protected void OnFirstColumnChanged()
  {
  }

  protected void OnLastRowChanged()
  {
  }

  protected void OnFirstRowChanged()
  {
  }

  protected internal void OnStyleChanged(RangeImpl.TCellType oldType)
  {
    if (oldType == RangeImpl.TCellType.LabelSST && this.CellType != RangeImpl.TCellType.LabelSST)
    {
      string str = this.Value;
      if (str != null && str.Length != 0)
        this.m_rtfString.Clear();
    }
    this.SetChanged();
  }

  protected internal void OnValueChanged(string old, string value)
  {
    string s1 = (string) null;
    string s2 = (string) null;
    bool flag1 = false;
    bool flag2 = false;
    long result1 = 0;
    CultureInfo cultureInfo = this.AppImplementation.CheckAndApplySeperators();
    int currencyPositivePattern = cultureInfo.NumberFormat.CurrencyPositivePattern;
    string currencySymbol = cultureInfo.NumberFormat.CurrencySymbol;
    this.SetChanged();
    if (old != value)
      this.OnCellValueChanged((object) old, (object) value, (IRange) this);
    FormatImpl innerFormat = this.m_book.InnerFormats[this.m_book.InnerExtFormats[(int) this.ExtendedFormatIndex].NumberFormatIndex];
    int length = value != null ? value.Length : 0;
    if (length == 0)
    {
      if (!(this.Record is BlankRecord))
        this.Record = this.CreateRecordWithoutAdd(TBIFFRecord.Blank);
    }
    else
    {
      if (value == old)
        return;
      bool? nullable1 = this.IsStringsPreserved;
      if (!nullable1.HasValue)
        nullable1 = new bool?(this.m_worksheet.IsStringsPreserved);
      bool? nullable2 = nullable1;
      if ((!nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
        this.Text = value;
      else if (value[0] == '=' && length > 1 && value[1] != '&')
        this.SetFormula(value);
      else if (!this.DetectAndSetBoolErrValue(value) && (innerFormat.FormatType != ExcelFormatType.Number || !this.DetectAndSetFractionValue(value)))
      {
        DateTime dateTime = DateTime.FromOADate(0.0);
        double result2;
        bool flag3 = double.TryParse(value, Array.IndexOf<string>(this.floatNumberStyleCultures, Thread.CurrentThread.CurrentCulture.Name) >= 0 ? NumberStyles.Number : NumberStyles.Any, (IFormatProvider) cultureInfo, out result2) && !double.IsInfinity(result2);
        if (!flag3)
          flag3 = double.TryParse(value, Array.IndexOf<string>(this.floatNumberStyleCultures, Thread.CurrentThread.CurrentCulture.Name) >= 0 ? NumberStyles.Float : NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2) && !double.IsInfinity(result2);
        if (flag3)
          flag3 = this.m_worksheet.checkIsNumber(value, cultureInfo);
        bool flag4 = !flag3 && (this.m_worksheet.TryParseDateTime(value, out dateTime) || this.m_worksheet.TryParseExactDateTime(value, out dateTime));
        string str = (string) null;
        if (!flag3 && !flag4)
        {
          string dateSeparator = cultureInfo.DateTimeFormat.DateSeparator;
          string timeSeparator = cultureInfo.DateTimeFormat.TimeSeparator;
          if (dateSeparator == "/" && value.Contains("-"))
            str = value.Replace("-", "/");
          else if (dateSeparator == "-" && value.Contains("/"))
            str = value.Replace("/", "-");
          else if (dateSeparator == "." && value.Contains("/"))
            str = value.Replace("/", ".");
          else if (dateSeparator == "." && value.Contains("-"))
            str = value.Replace("-", ".");
          if (str != null)
            flag4 = this.m_worksheet.TryParseDateTime(str, out dateTime);
          else if (value.Contains(timeSeparator))
            flag4 = DateTime.TryParse(value, out dateTime);
        }
        if (flag4)
        {
          if (dateTime.Ticks < RangeImpl.MinAllowedDateTicks)
          {
            flag4 = false;
          }
          else
          {
            result2 = dateTime.ToOADate();
            if (result2 < 60.0 && this.Worksheet.IsDisplayZeros)
              --result2;
          }
        }
        bool flag5 = false;
        if (flag4 && innerFormat.FormatType == ExcelFormatType.General)
        {
          DateTime result3 = new DateTime();
          flag5 = DateTime.TryParseExact(value, this.Workbook.DateTimePatterns, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result3) || this.m_worksheet.TryParseExactDateTime(value, out result3);
        }
        if (value[0].ToString() == "%")
          s1 = value.Remove(0, 1);
        else if (value[value.Length - 1].ToString() == "%")
          s1 = value.Remove(value.Length - 1, 1);
        else if (value[0].ToString() == currencySymbol && this.NumberFormat == "General")
        {
          if (currencyPositivePattern % 2 == 0)
            flag1 = true;
          s2 = value.Remove(0, 1);
        }
        else if (value[value.Length - 1].ToString() == currencySymbol && this.NumberFormat == "General")
        {
          if (currencyPositivePattern % 2 != 0)
            flag1 = true;
          s2 = value.Remove(value.Length - 1, 1);
        }
        else if (value.EndsWith(currencySymbol, StringComparison.Ordinal))
        {
          if (currencyPositivePattern % 2 != 0)
            flag1 = true;
          s2 = value.Remove(value.Length - currencySymbol.Length, currencySymbol.Length);
        }
        double result4;
        bool flag6 = double.TryParse(s1, out result4);
        if (flag6)
        {
          flag6 = this.m_worksheet.checkIsNumber(s1, cultureInfo);
        }
        else
        {
          flag2 = double.TryParse(s2, out result4);
          if (flag2)
            flag2 = this.m_worksheet.checkIsNumber(s2, cultureInfo);
        }
        if (flag6)
        {
          if (this.NumberFormat == "@")
            this.RichText.Text = value;
          else
            this.SetNumber((double) (Convert.ToDecimal(s1) / 100M));
          if (this.NumberFormat == "General" || this.NumberFormat == "mm/dd/yyyy")
            this.FormatType = !long.TryParse(s1, out result1) ? ExcelFormatType.DecimalPercentage : ExcelFormatType.Percentage;
        }
        else if (flag2 && flag1)
        {
          this.SetNumber(Convert.ToDouble(s2));
          this.FormatType = ExcelFormatType.Currency;
        }
        else if ((flag3 || flag4) && value != double.NaN.ToString() && (innerFormat.FormatType == ExcelFormatType.General || innerFormat.FormatType == ExcelFormatType.Number || innerFormat.FormatType == ExcelFormatType.DateTime) && (!flag4 || innerFormat.FormatType != ExcelFormatType.General || flag5) && s2 == null && !flag1)
        {
          if (flag4 && value.Contains(RangeImpl.TimeSeparator) && !value.Contains(RangeImpl.DateSeperator) && str == null && (this.NumberFormat == "General" || innerFormat.IsTimeFormat(result2)))
          {
            if (this.NumberFormat == "General")
              this.SetTimeFormat(value);
            result2.ToString();
            result2 = this.GetSerialDateTimeFromDate(DateTime.Today) < result2 ? result2 - this.GetSerialDateTimeFromDate(DateTime.Today) : result2;
          }
          if (flag4 && str != null && str.Contains(RangeImpl.DateSeperator) && value.Contains(RangeImpl.TimeSeparator) && (this.NumberFormat == "General" || innerFormat.IsDateFormat(result2)))
          {
            DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            this.NumberFormat = $"{dateTimeFormat.ShortDatePattern} {dateTimeFormat.ShortTimePattern.Replace("tt", string.Empty)}";
          }
          else if (flag4)
            this.FormatType = ExcelFormatType.DateTime;
          else if (value.Contains("E+") && (this.NumberFormat == "General" || this.NumberFormat == "mm/dd/yyyy"))
            this.FormatType = ExcelFormatType.Exponential;
          if (flag4 && value.Contains(RangeImpl.TimeSeparator) && !value.Contains(RangeImpl.DateSeperator))
          {
            DateTime result5 = DateTime.MinValue;
            this.m_worksheet.TryParseExactDateTime(value, out result5);
            if (result5.TimeOfDay == TimeSpan.Zero && result5.Date == DateTime.Now.Date)
              this.SetNumber(0.0);
            else
              this.SetNumber(result2);
          }
          else
            this.SetNumber(result2);
        }
        else
        {
          value = this.CheckApostrophe(value);
          this.RichText.Text = value;
        }
      }
    }
    if (!(this.Parent is WorksheetImpl) || ((WorksheetImpl) this.Parent).CalcEngine == null || value == null)
      return;
    ((WorksheetImpl) this.Parent).OnValueChanged(this.Row, this.Column, value);
  }

  private double GetSerialDateTimeFromDate(DateTime dt)
  {
    DateTime inDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
    double num = dt.ToOADate() - CalcEngineHelper.ToOADate(inDateTime);
    double dateTimeFromDate = 1.0 + dt.ToOADate() - CalcEngineHelper.ToOADate(inDateTime);
    if (dateTimeFromDate > 59.0)
      ++dateTimeFromDate;
    return dateTimeFromDate;
  }

  private bool DetectAndSetFractionValue(string value)
  {
    double result1 = 0.0;
    double result2 = 0.0;
    Fraction fraction1 = (Fraction) null;
    string str = value;
    if (value.Contains("/"))
    {
      Fraction fraction2;
      if (value.Contains(" "))
      {
        string[] strArray = str.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (!double.TryParse(strArray[0], out result1))
          return false;
        fraction2 = new Fraction(result1, 1.0);
        if (strArray.Length > 1)
          str = strArray[1];
      }
      else
        fraction2 = new Fraction(0.0, 1.0);
      string[] strArray1 = str.Split(new char[1]{ '/' }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length > 0)
        double.TryParse(strArray1[0], out result1);
      if (strArray1.Length > 1)
      {
        if (double.TryParse(strArray1[1], out result2) && result2 != 0.0)
          fraction1 = new Fraction(result1, result2);
        if (fraction1 != null)
        {
          Fraction fraction3 = fraction2 + fraction1;
          this.SetNumber(fraction3.Numerator / fraction3.Denumerator);
          return true;
        }
      }
    }
    return false;
  }

  private void SetTimeFormat(string value)
  {
    bool flag = false;
    int num1 = 0;
    string amDesignator = CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator;
    string pmDesignator = CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator;
    string str1 = "h";
    string str2 = "mm";
    string str3 = "ss";
    string str4 = " AM/PM";
    string timeSeparator = RangeImpl.TimeSeparator;
    string str5 = " ";
    string source = value;
    if (source.Contains(timeSeparator))
    {
      while (source.Contains(str5) && source.IndexOf(str5) < source.IndexOf(timeSeparator))
        source = source.Remove(0, source.IndexOf(str5) + 1);
    }
    if ((CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, amDesignator, CompareOptions.IgnoreCase) >= 0 || CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, pmDesignator, CompareOptions.IgnoreCase) >= 0) && amDesignator != "" && pmDesignator != "")
      flag = true;
    int num2;
    for (; source.Contains(timeSeparator); source = source.Remove(num2, 1))
    {
      ++num1;
      num2 = source.IndexOf(timeSeparator);
      if (num1 == 1)
      {
        int result;
        int.TryParse(source.Substring(0, num2), out result);
        if (result >= 24)
          str1 = "[h]";
      }
    }
    switch (num1)
    {
      case 1:
        if (str1 == "[h]")
        {
          this.NumberFormat = str1 + timeSeparator + str2 + timeSeparator + str3;
          break;
        }
        if (flag)
        {
          this.NumberFormat = str1 + timeSeparator + str2 + str4;
          break;
        }
        this.NumberFormat = str1 + timeSeparator + str2;
        break;
      case 2:
        if (flag)
        {
          this.NumberFormat = str1 + timeSeparator + str2 + timeSeparator + str3 + str4;
          break;
        }
        this.NumberFormat = str1 + timeSeparator + str2 + timeSeparator + str3;
        break;
    }
  }

  internal string CheckApostrophe(string value)
  {
    if (value == null || value.Length == 0 || this.m_book.Loading)
      return value;
    if (value[0] == '\'')
    {
      this.CellStyle.IsFirstSymbolApostrophe = true;
      value = value.Substring(1);
    }
    else if (this.m_book.InnerExtFormats[(int) this.ExtendedFormatIndex].IsFirstSymbolApostrophe)
      this.CellStyle.IsFirstSymbolApostrophe = false;
    return value;
  }

  protected double ObjectToDouble(object value)
  {
    switch (value)
    {
      case double num1:
        return num1;
      case int num2:
        return Convert.ToDouble(num2);
      default:
        return double.Parse(value.ToString());
    }
  }

  protected RangeImpl ToggleGroup(ExcelGroupBy groupBy, bool isGroup, bool bCollapsed)
  {
    return this.ToggleGroup(groupBy, isGroup, bCollapsed, false);
  }

  internal RangeImpl ToggleGroup(
    ExcelGroupBy groupBy,
    bool isGroup,
    bool bCollapsed,
    bool isImport)
  {
    RowStorage rowStorage1 = (RowStorage) null;
    if (isGroup)
      this.SetWorksheetSize();
    if (this.m_outlineWrapperUtility == null)
      this.m_outlineWrapperUtility = new OutlineWrapperUtility();
    if (this.m_worksheet.ProtectContents)
      return this;
    int num1;
    int num2;
    RangeImpl.OutlineGetter outlineGetter;
    bool flag1;
    if (groupBy == ExcelGroupBy.ByRows)
    {
      num1 = this.FirstRow;
      num2 = this.LastRow;
      outlineGetter = new RangeImpl.OutlineGetter(this.GetRowOutline);
      flag1 = this.m_worksheet.PageSetup.IsSummaryRowBelow;
    }
    else
    {
      num1 = this.FirstColumn;
      num2 = this.LastColumn;
      outlineGetter = new RangeImpl.OutlineGetter(this.GetColumnOutline);
      flag1 = this.m_worksheet.PageSetup.IsSummaryColumnRight;
    }
    int start = num1;
    int previousOutlineLevel = -2;
    bool addGroup = true;
    int heightInRowUnits = (this.m_worksheet.Application as ApplicationImpl).StandardHeightInRowUnits;
    ExcelVersion version = this.m_worksheet.Workbook.Version;
    RecordTable table = this.m_worksheet.CellRecords.Table;
    bool flag2 = false;
    if (table.Rows.GetCount() == 0 && num2 - num1 != 0)
    {
      table.m_arrRows = new ArrayListEx(num2 - num1);
      flag2 = true;
    }
    for (int iOutlineIndex = num1; iOutlineIndex <= num2; ++iOutlineIndex)
    {
      int num3 = iOutlineIndex - 1;
      IOutline outline;
      if (groupBy == ExcelGroupBy.ByRows)
      {
        RowStorage rowStorage2;
        if (flag2)
        {
          ArrayListEx arrRows = table.m_arrRows;
          int index = num3;
          RowStorage cellCollection;
          rowStorage1 = cellCollection = table.CreateCellCollection(num3, heightInRowUnits, version);
          RowStorage rowStorage3 = cellCollection;
          arrRows[index] = cellCollection;
          rowStorage2 = rowStorage3;
          table.AccessRow(num3);
        }
        else
        {
          rowStorage2 = table.Rows[num3];
          if (rowStorage2 == null)
          {
            ArrayListEx arrRows = table.m_arrRows;
            int index = num3;
            RowStorage cellCollection;
            rowStorage1 = cellCollection = table.CreateCellCollection(num3, heightInRowUnits, version);
            RowStorage rowStorage4 = cellCollection;
            arrRows[index] = cellCollection;
            rowStorage2 = rowStorage4;
          }
        }
        outline = (IOutline) rowStorage2;
      }
      else
        outline = outlineGetter(iOutlineIndex);
      ushort outlineLevel = outline.OutlineLevel;
      if (outlineLevel != (ushort) 7 || !isGroup)
      {
        int num4;
        if (outlineLevel == (ushort) 0 && !isGroup)
          num4 = -1;
        else if (isGroup)
        {
          outline.OutlineLevel = (ushort) ((uint) outlineLevel + 1U);
          num4 = (int) outline.OutlineLevel;
        }
        else
        {
          outline.OutlineLevel = (ushort) ((uint) outlineLevel - 1U);
          num4 = (int) outline.OutlineLevel;
        }
        if (previousOutlineLevel == -2)
          previousOutlineLevel = num4;
        if (previousOutlineLevel != num4)
        {
          int end = iOutlineIndex - 1;
          this.UpdateRowColumn(start, end, previousOutlineLevel, groupBy, addGroup, isGroup, isImport);
          start = iOutlineIndex;
          previousOutlineLevel = num4;
        }
        if (outline.OutlineLevel == (ushort) 0)
          outline.IsHidden = false;
        else if (isGroup && (outline.OutlineLevel >= (ushort) 1 || bCollapsed) && !outline.IsHidden)
          outline.IsHidden = bCollapsed;
        if (iOutlineIndex == num2 && bCollapsed)
        {
          if (flag1)
            outline = outlineGetter(iOutlineIndex + 1);
          else if (num1 > 1)
            outline = outlineGetter(num1 - 1);
          outline.IsCollapsed = bCollapsed;
        }
      }
      else
        addGroup = false;
    }
    int end1 = num2;
    this.UpdateRowColumn(start, end1, previousOutlineLevel, groupBy, addGroup, isGroup, isImport);
    this.m_outlineWrapperUtility = (OutlineWrapperUtility) null;
    this.m_worksheet.m_outlineWrappers = (List<IOutlineWrapper>) null;
    return this;
  }

  private void UpdateRowColumn(
    int start,
    int end,
    int previousOutlineLevel,
    ExcelGroupBy groupBy,
    bool addGroup,
    bool isGroup,
    bool isImport)
  {
    if (isGroup && addGroup)
      this.GroupRowColumn(start, end, previousOutlineLevel, groupBy, isImport);
    else
      this.UngroupRowColumn(start, end, previousOutlineLevel, groupBy);
  }

  private void GroupRowColumn(
    int start,
    int end,
    int previousOutlineLevel,
    ExcelGroupBy groupBy,
    bool isImport)
  {
    if (groupBy == ExcelGroupBy.ByRows)
      this.m_outlineWrapperUtility.AddRowLevel(this.m_worksheet, start, end, previousOutlineLevel, false, isImport);
    else
      this.m_outlineWrapperUtility.AddColumnLevel(this.m_worksheet, previousOutlineLevel, start, end, false, isImport);
  }

  private void UngroupRowColumn(
    int start,
    int end,
    int previousOutlineLevel,
    ExcelGroupBy groupBy)
  {
    if (groupBy == ExcelGroupBy.ByRows)
      this.m_outlineWrapperUtility.SubRowLevel(this.m_worksheet, start, end, previousOutlineLevel + 1);
    else
      this.m_outlineWrapperUtility.SubColumnLevel(this.m_worksheet, start, end, previousOutlineLevel + 1);
  }

  public IOutline GetRowOutline(int iRowIndex)
  {
    return (IOutline) WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, iRowIndex - 1, true);
  }

  public void SubTotal(int groupBy, ConsolidationFunction function, int[] totalList)
  {
    this.SubTotal(groupBy, function, totalList, true, false, true);
  }

  public void SubTotal(
    int groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    if (totalList == null)
      return;
    SubTotalImpl subTotalImpl = new SubTotalImpl(this.m_worksheet);
    ++this.m_noOfSubtotals;
    bool flag = false;
    if (this.m_worksheet.CalcEngine == null)
    {
      this.m_worksheet.EnableSheetCalculations();
      flag = true;
    }
    this.LastRow = subTotalImpl.CalculateSubTotal(this.FirstRow, this.FirstColumn - 1, this.LastRow, this.LastColumn - 1, groupBy, function, this.m_noOfSubtotals, totalList, replace, pageBreaks, summaryBelowData, 1);
    this.m_worksheet.IsInsertingSubTotal = false;
    this.UpdateRowOffSet();
    this.UpdateFormulas();
    if (flag)
      this.m_worksheet.DisableSheetCalculations();
    this.m_worksheet.IsSubtotal = true;
  }

  public void SubTotal(
    int[] groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    if (totalList == null)
      return;
    SubTotalImpl subTotalImpl = new SubTotalImpl(this.m_worksheet);
    ++this.m_noOfSubtotals;
    bool flag = false;
    if (this.m_worksheet.CalcEngine == null)
    {
      this.m_worksheet.EnableSheetCalculations();
      flag = true;
    }
    for (int index = 0; index < groupBy.Length; ++index)
    {
      this.LastRow = subTotalImpl.CalculateSubTotal(this.FirstRow, this.FirstColumn - 1, this.LastRow, this.LastColumn - 1, groupBy[index], function, this.m_noOfSubtotals, totalList, replace, pageBreaks, summaryBelowData, groupBy.Length);
      this.UpdateRowOffSet();
      this.UpdateFormulas();
    }
    this.m_worksheet.IsInsertingSubTotal = false;
    if (flag)
      this.m_worksheet.DisableSheetCalculations();
    this.m_worksheet.IsSubtotal = true;
  }

  internal void UpdateRowOffSet()
  {
    RecordTable table = (this.Worksheet as WorksheetImpl).RecordsCells.Table;
    int iDeltaRow = 0;
    if ((this.Worksheet as WorksheetImpl).MovedRows == null)
      return;
    foreach (int key in (this.Worksheet as WorksheetImpl).MovedRows.Keys)
    {
      RowStorage row = WorksheetHelper.GetOrCreateRow(this.Worksheet as IInternalWorksheet, key - 1, false);
      if (row != null)
      {
        (this.Worksheet as WorksheetImpl).MovedRows.TryGetValue(key, out iDeltaRow);
        row.RowColumnOffset(iDeltaRow, 0, this.m_book.Application.RowStorageAllocationBlockSize);
      }
    }
    (this.Worksheet as WorksheetImpl).MovedRows = (SortedDictionary<int, int>) null;
  }

  internal void UpdateFormulas()
  {
    if (this.m_book.WorkbookFormulas == null || this.m_worksheet.InsertedRows == null)
      return;
    foreach (string key in this.m_book.WorkbookFormulas.Keys)
    {
      List<int> intList = (List<int>) null;
      this.Workbook.WorkbookFormulas.TryGetValue(key, out intList);
      int index = this.m_book.Worksheets[key].Index;
      intList.Sort();
      foreach (int num in intList)
        WorksheetHelper.GetOrCreateRow(this.m_book.Worksheets[key] as IInternalWorksheet, num - 1, false)?.UpdateSubTotalFormula(this.m_worksheet.InsertedRows, index, this.m_worksheet.Index, this.m_worksheet.Index, this.m_book);
    }
    this.m_worksheet.InsertedRows = (List<int>) null;
  }

  public IOutline GetColumnOutline(int iColumnIndex)
  {
    ColumnInfoRecord record = this.m_worksheet.ColumnInformation[iColumnIndex];
    if (record == null)
    {
      record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
      record.FirstColumn = record.LastColumn = (ushort) (iColumnIndex - 1);
      record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
      this.m_worksheet.ColumnInformation[iColumnIndex] = record;
    }
    return (IOutline) record;
  }

  private void SetWorksheetSize()
  {
    if (this.m_worksheet.FirstRow > this.FirstRow || this.m_worksheet.FirstRow == -1)
      this.m_worksheet.FirstRow = this.FirstRow;
    if (this.m_worksheet.LastRow < this.LastRow)
      this.m_worksheet.LastRow = this.LastRow;
    if (this.m_worksheet.FirstColumn > this.FirstColumn || this.m_worksheet.FirstColumn == int.MaxValue)
      this.m_worksheet.FirstColumn = this.FirstColumn;
    if (this.m_worksheet.LastColumn >= this.LastColumn && this.m_worksheet.LastColumn != int.MaxValue)
      return;
    this.m_worksheet.LastColumn = this.LastColumn;
  }

  internal void SetWorkbook(WorkbookImpl book) => this.m_book = book;

  private IOutline GetOrCreateOutline(
    ExcelGroupBy groupBy,
    IDictionary information,
    int iIndex,
    bool bThrowExceptions)
  {
    if (information == null)
      throw new ArgumentNullException(nameof (information));
    if (iIndex < 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    IOutline outline;
    if (information.Contains((object) iIndex))
    {
      outline = (IOutline) information[(object) iIndex];
    }
    else
    {
      if (groupBy == ExcelGroupBy.ByRows)
      {
        if (iIndex > this.m_book.MaxRowCount)
        {
          if (bThrowExceptions)
            throw new ArgumentOutOfRangeException(nameof (iIndex));
          return (IOutline) null;
        }
        RowRecord record = (RowRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Row);
        record.RowNumber = (ushort) (iIndex - 1);
        record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        record.Height = (ushort) this.m_worksheet.DefaultRowHeight;
        record.IsBadFontHeight = false;
        outline = (IOutline) record;
      }
      else
      {
        if (iIndex > this.m_book.MaxColumnCount)
        {
          if (bThrowExceptions)
            throw new ArgumentOutOfRangeException(nameof (iIndex));
          return (IOutline) null;
        }
        ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
        record.LastColumn = record.FirstColumn = (ushort) (iIndex - 1);
        record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
        outline = (IOutline) record;
      }
      information.Add((object) iIndex, (object) outline);
    }
    return outline;
  }

  protected string GetDisplayString()
  {
    switch (this.CellType)
    {
      case RangeImpl.TCellType.Formula:
        string formulaStringValue = this.FormulaStringValue;
        if (formulaStringValue != null && formulaStringValue.Length != 0)
          return this.FormulaStringValue;
        break;
      case RangeImpl.TCellType.RString:
      case RangeImpl.TCellType.LabelSST:
      case RangeImpl.TCellType.Label:
        return this.m_worksheet.GetText(this.m_iTopRow, this.m_iLeftColumn);
      case RangeImpl.TCellType.BoolErr:
        return this.Value;
      case RangeImpl.TCellType.RK:
        return this.ParseNumberFormat();
    }
    return string.Empty;
  }

  private string ParseNumberFormat()
  {
    StringBuilder stringBuilder = new StringBuilder();
    string[] strArray = this.GetNumberFormat().Split(';');
    int index = 0;
    for (int length = strArray.Length; index < length; ++index)
    {
      if (Array.IndexOf<char>(strArray[index].ToCharArray(), '@') >= 0)
      {
        string str = strArray[index - 1];
        char[] chArray = new char[1]{ '"' };
        foreach (string splitFormat in str.Split(chArray))
        {
          if (splitFormat.Contains("*"))
            stringBuilder.Append(" ");
          else if (!this.CheckUnnecessaryChar(splitFormat))
            stringBuilder.Append(splitFormat);
          else if (splitFormat.Contains("?"))
          {
            foreach (char ch in splitFormat.ToCharArray())
            {
              if (ch == '?')
              {
                stringBuilder.Append(" ");
                stringBuilder.Append(" ");
              }
            }
          }
        }
      }
    }
    return stringBuilder.ToString();
  }

  private bool CheckUnnecessaryChar(string splitFormat)
  {
    bool flag = false;
    foreach (char ch in splitFormat.ToCharArray())
    {
      if (Array.IndexOf<char>(this.unnecessaryChar, ch) >= 0)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  protected DateTime GetDateTime()
  {
    return UtilityMethods.ConvertNumberToDateTime(this.GetNumber(), this.m_book.Date1904);
  }

  internal void SetDifferedColumnWidth(RangeImpl sourceRange, RangeImpl destinationRange)
  {
    int length = sourceRange.Columns.Length;
    for (int index = 0; index < length; ++index)
      destinationRange.Columns[index].ColumnWidth = sourceRange.Columns[index].ColumnWidth;
  }

  internal void SetDifferedRowHeight(RangeImpl sourceRange, RangeImpl destinationRange)
  {
    int length = sourceRange.Rows.Length;
    for (int index = 0; index < length; ++index)
      destinationRange.Rows[index].RowHeight = sourceRange.Rows[index].RowHeight;
  }

  protected void SetDateTime(DateTime value)
  {
    double number = UtilityMethods.ConvertDateTimeToNumber(value);
    if (number >= 0.0)
    {
      this.SetNumber(number);
    }
    else
    {
      this.Text = value.ToShortDateString();
      this.NumberFormat = "mm/dd/yyyy";
    }
    if (this.m_rtfString == null)
      return;
    this.m_rtfString.Clear();
  }

  protected void SetTimeSpan(TimeSpan time)
  {
    string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
    string customizedFormat = this.GetCustomizedFormat(shortDatePattern);
    if (this.NumberFormat == shortDatePattern || this.NumberFormat == customizedFormat)
      this.NumberFormat = "h:mm:ss";
    this.SetNumber((double) time.Ticks / 864000000000.0);
    if (this.m_rtfString == null)
      return;
    this.m_rtfString.Clear();
  }

  protected double GetNumber()
  {
    double d = double.NaN;
    if (this.IsSingleCell)
    {
      switch (this.CellType)
      {
        case RangeImpl.TCellType.Formula:
          d = ((FormulaRecord) this.Record).Value;
          break;
        case RangeImpl.TCellType.Number:
          d = ((NumberRecord) this.Record).Value;
          break;
        case RangeImpl.TCellType.RK:
          d = ((RKRecord) this.Record).RKNumber;
          break;
      }
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
      d = migrantRangeImpl.GetNumber();
      if (!double.IsNaN(d))
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            migrantRangeImpl.ResetRowColumn(row, column);
            double number = migrantRangeImpl.GetNumber();
            if (d != number)
            {
              d = double.NaN;
              break;
            }
          }
        }
      }
    }
    return d;
  }

  protected void SetNumber(double value)
  {
    this.TryRemoveFormulaArrays();
    this.Record = this.CreateNumberRecord(value);
    if (this.m_rtfString == null)
      return;
    this.m_rtfString.Clear();
  }

  private void SetNumberAndFormat(double value, bool isPreserveFormat)
  {
    this.TryRemoveFormulaArrays();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    BiffRecordRaw numberRecord = this.CreateNumberRecord(value);
    ICellPositionFormat cellPositionFormat = numberRecord as ICellPositionFormat;
    int extendedFormatIndex = (int) cellPositionFormat.ExtendedFormatIndex;
    ExtendedFormatImpl extendedFormatImpl1 = innerExtFormats[extendedFormatIndex];
    FormatImpl innerFormat = this.m_book.InnerFormats[extendedFormatImpl1.NumberFormatIndex];
    if (innerFormat.FormatString != "General")
    {
      ExcelFormatType formatType = innerFormat.GetFormatType(value);
      switch (formatType)
      {
        case ExcelFormatType.General:
        case ExcelFormatType.Number:
          break;
        default:
          if (!isPreserveFormat && formatType != ExcelFormatType.Unknown)
          {
            int orCreateFormat = this.m_book.InnerFormats.FindOrCreateFormat("0.00");
            ExtendedFormatImpl format = extendedFormatImpl1.Clone() as ExtendedFormatImpl;
            format.NumberFormatIndex = orCreateFormat;
            ExtendedFormatImpl extendedFormatImpl2 = this.m_book.InnerExtFormats.Add(format);
            cellPositionFormat.ExtendedFormatIndex = (ushort) extendedFormatImpl2.Index;
            break;
          }
          break;
      }
    }
    this.Record = numberRecord;
    if (this.m_rtfString == null)
      return;
    this.m_rtfString.Clear();
  }

  private BiffRecordRaw CreateNumberRecord(double value)
  {
    BiffRecordRaw numberRecord = (BiffRecordRaw) this.m_worksheet.TryCreateRkRecord(this.m_iTopRow, this.m_iLeftColumn, value);
    if (numberRecord == null)
    {
      NumberRecord recordWithoutAdd = (NumberRecord) this.CreateRecordWithoutAdd(TBIFFRecord.Number);
      recordWithoutAdd.Value = value;
      numberRecord = (BiffRecordRaw) recordWithoutAdd;
    }
    return numberRecord;
  }

  protected void SetBoolean(bool value)
  {
    BoolErrRecord recordWithoutAdd = (BoolErrRecord) this.CreateRecordWithoutAdd(TBIFFRecord.BoolErr);
    recordWithoutAdd.IsErrorCode = false;
    recordWithoutAdd.BoolOrError = value ? (byte) 1 : (byte) 0;
    this.Record = (BiffRecordRaw) recordWithoutAdd;
    if (this.m_rtfString == null)
      return;
    this.m_rtfString.Clear();
  }

  protected void SetError(string strError)
  {
    switch (strError)
    {
      case null:
        throw new ArgumentNullException(nameof (strError));
      case "":
        throw new ArgumentException("string can't be empty");
      default:
        int errorCodeByString = this.GetErrorCodeByString(strError);
        if (errorCodeByString == -1)
          throw new ArgumentOutOfRangeException("Not error string");
        BoolErrRecord recordWithoutAdd = (BoolErrRecord) this.CreateRecordWithoutAdd(TBIFFRecord.BoolErr);
        recordWithoutAdd.IsErrorCode = true;
        recordWithoutAdd.BoolOrError = (byte) errorCodeByString;
        this.Record = (BiffRecordRaw) recordWithoutAdd;
        if (this.m_rtfString == null)
          break;
        this.m_rtfString.Clear();
        break;
    }
  }

  private int GetErrorCodeByString(string strError)
  {
    strError = strError != null && strError.Length != 0 ? strError.ToUpper() : throw new ArgumentNullException(nameof (strError));
    if (strError[0] != '#')
      strError = '#'.ToString() + strError;
    int num;
    return !FormulaUtil.ErrorNameToCode.TryGetValue(strError, out num) ? -1 : num;
  }

  protected internal void SetFormula(string value)
  {
    this.SetFormula(value, (Dictionary<string, string>) null, false);
  }

  protected internal void SetFormula(
    string value,
    Dictionary<string, string> hashWorksheetNames,
    bool bR1C1)
  {
    if (this.Workbook.Version == ExcelVersion.Excel97to2003 && value.Length > 1024 /*0x0400*/)
      throw new ArgumentException("The formula is too long. Length should not be longer than 1024");
    if (value.Length > 8192 /*0x2000*/)
      throw new ArgumentException("The formula is too long.Formulas length should not be longer then 8192");
    if (value != null && value.Contains("_xlfn."))
      value = value.Replace("_xlfn.", string.Empty);
    if (value[0] == '=')
      value = value.Substring(1, value.Length - 1);
    int iCellRow = this.Row - 1;
    int iCellColumn = this.Column - 1;
    Ptg[] array = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) this.m_worksheet, hashWorksheetNames, iCellRow, iCellColumn, bR1C1);
    FormulaRecord recordWithoutAdd = (FormulaRecord) this.CreateRecordWithoutAdd(TBIFFRecord.Formula);
    recordWithoutAdd.ParsedExpression = array;
    if (this.Parent is IWorksheet && ((IWorksheet) this.Parent).CalcEngine == null)
    {
      recordWithoutAdd.RecalculateAlways = true;
      recordWithoutAdd.CalculateOnOpen = true;
    }
    else
    {
      recordWithoutAdd.RecalculateAlways = false;
      recordWithoutAdd.CalculateOnOpen = false;
    }
    this.Record = (BiffRecordRaw) recordWithoutAdd;
    FormulaUtil.RaiseFormulaEvaluation((object) this, new EvaluateEventArgs((IRange) this, array));
  }

  [CLSCompliant(false)]
  protected internal void SetFormula(FormulaRecord record)
  {
    this.Record = record != null ? (BiffRecordRaw) record : throw new ArgumentNullException(nameof (record));
  }

  [CLSCompliant(false)]
  internal ExcelFormatType FormatType
  {
    get
    {
      return !this.ContainsNumber ? this.InnerNumberFormat.GetFormatType(this.m_worksheet.GetValue(this.Record as ICellPositionFormat, false)) : this.InnerNumberFormat.GetFormatType(this.GetNumber());
    }
    set
    {
      if (value == this.FormatType)
        return;
      switch (value)
      {
        case ExcelFormatType.Text:
          this.NumberFormat = "@";
          break;
        case ExcelFormatType.Number:
          this.NumberFormat = "0.00";
          break;
        case ExcelFormatType.DateTime:
          if (this.m_worksheet.IsImporting)
          {
            this.NumberFormat = $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt", "AM/PM")}";
            break;
          }
          this.NumberFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
          break;
        case ExcelFormatType.Percentage:
          this.NumberFormat = "0%";
          break;
        case ExcelFormatType.Currency:
          string str = (string) null;
          this.m_book.InnerFormats.CurrencyFormatStrings.TryGetValue(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, out str);
          if (str == null)
            break;
          this.NumberFormat = str;
          break;
        case ExcelFormatType.DecimalPercentage:
          this.NumberFormat = "0.00%";
          break;
        case ExcelFormatType.Exponential:
          this.NumberFormat = "0.00E+00";
          break;
      }
    }
  }

  [CLSCompliant(false)]
  protected FormatRecord Format
  {
    get
    {
      return this.m_book.InnerFormats[this.m_book.GetExtFormat((int) this.ExtendedFormatIndex).NumberFormatIndex].Record;
    }
  }

  protected void SetChanged() => this.m_worksheet.SetChanged();

  protected void CheckRange(int row, int column)
  {
    if (row < 1 || row > this.m_book.MaxRowCount || column < 1 || column > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException();
  }

  protected IWorksheet FindWorksheet(string sheetName)
  {
    return this.m_book.Worksheets[sheetName] ?? throw new ArgumentOutOfRangeException(nameof (sheetName));
  }

  public void ReparseFormulaString()
  {
    if (!this.IsSingleCell)
      return;
    if (this.CellType != RangeImpl.TCellType.Formula)
      return;
    try
    {
      this.SetFormula(this.Formula);
    }
    catch (ParseException ex)
    {
      if (this.m_book.Loading)
        this.m_book.AddForReparse((IReparse) this);
      else
        throw;
    }
  }

  private void MoveCellsUp(ExcelCopyRangeOptions options)
  {
    int row = this.LastRow + 1;
    int firstColumn = this.FirstColumn;
    int lastRow = this.m_worksheet.UsedRange.LastRow;
    int lastColumn = this.LastColumn;
    if (row > lastRow)
      return;
    this.m_worksheet.MoveRange(this.m_worksheet.Range[this.FirstRow, this.FirstColumn], this.m_worksheet.Range[row, firstColumn, lastRow, lastColumn], options, true);
  }

  private void MoveCellsLeft(ExcelCopyRangeOptions options)
  {
    int firstRow = this.FirstRow;
    int column = this.LastColumn + 1;
    int lastRow = this.LastRow;
    int lastColumn = this.m_worksheet.UsedRange.LastColumn;
    if (column > lastColumn)
      return;
    this.m_worksheet.MoveRange(this.m_worksheet.Range[this.FirstRow, this.FirstColumn], this.m_worksheet.Range[firstRow, column, lastRow, lastColumn], options, false);
  }

  private string ParseLabelSST(LabelSSTRecord label)
  {
    return (string) this.m_book.InnerSST.GetStringByIndex(label.SSTIndex);
  }

  private string ParseFormula(FormulaRecord formula) => this.ParseFormula(formula, false);

  private string ParseFormula(FormulaRecord formula, bool bR1C1ReferenceStyle)
  {
    try
    {
      FormulaUtil formulaUtil = this.m_book.FormulaUtil;
      ArrayRecord arrayRecord = this.m_worksheet.CellRecords.GetArrayRecord(formula.Row + 1, formula.Column + 1);
      string ptgArray;
      if (arrayRecord != null)
      {
        ptgArray = formulaUtil.ParsePtgArray(arrayRecord.Formula, arrayRecord.FirstRow, arrayRecord.FirstColumn, bR1C1ReferenceStyle, false);
      }
      else
      {
        formula.RecalculateAlways = true;
        formula.CalculateOnOpen = true;
        ptgArray = formulaUtil.ParsePtgArray(formula.ParsedExpression, this.Row - 1, this.Column - 1, bR1C1ReferenceStyle, false);
      }
      return "=" + ptgArray;
    }
    catch (ParseException ex)
    {
      if (this.m_book.Loading)
        this.m_book.AddForReparse((IReparse) this);
      else
        throw;
    }
    catch (Exception ex)
    {
      throw;
    }
    return (string) null;
  }

  public void SetRowHeight(double value, bool bIsBadFontHeight)
  {
    this.SetRowHeight(value, bIsBadFontHeight, true);
  }

  internal void SetRowHeight(double value, bool bIsBadFontHeight, bool raiseEvent)
  {
    if (value < 0.0 || value > 409.5)
      throw new ArgumentOutOfRangeException("RowHeight", "Row Height must be in range from 0 to 409.5");
    int num1 = this.FirstRow;
    int num2 = this.LastRow;
    if (this.LastRow - this.FirstRow > this.m_book.MaxRowCount - (this.LastRow - this.FirstRow) && this.LastRow == this.m_book.MaxRowCount)
    {
      num1 = 1;
      num2 = this.FirstRow - 1;
      this.m_worksheet.IsZeroHeight = true;
      this.m_worksheet.IsVisible = true;
    }
    else
      this.m_worksheet.IsVisible = false;
    int iRowIndex = num1;
    for (int index = num2; iRowIndex <= index; ++iRowIndex)
      this.m_worksheet.InnerSetRowHeight(iRowIndex, value, bIsBadFontHeight, MeasureUnits.Point, raiseEvent);
  }

  protected void CreateRichTextString()
  {
    if (this.IsSingleCell)
      this.m_rtfString = (IRTFWrapper) new RangeRichTextString(this.Application, (object) this.m_worksheet, this.m_iTopRow, this.m_iLeftColumn);
    else
      this.m_rtfString = (IRTFWrapper) new RTFStringArray((IRange) this);
  }

  private object TryCreateValue2()
  {
    if (this.IsBoolean)
      return (object) this.Boolean;
    if (this.HasNumber)
      return (object) this.Number;
    bool flag = false;
    if (this.NumberFormat.Equals(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, StringComparison.OrdinalIgnoreCase) && this.Record.TypeCode == TBIFFRecord.LabelSST)
      flag = true;
    if (!this.HasDateTime || flag)
      return (object) null;
    FormatImpl innerNumberFormat = this.InnerNumberFormat;
    if (this.Number >= CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime.ToOADate())
      return (object) this.Number;
    return !innerNumberFormat.IsTimeFormat(this.Number) ? (object) this.DateTime : (object) this.TimeSpan;
  }

  private bool DetectAndSetBoolErrValue(string strValue)
  {
    if (string.Compare(strValue, bool.TrueString, StringComparison.CurrentCultureIgnoreCase) == 0)
    {
      this.Boolean = true;
      return true;
    }
    if (string.Compare(strValue, bool.FalseString, StringComparison.CurrentCultureIgnoreCase) == 0)
    {
      this.Boolean = false;
      return true;
    }
    if (!FormulaUtil.ErrorNameToCode.ContainsKey(strValue))
      return false;
    this.Error = strValue;
    return true;
  }

  protected internal void SetLabelSSTIndex(int index)
  {
    if (index == -1)
    {
      if (this.CellType == RangeImpl.TCellType.Blank)
        return;
      this.Record = this.CreateRecordWithoutAdd(TBIFFRecord.Blank);
    }
    else
    {
      if (index < 0 || index >= this.m_book.InnerSST.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      LabelSSTRecord recordWithoutAdd = (LabelSSTRecord) this.CreateRecordWithoutAdd(TBIFFRecord.LabelSST);
      recordWithoutAdd.SSTIndex = index;
      this.Record = (BiffRecordRaw) recordWithoutAdd;
    }
  }

  private void TryRemoveFormulaArrays()
  {
    Dictionary<ArrayRecord, object> formulaArrays = this.FormulaArrays;
    if (formulaArrays == null || formulaArrays.Count == 0)
      return;
    ICollection<ArrayRecord> keys = (ICollection<ArrayRecord>) formulaArrays.Keys;
    if (!this.CheckFormulaArraysNotSeparated(keys))
      throw new InvalidRangeException("Can't set value.");
    this.m_worksheet.RemoveArrayFormulas(keys, false);
  }

  public void SetDataValidation(DataValidationImpl dv)
  {
    this.m_dataValidation = dv != null ? this.AppImplementation.CreateDataValidationWrapper(this, dv) : throw new ArgumentNullException(nameof (dv));
  }

  private void BlankCell() => this.Record = this.CreateRecord(TBIFFRecord.Blank);

  public void AddComment(ICommentShape comment)
  {
    if (comment == null)
      throw new ArgumentNullException(nameof (comment));
    ((TextBoxShapeBase) this.AddComment()).CopyFrom((TextBoxShapeBase) comment, (Dictionary<int, int>) null);
  }

  protected internal void SetParent(WorksheetImpl parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    if (this.Parent == parent)
      return;
    this.m_worksheet = parent;
    this.m_book = parent.ParentWorkbook;
  }

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (!(this.Record is FormulaRecord record))
      return;
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    Ptg[] parsedExpression = record.ParsedExpression;
    if (!this.m_book.FormulaUtil.UpdateNameIndex(parsedExpression, arrNewIndex))
      return;
    record.ParsedExpression = parsedExpression;
  }

  private BiffRecordRaw CreateRecord(TBIFFRecord recordType)
  {
    BiffRecordRaw recordWithoutAdd = this.CreateRecordWithoutAdd(recordType);
    this.m_worksheet.InnerSetCell(this.m_iLeftColumn, this.m_iTopRow, recordWithoutAdd);
    return recordWithoutAdd;
  }

  private BiffRecordRaw CreateRecordWithoutAdd(TBIFFRecord recordType)
  {
    return this.m_worksheet.GetRecord(recordType, this.m_iTopRow, this.m_iLeftColumn);
  }

  public void UpdateRange(int iFirstRow, int iFirstColumn, int iLastRow, int iLastColumn)
  {
    this.FirstRow = iFirstRow;
    this.FirstColumn = iFirstColumn;
    this.LastRow = iLastRow;
    this.LastColumn = iLastColumn;
    this.ResetCells();
  }

  public bool ContainsNumber
  {
    get
    {
      switch (this.CellType)
      {
        case RangeImpl.TCellType.Formula:
          return this.FormulaStringValue == null;
        case RangeImpl.TCellType.Number:
        case RangeImpl.TCellType.RK:
          return true;
        default:
          return false;
      }
    }
  }

  internal static string GetDateSeperator()
  {
    return CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator;
  }

  private static string GetTimeSeperator()
  {
    return CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator;
  }

  private IRange ParseR1C1Reference(string strReference)
  {
    switch (strReference)
    {
      case null:
        throw new ArgumentNullException(nameof (strReference));
      case "":
        throw new ArgumentException("strReference - string cannot be empty.");
      default:
        string[] strArray = strReference.Split(':');
        int length = strArray.Length;
        if (length > 2)
          throw new ArgumentOutOfRangeException(nameof (strReference));
        Rectangle rec = Rectangle.FromLTRB(1, 1, this.m_book.MaxColumnCount, this.m_book.MaxRowCount);
        Rectangle r1C1Expression = this.ParseR1C1Expression(strArray[0], rec, true);
        if (length == 2)
          r1C1Expression = this.ParseR1C1Expression(strArray[1], r1C1Expression, false);
        return this.Worksheet[r1C1Expression.Top, r1C1Expression.Left, r1C1Expression.Bottom, r1C1Expression.Right];
    }
  }

  private Rectangle ParseR1C1Expression(string strName, Rectangle rec, bool bIsFirst)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentOutOfRangeException("strName is empty.");
      default:
        int num = strName.IndexOf('C');
        bool flag1 = strName[0] == 'R';
        bool flag2 = num != -1;
        if (!flag1 && !flag2)
          throw new ArgumentOutOfRangeException("strReference", "Can't locate row or column section.");
        string strValue = flag2 ? strName.Substring(num + 1) : (string) null;
        int length = (flag2 ? num : strName.Length) - 1;
        int indexFromR1C1_1 = this.GetIndexFromR1C1(flag1 ? strName.Substring(1, length) : (string) null, true);
        int indexFromR1C1_2 = this.GetIndexFromR1C1(strValue, false);
        if (flag1)
        {
          if (bIsFirst)
          {
            rec.Y = indexFromR1C1_1;
            rec.Height = 0;
          }
          else
            rec.Height = indexFromR1C1_1 - rec.Y;
        }
        if (flag2)
        {
          if (bIsFirst)
          {
            rec.X = indexFromR1C1_2;
            rec.Width = 0;
          }
          else
            rec.Width = indexFromR1C1_2 - rec.X;
        }
        return rec;
    }
  }

  private int GetIndexFromR1C1(string strValue, bool bRow)
  {
    if (strValue == null)
      return !bRow ? this.m_book.MaxColumnCount : this.m_book.MaxRowCount;
    int length = strValue.Length;
    if (length == 0)
      return !bRow ? this.Column : this.Row;
    bool flag = false;
    if (strValue[0] == '[' && strValue[length - 1] == ']')
    {
      strValue = strValue.Substring(1, length - 2);
      flag = true;
    }
    double result;
    int indexFromR1C1 = double.TryParse(strValue, NumberStyles.Integer, (IFormatProvider) null, out result) && result >= (double) int.MinValue && result <= (double) int.MaxValue ? (int) result : throw new ApplicationException("Cannot parse expression.");
    if (flag)
      indexFromR1C1 += bRow ? this.Row : this.Column;
    return indexFromR1C1;
  }

  private string GetFormulaArray(bool bR1C1)
  {
    if (this.CellType != RangeImpl.TCellType.Formula && !this.IsSingleCell)
      return (string) null;
    string formulaArray1 = (string) null;
    if (this.IsSingleCell)
    {
      if (this.Record is FormulaRecord record && this.m_worksheet.IsArrayFormula(record))
        formulaArray1 = this.ParseFormula(record, bR1C1);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
      formulaArray1 = migrantRangeImpl.GetFormulaArray(bR1C1);
      if (formulaArray1 != null)
      {
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            migrantRangeImpl.ResetRowColumn(row, column);
            string formulaArray2 = migrantRangeImpl.GetFormulaArray(bR1C1);
            if (formulaArray1 != formulaArray2)
            {
              formulaArray1 = (string) null;
              break;
            }
          }
        }
      }
    }
    return formulaArray1;
  }

  private void SetFormulaArray(string value, bool bR1C1)
  {
    int num1 = value != null ? value.Length : throw new ArgumentNullException("FormulaArray");
    if (num1 == 0)
      throw new ArgumentException("FormulaArray can't be empty");
    if (value.StartsWith("{=") && value[num1 - 1] == '}')
    {
      value = value.Substring(2, num1 - 3);
      int num2 = num1 - 3;
    }
    else if (value[0] == '=')
      value = value.Substring(1, num1 - 1);
    this.TryRemoveFormulaArrays();
    ExcelParseFormulaOptions options = ExcelParseFormulaOptions.RootLevel | ExcelParseFormulaOptions.InArray;
    if (bR1C1)
      options |= ExcelParseFormulaOptions.UseR1C1;
    Ptg[] ptgArray = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) this.m_worksheet, (Dictionary<Type, ReferenceIndexAttribute>) null, 0, (Dictionary<string, string>) null, options, this.Row - 1, this.Column - 1);
    ArrayRecord record = (ArrayRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Array);
    record.Formula = ptgArray;
    record.IsRecalculateOnOpen = true;
    record.FirstRow = this.FirstRow - 1;
    record.FirstColumn = this.FirstColumn - 1;
    record.LastRow = this.LastRow - 1;
    record.LastColumn = this.LastColumn - 1;
    this.SetFormulaArrayRecord(record);
  }

  [CLSCompliant(false)]
  public void SetFormulaArrayRecord(ArrayRecord record) => this.SetFormulaArrayRecord(record, -1);

  [CLSCompliant(false)]
  public void SetFormulaArrayRecord(ArrayRecord record, int iXFIndex)
  {
    Ptg[] ptgArray = new Ptg[1]
    {
      FormulaUtil.CreatePtg(FormulaToken.tExp, (object) record.FirstRow, (object) record.FirstColumn)
    };
    FormulaRecord record1 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
    record1.ParsedExpression = ptgArray;
    if (iXFIndex != -1)
      record1.ExtendedFormatIndex = (ushort) iXFIndex;
    if (this.IsSingleCell)
    {
      this.UpdateRecord((ICellPositionFormat) record1, this, iXFIndex);
      this.Record = (BiffRecordRaw) record1;
    }
    else
    {
      RangeImpl cell1 = (RangeImpl) this.m_worksheet[this.FirstRow, this.FirstColumn];
      this.UpdateRecord((ICellPositionFormat) record1, cell1, iXFIndex);
      cell1.Record = (BiffRecordRaw) record1;
      for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
      {
        for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
        {
          RangeImpl cell2 = (RangeImpl) this.m_worksheet[firstRow, firstColumn];
          record1 = (FormulaRecord) record1.Clone();
          this.UpdateRecord((ICellPositionFormat) record1, cell2, iXFIndex);
          cell2.SetFormula(record1);
        }
      }
    }
    this.m_worksheet.CellRecords.SetArrayFormula(record);
  }

  private void UpdateRecord(ICellPositionFormat record, RangeImpl cell, int iXFIndex)
  {
    record.Row = cell.FirstRow - 1;
    record.Column = cell.FirstColumn - 1;
    if (iXFIndex != -1)
      return;
    record.ExtendedFormatIndex = cell.ExtendedFormatIndex;
  }

  private int NormalizeRowIndex(int iRow, int iStartCol, int iEndCol)
  {
    switch (iRow)
    {
      case -4:
        return this.m_worksheet.CellRecords.GetMinimumRowIndex(iStartCol, iEndCol);
      case -3:
        return this.m_worksheet.CellRecords.GetMaximumRowIndex(iStartCol, iEndCol);
      case -2:
        return this.m_book.MaxRowCount;
      case -1:
        return 1;
      default:
        return iRow;
    }
  }

  private int NormalizeColumnIndex(int iColumn, int iStartRow, int iEndRow)
  {
    switch (iColumn)
    {
      case -4:
        return this.m_worksheet.CellRecords.GetMinimumColumnIndex(iStartRow, iEndRow);
      case -3:
        return this.m_worksheet.CellRecords.GetMaximumColumnIndex(iStartRow, iEndRow);
      case -2:
        return this.m_book.MaxColumnCount;
      case -1:
        return 1;
      default:
        return iColumn;
    }
  }

  private DataValidationImpl FindDataValidation()
  {
    return this.m_worksheet.DVTable.FindDataValidation(RangeImpl.GetCellIndex(this.m_iLeftColumn, this.m_iTopRow));
  }

  public void PartialClear()
  {
    this.m_cells = (List<IRange>) null;
    this.m_style = (Syncfusion.XlsIO.Implementation.CellStyle) null;
    this.m_bCells = false;
    this.m_dataValidation = (DataValidationWrapper) null;
    this.m_rtfString = (IRTFWrapper) null;
  }

  protected void SetBorderToSingleCell(
    ExcelBordersIndex borderIndex,
    ExcelLineStyle borderLine,
    ExcelKnownColors borderColor)
  {
    if (!this.IsSingleCell)
      throw new NotSupportedException("Supports only for single cell.");
    IBorder border = this.Borders[borderIndex];
    border.LineStyle = borderLine;
    if (border.Color == ExcelKnownColors.BlackCustom && borderColor == ExcelKnownColors.Black || border.Color == borderColor)
      return;
    border.Color = borderColor;
  }

  private void CollapseExpand(ExcelGroupBy groupBy, bool isCollapsed, ExpandCollapseFlags flags)
  {
    if (this.m_worksheet.OutlineWrappers != null)
      this.m_worksheet.OutlineWrappers = (List<IOutlineWrapper>) null;
    int iStartIndex;
    int iEndIndex;
    int iMaxIndex;
    bool bLastIndex;
    RangeImpl.OutlineGetter outlineGetter;
    if (groupBy == ExcelGroupBy.ByRows)
    {
      iStartIndex = this.Row;
      iEndIndex = this.LastRow;
      iMaxIndex = this.m_book.MaxRowCount;
      bLastIndex = this.m_worksheet.PageSetup.IsSummaryRowBelow;
      outlineGetter = new RangeImpl.OutlineGetter(this.GetRowOutline);
    }
    else
    {
      iStartIndex = this.Column;
      iEndIndex = this.LastColumn;
      iMaxIndex = this.m_book.MaxColumnCount;
      bLastIndex = this.m_worksheet.PageSetup.IsSummaryColumnRight;
      outlineGetter = new RangeImpl.OutlineGetter(this.GetColumnOutline);
    }
    this.CollapseExpand(isCollapsed, iStartIndex, iEndIndex, iMaxIndex, bLastIndex, outlineGetter, flags);
  }

  private void CollapseExpand(
    bool isCollapsed,
    int iStartIndex,
    int iEndIndex,
    int iMaxIndex,
    bool bLastIndex,
    RangeImpl.OutlineGetter outlineGetter,
    ExpandCollapseFlags flags)
  {
    bool includeSubgroups = (flags & ExpandCollapseFlags.IncludeSubgroups) != ExpandCollapseFlags.Default;
    int iOutlineIndex = bLastIndex ? iEndIndex : iStartIndex - 1;
    if (iOutlineIndex <= iMaxIndex && iOutlineIndex > 0 && iEndIndex != iMaxIndex && bLastIndex)
      outlineGetter(iOutlineIndex + 1).IsCollapsed = isCollapsed;
    else if (iStartIndex > 1)
      outlineGetter(iOutlineIndex).IsCollapsed = isCollapsed;
    int num = (int) Math.Min(outlineGetter(iStartIndex).OutlineLevel, outlineGetter(iEndIndex).OutlineLevel);
    int iStartIndex1 = iStartIndex;
    int iEndIndex1 = iEndIndex;
    bool flag = this.IsParentGroupVisible(ref iStartIndex1, ref iEndIndex1, iMaxIndex, outlineGetter);
    if (!flag && (flags & ExpandCollapseFlags.ExpandParent) != ExpandCollapseFlags.Default)
    {
      flag = true;
      this.CollapseExpand(isCollapsed, iStartIndex1, iEndIndex1, iMaxIndex, bLastIndex, outlineGetter, ExpandCollapseFlags.ExpandParent);
    }
    if (isCollapsed)
    {
      this.SetHiddenState(iStartIndex, iEndIndex, outlineGetter, true);
    }
    else
    {
      if (!flag)
        return;
      this.ExpandOutlines(iStartIndex, iEndIndex, outlineGetter, includeSubgroups, bLastIndex);
    }
  }

  private void SetHiddenState(
    int iStartIndex,
    int iEndIndex,
    RangeImpl.OutlineGetter outlineGetter,
    bool state)
  {
    for (int iOutlineIndex = iStartIndex; iOutlineIndex <= iEndIndex; ++iOutlineIndex)
      outlineGetter(iOutlineIndex).IsHidden = state;
  }

  private void ExpandOutlines(
    int iStartIndex,
    int iEndIndex,
    RangeImpl.OutlineGetter outlineGetter,
    bool includeSubgroups,
    bool bLastIndex)
  {
    if (includeSubgroups)
    {
      this.SetHiddenState(iStartIndex, iEndIndex, outlineGetter, false);
    }
    else
    {
      int delta;
      if (bLastIndex)
      {
        this.SwapValues(ref iStartIndex, ref iEndIndex);
        delta = -1;
      }
      else
        delta = 1;
      int iOutlineIndex = iStartIndex;
      for (int index = iEndIndex + delta; iOutlineIndex != index; iOutlineIndex += delta)
      {
        IOutline outline1 = outlineGetter(iOutlineIndex);
        if (outline1.IsCollapsed)
        {
          IOutline outline2 = outlineGetter(iOutlineIndex + delta);
          if ((int) outline1.OutlineLevel >= (int) outline2.OutlineLevel)
          {
            outline1.IsCollapsed = false;
            outline1.IsHidden = false;
          }
          else
          {
            iOutlineIndex = this.FindGroupEdge(iOutlineIndex + delta, delta, int.MaxValue, outlineGetter, (int) outline2.OutlineLevel);
            outline1.IsHidden = false;
          }
        }
        else
          outline1.IsHidden = false;
      }
    }
  }

  private void SwapValues(ref int iStartIndex, ref int iEndIndex)
  {
    int num = iEndIndex;
    iEndIndex = iStartIndex;
    iStartIndex = num;
  }

  private bool IsParentGroupVisible(
    ref int iStartIndex,
    ref int iEndIndex,
    int iMaxIndex,
    RangeImpl.OutlineGetter outlineGetter)
  {
    if (outlineGetter(iStartIndex).OutlineLevel <= (ushort) 1)
      return true;
    int firstWithLowerLevel1 = this.FindFirstWithLowerLevel(iStartIndex, -1, iMaxIndex, outlineGetter);
    int firstWithLowerLevel2 = this.FindFirstWithLowerLevel(iEndIndex, 1, iMaxIndex, outlineGetter);
    int parentGroupLevel = Math.Min(firstWithLowerLevel1 > 0 ? (int) outlineGetter(firstWithLowerLevel1).OutlineLevel : 0, firstWithLowerLevel2 > 0 ? (int) outlineGetter(firstWithLowerLevel2).OutlineLevel : 0);
    if (parentGroupLevel == 0)
      return true;
    int groupEdge1 = this.FindGroupEdge(iStartIndex, -1, iMaxIndex, outlineGetter, parentGroupLevel);
    int groupEdge2 = this.FindGroupEdge(iEndIndex, 1, iMaxIndex, outlineGetter, parentGroupLevel);
    int outlineLevel = (int) outlineGetter(groupEdge1).OutlineLevel;
    iStartIndex = groupEdge1;
    iEndIndex = groupEdge2;
    return this.FindVisibleOutline(iStartIndex, iEndIndex, outlineGetter, outlineLevel) != -1;
  }

  private int FindFirstWithLowerLevel(
    int startIndex,
    int delta,
    int maximum,
    RangeImpl.OutlineGetter outlineGetter)
  {
    int outlineLevel = (int) outlineGetter(startIndex).OutlineLevel;
    int firstWithLowerLevel = -1;
    for (int iOutlineIndex = startIndex + delta; iOutlineIndex > 0 && iOutlineIndex <= maximum; iOutlineIndex += delta)
    {
      if ((int) outlineGetter(iOutlineIndex).OutlineLevel < outlineLevel)
      {
        firstWithLowerLevel = iOutlineIndex;
        break;
      }
    }
    return firstWithLowerLevel;
  }

  private int FindGroupEdge(
    int startIndex,
    int delta,
    int maximum,
    RangeImpl.OutlineGetter outlineGetter,
    int parentGroupLevel)
  {
    int iOutlineIndex = startIndex;
    do
    {
      iOutlineIndex += delta;
    }
    while (iOutlineIndex > 0 && iOutlineIndex <= maximum && (int) outlineGetter(iOutlineIndex).OutlineLevel >= parentGroupLevel);
    return iOutlineIndex - delta;
  }

  private int FindVisibleOutline(
    int startIndex,
    int endIndex,
    RangeImpl.OutlineGetter outlineGetter,
    int outlineLevel)
  {
    int visibleOutline = -1;
    for (int iOutlineIndex = startIndex; iOutlineIndex <= endIndex; ++iOutlineIndex)
    {
      IOutline outline = outlineGetter(iOutlineIndex);
      if ((int) outline.OutlineLevel == outlineLevel && !outline.IsHidden)
      {
        visibleOutline = iOutlineIndex;
        break;
      }
    }
    return visibleOutline;
  }

  internal IList<object> GetUniqueValues(ref PivotDataType fieldType)
  {
    bool flag = true;
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    int row1 = this.Row;
    int column = this.Column;
    int lastRow = this.LastRow;
    FormatImpl innerNumberFormat = this.InnerNumberFormat;
    IList<object> uniqueValues = (IList<object>) new List<object>();
    for (int row2 = row1; row2 <= lastRow; ++row2)
    {
      WorksheetImpl.TRangeValueType cellType = this.m_worksheet.GetCellType(row2, column, false);
      string str = (string) null;
      object obj1;
      string key;
      switch (cellType)
      {
        case WorksheetImpl.TRangeValueType.Blank:
          fieldType |= PivotDataType.Blank;
          obj1 = (object) str;
          key = "Blank_Key";
          break;
        case WorksheetImpl.TRangeValueType.Boolean:
          obj1 = (object) this.m_worksheet.GetBoolean(row2, column);
          fieldType |= PivotDataType.Boolean;
          key = obj1.ToString();
          break;
        case WorksheetImpl.TRangeValueType.Number:
          double number = this.m_worksheet.GetNumber(row2, column);
          if (!double.IsNaN(number))
          {
            ExcelFormatType formatType = innerNumberFormat.GetFormatType(number);
            if (formatType == ExcelFormatType.Unknown && number == 0.0)
              formatType = innerNumberFormat.GetFormatType(1.0);
            if (formatType == ExcelFormatType.Number || formatType == ExcelFormatType.General)
            {
              obj1 = (object) number;
              if (flag)
              {
                fieldType |= PivotDataType.Number;
                fieldType |= PivotDataType.Integer;
              }
              if (number - Math.Floor(number) > 0.0)
              {
                fieldType &= ~PivotDataType.Integer;
                flag = false;
              }
            }
            else
            {
              fieldType |= PivotDataType.Date;
              object obj2 = innerNumberFormat.IsTimeFormat(number) ? (object) DateTime.FromOADate(number).TimeOfDay : (object) UtilityMethods.ConvertNumberToDateTime(number, this.m_book.Date1904);
              DateTime dateTime = !(obj2 is TimeSpan) ? (DateTime) obj2 : Convert.ToDateTime(obj2.ToString());
              dateTime = dateTime.AddMilliseconds((double) -dateTime.Millisecond);
              obj1 = (object) dateTime;
            }
          }
          else
            obj1 = (object) number;
          key = obj1.ToString().ToString((IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case WorksheetImpl.TRangeValueType.Formula:
          bool isString = false;
          obj1 = this.GetFormulaValue(ref fieldType, row2, column, ref isString);
          key = obj1.ToString().ToLower();
          break;
        case WorksheetImpl.TRangeValueType.String:
          string text = this.m_worksheet.GetText(row2, column);
          if (text != null && text.Length > 0 || text == string.Empty)
          {
            if (text.Length > (int) byte.MaxValue)
              fieldType |= PivotDataType.LongText;
            fieldType |= PivotDataType.String;
          }
          else
            fieldType |= PivotDataType.Blank;
          obj1 = (object) text;
          key = text.ToLower();
          break;
        default:
          obj1 = (object) this.m_worksheet[row2, column].DisplayText;
          fieldType |= PivotDataType.String;
          key = obj1.ToString().ToLower();
          break;
      }
      if (key != null && !dictionary.ContainsKey(key))
      {
        dictionary.Add(key, obj1);
        uniqueValues.Add(obj1);
      }
    }
    return uniqueValues;
  }

  internal object GetFormulaValue(
    ref PivotDataType fieldType,
    int row,
    int column,
    ref bool isString)
  {
    RangeImpl.TCellType cellType = (this.m_worksheet[row, column] as RangeImpl).CellType;
    string formulaStringValue = this.m_worksheet.GetFormulaStringValue(row, column);
    object formulaValue;
    if ((this.m_worksheet.GetCellType(row, column, true) & WorksheetImpl.TRangeValueType.Boolean) == WorksheetImpl.TRangeValueType.Boolean)
    {
      formulaValue = (object) this.m_worksheet.GetFormulaBoolValue(row, column);
      fieldType |= PivotDataType.Boolean;
    }
    else
    {
      if (formulaStringValue != null)
      {
        switch (cellType)
        {
          case RangeImpl.TCellType.Formula:
          case RangeImpl.TCellType.RString:
          case RangeImpl.TCellType.LabelSST:
          case RangeImpl.TCellType.Label:
            formulaValue = (object) this.m_worksheet.GetFormulaStringValue(row, column);
            fieldType |= PivotDataType.String;
            isString = true;
            goto label_14;
          case RangeImpl.TCellType.Blank:
            formulaValue = (object) string.Empty;
            fieldType |= PivotDataType.Blank;
            isString = true;
            goto label_14;
          case RangeImpl.TCellType.Number:
          case RangeImpl.TCellType.RK:
            break;
          default:
            formulaValue = (object) (this.m_worksheet[row, column] as RangeImpl).GetDisplayString();
            fieldType |= PivotDataType.String;
            isString = true;
            goto label_14;
        }
      }
      double formulaNumberValue = this.m_worksheet.GetFormulaNumberValue(row, column);
      if (!double.IsNaN(formulaNumberValue))
      {
        ExcelFormatType formatType = this.InnerNumberFormat.GetFormatType(formulaNumberValue);
        if (formatType == ExcelFormatType.Unknown && formulaNumberValue == 0.0)
          formatType = this.InnerNumberFormat.GetFormatType(1.0);
        if (formatType == ExcelFormatType.Number || formatType == ExcelFormatType.General)
        {
          formulaValue = (object) formulaNumberValue;
          fieldType |= PivotDataType.Number;
          fieldType |= formulaNumberValue > (double) int.MaxValue || formulaNumberValue < (double) int.MinValue || Math.Round(formulaNumberValue) != formulaNumberValue ? PivotDataType.Float : PivotDataType.Integer;
        }
        else
        {
          FormatImpl innerNumberFormat = this.InnerNumberFormat;
          fieldType |= PivotDataType.Date;
          formulaValue = innerNumberFormat.IsTimeFormat(formulaNumberValue) ? (object) TimeSpan.FromDays(formulaNumberValue) : (object) UtilityMethods.ConvertNumberToDateTime(formulaNumberValue, this.m_book.Date1904);
        }
      }
      else
        formulaValue = (object) formulaNumberValue;
    }
label_14:
    return formulaValue;
  }

  private string GetFormulaValue(int row, int column, FormatImpl formatImpl)
  {
    string formulaValue = (string) null;
    RangeImpl.TCellType cellType = (this.m_worksheet[row, column] as RangeImpl).CellType;
    string formulaStringValue1 = this.m_worksheet.GetFormulaStringValue(row, column);
    if ((this.m_worksheet.GetCellType(row, column, true) & WorksheetImpl.TRangeValueType.Boolean) == WorksheetImpl.TRangeValueType.Boolean)
    {
      formulaValue = this.m_worksheet.GetFormulaBoolValue(row, column).ToString();
    }
    else
    {
      if (formulaStringValue1 != null)
      {
        switch (cellType)
        {
          case RangeImpl.TCellType.Formula:
          case RangeImpl.TCellType.RString:
          case RangeImpl.TCellType.LabelSST:
          case RangeImpl.TCellType.Label:
            string formulaStringValue2 = this.m_worksheet.GetFormulaStringValue(row, column);
            formatImpl.ApplyFormat(formulaStringValue2);
            goto label_8;
          case RangeImpl.TCellType.Blank:
            formulaValue = "";
            goto label_8;
          case RangeImpl.TCellType.Number:
          case RangeImpl.TCellType.RK:
            break;
          default:
            formulaValue = (this.m_worksheet[row, column] as RangeImpl).GetDisplayString();
            goto label_8;
        }
      }
      double formulaNumberValue = this.m_worksheet.GetFormulaNumberValue(row, column);
      formulaValue = double.IsNaN(formulaNumberValue) ? "" : formatImpl.ApplyFormat(formulaNumberValue, false, this.m_worksheet[row, column] as RangeImpl);
    }
label_8:
    return formulaValue;
  }

  internal static HatchStyle GetHatchStyle(ExcelPattern pattern)
  {
    HatchStyle hatchStyle = ~HatchStyle.Horizontal;
    switch (pattern)
    {
      case ExcelPattern.Percent50:
        hatchStyle = HatchStyle.Percent50;
        break;
      case ExcelPattern.Percent70:
        hatchStyle = HatchStyle.Percent70;
        break;
      case ExcelPattern.Percent25:
        hatchStyle = HatchStyle.Percent25;
        break;
      case ExcelPattern.DarkHorizontal:
        hatchStyle = HatchStyle.DarkHorizontal;
        break;
      case ExcelPattern.DarkVertical:
        hatchStyle = HatchStyle.DarkVertical;
        break;
      case ExcelPattern.DarkDownwardDiagonal:
        hatchStyle = HatchStyle.DarkDownwardDiagonal;
        break;
      case ExcelPattern.DarkUpwardDiagonal:
        hatchStyle = HatchStyle.DarkUpwardDiagonal;
        break;
      case ExcelPattern.Percent75:
        hatchStyle = HatchStyle.Percent75;
        break;
      case ExcelPattern.LightDownwardDiagonal:
        hatchStyle = HatchStyle.LightDownwardDiagonal;
        break;
      case ExcelPattern.LightUpwardDiagonal:
        hatchStyle = HatchStyle.LightUpwardDiagonal;
        break;
      case ExcelPattern.Percent60:
        hatchStyle = HatchStyle.Percent60;
        break;
      case ExcelPattern.Percent10:
        hatchStyle = HatchStyle.Percent10;
        break;
      case ExcelPattern.Percent05:
        hatchStyle = HatchStyle.Percent05;
        break;
    }
    switch (pattern - 9)
    {
      case ExcelPattern.None:
        hatchStyle = HatchStyle.DiagonalCross;
        break;
      case ExcelPattern.Percent50:
        hatchStyle = HatchStyle.LightHorizontal;
        break;
      case ExcelPattern.Percent70:
        hatchStyle = HatchStyle.LightVertical;
        break;
      case ExcelPattern.DarkVertical:
        hatchStyle = HatchStyle.SmallGrid;
        break;
    }
    return hatchStyle;
  }

  private void SetAutoFormatPattern(
    ExcelKnownColors color,
    int iRow,
    int iLastRow,
    int iCol,
    int iLastCol)
  {
    this.SetAutoFormatPattern(color, iRow, iLastRow, iCol, iLastCol, ExcelKnownColors.Black, ExcelPattern.Solid);
  }

  private void SetAutoFormatPattern(
    ExcelKnownColors color,
    int iRow,
    int iLastRow,
    int iCol,
    int iLastCol,
    ExcelKnownColors patCol,
    ExcelPattern pat)
  {
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, this.Worksheet);
    for (int iRow1 = iRow; iRow1 <= iLastRow; ++iRow1)
    {
      for (int iColumn = iCol; iColumn <= iLastCol; ++iColumn)
      {
        migrantRangeImpl.ResetRowColumn(iRow1, iColumn);
        IStyle cellStyle = migrantRangeImpl.CellStyle;
        cellStyle.FillPattern = pat;
        cellStyle.ColorIndex = color;
        cellStyle.PatternColorIndex = patCol;
      }
    }
  }

  private void SetAutoFormatPatterns(ExcelAutoFormat type)
  {
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    ExcelKnownColors excelKnownColors1 = ExcelKnownColors.None;
    ExcelKnownColors excelKnownColors2 = ExcelKnownColors.BlackCustom;
    switch (type)
    {
      case ExcelAutoFormat.Classic_2:
        this.SetAutoFormatPattern(excelKnownColors1, firstRow + 1, lastRow, firstColumn + 1, lastColumn, excelKnownColors2, ExcelPattern.None);
        this.SetAutoFormatPattern(ExcelKnownColors.Grey_25_percent, firstRow + 1, lastRow, firstColumn, firstColumn);
        this.SetAutoFormatPattern(ExcelKnownColors.Violet, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Classic_3:
        this.SetAutoFormatPattern(excelKnownColors1, lastRow, lastRow, firstColumn, lastColumn, excelKnownColors2, ExcelPattern.None);
        this.SetAutoFormatPattern(ExcelKnownColors.Grey_25_percent, firstRow + 1, lastRow - 1, firstColumn, lastColumn);
        this.SetAutoFormatPattern(ExcelKnownColors.Dark_blue, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Colorful_1:
        this.SetAutoFormatPattern(ExcelKnownColors.Dark_blue, firstRow + 1, lastRow, firstColumn, lastColumn);
        this.SetAutoFormatPattern(ExcelKnownColors.Teal, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
        this.SetAutoFormatPattern(ExcelKnownColors.Black, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Colorful_2:
        int iLastCol = firstColumn == lastColumn ? lastColumn : lastColumn - 1;
        this.SetAutoFormatPattern(ExcelKnownColors.Grey_25_percent, firstRow + 1, lastRow, lastColumn, lastColumn);
        this.SetAutoFormatPattern(ExcelKnownColors.YellowCustom, firstRow + 1, lastRow, firstColumn, iLastCol, ExcelKnownColors.WhiteCustom, ExcelPattern.Percent70);
        this.SetAutoFormatPattern(ExcelKnownColors.Dark_red, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Colorful_3:
        this.SetAutoFormatPattern(ExcelKnownColors.Black, firstRow, lastRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.List_1:
        this.SetListAutoFormatPattern(true, excelKnownColors1, excelKnownColors2);
        break;
      case ExcelAutoFormat.List_2:
        this.SetListAutoFormatPattern(false, excelKnownColors1, excelKnownColors2);
        break;
      case ExcelAutoFormat.Effect3D_1:
      case ExcelAutoFormat.Effect3D_2:
        this.SetAutoFormatPattern(ExcelKnownColors.Grey_25_percent, firstRow, lastRow, firstColumn, lastColumn);
        break;
      default:
        this.SetAutoFormatPattern(excelKnownColors1, firstRow, lastRow, firstColumn, lastColumn, excelKnownColors2, ExcelPattern.None);
        break;
    }
  }

  private void SetListAutoFormatPattern(
    bool bIsList_1,
    ExcelKnownColors foreCol,
    ExcelKnownColors backColor)
  {
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    this.SetAutoFormatPattern(foreCol, lastRow, lastRow, firstColumn, lastColumn, backColor, ExcelPattern.None);
    ExcelKnownColors color = bIsList_1 ? ExcelKnownColors.Grey_25_percent : ExcelKnownColors.Light_green;
    int num1 = bIsList_1 ? 2 : 4;
    int num2 = 0;
    for (int index = lastRow - firstRow - 1; num2 < index; ++num2)
    {
      if (num2 % num1 < num1 / 2)
        this.SetAutoFormatPattern(color, num2 + firstRow + 1, num2 + firstRow + 1, firstColumn, lastColumn);
      else
        this.SetAutoFormatPattern(foreCol, num2 + firstRow + 1, num2 + firstRow + 1, firstColumn, lastColumn, backColor, ExcelPattern.None);
    }
    if (bIsList_1)
      this.SetAutoFormatPattern(color, firstRow, firstRow, firstColumn, lastColumn);
    else
      this.SetAutoFormatPattern(ExcelKnownColors.Green, firstRow, firstRow, firstColumn, lastColumn, ExcelKnownColors.Teal, ExcelPattern.Percent70);
  }

  private void SetAutoFormatAlignments(ExcelAutoFormat type)
  {
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    if (type == ExcelAutoFormat.None)
    {
      this.SetAutoFormatAlignment(ExcelHAlign.HAlignGeneral, firstRow, lastRow, firstColumn, lastColumn);
    }
    else
    {
      ExcelHAlign align1 = ExcelHAlign.HAlignLeft;
      this.SetAutoFormatAlignment(ExcelHAlign.HAlignGeneral, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
      this.SetAutoFormatAlignment(ExcelHAlign.HAlignGeneral, firstRow, firstRow, firstColumn, firstColumn);
      if (firstRow != lastRow)
        this.SetAutoFormatAlignment(ExcelHAlign.HAlignLeft, lastRow, lastRow, firstColumn, firstColumn);
      if (type == ExcelAutoFormat.List_3)
        align1 = ExcelHAlign.HAlignGeneral;
      this.SetAutoFormatAlignment(align1, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
      ExcelHAlign align2 = ExcelHAlign.HAlignCenter;
      if (Array.IndexOf<ExcelAutoFormat>(RangeImpl.DEF_AUTOFORMAT_RIGHT, type) != -1)
        align2 = ExcelHAlign.HAlignRight;
      this.SetAutoFormatAlignment(align2, firstRow, firstRow, firstColumn + 1, lastColumn);
    }
  }

  private void SetAutoFormatAlignment(
    ExcelHAlign align,
    int iRow,
    int iLastRow,
    int iCol,
    int iLastCol)
  {
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, this.Worksheet);
    for (int iRow1 = iRow; iRow1 <= iLastRow; ++iRow1)
    {
      for (int iColumn = iCol; iColumn <= iLastCol; ++iColumn)
      {
        migrantRangeImpl.ResetRowColumn(iRow1, iColumn);
        IStyle cellStyle = migrantRangeImpl.CellStyle;
        cellStyle.HorizontalAlignment = align;
        cellStyle.VerticalAlignment = ExcelVAlign.VAlignBottom;
        cellStyle.Rotation = 0;
        cellStyle.IndentLevel = 0;
      }
    }
  }

  private void SetAutoFormatWidthHeight(ExcelAutoFormat type)
  {
    if (type == ExcelAutoFormat.None)
      return;
    int firstRow = this.FirstRow;
    for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
      (this.Worksheet as WorksheetImpl).AutofitRow(firstRow);
    int firstColumn = this.FirstColumn;
    for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
      (this.Worksheet as WorksheetImpl).AutofitColumn(firstColumn);
  }

  private void SetAutoFormatNumbers(ExcelAutoFormat type)
  {
    bool flag = type == ExcelAutoFormat.None;
    if (!flag && Array.IndexOf<ExcelAutoFormat>(RangeImpl.DEF_AUTOFORMAT_NUMBER, type) == -1)
      return;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
    int num = 0;
    int iRow = this.Row + 1;
    for (int lastRow = this.LastRow; iRow <= lastRow; ++iRow)
    {
      int column = this.Column;
      for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
      {
        migrantRangeImpl.ResetRowColumn(iRow, column);
        IStyle cellStyle = migrantRangeImpl.CellStyle;
        if (!flag)
          num = iRow == this.Row + 1 ? 44 : 43;
        cellStyle.NumberFormatIndex = num;
      }
    }
  }

  private void SetAutoFormatFontBorder(ExcelAutoFormat type, bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    switch (type)
    {
      case ExcelAutoFormat.Simple:
        this.SetAutoFormatSimpleFontBorder(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Classic_1:
        this.SetAutoFormatFontBorderClassic_1(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Classic_2:
        this.SetAutoFormatFontBorderClassic_2(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Classic_3:
        this.SetAutoFormatFontBorderClassic_3(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Accounting_1:
        this.SetAutoFormatFontBorderAccounting_1(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Accounting_2:
        this.SetAutoFormatFontBorderAccounting_2(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Accounting_3:
        this.SetAutoFormatFontBorderAccounting_3(bIsFont, bIsBorder);
        break;
      case ExcelAutoFormat.Accounting_4:
        this.SetAutoFormatFontBorderAccounting_4(bIsFont, bIsBorder);
        break;
      default:
        throw new NotSupportedException("Unknown auto format type.");
    }
  }

  private void SetAutoFormatSimpleFontBorder(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    bool flag = firstRow == lastRow;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) this.m_book.InnerFonts[0]).Clone((object) this.m_book.InnerFonts);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    int iLastRow = flag ? lastRow : lastRow - 1;
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, iLastRow, firstColumn, firstColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) this.m_book.InnerFonts);
    fontImpl2.Bold = true;
    this.SetAutoFormatFont((IFont) fontImpl2, firstRow, firstRow, firstColumn + 1, lastColumn);
    if (flag)
      return;
    this.SetAutoFormatFont((IFont) fontImpl2, lastRow, lastRow, firstColumn, firstColumn);
  }

  private void SetAutoFormatFontBorderClassic_1(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    bool flag = firstRow == lastRow;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) this.m_book.InnerFonts[0]).Clone((object) this.m_book.InnerFonts);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    int iLastRow = flag ? lastRow : lastRow - 1;
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, iLastRow, firstColumn, firstColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) this.m_book.InnerFonts);
    fontImpl2.Bold = true;
    this.SetAutoFormatFont((IFont) fontImpl2, firstRow, firstRow, firstColumn + 1, lastColumn);
    if (!flag)
      this.SetAutoFormatFont((IFont) fontImpl2, lastRow, lastRow, firstColumn, firstColumn);
    if (firstColumn != lastColumn)
      this.SetAutoFormatFont((IFont) fontImpl2, firstRow, firstRow, lastColumn, lastColumn);
    FontImpl fontImpl3 = fontImpl2.Clone((object) this.m_book.InnerFonts);
    fontImpl3.Bold = false;
    fontImpl3.Italic = true;
    this.SetAutoFormatFont((IFont) fontImpl3, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
  }

  private void SetAutoFormatFontBorderClassic_2(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, firstRow, firstColumn, firstColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) innerFonts);
    fontImpl2.Bold = true;
    this.SetAutoFormatFont((IFont) fontImpl2, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
    FontImpl fontImpl3 = fontImpl2.Clone((object) innerFonts);
    fontImpl3.Color = ExcelKnownColors.Dark_blue;
    if (firstRow != lastRow)
      this.SetAutoFormatFont((IFont) fontImpl3, lastRow, lastRow, firstColumn, firstColumn);
    FontImpl fontImpl4 = fontImpl3.Clone((object) innerFonts);
    fontImpl4.Bold = false;
    fontImpl4.Size = 9.0;
    fontImpl4.Color = ExcelKnownColors.White;
    this.SetAutoFormatFont((IFont) fontImpl4, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
    FontImpl fontImpl5 = fontImpl4.Clone((object) innerFonts);
    fontImpl5.Bold = true;
    if (firstColumn == lastColumn)
      return;
    this.SetAutoFormatFont((IFont) fontImpl5, firstRow, firstRow, lastColumn, lastColumn);
  }

  private void SetAutoFormatFontBorderClassic_3(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    fontImpl1.Color = ExcelKnownColors.Dark_blue;
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, firstRow, firstColumn, firstColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) innerFonts);
    fontImpl2.Bold = true;
    fontImpl2.Color = ExcelKnownColors.Black;
    this.SetAutoFormatFont((IFont) fontImpl2, firstRow + 1, lastRow, firstColumn, firstColumn);
    FontImpl fontImpl3 = fontImpl2.Clone((object) innerFonts);
    fontImpl3.Color = ExcelKnownColors.White;
    fontImpl3.Italic = true;
    fontImpl3.Size = 9.0;
    this.SetAutoFormatFont((IFont) fontImpl3, firstRow, firstRow, firstColumn + 1, lastColumn);
  }

  private void SetAutoFormatFontBorderAccounting_1(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, firstRow, firstColumn, firstColumn);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) innerFonts);
    fontImpl2.Italic = true;
    if (firstRow != lastRow)
      this.SetAutoFormatFont((IFont) fontImpl2, lastRow, lastRow, firstColumn, firstColumn);
    FontImpl fontImpl3 = fontImpl2.Clone((object) innerFonts);
    fontImpl3.Bold = true;
    fontImpl3.Size = 9.0;
    if (firstColumn != lastColumn)
      this.SetAutoFormatFont((IFont) fontImpl3, firstRow, firstRow, lastColumn, lastColumn);
    FontImpl fontImpl4 = fontImpl3.Clone((object) innerFonts);
    fontImpl4.Color = ExcelKnownColors.Grey_50_percent;
    this.SetAutoFormatFont((IFont) fontImpl4, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
  }

  private void SetAutoFormatFontBorderAccounting_2(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    this.SetAutoFormatFont((IFont) ((FontImpl) innerFonts[0]).Clone((object) innerFonts), this.Row, this.LastRow, this.Column, this.LastColumn);
  }

  private void SetAutoFormatFontBorderAccounting_3(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, firstRow, firstColumn, firstColumn);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) innerFonts);
    fontImpl2.Italic = true;
    this.SetAutoFormatFont((IFont) fontImpl2, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
    FontImpl fontImpl3 = fontImpl2.Clone((object) innerFonts);
    fontImpl3.Size = 9.0;
    this.SetAutoFormatFont((IFont) fontImpl3, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
    FontImpl fontImpl4 = fontImpl3.Clone((object) innerFonts);
    fontImpl4.Bold = true;
    fontImpl4.Italic = true;
    if (firstColumn != lastColumn)
      this.SetAutoFormatFont((IFont) fontImpl4, firstRow, firstRow, lastColumn, lastColumn);
    FontImpl fontImpl5 = fontImpl4.Clone((object) innerFonts);
    fontImpl5.Bold = true;
    fontImpl5.Italic = false;
    fontImpl5.Size = 10.0;
    if (firstRow == lastRow)
      return;
    this.SetAutoFormatFont((IFont) fontImpl5, lastRow, lastRow, firstColumn, firstColumn);
  }

  private void SetAutoFormatFontBorderAccounting_4(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    FontImpl fontImpl1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow, lastRow, firstColumn, firstColumn);
    this.SetAutoFormatFont((IFont) fontImpl1, firstRow + 1, lastRow - 2, firstColumn + 1, lastColumn);
    FontImpl fontImpl2 = fontImpl1.Clone((object) innerFonts);
    fontImpl2.Underline = ExcelUnderline.SingleAccounting;
    this.SetAutoFormatFont((IFont) fontImpl2, firstRow, firstRow, firstColumn + 1, lastColumn);
    if (lastRow - firstRow > 1)
      this.SetAutoFormatFont((IFont) fontImpl2, lastRow - 1, lastRow - 1, firstColumn + 1, lastColumn);
    FontImpl fontImpl3 = fontImpl2.Clone((object) innerFonts);
    fontImpl3.Underline = ExcelUnderline.DoubleAccounting;
    if (firstRow == lastRow)
      return;
    this.SetAutoFormatFont((IFont) fontImpl3, lastRow, lastRow, firstColumn + 1, lastColumn);
  }

  private void SetAutoFormatFont(IFont font, int iRow, int iLastRow, int iCol, int iLastCol)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (iRow > iLastRow || iCol > iLastCol)
      return;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, this.Worksheet);
    migrantRangeImpl.ResetRowColumn(iRow, iCol);
    IFont font1 = migrantRangeImpl.CellStyle.Font;
    font1.BeginUpdate();
    font1.Bold = font.Bold;
    font1.Color = font.Color;
    font1.FontName = font.FontName;
    font1.Italic = font.Italic;
    font1.MacOSOutlineFont = font.MacOSOutlineFont;
    font1.MacOSShadow = font.MacOSShadow;
    font1.Size = font.Size;
    font1.Strikethrough = font.Strikethrough;
    font1.Subscript = font.Subscript;
    font1.Superscript = font.Superscript;
    font1.Strikethrough = font.Strikethrough;
    font1.Underline = font.Underline;
    font1.EndUpdate();
    int fontIndex = migrantRangeImpl.m_style.FontIndex;
    for (int iRow1 = iRow; iRow1 <= iLastRow; ++iRow1)
    {
      for (int iColumn = iCol; iColumn <= iLastCol; ++iColumn)
      {
        migrantRangeImpl.ResetRowColumn(iRow1, iColumn);
        ((ExtendedFormatWrapper) migrantRangeImpl.CellStyle).FontIndex = fontIndex;
      }
    }
  }

  public RangeImpl(IApplication application, object parent) => this.SetParents(parent);

  [CLSCompliant(false)]
  public RangeImpl(IApplication application, object parent, BiffReader reader)
    : this(application, parent)
  {
    this.Parse(reader);
  }

  [CLSCompliant(false)]
  public RangeImpl(IApplication application, object parent, BiffRecordRaw[] data, int position)
    : this(application, parent)
  {
    this.Parse(data, ref position);
  }

  [CLSCompliant(false)]
  public RangeImpl(
    IApplication application,
    object parent,
    BiffRecordRaw[] data,
    ref int position)
    : this(application, parent)
  {
    this.Parse(data, ref position);
  }

  [CLSCompliant(false)]
  public RangeImpl(
    IApplication application,
    object parent,
    BiffRecordRaw[] data,
    ref int position,
    bool ignoreStyles)
    : this(application, parent)
  {
    this.Parse((IList) data, ref position, ignoreStyles);
  }

  public RangeImpl(
    IApplication application,
    object parent,
    List<BiffRecordRaw> data,
    ref int position,
    bool ignoreStyles)
    : this(application, parent)
  {
    this.Parse((IList) data, ref position, ignoreStyles);
  }

  public RangeImpl(
    IApplication application,
    object parent,
    int firstCol,
    int firstRow,
    int lastCol,
    int lastRow)
    : this(application, parent)
  {
    if (firstCol > lastCol)
      throw new ArgumentOutOfRangeException("firstCol or lastCol");
    if (firstRow > lastRow)
      throw new ArgumentOutOfRangeException("firstRow or lastRow");
    this.FirstColumn = firstCol;
    this.FirstRow = firstRow;
    this.LastColumn = lastCol;
    this.LastRow = lastRow;
  }

  public RangeImpl(IApplication application, object parent, int column, int row)
    : this(application, parent)
  {
    this.FirstColumn = column;
    this.LastColumn = column;
    this.FirstRow = row;
    this.LastRow = row;
  }

  [CLSCompliant(false)]
  public RangeImpl(
    IApplication application,
    object parent,
    BiffRecordRaw record,
    bool bIgnoreStyles)
    : this(application, parent, new BiffRecordRaw[1]
    {
      record
    }, 0)
  {
  }

  protected internal void InfillCells()
  {
    if (this.m_bCells)
      return;
    this.m_cells = new List<IRange>();
    if (this.FirstRow > 0 && this.FirstColumn > 0)
    {
      int firstRow = this.FirstRow;
      for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
      {
        int firstColumn = this.FirstColumn;
        for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
          this.m_cells.Add(this.m_worksheet.InnerGetCell(firstColumn, firstRow));
      }
    }
    this.m_bCells = true;
  }

  protected internal void ResetCells()
  {
    this.m_cells = (List<IRange>) null;
    this.m_bCells = false;
  }

  public void Dispose()
  {
    if (this.m_style != null)
      this.m_style = (Syncfusion.XlsIO.Implementation.CellStyle) null;
    if (this.m_rtfString != null)
      this.m_rtfString.Dispose();
    GC.SuppressFinalize((object) this);
  }

  private void CheckDisposed()
  {
  }

  private void SetParents(object parent)
  {
    this.m_worksheet = parent as WorksheetImpl;
    this.m_book = this.m_worksheet != null ? this.m_worksheet.ParentWorkbook : throw new ApplicationException("Range object must be a child of worksheet object tree");
  }

  [CLSCompliant(false)]
  public void Parse(BiffReader reader) => throw new NotImplementedException();

  [CLSCompliant(false)]
  public void Parse(BiffRecordRaw[] data, ref int position)
  {
    this.Parse((IList) data, ref position, false);
  }

  public void Parse(IList data, ref int position, bool ignoreStyles)
  {
    BiffRecordRaw biffRecordRaw = (BiffRecordRaw) data[position];
    ICellPositionFormat cellPositionFormat = (ICellPositionFormat) biffRecordRaw;
    this.FirstColumn = this.LastColumn = cellPositionFormat.Column + 1;
    this.FirstRow = this.LastRow = cellPositionFormat.Row + 1;
    switch (biffRecordRaw.TypeCode)
    {
      case TBIFFRecord.Formula:
        this.ParseFormula((FormulaRecord) biffRecordRaw, data, ref position);
        break;
      case TBIFFRecord.RString:
        this.ParseRString((RStringRecord) biffRecordRaw);
        break;
      case TBIFFRecord.LabelSST:
        break;
      case TBIFFRecord.Blank:
        this.ParseBlank((BlankRecord) biffRecordRaw);
        break;
      case TBIFFRecord.Number:
      case TBIFFRecord.RK:
        this.ParseDouble((IDoubleValue) biffRecordRaw);
        break;
      case TBIFFRecord.Label:
        break;
      case TBIFFRecord.BoolErr:
        RangeImpl.ParseBoolError((BoolErrRecord) biffRecordRaw);
        break;
      default:
        throw new ArgumentException("Unknown to Range biff record type");
    }
  }

  protected string ParseDouble(IDoubleValue value)
  {
    double doubleValue = value.DoubleValue;
    return this.InnerNumberFormat.GetFormatType(doubleValue) == ExcelFormatType.DateTime && doubleValue < 2958466.0 ? this.DateTime.ToString() : doubleValue.ToString();
  }

  [CLSCompliant(false)]
  protected string ParseBlank(BlankRecord blank) => string.Empty;

  [CLSCompliant(false)]
  protected void ReParseFormula(FormulaRecord formula) => throw new NotImplementedException();

  [CLSCompliant(false)]
  protected void ParseFormula(FormulaRecord formula, IList data, ref int pos)
  {
  }

  [CLSCompliant(false)]
  public static string ParseBoolError(BoolErrRecord error)
  {
    if (!error.IsErrorCode)
      return (error.BoolOrError == (byte) 1).ToString().ToUpper();
    return FormulaUtil.ErrorCodeToName.ContainsKey((int) error.BoolOrError) ? FormulaUtil.ErrorCodeToName[(int) error.BoolOrError] : "#N/A";
  }

  [CLSCompliant(false)]
  protected string ParseRString(RStringRecord rstring) => string.Empty;

  private void AddRemoveEventListenersForNameX(
    Ptg[] parsedFormula,
    int iBookIndex,
    int iNameIndex,
    bool bAdd)
  {
    if (parsedFormula == null)
      throw new ArgumentNullException(nameof (parsedFormula));
    RangeImpl.AttachDetachNameIndexChangedEvent(this.m_book, new NameImpl.NameIndexChangedEventHandler(this.OnNameXIndexChanged), parsedFormula, iBookIndex, iNameIndex, bAdd);
  }

  public static void AttachDetachNameIndexChangedEvent(
    WorkbookImpl book,
    NameImpl.NameIndexChangedEventHandler handler,
    Ptg[] parsedFormula,
    int iBookIndex,
    int iNewIndex,
    bool bAdd)
  {
    try
    {
      Dictionary<long, object> indexes = new Dictionary<long, object>();
      int index = 0;
      for (int length = parsedFormula.Length; index < length; ++index)
      {
        if (FormulaUtil.IndexOf(FormulaUtil.NameXCodes, parsedFormula[index].TokenCode) != -1)
        {
          NameXPtg namex = (NameXPtg) parsedFormula[index];
          int nameIndex = (int) namex.NameIndex;
          int refIndex = (int) namex.RefIndex;
          RangeImpl.AttachDetachExternNameEvent(book, namex, iBookIndex, iNewIndex, handler, indexes, bAdd);
        }
        else if (FormulaUtil.IndexOf(FormulaUtil.NameCodes, parsedFormula[index].TokenCode) != -1)
        {
          NamePtg name = (NamePtg) parsedFormula[index];
          int externNameIndexInt = name.ExternNameIndexInt;
          RangeImpl.AttachDetachLocalNameEvent(book, name, iBookIndex, iNewIndex, handler, indexes, bAdd);
        }
      }
    }
    catch (Exception ex)
    {
      if (book.Loading)
        throw new ParseException("Parse exception", ex);
      throw;
    }
  }

  private static void AttachDetachExternNameEvent(
    WorkbookImpl book,
    NameXPtg namex,
    int iBookIndex,
    int iNewIndex,
    NameImpl.NameIndexChangedEventHandler handler,
    Dictionary<long, object> indexes,
    bool bAdd)
  {
    if (namex == null)
      throw new ArgumentNullException(nameof (namex));
    if (handler == null)
      throw new ArgumentNullException(nameof (handler));
    int bookIndex = book.GetBookIndex((int) namex.RefIndex);
    bool flag1 = namex.RefIndex < (ushort) 0 || book.IsLocalReference((int) namex.RefIndex);
    long index = RangeImpl.GetIndex(flag1 ? -1 : bookIndex, (int) namex.NameIndex);
    if (indexes.ContainsKey(index))
      return;
    bool flag2 = iBookIndex == -1 && iNewIndex == -1;
    if (flag1 && ((int) namex.NameIndex == iNewIndex || flag2))
    {
      ((NameImpl) book.InnerNamesColection[(int) namex.NameIndex - 1]).NameIndexChanged += handler;
      indexes.Add(index, (object) null);
    }
    else
    {
      if (flag1 || (iBookIndex != bookIndex || iBookIndex == -1 || (int) namex.NameIndex != iNewIndex) && !flag2)
        return;
      ExternNameImpl externName = book.ExternWorkbooks[bookIndex].ExternNames[(int) namex.NameIndex - 1];
      if (bAdd)
        externName.NameIndexChanged += handler;
      else
        externName.NameIndexChanged -= handler;
      indexes.Add(index, (object) null);
    }
  }

  private static void AttachDetachLocalNameEvent(
    WorkbookImpl book,
    NamePtg name,
    int iBookIndex,
    int iNewIndex,
    NameImpl.NameIndexChangedEventHandler handler,
    Dictionary<long, object> indexes,
    bool bAdd)
  {
    if (name == null)
      throw new ArgumentNullException("namex");
    if (handler == null)
      throw new ArgumentNullException(nameof (handler));
    long index = RangeImpl.GetIndex(-1, name.ExternNameIndexInt);
    bool flag = iBookIndex == -1 && iNewIndex == -1;
    if (indexes.ContainsKey(index) || (iBookIndex != -1 || name.ExternNameIndexInt != iNewIndex) && !flag)
      return;
    NameImpl nameImpl = (NameImpl) book.InnerNamesColection[name.ExternNameIndexInt - 1];
    if (bAdd)
      nameImpl.NameIndexChanged += handler;
    else
      nameImpl.NameIndexChanged -= handler;
    indexes.Add(index, (object) null);
  }

  private static long GetIndex(int iBookIndex, int iNameIndex)
  {
    return (long) (iBookIndex << 32 /*0x20*/ + iNameIndex);
  }

  private void OnNameXIndexChanged(object sender, NameIndexChangedEventArgs e)
  {
    ((INameIndexChangedEventProvider) sender).NameIndexChanged -= new NameImpl.NameIndexChangedEventHandler(this.OnNameXIndexChanged);
    switch (sender)
    {
      case NameImpl _:
        this.LocalIndexChanged((NameImpl) sender, e);
        break;
      case ExternNameImpl _:
        this.ExternIndexChanged((ExternNameImpl) sender, e);
        break;
    }
  }

  private void LocalIndexChanged(NameImpl sender, NameIndexChangedEventArgs e)
  {
    if (this.CellType != RangeImpl.TCellType.Formula)
      return;
    FormulaRecord record = (FormulaRecord) this.Record;
    Ptg[] parsedExpression = record.ParsedExpression;
    int index = 0;
    for (int length = parsedExpression.Length; index < length; ++index)
    {
      Ptg ptg = parsedExpression[index];
      if (ptg is NameXPtg)
      {
        NameXPtg nameXptg = ptg as NameXPtg;
        record.IsFillFromExpression = true;
        if (nameXptg.RefIndex == ushort.MaxValue || this.m_book.IsLocalReference((int) nameXptg.RefIndex) && e.OldIndex == (int) nameXptg.NameIndex - 1)
          nameXptg.NameIndex = (ushort) (e.NewIndex + 1);
      }
      if (ptg is NamePtg)
      {
        NamePtg namePtg = ptg as NamePtg;
        record.IsFillFromExpression = true;
        if (e.OldIndex == namePtg.ExternNameIndexInt - 1)
          namePtg.ExternNameIndexInt = e.NewIndex + 1;
      }
    }
  }

  private void ExternIndexChanged(ExternNameImpl sender, NameIndexChangedEventArgs e)
  {
    if (this.CellType != RangeImpl.TCellType.Formula)
      return;
    FormulaRecord record = (FormulaRecord) this.Record;
    Ptg[] parsedExpression = record.ParsedExpression;
    int index = 0;
    for (int length = parsedExpression.Length; index < length; ++index)
    {
      Ptg ptg = parsedExpression[index];
      if (ptg is NameXPtg)
      {
        NameXPtg nameXptg = ptg as NameXPtg;
        record.IsFillFromExpression = true;
        if ((int) nameXptg.RefIndex == sender.BookIndex && e.OldIndex == (int) nameXptg.NameIndex - 1)
          nameXptg.NameIndex = (ushort) (e.NewIndex + 1);
      }
    }
  }

  internal string GetFormula(int row, int column)
  {
    string input = (string) null;
    string formula;
    if ((this.Worksheet as WorksheetImpl).FormulaValues.TryGetValue(RangeImpl.GetCellIndex(column, row), out input))
    {
      foreach (Match match in Regex.Matches(input, "\\[[\\d]+\\]"))
      {
        int num = (int) double.Parse(match.ToString().Trim('[', ']'));
        if (num <= this.Workbook.ExternWorkbooks.Count)
          input = input.Replace(match.ToString(), $"[{this.Workbook.ExternWorkbooks[num - 1].ShortName}]");
      }
      formula = "=" + input;
    }
    else
      formula = this.m_worksheet.GetFormulaFromWorksheet(row, column, false);
    return formula;
  }

  public IRange Activate()
  {
    this.CheckDisposed();
    this.m_worksheet.SetActiveCell((IRange) this);
    return (IRange) this;
  }

  public virtual IRange Activate(bool scroll)
  {
    this.Activate();
    if (scroll)
      this.m_worksheet.TopLeftCell = (IRange) this;
    this.m_worksheet.Activate();
    return (IRange) this;
  }

  public IRange Group(ExcelGroupBy groupBy, bool bCollapsed)
  {
    this.CheckDisposed();
    return (IRange) this.ToggleGroup(groupBy, true, bCollapsed);
  }

  internal IRange Group(ExcelGroupBy groupBy, bool bCollapsed, bool isImport)
  {
    this.CheckDisposed();
    return (IRange) this.ToggleGroup(groupBy, true, bCollapsed, isImport);
  }

  public IRange Group(ExcelGroupBy groupBy)
  {
    this.CheckDisposed();
    return this.Group(groupBy, false);
  }

  internal List<double> GetNumberList(bool considerDateAsNumber)
  {
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
    List<double> numberList = new List<double>();
    bool flag = false;
    if (this.m_worksheet.CalcEngine == null)
    {
      this.m_worksheet.EnableSheetCalculations();
      flag = true;
    }
    for (int row = this.Row; row <= this.LastRow; ++row)
    {
      for (int column = this.Column; column <= this.LastColumn; ++column)
      {
        migrantRangeImpl.ResetRowColumn(row, column);
        WorksheetImpl.TRangeValueType cellType = this.m_worksheet.GetCellType(row, column, false);
        if (cellType == WorksheetImpl.TRangeValueType.Formula)
        {
          RangeImpl.UpdateCellValue(this.Parent, column, row, true);
          cellType = this.m_worksheet.GetCellType(row, column, true);
        }
        if ((cellType == WorksheetImpl.TRangeValueType.Number || cellType == (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula)) && (considerDateAsNumber || migrantRangeImpl.InnerNumberFormat.GetFormatType(0.0) != ExcelFormatType.DateTime))
          numberList.Add(cellType == WorksheetImpl.TRangeValueType.Number ? migrantRangeImpl.Number : migrantRangeImpl.FormulaNumberValue);
      }
    }
    if (flag)
      this.m_worksheet.DisableSheetCalculations();
    return numberList;
  }

  public double Sum() => this.Sum(false);

  public double Sum(bool considerDateAsNumber)
  {
    double num = 0.0;
    foreach (double number in this.GetNumberList(considerDateAsNumber))
      num += number;
    return num;
  }

  public double Average() => this.Average(false);

  public double Average(bool considerDateAsNumber)
  {
    double num1 = 0.0;
    List<double> numberList = this.GetNumberList(considerDateAsNumber);
    foreach (double num2 in numberList)
      num1 += num2;
    return num1 / (double) numberList.Count;
  }

  public double Min() => this.Min(false);

  public double Min(bool considerDateAsNumber)
  {
    double num1 = double.MaxValue;
    List<double> numberList = this.GetNumberList(considerDateAsNumber);
    foreach (double num2 in numberList)
    {
      if (num2 < num1)
        num1 = num2;
    }
    return numberList.Count <= 0 ? 0.0 : num1;
  }

  public double Max() => this.Max(false);

  public double Max(bool considerDateAsNumber)
  {
    double num1 = double.MinValue;
    List<double> numberList = this.GetNumberList(considerDateAsNumber);
    foreach (double num2 in numberList)
    {
      if (num2 > num1)
        num1 = num2;
    }
    return numberList.Count <= 0 ? 0.0 : num1;
  }

  public IRange Trim()
  {
    if (this.IsBlank)
      return (IRange) null;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    for (int row = this.Row; row <= this.LastRow && this.m_worksheet[row, this.Column, row, this.LastColumn].IsBlank; ++row)
      ++num3;
    for (int lastRow = this.LastRow; lastRow >= this.Row && this.m_worksheet[lastRow, this.Column, lastRow, this.LastColumn].IsBlank; --lastRow)
      ++num4;
    for (int column = this.Column; column <= this.LastColumn && this.m_worksheet[this.Row, column, this.LastRow, column].IsBlank; ++column)
      ++num1;
    for (int lastColumn = this.LastColumn; lastColumn >= this.Column && this.m_worksheet[this.Row, lastColumn, this.LastRow, lastColumn].IsBlank; --lastColumn)
      ++num2;
    return this.m_worksheet[this.Row + num3, this.Column + num1, this.LastRow - num4, this.LastColumn - num2];
  }

  public void Merge() => this.Merge(false);

  public void Merge(bool clearCells)
  {
    this.CheckDisposed();
    if (this.IsSingleCell)
      return;
    double rowHeight = this.RowHeight;
    if (!this.m_book.IsLoaded)
    {
      int column = this.Column;
      int row1 = this.Row;
      int lastRow = this.LastRow;
      int lastColumn = this.LastColumn;
      for (int index1 = row1; index1 <= lastRow; ++index1)
      {
        for (int index2 = column; index2 <= lastColumn; ++index2)
        {
          if (this.m_worksheet[index1, index2].WrapText && (this.Worksheet as WorksheetImpl).GetCellType(index1, index2, false) != WorksheetImpl.TRangeValueType.Blank)
          {
            RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) (this.Worksheet as WorksheetImpl), index1 - 1, false);
            this.m_worksheet.hasWrapMerge = true;
            if (!row2.IsBadFontHeight)
            {
              this.m_worksheet.AutofitRow(index1, index2, lastColumn, true);
              row2.IsBadFontHeight = true;
              rowHeight = this.RowHeight;
            }
            this.m_worksheet.hasWrapMerge = false;
            break;
          }
        }
      }
    }
    RangeImpl fromMergedRegion = this.m_worksheet.GetRangeFromMergedRegion((IRange) this) as RangeImpl;
    if (this.AddressLocal != fromMergedRegion.AddressLocal)
    {
      this.m_iTopRow = fromMergedRegion.Row;
      this.m_iLeftColumn = fromMergedRegion.Column;
      this.m_iBottomRow = fromMergedRegion.LastRow;
      this.m_iRightColumn = fromMergedRegion.LastColumn;
    }
    this.UpdateMergedRegion(fromMergedRegion, clearCells);
    this.m_worksheet.MergeCells.AddMerge(this, ExcelMergeOperation.Delete);
    if (rowHeight <= 0.0 || rowHeight > 409.5 || this.RowHeight == rowHeight)
      return;
    this.RowHeight = rowHeight;
  }

  internal void MergeWithoutCheck()
  {
    if (this.IsSingleCell)
      return;
    this.m_worksheet.MergeCells.AddMerge(this, ExcelMergeOperation.Leave);
  }

  internal void UpdateMergedRegion(RangeImpl cells, bool clearCells)
  {
    bool flag1 = false;
    bool flag2 = false;
    List<IRange> cellsList = cells.CellsList;
    if (cellsList[0].Value != "")
    {
      flag1 = true;
      if (clearCells)
        this.ApplyFirstCellFormats();
    }
    else
      flag2 = true;
    int index = 1;
    for (int count = cellsList.Count; index < count; ++index)
    {
      RangeImpl rangeImpl = (RangeImpl) cellsList[index];
      if (rangeImpl.Value != "" && !flag1)
      {
        flag1 = true;
        rangeImpl.CopyTo(cellsList[0], ExcelCopyRangeOptions.CopyConditionalFormats);
        this.CellStyle = rangeImpl.CellStyle;
        if (flag2)
          rangeImpl.Value = (string) null;
        if (rangeImpl.NumberFormat != "General")
          this.NumberFormat = rangeImpl.NumberFormat;
      }
      if (clearCells)
      {
        rangeImpl.Clear(ExcelClearOptions.ClearDataValidations);
        rangeImpl.Clear(ExcelClearOptions.ClearConditionalFormats);
        rangeImpl.Clear(ExcelClearOptions.ClearContent);
        rangeImpl.Clear(ExcelClearOptions.ClearComment);
      }
    }
    if (!flag1 && clearCells)
      this.ApplyFirstCellFormats();
    if (!this.m_book.m_usedCellStyleIndex.ContainsKey((int) this.ExtendedFormatIndex))
      return;
    this.m_book.UpdateUsedStyleIndex((int) this.ExtendedFormatIndex, this.Cells.Length);
  }

  private void ApplyFirstCellFormats()
  {
    Dictionary<IRange, Dictionary<ExcelBordersIndex, ExcelLineStyle>> dictionary1 = new Dictionary<IRange, Dictionary<ExcelBordersIndex, ExcelLineStyle>>();
    if (this.CellsList[0].NumberFormat != "General")
      this.NumberFormat = this.CellsList[0].NumberFormat;
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    ExtendedFormatImpl extendedFormatImpl1 = innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)];
    if (this.CellStyle.HasBorder)
    {
      ExtendedFormatImpl extendedFormatImpl2 = innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)];
      for (int column = this.Column; column <= this.LastColumn; ++column)
      {
        if (column < this.LastColumn)
        {
          ExtendedFormatImpl extendedFormatImpl3 = innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, column + 1)];
          if (extendedFormatImpl2.TopBorderLineStyle != extendedFormatImpl3.TopBorderLineStyle || extendedFormatImpl2.TopBorderColor != extendedFormatImpl3.TopBorderColor)
          {
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeTop,
                ExcelLineStyle.None
              }
            });
            break;
          }
        }
        else if (this.Row == this.LastRow)
          dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeTop,
              extendedFormatImpl1.TopBorderLineStyle
            }
          });
        else if (extendedFormatImpl1.TopBorderLineStyle != ExcelLineStyle.None)
          dictionary1.Add(this.Worksheet[this.Row + 1, this.Column, this.LastRow, this.LastColumn], new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeTop,
              ExcelLineStyle.None
            }
          });
        else
          dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeTop,
              ExcelLineStyle.None
            }
          });
      }
      ExtendedFormatImpl extendedFormatImpl4 = innerExtFormats[this.m_worksheet.GetXFIndex(this.LastRow, this.Column)];
      for (int column = this.Column; column <= this.LastColumn; ++column)
      {
        if (column < this.LastColumn)
        {
          ExtendedFormatImpl extendedFormatImpl5 = innerExtFormats[this.m_worksheet.GetXFIndex(this.LastRow, column + 1)];
          if (extendedFormatImpl4.BottomBorderLineStyle != extendedFormatImpl5.BottomBorderLineStyle || extendedFormatImpl4.BottomBorderColor != extendedFormatImpl5.BottomBorderColor)
          {
            if (dictionary1.ContainsKey((IRange) this))
            {
              Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary2 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
              dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeBottom, ExcelLineStyle.None);
              break;
            }
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeBottom,
                ExcelLineStyle.None
              }
            });
            break;
          }
        }
        else if (this.Row == this.LastRow)
        {
          if (dictionary1.ContainsKey((IRange) this))
          {
            Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary3 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
            dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeBottom, extendedFormatImpl1.BottomBorderLineStyle);
          }
          else
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeBottom,
                extendedFormatImpl1.BottomBorderLineStyle
              }
            });
        }
        else if (innerExtFormats[this.m_worksheet.GetXFIndex(this.LastRow, this.Column)].BottomBorderLineStyle != ExcelLineStyle.None && this.Row != this.LastRow)
        {
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary4 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary5 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          dictionary4.Add(ExcelBordersIndex.EdgeBottom, ExcelLineStyle.None);
          dictionary5.Add(ExcelBordersIndex.EdgeBottom, this.m_worksheet[this.LastRow, this.Column].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle);
          dictionary1.Add(this.m_worksheet[this.Row, this.Column, this.LastRow - 1, this.LastColumn], dictionary4);
          dictionary1.Add(this.m_worksheet[this.LastRow, this.Column, this.LastRow, this.LastColumn], dictionary5);
        }
        else if (extendedFormatImpl1.BottomBorderLineStyle != ExcelLineStyle.None && innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)].LeftBorderLineStyle == ExcelLineStyle.None && innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)].TopBorderLineStyle == ExcelLineStyle.None)
        {
          if (dictionary1.ContainsKey((IRange) this))
          {
            Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary6 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
            dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeBottom, this.m_worksheet[this.Row, this.Column].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle);
          }
          else
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeBottom,
                this.m_worksheet[this.Row, this.Column].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle
              }
            });
        }
        else if (dictionary1.ContainsKey((IRange) this))
        {
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary7 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeBottom, ExcelLineStyle.None);
        }
        else
          dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeBottom,
              ExcelLineStyle.None
            }
          });
      }
      ExtendedFormatImpl extendedFormatImpl6 = innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)];
      for (int row = this.Row; row <= this.LastRow; ++row)
      {
        if (row < this.LastRow)
        {
          ExtendedFormatImpl extendedFormatImpl7 = innerExtFormats[this.m_worksheet.GetXFIndex(row + 1, this.Column)];
          if (extendedFormatImpl6.LeftBorderLineStyle != extendedFormatImpl7.LeftBorderLineStyle || extendedFormatImpl6.LeftBorderColor != extendedFormatImpl7.LeftBorderColor)
          {
            if (dictionary1.ContainsKey((IRange) this))
            {
              Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary8 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
              dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeLeft, ExcelLineStyle.None);
              break;
            }
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeLeft,
                ExcelLineStyle.None
              }
            });
            break;
          }
        }
        else if (this.Column == this.LastColumn)
        {
          if (dictionary1.ContainsKey((IRange) this))
          {
            Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary9 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
            dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeLeft, extendedFormatImpl1.LeftBorderLineStyle);
          }
          else
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeLeft,
                extendedFormatImpl1.LeftBorderLineStyle
              }
            });
        }
        else if (extendedFormatImpl1.LeftBorderLineStyle != ExcelLineStyle.None)
          dictionary1.Add(this.Worksheet[this.Row, this.Column + 1, this.LastRow, this.LastColumn], new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeLeft,
              ExcelLineStyle.None
            }
          });
        else if (dictionary1.ContainsKey((IRange) this))
        {
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary10 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeLeft, ExcelLineStyle.None);
        }
        else
          dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeLeft,
              ExcelLineStyle.None
            }
          });
      }
      ExtendedFormatImpl extendedFormatImpl8 = innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.LastColumn)];
      for (int row = this.Row; row <= this.LastRow; ++row)
      {
        if (row < this.LastRow)
        {
          ExtendedFormatImpl extendedFormatImpl9 = innerExtFormats[this.m_worksheet.GetXFIndex(row + 1, this.LastColumn)];
          if (extendedFormatImpl8.RightBorderLineStyle != extendedFormatImpl9.RightBorderLineStyle || extendedFormatImpl8.RightBorderColor != extendedFormatImpl9.RightBorderColor)
          {
            if (dictionary1.ContainsKey((IRange) this))
            {
              Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary11 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
              dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeRight, ExcelLineStyle.None);
              break;
            }
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeRight,
                ExcelLineStyle.None
              }
            });
            break;
          }
        }
        else if (this.Column == this.LastColumn)
        {
          if (dictionary1.ContainsKey((IRange) this))
          {
            Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary12 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
            dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeRight, extendedFormatImpl1.RightBorderLineStyle);
          }
          else
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeRight,
                extendedFormatImpl1.RightBorderLineStyle
              }
            });
        }
        else if (innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.LastColumn)].RightBorderLineStyle != ExcelLineStyle.None && this.Column != this.LastColumn)
        {
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary13 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          dictionary13.Add(ExcelBordersIndex.EdgeRight, ExcelLineStyle.None);
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary14 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          dictionary14.Add(ExcelBordersIndex.EdgeRight, this.m_worksheet[this.Row, this.LastColumn].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle);
          dictionary1.Add(this.Worksheet[this.Row, this.Column, this.LastRow, this.LastColumn - 1], dictionary13);
          dictionary1.Add(this.Worksheet[this.Row, this.LastColumn, this.LastRow, this.LastColumn], dictionary14);
        }
        else if (extendedFormatImpl1.RightBorderLineStyle != ExcelLineStyle.None && innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)].TopBorderLineStyle == ExcelLineStyle.None && innerExtFormats[this.m_worksheet.GetXFIndex(this.Row, this.Column)].LeftBorderLineStyle == ExcelLineStyle.None)
        {
          if (dictionary1.ContainsKey((IRange) this))
          {
            Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary15 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
            dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeRight, this.m_worksheet[this.Row, this.Column].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle);
          }
          else
            dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
            {
              {
                ExcelBordersIndex.EdgeRight,
                this.m_worksheet[this.Row, this.Column].CellStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle
              }
            });
        }
        else if (dictionary1.ContainsKey((IRange) this))
        {
          Dictionary<ExcelBordersIndex, ExcelLineStyle> dictionary16 = new Dictionary<ExcelBordersIndex, ExcelLineStyle>();
          dictionary1[(IRange) this].Add(ExcelBordersIndex.EdgeRight, ExcelLineStyle.None);
        }
        else
          dictionary1.Add((IRange) this, new Dictionary<ExcelBordersIndex, ExcelLineStyle>()
          {
            {
              ExcelBordersIndex.EdgeRight,
              ExcelLineStyle.None
            }
          });
      }
    }
    this.CellStyle = this.CellsList[0].CellStyle;
    foreach (KeyValuePair<IRange, Dictionary<ExcelBordersIndex, ExcelLineStyle>> keyValuePair1 in dictionary1)
    {
      IRange key = keyValuePair1.Key;
      foreach (KeyValuePair<ExcelBordersIndex, ExcelLineStyle> keyValuePair2 in keyValuePair1.Value)
        key.CellStyle.Borders[keyValuePair2.Key].LineStyle = keyValuePair2.Value;
    }
  }

  public IRange Ungroup(ExcelGroupBy groupBy)
  {
    this.CheckDisposed();
    return (IRange) this.ToggleGroup(groupBy, false, false);
  }

  public void UnMerge()
  {
    this.CheckDisposed();
    this.m_worksheet.MergeCells.DeleteMerge(Rectangle.FromLTRB(this.FirstColumn - 1, this.FirstRow - 1, this.LastColumn - 1, this.LastRow - 1));
  }

  public void FreezePanes()
  {
    this.CheckDisposed();
    if (this.IsSingleCell)
    {
      if (this.Column == 1)
      {
        this.m_worksheet.WindowTwo.IsFreezePanes = true;
        this.m_worksheet.TopLeftCell = this.m_worksheet[this.FirstColumn, this.FirstColumn];
      }
      this.m_worksheet.SetPaneCell((IRange) this);
      this.m_worksheet.SetActiveCell(this.m_worksheet[this.FirstRow, this.FirstColumn]);
    }
    else
      this.m_worksheet.SetPaneCell((IRange) (this.Worksheet[this.FirstRow, this.FirstColumn] as RangeImpl));
  }

  public void Clear()
  {
    this.CheckDisposed();
    this.Clear(false);
  }

  public void Clear(bool isClearFormat)
  {
    this.CheckDisposed();
    if ((this.Worksheet as WorksheetImpl).IsEmpty)
      return;
    if (this.IsSingleCell)
    {
      this.BlankCell();
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      int lastRow = this.LastRow;
      int lastColumn = this.LastColumn;
      if (lastRow == this.m_book.MaxRowCount)
        lastRow = this.m_worksheet.UsedRange.LastRow;
      if (lastColumn == this.m_book.MaxColumnCount)
        lastColumn = this.m_worksheet.UsedRange.LastColumn;
      for (int firstRow = this.FirstRow; firstRow <= lastRow; ++firstRow)
      {
        for (int firstColumn = this.FirstColumn; firstColumn <= lastColumn; ++firstColumn)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
          if (!migrantRangeImpl.IsBlank)
            migrantRangeImpl.BlankCell();
        }
      }
    }
    if (!isClearFormat)
      return;
    this.CellStyleName = "Normal";
  }

  public void FullClear()
  {
    this.CheckDisposed();
    if (this.IsSingleCell)
    {
      this.Clear(true);
      CommentsCollection innerComments = this.m_worksheet.InnerComments;
      ICommentShape comment = innerComments[this.FirstRow, this.FirstColumn];
      if (comment == null)
        return;
      innerComments.Remove(comment);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
      {
        for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
          migrantRangeImpl.FullClear();
        }
      }
    }
  }

  public void Clear(ExcelMoveDirection direction)
  {
    this.CheckDisposed();
    this.Clear(direction, ExcelCopyRangeOptions.None);
  }

  public void Clear(ExcelMoveDirection direction, ExcelCopyRangeOptions options)
  {
    this.CheckDisposed();
    switch (direction)
    {
      case ExcelMoveDirection.MoveLeft:
        this.Clear(true);
        this.MoveCellsLeft(options);
        break;
      case ExcelMoveDirection.MoveUp:
        this.Clear(true);
        this.MoveCellsUp(options);
        break;
      case ExcelMoveDirection.None:
        this.Clear(true);
        break;
    }
  }

  internal void ClearOption(ExcelClearOptions option)
  {
    this.CheckDisposed();
    switch (option)
    {
      case ExcelClearOptions.ClearFormat:
        if (this.IsMerged)
          this.UnMerge();
        if (!(this.Worksheet as WorksheetImpl).IsEmpty)
          this.CellStyleName = "Normal";
        int num1 = -1;
        int num2 = -1;
        int num3 = int.MaxValue;
        int num4 = int.MaxValue;
        List<IRange> cellsList1 = this.CellsList;
        int index1 = 0;
        for (int count = cellsList1.Count; index1 < count; ++index1)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList1[index1];
          if (rangeImpl.ConditionalFormats != null)
            rangeImpl.ClearConditionalFormats();
          if (num1 == -1 && !rangeImpl.IsBlank)
            num1 = rangeImpl.Row;
          if (!rangeImpl.IsBlank)
            num2 = rangeImpl.Row;
          if (num3 == int.MaxValue && !rangeImpl.IsBlank)
            num3 = rangeImpl.Column;
          if (!rangeImpl.IsBlank)
            num4 = rangeImpl.Column;
        }
        if (this.IsEntireRow && this.IsBlankorHasStyle)
        {
          if (this.LastRow == this.m_worksheet.LastRow)
            this.m_worksheet.LastRow = this.Row - 1;
          else if (this.LastRow <= this.m_worksheet.LastRow && this.Row == this.m_worksheet.FirstRow)
            this.m_worksheet.FirstRow = this.LastRow + 1;
        }
        else
        {
          if (num1 != this.FirstRow && num2 != this.LastRow && this.m_worksheet.FirstRow >= this.FirstRow && this.m_worksheet.LastRow == this.LastRow)
          {
            this.m_worksheet.FirstRow = num1;
            this.m_worksheet.LastRow = num2;
          }
          if (num2 == this.LastRow && this.m_worksheet.FirstRow >= this.FirstRow)
            this.m_worksheet.FirstRow = num1;
          if (num1 == this.FirstRow && this.m_worksheet.LastRow == this.LastRow)
            this.m_worksheet.LastRow = num2;
        }
        if (this.IsEntireColumn && this.IsBlankorHasStyle)
        {
          if (this.LastColumn == this.m_worksheet.LastColumn)
          {
            this.m_worksheet.LastColumn = this.Column - 1;
            break;
          }
          if (this.LastColumn > this.m_worksheet.LastColumn || this.Column != this.m_worksheet.FirstColumn)
            break;
          this.m_worksheet.FirstColumn = this.LastColumn + 1;
          break;
        }
        if (num3 != this.FirstColumn && num4 != this.LastColumn && this.m_worksheet.FirstColumn >= this.FirstColumn && this.m_worksheet.LastColumn == this.LastColumn)
        {
          this.m_worksheet.FirstColumn = num3;
          this.m_worksheet.LastColumn = num4;
        }
        if (num4 == this.LastColumn && this.m_worksheet.FirstColumn >= this.FirstColumn)
          this.m_worksheet.FirstColumn = num3;
        if (num3 != this.FirstColumn || this.m_worksheet.LastColumn != this.LastColumn)
          break;
        this.m_worksheet.LastColumn = num4;
        break;
      case ExcelClearOptions.ClearContent:
        if (this.HasRichText)
          this.RichText.Text = string.Empty;
        List<IRange> cellsList2 = this.CellsList;
        int index2 = 0;
        for (int count = cellsList2.Count; index2 < count; ++index2)
          ((RangeImpl) cellsList2[index2]).Value = (string) null;
        break;
      case ExcelClearOptions.ClearComment:
        List<IRange> cellsList3 = this.CellsList;
        int index3 = 0;
        for (int count = cellsList3.Count; index3 < count; ++index3)
          ((RangeImpl) cellsList3[index3]).Comments();
        break;
      case ExcelClearOptions.ClearConditionalFormats:
        List<IRange> cellsList4 = this.CellsList;
        int index4 = 0;
        for (int count = cellsList4.Count; index4 < count; ++index4)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList4[index4];
          if (rangeImpl.ConditionalFormats != null)
            rangeImpl.ClearConditionalFormats();
        }
        break;
      case ExcelClearOptions.ClearDataValidations:
        List<IRange> cellsList5 = this.CellsList;
        int index5 = 0;
        for (int count = cellsList5.Count; index5 < count; ++index5)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList5[index5];
          if (rangeImpl.HasDataValidation)
            rangeImpl.ClearDataValidations();
        }
        break;
      default:
        if (this.IsMerged)
          this.UnMerge();
        List<IRange> cellsList6 = this.CellsList;
        int index6 = 0;
        for (int count = cellsList6.Count; index6 < count; ++index6)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList6[index6];
          rangeImpl.Value = (string) null;
          rangeImpl.Comments();
          if (rangeImpl.ConditionalFormats != null)
            rangeImpl.ClearConditionalFormats();
          if (rangeImpl.HasDataValidation)
            rangeImpl.ClearDataValidations();
        }
        if (!(this.Worksheet as WorksheetImpl).IsEmpty)
          this.CellStyleName = "Normal";
        if (this.Row == this.m_worksheet.FirstRow && this.LastRow == this.m_worksheet.LastRow && this.Column == this.m_worksheet.FirstColumn && this.LastColumn == this.m_worksheet.LastColumn)
        {
          this.m_worksheet.Clear();
          break;
        }
        int firstRow1 = this.FirstRow;
        int lastRow1 = this.LastRow;
        int firstColumn = this.FirstColumn;
        int lastColumn = this.LastColumn;
        if (this.LastRow == this.m_worksheet.LastRow)
        {
          for (int lastRow2 = this.m_worksheet.LastRow; lastRow2 >= this.m_worksheet.FirstRow; --lastRow2)
          {
            RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, lastRow2 - 1, false);
            if (row != null)
            {
              int num5 = row.FirstColumn + 1;
              int num6 = row.LastColumn + 1;
              if (lastRow2 >= firstRow1 && num5 >= firstColumn && num6 <= lastColumn)
                --this.m_worksheet.LastRow;
              else
                break;
            }
            else
              --this.m_worksheet.LastRow;
          }
        }
        if (this.FirstRow != this.m_worksheet.FirstRow)
          break;
        for (int firstRow2 = this.m_worksheet.FirstRow; firstRow2 <= this.m_worksheet.LastRow; ++firstRow2)
        {
          RowStorage row = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, firstRow2 - 1, false);
          if (row != null)
          {
            int num7 = row.FirstColumn + 1;
            int num8 = row.LastColumn + 1;
            if (firstRow2 > lastRow1 || num7 < firstColumn || num8 > lastColumn)
              break;
            ++this.m_worksheet.FirstRow;
          }
          else
            ++this.m_worksheet.FirstRow;
        }
        break;
    }
  }

  public void Clear(ExcelClearOptions option) => this.ClearOption(option);

  internal void Comments()
  {
    if (!this.IsSingleCell)
      return;
    CommentsCollection innerComments = this.m_worksheet.InnerComments;
    ICommentShape comment = innerComments[this.FirstRow, this.FirstColumn];
    if (comment == null)
      return;
    innerComments.Remove(comment);
  }

  public void MoveTo(IRange destination)
  {
    this.CheckDisposed();
    this.MoveTo(destination, ExcelCopyRangeOptions.All);
  }

  public void MoveTo(IRange destination, ExcelCopyRangeOptions options)
  {
    this.CheckDisposed();
    if (this == destination)
      return;
    this.m_worksheet.MoveRange(destination, (IRange) this, options, false);
  }

  public IRange CopyTo(IRange destination)
  {
    this.CheckDisposed();
    return this == destination ? destination : this.m_worksheet.CopyRange(destination, (IRange) this, ExcelCopyRangeOptions.All);
  }

  public IRange CopyTo(IRange destination, bool pasteLink)
  {
    this.CheckDisposed();
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    return !pasteLink ? this.CopyTo(destination) : this.m_worksheet.CopyRange(destination, (IRange) this, pasteLink);
  }

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options)
  {
    this.CheckDisposed();
    return this == destination ? destination : this.m_worksheet.CopyRange(destination, (IRange) this, options);
  }

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options, bool skipBlank)
  {
    this.CheckDisposed();
    return this == destination ? destination : this.m_worksheet.CopyRange(destination, (IRange) this, options, skipBlank);
  }

  public IRange IntersectWith(IRange range)
  {
    this.CheckDisposed();
    return this.m_worksheet.IntersectRanges((IRange) this, range);
  }

  public IRange MergeWith(IRange range)
  {
    this.CheckDisposed();
    return this.m_worksheet.MergeRanges((IRange) this, range);
  }

  public ICommentShape AddComment() => this.AddComment(true);

  public ICommentShape AddComment(bool bIsParseOptions)
  {
    this.CheckDisposed();
    CommentsCollection innerComments = this.m_worksheet.InnerComments;
    return innerComments[this.m_iTopRow, this.m_iLeftColumn] ?? innerComments.AddComment(this.m_iTopRow, this.m_iLeftColumn, bIsParseOptions);
  }

  public SizeF MeasureString(string strMeasure)
  {
    this.CheckDisposed();
    return (this.CellStyle.Font as FontWrapper).Wrapped.MeasureString(strMeasure);
  }

  public void AutofitRows()
  {
    this.CheckDisposed();
    int column = this.Column;
    int lastColumn = this.LastColumn;
    int firstRow = this.FirstRow;
    for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
      this.m_worksheet.AutofitRow(firstRow, column, lastColumn, true);
  }

  public void AutofitColumns()
  {
    this.CheckDisposed();
    int row = this.Row;
    int lastRow = this.LastRow;
    this.AutoFitToColumn(this.FirstColumn, this.LastColumn);
  }

  public void AutoFitToColumn(int firstColumn, int lastColumn)
  {
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    if (firstRow == 0 || lastRow == 0 || firstRow > lastRow)
      return;
    if (firstColumn < 1 || firstColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (firstColumn));
    if (lastColumn < 1 || lastColumn > this.m_book.MaxColumnCount)
      throw new ArgumentOutOfRangeException(nameof (lastColumn));
    using (AutoFitManager autoFitManager = new AutoFitManager(firstRow, firstColumn, lastRow, lastColumn, this))
      autoFitManager.MeasureToFitColumn();
  }

  internal static bool IsMergedCell(
    MergeCellsImpl mergedCells,
    int iRow,
    int iColumn,
    bool isRow,
    ref int num4)
  {
    if (mergedCells != null)
    {
      Rectangle rect = Rectangle.FromLTRB(iColumn - 1, iRow - 1, iColumn - 1, iRow - 1);
      MergeCellsRecord.MergedRegion mergedCell = mergedCells[rect];
      if (mergedCell != null && mergedCell.RowFrom <= iRow - 1 && mergedCell.RowTo >= iRow - 1 && mergedCell.ColumnFrom <= iColumn - 1 && mergedCell.ColumnTo >= iColumn - 1)
      {
        if (isRow)
        {
          if (mergedCell.RowFrom == mergedCell.RowTo)
            num4 = mergedCell.ColumnTo - mergedCell.ColumnFrom;
        }
        else if (mergedCell.ColumnFrom == mergedCell.ColumnTo)
          num4 = mergedCell.RowTo - mergedCell.RowFrom;
        return true;
      }
    }
    return false;
  }

  internal string GetDisplayText(int row, int column)
  {
    WorksheetImpl.TRangeValueType cellType1 = this.m_worksheet.GetCellType(row, column, false);
    string str = (string) null;
    switch (cellType1)
    {
      case WorksheetImpl.TRangeValueType.Blank:
        return string.Empty;
      case WorksheetImpl.TRangeValueType.Error:
        return this.m_worksheet.GetError(row, column);
      case WorksheetImpl.TRangeValueType.Boolean:
        return this.InnerNumberFormat.ApplyFormat(this.m_worksheet.GetBoolean(row, column).ToString().ToUpper()) ?? "";
      case WorksheetImpl.TRangeValueType.Number:
        return this.GetNumberOrDateTime(this.InnerNumberFormat, this.m_worksheet.GetNumber(row, column), row, column);
      case WorksheetImpl.TRangeValueType.Formula:
        bool flag = this.UpdateNumberFormat();
        RangeImpl.UpdateCellValue(this.Parent, column, row, true);
        int cellType2 = (int) this.m_worksheet.GetCellType(row, column, true);
        FormatImpl formatImpl = !flag ? this.InnerNumberFormat : this.m_book.InnerFormats[this.m_book.InnerExtFormats[(int) this.ExtendedFormatIndex].NumberFormatIndex];
        switch (cellType2)
        {
          case 8:
            return formatImpl.ApplyFormat(this.GetDisplayString());
          case 9:
            return this.m_worksheet.GetFormulaErrorValue(row, column);
          case 10:
            string upper = this.m_worksheet.GetFormulaBoolValue(row, column).ToString().ToUpper();
            return formatImpl.ApplyFormat(upper) ?? "";
          case 12:
            double formulaNumberValue = this.m_worksheet.GetFormulaNumberValue(row, column);
            return this.GetNumberOrDateTime(formatImpl, formulaNumberValue, row, column);
          case 24:
            string formulaStringValue = this.m_worksheet.GetFormulaStringValue(row, column);
            return formatImpl.ApplyFormat(formulaStringValue);
          default:
            return string.Empty;
        }
      case WorksheetImpl.TRangeValueType.String:
        return this.InnerNumberFormat.ApplyFormat(this.m_worksheet.GetText(row, column));
      default:
        return this.HasFormulaStringValue ? this.FormulaStringValue : str;
    }
  }

  internal static string GetFormula(string formula)
  {
    if (formula != null)
    {
      formula = formula.Replace("=", " ");
      formula = formula.Trim();
      int length = formula.IndexOf("(");
      if (length > 0)
        formula = formula.Substring(0, length);
    }
    return formula;
  }

  private bool UpdateNumberFormat()
  {
    CalcEngine calcEngine = this.m_worksheet.CalcEngine;
    bool flag = false;
    if (this.NumberFormat == "General" && this.Formula != null)
    {
      DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
      switch (RangeImpl.GetFormula(this.Formula))
      {
        case "TIME":
          this.NumberFormat = dateTimeFormat.ShortTimePattern;
          flag = true;
          break;
        case "DATE":
        case "TODAY":
          this.NumberFormat = dateTimeFormat.ShortDatePattern;
          flag = true;
          break;
        case "NOW":
          this.NumberFormat = $"{dateTimeFormat.ShortDatePattern} {dateTimeFormat.ShortTimePattern}";
          flag = true;
          break;
      }
      if (calcEngine != null && !calcEngine.ExcelLikeComputations)
        calcEngine.ExcelLikeComputations = true;
    }
    else if (calcEngine != null && this.NumberFormat != "General")
      calcEngine.ExcelLikeComputations = false;
    return flag;
  }

  private string GetNumberOrDateTime(FormatImpl formatImpl, double dValue, int row, int column)
  {
    string result1 = string.Empty;
    Match match1 = RangeImpl.DayRegex.Match(formatImpl.FormatString);
    Match match2 = RangeImpl.MonthRegex.Match(formatImpl.FormatString);
    Match match3 = RangeImpl.YearRegex.Match(formatImpl.FormatString);
    ExcelFormatType formatType = formatImpl.GetFormatType(0.0);
    if (dValue == 0.0 && !this.m_worksheet.WindowTwo.IsDisplayZeros)
    {
      string str;
      if (formatType == ExcelFormatType.Number || formatType == ExcelFormatType.General)
        return str = this.GetDisplayString();
      if (formatImpl.ApplyFormat(this.GetDisplayString()).Length == 0)
        return str = string.Empty;
    }
    switch (formatType)
    {
      case ExcelFormatType.General:
      case ExcelFormatType.Text:
      case ExcelFormatType.Number:
        if (double.IsNaN(dValue) || !(result1 == string.Empty))
          return result1;
        ExtendedFormatImpl result2 = (ExtendedFormatImpl) null;
        string displayText;
        if (double.IsNaN(dValue))
        {
          displayText = dValue.ToString();
        }
        else
        {
          if (double.IsInfinity(dValue))
            return "#DIV/0!";
          displayText = this.m_book.Loading || this.m_worksheet.ConditionalFormats.Count <= 0 ? formatImpl.ApplyFormat(dValue, false, this.Worksheet[row, column] as RangeImpl) : formatImpl.ApplyCFFormat(dValue, this, formatImpl, out result2);
        }
        return this.UpdateText(displayText, dValue, formatImpl, result2);
      case ExcelFormatType.DateTime:
        bool flag1 = false;
        bool flag2 = false;
        if (result1 == string.Empty)
        {
          dValue = this.GetCalculateOnOpen(dValue);
          if (this.m_book.Date1904 && (match2.Success || match1.Success || match3.Success))
            dValue += 1462.0;
          else if (dValue < 60.0)
          {
            if (dValue < 0.0 && this.m_book.Date1904)
              flag1 = true;
            if (dValue < 0.0)
            {
              flag2 = true;
              dValue *= -1.0;
            }
            ++dValue;
          }
          if (dValue > CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime.ToOADate() || (flag2 || dValue < 0.0) && !this.m_book.Date1904)
          {
            result1 = "######";
          }
          else
          {
            result1 = formatImpl.ApplyFormat(dValue, false, this.Worksheet[row, column] as RangeImpl);
            if (flag1)
              result1 = "-" + result1;
          }
        }
        return this.m_hasDefaultFormat ? this.GetCultureFormat(result1, dValue, formatImpl) : result1;
      default:
        return formatImpl.ApplyFormat(dValue, false, this.Worksheet[row, column] as RangeImpl);
    }
  }

  private double GetCalculateOnOpen(double dValue)
  {
    if (this.Record != null && this.Record.TypeCode == TBIFFRecord.Formula && ((FormulaRecord) this.Record).CalculateOnOpen)
    {
      bool enabledCalcEngine = this.Workbook.EnabledCalcEngine;
      if (Array.IndexOf<string>(this.DEF_DATETIME_FORMULA, RangeImpl.GetFormula(this.Formula)) > 1)
      {
        if (!enabledCalcEngine)
          this.Worksheet.EnableSheetCalculations();
        dValue = Convert.ToDouble(this.CalculatedValue);
        if (!enabledCalcEngine)
          this.Worksheet.DisableSheetCalculations();
      }
    }
    return dValue;
  }

  public IRange Offset(int row, int column)
  {
    int row1 = row + this.Row;
    int column1 = column + this.Column;
    this.CheckRange(row1, column1);
    return this.Worksheet.Range[row1, column1];
  }

  public IRange Resize(int row, int column)
  {
    int num1 = row - 1 + this.Row;
    int num2 = column - 1 + this.Column;
    this.CheckRange(row, column);
    this.CheckRange(num1, num2);
    return this.Worksheet.Range[this.Row, this.Column, num1, num2];
  }

  private string UpdateText(
    string displayText,
    double dValue,
    FormatImpl formatImpl,
    ExtendedFormatImpl result)
  {
    string sectionFormat = this.GetSectionFormat(dValue, formatImpl.FormatString);
    if (sectionFormat.Contains("*"))
    {
      Font nativeFont = this.CellStyle.Font.GenerateNativeFont();
      int startIndex = displayText.IndexOf("*");
      int num1 = displayText.Trim().IndexOf("*");
      if (startIndex != -1)
      {
        string newValue = sectionFormat[sectionFormat.IndexOf("*") + 1].ToString();
        if (this.m_bAutofitText)
          return displayText.Replace("*", string.Empty);
        displayText = displayText.Replace("*", newValue);
        double textWidth = this.GetTextWidth(displayText, nativeFont);
        double num2;
        if (this.IsMerged)
        {
          double Width = 0.0;
          foreach (IRange column in this.MergeArea.Rows[0].Columns)
            Width += column.ColumnWidth;
          num2 = !this.m_book.IsConverting ? (double) this.Worksheet.ColumnWidthToPixels(Width) : (double) this.Worksheet.ColumnWidthToPixels(Width * this.m_book.GetCellScaledWidthHeight(this.Worksheet)[0]);
        }
        else
          num2 = !this.m_book.IsConverting ? (double) this.Worksheet.ColumnWidthToPixels(this.ColumnWidth) : (double) this.Worksheet.ColumnWidthToPixels(this.ColumnWidth * this.m_book.GetCellScaledWidthHeight(this.Worksheet)[0]);
        if (this.m_book.IsConverting && result != null && result is ExtendedFormatStandAlone && (result as ExtendedFormatStandAlone).AdvancedCFIcon != null)
          num2 -= (double) (result as ExtendedFormatStandAlone).AdvancedCFIcon.Width * this.m_book.GetCellScaledWidthHeight(this.Worksheet)[0];
        if (this.ExtendedFormat.IndentLevel > 0 && this.m_book.IsConverting && num1 == 0)
          num2 -= this.Application.ConvertUnits((double) this.ExtendedFormat.IndentLevel * 7.1999998092651367, MeasureUnits.Point, MeasureUnits.Pixel);
        for (; num2 > textWidth; textWidth = this.GetTextWidth(displayText, nativeFont))
          displayText = displayText.Insert(startIndex, newValue);
        if (textWidth >= num2)
          displayText = displayText.Remove(startIndex, 1);
      }
    }
    return displayText;
  }

  internal double GetTextWidth(string displayText, Font font)
  {
    this.Workbook.InnerGraphics.PageUnit = GraphicsUnit.Pixel;
    if (this.CellStyle.HorizontalAlignment == ExcelHAlign.HAlignCenter)
      this.StringFormt.Alignment = StringAlignment.Center;
    if (this.CellStyle.HorizontalAlignment == ExcelHAlign.HAlignRight)
      this.StringFormt.Alignment = StringAlignment.Far;
    return Math.Round((double) this.Workbook.InnerGraphics.MeasureString(displayText, font, new PointF(0.0f, 0.0f), this.StringFormt).Width);
  }

  private string GetSectionFormat(double dValue, string formatString)
  {
    string[] strArray = formatString.Split(new char[1]
    {
      ';'
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length > 0)
    {
      if (dValue > 0.0 && strArray.Length >= 1)
        return strArray[0];
      if (dValue < 0.0 && strArray.Length >= 2)
        return strArray[1];
      if (dValue == 0.0 && strArray.Length >= 3)
        return strArray[2];
    }
    return formatString;
  }

  public void Replace(string oldValue, string newValue)
  {
    this.CheckDisposed();
    IRange[] rangesWithValues = this.m_worksheet.FindRangesWithValues(oldValue, ExcelFindOptions.None, (IRange) this);
    this.m_worksheet.ReplaceWithValues(oldValue, newValue, rangesWithValues, ExcelFindOptions.None);
  }

  public void Replace(string oldValue, string newValue, ExcelFindOptions findOptions)
  {
    IRange[] rangesWithValues = this.m_worksheet.FindRangesWithValues(oldValue, findOptions, (IRange) this);
    this.m_worksheet.ReplaceWithValues(oldValue, newValue, rangesWithValues, findOptions);
  }

  public void Replace(string oldValue, double newValue)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangesWithValues = this.m_worksheet.FindRangesWithValues(oldValue, ExcelFindOptions.None, (IRange) this);
    if (rangesWithValues == null)
      return;
    foreach (IRange range in rangesWithValues)
      range.Number = newValue;
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    IRange[] rangesWithValues = this.m_worksheet.FindRangesWithValues(oldValue, ExcelFindOptions.None, (IRange) this);
    if (rangesWithValues == null)
      return;
    foreach (IRange range in rangesWithValues)
      range.DateTime = newValue;
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    if (this.IsSingleCell && this.Text == oldValue)
    {
      this.m_worksheet.ImportArray(newValues, this.Row, this.Column, isVertical);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.Text == oldValue)
            this.m_worksheet.ImportArray(newValues, row, column, isVertical);
        }
      }
    }
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    if (this.IsSingleCell && this.Text == oldValue)
    {
      this.m_worksheet.ImportArray(newValues, this.Row, this.Column, isVertical);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.Text == oldValue)
            this.m_worksheet.ImportArray(newValues, row, column, isVertical);
        }
      }
    }
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    if (this.IsSingleCell && this.Text == oldValue)
    {
      this.m_worksheet.ImportArray(newValues, this.Row, this.Column, isVertical);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.Text == oldValue)
            this.m_worksheet.ImportArray(newValues, row, column, isVertical);
        }
      }
    }
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    if (this.IsSingleCell && this.Text == oldValue)
    {
      this.m_worksheet.ImportDataTable(newValues, isFieldNamesShown, this.Row, this.Column);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.Text == oldValue)
            this.m_worksheet.ImportDataTable(newValues, isFieldNamesShown, row, column);
        }
      }
    }
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    this.CheckDisposed();
    if (this.Worksheet.IsPasswordProtected)
      throw new ApplicationException("You cannot use this command on a protected sheet");
    if (this.IsSingleCell && this.Text == oldValue)
    {
      this.m_worksheet.ImportDataColumn(newValues, isFieldNamesShown, this.Row, this.Column);
    }
    else
    {
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          migrantRangeImpl.ResetRowColumn(row, column);
          if (migrantRangeImpl.Text == oldValue)
            this.m_worksheet.ImportDataColumn(newValues, isFieldNamesShown, row, column);
        }
      }
    }
  }

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    this.CheckDisposed();
    if (findValue == null)
      return (IRange) null;
    bool flag1 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag4 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag5 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    bool flag6 = (flags & ExcelFindType.Values) == ExcelFindType.Values;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !flag5 && !flag6)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (this.IsSingleCell && !flag5)
    {
      if (flag4 && this.IsError && this.Error == findValue)
        return (IRange) this;
      if (flag1 && this.HasFormula && this.Formula == findValue)
        return (IRange) this;
      if (flag3 && this.FormulaStringValue != null && this.FormulaStringValue == findValue)
        return (IRange) this;
      return flag2 && this.HasString && this.Text == findValue ? (IRange) this : (IRange) null;
    }
    return (!findValue.Equals(string.Empty) ? this.m_worksheet.Find((IRange) this, findValue, flags, true) : this.m_worksheet.FindEmpty((IRange) this, findValue, true, flags))?[0];
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    this.CheckDisposed();
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag3 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (this.IsSingleCell && !flag3)
    {
      if (flag2 && this.HasNumber && this.Number == findValue)
        return (IRange) this;
      return flag1 && this.HasFormula && this.FormulaNumberValue == findValue ? (IRange) this : (IRange) null;
    }
    return this.m_worksheet.Find((IRange) this, findValue, flags, true)?[0];
  }

  public IRange FindFirst(bool findValue)
  {
    this.CheckDisposed();
    return this.IsSingleCell ? (!this.IsBoolean || this.Boolean != findValue ? (IRange) null : (IRange) this) : this.m_worksheet.Find((IRange) this, findValue ? (byte) 1 : (byte) 0, false, true)?[0];
  }

  public IRange FindFirst(DateTime findValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell)
      return this.FindFirst(findValue.ToOADate(), ExcelFindType.Number | ExcelFindType.FormulaValue);
    return this.HasDateTime && this.DateTime == findValue ? (IRange) this : (IRange) null;
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell)
      return this.FindFirst(findValue.TotalDays, ExcelFindType.Number | ExcelFindType.FormulaValue);
    return this.HasDateTime && this.TimeSpan == findValue ? (IRange) this : (IRange) null;
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    this.CheckDisposed();
    if (findValue == null)
      return (IRange[]) null;
    bool flag1 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag4 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag5 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    bool flag6 = (flags & ExcelFindType.Values) == ExcelFindType.Values;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !flag5 && !flag6)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (this.IsSingleCell && !flag5)
    {
      if ((!flag4 || !this.IsError || !(this.Error == findValue)) && (!flag1 || !this.HasFormula || !(this.Formula == findValue)) && (!flag3 || this.FormulaStringValue == null || !(this.FormulaStringValue == findValue)) && (!flag2 || !this.HasString || !(this.Text == findValue)))
        return (IRange[]) null;
      return new IRange[1]{ (IRange) this };
    }
    IRange[] rangeArray = new IRange[1]
    {
      this.m_worksheet["A1"]
    };
    if (!findValue.Equals(string.Empty))
      return this.m_worksheet.Find((IRange) this, findValue, flags, false);
    return this.FirstRow == 1 ? this.m_worksheet.FindEmpty((IRange) this, findValue, false, flags) : rangeArray;
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    this.CheckDisposed();
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    bool flag3 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (!this.IsSingleCell || flag3)
      return this.m_worksheet.Find((IRange) this, findValue, flags, false);
    if ((!flag2 || !this.HasNumber || this.Number != findValue) && (!flag1 || !this.HasFormula || this.FormulaNumberValue != findValue))
      return (IRange[]) null;
    return new IRange[1]{ (IRange) this };
  }

  public IRange[] FindAll(bool findValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell)
      return this.m_worksheet.Find((IRange) this, findValue ? (byte) 1 : (byte) 0, false, false);
    if (!this.IsBoolean || this.Boolean != findValue)
      return (IRange[]) null;
    return new IRange[1]{ (IRange) this };
  }

  public IRange[] FindAll(DateTime findValue)
  {
    this.CheckDisposed();
    List<IRange> rangeList = new List<IRange>();
    if (!this.IsSingleCell)
      return this.FindAll(findValue.ToOADate(), ExcelFindType.Number | ExcelFindType.FormulaValue);
    if (this.HasDateTime && this.DateTime == findValue)
      rangeList.Add((IRange) this);
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    this.CheckDisposed();
    List<IRange> rangeList = new List<IRange>();
    if (!this.IsSingleCell)
      return this.FindAll(findValue.TotalDays, ExcelFindType.Number | ExcelFindType.FormulaValue);
    if (this.HasDateTime && this.TimeSpan == findValue)
      rangeList.Add((IRange) this);
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public void CopyToClipboard()
  {
    this.AppImplementation.CreateClipboardProvider((IWorksheet) this.m_worksheet).SetClipboard((IRange) this);
  }

  public void BorderAround() => this.BorderAround(ExcelLineStyle.Thin);

  public void BorderAround(ExcelLineStyle borderLine)
  {
    this.BorderAround(borderLine, ExcelKnownColors.Black);
  }

  public void BorderAround(ExcelLineStyle borderLine, Color borderColor)
  {
    ExcelKnownColors nearestColor = this.m_book.GetNearestColor(borderColor);
    this.BorderAround(borderLine, nearestColor);
  }

  public void BorderAround(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
  {
    int column = this.Column;
    int lastColumn = this.LastColumn;
    int row = this.Row;
    int lastRow = this.LastRow;
    RangeImpl migrantRange = (RangeImpl) new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
    IStyle cellStyle = this.CellStyle;
    for (int index = column; index <= lastColumn; ++index)
    {
      RangeImpl range = this.GetRange(row, index, migrantRange, cellStyle);
      range.SetBorderToSingleCell(ExcelBordersIndex.EdgeTop, borderLine, borderColor);
      if (row > 1 && this.m_worksheet.CellRecords[row - 1, index] != null)
      {
        range = this.GetRange(row - 1, index, range, cellStyle);
        range.SetBorderToSingleCell(ExcelBordersIndex.EdgeBottom, ExcelLineStyle.None, borderColor);
      }
      migrantRange = this.GetRange(lastRow, index, range, cellStyle);
      migrantRange.SetBorderToSingleCell(ExcelBordersIndex.EdgeBottom, borderLine, borderColor);
      if (lastRow < this.Workbook.MaxRowCount && this.m_worksheet.CellRecords[lastRow + 1, index] != null)
      {
        migrantRange = this.GetRange(lastRow + 1, index, migrantRange, cellStyle);
        migrantRange.SetBorderToSingleCell(ExcelBordersIndex.EdgeTop, ExcelLineStyle.None, borderColor);
      }
    }
    for (int index = row; index <= lastRow; ++index)
    {
      RangeImpl range = this.GetRange(index, column, migrantRange, cellStyle);
      range.SetBorderToSingleCell(ExcelBordersIndex.EdgeLeft, borderLine, borderColor);
      if (column > 1 && this.m_worksheet.CellRecords[index, column - 1] != null)
      {
        range = this.GetRange(index, column - 1, range, cellStyle);
        range.SetBorderToSingleCell(ExcelBordersIndex.EdgeRight, ExcelLineStyle.None, borderColor);
      }
      migrantRange = this.GetRange(index, lastColumn, range, cellStyle);
      migrantRange.SetBorderToSingleCell(ExcelBordersIndex.EdgeRight, borderLine, borderColor);
      if (lastColumn < this.Workbook.MaxColumnCount && this.m_worksheet.CellRecords[index, lastColumn + 1] != null)
      {
        migrantRange = this.GetRange(index, lastColumn + 1, migrantRange, cellStyle);
        migrantRange.SetBorderToSingleCell(ExcelBordersIndex.EdgeLeft, ExcelLineStyle.None, borderColor);
      }
    }
  }

  internal RangeImpl GetRange(int row, int column, RangeImpl migrantRange, IStyle cellStyle)
  {
    if (cellStyle is Syncfusion.XlsIO.Implementation.CellStyle && (cellStyle as Syncfusion.XlsIO.Implementation.CellStyle).BeginCallsCount > 0)
      return this;
    if (cellStyle is StyleArrayWrapper && (cellStyle as StyleArrayWrapper).BeginCallsCount > 0)
    {
      foreach (IRange range in (cellStyle as StyleArrayWrapper).Ranges)
      {
        if (range.Row == row && range.Column == column)
          return range as RangeImpl;
      }
    }
    if (migrantRange is IMigrantRange)
      (migrantRange as IMigrantRange).ResetRowColumn(row, column);
    return migrantRange;
  }

  public void BorderInside() => this.BorderInside(ExcelLineStyle.Thin);

  public void BorderInside(ExcelLineStyle borderLine)
  {
    this.BorderInside(borderLine, ExcelKnownColors.Black);
  }

  public void BorderInside(ExcelLineStyle borderLine, Color borderColor)
  {
    ExcelKnownColors nearestColor = this.m_book.GetNearestColor(borderColor);
    this.BorderInside(borderLine, nearestColor);
  }

  public void BorderInside(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
  {
    if (this.IsSingleCell)
      throw new NotSupportedException("This method doesn't support for single cell.");
    int column = this.Column;
    int lastColumn = this.LastColumn;
    int row = this.Row;
    int lastRow = this.LastRow;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
    for (int iColumn = column; iColumn <= lastColumn; ++iColumn)
    {
      for (int iRow = row; iRow <= lastRow; ++iRow)
      {
        migrantRangeImpl.ResetRowColumn(iRow, iColumn);
        migrantRangeImpl.CellStyle.BeginUpdate();
        if (iColumn != this.LastColumn)
          migrantRangeImpl.SetBorderToSingleCell(ExcelBordersIndex.EdgeRight, borderLine, borderColor);
        if (iColumn != this.Column)
          migrantRangeImpl.SetBorderToSingleCell(ExcelBordersIndex.EdgeLeft, borderLine, borderColor);
        if (iRow != this.LastRow)
          migrantRangeImpl.SetBorderToSingleCell(ExcelBordersIndex.EdgeBottom, borderLine, borderColor);
        if (iRow != this.Row)
          migrantRangeImpl.SetBorderToSingleCell(ExcelBordersIndex.EdgeTop, borderLine, borderColor);
        migrantRangeImpl.CellStyle.EndUpdate();
      }
    }
  }

  public void BorderNone()
  {
    int firstColumn = this.FirstColumn;
    for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
    {
      int firstRow = this.FirstRow;
      for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        if (firstColumn == this.FirstColumn && firstColumn > 1 && this.m_worksheet.CellRecords[firstRow, firstColumn - 1] != null)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn - 1);
          migrantRangeImpl.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
        }
        if (firstColumn == this.LastColumn && firstColumn < this.Workbook.MaxColumnCount && this.m_worksheet.CellRecords[firstRow, firstColumn + 1] != null)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn + 1);
          migrantRangeImpl.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
        }
        if (firstRow == this.FirstRow && firstRow > 1 && this.m_worksheet.CellRecords[firstRow - 1, firstColumn] != null)
        {
          migrantRangeImpl.ResetRowColumn(firstRow - 1, firstColumn);
          migrantRangeImpl.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
        }
        if (firstRow == this.LastRow && firstRow < this.Workbook.MaxRowCount && this.m_worksheet.CellRecords[firstRow + 1, firstColumn] != null)
        {
          migrantRangeImpl.ResetRowColumn(firstRow + 1, firstColumn);
          migrantRangeImpl.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.None;
        }
        migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
        migrantRangeImpl.CellStyle.BeginUpdate();
        migrantRangeImpl.Borders.LineStyle = ExcelLineStyle.None;
        migrantRangeImpl.CellStyle.EndUpdate();
      }
    }
  }

  public void SetAutoFormat(ExcelAutoFormat format)
  {
    this.SetAutoFormat(format, ExcelAutoFormatOptions.All);
  }

  public void SetAutoFormat(ExcelAutoFormat format, ExcelAutoFormatOptions options)
  {
    if (this.IsSingleCell)
      throw new NotSupportedException("Auto format doesn't suport in single cell.");
    if (options == ExcelAutoFormatOptions.None)
      return;
    bool flag1 = (options & ExcelAutoFormatOptions.Patterns) == ExcelAutoFormatOptions.Patterns;
    bool flag2 = (options & ExcelAutoFormatOptions.Alignment) == ExcelAutoFormatOptions.Alignment;
    bool flag3 = (options & ExcelAutoFormatOptions.Width_Height) == ExcelAutoFormatOptions.Width_Height;
    bool flag4 = (options & ExcelAutoFormatOptions.Number) == ExcelAutoFormatOptions.Number;
    bool bIsFont = (options & ExcelAutoFormatOptions.Font) == ExcelAutoFormatOptions.Font;
    bool bIsBorder = (options & ExcelAutoFormatOptions.Border) == ExcelAutoFormatOptions.Border;
    if (flag1)
      this.SetAutoFormatPatterns(format);
    if (flag2)
      this.SetAutoFormatAlignments(format);
    if (flag3)
      this.SetAutoFormatWidthHeight(format);
    if (flag4)
      this.SetAutoFormatNumbers(format);
    this.SetAutoFormatFontBorder(format, bIsFont, bIsBorder);
  }

  private void SetSingleCellValue2(object value)
  {
    bool isPreserveFormat = true;
    if (value != null)
    {
      bool? nullable1 = this.IsStringsPreserved;
      if (!nullable1.HasValue)
        nullable1 = new bool?(this.m_worksheet.IsStringsPreserved);
      bool? nullable2 = nullable1;
      if ((nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
      {
        switch (value)
        {
          case DateTime dateTime when dateTime >= RangeImpl.DEF_MIN_DATETIME:
            this.DateTime = (DateTime) value;
            break;
          case TimeSpan timeSpan:
            this.TimeSpan = timeSpan;
            break;
          case double _ when this.Format.FormatString == "General":
            this.Number = (double) value;
            break;
          case int _ when this.Format.FormatString == "General":
            this.SetNumberAndFormat((double) (int) value, isPreserveFormat);
            break;
          case Decimal _ when this.Format.FormatString == "General":
            this.Number = double.Parse(value.ToString());
            break;
          default:
            this.Value = value.ToString();
            break;
        }
      }
      else
        this.Value = value.ToString();
    }
    else
      this.Text = "";
  }

  public void CollapseGroup(ExcelGroupBy groupBy)
  {
    this.CollapseExpand(groupBy, true, ExpandCollapseFlags.Default);
  }

  public void ExpandGroup(ExcelGroupBy groupBy)
  {
    this.ExpandGroup(groupBy, ExpandCollapseFlags.Default);
  }

  public void ExpandGroup(ExcelGroupBy groupBy, ExpandCollapseFlags flags)
  {
    this.CollapseExpand(groupBy, false, flags);
  }

  private string CheckAndGetDateUncustomizedString(string inputFormat)
  {
    bool flag = false;
    if ((inputFormat.Contains(",") || inputFormat.Contains(".") || inputFormat.Contains(" ") || inputFormat.Contains("-")) && (inputFormat.Contains("d") || inputFormat.Contains("m") || inputFormat.Contains("y") || inputFormat.Contains("h") || inputFormat.Contains("s")))
    {
      int num1 = inputFormat.IndexOf('\\');
      int num2 = inputFormat.LastIndexOf('\\');
      int length = inputFormat.Length;
      List<char> charList = new List<char>((IEnumerable<char>) inputFormat.ToCharArray());
      for (int index = num1; index <= num2; ++index)
      {
        if (index != 0 && index != length - 1 && charList[index] == '\\' && (charList[index + 1] == ',' || charList[index + 1] == '.' || charList[index + 1] == ' ' || charList[index + 1] == '-'))
        {
          charList.RemoveAt(index);
          --length;
          --num2;
          flag = true;
        }
      }
      inputFormat = new string(charList.ToArray());
    }
    if (!flag)
      inputFormat = inputFormat.Replace("\\", "");
    return inputFormat;
  }

  private string CheckForAccountingString(string inputFormat)
  {
    if (inputFormat.Contains("\""))
    {
      string currencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
      int num = inputFormat.IndexOf(currencySymbol);
      inputFormat.IndexOf('"');
      inputFormat.LastIndexOf('"');
      if (num != -1)
        inputFormat = inputFormat.Replace($"\"{currencySymbol}\"", currencySymbol);
    }
    return inputFormat;
  }

  public IRange[] GetDependents() => this.GetDependents(false);

  public IRange[] GetDependents(bool isEntireWorkbook)
  {
    return this.GetDependentRanges(isEntireWorkbook, false);
  }

  public IRange[] GetDirectDependents() => this.GetDirectDependents(false);

  public IRange[] GetDirectDependents(bool isEntireWorkbook)
  {
    return this.GetDependentRanges(isEntireWorkbook, true);
  }

  public IRange[] GetPrecedents() => this.GetPrecedents(false);

  public IRange[] GetPrecedents(bool isEntireWorkbook)
  {
    return this.GetPrecedentsRange(isEntireWorkbook, false);
  }

  public IRange[] GetDirectPrecedents() => this.GetDirectPrecedents(false);

  public IRange[] GetDirectPrecedents(bool isEntireWorkbook)
  {
    return this.GetPrecedentsRange(isEntireWorkbook, true);
  }

  private void GetDependentsBySheet(IWorksheet worksheet, bool isEntireWorkbook)
  {
    int firstRow = (worksheet as WorksheetImpl).FirstRow;
    int firstColumn = (worksheet as WorksheetImpl).FirstColumn;
    int lastRow = (worksheet as WorksheetImpl).LastRow;
    int lastColumn = (worksheet as WorksheetImpl).LastColumn;
    if (firstColumn == lastColumn && firstColumn == int.MaxValue || firstRow == lastRow && firstRow < 0)
      return;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(worksheet.Application, worksheet);
    for (int iRow = firstRow; iRow <= lastRow; ++iRow)
    {
      for (int iColumn = firstColumn; iColumn <= lastColumn; ++iColumn)
      {
        migrantRangeImpl.ResetRowColumn(iRow, iColumn);
        migrantRangeImpl.GetPrecedents(isEntireWorkbook);
      }
    }
  }

  private Dictionary<string, IRange> GetReferenceCells(
    IRange range,
    Dictionary<string, List<string>> precedentsCache,
    bool isEntireWorkbook,
    bool isDirect)
  {
    Dictionary<string, IRange> resultCells = new Dictionary<string, IRange>();
    List<string> sourceList = new List<string>();
    sourceList.Add(range.Address);
    while (sourceList.Count > 0)
    {
      IRange rangeByString = (range.Worksheet as WorksheetImpl).GetRangeByString(sourceList[0], false);
      int row1 = rangeByString.Row;
      int column1 = rangeByString.Column;
      int lastRow1 = rangeByString.LastRow;
      int lastColumn1 = rangeByString.LastColumn;
      MigrantRangeImpl sourceRange = new MigrantRangeImpl(rangeByString.Worksheet.Application, rangeByString.Worksheet);
      sourceList.RemoveAt(0);
      for (int iRow1 = row1; iRow1 <= lastRow1; ++iRow1)
      {
        for (int iColumn1 = column1; iColumn1 <= lastColumn1; ++iColumn1)
        {
          sourceRange.ResetRowColumn(iRow1, iColumn1);
          if (sourceRange.HasFormula)
          {
            string formula = sourceRange.Formula;
            if (formula != null && formula.Length > 0 && formula[0] == '=')
            {
              foreach (Ptg ptg in this.m_book.FormulaUtil.ParseString(formula.Substring(1), rangeByString.Worksheet, (Dictionary<string, string>) null))
              {
                if (ptg is IRangeGetter rangeGetter)
                {
                  IRange range1 = rangeGetter.GetRange(rangeByString.Worksheet.Workbook, rangeByString.Worksheet);
                  if (range1 != null && range1 is NameImpl)
                  {
                    if ((range1 as NameImpl).RefersToRange != null)
                      this.AddNamedRangeCells((IRange) sourceRange, (range1 as NameImpl).RefersToRange, resultCells, precedentsCache, sourceList, isEntireWorkbook);
                  }
                  else if (range1 != null && (isEntireWorkbook ? 1 : (range1.Worksheet == rangeByString.Worksheet ? 1 : 0)) != 0 && !resultCells.ContainsKey(range1.Address))
                  {
                    if (precedentsCache == null)
                    {
                      if (!resultCells.ContainsKey(range1.Address))
                        resultCells.Add(range1.Address, range1);
                      if (!sourceList.Contains(range1.Address) && !isDirect)
                        sourceList.Add(range1.Address);
                    }
                    else
                    {
                      int row2 = range1.Row;
                      int column2 = range1.Column;
                      int lastRow2 = range1.LastRow;
                      int lastColumn2 = range1.LastColumn;
                      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(range1.Worksheet.Application, range1.Worksheet);
                      for (int iRow2 = row2; iRow2 <= lastRow2; ++iRow2)
                      {
                        for (int iColumn2 = column2; iColumn2 <= lastColumn2; ++iColumn2)
                        {
                          string address1 = sourceRange.Address;
                          migrantRangeImpl.ResetRowColumn(iRow2, iColumn2);
                          string address2 = migrantRangeImpl.Address;
                          if (!precedentsCache.ContainsKey(address1))
                          {
                            precedentsCache.Add(address1, new List<string>()
                            {
                              migrantRangeImpl.Address
                            });
                          }
                          else
                          {
                            List<string> stringList = precedentsCache[address1];
                            precedentsCache[address1].Add(address2);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    return resultCells;
  }

  private IRange[] GetDependentRanges(bool isEntireWorkbook, bool isDirect)
  {
    Dictionary<string, IRange> resultCells = new Dictionary<string, IRange>();
    this.m_book.InitializePrecedentsCache();
    if (!isEntireWorkbook)
    {
      this.GetDependentsBySheet(this.Worksheet, isEntireWorkbook);
    }
    else
    {
      foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.m_book.Worksheets)
        this.GetDependentsBySheet(worksheet, isEntireWorkbook);
    }
    List<string> stringList = new List<string>();
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.m_worksheet.Application, (IWorksheet) this.m_worksheet);
    if (this.m_iTopRow > 0 && this.m_iLeftColumn > 0)
    {
      for (int iTopRow = this.m_iTopRow; iTopRow <= this.m_iBottomRow; ++iTopRow)
      {
        for (int iLeftColumn = this.m_iLeftColumn; iLeftColumn <= this.m_iRightColumn; ++iLeftColumn)
        {
          migrantRangeImpl.ResetRowColumn(iTopRow, iLeftColumn);
          stringList.Add(migrantRangeImpl.Address);
        }
      }
    }
    if (this.m_book.PrecedentsCache != null)
    {
      for (int index = 0; index < stringList.Count; ++index)
        this.GetPrecedentsFromCache(stringList[index], this.m_book.PrecedentsCache, resultCells, isDirect);
    }
    this.m_book.PrecedentsCache.Clear();
    this.m_book.PrecedentsCache = (Dictionary<string, List<string>>) null;
    return resultCells == null || resultCells.Count == 0 ? (IRange[]) null : new List<IRange>((IEnumerable<IRange>) resultCells.Values).ToArray();
  }

  private IRange[] GetPrecedentsRange(bool isEntireWorkbook, bool isDirect)
  {
    Dictionary<string, IRange> referenceCells = this.GetReferenceCells((IRange) this, this.m_book.PrecedentsCache, isEntireWorkbook, isDirect);
    return referenceCells == null || referenceCells.Count == 0 ? (IRange[]) null : new List<IRange>((IEnumerable<IRange>) referenceCells.Values).ToArray();
  }

  private void AddNamedRangeCells(
    IRange sourceRange,
    IRange refersToRange,
    Dictionary<string, IRange> resultCells,
    Dictionary<string, List<string>> precedentsCache,
    List<string> sourceList,
    bool isEntireWorkbook)
  {
    if (refersToRange != null && refersToRange is NameImpl)
    {
      if ((refersToRange as NameImpl).RefersToRange == null)
        return;
      this.AddNamedRangeCells(sourceRange, (refersToRange as NameImpl).RefersToRange, resultCells, precedentsCache, sourceList, isEntireWorkbook);
    }
    else if (refersToRange is RangesCollection)
    {
      for (int index = 0; index < (refersToRange as RangesCollection).InnerList.Count; ++index)
      {
        IRange inner = (refersToRange as RangesCollection).InnerList[index];
        this.AddNamedRangeCells(sourceRange, inner, resultCells, precedentsCache, sourceList, isEntireWorkbook);
      }
    }
    else
    {
      if (refersToRange == null || (isEntireWorkbook ? 1 : (refersToRange.Worksheet == sourceRange.Worksheet ? 1 : 0)) == 0 || resultCells.ContainsKey(refersToRange.Address))
        return;
      if (precedentsCache == null)
      {
        if (!resultCells.ContainsKey(refersToRange.Address))
          resultCells.Add(refersToRange.Address, refersToRange);
        if (sourceList.Contains(refersToRange.Address))
          return;
        sourceList.Add(refersToRange.Address);
      }
      else
      {
        int row = refersToRange.Row;
        int column = refersToRange.Column;
        int lastRow = refersToRange.LastRow;
        int lastColumn = refersToRange.LastColumn;
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(refersToRange.Worksheet.Application, refersToRange.Worksheet);
        for (int iRow = row; iRow <= lastRow; ++iRow)
        {
          for (int iColumn = column; iColumn <= lastColumn; ++iColumn)
          {
            migrantRangeImpl.ResetRowColumn(iRow, iColumn);
            string address = sourceRange.Address;
            if (!precedentsCache.ContainsKey(address))
            {
              precedentsCache.Add(address, new List<string>()
              {
                migrantRangeImpl.Address
              });
            }
            else
            {
              List<string> stringList = precedentsCache[address];
              precedentsCache[address].Add(migrantRangeImpl.Address);
            }
            if (precedentsCache.ContainsKey(migrantRangeImpl.Address))
            {
              List<string> collection = precedentsCache[migrantRangeImpl.Address];
              precedentsCache[address].AddRange((IEnumerable<string>) collection);
            }
          }
        }
      }
    }
  }

  private void GetPrecedentsFromCache(
    string sourceCellsAddress,
    Dictionary<string, List<string>> precedentsCache,
    Dictionary<string, IRange> resultCells,
    bool isDirect)
  {
    List<string> stringList = new List<string>();
    stringList.Add(sourceCellsAddress);
    while (stringList.Count > 0)
    {
      string str = stringList[0];
      stringList.RemoveAt(0);
      foreach (KeyValuePair<string, List<string>> keyValuePair in precedentsCache)
      {
        if (keyValuePair.Value.Contains(str) && !resultCells.ContainsKey(keyValuePair.Key))
        {
          resultCells.Add(keyValuePair.Key, this.m_worksheet.GetRangeByString(keyValuePair.Key, false));
          if (!isDirect)
            stringList.Add(keyValuePair.Key);
        }
      }
    }
  }

  public string GetNewAddress(Dictionary<string, string> names, out string strSheetName)
  {
    strSheetName = this.m_worksheet.Name;
    if (names == null || !names.ContainsKey(strSheetName))
      return this.Address;
    strSheetName = names[strSheetName];
    return $"'{strSheetName.Replace("'", "''")}'!{this.AddressLocal}";
  }

  public IRange Clone(object parent, Dictionary<string, string> hashNewNames, WorkbookImpl book)
  {
    string str = this.m_worksheet.Name;
    if (hashNewNames != null && hashNewNames.ContainsKey(str))
      str = hashNewNames[str];
    WorksheetImpl worksheet = (WorksheetImpl) book.Worksheets[str];
    return worksheet == null ? this.m_worksheet.Range[this.FirstRow, this.FirstColumn, this.LastRow, this.LastColumn] : worksheet.Range[this.FirstRow, this.FirstColumn, this.LastRow, this.LastColumn];
  }

  public void ClearConditionalFormats()
  {
    this.m_worksheet.ConditionalFormats.Remove(this.GetRectangles(), false);
  }

  internal void ClearConditionalFormats(bool isCF)
  {
    this.m_worksheet.ConditionalFormats.Remove(this.GetRectangles(), isCF);
  }

  public void ClearDataValidations() => this.m_worksheet.DVTable.Remove(this.GetRectangles());

  public Rectangle[] GetRectangles()
  {
    return new Rectangle[1]
    {
      Rectangle.FromLTRB(this.FirstColumn - 1, this.FirstRow - 1, this.LastColumn - 1, this.LastRow - 1)
    };
  }

  public int GetRectanglesCount() => 1;

  public string WorksheetName => this.Worksheet.Name;

  internal static void UpdateCellValue(object Parent, int Column, int Row, bool updateCellVaue)
  {
    if (!(Parent is IWorksheet) || ((IWorksheet) Parent).CalcEngine == null || !updateCellVaue || !(((WorksheetBaseImpl) Parent).Workbook as WorkbookImpl).EnabledCalcEngine)
      return;
    string cellRef = RangeInfo.GetAlphaLabel(Column) + Row.ToString();
    ((IWorksheet) Parent).CalcEngine.PullUpdatedValue(cellRef);
  }

  [Obsolete("This method is obsolete and will be removed soon. Please use GetR1C1AddressFromCellIndex(long cellIndex) method. Sorry for inconvenience.")]
  public static string GetR1C1AddresFromCellIndex(long cellIndex)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    return RangeImpl.GetAddressLocal(rowFromCellIndex, columnFromCellIndex, rowFromCellIndex, columnFromCellIndex, true);
  }

  public static string GetR1C1AddressFromCellIndex(long cellIndex)
  {
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(cellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(cellIndex);
    return RangeImpl.GetAddressLocal(rowFromCellIndex, columnFromCellIndex, rowFromCellIndex, columnFromCellIndex, true);
  }

  public static long CellNameToIndex(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (name.Length < 2)
      throw new ArgumentException("name cannot be less then 2 symbols");
    int iRow = 0;
    int iColumn = 0;
    RangeImpl.CellNameToRowColumn(name, out iRow, out iColumn);
    return RangeImpl.GetCellIndex(iColumn, iRow);
  }

  public static void CellNameToRowColumn(string name, out int iRow, out int iColumn)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (name.Length < 2)
      throw new ArgumentException("name cannot be less then 2 symbols");
    int startIndex1 = -1;
    int length1 = 0;
    int startIndex2 = -1;
    int length2 = 0;
    int index = 0;
    for (int length3 = name.Length; index < length3; ++index)
    {
      char c = name[index];
      if (char.IsDigit(c))
      {
        if (startIndex2 < 0)
          startIndex2 = index;
        ++length2;
      }
      else if (char.IsLetter(c))
      {
        if (startIndex1 < 0)
          startIndex1 = index;
        ++length1;
      }
      else if (!char.IsPunctuation(c) && !char.IsWhiteSpace(c) && c != '$')
        throw new ArgumentOutOfRangeException(nameof (name), $"Character {(object) c} was not expected.");
    }
    if (startIndex2 == -1)
    {
      startIndex2 = 1;
      ++length2;
    }
    if (startIndex1 == -1)
    {
      startIndex1 = 0;
      ++length1;
    }
    string s = name.Substring(startIndex2, length2);
    string columnName = name.Substring(startIndex1, length1);
    if (char.IsLetter(s[0]))
      s = ((int) s[0]).ToString();
    iRow = int.Parse(s, NumberStyles.None, (IFormatProvider) NumberFormatInfo.InvariantInfo);
    iColumn = RangeImpl.GetColumnIndex(columnName);
  }

  public static int GetColumnIndex(string columnName)
  {
    int columnIndex = 0;
    int index = 0;
    for (int length = columnName.Length; index < length; ++index)
    {
      char ch = columnName[index];
      columnIndex = columnIndex * 26 + (1 + (ch >= 'a' ? (int) ch - 97 : (int) ch - 65));
    }
    if (columnIndex < 0)
      columnIndex = -columnIndex;
    return columnIndex;
  }

  public static string GetColumnName(int iColumn)
  {
    if (iColumn < 1)
      throw new ArgumentOutOfRangeException(nameof (iColumn), "Value cannot be less than 1.");
    --iColumn;
    string columnName = string.Empty;
    do
    {
      int num = iColumn % 26;
      iColumn = iColumn / 26 - 1;
      columnName = Convert.ToChar(65 + num).ToString() + columnName;
    }
    while (iColumn >= 0);
    return columnName;
  }

  public static string GetCellName(int firstColumn, int firstRow)
  {
    return RangeImpl.GetCellName(firstColumn, firstRow, false);
  }

  public static string GetCellName(int firstColumn, int firstRow, bool bR1C1)
  {
    return RangeImpl.GetCellName(firstColumn, firstRow, bR1C1, false);
  }

  public static string GetCellName(int firstColumn, int firstRow, bool bR1C1, bool bUseSeparater)
  {
    if (firstRow < 1)
      throw new ArgumentOutOfRangeException("Row index is wrong. It cannot be less then 1");
    if (bR1C1)
      return $"R{firstRow}C{firstColumn}";
    if (!bUseSeparater)
      return RangeImpl.GetColumnName(firstColumn) + (object) firstRow;
    return '$'.ToString() + RangeImpl.GetColumnName(firstColumn) + (object) '$' + (object) firstRow;
  }

  public static string GetAddressLocal(
    int iFirstRow,
    int iFirstColumn,
    int iLastRow,
    int iLastColumn)
  {
    string cellName1 = RangeImpl.GetCellName(iFirstColumn, iFirstRow);
    if (iFirstRow == iLastRow && iFirstColumn == iLastColumn)
      return cellName1;
    string cellName2 = RangeImpl.GetCellName(iLastColumn, iLastRow);
    return $"{cellName1}:{cellName2}";
  }

  public static string GetAddressLocal(
    int iFirstRow,
    int iFirstColumn,
    int iLastRow,
    int iLastColumn,
    bool bR1C1)
  {
    string cellName1 = RangeImpl.GetCellName(iFirstColumn, iFirstRow, bR1C1);
    if (iFirstRow == iLastRow && iFirstColumn == iLastColumn)
      return cellName1;
    string cellName2 = RangeImpl.GetCellName(iLastColumn, iLastRow, bR1C1);
    return $"{cellName1}:{cellName2}";
  }

  public static string GetCellNameWithDollars(int firstColumn, int firstRow)
  {
    if (firstColumn < 1 || firstRow < 1)
      throw new ArgumentOutOfRangeException("column or row index is wrong. It cannot be less then 1");
    return $"${RangeImpl.GetColumnName(firstColumn)}${(object) firstRow}";
  }

  public static long GetCellIndex(int firstColumn, int firstRow)
  {
    if (firstColumn == -1 || firstRow == -1)
      return -1;
    if (firstRow < 0 || firstColumn < 0)
      throw new ArgumentOutOfRangeException("wrong row or column index");
    return ((long) firstRow << 32 /*0x20*/) + (long) firstColumn;
  }

  [DebuggerStepThrough]
  public static int GetRowFromCellIndex(long index) => (int) (index >>> 32 /*0x20*/);

  [DebuggerStepThrough]
  public static int GetColumnFromCellIndex(long index) => (int) (index & (long) uint.MaxValue);

  public static string GetWorksheetName(ref string rangeName)
  {
    if (rangeName == null)
      throw new ArgumentNullException(nameof (rangeName));
    if (rangeName.Length == 0)
      throw new ArgumentException("rangeName - string cannot be empty");
    int startIndex = 0;
    string str1 = rangeName;
    int num1 = 0;
    int num2 = 0;
    string worksheetName = (string) null;
    char[] chArray = new char[rangeName.Length];
    char[] charArray = rangeName.ToCharArray();
    for (int index = 0; index < charArray.Length - 1; ++index)
    {
      if (charArray[index] == '\'' && charArray[index + 1] != '\'' && num2 == 0)
      {
        startIndex = index;
        ++num2;
      }
      if (charArray[index] == '!' && index > 2 && charArray[index - 1] == '\'' && charArray[index - 2] != '\'')
      {
        num1 = index;
        break;
      }
    }
    int length1 = rangeName.IndexOf('!');
    if (length1 != -1)
    {
      worksheetName = rangeName.Substring(startIndex, num1 - startIndex).Replace("''", "'");
      string str2 = rangeName.Substring(length1 + 1, rangeName.Length - length1 - 1);
      if (worksheetName == "")
      {
        string str3 = rangeName.Substring(0, length1);
        if (str3.Contains("("))
          str3 = str3.Substring(str3.IndexOf('(') + 1);
        worksheetName = str3;
        if (!str2.Contains(worksheetName))
          rangeName = rangeName.Substring(length1 + 1, rangeName.Length - length1 - 1);
      }
      else if (!str2.Contains(worksheetName))
        rangeName = rangeName.Substring(num1 + 1, rangeName.Length - num1 - 1);
      if (num1 != 0)
      {
        int length2 = worksheetName.Length;
        if (worksheetName[0] == '\'' && worksheetName[length2 - 1] == '\'')
          worksheetName = worksheetName.Substring(1, length2 - 2);
      }
    }
    return worksheetName;
  }

  public static bool GetWrapText(IList rangeColection)
  {
    if (rangeColection == null)
      throw new ArgumentNullException(nameof (rangeColection));
    bool wrapText = true;
    int count = rangeColection.Count;
    for (int index = 0; wrapText && index < count; ++index)
    {
      if (!(rangeColection[index] as IRange).WrapText)
      {
        wrapText = false;
        break;
      }
    }
    return wrapText;
  }

  public static void SetWrapText(IList rangeColection, bool wrapText)
  {
    int index = 0;
    for (int count = rangeColection.Count; index < count; ++index)
      ((IRange) rangeColection[index]).WrapText = wrapText;
  }

  public static string GetNumberFormat(IList rangeColection)
  {
    int count = rangeColection.Count;
    if (count == 0)
      return (string) null;
    IRange range1 = (IRange) rangeColection[0];
    string numberFormat = (string) null;
    if (!(range1 is ExternalRange))
    {
      numberFormat = range1.NumberFormat;
      for (int index = 1; index < count; ++index)
      {
        IRange range2 = (IRange) rangeColection[index];
        if (numberFormat != range2.NumberFormat)
          return (string) null;
      }
    }
    return numberFormat;
  }

  public static string GetCellStyleName(IList<IRange> rangeColection)
  {
    int count = rangeColection.Count;
    if (count == 0)
      return (string) null;
    string cellStyleName = rangeColection[0].CellStyleName;
    for (int index = 1; index < count; ++index)
    {
      IRange range = rangeColection[index];
      if (cellStyleName != range.CellStyleName)
        return (string) null;
    }
    return cellStyleName;
  }

  public static int ParseRangeString(
    string range,
    IWorkbook book,
    out int iFirstRow,
    out int iFirstColumn,
    out int iLastRow,
    out int iLastColumn)
  {
    iLastColumn = iLastRow = iFirstColumn = iFirstRow = -1;
    string[] cells = range.Split(':');
    int length = cells.Length;
    Match match1 = FormulaUtil.FullRowRangeRegex.Match(range);
    if (match1.Success && match1.Index == 0 && match1.Length == range.Length)
    {
      iFirstColumn = 1;
      iLastColumn = book.MaxColumnCount;
      string str1 = match1.Groups["Row1"].Value;
      if (str1.StartsWith("$"))
        str1 = UtilityMethods.RemoveFirstCharUnsafe(str1);
      string str2 = match1.Groups["Row2"].Value;
      if (str2.StartsWith("$"))
        str2 = UtilityMethods.RemoveFirstCharUnsafe(str2);
      iFirstRow = Convert.ToInt32(str1);
      iLastRow = Convert.ToInt32(str2);
      return length;
    }
    Match match2 = FormulaUtil.FullColumnRangeRegex.Match(range);
    if (match2.Success && match2.Index == 0 && match2.Length == range.Length)
    {
      string columnName1 = match2.Groups["Column1"].Value;
      if (columnName1.StartsWith("$"))
        columnName1 = UtilityMethods.RemoveFirstCharUnsafe(columnName1);
      string columnName2 = match2.Groups["Column2"].Value;
      if (columnName2.StartsWith("$"))
        columnName2 = UtilityMethods.RemoveFirstCharUnsafe(columnName2);
      iFirstColumn = RangeImpl.GetColumnIndex(columnName1);
      iLastColumn = RangeImpl.GetColumnIndex(columnName2);
      iFirstRow = 1;
      iLastRow = book.MaxRowCount;
      return length;
    }
    long index1 = -1;
    if (length >= 1)
    {
      index1 = RangeImpl.CellNameToIndex(cells[0]);
      iLastRow = iFirstRow = RangeImpl.GetRowFromCellIndex(index1);
      iLastColumn = iFirstColumn = RangeImpl.GetColumnFromCellIndex(index1);
    }
    if (length == 2)
    {
      long index2 = RangeImpl.CellNameToIndex(cells[1]);
      if (index1 != index2)
      {
        iLastRow = RangeImpl.GetRowFromCellIndex(index2);
        iLastColumn = RangeImpl.GetColumnFromCellIndex(index2);
      }
    }
    else if (length > 2)
      RangeImpl.GetMinMaxRowColFromCellNames(cells, ref iFirstRow, ref iFirstColumn, ref iLastRow, ref iLastColumn);
    return length;
  }

  internal static void GetMinMaxRowColFromCellNames(
    string[] cells,
    ref int iFirstRow,
    ref int iFirstColumn,
    ref int iLastRow,
    ref int iLastColumn)
  {
    foreach (string cell in cells)
    {
      int iRow;
      int iColumn;
      RangeImpl.CellNameToRowColumn(cell, out iRow, out iColumn);
      iFirstRow = Math.Min(iFirstRow, iRow);
      iLastRow = Math.Max(iLastRow, iRow);
      iFirstColumn = Math.Min(iFirstColumn, iColumn);
      iLastColumn = Math.Max(iLastColumn, iColumn);
    }
  }

  [Obsolete("This method is obsolete and will be removed soon. Please use GetRectangleOfRange(IRange range, bool bThrowExcONNullRange) method. Sorry for inconvenience.")]
  public static Rectangle GetRectangeOfRange(IRange range, bool bThrowExcONNullRange)
  {
    Rectangle rectangeOfRange = new Rectangle(-1, -1, -1, -1);
    if (range == null)
    {
      if (bThrowExcONNullRange)
        throw new ArgumentNullException(nameof (range));
      return rectangeOfRange;
    }
    rectangeOfRange.Y = range.Row;
    rectangeOfRange.Height = range.LastRow - rectangeOfRange.Y;
    rectangeOfRange.X = range.Column;
    rectangeOfRange.Width = range.LastColumn - rectangeOfRange.X;
    return rectangeOfRange;
  }

  public static Rectangle GetRectangleOfRange(IRange range, bool bThrowExcONNullRange)
  {
    Rectangle rectangleOfRange = new Rectangle(-1, -1, -1, -1);
    if (range == null)
    {
      if (bThrowExcONNullRange)
        throw new ArgumentNullException(nameof (range));
      return rectangleOfRange;
    }
    rectangleOfRange.Y = range.Row;
    rectangleOfRange.Height = range.LastRow - rectangleOfRange.Y;
    rectangleOfRange.X = range.Column;
    rectangleOfRange.Width = range.LastColumn - rectangleOfRange.X;
    return rectangleOfRange;
  }

  internal Color GetNumberFormatColor(ExtendedFormatImpl extendedFormatImpl)
  {
    Color empty = Color.Empty;
    if (extendedFormatImpl.IncludeNumberFormat)
    {
      Dictionary<char, Color> fromNumberFormat = RangeImpl.GetColorsFromNumberFormat(extendedFormatImpl.NumberFormat, empty);
      if (fromNumberFormat != null)
      {
        switch ((this.Worksheet as WorksheetImpl).GetCellType(this.Row, this.Column, true))
        {
          case WorksheetImpl.TRangeValueType.Number:
            if (this.Number > 0.0 && fromNumberFormat.ContainsKey('+'))
              empty = fromNumberFormat['+'];
            if (this.Number < 0.0 && fromNumberFormat.ContainsKey('-'))
              empty = fromNumberFormat['-'];
            if (this.Number == 0.0 && fromNumberFormat.ContainsKey('0'))
            {
              empty = fromNumberFormat['0'];
              break;
            }
            break;
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            if (this.FormulaNumberValue > 0.0 && fromNumberFormat.ContainsKey('+'))
              empty = fromNumberFormat['+'];
            if (this.FormulaNumberValue < 0.0 && fromNumberFormat.ContainsKey('-'))
              empty = fromNumberFormat['-'];
            if (this.FormulaNumberValue == 0.0 && fromNumberFormat.ContainsKey('0'))
            {
              empty = fromNumberFormat['0'];
              break;
            }
            break;
          default:
            if (fromNumberFormat.ContainsKey('t'))
            {
              empty = fromNumberFormat['t'];
              break;
            }
            break;
        }
      }
    }
    return empty;
  }

  internal static Dictionary<char, Color> GetColorsFromNumberFormat(
    string numberFormat,
    Color defaultColor)
  {
    Dictionary<char, Color> fromNumberFormat = (Dictionary<char, Color>) null;
    if (numberFormat.Contains("[") && numberFormat.Contains("]"))
    {
      Regex regex = new Regex("(\\[(?<Color>.*)\\])(?<Value>.*)");
      if (numberFormat.Contains(";"))
      {
        string[] strArray = Regex.Split(numberFormat, ";");
        for (int index = 0; index < strArray.Length && index < 5; ++index)
        {
          Match match = regex.Match(strArray[index]);
          if (match.Success)
          {
            if (fromNumberFormat == null)
            {
              fromNumberFormat = new Dictionary<char, Color>(strArray.Length + 1);
              fromNumberFormat['d'] = defaultColor;
            }
            Color color = RangeImpl.GetColor(match.Groups["Color"].Value);
            switch (index)
            {
              case 0:
                fromNumberFormat['+'] = color;
                continue;
              case 1:
                fromNumberFormat['-'] = color;
                continue;
              case 2:
                fromNumberFormat['0'] = color;
                continue;
              case 3:
                fromNumberFormat['t'] = color;
                continue;
              default:
                continue;
            }
          }
        }
      }
      else
      {
        Match match = regex.Match(numberFormat);
        if (match.Success)
        {
          fromNumberFormat = new Dictionary<char, Color>(2);
          fromNumberFormat['d'] = defaultColor;
          fromNumberFormat['+'] = RangeImpl.GetColor(match.Groups["Color"].Value);
        }
      }
    }
    return fromNumberFormat;
  }

  private static Color GetColor(string colorName)
  {
    Color color1 = new Color();
    Color color2 = colorName.Equals("green", StringComparison.OrdinalIgnoreCase) ? Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0) : Color.FromName(colorName);
    if (!color2.IsKnownColor && colorName.IndexOf("Color", StringComparison.InvariantCultureIgnoreCase) != -1)
    {
      Regex regex = new Regex("\\d+");
      if (regex.IsMatch(colorName))
      {
        int int32 = Convert.ToInt32(regex.Match(colorName).Value);
        if (int32 > 0 && int32 <= 56)
          color2 = int32 > 8 ? WorkbookImpl.DEF_PALETTE[int32 + 7] : WorkbookImpl.DEF_PALETTE[int32 - 1];
      }
    }
    if (color2.ToArgb() != 0)
      color1 = Color.FromArgb((int) byte.MaxValue, (int) color2.R, (int) color2.G, (int) color2.B);
    return color1;
  }

  internal static Image CropImage(
    Image cropableImage,
    double leftOffset,
    double topOffset,
    double rightOffset,
    double bottomOffset,
    bool isTransparent)
  {
    double width1 = (double) cropableImage.Width;
    double height1 = (double) cropableImage.Height;
    leftOffset = width1 * (leftOffset / 100.0);
    topOffset = height1 * (topOffset / 100.0);
    rightOffset = width1 * (rightOffset / 100.0);
    bottomOffset = height1 * (bottomOffset / 100.0);
    int width2 = (int) (width1 - (leftOffset + rightOffset));
    int height2 = (int) (height1 - (topOffset + bottomOffset));
    Bitmap bitmap = !RangeImpl.IsPixelFormatSupported(cropableImage.PixelFormat) ? new Bitmap(width2, height2) : new Bitmap(width2, height2, cropableImage.PixelFormat);
    bitmap.SetResolution(cropableImage.VerticalResolution, cropableImage.HorizontalResolution);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    if (isTransparent)
    {
      graphics.CompositingMode = CompositingMode.SourceCopy;
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
      graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
      graphics.Clear(Color.Transparent);
    }
    Rectangle srcRect = new Rectangle((int) leftOffset, (int) topOffset, width2, height2);
    graphics.DrawImage(cropableImage, 0, 0, srcRect, GraphicsUnit.Pixel);
    if (isTransparent)
      bitmap.MakeTransparent();
    return (Image) bitmap;
  }

  private static bool IsPixelFormatSupported(PixelFormat pixelFormat)
  {
    switch (pixelFormat)
    {
      case PixelFormat.Format16bppRgb555:
      case PixelFormat.Format16bppRgb565:
      case PixelFormat.Format24bppRgb:
      case PixelFormat.Format32bppRgb:
      case PixelFormat.Format32bppPArgb:
      case PixelFormat.Format48bppRgb:
      case PixelFormat.Format64bppPArgb:
      case PixelFormat.Format32bppArgb:
      case PixelFormat.Format64bppArgb:
        return true;
      default:
        return false;
    }
  }

  protected internal void wrapStyle_OnNumberFormatChanged(object sender, EventArgs e)
  {
    RangeImpl.TCellType cellType = this.CellType;
    string old = this.Value;
    this.OnValueChanged(old, old);
    this.OnStyleChanged(cellType);
  }

  private void AttachEventToStyle()
  {
    this.AttachEvent((ExtendedFormatWrapper) this.m_book.InnerStyles.GetByXFIndex(this.m_book.InnerExtFormats[(int) this.ExtendedFormatIndex].ParentIndex), new EventHandler(this.wrapStyle_OnNumberFormatChanged));
  }

  private void AttachEventToCellStyle()
  {
    this.AttachEvent((ExtendedFormatWrapper) this.m_style, new EventHandler(this.wrapStyle_OnNumberFormatChanged));
  }

  private void AttachEvent(ExtendedFormatWrapper wrapper, EventHandler handler)
  {
    wrapper.NumberFormatChanged += handler;
  }

  protected void CreateStyle()
  {
    int num = this.m_book.DefaultXFIndex;
    if (!this.m_book.Loading && !this.m_book.m_bisUnusedXFRemoved && !this.m_book.IsConverting)
      this.m_book.RemoveUnusedXFRecord();
    BiffRecordRaw record = this.Record;
    if (record != null)
      num = (int) ((ICellPositionFormat) record).ExtendedFormatIndex;
    this.CreateStyleWrapper(num);
  }

  protected void CreateStyleWrapper(int value)
  {
    if (!this.IsSingleCell && !this.IsEntireRow && !this.IsEntireColumn)
      throw new ArgumentException("This method can be used only for single cell not a range");
    Syncfusion.XlsIO.Implementation.CellStyle style = this.m_style;
    this.m_style = new Syncfusion.XlsIO.Implementation.CellStyle(this, value);
  }

  internal static IStyle CreateTempStyleWrapperWithoutRange(RangeImpl rangeImpl, int value)
  {
    return (IStyle) new Syncfusion.XlsIO.Implementation.CellStyle(rangeImpl, value);
  }

  public void SetXFormatIndex(int index)
  {
    if (!this.IsSingleCell && !this.IsEntireRow && !this.IsEntireColumn)
      throw new ApplicationException("This method should be called for single range cells only");
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0");
    ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[index];
    ExtendedFormatImpl extendedFormatImpl = innerExtFormat.Record.XFType != ExtendedFormatRecord.TXFType.XF_CELL ? innerExtFormat.CreateChildFormat() : innerExtFormat.CreateChildFormat(this.m_book.InnerExtFormats[(int) this.ExtendedFormatIndex]);
    index = extendedFormatImpl.Index;
    if (this.IsEntireRow)
    {
      int row = this.Row;
      int lastRow = this.LastRow;
      for (int iRow = row; iRow <= lastRow; ++iRow)
        this.m_worksheet.CellRecords.SetCellStyle(iRow, index);
    }
    else if (this.IsEntireColumn)
    {
      int firstColumn = this.FirstColumn;
      int lastColumn = this.LastColumn;
      for (int index1 = firstColumn; index1 <= lastColumn; ++index1)
      {
        ColumnInfoRecord record = this.m_worksheet.ColumnInformation[index1];
        if (record == null)
        {
          record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
          record.FirstColumn = (ushort) (index1 - 1);
          record.LastColumn = (ushort) (index1 - 1);
        }
        record.ExtendedFormatIndex = (ushort) index;
        this.m_worksheet.ColumnInformation[index1] = record;
      }
    }
    else
      this.m_worksheet.CellRecords.SetCellStyle(this.Row, this.Column, index);
    if (this.m_style != null)
      this.m_style.SetFormatIndex(index);
    if (Array.IndexOf<ExcelLineStyle>(RangeImpl.ThinBorders, extendedFormatImpl.BottomBorderLineStyle) >= 0)
      return;
    RowStorage row1 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, this.Row - 1, false);
    if (row1 == null)
      return;
    row1.IsSpaceBelowRow = true;
  }

  private StyleImpl ChangeStyleName(string strNewName)
  {
    int Index = this.m_book.DefaultXFIndex;
    StyleImpl styleImpl = (StyleImpl) null;
    if (strNewName != null && strNewName.Length > 0)
    {
      if (!this.m_book.InnerStyles.ContainsName(strNewName) && this.m_book.Version != ExcelVersion.Excel97to2003)
      {
        Array.IndexOf<string>(this.DefaultStyleNames, strNewName);
        styleImpl = this.m_book.InnerStyles.CreateBuiltInStyle(strNewName);
      }
      else
        styleImpl = (StyleImpl) this.m_book.Styles[strNewName];
      if (styleImpl != null)
      {
        if (this.ExtendedFormat.IsFirstSymbolApostrophe)
        {
          ExtendedFormatImpl extFormat = this.Workbook.CreateExtFormat(true) as ExtendedFormatImpl;
          extFormat.ParentIndex = styleImpl.Index;
          extFormat.IsFirstSymbolApostrophe = this.ExtendedFormat.IsFirstSymbolApostrophe;
          Index = extFormat.Index;
        }
        else
        {
          Index = styleImpl.Index;
          if (styleImpl.BuiltIn && styleImpl.Name != "Normal" && (this.CellStyle.HorizontalAlignment != ExcelHAlign.HAlignGeneral || this.CellStyle.VerticalAlignment != ExcelVAlign.VAlignBottom))
          {
            ExcelHAlign horizontalAlignment = this.CellStyle.HorizontalAlignment;
            ExcelVAlign verticalAlignment = this.CellStyle.VerticalAlignment;
            this.ExtendedFormatIndex = (ushort) Index;
            Syncfusion.XlsIO.Implementation.CellStyle cellStyle = (Syncfusion.XlsIO.Implementation.CellStyle) this.m_style.Clone((object) this.m_book);
            cellStyle.HorizontalAlignment = horizontalAlignment;
            cellStyle.VerticalAlignment = verticalAlignment;
            this.UpdateCellStyleIndex();
            return styleImpl;
          }
          if (this.CellStyle != null && this.CellStyle.HasBorder)
          {
            IBorder border1 = this.CellStyle.Borders[ExcelBordersIndex.EdgeTop];
            IBorder border2 = this.CellStyle.Borders[ExcelBordersIndex.EdgeBottom];
            IBorder border3 = this.CellStyle.Borders[ExcelBordersIndex.EdgeLeft];
            IBorder border4 = this.CellStyle.Borders[ExcelBordersIndex.EdgeRight];
            ExcelLineStyle lineStyle1 = border1.LineStyle;
            ExcelLineStyle lineStyle2 = border2.LineStyle;
            ExcelLineStyle lineStyle3 = border3.LineStyle;
            ExcelLineStyle lineStyle4 = border4.LineStyle;
            this.ExtendedFormatIndex = (ushort) Index;
            Syncfusion.XlsIO.Implementation.CellStyle cellStyle = (Syncfusion.XlsIO.Implementation.CellStyle) this.m_style.Clone((object) this.m_book);
            if (cellStyle.RightBorderLineStyle == ExcelLineStyle.None & cellStyle.LeftBorderLineStyle == ExcelLineStyle.None & cellStyle.TopBorderLineStyle == ExcelLineStyle.None & cellStyle.BottomBorderLineStyle == ExcelLineStyle.None & cellStyle.Name != "Normal" && this.IsSingleCell)
            {
              this.SetBorderToSingleCell(ExcelBordersIndex.EdgeTop, lineStyle1, border1.Color);
              this.SetBorderToSingleCell(ExcelBordersIndex.EdgeBottom, lineStyle2, border2.Color);
              this.SetBorderToSingleCell(ExcelBordersIndex.EdgeLeft, lineStyle3, border3.Color);
              this.SetBorderToSingleCell(ExcelBordersIndex.EdgeRight, lineStyle4, border4.Color);
              this.UpdateCellStyleIndex();
              return styleImpl;
            }
          }
          if (this.CellStyle.Name != "Normal" && this.CellStyle.FillForeground != ExcelKnownColors.BlackCustom)
          {
            Color color = this.CellStyle.Color;
            this.ExtendedFormatIndex = (ushort) Index;
            Syncfusion.XlsIO.Implementation.CellStyle cellStyle = (Syncfusion.XlsIO.Implementation.CellStyle) this.m_style.Clone((object) this.m_book);
            if (this.CellStyle.ColorIndex == ExcelKnownColors.None && this.CellStyle.Name != "Normal")
              cellStyle.Color = color;
            this.UpdateCellStyleIndex();
            return styleImpl;
          }
        }
      }
      else
        styleImpl = (StyleImpl) this.m_book.Styles[Index];
    }
    ExtendedFormatRecord record1 = styleImpl.m_xFormat.Record;
    ExtendedFormatRecord record2 = this.ExtendedFormat.Record;
    this.ExtendedFormatIndex = (ushort) Index;
    this.UpdateCellStyleIndex();
    switch (styleImpl.Name)
    {
      case "Comma":
        this.NumberFormat = this.m_book.InnerFormats.DEF_FORMAT_STRING[29];
        break;
      case "Comma [0]":
        this.NumberFormat = this.m_book.InnerFormats.DEF_FORMAT_STRING[27];
        break;
      case "Currency":
        this.NumberFormat = this.m_book.InnerFormats.DEF_FORMAT_STRING[30];
        break;
      case "Currency [0]":
        this.NumberFormat = this.m_book.InnerFormats.DEF_FORMAT_STRING[28];
        break;
      case "Percent":
        this.NumberFormat = this.m_book.InnerFormats.DEF_FORMAT_STRING[9];
        break;
    }
    return styleImpl;
  }

  private void UpdateCellStyleIndex()
  {
    if (!this.m_book.m_bisXml || this.m_book.IsConverting || this.m_book.Options == ExcelParseOptions.ParseWorksheetsOnDemand)
      return;
    if (this.m_book.m_usedCellStyleIndex.ContainsKey((int) this.ExtendedFormatIndex))
    {
      int num;
      this.m_book.m_usedCellStyleIndex.TryGetValue((int) this.ExtendedFormatIndex, out num);
      this.m_book.m_usedCellStyleIndex[(int) this.ExtendedFormatIndex] = num + 1;
    }
    else
      this.m_book.m_usedCellStyleIndex.Add((int) this.ExtendedFormatIndex, this.Cells.Length);
  }

  private string GetStyleName()
  {
    if (this.m_style != null)
      return this.m_style.Name;
    int extendedFormatIndex = (int) this.ExtendedFormatIndex;
    StyleImpl styleImpl = this.m_book.InnerStyles.GetByXFIndex(extendedFormatIndex);
    if (styleImpl == null)
    {
      styleImpl = this.m_book.InnerStyles.GetByXFIndex(this.m_book.InnerExtFormats[extendedFormatIndex].ParentIndex);
      if (styleImpl == null)
      {
        styleImpl = this.m_book.InnerStyles["Normal"] as StyleImpl;
        this.ExtendedFormatIndex = (ushort) styleImpl.Index;
      }
    }
    return styleImpl.Name;
  }

  private bool GetWrapText()
  {
    return this.m_style != null ? this.m_style.WrapText : this.ExtendedFormat.WrapText;
  }

  private string GetNumberFormat()
  {
    return this.m_style != null ? this.m_style.NumberFormat : this.ExtendedFormat.NumberFormat;
  }

  internal bool TryGetDateTimeByCulture(string strDateTime, bool isUKCulture, out DateTime dtValue)
  {
    if (strDateTime == null)
      throw new ArgumentNullException(nameof (strDateTime));
    string shortDatePattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
    dtValue = DateTime.Now;
    if (this.ExtendedFormat.NumberFormatIndex == 14)
    {
      if (DateTime.TryParse(strDateTime, (IFormatProvider) Thread.CurrentThread.CurrentCulture, DateTimeStyles.NoCurrentDateDefault, out dtValue) || DateTime.TryParse(strDateTime, (IFormatProvider) new CultureInfo("cy-GB"), DateTimeStyles.None, out dtValue))
        return true;
    }
    else if (isUKCulture && DateTime.TryParse(strDateTime, (IFormatProvider) new CultureInfo("cy-GB"), DateTimeStyles.None, out dtValue))
      return true;
    return false;
  }

  internal ExtendedFormatImpl ExtendedFormat
  {
    get => this.m_book.InnerExtFormats[(int) this.ExtendedFormatIndex];
  }

  public void UpdateRecord()
  {
    if (this.m_style == null)
      return;
    this.m_style.SetFormatIndex((int) ((ICellPositionFormat) this.Record).ExtendedFormatIndex);
  }

  public bool GetAreArrayFormulasNotSeparated(Dictionary<ArrayRecord, object> hashToSkip)
  {
    if (this.m_worksheet.IsInsertingSubTotal && this.m_worksheet.IsArrayFormulaSeparated)
      return true;
    if (hashToSkip == null)
      hashToSkip = new Dictionary<ArrayRecord, object>();
    CellRecordCollection cellRecords = this.m_worksheet.CellRecords;
    int firstRow = this.FirstRow;
    for (int lastRow = this.LastRow; firstRow <= lastRow; ++firstRow)
    {
      int iCol = this.FirstColumn;
      int record;
      for (int lastColumn = this.LastColumn; iCol <= lastColumn; iCol = record + 1)
      {
        record = cellRecords.FindRecord(TBIFFRecord.Formula, firstRow, iCol, lastColumn);
        if (record <= lastColumn)
        {
          ArrayRecord arrayRecord = cellRecords.GetArrayRecord(firstRow, record);
          if (arrayRecord != null && !hashToSkip.ContainsKey(arrayRecord))
          {
            if (arrayRecord.FirstRow + 1 < this.FirstRow || arrayRecord.LastRow + 1 > lastRow || arrayRecord.FirstColumn + 1 < this.FirstColumn || arrayRecord.LastColumn + 1 > lastColumn)
              return false;
            hashToSkip.Add(arrayRecord, (object) null);
          }
        }
      }
    }
    this.m_worksheet.IsArrayFormulaSeparated = true;
    return true;
  }

  private string GetCustomizedFormat(string format)
  {
    string customizedFormat = format.ToLower();
    string[] strArray = new string[4]{ ",", " ", ".", "-" };
    foreach (string oldValue in strArray)
    {
      if (customizedFormat.Contains(oldValue))
      {
        string newValue = oldValue.PadLeft(2, '\\');
        customizedFormat = customizedFormat.Replace(oldValue, newValue);
      }
    }
    return customizedFormat;
  }

  public void Reparse()
  {
    if (this.CellType == RangeImpl.TCellType.Formula)
      this.ReParseFormula((FormulaRecord) this.Record);
  }

  TBIFFRecord ICellPositionFormat.TypeCode => (TBIFFRecord) this.CellType;

  int ICellPositionFormat.Column
  {
    get => this.FirstColumn - 1;
    set
    {
      if (!this.IsSingleCell)
        throw new ArgumentException("This property can be called only for single cell ranges");
      this.FirstColumn = this.LastColumn = value + 1;
    }
  }

  int ICellPositionFormat.Row
  {
    get => this.FirstRow - 1;
    set
    {
      if (!this.IsSingleCell)
        throw new ArgumentException("This property can be called only for single cell ranges");
      this.FirstRow = this.LastRow = value + 1;
    }
  }

  public Ptg[] GetNativePtg()
  {
    int num = this.m_book.AddSheetReference(this.m_worksheet.Name);
    Ptg ptg;
    if (this.IsSingleCell)
      ptg = FormulaUtil.CreatePtg(FormulaToken.tRef3d1, (object) num, (object) (this.FirstRow - 1), (object) (this.FirstColumn - 1), (object) (byte) 0);
    else
      ptg = FormulaUtil.CreatePtg(FormulaToken.tArea3d1, (object) num, (object) (this.FirstRow - 1), (object) (this.FirstColumn - 1), (object) (this.LastRow - 1), (object) (this.LastColumn - 1), (object) (byte) 0, (object) (byte) 0);
    return new Ptg[1]{ ptg };
  }

  public IEnumerator<IRange> GetEnumerator()
  {
    return ((IEnumerable<IRange>) this.CellsList).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.CellsList).GetEnumerator();

  internal void OnCellValueChanged(object oldValue, object newValue, IRange range)
  {
    (this.Worksheet as WorksheetImpl).OnCellValueChanged(oldValue, newValue, range);
  }

  public enum TCellType
  {
    Formula = 6,
    RString = 214, // 0x000000D6
    LabelSST = 253, // 0x000000FD
    Blank = 513, // 0x00000201
    Number = 515, // 0x00000203
    Label = 516, // 0x00000204
    BoolErr = 517, // 0x00000205
    RK = 638, // 0x0000027E
  }

  private delegate IOutline OutlineGetter(int iOutlineIndex);

  public delegate void CellValueChangedEventHandler(object sender, CellValueChangedEventArgs e);
}
