// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.NameImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class NameImpl : 
  CommonObject,
  IName,
  INameIndexChangedEventProvider,
  IParseable,
  INativePTG,
  ICloneParent,
  ICombinedRange,
  IRange,
  IParentApplication,
  IEnumerable<IRange>,
  IEnumerable,
  IDisposable
{
  private const string DEF_SHEETNAME_SEPARATER = "!";
  private const string DEF_RANGE_FORMAT = "{2}!{0}:{1}";
  public const int DEF_NAME_SHEET_INDEX = 65534;
  private const string WorkbookScope = "Workbook";
  private static readonly char[] DEF_VALID_SYMBOL = new char[6]
  {
    '_',
    '?',
    '\\',
    '№',
    '.',
    '#'
  };
  private NameRecord m_name;
  private WorkbookImpl m_book;
  private WorksheetImpl m_worksheet;
  private int m_index = -1;
  private bool m_bIsDeleted;
  private bool m_bIsNumReference;
  private bool m_bIsMultiReference;
  private bool m_bIsStringReference;
  private bool m_isQueryRange;
  private int m_sheetindex;
  private bool m_isCommon;
  private string m_description;
  internal bool m_isTableNamedRange;
  internal bool m_isTableNamedRangeDeleted;
  internal bool m_isFormulaNamedRange;

  public NameImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_name = (NameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Name);
    this.SetParents();
  }

  [CLSCompliant(false)]
  public NameImpl(IApplication application, object parent, NameRecord name, int index)
    : base(application, parent)
  {
    this.m_index = index;
    this.SetParents();
    this.Parse(name);
  }

  [CLSCompliant(false)]
  public NameImpl(IApplication application, object parent, NameRecord name)
    : this(application, parent, name, -1)
  {
  }

  public NameImpl(IApplication application, object parent, string name, IRange range, int index)
    : this(application, parent, name, range, index, false)
  {
  }

  public NameImpl(IApplication application, object parent, string name, int index)
    : this(application, parent, name, index, false)
  {
  }

  public NameImpl(IApplication application, object parent, string name, int index, bool bIsLocal)
    : this(application, parent)
  {
    this.m_index = index;
    this.Name = name;
    this.SetIndexOrGlobal(bIsLocal);
  }

  public NameImpl(
    IApplication application,
    object parent,
    string name,
    IRange range,
    int index,
    bool bIsLocal)
    : this(application, parent)
  {
    this.m_index = index;
    this.m_name = (NameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Name);
    this.SetIndexOrGlobal(bIsLocal);
    this.SetParents();
    this.Name = name;
    this.RefersToRange = range;
  }

  private void SetIndexOrGlobal(bool bIsLocal)
  {
    this.m_name.IndexOrGlobal = bIsLocal ? (ushort) (this.m_worksheet.RealIndex + 1) : (ushort) 0;
  }

  internal NameRecord NameRecord => this.m_name;

  internal bool IsDeleted
  {
    get => this.m_bIsDeleted;
    set => this.m_bIsDeleted = value;
  }

  public int Index => this.m_index;

  int IName.Index
  {
    get
    {
      WorksheetNamesCollection worksheetNamesCollection = (WorksheetNamesCollection) null;
      WorkbookNamesCollection workbookNamesCollection = (WorkbookNamesCollection) null;
      if (this.m_worksheet != null)
        worksheetNamesCollection = this.m_worksheet.InnerNames;
      else
        workbookNamesCollection = this.m_book.InnerNamesColection;
      int index1 = 0;
      if (worksheetNamesCollection != null)
      {
        for (int index2 = 0; index2 < worksheetNamesCollection.Count; ++index2)
        {
          NameImpl nameImpl = worksheetNamesCollection[index2] as NameImpl;
          if (!nameImpl.IsDeleted && !nameImpl.m_isTableNamedRange)
          {
            if (nameImpl.Name == this.Name)
              return index1;
            ++index1;
          }
        }
      }
      else
      {
        for (int index3 = 0; index3 < workbookNamesCollection.Count; ++index3)
        {
          NameImpl nameImpl = workbookNamesCollection[index3] as NameImpl;
          if (!nameImpl.IsDeleted && !nameImpl.m_isTableNamedRange)
          {
            if (nameImpl.Name == this.Name)
              return index1;
            ++index1;
          }
        }
      }
      return -1;
    }
  }

  public string Name
  {
    get => this.m_name.Name;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (!(value != this.m_name.Name))
        return;
      string name = this.m_name.Name;
      if (!this.m_book.Saving && !this.m_book.Loading && name != string.Empty)
        this.ReplaceName(this.m_book.InnerNamesColection.m_hashNameToIName, name, value);
      this.m_name.IsBuinldInName = NameRecord.IsPredefinedName(value);
      this.m_name.Name = value;
      this.m_book.InnerNamesColection.IsWorkbookNamesChanged = true;
      if (this.m_worksheet == null)
        return;
      this.Worksheet.InnerNames.Rename((IName) this, name);
    }
  }

  public string NameLocal
  {
    get => this.m_name.Name;
    set => this.m_name.Name = value;
  }

  public IRange RefersToRange
  {
    get
    {
      if ((this.m_name.FormulaTokens == null || this.m_name.FormulaTokens.Length != 1 ? (NameXPtg) null : this.m_name.FormulaTokens[0] as NameXPtg) != null)
        return (IRange) null;
      IRange refersToRange = (IRange) null;
      string str = this.Value;
      double result;
      double.TryParse(str, out result);
      bool flag1 = result == (double) (int) result;
      if (str != null && str.Length != 0 && flag1 && (!this.Value.StartsWith("\"") || !this.Value.EndsWith("\"")) && !this.Value.StartsWith("'\\") && !this.Value.StartsWith("'/"))
      {
        if (this.m_worksheet != null)
        {
          refersToRange = this.m_worksheet.GetRangeByString(str, true);
        }
        else
        {
          string rangeName = str;
          string worksheetName = RangeImpl.GetWorksheetName(ref rangeName);
          WorksheetImpl worksheet = worksheetName != null ? (WorksheetImpl) this.m_book.Worksheets[worksheetName] : (WorksheetImpl) null;
          if ($"'{worksheetName}'!{rangeName}" != str)
            rangeName = str;
          if (worksheet != null)
            refersToRange = worksheet.GetRangeByString(rangeName, true);
          if (refersToRange == null)
            refersToRange = this.Workbook.TryParseTableOrNamedRange(rangeName, this.m_worksheet);
        }
        if (refersToRange == null && this.m_name.FormulaTokens.Length > 2 && this.m_name.FormulaTokens[0] is IRangeGetter)
        {
          bool flag2 = true;
          for (int index = 0; index < this.m_name.FormulaTokens.Length; ++index)
          {
            if ((this.m_name.FormulaTokens[index].TokenCode == FormulaToken.tCellRangeList ? 0 : (!(this.m_name.FormulaTokens[index] is IRangeGetter) ? 1 : 0)) != 0)
            {
              flag2 = false;
              break;
            }
          }
          if (flag2)
            refersToRange = ChartParser.GetRange(this.m_book, str);
        }
      }
      return refersToRange;
    }
    set
    {
      this.Value = value != null ? value.AddressGlobal : throw new ArgumentNullException(nameof (value));
      this.m_bIsDeleted = false;
    }
  }

  public string Value
  {
    get
    {
      string str = (string) null;
      try
      {
        str = this.m_book.FormulaUtil.ParsePtgArray(this.m_name.FormulaTokens);
      }
      catch (ParseException ex)
      {
      }
      return str;
    }
    set
    {
      this.SetValue(value, false);
      if (this.m_book.Loading)
        return;
      foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.m_book.Worksheets)
      {
        if (worksheet.CalcEngine != null && this.Value != null)
        {
          Hashtable namedRanges = worksheet.CalcEngine.NamedRanges;
          if (namedRanges != null)
          {
            string upper = this.Value.Replace("'", "").ToUpper(CultureInfo.InvariantCulture);
            if (this.IsLocal)
            {
              if (namedRanges.ContainsKey((object) $"{this.Scope}!{this.Name}".ToUpper()))
                namedRanges.Remove((object) $"{this.Scope}!{this.Name}".ToUpper());
              namedRanges.Add((object) $"{this.Scope}!{this.Name}".ToUpper(), (object) upper);
            }
            else
            {
              if (namedRanges.ContainsKey((object) this.Name.ToUpper()))
                namedRanges.Remove((object) this.Name.ToUpper());
              namedRanges.Add((object) this.Name.ToUpper(), (object) upper);
            }
          }
        }
      }
    }
  }

  public string Description
  {
    get => this.m_description;
    set => this.m_description = value;
  }

  public string ValueR1C1
  {
    get
    {
      try
      {
        return this.m_book.FormulaUtil.ParsePtgArray(this.m_name.FormulaTokens, 0, 0, true, false);
      }
      catch
      {
        return (string) null;
      }
    }
    set => this.SetValue(value, true);
  }

  public string RefersTo
  {
    get => '='.ToString() + this.Value;
    set => this.SetValue(value, false);
  }

  public string RefersToR1C1
  {
    get => '='.ToString() + this.ValueR1C1;
    set => this.SetValue(value, true);
  }

  public bool Visible
  {
    get => !this.m_name.IsNameHidden;
    set => this.m_name.IsNameHidden = !value;
  }

  public bool IsLocal => this.m_name.IndexOrGlobal != (ushort) 0;

  IWorksheet IName.Worksheet => (IWorksheet) this.m_worksheet;

  public bool IsQueryTableRange
  {
    get => this.m_isQueryRange;
    set => this.m_isQueryRange = value;
  }

  public int SheetIndex
  {
    get => this.m_sheetindex;
    set => this.m_sheetindex = value;
  }

  public string Scope => !this.IsLocal ? "Workbook" : this.m_worksheet.Name;

  public string HtmlString
  {
    get => this.RefersToRange.HtmlString;
    set => this.RefersToRange.HtmlString = value;
  }

  public string Address
  {
    get => this.m_worksheet == null ? this.Name : $"'{this.m_worksheet.Name}'!{this.Name}";
  }

  public string AddressLocal => this.RefersToRange?.AddressLocal;

  public string AddressGlobal
  {
    get => this.m_worksheet == null ? this.Name : $"'{this.m_worksheet.Name}'!{this.Name}";
  }

  public string AddressGlobalWithoutSheetName
  {
    get => ((RangeImpl) this.RefersToRange).AddressGlobalWithoutSheetName;
  }

  public string AddressR1C1 => this.RefersToRange.AddressR1C1;

  public string AddressR1C1Local => this.RefersToRange.AddressR1C1Local;

  public bool Boolean
  {
    get => this.RefersToRange.Boolean;
    set => this.RefersToRange.Boolean = value;
  }

  public IBorders Borders => this.RefersToRange.Borders;

  public IRange[] Cells => this.RefersToRange.Cells;

  public int Column
  {
    get
    {
      IRange refersToRange = this.RefersToRange;
      return refersToRange == null ? -1 : refersToRange.Column;
    }
  }

  public int ColumnGroupLevel => this.RefersToRange.ColumnGroupLevel;

  public double ColumnWidth
  {
    get => this.RefersToRange.ColumnWidth;
    set => this.RefersToRange.ColumnWidth = value;
  }

  public int Count
  {
    get
    {
      IRange refersToRange = this.RefersToRange;
      return refersToRange == null ? 1 : refersToRange.Count;
    }
  }

  public DateTime DateTime
  {
    get => this.RefersToRange.DateTime;
    set => this.RefersToRange.DateTime = value;
  }

  public string DisplayText => this.RefersToRange.DisplayText;

  public IRange End => this.RefersToRange.End;

  public IRange EntireColumn => this.RefersToRange.EntireColumn;

  public IRange EntireRow => this.RefersToRange.EntireRow;

  public string Error
  {
    get => this.RefersToRange.Error;
    set => this.RefersToRange.Error = value;
  }

  public string Formula
  {
    get => this.RefersToRange.Formula;
    set => this.RefersToRange.Formula = value;
  }

  public string FormulaArray
  {
    get => this.RefersToRange.FormulaArray;
    set => this.RefersToRange.FormulaArray = value;
  }

  public string FormulaArrayR1C1
  {
    get => this.RefersToRange.FormulaArrayR1C1;
    set => this.RefersToRange.FormulaArrayR1C1 = value;
  }

  public bool FormulaHidden
  {
    get => this.RefersToRange.FormulaHidden;
    set => this.RefersToRange.FormulaHidden = value;
  }

  public DateTime FormulaDateTime
  {
    get => this.RefersToRange.FormulaDateTime;
    set => this.RefersToRange.FormulaDateTime = value;
  }

  public string FormulaR1C1
  {
    get => this.RefersToRange.FormulaR1C1;
    set => this.RefersToRange.FormulaR1C1 = value;
  }

  public bool HasDataValidation => this.RefersToRange.HasDataValidation;

  public bool HasBoolean => this.RefersToRange.HasBoolean;

  public bool HasDateTime => this.RefersToRange.HasDateTime;

  public bool HasFormulaBoolValue => this.RefersToRange.HasFormulaBoolValue;

  public bool HasFormulaErrorValue => this.RefersToRange.HasFormulaErrorValue;

  public bool HasFormulaDateTime => this.RefersToRange.HasFormulaDateTime;

  public bool HasFormulaNumberValue => this.RefersToRange.HasFormulaNumberValue;

  public bool HasFormulaStringValue => this.RefersToRange.HasFormulaStringValue;

  public bool HasFormula => this.RefersToRange.HasFormula;

  public bool HasFormulaArray => this.RefersToRange.HasFormulaArray;

  public bool HasNumber
  {
    get
    {
      return this.RefersToRange == null ? double.TryParse(this.Value, out double _) : this.RefersToRange.HasNumber;
    }
  }

  public bool HasRichText => this.RefersToRange.HasRichText;

  public bool HasString => this.RefersToRange != null && this.RefersToRange.HasString;

  public bool HasStyle => this.RefersToRange.HasStyle;

  public IHyperLinks Hyperlinks => this.RefersToRange.Hyperlinks;

  public ExcelHAlign HorizontalAlignment
  {
    get => this.RefersToRange.HorizontalAlignment;
    set => this.RefersToRange.HorizontalAlignment = value;
  }

  public int IndentLevel
  {
    get => this.RefersToRange.IndentLevel;
    set => this.RefersToRange.IndentLevel = value;
  }

  public bool IsBlank
  {
    get
    {
      return this.RefersToRange == null ? !string.IsNullOrEmpty(this.Value) : this.RefersToRange.IsBlank;
    }
  }

  public bool IsBoolean => this.RefersToRange.IsBoolean;

  public bool IsError => this.RefersToRange.IsError;

  public bool IsGroupedByColumn => this.RefersToRange.IsGroupedByColumn;

  public bool IsGroupedByRow => this.RefersToRange.IsGroupedByRow;

  public bool IsInitialized => this.RefersToRange.IsInitialized;

  public int LastColumn
  {
    get
    {
      IRange refersToRange = this.RefersToRange;
      return refersToRange == null ? -1 : refersToRange.LastColumn;
    }
  }

  public int LastRow
  {
    get
    {
      IRange refersToRange = this.RefersToRange;
      return refersToRange == null ? -1 : refersToRange.LastRow;
    }
  }

  public double Number
  {
    get => this.RefersToRange.Number;
    set
    {
      if (this.RefersToRange == null)
        return;
      this.RefersToRange.Number = value;
    }
  }

  public string NumberFormat
  {
    get => this.RefersToRange.NumberFormat;
    set => this.RefersToRange.NumberFormat = value;
  }

  public int Row
  {
    get
    {
      IRange refersToRange = this.RefersToRange;
      return refersToRange == null ? -1 : refersToRange.Row;
    }
  }

  public int RowGroupLevel => this.RefersToRange.RowGroupLevel;

  public double RowHeight
  {
    get => this.RefersToRange.RowHeight;
    set => this.RefersToRange.RowHeight = value;
  }

  public IRange[] Rows => this.RefersToRange.Rows;

  public IRange[] Columns => this.RefersToRange.Columns;

  public IStyle CellStyle
  {
    get => this.RefersToRange.CellStyle;
    set => this.RefersToRange.CellStyle = value;
  }

  public string CellStyleName
  {
    get => this.RefersToRange.CellStyleName;
    set => this.RefersToRange.CellStyleName = value;
  }

  public string Text
  {
    get => this.RefersToRange.Text;
    set => this.RefersToRange.Text = value;
  }

  public TimeSpan TimeSpan
  {
    get => this.RefersToRange.TimeSpan;
    set => this.RefersToRange.TimeSpan = value;
  }

  string IRange.Value
  {
    get => this.RefersToRange.Value;
    set => this.RefersToRange.Value = value;
  }

  public string CalculatedValue
  {
    get
    {
      return this.Parent is IWorksheet && ((IWorksheet) this.Parent).CalcEngine != null ? ((IWorksheet) this.Parent).CalcEngine.PullUpdatedValue(RangeInfo.GetAlphaLabel(this.Column) + this.Row.ToString()) : (string) null;
    }
  }

  public object Value2
  {
    get => this.RefersToRange.Value2;
    set => this.RefersToRange.Value2 = value;
  }

  public ExcelVAlign VerticalAlignment
  {
    get => this.RefersToRange.VerticalAlignment;
    set => this.RefersToRange.VerticalAlignment = value;
  }

  IWorksheet IRange.Worksheet => this.RefersToRange?.Worksheet;

  public IConditionalFormats ConditionalFormats => this.RefersToRange.ConditionalFormats;

  public IDataValidation DataValidation => this.RefersToRange.DataValidation;

  public string FormulaStringValue
  {
    get => this.RefersToRange.FormulaStringValue;
    set => this.RefersToRange.FormulaStringValue = value;
  }

  public double FormulaNumberValue
  {
    get => this.RefersToRange.FormulaNumberValue;
    set => this.RefersToRange.FormulaNumberValue = value;
  }

  public bool FormulaBoolValue
  {
    get => this.RefersToRange.FormulaBoolValue;
    set => this.RefersToRange.FormulaBoolValue = value;
  }

  public string FormulaErrorValue
  {
    get => this.RefersToRange.FormulaErrorValue;
    set => this.RefersToRange.FormulaErrorValue = value;
  }

  public ICommentShape Comment => this.RefersToRange.Comment;

  public IRichTextString RichText => this.RefersToRange.RichText;

  public bool IsMerged => this.RefersToRange.IsMerged;

  public IRange MergeArea => this.RefersToRange.MergeArea;

  public bool WrapText
  {
    get => this.RefersToRange.WrapText;
    set => this.RefersToRange.WrapText = value;
  }

  public IRange this[int row, int column]
  {
    get => this.RefersToRange.Worksheet[row, column];
    set => this.RefersToRange.Worksheet.Range[row, column] = value;
  }

  public IRange this[int row, int column, int lastRow, int lastColumn]
  {
    get => this.RefersToRange.Worksheet[row, column, lastRow, lastColumn];
  }

  public IRange this[string name] => this[name, false];

  public IRange this[string name, bool IsR1C1Notation] => this.RefersToRange[name, IsR1C1Notation];

  public bool HasExternalFormula => this.RefersToRange.HasExternalFormula;

  public ExcelIgnoreError IgnoreErrorOptions
  {
    get => this.RefersToRange.IgnoreErrorOptions;
    set => this.RefersToRange.IgnoreErrorOptions = value;
  }

  public bool? IsStringsPreserved
  {
    get
    {
      return !(this.RefersToRange is ICombinedRange refersToRange) ? new bool?() : this.m_worksheet.GetStringPreservedValue(refersToRange);
    }
    set
    {
      if (!(this.RefersToRange is ICombinedRange refersToRange))
        return;
      this.m_worksheet.SetStringPreservedValue(refersToRange, value);
    }
  }

  public BuiltInStyles? BuiltInStyle
  {
    get => this.RefersToRange.BuiltInStyle;
    set => this.RefersToRange.BuiltInStyle = value;
  }

  public void CopyToClipboard() => throw new NotImplementedException();

  public IRange Offset(int row, int column) => throw new NotImplementedException();

  public IRange Resize(int row, int column) => throw new NotImplementedException();

  public IRange[] FindAll(TimeSpan findValue) => this.RefersToRange.FindAll(findValue);

  public IRange[] FindAll(DateTime findValue) => this.RefersToRange.FindAll(findValue);

  public IRange[] FindAll(bool findValue) => this.RefersToRange.FindAll(findValue);

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    return this.RefersToRange.FindAll(findValue, flags);
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    return this.RefersToRange.FindAll(findValue, flags);
  }

  public IRange FindFirst(TimeSpan findValue) => this.RefersToRange.FindFirst(findValue);

  public IRange FindFirst(DateTime findValue) => this.RefersToRange.FindFirst(findValue);

  public IRange FindFirst(bool findValue) => this.RefersToRange.FindFirst(findValue);

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    return this.RefersToRange.FindFirst(findValue, flags);
  }

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    return this.RefersToRange.FindFirst(findValue, flags);
  }

  public void Replace(string oldValue, string newValue)
  {
    this.RefersToRange.Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, string newValue, ExcelFindOptions findOptions)
  {
    this.RefersToRange.Replace(oldValue, newValue, findOptions);
  }

  public void Replace(string oldValue, double newValue)
  {
    this.RefersToRange.Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    this.RefersToRange.Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    this.RefersToRange.Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    this.RefersToRange.Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    this.RefersToRange.Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    this.RefersToRange.Replace(oldValue, newValues, isFieldNamesShown);
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    this.RefersToRange.Replace(oldValue, newValues, isFieldNamesShown);
  }

  public ICommentShape AddComment() => this.RefersToRange.AddComment();

  public void AutofitColumns() => this.RefersToRange.AutofitColumns();

  public void AutofitRows() => this.RefersToRange.AutofitRows();

  public IRange MergeWith(IRange range) => this.RefersToRange.MergeWith(range);

  public IRange IntersectWith(IRange range) => this.RefersToRange.IntersectWith(range);

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options)
  {
    return this.RefersToRange.CopyTo(destination, options);
  }

  public IRange CopyTo(IRange destination, bool pasteLink)
  {
    return this.RefersToRange.CopyTo(destination, pasteLink);
  }

  public IRange CopyTo(IRange destination) => this.RefersToRange.CopyTo(destination);

  public IRange CopyTo(IRange destination, ExcelCopyRangeOptions options, bool skipBlank)
  {
    return this.RefersToRange.CopyTo(destination, options, skipBlank);
  }

  public void MoveTo(IRange destination) => this.RefersToRange.MoveTo(destination);

  public void Clear(ExcelMoveDirection direction, ExcelCopyRangeOptions options)
  {
    this.RefersToRange.Clear(direction, options);
  }

  public void Clear(ExcelClearOptions option) => this.RefersToRange.Clear(option);

  public void Clear(ExcelMoveDirection direction) => this.RefersToRange.Clear(direction);

  public void Clear(bool isClearFormat) => this.RefersToRange.Clear(isClearFormat);

  public void Clear() => this.RefersToRange.Clear();

  public void FreezePanes() => this.RefersToRange.FreezePanes();

  public void UnMerge() => this.RefersToRange.UnMerge();

  public IRange Ungroup(ExcelGroupBy groupBy) => this.RefersToRange.Ungroup(groupBy);

  public double Sum() => this.RefersToRange.Sum();

  public double Sum(bool considerDateAsNumber) => this.RefersToRange.Sum(considerDateAsNumber);

  public double Average() => this.RefersToRange.Average();

  public double Average(bool considerDateAsNumber)
  {
    return this.RefersToRange.Average(considerDateAsNumber);
  }

  public double Min() => this.RefersToRange.Min();

  public double Min(bool considerDateAsNumber) => this.RefersToRange.Min(considerDateAsNumber);

  public double Max() => this.RefersToRange.Max();

  public double Max(bool considerDateAsNumber) => this.RefersToRange.Max(considerDateAsNumber);

  public IRange Trim() => this.RefersToRange.Trim();

  public void Merge() => this.RefersToRange.Merge();

  public void Merge(bool clearCells) => this.RefersToRange.Merge(clearCells);

  public IRange Group(ExcelGroupBy groupBy, bool bCollapsed)
  {
    return this.RefersToRange.Group(groupBy, bCollapsed);
  }

  public IRange Group(ExcelGroupBy groupBy) => this.RefersToRange.Group(groupBy);

  public void SubTotal(int groupBy, ConsolidationFunction function, int[] totalList)
  {
    this.RefersToRange.SubTotal(groupBy, function, totalList);
  }

  public void SubTotal(
    int groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    this.RefersToRange.SubTotal(groupBy, function, totalList, replace, pageBreaks, summaryBelowData);
  }

  public void SubTotal(
    int[] groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData)
  {
    this.RefersToRange.SubTotal(groupBy, function, totalList, replace, pageBreaks, summaryBelowData);
  }

  public IRange Activate() => this.RefersToRange.Activate();

  public IRange Activate(bool scroll) => this.RefersToRange.Activate(scroll);

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
    this.RefersToRange.BorderAround(borderLine, borderColor);
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
    this.RefersToRange.BorderInside(borderLine, borderColor);
  }

  public void BorderNone() => this.RefersToRange.BorderNone();

  public void CollapseGroup(ExcelGroupBy groupBy) => this.RefersToRange.CollapseGroup(groupBy);

  public void ExpandGroup(ExcelGroupBy groupBy) => this.RefersToRange.ExpandGroup(groupBy);

  public void ExpandGroup(ExcelGroupBy groupBy, ExpandCollapseFlags flags)
  {
    this.RefersToRange.ExpandGroup(groupBy, flags);
  }

  public IRange[] GetDependents() => throw new NotImplementedException();

  public IRange[] GetDependents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetPrecedents() => throw new NotImplementedException();

  public IRange[] GetPrecedents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetDirectDependents() => throw new NotImplementedException();

  public IRange[] GetDirectDependents(bool isEntireWorkbook) => throw new NotImplementedException();

  public IRange[] GetDirectPrecedents() => throw new NotImplementedException();

  public IRange[] GetDirectPrecedents(bool isEntireWorkbook) => throw new NotImplementedException();

  [CLSCompliant(false)]
  public NameRecord Record => this.m_name;

  public WorksheetImpl Worksheet => this.m_worksheet;

  public WorkbookImpl Workbook => this.m_book;

  public bool IsExternName
  {
    get
    {
      if (this.m_name == null || this.m_name.FormulaTokens == null)
        return false;
      int index = 0;
      for (int length = this.m_name.FormulaTokens.Length; index < length; ++index)
      {
        if (this.m_name.FormulaTokens[index] is IReference && this.m_book.IsExternalReference((int) (this.m_name.FormulaTokens[index] as IReference).RefIndex))
          return true;
      }
      return false;
    }
  }

  [CLSCompliant(false)]
  public MergeCellsRecord.MergedRegion Region
  {
    get
    {
      string addressLocal = this.AddressLocal;
      if (addressLocal == null)
        return (MergeCellsRecord.MergedRegion) null;
      string[] strArray = addressLocal.Split(':');
      long index1 = 0;
      long index2 = 0;
      if (strArray.Length > 2)
        return (MergeCellsRecord.MergedRegion) null;
      if (strArray.Length >= 1)
      {
        try
        {
          index1 = RangeImpl.CellNameToIndex(strArray[0]);
          index2 = index1;
        }
        catch (ArgumentException ex)
        {
          return (MergeCellsRecord.MergedRegion) null;
        }
      }
      if (strArray.Length == 2)
      {
        try
        {
          index2 = RangeImpl.CellNameToIndex(strArray[1]);
        }
        catch (ArgumentException ex)
        {
          return (MergeCellsRecord.MergedRegion) null;
        }
      }
      ushort rowFrom = (ushort) (RangeImpl.GetRowFromCellIndex(index1) - 1);
      ushort colFrom = (ushort) (RangeImpl.GetColumnFromCellIndex(index1) - 1);
      ushort rowTo = (ushort) (RangeImpl.GetRowFromCellIndex(index2) - 1);
      ushort colTo = (ushort) (RangeImpl.GetColumnFromCellIndex(index2) - 1);
      return new MergeCellsRecord.MergedRegion((int) rowFrom, (int) rowTo, (int) colFrom, (int) colTo);
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Region));
      if (this.Region == value)
        return;
      int firstRow1 = value.RowFrom + 1;
      int firstColumn1 = value.ColumnFrom + 1;
      int firstRow2 = value.RowTo + 1;
      int firstColumn2 = value.ColumnTo + 1;
      string cellName1 = RangeImpl.GetCellName(firstColumn1, firstRow1, false, true);
      string cellName2 = RangeImpl.GetCellName(firstColumn2, firstRow2, false, true);
      string str1 = this.Value;
      int length = str1.IndexOf("!");
      string str2 = length >= 1 ? str1.Substring(0, length) : throw new NotSupportedException("Cannot find sheet name separater.");
      if (cellName1 == cellName2)
        this.Value = $"{str2}!{cellName1}";
      else
        this.Value = string.Format("{2}!{0}:{1}", (object) cellName1, (object) cellName2, (object) str2);
    }
  }

  public bool IsBuiltIn
  {
    get => this.m_name.IsBuinldInName;
    set => this.m_name.IsBuinldInName = value;
  }

  public int NameIndexChangedHandlersCount
  {
    get => this.NameIndexChanged != null ? this.NameIndexChanged.GetInvocationList().Length : 0;
  }

  public bool IsFunction
  {
    get => this.m_name.IsNameFunction;
    set => this.m_name.IsNameFunction = value;
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

  internal bool IsCommon
  {
    get => this.m_isCommon;
    set => this.m_isCommon = value;
  }

  public event NameImpl.NameIndexChangedEventHandler NameIndexChanged;

  public void Delete()
  {
    if (!this.m_book.Loading)
    {
      foreach (IWorksheet worksheet in (IEnumerable<IWorksheet>) this.m_book.Worksheets)
      {
        if (worksheet.CalcEngine != null)
        {
          Hashtable namedRanges = worksheet.CalcEngine.NamedRanges;
          if (namedRanges != null)
          {
            if (this.IsLocal)
            {
              if (namedRanges.ContainsKey((object) $"{this.Scope}!{this.Name}".ToUpper()))
                namedRanges.Remove((object) $"{this.Scope}!{this.Name}".ToUpper());
            }
            else if (namedRanges.ContainsKey((object) this.Name.ToUpper()))
              namedRanges.Remove((object) this.Name.ToUpper());
          }
        }
      }
    }
    this.m_name.Delete();
    this.m_bIsDeleted = true;
  }

  private void SetParents()
  {
    this.m_worksheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    if (this.m_worksheet != null)
    {
      this.m_book = this.m_worksheet.Workbook as WorkbookImpl;
    }
    else
    {
      this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
      if (this.m_book == null)
        throw new ArgumentNullException("IName has no parent workbook");
    }
  }

  [CLSCompliant(false)]
  public void Parse(NameRecord name)
  {
    this.m_name = name != null ? (NameRecord) name.Clone() : throw new ArgumentNullException(nameof (name));
  }

  private void OnValueChanged(string oldValue, string newValue, bool useR1C1)
  {
    if (!(oldValue != newValue))
      return;
    Dictionary<Type, ReferenceIndexAttribute> indexes = new Dictionary<Type, ReferenceIndexAttribute>();
    indexes.Add(typeof (Area3DPtg), new ReferenceIndexAttribute(1));
    indexes.Add(typeof (Ref3DPtg), new ReferenceIndexAttribute(1));
    ExcelParseFormulaOptions options = ExcelParseFormulaOptions.RootLevel | ExcelParseFormulaOptions.InName;
    if (useR1C1)
      options |= ExcelParseFormulaOptions.UseR1C1;
    Decimal result;
    if (Decimal.TryParse(newValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      newValue = result.ToString();
    if (new Regex("[^a-z0-9A-Z]").IsMatch(newValue) && !newValue.Contains("$"))
      newValue = '"'.ToString() + newValue + (object) '"';
    this.m_name.FormulaTokens = this.m_book.FormulaUtil.ParseString(newValue, (IWorksheet) this.m_worksheet, indexes, 0, (Dictionary<string, string>) null, options, 0, 0);
    this.RaiseNameIndexChangedEvent(new NameIndexChangedEventArgs(this.Index, this.Index));
  }

  public void SetValue(Ptg[] parsedExpression)
  {
    this.m_name.FormulaTokens = parsedExpression;
    this.RaiseNameIndexChangedEvent(new NameIndexChangedEventArgs(this.Index, this.Index));
  }

  private void RaiseNameIndexChangedEvent(NameIndexChangedEventArgs e)
  {
    if (this.NameIndexChanged == null)
      return;
    this.NameIndexChanged.GetInvocationList();
    this.NameIndexChanged((object) this, e);
  }

  private bool IsValidName(string str)
  {
    if (str == null || str.Length == 0)
      return false;
    int index = 0;
    for (int length = str.Length; index < length; ++index)
    {
      char c = str[index];
      if (!char.IsLetterOrDigit(c) && Array.IndexOf<char>(NameImpl.DEF_VALID_SYMBOL, c) == -1 && (int) c > NameRecord.PREDEFINED_NAMES.Length)
        return false;
    }
    return true;
  }

  private void SetValue(string strValue, bool useR1C1)
  {
    if (strValue != null && strValue.Length > 0 && strValue[0] == '=')
      strValue = strValue.Substring(1);
    string oldValue = this.Value;
    if (!(oldValue != strValue))
      return;
    this.OnValueChanged(oldValue, strValue, useR1C1);
  }

  public void ConvertFullRowColumnName(ExcelVersion version)
  {
    FormulaRecord.ConvertFormulaTokens(this.m_name.FormulaTokens, version == ExcelVersion.Excel97to2003);
  }

  public string GetValue(FormulaUtil formulaUtil)
  {
    return formulaUtil.ParsePtgArray(this.m_name.FormulaTokens);
  }

  public void SetIndex(int index) => this.SetIndex(index, true);

  public void SetIndex(int index, bool bRaiseEvent)
  {
    if (index == this.m_index)
      return;
    int index1 = this.m_index;
    this.m_index = index;
    if (!bRaiseEvent)
      return;
    this.RaiseNameIndexChangedEvent(new NameIndexChangedEventArgs(index1, index));
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.IsDeleted && this.Worksheet != null && !(this.Workbook.Worksheets as WorksheetsCollection).Contains((IWorksheet) this.Worksheet))
      return;
    records.Add((IBiffStorage) this.m_name);
  }

  public void SetSheetIndex(int iSheetIndex)
  {
    this.m_name.IndexOrGlobal = (ushort) (iSheetIndex + 1);
  }

  private void ReplaceName(
    Dictionary<string, IName> namesCollection,
    string oldName,
    string newName)
  {
    Dictionary<string, IName> dictionary = new Dictionary<string, IName>();
    foreach (KeyValuePair<string, IName> names in namesCollection)
    {
      if (names.Key == oldName)
        dictionary.Add(newName, names.Value);
      else if (!(names.Value as NameImpl).IsDeleted)
        dictionary.Add(names.Key, names.Value);
    }
    namesCollection.Clear();
    foreach (KeyValuePair<string, IName> keyValuePair in dictionary)
      namesCollection.Add(keyValuePair.Key, keyValuePair.Value);
    dictionary?.Clear();
  }

  void IParseable.Parse()
  {
  }

  public Ptg[] GetNativePtg()
  {
    Ptg[] nativePtg = new Ptg[1];
    int num = this.m_book.AddSheetReference(this.m_book.ExternWorkbooks.InsertSelfSupbook(), 65534, 65534);
    nativePtg[0] = FormulaUtil.CreatePtg(FormulaToken.tNameX1, (object) num, (object) this.Index);
    return nativePtg;
  }

  public object Clone(object parent)
  {
    NameImpl nameImpl = (NameImpl) this.MemberwiseClone();
    nameImpl.SetParent(parent);
    nameImpl.SetParents();
    nameImpl.m_name = (NameRecord) CloneUtils.CloneCloneable((ICloneable) this.m_name);
    int indexOrGlobal = (int) this.m_name.IndexOrGlobal;
    if (indexOrGlobal != 0)
    {
      int index = indexOrGlobal - 1;
      WorksheetImpl worksheetImpl = (WorksheetImpl) nameImpl.m_book.Objects[index];
      worksheetImpl.InnerNames.AddLocal((IName) nameImpl);
      nameImpl.m_worksheet = worksheetImpl;
    }
    return (object) nameImpl;
  }

  public IEnumerator<IRange> GetEnumerator() => this.RefersToRange.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.RefersToRange.GetEnumerator();

  public string GetNewAddress(Dictionary<string, string> names, out string strSheetName)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange Clone(object parent, Dictionary<string, string> hashNewNames, WorkbookImpl book)
  {
    if (this.Worksheet == null)
      return (IRange) (book.Names[this.Name] as NameImpl);
    string str = this.Worksheet.Name;
    if (hashNewNames != null && hashNewNames.ContainsKey(str))
      str = hashNewNames[str];
    WorksheetImpl worksheet = (WorksheetImpl) book.Worksheets[str];
    IRange name;
    if (worksheet != null)
    {
      int count = worksheet.Names.Count;
      name = (IRange) (worksheet.Names[this.Name] as NameImpl);
    }
    else
      name = (IRange) (this.Worksheet.Names[this.Name] as NameImpl);
    return name;
  }

  public void ClearConditionalFormats()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public Rectangle[] GetRectangles()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int GetRectanglesCount()
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public int CellsCount
  {
    get => this.RefersToRange != null ? (this.RefersToRange as ICombinedRange).CellsCount : 0;
  }

  public string AddressGlobal2007
  {
    get => this.m_worksheet == null ? "[0]!" + this.Name : $"'{this.m_worksheet.Name}'!{this.Name}";
  }

  public string WorksheetName => this.Worksheet.Name;

  internal void ClearAll()
  {
    if (this.m_name != null)
      this.m_name.ClearData();
    this.m_name = (NameRecord) null;
    this.Dispose();
  }

  void IDisposable.Dispose() => GC.SuppressFinalize((object) this);

  public delegate void NameIndexChangedEventHandler(object sender, NameIndexChangedEventArgs data);
}
