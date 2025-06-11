// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RangeImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.FormatParser;
using Syncfusion.OfficeChart.FormatParser.FormatTokens;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RangeImpl : 
  IEnumerable<IRange>,
  IReparse,
  ICombinedRange,
  IRange,
  IParentApplication,
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
  private const int FormulaLengthXls = 255 /*0xFF*/;
  private const int FormulaLengthXlsX = 8192 /*0x2000*/;
  internal const char SingleQuote = '\'';
  private const string NEW_LINE = "\n";
  private const string DEF_PERCENTAGE_FORMAT = "0%";
  private const string DEF_DECIMAL_PERCENTAGE_FORMAT = "0.00%";
  private const string DEF_EXPONENTIAL_FORMAT = "0.00E+00";
  private const string DEF_CULTUREINFO_TIMETOKEN = "tt";
  private const string DEF_TIMETOKEN_FORMAT = "AM/PM";
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
  private static readonly OfficeLineStyle[] ThinBorders = new OfficeLineStyle[3]
  {
    OfficeLineStyle.None,
    OfficeLineStyle.Hair,
    OfficeLineStyle.Thin
  };
  private string[] DEF_DATETIME_FORMULA = new string[4]
  {
    "TIME",
    "DATE",
    "TODAY",
    "NOW"
  };
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
  protected Syncfusion.OfficeChart.Implementation.CellStyle m_style;
  private bool m_bCells;
  [ThreadStatic]
  private static string m_dateSeparator = (string) null;
  [ThreadStatic]
  private static string m_timeSeparator = (string) null;
  protected IRTFWrapper m_rtfString;
  private char[] unnecessaryChar = new char[3]
  {
    '_',
    '?',
    '*'
  };
  private string[] osCultureSpecficFormats = new string[2]
  {
    "m/d/yyyy",
    "m/d/yy h:mm"
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
  private Dictionary<int, List<Point>> m_outlineLevels;
  private int m_noOfSubtotals;

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
        columnGroupLevel = this[firstRow, firstColumn].ColumnGroupLevel;
        for (int column = firstColumn + 1; column <= lastColumn; ++column)
        {
          if (columnGroupLevel != this[firstRow, column].ColumnGroupLevel)
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

  public DateTime DateTime
  {
    get
    {
      this.CheckDisposed();
      double number1 = this.m_worksheet.GetNumber(this.Row, this.Column);
      if (number1 < 0.0 || this.InnerNumberFormat.GetFormatType(number1) != OfficeFormatType.DateTime)
        return Convert.ToDateTime(this.m_worksheet.GetText(this.Row, this.Column));
      if (number1 == double.NaN || this.InnerNumberFormat.GetFormatType(number1) != OfficeFormatType.DateTime)
        return DateTime.MinValue;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double number2 = this.m_worksheet.GetNumber(row, column);
          if (number2 == double.NaN || number1 != number2 || this.InnerNumberFormat.GetFormatType(number2) != OfficeFormatType.DateTime)
            return DateTime.MinValue;
        }
      }
      return UtilityMethods.ConvertNumberToDateTime(number1, this.m_book.Date1904);
    }
    set
    {
      this.CheckDisposed();
      if (this.m_book.Date1904)
        value = DateTime.FromOADate(value.ToOADate() - 1462.0);
      if (this.IsSingleCell)
      {
        this.FormatType = OfficeFormatType.DateTime;
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

  public string DisplayText => this.GetDisplayText(this.Row, this.Column, this.InnerNumberFormat);

  private string GetCultureFormat(string result, double dNumber, FormatImpl numberFormat)
  {
    if (numberFormat.FormatType == OfficeFormatType.DateTime && this.CheckOSSpecificDateFormats(numberFormat) && result != string.Empty)
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
    get => this.m_isEntireRow;
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
      RangeImpl entireColumn = this[1, this.FirstColumn, this.m_book.MaxRowCount, this.LastColumn] as RangeImpl;
      entireColumn.IsEntireColumn = true;
      return (IRange) entireColumn;
    }
  }

  public IRange EntireRow
  {
    get
    {
      this.CheckDisposed();
      RangeImpl entireRow = this[this.FirstRow, 1, this.LastRow, this.m_book.MaxColumnCount] as RangeImpl;
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
      string formula1;
      if (this.IsSingleCell)
      {
        formula1 = this.HasFormulaArray ? $"{{{this.FormulaArray}}}" : this.m_worksheet.GetFormula(this.Row, this.Column, false);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
        formula1 = migrantRangeImpl.Formula;
        if (formula1 != null)
        {
          int row = this.Row;
          for (int lastRow = this.LastRow; row <= lastRow; ++row)
          {
            int column = this.Column;
            for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
            {
              migrantRangeImpl.ResetRowColumn(row, column);
              string formula2 = migrantRangeImpl.Formula;
              if (formula1 != formula2)
              {
                formula1 = (string) null;
                break;
              }
            }
          }
        }
      }
      return formula1;
    }
    set
    {
      if (this.Workbook.Version == OfficeVersion.Excel97to2003 && value.Length > (int) byte.MaxValue)
        throw new ArgumentException("The formula is too long.Formulas length should not be longer then 255");
      if (value.Length > 8192 /*0x2000*/)
        throw new ArgumentException("The formula is too long.Formulas length should not be longer then 8192");
      this.CheckDisposed();
      this.TryRemoveFormulaArrays();
      if (value[0] != '=')
        value = '='.ToString() + value;
      this.Value = value;
    }
  }

  public string FormulaArray
  {
    get
    {
      this.CheckDisposed();
      return this.GetFormulaArray(false);
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
      if (formulaNumberValue1 == double.NaN || this.InnerNumberFormat.GetFormatType(formulaNumberValue1) != OfficeFormatType.DateTime)
        return DateTime.MinValue;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double formulaNumberValue2 = this.m_worksheet.GetFormulaNumberValue(row, column);
          if (formulaNumberValue2 == double.NaN || formulaNumberValue1 != formulaNumberValue2 || this.InnerNumberFormat.GetFormatType(formulaNumberValue2) != OfficeFormatType.DateTime)
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
        this.FormatType = OfficeFormatType.DateTime;
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
      if (this.IsSingleCell)
        return !this.HasFormulaArray ? this.m_worksheet.GetFormula(this.Row, this.Column, true) : $"{{{this.FormulaArrayR1C1}}}";
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
      string formulaR1C1_1 = migrantRangeImpl.FormulaR1C1;
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
    }
  }

  public string FormulaArrayR1C1
  {
    get
    {
      this.CheckDisposed();
      return this.GetFormulaArray(true);
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

  public OfficeHAlign HorizontalAlignment
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.CellStyle.HorizontalAlignment : OfficeHAlign.HAlignGeneral;
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
        if (!this[firstRow, column].IsGroupedByColumn)
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
        if (!this[row, firstColumn].IsGroupedByRow)
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
          this.SetNumberAndFormat(value, false);
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
            this[firstRow, firstColumn].Number = value;
        }
      }
    }
  }

  public string NumberFormat
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.GetNumberFormat() : RangeImpl.GetNumberFormat((IList) this.CellsList);
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
      int rowGroupLevel = this[firstRow, firstColumn].RowGroupLevel;
      for (int row = firstRow + 1; row <= lastRow; ++row)
      {
        if (rowGroupLevel != this[row, firstColumn].RowGroupLevel)
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
      this.SetRowHeight(value, true);
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
            RangeImpl rangeImpl = this[row, column] as RangeImpl;
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
            RangeImpl rangeImpl = this[row, column] as RangeImpl;
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
        if (this.HasRichText)
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
          string str = this.Value;
        }
        this.OnStyleChanged(cellType);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.CellStyle = value;
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
        string str = this.Value;
        this.OnStyleChanged(cellType);
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
            migrantRangeImpl.CellStyleName = value;
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
      if (this.ExtendedFormat.IsFirstSymbolApostrophe)
        text = "'" + text;
      return text;
    }
    set
    {
      this.CheckDisposed();
      if (value == null)
        throw new ArgumentNullException(nameof (Text));
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
          if (this.NumberFormat != "General" && this.FormatType != OfficeFormatType.Unknown)
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
      }
      if (!value.Contains(Environment.NewLine) && !value.Contains("\n"))
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
      if (number1 == double.NaN || this.InnerNumberFormat.GetFormatType(number1) != OfficeFormatType.DateTime)
        return TimeSpan.MinValue;
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          double number2 = this.m_worksheet.GetNumber(row, column);
          if (number2 == double.NaN || number1 != number2 || this.InnerNumberFormat.GetFormatType(number2) != OfficeFormatType.DateTime)
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
        this.FormatType = OfficeFormatType.DateTime;
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
      string s;
      if (this.IsSingleCell)
      {
        s = this.m_worksheet.GetValue(this.Record as ICellPositionFormat, false);
      }
      else
      {
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        migrantRangeImpl.ResetRowColumn(this.Row, this.Column);
        s = migrantRangeImpl.Value;
        int row = this.Row;
        for (int lastRow = this.LastRow; row <= lastRow; ++row)
        {
          int column = this.Column;
          for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
          {
            migrantRangeImpl.ResetRowColumn(row, column);
            string str = migrantRangeImpl.Value;
            if (s != str)
            {
              s = (string) null;
              break;
            }
          }
        }
      }
      if (!this.Workbook.Date1904 || !this.HasDateTime)
        return s;
      DateTime.Parse(s).ToOADate();
      return s;
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
        MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
        for (int firstRow = this.FirstRow; firstRow <= this.LastRow; ++firstRow)
        {
          for (int firstColumn = this.FirstColumn; firstColumn <= this.LastColumn; ++firstColumn)
          {
            migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
            migrantRangeImpl.Value = value;
          }
        }
      }
      if (value == null || !value.Contains(Environment.NewLine) && !value.Contains("\n"))
        return;
      this.WrapText = true;
    }
  }

  protected void OnValueChanged(string old, string value)
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
      if (this.Record is BlankRecord)
        return;
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
      {
        this.SetFormula(value);
      }
      else
      {
        if (this.DetectAndSetBoolErrValue(value) || innerFormat.FormatType == OfficeFormatType.Number && this.DetectAndSetFractionValue(value))
          return;
        DateTime dateTime = DateTime.FromOADate(0.0);
        double result2;
        bool flag3 = double.TryParse(value, Array.IndexOf<string>(this.floatNumberStyleCultures, Thread.CurrentThread.CurrentCulture.Name) >= 0 ? NumberStyles.Float : NumberStyles.Any, (IFormatProvider) cultureInfo, out result2);
        if (!flag3)
          flag3 = double.TryParse(value, Array.IndexOf<string>(this.floatNumberStyleCultures, Thread.CurrentThread.CurrentCulture.Name) >= 0 ? NumberStyles.Float : NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2);
        if (flag3)
          flag3 = this.m_worksheet.checkIsNumber(value, cultureInfo);
        bool flag4 = !flag3 && this.TryParseDateTime(value, out dateTime);
        if (!flag3 && !flag4)
        {
          string dateSeparator = cultureInfo.DateTimeFormat.DateSeparator;
          string timeSeparator = cultureInfo.DateTimeFormat.TimeSeparator;
          string str = (string) null;
          if (dateSeparator == "/" && value.Contains("-"))
            str = value.Replace("-", "/");
          else if (dateSeparator == "-" && value.Contains("/"))
            str = value.Replace("/", "-");
          if (str != null)
            flag4 = this.TryParseDateTime(str, out dateTime);
          else if (value.Contains(timeSeparator))
            flag4 = DateTime.TryParse(value, out dateTime);
        }
        if (flag4)
        {
          long ticks = dateTime.Ticks;
          if (ticks < RangeImpl.MinAllowedDateTicks && ticks != 0L)
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
        if (flag4 && innerFormat.FormatType == OfficeFormatType.General)
        {
          DateTime result3 = new DateTime();
          flag5 = DateTime.TryParseExact(value, this.Workbook.DateTimePatterns, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result3);
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
          this.SetNumber((double) (Convert.ToDecimal(s1) / 100M));
          if (!(this.NumberFormat == "General") && !(this.NumberFormat == "mm/dd/yyyy"))
            return;
          if (long.TryParse(s1, out result1))
            this.FormatType = OfficeFormatType.Percentage;
          else
            this.FormatType = OfficeFormatType.DecimalPercentage;
        }
        else if (flag2 && flag1)
        {
          this.SetNumber(Convert.ToDouble(s2));
          this.FormatType = OfficeFormatType.Currency;
        }
        else if ((flag3 || flag4) && value != double.NaN.ToString() && (innerFormat.FormatType == OfficeFormatType.General || innerFormat.FormatType == OfficeFormatType.Number || innerFormat.FormatType == OfficeFormatType.DateTime) && (!flag4 || innerFormat.FormatType != OfficeFormatType.General || flag5) && s2 == null && !flag1)
        {
          if (flag4 && value.Contains(RangeImpl.TimeSeparator) && !value.Contains(RangeImpl.DateSeperator) && (this.NumberFormat == "General" || innerFormat.IsTimeFormat(result2)))
          {
            if (this.NumberFormat == "General")
              this.SetTimeFormat(value);
            string str = result2.ToString();
            int startIndex = str.IndexOf(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            if (startIndex != -1)
              result2 = Convert.ToDouble(str.Substring(startIndex, str.Length - startIndex));
            else
              this.FormatType = OfficeFormatType.DateTime;
          }
          else if (flag4)
            this.FormatType = OfficeFormatType.DateTime;
          else if (value.Contains("E+") && (this.NumberFormat == "General" || this.NumberFormat == "mm/dd/yyyy"))
            this.FormatType = OfficeFormatType.Exponential;
          this.SetNumber(result2);
        }
        else
        {
          value = this.CheckApostrophe(value);
          this.RichText.Text = value;
        }
      }
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

  public OfficeVAlign VerticalAlignment
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell ? this.CellStyle.VerticalAlignment : OfficeVAlign.VAlignBottom;
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
      row = this.NormalizeRowIndex(row, column, lastColumn);
      lastRow = this.NormalizeRowIndex(lastRow, column, lastColumn);
      column = this.NormalizeColumnIndex(column, row, lastRow);
      lastColumn = this.NormalizeColumnIndex(lastColumn, row, lastRow);
      this.CheckRange(row, column);
      this.CheckRange(lastRow, lastColumn);
      return row != lastRow || column != lastColumn ? (IRange) this.AppImplementation.CreateRange(this.Parent, column, row, lastColumn, lastRow) : this[row, column];
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
      name = name.ToUpper();
      if (IsR1C1Notation)
        return this.ParseR1C1Reference(name);
      int iFirstRow;
      int iFirstColumn;
      int iLastRow;
      int iLastColumn;
      switch (RangeImpl.ParseRangeString(name, (IWorkbook) this.Workbook, out iFirstRow, out iFirstColumn, out iLastRow, out iLastColumn))
      {
        case 1:
          return this[iFirstRow, iFirstColumn];
        case 2:
          return this[iFirstRow, iFirstColumn, iLastRow, iLastColumn];
        default:
          throw new ArgumentException();
      }
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
          if (double.IsNaN(formulaNumberValue) || this.InnerNumberFormat.GetFormatType(formulaNumberValue) != OfficeFormatType.DateTime)
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
          if (double.IsNaN(formulaNumberValue) || this.InnerNumberFormat.GetFormatType(formulaNumberValue) == OfficeFormatType.DateTime)
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
          if (this.m_worksheet.GetCellType(row, column, false) != WorksheetImpl.TRangeValueType.Blank || this.Worksheet[row, column].HasStyle && this.Worksheet[row, column].CellStyle.FillPattern != OfficePattern.None || this.Worksheet[row, column].CellStyle.HasBorder)
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
          if (double.IsNaN(number) || this.InnerNumberFormat.GetFormatType(number) != OfficeFormatType.DateTime)
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
            OfficeFormatType formatType = this.InnerNumberFormat.GetFormatType(number);
            if (formatType == OfficeFormatType.Unknown && number == 0.0)
              formatType = this.InnerNumberFormat.GetFormatType(1.0);
            if (formatType != OfficeFormatType.DateTime && formatType != OfficeFormatType.Text)
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

  public IRichTextString RichText
  {
    get
    {
      this.CheckDisposed();
      if (this.m_rtfString == null)
        this.CreateRichTextString();
      return (IRichTextString) this.m_rtfString;
    }
  }

  public bool HasRichText
  {
    get
    {
      this.CheckDisposed();
      return this.IsSingleCell && this.HasString && this.RichText.IsFormatted;
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
      return mergeCell1 != null || mergeCell2 != null;
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
          if (this.m_worksheet.Range[row, column].HasStyle)
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
        if (row != null && !this.Workbook.IsWorkbookOpening)
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
          if (row != null && !row.IsBadFontHeight && !this.Workbook.IsWorkbookOpening)
            row.IsWrapText = value;
        }
      }
      this.SetChanged();
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

  internal static string DateSeperator
  {
    get
    {
      if (RangeImpl.m_dateSeparator == null)
        RangeImpl.m_dateSeparator = RangeImpl.GetDateSeperator();
      return RangeImpl.m_dateSeparator;
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

  protected void OnStyleChanged(RangeImpl.TCellType oldType)
  {
    if (oldType == RangeImpl.TCellType.LabelSST && this.CellType != RangeImpl.TCellType.LabelSST)
    {
      string str = this.Value;
      if (str != null && str.Length != 0)
        this.m_rtfString.Clear();
    }
    this.SetChanged();
  }

  private string CheckApostrophe(string value)
  {
    if (value == null || value.Length == 0 || this.m_book.IsWorkbookOpening)
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

  protected RangeImpl ToggleGroup(OfficeGroupBy groupBy, bool isGroup, bool bCollapsed)
  {
    WorksheetImpl worksheet = this.Worksheet as WorksheetImpl;
    Dictionary<int, int> dictionary1 = new Dictionary<int, int>();
    Dictionary<int, int> indexAndLevels = worksheet.IndexAndLevels;
    if (isGroup)
      this.SetWorksheetSize();
    if (this.m_outlineLevels == null)
      this.m_outlineLevels = new Dictionary<int, List<Point>>();
    worksheet.OutlineWrappers = (List<IOutlineWrapper>) null;
    int num1;
    int num2;
    RangeImpl.OutlineGetter outlineGetter;
    if (groupBy == OfficeGroupBy.ByRows)
    {
      num1 = this.FirstRow;
      num2 = this.LastRow;
      outlineGetter = new RangeImpl.OutlineGetter(this.GetRowOutline);
    }
    else
    {
      num1 = this.FirstColumn;
      num2 = this.LastColumn;
      outlineGetter = new RangeImpl.OutlineGetter(this.GetColumnOutline);
    }
    for (int index = num1; index <= num2; ++index)
    {
      IOutline outline = outlineGetter(index);
      if (isGroup && outline.OutlineLevel < (ushort) 7)
      {
        ++outline.OutlineLevel;
        if (groupBy == OfficeGroupBy.ByColumns)
        {
          if (indexAndLevels.ContainsKey(index))
            ++indexAndLevels[index];
          else
            indexAndLevels.Add(index, (int) outline.OutlineLevel);
        }
      }
      else if (!isGroup && outline.OutlineLevel > (ushort) 0)
      {
        int num3 = (int) outline.OutlineLevel--;
        if (groupBy == OfficeGroupBy.ByColumns)
        {
          if (num3 > 1)
            --indexAndLevels[index];
          else
            indexAndLevels.Remove(index);
        }
      }
      if (outline.OutlineLevel == (ushort) 0)
        outline.IsHidden = false;
      else if (isGroup && (outline.OutlineLevel >= (ushort) 1 || bCollapsed))
      {
        outline.IsHidden = bCollapsed;
        outline.IsCollapsed = bCollapsed;
      }
    }
    if (groupBy != OfficeGroupBy.ByRows)
    {
      int[] numArray = new int[indexAndLevels.Count];
      indexAndLevels.Keys.CopyTo(numArray, 0);
      List<int> intList = new List<int>((IEnumerable<int>) numArray);
      intList.Sort();
      Dictionary<int, int> dictionary2 = indexAndLevels;
      Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
      foreach (int key in intList)
        dictionary3.Add(key, dictionary2[key]);
    }
    return this;
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
    this.LastRow = subTotalImpl.CalculateSubTotal(this.FirstRow, this.FirstColumn - 1, this.LastRow, this.LastColumn - 1, groupBy, function, this.m_noOfSubtotals, totalList, replace, pageBreaks, summaryBelowData);
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
    OfficeGroupBy groupBy,
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
      if (groupBy == OfficeGroupBy.ByRows)
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
    BiffRecordRaw numberRecord = this.CreateNumberRecord(value);
    ICellPositionFormat cellPositionFormat = numberRecord as ICellPositionFormat;
    ExtendedFormatImpl innerExtFormat = this.m_book.InnerExtFormats[(int) cellPositionFormat.ExtendedFormatIndex];
    FormatImpl innerFormat = this.m_book.InnerFormats[innerExtFormat.NumberFormatIndex];
    if (innerFormat.FormatString != "General")
    {
      OfficeFormatType formatType = innerFormat.GetFormatType(value);
      switch (formatType)
      {
        case OfficeFormatType.General:
        case OfficeFormatType.Number:
          break;
        default:
          if (!isPreserveFormat && formatType != OfficeFormatType.Unknown)
          {
            int orCreateFormat = this.m_book.InnerFormats.FindOrCreateFormat("0.00");
            ExtendedFormatImpl format = innerExtFormat.Clone() as ExtendedFormatImpl;
            format.NumberFormatIndex = orCreateFormat;
            ExtendedFormatImpl extendedFormatImpl = this.m_book.InnerExtFormats.Add(format);
            cellPositionFormat.ExtendedFormatIndex = (ushort) extendedFormatImpl.Index;
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

  [CLSCompliant(false)]
  protected internal void SetFormula(FormulaRecord record)
  {
    this.Record = record != null ? (BiffRecordRaw) record : throw new ArgumentNullException(nameof (record));
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

  private void MoveCellsUp(OfficeCopyRangeOptions options)
  {
    int row = this.LastRow + 1;
    int firstColumn = this.FirstColumn;
    int lastRow = this.m_worksheet.UsedRange.LastRow;
    int lastColumn = this.LastColumn;
    if (row > lastRow)
      return;
    this.m_worksheet.MoveRange(this.m_worksheet.Range[this.FirstRow, this.FirstColumn], this.m_worksheet.Range[row, firstColumn, lastRow, lastColumn], options, true);
  }

  private void MoveCellsLeft(OfficeCopyRangeOptions options)
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
      if (this.m_book.IsWorkbookOpening)
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
      this.m_worksheet.InnerSetRowHeight(iRowIndex, value, bIsBadFontHeight, MeasureUnits.Point, true);
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

  private void BlankCell() => this.Record = this.CreateRecord(TBIFFRecord.Blank);

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

  internal bool TryParseDateTime(string value, out DateTime dateValue)
  {
    if (this.m_book.DetectDateTimeInValue && value.Contains(RangeImpl.DateSeperator))
      return DateTime.TryParse(value, out dateValue);
    dateValue = DateTime.MinValue;
    return false;
  }

  private static string GetDateSeperator()
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
        return this[r1C1Expression.Top, r1C1Expression.Left, r1C1Expression.Bottom, r1C1Expression.Right];
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
    OfficeParseFormulaOptions options = OfficeParseFormulaOptions.RootLevel | OfficeParseFormulaOptions.InArray;
    if (bR1C1)
      options |= OfficeParseFormulaOptions.UseR1C1;
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

  public void PartialClear()
  {
    this.m_cells = (List<IRange>) null;
    this.m_style = (Syncfusion.OfficeChart.Implementation.CellStyle) null;
    this.m_bCells = false;
    this.m_rtfString = (IRTFWrapper) null;
  }

  protected void SetBorderToSingleCell(
    OfficeBordersIndex borderIndex,
    OfficeLineStyle borderLine,
    OfficeKnownColors borderColor)
  {
    if (!this.IsSingleCell)
      throw new NotSupportedException("Supports only for single cell.");
    IBorder border = this.Borders[borderIndex];
    border.LineStyle = borderLine;
    border.Color = borderColor;
  }

  private void CollapseExpand(OfficeGroupBy groupBy, bool isCollapsed, ExpandCollapseFlags flags)
  {
    int iStartIndex;
    int iEndIndex;
    int iMaxIndex;
    bool bLastIndex;
    RangeImpl.OutlineGetter outlineGetter;
    if (groupBy == OfficeGroupBy.ByRows)
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
    if (iOutlineIndex <= iMaxIndex && iOutlineIndex > 0)
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
    {
      IOutline outline = outlineGetter(iOutlineIndex);
      outline.IsHidden = state;
      outline.IsCollapsed = state;
    }
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
    IOutline outline;
    do
    {
      iOutlineIndex += delta;
      outline = outlineGetter(iOutlineIndex);
    }
    while (iOutlineIndex > 0 && iOutlineIndex <= maximum && (int) outline.OutlineLevel >= parentGroupLevel);
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
      formulaValue = double.IsNaN(formulaNumberValue) ? "" : formatImpl.ApplyFormat(formulaNumberValue);
    }
label_8:
    return formulaValue;
  }

  [CLSCompliant(false)]
  protected OfficeFormatType FormatType
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
        case OfficeFormatType.Text:
          this.NumberFormat = "@";
          break;
        case OfficeFormatType.Number:
          this.NumberFormat = "0.00";
          break;
        case OfficeFormatType.DateTime:
          if (this.m_worksheet.IsImporting)
          {
            this.NumberFormat = $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt", "AM/PM")}";
            break;
          }
          this.NumberFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
          break;
        case OfficeFormatType.Percentage:
          this.NumberFormat = "0%";
          break;
        case OfficeFormatType.Currency:
          string str = (string) null;
          this.m_book.InnerFormats.CurrencyFormatStrings.TryGetValue(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, out str);
          this.NumberFormat = str;
          break;
        case OfficeFormatType.DecimalPercentage:
          this.NumberFormat = "0.00%";
          break;
        case OfficeFormatType.Exponential:
          this.NumberFormat = "0.00E+00";
          break;
      }
    }
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
        double.TryParse(strArray[0], out result1);
        fraction2 = new Fraction(result1, 1.0);
        if (strArray.Length > 1)
          str = strArray[1];
      }
      else
        fraction2 = new Fraction(0.0, 1.0);
      string[] strArray1 = str.Split(new char[1]{ '/' }, StringSplitOptions.RemoveEmptyEntries);
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
    int num = 0;
    string amDesignator = CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator;
    string pmDesignator = CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator;
    string str = value;
    if ((str.Contains(amDesignator) || str.Contains(pmDesignator)) && amDesignator != "" && pmDesignator != "")
      flag = true;
    int startIndex;
    for (; str.Contains(RangeImpl.TimeSeparator); str = str.Remove(startIndex, 1))
    {
      ++num;
      startIndex = str.IndexOf(RangeImpl.TimeSeparator);
    }
    switch (num)
    {
      case 1:
        if (flag)
        {
          this.NumberFormat = $"h{RangeImpl.TimeSeparator}mm AM/PM";
          break;
        }
        this.NumberFormat = $"h{RangeImpl.TimeSeparator}mm";
        break;
      case 2:
        if (flag)
        {
          this.NumberFormat = $"h{RangeImpl.TimeSeparator}mm{RangeImpl.TimeSeparator}ss AM/PM";
          break;
        }
        this.NumberFormat = $"h{RangeImpl.TimeSeparator}mm{RangeImpl.TimeSeparator}ss";
        break;
    }
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
    if (this.Workbook.Version == OfficeVersion.Excel97to2003 && value.Length > (int) byte.MaxValue)
      throw new ArgumentException("The formula is too long. Length should not be longer than 1024");
    if (value.Length > 8192 /*0x2000*/)
      throw new ArgumentException("The formula is too long.Formulas length should not be longer then 8192");
    if (value[0] == '=')
      value = value.Substring(1, value.Length - 1);
    int iCellRow = this.Row - 1;
    int iCellColumn = this.Column - 1;
    Ptg[] array = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) this.m_worksheet, hashWorksheetNames, iCellRow, iCellColumn, bR1C1);
    FormulaRecord recordWithoutAdd = (FormulaRecord) this.CreateRecordWithoutAdd(TBIFFRecord.Formula);
    recordWithoutAdd.ParsedExpression = array;
    this.Record = (BiffRecordRaw) recordWithoutAdd;
    FormulaUtil.RaiseFormulaEvaluation((object) this, new EvaluateEventArgs((IRange) this, array));
  }

  private void SetAutoFormatPattern(
    OfficeKnownColors color,
    int iRow,
    int iLastRow,
    int iCol,
    int iLastCol)
  {
    this.SetAutoFormatPattern(color, iRow, iLastRow, iCol, iLastCol, OfficeKnownColors.Black, OfficePattern.Solid);
  }

  private void SetAutoFormatPattern(
    OfficeKnownColors color,
    int iRow,
    int iLastRow,
    int iCol,
    int iLastCol,
    OfficeKnownColors patCol,
    OfficePattern pat)
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
    OfficeKnownColors officeKnownColors1 = OfficeKnownColors.White | OfficeKnownColors.BlackCustom;
    OfficeKnownColors officeKnownColors2 = OfficeKnownColors.BlackCustom;
    switch (type)
    {
      case ExcelAutoFormat.Classic_2:
        this.SetAutoFormatPattern(officeKnownColors1, firstRow + 1, lastRow, firstColumn + 1, lastColumn, officeKnownColors2, OfficePattern.None);
        this.SetAutoFormatPattern(OfficeKnownColors.Grey_25_percent, firstRow + 1, lastRow, firstColumn, firstColumn);
        this.SetAutoFormatPattern(OfficeKnownColors.Violet, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Classic_3:
        this.SetAutoFormatPattern(officeKnownColors1, lastRow, lastRow, firstColumn, lastColumn, officeKnownColors2, OfficePattern.None);
        this.SetAutoFormatPattern(OfficeKnownColors.Grey_25_percent, firstRow + 1, lastRow - 1, firstColumn, lastColumn);
        this.SetAutoFormatPattern(OfficeKnownColors.Dark_blue, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Colorful_1:
        this.SetAutoFormatPattern(OfficeKnownColors.Dark_blue, firstRow + 1, lastRow, firstColumn, lastColumn);
        this.SetAutoFormatPattern(OfficeKnownColors.Teal, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
        this.SetAutoFormatPattern(OfficeKnownColors.Black, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Colorful_2:
        int iLastCol = firstColumn == lastColumn ? lastColumn : lastColumn - 1;
        this.SetAutoFormatPattern(OfficeKnownColors.Grey_25_percent, firstRow + 1, lastRow, lastColumn, lastColumn);
        this.SetAutoFormatPattern(OfficeKnownColors.YellowCustom, firstRow + 1, lastRow, firstColumn, iLastCol, OfficeKnownColors.WhiteCustom, OfficePattern.Percent70);
        this.SetAutoFormatPattern(OfficeKnownColors.Dark_red, firstRow, firstRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.Colorful_3:
        this.SetAutoFormatPattern(OfficeKnownColors.Black, firstRow, lastRow, firstColumn, lastColumn);
        break;
      case ExcelAutoFormat.List_1:
        this.SetListAutoFormatPattern(true, officeKnownColors1, officeKnownColors2);
        break;
      case ExcelAutoFormat.List_2:
        this.SetListAutoFormatPattern(false, officeKnownColors1, officeKnownColors2);
        break;
      case ExcelAutoFormat.Effect3D_1:
      case ExcelAutoFormat.Effect3D_2:
        this.SetAutoFormatPattern(OfficeKnownColors.Grey_25_percent, firstRow, lastRow, firstColumn, lastColumn);
        break;
      default:
        this.SetAutoFormatPattern(officeKnownColors1, firstRow, lastRow, firstColumn, lastColumn, officeKnownColors2, OfficePattern.None);
        break;
    }
  }

  private void SetListAutoFormatPattern(
    bool bIsList_1,
    OfficeKnownColors foreCol,
    OfficeKnownColors backColor)
  {
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    this.SetAutoFormatPattern(foreCol, lastRow, lastRow, firstColumn, lastColumn, backColor, OfficePattern.None);
    OfficeKnownColors color = bIsList_1 ? OfficeKnownColors.Grey_25_percent : OfficeKnownColors.Light_green;
    int num1 = bIsList_1 ? 2 : 4;
    int num2 = 0;
    for (int index = lastRow - firstRow - 1; num2 < index; ++num2)
    {
      if (num2 % num1 < num1 / 2)
        this.SetAutoFormatPattern(color, num2 + firstRow + 1, num2 + firstRow + 1, firstColumn, lastColumn);
      else
        this.SetAutoFormatPattern(foreCol, num2 + firstRow + 1, num2 + firstRow + 1, firstColumn, lastColumn, backColor, OfficePattern.None);
    }
    if (bIsList_1)
      this.SetAutoFormatPattern(color, firstRow, firstRow, firstColumn, lastColumn);
    else
      this.SetAutoFormatPattern(OfficeKnownColors.Green, firstRow, firstRow, firstColumn, lastColumn, OfficeKnownColors.Teal, OfficePattern.Percent70);
  }

  private void SetAutoFormatAlignments(ExcelAutoFormat type)
  {
    int firstRow = this.FirstRow;
    int lastRow = this.LastRow;
    int firstColumn = this.FirstColumn;
    int lastColumn = this.LastColumn;
    if (type == ExcelAutoFormat.None)
    {
      this.SetAutoFormatAlignment(OfficeHAlign.HAlignGeneral, firstRow, lastRow, firstColumn, lastColumn);
    }
    else
    {
      OfficeHAlign align1 = OfficeHAlign.HAlignLeft;
      this.SetAutoFormatAlignment(OfficeHAlign.HAlignGeneral, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
      this.SetAutoFormatAlignment(OfficeHAlign.HAlignGeneral, firstRow, firstRow, firstColumn, firstColumn);
      if (firstRow != lastRow)
        this.SetAutoFormatAlignment(OfficeHAlign.HAlignLeft, lastRow, lastRow, firstColumn, firstColumn);
      if (type == ExcelAutoFormat.List_3)
        align1 = OfficeHAlign.HAlignGeneral;
      this.SetAutoFormatAlignment(align1, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
      OfficeHAlign align2 = OfficeHAlign.HAlignCenter;
      if (Array.IndexOf<ExcelAutoFormat>(RangeImpl.DEF_AUTOFORMAT_RIGHT, type) != -1)
        align2 = OfficeHAlign.HAlignRight;
      this.SetAutoFormatAlignment(align2, firstRow, firstRow, firstColumn + 1, lastColumn);
    }
  }

  private void SetAutoFormatAlignment(
    OfficeHAlign align,
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
        cellStyle.VerticalAlignment = OfficeVAlign.VAlignBottom;
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
      this.Worksheet.AutofitRow(firstRow);
    int firstColumn = this.FirstColumn;
    for (int lastColumn = this.LastColumn; firstColumn <= lastColumn; ++firstColumn)
      this.Worksheet.AutofitColumn(firstColumn);
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
    FontImpl font1 = ((FontImpl) this.m_book.InnerFonts[0]).Clone((object) this.m_book.InnerFonts);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    int iLastRow = flag ? lastRow : lastRow - 1;
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, iLastRow, firstColumn, firstColumn);
    FontImpl font2 = font1.Clone((object) this.m_book.InnerFonts);
    font2.Bold = true;
    this.SetAutoFormatFont((IOfficeFont) font2, firstRow, firstRow, firstColumn + 1, lastColumn);
    if (flag)
      return;
    this.SetAutoFormatFont((IOfficeFont) font2, lastRow, lastRow, firstColumn, firstColumn);
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
    FontImpl font1 = ((FontImpl) this.m_book.InnerFonts[0]).Clone((object) this.m_book.InnerFonts);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    int iLastRow = flag ? lastRow : lastRow - 1;
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, iLastRow, firstColumn, firstColumn);
    FontImpl font2 = font1.Clone((object) this.m_book.InnerFonts);
    font2.Bold = true;
    this.SetAutoFormatFont((IOfficeFont) font2, firstRow, firstRow, firstColumn + 1, lastColumn);
    if (!flag)
      this.SetAutoFormatFont((IOfficeFont) font2, lastRow, lastRow, firstColumn, firstColumn);
    if (firstColumn != lastColumn)
      this.SetAutoFormatFont((IOfficeFont) font2, firstRow, firstRow, lastColumn, lastColumn);
    FontImpl font3 = font2.Clone((object) this.m_book.InnerFonts);
    font3.Bold = false;
    font3.Italic = true;
    this.SetAutoFormatFont((IOfficeFont) font3, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
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
    FontImpl font1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, firstRow, firstColumn, firstColumn);
    FontImpl font2 = font1.Clone((object) innerFonts);
    font2.Bold = true;
    this.SetAutoFormatFont((IOfficeFont) font2, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
    FontImpl font3 = font2.Clone((object) innerFonts);
    font3.Color = OfficeKnownColors.Dark_blue;
    if (firstRow != lastRow)
      this.SetAutoFormatFont((IOfficeFont) font3, lastRow, lastRow, firstColumn, firstColumn);
    FontImpl font4 = font3.Clone((object) innerFonts);
    font4.Bold = false;
    font4.Size = 9.0;
    font4.Color = OfficeKnownColors.White;
    this.SetAutoFormatFont((IOfficeFont) font4, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
    FontImpl font5 = font4.Clone((object) innerFonts);
    font5.Bold = true;
    if (firstColumn == lastColumn)
      return;
    this.SetAutoFormatFont((IOfficeFont) font5, firstRow, firstRow, lastColumn, lastColumn);
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
    FontImpl font1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    font1.Color = OfficeKnownColors.Dark_blue;
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, firstRow, firstColumn, firstColumn);
    FontImpl font2 = font1.Clone((object) innerFonts);
    font2.Bold = true;
    font2.Color = OfficeKnownColors.Black;
    this.SetAutoFormatFont((IOfficeFont) font2, firstRow + 1, lastRow, firstColumn, firstColumn);
    FontImpl font3 = font2.Clone((object) innerFonts);
    font3.Color = OfficeKnownColors.White;
    font3.Italic = true;
    font3.Size = 9.0;
    this.SetAutoFormatFont((IOfficeFont) font3, firstRow, firstRow, firstColumn + 1, lastColumn);
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
    FontImpl font1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, firstRow, firstColumn, firstColumn);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
    FontImpl font2 = font1.Clone((object) innerFonts);
    font2.Italic = true;
    if (firstRow != lastRow)
      this.SetAutoFormatFont((IOfficeFont) font2, lastRow, lastRow, firstColumn, firstColumn);
    FontImpl font3 = font2.Clone((object) innerFonts);
    font3.Bold = true;
    font3.Size = 9.0;
    if (firstColumn != lastColumn)
      this.SetAutoFormatFont((IOfficeFont) font3, firstRow, firstRow, lastColumn, lastColumn);
    FontImpl font4 = font3.Clone((object) innerFonts);
    font4.Color = OfficeKnownColors.Grey_50_percent;
    this.SetAutoFormatFont((IOfficeFont) font4, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
  }

  private void SetAutoFormatFontBorderAccounting_2(bool bIsFont, bool bIsBorder)
  {
    if (!bIsFont && !bIsBorder)
      return;
    FontsCollection innerFonts = this.m_book.InnerFonts;
    if (!bIsFont)
      return;
    this.SetAutoFormatFont((IOfficeFont) ((FontImpl) innerFonts[0]).Clone((object) innerFonts), this.Row, this.LastRow, this.Column, this.LastColumn);
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
    FontImpl font1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, firstRow, firstColumn, firstColumn);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow, firstColumn + 1, lastColumn);
    FontImpl font2 = font1.Clone((object) innerFonts);
    font2.Italic = true;
    this.SetAutoFormatFont((IOfficeFont) font2, firstRow + 1, lastRow - 1, firstColumn, firstColumn);
    FontImpl font3 = font2.Clone((object) innerFonts);
    font3.Size = 9.0;
    this.SetAutoFormatFont((IOfficeFont) font3, firstRow, firstRow, firstColumn + 1, lastColumn - 1);
    FontImpl font4 = font3.Clone((object) innerFonts);
    font4.Bold = true;
    font4.Italic = true;
    if (firstColumn != lastColumn)
      this.SetAutoFormatFont((IOfficeFont) font4, firstRow, firstRow, lastColumn, lastColumn);
    FontImpl font5 = font4.Clone((object) innerFonts);
    font5.Bold = true;
    font5.Italic = false;
    font5.Size = 10.0;
    if (firstRow == lastRow)
      return;
    this.SetAutoFormatFont((IOfficeFont) font5, lastRow, lastRow, firstColumn, firstColumn);
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
    FontImpl font1 = ((FontImpl) innerFonts[0]).Clone((object) innerFonts);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow, lastRow, firstColumn, firstColumn);
    this.SetAutoFormatFont((IOfficeFont) font1, firstRow + 1, lastRow - 2, firstColumn + 1, lastColumn);
    FontImpl font2 = font1.Clone((object) innerFonts);
    font2.Underline = OfficeUnderline.SingleAccounting;
    this.SetAutoFormatFont((IOfficeFont) font2, firstRow, firstRow, firstColumn + 1, lastColumn);
    if (lastRow - firstRow > 1)
      this.SetAutoFormatFont((IOfficeFont) font2, lastRow - 1, lastRow - 1, firstColumn + 1, lastColumn);
    FontImpl font3 = font2.Clone((object) innerFonts);
    font3.Underline = OfficeUnderline.DoubleAccounting;
    if (firstRow == lastRow)
      return;
    this.SetAutoFormatFont((IOfficeFont) font3, lastRow, lastRow, firstColumn + 1, lastColumn);
  }

  private void SetAutoFormatFont(
    IOfficeFont font,
    int iRow,
    int iLastRow,
    int iCol,
    int iLastCol)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (iRow > iLastRow || iCol > iLastCol)
      return;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, this.Worksheet);
    migrantRangeImpl.ResetRowColumn(iRow, iCol);
    IOfficeFont font1 = migrantRangeImpl.CellStyle.Font;
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
    if (this.m_cells != null)
      this.m_cells.Clear();
    this.m_cells = (List<IRange>) null;
    this.m_bCells = false;
  }

  public void Dispose()
  {
    if (this.m_style != null)
      this.m_style = (Syncfusion.OfficeChart.Implementation.CellStyle) null;
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
    return this.InnerNumberFormat.GetFormatType(doubleValue) == OfficeFormatType.DateTime && doubleValue < 2958466.0 ? this.DateTime.ToString() : doubleValue.ToString();
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
          int externNameIndex = (int) name.ExternNameIndex;
          RangeImpl.AttachDetachLocalNameEvent(book, name, iBookIndex, iNewIndex, handler, indexes, bAdd);
        }
      }
    }
    catch (Exception ex)
    {
      if (book.IsWorkbookOpening)
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
      ((NameImpl) book.Names[(int) namex.NameIndex - 1]).NameIndexChanged += handler;
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
    long index = RangeImpl.GetIndex(-1, (int) name.ExternNameIndex);
    bool flag = iBookIndex == -1 && iNewIndex == -1;
    if (indexes.ContainsKey(index) || (iBookIndex != -1 || (int) name.ExternNameIndex != iNewIndex) && !flag)
      return;
    NameImpl name1 = (NameImpl) book.Names[(int) name.ExternNameIndex - 1];
    if (bAdd)
      name1.NameIndexChanged += handler;
    else
      name1.NameIndexChanged -= handler;
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
        if (e.OldIndex == (int) namePtg.ExternNameIndex - 1)
          namePtg.ExternNameIndex = (ushort) (e.NewIndex + 1);
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

  public IRange Activate()
  {
    this.CheckDisposed();
    if (!this.IsSingleCell)
      return (IRange) null;
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

  public IRange Group(OfficeGroupBy groupBy, bool bCollapsed)
  {
    this.CheckDisposed();
    return (IRange) this.ToggleGroup(groupBy, true, bCollapsed);
  }

  public IRange Group(OfficeGroupBy groupBy)
  {
    this.CheckDisposed();
    return this.Group(groupBy, false);
  }

  public void Merge() => this.Merge(false);

  public void Merge(bool clearCells)
  {
    this.CheckDisposed();
    if (this.IsSingleCell)
      return;
    this.m_worksheet.MergeCells.AddMerge(this, OfficeMergeOperation.Delete);
    this.m_worksheet.ClearExceptFirstCell(this, clearCells);
    if (this.m_book.IsLoaded)
      return;
    int column = this.Column;
    int row1 = this.Row;
    int lastRow = this.LastRow;
    int lastColumn1 = this.LastColumn;
    if (row1 != lastRow)
      return;
    int num = column;
    for (int lastColumn2 = this.LastColumn; num <= lastColumn2; ++num)
    {
      if (this[row1, num] != null)
      {
        RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) (this.Worksheet as WorksheetImpl), row1 - 1, false);
        if (this[row1, num].WrapText && !row2.IsBadFontHeight)
          this.m_worksheet.AutofitRow(row1, num, lastColumn2, true);
      }
    }
  }

  internal void MergeWithoutCheck()
  {
    if (this.IsSingleCell)
      return;
    this.m_worksheet.MergeCells.AddMerge(this, OfficeMergeOperation.Leave);
  }

  public IRange Ungroup(OfficeGroupBy groupBy)
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
        this.m_worksheet.TopLeftCell = this.m_worksheet[this.FirstColumn, this.FirstColumn];
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

  public void Clear(OfficeMoveDirection direction)
  {
    this.CheckDisposed();
    this.Clear(direction, OfficeCopyRangeOptions.None);
  }

  public void Clear(OfficeMoveDirection direction, OfficeCopyRangeOptions options)
  {
    this.CheckDisposed();
    switch (direction)
    {
      case OfficeMoveDirection.MoveLeft:
        this.Clear(true);
        this.MoveCellsLeft(options);
        break;
      case OfficeMoveDirection.MoveUp:
        this.Clear(true);
        this.MoveCellsUp(options);
        break;
      case OfficeMoveDirection.None:
        this.Clear(true);
        break;
    }
  }

  internal void ClearOption(OfficeClearOptions option)
  {
    this.CheckDisposed();
    switch (option)
    {
      case OfficeClearOptions.ClearFormat:
        this.CellStyleName = "Normal";
        int num1 = 0;
        int num2 = 0;
        List<IRange> cellsList1 = this.CellsList;
        int index1 = 0;
        for (int count = cellsList1.Count; index1 < count; ++index1)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList1[index1];
          if (num1 == 0 && !rangeImpl.IsBlank)
            num1 = rangeImpl.Row;
          if (!rangeImpl.IsBlank)
            num2 = rangeImpl.Row;
        }
        if (this.IsEntireRow && this.IsBlankorHasStyle)
        {
          if (this.LastRow == this.m_worksheet.LastRow)
          {
            this.m_worksheet.LastRow = this.Row - 1;
            break;
          }
          if (this.LastRow > this.m_worksheet.LastRow || this.Row != this.m_worksheet.FirstRow)
            break;
          this.m_worksheet.FirstRow = this.LastRow + 1;
          break;
        }
        if (num1 != this.FirstRow && num2 != this.LastRow && this.m_worksheet.FirstRow >= this.FirstRow && this.m_worksheet.LastRow == this.LastRow)
        {
          this.m_worksheet.FirstRow = num1;
          this.m_worksheet.LastRow = num2;
        }
        if (num2 == this.LastRow && this.m_worksheet.FirstRow >= this.FirstRow)
          this.m_worksheet.FirstRow = num1;
        if (num1 != this.FirstRow || this.m_worksheet.LastRow != this.LastRow)
          break;
        this.m_worksheet.LastRow = num2;
        break;
      case OfficeClearOptions.ClearContent:
        List<IRange> cellsList2 = this.CellsList;
        int index2 = 0;
        for (int count = cellsList2.Count; index2 < count; ++index2)
          ((RangeImpl) cellsList2[index2]).Value = (string) null;
        break;
      case OfficeClearOptions.ClearComment:
        List<IRange> cellsList3 = this.CellsList;
        int index3 = 0;
        for (int count = cellsList3.Count; index3 < count; ++index3)
          ((RangeImpl) cellsList3[index3]).Comments();
        break;
      case OfficeClearOptions.ClearConditionalFormats:
        List<IRange> cellsList4 = this.CellsList;
        int index4 = 0;
        for (int count = cellsList4.Count; index4 < count; ++index4)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList4[index4];
        }
        break;
      case OfficeClearOptions.ClearDataValidations:
        List<IRange> cellsList5 = this.CellsList;
        int index5 = 0;
        for (int count = cellsList5.Count; index5 < count; ++index5)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList5[index5];
        }
        break;
      default:
        List<IRange> cellsList6 = this.CellsList;
        int index6 = 0;
        for (int count = cellsList6.Count; index6 < count; ++index6)
        {
          RangeImpl rangeImpl = (RangeImpl) cellsList6[index6];
          rangeImpl.Value = (string) null;
          rangeImpl.Comments();
        }
        this.CellStyleName = "Normal";
        if (this.Row == this.m_worksheet.FirstRow && this.LastRow == this.m_worksheet.LastRow)
        {
          this.m_worksheet.Clear();
          break;
        }
        if (this.LastRow == this.m_worksheet.LastRow)
        {
          this.m_worksheet.LastRow = this.Row - 1;
          break;
        }
        if (this.LastRow > this.m_worksheet.LastRow || this.Row != this.m_worksheet.FirstRow)
          break;
        this.m_worksheet.FirstRow = this.LastRow + 1;
        break;
    }
  }

  public void Clear(OfficeClearOptions option) => this.ClearOption(option);

  internal void Comments()
  {
  }

  public void MoveTo(IRange destination)
  {
    this.CheckDisposed();
    this.MoveTo(destination, OfficeCopyRangeOptions.All);
  }

  public void MoveTo(IRange destination, OfficeCopyRangeOptions options)
  {
    this.CheckDisposed();
    if (this == destination)
      return;
    this.m_worksheet.MoveRange(destination, (IRange) this, options, false);
  }

  public IRange CopyTo(IRange destination)
  {
    this.CheckDisposed();
    return this == destination ? destination : this.m_worksheet.CopyRange(destination, (IRange) this, OfficeCopyRangeOptions.All);
  }

  public IRange CopyTo(IRange destination, OfficeCopyRangeOptions options)
  {
    this.CheckDisposed();
    return this == destination ? destination : this.m_worksheet.CopyRange(destination, (IRange) this, options);
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

  internal string GetDisplayText(int row, int column, FormatImpl formatImpl)
  {
    WorksheetImpl.TRangeValueType cellType = this.m_worksheet.GetCellType(row, column, false);
    string displayText = (string) null;
    switch (cellType)
    {
      case WorksheetImpl.TRangeValueType.Blank:
        return string.Empty;
      case WorksheetImpl.TRangeValueType.Error:
        return this.m_worksheet.GetError(row, column);
      case WorksheetImpl.TRangeValueType.Boolean:
        string upper1 = this.m_worksheet.GetBoolean(row, column).ToString().ToUpper();
        return formatImpl.ApplyFormat(upper1) ?? "";
      case WorksheetImpl.TRangeValueType.Number:
        double number = this.m_worksheet.GetNumber(row, column);
        return this.GetNumberOrDateTime(formatImpl, number);
      case WorksheetImpl.TRangeValueType.Formula:
        switch (this.m_worksheet.GetCellType(row, column, true))
        {
          case WorksheetImpl.TRangeValueType.Formula:
            return formatImpl.ApplyFormat(this.GetDisplayString());
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
            return this.m_worksheet.GetFormulaErrorValue(row, column);
          case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            string upper2 = this.m_worksheet.GetFormulaBoolValue(row, column).ToString().ToUpper();
            return formatImpl.ApplyFormat(upper2) ?? "";
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            double formulaNumberValue = this.m_worksheet.GetFormulaNumberValue(row, column);
            return this.GetNumberOrDateTime(formatImpl, formulaNumberValue);
          case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
            string formulaStringValue = this.m_worksheet.GetFormulaStringValue(row, column);
            return formatImpl.ApplyFormat(formulaStringValue);
        }
        break;
      case WorksheetImpl.TRangeValueType.String:
        string text = this.m_worksheet.GetText(row, column);
        return formatImpl.ApplyFormat(text);
      default:
        if (this.HasFormulaStringValue)
          return this.FormulaStringValue;
        break;
    }
    return displayText;
  }

  private string GetNumberOrDateTime(FormatImpl formatImpl, double dValue)
  {
    string result = string.Empty;
    OfficeFormatType formatType = formatImpl.GetFormatType(0.0);
    string str;
    if (dValue == 0.0 && !this.m_worksheet.WindowTwo.IsDisplayZeros)
    {
      if (formatType == OfficeFormatType.Number || formatType == OfficeFormatType.General)
        return str = this.GetDisplayString();
      if (formatImpl.ApplyFormat(this.GetDisplayString()).Length == 0)
        return str = string.Empty;
    }
    switch (formatType)
    {
      case OfficeFormatType.General:
      case OfficeFormatType.Text:
      case OfficeFormatType.Number:
        return !double.IsNaN(dValue) && result == string.Empty ? (str = double.IsNaN(dValue) ? dValue.ToString() : formatImpl.ApplyFormat(dValue)) : result;
      case OfficeFormatType.DateTime:
        if (result == string.Empty)
        {
          if (this.m_book.Date1904)
            dValue += 1462.0;
          else if (dValue < 60.0 && this.m_worksheet.WindowTwo.IsDisplayZeros)
            ++dValue;
          result = dValue <= CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime.ToOADate() ? formatImpl.ApplyFormat(dValue) : "######";
        }
        return this.m_hasDefaultFormat ? this.GetCultureFormat(result, dValue, formatImpl) : result;
      default:
        return formatImpl.ApplyFormat(dValue);
    }
  }

  public void Replace(string oldValue, string newValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.Text = newValue;
  }

  public void Replace(string oldValue, double newValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.Number = newValue;
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.DateTime = newValue;
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.m_worksheet.ImportArray(newValues, this.Row, this.Column, isVertical);
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.m_worksheet.ImportArray(newValues, this.Row, this.Column, isVertical);
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.m_worksheet.ImportArray(newValues, this.Row, this.Column, isVertical);
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.m_worksheet.ImportDataTable(newValues, isFieldNamesShown, this.Row, this.Column);
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell || !(this.Text == oldValue))
      return;
    this.m_worksheet.ImportDataColumn(newValues, isFieldNamesShown, this.Row, this.Column);
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    if (findValue == null || findValue.Length == 0)
      return (IRange) null;
    bool flag1 = (flags & OfficeFindType.Formula) == OfficeFindType.Formula;
    bool flag2 = (flags & OfficeFindType.Text) == OfficeFindType.Text;
    bool flag3 = (flags & OfficeFindType.FormulaStringValue) == OfficeFindType.FormulaStringValue;
    bool flag4 = (flags & OfficeFindType.Error) == OfficeFindType.Error;
    if (!flag1 && !flag2 && !flag3 && !flag4)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (this.IsSingleCell)
    {
      if (flag4 && this.IsError && this.Error == findValue)
        return (IRange) this;
      if (flag1 && this.HasFormula && this.Formula == findValue)
        return (IRange) this;
      if (flag3 && this.FormulaStringValue != null && this.FormulaStringValue == findValue)
        return (IRange) this;
      return flag2 && this.HasString && this.Text == findValue ? (IRange) this : (IRange) null;
    }
    return this.m_worksheet.Find((IRange) this, findValue, flags, true)?[0];
  }

  public IRange FindFirst(double findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (this.IsSingleCell)
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
      return this.FindFirst(findValue.ToOADate(), OfficeFindType.Number | OfficeFindType.FormulaValue);
    return this.HasDateTime && this.DateTime == findValue ? (IRange) this : (IRange) null;
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    this.CheckDisposed();
    if (!this.IsSingleCell)
      return this.FindFirst(findValue.TotalDays, OfficeFindType.Number | OfficeFindType.FormulaValue);
    return this.HasDateTime && this.TimeSpan == findValue ? (IRange) this : (IRange) null;
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    if (findValue == null || findValue.Length == 0)
      return (IRange[]) null;
    bool flag1 = (flags & OfficeFindType.Formula) == OfficeFindType.Formula;
    bool flag2 = (flags & OfficeFindType.Text) == OfficeFindType.Text;
    bool flag3 = (flags & OfficeFindType.FormulaStringValue) == OfficeFindType.FormulaStringValue;
    bool flag4 = (flags & OfficeFindType.Error) == OfficeFindType.Error;
    if (!flag1 && !flag2 && !flag3 && !flag4)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (!this.IsSingleCell)
      return this.m_worksheet.Find((IRange) this, findValue, flags, false);
    if ((!flag4 || !this.IsError || !(this.Error == findValue)) && (!flag1 || !this.HasFormula || !(this.Formula == findValue)) && (!flag3 || this.FormulaStringValue == null || !(this.FormulaStringValue == findValue)) && (!flag2 || !this.HasString || !(this.Text == findValue)))
      return (IRange[]) null;
    return new IRange[1]{ (IRange) this };
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    this.CheckDisposed();
    bool flag1 = (flags & OfficeFindType.FormulaValue) == OfficeFindType.FormulaValue;
    bool flag2 = (flags & OfficeFindType.Number) == OfficeFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    if (!this.IsSingleCell)
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
      return this.FindAll(findValue.ToOADate(), OfficeFindType.Number | OfficeFindType.FormulaValue);
    if (this.HasDateTime && this.DateTime == findValue)
      rangeList.Add((IRange) this);
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    this.CheckDisposed();
    List<IRange> rangeList = new List<IRange>();
    if (!this.IsSingleCell)
      return this.FindAll(findValue.TotalDays, OfficeFindType.Number | OfficeFindType.FormulaValue);
    if (this.HasDateTime && this.TimeSpan == findValue)
      rangeList.Add((IRange) this);
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public void CopyToClipboard()
  {
    this.AppImplementation.CreateClipboardProvider((IWorksheet) this.m_worksheet).SetClipboard((IRange) this);
  }

  public void BorderAround() => this.BorderAround(OfficeLineStyle.Thin);

  public void BorderAround(OfficeLineStyle borderLine)
  {
    this.BorderAround(borderLine, OfficeKnownColors.Black);
  }

  public void BorderAround(OfficeLineStyle borderLine, Color borderColor)
  {
    OfficeKnownColors nearestColor = this.m_book.GetNearestColor(borderColor);
    this.BorderAround(borderLine, nearestColor);
  }

  public void BorderAround(OfficeLineStyle borderLine, OfficeKnownColors borderColor)
  {
    if (this.IsSingleCell)
    {
      this.SetBorderToSingleCell(OfficeBordersIndex.EdgeLeft, borderLine, borderColor);
      this.SetBorderToSingleCell(OfficeBordersIndex.EdgeRight, borderLine, borderColor);
      this.SetBorderToSingleCell(OfficeBordersIndex.EdgeTop, borderLine, borderColor);
      this.SetBorderToSingleCell(OfficeBordersIndex.EdgeBottom, borderLine, borderColor);
    }
    else
    {
      int column = this.Column;
      int lastColumn = this.LastColumn;
      int row = this.Row;
      int lastRow = this.LastRow;
      MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.Application, (IWorksheet) this.m_worksheet);
      for (int iColumn = column; iColumn <= lastColumn; ++iColumn)
      {
        migrantRangeImpl.ResetRowColumn(row, iColumn);
        migrantRangeImpl.SetBorderToSingleCell(OfficeBordersIndex.EdgeTop, borderLine, borderColor);
        migrantRangeImpl.ResetRowColumn(lastRow, iColumn);
        migrantRangeImpl.SetBorderToSingleCell(OfficeBordersIndex.EdgeBottom, borderLine, borderColor);
      }
      for (int iRow = row; iRow <= lastRow; ++iRow)
      {
        migrantRangeImpl.ResetRowColumn(iRow, column);
        migrantRangeImpl.SetBorderToSingleCell(OfficeBordersIndex.EdgeLeft, borderLine, borderColor);
        migrantRangeImpl.ResetRowColumn(iRow, lastColumn);
        migrantRangeImpl.SetBorderToSingleCell(OfficeBordersIndex.EdgeRight, borderLine, borderColor);
      }
    }
  }

  public void BorderInside() => this.BorderInside(OfficeLineStyle.Thin);

  public void BorderInside(OfficeLineStyle borderLine)
  {
    this.BorderInside(borderLine, OfficeKnownColors.Black);
  }

  public void BorderInside(OfficeLineStyle borderLine, Color borderColor)
  {
    OfficeKnownColors nearestColor = this.m_book.GetNearestColor(borderColor);
    this.BorderInside(borderLine, nearestColor);
  }

  public void BorderInside(OfficeLineStyle borderLine, OfficeKnownColors borderColor)
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
        if (iColumn != this.LastColumn)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          migrantRangeImpl.SetBorderToSingleCell(OfficeBordersIndex.EdgeRight, borderLine, borderColor);
        }
        if (iRow != this.LastRow)
        {
          migrantRangeImpl.ResetRowColumn(iRow, iColumn);
          migrantRangeImpl.SetBorderToSingleCell(OfficeBordersIndex.EdgeBottom, borderLine, borderColor);
        }
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
        if (firstColumn == this.FirstColumn && firstColumn > 0)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn - 1);
          migrantRangeImpl.Borders[OfficeBordersIndex.EdgeRight].LineStyle = OfficeLineStyle.None;
        }
        if (firstColumn == this.LastColumn && firstColumn < this.Workbook.MaxColumnCount)
        {
          migrantRangeImpl.ResetRowColumn(firstRow, firstColumn + 1);
          migrantRangeImpl.Borders[OfficeBordersIndex.EdgeLeft].LineStyle = OfficeLineStyle.None;
        }
        if (firstRow == this.FirstRow && firstRow > 0)
        {
          migrantRangeImpl.ResetRowColumn(firstRow - 1, firstColumn);
          migrantRangeImpl.Borders[OfficeBordersIndex.EdgeBottom].LineStyle = OfficeLineStyle.None;
        }
        if (firstRow == this.LastRow && firstRow < this.Workbook.MaxRowCount)
        {
          migrantRangeImpl.ResetRowColumn(firstRow + 1, firstColumn);
          migrantRangeImpl.Borders[OfficeBordersIndex.EdgeTop].LineStyle = OfficeLineStyle.None;
        }
        migrantRangeImpl.ResetRowColumn(firstRow, firstColumn);
        migrantRangeImpl.Borders.LineStyle = OfficeLineStyle.None;
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
          case double num1:
            this.Number = num1;
            break;
          case int num2:
            this.SetNumberAndFormat((double) num2, isPreserveFormat);
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

  public void CollapseGroup(OfficeGroupBy groupBy)
  {
    this.CollapseExpand(groupBy, true, ExpandCollapseFlags.Default);
  }

  public void ExpandGroup(OfficeGroupBy groupBy)
  {
    this.ExpandGroup(groupBy, ExpandCollapseFlags.Default);
  }

  public void ExpandGroup(OfficeGroupBy groupBy, ExpandCollapseFlags flags)
  {
    this.CollapseExpand(groupBy, false, flags);
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
  }

  public void ClearDataValidations()
  {
  }

  public Rectangle[] GetRectangles()
  {
    return new Rectangle[1]
    {
      Rectangle.FromLTRB(this.FirstColumn - 1, this.FirstRow - 1, this.LastColumn - 1, this.LastRow - 1)
    };
  }

  public int GetRectanglesCount() => 1;

  public string WorksheetName => this.Worksheet.Name;

  internal bool IsSingleCellContainsString
  {
    get
    {
      int row = this.Row;
      for (int lastRow = this.LastRow; row <= lastRow; ++row)
      {
        int column = this.Column;
        for (int lastColumn = this.LastColumn; column <= lastColumn; ++column)
        {
          if (this.m_worksheet.GetCellType(row, column, false) == WorksheetImpl.TRangeValueType.String)
            return true;
        }
      }
      return false;
    }
  }

  public static string GetR1C1AddresFromCellIndex(long cellIndex)
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
    string numberFormat = ((IRange) rangeColection[0]).NumberFormat;
    for (int index = 1; index < count; ++index)
    {
      IRange range = (IRange) rangeColection[index];
      if (numberFormat != range.NumberFormat)
        return (string) null;
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
    string[] strArray = range.Split(':');
    int length = strArray.Length;
    Match match1 = FormulaUtil.FullRowRangeRegex.Match(range);
    if (match1.Success && match1.Index == 0 && match1.Length == range.Length)
    {
      iFirstColumn = 1;
      iLastColumn = book.MaxColumnCount;
      string str1 = UtilityMethods.RemoveFirstCharUnsafe(match1.Groups["Row1"].Value);
      string str2 = UtilityMethods.RemoveFirstCharUnsafe(match1.Groups["Row2"].Value);
      iFirstRow = Convert.ToInt32(str1);
      iLastRow = Convert.ToInt32(str2);
      return length;
    }
    Match match2 = FormulaUtil.FullColumnRangeRegex.Match(range);
    if (match2.Success && match2.Index == 0 && match2.Length == range.Length)
    {
      string columnName1 = UtilityMethods.RemoveFirstCharUnsafe(match2.Groups["Column1"].Value);
      string columnName2 = UtilityMethods.RemoveFirstCharUnsafe(match2.Groups["Column2"].Value);
      iFirstColumn = RangeImpl.GetColumnIndex(columnName1);
      iLastColumn = RangeImpl.GetColumnIndex(columnName2);
      iFirstRow = 1;
      iLastRow = book.MaxRowCount;
      return length;
    }
    long index1 = -1;
    if (length >= 1)
    {
      index1 = RangeImpl.CellNameToIndex(strArray[0]);
      iLastRow = iFirstRow = RangeImpl.GetRowFromCellIndex(index1);
      iLastColumn = iFirstColumn = RangeImpl.GetColumnFromCellIndex(index1);
    }
    if (length == 2)
    {
      long index2 = RangeImpl.CellNameToIndex(strArray[1]);
      if (index1 != index2)
      {
        iLastRow = RangeImpl.GetRowFromCellIndex(index2);
        iLastColumn = RangeImpl.GetColumnFromCellIndex(index2);
      }
    }
    else if (length > 2)
      throw new ArgumentException();
    return length;
  }

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

  protected internal void wrapStyle_OnNumberFormatChanged(object sender, EventArgs e)
  {
    RangeImpl.TCellType cellType = this.CellType;
    string str = this.Value;
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
    BiffRecordRaw record = this.Record;
    if (record != null)
      num = (int) ((ICellPositionFormat) record).ExtendedFormatIndex;
    this.CreateStyleWrapper(num);
  }

  protected void CreateStyleWrapper(int value)
  {
    if (!this.IsSingleCell && !this.IsEntireRow && !this.IsEntireColumn)
      throw new ArgumentException("This method can be used only for single cell not a range");
    Syncfusion.OfficeChart.Implementation.CellStyle style = this.m_style;
    this.m_style = new Syncfusion.OfficeChart.Implementation.CellStyle(this, value);
  }

  internal static IStyle CreateTempStyleWrapperWithoutRange(RangeImpl rangeImpl, int value)
  {
    return (IStyle) new Syncfusion.OfficeChart.Implementation.CellStyle(rangeImpl, value);
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
        ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
        record.FirstColumn = (ushort) (firstColumn - 1);
        record.LastColumn = (ushort) (this.LastColumn - 1);
        record.ExtendedFormatIndex = (ushort) index;
        this.m_worksheet.ColumnInformation[index1] = record;
      }
    }
    else
      this.m_worksheet.CellRecords.SetCellStyle(this.Row, this.Column, index);
    if (this.m_style != null)
      this.m_style.SetFormatIndex(index);
    if (Array.IndexOf<OfficeLineStyle>(RangeImpl.ThinBorders, extendedFormatImpl.BottomBorderLineStyle) >= 0)
      return;
    RowStorage row1 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, this.Row - 1, false);
    if (row1 == null)
      return;
    row1.IsSpaceBelowRow = true;
  }

  private StyleImpl ChangeStyleName(string strNewName)
  {
    int Index = this.m_book.DefaultXFIndex;
    StyleImpl styleImpl;
    if (strNewName != null && strNewName.Length > 0)
    {
      if (!this.m_book.InnerStyles.ContainsName(strNewName) && this.m_book.Version != OfficeVersion.Excel97to2003)
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
          Index = styleImpl.Index;
      }
    }
    else
      styleImpl = (StyleImpl) this.m_book.Styles[Index];
    this.ExtendedFormatIndex = (ushort) Index;
    return styleImpl;
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

  private ExtendedFormatImpl ExtendedFormat
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
