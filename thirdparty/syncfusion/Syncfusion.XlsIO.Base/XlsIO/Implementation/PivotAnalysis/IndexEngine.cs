// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.IndexEngine
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class IndexEngine
{
  public List<ListIndexInfo> allIndexesRowFirst;
  public List<IComparer> allComparersRowFirst;
  internal int highRowLevel = -1;
  internal List<CoveredCellRange> coveredRanges;
  internal int valColStart;
  internal int valRowStart;
  internal List<object> baseItems = new List<object>();
  internal List<string> names = new List<string>();
  internal Dictionary<int, object> baseItemCollection = new Dictionary<int, object>();
  internal int count;
  internal int key;
  internal int key1;
  internal IEnumerable dataSourceList;
  internal int rowCount;
  internal int columnCount;
  internal bool CalculationRender;
  internal PivotCellInfos CellInfo;
  internal PivotCellInfos RowInfo;
  internal PivotCellInfos ColInfo;
  internal PivotCellInfos pivotInfoCache;
  internal int headerColsCount;
  internal int headerRowsCount;
  internal List<ListIndexInfo> rowIndexes;
  internal List<ListIndexInfo> columnIndexes;
  private int initialRowLoadAmount = 40;
  private int[] parentRowLocation = new int[2];
  private int[] parentColumnLocation = new int[2];
  private List<IndexEngine.SortKey> sortKeys;
  private ListSortDirection calcSortDirection;
  private Dictionary<string, PropertyInfo> lookUp;
  private GetValueDelegate getValue;
  private List<PivotItem> pivotRows;
  private List<PivotItem> pivotColumns;
  private List<PivotComputationInfo> pivotCalculations;
  private FilterHelper filters;
  private object dataSource;
  private bool usePercentageFormat;
  private int rowOffset;
  private int columnOffset;
  private int levelPopulateHeaders = -1;
  private int levelGetListFrom = -1;
  private List<IComparable> calcList;
  private PivotCellInfo lastExpander;
  private PivotEngine pivotEngine;

  public IndexEngine(PivotEngine pivotEngine)
  {
    this.pivotEngine = pivotEngine;
    if (pivotEngine == null)
      throw new NullReferenceException("PivotEngine cannot be null.");
  }

  public IndexEngine(PivotEngine pivotEngine, GetValueDelegate del)
  {
    this.pivotEngine = pivotEngine;
    if (pivotEngine == null)
      throw new NullReferenceException("PivotEngine cannot be null.");
    this.GetValue = del;
  }

  public IndexEngine()
  {
  }

  internal Dictionary<string, PropertyInfo> LookUp
  {
    get => this.lookUp;
    set => this.lookUp = value;
  }

  public int HighRowLevel => this.highRowLevel;

  public int InitialRowLoadAmount
  {
    get => this.initialRowLoadAmount;
    set => this.initialRowLoadAmount = value;
  }

  public GetValueDelegate GetValue
  {
    get
    {
      if (this.getValue == null)
        this.getValue = new GetValueDelegate(this.GetReflectedValue);
      return this.getValue;
    }
    set => this.getValue = value;
  }

  public List<PivotItem> PivotRows
  {
    get
    {
      if (this.pivotEngine != null)
        return this.pivotEngine.PivotRows;
      if (this.pivotRows == null)
        this.pivotRows = new List<PivotItem>();
      return this.pivotRows;
    }
  }

  public List<PivotItem> PivotColumns
  {
    get
    {
      if (this.pivotEngine != null)
        return this.pivotEngine.PivotColumns;
      if (this.pivotColumns == null)
        this.pivotColumns = new List<PivotItem>();
      return this.pivotColumns;
    }
  }

  public List<PivotComputationInfo> PivotCalculations
  {
    get
    {
      if (this.pivotEngine != null)
        return this.pivotEngine.PivotCalculations;
      if (this.pivotCalculations == null)
        this.pivotCalculations = new List<PivotComputationInfo>();
      return this.pivotCalculations;
    }
  }

  public List<FilterExpression> Filters
  {
    get
    {
      if (this.pivotEngine != null)
        return this.pivotEngine.Filters;
      if (this.filters == null)
        this.filters = new FilterHelper();
      return this.filters.filterExpressions;
    }
  }

  public object DataSource
  {
    get => this.dataSource;
    set
    {
      if (this.dataSource == value)
        return;
      this.dataSource = value;
      this.dataSourceList = (IEnumerable) null;
    }
  }

  public List<CoveredCellRange> CoveredRanges
  {
    get
    {
      if (this.coveredRanges == null)
        this.coveredRanges = new List<CoveredCellRange>();
      return this.coveredRanges;
    }
  }

  public bool UsePercentageFormat
  {
    get => this.usePercentageFormat;
    set => this.usePercentageFormat = value;
  }

  public int RowCount => this.rowCount;

  public int ColumnCount => this.columnCount;

  public int ColumnOffSetToValues => this.columnOffset;

  public int RowOffSetToValues => this.rowOffset;

  public ListSortDirection SortDirection => this.calcSortDirection;

  internal List<object> PivotBaseRowItems { get; set; }

  internal List<object> PivotBaseColumnItems { get; set; }

  internal List<string> UsedRowColumnPivots { get; set; }

  internal IEnumerable DataSourceList
  {
    get
    {
      if (this.dataSourceList == null)
      {
        if (this.DataSource is IEnumerable)
          this.dataSourceList = this.DataSource as IEnumerable;
        else if (this.DataSource is DataTable)
          this.dataSourceList = (IEnumerable) ((DataTable) this.DataSource).DefaultView;
      }
      return this.dataSourceList;
    }
    set => this.dataSourceList = value;
  }

  public PivotCellInfo this[int row, int col]
  {
    get
    {
      if (row < 0 || row >= this.rowCount)
        throw new ArgumentException("row outside of valid range.");
      if (col < 0 || col >= this.columnCount)
        throw new ArgumentException("col outside of valid range.");
      if (this.highRowLevel > -1 && row > this.highRowLevel)
      {
        for (int i = this.highRowLevel + 1; i <= row; ++i)
          this.ProcessIJ(i);
        this.highRowLevel = row;
      }
      return this.sortKeys != null && col >= this.PivotRows.Count - 1 ? this.pivotInfoCache[this.sortKeys[row].Index, col] : this.pivotInfoCache[row, col];
    }
  }

  public bool IndexData() => this.IndexData(false);

  public bool IndexData(bool onDemand)
  {
    if (onDemand)
      this.highRowLevel = 0;
    DateTime now = DateTime.Now;
    this.sortKeys = (List<IndexEngine.SortKey>) null;
    bool flag = this.DataSourceList == null || (this.PivotColumns == null || this.PivotColumns.Count == 0) && (this.PivotRows == null || this.PivotRows.Count == 0) && (this.PivotCalculations == null || this.PivotCalculations.Count == 0);
    if (!flag)
    {
      this.headerColsCount = this.PivotColumns.Count + (this.PivotColumns.Count == 0 || this.PivotCalculations.Count > 1 ? 1 : 0);
      this.headerRowsCount = this.PivotRows.Count + (this.PivotColumns.Count <= 0 || this.PivotRows.Count != 0 || this.PivotCalculations.Count <= 0 ? 0 : 1);
      if (this.PivotColumns.Count == 0 && this.PivotRows.Count == 0 && this.PivotCalculations.Count > 0)
      {
        this.headerColsCount = this.PivotCalculations.Count;
        this.headerRowsCount = 1;
      }
      this.rowOffset = this.headerColsCount + 1;
      this.columnOffset = this.headerRowsCount + 1;
      List<object> objectList1;
      if (this.DataSourceList is IEnumerable<object>)
      {
        objectList1 = new List<object>(this.DataSourceList as IEnumerable<object>);
      }
      else
      {
        objectList1 = new List<object>();
        foreach (object dataSource in this.DataSourceList)
          objectList1.Add(dataSource);
      }
      List<string> stringList1 = new List<string>();
      List<string> formats = new List<string>();
      List<IComparer> comparers = new List<IComparer>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = new List<string>();
      List<IComparer> comparerList1 = new List<IComparer>();
      List<string> stringList4 = new List<string>();
      List<string> stringList5 = new List<string>();
      List<IComparer> comparerList2 = new List<IComparer>();
      this.UsedRowColumnPivots = new List<string>();
      if (this.PivotRows != null && this.PivotRows.Count > 0)
      {
        stringList4.AddRange(this.PivotRows.Select<PivotItem, string>((System.Func<PivotItem, string>) (pi => pi.FieldMappingName)));
        stringList5.AddRange(this.PivotRows.Select<PivotItem, string>((System.Func<PivotItem, string>) (pi => pi.Format == null || pi.Format.Length <= 0 ? (string) null : $"{{0:{pi.Format}}}")));
        comparerList2.AddRange(this.PivotRows.Select<PivotItem, IComparer>((System.Func<PivotItem, IComparer>) (pi => pi.Comparer)));
        stringList1.AddRange((IEnumerable<string>) stringList4);
        formats.AddRange((IEnumerable<string>) stringList5);
        comparers.AddRange((IEnumerable<IComparer>) comparerList2);
        this.UsedRowColumnPivots.AddRange(this.PivotRows.Select<PivotItem, string>((System.Func<PivotItem, string>) (pi => pi.FieldMappingName)));
      }
      if (this.PivotColumns != null && this.PivotColumns.Count > 0)
      {
        stringList2.AddRange(this.PivotColumns.Select<PivotItem, string>((System.Func<PivotItem, string>) (pi => pi.FieldMappingName)));
        stringList3.AddRange(this.PivotColumns.Select<PivotItem, string>((System.Func<PivotItem, string>) (pi => pi.Format == null || pi.Format.Length <= 0 ? (string) null : $"{{0:{pi.Format}}}")));
        comparerList1.AddRange(this.PivotColumns.Select<PivotItem, IComparer>((System.Func<PivotItem, IComparer>) (pi => pi.Comparer)));
        stringList1.AddRange((IEnumerable<string>) stringList2);
        formats.AddRange((IEnumerable<string>) stringList3);
        comparers.AddRange((IEnumerable<IComparer>) comparerList1);
        this.UsedRowColumnPivots.AddRange(this.PivotColumns.Select<PivotItem, string>((System.Func<PivotItem, string>) (pi => pi.FieldMappingName)));
      }
      List<object> objectList2 = new List<object>((IEnumerable<object>) objectList1);
      objectList2.Sort((IComparer<object>) new SortComparer(stringList2, stringList3, comparerList1, this.GetValue));
      this.columnIndexes = this.IndexTheList((IList) objectList2, stringList2, stringList3, comparerList1, false);
      objectList1.Sort((IComparer<object>) new SortComparer(stringList1, formats, comparers, this.GetValue));
      this.rowIndexes = this.IndexTheList((IList) objectList1, stringList4, stringList5, comparerList2, true);
      this.allIndexesRowFirst = this.IndexTheList((IList) objectList1, stringList1, formats, comparers, true);
      this.allComparersRowFirst = comparers;
      this.rowCount = this.GetRowDimension();
      this.columnCount = this.GetColumnDimension();
      this.ProcessCalcs(objectList1, stringList1, this.allIndexesRowFirst);
      this.pivotInfoCache = new PivotCellInfos(this.rowCount, this.columnCount);
      this.PopulateCache();
    }
    return flag;
  }

  public void SortByCalculation(int colIndex)
  {
    this.calcSortDirection = this.calcSortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
    IndexEngine.CalcSortComparer calcSortComparer = new IndexEngine.CalcSortComparer(this.calcSortDirection);
    List<IndexEngine.SortKey> sortKeyList1 = new List<IndexEngine.SortKey>(this.RowCount);
    this.sortKeys = (List<IndexEngine.SortKey>) null;
    for (int row = 0; row < this.RowCount; ++row)
      sortKeyList1.Add(new IndexEngine.SortKey()
      {
        Index = row,
        Key = (IComparable) (this[row, colIndex] != null ? this[row, colIndex].DoubleValue : double.MaxValue)
      });
    int num1 = 0;
    List<IndexEngine.SortKey> sortKeyList2 = new List<IndexEngine.SortKey>();
    while (num1 < this.rowCount)
    {
      int num2 = 0;
      int num3 = num1;
      sortKeyList2.Clear();
      for (; num1 < this.rowCount && this[num1, colIndex] != null && this[num1, colIndex].CellType == PivotCellType.ValueCell; ++num1)
      {
        sortKeyList2.Add(new IndexEngine.SortKey()
        {
          Key = sortKeyList1[num1].Key,
          Index = sortKeyList1[num1].Index
        });
        ++num2;
      }
      if (num2 > 0)
      {
        sortKeyList2.Sort((IComparer<IndexEngine.SortKey>) calcSortComparer);
        for (int index = 0; index < num2; ++index)
          sortKeyList1[index + num3] = sortKeyList2[index];
      }
      while (num1 < this.rowCount && (this[num1, colIndex] == null || this[num1, colIndex].CellType != PivotCellType.ValueCell))
        ++num1;
      if (num3 == num1)
        ++num1;
    }
    this.sortKeys = sortKeyList1;
  }

  internal IComparable GetReflectedValue(object component, string property)
  {
    object reflectedValue = (object) null;
    if (component is DataRowView && this.pivotEngine.ItemProperties != null)
    {
      reflectedValue = this.pivotEngine.ItemProperties[property].GetValue(component);
    }
    else
    {
      if (this.LookUp == null && component != null)
      {
        PropertyInfo[] properties = component.GetType().GetProperties();
        this.LookUp = new Dictionary<string, PropertyInfo>();
        foreach (PropertyInfo propertyInfo in properties)
          this.LookUp.Add(propertyInfo.Name, propertyInfo);
      }
      PropertyInfo propertyInfo1 = (PropertyInfo) null;
      if (this.LookUp != null && this.LookUp.TryGetValue(property, out propertyInfo1))
        reflectedValue = propertyInfo1.GetValue(component, (object[]) null);
    }
    return reflectedValue as IComparable;
  }

  internal List<ListIndexInfo> IndexTheList(
    IList list,
    List<string> properties,
    List<string> formats,
    List<IComparer> comparers,
    bool generateCalcs)
  {
    return this.GetListFrom(list, 0, properties, formats, comparers, 0, list.Count, (ListIndexInfo) null, (IComparable) "");
  }

  internal List<ListIndexInfo> GetListFrom(
    IList list,
    int propertyIndex,
    List<string> properties,
    List<string> formats,
    List<IComparer> comparers,
    int start,
    int count,
    ListIndexInfo parentInfo,
    IComparable parentDisplay)
  {
    ++this.levelGetListFrom;
    List<ListIndexInfo> listFrom = new List<ListIndexInfo>();
    if (properties != null && properties.Count > 0)
    {
      HashSet<IComparable> comparableSet = new HashSet<IComparable>();
      int num1 = 0;
      string format = formats[propertyIndex];
      IComparable comparable1 = (IComparable) null;
      int num2 = Math.Min(start + count, list.Count);
      bool flag1 = this.Filters.Count > 0;
      PropertyInfo[] source = (PropertyInfo[]) null;
      if (list != null && list.Count > 0)
        source = list[0].GetType().GetProperties();
      for (int index = start; index < num2; ++index)
      {
        object component = list[index];
        IComparable comparable2 = this.GetValue(list[index], properties[propertyIndex]);
        if (flag1)
        {
          bool flag2 = true;
          foreach (FilterExpression filter in this.Filters)
          {
            FilterExpression exp = filter;
            if ((this.UsedRowColumnPivots.Any<string>((System.Func<string, bool>) (pi => pi == exp.Name)) ? (exp.Name != properties[propertyIndex] ? 1 : 0) : 0) == 0 && (exp.Format == null || formats[propertyIndex] == null || formats[propertyIndex].Contains(exp.Format)))
            {
              if (source != null && ((IEnumerable<PropertyInfo>) source).Count<PropertyInfo>() > 0)
              {
                foreach (PropertyInfo propertyInfo in source)
                {
                  if (propertyInfo.CanWrite && propertyInfo.Name.Equals(exp.Name) && propertyInfo.GetValue(component, (object[]) null) != null)
                  {
                    string name = propertyInfo.GetValue(component, (object[]) null).GetType().Name;
                    string str1 = propertyInfo.GetValue(component, (object[]) null).ToString();
                    if (str1.StartsWith(" ") || str1.EndsWith(" "))
                    {
                      string str2 = propertyInfo.GetValue(component, (object[]) null).ToString().Trim();
                      if (name.Contains("Int32"))
                        propertyInfo.SetValue(component, (object) Convert.ToInt32(str2), (object[]) null);
                      if (name.Contains("Int64"))
                        propertyInfo.SetValue(component, (object) Convert.ToInt64(str2), (object[]) null);
                      if (name.Contains("Double"))
                        propertyInfo.SetValue(component, (object) Convert.ToDouble(str2), (object[]) null);
                      if (name.Contains("Single") || name.Contains("float"))
                        propertyInfo.SetValue(component, (object) Convert.ToSingle(str2), (object[]) null);
                      if (name.Contains("Decimal"))
                      {
                        propertyInfo.SetValue(component, (object) Convert.ToDecimal(str2), (object[]) null);
                        break;
                      }
                      break;
                    }
                    break;
                  }
                }
              }
              flag2 = (bool) exp.ComputedValue(component);
              if (!flag2)
                break;
            }
          }
          if (!flag2)
            continue;
        }
        if (comparable2 != null && format != null)
          comparable2 = (IComparable) string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) comparable2);
        if (comparable1 == null || comparable2 != null && comparable1.ToString().Equals(comparable2.ToString()))
          ++num1;
        comparable1 = comparable2;
        if (!comparableSet.Contains(comparable2))
        {
          comparableSet.Add(comparable2);
          listFrom.Add(new ListIndexInfo()
          {
            Display = comparable2,
            StartIndex = index,
            Children = new List<ListIndexInfo>(),
            ParentInfo = parentInfo
          });
          if (listFrom.Count > 1)
          {
            listFrom[listFrom.Count - 2].LastIndex = listFrom[listFrom.Count - 2].StartIndex + num1 - 1;
            num1 = 1;
          }
        }
      }
      if (listFrom.Count > 0)
        listFrom[listFrom.Count - 1].LastIndex = listFrom[listFrom.Count - 1].StartIndex + num1 - 1;
      if (propertyIndex < properties.Count - 1)
      {
        foreach (ListIndexInfo parentInfo1 in listFrom)
          parentInfo1.Children = this.GetListFrom(list, propertyIndex + 1, properties, formats, comparers, parentInfo1.StartIndex, parentInfo1.LastIndex - parentInfo1.StartIndex + 1, parentInfo1, parentInfo1.Display);
      }
    }
    --this.levelGetListFrom;
    listFrom.Add(new ListIndexInfo()
    {
      Display = (IComparable) $"{parentDisplay}",
      Type = RowType.Summary,
      Summaries = this.InitSummaries(),
      ParentInfo = parentInfo
    });
    return listFrom;
  }

  internal int GetCount(ListIndexInfo info)
  {
    if (info.Children == null || info.Children.Count == 0)
      return 1;
    int count = 0;
    foreach (ListIndexInfo child in info.Children)
      count += this.GetCount(child);
    return count;
  }

  internal int GetCount(List<ListIndexInfo> list)
  {
    int count = 0;
    if (list != null)
    {
      foreach (ListIndexInfo info in list)
        count += this.GetCount(info);
    }
    return count;
  }

  internal int GetRowDimension()
  {
    if (this.PivotCalculations.Count == 0 && this.PivotColumns.Count == 0 && this.PivotRows.Count == 0)
      return 0;
    if (this.PivotCalculations.Count > 0 && this.PivotColumns.Count == 0 && this.PivotRows.Count == 0)
      return 2;
    return this.PivotCalculations.Count == 0 && this.PivotColumns.Count > 0 && this.PivotRows.Count == 0 ? this.PivotColumns.Count : this.GetCount(this.rowIndexes) + this.rowOffset - 1;
  }

  internal int GetColumnDimension()
  {
    if (this.PivotRows.Count == 0 && this.PivotCalculations.Count == 0 && this.PivotColumns.Count == 0)
      return 0;
    return this.PivotRows.Count > 0 && this.PivotCalculations.Count == 0 && this.PivotColumns.Count == 0 ? this.PivotRows.Count : this.columnOffset + this.GetCount(this.columnIndexes) * Math.Max(1, this.PivotCalculations.Count) - 1;
  }

  private List<SummaryBase> FindCalcFromKeys(
    List<IComparable> keys,
    List<ListIndexInfo> list,
    bool isColSummary)
  {
    ListIndexInfo listIndexInfo1 = (ListIndexInfo) null;
    ListIndexInfo listIndexInfo2 = (ListIndexInfo) null;
    List<ListIndexInfo> listIndexInfoList = list;
    int index1 = 0;
    foreach (IComparable key in keys)
    {
      if (listIndexInfoList != null)
      {
        int index2 = listIndexInfoList.BinarySearch(0, listIndexInfoList.Count - 1, new ListIndexInfo()
        {
          Display = key
        }, (IComparer<ListIndexInfo>) new IndexEngine.IndexSorter(this.allComparersRowFirst[index1]));
        if (index2 > -1)
        {
          listIndexInfo1 = listIndexInfoList[index2];
          listIndexInfo2 = listIndexInfo1;
          listIndexInfoList = listIndexInfo1.Children;
          ++index1;
        }
        else
        {
          listIndexInfo1 = (ListIndexInfo) null;
          break;
        }
      }
      else
      {
        listIndexInfo1 = (ListIndexInfo) null;
        break;
      }
    }
    return listIndexInfo1 != null ? (!listIndexInfo1.Summaries.ToList<SummaryBase>().All<SummaryBase>((System.Func<SummaryBase, bool>) (x => x.GetResult() == null)) || listIndexInfo1.ParentInfo == null ? listIndexInfo1.Summaries : listIndexInfo1.ParentInfo.Summaries) : (listIndexInfo2 != null && isColSummary && listIndexInfo2.Type != RowType.None ? listIndexInfo2.Summaries : (List<SummaryBase>) null);
  }

  private void ProcessCalcs(
    List<object> allListCopy,
    List<string> allPivotsUsed,
    List<ListIndexInfo> indexes)
  {
    foreach (ListIndexInfo index in indexes)
    {
      if (index.Type != RowType.Summary)
        this.ProcessCalcsOnIndexInfo(allListCopy, allPivotsUsed, index);
    }
  }

  private void ProcessCalcsOnList(List<ListIndexInfo> indexes, List<SummaryBase> summaries)
  {
    int index1 = indexes.Count - 1;
    if (index1 <= 0)
      return;
    if (summaries == null || summaries.Count == 0 && this.PivotCalculations.Count > 0)
    {
      foreach (PivotComputationInfo pivotCalculation in this.PivotCalculations)
        summaries.Add(pivotCalculation.Summary.GetInstance());
    }
    for (int index2 = 0; index2 < this.PivotCalculations.Count; ++index2)
    {
      for (int index3 = 0; index3 < index1; ++index3)
        summaries[index2].Combine(indexes[index3].Summaries[index2].GetResult());
    }
    indexes[index1].Summaries = summaries;
  }

  private void ProcessCalcsOnIndexInfo(
    List<object> allListCopy,
    List<string> allPivotsUsed,
    ListIndexInfo info)
  {
    if (info.Summaries == null)
      info.Summaries = this.InitSummaries();
    if (info.Children == null || info.Children.Count == 0)
    {
      for (int startIndex = info.StartIndex; startIndex <= info.LastIndex; ++startIndex)
      {
        int index = 0;
        foreach (SummaryBase summary in info.Summaries)
        {
          object other = (object) this.GetValue(allListCopy[startIndex], this.PivotCalculations[index].FieldName);
          if (other != null)
            summary.Combine(other);
          else if (other == null && allListCopy[startIndex] is DataRowView)
            summary.Combine((allListCopy[startIndex] as DataRowView)[this.PivotCalculations[index].FieldName]);
          ++index;
        }
      }
    }
    else
    {
      foreach (ListIndexInfo child in info.Children)
      {
        if (child.Type != RowType.Summary)
          this.ProcessCalcsOnIndexInfo(allListCopy, allPivotsUsed, child);
      }
    }
  }

  private bool HasNoPivots()
  {
    return this.PivotRows.Count == 0 && this.PivotColumns.Count == 0 && this.PivotCalculations.Count > 0;
  }

  private void PopulateCache()
  {
    this.CoveredRanges.Clear();
    if (this.HasNoPivots())
    {
      this.CoveredRanges.Add(new CoveredCellRange(0, 0, 0, 0));
      this.pivotInfoCache[0, 0] = new PivotCellInfo()
      {
        CellType = PivotCellType.TopLeftCell
      };
      this.pivotInfoCache[0, 0].CellRange = this.CoveredRanges[0];
    }
    else if (this.rowOffset >= 2 && this.columnOffset >= 2)
    {
      int count = this.PivotRows.Count;
      this.pivotInfoCache[0, 0] = new PivotCellInfo()
      {
        CellType = PivotCellType.TopLeftCell,
        CellRange = new CoveredCellRange(0, 0, this.PivotColumns.Count - (this.PivotCalculations.Count > 1 ? 0 : 1), count - 1)
      };
      this.CoveredRanges.Add(new CoveredCellRange(0, 0, this.rowOffset - 2, this.columnOffset - 2));
    }
    int row = this.rowOffset - 1;
    int col1 = 0;
    this.levelPopulateHeaders = -1;
    this.PopulateHeaders(true, this.rowIndexes, this.headerRowsCount, ref row, ref col1, (PivotCellInfo) null);
    row = 0;
    int col2 = this.columnOffset - 1;
    this.levelPopulateHeaders = -1;
    this.PopulateHeaders(false, this.columnIndexes, this.headerColsCount, ref row, ref col2, (PivotCellInfo) null);
    this.PopulateCalculations();
  }

  private void ApplyFormat(int i, int j)
  {
    object obj = (object) null;
    if (this.pivotInfoCache[i, j] != null && this.pivotInfoCache[i, j].Summary != null && this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.NoCalculation)
      obj = this.pivotInfoCache[i, j].Summary.GetResult();
    else if (this.pivotInfoCache[i, j] != null)
      obj = this.pivotInfoCache[i, j].Value;
    string format = $"{{0:{this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].Format}}}";
    if (this.pivotInfoCache[i, j] != null && format.Equals("{0:#.##}") && obj != null && obj.Equals((object) 0.0) && !this.UsePercentageFormat)
      this.pivotInfoCache[i, j].FormattedText = "0.0";
    else if (this.pivotInfoCache[i, j] != null && format.Equals("{0:C}") && obj != null && !this.UsePercentageFormat)
    {
      this.pivotInfoCache[i, j].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, obj);
      if (!string.IsNullOrEmpty(obj.ToString()) && Convert.ToDouble(obj) < 0.0)
        this.pivotInfoCache[i, j].FormattedText = string.Format((IFormatProvider) new CultureInfo(CultureInfo.CurrentUICulture.ToString())
        {
          NumberFormat = {
            CurrencyNegativePattern = 1
          }
        }, "{0:C}", obj);
    }
    else if (this.pivotInfoCache[i, j] != null)
    {
      if (this.UsePercentageFormat)
      {
        if (this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.Index)
        {
          this.pivotInfoCache[i, j].FormattedText = Math.Round((double) obj, 9).ToString((IFormatProvider) CultureInfo.CurrentUICulture);
          this.UsePercentageFormat = false;
        }
        else if (this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.RankLargestToSmallest || this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.RankSmallestToLargest)
        {
          this.pivotInfoCache[i, j].FormattedText = obj.ToString();
          this.UsePercentageFormat = false;
        }
        else
        {
          this.pivotInfoCache[i, j].FormattedText = Convert.ToDouble(obj).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
          this.UsePercentageFormat = false;
        }
      }
      else
        this.pivotInfoCache[i, j].FormattedText = this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.RankLargestToSmallest || this.PivotCalculations[(j - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.RankSmallestToLargest ? obj.ToString() : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, obj);
    }
    if (this.pivotInfoCache[i, j] == null)
      return;
    this.pivotInfoCache[i, j].Format = this.PivotCalculations[j % this.PivotCalculations.Count].Format;
  }

  private void PopulateCalculations()
  {
    this.calcList = new List<IComparable>();
    this.ProcessCalculations();
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
    {
      if (this.PivotCalculations[index].CalculationType != CalculationType.NoCalculation)
      {
        this.CalculationRender = true;
        break;
      }
    }
    if (!this.CalculationRender)
      return;
    this.CellInfo = new PivotCellInfos(this.rowCount - (this.PivotColumns.Count + 1), this.columnCount - this.PivotRows.Count);
    int rowIndex1 = this.PivotColumns.Count + 1;
    int rowIndex2 = 0;
    while (rowIndex1 < this.rowCount)
    {
      int count = this.PivotRows.Count;
      int colIndex = 0;
      while (count < this.columnCount)
      {
        this.CellInfo[rowIndex2, colIndex] = new PivotCellInfo();
        this.CellInfo[rowIndex2, colIndex] = this.pivotInfoCache[rowIndex1, count];
        ++count;
        ++colIndex;
      }
      ++rowIndex1;
      ++rowIndex2;
    }
    this.RowInfo = new PivotCellInfos(this.rowCount, this.PivotRows.Count);
    this.ColInfo = new PivotCellInfos(this.PivotColumns.Count + 1, this.columnCount);
    for (int rowIndex3 = 0; rowIndex3 < this.rowCount; ++rowIndex3)
    {
      for (int colIndex = 0; colIndex < this.PivotRows.Count; ++colIndex)
      {
        this.RowInfo[rowIndex3, colIndex] = new PivotCellInfo();
        this.RowInfo[rowIndex3, colIndex] = this.pivotInfoCache[rowIndex3, colIndex];
      }
    }
    for (int rowIndex4 = 0; rowIndex4 < this.PivotColumns.Count + 1; ++rowIndex4)
    {
      for (int colIndex = 0; colIndex < this.columnCount; ++colIndex)
      {
        this.ColInfo[rowIndex4, colIndex] = new PivotCellInfo();
        this.ColInfo[rowIndex4, colIndex] = this.pivotInfoCache[rowIndex4, colIndex];
      }
    }
    this.ProcessCalculations();
    this.CalculationRender = false;
  }

  private int GetRowIndentLevel(int row)
  {
    int colIndex = this.columnOffset - 2;
    while (colIndex > 0 && this.pivotInfoCache[row, colIndex] == null)
      --colIndex;
    return colIndex;
  }

  private int GetColIndentLevel(int col)
  {
    int rowIndex = this.rowOffset - 2;
    if (this.PivotCalculations.Count > 1 || this.PivotCalculations.Count == 1 && this.PivotColumns.Count == 0)
      --rowIndex;
    while (rowIndex > 0 && this.pivotInfoCache[rowIndex, col] == null)
      --rowIndex;
    return rowIndex;
  }

  private void ProcessCalculations()
  {
    if (this.PivotCalculations.Count == 0)
      return;
    if (this.HasNoPivots())
    {
      this.HandleNoPivots();
    }
    else
    {
      bool flag = this.PivotColumns.Count == 0 && this.PivotCalculations.Count == 1 || this.PivotCalculations.Count > 1;
      int num1 = this.rowOffset - 2 - (flag ? 1 : 0);
      int num2 = flag ? num1 + 2 : num1 + 1;
      int num3 = this.highRowLevel != 0 || this.initialRowLoadAmount >= this.rowCount ? this.rowCount : this.initialRowLoadAmount;
      for (int i = num2; i < num3; ++i)
      {
        this.ProcessIJ(i);
        this.highRowLevel = i;
      }
    }
  }

  private void ProcessIJ(int i)
  {
    this.valColStart = this.headerRowsCount;
    this.valRowStart = this.headerColsCount + 1;
    bool flag1 = this.PivotColumns.Count == 0 && this.PivotCalculations.Count > 0 && this.PivotRows.Count > 0;
    bool flag2 = this.PivotRows.Count == 0 && this.PivotCalculations.Count > 0 && this.PivotColumns.Count > 0;
    bool flag3 = this.PivotColumns.Count == 0 && this.PivotCalculations.Count == 1 || this.PivotCalculations.Count > 1;
    int num1 = Math.Max(1, this.PivotCalculations.Count);
    int colIndex1 = this.columnOffset - 2;
    int rowIndex1 = this.rowOffset - 2 - (flag3 ? 1 : 0);
    bool flag4 = colIndex1 == -1 || this.pivotInfoCache[i, colIndex1] == null || i == this.rowCount - 1;
    this.calcList.Clear();
    for (PivotCellInfo parentCell = colIndex1 >= 0 ? this.pivotInfoCache[i, colIndex1] : (PivotCellInfo) null; parentCell != null; parentCell = parentCell.ParentCell)
      this.calcList.Insert(0, parentCell.Value as IComparable);
    int index1 = this.calcList.Count;
    for (int index2 = colIndex1 + 1; index2 < this.columnCount; ++index2)
    {
      bool IsSummaryRow1 = flag4;
      int index3 = (index2 - (this.columnOffset - 1)) % Math.Max(this.PivotCalculations.Count, 1);
      PivotComputationInfo pivotCalculation = this.PivotCalculations.Count == 0 ? (PivotComputationInfo) null : this.PivotCalculations[(index2 - (this.columnOffset - 1)) % this.PivotCalculations.Count];
      bool IsSummaryColumn1 = rowIndex1 == -1 || this.pivotInfoCache[rowIndex1, index2 - index3] == null || index2 - index3 == this.columnCount - num1;
      PivotCellInfo pivotCellInfo1;
      if (IsSummaryRow1 && !flag2)
      {
        PivotCellInfo pivotCellInfo2 = new PivotCellInfo();
        bool flag5 = index2 >= this.columnCount - num1 && this.PivotColumns.Count > 0 || i == this.rowCount - 1;
        pivotCellInfo2.CellType = (PivotCellType) (1 | (flag5 ? 256 /*0x0100*/ : 16 /*0x10*/));
        List<SummaryBase> summaryBaseList = this.InitSummaries();
        pivotCellInfo1 = colIndex1 >= 0 ? this.pivotInfoCache[i, colIndex1] : (PivotCellInfo) null;
        PivotCellInfo pivotCellInfo3 = colIndex1 <= 0 ? (PivotCellInfo) null : this.pivotInfoCache[i, colIndex1 - 1];
        if (pivotCellInfo3 == null || (pivotCellInfo3.CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
        {
          int colIndex2 = colIndex1 - 2;
          while (pivotCellInfo3 == null && colIndex2 >= 0)
            pivotCellInfo3 = this.pivotInfoCache[i, colIndex2--];
          int num2 = i - 1;
          bool IsSummaryRow2 = colIndex1 < 0 || this.pivotInfoCache[num2, colIndex1] == null;
          int rowIndentLevel = this.GetRowIndentLevel(num2);
          bool flag6 = false;
          while (!flag6 && num2 > rowIndex1 && (colIndex2 < 0 || this.pivotInfoCache[num2, colIndex2] == null))
          {
            PivotCellInfo pivotCellInfo4 = this.pivotInfoCache[num2, index2];
            if (pivotCellInfo4 != null)
            {
              foreach (SummaryBase summaryBase in summaryBaseList)
              {
                if (pivotCellInfo4 != null && pivotCellInfo4.Summary != null && pivotCellInfo4.Summary.ToString() == summaryBase.ToString())
                {
                  if (pivotCellInfo4.Summary != null)
                    summaryBase.CombineSummary(pivotCellInfo4.Summary);
                  else
                    summaryBase.Combine((object) pivotCellInfo4.DoubleValue);
                }
              }
              if (IsSummaryRow2)
              {
                PivotCellInfo pivotCellInfo5 = this.pivotInfoCache[num2, rowIndentLevel];
                if (pivotCellInfo5 != null && pivotCellInfo5.ParentCell != null && pivotCellInfo5.ParentCell.CellRange != null)
                  num2 = pivotCellInfo5.ParentCell.CellRange.Top - 1;
                else
                  --num2;
                flag6 = num2 <= rowIndex1 || rowIndentLevel != this.GetRowIndentLevel(num2);
              }
              else
                --num2;
            }
            else
              --num2;
          }
          if (summaryBaseList.Count > 0)
          {
            pivotCellInfo2.Summary = summaryBaseList[(index2 - (this.columnOffset - 1)) % this.PivotCalculations.Count];
            object result = summaryBaseList[(index2 - (this.columnOffset - 1)) % this.PivotCalculations.Count].GetResult();
            pivotCellInfo2.Value = !this.CalculationRender || this.pivotInfoCache[i, index2] == null ? (result == null ? (object) "" : (object) result.ToString()) : this.CalculateValuesToRenderer(this.pivotInfoCache[i, index2].Value, i, index2, index2 - (this.columnOffset - 1), IsSummaryRow2, IsSummaryColumn1, pivotCalculation, this.PivotRows, this.PivotColumns);
            this.pivotInfoCache[i, index2] = pivotCellInfo2;
            this.ApplyFormat(i, index2);
          }
        }
        else
        {
          int top = pivotCellInfo3.ParentCell != null ? pivotCellInfo3.ParentCell.CellRange.Top : 0;
          int bottom = pivotCellInfo3.ParentCell != null ? pivotCellInfo3.ParentCell.CellRange.Bottom : 0;
          foreach (SummaryBase summaryBase in summaryBaseList)
          {
            for (int rowIndex2 = top; rowIndex2 <= bottom; ++rowIndex2)
            {
              PivotCellInfo pivotCellInfo6 = this.pivotInfoCache[rowIndex2, index2];
              if (pivotCellInfo6 != null && pivotCellInfo6.Summary != null && pivotCellInfo6.Summary.ToString() == summaryBase.ToString())
              {
                if (pivotCellInfo6.Summary != null)
                  summaryBase.CombineSummary(pivotCellInfo6.Summary);
                else
                  summaryBase.Combine((object) pivotCellInfo6.DoubleValue);
              }
            }
          }
          if (summaryBaseList.Count > 0)
          {
            pivotCellInfo2.Summary = summaryBaseList[(index2 - (this.columnOffset - 1)) % this.PivotCalculations.Count];
            object result = summaryBaseList[(index2 - (this.columnOffset - 1)) % this.PivotCalculations.Count].GetResult();
            pivotCellInfo2.Value = !this.CalculationRender || this.pivotInfoCache[i, index2] == null ? (result == null ? (object) "" : (object) result.ToString()) : this.CalculateValuesToRenderer(this.pivotInfoCache[i, index2].Value, i, index2, index2 - (this.columnOffset - 1), IsSummaryRow1, IsSummaryColumn1, pivotCalculation, this.PivotRows, this.PivotColumns);
            this.pivotInfoCache[i, index2] = pivotCellInfo2;
            this.ApplyFormat(i, index2);
          }
        }
      }
      else if (IsSummaryColumn1 && !flag1)
      {
        PivotCellInfo pivotCellInfo7 = new PivotCellInfo();
        bool flag7 = index2 >= this.columnCount - num1 || i == this.rowCount - 1;
        pivotCellInfo7.CellType = (PivotCellType) (1 | (flag7 ? 256 /*0x0100*/ : 16 /*0x10*/));
        SummaryBase instance = this.PivotCalculations[index3].Summary.GetInstance();
        pivotCellInfo1 = rowIndex1 < 0 ? (PivotCellInfo) null : this.pivotInfoCache[rowIndex1, index2];
        PivotCellInfo pivotCellInfo8 = rowIndex1 < 1 ? (PivotCellInfo) null : this.pivotInfoCache[rowIndex1 - 1, index2 - index3];
        if (pivotCellInfo8 == null || (pivotCellInfo8.CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
        {
          int rowIndex3 = rowIndex1 - 2;
          while (pivotCellInfo8 == null && rowIndex3 >= 0)
            pivotCellInfo8 = this.pivotInfoCache[rowIndex3--, index2 - index3];
          int num3 = index2 - num1 - index3;
          bool IsSummaryColumn2 = rowIndex1 >= 0 && num3 >= 0 && this.pivotInfoCache[rowIndex1, num3] == null;
          int colIndentLevel = this.GetColIndentLevel(num3);
          bool flag8 = false;
          while (!flag8 && num3 > colIndex1 && (rowIndex3 < 0 || this.pivotInfoCache[rowIndex3, num3] == null))
          {
            PivotCellInfo pivotCellInfo9 = this.pivotInfoCache[i, num3 + index3];
            if (pivotCellInfo9 != null)
            {
              if (instance == null)
                instance = pivotCellInfo9.Summary.GetInstance();
              if (pivotCellInfo9 != null)
              {
                if (pivotCellInfo9.Summary != null)
                  instance.CombineSummary(pivotCellInfo9.Summary);
                else
                  instance.Combine((object) pivotCellInfo9.DoubleValue);
              }
              if (IsSummaryColumn2)
              {
                PivotCellInfo pivotCellInfo10 = this.pivotInfoCache[colIndentLevel, num3];
                if (pivotCellInfo10 != null && pivotCellInfo10.ParentCell != null && pivotCellInfo10.ParentCell.CellRange != null)
                  num3 = pivotCellInfo10.ParentCell.CellRange.Left - num1;
                else
                  num3 -= num1;
                flag8 = num3 <= colIndex1 || colIndentLevel != this.GetColIndentLevel(num3);
              }
              else
                num3 -= num1;
            }
            else
              num3 -= num1;
          }
          pivotCellInfo7.Summary = instance;
          object result = instance.GetResult();
          pivotCellInfo7.Value = !this.CalculationRender || this.pivotInfoCache[i, index2] == null ? (result == null ? (object) "" : (object) result.ToString()) : this.CalculateValuesToRenderer(this.pivotInfoCache[i, index2].Value, i, index2, index2 - (this.columnOffset - 1), IsSummaryRow1, IsSummaryColumn2, pivotCalculation, this.PivotRows, this.PivotColumns);
          this.pivotInfoCache[i, index2] = pivotCellInfo7;
          this.ApplyFormat(i, index2);
        }
        else if (pivotCellInfo8.ParentCell != null)
        {
          int num4 = pivotCellInfo8.ParentCell.CellRange.Left + index3;
          int num5 = pivotCellInfo8.ParentCell.CellRange.Right + index3;
          int colIndex3 = num4;
          for (int index4 = num4; index4 <= num5; ++index4)
          {
            PivotCellInfo pivotCellInfo11 = this.pivotInfoCache[i, colIndex3];
            if (pivotCellInfo11 != null)
            {
              if (instance == null)
                instance = pivotCellInfo11.Summary.GetInstance();
              if (pivotCellInfo11.Summary != null)
                instance.CombineSummary(pivotCellInfo11.Summary);
              else
                instance.Combine((object) pivotCellInfo11.DoubleValue);
            }
            colIndex3 += num1;
            if (colIndex3 >= this.columnCount)
              break;
          }
          pivotCellInfo7.Summary = instance;
          object result = instance.GetResult();
          pivotCellInfo7.Value = !this.CalculationRender || this.pivotInfoCache[i, index2] == null ? (result == null || result.ToString() == "0" ? (object) "" : (object) result.ToString()) : this.CalculateValuesToRenderer(this.pivotInfoCache[i, index2].Value, i, index2, index2 - (this.columnOffset - 1), IsSummaryRow1, IsSummaryColumn1, pivotCalculation, this.PivotRows, this.PivotColumns);
          this.pivotInfoCache[i, index2] = pivotCellInfo7;
          this.ApplyFormat(i, index2);
        }
      }
      else
      {
        PivotCellInfo parentCell = rowIndex1 < 0 ? (PivotCellInfo) null : this.pivotInfoCache[rowIndex1, index2 - index3];
        if (flag2)
        {
          this.calcList.Clear();
          index1 = 0;
        }
        for (; parentCell != null && !flag1; parentCell = parentCell.ParentCell)
          this.calcList.Insert(index1, parentCell.Value as IComparable);
        List<SummaryBase> calcFromKeys = this.FindCalcFromKeys(this.calcList, this.allIndexesRowFirst, false);
        while (this.calcList.Count > index1)
          this.calcList.RemoveAt(this.calcList.Count - 1);
        if (calcFromKeys != null)
        {
          SummaryBase summaryBase = calcFromKeys[index3];
          object result = summaryBase.GetResult();
          if (result != null)
          {
            PivotCellInfo pivotCellInfo12 = new PivotCellInfo();
            pivotCellInfo12.CellType = PivotCellType.ValueCell;
            if (i == this.rowCount - 1 || index2 >= this.columnCount - num1 && this.PivotColumns.Count > 0)
              pivotCellInfo12.CellType |= PivotCellType.GrandTotalCell;
            pivotCellInfo12.Value = !this.CalculationRender || this.pivotInfoCache[i, index2] == null ? (result == null ? (object) "" : (object) result.ToString()) : this.CalculateValuesToRenderer(this.pivotInfoCache[i, index2].Value, i, index2, index2 - (this.columnOffset - 1), IsSummaryRow1, IsSummaryColumn1, pivotCalculation, this.PivotRows, this.PivotColumns);
            pivotCellInfo12.Summary = summaryBase;
            this.pivotInfoCache[i, index2] = pivotCellInfo12;
            this.ApplyFormat(i, index2);
          }
          else
          {
            PivotCellInfo pivotCellInfo13 = new PivotCellInfo();
            pivotCellInfo13.CellType = PivotCellType.ValueCell;
            if (this.PivotCalculations[(index2 - (this.columnOffset - 1)) % this.PivotCalculations.Count].CalculationType == CalculationType.Formula)
              pivotCellInfo13.Value = this.CalculateFormula(i, index2, index2 - (this.columnOffset - 1), pivotCalculation);
            pivotCellInfo13.Summary = summaryBase;
            this.pivotInfoCache[i, index2] = pivotCellInfo13;
            this.ApplyFormat(i, index2);
          }
          index2 = index2 + 1 - 1;
        }
        else
          this.pivotInfoCache[i, index2] = new PivotCellInfo();
      }
    }
  }

  private object CalculateValuesToRenderer(
    object v,
    int row,
    int column,
    int Offset,
    bool IsSummaryRow,
    bool IsSummaryColumn,
    PivotComputationInfo compInfo,
    List<PivotItem> rowList,
    List<PivotItem> columnList)
  {
    if (this.PivotRows == null || this.PivotColumns == null || this.PivotCalculations == null)
      return (object) null;
    object valuesToRenderer = (object) null;
    object obj1 = (object) null;
    string str1 = string.Empty;
    switch (compInfo.CalculationType)
    {
      case CalculationType.NoCalculation:
        valuesToRenderer = v;
        break;
      case CalculationType.PercentageOfGrandTotal:
        object obj2 = this.CellInfo[this.CellInfo.Count - 1, this.CellInfo[0].Count + (column - this.valColStart % this.PivotCalculations.Count - this.PivotCalculations.Count)].Value;
        valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj2) * 100.0);
        this.usePercentageFormat = true;
        break;
      case CalculationType.PercentageOfColumnTotal:
        object obj3 = this.CellInfo[this.CellInfo.Count - 1, column - this.valColStart].Value;
        valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj3) * 100.0);
        this.usePercentageFormat = true;
        break;
      case CalculationType.PercentageOfRowTotal:
        object obj4 = this.CellInfo[row - (this.valRowStart - 1), this.CellInfo[0].Count + (column - this.valColStart % this.PivotCalculations.Count - this.PivotCalculations.Count)].Value;
        valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj4) * 100.0);
        this.usePercentageFormat = true;
        break;
      case CalculationType.PercentageOfParentColumnTotal:
        object obj5;
        if (IsSummaryColumn)
        {
          this.parentColumnLocation = this.GetNextParentColumnIndex(this.parentColumnLocation[0], column);
          obj5 = this.CellInfo[row - (this.valRowStart - 1), this.parentColumnLocation[1]].Value;
        }
        else
        {
          this.parentColumnLocation = this.GetNextParentColumnIndex(this.ColInfo.Count - (1 + (this.PivotCalculations.Count > 1 ? 1 : 0)), column);
          obj5 = this.CellInfo[row - (this.valRowStart - 1), this.parentColumnLocation[1]].Value;
        }
        valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj5) * 100.0);
        this.usePercentageFormat = true;
        break;
      case CalculationType.PercentageOfParentRowTotal:
        object obj6 = (object) 0;
        object obj7;
        if (IsSummaryRow)
        {
          int parentColumnIndex = 0;
          for (int colIndex = 0; colIndex < this.PivotRows.Count; ++colIndex)
          {
            if (this.RowInfo[row, colIndex] == null)
            {
              parentColumnIndex = colIndex - 1;
              break;
            }
          }
          if (this.PivotRows.Count > 2 && parentColumnIndex > 0)
          {
            this.parentRowLocation = this.GetNextParentRowIndex(row, parentColumnIndex);
            obj7 = this.CellInfo[this.parentRowLocation[0], column - this.valColStart].Value;
          }
          else
          {
            this.parentRowLocation = this.GetNextParentRowIndex(row, this.RowInfo[0].Count - this.PivotRows.Count);
            obj7 = this.CellInfo[this.parentRowLocation[0], column - this.valColStart].Value;
          }
        }
        else
        {
          this.parentRowLocation = this.GetNextParentRowIndex(row, this.RowInfo[0].Count - 1);
          obj7 = this.CellInfo[this.parentRowLocation[0], column - this.valColStart].Value;
        }
        valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj7) * 100.0);
        this.usePercentageFormat = true;
        break;
      case CalculationType.PercentageOfParentTotal:
        if (compInfo.BaseField == null)
          throw new ArgumentNullException("Base Field value should be required.");
        int index1 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
        if (index1 != -1)
        {
          int parentRowIndex = this.GetParentRowIndex(row - (this.valRowStart - 1), index1, IsSummaryRow);
          obj1 = parentRowIndex != -1 ? this.CellInfo[parentRowIndex, column - this.valColStart].Value : (object) null;
        }
        else
        {
          int index2 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
          if (index2 != -1)
          {
            int parentColumnIndex = this.GetParentColumnIndex(index2, column - this.valColStart, IsSummaryColumn);
            obj1 = parentColumnIndex != -1 ? this.CellInfo[row - (this.valRowStart - 1), parentColumnIndex].Value : (object) null;
          }
        }
        if (obj1 != null)
        {
          valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj1) * 100.0);
          this.usePercentageFormat = true;
          break;
        }
        valuesToRenderer = (object) null;
        break;
      case CalculationType.Index:
        object obj8 = this.CellInfo[this.CellInfo.Count - 1, this.CellInfo[0].Count + (column - this.valColStart % this.PivotCalculations.Count - this.PivotCalculations.Count)].Value;
        object obj9 = this.CellInfo[this.CellInfo.Count - 1, column - this.valColStart].Value;
        object obj10 = this.CellInfo[row - (this.valRowStart - 1), this.CellInfo[0].Count + (column - this.valColStart % this.PivotCalculations.Count - this.PivotCalculations.Count)].Value;
        valuesToRenderer = (object) (Convert.ToDouble(v) * Convert.ToDouble(obj8) / (Convert.ToDouble(obj10) * Convert.ToDouble(obj9)));
        this.usePercentageFormat = true;
        break;
      case CalculationType.Formula:
        valuesToRenderer = this.CalculateFormula(row, column, Offset, compInfo);
        break;
      case CalculationType.PercentageOf:
      case CalculationType.DifferenceFrom:
      case CalculationType.PercentageOfDifferenceFrom:
        int rowIndex1 = row - (this.valRowStart - 1);
        int colIndex1 = column - this.valColStart;
        if (compInfo.BaseField == null || compInfo.BaseItem == null)
          throw new ArgumentNullException("Base Field value should be required.");
        this.FindRowColumnItems(rowList, columnList);
        int num1 = 0;
        int index3 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
        object obj11 = (object) null;
        if (index3 != -1)
        {
          int index4 = this.PivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (p => p.FieldName == compInfo.FieldName));
          if (rowIndex1 == 0 && colIndex1 == index4)
          {
            for (int rowIndex2 = this.PivotColumns.Count + 1; rowIndex2 < this.rowCount - 1; ++rowIndex2)
            {
              if (this.pivotInfoCache[rowIndex2, index3 + 1] != null && this.pivotInfoCache[rowIndex2, index3 + 1].Value != null)
              {
                string formattedText = this.pivotInfoCache[rowIndex2, index3 + 1].FormattedText;
                if (this.pivotInfoCache[rowIndex2, index3 + 1].ParentCell.Value.ToString() == compInfo.BaseItem && !this.names.Contains(formattedText))
                  this.names.Add(formattedText);
              }
            }
            this.count = this.names.Count;
          }
          int count = this.count;
          string str2 = string.Empty;
          this.baseItems = this.PivotBaseRowItems[index3] as List<object>;
          int num2 = this.baseItems.FindIndex((Predicate<object>) (f => f.ToString() == compInfo.BaseItem));
          if (this.pivotInfoCache[rowIndex1 + this.PivotColumns.Count + 1, index3] != null)
          {
            str2 = this.pivotInfoCache[rowIndex1 + this.PivotColumns.Count + 1, index3].FormattedText;
          }
          else
          {
            int num3 = row;
            int num4 = 1;
            while (num3 >= 0)
            {
              if (this.pivotInfoCache[rowIndex1 + this.PivotColumns.Count + 1 - num4, index3] != null)
              {
                str2 = this.pivotInfoCache[rowIndex1 + this.PivotColumns.Count + 1 - num4, index3].FormattedText;
                break;
              }
              --num3;
              ++num4;
            }
          }
          if (index3 == this.PivotRows.Count - 1)
          {
            if (row > this.baseItems.Count)
              num2 = this.baseItems.Count + num2 + 1;
            if (this.pivotInfoCache[this.PivotRows.Count - index3 - 1 + num2, colIndex1] != null)
              obj11 = index3 != -1 ? this.CellInfo[this.PivotRows.Count - index3 - 1 + num2, colIndex1].Value : (object) null;
          }
          else if (index3 == 0)
          {
            if (num2 == 0)
            {
              if (rowIndex1 <= count)
                obj11 = index3 == -1 || this.CellInfo[index3 + rowIndex1, colIndex1] == null ? (object) null : this.CellInfo[index3 + rowIndex1, colIndex1].Value;
              else if (this.CellInfo[index3 + rowIndex1 - count - 1, colIndex1] != null)
                obj11 = index3 != -1 ? this.CellInfo[index3 + rowIndex1 - count - 1, colIndex1].Value : (object) null;
            }
            else if (str2.Contains(compInfo.BaseItem))
              obj11 = index3 == -1 || this.CellInfo[index3 + rowIndex1, colIndex1] == null ? (object) null : this.CellInfo[index3 + rowIndex1, colIndex1].Value;
            else if (!str2.Contains("Grand") && this.CellInfo[index3 + rowIndex1 + count, colIndex1] != null)
              obj11 = index3 != -1 ? this.CellInfo[index3 + rowIndex1 + count, colIndex1].Value : (object) null;
          }
        }
        else
        {
          int index5 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
          if (index5 != -1)
          {
            int index6 = this.PivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (p => p.FieldName == compInfo.FieldName));
            if (rowIndex1 == 0 && colIndex1 == index6)
            {
              for (int count = this.PivotRows.Count; count < this.columnCount - 1; count += this.PivotCalculations.Count)
              {
                if (this.pivotInfoCache[index5 + 1, count] != null && this.pivotInfoCache[index5 + 1, count].FormattedText != null)
                {
                  string formattedText = this.pivotInfoCache[index5 + 1, count].FormattedText;
                  if (this.pivotInfoCache[index5 + 1, count].ParentCell != null && this.pivotInfoCache[index5 + 1, count].ParentCell.Value != null && this.pivotInfoCache[index5 + 1, count].ParentCell.Value.ToString().Contains(compInfo.BaseItem) && !this.names.Contains(formattedText))
                    this.names.Add(formattedText);
                }
              }
              this.count = this.names.Count;
            }
          }
          string str3 = string.Empty;
          if (this.pivotInfoCache[index5, this.PivotRows.Count + colIndex1] != null)
          {
            str3 = this.pivotInfoCache[index5, this.PivotRows.Count + colIndex1].FormattedText;
          }
          else
          {
            int num5 = column;
            int num6 = 1;
            while (num5 >= 0)
            {
              if (this.pivotInfoCache[index5, this.PivotRows.Count + colIndex1 - num6] != null)
                str3 = this.pivotInfoCache[index5, this.PivotRows.Count + colIndex1 - num6].FormattedText;
              --num5;
              ++num6;
            }
          }
          this.baseItems = this.PivotBaseColumnItems[index5] as List<object>;
          num1 = this.baseItems.FindIndex((Predicate<object>) (f => f.ToString() == compInfo.BaseItem));
          if (index5 == this.PivotColumns.Count - 1)
          {
            for (int count = this.PivotRows.Count; count < this.columnCount - 1; ++count)
            {
              string formattedText1 = this.pivotInfoCache[index5, count].FormattedText;
              string formattedText2 = this.pivotInfoCache[index5 + 1, count].FormattedText;
              if (formattedText1 == compInfo.BaseItem && formattedText2 == compInfo.FieldName && this.CellInfo[rowIndex1, count - this.PivotRows.Count] != null)
              {
                obj11 = (object) this.pivotInfoCache[rowIndex1, count - this.PivotRows.Count].FormattedText;
                break;
              }
            }
          }
          else if (str3.Contains(compInfo.BaseItem))
          {
            obj11 = index5 == -1 || this.CellInfo[rowIndex1, colIndex1] == null ? (object) (string) null : (object) this.CellInfo[rowIndex1, colIndex1].FormattedText;
            this.baseItemCollection.Add(this.key, obj11);
            ++this.key;
          }
          else if (this.baseItemCollection.Count != 0)
          {
            if (this.key1 >= this.key)
              this.key1 = 0;
            obj11 = this.baseItemCollection[this.key1];
            ++this.key1;
          }
        }
        if (obj11 != null && rowIndex1 != this.rowCount - this.valRowStart)
        {
          if (compInfo.CalculationType == CalculationType.PercentageOf)
          {
            valuesToRenderer = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj11) * 100.0);
            this.UsePercentageFormat = true;
          }
          if (compInfo.CalculationType == CalculationType.DifferenceFrom)
          {
            valuesToRenderer = Convert.ToDouble(v) <= Convert.ToDouble(obj11) ? (object) (Convert.ToDouble(obj11) - Convert.ToDouble(v)) : (object) (Convert.ToDouble(v) - Convert.ToDouble(obj11));
            this.UsePercentageFormat = false;
          }
          if (compInfo.CalculationType == CalculationType.PercentageOfDifferenceFrom)
          {
            valuesToRenderer = Convert.ToDouble(v) <= Convert.ToDouble(obj11) ? (object) ((Convert.ToDouble(obj11) - Convert.ToDouble(v)) / Convert.ToDouble(obj11) * 100.0) : (object) ((Convert.ToDouble(v) - Convert.ToDouble(obj11)) / Convert.ToDouble(obj11) * 100.0);
            this.UsePercentageFormat = true;
            break;
          }
          break;
        }
        valuesToRenderer = (object) null;
        break;
      case CalculationType.RunningTotalIn:
      case CalculationType.PercentageOfRunningTotalIn:
        if (compInfo.BaseField == null)
          throw new ArgumentNullException("Base Field value should be required.");
        int rowIndex3 = row - (this.valRowStart - 1);
        int colIndex2 = column - this.valColStart;
        this.FindRowColumnItems(rowList, columnList);
        int index7 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
        object obj12 = (object) null;
        if (index7 != -1)
        {
          this.baseItems = this.PivotBaseRowItems[this.PivotRows.Count - 1] as List<object>;
          if (index7 == this.PivotRows.Count - 1)
          {
            int rowIndex4 = rowIndex3 != this.baseItems.Count + 1 ? 0 : rowIndex3;
            if (this.CellInfo[rowIndex4, colIndex2] != null)
              obj12 = this.CellInfo[rowIndex4, colIndex2].Value;
          }
          else
          {
            int count = this.baseItems.Count;
            if (rowIndex3 <= count && this.CellInfo[rowIndex3, colIndex2] != null)
              obj12 = this.CellInfo[rowIndex3, colIndex2].Value;
            else if (this.CellInfo[rowIndex3 - count - 1, colIndex2] != null)
              obj12 = this.CellInfo[rowIndex3 - count - 1, colIndex2].Value;
          }
        }
        else
        {
          int index8 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
          this.baseItems = this.PivotBaseColumnItems[index8] as List<object>;
          string str4 = this.baseItems[0].ToString();
          this.names.Clear();
          for (int count = this.PivotRows.Count; count < this.columnCount - 1; ++count)
          {
            string formattedText = this.pivotInfoCache[index8, count].FormattedText;
            if (formattedText.Contains(str4))
              this.names.Add(formattedText);
          }
          int count1 = this.names.Count;
          if (index8 != -1)
          {
            if (index8 == this.PivotColumns.Count - 1)
            {
              int colIndex3 = index8 != count1 + 1 ? colIndex2 : colIndex2;
              if (this.CellInfo[rowIndex3, colIndex3] != null)
                obj12 = this.CellInfo[rowIndex3, colIndex3].Value;
            }
            else if (colIndex2 < count1 && this.CellInfo[rowIndex3, colIndex2] != null)
              obj12 = this.CellInfo[rowIndex3, colIndex2].Value;
            else if (this.CellInfo[rowIndex3, colIndex2 - count1] != null)
              obj12 = this.CellInfo[rowIndex3, colIndex2 - count1].Value;
          }
        }
        if (obj12 != null && rowIndex3 != this.rowCount - this.valRowStart)
        {
          if (compInfo.CalculationType == CalculationType.RunningTotalIn)
          {
            valuesToRenderer = Convert.ToDouble(v) != Convert.ToDouble(obj12) ? (object) (Convert.ToDouble(v) + Convert.ToDouble(obj12)) : (object) Convert.ToDouble(v);
            break;
          }
          valuesToRenderer = Convert.ToDouble(v) != Convert.ToDouble(obj12) ? (object) ((Convert.ToDouble(v) + Convert.ToDouble(obj12)) / Convert.ToDouble(obj12) * 100.0) : (object) (Convert.ToDouble(v) / Convert.ToDouble(obj12) * 100.0);
          this.UsePercentageFormat = true;
          break;
        }
        valuesToRenderer = (object) null;
        break;
      case CalculationType.RankSmallestToLargest:
      case CalculationType.RankLargestToSmallest:
        if (compInfo.BaseField == null)
          throw new ArgumentNullException("Base Field value should be required.");
        int rowIndex5 = row - (this.valRowStart - 1);
        int colIndex4 = column - this.valColStart;
        this.FindRowColumnItems(rowList, columnList);
        int index9 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
        object obj13 = (object) null;
        if (index9 != -1)
        {
          if (index9 != this.PivotRows.Count - 1)
          {
            this.baseItems.Clear();
            string str5 = string.Empty;
            if (this.pivotInfoCache[row, index9] != null)
            {
              str5 = this.pivotInfoCache[row, index9].FormattedText;
            }
            else
            {
              int num7 = row;
              int num8 = 1;
              while (num7 >= 0)
              {
                if (this.pivotInfoCache[row - num8, index9] != null)
                {
                  str5 = this.pivotInfoCache[row - num8, index9].FormattedText;
                  break;
                }
                --num7;
                ++num8;
              }
            }
            string str6 = string.Empty;
            if (this.pivotInfoCache[row, index9 + 1] != null)
              str6 = this.pivotInfoCache[row, index9 + 1].FormattedText;
            if (!str5.Contains("Total"))
            {
              for (int rowIndex6 = this.PivotColumns.Count + 1; rowIndex6 < this.rowCount - 1; ++rowIndex6)
              {
                string str7 = string.Empty;
                if (this.pivotInfoCache[rowIndex6, index9 + 1] != null)
                  str7 = this.pivotInfoCache[rowIndex6, index9 + 1].FormattedText;
                if (str6 == str7 && this.CellInfo[rowIndex6 - (this.PivotColumns.Count + 1), colIndex4] != null)
                  this.baseItems.Add(this.CellInfo[rowIndex6 - (this.PivotColumns.Count + 1), colIndex4].Value);
              }
              this.baseItems.Sort();
              if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                this.baseItems.Reverse();
              obj13 = (object) (this.baseItems.IndexOf(v) + 1);
            }
          }
          else
          {
            this.baseItems.Clear();
            string str8 = string.Empty;
            if (this.pivotInfoCache[row, index9 - 1] != null)
            {
              str8 = this.pivotInfoCache[row, index9 - 1].FormattedText;
            }
            else
            {
              int num9 = row;
              int num10 = 1;
              while (num9 >= 0)
              {
                if (this.pivotInfoCache[row - num10, index9 - 1] != null)
                {
                  str8 = this.pivotInfoCache[row - num10, index9 - 1].FormattedText;
                  break;
                }
                --num9;
                ++num10;
              }
            }
            if (!str8.Contains("Total"))
            {
              for (int rowIndex7 = this.PivotColumns.Count + 1; rowIndex7 < this.rowCount - 1; ++rowIndex7)
              {
                string str9 = string.Empty;
                if (this.pivotInfoCache[rowIndex7, index9] != null)
                {
                  str9 = this.pivotInfoCache[rowIndex7, index9].FormattedText;
                }
                else
                {
                  int num11 = row;
                  int num12 = 1;
                  while (num11 >= 0)
                  {
                    if (this.pivotInfoCache[rowIndex7 - num12, index9] != null)
                    {
                      str8 = this.pivotInfoCache[rowIndex7 - num12, index9].FormattedText;
                      break;
                    }
                    --num11;
                    ++num12;
                  }
                }
                if (str9.Contains(str8) && this.CellInfo[rowIndex7 - (this.PivotColumns.Count + 1), colIndex4] != null)
                  this.baseItems.Add(this.CellInfo[rowIndex7 - (this.PivotColumns.Count + 1), colIndex4].Value);
              }
              this.baseItems.Sort();
              if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                this.baseItems.Reverse();
              obj13 = (object) (this.baseItems.IndexOf(v) + 1);
            }
          }
        }
        else
        {
          int index10 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
          if (index10 != -1)
          {
            List<object> objectList = new List<object>();
            int index11 = this.PivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (x => x.FieldName == compInfo.FieldName));
            if (index10 != this.PivotColumns.Count - 1)
            {
              if (rowIndex5 == this.key)
              {
                this.baseItems.Clear();
                for (int count = this.PivotRows.Count; count < this.columnCount - 1; count += this.PivotCalculations.Count)
                {
                  string str10 = string.Empty;
                  if (this.pivotInfoCache[index10, count] != null)
                  {
                    str10 = this.pivotInfoCache[index10, count].FormattedText;
                  }
                  else
                  {
                    int num13 = column;
                    int num14 = 1;
                    while (num13 >= 0)
                    {
                      if (this.pivotInfoCache[index10, count - num14] != null)
                      {
                        str10 = this.pivotInfoCache[index10, count].FormattedText;
                        break;
                      }
                      --num13;
                      ++num14;
                    }
                  }
                  if (!str10.Contains("Total") && this.CellInfo[rowIndex5, count - index11] != null)
                    this.baseItems.Add(this.CellInfo[rowIndex5, count - index11].Value);
                  else if (this.CellInfo[rowIndex5, count - index11] != null)
                    objectList.Add(this.CellInfo[rowIndex5, count - index11].Value);
                }
                this.key = rowIndex5 + 1;
              }
              if (this.pivotInfoCache[index10, colIndex4] != null && this.pivotInfoCache[index10, colIndex4].ParentCell != null && !this.pivotInfoCache[index10, colIndex4].ParentCell.Value.ToString().Contains("Total"))
              {
                this.baseItems.Sort();
                if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                  this.baseItems.Reverse();
                obj13 = (object) (this.baseItems.IndexOf(v) + 1);
              }
              else
              {
                objectList.Sort();
                if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                  objectList.Reverse();
                obj13 = (object) (objectList.IndexOf(v) + 1);
              }
            }
            else
            {
              this.baseItems.Clear();
              if (this.pivotInfoCache[index10 - 1, colIndex4] != null)
              {
                str1 = this.pivotInfoCache[index10 - 1, colIndex4].FormattedText;
              }
              else
              {
                int num15 = column;
                int num16 = 1;
                while (num15 >= 0)
                {
                  if (this.pivotInfoCache[index10 - 1, colIndex4 - num16] != null)
                  {
                    str1 = this.pivotInfoCache[index10 - 1, colIndex4 - num16].FormattedText;
                    break;
                  }
                  --num15;
                  ++num16;
                }
              }
              if (!str1.Contains("Total"))
              {
                for (int count = this.PivotRows.Count; count < this.columnCount - 1; count += this.PivotCalculations.Count)
                {
                  string str11 = string.Empty;
                  if (this.pivotInfoCache[index10, count] != null)
                  {
                    str11 = this.pivotInfoCache[index10, count].FormattedText;
                  }
                  else
                  {
                    int num17 = column;
                    int num18 = 1;
                    while (num17 >= 0)
                    {
                      if (this.pivotInfoCache[index10, count - num18] != null)
                      {
                        str1 = this.pivotInfoCache[index10, count - num18].FormattedText;
                        break;
                      }
                      --num17;
                      ++num18;
                    }
                  }
                  if (str11.Contains(str1) && this.CellInfo[row, count - index11] != null)
                    this.baseItems.Add(this.CellInfo[row, count - index11].Value);
                }
                this.baseItems.Sort();
                if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                  this.baseItems.Reverse();
                obj13 = (object) (this.baseItems.IndexOf(v) + 1);
              }
            }
          }
        }
        if (obj13 != null)
        {
          valuesToRenderer = obj13;
          this.UsePercentageFormat = true;
          break;
        }
        valuesToRenderer = obj13 != null || v == null ? (object) null : (object) Convert.ToDouble(v);
        break;
    }
    return valuesToRenderer;
  }

  private int GetParentRowIndex(int currentRow, int column, bool isSummaryRow)
  {
    if (column == this.PivotRows.Count - 1)
      return currentRow;
    for (int rowIndex = currentRow + (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0)); rowIndex < this.RowInfo.Count && (this.RowInfo[rowIndex, column] != null || !isSummaryRow); ++rowIndex)
    {
      if (this.RowInfo[rowIndex, column] != null && this.RowInfo[rowIndex, column].Value != null && (this.RowInfo[rowIndex, column].Value.ToString().Contains("Total") || this.RowInfo[rowIndex, column].Value.ToString().Contains("Grand")))
        return rowIndex - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0));
    }
    return -1;
  }

  private int[] GetNextParentRowIndex(int currentRowIndex, int parentColumnIndex)
  {
    int[] nextParentRowIndex = new int[2];
    for (int rowIndex = currentRowIndex + 1; rowIndex < this.RowInfo.Count; ++rowIndex)
    {
      if (parentColumnIndex > 0)
      {
        for (int colIndex = parentColumnIndex - 1; colIndex >= 0; --colIndex)
        {
          if (this.RowInfo[rowIndex, colIndex] != null && this.RowInfo[rowIndex, colIndex].Value != null && this.RowInfo[rowIndex, colIndex].Value.ToString().Contains("Total"))
          {
            nextParentRowIndex[0] = rowIndex - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0));
            nextParentRowIndex[1] = colIndex;
            return nextParentRowIndex;
          }
        }
      }
      else if (this.RowInfo[rowIndex, parentColumnIndex] != null && this.RowInfo[rowIndex, parentColumnIndex].Value != null && this.RowInfo[rowIndex, parentColumnIndex].Value.ToString().Contains("Grand"))
      {
        nextParentRowIndex[0] = rowIndex - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0));
        nextParentRowIndex[1] = parentColumnIndex;
        return nextParentRowIndex;
      }
    }
    nextParentRowIndex[0] = currentRowIndex - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0));
    return nextParentRowIndex;
  }

  private int GetParentColumnIndex(int row, int currentColumn, bool isSummaryColumn)
  {
    if (row == this.PivotColumns.Count - 1)
      return currentColumn;
    for (int colIndex = currentColumn + this.PivotRows.Count; colIndex < this.ColInfo[0].Count; ++colIndex)
    {
      if (this.ColInfo[row, colIndex] == null && isSummaryColumn)
        return colIndex - this.PivotRows.Count;
      if (this.ColInfo[row, colIndex] != null && this.ColInfo[row, colIndex].Value != null && (this.ColInfo[row, colIndex].Value.ToString().Contains("Total") || this.ColInfo[row, colIndex].Value.ToString().Contains("Grand")))
        return colIndex - this.PivotRows.Count + currentColumn % this.PivotCalculations.Count;
    }
    return -1;
  }

  private int[] GetNextParentColumnIndex(int parentRowIndex, int currentColumnIndex)
  {
    int[] parentColumnIndex = new int[2];
    for (int colIndex = currentColumnIndex + 1; colIndex < this.ColInfo[0].Count; ++colIndex)
    {
      if (parentRowIndex > 0)
      {
        for (int rowIndex = parentRowIndex - 1; rowIndex >= 0; --rowIndex)
        {
          if (this.ColInfo[rowIndex, colIndex] != null && this.ColInfo[parentRowIndex, colIndex].Value != null && this.ColInfo[rowIndex, colIndex].Value.ToString().Contains("Total"))
          {
            parentColumnIndex[1] = colIndex - (this.PivotRows.Count - (currentColumnIndex - this.PivotRows.Count) % this.PivotCalculations.Count);
            parentColumnIndex[0] = rowIndex;
            return parentColumnIndex;
          }
        }
      }
      else if (this.ColInfo[parentRowIndex, colIndex] != null && this.ColInfo[parentRowIndex, colIndex].Value != null && this.ColInfo[parentRowIndex, colIndex].Value.ToString().Contains("Grand"))
      {
        parentColumnIndex[1] = colIndex - (this.PivotRows.Count - (currentColumnIndex - this.PivotRows.Count) % this.PivotCalculations.Count);
        parentColumnIndex[0] = parentRowIndex;
        return parentColumnIndex;
      }
    }
    parentColumnIndex[1] = currentColumnIndex - this.PivotRows.Count;
    return parentColumnIndex;
  }

  private void FindRowColumnItems(List<PivotItem> rowList, List<PivotItem> columnList)
  {
    List<object> source = new List<object>();
    this.PivotBaseRowItems = new List<object>();
    this.PivotBaseColumnItems = new List<object>();
    int num1;
    foreach (PivotItem pivotRow in this.PivotRows)
    {
      int num2 = this.PivotRows.IndexOf(pivotRow) + 1;
      source.Clear();
      for (int rowIndex = this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0); rowIndex < this.RowCount - 1; ++rowIndex)
      {
        if (this.pivotInfoCache[rowIndex, num2 - 1] != null && this.pivotInfoCache[rowIndex, num2 - 1].FormattedText != null && !this.pivotInfoCache[rowIndex, num2 - 1].FormattedText.Contains("Total") && !source.Contains((object) this.pivotInfoCache[rowIndex, num2 - 1].FormattedText))
          source.Add((object) this.pivotInfoCache[rowIndex, num2 - 1].FormattedText);
      }
      num1 = num2 + 1;
      this.PivotBaseRowItems.Add((object) source.ToList<object>());
    }
    foreach (PivotItem pivotColumn in this.PivotColumns)
    {
      int num3 = this.PivotColumns.IndexOf(pivotColumn) + this.PivotRows.Count + 1;
      source.Clear();
      for (int count = this.PivotRows.Count; count < this.ColumnCount - 1; ++count)
      {
        if (this.pivotInfoCache[num3 - this.PivotRows.Count - 1, count] != null && this.pivotInfoCache[num3 - this.PivotRows.Count - 1, count].FormattedText != null && !this.pivotInfoCache[num3 - this.PivotRows.Count - 1, count].FormattedText.Contains("Total") && !source.Contains((object) this.pivotInfoCache[num3 - this.PivotRows.Count - 1, count].FormattedText))
          source.Add((object) this.pivotInfoCache[num3 - this.PivotRows.Count - 1, count].FormattedText);
      }
      num1 = num3 + 1;
      this.PivotBaseColumnItems.Add((object) source.ToList<object>());
    }
  }

  private object CalculateFormula(int row1, int col1, int col, PivotComputationInfo compInfo)
  {
    Dictionary<string, double> component = new Dictionary<string, double>();
    int colIndex = col1 - col % this.PivotCalculations.Count;
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
    {
      if (this.PivotCalculations[index].CalculationName == null)
        this.PivotCalculations[index].CalculationName = $"Computation{index}";
      if (this.PivotCalculations[index].Formula == null)
        component.Add(this.PivotCalculations[index].FieldName, this.pivotInfoCache[row1, colIndex] == null ? 0.0 : this.pivotInfoCache[row1, colIndex].DoubleValue);
      else
        this.PivotCalculations[index].AllowRunTimeGroupByField = false;
      ++colIndex;
    }
    if (compInfo.Expression == null)
      compInfo.Expression = new FilterExpression(compInfo.CalculationName, compInfo.Formula);
    else
      compInfo.Expression.Expression = compInfo.Formula;
    return compInfo.Expression.ComputedValue((object) component);
  }

  private void HandleNoPivots()
  {
    List<SummaryBase> summaryBaseList = this.InitSummaries();
    foreach (object dataSource in this.DataSourceList)
    {
      for (int index = 0; index < this.PivotCalculations.Count; ++index)
        summaryBaseList[index].Combine((object) this.GetValue(dataSource, this.PivotCalculations[index].FieldName));
    }
    this.pivotInfoCache[1, 0] = new PivotCellInfo()
    {
      CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell,
      Value = (object) "Grand",
      FormattedText = "Grand"
    };
    int colIndex = 1;
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
    {
      this.pivotInfoCache[1, colIndex] = new PivotCellInfo()
      {
        CellType = PivotCellType.ValueCell | PivotCellType.GrandTotalCell,
        Value = summaryBaseList[index].GetResult(),
        Summary = summaryBaseList[index]
      };
      this.ApplyFormat(1, colIndex++);
    }
  }

  private List<SummaryBase> InitSummaries()
  {
    List<SummaryBase> summaryBaseList = new List<SummaryBase>();
    foreach (PivotComputationInfo pivotCalculation in this.PivotCalculations)
      summaryBaseList.Add(pivotCalculation.Summary.GetInstance());
    return summaryBaseList;
  }

  private void PopulateHeaders(
    bool isRowHeaders,
    List<ListIndexInfo> list,
    int depth,
    ref int row,
    ref int col,
    PivotCellInfo parent)
  {
    bool flag = this.PivotColumns.Count == 0 && this.PivotCalculations.Count > 0 || this.PivotCalculations.Count > 1;
    ++this.levelPopulateHeaders;
    if (isRowHeaders)
    {
      col = this.levelPopulateHeaders;
      if (col >= this.columnOffset - 1)
        return;
    }
    else
    {
      if (flag && (row == this.rowOffset - 1 || this.PivotColumns.Count == 0))
      {
        using (List<PivotComputationInfo>.Enumerator enumerator = this.PivotCalculations.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PivotComputationInfo current = enumerator.Current;
            PivotCellInfo pivotCellInfo = new PivotCellInfo()
            {
              CellType = PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell,
              Value = string.IsNullOrEmpty(current.FieldHeader) || current.FieldHeader == null ? (object) current.FieldName : (object) current.FieldHeader
            };
            pivotCellInfo.FormattedText = pivotCellInfo.Value.ToString();
            this.pivotInfoCache[row, col++] = pivotCellInfo;
          }
          return;
        }
      }
      row = this.levelPopulateHeaders;
      if (row >= this.rowOffset - 1)
        return;
    }
    int index = 0;
    int num1 = isRowHeaders ? 1 : Math.Max(1, this.PivotCalculations.Count);
    foreach (ListIndexInfo info in list)
    {
      if (this.levelPopulateHeaders < depth - 1 && info.Children != null && info.Children.Count > 0)
      {
        PivotCellInfo parent1 = this.SetCellInfo(isRowHeaders, this.GetCount(info), ref row, ref col, depth, index, info.Display, index == list.Count - 1, true, parent);
        this.PopulateHeaders(isRowHeaders, info.Children, isRowHeaders ? this.headerRowsCount : this.headerColsCount, ref row, ref col, parent1);
      }
      else if (this.levelPopulateHeaders < depth)
      {
        this.SetCellInfo(isRowHeaders, list.Count, ref row, ref col, depth, index, info.Display, index == list.Count - 1, false, parent);
        if (!isRowHeaders && flag && row <= this.PivotColumns.Count - 1)
        {
          int num2 = row;
          row = this.PivotColumns.Count;
          if (col < this.columnCount - num1 && this.PivotCalculations.Count > 1 && row > 0 && this.pivotInfoCache[row - 1, col] != null)
          {
            this.pivotInfoCache[row - 1, col].CellRange = new CoveredCellRange(row - 1, col, row - 1, col + this.PivotCalculations.Count - 1);
            this.CoveredRanges.Add(this.pivotInfoCache[row - 1, col].CellRange);
          }
          int rowIndex = row - 1;
          while (rowIndex >= 0 && this.pivotInfoCache[rowIndex, col] == null)
            --rowIndex;
          PivotCellType pivotCellType = rowIndex >= 0 && this.pivotInfoCache[rowIndex, col] != null && (this.pivotInfoCache[rowIndex, col].CellType & PivotCellType.TotalCell) != (PivotCellType) 0 ? PivotCellType.TotalCell | PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell : PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell;
          foreach (PivotComputationInfo pivotCalculation in this.PivotCalculations)
          {
            PivotCellInfo pivotCellInfo = new PivotCellInfo()
            {
              CellType = pivotCellType,
              Value = string.IsNullOrEmpty(pivotCalculation.FieldHeader) || pivotCalculation.FieldHeader == null ? (object) pivotCalculation.FieldName : (object) pivotCalculation.FieldHeader
            };
            pivotCellInfo.FormattedText = pivotCellInfo.Value.ToString();
            if (col >= this.columnCount - num1)
              pivotCellInfo.CellType |= PivotCellType.GrandTotalCell;
            this.pivotInfoCache[row, col++] = pivotCellInfo;
          }
          row = num2;
        }
        else if (isRowHeaders)
          row += num1;
        else
          col += num1;
      }
      ++index;
    }
    --this.levelPopulateHeaders;
  }

  private PivotCellInfo SetCellInfo(
    bool isRowHeaders,
    int count,
    ref int row,
    ref int col,
    int depth,
    int index,
    IComparable display,
    bool isLast,
    bool isExpander,
    PivotCellInfo parent)
  {
    int num1 = isRowHeaders ? 1 : Math.Max(1, this.PivotCalculations.Count);
    PivotCellInfo pivotCellInfo = new PivotCellInfo();
    pivotCellInfo.Value = (object) display;
    pivotCellInfo.FormattedText = display != null ? display.ToString() : "";
    pivotCellInfo.CellType = !isRowHeaders ? (row == this.PivotColumns.Count - 1 ? PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell : PivotCellType.ColumnHeaderCell) : (col == this.PivotRows.Count - 1 || this.PivotRows.Count == 0 ? PivotCellType.HeaderCell | PivotCellType.RowHeaderCell : PivotCellType.RowHeaderCell);
    if (isExpander)
    {
      pivotCellInfo.CellType |= PivotCellType.ExpanderCell;
      pivotCellInfo.CellType &= ~PivotCellType.HeaderCell;
      CoveredCellRange coveredCellRange;
      if (isRowHeaders)
      {
        coveredCellRange = new CoveredCellRange()
        {
          Top = row,
          Left = col,
          Bottom = row + (isRowHeaders ? count - 2 : 1),
          Right = col
        };
      }
      else
      {
        int num2 = !isRowHeaders ? (count - 2) * num1 + (this.PivotCalculations.Count > 1 ? this.PivotCalculations.Count - 1 : 0) : 1;
        coveredCellRange = new CoveredCellRange()
        {
          Top = row,
          Left = col,
          Bottom = row,
          Right = col + num2
        };
      }
      pivotCellInfo.CellRange = coveredCellRange;
      this.CoveredRanges.Add(coveredCellRange);
      this.lastExpander = pivotCellInfo;
    }
    if (isLast)
    {
      if (this.lastExpander != null)
      {
        parent = this.lastExpander;
        this.lastExpander = (PivotCellInfo) null;
      }
      pivotCellInfo.CellType |= this.levelPopulateHeaders == 0 ? PivotCellType.GrandTotalCell : PivotCellType.TotalCell;
      if ((pivotCellInfo.CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0)
        pivotCellInfo.CellType &= ~PivotCellType.HeaderCell;
      if (isRowHeaders)
      {
        if (col < this.columnOffset - 1 && (col > 0 || col < this.columnOffset - 2))
        {
          CoveredCellRange coveredCellRange = new CoveredCellRange()
          {
            Top = row,
            Left = col > 0 ? col - 1 : 0,
            Bottom = row,
            Right = this.columnOffset - 2
          };
          this.CoveredRanges.Add(coveredCellRange);
          pivotCellInfo.CellRange = coveredCellRange;
        }
      }
      else
      {
        CoveredCellRange coveredCellRange = new CoveredCellRange()
        {
          Top = row > 0 ? row - 1 : 0,
          Left = col,
          Bottom = this.rowOffset - 2 - (num1 > 1 ? 1 : 0),
          Right = col + num1 - 1
        };
        if (coveredCellRange.Top != coveredCellRange.Bottom || coveredCellRange.Left != coveredCellRange.Right)
        {
          this.CoveredRanges.Add(coveredCellRange);
          pivotCellInfo.CellRange = coveredCellRange;
        }
      }
      if (this.levelPopulateHeaders == 0)
      {
        string str1 = !isRowHeaders || this.PivotRows.Count <= 0 ? (isRowHeaders || this.PivotColumns.Count <= 0 ? string.Empty : this.PivotColumns[0].TotalHeader) : this.PivotRows[0].TotalHeader;
        string str2 = "Grand";
        pivotCellInfo.Value = (object) (str1 + str2);
        pivotCellInfo.FormattedText = str1 == null || str1.Length <= 0 ? str2 : $"{str2} {str1}";
      }
      else
      {
        string str3 = !isRowHeaders || this.PivotRows.Count <= this.levelPopulateHeaders - 1 ? (isRowHeaders || this.PivotColumns.Count <= this.levelPopulateHeaders - 1 ? string.Empty : this.PivotColumns[this.levelPopulateHeaders - 1].TotalHeader) : this.PivotRows[this.levelPopulateHeaders - 1].TotalHeader;
        string str4 = $"{pivotCellInfo.Value}";
        pivotCellInfo.Value = (object) (str4 + str3);
        pivotCellInfo.FormattedText = str3 == null || str3.Length <= 0 ? str4 : $"{str4} {str3}";
      }
      if (isRowHeaders)
      {
        if (col > 0)
          --col;
      }
      else if (row > 0)
        --row;
    }
    if (num1 > 1 && !isLast && row == this.PivotColumns.Count)
    {
      CoveredCellRange coveredCellRange = new CoveredCellRange()
      {
        Top = row > 0 ? row - 1 : 0,
        Left = col,
        Bottom = row > 0 ? row - 1 : 0,
        Right = col + num1 - 1
      };
      if (coveredCellRange.Top != coveredCellRange.Bottom || coveredCellRange.Left != coveredCellRange.Right)
      {
        this.CoveredRanges.Add(coveredCellRange);
        pivotCellInfo.CellRange = coveredCellRange;
      }
    }
    if (col < this.columnCount && row < this.rowCount)
    {
      pivotCellInfo.ParentCell = parent;
      this.pivotInfoCache[row, col] = pivotCellInfo;
    }
    return pivotCellInfo;
  }

  private class CalcSortComparer : IComparer<IndexEngine.SortKey>
  {
    private ListSortDirection dir;

    public CalcSortComparer(ListSortDirection dir) => this.dir = dir;

    public int Compare(IndexEngine.SortKey x, IndexEngine.SortKey y)
    {
      int num = x.Key.CompareTo((object) y.Key);
      return this.dir != ListSortDirection.Descending ? num : -num;
    }
  }

  private class SortKey
  {
    public int Index { get; set; }

    public IComparable Key { get; set; }
  }

  private class IndexSorter : IComparer<ListIndexInfo>
  {
    private IComparer fieldComparer;

    public IndexSorter(IComparer fieldComparer) => this.fieldComparer = fieldComparer;

    public int Compare(ListIndexInfo x, ListIndexInfo y)
    {
      if (this.fieldComparer != null)
        return this.fieldComparer.Compare((object) x.Display, (object) y.Display);
      if (x != null)
        return x.CompareTo(y);
      return y != null ? -y.CompareTo(x) : 0;
    }
  }
}
