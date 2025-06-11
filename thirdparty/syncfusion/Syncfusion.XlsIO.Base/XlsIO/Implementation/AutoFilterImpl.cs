// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.AutoFilterImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class AutoFilterImpl : IAutoFilter, ICloneParent
{
  private AutoFilterConditionImpl m_firstCondition;
  private AutoFilterConditionImpl m_secondCondition;
  private AutoFilterRecord m_record;
  private FormControlShapeImpl m_shape;
  private AutoFiltersCollection m_autofilters;
  private ExcelFilterType m_filterType;
  private CombinationFilter m_combinationFilter;
  private ColorFilter m_colorFilter;
  private IconFilter m_iconFilter;
  private DynamicFilter m_dateFilter;
  internal int m_colIndex;
  internal double m_filterValue;
  internal bool m_showButton = true;
  private List<KeyValuePair<IRange, double>> m_rangeList = new List<KeyValuePair<IRange, double>>();

  public AutoFilterImpl(AutoFiltersCollection parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    this.InitializeConditions(parent);
    this.m_record = (AutoFilterRecord) BiffRecordFactory.GetRecord(TBIFFRecord.AutoFilter);
    this.m_autofilters = parent;
  }

  public AutoFilterImpl(
    AutoFiltersCollection parent,
    int iColumnIndex,
    int iLastColumn,
    int iRowIndex)
    : this(parent)
  {
    this.m_shape = this.WorksheetShapes.AddFormControlShape();
    this.m_shape.UpdatePositions = false;
    this.m_shape.LeftColumn = iColumnIndex;
    this.m_shape.RightColumn = iLastColumn;
    this.m_shape.TopRow = iRowIndex;
    this.m_shape.BottomRow = iRowIndex;
  }

  [CLSCompliant(false)]
  public AutoFilterImpl(
    AutoFiltersCollection parent,
    AutoFilterRecord record,
    int iColumnIndex,
    int iTopRow)
  {
    this.InitializeConditions(parent);
    this.Parse(record, iColumnIndex, iTopRow);
    this.m_autofilters = parent;
  }

  private void InitializeConditions(AutoFiltersCollection parent)
  {
    this.m_firstCondition = new AutoFilterConditionImpl(parent, this);
    this.m_secondCondition = new AutoFilterConditionImpl(parent, this);
    this.m_combinationFilter = new CombinationFilter((IAutoFilter) this);
    this.m_colorFilter = new ColorFilter();
    this.m_dateFilter = new DynamicFilter();
    this.m_iconFilter = new IconFilter();
  }

  internal void Dispose()
  {
    this.m_colorFilter = (ColorFilter) null;
    this.m_iconFilter = (IconFilter) null;
    this.m_dateFilter = (DynamicFilter) null;
    if (this.m_combinationFilter != null)
      this.m_combinationFilter.Dispose();
    if (this.m_firstCondition != null)
      this.m_firstCondition.Dispose();
    if (this.m_secondCondition != null)
      this.m_secondCondition.Dispose();
    this.m_combinationFilter = (CombinationFilter) null;
    this.m_firstCondition = (AutoFilterConditionImpl) null;
    this.m_secondCondition = (AutoFilterConditionImpl) null;
  }

  public IAutoFilterCondition FirstCondition => (IAutoFilterCondition) this.m_firstCondition;

  public IAutoFilterCondition SecondCondition => (IAutoFilterCondition) this.m_secondCondition;

  internal AutoFilterRecord Record => this.m_record;

  public bool IsFiltered
  {
    get
    {
      return this.IsTop10 && this.Top10Number > 0 || this.IsSimple1 || this.IsSimple2 || this.m_combinationFilter.Count != 0 || this.m_dateFilter.DateFilterType != DynamicFilterType.None || this.IsFirstCondition || this.IsSecondCondition || this.IsColorFilter || this.IsIconFilter || (this.FilterType != ExcelFilterType.CombinationFilter || this.FilteredItems is CombinationFilter filteredItems && filteredItems.Count != 0) && (this.FilterType != ExcelFilterType.CustomFilter || this.FirstCondition.DataType != ExcelFilterDataType.NotUsed || this.SecondCondition.DataType != ExcelFilterDataType.NotUsed);
    }
  }

  public bool IsAnd
  {
    get => this.m_record.IsAnd;
    set => this.m_record.IsAnd = value;
  }

  internal bool IsColorFilter => this.FilterType == ExcelFilterType.ColorFilter;

  internal bool IsIconFilter => this.FilterType == ExcelFilterType.IconFilter;

  public bool IsPercent
  {
    get => this.m_record.IsPercent;
    set => this.m_record.IsPercent = value;
  }

  public bool IsSimple1
  {
    get => this.m_record.IsSimple1;
    set => this.m_record.IsSimple1 = value;
  }

  public bool IsSimple2
  {
    get => this.m_record.IsSimple2;
    set => this.m_record.IsSimple2 = value;
  }

  public bool IsTop
  {
    get => this.m_record.IsTop;
    set => this.m_record.IsTop = value;
  }

  public bool IsTop10
  {
    get => this.m_record.IsTop10;
    set => this.m_record.IsTop10 = value;
  }

  public int Top10Number
  {
    get => this.m_record.Top10Number;
    set
    {
      this.m_record.Top10Number = value;
      this.SelectRangesToFilter();
      this.SetTop10();
    }
  }

  public ExcelFilterType FilterType
  {
    get => this.m_filterType;
    set => this.m_filterType = value;
  }

  public IFilter FilteredItems
  {
    get
    {
      switch (this.FilterType)
      {
        case ExcelFilterType.CombinationFilter:
          return (IFilter) this.m_combinationFilter;
        case ExcelFilterType.DynamicFilter:
          return (IFilter) this.m_dateFilter;
        case ExcelFilterType.ColorFilter:
          return (IFilter) this.m_colorFilter;
        case ExcelFilterType.IconFilter:
          return (IFilter) this.m_iconFilter;
        default:
          return (IFilter) null;
      }
    }
  }

  public WorksheetImpl Worksheet => this.m_autofilters.Worksheet;

  public ShapesCollection WorksheetShapes => (ShapesCollection) this.Worksheet.Shapes;

  public int Index
  {
    get => (int) this.m_record.Index + 1;
    set => this.m_record.Index = (ushort) value;
  }

  public bool IsFirstCondition
  {
    get => !this.IsTop10 && this.m_firstCondition.DataType != ExcelFilterDataType.NotUsed;
  }

  public bool IsSecondCondition
  {
    get => !this.IsTop10 && this.m_secondCondition.DataType != ExcelFilterDataType.NotUsed;
  }

  public bool IsBlanks => this.m_record.IsBlank;

  public bool IsNonBlanks => this.m_record.IsNonBlank;

  public void Clear()
  {
    if (this.m_shape == null)
      return;
    this.m_shape.Remove();
    this.m_shape = (FormControlShapeImpl) null;
  }

  public object Clone(object parent)
  {
    AutoFilterImpl parent1 = new AutoFilterImpl((AutoFiltersCollection) CommonObject.FindParent(parent, typeof (AutoFiltersCollection)));
    if (this.m_firstCondition != null)
      parent1.m_firstCondition = this.m_firstCondition.Clone((object) parent1);
    if (this.m_secondCondition != null)
      parent1.m_secondCondition = this.m_secondCondition.Clone((object) parent1);
    if (this.m_record != null)
      parent1.m_record = (AutoFilterRecord) this.m_record.Clone();
    if (this.m_colorFilter != null)
      parent1.m_colorFilter = this.m_colorFilter.Clone();
    if (this.m_iconFilter != null)
      parent1.m_iconFilter = this.m_iconFilter.Clone();
    parent1.m_filterType = this.m_filterType;
    if (this.m_shape != null)
    {
      ShapesCollection worksheetShapes = parent1.WorksheetShapes;
      bool flag = false;
      int index = 0;
      for (int count = worksheetShapes.Count; index < count; ++index)
      {
        if (worksheetShapes[index] is FormControlShapeImpl controlShapeImpl && controlShapeImpl.TopRow == this.m_shape.TopRow && controlShapeImpl.BottomRow == this.m_shape.BottomRow && controlShapeImpl.LeftColumn == this.m_shape.LeftColumn && controlShapeImpl.RightColumn == this.m_shape.RightColumn && controlShapeImpl.ShapeType == this.m_shape.ShapeType)
        {
          flag = true;
          parent1.m_shape = controlShapeImpl;
          break;
        }
      }
      if (!flag)
        parent1.m_shape = (FormControlShapeImpl) this.m_shape.Clone((object) worksheetShapes, (Dictionary<string, string>) null, (Dictionary<int, int>) null, true);
    }
    return (object) parent1;
  }

  internal void SelectRangesToFilter()
  {
    IRange filterRange = this.m_autofilters.FilterRange;
    int row1 = filterRange.Row;
    int column = filterRange.Column;
    int lastRow1 = filterRange.LastRow;
    int lastColumn = filterRange.LastColumn;
    int lastRow2 = this.m_autofilters.IncludeBottomAdjacents(row1, column, lastRow1, lastColumn, filterRange).LastRow;
    this.m_rangeList.Clear();
    for (int row2 = row1 + 1; row2 <= lastRow2; ++row2)
    {
      IRange key = this.Worksheet[row2, this.m_colIndex];
      this.m_rangeList.Add(new KeyValuePair<IRange, double>(key, key.Number));
    }
    this.m_rangeList.Sort((Comparison<KeyValuePair<IRange, double>>) ((x, y) => x.Value.CompareTo(y.Value)));
  }

  internal void SetTop10()
  {
    int count = this.m_rangeList.Count;
    int index1;
    int index2;
    if (this.IsTop)
    {
      int index3;
      if (this.IsPercent)
      {
        int num = count * this.Top10Number / 100;
        index3 = count - num;
      }
      else
        index3 = count - this.Top10Number;
      this.m_filterValue = index3 < 0 ? this.m_rangeList[0].Value : this.m_rangeList[index3].Value;
      index1 = 0;
      index2 = index3 + 1;
      if (double.IsNaN(this.m_filterValue))
      {
        while (index2 < count && double.IsNaN(this.m_rangeList[index2].Value))
          ++index2;
      }
    }
    else
    {
      int index4 = this.Top10Number - 1;
      this.m_filterValue = this.m_rangeList[index4].Value;
      index1 = index4 + 1;
      index2 = count;
      if (double.IsNaN(this.m_filterValue))
      {
        while (index1 > 0)
        {
          --index1;
          if (!double.IsNaN(this.m_rangeList[index1].Value))
            break;
        }
      }
    }
    for (int index5 = index1; index5 < index2; ++index5)
    {
      CellRecordCollection cellRecords = this.Worksheet.CellRecords;
      if (cellRecords.Table.Rows[this.m_rangeList[index5].Key.Row - 1] != null && (double.IsNaN(this.m_rangeList[index5].Value) || this.m_rangeList[index5].Value != this.m_filterValue && !cellRecords.Table.Rows[this.m_rangeList[index5].Key.Row - 1].IsHidden))
        (this.m_rangeList[index5].Key.Worksheet as WorksheetImpl).ShowFilteredRows(this.m_rangeList[index5].Key.Row, this.m_rangeList[index5].Key.Column, false, false, true);
    }
  }

  internal void SetCondition(
    ExcelFilterCondition conditionOperator,
    ExcelFilterDataType datatype,
    object conditionValue,
    int currentAutoFilter,
    bool isFirstCondition)
  {
    WorksheetImpl worksheet = this.Worksheet;
    Type type = double.TryParse(Convert.ToString(conditionValue), out double _) ? typeof (double) : (DateTime.TryParse(Convert.ToString(conditionValue), out DateTime _) ? typeof (DateTime) : typeof (string));
    if (datatype == ExcelFilterDataType.MatchAllBlanks)
      this.SetMatchAllBlanks(isFirstCondition);
    else if (datatype == ExcelFilterDataType.MatchAllNonBlanks)
    {
      this.SetMatchAllNonBlanks(isFirstCondition);
    }
    else
    {
      if (conditionOperator == ExcelFilterCondition.Equal && datatype == ExcelFilterDataType.String)
        this.IsSimple1 = true;
      conditionValue = (object) conditionValue.ToString().Replace("*", "");
      for (int index = 0; index < this.m_rangeList.Count; ++index)
        this.SetCondition(conditionOperator, conditionValue, worksheet, this.m_rangeList[index], isFirstCondition, type);
    }
  }

  private void SetCondition(
    ExcelFilterCondition conditionOperator,
    object conditionValue,
    WorksheetImpl worksheet,
    KeyValuePair<IRange, double> range,
    bool isFirstCondition,
    Type type)
  {
    bool disableCalEngine = false;
    IRange key = range.Key;
    WorksheetImpl worksheet1 = key.Worksheet as WorksheetImpl;
    if (key.HasFormula)
    {
      if (!worksheet.IsParsing)
        this.InitializeCalcEngine(ref disableCalEngine);
      else if (((FormulaRecord) worksheet.CellRecords.GetCellRecord(key.Row, this.m_colIndex)).CalculateOnOpen && !(worksheet.Workbook as WorkbookImpl).EnabledCalcEngine)
        this.InitializeCalcEngine(ref disableCalEngine);
      (worksheet.Workbook as WorkbookImpl).EnabledCalcEngine = false;
      if (key.HasFormulaNumberValue)
        worksheet1.ShowFilteredRows(key.Row, key.Column, this.GetComparerResult((object) key.FormulaNumberValue, conditionValue, conditionOperator), this.IsAnd, isFirstCondition);
      else if (key.HasFormulaStringValue)
        worksheet1.ShowFilteredRows(key.Row, key.Column, this.GetComparerResult((object) key.FormulaStringValue, conditionValue, conditionOperator), this.IsAnd, isFirstCondition);
      else
        worksheet1.ShowFilteredRows(key.Row, key.Column, this.GetComparerResult((object) key.CalculatedValue, conditionValue, conditionOperator), this.IsAnd, isFirstCondition);
    }
    else if (type.Equals(typeof (double)))
    {
      object a;
      object b;
      switch (conditionOperator)
      {
        case ExcelFilterCondition.Equal:
          a = (object) key.DisplayText;
          b = (object) Convert.ToString(conditionValue);
          break;
        case ExcelFilterCondition.Contains:
        case ExcelFilterCondition.DoesNotContain:
        case ExcelFilterCondition.BeginsWith:
        case ExcelFilterCondition.DoesNotBeginWith:
        case ExcelFilterCondition.EndsWith:
        case ExcelFilterCondition.DoesNotEndWith:
          a = (object) key.Text;
          b = (object) Convert.ToString(conditionValue);
          break;
        default:
          a = (object) key.Number;
          b = (object) Convert.ToDouble(conditionValue);
          break;
      }
      worksheet1.ShowFilteredRows(key.Row, key.Column, this.GetComparerResult(a, b, conditionOperator), this.IsAnd, isFirstCondition);
    }
    else if (type.Equals(typeof (DateTime)))
    {
      switch (conditionOperator)
      {
        case ExcelFilterCondition.Contains:
        case ExcelFilterCondition.BeginsWith:
        case ExcelFilterCondition.EndsWith:
          worksheet1.ShowFilteredRows(key.Row, key.Column, false, this.IsAnd, isFirstCondition);
          break;
        case ExcelFilterCondition.DoesNotContain:
        case ExcelFilterCondition.DoesNotBeginWith:
        case ExcelFilterCondition.DoesNotEndWith:
          worksheet1.ShowFilteredRows(key.Row, key.Column, true, this.IsAnd, isFirstCondition);
          break;
        default:
          worksheet1.ShowFilteredRows(key.Row, key.Column, this.GetComparerResult((object) key.DateTime, (object) Convert.ToDateTime(conditionValue), conditionOperator), this.IsAnd, isFirstCondition);
          break;
      }
    }
    else if (type.Equals(typeof (string)))
    {
      object a = conditionOperator != ExcelFilterCondition.Equal ? (object) key.Text : (object) key.DisplayText;
      worksheet1.ShowFilteredRows(key.Row, key.Column, this.GetComparerResult(a, conditionValue, conditionOperator), this.IsAnd, isFirstCondition);
    }
    else
      worksheet1.ShowFilteredRows(key.Row, key.Column, false, this.IsAnd, isFirstCondition);
  }

  private bool GetComparerResult(object a, object b, ExcelFilterCondition conditionOperator)
  {
    string str1 = Convert.ToString(a);
    string str2 = Convert.ToString(b);
    if (!(str1 != "NaN") && conditionOperator != ExcelFilterCondition.NotEqual)
      return false;
    switch (conditionOperator)
    {
      case ExcelFilterCondition.Less:
        return this.GetComparerResult(a, b, StringComparison.OrdinalIgnoreCase) < 0 && a != null;
      case ExcelFilterCondition.Equal:
        return this.GetComparerResult(a, b, StringComparison.OrdinalIgnoreCase) == 0;
      case ExcelFilterCondition.LessOrEqual:
        return this.GetComparerResult(a, b, StringComparison.OrdinalIgnoreCase) <= 0 && a != null;
      case ExcelFilterCondition.Greater:
        return this.GetComparerResult(a, b, StringComparison.OrdinalIgnoreCase) > 0;
      case ExcelFilterCondition.NotEqual:
        return this.GetComparerResult(a, b, StringComparison.OrdinalIgnoreCase) != 0;
      case ExcelFilterCondition.GreaterOrEqual:
        return this.GetComparerResult(a, b, StringComparison.OrdinalIgnoreCase) >= 0;
      case ExcelFilterCondition.Contains:
        return str1.Contains(str2);
      case ExcelFilterCondition.DoesNotContain:
        return !str1.Contains(str2);
      case ExcelFilterCondition.BeginsWith:
        return str1.StartsWith(str2);
      case ExcelFilterCondition.DoesNotBeginWith:
        return !str1.StartsWith(str2);
      case ExcelFilterCondition.EndsWith:
        return str1.EndsWith(str2);
      case ExcelFilterCondition.DoesNotEndWith:
        return !str1.EndsWith(str2);
      default:
        return false;
    }
  }

  private int GetComparerResult(object a, object b, StringComparison comparisonType)
  {
    if (a == null && b == null)
      return 0;
    if (a == null)
      return -1;
    if (b == null)
      return 1;
    if (a is string && b is string)
      return string.Compare(a as string, b as string, comparisonType);
    return !a.GetType().Equals(b.GetType()) ? string.Compare(a.ToString(), b.ToString(), comparisonType) : Comparer<object>.Default.Compare(a, b);
  }

  private void SetMatchAllBlanks(bool isFirstCondition)
  {
    this.IsSimple1 = true;
    for (int index = 0; index < this.m_rangeList.Count; ++index)
    {
      WorksheetImpl worksheet = this.m_rangeList[index].Key.Worksheet as WorksheetImpl;
      if (this.m_rangeList[index].Key.DisplayText == string.Empty)
        worksheet.ShowFilteredRows(this.m_rangeList[index].Key.Row, this.m_rangeList[index].Key.Column, true, this.IsAnd, isFirstCondition);
      else
        worksheet.ShowFilteredRows(this.m_rangeList[index].Key.Row, this.m_rangeList[index].Key.Column, false, this.IsAnd, isFirstCondition);
    }
  }

  private void SetMatchAllNonBlanks(bool isFirstCondition)
  {
    this.IsSimple1 = true;
    for (int index = 0; index < this.m_rangeList.Count; ++index)
    {
      WorksheetImpl worksheet = this.m_rangeList[index].Key.Worksheet as WorksheetImpl;
      if (this.m_rangeList[index].Key.DisplayText != string.Empty)
        worksheet.ShowFilteredRows(this.m_rangeList[index].Key.Row, this.m_rangeList[index].Key.Column, true, this.IsAnd, isFirstCondition);
      else
        worksheet.ShowFilteredRows(this.m_rangeList[index].Key.Row, this.m_rangeList[index].Key.Column, false, this.IsAnd, isFirstCondition);
    }
  }

  public void AddTextFilter(IEnumerable<string> filterCollection)
  {
    this.FilterType = ExcelFilterType.CombinationFilter;
    List<IMultipleFilter> filterCollection1 = this.m_combinationFilter.m_filterCollection;
    foreach (string filter in filterCollection)
    {
      if (filter.Trim() == string.Empty)
        this.m_combinationFilter.IsBlank = true;
      else if (!this.m_combinationFilter.TextFiltersCollection.Contains(filter))
        filterCollection1.Add((IMultipleFilter) new TextFilter()
        {
          Text = filter.Trim()
        });
    }
    this.ApplyTextFilter();
  }

  public void AddColorFilter(Color color, ExcelColorFilterType colorFilterType)
  {
    if (this.IsColorFilter)
      this.RemoveColorFilter();
    this.FilterType = ExcelFilterType.ColorFilter;
    this.m_colorFilter.Color = color;
    this.m_colorFilter.ColorFilterType = colorFilterType;
    IRange filterRange = this.m_autofilters.FilterRange;
    IMigrantRange migrantRange = (IMigrantRange) new MigrantRangeImpl((IApplication) this.Worksheet.AppImplementation, (IWorksheet) this.Worksheet);
    WorksheetImpl worksheet = this.Worksheet;
    SortedList<long, ExtendedFormatImpl> sortedList = worksheet.ApplyCF(filterRange);
    if (color.A == (byte) 0 && color.R == (byte) 0 && color.G == (byte) 0 && color.B == (byte) 0)
    {
      switch (colorFilterType)
      {
        case ExcelColorFilterType.CellColor:
          color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          break;
        case ExcelColorFilterType.FontColor:
          color = this.Worksheet.Workbook.Styles["Normal"].Font.RGBColor;
          break;
      }
    }
    for (int index = filterRange.Row + 1; index <= filterRange.LastRow; ++index)
    {
      migrantRange.ResetRowColumn(index, this.m_colIndex);
      Color color1 = Color.FromArgb(0, 0, 0, 0);
      ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
      if (!sortedList.TryGetValue(RangeImpl.GetCellIndex(this.m_colIndex, index), out extendedFormatImpl))
        extendedFormatImpl = (migrantRange.CellStyle as CellStyle).Wrapped;
      color1 = colorFilterType != ExcelColorFilterType.CellColor ? extendedFormatImpl.Font.RGBColor : extendedFormatImpl.Color;
      if ((int) color.R != (int) color1.R || (int) color.G != (int) color1.G || (int) color.B != (int) color1.B)
        worksheet.ShowFilteredRows(index, this.m_colIndex, false);
    }
  }

  public void RemoveColorFilter()
  {
    if (!this.IsColorFilter)
      return;
    this.m_colorFilter.ColorFilterType = (ExcelColorFilterType) 0;
    this.RemoveFilter();
  }

  public void AddIconFilter(ExcelIconSetType iconSetType, int iconId)
  {
    if (iconSetType.GetHashCode() >= CF.IconSetTypeNames.Length || iconSetType.GetHashCode() < -1)
      throw new ArgumentOutOfRangeException(nameof (iconSetType));
    if (iconId < -1)
      throw new ArgumentOutOfRangeException(nameof (iconId));
    if (iconSetType.GetHashCode() != -1)
    {
      int num = int.Parse(CF.IconSetTypeNames[(int) iconSetType][0].ToString());
      if (iconId >= num)
        throw new ArgumentOutOfRangeException("IconId");
    }
    this.FilterType = ExcelFilterType.IconFilter;
    this.m_iconFilter.IconSetType = iconId == -1 ? ~ExcelIconSetType.ThreeArrows : iconSetType;
    this.m_iconFilter.IconId = iconSetType.GetHashCode() == -1 ? -1 : iconId;
    IRange filterRange = this.m_autofilters.FilterRange;
    IMigrantRange migrantRange = (IMigrantRange) new MigrantRangeImpl((IApplication) this.Worksheet.AppImplementation, (IWorksheet) this.Worksheet);
    WorksheetImpl worksheet = this.Worksheet;
    SortedList<long, ExtendedFormatImpl> sortedList = worksheet.ApplyCF(filterRange);
    for (int index = filterRange.Row + 1; index <= filterRange.LastRow; ++index)
    {
      migrantRange.ResetRowColumn(index, this.m_colIndex);
      ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
      if (!sortedList.TryGetValue(RangeImpl.GetCellIndex(this.m_colIndex, index), out extendedFormatImpl))
        extendedFormatImpl = (migrantRange.CellStyle as CellStyle).Wrapped;
      if (!(extendedFormatImpl is ExtendedFormatStandAlone) || !(extendedFormatImpl as ExtendedFormatStandAlone).HasIconSet)
      {
        if (this.m_iconFilter.IconId != -1)
          worksheet.ShowFilteredRows(index, this.m_colIndex, false);
      }
      else if ((extendedFormatImpl as ExtendedFormatStandAlone).IconSet.GetHashCode() != iconSetType.GetHashCode() || (extendedFormatImpl as ExtendedFormatStandAlone).IconId != iconId)
        worksheet.ShowFilteredRows(index, this.m_colIndex, false);
    }
  }

  public void RemoveIconFilter()
  {
    if (this.m_iconFilter.IconId == int.MinValue)
      return;
    this.RemoveFilter();
    this.m_iconFilter.IconId = int.MinValue;
  }

  private void RemoveFilter()
  {
    this.FilterType = ExcelFilterType.CustomFilter;
    IRange filterRange = this.m_autofilters.FilterRange;
    WorksheetImpl worksheet = this.Worksheet;
    for (int rowIndex = filterRange.Row + 1; rowIndex <= filterRange.LastRow; ++rowIndex)
      worksheet.ShowFilteredRows(rowIndex, this.m_colIndex, true);
  }

  public void AddTextFilter(string filter)
  {
    this.FilterType = ExcelFilterType.CombinationFilter;
    if (filter == null)
      return;
    if (filter.Trim() == string.Empty)
    {
      this.m_combinationFilter.IsBlank = true;
    }
    else
    {
      if (this.m_combinationFilter.TextFiltersCollection.Contains(filter))
        return;
      this.m_combinationFilter.m_filterCollection.Add((IMultipleFilter) new TextFilter()
      {
        Text = filter.Trim()
      });
      this.ApplyTextFilter();
    }
  }

  public bool RemoveText(string filter)
  {
    if (this.m_combinationFilter != null)
    {
      foreach (IMultipleFilter filter1 in this.m_combinationFilter.m_filterCollection)
      {
        if (filter1.CombinationFilterType == ExcelCombinationFilterType.TextFilter && (filter1 as TextFilter).Text == filter)
        {
          this.m_combinationFilter.m_filterCollection.Remove(filter1);
          this.ApplyTextFilter();
          return true;
        }
      }
    }
    return false;
  }

  public bool RemoveText(IEnumerable<string> filterCollection)
  {
    bool flag = false;
    if (this.m_combinationFilter != null)
    {
      List<IMultipleFilter> filterCollection1 = this.m_combinationFilter.m_filterCollection;
      foreach (string filter in filterCollection)
      {
        if (filter.Trim() == string.Empty)
        {
          this.m_combinationFilter.IsBlank = false;
          flag = true;
        }
        else if (this.m_combinationFilter.TextFiltersCollection.Contains(filter))
        {
          foreach (IMultipleFilter multipleFilter in filterCollection1)
          {
            if (multipleFilter.CombinationFilterType == ExcelCombinationFilterType.TextFilter && (multipleFilter as TextFilter).Text == filter)
            {
              filterCollection1.Remove(multipleFilter);
              flag = true;
              break;
            }
          }
        }
      }
      if (flag)
      {
        this.ApplyTextFilter();
        return true;
      }
    }
    return false;
  }

  public void AddDateFilter(
    int year,
    int month,
    int day,
    int hour,
    int mintue,
    int second,
    DateTimeGroupingType groupingType)
  {
    this.AddDateFilter(new DateTime(year, month, day, hour, mintue, second), groupingType);
  }

  public void AddDateFilter(DateTime dateTime, DateTimeGroupingType groupingType)
  {
    this.FilterType = ExcelFilterType.CombinationFilter;
    this.m_combinationFilter.m_filterCollection.Add((IMultipleFilter) new DateTimeFilter()
    {
      DateTimeValue = dateTime,
      GroupingType = groupingType
    });
    this.ApplyTextFilter();
  }

  public bool RemoveDate(
    int year,
    int month,
    int day,
    int hour,
    int mintue,
    int second,
    DateTimeGroupingType groupingType)
  {
    return this.RemoveDate(new DateTime(year, month, day, hour, mintue, second), groupingType);
  }

  public bool RemoveDate(DateTime dateTime, DateTimeGroupingType groupingType)
  {
    if (this.m_combinationFilter != null)
    {
      foreach (IMultipleFilter filter in this.m_combinationFilter.m_filterCollection)
      {
        if (filter.CombinationFilterType == ExcelCombinationFilterType.DateTimeFilter)
        {
          DateTimeFilter dateTimeFilter = filter as DateTimeFilter;
          DateTimeGroupingType groupingType1 = dateTimeFilter.GroupingType;
          DateTime dateTimeValue = dateTimeFilter.DateTimeValue;
          if (groupingType1 == groupingType && dateTimeValue.Year == dateTime.Year)
          {
            if (groupingType1 == DateTimeGroupingType.year)
            {
              this.m_combinationFilter.m_filterCollection.Remove(filter);
              this.ApplyTextFilter();
              return true;
            }
            if (dateTimeValue.Month == dateTime.Month)
            {
              if (groupingType1 == DateTimeGroupingType.month)
              {
                this.m_combinationFilter.m_filterCollection.Remove(filter);
                this.ApplyTextFilter();
                return true;
              }
              if (dateTimeValue.Day == dateTime.Day)
              {
                if (groupingType1 == DateTimeGroupingType.day)
                {
                  this.m_combinationFilter.m_filterCollection.Remove(filter);
                  this.ApplyTextFilter();
                  return true;
                }
                if (dateTimeValue.Hour == dateTime.Hour)
                {
                  if (groupingType1 == DateTimeGroupingType.hour)
                  {
                    this.m_combinationFilter.m_filterCollection.Remove(filter);
                    this.ApplyTextFilter();
                    return true;
                  }
                  if (dateTimeValue.Minute == dateTime.Minute)
                  {
                    if (groupingType1 == DateTimeGroupingType.minute)
                    {
                      this.m_combinationFilter.m_filterCollection.Remove(filter);
                      this.ApplyTextFilter();
                      return true;
                    }
                    if (dateTimeValue.Second == dateTime.Second)
                    {
                      this.m_combinationFilter.m_filterCollection.Remove(filter);
                      this.ApplyTextFilter();
                      return true;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    return false;
  }

  public void AddDynamicFilter(DynamicFilterType dateFilterType)
  {
    if (this.m_dateFilter == null)
      this.m_dateFilter = new DynamicFilter();
    this.FilterType = ExcelFilterType.DynamicFilter;
    this.m_dateFilter.DateFilterType = dateFilterType;
    this.ApplyDynamicFilter();
  }

  public bool RemoveDynamicFilter()
  {
    if (this.m_dateFilter == null)
      return false;
    this.m_filterType = ExcelFilterType.CustomFilter;
    this.m_dateFilter.DateFilterType = DynamicFilterType.None;
    IRange filterRange = this.m_autofilters.FilterRange;
    WorksheetImpl worksheet = this.Worksheet;
    int row = filterRange.Row;
    int lastRow = filterRange.LastRow;
    for (int rowIndex = row + 1; rowIndex <= lastRow; ++rowIndex)
      worksheet.ShowFilteredRows(rowIndex, this.m_colIndex, true);
    return true;
  }

  private void ApplyTextFilter()
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    if (this.m_combinationFilter.IsBlank)
      dictionary.Add(string.Empty, string.Empty);
    foreach (IMultipleFilter filter in this.m_combinationFilter.m_filterCollection)
    {
      if (filter.CombinationFilterType == ExcelCombinationFilterType.TextFilter)
      {
        string lower = (filter as TextFilter).Text.ToLower();
        dictionary.Add(lower, lower);
      }
    }
    IRange filterRange = this.m_autofilters.FilterRange;
    IMigrantRange migrantRange = this.Worksheet.MigrantRange;
    bool disableCalEngine = false;
    int row = filterRange.Row;
    int lastRow = filterRange.LastRow;
    WorksheetImpl worksheet = this.Worksheet;
    for (int index = row + 1; index <= lastRow; ++index)
    {
      migrantRange.ResetRowColumn(index, this.m_colIndex);
      if (dictionary.Count != 0)
      {
        string key = (string) null;
        if (!worksheet.IsParsing)
          key = migrantRange.DisplayText.Trim().ToLower();
        else if (worksheet.IsParsing && !worksheet.m_autoFilterDisplayTexts.TryGetValue(migrantRange.Address, out key))
        {
          key = migrantRange.DisplayText.Trim().ToLower();
          worksheet.m_autoFilterDisplayTexts.Add(migrantRange.Address, key);
        }
        if (migrantRange.HasFormula)
        {
          if (worksheet.IsParsing && ((FormulaRecord) worksheet.CellRecords.GetCellRecord(index, this.m_colIndex)).CalculateOnOpen && !(worksheet.Workbook as WorkbookImpl).EnabledCalcEngine)
            this.InitializeCalcEngine(ref disableCalEngine);
          else if (!worksheet.IsParsing)
            this.InitializeCalcEngine(ref disableCalEngine);
          (this.Worksheet.Workbook as WorkbookImpl).EnabledCalcEngine = false;
          if (dictionary.ContainsKey(key))
            worksheet.ShowFilteredRows(index, this.m_colIndex, true);
          else
            worksheet.ShowFilteredRows(index, this.m_colIndex, false);
        }
        else if (dictionary.ContainsKey(key))
        {
          worksheet.ShowFilteredRows(index, this.m_colIndex, true);
          RowStorage rowStorage = (migrantRange as RangeImpl).RowStorage;
          rowStorage.IsFilteredRow = rowStorage.IsHidden;
        }
        else
        {
          worksheet.ShowFilteredRows(index, this.m_colIndex, false);
          (migrantRange as RangeImpl).RowStorage.IsFilteredRow = true;
        }
      }
      else
      {
        worksheet.ShowFilteredRows(index, this.m_colIndex, false);
        (migrantRange as RangeImpl).RowStorage.IsFilteredRow = true;
      }
    }
    if (disableCalEngine)
      this.m_autofilters.Worksheet.DisableSheetCalculations();
    else
      (this.Worksheet.Workbook as WorkbookImpl).EnabledCalcEngine = true;
    if (this.m_combinationFilter.TextFiltersCollection.Count == this.m_combinationFilter.Count)
      return;
    this.ApplyDateTimeFilter();
  }

  private void InitializeCalcEngine(ref bool disableCalEngine)
  {
    if (this.Worksheet.CalcEngine == null)
    {
      disableCalEngine = true;
      this.m_autofilters.Worksheet.EnableSheetCalculations();
      this.m_autofilters.Worksheet.CalcEngine.UseFormulaValues = true;
    }
    else if (!this.m_autofilters.Worksheet.CalcEngine.UseFormulaValues)
      this.m_autofilters.Worksheet.CalcEngine.UseFormulaValues = true;
    if ((this.Worksheet.Workbook as WorkbookImpl).EnabledCalcEngine)
      return;
    (this.Worksheet.Workbook as WorkbookImpl).EnabledCalcEngine = true;
  }

  private void ApplyDateTimeFilter()
  {
    if (this.m_combinationFilter == null)
      return;
    IRange filterRange = this.m_autofilters.FilterRange;
    IMigrantRange migrantRange = this.Worksheet.MigrantRange;
    for (int index = filterRange.Row + 1; index <= filterRange.LastRow; ++index)
    {
      migrantRange.ResetRowColumn(index, this.m_colIndex);
      if (migrantRange.HasDateTime)
      {
        foreach (IMultipleFilter filter in this.m_combinationFilter.m_filterCollection)
        {
          if (filter.CombinationFilterType == ExcelCombinationFilterType.DateTimeFilter)
          {
            DateTimeFilter dateTimeFilter = filter as DateTimeFilter;
            DateTimeGroupingType groupingType = dateTimeFilter.GroupingType;
            DateTime dateTimeValue = dateTimeFilter.DateTimeValue;
            DateTime dateTime = migrantRange.DateTime;
            if (dateTimeValue.Year == dateTime.Year)
            {
              if (groupingType == DateTimeGroupingType.year)
              {
                this.Worksheet.ShowFilteredRows(index, this.m_colIndex, true);
                break;
              }
              if (dateTimeValue.Month == dateTime.Month)
              {
                if (groupingType == DateTimeGroupingType.month)
                {
                  this.Worksheet.ShowFilteredRows(index, this.m_colIndex, true);
                  break;
                }
                if (dateTimeValue.Day == dateTime.Day)
                {
                  if (groupingType == DateTimeGroupingType.day)
                  {
                    this.Worksheet.ShowFilteredRows(index, this.m_colIndex, true);
                    break;
                  }
                  if (dateTimeValue.Hour == dateTime.Hour)
                  {
                    if (groupingType == DateTimeGroupingType.hour)
                    {
                      this.Worksheet.ShowFilteredRows(index, this.m_colIndex, true);
                      break;
                    }
                    if (dateTimeValue.Minute == dateTime.Minute)
                    {
                      if (groupingType == DateTimeGroupingType.minute)
                      {
                        this.Worksheet.ShowFilteredRows(index, this.m_colIndex, true);
                        break;
                      }
                      if (dateTimeValue.Second == dateTime.Second)
                      {
                        this.Worksheet.ShowFilteredRows(index, this.m_colIndex, true);
                        break;
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

  private void ApplyDynamicFilter()
  {
    if (this.m_dateFilter == null)
      return;
    if (this.m_dateFilter.DateFilterType == DynamicFilterType.None)
    {
      this.RemoveDynamicFilter();
    }
    else
    {
      IRange filterRange = this.m_autofilters.FilterRange;
      IMigrantRange migrantRange = this.Worksheet.MigrantRange;
      bool flag1 = false;
      for (int index1 = filterRange.Row + 1; index1 <= filterRange.LastRow; ++index1)
      {
        WorksheetImpl worksheet = this.Worksheet;
        migrantRange.ResetRowColumn(index1, this.m_colIndex);
        bool flag2 = false;
        DateTime result = new DateTime();
        if (migrantRange.HasFormula)
        {
          if (worksheet.CalcEngine == null)
          {
            worksheet.EnableSheetCalculations();
            flag1 = true;
          }
          flag2 = DateTime.TryParse(migrantRange.CalculatedValue, out result);
        }
        if (flag2 || migrantRange.HasDateTime)
        {
          bool isVisible = false;
          DateTime today = DateTime.Today;
          if (!flag2)
            result = migrantRange.DateTime;
          DateTime dateTime1;
          DateTime dateTime2;
          switch (this.m_dateFilter.DateFilterType)
          {
            case DynamicFilterType.Tomorrow:
              if (result.Year == today.Year && result.Month == today.Month && result.Day == today.Day + 1)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.Today:
              if (result.Year == today.Year && result.Month == today.Month && result.Day == today.Day)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.Yesterday:
              if (result.Year == today.Year && result.Month == today.Month && result.Day == today.Day - 1)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.NextWeek:
              dateTime1 = today.AddDays((double) (7 - today.DayOfWeek));
              dateTime2 = dateTime1.AddDays(6.0);
              if (result.Year >= dateTime1.Year && result.Year <= dateTime2.Year && result.Month >= dateTime1.Month && result.Month <= dateTime2.Month && result.Day >= dateTime1.Day && result.Day <= dateTime2.Day)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.ThisWeek:
              dateTime1 = today.AddDays((double) -(int) today.DayOfWeek);
              dateTime2 = dateTime1.AddDays(6.0);
              if (result.Year >= dateTime1.Year && result.Year <= dateTime2.Year && result.Month >= dateTime1.Month && result.Month <= dateTime2.Month && result.Day >= dateTime1.Day && result.Day <= dateTime2.Day)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.LastWeek:
              dateTime2 = today.AddDays((double) -(int) (today.DayOfWeek + 1));
              dateTime1 = dateTime2.AddDays(-6.0);
              if (result.Year >= dateTime1.Year && result.Year <= dateTime2.Year && result.Month >= dateTime1.Month && result.Month <= dateTime2.Month && result.Day >= dateTime1.Day && result.Day <= dateTime2.Day)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.NextMonth:
              dateTime1 = today.AddMonths(1);
              if (result.Year == dateTime1.Year && result.Month == dateTime1.Month)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.ThisMonth:
              if (result.Year == today.Year && result.Month == today.Month)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.LastMonth:
              dateTime1 = today.AddMonths(-1);
              if (result.Year == dateTime1.Year && result.Month == dateTime1.Month)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.NextQuarter:
              int num1 = today.Month / 3;
              if (today.Month % 3 != 0)
                ++num1;
              if (num1 == 4)
              {
                int num2 = 1;
                for (int index2 = num2 * 3 - 2; index2 <= num2 * 3; ++index2)
                {
                  if (result.Year == today.Year + 1 && result.Month == index2)
                  {
                    isVisible = true;
                    break;
                  }
                }
                break;
              }
              int num3 = num1 + 1;
              for (int index3 = num3 * 3 - 2; index3 <= num3 * 3; ++index3)
              {
                if (result.Year == today.Year && result.Month == index3)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.ThisQuarter:
              int num4 = today.Month / 3;
              if (today.Month % 3 != 0)
                ++num4;
              for (int index4 = num4 * 3 - 2; index4 <= num4 * 3; ++index4)
              {
                if (result.Year == today.Year && result.Month == index4)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.LastQuarter:
              int num5 = today.Month / 3;
              if (today.Month % 3 != 0)
                ++num5;
              if (num5 == 1)
              {
                int num6 = 4;
                for (int index5 = num6 * 3 - 2; index5 <= num6 * 3; ++index5)
                {
                  if (result.Year == today.Year - 1 && result.Month == index5)
                  {
                    isVisible = true;
                    break;
                  }
                }
                break;
              }
              int num7 = num5 - 1;
              for (int index6 = num7 * 3 - 2; index6 <= num7 * 3; ++index6)
              {
                if (result.Year == today.Year && result.Month == index6)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.NextYear:
              if (result.Year == today.Year + 1)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.ThisYear:
              if (result.Year == today.Year)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.LastYear:
              if (result.Year == today.Year - 1)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.Quarter1:
              for (int index7 = 1; index7 <= 3; ++index7)
              {
                if (result.Month == index7)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.Quarter2:
              for (int index8 = 4; index8 <= 6; ++index8)
              {
                if (result.Month == index8)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.Quarter3:
              for (int index9 = 7; index9 <= 9; ++index9)
              {
                if (result.Month == index9)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.Quarter4:
              for (int index10 = 10; index10 <= 12; ++index10)
              {
                if (result.Month == index10)
                {
                  isVisible = true;
                  break;
                }
              }
              break;
            case DynamicFilterType.January:
              if (result.Month == 1)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.February:
              if (result.Month == 2)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.March:
              if (result.Month == 3)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.April:
              if (result.Month == 4)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.May:
              if (result.Month == 5)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.June:
              if (result.Month == 6)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.July:
              if (result.Month == 7)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.August:
              if (result.Month == 8)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.September:
              if (result.Month == 9)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.October:
              if (result.Month == 10)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.November:
              if (result.Month == 11)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.December:
              if (result.Month == 12)
              {
                isVisible = true;
                break;
              }
              break;
            case DynamicFilterType.YearToDate:
              if (today.Year == result.Year && today.Day >= result.Day && today.Month >= result.Month)
              {
                isVisible = true;
                break;
              }
              break;
          }
          worksheet.ShowFilteredRows(index1, this.m_colIndex, isVisible);
        }
        else
          worksheet.ShowFilteredRows(index1, this.m_colIndex, false);
      }
      if (!flag1)
        return;
      this.Worksheet.DisableSheetCalculations();
    }
  }

  [CLSCompliant(false)]
  public void Parse(AutoFilterRecord record, int iColumnIndex, int iRowIndex)
  {
    this.m_record = (AutoFilterRecord) record.Clone();
    this.m_firstCondition.Parse(record.FirstCondition);
    this.m_secondCondition.Parse(record.SecondCondition);
    ShapesCollection worksheetShapes = this.WorksheetShapes;
    int index = 0;
    for (int count = worksheetShapes.Count; index < count; ++index)
    {
      IShape shape = worksheetShapes[index];
      if (shape is FormControlShapeImpl)
      {
        FormControlShapeImpl controlShapeImpl = shape as FormControlShapeImpl;
        if (controlShapeImpl.LeftColumn == iColumnIndex && controlShapeImpl.Top == iRowIndex)
        {
          this.m_shape = controlShapeImpl;
          return;
        }
      }
    }
    this.m_shape = this.WorksheetShapes.AddFormControlShape();
    this.m_shape.LeftColumn = iColumnIndex;
    this.m_shape.TopRow = iRowIndex;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    this.m_firstCondition.Serialize(this.m_record.FirstCondition);
    this.m_secondCondition.Serialize(this.m_record.SecondCondition);
    records.Add((IBiffStorage) this.m_record);
  }
}
