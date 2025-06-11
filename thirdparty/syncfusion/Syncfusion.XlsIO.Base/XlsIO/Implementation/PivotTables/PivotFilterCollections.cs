// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFilterCollections
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFilterCollections : IPivotFilters
{
  private List<PivotFilterImpl> m_pivotFilterImpl = new List<PivotFilterImpl>();
  private IPivotValueLableFilter m_valueFilter;
  private IPivotField m_parent;

  public IPivotValueLableFilter ValueFilter
  {
    get => this.m_valueFilter;
    set => this.m_valueFilter = value;
  }

  public IPivotField Parent
  {
    get => this.m_parent;
    set => this.m_parent = value;
  }

  public PivotFilterCollections(IPivotField field) => this.m_parent = field;

  public IPivotValueLableFilter Add(
    PivotFilterType filterType,
    IPivotField dataField,
    string Value1,
    string Value2)
  {
    PivotFieldImpl parent = this.Parent as PivotFieldImpl;
    PivotTableImpl table = parent.m_table;
    table.Cache.IsRefreshOnLoad = true;
    parent.IsMultiSelected = false;
    if (parent.Axis == PivotAxisTypes.Row && parent.Axis == PivotAxisTypes.Column)
      throw new ArgumentException("Field must be Row based or Column Based");
    if (filterType.ToString().Contains("value"))
    {
      if ((filterType == PivotFilterType.ValueNotBetween || filterType == PivotFilterType.ValueBetween) && !this.IsIntValue(Value2.ToString()))
        throw new ArgumentException("Value2 must be Number");
      if (!this.IsIntValue(Value1.ToString()))
        throw new ArgumentException("Value1 must be Number");
      if (dataField == null)
        throw new ArgumentException("DataField must have values");
    }
    PivotValueLableFilter valueLableFilter = new PivotValueLableFilter();
    valueLableFilter.Value1 = Value1;
    valueLableFilter.Value2 = Value2;
    valueLableFilter.DataField = dataField;
    valueLableFilter.Type = filterType;
    this.ValueFilter = (IPivotValueLableFilter) valueLableFilter;
    PivotTableFields fields = table.Fields;
    int num1 = 0;
    int num2 = 0;
    int i = 0;
    for (int count = table.Fields.Count; i < count; ++i)
    {
      if (table.Fields[i].IsDataField)
      {
        if (i == fields.IndexOf((PivotFieldImpl) dataField))
          num1 = num2;
        ++num2;
      }
    }
    PivotTableFilters pivotTableFilters = new PivotTableFilters();
    if (!(table.Options as PivotTableOptions).IsMultiFieldFilter)
    {
      int index = 0;
      for (int count = table.Filters.Count; index < count; ++index)
      {
        if (table.Filters[index].Field == fields.IndexOf(parent))
          table.Filters.Remove(table.Filters[index]);
      }
    }
    if (table.Filters.Count > 0)
      pivotTableFilters = table.Filters;
    Dictionary<int, PivotItemOptions> itemOptions = parent.ItemOptions;
    if (!(table.Options as PivotTableOptions).IsMultiFieldFilter && itemOptions.Count > 0)
    {
      foreach (KeyValuePair<int, PivotItemOptions> keyValuePair in itemOptions)
      {
        if (keyValuePair.Value != null)
          keyValuePair.Value.IsHidden = false;
      }
    }
    PivotTableFilter pivotTableFilter = new PivotTableFilter();
    pivotTableFilter.Type = filterType;
    pivotTableFilter.EvalOrder = -1;
    pivotTableFilter.FilterId = 2;
    if (dataField != null)
      pivotTableFilter.MeasureFld = num1;
    pivotTableFilter.Field = fields.IndexOf(parent);
    pivotTableFilter.Value1 = Value1;
    if (Value2 != null)
      pivotTableFilter.Value2 = Value2;
    PivotAutoFilter AutoFilter = new PivotAutoFilter();
    AutoFilter.FilterRange = "A1";
    PivotFilterColumn filterColumn = new PivotFilterColumn();
    filterColumn.ColumnId = 0;
    switch (filterType)
    {
      case PivotFilterType.Count:
        PivotTop10Filter pivotTop10Filter = new PivotTop10Filter()
        {
          Value = Convert.ToDouble(Value1)
        };
        pivotTop10Filter.FilterValue = pivotTop10Filter.Value;
        filterColumn.Top10Filters = pivotTop10Filter;
        break;
      case PivotFilterType.yearToDate:
      case PivotFilterType.NextMonth:
      case PivotFilterType.ThisMonth:
      case PivotFilterType.LastMonth:
      case PivotFilterType.NextYear:
      case PivotFilterType.ThisYear:
      case PivotFilterType.LastYear:
      case PivotFilterType.NexWeek:
      case PivotFilterType.ThisWeek:
      case PivotFilterType.LastWeek:
      case PivotFilterType.M1:
      case PivotFilterType.M2:
      case PivotFilterType.M3:
      case PivotFilterType.M4:
      case PivotFilterType.M5:
      case PivotFilterType.M6:
      case PivotFilterType.M7:
      case PivotFilterType.M8:
      case PivotFilterType.M9:
      case PivotFilterType.M10:
      case PivotFilterType.M11:
      case PivotFilterType.M12:
      case PivotFilterType.Q1:
      case PivotFilterType.Q2:
      case PivotFilterType.Q3:
      case PivotFilterType.Q4:
      case PivotFilterType.NextQuarter:
      case PivotFilterType.ThisQuarter:
      case PivotFilterType.LastQuarter:
      case PivotFilterType.Tomorrow:
      case PivotFilterType.Today:
      case PivotFilterType.Yesterday:
        filterColumn.DynamicFilter = new PivotDynamicFilter()
        {
          DateFilterType = PivotFilterCollections.GetDynamicFilter(filterType)
        };
        break;
      default:
        PivotCustomFilters pivotCustomFilters = new PivotCustomFilters();
        pivotCustomFilters.HasAnd = false;
        if (filterType == PivotFilterType.CaptionBetween || filterType == PivotFilterType.ValueBetween)
          pivotCustomFilters.HasAnd = true;
        PivotCustomFilter customFilter1 = new PivotCustomFilter();
        if (filterType != PivotFilterType.CaptionNotBetween && filterType != PivotFilterType.CaptionBetween && filterType != PivotFilterType.ValueNotBetween && filterType != PivotFilterType.ValueBetween)
          customFilter1.FilterOperator = this.GetOperator(filterType);
        else if (filterType == PivotFilterType.CaptionBetween || filterType == PivotFilterType.ValueBetween)
          customFilter1.FilterOperator = FilterOperator2007.GreaterThanOrEqual;
        else if (filterType == PivotFilterType.CaptionNotBetween || filterType == PivotFilterType.ValueNotBetween)
          customFilter1.FilterOperator = FilterOperator2007.LessThan;
        customFilter1.Value = this.GetValue(Value1, filterType);
        pivotCustomFilters.Add(customFilter1);
        if (filterType == PivotFilterType.CaptionNotBetween || filterType == PivotFilterType.CaptionBetween || filterType == PivotFilterType.ValueNotBetween || filterType == PivotFilterType.ValueBetween)
        {
          PivotCustomFilter customFilter2 = new PivotCustomFilter();
          if (filterType == PivotFilterType.CaptionBetween || filterType == PivotFilterType.ValueBetween)
            customFilter2.FilterOperator = FilterOperator2007.LessThanOrEqual;
          else if (filterType == PivotFilterType.CaptionNotBetween || filterType == PivotFilterType.ValueNotBetween)
            customFilter1.FilterOperator = FilterOperator2007.GreaterThan;
          customFilter2.Value = Value2 != null ? this.GetValue(Value2, filterType) : throw new ArgumentException("Value2 is not set");
          pivotCustomFilters.Add(customFilter2);
        }
        filterColumn.CustomFilters = pivotCustomFilters;
        break;
    }
    AutoFilter.Add(filterColumn);
    pivotTableFilter.Add(AutoFilter);
    pivotTableFilters.Add(pivotTableFilter);
    table.Filters = pivotTableFilters;
    return (IPivotValueLableFilter) valueLableFilter;
  }

  internal bool IsIntValue(string Value) => int.TryParse(Value.ToString(), out int _);

  public FilterOperator2007 GetOperator(PivotFilterType filterType)
  {
    FilterOperator2007 filterOperator2007 = FilterOperator2007.Equal;
    switch (filterType)
    {
      case PivotFilterType.CaptionGreaterThan:
      case PivotFilterType.ValueGreaterThan:
        filterOperator2007 = FilterOperator2007.GreaterThan;
        break;
      case PivotFilterType.CaptionGreaterThanOrEqual:
      case PivotFilterType.ValueGreaterThanOrEqual:
        filterOperator2007 = FilterOperator2007.GreaterThanOrEqual;
        break;
      case PivotFilterType.CaptionLessThan:
      case PivotFilterType.ValueLessThan:
        filterOperator2007 = FilterOperator2007.LessThan;
        break;
      case PivotFilterType.CaptionLessThanOrEqual:
      case PivotFilterType.ValueLessThanOrEqual:
        filterOperator2007 = FilterOperator2007.LessThanOrEqual;
        break;
      case PivotFilterType.CaptionNotBeginsWith:
      case PivotFilterType.CaptionNotContains:
      case PivotFilterType.CaptionNotEndsWith:
      case PivotFilterType.CaptionNotEqual:
      case PivotFilterType.ValueNotEqual:
        filterOperator2007 = FilterOperator2007.NotEqual;
        break;
    }
    return filterOperator2007;
  }

  public string GetValue(string Value, PivotFilterType filterType)
  {
    string str1 = Value;
    string str2 = "*";
    switch (filterType)
    {
      case PivotFilterType.CaptionBeginsWith:
      case PivotFilterType.CaptionNotBeginsWith:
        str1 += str2;
        break;
      case PivotFilterType.CaptionContains:
      case PivotFilterType.CaptionNotContains:
        str1 = str2 + str1 + str2;
        break;
      case PivotFilterType.CaptionEndsWith:
      case PivotFilterType.CaptionNotEndsWith:
        str1 = str2 + str1;
        break;
    }
    return str1;
  }

  internal static DynamicFilterType GetDynamicFilter(PivotFilterType filterType)
  {
    switch (filterType)
    {
      case PivotFilterType.yearToDate:
        return DynamicFilterType.YearToDate;
      case PivotFilterType.NextMonth:
        return DynamicFilterType.NextMonth;
      case PivotFilterType.ThisMonth:
        return DynamicFilterType.ThisMonth;
      case PivotFilterType.LastMonth:
        return DynamicFilterType.LastMonth;
      case PivotFilterType.NextYear:
        return DynamicFilterType.NextYear;
      case PivotFilterType.ThisYear:
        return DynamicFilterType.ThisYear;
      case PivotFilterType.LastYear:
        return DynamicFilterType.LastYear;
      case PivotFilterType.NexWeek:
        return DynamicFilterType.NextWeek;
      case PivotFilterType.ThisWeek:
        return DynamicFilterType.ThisWeek;
      case PivotFilterType.LastWeek:
        return DynamicFilterType.LastWeek;
      case PivotFilterType.M1:
        return DynamicFilterType.January;
      case PivotFilterType.M2:
        return DynamicFilterType.February;
      case PivotFilterType.M3:
        return DynamicFilterType.March;
      case PivotFilterType.M4:
        return DynamicFilterType.April;
      case PivotFilterType.M5:
        return DynamicFilterType.May;
      case PivotFilterType.M6:
        return DynamicFilterType.June;
      case PivotFilterType.M7:
        return DynamicFilterType.July;
      case PivotFilterType.M8:
        return DynamicFilterType.August;
      case PivotFilterType.M9:
        return DynamicFilterType.September;
      case PivotFilterType.M10:
        return DynamicFilterType.October;
      case PivotFilterType.M11:
        return DynamicFilterType.November;
      case PivotFilterType.M12:
        return DynamicFilterType.December;
      case PivotFilterType.Q1:
        return DynamicFilterType.Quarter1;
      case PivotFilterType.Q2:
        return DynamicFilterType.Quarter2;
      case PivotFilterType.Q3:
        return DynamicFilterType.Quarter3;
      case PivotFilterType.Q4:
        return DynamicFilterType.Quarter4;
      case PivotFilterType.NextQuarter:
        return DynamicFilterType.NextQuarter;
      case PivotFilterType.ThisQuarter:
        return DynamicFilterType.ThisQuarter;
      case PivotFilterType.LastQuarter:
        return DynamicFilterType.LastQuarter;
      case PivotFilterType.Tomorrow:
        return DynamicFilterType.Tomorrow;
      case PivotFilterType.Today:
        return DynamicFilterType.Today;
      case PivotFilterType.Yesterday:
        return DynamicFilterType.Yesterday;
      default:
        return DynamicFilterType.None;
    }
  }

  public IPivotFilter Add()
  {
    (this.Parent as PivotFieldImpl).m_table.Cache.IsRefreshOnLoad = true;
    PivotFilterImpl pivotFilterImpl = new PivotFilterImpl(this);
    pivotFilterImpl.Value1 = "";
    this.m_pivotFilterImpl.Add(pivotFilterImpl);
    return (IPivotFilter) pivotFilterImpl;
  }

  public IPivotFilter this[int index]
  {
    get
    {
      return this.m_pivotFilterImpl.Count > 0 ? (IPivotFilter) this.m_pivotFilterImpl[index] : (IPivotFilter) null;
    }
  }

  public void Remove(int index) => this.m_pivotFilterImpl.RemoveAt(index);
}
