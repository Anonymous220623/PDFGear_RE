// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.PivotEngine
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
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class PivotEngine : INotifyPropertyChanged
{
  private const double defaultSummaryValue = 0.0;
  public List<int> sortColIndexes = new List<int>();
  public List<int> columnIndexes;
  public int highWaterRowIndex;
  internal bool? notPopulated = new bool?(false);
  internal double populationStatus;
  private IndexEngine indexEngine;
  private GetValueDelegate getValue;
  private bool useIndexedEngine;
  private bool enableDataOptimization;
  private bool applyFormattedSummary;
  private Dictionary<string, Array> editCellsInfo = new Dictionary<string, Array>();
  private bool showGrandTotals = true;
  private bool isDataDynamic;
  private GridLayout gridLayout;
  private bool showSingleCalculationHeader;
  private bool populationCompleted;
  private bool enableOnDemandCalculations;
  private bool enableLazyLoadOnDemandCalculations;
  private bool showNullAsBlank = true;
  private bool enableSubTotalHiding;
  private bool showSubTotalsForChildren;
  private int rowCount;
  private int columnCount;
  private int grandRowIndex;
  private int grandColumnIndex;
  private List<CoveredCellRange> coveredRanges;
  private FilterHelper filters;
  private Dictionary<string, SummaryBase> summaryLibrary;
  private PivotCellInfos pivotValues;
  private HashSet<PivotCellInfo> hiddenRowIndexes;
  private bool rowPivotsOnly;
  private int initialRowLoadAmount = 40;
  private int highRowLevel = -1;
  private List<ListSortDirection> sortDirs = new List<ListSortDirection>();
  private List<PivotEngine.SortKeys> sortKeys;
  private bool isColumnSorting;
  private ListSortDirection calcSortDirection;
  private ListSortDirection sortDirection;
  private bool ignoreRefresh;
  private List<PivotItem> pivotRows;
  private List<PivotItem> pivotColumns;
  private List<PivotComputationInfo> pivotCalculations;
  private bool useDescriptionInCalculationHeader;
  private bool emptyPivot;
  private string emptyPivotString = "No items in result.";
  private string grandString = "Grand";
  private bool usePercentageFormat = true;
  private bool loadInBackground;
  private bool cacheRawValues;
  private bool showCalculationsAsColumns = true;
  private bool showEmptyCell = true;
  private object dataSource;
  private IEnumerable dataSourceList;
  private PropertyDescriptorCollection itemProperties;
  private List<FieldInfo> allowedFields;
  private int highWaterColumnIndex;
  private FilterItemsCollection itemCollection;
  private List<object> visibleRecords;
  private bool isForRawItem;
  private bool lockComputations;
  private bool ignoreWhitespace;
  private Dictionary<int, List<int>> colSummands;
  private Dictionary<int, List<int>> rowSummands;
  private string delimiter = new string('\u0083', 1);
  private SummaryBase[,] valuesArea;
  private IComparable[,] rowHeaders;
  private IComparable[,] columnHeaders;
  private string[,] rowHeaderUniqueValues;
  private string[,] columnHeaderUniqueValues;
  private bool[] rowSummary;
  private bool[] columnSummary;
  private int[] parentRowLocation = new int[2];
  private int[] parentColLocation = new int[2];
  private BinaryList columnKeysCalcValues;
  private BinaryList rowKeysCalcValues;
  private BinaryList tableKeysCalcValues;
  private Dictionary<string, PropertyInfo> lookUp;
  private int rowOffSet;
  private int colOffSet;

  public event PivotSchemaChangedEventHandler PivotSchemaChanged;

  public event PropertyChangedEventHandler PropertyChanged;

  internal Dictionary<string, PropertyInfo> LookUp
  {
    get => this.lookUp;
    set => this.lookUp = value;
  }

  public IndexEngine IndexEngine
  {
    get => this.indexEngine;
    set => this.indexEngine = value;
  }

  public GetValueDelegate GetValue
  {
    get => this.getValue;
    set => this.getValue = value;
  }

  public bool EnableDataOptimization
  {
    get => this.enableDataOptimization;
    set
    {
      if (this.enableDataOptimization == value)
        return;
      this.enableDataOptimization = value;
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs()
      {
        ChangeHints = SchemaChangeHints.None
      });
    }
  }

  public bool UseIndexedEngine
  {
    get => this.useIndexedEngine;
    set
    {
      if (this.useIndexedEngine == value)
        return;
      this.useIndexedEngine = value;
      if (!this.useIndexedEngine)
      {
        this.indexEngine.DataSourceList = (IEnumerable) null;
        this.indexEngine = (IndexEngine) null;
      }
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs()
      {
        ChangeHints = SchemaChangeHints.None
      });
    }
  }

  public bool ApplyFormattedSummary
  {
    get => this.applyFormattedSummary;
    set => this.applyFormattedSummary = value;
  }

  public Dictionary<string, Array> EditCellsInfo
  {
    get => this.editCellsInfo;
    set => this.editCellsInfo = value;
  }

  public bool IsDataDynamic
  {
    get => this.isDataDynamic;
    set => this.isDataDynamic = value;
  }

  public bool ShowGrandTotals
  {
    get => this.showGrandTotals;
    set
    {
      this.showGrandTotals = value;
      this.CoverGrandTotalRanges();
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs()
      {
        ChangeHints = SchemaChangeHints.GrandTotalVisibility,
        OverrideDeferLayoutUpdate = true
      });
    }
  }

  [DefaultValue(GridLayout.Normal)]
  public GridLayout GridLayout
  {
    get => this.gridLayout;
    set
    {
      this.gridLayout = value;
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
    }
  }

  public bool ShowSingleCalculationHeader
  {
    get => this.showSingleCalculationHeader;
    set
    {
      this.showSingleCalculationHeader = value;
      if (!this.showSingleCalculationHeader)
        return;
      this.EnableOnDemandCalculations = false;
    }
  }

  public bool EnableLazyLoadOnDemandCalculations
  {
    get => this.enableLazyLoadOnDemandCalculations;
    set => this.enableLazyLoadOnDemandCalculations = value;
  }

  public bool EnableOnDemandCalculations
  {
    get
    {
      if (!this.enableOnDemandCalculations || this.RowPivotsOnly || this.PivotCalculations.Count <= 0)
        return false;
      return this.PivotColumns.Count > 0 || this.PivotRows.Count > 0;
    }
    set => this.enableOnDemandCalculations = value;
  }

  public bool EnableSubTotalHiding
  {
    get => this.enableSubTotalHiding;
    set => this.enableSubTotalHiding = value;
  }

  public bool ShowSubTotalsForChildren
  {
    get => this.showSubTotalsForChildren;
    set
    {
      this.showSubTotalsForChildren = value;
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
    }
  }

  public bool ShowNullAsBlank
  {
    get => this.showNullAsBlank;
    set => this.showNullAsBlank = value;
  }

  public int RowCount
  {
    get
    {
      int num = 0;
      if (this.indexEngine != null)
        return !this.ShowGrandTotals ? this.indexEngine.RowCount - (this.ShowCalculationsAsColumns || this.PivotCalculations.Count <= 1 ? 1 : this.PivotCalculations.Count) : this.indexEngine.RowCount;
      if (!this.ShowGrandTotals)
        num = this.PivotCalculations.Count != 0 || this.PivotRows.Count != 0 ? (this.ShowCalculationsAsColumns || this.PivotCalculations.Count <= 1 ? 1 : this.PivotCalculations.Count) : 0;
      return this.rowCount - num;
    }
  }

  public int ColumnCount
  {
    get
    {
      int num1 = 0;
      if (this.indexEngine != null)
      {
        if (this.ShowGrandTotals)
          return this.indexEngine.ColumnCount;
        int num2 = !this.ShowCalculationsAsColumns || this.PivotCalculations.Count <= 1 ? 1 : this.PivotCalculations.Count;
        return this.PivotColumns.Count <= 0 ? this.IndexEngine.ColumnCount : this.indexEngine.ColumnCount - num2;
      }
      if (!this.ShowGrandTotals)
        num1 = this.PivotCalculations.Count != 0 || this.PivotRows.Count != 0 && this.PivotColumns.Count != 0 ? (!this.ShowCalculationsAsColumns || this.PivotCalculations.Count <= 1 ? 1 : this.PivotCalculations.Count) : 0;
      return this.columnCount - num1;
    }
  }

  public List<CoveredCellRange> CoveredRanges
  {
    get
    {
      if (this.indexEngine != null)
        return this.indexEngine.CoveredRanges;
      if (this.coveredRanges == null)
        this.coveredRanges = new List<CoveredCellRange>();
      return this.coveredRanges;
    }
  }

  public List<FilterExpression> Filters
  {
    get
    {
      if (this.filters == null)
        this.filters = new FilterHelper();
      return this.filters.filterExpressions;
    }
    set
    {
      if (this.filters == null)
        this.filters = new FilterHelper();
      if (value == null)
        this.filters.filterExpressions = new List<FilterExpression>();
      else
        this.filters.filterExpressions = value;
    }
  }

  public Dictionary<string, SummaryBase> SummaryLibrary
  {
    get
    {
      if (this.summaryLibrary == null)
      {
        this.summaryLibrary = new Dictionary<string, SummaryBase>();
        this.PopulateDefaultSummaryLibrary();
      }
      return this.summaryLibrary;
    }
  }

  public PivotCellInfos PivotValues => this.pivotValues;

  public HashSet<PivotCellInfo> HiddenRowIndexes
  {
    get
    {
      if (this.hiddenRowIndexes == null)
        this.hiddenRowIndexes = new HashSet<PivotCellInfo>();
      return this.hiddenRowIndexes;
    }
    set => this.hiddenRowIndexes = value;
  }

  public Dictionary<int, List<HiddenGroup>> HiddenPivotRowGroups { get; set; }

  public Dictionary<int, List<HiddenGroup>> HiddenPivotColumnGroups { get; set; }

  public bool RowPivotsOnly
  {
    get => this.rowPivotsOnly;
    set => this.rowPivotsOnly = value;
  }

  public List<PivotItem> PivotRows
  {
    get
    {
      if (this.pivotRows == null)
        this.pivotRows = new List<PivotItem>();
      return this.pivotRows;
    }
    set => this.pivotRows = value;
  }

  public List<PivotItem> PivotColumns
  {
    get
    {
      if (this.pivotColumns == null)
        this.pivotColumns = new List<PivotItem>();
      return this.pivotColumns;
    }
    set => this.pivotColumns = value;
  }

  public List<PivotComputationInfo> PivotCalculations
  {
    get
    {
      if (this.pivotCalculations == null)
        this.pivotCalculations = new List<PivotComputationInfo>();
      return this.pivotCalculations;
    }
    set => this.pivotCalculations = value;
  }

  [DefaultValue(false)]
  public bool UseDescriptionInCalculationHeader
  {
    get => this.useDescriptionInCalculationHeader;
    set => this.useDescriptionInCalculationHeader = value;
  }

  public bool EmptyPivot
  {
    get => this.emptyPivot;
    internal set => this.emptyPivot = value;
  }

  [DefaultValue("No items in result.")]
  public string EmptyPivotString
  {
    get => this.emptyPivotString;
    set => this.emptyPivotString = value;
  }

  [DefaultValue("Grand")]
  public string GrandString
  {
    get => this.grandString;
    set => this.grandString = value;
  }

  public bool UsePercentageFormat
  {
    get => this.usePercentageFormat;
    set => this.usePercentageFormat = value;
  }

  public bool IgnoreWhitespace
  {
    get => this.ignoreWhitespace;
    set => this.ignoreWhitespace = value;
  }

  public bool LoadInBackground
  {
    get => this.loadInBackground;
    set
    {
      this.loadInBackground = value;
      this.OnPropertyChanged(nameof (LoadInBackground));
    }
  }

  public bool CacheRawValues
  {
    get => this.cacheRawValues;
    set => this.cacheRawValues = value;
  }

  public bool ShowCalculationsAsColumns
  {
    get => this.showCalculationsAsColumns;
    set
    {
      if (this.showCalculationsAsColumns == value)
        return;
      this.showCalculationsAsColumns = value;
      if (this.PivotCalculations.Count <= 0)
        return;
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
    }
  }

  [DefaultValue(true)]
  public bool ShowEmptyCells
  {
    get => this.showEmptyCell;
    set => this.showEmptyCell = value;
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
      this.ItemType = (Type) null;
      this.itemProperties = (PropertyDescriptorCollection) null;
      if (this.IndexEngine != null && this.IndexEngine.LookUp != null)
        this.IndexEngine.LookUp = (Dictionary<string, PropertyInfo>) null;
      if (this.LookUp != null)
        this.LookUp = (Dictionary<string, PropertyInfo>) null;
      this.RefreshItemProperties();
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
    }
  }

  public virtual IEnumerable DataSourceList
  {
    get
    {
      if (this.dataSourceList == null)
      {
        if (this.DataSource is IEnumerable && !(this.DataSource is DataTable))
          this.dataSourceList = this.DataSource as IEnumerable;
        else if (this.DataSource is DataTable)
          this.dataSourceList = (IEnumerable) ((DataTable) this.DataSource).DefaultView;
      }
      return this.dataSourceList;
    }
    set => this.dataSourceList = value;
  }

  public Type ItemType { get; set; }

  public PropertyDescriptorCollection ItemProperties
  {
    get
    {
      if (this.IsItemPropertiesFilled && !this.isDataDynamic)
        return this.itemProperties;
      CalculationExtensions.Engine = this;
      if (this.DataSourceList != null || this.ItemType != (Type) null)
      {
        if (this.isDataDynamic)
        {
          IDictionary<string, object> dictionary1 = (IDictionary<string, object>) null;
          IEnumerator enumerator = this.DataSourceList.GetEnumerator();
          try
          {
            if (enumerator.MoveNext())
              dictionary1 = enumerator.Current as IDictionary<string, object>;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
          if (dictionary1 != null)
          {
            List<DynamicPropertyDescriptor> propertyDescriptorList = new List<DynamicPropertyDescriptor>();
            Dictionary<string, Type> dictionary2 = new Dictionary<string, Type>();
            foreach (string key in (IEnumerable<string>) dictionary1.Keys)
            {
              propertyDescriptorList.Add(new DynamicPropertyDescriptor(key, (Attribute[]) null));
              object obj = dictionary1[key];
              if (obj == null)
                dictionary2.Add(key, typeof (object));
              else
                dictionary2.Add(key, obj.GetType());
            }
            this.itemProperties = new PropertyDescriptorCollection((PropertyDescriptor[]) propertyDescriptorList.ToArray());
            CalculationExtensions.DynamicPropertyTypeTable = dictionary2;
            new CalculationExtensionsBackground().DynamicPropertyTypeTable = dictionary2;
          }
        }
        else if (this.dataSourceList is ITypedList)
        {
          this.itemProperties = ((ITypedList) this.dataSourceList).GetItemProperties((PropertyDescriptor[]) null);
        }
        else
        {
          if (this.ItemType == (Type) null)
          {
            if ((this.DataSourceList as IList).Count > 0)
            {
              IEnumerator enumerator = this.DataSourceList.GetEnumerator();
              try
              {
                if (enumerator.MoveNext())
                  this.ItemType = enumerator.Current.GetType();
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            }
            else
            {
              for (int index = 0; index < ((IEnumerable<PropertyInfo>) (this.DataSourceList as IList).GetType().GetProperties()).Count<PropertyInfo>(); ++index)
              {
                if ((this.DataSourceList as IList).GetType().GetProperties()[index].PropertyType.IsClass)
                  this.ItemType = (this.DataSourceList as IList).GetType().GetProperties()[index].PropertyType;
              }
            }
          }
          if (this.ItemType != (Type) null)
            this.itemProperties = TypeDescriptor.GetProperties(this.ItemType);
        }
      }
      List<PropertyDescriptor> propertyDescriptorList1 = new List<PropertyDescriptor>();
      if (this.itemProperties != null)
      {
        foreach (PropertyDescriptor itemProperty in this.itemProperties)
        {
          if (this.AllowedFields.Count != 0)
          {
            if (this.AllowedFields.IndexOf(new FieldInfo()
            {
              Name = itemProperty.Name
            }) <= -1)
            {
              if (!propertyDescriptorList1.Contains(itemProperty))
              {
                propertyDescriptorList1.Add(itemProperty);
                continue;
              }
              continue;
            }
          }
          propertyDescriptorList1.Add(itemProperty);
        }
      }
      foreach (FieldInfo allowedField in this.AllowedFields)
      {
        if (allowedField.FieldType == FieldTypes.Expression && propertyDescriptorList1.IndexOf((PropertyDescriptor) new ExpressionPropertyDescriptor(allowedField.Name, (Attribute[]) null, allowedField.Expression, allowedField.Format, this.filters)) == -1)
          propertyDescriptorList1.Add((PropertyDescriptor) new ExpressionPropertyDescriptor(allowedField.Name, (Attribute[]) null, allowedField.Expression, allowedField.Format, this.filters));
      }
      this.itemProperties = new PropertyDescriptorCollection(propertyDescriptorList1.ToArray());
      if (this.itemProperties != null && this.itemProperties.Count > 0)
        this.IsItemPropertiesFilled = true;
      return this.itemProperties;
    }
    set => this.itemProperties = value;
  }

  public List<FieldInfo> AllowedFields
  {
    get
    {
      if (this.allowedFields == null)
        this.allowedFields = new List<FieldInfo>();
      return this.allowedFields;
    }
  }

  public bool? NotPopulated
  {
    get => this.notPopulated;
    set => this.notPopulated = value;
  }

  public double PopulationStatus
  {
    get => this.populationStatus;
    set => this.populationStatus = value;
  }

  public FilterItemsCollection ItemCollection
  {
    get => this.itemCollection;
    set => this.itemCollection = value;
  }

  public List<object> VisibleRecords
  {
    get => this.visibleRecords;
    set => this.visibleRecords = value;
  }

  public bool LockComputations
  {
    get => this.lockComputations;
    set => this.lockComputations = value;
  }

  public ListSortDirection SortDirection => this.calcSortDirection;

  internal List<object> PivotBaseRowItems { get; set; }

  internal List<object> PivotBaseColumnItems { get; set; }

  internal bool IsItemPropertiesFilled { get; set; }

  internal Dictionary<int, List<int>> ColSummands
  {
    get
    {
      if (this.colSummands == null)
        this.colSummands = new Dictionary<int, List<int>>();
      return this.colSummands;
    }
  }

  internal Dictionary<int, List<int>> RowSummands
  {
    get
    {
      if (this.rowSummands == null)
        this.rowSummands = new Dictionary<int, List<int>>();
      return this.rowSummands;
    }
  }

  private bool IsCalculationHeaderVisible
  {
    get => this.ShowSingleCalculationHeader || this.PivotCalculations.Count > 1;
  }

  private bool okToPopulate => !this.lockComputations && this.DataSourceList != null;

  public PivotCellInfo this[int rowIndex, int columnIndex1]
  {
    get => this.GetPivotEngineValueFor(rowIndex, columnIndex1, true);
  }

  public void RefreshItemProperties() => this.IsItemPropertiesFilled = false;

  public void AddFilter(FilterExpression item)
  {
    if (this == null || this.DataSource == null)
      return;
    IList list = this.DataSource is DataTable || this.DataSource is DataView ? this.DataSourceList as IList : this.DataSource as IList;
    if (list == null)
      return;
    PropertyInfo[] source = (PropertyInfo[]) null;
    if (list.GetEnumerator().MoveNext())
    {
      source = list[0].GetType().GetProperties();
    }
    else
    {
      for (int index = 0; index < ((IEnumerable<PropertyInfo>) list.GetType().GetProperties()).Count<PropertyInfo>(); ++index)
      {
        if (list.GetType().GetProperties()[index].PropertyType.IsClass)
          this.ItemType = list.GetType().GetProperties()[index].PropertyType;
      }
      if (this.ItemType != (Type) null)
        source = this.ItemType.GetProperties();
    }
    PropertyDescriptor propertyDescriptor = source.OfType<PropertyDescriptor>().Cast<PropertyDescriptor>().Where<PropertyDescriptor>((System.Func<PropertyDescriptor, bool>) (p => p.Name == item.DimensionName)).FirstOrDefault<PropertyDescriptor>();
    if (propertyDescriptor == null && this.ItemProperties != null && this.ItemProperties.Count > 0)
      propertyDescriptor = item.DimensionName == null ? this.ItemProperties[item.Name] : this.ItemProperties[item.DimensionName];
    else if (propertyDescriptor == null)
      return;
    this.itemCollection = new FilterItemsCollection()
    {
      FilterProperty = propertyDescriptor,
      Name = item.DimensionName,
      DisplayHeader = item.DimensionHeader,
      FieldCaption = item.FieldCaption
    };
    foreach (object component in (IEnumerable) list)
    {
      if (propertyDescriptor.PropertyType == typeof (int) || propertyDescriptor.PropertyType == typeof (double) || propertyDescriptor.PropertyType == typeof (float) || propertyDescriptor.PropertyType == typeof (Decimal) || propertyDescriptor.PropertyType == typeof (short) || propertyDescriptor.PropertyType == typeof (long) || propertyDescriptor.PropertyType == typeof (DateTime))
        this.itemCollection.AddIfUnique(new FilterItemElement()
        {
          Key = propertyDescriptor.GetValue(component).ToString()
        });
      else
        this.itemCollection.AddIfUnique(new FilterItemElement()
        {
          Key = !(this.DataSource is DataView) ? propertyDescriptor.GetValue(component) as string : propertyDescriptor.GetValue(component).ToString()
        });
    }
    this.itemCollection.Reverse();
  }

  public void InsertFilter(int index, FilterExpression exp)
  {
    this.Filters.Insert(index, exp);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void RemoveFilter(FilterExpression exp)
  {
    if (exp != null)
    {
      if (this.Filters != null)
        this.Filters.Remove(exp);
      if (this.DataSourceList != null && this.DataSourceList is DataView)
      {
        string str = "";
        foreach (FilterExpression filter in this.Filters)
        {
          if (!string.IsNullOrEmpty(str))
            str += " AND ";
          str = $"{str}({filter.Expression})";
        }
        ((DataView) this.DataSourceList).RowFilter = str;
      }
      this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
    }
    else
    {
      if (this.Filters == null)
        return;
      FilterExpression filterExpression = this.Filters.Where<FilterExpression>((System.Func<FilterExpression, bool>) (q => q.Name == this.itemCollection.DisplayHeader)).FirstOrDefault<FilterExpression>();
      if (filterExpression == null)
        return;
      this.Filters.Remove(filterExpression);
    }
  }

  public void ClearFilters()
  {
    this.Filters.Clear();
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public FilterExpression FindFilterByName(string filterName)
  {
    return this.Filters.Where<FilterExpression>((System.Func<FilterExpression, bool>) (x => x.Name == filterName)).FirstOrDefault<FilterExpression>();
  }

  public PivotCellInfo GetPivotEngineValueFor(
    int rowIndex,
    int columnIndex1,
    bool shouldCalculateTotal)
  {
    int num = !this.rowPivotsOnly || this.columnIndexes == null || columnIndex1 >= this.columnIndexes.Count ? columnIndex1 : this.columnIndexes[columnIndex1];
    bool? notPopulated = this.notPopulated;
    if ((notPopulated.GetValueOrDefault() ? 1 : (!notPopulated.HasValue ? 1 : 0)) != 0)
      return new PivotCellInfo();
    if (rowIndex < 0 || rowIndex >= this.rowCount)
      throw new ArgumentOutOfRangeException("RowIndex out of range.");
    if (num < 0 || num >= this.columnCount)
      throw new ArgumentOutOfRangeException("ColumnIndex out of range.");
    if (this.EnableDataOptimization && this.highRowLevel > -1 && rowIndex > this.initialRowLoadAmount)
    {
      bool showGrandTotals = this.ShowGrandTotals;
      if (!this.ShowCalculationsAsColumns && this.PivotCalculations.Count > 0)
      {
        this.showGrandTotals = true;
        this.SwapRowsColumns(true);
      }
      this.DoCalculationTable(rowIndex);
      this.initialRowLoadAmount = rowIndex;
      this.ProcessTotals3();
      this.PopulateRow(rowIndex);
      this.CoverRowHeader(rowIndex);
      if ((this.PivotRows.Any<PivotItem>((System.Func<PivotItem, bool>) (x => !x.ShowSubTotal)) || this.PivotColumns.Any<PivotItem>((System.Func<PivotItem, bool>) (x => !x.ShowSubTotal)) || this.EnableOnDemandCalculations || !this.ShowCalculationsAsColumns) && (this.GridLayout == GridLayout.TopSummary || this.GridLayout == GridLayout.ExcelLikeLayout))
      {
        this.ReArrangePivotValuesForRows();
        if (this.GridLayout == GridLayout.TopSummary)
          this.ReArrangePivotValuesForColumn();
      }
      if (!this.ShowCalculationsAsColumns && this.PivotCalculations.Count > 0)
      {
        this.TransposePivotTable();
        this.SwapRowsColumns(false);
        if (!showGrandTotals)
          this.showGrandTotals = showGrandTotals;
      }
      this.RemoveDelimeter();
    }
    if (this.indexEngine != null)
    {
      PivotCellInfo pivotEngineValueFor = this.indexEngine[rowIndex, num];
      if (pivotEngineValueFor != null)
      {
        pivotEngineValueFor.RowIndex = rowIndex;
        pivotEngineValueFor.ColumnIndex = num;
      }
      if (pivotEngineValueFor != null)
        return pivotEngineValueFor;
      return new PivotCellInfo()
      {
        RowIndex = rowIndex,
        ColumnIndex = num
      };
    }
    if (this.EnableOnDemandCalculations && rowIndex >= (this.showCalculationsAsColumns ? this.rowOffSet : this.PivotRows.Count) && num >= (this.ShowCalculationsAsColumns ? this.colOffSet : this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0)) && this.populationCompleted)
      this.EnsureCalculationAt(rowIndex, num, shouldCalculateTotal);
    return this.sortKeys != null && rowIndex < this.sortKeys.Count && num >= this.PivotRows.Count - 1 ? this.pivotValues[this.sortKeys[rowIndex].Index, num] : this.pivotValues[rowIndex, num];
  }

  public void EnsureCalculationsLoaded()
  {
    if (!this.EnableOnDemandCalculations)
      return;
    while (this.highWaterRowIndex < this.RowCount - 1)
      this.DoLazyCalculation();
  }

  public bool DoLazyCalculation()
  {
    if (!this.EnableOnDemandCalculations || this.UseIndexedEngine)
      return true;
    if (this.highWaterRowIndex < this.RowCount - 1)
    {
      for (int colOffSet = this.colOffSet; colOffSet < this.ColumnCount - 1; ++colOffSet)
        this.EnsureCalculationAt(this.highWaterRowIndex, colOffSet, true);
      ++this.highWaterRowIndex;
    }
    return this.highWaterRowIndex >= this.RowCount - 1;
  }

  public void AddAllowedField(FieldInfo fi)
  {
    if (this.AllowedFields != null)
    {
      if (this.AllowedFields.IndexOf(new FieldInfo()
      {
        Name = fi.Name
      }) == -1)
      {
        this.AllowedFields.Add(fi);
        this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
        return;
      }
    }
    throw new ArgumentException($"{fi.Name} already in collection.");
  }

  public List<object> GetVisibleRecords(List<FilterExpression> Filters)
  {
    List<object> visibleRecords = new List<object>();
    IEnumerable dataSourceList = this.DataSourceList;
    foreach (FilterExpression filter in Filters)
      filter.Expression = (filter.Tag as FilterItemsCollection).GetExpressionForVisibleRecords(this.DataSource is IEnumerable);
    foreach (object component in dataSourceList)
    {
      flag = true;
      foreach (FilterExpression filter in Filters)
      {
        if (!(filter.ComputedValue(component) is bool flag))
          ;
        if (!flag)
          break;
      }
      if (flag && !visibleRecords.Contains(component))
        visibleRecords.Add(component);
    }
    return visibleRecords;
  }

  public List<IComparable> GetRowColumnPivotValuesAt(int row, int column, out string calcFieldName)
  {
    calcFieldName = this.GetCalculationNameFromGridRowColumnIndex(column);
    return this.GetKeyAt(row, column);
  }

  public List<object> GetRawItemsForEach(int row, int col)
  {
    if (this.PivotCalculations.Count <= 1 && row > 0 && this.PivotColumns.Count == 0)
      --row;
    List<object> rawItemsForEach = new List<object>();
    List<int> intList = new List<int>();
    List<List<IComparable>> pivotValuesCollection = new List<List<IComparable>>();
    string unused = "";
    if (this.ShowCalculationsAsColumns && row >= 0 && this.PivotRows.Count != 0 && this.PivotColumns.Count != 0 && this.rowHeaders[row, 0] != null && this.rowHeaders[row, 0].ToString().Contains("Total"))
    {
      for (int rowIndex = this.PivotColumns.Count + 1; rowIndex < row; ++rowIndex)
      {
        int columnIndex1 = this.PivotRows.Count - 1;
        PivotCellInfo pivotCellInfo = this[rowIndex, columnIndex1];
        if (pivotCellInfo.ParentCell != null && pivotCellInfo.ParentCell.ToString().Contains("Total") && (this.rowHeaders[row, 0].ToString().Contains(this[rowIndex, 0].Key.ToString()) || this.rowHeaders[row, 0].ToString().Contains("Grand") && this.rowHeaders[rowIndex, 1] != null))
          intList.Add(rowIndex - 1);
      }
      intList.ForEach((Action<int>) (rowIndex => pivotValuesCollection.Add(this.ShowCalculationsAsColumns ? this.GetRowColumnPivotValuesAt(rowIndex, col, out unused) : this.GetKeyForRawItem(col, rowIndex))));
    }
    else
    {
      List<IComparable> comparableList = this.ShowCalculationsAsColumns ? this.GetRowColumnPivotValuesAt(row, col, out unused) : this.GetKeyForRawItem(col, row);
      pivotValuesCollection.Add(comparableList);
    }
    IList data = !(this.dataSource is DataView) ? (!(this.DataSource is IList) ? this.DataSourceList as IList : this.DataSource as IList) : (IList) new DataView(((DataView) this.DataSourceList).Table);
    foreach (List<IComparable> comparableList in pivotValuesCollection)
    {
      if (data != null)
      {
        PropertyDescriptorCollection descriptorCollection = this.GetPropertyDescriptorCollection(data);
        foreach (object component in (IEnumerable) data)
        {
          bool flag1 = true;
          int index = 0;
          foreach (PivotItem pivotRow in this.PivotRows)
          {
            object obj = descriptorCollection[pivotRow.FieldMappingName].GetValue(component);
            if (obj != null)
              obj = pivotRow.Format == null || pivotRow.Format.Length <= 0 ? (object) obj.ToString() : (object) string.Format($"{{0:{pivotRow.Format}}}", obj);
            if (comparableList.Count > 0 && obj != null && (comparableList[index] == null || !(comparableList[index].ToString() == obj.ToString()) && obj != null))
            {
              flag1 = false;
              break;
            }
            ++index;
          }
          if (flag1)
          {
            foreach (PivotItem pivotColumn in this.PivotColumns)
            {
              object obj = descriptorCollection[pivotColumn.FieldMappingName].GetValue(component);
              if (obj != null)
                obj = pivotColumn.Format == null || pivotColumn.Format.Length <= 0 ? (object) obj.ToString() : (object) string.Format($"{{0:{pivotColumn.Format}}}", obj);
              if (comparableList.Count > 0 && comparableList.Count > index && (obj != null && comparableList[index] != null && !(comparableList[index].ToString() == obj.ToString()) || obj == null && comparableList[index] != null && comparableList[index].ToString() != " "))
              {
                flag1 = false;
                break;
              }
              ++index;
            }
          }
          if (flag1)
          {
            flag2 = true;
            foreach (FilterExpression filter in this.Filters)
            {
              if (!((!(component is DataRowView) ? filter.ComputedValue(component) : (object) filter.Expression.Contains(descriptorCollection[filter.Name].GetValue(component).ToString())) is bool flag2))
                ;
              if (!flag2)
                break;
            }
            if (flag2)
              rawItemsForEach.Add(component);
          }
        }
      }
    }
    return rawItemsForEach;
  }

  public void RemoveAllowedField(FieldInfo fi)
  {
    if (this.AllowedFields == null || !this.AllowedFields.Remove(fi))
      throw new ArgumentException($"{fi.Name} not in collection.");
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void PopulateDefaultPropertyFields()
  {
    if (this.AllowedFields != null)
    {
      this.AllowedFields.Clear();
      foreach (PropertyDescriptor itemProperty in this.ItemProperties)
      {
        Type type = itemProperty.GetType();
        ExpressionPropertyDescriptor propertyDescriptor = itemProperty as ExpressionPropertyDescriptor;
        if (type.Name == "ExpressionPropertyDescriptor" && propertyDescriptor != null)
          this.AllowedFields.Add(new FieldInfo()
          {
            Name = itemProperty.Name,
            FieldType = FieldTypes.Expression,
            Expression = propertyDescriptor.Expression,
            Format = propertyDescriptor.Format
          });
        else
          this.AllowedFields.Add(new FieldInfo()
          {
            Name = itemProperty.Name
          });
      }
    }
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void ClearAllowedFields()
  {
    if (this.AllowedFields != null)
      this.AllowedFields.Clear();
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void SuspendComputations() => this.lockComputations = true;

  public void ResumeComputations() => this.ResumeComputations(false, false);

  public void ResumeComputations(bool resetPivotCollections) => this.ResumeComputations(true, true);

  public void ResumeComputations(bool resetPivotCollections, bool shouldRefresh)
  {
    if (this.EnableDataOptimization)
      this.highRowLevel = 0;
    if (!resetPivotCollections)
      return;
    bool showGrandTotals = this.ShowGrandTotals;
    if (!this.ShowCalculationsAsColumns && this.PivotCalculations.Count > 0)
    {
      this.showGrandTotals = true;
      this.SwapRowsColumns(true);
    }
    this.PopulatePivotTable();
    if (this.EmptyPivot)
    {
      this.populationCompleted = true;
    }
    else
    {
      this.CalculateValues();
      if (!this.EnableDataOptimization)
        this.PopulatePivotGridControl();
      if ((this.PivotRows.Any<PivotItem>((System.Func<PivotItem, bool>) (x => !x.ShowSubTotal)) || this.PivotColumns.Any<PivotItem>((System.Func<PivotItem, bool>) (x => !x.ShowSubTotal)) || this.EnableOnDemandCalculations || !this.ShowCalculationsAsColumns) && (this.GridLayout == GridLayout.TopSummary || this.GridLayout == GridLayout.ExcelLikeLayout))
      {
        this.ReArrangePivotValuesForRows();
        if (this.GridLayout == GridLayout.TopSummary)
          this.ReArrangePivotValuesForColumn();
      }
      if (!this.ShowCalculationsAsColumns && this.PivotCalculations.Count > 0)
      {
        this.TransposePivotTable();
        this.SwapRowsColumns(false);
        if (!showGrandTotals)
          this.showGrandTotals = showGrandTotals;
      }
      if (this.EnableOnDemandCalculations)
        this.SetSummands();
      this.RemoveDelimeter();
      if (!this.RowPivotsOnly)
      {
        if (!shouldRefresh || this.LoadInBackground)
          return;
        this.lockComputations = false;
        this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs()
        {
          ChangeHints = SchemaChangeHints.GrandTotalVisibility
        });
      }
      else
      {
        if (!this.RowPivotsOnly || !this.lockComputations || !shouldRefresh)
          return;
        this.lockComputations = false;
        this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs()
        {
          ChangeHints = SchemaChangeHints.GrandTotalVisibility
        });
      }
    }
  }

  public void Populate()
  {
    bool demandCalculations = this.enableOnDemandCalculations;
    if (this.enableOnDemandCalculations && (this.PivotCalculations.Count == 0 || this.PivotCalculations.Any<PivotComputationInfo>((System.Func<PivotComputationInfo, bool>) (o => o.CalculationType != CalculationType.NoCalculation && o.CalculationType != CalculationType.Formula)) || this.GridLayout == GridLayout.TopSummary))
      this.enableOnDemandCalculations = false;
    if (this.UseIndexedEngine)
    {
      if (this.indexEngine == null)
        this.indexEngine = new IndexEngine(this);
      this.indexEngine.GetValue = this.GetValue;
      this.indexEngine.DataSource = this.DataSource;
      this.CoveredRanges.Clear();
      if (!this.ShowCalculationsAsColumns)
        this.SwapRowsColumns(true);
      this.indexEngine.IndexData(this.EnableOnDemandCalculations);
      this.rowCount = this.indexEngine.RowCount + 1;
      this.columnCount = this.indexEngine.ColumnCount + 1;
      if (!this.ShowCalculationsAsColumns)
      {
        this.TransposePivotTable();
        this.SwapRowsColumns(false);
      }
      this.enableOnDemandCalculations = demandCalculations;
    }
    else
    {
      this.sortKeys = (List<PivotEngine.SortKeys>) null;
      DateTime now = DateTime.Now;
      this.populationCompleted = false;
      this.CoveredRanges.Clear();
      if (!this.lockComputations)
        this.ResumeComputations(true);
      if (this.RowPivotsOnly)
        this.columnIndexes = Enumerable.Range(0, this.ColumnCount - 1).ToList<int>();
      this.highWaterRowIndex = this.rowOffSet;
      if (this.highWaterColumnIndex != this.colOffSet)
        this.highWaterColumnIndex = this.colOffSet;
      this.populationCompleted = true;
      this.enableOnDemandCalculations = demandCalculations;
    }
  }

  public void Reset()
  {
    this.lockComputations = true;
    this.sortKeys = (List<PivotEngine.SortKeys>) null;
    this.DataSource = (object) null;
    this.DataSourceList = (IEnumerable) null;
    if (this.PivotCalculations != null)
      this.PivotCalculations.Clear();
    if (this.PivotColumns != null)
      this.PivotColumns.Clear();
    if (this.PivotRows != null)
      this.PivotRows.Clear();
    if (this.Filters != null)
      this.Filters.Clear();
    this.valuesArea = (SummaryBase[,]) null;
    this.rowHeaders = (IComparable[,]) null;
    this.columnHeaders = (IComparable[,]) null;
    this.rowCount = 0;
    this.columnCount = 0;
    if (this.CoveredRanges != null)
      this.CoveredRanges.Clear();
    this.lockComputations = false;
  }

  public void Dispose()
  {
    this.getValue = (GetValueDelegate) null;
    if (this.filters != null)
    {
      this.filters.filterExpressions.Clear();
      this.filters.filterExpressions = (List<FilterExpression>) null;
      this.filters = (FilterHelper) null;
    }
    this.hiddenRowIndexes = (HashSet<PivotCellInfo>) null;
    if (this.sortKeys != null)
    {
      this.sortKeys.Clear();
      this.sortKeys = (List<PivotEngine.SortKeys>) null;
    }
    this.dataSource = (object) null;
    this.dataSourceList = (IEnumerable) null;
    this.colSummands = (Dictionary<int, List<int>>) null;
    this.rowSummands = (Dictionary<int, List<int>>) null;
    if (this.indexEngine != null)
      this.indexEngine = (IndexEngine) null;
    if (this.sortColIndexes != null)
    {
      this.sortColIndexes.Clear();
      this.sortColIndexes = (List<int>) null;
    }
    if (this.columnIndexes != null)
    {
      this.columnIndexes.Clear();
      this.columnIndexes = (List<int>) null;
    }
    if (this.sortDirs != null)
    {
      this.sortDirs.Clear();
      this.sortDirs = (List<ListSortDirection>) null;
    }
    if (this.PivotRows != null)
    {
      this.PivotRows.ForEach((Action<PivotItem>) (i => i.Dispose()));
      this.PivotRows.Clear();
      this.PivotRows = (List<PivotItem>) null;
    }
    if (this.PivotColumns != null)
    {
      this.PivotColumns.ForEach((Action<PivotItem>) (i => i.Dispose()));
      this.PivotColumns.Clear();
      this.PivotColumns = (List<PivotItem>) null;
    }
    if (this.PivotCalculations != null)
    {
      this.PivotCalculations.ForEach((Action<PivotComputationInfo>) (i => i.Dispose()));
      this.PivotCalculations.Clear();
      this.PivotCalculations = (List<PivotComputationInfo>) null;
    }
    if (this.Filters != null)
    {
      this.Filters.ForEach((Action<FilterExpression>) (i =>
      {
        i.Evaluator = (Delegate) null;
        i.Tag = (object) null;
        i.DimensionHeader = i.DimensionName = i.Name = i.Expression = i.Format = i.FieldCaption = i.Name = (string) null;
      }));
      this.Filters.Clear();
      this.Filters = (List<FilterExpression>) null;
    }
    this.Reset();
    if (this.allowedFields != null)
    {
      this.allowedFields.Clear();
      this.allowedFields = (List<FieldInfo>) null;
    }
    if (this.visibleRecords != null)
    {
      this.visibleRecords.Clear();
      this.visibleRecords = (List<object>) null;
    }
    this.rowHeaderUniqueValues = (string[,]) null;
    this.columnHeaderUniqueValues = (string[,]) null;
    if (this.HiddenPivotRowGroups != null)
    {
      this.HiddenPivotRowGroups.Clear();
      this.HiddenPivotRowGroups = (Dictionary<int, List<HiddenGroup>>) null;
    }
    if (this.HiddenPivotColumnGroups != null)
    {
      this.HiddenPivotColumnGroups.Clear();
      this.HiddenPivotColumnGroups = (Dictionary<int, List<HiddenGroup>>) null;
    }
    this.LookUp = (Dictionary<string, PropertyInfo>) null;
    this.delimiter = (string) null;
    if (this.coveredRanges != null)
    {
      this.coveredRanges.Clear();
      this.coveredRanges = (List<CoveredCellRange>) null;
    }
    this.parentColLocation = this.parentRowLocation = (int[]) null;
    this.PivotSchemaChanged = (PivotSchemaChangedEventHandler) null;
    this.columnKeysCalcValues = this.rowKeysCalcValues = this.tableKeysCalcValues = (BinaryList) null;
    if (this.pivotValues != null)
    {
      for (int index1 = 0; index1 < this.pivotValues.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.pivotValues[index1].Count; ++index2)
        {
          if (this.pivotValues[index1][index2] != null)
          {
            this.pivotValues[index1][index2].Dispose();
            this.pivotValues[index1][index2] = (PivotCellInfo) null;
          }
        }
      }
      this.pivotValues.Clear();
      this.pivotValues = (PivotCellInfos) null;
    }
    if (this.SummaryLibrary != null)
    {
      this.summaryLibrary.Add("Text", (SummaryBase) new TextSummary());
      this.SummaryLibrary.Clear();
    }
    this.summaryLibrary = (Dictionary<string, SummaryBase>) null;
    this.PivotBaseRowItems = (List<object>) null;
    this.PivotBaseColumnItems = (List<object>) null;
    this.hiddenRowIndexes = (HashSet<PivotCellInfo>) null;
    this.editCellsInfo = (Dictionary<string, Array>) null;
    this.grandString = this.emptyPivotString = (string) null;
    this.PropertyChanged = (PropertyChangedEventHandler) null;
    if (this.ItemCollection != null)
    {
      this.ItemCollection.Dispose();
      this.ItemCollection = (FilterItemsCollection) null;
    }
    if (this.ItemProperties != null)
    {
      this.ItemProperties.Clear();
      this.ItemProperties = (PropertyDescriptorCollection) null;
    }
    if (this.EditCellsInfo != null)
      this.EditCellsInfo = (Dictionary<string, Array>) null;
    if (this.VisibleRecords == null)
      return;
    this.VisibleRecords.Clear();
    this.VisibleRecords = (List<object>) null;
  }

  public void AddPivotCalculation(PivotComputationInfo info)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value)
      return;
    if (this.PivotCalculations == null)
      this.PivotCalculations = new List<PivotComputationInfo>();
    this.PivotCalculations.Add(info);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void InsertPivotCalculation(int index, PivotComputationInfo info)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value && this.tableKeysCalcValues.Count != 0)
      return;
    if (this.PivotCalculations == null)
      this.PivotCalculations = new List<PivotComputationInfo>();
    this.PivotCalculations.Insert(index, info);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void RemovePivotCalculation(PivotComputationInfo info)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value)
      return;
    if (this.PivotCalculations != null)
      this.PivotCalculations.Remove(info);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void AddRowPivot(PivotItem pivotItem)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value)
      return;
    if (this.PivotRows == null)
      this.PivotRows = new List<PivotItem>();
    this.PivotRows.Add(pivotItem);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void InsertRowPivot(int index, PivotItem pivotItem)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value && this.rowKeysCalcValues.Count != 0)
      return;
    if (this.PivotRows == null)
      this.PivotRows = new List<PivotItem>();
    this.PivotRows.Insert(index, pivotItem);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void RemoveRowPivot(PivotItem pivotItem)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value)
      return;
    if (this.PivotRows != null)
      this.PivotRows.Remove(pivotItem);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void AddColumnPivot(PivotItem pivotItem)
  {
    if (this.PivotColumns == null)
      this.PivotColumns = new List<PivotItem>();
    if (this.NotPopulated.HasValue && this.NotPopulated.Value)
      return;
    this.PivotColumns.Add(pivotItem);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void InsertColumnPivot(int index, PivotItem pivotItem)
  {
    if (this.PivotColumns == null)
      this.PivotColumns = new List<PivotItem>();
    if (this.NotPopulated.HasValue && this.NotPopulated.Value && this.columnKeysCalcValues.Count != 0)
      return;
    this.PivotColumns.Insert(index, pivotItem);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public void RemoveColumnPivot(PivotItem pivotItem)
  {
    if (this.NotPopulated.HasValue && this.NotPopulated.Value)
      return;
    if (this.PivotColumns != null)
      this.PivotColumns.Remove(pivotItem);
    this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs());
  }

  public List<object> GetRawItemsFor(int row, int col)
  {
    if (this.PivotCalculations == null && this.PivotRows == null && this.PivotColumns == null)
      return (List<object>) null;
    int valueStartRow = this.GetValueStartRow();
    int valueStartColumn = this.GetValueStartColumn();
    if (row < valueStartRow || col < valueStartColumn)
      return new List<object>();
    if (this.CacheRawValues)
      return this.GetCachedRawValues(row, col);
    this.isForRawItem = true;
    List<object> rawItemsFor = new List<object>();
    int num1 = this.PivotCalculations.Count > 0 ? this.PivotCalculations.Count : 1;
    int num2 = this.PivotRows.Count > 0 ? this.PivotRows.Count : 1;
    string rowName = "";
    string colName = "";
    int num3 = 0;
    int num4 = 0;
    int num5 = this.PivotCalculations.Count > 1 || this.PivotCalculations.Count == 1 && this.ShowSingleCalculationHeader ? this.PivotColumns.Count + 1 : this.PivotColumns.Count;
    if (this.IsRowSummary(row) || this.IsSummaryColumn(col) || !this.ShowCalculationsAsColumns || this.PivotRows.Count == 0 || this.PivotCalculations.Count == 1 && this.PivotColumns.Count == 0)
    {
      if (this.ShowCalculationsAsColumns)
      {
        for (int columnIndex1 = 0; columnIndex1 < this.PivotRows.Count; ++columnIndex1)
        {
          if (row > -1 && columnIndex1 > -1 && this[row, columnIndex1] != null && this[row, columnIndex1].FormattedText != null)
          {
            rowName = this[row, columnIndex1].FormattedText;
            num3 = columnIndex1;
            break;
          }
        }
      }
      else
      {
        for (int rowIndex = row; rowIndex > row - this.PivotCalculations.Count; --rowIndex)
        {
          for (int columnIndex1 = this.PivotRows.Count - 1; columnIndex1 >= 0; --columnIndex1)
          {
            if (rowIndex > -1 && columnIndex1 > -1 && this[rowIndex, columnIndex1] != null && this[rowIndex, columnIndex1].FormattedText != null)
            {
              rowName = this[rowIndex, columnIndex1].FormattedText;
              num3 = columnIndex1;
              break;
            }
          }
          if (rowName.Contains("Total"))
            break;
        }
      }
      for (int rowIndex = 0; rowIndex < num5; ++rowIndex)
      {
        if (this[rowIndex, col] != null && this[rowIndex, col].UniqueText != null && this[rowIndex, col].UniqueText != "x")
        {
          colName = this[rowIndex, col].CellType.ToString().Contains("GrandTotal") ? "GrandTotal" : this[rowIndex, col].UniqueText;
          num4 = rowIndex;
          break;
        }
      }
    }
    if (!this.ShowCalculationsAsColumns)
      return this.GetRawItemsFor(col, row, rowName, colName, num3, num4);
    if (this.PivotRows.Count == 0)
    {
      --col;
      if (!this.IsRowSummary(row) && !this.IsSummaryColumn(col) && !this.IsGrandTotalCell(row, col) && this.PivotColumns.Count > 0)
        rawItemsFor = this.GetRawItemsForEach(row, col);
      else if (this.IsSummaryColumn(col) && !this.IsGrandTotalCell(row, col))
      {
        int num6 = this.pivotCalculations.Count > 1 ? this.pivotCalculations.IndexOf(this.PivotCalculations.Where<PivotComputationInfo>((System.Func<PivotComputationInfo, bool>) (x => x.FieldName == this[row - 1, col + 1].FormattedText)).FirstOrDefault<PivotComputationInfo>()) : 0;
        for (int columnIndex1 = col - num6; columnIndex1 >= 0 && (columnIndex1 == col || this[num4, columnIndex1] == null || this[num4, columnIndex1].FormattedText == null || colName == null || colName.Contains(this[num4, columnIndex1].FormattedText)); columnIndex1 = columnIndex1 - this.PivotCalculations.Count + 1 - 1)
        {
          if (this[row, columnIndex1] != null && this[row, columnIndex1].CellType == PivotCellType.ValueCell || this.PivotRows.Count == 0 && this[row, columnIndex1].CellType == (PivotCellType.ValueCell | PivotCellType.GrandTotalCell))
          {
            foreach (object obj in this.GetRawItemsForEach(row, columnIndex1 - 1))
              rawItemsFor.Add(obj);
          }
        }
      }
      else
      {
        IList data = !(this.dataSource is DataView) ? (!(this.DataSource is IList) ? this.DataSourceList as IList : this.DataSource as IList) : (IList) new DataView(((DataView) this.DataSourceList).Table);
        PropertyDescriptorCollection descriptorCollection = this.GetPropertyDescriptorCollection(data);
        rawItemsFor = this.GetRawItemsList(data, descriptorCollection);
      }
      this.isForRawItem = false;
      return rawItemsFor;
    }
    if (this.PivotCalculations.Count > 0 && this[row, col] != null && this[row, col].FormattedText != null && this[row, col].CellType == PivotCellType.ValueCell)
    {
      List<object> rawItemsForEach = this.GetRawItemsForEach(row, col);
      this.isForRawItem = false;
      return rawItemsForEach;
    }
    if (this.PivotCalculations.Count > 0 && (this[row, 0] != null && (this[row, 0].CellType == (PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell) || this[row, 0].CellType == (PivotCellType.HeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell) || this.PivotRows.Count == 1 && this[row, col] != null && this[row, col].CellType == (PivotCellType.ValueCell | PivotCellType.TotalCell)) || this.IsGrandTotalCell(row, col)) && (this.PivotColumns.Count > 0 ? (this[0, col] == null ? 0 : (this[0, col].CellType == (PivotCellType.ColumnHeaderCell | PivotCellType.GrandTotalCell) ? 1 : (this[0, col].CellType == (PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell | PivotCellType.GrandTotalCell) ? 1 : 0))) : (this.PivotColumns.Count == 0 ? 1 : (this.PivotRows.Count == 1 ? 1 : 0))) != 0 && this.ShowGrandTotals && row == this.RowCount - 2 && col > this.ColumnCount - (num2 > 1 ? num1 + num2 : num1 + num2 + 1))
    {
      IList data = !(this.dataSource is DataView) ? (!(this.DataSource is IList) ? this.DataSourceList as IList : this.DataSource as IList) : (IList) new DataView(((DataView) this.DataSourceList).Table);
      PropertyDescriptorCollection descriptorCollection = this.GetPropertyDescriptorCollection(data);
      List<object> rawItemsList = this.GetRawItemsList(data, descriptorCollection);
      this.isForRawItem = false;
      return rawItemsList;
    }
    if (this.IsRowSummary(row) && this.PivotCalculations.Count > 0 || this.PivotColumns.Count == 0 && this.PivotCalculations.Count == 1)
    {
      if (!this.IsSummaryColumn(col) && !this.IsGrandTotalCell(row, col) || !this.IsSummaryColumn(col) && this.IsGrandTotalCell(row, col) && this[row, col].CellType == (PivotCellType.ValueCell | PivotCellType.TotalCell))
      {
        for (int index = row; index >= num5 && (index == row || this[index, num3] == null || this[index, num3].FormattedText == null || rowName == null || rowName.Contains(this[index, num3].FormattedText)); --index)
        {
          if (this[index, col] != null && this[index, col].FormattedText != null && this[index, col].CellType == PivotCellType.ValueCell)
          {
            foreach (object obj in this.GetRawItemsForEach(index, col))
              rawItemsFor.Add(obj);
          }
        }
      }
      else if (this.IsGrandTotalCell(row, col) && !this.IsSummaryColumn(col))
      {
        for (int index = row; index >= num5; --index)
        {
          if (this[index, col] != null && this[index, col].CellType == PivotCellType.ValueCell)
          {
            foreach (object obj in this.GetRawItemsForEach(index, col))
              rawItemsFor.Add(obj);
          }
        }
      }
      else if (this.IsSummaryColumn(col) && !this.IsGrandTotalCell(row, col))
      {
        for (int index1 = row - 1; index1 >= num5 && (index1 == row || this[index1, num3] == null || this[index1, num3].FormattedText == null || rowName == null || rowName.Contains(this[index1, num3].FormattedText)); --index1)
        {
          for (int index2 = col - this.PivotCalculations.Count; index2 >= (this.PivotRows.Count > 0 ? this.PivotRows.Count : 1) && (index2 == col || this[num4, index2] == null || this[num4, index2].FormattedText == null || colName == null || colName.Contains(this[num4, index2].FormattedText)); index2 = index2 - this.PivotCalculations.Count + 1 - 1)
          {
            if (this[index1, index2] != null && this[index1, index2].CellType == PivotCellType.ValueCell)
            {
              foreach (object obj in this.GetRawItemsForEach(index1, index2))
                rawItemsFor.Add(obj);
            }
          }
        }
      }
      else if (this.IsSummaryColumn(col) && this.IsGrandTotalCell(row, col))
      {
        if (col > this.ColumnCount - (num2 > 1 ? num1 + num2 : num1 + num2 + 1))
        {
          for (int index3 = row - 1; index3 >= num5 && (index3 == row || this[index3, num3] == null || this[index3, num3].FormattedText == null || rowName == null || rowName.Contains(this[index3, num3].FormattedText) && (!rowName.Contains(this[index3, num3].FormattedText) || !this[index3, num3].CellType.ToString().Contains("Total"))); --index3)
          {
            for (int index4 = col - this.PivotCalculations.Count; index4 >= (this.PivotRows.Count > 0 ? this.PivotRows.Count : 1); index4 = index4 - this.PivotCalculations.Count + 1 - 1)
            {
              if (this[index3, index4] != null && this[index3, index4].CellType == PivotCellType.ValueCell)
              {
                foreach (object obj in this.GetRawItemsForEach(index3, index4))
                  rawItemsFor.Add(obj);
              }
            }
          }
        }
        else
        {
          for (int index5 = row - 1; index5 >= num5; --index5)
          {
            for (int index6 = col - this.PivotCalculations.Count; index6 >= (this.PivotRows.Count > 0 ? this.PivotRows.Count : 1) && (index6 == col || this[num4, index6] == null || this[num4, index6].FormattedText == null || colName == null || colName.Contains(this[num4, index6].FormattedText)); index6 = index6 - this.PivotCalculations.Count + 1 - 1)
            {
              if (this[index5, index6] != null && this[index5, index6].CellType == PivotCellType.ValueCell)
              {
                foreach (object obj in this.GetRawItemsForEach(index5, index6))
                  rawItemsFor.Add(obj);
              }
            }
          }
        }
      }
      this.isForRawItem = false;
      return rawItemsFor;
    }
    if (!this.IsSummaryColumn(col) || this.PivotCalculations.Count <= 0)
      return rawItemsFor;
    for (int index = col - this.PivotCalculations.Count; index >= (this.PivotRows.Count > 0 ? this.PivotRows.Count : 1) && (index == col || this[num4, index] == null || this[num4, index].FormattedText == null || colName == null || colName.Contains(this[num4, index].FormattedText) || this.IsGrandTotalCell(row, col)); index = index - this.PivotCalculations.Count + 1 - 1)
    {
      if ((this[row, index] != null && this[row, index].FormattedText != null && this[row, index].CellType == PivotCellType.ValueCell || this.PivotRows.Count == 0 && this[row, index].CellType == (PivotCellType.ValueCell | PivotCellType.GrandTotalCell)) && this[num4, index] != null && this[num4, index].FormattedText != null && colName != null && (colName.Contains(this[num4, index].FormattedText) || colName == "GrandTotal"))
      {
        foreach (object obj in this.GetRawItemsForEach(row, index))
          rawItemsFor.Add(obj);
      }
    }
    this.isForRawItem = false;
    return rawItemsFor;
  }

  public void PopulateValueCells()
  {
    if (this.valuesArea == null)
      return;
    for (int row = 0; row < this.valuesArea.GetLength(0); ++row)
    {
      int valueRowStart = this.columnHeaders.GetLength(0) + 1;
      bool isSummaryRow = this.rowSummary[row + valueRowStart - 1];
      for (int col = 0; col < this.valuesArea.GetLength(1); ++col)
        this.PopulateValueCell(isSummaryRow, row, col, this.rowHeaders.GetLength(1), valueRowStart, 0, 0, 0, 0);
    }
  }

  public string UpdateCalculatedValue(int row, int col, object v, string fieldName)
  {
    if (this.sortKeys != null && this.sortKeys.Count > 0 && row >= 0)
      row = this.sortKeys[row].Index;
    return this.DoCalculatedValue(row, col, v, fieldName);
  }

  public void CalculateFormula(int row1, int col1, int col, PivotComputationInfo compInfo)
  {
    Dictionary<string, double> dictionary1 = new Dictionary<string, double>();
    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
    List<string> stringList = new List<string>();
    string modFormula = string.Empty;
    string modExpression = string.Empty;
    string[] source = (string[]) null;
    if (this.pivotValues[row1, col1] == null || this.pivotCalculations.Count == 0)
      return;
    int colIndex = col1 - col % this.PivotCalculations.Count;
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
    {
      if (this.PivotCalculations[index].CalculationName == null)
      {
        this.PivotCalculations[index].CalculationName = $"Computation{index}";
        this.pivotCalculations[index].AllowRunTimeGroupByField = false;
      }
      if (this.PivotCalculations[index].CalculationType != CalculationType.Formula)
      {
        dictionary1.Add(this.PivotCalculations[index].FieldCaption ?? this.PivotCalculations[index].FieldName, this.pivotValues[row1, colIndex] == null ? 0.0 : (this.ItemProperties[this.PivotCalculations[index].FieldName] == null || !(this.ItemProperties[this.PivotCalculations[index].FieldName].PropertyType == typeof (DateTime)) ? this.pivotValues[row1, colIndex].DoubleValue : Convert.ToDateTime(this.pivotValues[row1, colIndex].Value).ToOADate()));
        this.pivotCalculations[index].AllowRunTimeGroupByField = false;
      }
      else
        this.PivotCalculations[index].AllowRunTimeGroupByField = false;
      stringList.Add(this.pivotCalculations[index].FieldName);
      ++colIndex;
    }
    if (compInfo.Expression == null)
      compInfo.Expression = new FilterExpression(compInfo.CalculationName, compInfo.Formula);
    else
      compInfo.Expression.Expression = compInfo.Formula;
    if (this.ItemProperties.Cast<PropertyDescriptor>().ToList<PropertyDescriptor>().Any<PropertyDescriptor>((System.Func<PropertyDescriptor, bool>) (x => x.PropertyType == typeof (DateTime))) && compInfo.FieldType == null && this.PivotCalculations.Any<PivotComputationInfo>((System.Func<PivotComputationInfo, bool>) (x => this.ItemProperties[x.FieldName] != null && this.ItemProperties[x.FieldName].PropertyType == typeof (DateTime))) && compInfo.Formula.ToString().Contains(this.PivotCalculations.Where<PivotComputationInfo>((System.Func<PivotComputationInfo, bool>) (x => this.ItemProperties[x.FieldName] != null && this.ItemProperties[x.FieldName].PropertyType == typeof (DateTime))).FirstOrDefault<PivotComputationInfo>().FieldName))
    {
      this.pivotValues[row1, col1].Value = this.CalculateValuesForDateTime(compInfo, dictionary1);
      this.pivotValues[row1, col1].FormattedText = this.pivotValues[row1, col1].Value != null ? (compInfo.Format != null ? string.Format(this.pivotValues[row1, col1].Value.ToString(), (object) compInfo.Format, (object) CultureInfo.CurrentUICulture) : this.pivotValues[row1, col1].Value.ToString()) : "";
    }
    else
    {
      if (compInfo.Formula != null)
        modFormula = compInfo.Formula;
      stringList.ForEach((Action<string>) (x =>
      {
        if (!compInfo.Formula.Contains(x))
          return;
        modFormula = modFormula.Replace(x, "");
      }));
      modExpression = compInfo.Expression.Expression;
      stringList.ForEach((Action<string>) (x =>
      {
        if (!compInfo.Expression.Expression.Contains(x))
          return;
        modExpression = modExpression.Replace(x, "");
      }));
      if (!string.IsNullOrEmpty(modFormula) && modFormula.ToUpper().Contains("IF") && modFormula.ToList<char>().Where<char>((System.Func<char, bool>) (x => x.ToString() == "(")).Count<char>() == 1)
      {
        source = compInfo.Formula.Split(',', '(', ')');
        compInfo.Expression.Expression = ((IEnumerable<string>) source).Where<string>((System.Func<string, bool>) (x => x.ToString().Contains(">") || x.ToString().Contains("<") || x.ToString().Contains("!"))).FirstOrDefault<string>();
      }
      if (!string.IsNullOrEmpty(modFormula) && modFormula.ToUpper().Contains("AND") || modFormula.ToUpper().Contains("OR") || modFormula.ToUpper().Contains("&&") || modFormula.ToUpper().Contains("||") || modFormula.ToUpper().Contains("NOT") || modFormula.ToUpper().Contains("!"))
      {
        string[] strArray;
        if (compInfo.Formula.ToList<char>().Where<char>((System.Func<char, bool>) (x => x.ToString() == "(")).Count<char>() <= 1)
          strArray = compInfo.Formula.Split(',', '(', ')');
        else
          strArray = compInfo.Formula.Split('(');
        source = strArray;
        for (int index = 1; index < ((IEnumerable<string>) source).Count<string>() - 1; ++index)
        {
          compInfo.Expression.Expression = source[index];
          object obj = compInfo.Expression.ComputedValue((object) dictionary1);
          dictionary2.Add(source[index], obj);
        }
        this.pivotValues[row1, col1].Value = (object) (!modFormula.ToUpper().Contains("AND") && !modFormula.Contains("&&") || !dictionary2.Values.All<object>((System.Func<object, bool>) (x => x.ToString() == "True")) ? (!modFormula.ToUpper().Contains("OR") && !modFormula.Contains("||") || !dictionary2.Values.Any<object>((System.Func<object, bool>) (x => x.ToString() == "True")) ? (!modFormula.ToUpper().Contains("NOT") && !modFormula.Contains("!") || !dictionary2.Values.Any<object>((System.Func<object, bool>) (x => x.ToString() == "False")) ? 0 : 1) : 1) : 1);
      }
      else
      {
        this.pivotValues[row1, col1].Value = compInfo.Expression.ComputedValue((object) dictionary1);
        if (modExpression.ToUpper().Contains("AVERAGE"))
          this.pivotValues[row1, col1].Value = (object) ((double) this.pivotValues[row1, col1].Value / (double) dictionary1.Count);
        if (modExpression.ToUpper().Contains("COUNT") && !dictionary1.Any<KeyValuePair<string, double>>((System.Func<KeyValuePair<string, double>, bool>) (x => x.Key.ToUpper().Contains("COUNT"))))
          this.pivotValues[row1, col1].Value = (object) dictionary1.ToList<KeyValuePair<string, double>>().FindAll((Predicate<KeyValuePair<string, double>>) (x => Regex.IsMatch(x.ToString(), "\\d"))).Count;
      }
      if (this.pivotValues[row1, col1].Value is bool)
        this.pivotValues[row1, col1].Value = modExpression.ToUpper().Contains("MIN") ? (object) dictionary1.OrderBy<KeyValuePair<string, double>, double>((System.Func<KeyValuePair<string, double>, double>) (x => x.Value)).ToList<KeyValuePair<string, double>>()[0].Value : (modExpression.ToUpper().Contains("MAX") ? (object) dictionary1.OrderBy<KeyValuePair<string, double>, double>((System.Func<KeyValuePair<string, double>, double>) (x => x.Value)).ToList<KeyValuePair<string, double>>()[dictionary1.Count - 1].Value : this.pivotValues[row1, col1].Value);
      if (this.pivotValues[row1, col1].Value != null && this.pivotValues[row1, col1].Value.ToString() == "NaN")
        this.pivotValues[row1, col1].Value = (object) null;
      if (this.pivotValues[row1, col1].Value != null && compInfo.Format != null && compInfo.Format.Length > 0 && !(this.pivotValues[row1, col1].Value is bool))
      {
        this.pivotValues[row1, col1].FormattedText = this.pivotValues[row1, col1].Value == null || !this.pivotValues[row1, col1].Value.GetType().ToString().Contains("Int") && !(this.pivotValues[row1, col1].Value.GetType() == typeof (string)) ? (this.pivotValues[row1, col1].Value.ToString() != "Infinity" ? (double.TryParse(this.pivotValues[row1, col1].Value.ToString(), out double _) ? ((double) this.pivotValues[row1, col1].Value).ToString(compInfo.Format, (IFormatProvider) CultureInfo.CurrentUICulture) : (string) null) : " ") : this.pivotValues[row1, col1].Value.ToString();
        this.pivotValues[row1, col1].FormattedText = this.pivotValues[row1, col1].FormattedText.Replace("(", "").Replace(")", "");
      }
      else if (this.pivotValues[row1, col1].Value != null && source != null && ((IEnumerable<string>) source).Count<string>() > 1 && this.pivotValues[row1, col1].Value is bool)
      {
        int index = ((IEnumerable<string>) source).ToList<string>().FindIndex((Predicate<string>) (x => x.ToString() == compInfo.Expression.Expression));
        this.pivotValues[row1, col1].FormattedText = this.pivotValues[row1, col1].Value.ToString() == "True" ? (source[index].Contains(source[index + 1]) ? dictionary1[source[index + 1]].ToString() : source[index + 1]) : (((IEnumerable<string>) source).Contains<string>(source[index + 2]) ? dictionary1[source[index + 2]].ToString() : source[index + 2]);
      }
      else if (this.pivotValues[row1, col1].Value != null)
        this.pivotValues[row1, col1].FormattedText = this.pivotValues[row1, col1].Value is bool ? this.pivotValues[row1, col1].Value.ToString() : Math.Round((double) this.pivotValues[row1, col1].Value, 9).ToString((IFormatProvider) CultureInfo.CurrentUICulture);
    }
  }

  public void CoverColumnHeaders()
  {
    int val1 = this.PivotColumns.Count + 1;
    int num1 = 1;
    if (this.PivotColumns.Count > 0)
    {
      for (; num1 < val1; ++num1)
      {
        int num2;
        for (int index1 = (num2 = this.PivotRows.Count + 1) + 1; index1 <= this.ColumnCount && num1 < val1; ++index1)
        {
          int rowIndex = num1 - 1;
          int colIndex = index1 - 2;
          for (string uniqueText = this.pivotValues[num1 - 1, index1 - 2].UniqueText; index1 < this.ColumnCount && this.pivotValues[num1 - 1, index1 - 1] != null && (this.pivotValues[num1 - 1, index1 - 1].UniqueText == null || this.pivotValues[num1 - 1, index1 - 1].UniqueText.Equals(uniqueText)); ++index1)
            this.pivotValues[num1 - 1, index1 - 1].CellType = PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell;
          int num3;
          for (num3 = num1 + 1; num3 < val1 && this.pivotValues[num3 - 1, num2 - 1].UniqueText == null; ++num3)
            this.pivotValues[num3 - 1, num2 - 1].CellType = PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell;
          int val2 = num3 - 1;
          if (index1 > num2 + 1 || val2 > num1)
          {
            for (int index2 = num1; index2 <= Math.Min(val1, val2); ++index2)
            {
              for (int index3 = num2; index3 <= index1 - 1; ++index3)
              {
                if (this.pivotValues[index2 - 1, index3 - 1] != null && this.pivotValues[index2 - 1, index3 - 1].UniqueText == null)
                {
                  this.pivotValues[index2 - 1, index3 - 1].Value = (object) 'x';
                  this.pivotValues[index2 - 1, index3 - 1].UniqueText = "x";
                  this.pivotValues[index2 - 1, index3 - 1].CellType = PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell;
                  this.pivotValues[index2 - 1, index3 - 1].ParentCell = this.pivotValues[rowIndex, colIndex];
                }
              }
            }
            if (this.pivotValues[num1 - 1, num2 - 1] != null && this.pivotValues[num1 - 1, num2 - 1].Key != null)
            {
              CoveredCellRange coveredCellRange = new CoveredCellRange(num1 - 1, num2 - 1, Math.Min(val1, val2) - 1, index1 - 2);
              this.CoveredRanges.Add(coveredCellRange);
              this.pivotValues[num1 - 1, num2 - 1].CellRange = coveredCellRange;
              this.pivotValues[num1 - 1, num2 - 1].CellType = (PivotCellType) (128 /*0x80*/ | (num1 != val1 - 1 || !this.IsCalculationHeaderVisible || !(this.pivotValues[val2 - 1, num2 - 1].UniqueText.ToString() != "x") ? (coveredCellRange.Bottom - coveredCellRange.Top == 0 ? 2 : 16 /*0x10*/) : 4));
              if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && this.gridLayout != GridLayout.TopSummary && num1 == val1 - 1)
                this.pivotValues[num1 - 1, num2 - 1].CellType = (PivotCellType) (128 /*0x80*/ | (this.pivotValues[num1 - 1, num2 - 1] == null || this.pivotValues[num1 - 1, num2 - 1].UniqueText == null || !(this.pivotValues[num1 - 1, num2 - 1].UniqueText.ToString() != "x") || this.pivotValues[num1 - 1, num2 - 1].UniqueText.ToString().Contains(".") ? 4 : 16 /*0x10*/));
            }
          }
          else if ((num1 < val1 - 1 || num1 < val1 - 2 && this.IsCalculationHeaderVisible) && this.pivotValues[val2 - 1, num2 - 1] != null && this.pivotValues[val2 - 1, num2 - 1].UniqueText != null && !this.pivotValues[val2 - 1, num2 - 1].UniqueText.ToString().Contains(this.delimiter) && this.pivotValues[val2 - 1, num2 - 1].UniqueText.ToString() != "x")
          {
            CoveredCellRange coveredCellRange = new CoveredCellRange(num1 - 1, num2 - 1, num1 - 1, num2 - 1);
            this.CoveredRanges.Add(coveredCellRange);
            this.pivotValues[num1 - 1, num2 - 1].CellRange = coveredCellRange;
            this.pivotValues[num1 - 1, num2 - 1].CellType = (PivotCellType) (128 /*0x80*/ | (num1 != val1 - 1 || !this.IsCalculationHeaderVisible ? (coveredCellRange.Bottom - coveredCellRange.Top == 0 ? 2 : 16 /*0x10*/) : 4));
          }
          else if (this.pivotValues[num1 - 1, num2 - 1] != null)
            this.pivotValues[num1 - 1, num2 - 1].CellType = PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell;
          num2 = index1;
        }
      }
    }
    if (!this.IsCalculationHeaderVisible)
      return;
    int k = 0;
    for (int index4 = this.PivotRows.Count + 1; index4 < this.ColumnCount; ++index4)
    {
      if (this.pivotValues[val1 - 1, index4 - 1] != null && this.pivotValues[val1 - 1, index4 - 1].CellType == PivotCellType.ValueCell)
        this.pivotValues[val1 - 1, index4 - 1].CellType = !this.ShowSubTotalsForChildren || this.PivotColumns.Count <= 2 || this.pivotValues[val1 - 1, index4 - 1] == null || this.pivotValues[val1 - 1, index4 - 1].UniqueText == null || this.pivotValues[val1 - 1, index4 - 1].FormattedText == null || !(this.pivotValues[val1 - 1, index4 - 1].FormattedText != this.pivotValues[val1 - 1, index4 - 1].UniqueText) ? PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell : PivotCellType.TotalCell | PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell;
      if (this.IsSummaryColumn(index4 - 1, ref k))
      {
        for (int index5 = 0; index5 < this.PivotCalculations.Count; ++index5)
        {
          if (this.pivotValues[val1 - 1, index4 - 1 + index5] != null)
            this.pivotValues[val1 - 1, index4 - 1 + index5].CellType = this.IsCalculationHeaderVisible ? PivotCellType.TotalCell | PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell : PivotCellType.TotalCell | PivotCellType.ColumnHeaderCell;
        }
      }
    }
  }

  public void CoverGrandTotalRanges()
  {
    if (!this.ShowGrandTotals || this.PivotCalculations.Count <= 0)
      return;
    int num1 = 0;
    int count1 = this.PivotRows.Count;
    int num2 = this.ShowCalculationsAsColumns ? 1 : Math.Max(1, this.PivotCalculations.Count);
    int num3 = this.RowCount - num2 - 1;
    if (this.RowCount > 0)
    {
      CoveredCellRange range = new CoveredCellRange(num3, num1, num3 + num2 - 1, num1 + count1 - 1);
      if (this.OkToAddRange(range))
        this.CoveredRanges.Add(range);
      if (this.pivotValues[num3, num1] != null)
      {
        this.pivotValues[num3, num1].CellRange = range;
        this.pivotValues[num3, num1].CellType = PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell;
      }
      if (!this.ShowCalculationsAsColumns && this.IsCalculationHeaderVisible && num1 + count1 < this.pivotValues.GetLength(1))
      {
        for (int index = 0; index < num2; ++index)
        {
          if (this.pivotValues[num3 + index, num1 + count1] != null)
            this.pivotValues[num3 + index, num1 + count1].CellType = PivotCellType.CalculationHeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell;
        }
      }
    }
    if (this.ColumnCount <= 0)
      return;
    int num4 = this.ShowCalculationsAsColumns ? Math.Max(1, this.PivotCalculations.Count) : 1;
    int num5 = this.ColumnCount - num4 - 1;
    int count2 = this.PivotColumns.Count;
    int num6 = 0;
    CoveredCellRange range1 = new CoveredCellRange(num6, num5, num6 + count2 - 1, num5 + num4 - 1);
    if (this.OkToAddRange(range1))
      this.CoveredRanges.Add(range1);
    if (this.pivotValues[num6, num5] != null)
    {
      this.pivotValues[num6, num5].CellRange = range1;
      this.pivotValues[num6, num5].CellType = PivotCellType.ColumnHeaderCell | PivotCellType.GrandTotalCell;
    }
    if (!this.ShowCalculationsAsColumns || !this.IsCalculationHeaderVisible || num6 + count2 >= this.pivotValues.GetLength(0))
      return;
    for (int index = 0; index < num4; ++index)
    {
      if (this.pivotValues[num6 + count2, num5 + index] != null)
        this.pivotValues[num6 + count2, num5 + index].CellType = PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell | PivotCellType.GrandTotalCell;
    }
  }

  public void CoverRowHeaders()
  {
    if (this.PivotRows.Count == 0)
      return;
    int count1 = this.PivotRows.Count;
    int num1 = this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0);
    CoveredCellRange coveredCellRange1 = new CoveredCellRange(0, 0, num1 - 1, count1 - 1);
    if (num1 >= 1 && this.pivotValues != null)
    {
      this.pivotValues[0, 0].CellRange = coveredCellRange1;
      this.pivotValues[0, 0].CellType = PivotCellType.TopLeftCell;
      this.CoveredRanges.Add(coveredCellRange1);
    }
    int num2 = num1;
    int count2 = this.PivotRows.Count;
    for (int index1 = 1; index1 < count2; ++index1)
    {
      int num3;
      for (int index2 = (num3 = num2 + 1) + 1; index2 <= this.RowCount && index1 < count2; ++index2)
      {
        int rowIndex = index2 - 2;
        int colIndex = index1 - 1;
        string uniqueText = this.pivotValues[index2 - 2, index1 - 1].UniqueText;
        while (index2 < this.RowCount - 1 && this.pivotValues[index2 - 1, index1 - 1] != null && (this.pivotValues[index2 - 1, index1 - 1].UniqueText == null || (!this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 ? (this.pivotValues[index2 - 1, index1 - 1].UniqueText.Equals(uniqueText) ? 1 : 0) : (!(this.pivotValues[index2 - 1, index1 - 1].UniqueText.ToString() != "x") ? 0 : (this.pivotValues[index2 - 1, index1 - 1].UniqueText.Equals(uniqueText) ? 1 : 0))) != 0))
          ++index2;
        int num4 = index1 + 1;
        while (num4 <= count2 && this.pivotValues[num3 - 1, num4 - 1] != null && this.pivotValues[num3 - 1, num4 - 1].UniqueText == null)
          ++num4;
        int num5 = num4 - 1;
        if (index2 > num3 + 1 || num5 > index1)
        {
          for (int index3 = num3; index3 <= index2 - 1; ++index3)
          {
            for (int index4 = index1; index4 <= Math.Min(count2, num5); ++index4)
            {
              if (this.pivotValues[index3 - 1, index4 - 1] != null && this.pivotValues[index3 - 1, index4 - 1].UniqueText == null)
              {
                this.pivotValues[index3 - 1, index4 - 1].Value = (object) 'x';
                this.pivotValues[index3 - 1, index4 - 1].UniqueText = "x";
                this.pivotValues[index3 - 1, index4 - 1].ParentCell = this.pivotValues[rowIndex, colIndex];
              }
            }
          }
          CoveredCellRange coveredCellRange2 = new CoveredCellRange(num3 - 1, index1 - 1, index2 - 2, Math.Min(num5, count2) - 1);
          if (this.pivotValues[num3 - 1, index1 - 1].UniqueText != "x" || num5 > index1)
            this.CoveredRanges.Add(coveredCellRange2);
          if (this.pivotValues[num3 - 1, index1 - 1] != null)
            this.pivotValues[num3 - 1, index1 - 1].CellRange = coveredCellRange2;
          if (this.ShowSubTotalsForChildren && this.PivotRows.Count > 2 && this.pivotValues[num3 - 1, index1 - 1] != null)
            this.pivotValues[num3 - 1, index1 - 1].CellType = (PivotCellType) (64 /*0x40*/ | (coveredCellRange2.Right - coveredCellRange2.Left == 0 ? 2 : 16 /*0x10*/));
          else if (this.pivotValues[num3 - 1, index1 - 1] != null)
            this.pivotValues[num3 - 1, index1 - 1].CellType = (PivotCellType) (64 /*0x40*/ | (coveredCellRange2.Bottom - coveredCellRange2.Top != 0 ? 2 : 16 /*0x10*/));
        }
        else if (this.PivotCalculations.Count >= 0 && this.pivotValues[index2 - 1, index1 - 1] != null && this.pivotValues[index2 - 1, index1 - 1].UniqueText != null && this.pivotValues[index2 - 1, index1 - 1].UniqueText.ToString().Contains(this.delimiter) && this.pivotValues[index2 - 1, index1 - 1].UniqueText.ToString() != "x")
        {
          CoveredCellRange coveredCellRange3 = new CoveredCellRange(num3 - 1, index1 - 1, num3 - 1, index1 - 1);
          this.CoveredRanges.Add(coveredCellRange3);
          this.pivotValues[num3 - 1, index1 - 1].CellRange = coveredCellRange3;
          this.pivotValues[num3 - 1, index1 - 1].CellType = PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell;
        }
        else if (this.pivotValues[num3 - 1, index1 - 1] != null)
          this.pivotValues[num3 - 1, index1 - 1].CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell;
        num3 = index2;
      }
    }
    if (num2 <= 0)
      return;
    for (int index = num2; index < this.RowCount; ++index)
    {
      if (this.pivotValues[index - 1, count2 - 1] != null && (this.pivotValues[index - 1, count2 - 1].CellType & PivotCellType.ValueCell) != (PivotCellType) 0)
        this.pivotValues[index - 1, count2 - 1].CellType = !this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 || this.GridLayout == GridLayout.TopSummary || this.pivotValues[index - 1, count2 - 1] == null || this.pivotValues[index - 1, count2 - 1].UniqueText == null || this.pivotValues[index - 1, count2 - 1].UniqueText.Contains(".") ? PivotCellType.HeaderCell | PivotCellType.RowHeaderCell : PivotCellType.TotalCell | PivotCellType.RowHeaderCell;
    }
  }

  public void ReArrangePivotValuesForColumn()
  {
    int colIndex1 = 0;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    List<PivotCellInfo> pivotCellInfoList = new List<PivotCellInfo>();
    List<int> intList = new List<int>();
    if (this.PivotValues == null)
      return;
    for (int index1 = 0; index1 < this.PivotColumns.Count - 1; ++index1)
    {
      for (int colIndex2 = 0; colIndex2 < this.PivotValues[index1].Count; ++colIndex2)
      {
        if (this.PivotValues[index1, colIndex2] != null && this.PivotValues[index1, colIndex2].CellType == (PivotCellType.TotalCell | PivotCellType.ColumnHeaderCell))
        {
          int num4 = (this.PivotCalculations.Count > 0 ? this.PivotCalculations.Count : 1) - 1;
          intList.Add(colIndex2 + num4);
        }
      }
      for (int index2 = index1; index2 < this.PivotValues.Count; ++index2)
      {
        int colIndex3 = this.PivotRows.Count > 0 ? this.PivotRows.Count + index1 * this.PivotCalculations.Count : 1 + index1 * this.PivotCalculations.Count;
        if (colIndex1 == 0)
          colIndex1 = colIndex3;
        for (int colIndex4 = 0; colIndex4 < this.PivotValues[index2].Count; ++colIndex4)
        {
          for (int index3 = 0; index3 < intList.Count; ++index3)
          {
            if (intList[index3] == colIndex4)
            {
              int num5 = 0;
              int num6 = 0;
              for (int index4 = 0; index4 < (this.PivotCalculations.Count > 0 ? this.PivotCalculations.Count : 1); ++index4)
              {
                if (this.PivotValues[index2, colIndex4] != null)
                {
                  if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && num5 == 0)
                  {
                    num1 = index1 != 0 || index2 != 0 ? (index1 != 0 || this.PivotValues[index1, colIndex3] == null || this.PivotValues[index1, colIndex3].CellRange == null ? 0 : this.PivotValues[index1, colIndex3].CellRange.Right - colIndex3 - (this.PivotCalculations.Count > 1 ? this.PivotCalculations.Count - 1 : 0)) : (this.PivotValues[index2, this.PivotCalculations.Count > 1 ? colIndex4 - (this.PivotCalculations.Count - 1) : colIndex4] == null || this.PivotValues[index2, this.PivotCalculations.Count > 1 ? colIndex4 - (this.PivotCalculations.Count - 1) : colIndex4].CellRange == null ? 0 : this.PivotValues[index2, this.PivotCalculations.Count > 1 ? colIndex4 - (this.PivotCalculations.Count - 1) : colIndex4].CellRange.Right - this.PivotValues[index2, this.PivotCalculations.Count > 1 ? colIndex4 - (this.PivotCalculations.Count - 1) : colIndex4].CellRange.Left - (this.PivotCalculations.Count > 1 ? this.PivotCalculations.Count - 1 : 0));
                    num2 = num1;
                    if (num3 == 0)
                      num3 = num1;
                    if (index1 != 0)
                      num6 = num3;
                    for (int index5 = 0; index5 <= num1; ++index5)
                      pivotCellInfoList.Add(this.PivotValues[index2, colIndex4 + num1 - index5]);
                    for (int index6 = 0; index6 <= num1; ++index6)
                    {
                      this.PivotValues[index2].RemoveAt(colIndex4 + num1);
                      this.PivotValues[index2].Insert(colIndex3 + num6, pivotCellInfoList[index6]);
                    }
                    if (this.PivotCalculations.Count <= 1)
                      pivotCellInfoList.Reverse();
                    ++num5;
                  }
                  else
                  {
                    pivotCellInfoList.Add(this.PivotValues[index2, colIndex4 + num1]);
                    this.PivotValues[index2].RemoveAt(colIndex4 + num1);
                    this.PivotValues[index2].Insert(colIndex3 + num6, pivotCellInfoList[0]);
                  }
                  if (index2 == this.PivotColumns.Count - 1)
                    num2 = 0;
                  for (int colIndex5 = colIndex3; colIndex5 <= colIndex4 + num1; ++colIndex5)
                  {
                    if (this.pivotValues[index2, colIndex5] != null && this.pivotValues[index2, colIndex5].CellRange != null && this.pivotValues[index2, colIndex5].CellRange == pivotCellInfoList[0].CellRange)
                    {
                      int index7 = this.CoveredRanges.IndexOf(pivotCellInfoList[0].CellRange);
                      this.PivotValues[index2, colIndex5].CellRange = new CoveredCellRange(pivotCellInfoList[0].CellRange.Top, this.PivotRows.Count <= 1 || this.ShowSubTotalsForChildren ? (!this.ShowSubTotalsForChildren || this.PivotColumns.Count <= 2 || index2 != this.PivotColumns.Count - 1 ? pivotCellInfoList[0].CellRange.Left : colIndex3) : colIndex3, pivotCellInfoList[0].CellRange.Bottom, !this.ShowSubTotalsForChildren || this.PivotColumns.Count <= 2 || index2 != this.PivotColumns.Count - 1 ? colIndex3 + this.PivotCalculations.Count - 1 + num1 + num6 : colIndex3 + this.PivotCalculations.Count - 1);
                      if (this.PivotValues[index2, colIndex5].CellType == (PivotCellType.TotalCell | PivotCellType.ColumnHeaderCell))
                        this.PivotValues[index2, colIndex5].CellType = PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell;
                      this.CoveredRanges.RemoveAt(index7);
                      this.CoveredRanges.Insert(index7 - 1, this.PivotValues[index2, colIndex5].CellRange);
                    }
                    else if (this.pivotValues[index2, colIndex5] != null && this.pivotValues[index2, colIndex5].CellRange != null)
                    {
                      int index8 = this.CoveredRanges.IndexOf(this.PivotValues[index2, colIndex5].CellRange);
                      this.PivotValues[index2, colIndex5].CellRange = new CoveredCellRange(this.PivotValues[index2, colIndex5].CellRange.Top, !this.ShowSubTotalsForChildren || this.PivotColumns.Count <= 2 || index2 != this.PivotColumns.Count - 1 ? this.PivotValues[index2, colIndex5].CellRange.Left + 1 + num2 : colIndex5, this.PivotValues[index2, colIndex5].CellRange.Bottom, !this.ShowSubTotalsForChildren || this.PivotColumns.Count <= 2 || index2 != this.PivotColumns.Count - 1 ? this.PivotValues[index2, colIndex5].CellRange.Right + 1 + num2 : this.PivotCalculations.Count - 1 + colIndex5);
                      if (this.PivotValues[index2, colIndex5].CellType == (PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell))
                        this.PivotValues[index2, colIndex5].CellType = PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell;
                      this.CoveredRanges.RemoveAt(index8);
                      this.CoveredRanges.Insert(index8, this.PivotValues[index2, colIndex5].CellRange);
                    }
                  }
                  num2 = 0;
                  pivotCellInfoList.Clear();
                }
              }
              if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && index1 != 0 && index2 != 0)
              {
                num3 = 0;
                num1 = this.PivotValues[0, colIndex4 + 1].CellRange != null ? this.PivotValues[0, colIndex4 + 1].CellRange.Right - colIndex4 - this.PivotCalculations.Count : 0;
              }
              colIndex3 = colIndex4 + num1 + (index1 > 0 ? (this.PivotValues[index1, colIndex4 + 1].CellRange == null ? index1 * this.PivotCalculations.Count + 1 : 1) : 1);
            }
          }
        }
        if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && index1 != 0)
          num3 = this.PivotValues[0, colIndex1].CellRange != null ? this.PivotValues[0, colIndex1].CellRange.Right - this.PivotCalculations.Count : 0;
      }
      intList.Clear();
    }
  }

  public void ReArrangePivotValuesForRows()
  {
    int num1 = this.PivotCalculations.Count > 1 ? this.PivotColumns.Count : (this.PivotColumns.Count >= 1 ? this.PivotColumns.Count - 1 : this.PivotColumns.Count);
    int rowIndex1 = num1 + 1;
    int num2 = 0;
    PivotCellInfos tempInfo = new PivotCellInfos();
    if (this.PivotValues == null || this.PivotRows.Count < 2)
      return;
    for (int pivotRow = 0; pivotRow < this.PivotRows.Count; ++pivotRow)
    {
      int num3 = num1 + 1;
      int num4 = num3;
      int num5 = 0;
      for (int index1 = 0; index1 < this.PivotValues.Count; ++index1)
      {
        if (this.PivotValues[index1, pivotRow] != null && this.PivotValues[index1, pivotRow].CellType == (PivotCellType.TotalCell | PivotCellType.RowHeaderCell))
        {
          tempInfo.Add(this.PivotValues[index1]);
          for (int colIndex = pivotRow; colIndex >= 0; --colIndex)
          {
            if (tempInfo[0, colIndex] != null && tempInfo[0, colIndex].CellRange == null && tempInfo[0, colIndex].FormattedText == null)
            {
              tempInfo[0, colIndex].CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell;
              tempInfo[0, colIndex].FormattedText = this.PivotValues[num3, pivotRow - 1].FormattedText;
            }
          }
          int index2 = this.GridLayout == GridLayout.ExcelLikeLayout ? this.CoveredRanges.FindIndex((Predicate<CoveredCellRange>) (x => x.Bottom == tempInfo[0, pivotRow].CellRange.Bottom && x.Left == tempInfo[0, pivotRow].CellRange.Left && x.Top == tempInfo[0, pivotRow].CellRange.Top && x.Right == tempInfo[0, pivotRow].CellRange.Right)) : this.CoveredRanges.IndexOf(tempInfo[0, pivotRow].CellRange);
          int num6 = !this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 || tempInfo[0, pivotRow] == null || tempInfo[0, pivotRow].CellRange == null ? 0 : tempInfo[0, pivotRow].CellRange.Bottom - tempInfo[0, pivotRow].CellRange.Top;
          if (tempInfo[0, pivotRow] != null)
            tempInfo[0, pivotRow].CellRange = new CoveredCellRange(num3, tempInfo[0, pivotRow].CellRange.Left, !this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 ? num3 : num3 + num6, tempInfo[0, pivotRow].CellRange.Right);
          if (index2 > -1)
            this.CoveredRanges.RemoveAt(index2);
          if (tempInfo[0, pivotRow] != null)
            tempInfo[0, pivotRow].CellType = PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell;
          if (this.ShowSubTotalsForChildren && this.PivotRows.Count > 2)
          {
            for (int index3 = 1; index3 <= num6; ++index3)
              tempInfo.Add(this.PivotValues[index1 + index3]);
            for (int index4 = 0; index4 <= num6; ++index4)
            {
              this.PivotValues.RemoveAt(index1 + index4);
              this.PivotValues.Insert(num3 + index4, tempInfo[index4]);
            }
          }
          else
          {
            if (this.GridLayout == GridLayout.ExcelLikeLayout && tempInfo[0, pivotRow] != null && this.PivotValues[num3, pivotRow] != null)
              tempInfo[0, pivotRow].FormattedText = this.PivotValues[num3, pivotRow].FormattedText;
            this.PivotValues.RemoveAt(index1);
            this.PivotValues.Insert(num3, tempInfo[0]);
          }
          if (this.PivotValues[num3 + 1 + num6, pivotRow] != null)
          {
            index2 = this.CoveredRanges.IndexOf(this.PivotValues[num3 + 1 + num6, pivotRow].CellRange);
            if (this.PivotValues[num3 + 1 + num6, pivotRow].CellRange != null)
            {
              if (pivotRow != 0 && this.ShowSubTotalsForChildren && this.PivotRows.Count > 2)
              {
                num2 = this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Bottom - this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Top;
                this.PivotValues[num3 + 1 + num6, pivotRow].CellRange = new CoveredCellRange(num3 + 1 + num6, this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Left, num3 + 1 + num6 + num2, this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Right);
              }
              else
                this.PivotValues[num3 + 1 + num6, pivotRow].CellRange = new CoveredCellRange(this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Top + pivotRow + 1 + (!this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 ? 0 : num6 + num2), this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Left, this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Bottom + pivotRow + 1 + (!this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 ? 0 : num6 + num2), this.PivotValues[num3 + 1 + num6, pivotRow].CellRange.Right);
            }
          }
          if (index2 != -1)
          {
            this.CoveredRanges.RemoveAt(index2);
            if (this.PivotValues[num3 + 1 + num6, pivotRow] != null)
              this.CoveredRanges.Insert(index2, this.PivotValues[num3 + 1 + num6, pivotRow].CellRange);
            this.CoveredRanges.Insert(index2, tempInfo[0, pivotRow].CellRange);
          }
          else
          {
            if (this.PivotValues[num3 + 1 + num6, pivotRow] != null)
              this.CoveredRanges.Add(this.PivotValues[num3 + 1 + num6, pivotRow].CellRange);
            this.CoveredRanges.Add(tempInfo[0, pivotRow].CellRange);
          }
          if (this.PivotValues[num3 + 1 + num6, pivotRow] != null)
            this.PivotValues[num3 + 1 + num6, pivotRow].CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell;
          tempInfo.Clear();
          int num7 = 1;
          if (num5 == 0 && this.PivotValues[rowIndex1, 0] != null)
            num5 = !this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 || this.PivotValues[rowIndex1, 0].CellRange == null ? 0 : this.PivotValues[rowIndex1, 0].CellRange.Bottom - this.PivotValues[rowIndex1, 0].CellRange.Top;
          if (pivotRow > 0)
          {
            if (this.IsRowSummary(index1 - num5 - pivotRow) && !this.IsGrandTotalCell(index1 - num5 - pivotRow, 0))
            {
              if (this.PivotCalculations.Count == 0 && this.PivotColumns.Count == 0 || this.PivotColumns.Count == 0 && this.PivotCalculations.Count <= 1)
              {
                for (int rowIndex2 = index1 - num5 - pivotRow; rowIndex2 < this.PivotValues.Count && this.IsRowSummary(rowIndex2) && !this.IsGrandTotalCell(index1 - num5 - pivotRow, 0); ++rowIndex2)
                  ++num7;
              }
              if (this.PivotColumns.Count > 0 || this.PivotCalculations.Count > 1)
              {
                for (int rowIndex3 = index1 - num5 - pivotRow + 1; rowIndex3 <= index1 - num5 - pivotRow + this.pivotRows.Count && this.IsRowSummary(rowIndex3) && !this.IsGrandTotalCell(index1 - num5 - pivotRow, 0); ++rowIndex3)
                  ++num7;
                if (this.ShowSubTotalsForChildren && this.PivotRows.Count > 2 && num7 > this.PivotRows.Count - 1)
                {
                  num7 = this.PivotValues[index1 + 1, 0].CellRange != null ? this.PivotValues[index1 + 1, 0].CellRange.Bottom - (this.PivotValues[index1 + 1, 0].CellRange.Top - 1) + pivotRow : num7;
                  num5 = num7 - pivotRow - 1;
                }
              }
            }
            num3 = index1 + num7;
          }
          else
            num3 = index1 + 1 + (!this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 ? 0 : num6);
        }
      }
      num1 = num4 + (!this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 || this.PivotValues[rowIndex1, pivotRow] == null || this.PivotValues[rowIndex1, pivotRow].CellRange == null ? 0 : this.PivotValues[rowIndex1, pivotRow].CellRange.Bottom - this.PivotValues[rowIndex1, pivotRow].CellRange.Top);
    }
    if (this.GridLayout != GridLayout.ExcelLikeLayout)
      return;
    this.UpdateRowHeaderForExcelLayout();
  }

  public bool IsSummaryColumn(int colIndex)
  {
    bool flag = false;
    if (colIndex < this.columnHeaders.GetLength(1) && colIndex >= 0)
    {
      for (int index = 0; index < this.columnHeaders.GetLength(0); ++index)
      {
        if (this.columnHeaderUniqueValues[index, colIndex] != null && this.columnHeaderUniqueValues[index, colIndex].ToString().Contains(this.delimiter))
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public bool IsSummaryColumn(int colIndex, ref int k)
  {
    bool flag = false;
    if (colIndex < this.columnHeaders.GetLength(1))
    {
      for (int index = 0; index < this.columnHeaders.GetLength(0); ++index)
      {
        if (this.columnHeaders[index, colIndex] != null && this.columnHeaders[index, colIndex].ToString().Contains(this.delimiter))
        {
          k = index;
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public bool IsSummaryColumnWhileOnDemand(int colIndex, ref int k)
  {
    bool flag = false;
    if (colIndex < this.columnHeaders.GetLength(1))
    {
      for (int rowIndex = 0; rowIndex < this.columnHeaders.GetLength(0); ++rowIndex)
      {
        if (rowIndex < this.rowCount && colIndex < this.ColumnCount && this.PivotValues != null && this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].Key != null && (this.PivotValues[rowIndex, colIndex].Key.IndexOf(this.delimiter) > -1 || this.PivotValues[rowIndex, colIndex].Key.IndexOf(this.grandString + this.delimiter) > -1))
        {
          k = rowIndex;
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public bool IsSummaryColumnWhileTopSummary(int colIndex)
  {
    bool flag = false;
    if (colIndex < this.RowCount && colIndex < this.ColumnCount)
    {
      for (int rowIndex = 0; rowIndex < this.RowCount; ++rowIndex)
      {
        if (this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].CellType == (PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell))
          flag = true;
      }
    }
    return flag;
  }

  public bool IsSummaryRowWhileTopSummary(int rowIndex)
  {
    bool flag = false;
    if (rowIndex < this.RowCount)
    {
      for (int colIndex = 0; colIndex < this.ColumnCount; ++colIndex)
      {
        if (this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].CellType == (PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell))
          flag = true;
        else if (this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].FormattedText != null && this.PivotValues[rowIndex, colIndex].FormattedText.ToString().Contains(this.grandString))
          flag = true;
      }
    }
    return flag;
  }

  public bool IsSummaryRow(int rowIndex, ref int k)
  {
    bool flag = false;
    if (rowIndex < this.rowHeaders.GetLength(0))
    {
      for (int index = 0; index < this.rowHeaders.GetLength(1); ++index)
      {
        if (this.rowHeaders[rowIndex, index] != null && this.rowHeaders[rowIndex, index].ToString().Contains(this.delimiter))
        {
          flag = true;
          k = index;
          break;
        }
      }
    }
    return flag;
  }

  public bool IsSummaryRowWhileOnDemand(int rowIndex, ref int k)
  {
    bool flag = false;
    if (this.PivotColumns.Count == 0 && rowIndex <= this.rowHeaders.GetLength(0) || rowIndex < this.rowHeaders.GetLength(0))
    {
      for (int colIndex = 0; colIndex < this.rowHeaders.GetLength(1); ++colIndex)
      {
        if (rowIndex < this.rowCount && colIndex < this.ColumnCount && this.PivotValues != null && this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].Key != null && this.PivotValues[rowIndex, colIndex].Key.IndexOf(this.delimiter) > -1)
        {
          flag = true;
          k = colIndex;
          break;
        }
      }
    }
    return flag;
  }

  public bool IsRowSummary(int rowIndex)
  {
    bool flag = false;
    if (rowIndex < this.rowHeaders.GetLength(0) && rowIndex >= 0)
    {
      for (int index = 0; index < this.rowHeaders.GetLength(1); ++index)
      {
        if (this.rowHeaders[rowIndex, index] != null && this.rowHeaders[rowIndex, index].ToString().Contains(this.delimiter))
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public IComparer AddComparers(Type type)
  {
    if (this.DataSource == null)
      return (IComparer) null;
    if (type.Name.ToString() == "Int32" || type == typeof (int?))
      return (IComparer) new IntComparer();
    if (type.Name.ToString() == "Double" || type == typeof (double?) || type == typeof (float) || type == typeof (float?))
      return (IComparer) new DoubleComparer();
    if (type.Name.ToString() == "Decimal" || type == typeof (Decimal?))
      return (IComparer) new DecimalComparer();
    return type.Name.ToString() == "DateTime" || type == typeof (DateTimeOffset) ? (IComparer) new DateComparer() : (IComparer) null;
  }

  public ListSortDirection GetSortDirection(int col)
  {
    int index = this.sortColIndexes.IndexOf(col);
    return index <= -1 ? ListSortDirection.Descending : this.sortDirs[index];
  }

  public void ClearSorts()
  {
    if (!this.RowPivotsOnly)
      return;
    this.calcSortDirection = ListSortDirection.Ascending;
    this.sortColIndexes.Clear();
    this.sortDirs.Clear();
    if (this.sortKeys == null)
      return;
    this.sortKeys.Clear();
    this.sortKeys = (List<PivotEngine.SortKeys>) null;
  }

  public void ClearSortAt(int index)
  {
    if (this.sortColIndexes == null)
      return;
    this.sortColIndexes.Remove(index);
  }

  public bool AnyValueColumnsSorted() => this.sortColIndexes.Count > 0;

  public int ResolveColumnIndex(int columnIndex)
  {
    return !this.RowPivotsOnly || this.columnIndexes == null || this.columnIndexes.Count <= 0 || columnIndex >= this.columnIndexes.Count ? columnIndex : this.columnIndexes[columnIndex];
  }

  public bool IsColumnSorted(int columnIndex) => this.sortColIndexes.IndexOf(columnIndex) > -1;

  public bool CanFilterColumn(int columnIndex)
  {
    int index = this.ResolveColumnIndex(columnIndex);
    if (!this.RowPivotsOnly)
      return false;
    if (index < this.PivotRows.Count && this.PivotRows[index].AllowFilter)
      return true;
    return index >= this.PivotRows.Count && index - this.PivotRows.Count < this.pivotCalculations.Count && this.pivotCalculations[index - this.PivotRows.Count].AllowFilter;
  }

  public bool CanSortColumn(int columnIndex)
  {
    int index = this.ResolveColumnIndex(columnIndex);
    if (!this.RowPivotsOnly)
      return false;
    if (index < this.PivotRows.Count && this.PivotRows[index].AllowSort)
      return true;
    return index >= this.PivotRows.Count && this.pivotCalculations[index - this.PivotRows.Count].AllowSort;
  }

  public string GetFieldNameAtIndex(int columnIndex)
  {
    int index = this.ResolveColumnIndex(columnIndex);
    if (this.RowPivotsOnly && index < this.ColumnCount && index >= this.PivotRows.Count)
      return this.pivotCalculations[index - this.PivotRows.Count].FieldName;
    return index >= this.PivotRows.Count ? "" : this.PivotRows[index].FieldMappingName;
  }

  public void AdjustColumnIndexes(int from, int to)
  {
    int columnIndex = this.columnIndexes[from];
    this.columnIndexes.RemoveAt(from);
    this.columnIndexes.Insert(to, columnIndex);
    this.AdjustSortKeysForMoving(from, to);
  }

  public void SortByCalculation(int colIndex) => this.SortByCalculation(colIndex, false);

  public void SortByCalculation(int colIndex, bool isMultiColumn)
  {
    this.isColumnSorting = true;
    this.SortByCalculation(colIndex, isMultiColumn, this.calcSortDirection);
  }

  public void SortByCalculation(int colIndex, bool isMultiColumn, ListSortDirection dir)
  {
    if (this.UseIndexedEngine)
    {
      if (this.indexEngine == null)
        return;
      this.indexEngine.SortByCalculation(colIndex);
    }
    else
    {
      this.calcSortDirection = dir;
      this.calcSortDirection = this.GetCalcSortDirection(this.calcSortDirection);
      if (!isMultiColumn)
      {
        if (this.sortColIndexes != null)
          this.sortColIndexes.Clear();
        if (this.sortDirs != null)
          this.sortDirs.Clear();
      }
      else
      {
        int index = this.sortColIndexes != null ? this.sortColIndexes.IndexOf(colIndex) : -1;
        if (index > -1)
        {
          if (this.sortDirs != null)
            this.sortDirs.RemoveAt(index);
          if (this.sortColIndexes != null)
            this.sortColIndexes.RemoveAt(index);
        }
      }
      bool flag1 = false;
      if (this.RowPivotsOnly && (colIndex < this.PivotRows.Count - 1 || colIndex == this.PivotRows.Count - 1 && this.PivotRows[colIndex].Comparer != null))
      {
        PivotItem pivotRow = this.PivotRows[colIndex];
        if (pivotRow != null && pivotRow.Comparer == null)
          pivotRow.Comparer = (IComparer) new PivotEngine.ReverseOrderComparer();
        else if (pivotRow != null && pivotRow.Comparer is PivotEngine.ReverseOrderComparer)
          pivotRow.Comparer = (IComparer) null;
        else if (pivotRow != null)
        {
          if (pivotRow.Comparer is PivotEngine.SortWithDirComparer)
            ((PivotEngine.SortWithDirComparer) pivotRow.Comparer).dir = this.calcSortDirection;
          else
            pivotRow.Comparer = (IComparer) new PivotEngine.SortWithDirComparer(pivotRow.Comparer, this.calcSortDirection);
        }
        flag1 = colIndex < this.PivotRows.Count - 1;
      }
      if (this.sortColIndexes != null)
        this.sortColIndexes.Add(colIndex);
      if (this.sortDirs != null)
        this.sortDirs.Add(this.calcSortDirection);
      List<ListSortDirection> sortDirs;
      if (!isMultiColumn)
        sortDirs = this.sortDirs;
      else
        sortDirs = new List<ListSortDirection>()
        {
          this.SortDirection
        };
      PivotEngine.CalcSortComparer calcSortComparer = new PivotEngine.CalcSortComparer(sortDirs);
      int capacity = this.RowCount - 1;
      List<PivotEngine.SortKeys> sortKeysList1 = new List<PivotEngine.SortKeys>(capacity);
      bool flag2 = colIndex == this.PivotRows.Count - 1;
      this.sortKeys = (List<PivotEngine.SortKeys>) null;
      for (int rowIndex = 0; rowIndex < capacity; ++rowIndex)
      {
        IComparable[] comparableArray = new IComparable[this.sortColIndexes.Count];
        int num = 0;
        foreach (int sortColIndex in this.sortColIndexes)
          comparableArray[num++] = this[rowIndex, sortColIndex] == null || sortColIndex != this.PivotRows.Count - 1 && (sortColIndex < this.PivotRows.Count || this.PivotCalculations.Count <= sortColIndex - this.PivotRows.Count || this.PivotCalculations[this.ResolveColumnIndex(sortColIndex) - this.PivotRows.Count].SummaryType != SummaryType.DisplayIfDiscreteValuesEqual) ? (this[rowIndex, sortColIndex] == null ? (IComparable) double.MaxValue : (IComparable) this[rowIndex, sortColIndex].DoubleValue) : (this[rowIndex, sortColIndex].Value == null || this[rowIndex, sortColIndex].Value is string ? (this[rowIndex, sortColIndex].FormattedText != null ? (IComparable) this[rowIndex, sortColIndex].FormattedText : (IComparable) string.Empty) : this[rowIndex, sortColIndex].Value as IComparable);
        sortKeysList1.Add(new PivotEngine.SortKeys()
        {
          Index = rowIndex,
          Keys = comparableArray
        });
      }
      int num1 = 0;
      List<PivotEngine.SortKeys> sortKeysList2 = new List<PivotEngine.SortKeys>();
      if (flag2)
        ++colIndex;
      if (flag1)
        num1 = this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0);
      while (num1 < capacity)
      {
        int num2 = 0;
        int num3 = num1;
        sortKeysList2.Clear();
        for (; num1 < capacity && !flag1 && this[num1, colIndex] != null && (this[num1, colIndex].CellType == PivotCellType.ValueCell || this[num1, colIndex].CellType == (PivotCellType.ValueCell | PivotCellType.GrandTotalCell) || this[num1, colIndex].CellType == (PivotCellType.ValueCell | PivotCellType.TotalCell) || flag2 && this[num1, colIndex].CellType == (PivotCellType.HeaderCell | PivotCellType.RowHeaderCell)) && this[num1, 0] != null && this[num1, 0].ParentCell == null && this[num1, 0].CellType != (this.GridLayout == GridLayout.TopSummary ? PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell : PivotCellType.TotalCell | PivotCellType.RowHeaderCell) && !this.IsRowSummary(this.RowPivotsOnly ? num1 : (this.PivotColumns.Count != 0 || this.PivotCalculations.Count != 1 || this.PivotRows.Count > 1 ? (this.GridLayout == GridLayout.TopSummary ? 0 : num1) : num1 - 1)); ++num1)
        {
          sortKeysList2.Add(new PivotEngine.SortKeys()
          {
            Keys = sortKeysList1[num1].Keys,
            Index = sortKeysList1[num1].Index
          });
          ++num2;
        }
        if (num2 > 0)
        {
          sortKeysList2.Sort((IComparer<PivotEngine.SortKeys>) calcSortComparer);
          for (int index = 0; index < num2; ++index)
            sortKeysList1[index + num3] = sortKeysList2[index];
        }
        while (num1 < capacity && (this[num1, colIndex] == null || this[num1, colIndex].CellType != PivotCellType.ValueCell && flag2 && this[num1, colIndex].CellType != (PivotCellType.HeaderCell | PivotCellType.RowHeaderCell)))
          ++num1;
        if (num3 == num1)
          ++num1;
      }
      this.sortKeys = sortKeysList1;
      if (flag1)
        this.RaisePivotSchemaChangedEvent(new PivotSchemaChangedArgs()
        {
          ChangeHints = SchemaChangeHints.None
        });
      if (!this.ShowEmptyCells || !this.EnableLazyLoadOnDemandCalculations)
        return;
      this.EnableOnDemandCalculations = false;
    }
  }

  public void SortColumnHeader(int colHeaderIndex)
  {
    Dictionary<int, string> dictionColumnHeaders = new Dictionary<int, string>();
    Dictionary<int, List<PivotCellInfo>> tempDictionary = new Dictionary<int, List<PivotCellInfo>>();
    List<string> stringList = new List<string>();
    int col = this.PivotRows.Count;
    this.sortDirection = this.sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
    for (int i = this.PivotRows.Count; i < this.ColumnCount - (this.PivotRows.Count + this.PivotCalculations.Count - 1); ++i)
    {
      if (this[colHeaderIndex, i] != null && this[colHeaderIndex, i].FormattedText == null && this[colHeaderIndex, i].UniqueText.Contains("Total"))
        dictionColumnHeaders.Add(i, this[colHeaderIndex, i].UniqueText);
      else if (this[colHeaderIndex, i] != null)
        dictionColumnHeaders.Add(i, this[colHeaderIndex, i].FormattedText);
      if (this[colHeaderIndex, i] != null && (!stringList.Any<string>((System.Func<string, bool>) (x => x == this[colHeaderIndex, i].FormattedText)) || this[colHeaderIndex, i].FormattedText == null || this[colHeaderIndex, i - 1].FormattedText == null) && !this[colHeaderIndex, i].CellType.ToString().Contains("TotalCell") && (this[colHeaderIndex, i].FormattedText != null || this[colHeaderIndex, i].UniqueText == "x") && !this[colHeaderIndex, i].UniqueText.ToString().Contains("Total"))
        stringList.Add(this[colHeaderIndex, i].FormattedText);
    }
    if (!stringList.Contains((string) null))
    {
      stringList.Reverse();
      this.SwapPivotColumns(colHeaderIndex, dictionColumnHeaders, tempDictionary, stringList, col);
    }
    else
    {
      int index = 0;
      while (index < stringList.Count)
      {
        List<string> uniqueHeader = new List<string>();
        if (stringList[index] == null)
        {
          stringList.RemoveAt(index);
        }
        else
        {
          for (; index < stringList.Count && stringList[index] != null; ++index)
            uniqueHeader.Add(stringList[index]);
          uniqueHeader.Reverse();
          col = this.SwapPivotColumns(colHeaderIndex, dictionColumnHeaders, tempDictionary, uniqueHeader, col);
          if (colHeaderIndex == this.PivotColumns.Count - 1)
            col += this.PivotCalculations.Count;
        }
      }
    }
  }

  public void UpdateAllSummariesRespectingHiddenRowIndexes()
  {
    SummaryBase[] emptySummaries = this.GetEmptySummaries();
    int startRow = 1;
    this.UpdateSummariesForNextLevel(0, emptySummaries, ref startRow);
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
    {
      if (this.pivotValues[startRow, this.PivotRows.Count + index] != null)
        this.pivotValues[startRow, this.PivotRows.Count + index].Summary = emptySummaries[index];
      this.InitSummary(startRow, this.PivotRows.Count + index, emptySummaries[index]);
    }
  }

  public int GetHiddenRowKeyValueColumnIndex()
  {
    if (this.columnIndexes != null && this.columnIndexes.Count > 0)
    {
      for (int index = 0; index < this.columnIndexes.Count; ++index)
      {
        if (this.columnIndexes[index] == this.PivotRows.Count)
          return index;
      }
    }
    return this.PivotRows.Count;
  }

  public PivotCellInfo GetUnindexedPivotCellInfo(int row, int col) => this.pivotValues[row, col];

  public void RaisePivotSchemaChangedEvent(PivotSchemaChangedArgs e)
  {
    if (!this.ShowCalculationsAsColumns && e.ChangeHints == SchemaChangeHints.CalculationChanged)
      this.TransposePivotTable();
    this.OnPivotSchemaChanged(e);
    if (this.ShowCalculationsAsColumns || e.ChangeHints != SchemaChangeHints.CalculationChanged)
      return;
    this.TransposePivotTable();
  }

  internal object GetReflectedValue(object component, string property)
  {
    object reflectedValue = (object) null;
    PropertyInfo propertyInfo1 = (PropertyInfo) null;
    string propName = "";
    if (this.LookUp == null && component != null)
    {
      PropertyInfo[] properties = component.GetType().GetProperties();
      this.LookUp = new Dictionary<string, PropertyInfo>();
      foreach (PropertyInfo propertyInfo2 in properties)
        this.LookUp.Add(propertyInfo2.Name, propertyInfo2);
    }
    if (this.LookUp != null)
    {
      if (this.LookUp.TryGetValue(property, out propertyInfo1))
      {
        reflectedValue = propertyInfo1.GetValue(component, (object[]) null);
      }
      else
      {
        if (this.PivotCalculations.Where<PivotComputationInfo>((System.Func<PivotComputationInfo, bool>) (x => x.FieldName.Contains(property))).FirstOrDefault<PivotComputationInfo>() != null)
          propName = this.PivotCalculations.Where<PivotComputationInfo>((System.Func<PivotComputationInfo, bool>) (x => x.FieldName.Contains(property))).FirstOrDefault<PivotComputationInfo>().FieldName;
        else if (this.PivotRows.Where<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName.Contains(property))).FirstOrDefault<PivotItem>() != null)
          propName = this.PivotRows.Where<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName.Contains(property))).FirstOrDefault<PivotItem>().FieldMappingName;
        else if (this.PivotColumns.Where<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName.Contains(property))).FirstOrDefault<PivotItem>() != null)
          propName = this.PivotColumns.Where<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName.Contains(property))).FirstOrDefault<PivotItem>().FieldMappingName;
        if (((IEnumerable<string>) propName.Split('.')).Count<string>() > 0 && propName.IndexOf(".") > -1 && this.lookUp.Any<KeyValuePair<string, PropertyInfo>>((System.Func<KeyValuePair<string, PropertyInfo>, bool>) (x => propName.Contains(x.Key))))
        {
          object obj = this.lookUp[propName.Remove(propName.IndexOf("."))].GetValue(component, (object[]) null);
          reflectedValue = obj.GetType().GetProperty(property).GetValue(obj, (object[]) null);
        }
      }
    }
    return reflectedValue;
  }

  protected virtual void OnPivotSchemaChanged(PivotSchemaChangedArgs e)
  {
    if (this.PivotSchemaChanged == null)
      return;
    this.PivotSchemaChanged((object) this, e);
  }

  private void UpdateSummariesForNextLevel(
    int levelCol,
    SummaryBase[] parentSummaries,
    ref int startRow)
  {
    if (levelCol == this.PivotRows.Count - 1)
    {
      int rowIndex = startRow;
      for (int count = this.PivotRows.Count; rowIndex < this.RowCount - 1 && this.pivotValues[rowIndex, count].CellType == PivotCellType.ValueCell; ++rowIndex)
      {
        if (!this.HiddenRowIndexes.Contains(this.pivotValues[rowIndex, count]))
        {
          for (int index = 0; index < this.PivotCalculations.Count; ++index)
          {
            if (this.pivotValues[rowIndex, count + index] != null && this.pivotValues[rowIndex, count + index].Summary != null)
              parentSummaries[index].CombineSummary(this.pivotValues[rowIndex, count + index].Summary);
          }
        }
      }
      startRow = rowIndex;
    }
    else
    {
      while (startRow < this.RowCount - this.PivotRows.Count && this.pivotValues[startRow, levelCol] != null && (this.pivotValues[startRow, levelCol].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 && this.pivotValues[startRow, levelCol].CellType != (PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell) && this.pivotValues[startRow, levelCol].Value.ToString() != "x")
      {
        SummaryBase[] emptySummaries = this.GetEmptySummaries();
        this.UpdateSummariesForNextLevel(levelCol + 1, emptySummaries, ref startRow);
        for (int index = 0; index < this.PivotCalculations.Count; ++index)
        {
          if (this.pivotValues[startRow, this.PivotRows.Count + index] != null)
            this.pivotValues[startRow, this.PivotRows.Count + index].Summary = emptySummaries[index];
          this.InitSummary(startRow, this.PivotRows.Count + index, emptySummaries[index]);
        }
        for (int index = 0; index < this.PivotCalculations.Count; ++index)
          parentSummaries[index].CombineSummary(emptySummaries[index]);
        ++startRow;
      }
    }
  }

  private void CombineSummaries(SummaryBase[] levelSummaries, SummaryBase[] parentSummaries)
  {
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
      parentSummaries[index].CombineSummary(levelSummaries[index]);
  }

  private SummaryBase[] GetEmptySummaries()
  {
    SummaryBase[] emptySummaries = new SummaryBase[this.PivotCalculations.Count];
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
      emptySummaries[index] = this.PivotCalculations[index].Summary.GetInstance();
    return emptySummaries;
  }

  private void EnsureCalculationAt(int rowIndex, int columnIndex, bool shouldCalculateTotal)
  {
    if (this.pivotValues[rowIndex, columnIndex] != null && this.pivotValues[rowIndex, columnIndex].Summary == null)
      this.pivotValues[rowIndex, columnIndex].Summary = this.GetOnDemandValue(rowIndex, columnIndex);
    if (!shouldCalculateTotal || this.pivotValues[rowIndex, columnIndex] == null || this.pivotValues[rowIndex, columnIndex].Summary != null || this.pivotValues[rowIndex, columnIndex].CellType == PivotCellType.ValueCell)
      return;
    this.pivotValues[rowIndex, columnIndex].Summary = this.GetOnDemandTotal(rowIndex, columnIndex);
  }

  private void PopulateDefaultSummaryLibrary()
  {
    if (this.SummaryLibrary == null)
      return;
    this.SummaryLibrary.Clear();
    foreach (string computationType in PivotComputationInfo.GetComputationTypes())
      this.SummaryLibrary.Add(computationType, PivotComputationInfo.GetSummaryInstance((SummaryType) Enum.Parse(typeof (SummaryType), computationType)));
  }

  private SummaryBase GetOnDemandTotal(int row, int col)
  {
    SummaryBase sb = (SummaryBase) null;
    if (this.PivotCalculations.Count > 0)
    {
      int index1 = !this.showCalculationsAsColumns ? (row - this.PivotRows.Count) % this.PivotCalculations.Count : (col - this.colOffSet) % this.PivotCalculations.Count;
      sb = this.PivotCalculations[index1].Summary.GetInstance();
      this.InitSummary(row, col, sb);
      bool flag1 = this.GridLayout == GridLayout.TopSummary ? this.pivotValues[this.rowOffSet, col] != null && (PivotCellType) 0 != (this.pivotValues[this.rowOffSet, col].CellType & PivotCellType.ExpanderCell) : this.pivotValues[this.rowOffSet, col] != null && (PivotCellType) 0 != (this.pivotValues[this.rowOffSet, col].CellType & PivotCellType.TotalCell);
      int k = 0;
      if (!flag1 && this.IsSummaryRow(row, ref k) && this.GridLayout != GridLayout.TopSummary || this.GridLayout == GridLayout.TopSummary && !flag1 && this.IsSummaryRowWhileTopSummary(row))
      {
        if (this.RowSummands.ContainsKey(row))
        {
          bool flag2 = false;
          foreach (int rowIndex in this.RowSummands[row])
          {
            if (this[rowIndex, col] != null && this[rowIndex, col].Summary != null)
            {
              sb.CombineSummary(this[rowIndex, col].Summary);
              flag2 = true;
            }
          }
          this.pivotValues[row, col].Summary = sb;
          this.InitSummary(row, col, sb);
          if (!flag2 && this.pivotValues[row, col] != null && this.pivotValues[row, col].FormattedText != null && (this.pivotValues[row, col].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
          {
            this.pivotValues[row, col].Value = (object) "0";
            this.pivotValues[row, col].FormattedText = 0.ToString(this.PivotCalculations[index1].Format);
          }
        }
        else if (this.PivotColumns.Count == 0 && row == 0 && this.pivotValues[row, col] != null)
          this.pivotValues[row, col].FormattedText = this.PivotCalculations[index1].CalculationName;
      }
      else
      {
        int num1 = (col - this.colOffSet) % this.PivotCalculations.Count;
        int num2 = num1;
        int num3 = num1 + 1;
        if (this.ColSummands.ContainsKey(col - num1))
        {
          bool flag3 = false;
          if (this.PivotColumns.Count <= 1 && this.PivotRows.Count == 0)
          {
            flag3 = true;
            num3 = this.PivotCalculations.Count;
            num2 = 0;
          }
          for (int index2 = num2; index2 < num3; ++index2)
          {
            sb = this.PivotCalculations[index2].Summary.GetInstance();
            bool flag4 = false;
            foreach (int num4 in this.colSummands[col - num1])
            {
              if (num4 != 0 && (num4 - (this.ShowCalculationsAsColumns ? this.colOffSet : this.PivotRows.Count + 1) + num1) % this.PivotCalculations.Count == index2 && this[row, num4 + num1] != null && this[row, num4 + num1].Summary != null)
              {
                sb.CombineSummary(this[row, num4 + num1].Summary);
                flag4 = true;
              }
            }
            int num5 = flag3 ? col + index2 : col;
            this.pivotValues[row, num5].Summary = sb;
            this.InitSummary(row, num5, sb);
            if (!flag4 && this.pivotValues[row, num5] != null && (this.pivotValues[row, num5].CellType & PivotCellType.TotalCell) != (PivotCellType) 0 && !this.ShowNullAsBlank)
            {
              this.pivotValues[row, num5].Value = (object) "0";
              this.pivotValues[row, num5].FormattedText = 0.ToString(this.PivotCalculations[index2].Format);
            }
          }
        }
        else if (this.PivotColumns.Count == 0 && row == 0 && this.pivotValues[row, col] != null)
          this.pivotValues[row, col].FormattedText = this.PivotCalculations[index1].CalculationName != null ? this.PivotCalculations[index1].CalculationName : this.PivotCalculations[index1].FieldHeader;
        else if (this.PivotColumns.Count == 0 && row > 0 && this.GridLayout != GridLayout.TopSummary)
        {
          if (this.RowSummands.ContainsKey(row))
          {
            bool flag5 = false;
            foreach (int rowIndex in this.RowSummands[row])
            {
              if (this[rowIndex, col] != null && this[rowIndex, col].Summary != null)
              {
                sb.CombineSummary(this[rowIndex, col].Summary);
                flag5 = true;
              }
            }
            this.pivotValues[row, col].Summary = sb;
            this.InitSummary(row, col, sb);
            if (!flag5 && this.pivotValues[row, col] != null && this.pivotValues[row, col].FormattedText != null && (this.pivotValues[row, col].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
            {
              this.pivotValues[row, col].Value = (object) "0";
              this.pivotValues[row, col].FormattedText = 0.ToString(this.PivotCalculations[index1].Format);
            }
          }
        }
        else if (this.PivotRows.Count == 0 && col == 0 && this.pivotValues[row, col] != null)
        {
          this.pivotValues[row, col].FormattedText = $"{this.GrandString} {this.PivotColumns[0].TotalHeader}";
        }
        else
        {
          int num6 = this.PivotColumns.Count <= 0 || this.pivotRows.Count != 0 || this.PivotCalculations.Count <= 0 ? this.rowOffSet : 1;
          int index3 = (col - num6) % this.PivotCalculations.Count;
          if (this.ColSummands.ContainsKey(col - index3))
          {
            bool flag6 = false;
            foreach (int num7 in this.colSummands[col - index3])
            {
              if (this[row, num7 + index3] != null && this[row, num7 + index3].Summary != null)
              {
                sb.CombineSummary(this[row, num7 + index3].Summary);
                flag6 = true;
              }
            }
            this.pivotValues[row, col].Summary = sb;
            this.InitSummary(row, col, sb);
            if (!flag6 && this.pivotValues[row, col] != null && this.pivotValues[row, col].FormattedText != null && (this.pivotValues[row, col].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
            {
              this.pivotValues[row, col].Value = (object) "0";
              this.pivotValues[row, col].FormattedText = 0.ToString(this.PivotCalculations[index3].Format);
            }
          }
        }
      }
    }
    return sb;
  }

  private SummaryBase GetOnDemandValue(int row, int col)
  {
    SummaryBase sb = (SummaryBase) null;
    if (row <= this.RowCount && col <= this.ColumnCount && this.PivotCalculations.Count > 0)
    {
      List<IComparable> keyAt = this.GetKeyAt(row, col, true);
      int num;
      if (keyAt.Count <= 0)
        num = -1;
      else
        num = this.tableKeysCalcValues.BinarySearch((IComparable) new KeysCalculationValues()
        {
          Keys = keyAt
        });
      int index = num;
      if (index > -1)
      {
        sb = (this.tableKeysCalcValues[index] as KeysCalculationValues).Values[!this.showCalculationsAsColumns ? (row - this.PivotColumns.Count) % this.PivotCalculations.Count : (col - this.colOffSet) % this.PivotCalculations.Count];
        this.InitSummary(row, col, sb);
      }
    }
    return sb;
  }

  private void InitSummary(int row, int col, SummaryBase sb)
  {
    if (sb == null)
      return;
    if (sb.GetResult() == null)
      sb = sb.GetInstance();
    object result = sb.GetResult();
    string format = !this.ShowCalculationsAsColumns ? $"{{0:{this.PivotCalculations[(row - this.PivotRows.Count) % this.PivotCalculations.Count].Format}}}" : $"{{0:{this.PivotCalculations[(col - this.colOffSet) % this.PivotCalculations.Count].Format}}}";
    if (this.PivotCalculations[(col - this.colOffSet) % this.PivotCalculations.Count].CalculationType != CalculationType.Formula)
    {
      if (this.pivotValues[row, col] != null && format.Equals("{0:#.##}") && result != null && result.Equals((object) 0.0) && !this.ShowNullAsBlank && this.pivotValues[row, col].FormattedText == null)
        this.pivotValues[row, col].FormattedText = "0.0";
      else if (this.pivotValues[row, col] != null && (this.pivotValues[row, col].FormattedText != null || this.EnableOnDemandCalculations) && (result != null && !result.Equals((object) 0.0) && !result.Equals((object) 0) && this.ShowNullAsBlank && this.pivotValues[row, col].Value == null || this.pivotValues[row, col].Value != null || !this.ShowNullAsBlank) && col > (this.ShowCalculationsAsColumns ? this.PivotRows.Count - 1 : this.PivotRows.Count) && row > this.PivotColumns.Count - 1 + (!this.showCalculationsAsColumns || !this.IsCalculationHeaderVisible ? 0 : 1))
        this.pivotValues[row, col].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, result);
      if (this.pivotValues[row, col] == null)
        return;
      this.pivotValues[row, col].Value = result;
      this.pivotValues[row, col].Format = this.PivotCalculations[col % this.PivotCalculations.Count].Format;
    }
    else
      this.CalculateFormula(row, col, col - this.colOffSet, this.PivotCalculations[(col - this.colOffSet) % this.PivotCalculations.Count]);
  }

  private void ResetForSwapRowsColumns(bool reset)
  {
    this.lockComputations = true;
    this.PivotColumns.Clear();
    this.PivotRows.Clear();
    this.valuesArea = (SummaryBase[,]) null;
    this.rowHeaders = (IComparable[,]) null;
    this.columnHeaders = (IComparable[,]) null;
    this.rowCount = 0;
    this.columnCount = 0;
    if (reset)
      this.CoveredRanges.Clear();
    this.lockComputations = false;
  }

  private string GetCalculationNameFromGridRowColumnIndex(int index)
  {
    int num = index - (this.ShowCalculationsAsColumns ? this.PivotRows.Count : this.PivotColumns.Count);
    return this.PivotCalculations.Count <= 0 || num < 0 ? "" : this.PivotCalculations[num % this.PivotCalculations.Count].FieldName;
  }

  private List<object> GetRawItemsFor(
    int row,
    int col,
    string rowName,
    string colName,
    int row1,
    int col1)
  {
    List<object> rawItemsFor = new List<object>();
    int num = this.PivotRows.Count > 0 ? this.PivotRows.Count + (this.PivotCalculations.Count > 1 || this.showSingleCalculationHeader ? 1 : 0) : 1;
    if (this.IsSummaryColumn(col) && this.IsRowSummary(row) && this.IsGrandTotalCell(row, col) && this[col, 0] != null && this[col, 0].CellType.ToString().Contains("Grand") && this[0, row] != null && this[0, row].CellType.ToString().Contains("Grand"))
    {
      IList data = !(this.dataSource is DataView) ? (!(this.DataSource is IList) ? this.DataSourceList as IList : this.DataSource as IList) : (IList) new DataView(((DataView) this.DataSourceList).Table);
      PropertyDescriptorCollection descriptorCollection = this.GetPropertyDescriptorCollection(data);
      List<object> rawItemsList = this.GetRawItemsList(data, descriptorCollection);
      this.isForRawItem = false;
      return rawItemsList;
    }
    if (this.PivotColumns.Count == 0)
    {
      if (this.ShowCalculationsAsColumns)
        --col;
      int rowIndex;
      if (!this.IsSummaryColumn(col) && !this.IsRowSummary(row) && !this.IsGrandTotalCell(row, col))
        rawItemsFor = this.GetRawItemsForEach(row, col);
      else if (this.IsSummaryColumn(col))
      {
        for (int index = col; index >= this.PivotColumns.Count && (this.PivotCalculations.Count <= 1 || index == col || this[index, row1] == null || this[index, row1].FormattedText == null || string.IsNullOrEmpty(rowName) || rowName.Contains(this[index, row1].FormattedText) || this.IsGrandTotalCell(row, col)); index = rowIndex - 1)
        {
          foreach (object obj in this.GetRawItemsForEach(row, index))
            rawItemsFor.Add(obj);
          rowIndex = index - this.PivotCalculations.Count + 1;
          if (this.PivotCalculations.Count == 1 && rowIndex != col && this[rowIndex, row1] != null && this[rowIndex, row1].FormattedText != null && !string.IsNullOrEmpty(rowName) && !rowName.Contains(this[rowIndex, row1].FormattedText) && !this.IsGrandTotalCell(row, col))
            break;
        }
      }
      this.isForRawItem = false;
      return rawItemsFor;
    }
    if (this.IsSummaryColumn(col))
    {
      if (!this.IsRowSummary(row) && !this.IsGrandTotalCell(row, col))
      {
        for (int index = col; index >= this.PivotColumns.Count && (index == col || this[index, row1] == null || this[index, row1].FormattedText == null || string.IsNullOrEmpty(rowName) || rowName.Contains(this[index, row1].FormattedText)); index = index - this.PivotCalculations.Count + 1 - 1)
        {
          foreach (object obj in this.GetRawItemsForEach(row, index))
            rawItemsFor.Add(obj);
        }
      }
      else if (!this.IsRowSummary(row) && this.IsGrandTotalCell(row, col))
      {
        for (int col2 = col; col2 >= this.PivotColumns.Count; col2 = col2 - this.PivotCalculations.Count + 1 - 1)
        {
          foreach (object obj in this.GetRawItemsForEach(row, col2))
            rawItemsFor.Add(obj);
        }
      }
      else if (this.IsRowSummary(row))
      {
        for (int index1 = col; index1 >= this.PivotColumns.Count && (index1 == col || this[index1, row1] == null || this[index1, row1].FormattedText == null || string.IsNullOrEmpty(rowName) || rowName.Contains(this[index1, row1].FormattedText) || this.IsGrandTotalCell(row, col) && (!this.IsGrandTotalCell(row, index1) || this[index1, row].CellType != (PivotCellType.ValueCell | PivotCellType.GrandTotalCell))); index1 = index1 - this.PivotCalculations.Count + 1 - 1)
        {
          for (int index2 = row; index2 >= num && (index2 == row || this[col1, index2] == null || this[col1, index2].FormattedText == null || string.IsNullOrEmpty(colName) || colName.Contains(this[col1, index2].FormattedText) || this[index1, row].CellType == (PivotCellType.ValueCell | PivotCellType.GrandTotalCell)); --index2)
          {
            foreach (object obj in this.GetRawItemsForEach(index2, index1))
              rawItemsFor.Add(obj);
          }
        }
      }
      this.isForRawItem = false;
      return rawItemsFor;
    }
    if (this.IsRowSummary(row))
    {
      for (int index = row; index >= num && (index == row || this[col1, index] == null || this[col1, index].FormattedText == null || string.IsNullOrEmpty(colName) || colName.Contains(this[col1, index].FormattedText) || this.IsGrandTotalCell(row, col)); --index)
      {
        foreach (object obj in this.GetRawItemsForEach(index, col))
          rawItemsFor.Add(obj);
      }
      this.isForRawItem = false;
      return rawItemsFor;
    }
    List<object> rawItemsForEach = this.GetRawItemsForEach(row, col);
    this.isForRawItem = false;
    return rawItemsForEach;
  }

  private List<object> GetRawItemsList(IList data, PropertyDescriptorCollection pdc)
  {
    List<object> rawItemsList = new List<object>();
    foreach (object obj in (IEnumerable) data)
    {
      object item = obj;
      flag = true;
      foreach (FilterExpression filter in this.Filters)
      {
        FilterExpression exp = filter;
        FilterItemElement filterItemElement = exp.Tag == null || !(exp.Tag is FilterItemsCollection) ? (FilterItemElement) null : (exp.Tag as FilterItemsCollection).Where<FilterItemElement>((System.Func<FilterItemElement, bool>) (x => x.Key == pdc[exp.Name].GetValue(item).ToString())).FirstOrDefault<FilterItemElement>();
        if (!((!(item is DataRowView) ? exp.ComputedValue(item) : (object) (bool) (filterItemElement != null ? (exp.Expression.Contains(filterItemElement.Key) ? 1 : 0) : 0)) is bool flag))
          ;
        if (!flag)
          break;
      }
      if (flag)
        rawItemsList.Add(item);
    }
    return rawItemsList;
  }

  private void UpdateRowHeaderForExcelLayout()
  {
    for (int count = this.PivotColumns.Count; count < this.RowCount - 1; ++count)
    {
      for (int colIndex = 1; colIndex < this.PivotRows.Count; ++colIndex)
      {
        if (this.PivotValues[count, 0] != null && this.PivotValues[count, colIndex] != null && this.PivotValues[count, colIndex].FormattedText != null && (this.ShowCalculationsAsColumns || !this.ShowCalculationsAsColumns && !this.PivotValues[count, 0].CellType.ToString().Contains("ExpanderCell")))
        {
          this.CoveredRanges.Remove(this.PivotValues[count, 0].CellRange);
          this.PivotValues[count, 0] = this.PivotValues[count, colIndex];
          this.PivotValues[count, colIndex - 1] = this.PivotValues[count, colIndex];
        }
      }
    }
  }

  private List<object> GetCachedRawValues(int row, int col)
  {
    List<object> cachedRawValues = (List<object>) null;
    if ((!this.IsRowSummaryWhileOnDemand(row) || this.HiddenPivotRowGroups.Count <= 0) && (!this.IsSummaryColumnWhileOnDemand(col) || this.HiddenPivotColumnGroups.Count <= 0))
      cachedRawValues = this[row, col].RawValues;
    if (cachedRawValues == null)
    {
      int index1 = this.tableKeysCalcValues.BinarySearch((IComparable) new KeysCalculationValues()
      {
        Keys = this.GetKeyAt(row, col, true)
      });
      if (index1 > -1)
        cachedRawValues = ((KeysCalculationValues) this.tableKeysCalcValues[index1]).RawValues;
      else if (this.pivotValues[row, col] != null && this.pivotValues[row, col].Value != null)
      {
        HashSet<object> collection = new HashSet<object>();
        int num1 = this.ShowCalculationsAsColumns ? this.PivotRows.Count : this.PivotRows.Count + 1;
        cachedRawValues = new List<object>();
        bool flag1 = this.IsRowSummaryWhileOnDemand(row);
        bool flag2 = this.IsSummaryColumnWhileOnDemand(col);
        if (flag1 && !flag2)
        {
          int num2 = Math.Max(1, this.PivotCalculations.Count);
          int bottom = row;
          if (this.showCalculationsAsColumns)
          {
            --bottom;
          }
          else
          {
            while ((bottom - this.colOffSet) % num2 != 0 && bottom > this.colOffSet)
              --bottom;
          }
          int columnIndex1 = this.PivotRows.Count - 1;
          int num3 = 0;
          if (!this.showCalculationsAsColumns)
            num3 = (row - this.pivotColumns.Count) % this.pivotCalculations.Count;
          while (columnIndex1 > 0 && this[row - num3, columnIndex1] != null && (this[row - num3, columnIndex1].CellRange == null || this[row - num3, columnIndex1].UniqueText == "x"))
            --columnIndex1;
          PivotCellInfo pivotCellInfo = (PivotCellInfo) null;
          bool flag3 = (this[row, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0;
          bool flag4 = false;
          while (bottom >= this.PivotColumns.Count + (!this.showCalculationsAsColumns || !this.IsCalculationHeaderVisible ? 0 : 1) && ((pivotCellInfo = this[bottom, columnIndex1]) != null && !flag4 || flag3))
          {
            if (this.HiddenPivotRowGroups.Values.Any<List<HiddenGroup>>((System.Func<List<HiddenGroup>, bool>) (x => x.Any<HiddenGroup>((System.Func<HiddenGroup, bool>) (n => bottom >= n.From && bottom <= n.To)))))
            {
              if (this.showCalculationsAsColumns)
                --bottom;
              else
                bottom -= num2;
            }
            else
            {
              flag4 = (this[bottom, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0;
              if (this[bottom, col].CellType == PivotCellType.ValueCell)
              {
                List<object> rawItemsFor = this.GetRawItemsFor(bottom, col);
                if (rawItemsFor != null)
                {
                  foreach (object obj in rawItemsFor)
                    collection.Add(obj);
                }
              }
              if (this.showCalculationsAsColumns)
                --bottom;
              else
                bottom -= num2;
            }
          }
          cachedRawValues.AddRange((IEnumerable<object>) collection);
        }
        else if (!flag1 && flag2)
        {
          int num4 = Math.Max(1, this.PivotCalculations.Count);
          int right = col;
          if (this.showCalculationsAsColumns)
          {
            while ((right - this.colOffSet) % num4 != 0 && right > this.colOffSet)
              --right;
          }
          int rowIndex = this.PivotColumns.Count - 1;
          while (rowIndex > 0 && this[rowIndex, right] != null && (this[rowIndex, right].CellRange == null || this[rowIndex, right].UniqueText == "x"))
            --rowIndex;
          if (this.showCalculationsAsColumns)
            right -= num4;
          bool flag5 = (this[rowIndex, col].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0;
          bool flag6 = false;
          PivotCellInfo pivotCellInfo;
          while (right >= num1 && ((pivotCellInfo = this[rowIndex, right]) != null && !flag6 || flag5))
          {
            if (this.HiddenPivotColumnGroups.Values.Any<List<HiddenGroup>>((System.Func<List<HiddenGroup>, bool>) (x => x.Any<HiddenGroup>((System.Func<HiddenGroup, bool>) (n => right >= n.From && right <= n.To)))))
            {
              if (this.showCalculationsAsColumns)
                right -= num4;
              else
                --right;
            }
            else
            {
              flag6 = (pivotCellInfo.CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0;
              if (this[row, right].CellType == PivotCellType.ValueCell)
              {
                List<object> rawItemsFor = this.GetRawItemsFor(row, right);
                if (rawItemsFor != null)
                {
                  foreach (object obj in rawItemsFor)
                    collection.Add(obj);
                }
              }
              if (this.showCalculationsAsColumns)
                right -= num4;
              else
                --right;
            }
          }
          cachedRawValues.AddRange((IEnumerable<object>) collection);
        }
        else
        {
          int num5 = Math.Max(1, this.PivotCalculations.Count);
          int right = col;
          if (this.showCalculationsAsColumns)
          {
            while ((right - num1) % num5 != 0 && right > num1)
              --right;
          }
          int rowIndex = this.PivotColumns.Count - 1;
          while (rowIndex > 0 && this[rowIndex, right] != null && (this[rowIndex, right].CellRange == null || this[rowIndex, right].UniqueText == "x"))
            --rowIndex;
          if (this.showCalculationsAsColumns)
            right -= num5;
          else
            --right;
          PivotCellInfo pivotCellInfo1 = (PivotCellInfo) null;
          bool flag7 = (this[rowIndex, col].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0;
          bool flag8 = false;
          PivotCellInfo pivotCellInfo2;
          while (right >= num1 && ((pivotCellInfo2 = this[rowIndex, right]) != null && !flag8 || flag7))
          {
            if (this.HiddenPivotColumnGroups.Values.Any<List<HiddenGroup>>((System.Func<List<HiddenGroup>, bool>) (x => x.Any<HiddenGroup>((System.Func<HiddenGroup, bool>) (n => right >= n.From && right <= n.To)))))
            {
              if (this.showCalculationsAsColumns)
                right -= num5;
              else
                --right;
            }
            else
            {
              flag8 = (pivotCellInfo2.CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0;
              pivotCellInfo1 = this[row, right];
              bool flag9 = false;
              for (int index2 = 0; index2 < row; ++index2)
              {
                PivotCellInfo pivotCellInfo3;
                if (row - index2 >= 0 && (pivotCellInfo3 = this[row - index2, right]) != null && pivotCellInfo3.CellType == PivotCellType.ValueCell && !this.HiddenPivotRowGroups.Values.Any<List<HiddenGroup>>((System.Func<List<HiddenGroup>, bool>) (x => x.Any<HiddenGroup>((System.Func<HiddenGroup, bool>) (n => row >= n.From && row <= n.To)))))
                {
label_77:
                  while (right > this.pivotRows.Count + 1 && this.pivotValues[row, right] != null && this.pivotValues[row, right].Value == null)
                  {
                    --right;
                    while (true)
                    {
                      if (right > this.pivotRows.Count + 1 && this.pivotValues[row - index2, right] != null && this.pivotValues[row - index2, right].CellType != PivotCellType.ValueCell)
                        --right;
                      else
                        goto label_77;
                    }
                  }
                  flag9 = true;
                  break;
                }
              }
              if (flag9)
              {
                List<object> rawItemsFor = this.GetRawItemsFor(row, right);
                if (rawItemsFor != null)
                {
                  foreach (object obj in rawItemsFor)
                    collection.Add(obj);
                }
              }
              if (this.showCalculationsAsColumns)
                right -= num5;
              else
                --right;
            }
          }
          cachedRawValues.AddRange((IEnumerable<object>) collection);
        }
      }
      this[row, col].RawValues = cachedRawValues;
    }
    return cachedRawValues;
  }

  private PropertyDescriptorCollection GetPropertyDescriptorCollection(IList data)
  {
    if (this.DataSource == null)
      return (PropertyDescriptorCollection) null;
    PropertyDescriptorCollection descriptorCollection = (PropertyDescriptorCollection) null;
    if (data is ITypedList typedList)
      descriptorCollection = typedList.GetItemProperties((PropertyDescriptor[]) null);
    else if (data.Count > 0)
    {
      object obj = data[0];
      descriptorCollection = !(obj is ICustomTypeDescriptor customTypeDescriptor) ? TypeDescriptor.GetProperties(obj.GetType()) : customTypeDescriptor.GetProperties();
    }
    return descriptorCollection;
  }

  private void CalculateValues()
  {
    bool calculationHeaderVisible = this.IsCalculationHeaderVisible;
    int countInValuesArea1 = this.GetRowCountInValuesArea();
    int countInValuesArea2 = this.GetColumnCountInValuesArea();
    this.rowHeaders = new IComparable[countInValuesArea1 + this.PivotColumns.Count + (calculationHeaderVisible ? 1 : 0), this.PivotRows.Count];
    this.rowSummary = new bool[countInValuesArea1 + this.PivotColumns.Count + (calculationHeaderVisible ? 1 : 0)];
    this.rowHeaderUniqueValues = new string[countInValuesArea1 + this.PivotColumns.Count + (calculationHeaderVisible ? 1 : 0), this.PivotRows.Count];
    this.PopulateRowHeaders();
    int length1 = this.PivotColumns.Count + (calculationHeaderVisible ? 1 : 0);
    int length2 = countInValuesArea2 + this.PivotRows.Count;
    this.columnHeaders = new IComparable[length1, length2];
    this.columnSummary = new bool[length2];
    this.columnHeaderUniqueValues = new string[length1, length2];
    this.PopulateColumnHeaders();
    this.valuesArea = new SummaryBase[countInValuesArea1 - this.grandRowIndex, countInValuesArea2 - this.grandColumnIndex];
    this.rowOffSet = this.PivotColumns.Count + (calculationHeaderVisible ? 1 : 0);
    this.colOffSet = this.PivotRows.Count;
    if (this.EnableOnDemandCalculations && this.colOffSet == 0 && this.PivotColumns.Count > 0 && this.PivotCalculations.Count > 0)
      this.colOffSet = 1;
    if (this.EnableDataOptimization)
    {
      int num = this.highRowLevel != 0 || this.initialRowLoadAmount >= countInValuesArea1 ? countInValuesArea1 : this.initialRowLoadAmount;
      for (int i = 0; i <= num; ++i)
        this.DoCalculationTable(i);
      this.ProcessTotals3();
      for (int row = 0; row <= num; ++row)
      {
        this.PopulateRow(row);
        this.CoverRowHeader(row);
      }
      this.CoverColumnHeaders();
      this.MarkGrandTotalCellType();
    }
    else
    {
      if (this.EnableOnDemandCalculations)
        return;
      this.DoCalculationTable();
    }
  }

  private void SetSummands()
  {
    this.ProcessColSums();
    this.ProcessRowSums();
  }

  private void ProcessRowSums()
  {
    this.RowSummands.Clear();
    if (this.PivotRows.Count == 0)
      return;
    int colIndex = 0;
    int num1 = Math.Max(1, this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0));
    List<int>[] intListArray = new List<int>[this.PivotRows.Count];
    for (int index = 0; index < this.PivotRows.Count; ++index)
      intListArray[index] = new List<int>();
    int num2 = 1;
    int index1 = 0;
    if (this.GridLayout != GridLayout.TopSummary)
    {
label_16:
      while (num1 < this.RowCount - num2 - 1)
      {
        if (colIndex < this.PivotRows.Count)
        {
          if (this.PivotValues[num1, colIndex] != null && (this.PivotValues[num1, colIndex].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0)
          {
            CoveredCellRange coveredCellRange = colIndex > 0 ? this.PivotValues[num1, colIndex - 1].CellRange : new CoveredCellRange(0, 0, 0, 0);
            for (int index2 = 0; index2 <= coveredCellRange.Bottom - coveredCellRange.Top; index2 += num2)
              intListArray[index1].Add(index2 + num1);
            num1 += coveredCellRange.Bottom - coveredCellRange.Top + 1;
            bool flag = false;
            while (true)
            {
              if (index1 > 0 && colIndex > 0 && !flag)
              {
                --colIndex;
                --index1;
                flag = this.PivotValues[num1, colIndex] == null || (PivotCellType) 0 == (this.PivotValues[num1, colIndex].CellType & PivotCellType.TotalCell);
                if (!flag)
                {
                  this.RowSummands.Add(num1, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                  intListArray[index1 + 1].Clear();
                  intListArray[index1].Add(num1);
                  num1 += num2;
                }
                else
                {
                  ++colIndex;
                  ++index1;
                }
              }
              else
                goto label_16;
            }
          }
          else
          {
            ++colIndex;
            ++index1;
          }
        }
      }
    }
    else
    {
      List<int> intList = new List<int>();
      int index3 = 0;
      int num3 = Math.Max(1, this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0));
      for (int rowIndex = num3; rowIndex < this.RowCount; ++rowIndex)
      {
        if (this.PivotValues[rowIndex, 0] != null && this.PivotValues[rowIndex, 0].CellType == (PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell))
          intList.Add(rowIndex);
      }
      if (this.PivotRows.Count > 3)
        num3 = this.PivotRows.Count - 2;
label_42:
      while (num1 < this.RowCount - num2 - 1)
      {
        if (colIndex < this.PivotRows.Count)
        {
          if (this.PivotValues[num1, colIndex] != null && (this.PivotValues[num1, colIndex].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0)
          {
            CoveredCellRange coveredCellRange = colIndex > 0 ? this.PivotValues[num1, colIndex - 1].CellRange : new CoveredCellRange(0, 0, 0, 0);
            for (int index4 = 0; index4 <= coveredCellRange.Bottom - coveredCellRange.Top; index4 += num2)
              intListArray[index1].Add(index4 + num1);
            int num4 = num1 - 1;
            num1 += coveredCellRange.Bottom - coveredCellRange.Top + 1;
            bool flag = false;
            while (true)
            {
              if (index1 > 0 && colIndex > 0 && !flag)
              {
                --colIndex;
                --index1;
                flag = this.PivotValues[num4, colIndex] == null || this.PivotValues[num4, colIndex].CellType != (PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell);
                if (flag)
                {
                  flag = this.PivotValues[num1, colIndex] == null || this.PivotValues[num1, colIndex].CellType != (PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell) && (PivotCellType) 0 == (this.PivotValues[num1, colIndex].CellType & PivotCellType.GrandTotalCell);
                  if (!flag)
                  {
                    if (this.PivotRows.Count > 3 && this.PivotValues[num3, 0] != null && (this.PivotValues[num3, 0].CellType == (PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell) || (this.PivotValues[num3, 0].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0))
                    {
                      int key = intList[index3];
                      ++index3;
                      this.RowSummands.Add(key, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                      intListArray[index1 + 1].Clear();
                      intListArray[index1].Add(key);
                      num3 = num1 + 1;
                    }
                    else
                    {
                      this.RowSummands.Add(num3, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                      intListArray[index1 + 1].Clear();
                      intListArray[index1].Add(num3);
                      num3 = num1;
                    }
                  }
                  else
                  {
                    ++colIndex;
                    ++index1;
                  }
                }
                else if (!flag)
                {
                  this.RowSummands.Add(num4, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                  intListArray[index1 + 1].Clear();
                  intListArray[index1].Add(num4);
                }
                else
                {
                  ++colIndex;
                  ++index1;
                }
              }
              else
                goto label_42;
            }
          }
          else
          {
            ++num1;
            ++colIndex;
            ++index1;
          }
        }
      }
    }
    this.RowSummands.Add(num1, new List<int>((IEnumerable<int>) intListArray[0]));
    intListArray[0].Clear();
  }

  private void ProcessColSums()
  {
    this.ColSummands.Clear();
    if (this.PivotColumns.Count == 0)
      return;
    int count1 = this.PivotRows.Count;
    int rowIndex = 0;
    List<int>[] intListArray = new List<int>[this.PivotColumns.Count];
    for (int index = 0; index < this.PivotColumns.Count; ++index)
      intListArray[index] = new List<int>();
    int count2 = this.PivotCalculations.Count;
    int index1 = 0;
    if (this.GridLayout != GridLayout.TopSummary)
    {
label_16:
      while (count1 < this.ColumnCount - count2 - 1)
      {
        if (rowIndex < this.PivotColumns.Count)
        {
          if (this.PivotValues[rowIndex, count1] != null && (this.PivotValues[rowIndex, count1].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0)
          {
            CoveredCellRange coveredCellRange = rowIndex > 0 ? this.PivotValues[rowIndex - 1, count1].CellRange : new CoveredCellRange(0, 0, 0, 0);
            for (int index2 = 0; index2 <= coveredCellRange.Right - coveredCellRange.Left; index2 += count2)
              intListArray[index1].Add(index2 + count1);
            count1 += coveredCellRange.Right - coveredCellRange.Left + 1;
            bool flag = false;
            while (true)
            {
              if (index1 > 0 && rowIndex > 0 && !flag)
              {
                --rowIndex;
                --index1;
                flag = this.PivotValues[rowIndex, count1] == null || (PivotCellType) 0 == (this.PivotValues[rowIndex, count1].CellType & PivotCellType.TotalCell);
                if (!flag)
                {
                  this.ColSummands.Add(count1, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                  intListArray[index1 + 1].Clear();
                  intListArray[index1].Add(count1);
                  count1 += count2;
                }
                else
                {
                  ++rowIndex;
                  ++index1;
                }
              }
              else
                goto label_16;
            }
          }
          else
          {
            ++rowIndex;
            ++index1;
          }
        }
      }
    }
    else
    {
      List<int> intList = new List<int>();
      int index3 = 0;
      int num1 = count1;
      for (int colIndex = num1; colIndex < this.ColumnCount; ++colIndex)
      {
        if (this.PivotValues[0, colIndex] != null && this.PivotValues[0, colIndex].CellType == (PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell))
          intList.Add(colIndex);
      }
      if (this.PivotColumns.Count > 3)
        num1 = (this.PivotColumns.Count - 3) * this.PivotCalculations.Count + 1;
label_42:
      while (count1 < this.ColumnCount - count2 - 1)
      {
        if (rowIndex < this.PivotColumns.Count)
        {
          if (this.PivotValues[rowIndex, count1] != null && (this.PivotValues[rowIndex, count1].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0)
          {
            CoveredCellRange coveredCellRange = rowIndex > 0 ? this.PivotValues[rowIndex - 1, count1].CellRange : new CoveredCellRange(0, 0, 0, 0);
            for (int index4 = 0; index4 <= coveredCellRange.Right - coveredCellRange.Left; index4 += count2)
              intListArray[index1].Add(index4 + count1);
            int num2 = count1 - this.PivotCalculations.Count;
            count1 += coveredCellRange.Right - coveredCellRange.Left + 1;
            bool flag = false;
            while (true)
            {
              if (index1 > 0 && rowIndex > 0 && !flag)
              {
                --rowIndex;
                --index1;
                flag = this.PivotValues[rowIndex, num2] == null || this.PivotValues[rowIndex, num2].CellType != (PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell) && (PivotCellType) 0 == (this.PivotValues[rowIndex, num2].CellType & PivotCellType.GrandTotalCell);
                if (flag)
                {
                  flag = this.PivotValues[rowIndex, count1] == null || this.PivotValues[rowIndex, count1].CellType != (PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell) && this.PivotValues[rowIndex, count1].CellType != (PivotCellType.ColumnHeaderCell | PivotCellType.GrandTotalCell);
                  if (!flag)
                  {
                    if (this.PivotColumns.Count > 3 && this.PivotValues[0, num1] != null && (this.PivotValues[0, num1].CellType == (PivotCellType.ExpanderCell | PivotCellType.ColumnHeaderCell) || (this.PivotValues[0, num1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0))
                    {
                      int key = intList[index3];
                      ++index3;
                      this.ColSummands.Add(key, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                      intListArray[index1 + 1].Clear();
                      intListArray[index1].Add(key);
                      num1 = count1 + this.PivotCalculations.Count;
                    }
                    else
                    {
                      this.ColSummands.Add(num1, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                      intListArray[index1 + 1].Clear();
                      intListArray[index1].Add(num1);
                      num1 = count1;
                    }
                  }
                  else
                  {
                    ++rowIndex;
                    ++index1;
                  }
                }
                else if (!flag)
                {
                  this.ColSummands.Add(num2, new List<int>((IEnumerable<int>) intListArray[index1 + 1]));
                  intListArray[index1 + 1].Clear();
                  intListArray[index1].Add(num2);
                }
                else
                {
                  ++rowIndex;
                  ++index1;
                }
              }
              else
                goto label_42;
            }
          }
          else
          {
            count1 += this.PivotCalculations.Count;
            ++rowIndex;
            ++index1;
          }
        }
      }
    }
    this.ColSummands.Add(count1, new List<int>((IEnumerable<int>) intListArray[0]));
    intListArray[0].Clear();
  }

  private void RemoveDelimeter()
  {
    if (this.pivotValues == null)
      return;
    for (int index = 0; index < (this.PivotColumns.Count > 0 ? this.PivotColumns.Count : this.PivotValues.GetLength(0)); ++index)
    {
      for (int colIndex = 0; colIndex < this.pivotValues.GetLength(1); ++colIndex)
      {
        if (this.pivotValues[index, colIndex] != null && this.pivotValues[index, colIndex].FormattedText != null && this.pivotValues[index, colIndex].FormattedText.IndexOf(this.delimiter) > -1)
        {
          if (index < this.PivotColumns.Count && string.IsNullOrEmpty(this.PivotColumns[index].TotalHeader))
          {
            if (this.pivotValues[index, colIndex].Value != null)
              this.pivotValues[index, colIndex].Value = (object) this.pivotValues[index, colIndex].Value.ToString().Remove(this.pivotValues[index, colIndex].Value.ToString().IndexOf(this.delimiter));
            this.pivotValues[index, colIndex].FormattedText = this.pivotValues[index, colIndex].FormattedText.Remove(this.pivotValues[index, colIndex].FormattedText.IndexOf(this.delimiter));
          }
          else
            this.pivotValues[index, colIndex].FormattedText = this.pivotValues[index, colIndex].FormattedText.Replace(this.delimiter, " ");
        }
      }
    }
    for (int rowIndex = 0; rowIndex < this.pivotValues.GetLength(0); ++rowIndex)
    {
      for (int index = 0; index < (this.PivotRows.Count > 0 ? this.PivotRows.Count : this.PivotValues.GetLength(1) - 1); ++index)
      {
        if (this.pivotValues[rowIndex, index] != null && this.pivotValues[rowIndex, index].FormattedText != null && this.pivotValues[rowIndex, index].FormattedText.IndexOf(this.delimiter) > -1)
        {
          if (index < this.PivotRows.Count && string.IsNullOrEmpty(this.PivotRows[index].TotalHeader))
          {
            if (this.pivotValues[rowIndex, index].Value != null)
              this.pivotValues[rowIndex, index].Value = (object) this.pivotValues[rowIndex, index].Value.ToString().Remove(this.pivotValues[rowIndex, index].Value.ToString().IndexOf(this.delimiter));
            this.pivotValues[rowIndex, index].FormattedText = this.pivotValues[rowIndex, index].FormattedText.Remove(this.pivotValues[rowIndex, index].FormattedText.IndexOf(this.delimiter));
          }
          else
            this.pivotValues[rowIndex, index].FormattedText = this.pivotValues[rowIndex, index].FormattedText.Replace(this.delimiter, " ");
        }
      }
    }
  }

  private void PopulateRowHeaders()
  {
    if (this.rowKeysCalcValues == null)
      return;
    int rowIndex = this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0);
    KeysCalculationValues calculationValues = (KeysCalculationValues) null;
    int num1 = -1;
    BinaryList binaryList = new BinaryList();
    int index1 = 0;
    foreach (KeysCalculationValues rowKeysCalcValue in (List<IComparable>) this.rowKeysCalcValues)
    {
      if (rowKeysCalcValue.Keys == null && calculationValues != null && calculationValues.Keys != null)
      {
        ++num1;
        int index2 = calculationValues.Keys.Count - 2 - num1;
        if (index2 > -1 && (!this.EnableSubTotalHiding || this.EnableSubTotalHiding && this.pivotRows[index2].ShowSubTotal))
        {
          string str = calculationValues.Keys[index2].ToString() + this.delimiter + this.PivotRows[index2].TotalHeader;
          this.rowHeaders[rowIndex, index2] = (IComparable) str;
          for (int index3 = 0; index3 < calculationValues.Keys.Count - 1; ++index3)
          {
            if (index2 != index3)
            {
              if (this.GridLayout == GridLayout.ExcelLikeLayout)
              {
                string[,] headerUniqueValues;
                IntPtr index4;
                IntPtr index5;
                (headerUniqueValues = this.rowHeaderUniqueValues)[(int) (index4 = (IntPtr) rowIndex), (int) (index5 = (IntPtr) index2)] = $"{headerUniqueValues[(int) index4, (int) index5]}{(object) calculationValues.Keys[index3]}.";
              }
              else
                this.rowHeaderUniqueValues[rowIndex, index2] = calculationValues.Keys[index3].ToString() + ".";
            }
            else
            {
              string[,] headerUniqueValues;
              IntPtr index6;
              IntPtr index7;
              (headerUniqueValues = this.rowHeaderUniqueValues)[(int) (index6 = (IntPtr) rowIndex), (int) (index7 = (IntPtr) index2)] = headerUniqueValues[(int) index6, (int) index7] + str;
              break;
            }
          }
          for (int index8 = 0; index8 < index2; ++index8)
            this.rowHeaders[rowIndex, index8] = calculationValues.Keys[index8];
          for (int index9 = index2 + 1; index9 < calculationValues.Keys.Count; ++index9)
          {
            if (this.ShowSubTotalsForChildren && this.PivotRows.Count > 2 && str.Contains(calculationValues.Keys[0].ToString()) && index9 == calculationValues.Keys.Count - 1)
            {
              this.rowHeaders[rowIndex, index9] = binaryList[0];
              this.rowHeaderUniqueValues[rowIndex, index9] = this.rowHeaders[rowIndex, 0].ToString() + this.delimiter + binaryList[0].ToString() + this.PivotRows[this.PivotRows.Count - 1].TotalHeader;
              index1 = rowIndex;
            }
            else
              this.rowHeaders[rowIndex, index9] = (IComparable) null;
          }
          this.SetRowSummary(rowIndex);
          ++rowIndex;
        }
        else if (index2 == -1 && this.ShowSubTotalsForChildren)
        {
          for (int index10 = 1; index10 <= binaryList.Count; ++index10)
          {
            this.rowHeaders[rowIndex, 0] = this.rowHeaders[index1, 0];
            this.rowHeaderUniqueValues[rowIndex, 0] = this.rowHeaders[index1, 0].ToString();
            this.rowHeaders[rowIndex, calculationValues.Keys.Count - 1] = index10 < binaryList.Count ? binaryList[index10] : (IComparable) this.PivotRows[this.PivotRows.Count - 1].TotalHeader;
            this.rowHeaderUniqueValues[rowIndex, calculationValues.Keys.Count - 1] = index10 < binaryList.Count ? this.rowHeaders[index1, 0].ToString() + this.delimiter + binaryList[index10].ToString() : this.rowHeaders[index1, 0].ToString() + this.delimiter + this.PivotRows[this.PivotRows.Count - 1].TotalHeader;
            this.SetRowSummary(rowIndex);
            ++rowIndex;
          }
          binaryList.Clear();
        }
      }
      else
      {
        num1 = -1;
        calculationValues = rowKeysCalcValue;
        int index11 = 0;
        if (rowKeysCalcValue.Keys != null)
        {
          for (int index12 = 0; index12 < rowKeysCalcValue.Keys.Count; ++index12)
          {
            this.rowHeaders[rowIndex, index11] = rowKeysCalcValue.Keys[index12];
            for (int index13 = 0; index13 <= index12; ++index13)
              this.rowHeaderUniqueValues[rowIndex, index11] = index13 != 0 || rowKeysCalcValue.Keys[index13] == null ? $"{this.rowHeaderUniqueValues[rowIndex, index11]}.{Convert.ToString((object) rowKeysCalcValue.Keys[index13])}" : rowKeysCalcValue.Keys[index13].ToString();
            ++index11;
          }
        }
        this.SetRowSummary(rowIndex);
        ++rowIndex;
        if (this.PivotRows.Count > 0 && this.ShowSubTotalsForChildren)
          binaryList.AddIfUnique(calculationValues.Keys[this.PivotRows.Count - 1]);
      }
    }
    if (this.rowHeaders.GetLength(1) <= 0 || rowIndex >= this.rowHeaders.GetLength(0))
      return;
    if (this.pivotRows[0].TotalHeader == null)
    {
      this.rowHeaders[rowIndex, 0] = (IComparable) (this.GrandString + this.delimiter);
      this.rowHeaderUniqueValues[rowIndex, 0] = this.GrandString + this.delimiter;
    }
    else
    {
      this.rowHeaders[rowIndex, 0] = (IComparable) (this.GrandString + this.delimiter + this.PivotRows[0].TotalHeader);
      this.rowHeaderUniqueValues[rowIndex, 0] = this.GrandString + this.delimiter + this.PivotRows[0].TotalHeader;
      this.grandRowIndex = this.rowHeaders.GetLength(0) - rowIndex - 1;
    }
    this.SetRowSummary(rowIndex);
    int num2 = rowIndex + 1;
  }

  private void SetRowSummary(int rowIndex)
  {
    if (this.IsRowSummary(rowIndex))
      this.rowSummary[rowIndex] = true;
    else
      this.rowSummary[rowIndex] = false;
  }

  private void SetColumnSummary(int columnIndex)
  {
    if (this.IsSummaryColumn(columnIndex))
      this.columnSummary[columnIndex] = true;
    else
      this.columnSummary[columnIndex] = false;
  }

  private void PopulateColumnHeaders()
  {
    if (this.columnKeysCalcValues == null)
      return;
    bool calculationHeaderVisible = this.IsCalculationHeaderVisible;
    int columnIndex = this.PivotRows.Count != 0 || this.PivotCalculations.Count != 0 ? this.PivotRows.Count : 1;
    int num1 = calculationHeaderVisible ? this.PivotCalculations.Count : 1;
    int count = this.PivotColumns.Count;
    string str1 = string.Empty;
    KeysCalculationValues calculationValues = (KeysCalculationValues) null;
    BinaryList binaryList = new BinaryList();
    int index1 = 0;
    int num2 = -1;
    string str2 = "";
    foreach (KeysCalculationValues columnKeysCalcValue in (List<IComparable>) this.columnKeysCalcValues)
    {
      if (columnKeysCalcValue.Keys == null && calculationValues != null)
      {
        ++num2;
        if (calculationValues.Keys != null)
        {
          for (int index2 = 0; index2 < num1; ++index2)
          {
            int index3 = calculationValues.Keys.Count - 2 - num2;
            if (index3 > -1 && (!this.EnableSubTotalHiding || this.EnableSubTotalHiding && this.PivotColumns[index3].ShowSubTotal))
            {
              if (string.IsNullOrEmpty(Convert.ToString((object) calculationValues.Keys[index3])))
              {
                str2 = this.delimiter + this.PivotColumns[index3].TotalHeader;
                this.columnHeaders[index3, columnIndex] = (IComparable) str2;
              }
              else if (!this.ShowCalculationsAsColumns && calculationValues.Keys[index3].ToString() + this.delimiter + this.PivotColumns[index3].TotalHeader != str1 || this.ShowCalculationsAsColumns && this.PivotCalculations.Count > 0 && (columnIndex - this.PivotRows.Count) % this.PivotCalculations.Count == 0 || this.PivotRows.Count == 0 && this.PivotCalculations.Count == 0 || (this.PivotRows.Count > 0 || this.PivotColumns.Count > 0) && this.PivotCalculations.Count == 0)
              {
                str2 = calculationValues.Keys[index3].ToString() + this.delimiter + this.PivotColumns[index3].TotalHeader;
                this.columnHeaders[index3, columnIndex] = (IComparable) str2;
                str1 = str2;
              }
              for (int index4 = 0; index4 < calculationValues.Keys.Count - 1; ++index4)
              {
                if (index3 != index4)
                {
                  if (this.GridLayout == GridLayout.ExcelLikeLayout)
                  {
                    string[,] headerUniqueValues;
                    IntPtr index5;
                    IntPtr index6;
                    (headerUniqueValues = this.columnHeaderUniqueValues)[(int) (index5 = (IntPtr) index3), (int) (index6 = (IntPtr) columnIndex)] = $"{headerUniqueValues[(int) index5, (int) index6]}{(object) calculationValues.Keys[index4]}.";
                  }
                  else
                    this.columnHeaderUniqueValues[index3, columnIndex] = calculationValues.Keys[index4].ToString() + ".";
                }
                else
                {
                  string[,] headerUniqueValues;
                  IntPtr index7;
                  IntPtr index8;
                  (headerUniqueValues = this.columnHeaderUniqueValues)[(int) (index7 = (IntPtr) index3), (int) (index8 = (IntPtr) columnIndex)] = headerUniqueValues[(int) index7, (int) index8] + str2;
                  break;
                }
              }
              for (int index9 = 0; index9 < index3; ++index9)
                this.columnHeaders[index9, columnIndex] = calculationValues.Keys[index9];
              if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && str2.Contains(calculationValues.Keys[0].ToString()))
              {
                this.columnHeaders[calculationValues.Keys.Count - 1, columnIndex] = binaryList[0];
                if (index2 == 0)
                  index1 = columnIndex;
                this.columnHeaderUniqueValues[calculationValues.Keys.Count - 1, columnIndex] = this.columnHeaders[0, index1].ToString() + this.delimiter + binaryList[0].ToString();
              }
              if (calculationHeaderVisible)
              {
                this.columnHeaders[count, columnIndex] = this.UseDescriptionInCalculationHeader ? (IComparable) this.PivotCalculations[index2].Description : (IComparable) this.PivotCalculations[index2].FieldHeader;
                this.columnHeaderUniqueValues[count, columnIndex] = this.columnHeaderUniqueValues[index3, columnIndex] + this.delimiter + (this.UseDescriptionInCalculationHeader ? this.PivotCalculations[index2].Description : this.PivotCalculations[index2].FieldHeader);
              }
              this.SetColumnSummary(columnIndex);
              ++columnIndex;
            }
            else if (index3 == -1 && this.ShowSubTotalsForChildren)
            {
              for (int index10 = 1; index10 <= binaryList.Count && index2 == 0; ++index10)
              {
                for (int index11 = 0; index11 < num1; ++index11)
                {
                  this.columnHeaders[calculationValues.Keys.Count - 1, columnIndex] = index10 < binaryList.Count ? binaryList[index10] : (IComparable) this.PivotColumns[this.PivotColumns.Count - 1].TotalHeader;
                  this.columnHeaderUniqueValues[calculationValues.Keys.Count - 1, columnIndex] = index10 < binaryList.Count ? this.columnHeaders[0, index1].ToString() + this.delimiter + binaryList[index10].ToString() : this.columnHeaders[0, index1].ToString() + this.delimiter + this.PivotColumns[this.PivotColumns.Count - 1].TotalHeader;
                  if (calculationHeaderVisible)
                  {
                    this.columnHeaders[count, columnIndex] = this.UseDescriptionInCalculationHeader ? (IComparable) this.PivotCalculations[index2].Description : (IComparable) this.PivotCalculations[index11].FieldHeader;
                    this.columnHeaderUniqueValues[count, columnIndex] = this.columnHeaderUniqueValues[0, index1] + this.delimiter + (this.UseDescriptionInCalculationHeader ? this.PivotCalculations[index11].Description : this.PivotCalculations[index11].FieldHeader);
                  }
                  this.SetColumnSummary(columnIndex);
                  ++columnIndex;
                }
              }
              binaryList.Clear();
            }
          }
        }
      }
      else
      {
        num2 = -1;
        calculationValues = columnKeysCalcValue;
        for (int index12 = 0; index12 < num1; ++index12)
        {
          int index13 = 0;
          if (columnKeysCalcValue.Keys != null)
          {
            for (int index14 = 0; index14 < columnKeysCalcValue.Keys.Count; ++index14)
            {
              this.columnHeaders[index13, columnIndex] = columnKeysCalcValue.Keys[index14];
              for (int index15 = 0; index15 <= index14; ++index15)
                this.columnHeaderUniqueValues[index13, columnIndex] = index15 != 0 || columnKeysCalcValue.Keys[index15] == null ? $"{this.columnHeaderUniqueValues[index13, columnIndex]}.{Convert.ToString((object) columnKeysCalcValue.Keys[index15])}" : columnKeysCalcValue.Keys[index15].ToString();
              ++index13;
            }
          }
          if (calculationHeaderVisible && columnIndex < this.columnHeaders.GetLength(1))
          {
            this.columnHeaders[index13, columnIndex] = this.UseDescriptionInCalculationHeader ? (IComparable) this.PivotCalculations[index12].Description : (this.PivotCalculations[index12].FieldCaption != null ? (IComparable) this.PivotCalculations[index12].FieldCaption : (IComparable) this.PivotCalculations[index12].FieldHeader);
            this.columnHeaderUniqueValues[index13, columnIndex] = this.UseDescriptionInCalculationHeader ? this.PivotCalculations[index12].Description : (this.PivotCalculations[index12].FieldCaption != null ? this.PivotCalculations[index12].FieldCaption : this.PivotCalculations[index12].FieldHeader);
          }
          if (this.PivotRows.Count == 0 && this.PivotCalculations.Count == 0)
            this.SetColumnSummary(columnIndex - 1);
          else
            this.SetColumnSummary(columnIndex);
          ++columnIndex;
        }
      }
      if (this.PivotColumns.Count > 0 && this.ShowSubTotalsForChildren)
        binaryList.AddIfUnique(calculationValues.Keys[this.PivotColumns.Count - 1]);
    }
    for (int index16 = 0; index16 < num1 && columnIndex < this.columnHeaders.GetLength(1); ++index16)
    {
      if (this.PivotColumns.Count > 0)
      {
        if (index16 == 0)
        {
          if (this.pivotColumns[0].TotalHeader == null)
          {
            this.columnHeaders[0, columnIndex] = (IComparable) (this.GrandString + this.delimiter);
            this.columnHeaderUniqueValues[0, columnIndex] = this.GrandString + this.delimiter;
          }
          else
          {
            this.columnHeaders[0, columnIndex] = (IComparable) (this.GrandString + this.delimiter + this.PivotColumns[0].TotalHeader);
            this.columnHeaderUniqueValues[0, columnIndex] = this.GrandString + this.delimiter + this.PivotColumns[0].TotalHeader;
          }
        }
        else
          this.columnHeaderUniqueValues[0, columnIndex] = this.pivotColumns[0].TotalHeader != null ? this.GrandString + this.delimiter + this.PivotColumns[0].TotalHeader : this.GrandString + this.delimiter;
      }
      if (calculationHeaderVisible && columnIndex < this.columnHeaders.GetLength(1))
      {
        this.columnHeaders[count, columnIndex] = this.UseDescriptionInCalculationHeader ? (IComparable) this.PivotCalculations[index16].Description : (IComparable) this.PivotCalculations[index16].FieldHeader;
        this.columnHeaderUniqueValues[count, columnIndex] = this.GrandString + this.delimiter + (this.UseDescriptionInCalculationHeader ? this.PivotCalculations[index16].Description : this.PivotCalculations[index16].FieldHeader);
        this.grandColumnIndex = this.columnHeaders.GetLength(1) - columnIndex - 1;
      }
      this.SetColumnSummary(columnIndex);
      ++columnIndex;
    }
  }

  private void TransposePivotTable()
  {
    int colCount = !this.UseIndexedEngine ? this.RowCount : this.indexEngine.RowCount;
    int rowCount = !this.UseIndexedEngine ? this.ColumnCount : this.indexEngine.ColumnCount;
    PivotCellInfos pivotCellInfos = new PivotCellInfos(rowCount, colCount);
    for (int index1 = 0; index1 < rowCount; ++index1)
    {
      for (int index2 = 0; index2 < colCount; ++index2)
      {
        pivotCellInfos[index1, index2] = !this.LoadInBackground ? this[index2, index1] : this.PivotValues[index2, index1];
        if (!this.LoadInBackground && this[index2, index1] != null && this[index2, index1].CellRange != null)
          pivotCellInfos[index1, index2].CellRange = new CoveredCellRange(this[index2, index1].CellRange.Left, this[index2, index1].CellRange.Top, this[index2, index1].CellRange.Right, this[index2, index1].CellRange.Bottom);
        else if (this.PivotValues != null && index2 < this.PivotValues.GetLength(0) && index1 < this.PivotValues.GetLength(1) && this.PivotValues[index2, index1] != null && this.PivotValues[index2, index1].CellRange != null)
          pivotCellInfos[index1, index2].CellRange = new CoveredCellRange(this.PivotValues[index2, index1].CellRange.Left, this.PivotValues[index2, index1].CellRange.Top, this.PivotValues[index2, index1].CellRange.Right, this.PivotValues[index2, index1].CellRange.Bottom);
        PivotCellInfo pivotCellInfo = pivotCellInfos[index1, index2];
        if (pivotCellInfo != null)
        {
          if (this.GridLayout != GridLayout.TopSummary)
          {
            pivotCellInfo.RowIndex = index1;
            pivotCellInfo.ColumnIndex = index2;
          }
          if ((pivotCellInfo.CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0)
          {
            pivotCellInfo.CellType &= ~PivotCellType.RowHeaderCell;
            pivotCellInfo.CellType |= PivotCellType.ColumnHeaderCell;
          }
          else if ((pivotCellInfo.CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            pivotCellInfo.CellType &= ~PivotCellType.ColumnHeaderCell;
            pivotCellInfo.CellType |= PivotCellType.RowHeaderCell;
          }
        }
      }
    }
    this.rowCount = rowCount;
    this.columnCount = colCount;
    if (!this.UseIndexedEngine)
    {
      this.pivotValues = pivotCellInfos;
    }
    else
    {
      this.indexEngine.rowCount = rowCount;
      this.indexEngine.columnCount = colCount;
      this.indexEngine.highRowLevel = rowCount;
      this.indexEngine.pivotInfoCache = pivotCellInfos;
    }
    List<CoveredCellRange> coveredCellRangeList = new List<CoveredCellRange>();
    foreach (CoveredCellRange coveredRange in this.CoveredRanges)
      coveredCellRangeList.Add(new CoveredCellRange(coveredRange.Left, coveredRange.Top, coveredRange.Right, coveredRange.Bottom));
    if (!this.UseIndexedEngine)
      this.coveredRanges = coveredCellRangeList;
    else
      this.indexEngine.coveredRanges = coveredCellRangeList;
  }

  private void SwapRowsColumns(bool reset)
  {
    PivotItem[] pivotItemArray1 = new PivotItem[this.PivotColumns.Count];
    PivotItem[] pivotItemArray2 = new PivotItem[this.PivotRows.Count];
    this.PivotColumns.CopyTo(pivotItemArray1);
    this.PivotRows.CopyTo(pivotItemArray2);
    this.PivotColumns.Clear();
    this.PivotRows.Clear();
    if (reset)
      this.ResetForSwapRowsColumns(reset);
    this.PivotColumns.AddRange((IEnumerable<PivotItem>) pivotItemArray2);
    this.PivotRows.AddRange((IEnumerable<PivotItem>) pivotItemArray1);
  }

  private int GetRowCountInValuesArea()
  {
    if (this.rowKeysCalcValues == null)
      return 0;
    return this.PivotRows.Count != 0 || !this.ShowGrandTotals ? this.rowKeysCalcValues.Count + (this.PivotRows.Count > 0 ? 1 : 0) : this.rowKeysCalcValues.Count;
  }

  private List<IComparable> GetKeyForRawItem(int i, int j)
  {
    List<IComparable> keyForRawItem = new List<IComparable>();
    if (i < this.RowCount && i > -1)
    {
      for (int columnIndex1 = 0; columnIndex1 < this.PivotRows.Count; ++columnIndex1)
      {
        if (this[i, columnIndex1] != null && this[i, columnIndex1].Key != null)
          keyForRawItem.Add((IComparable) this[i, columnIndex1].Key.ToString());
        else
          keyForRawItem.Add((IComparable) null);
      }
    }
    if (j < this.ColumnCount && j > -1)
    {
      for (int rowIndex = 0; rowIndex < this.PivotColumns.Count; ++rowIndex)
      {
        if (this[rowIndex, j] != null && this[rowIndex, j].Key != null)
          keyForRawItem.Add((IComparable) this[rowIndex, j].Key.ToString());
        else
          keyForRawItem.Add((IComparable) null);
      }
    }
    return keyForRawItem;
  }

  private int GetColumnCountInValuesArea()
  {
    return this.columnKeysCalcValues != null ? (this.PivotColumns.Count != 0 || !this.ShowGrandTotals ? this.columnKeysCalcValues.Count + 1 : this.columnKeysCalcValues.Count) * Math.Max(1, this.PivotCalculations.Count) : 0;
  }

  private void PopulatePivotGridControl()
  {
    this.rowCount = this.rowHeaders.GetLength(0) + (this.PivotRows.Count <= 0 && this.PivotCalculations.Count <= 0 || !this.ShowGrandTotals ? (this.PivotRows.Count > 0 || this.PivotColumns.Count > 0 && this.PivotCalculations.Count > 0 ? 1 : 0) : 1) - this.grandRowIndex;
    this.columnCount = this.columnHeaders.GetLength(1) + (this.PivotColumns.Count <= 0 && this.PivotCalculations.Count <= 0 || !this.ShowGrandTotals ? (this.PivotColumns.Count > 0 || this.PivotRows.Count > 0 && this.PivotCalculations.Count > 0 ? 1 : 0) : 1) - this.grandColumnIndex;
    if (this.rowCount != 0 && this.columnCount != 0)
      this.pivotValues = new PivotCellInfos(this.rowCount, this.columnCount);
    int length1 = this.rowHeaders.GetLength(1);
    for (int rowIndex = 0; rowIndex < this.rowHeaders.GetLength(0) - this.grandRowIndex; ++rowIndex)
    {
      for (int colIndex = 0; colIndex < length1; ++colIndex)
      {
        this.pivotValues[rowIndex, colIndex] = new PivotCellInfo()
        {
          RowIndex = rowIndex,
          ColumnIndex = colIndex
        };
        if (rowIndex == 0 || this.rowHeaders[rowIndex, colIndex] != null && this.rowHeaders[rowIndex, colIndex].CompareTo((object) this.rowHeaders[rowIndex - 1, colIndex]) != 0)
        {
          this.pivotValues[rowIndex, colIndex].Value = (object) this.rowHeaders[rowIndex, colIndex];
          this.pivotValues[rowIndex, colIndex].FormattedText = this.rowHeaders[rowIndex, colIndex] == null ? (string) null : this.rowHeaders[rowIndex, colIndex].ToString();
        }
        this.pivotValues[rowIndex, colIndex].UniqueText = this.rowHeaderUniqueValues[rowIndex, colIndex];
        this.PivotValues[rowIndex, colIndex].Key = this.rowHeaders[rowIndex, colIndex] == null ? (this.pivotValues[rowIndex, colIndex].UniqueText == null ? (string) null : this.pivotValues[rowIndex, colIndex].UniqueText) : this.rowHeaders[rowIndex, colIndex].ToString();
      }
    }
    for (int rowIndex = 0; rowIndex < this.columnHeaders.GetLength(0); ++rowIndex)
    {
      for (int colIndex = 0; colIndex < this.columnHeaders.GetLength(1) - this.grandColumnIndex; ++colIndex)
      {
        if (this.pivotValues[rowIndex, colIndex] == null)
          this.pivotValues[rowIndex, colIndex] = new PivotCellInfo()
          {
            RowIndex = rowIndex,
            ColumnIndex = colIndex
          };
        if (colIndex == 0 || this.columnHeaderUniqueValues[rowIndex, colIndex] != null)
        {
          this.pivotValues[rowIndex, colIndex].Value = (object) this.columnHeaders[rowIndex, colIndex];
          this.pivotValues[rowIndex, colIndex].FormattedText = this.columnHeaders[rowIndex, colIndex] == null ? (string) null : this.columnHeaders[rowIndex, colIndex].ToString();
        }
        this.pivotValues[rowIndex, colIndex].UniqueText = this.columnHeaderUniqueValues[rowIndex, colIndex];
        this.PivotValues[rowIndex, colIndex].Key = this.columnHeaders[rowIndex, colIndex] == null ? (this.pivotValues[rowIndex, colIndex].UniqueText == null ? (string) null : this.pivotValues[rowIndex, colIndex].UniqueText) : this.columnHeaders[rowIndex, colIndex].ToString();
      }
    }
    this.PopulateValueCells();
    for (int index = 0; index < this.pivotCalculations.Count; ++index)
    {
      if (this.pivotCalculations[index].CalculationType == CalculationType.Formula && index < this.pivotCalculations.Count - 1)
        this.PopulateValueCells();
    }
    this.CoverRowHeaders();
    this.CoverColumnHeaders();
    this.MarkGrandTotalCellType();
    if (this.PivotColumns.Count == 0 && this.PivotRows.Count > 0 && this.PivotCalculations.Count <= 1 && !this.IsCalculationHeaderVisible)
    {
      int rowCount = this.pivotValues.GetLength(0) + 1;
      int length2 = this.pivotValues.GetLength(1);
      List<CoveredCellRange> coveredCellRangeList = new List<CoveredCellRange>();
      PivotCellInfos pivotCellInfos = new PivotCellInfos(rowCount, length2);
      for (int rowIndex = 1; rowIndex < rowCount; ++rowIndex)
      {
        for (int colIndex = 0; colIndex < length2; ++colIndex)
        {
          pivotCellInfos[rowIndex, colIndex] = this.pivotValues[rowIndex - 1, colIndex];
          if (pivotCellInfos[rowIndex, colIndex] != null && pivotCellInfos[rowIndex, colIndex].CellRange != null)
          {
            pivotCellInfos[rowIndex, colIndex].CellRange = new CoveredCellRange(pivotCellInfos[rowIndex, colIndex].CellRange.Top + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Left, pivotCellInfos[rowIndex, colIndex].CellRange.Bottom + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Right);
            if (this.pivotValues[rowIndex - 1, colIndex].UniqueText != "x" || pivotCellInfos[rowIndex, colIndex].CellRange.Right - pivotCellInfos[rowIndex, colIndex].CellRange.Left > 1)
              coveredCellRangeList.Add(pivotCellInfos[rowIndex, colIndex].CellRange);
          }
        }
      }
      int count = this.pivotRows.Count;
      for (int colIndex = count; colIndex < count + this.PivotCalculations.Count; ++colIndex)
        pivotCellInfos[0, colIndex] = new PivotCellInfo()
        {
          CellType = PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell,
          RowIndex = 0,
          ColumnIndex = colIndex,
          Value = this.UseDescriptionInCalculationHeader ? (object) this.PivotCalculations[colIndex - count].Description : (object) this.PivotCalculations[colIndex - count].FieldName,
          FormattedText = this.UseDescriptionInCalculationHeader ? this.PivotCalculations[colIndex - count].Description : this.PivotCalculations[colIndex - count].FieldHeader
        };
      pivotCellInfos[0, 0] = new PivotCellInfo()
      {
        CellType = PivotCellType.TopLeftCell,
        RowIndex = 0,
        ColumnIndex = 0,
        CellRange = new CoveredCellRange(0, 0, 0, count - 1)
      };
      coveredCellRangeList.Add(pivotCellInfos[0, 0].CellRange);
      for (int colIndex = 1; colIndex < count; ++colIndex)
        pivotCellInfos[0, colIndex] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = colIndex
        };
      this.pivotValues = pivotCellInfos;
      ++this.rowCount;
      this.coveredRanges = coveredCellRangeList;
    }
    else if (this.PivotColumns.Count == 0 && this.PivotRows.Count > 0 && this.PivotCalculations.Count > 0)
    {
      int count = this.pivotRows.Count;
      for (int colIndex = count; colIndex < count + this.PivotCalculations.Count; ++colIndex)
      {
        if (colIndex < this.pivotValues.GetLength(1) && this.pivotValues[0, colIndex] != null)
          this.pivotValues[0, colIndex].CellType = PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell;
      }
    }
    else if (this.PivotColumns.Count > 0 && this.PivotRows.Count == 0 && this.PivotCalculations.Count > 0)
    {
      int length3 = this.pivotValues.GetLength(0);
      int colCount = this.pivotValues.GetLength(1) + 1;
      List<CoveredCellRange> ranges = new List<CoveredCellRange>();
      PivotCellInfos pivotCellInfos = new PivotCellInfos(length3, colCount);
      for (int rowIndex = 0; rowIndex < length3; ++rowIndex)
      {
        for (int colIndex = 1; colIndex < colCount; ++colIndex)
        {
          pivotCellInfos[rowIndex, colIndex] = this.pivotValues[rowIndex, colIndex - 1];
          if (pivotCellInfos[rowIndex, colIndex] != null && pivotCellInfos[rowIndex, colIndex].CellRange != null)
          {
            pivotCellInfos[rowIndex, colIndex].CellRange = new CoveredCellRange(pivotCellInfos[rowIndex, colIndex].CellRange.Top, pivotCellInfos[rowIndex, colIndex].CellRange.Left + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Bottom, pivotCellInfos[rowIndex, colIndex].CellRange.Right + 1);
            if (this.OkToAddRange(pivotCellInfos[rowIndex, colIndex].CellRange, ranges))
              ranges.Add(pivotCellInfos[rowIndex, colIndex].CellRange);
          }
        }
      }
      pivotCellInfos[0, 0] = new PivotCellInfo()
      {
        CellType = PivotCellType.TopLeftCell,
        RowIndex = 0,
        ColumnIndex = 0,
        CellRange = new CoveredCellRange(0, 0, this.PivotColumns.Count - (this.IsCalculationHeaderVisible ? 0 : 1), 0)
      };
      ranges.Add(pivotCellInfos[0, 0].CellRange);
      for (int rowIndex = length3 - 1; rowIndex > 0; --rowIndex)
        pivotCellInfos[rowIndex, 0] = new PivotCellInfo()
        {
          RowIndex = rowIndex,
          ColumnIndex = 0,
          CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell,
          FormattedText = $"{this.GrandString}{this.delimiter} {this.PivotColumns[0].TotalHeader}"
        };
      this.pivotValues = pivotCellInfos;
      ++this.columnCount;
      this.coveredRanges = ranges;
    }
    else if (this.PivotColumns.Count == 0 && this.PivotRows.Count == 0 && this.PivotCalculations.Count > 0)
    {
      int length4 = this.pivotValues.GetLength(0);
      int colCount = this.pivotValues.GetLength(1) + 1;
      List<CoveredCellRange> ranges = new List<CoveredCellRange>();
      PivotCellInfos pivotCellInfos = new PivotCellInfos(length4, colCount);
      for (int rowIndex = 0; rowIndex < length4; ++rowIndex)
      {
        for (int colIndex = 1; colIndex < colCount; ++colIndex)
        {
          pivotCellInfos[rowIndex, colIndex] = this.pivotValues[rowIndex, colIndex - 1];
          if (pivotCellInfos[rowIndex, colIndex] != null && pivotCellInfos[rowIndex, colIndex].CellRange != null)
          {
            pivotCellInfos[rowIndex, colIndex].CellRange = new CoveredCellRange(pivotCellInfos[rowIndex, colIndex].CellRange.Top, pivotCellInfos[rowIndex, colIndex].CellRange.Left + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Bottom, pivotCellInfos[rowIndex, colIndex].CellRange.Right + 1);
            if (this.OkToAddRange(pivotCellInfos[rowIndex, colIndex].CellRange, ranges))
              ranges.Add(pivotCellInfos[rowIndex, colIndex].CellRange);
          }
        }
      }
      if (this.PivotCalculations.Count == 1)
        pivotCellInfos[0, 0] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = 0,
          CellType = PivotCellType.TopLeftCell,
          CellRange = new CoveredCellRange(0, 0, 0, 0)
        };
      else
        pivotCellInfos[0, 0] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = 0,
          CellType = PivotCellType.TopLeftCell,
          CellRange = new CoveredCellRange(0, 0, this.PivotColumns.Count - (this.IsCalculationHeaderVisible ? 0 : 1), 0)
        };
      ranges.Add(pivotCellInfos[0, 0].CellRange);
      for (int rowIndex = length4 - 1; rowIndex > 0; --rowIndex)
        pivotCellInfos[rowIndex, 0] = new PivotCellInfo()
        {
          RowIndex = rowIndex,
          ColumnIndex = 0,
          CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell,
          FormattedText = $"{this.GrandString}{this.delimiter} Total"
        };
      this.pivotValues = pivotCellInfos;
      this.columnCount += this.ShowGrandTotals ? 1 : 0;
      this.coveredRanges = ranges;
    }
    if (this.PivotColumns.Count <= 0 && this.PivotRows.Count <= 0)
      return;
    if (this.pivotValues != null && this.pivotValues[0, 0].CellRange != null)
    {
      for (int rowIndex = 0; rowIndex <= this.pivotValues[0, 0].CellRange.Bottom; ++rowIndex)
      {
        for (int colIndex = 0; colIndex <= this.pivotValues[0, 0].CellRange.Right; ++colIndex)
          this.pivotValues[rowIndex, colIndex].CellType = PivotCellType.ValueCell | PivotCellType.TopLeftCell;
      }
    }
    if (this.pivotValues == null)
      return;
    this.pivotValues[0, 0].CellType = PivotCellType.TopLeftCell;
  }

  private void FindRowColumnItems(List<PivotItem> rowList, List<PivotItem> columnList)
  {
    List<object> source = new List<object>();
    this.PivotBaseRowItems = new List<object>();
    this.PivotBaseColumnItems = new List<object>();
    int num1;
    foreach (PivotItem pivotRow in this.PivotRows)
    {
      source.Clear();
      int num2 = this.PivotRows.IndexOf(pivotRow) + 1;
      for (int rowIndex = this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0); rowIndex < this.RowCount - 1; ++rowIndex)
      {
        if (this.PivotValues[rowIndex, num2 - 1] != null && this.PivotValues[rowIndex, num2 - 1].FormattedText != null && !this.PivotValues[rowIndex, num2 - 1].FormattedText.Contains("Total") && !source.Contains((object) this.PivotValues[rowIndex, num2 - 1].FormattedText))
          source.Add((object) this.PivotValues[rowIndex, num2 - 1].FormattedText);
      }
      num1 = num2 + 1;
      this.PivotBaseRowItems.Add((object) source.ToList<object>());
    }
    foreach (PivotItem pivotColumn in this.PivotColumns)
    {
      source.Clear();
      int num3 = this.PivotColumns.IndexOf(pivotColumn) + this.PivotRows.Count + 1;
      for (int count = this.PivotRows.Count; count < this.ColumnCount - 1; ++count)
      {
        if (this.PivotValues[num3 - this.PivotRows.Count - 1, count] != null && this.PivotValues[num3 - this.PivotRows.Count - 1, count].FormattedText != null && !this.PivotValues[num3 - this.PivotRows.Count - 1, count].FormattedText.Contains("Total") && !source.Contains((object) this.PivotValues[num3 - this.PivotRows.Count - 1, count].FormattedText))
          source.Add((object) this.PivotValues[num3 - this.PivotRows.Count - 1, count].FormattedText);
      }
      num1 = num3 + 1;
      this.PivotBaseColumnItems.Add((object) source.ToList<object>());
    }
  }

  private object CalculateValuesForDateTime(
    PivotComputationInfo compInfo,
    Dictionary<string, double> component)
  {
    if (this.DataSource == null)
      return (object) null;
    DateTime valuesForDateTime = DateTime.Now;
    string str1 = compInfo.Expression.Expression.Split('(', ')')[0];
    string str2 = compInfo.Expression.Expression.Remove(0, str1.Length + 1);
    string[] source = str2.Remove(str2.Length - 1, 1).Split(',');
    string[] strArray = source[0].Split('-');
    string str3 = source[0].Remove(0, source[0].IndexOf("(") + 1);
    ((IEnumerable<string>) source).ToList<string>().ForEach((Action<string>) (p => p = new string(p.Where<char>((System.Func<char, bool>) (c => !char.IsWhiteSpace(c))).ToArray<char>())));
    switch (str1.ToUpper())
    {
      case "DATE":
        string key = str3.Remove(str3.IndexOf(")"), str3.Length - str3.IndexOf(")"));
        string[] yearsCol = source[((IEnumerable<string>) source).ToList<string>().FindIndex((Predicate<string>) (x => x.ToUpper().Contains("YEAR")))].Split('+');
        string[] monthCol = source[((IEnumerable<string>) source).ToList<string>().FindIndex((Predicate<string>) (x => x.ToUpper().Contains("MONTH")))].Split('+');
        string[] daysCol = source[((IEnumerable<string>) source).ToList<string>().FindIndex((Predicate<string>) (x => x.ToUpper().Contains("DAY")))].Split('+');
        valuesForDateTime = DateTime.FromOADate(component[key]).AddYears(component.Any<KeyValuePair<string, double>>((System.Func<KeyValuePair<string, double>, bool>) (x => x.Key == yearsCol[1].Remove(yearsCol[1].IndexOf(" "), 1))) ? int.Parse(component[yearsCol[1]].ToString().Remove(yearsCol[1].IndexOf(" "), 1)) : int.Parse(yearsCol[1])).AddMonths(component.Any<KeyValuePair<string, double>>((System.Func<KeyValuePair<string, double>, bool>) (x => x.Key == monthCol[1].Remove(monthCol[1].IndexOf(" "), 1))) ? int.Parse(component[monthCol[1]].ToString().Remove(monthCol[1].IndexOf(" "), 1)) : int.Parse(monthCol[1])).AddDays(component.Any<KeyValuePair<string, double>>((System.Func<KeyValuePair<string, double>, bool>) (x => x.Key == daysCol[1].Remove(daysCol[1].IndexOf(" "), 1))) ? component[daysCol[1].Remove(daysCol[1].IndexOf(" "), 1)] : double.Parse(daysCol[1]));
        break;
      case "DATEDIF":
        return (object) (source[2].ToUpper() == "D" ? Math.Abs((DateTime.FromOADate(component[source[1].Remove(source[1].IndexOf(" "), 1)]) - DateTime.FromOADate(component[source[0]])).TotalDays) : (source[2].ToUpper() == "YD" ? (double) Math.Abs((DateTime.FromOADate(component[source[1].Remove(source[1].IndexOf(" "), 1)]) - DateTime.FromOADate(component[source[0]])).Days) : (source[2].ToString().ToUpper() == "YM" ? (double) Math.Abs(DateTime.FromOADate(component[source[1].Remove(source[1].IndexOf(" "), 1)]).Month - DateTime.FromOADate(component[source[0]]).Month) : (DateTime.FromOADate(component[source[1].Remove(source[1].IndexOf(" "), 1)]) - DateTime.FromOADate(component[source[0]])).TotalHours)));
      case "TEXT":
        return (object) (source[1].ToUpper() == "H" ? (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Hours) : (source[1].ToUpper() == "MM" ? (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Minutes) : (source[1].ToUpper() == "SS" ? (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Seconds) : (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Milliseconds))));
      case "INT":
        return (object) (source[1] == "24" ? Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).TotalHours) : (source[1] == "1440" ? Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).TotalMinutes) : (source[1] == "86400" ? Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).TotalSeconds) : Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).TotalMilliseconds))));
      case "HOUR":
        return (object) (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Hours);
      case "MINUTE":
        return (object) (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Minutes);
      case "SECOND":
        return (object) (double) Math.Abs((DateTime.FromOADate(component[strArray[0].Remove(strArray[0].IndexOf(" "), 1)]) - DateTime.FromOADate(component[strArray[1].Remove(strArray[1].IndexOf(" "), 1)])).Seconds);
    }
    return (object) valuesForDateTime;
  }

  private string DoCalculatedValue(int row, int col, object v, string field)
  {
    CalculationType calculationType = this.PivotCalculations[(col - this.PivotRows.Count) % this.PivotCalculations.Count].CalculationType;
    bool isSummaryRow = this.IsRowSummary(row);
    bool isSummaryColumn = this.IsSummaryColumn(col);
    string format = $"{{0:{this.PivotCalculations[(col - this.PivotRows.Count) % this.PivotCalculations.Count].Format}}}";
    int num = (col - this.PivotRows.Count) % this.PivotCalculations.Count;
    int index1 = row - (this.PivotColumns.Count + 1);
    int index2 = col - this.pivotRows.Count;
    if (this.pivotValues[row, col] != null && format.Equals("{0:#.##}") && v != null && (v.Equals((object) 0.0) || v.Equals((object) 0)))
      this.pivotValues[row, col].FormattedText = !this.PivotCalculations[col % this.PivotCalculations.Count].CalculationType.ToString().Contains("Percentage") ? "0.0" : "0.00%";
    else if (this.pivotValues[row, col] != null && format.Equals("{0:###}") && v != null && (v.Equals((object) 0) || v.Equals((object) 0.0)))
      this.pivotValues[row, col].FormattedText = !this.PivotCalculations[col % this.PivotCalculations.Count].CalculationType.ToString().Contains("Percentage") ? "0" : "0%";
    else if (this.pivotValues[row, col] != null)
    {
      PivotComputationInfo compInfo = this.PivotCalculations[index2 % this.PivotCalculations.Count];
      switch (calculationType)
      {
        case CalculationType.NoCalculation:
          this.pivotValues[row, col].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, v);
          break;
        case CalculationType.PercentageOfGrandTotal:
          object result1 = this.valuesArea[this.valuesArea.GetLength(0) - 1, this.valuesArea.GetLength(1) - (this.PivotRows.Count - 1)].GetResult();
          this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) / Convert.ToDouble(result1) * 100.0);
          this.pivotValues[row, col].FormattedText = ((double) this.pivotValues[row, col].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
          break;
        case CalculationType.PercentageOfColumnTotal:
          object result2 = this.valuesArea[this.valuesArea.GetLength(0) - 1, index2].GetResult();
          this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) / Convert.ToDouble(result2) * 100.0);
          this.pivotValues[row, col].FormattedText = ((double) this.pivotValues[row, col].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
          break;
        case CalculationType.PercentageOfRowTotal:
          object result3 = this.valuesArea[index1, this.valuesArea.GetLength(1) - 1 - (this.PivotCalculations.Count - (num + 1))].GetResult();
          this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) / Convert.ToDouble(result3) * 100.0);
          this.pivotValues[row, col].FormattedText = ((double) this.pivotValues[row, col].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
          break;
        case CalculationType.PercentageOfParentColumnTotal:
          object result4;
          if (isSummaryColumn)
          {
            this.parentColLocation = this.GetNextParentColumnIndex(this.parentColLocation[0], col);
            result4 = this.valuesArea[index1, this.parentColLocation[1]].GetResult();
          }
          else
          {
            this.parentColLocation = this.GetNextParentColumnIndex(this.columnHeaders.GetLength(0) - (1 + (this.IsCalculationHeaderVisible ? 1 : 0)), col);
            result4 = this.valuesArea[index1, this.parentColLocation[1]].GetResult();
          }
          this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) / Convert.ToDouble(result4) * 100.0);
          this.pivotValues[row, col].FormattedText = ((double) this.pivotValues[row, col].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
          break;
        case CalculationType.PercentageOfParentRowTotal:
          object result5;
          if (isSummaryRow)
          {
            int parentColumnIndex = 0;
            for (int index3 = 0; index3 < this.PivotRows.Count; ++index3)
            {
              if (this.rowHeaders[index1, index3] == null)
              {
                parentColumnIndex = index3 - 1;
                break;
              }
            }
            if (this.PivotRows.Count > 2 && parentColumnIndex > 0)
            {
              this.parentRowLocation = this.GetNextParentRowIndex(row, parentColumnIndex);
              result5 = this.valuesArea[this.parentRowLocation[0], index2].GetResult();
            }
            else
            {
              this.parentRowLocation = this.GetNextParentRowIndex(row, this.rowHeaders.GetLength(1) - this.PivotRows.Count);
              result5 = this.valuesArea[this.parentRowLocation[0], index2].GetResult();
            }
          }
          else
          {
            this.parentRowLocation = this.GetNextParentRowIndex(row, this.rowHeaders.GetLength(1) - 1);
            result5 = this.valuesArea[this.parentRowLocation[0], index2].GetResult();
          }
          this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) / Convert.ToDouble(result5) * 100.0);
          this.pivotValues[row, col].FormattedText = ((double) this.pivotValues[row, col].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
          break;
        case CalculationType.PercentageOfParentTotal:
          if (compInfo.BaseField == null)
            throw new ArgumentNullException("Base Field value should be required.");
          int index4 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
          object obj = (object) null;
          if (index4 != -1)
          {
            int parentRowIndex = this.GetParentRowIndex(index1, index4, isSummaryRow);
            obj = parentRowIndex != -1 ? this.valuesArea[parentRowIndex, index2].GetResult() : (object) null;
          }
          else
          {
            int index5 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
            if (index5 != -1)
            {
              int parentColumnIndex = this.GetParentColumnIndex(index5, index2, isSummaryColumn);
              obj = parentColumnIndex != -1 ? this.valuesArea[index1, parentColumnIndex].GetResult() : (object) null;
            }
          }
          if (obj != null)
          {
            this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) / Convert.ToDouble(obj) * 100.0);
            this.pivotValues[row, col].FormattedText = ((double) this.pivotValues[row, col].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
            break;
          }
          this.pivotValues[row, col].Value = (object) null;
          this.pivotValues[row, col].FormattedText = string.Empty;
          break;
        case CalculationType.Index:
          object result6 = this.valuesArea[this.valuesArea.GetLength(0) - 1, this.valuesArea.GetLength(1) + (col % this.PivotCalculations.Count - this.PivotCalculations.Count)].GetResult();
          object result7 = this.valuesArea[this.valuesArea.GetLength(0) - 1, index2].GetResult();
          object result8 = this.valuesArea[index1, this.valuesArea.GetLength(1) + (col % this.PivotCalculations.Count - this.PivotCalculations.Count)].GetResult();
          this.pivotValues[row, col].Value = (object) (Convert.ToDouble(v) * Convert.ToDouble(result6) / (Convert.ToDouble(result8) * Convert.ToDouble(result7)));
          this.pivotValues[row, col].FormattedText = Math.Round((double) this.pivotValues[row, col].Value, 9).ToString((IFormatProvider) CultureInfo.CurrentUICulture);
          break;
        case CalculationType.Formula:
          this.CalculateFormula(index1, index2, col, compInfo);
          break;
      }
    }
    return this.pivotValues[row, col].FormattedText;
  }

  private void MarkGrandTotalCellType()
  {
    if (this.PivotColumns.Count == 0 && this.PivotRows.Count > 0)
    {
      if (this.IsCalculationHeaderVisible && this.ShowCalculationsAsColumns)
      {
        int length = this.pivotValues.GetLength(1);
        for (int colIndex = length - 2; colIndex >= length - this.PivotCalculations.Count - 1 && colIndex > 0; --colIndex)
        {
          if (this.pivotValues[0, colIndex] != null)
            this.pivotValues[0, colIndex].CellType = PivotCellType.HeaderCell | PivotCellType.ColumnHeaderCell | PivotCellType.GrandTotalCell;
        }
      }
      for (int rowIndex = 0; rowIndex < this.pivotValues.GetLength(0); ++rowIndex)
      {
        if (this.pivotValues[rowIndex, this.PivotRows.Count - 1] != null)
        {
          this.pivotValues[rowIndex, this.PivotRows.Count - 1].CellType = !this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 || this.GridLayout == GridLayout.TopSummary || this.pivotValues[rowIndex, this.PivotRows.Count - 1].UniqueText == null || this.pivotValues[rowIndex, this.PivotRows.Count - 1].UniqueText.Contains(".") ? PivotCellType.HeaderCell | PivotCellType.RowHeaderCell : PivotCellType.TotalCell | PivotCellType.RowHeaderCell;
          if (rowIndex == this.pivotValues.GetLength(0) - 1)
            this.pivotValues[rowIndex, this.PivotRows.Count - 1].CellType |= PivotCellType.GrandTotalCell;
        }
      }
    }
    if (this.RowCount < 2)
      return;
    if (!this.showGrandTotals && this.PivotRows.Count == 1 && this.PivotColumns.Count == 0 && this.PivotCalculations.Count > 0)
    {
      for (int colIndex = 0; colIndex < this.ColumnCount; ++colIndex)
      {
        if (this.pivotValues[this.RowCount - 1, colIndex] != null)
        {
          this.pivotValues[this.RowCount - 1, colIndex].CellType |= PivotCellType.GrandTotalCell;
          this.pivotValues[this.RowCount - 1, colIndex].CellType &= ~PivotCellType.TotalCell;
        }
      }
    }
    else if (this.ShowGrandTotals)
    {
      for (int colIndex = 0; colIndex < this.ColumnCount; ++colIndex)
      {
        if (this.pivotValues[this.RowCount - 2, colIndex] != null)
        {
          this.pivotValues[this.RowCount - 2, colIndex].CellType |= PivotCellType.GrandTotalCell;
          this.pivotValues[this.RowCount - 2, colIndex].CellType &= ~PivotCellType.TotalCell;
        }
      }
    }
    if (this.PivotColumns.Count <= 0)
      return;
    if (this.PivotCalculations.Count > 0)
    {
      for (int rowIndex = 0; rowIndex < this.RowCount; ++rowIndex)
      {
        for (int colIndex = this.ColumnCount - this.PivotCalculations.Count - 1; colIndex < (this.ShowGrandTotals ? this.ColumnCount - 1 : this.ColumnCount - (this.PivotCalculations.Count + 1)); ++colIndex)
        {
          if (this.pivotValues[rowIndex, colIndex] != null)
          {
            this.pivotValues[rowIndex, colIndex].CellType |= PivotCellType.GrandTotalCell;
            if (!this.IsRowSummary(rowIndex))
              this.pivotValues[rowIndex, colIndex].CellType &= ~PivotCellType.TotalCell;
          }
        }
      }
    }
    else
    {
      int num = 1;
      for (int rowIndex = 0; rowIndex < this.RowCount; ++rowIndex)
      {
        for (int colIndex = this.ColumnCount - num - (this.ShowGrandTotals || this.PivotCalculations.Count != 0 ? 1 : 0); colIndex < this.ColumnCount - 1; ++colIndex)
        {
          if (this.pivotValues[rowIndex, colIndex] != null)
          {
            this.pivotValues[rowIndex, colIndex].CellType |= PivotCellType.GrandTotalCell;
            this.pivotValues[rowIndex, colIndex].CellType &= ~PivotCellType.TotalCell;
          }
        }
      }
    }
  }

  private bool OkToAddRange(CoveredCellRange range) => this.OkToAddRange(range, this.CoveredRanges);

  private bool OkToAddRange(CoveredCellRange range, List<CoveredCellRange> ranges)
  {
    if (this.CoveredRanges == null)
      return false;
    bool addRange = true;
    if (range.Left >= 0 && range.Right >= 0 && range.Top >= 0 && range.Bottom >= 0)
    {
      foreach (CoveredCellRange range1 in ranges)
      {
        if (range.Left <= range1.Right && range.Right >= range1.Left && range.Top <= range1.Bottom && range.Bottom >= range1.Top)
        {
          if (range.Left >= range1.Left && range.Left <= range1.Right && range.Top >= range1.Top && range.Top <= range1.Bottom)
          {
            addRange = false;
            break;
          }
          if (range.Right >= range1.Left && range.Right <= range1.Right && range.Bottom >= range1.Top && range.Bottom <= range1.Bottom)
          {
            addRange = false;
            break;
          }
        }
      }
    }
    else
      addRange = false;
    return addRange;
  }

  private void PopulatePivotTable()
  {
    this.emptyPivot = false;
    if (this.DataSourceList != null)
    {
      this.columnKeysCalcValues = new BinaryList();
      this.rowKeysCalcValues = new BinaryList();
      this.tableKeysCalcValues = new BinaryList();
      PropertyDescriptor[] propertyDescriptorArray1 = this.ProcessList(this.PivotColumns);
      PropertyDescriptor[] propertyDescriptorArray2 = this.ProcessList(this.PivotRows);
      List<PropertyDescriptor> list1 = ((IEnumerable<PropertyDescriptor>) this.GetCalcValuesPDs()).ToList<PropertyDescriptor>();
      IComparer[] comparers1 = this.GetComparers(this.PivotColumns);
      IComparer[] comparers2 = this.GetComparers(this.PivotRows);
      DataView source1 = (DataView) null;
      KeysCalculationValues tempValue = (KeysCalculationValues) null;
      IEnumerable source2 = this.DataSourceList;
      this.VisibleRecords = new List<object>();
      bool flag1 = this.Filters.Count > 0;
      bool flag2 = this.Filters.Count > 0;
      string str1 = " ";
      string str2 = "";
      bool flag3 = false;
      if (this.DataSourceList != null && this.DataSourceList is DataView)
      {
        if (this.DataSourceList is DataView)
          source1 = (DataView) source2;
        str2 = source1.RowFilter;
        source2 = (IEnumerable) new DataView(source1.Table);
        flag3 = true;
        if (flag1)
        {
          string str3 = "";
          foreach (FilterExpression filter in this.Filters)
          {
            FilterExpression exp = filter;
            if (!string.IsNullOrEmpty(exp.Expression))
            {
              string str4;
              if (((IEnumerable<string>) new string[6]
              {
                " = ",
                " > ",
                " >= ",
                " < ",
                " <= ",
                " <> "
              }).Any<string>((System.Func<string, bool>) (c => exp.Expression.Contains(c))))
              {
                int length = exp.Expression.IndexOf(']') + 1;
                if (length <= 0)
                  length = exp.Expression.IndexOf(' ');
                str4 = $"convert({exp.Expression.Substring(0, length)}, 'System.String') {exp.Expression.Substring(length + 1, exp.Expression.Length - length - 1)} ";
              }
              else
                str4 = exp.Expression;
              str3 = str3.Length <= 0 ? $"({str4})" : $"{str3} AND ({str4})";
            }
          }
          source1.RowFilter = str2.Length == 0 ? str3 : $"({str2}) AND ({str3})";
          flag1 = false;
          flag2 = source1.Count == 0;
        }
      }
      int num1 = 100;
      int num2 = 0;
      int num3 = 1000;
      IList list2 = this.DataSourceList is DataView ? (IList) source1 : (IList) source2;
      if (list2 != null)
        num3 = list2.Count;
      foreach (object obj1 in this.DataSourceList is DataView ? source1.Cast<object>().ToList<object>() : source2.Cast<object>().ToList<object>())
      {
        object o = obj1;
        bool flag4 = false;
        bool flag5 = false;
        bool? notPopulated = this.NotPopulated;
        if ((notPopulated.GetValueOrDefault() ? 1 : (!notPopulated.HasValue ? 1 : 0)) != 0 && num2 > num1)
        {
          if (num2 >= num3)
            num3 += 1000;
          this.populationStatus = 0.95 * (double) num2 / (double) num3;
          num1 = num2;
        }
        ++num2;
        PropertyInfo[] properties = o.GetType().GetProperties();
        if (flag1)
        {
          flag6 = true;
          foreach (FilterExpression filter in this.Filters)
          {
            if (properties != null && ((IEnumerable<PropertyInfo>) properties).Count<PropertyInfo>() > 0)
            {
              foreach (PropertyInfo propertyInfo in properties)
              {
                if (propertyInfo.CanWrite && (propertyInfo.Name.Equals(filter.Name) || filter.Name.Contains(propertyInfo.Name)) && propertyInfo.GetValue(o, (object[]) null) != null)
                {
                  string name = propertyInfo.GetValue(o, (object[]) null).GetType().Name;
                  string str5 = propertyInfo.GetValue(o, (object[]) null).ToString();
                  if (str5.StartsWith(" ") || str5.EndsWith(" "))
                  {
                    string str6 = propertyInfo.GetValue(o, (object[]) null).ToString().Trim();
                    if (name.Contains("Int32"))
                      propertyInfo.SetValue(o, (object) Convert.ToInt32(str6), (object[]) null);
                    if (name.Contains("Int64"))
                      propertyInfo.SetValue(o, (object) Convert.ToInt64(str6), (object[]) null);
                    if (name.Contains("Double"))
                      propertyInfo.SetValue(o, (object) Convert.ToDouble(str6), (object[]) null);
                    if (name.Contains("Single") || name.Contains("float"))
                      propertyInfo.SetValue(o, (object) Convert.ToSingle(str6), (object[]) null);
                    if (name.Contains("Decimal"))
                    {
                      propertyInfo.SetValue(o, (object) Convert.ToDecimal(str6), (object[]) null);
                      break;
                    }
                    break;
                  }
                  break;
                }
              }
            }
            if (!((!this.loadInBackground ? (!(o is DataRowView) ? filter.ComputedValue(o) : filter.ComputedValue((object) (o as DataRowView).Row)) : filter.ComputedValue(o, true, this)) is bool flag6))
              ;
            if (!flag6)
              break;
          }
          if (flag6)
          {
            if (flag6)
              this.VisibleRecords.Add(o);
            flag2 = false;
          }
          else
            continue;
        }
        else
          this.VisibleRecords.Add(o);
        List<IComparable> comparableList1 = new List<IComparable>();
        List<IComparable> comparableList2 = new List<IComparable>();
        List<IComparable> comparableList3 = new List<IComparable>();
        List<object> values = new List<object>();
        list1.ForEach((Action<PropertyDescriptor>) (pd =>
        {
          if (pd == null)
          {
            values.Add(o);
          }
          else
          {
            object obj2 = (object) null;
            if (!(o is DataRowView))
              obj2 = this.GetReflectedValue(o, pd.Name);
            if (obj2 == null)
              obj2 = pd.GetValue(o);
            values.Add(obj2);
          }
        }));
        for (int index = 0; index < this.PivotRows.Count; ++index)
        {
          IComparable comparable1 = (IComparable) null;
          if (propertyDescriptorArray2[index] != null)
          {
            if (!(o is DataRowView))
              comparable1 = this.GetReflectedValue(o, propertyDescriptorArray2[index].Name) as IComparable;
            if (comparable1 == null)
              comparable1 = propertyDescriptorArray2[index] != null ? propertyDescriptorArray2[index].GetValue(o) as IComparable : (IComparable) null;
          }
          IComparable comparable2;
          if (comparable1 != null)
          {
            comparable2 = this.PivotRows[index].Format == null || this.PivotRows[index].Format.Length <= 0 ? (IComparable) comparable1.ToString() : (IComparable) string.Format((IFormatProvider) CultureInfo.CurrentUICulture, $"{{0:{this.PivotRows[index].Format}}}", (object) comparable1);
          }
          else
          {
            comparable2 = (IComparable) str1;
            flag4 = true;
          }
          if (this.IgnoreWhitespace && comparable2 != null && (char.IsWhiteSpace(comparable2.ToString()[0]) || char.IsWhiteSpace(comparable2.ToString()[comparable2.ToString().Length - 1])))
            comparable2 = (IComparable) comparable2.ToString().Trim(' ');
          comparableList2.Add(comparable2);
          comparableList3.Add(comparable2);
        }
        for (int index = 0; index < this.PivotColumns.Count; ++index)
        {
          IComparable comparable3 = (IComparable) null;
          if (propertyDescriptorArray1[index] != null)
          {
            if (!(o is DataRowView))
              comparable3 = this.GetReflectedValue(o, propertyDescriptorArray1[index].Name) as IComparable;
            if (comparable3 == null)
              comparable3 = propertyDescriptorArray1[index] != null ? propertyDescriptorArray1[index].GetValue(o) as IComparable : (IComparable) null;
          }
          IComparable comparable4;
          if (comparable3 != null)
          {
            comparable4 = this.PivotColumns[index].Format == null || this.PivotColumns[index].Format.Length <= 0 ? (IComparable) comparable3.ToString() : (IComparable) string.Format((IFormatProvider) CultureInfo.CurrentUICulture, $"{{0:{this.PivotColumns[index].Format}}}", (object) comparable3);
          }
          else
          {
            comparable4 = (IComparable) str1;
            flag5 = true;
          }
          if (this.IgnoreWhitespace && comparable4 != null && (char.IsWhiteSpace(comparable4.ToString()[0]) || char.IsWhiteSpace(comparable4.ToString()[comparable4.ToString().Length - 1])))
            comparable4 = (IComparable) comparable4.ToString().Trim(' ');
          comparableList1.Add(comparable4);
          comparableList3.Add(comparable4);
        }
        BinaryList columnKeysCalcValues = this.columnKeysCalcValues;
        KeysCalculationValues o1 = tempValue = new KeysCalculationValues()
        {
          Keys = comparableList1,
          Comparers = comparers1
        };
        int num4 = flag5 ? 1 : 0;
        int index1;
        if ((index1 = columnKeysCalcValues.AddIfUnique((IComparable) o1, num4 != 0)) < 0)
        {
          int k = 0;
          tempValue.Values = new List<SummaryBase>();
          this.PivotCalculations.ForEach((Action<PivotComputationInfo>) (info =>
          {
            sb = info.Summary.GetInstance();
            sb.ShowNullAsBlank = this.ShowNullAsBlank;
            sb.Combine(values[k++]);
            tempValue.Values.Add(sb);
          }));
        }
        else
        {
          tempValue = this.columnKeysCalcValues[index1] as KeysCalculationValues;
          for (int index2 = 0; index2 < this.PivotCalculations.Count; ++index2)
          {
            if (tempValue != null && tempValue.Values[index2] != null)
            {
              tempValue.Values[index2].ShowNullAsBlank = this.ShowNullAsBlank;
              tempValue.Values[index2].Combine(values[index2]);
            }
          }
        }
        BinaryList rowKeysCalcValues = this.rowKeysCalcValues;
        KeysCalculationValues o2 = tempValue = new KeysCalculationValues()
        {
          Keys = comparableList2,
          Comparers = comparers2
        };
        int num5 = flag4 ? 1 : 0;
        int index3;
        if ((index3 = rowKeysCalcValues.AddIfUnique((IComparable) o2, num5 != 0)) < 0)
        {
          int k = 0;
          tempValue.Values = new List<SummaryBase>();
          this.PivotCalculations.ForEach((Action<PivotComputationInfo>) (info =>
          {
            sb = info.Summary.GetInstance();
            sb.ShowNullAsBlank = this.ShowNullAsBlank;
            sb.Combine(values[k++]);
            tempValue.Values.Add(sb);
          }));
        }
        else
        {
          tempValue = this.rowKeysCalcValues[index3] as KeysCalculationValues;
          for (int index4 = 0; index4 < this.PivotCalculations.Count; ++index4)
          {
            if (tempValue != null && tempValue.Values != null)
            {
              tempValue.Values[index4].ShowNullAsBlank = this.ShowNullAsBlank;
              tempValue.Values[index4].Combine(values[index4++]);
            }
          }
        }
        BinaryList tableKeysCalcValues = this.tableKeysCalcValues;
        KeysCalculationValues o3 = tempValue = new KeysCalculationValues()
        {
          Keys = comparableList3
        };
        int index5;
        if ((index5 = tableKeysCalcValues.AddIfUnique((IComparable) o3)) < 0)
        {
          int k = 0;
          tempValue.Values = new List<SummaryBase>();
          this.PivotCalculations.ForEach((Action<PivotComputationInfo>) (info =>
          {
            sb = info.Summary.GetInstance();
            sb.ShowNullAsBlank = this.ShowNullAsBlank;
            sb.Combine(values[k++]);
            tempValue.Values.Add(sb);
          }));
        }
        else
        {
          tempValue = this.tableKeysCalcValues[index5] as KeysCalculationValues;
          for (int index6 = 0; index6 < this.PivotCalculations.Count; ++index6)
          {
            if (tempValue != null && tempValue.Values != null)
            {
              tempValue.Values[index6].ShowNullAsBlank = this.ShowNullAsBlank;
              tempValue.Values[index6].Combine(values[index6]);
            }
          }
        }
        if (this.CacheRawValues && tempValue.RawValues != null)
          tempValue.RawValues.Add(o);
      }
      if (flag2)
      {
        this.rowCount = 1;
        this.emptyPivot = true;
        this.columnCount = 1;
        if (this.pivotValues == null)
          return;
        this.pivotValues = new PivotCellInfos(1, 1);
        this.pivotValues[0, 0] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = 0
        };
        this.pivotValues[0, 0].CellType = PivotCellType.TopLeftCell;
        this.pivotValues[0, 0].Value = (object) this.EmptyPivotString;
        this.pivotValues[0, 0].FormattedText = this.EmptyPivotString;
        return;
      }
      if (flag3)
        source1.RowFilter = str2;
    }
    this.DoUniqueValuesCount(this.PivotColumns, this.columnKeysCalcValues, 1);
    this.DoUniqueValuesCount(this.PivotRows, this.rowKeysCalcValues, 1);
    bool? notPopulated1 = this.NotPopulated;
    if ((notPopulated1.GetValueOrDefault() ? 1 : (!notPopulated1.HasValue ? 1 : 0)) == 0)
      return;
    this.populationStatus = 1.0;
  }

  private void DoCalculationTable()
  {
    if (this.PivotCalculations.Count == 0)
      return;
    int length1 = this.valuesArea.GetLength(0);
    int length2 = this.valuesArea.GetLength(1);
    for (int i = 0; i < length1; ++i)
    {
      bool flag1 = false;
      if (this.rowSummary[i + this.rowOffSet])
        flag1 = true;
      for (int j = 0; j < length2; j += this.PivotCalculations.Count)
      {
        bool flag2 = false;
        if (!flag1 && this.columnSummary[j + this.colOffSet])
          flag2 = true;
        if (flag1 || flag2)
        {
          this.InitializeSummary(i, j);
        }
        else
        {
          int index1 = this.tableKeysCalcValues.BinarySearch((IComparable) new KeysCalculationValues()
          {
            Keys = this.GetKeyAt(i + this.rowOffSet, j + this.colOffSet)
          });
          if (index1 > -1)
          {
            for (int index2 = 0; index2 < this.PivotCalculations.Count; ++index2)
            {
              this.valuesArea[i, j + index2] = ((KeysCalculationValues) this.tableKeysCalcValues[index1]).Values[index2];
              this.valuesArea[i, j + index2].ShowNullAsBlank = this.ShowNullAsBlank;
            }
          }
          else
            this.InitializeSummary(i, j);
        }
      }
    }
    if (this.EnableOnDemandCalculations)
      return;
    this.ProcessTotals3();
  }

  internal void InitializeSummary(int i, int j)
  {
    int num = 0;
    foreach (PivotComputationInfo pivotCalculation in this.PivotCalculations)
    {
      SummaryBase instance = pivotCalculation.Summary.GetInstance();
      instance.ShowNullAsBlank = this.ShowNullAsBlank;
      if (!this.ShowNullAsBlank && pivotCalculation.DefaultValue == null)
      {
        instance.Combine(pivotCalculation.DefaultValue);
        this.valuesArea[i, j + num] = instance;
      }
      else if (pivotCalculation.DefaultValue != null)
      {
        instance.Combine(pivotCalculation.DefaultValue);
        this.valuesArea[i, j + num] = instance;
      }
      ++num;
    }
  }

  private void CellEditing()
  {
    int length1 = this.rowHeaders.GetLength(1);
    int length2 = this.columnHeaders.GetLength(0);
    Array array1 = this.EditCellsInfo["Value"];
    Array array2 = this.EditCellsInfo["Index"];
    Array array3 = this.EditCellsInfo["Measure"];
    for (int index1 = 0; index1 < array1.Length; ++index1)
    {
      int index2 = Convert.ToInt32(array2.GetValue(index1).ToString().Split(',')[0]) - length1;
      int index3 = Convert.ToInt32(array2.GetValue(index1).ToString().Split(',')[1]) - (length2 == 0 ? 1 : length2);
      foreach (PivotComputationInfo pivotCalculation in this.PivotCalculations)
      {
        if (pivotCalculation.FieldName.Equals(array3.GetValue(index1)))
        {
          SummaryBase instance = pivotCalculation.Summary.GetInstance();
          instance.ShowNullAsBlank = this.ShowNullAsBlank;
          int num = pivotCalculation.Format.Equals("P") ? 100 : 1;
          double result;
          if (double.TryParse(array1.GetValue(index1).ToString(), out result))
          {
            instance.Combine((object) result);
            this.valuesArea[index3, index2] = instance;
            break;
          }
          string input = Regex.Replace(array1.GetValue(index1).ToString(), "[^A-Za-z0-9~. _]", "").Trim();
          if (Regex.IsMatch(input, "^[0-9]+(\\.[0-9]+)?$") || Regex.IsMatch(input, "^\\d+$"))
            instance.Combine((object) (Regex.IsMatch(input, "^\\d+$") ? (double) (Convert.ToInt32(input) / num) : Convert.ToDouble(input) / (double) num));
          else
            instance.Combine((object) 0);
          this.valuesArea[index3, index2] = instance;
          break;
        }
      }
    }
  }

  private void CoverRowHeader(int row)
  {
    if (this.PivotRows.Count == 0)
      return;
    int count1 = this.PivotRows.Count;
    int num1 = this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0);
    if (num1 > row)
      return;
    if (row >= 1 && this.pivotValues != null && this.pivotValues[0, 0].CellRange == null)
    {
      CoveredCellRange coveredCellRange = new CoveredCellRange(0, 0, row - 1, count1 - 1);
      this.pivotValues[0, 0].CellRange = coveredCellRange;
      this.pivotValues[0, 0].CellType = PivotCellType.TopLeftCell;
      this.CoveredRanges.Add(coveredCellRange);
    }
    int count2 = this.PivotRows.Count;
    for (int startCol = 0; startCol < count2; ++startCol)
    {
      string uniqueText = this.pivotValues[row, startCol].UniqueText;
      int val1 = startCol + 1;
      while (val1 <= count2 && this.pivotValues[row, val1 - 1] != null && this.pivotValues[row, val1 - 1].UniqueText == null)
        ++val1;
      if (this.pivotValues[row, startCol] != null && uniqueText != null && this.pivotValues[row, startCol].FormattedText == uniqueText)
      {
        List<\u003C\u003Ef__AnonymousType0<string, int>> list = ((IEnumerable) this.rowHeaderUniqueValues).Cast<string>().ToList<string>().GroupBy<string, string>((System.Func<string, string>) (x => x)).Where<IGrouping<string, string>>((System.Func<IGrouping<string, string>, bool>) (x => x.Count<string>() > 1)).Select(y => new
        {
          Element = y.Key,
          Counter = y.Count<string>()
        }).ToList();
        int num2 = this.pivotValues[row, 0] == null || this.pivotValues[row, 0].FormattedText == null || !list.Any(x => x.Element == this.pivotValues[row, startCol].FormattedText) ? -1 : list.Where(x => x.Element == this.pivotValues[row, startCol].FormattedText).FirstOrDefault().Counter;
        CoveredCellRange range = new CoveredCellRange(row, startCol, num2 > -1 ? num2 + row - 1 : row, this.pivotValues[row, startCol].FormattedText == null || !this.pivotValues[row, startCol].FormattedText.Contains(this.delimiter) ? Math.Min(val1, count2) - 1 : count2 - 1);
        if (this.OkToAddRange(range))
          this.CoveredRanges.Add(range);
        this.pivotValues[row, startCol].CellRange = range;
        this.pivotValues[row, startCol].CellType = !this.ShowSubTotalsForChildren || this.PivotRows.Count <= 2 ? (PivotCellType) (64 /*0x40*/ | (range.Bottom - range.Top != 0 ? 2 : 16 /*0x10*/)) : (PivotCellType) (64 /*0x40*/ | (range.Right - range.Left == 0 ? 2 : 16 /*0x10*/));
      }
      else if (this.PivotCalculations.Count >= 0 && this.pivotValues[row, startCol] != null && this.pivotValues[row, startCol].UniqueText != null && this.pivotValues[row, startCol].UniqueText.ToString().IndexOf(this.delimiter) > -1 && this.pivotValues[row, startCol].UniqueText.ToString() != "x")
      {
        CoveredCellRange range = new CoveredCellRange(row, startCol, num1 - 1, startCol);
        if (this.OkToAddRange(range))
          this.CoveredRanges.Add(range);
        this.pivotValues[row, startCol].CellRange = range;
        this.pivotValues[row, startCol].CellType = PivotCellType.ExpanderCell | PivotCellType.RowHeaderCell;
      }
      else if (this.pivotValues[row, startCol] != null)
        this.pivotValues[row, startCol].CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell;
    }
  }

  private void PopulateRow(int row)
  {
    this.rowCount = this.rowHeaders.GetLength(0) + (this.PivotRows.Count <= 0 && this.PivotCalculations.Count <= 0 || !this.ShowGrandTotals ? (this.PivotRows.Count > 0 || this.PivotColumns.Count > 0 && this.PivotCalculations.Count > 0 ? 1 : 0) : 1) - this.grandRowIndex;
    this.columnCount = this.columnHeaders.GetLength(1) + (this.PivotColumns.Count <= 0 && this.PivotCalculations.Count <= 0 || !this.ShowGrandTotals ? (this.PivotColumns.Count > 0 || this.PivotRows.Count > 0 && this.PivotCalculations.Count > 0 ? 1 : 0) : 1) - this.grandColumnIndex;
    if (this.pivotValues == null && this.rowCount != 0 && this.columnCount != 0)
      this.pivotValues = new PivotCellInfos(this.rowCount, this.columnCount);
    int length1 = this.rowHeaders.GetLength(1);
    int num = this.columnHeaders.GetLength(0) + 1;
    if (row >= num - 1)
    {
      for (int colIndex = 0; colIndex < length1; ++colIndex)
      {
        this.pivotValues[row, colIndex] = new PivotCellInfo()
        {
          RowIndex = row,
          ColumnIndex = colIndex
        };
        if (row == 0 || this.rowHeaders[row, colIndex] != null && this.rowHeaders[row, colIndex].CompareTo((object) this.rowHeaders[row - 1, colIndex]) != 0)
        {
          this.pivotValues[row, colIndex].Value = (object) this.rowHeaders[row, colIndex];
          this.pivotValues[row, colIndex].FormattedText = this.rowHeaders[row, colIndex] == null ? (string) null : this.rowHeaders[row, colIndex].ToString();
        }
        this.pivotValues[row, colIndex].UniqueText = this.rowHeaderUniqueValues[row, colIndex];
        this.PivotValues[row, colIndex].Key = this.rowHeaders[row, colIndex] == null ? (this.pivotValues[row, colIndex].UniqueText == null ? (string) null : this.pivotValues[row, colIndex].UniqueText) : this.rowHeaders[row, colIndex].ToString();
      }
    }
    else if (row < num - 1)
    {
      for (int colIndex = 0; colIndex < this.columnHeaders.GetLength(1) - this.grandColumnIndex; ++colIndex)
      {
        if (this.pivotValues[row, colIndex] == null)
          this.pivotValues[row, colIndex] = new PivotCellInfo()
          {
            RowIndex = row,
            ColumnIndex = colIndex
          };
        if (colIndex == 0 || this.columnHeaderUniqueValues[row, colIndex] != null)
        {
          this.pivotValues[row, colIndex].Value = (object) this.columnHeaders[row, colIndex];
          this.pivotValues[row, colIndex].FormattedText = this.columnHeaders[row, colIndex] == null ? (string) null : this.columnHeaders[row, colIndex].ToString();
        }
        this.pivotValues[row, colIndex].UniqueText = this.columnHeaderUniqueValues[row, colIndex];
        this.PivotValues[row, colIndex].Key = this.columnHeaders[row, colIndex] == null ? (this.pivotValues[row, colIndex].UniqueText == null ? (string) null : this.pivotValues[row, colIndex].UniqueText) : this.columnHeaders[row, colIndex].ToString();
      }
    }
    bool isSummaryRow = this.IsRowSummary(row + num - 1);
    for (int col = 0; col < this.valuesArea.GetLength(1); ++col)
      this.PopulateValueCell(isSummaryRow, row, col, this.rowHeaders.GetLength(1), this.columnHeaders.GetLength(0) + 1, 0, 0, 0, 0);
    if (this.PivotColumns.Count == 0 && this.PivotRows.Count > 0 && this.PivotCalculations.Count <= 1 && !this.IsCalculationHeaderVisible)
    {
      int rowCount = this.pivotValues.GetLength(0) + 1;
      int length2 = this.pivotValues.GetLength(1);
      List<CoveredCellRange> ranges = new List<CoveredCellRange>();
      PivotCellInfos pivotCellInfos = new PivotCellInfos(rowCount, length2);
      int rowIndex = row;
      for (int colIndex = 0; colIndex < length2; ++colIndex)
      {
        pivotCellInfos[rowIndex, colIndex] = this.pivotValues[rowIndex - 1, colIndex];
        if (pivotCellInfos[rowIndex, colIndex] != null && pivotCellInfos[rowIndex, colIndex].CellRange != null)
        {
          pivotCellInfos[rowIndex, colIndex].CellRange = new CoveredCellRange(pivotCellInfos[rowIndex, colIndex].CellRange.Top + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Left, pivotCellInfos[rowIndex, colIndex].CellRange.Bottom + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Right);
          if (this.OkToAddRange(pivotCellInfos[rowIndex, colIndex].CellRange, ranges))
            ranges.Add(pivotCellInfos[rowIndex, colIndex].CellRange);
        }
      }
      int count = this.pivotRows.Count;
      for (int colIndex = count; colIndex < count + this.PivotCalculations.Count; ++colIndex)
        pivotCellInfos[0, colIndex] = new PivotCellInfo()
        {
          CellType = PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell,
          RowIndex = 0,
          ColumnIndex = colIndex,
          Value = this.UseDescriptionInCalculationHeader ? (object) this.PivotCalculations[colIndex - count].Description : (object) this.PivotCalculations[colIndex - count].FieldName,
          FormattedText = this.UseDescriptionInCalculationHeader ? this.PivotCalculations[colIndex - count].Description : this.PivotCalculations[colIndex - count].FieldHeader
        };
      pivotCellInfos[0, 0] = new PivotCellInfo()
      {
        ColumnIndex = 0,
        RowIndex = 0,
        CellType = PivotCellType.TopLeftCell,
        CellRange = new CoveredCellRange(0, 0, 0, count - 1)
      };
      ranges.Add(pivotCellInfos[0, 0].CellRange);
      for (int colIndex = 1; colIndex < count; ++colIndex)
        pivotCellInfos[0, colIndex] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = colIndex
        };
      this.pivotValues = pivotCellInfos;
      ++this.rowCount;
      this.coveredRanges = ranges;
    }
    else if (this.PivotColumns.Count == 0 && this.PivotRows.Count > 0 && this.PivotCalculations.Count > 0)
    {
      int count = this.pivotRows.Count;
      for (int colIndex = count; colIndex < count + this.PivotCalculations.Count; ++colIndex)
      {
        if (colIndex < this.pivotValues.GetLength(1) && this.pivotValues[0, colIndex] != null)
          this.pivotValues[0, colIndex].CellType = PivotCellType.CalculationHeaderCell | PivotCellType.ColumnHeaderCell;
      }
    }
    else if (this.PivotColumns.Count > 0 && this.PivotRows.Count == 0 && this.PivotCalculations.Count > 0)
    {
      int length3 = this.pivotValues.GetLength(0);
      int colCount = this.pivotValues.GetLength(1) + 1;
      List<CoveredCellRange> ranges = new List<CoveredCellRange>();
      PivotCellInfos pivotCellInfos = new PivotCellInfos(length3, colCount);
      for (int rowIndex = 0; rowIndex < length3; ++rowIndex)
      {
        for (int colIndex = 1; colIndex < colCount; ++colIndex)
        {
          pivotCellInfos[rowIndex, colIndex] = this.pivotValues[rowIndex, colIndex - 1];
          if (pivotCellInfos[rowIndex, colIndex] != null && pivotCellInfos[rowIndex, colIndex].CellRange != null)
          {
            pivotCellInfos[rowIndex, colIndex].CellRange = new CoveredCellRange(pivotCellInfos[rowIndex, colIndex].CellRange.Top, pivotCellInfos[rowIndex, colIndex].CellRange.Left + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Bottom, pivotCellInfos[rowIndex, colIndex].CellRange.Right + 1);
            if (this.OkToAddRange(pivotCellInfos[rowIndex, colIndex].CellRange, ranges))
              ranges.Add(pivotCellInfos[rowIndex, colIndex].CellRange);
          }
        }
      }
      pivotCellInfos[0, 0] = new PivotCellInfo()
      {
        ColumnIndex = 0,
        RowIndex = 0,
        CellType = PivotCellType.TopLeftCell,
        CellRange = new CoveredCellRange(0, 0, this.PivotColumns.Count - (this.IsCalculationHeaderVisible ? 0 : 1), 0)
      };
      ranges.Add(pivotCellInfos[0, 0].CellRange);
      for (int rowIndex = length3 - 1; rowIndex > 0; --rowIndex)
        pivotCellInfos[rowIndex, 0] = new PivotCellInfo()
        {
          RowIndex = rowIndex,
          ColumnIndex = 0,
          CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell,
          FormattedText = $"{this.GrandString}{this.delimiter} {this.PivotColumns[0].TotalHeader}"
        };
      this.pivotValues = pivotCellInfos;
      ++this.columnCount;
      this.coveredRanges = ranges;
    }
    else if (this.PivotColumns.Count == 0 && this.PivotRows.Count == 0 && this.PivotCalculations.Count > 0)
    {
      int length4 = this.pivotValues.GetLength(0);
      int colCount = this.pivotValues.GetLength(1) + 1;
      List<CoveredCellRange> ranges = new List<CoveredCellRange>();
      PivotCellInfos pivotCellInfos = new PivotCellInfos(length4, colCount);
      for (int rowIndex = 0; rowIndex < length4; ++rowIndex)
      {
        for (int colIndex = 1; colIndex < colCount; ++colIndex)
        {
          pivotCellInfos[rowIndex, colIndex] = this.pivotValues[rowIndex, colIndex - 1];
          if (pivotCellInfos[rowIndex, colIndex] != null && pivotCellInfos[rowIndex, colIndex].CellRange != null)
          {
            pivotCellInfos[rowIndex, colIndex].CellRange = new CoveredCellRange(pivotCellInfos[rowIndex, colIndex].CellRange.Top, pivotCellInfos[rowIndex, colIndex].CellRange.Left + 1, pivotCellInfos[rowIndex, colIndex].CellRange.Bottom, pivotCellInfos[rowIndex, colIndex].CellRange.Right + 1);
            if (this.OkToAddRange(pivotCellInfos[rowIndex, colIndex].CellRange, ranges))
              ranges.Add(pivotCellInfos[rowIndex, colIndex].CellRange);
          }
        }
      }
      if (this.PivotCalculations.Count == 1)
        pivotCellInfos[0, 0] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = 0,
          CellType = PivotCellType.TopLeftCell,
          CellRange = new CoveredCellRange(0, 0, 0, 0)
        };
      else
        pivotCellInfos[0, 0] = new PivotCellInfo()
        {
          RowIndex = 0,
          ColumnIndex = 0,
          CellType = PivotCellType.TopLeftCell,
          CellRange = new CoveredCellRange(0, 0, this.PivotColumns.Count - (this.IsCalculationHeaderVisible ? 0 : 1), 0)
        };
      ranges.Add(pivotCellInfos[0, 0].CellRange);
      for (int rowIndex = length4 - 1; rowIndex > 0; --rowIndex)
        pivotCellInfos[rowIndex, 0] = new PivotCellInfo()
        {
          RowIndex = rowIndex,
          ColumnIndex = 0,
          CellType = PivotCellType.HeaderCell | PivotCellType.RowHeaderCell | PivotCellType.GrandTotalCell,
          FormattedText = $"{this.GrandString}{this.delimiter} Total"
        };
      this.pivotValues = pivotCellInfos;
      this.columnCount += this.ShowGrandTotals ? 1 : 0;
      this.coveredRanges = ranges;
    }
    if (this.PivotColumns.Count <= 0 && this.PivotRows.Count <= 0)
      return;
    if (this.pivotValues != null && this.pivotValues[0, 0].CellRange != null)
    {
      for (int rowIndex = 0; rowIndex <= this.pivotValues[0, 0].CellRange.Bottom; ++rowIndex)
      {
        for (int colIndex = 0; colIndex <= this.pivotValues[0, 0].CellRange.Right; ++colIndex)
        {
          if (this.pivotValues[rowIndex, colIndex] != null)
            this.pivotValues[rowIndex, colIndex].CellType = PivotCellType.ValueCell | PivotCellType.TopLeftCell;
        }
      }
    }
    if (this.pivotValues == null || this.pivotValues[0, 0] == null)
      return;
    this.pivotValues[0, 0].CellType = PivotCellType.TopLeftCell;
  }

  private void DoCalculationTable(int i)
  {
    for (int index1 = 0; index1 < this.valuesArea.GetLength(1); index1 += this.PivotCalculations.Count)
    {
      int index2 = this.tableKeysCalcValues.BinarySearch((IComparable) new KeysCalculationValues()
      {
        Keys = this.GetKeyAt(i + this.rowOffSet, index1 + this.colOffSet)
      });
      if (index2 > -1)
      {
        for (int index3 = 0; index3 < this.PivotCalculations.Count; ++index3)
        {
          this.valuesArea[i, index1 + index3] = ((KeysCalculationValues) this.tableKeysCalcValues[index2]).Values[index3];
          this.valuesArea[i, index1 + index3].ShowNullAsBlank = this.ShowNullAsBlank;
        }
      }
      else
      {
        int num = 0;
        foreach (PivotComputationInfo pivotCalculation in this.PivotCalculations)
        {
          SummaryBase instance = pivotCalculation.Summary.GetInstance();
          instance.ShowNullAsBlank = this.ShowNullAsBlank;
          if (!this.ShowNullAsBlank && pivotCalculation.DefaultValue == null)
          {
            instance.Combine(pivotCalculation.DefaultValue);
            this.valuesArea[i, index1 + num] = instance;
          }
          else if (pivotCalculation.DefaultValue != null)
          {
            instance.Combine(pivotCalculation.DefaultValue);
            this.valuesArea[i, index1 + num] = instance;
          }
          ++num;
        }
      }
    }
  }

  private void PopulateValueCell(
    bool isSummaryRow,
    int row,
    int col,
    int valueColStart,
    int valueRowStart,
    int k,
    int key,
    int key1,
    int count)
  {
    if (row < 0 || col < 0 || row >= this.valuesArea.GetLength(0) || col >= this.valuesArea.GetLength(1))
      return;
    int[] numArray1 = new int[2];
    int[] numArray2 = new int[2];
    List<PivotItem> pivotRows = this.PivotRows;
    List<PivotItem> pivotColumns = this.PivotColumns;
    BinaryList binaryList = new BinaryList();
    List<object> objectList1 = new List<object>();
    List<string> stringList = new List<string>();
    Dictionary<int, object> dictionary = new Dictionary<int, object>();
    int index1 = row + valueRowStart - 1;
    int index2 = col + valueColStart;
    bool isSummaryColumn = this.columnSummary[index2];
    if (this.pivotValues[index1, index2] == null)
      this.pivotValues[index1, index2] = new PivotCellInfo()
      {
        RowIndex = index1,
        ColumnIndex = index2
      };
    if (index2 >= this.pivotValues.GetLength(1) || index1 >= this.pivotValues.GetLength(0))
      return;
    if (this.valuesArea[row, col] != null)
    {
      PivotComputationInfo compInfo = this.PivotCalculations.Count == 0 ? (PivotComputationInfo) null : this.PivotCalculations[col % this.PivotCalculations.Count];
      object result1 = this.valuesArea[row, col].GetResult() == null || !(this.valuesArea[row, col].GetResult().ToString() != "NaN") ? (object) null : this.valuesArea[row, col].GetResult();
      if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText != null && !this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText.ToUpper().Contains("TOTAL") && !this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText.Contains(this.PivotColumns[this.PivotColumns.Count - 1].TotalHeader))
        binaryList.AddIfUnique((IComparable) this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText);
      this.pivotValues[index1, index2].Summary = this.valuesArea[row, col];
      string format = $"{{0:{compInfo.Format}}}";
      if (format.Equals("{0:#.##}") && result1 != null && result1.Equals((object) 0.0) && compInfo.CalculationType != CalculationType.Formula)
      {
        if (this.pivotValues[index1, index2] != null && this.PivotCalculations[col % this.PivotCalculations.Count].CalculationType.ToString().Contains("Percentage"))
          this.pivotValues[index1, index2].FormattedText = "0.00%";
        else if (this.pivotValues[index1, index2] != null)
          this.pivotValues[index1, index2].FormattedText = "0.0";
      }
      else if (format.Equals("{0:###}") && result1 != null && result1.Equals((object) 0))
      {
        if (this.pivotValues[index1, index2] != null && this.PivotCalculations[col % this.PivotCalculations.Count].CalculationType.ToString().Contains("Percentage"))
          this.pivotValues[index1, index2].FormattedText = "0%";
        else if (this.pivotValues[index1, index2] != null)
          this.pivotValues[index1, index2].FormattedText = "0";
      }
      else
      {
        switch (this.PivotCalculations[col % this.PivotCalculations.Count].CalculationType)
        {
          case CalculationType.NoCalculation:
          case CalculationType.Distinct:
            if (this.pivotValues[index1, index2] != null)
            {
              if (result1 != null || !this.ShowNullAsBlank)
                this.pivotValues[index1, index2].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, result1);
              this.pivotValues[index1, index2].Value = result1;
              break;
            }
            break;
          case CalculationType.PercentageOfGrandTotal:
            object result2 = this.valuesArea[this.valuesArea.GetLength(0) - 1, this.valuesArea.GetLength(1) + (col % this.PivotCalculations.Count - this.PivotCalculations.Count)].GetResult();
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(result2) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            break;
          case CalculationType.PercentageOfColumnTotal:
            object result3 = this.valuesArea[this.valuesArea.GetLength(0) - 1, col].GetResult();
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(result3) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            break;
          case CalculationType.PercentageOfRowTotal:
            object result4 = this.valuesArea[row, this.valuesArea.GetLength(1) + (col % this.PivotCalculations.Count - this.PivotCalculations.Count)].GetResult();
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(result4) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            break;
          case CalculationType.PercentageOfParentColumnTotal:
            object result5;
            if (isSummaryColumn)
            {
              int[] parentColumnIndex = this.GetNextParentColumnIndex(numArray2[0], index2);
              result5 = this.valuesArea[row, parentColumnIndex[1]].GetResult();
            }
            else
            {
              int[] parentColumnIndex = this.GetNextParentColumnIndex(this.columnHeaders.GetLength(0) - (1 + (this.IsCalculationHeaderVisible ? 1 : 0)), index2);
              result5 = this.valuesArea[row, parentColumnIndex[1]].GetResult();
            }
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(result5) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            break;
          case CalculationType.PercentageOfParentRowTotal:
            object obj1 = (object) 0;
            object obj2;
            if (isSummaryRow)
            {
              int parentColumnIndex = 0;
              for (int index3 = 0; index3 < this.PivotRows.Count; ++index3)
              {
                if (this.rowHeaders[index1, index3] == null)
                {
                  parentColumnIndex = index3 - 1;
                  break;
                }
              }
              obj2 = this.PivotRows.Count <= 2 || parentColumnIndex <= 0 ? this.valuesArea[this.GetNextParentRowIndex(index1, this.rowHeaders.GetLength(1) - this.PivotRows.Count)[0], col].GetResult() : this.valuesArea[this.GetNextParentRowIndex(index1, parentColumnIndex)[0], col].GetResult();
            }
            else
              obj2 = this.valuesArea[this.GetNextParentRowIndex(index1, this.rowHeaders.GetLength(1) - 1)[0], col].GetResult();
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(obj2) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            break;
          case CalculationType.PercentageOfParentTotal:
            if (compInfo.BaseField == null)
              throw new ArgumentNullException("Base Field value should be required.");
            int index4 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
            object obj3 = (object) null;
            if (index4 != -1)
            {
              int parentRowIndex = this.GetParentRowIndex(row, index4, isSummaryRow);
              obj3 = parentRowIndex != -1 ? this.valuesArea[parentRowIndex, col].GetResult() : (object) null;
            }
            else
            {
              int index5 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
              if (index5 != -1)
              {
                int parentColumnIndex = this.GetParentColumnIndex(index5, col, isSummaryColumn);
                obj3 = parentColumnIndex != -1 ? this.valuesArea[row, parentColumnIndex].GetResult() : (object) null;
              }
            }
            if (obj3 != null && this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(obj3) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) null;
              this.pivotValues[index1, index2].FormattedText = string.Empty;
              break;
            }
            break;
          case CalculationType.Index:
            object result6 = this.valuesArea[this.valuesArea.GetLength(0) - 1, this.valuesArea.GetLength(1) + (col % this.PivotCalculations.Count - this.PivotCalculations.Count)].GetResult();
            object result7 = this.valuesArea[this.valuesArea.GetLength(0) - 1, col].GetResult();
            object result8 = this.valuesArea[row, this.valuesArea.GetLength(1) + (col % this.PivotCalculations.Count - this.PivotCalculations.Count)].GetResult();
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) * Convert.ToDouble(result6) / (Convert.ToDouble(result8) * Convert.ToDouble(result7)));
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? Math.Round((double) this.pivotValues[index1, index2].Value, 9).ToString((IFormatProvider) CultureInfo.CurrentUICulture) : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value, (object) 9);
              break;
            }
            break;
          case CalculationType.Formula:
            if (result1 != null)
            {
              this.CalculateFormula(index1, index2, col, compInfo);
              break;
            }
            break;
          case CalculationType.PercentageOf:
          case CalculationType.DifferenceFrom:
          case CalculationType.PercentageOfDifferenceFrom:
            if (compInfo.BaseField == null || compInfo.BaseItem == null)
              throw new ArgumentNullException("Base Field value should be required.");
            this.FindRowColumnItems(pivotRows, pivotColumns);
            int num1 = 0;
            int index6 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
            object obj4 = (object) null;
            if (index6 != -1)
            {
              int index7 = this.pivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (p => p.FieldName == compInfo.FieldName));
              if (row == 0 && col == index7)
              {
                for (int rowIndex = this.pivotColumns.Count + 1; rowIndex < this.rowCount - 1; ++rowIndex)
                {
                  if (this.pivotValues[rowIndex, index6 + 1] != null && this.pivotValues[rowIndex, index6 + 1].UniqueText != null)
                  {
                    string formattedText = this.pivotValues[rowIndex, index6 + 1].FormattedText;
                    if (formattedText != null && this.pivotValues[rowIndex, index6 + 1] != null && this.pivotValues[rowIndex, index6 + 1].UniqueText.Contains(compInfo.BaseItem))
                      stringList.Add(formattedText);
                  }
                }
                count = stringList.Count;
              }
              int num2 = count;
              List<object> pivotBaseRowItem = this.PivotBaseRowItems[index6] as List<object>;
              int num3 = pivotBaseRowItem.FindIndex((Predicate<object>) (f => f.ToString() == compInfo.BaseItem));
              string uniqueText = this.pivotValues[row + this.pivotColumns.Count + 1, index6] != null ? this.pivotValues[row + this.pivotColumns.Count + 1, index6].UniqueText : "";
              if (index6 == this.PivotRows.Count - 1)
              {
                if (row > pivotBaseRowItem.Count)
                  num3 = pivotBaseRowItem.Count + num3 + 1;
                if (this.valuesArea[this.PivotRows.Count - index6 - 1 + num3, col] != null)
                  obj4 = index6 != -1 ? this.valuesArea[this.PivotRows.Count - index6 - 1 + num3, col].GetResult() : (object) null;
              }
              else if (index6 == 0)
              {
                if (num3 == 0)
                {
                  if (row <= num2)
                    obj4 = index6 != -1 ? this.valuesArea[index6 + row, col].GetResult() : (object) null;
                  else if (this.valuesArea[index6 + row - num2 - 1, col] != null)
                    obj4 = index6 != -1 ? this.valuesArea[index6 + row - num2 - 1, col].GetResult() : (object) null;
                }
                else if (uniqueText != null && uniqueText.Contains(compInfo.BaseItem))
                  obj4 = index6 != -1 ? this.valuesArea[index6 + row, col].GetResult() : (object) null;
                else if (uniqueText != null && !uniqueText.Contains("Grand") && index6 + row + num2 < this.valuesArea.GetLength(0) && this.valuesArea[index6 + row + num2, col] != null)
                  obj4 = index6 != -1 ? this.valuesArea[index6 + row + num2, col].GetResult() : (object) null;
              }
            }
            else
            {
              int index8 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
              if (index8 != -1)
              {
                int index9 = this.pivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (p => p.FieldName == compInfo.FieldName));
                if (row == 0 && col == index9)
                {
                  for (int count1 = this.pivotRows.Count; count1 < this.columnCount - 1; count1 += this.pivotCalculations.Count)
                  {
                    if (this.pivotValues[index8 + 1, count1] != null && this.pivotValues[index8 + 1, count1].FormattedText != null)
                    {
                      string formattedText = this.pivotValues[index8 + 1, count1].FormattedText;
                      if (formattedText != null && this.pivotValues[index8 + 1, count1] != null && this.pivotValues[index8 + 1, count1].UniqueText != null && this.pivotValues[index8 + 1, count1].UniqueText.Contains(compInfo.BaseItem))
                        stringList.Add(formattedText);
                    }
                  }
                  count = stringList.Count;
                }
                string uniqueText = this.pivotValues[index8, this.pivotRows.Count + col].UniqueText;
                num1 = (this.PivotBaseColumnItems[index8] as List<object>).FindIndex((Predicate<object>) (f => f.ToString() == compInfo.BaseItem));
                if (index8 == this.pivotColumns.Count - 1)
                {
                  for (int count2 = this.pivotRows.Count; count2 < this.columnCount - 1; ++count2)
                  {
                    string str1 = this.pivotValues[index8, count2] != null ? this.pivotValues[index8, count2].FormattedText : " ";
                    string str2 = this.pivotValues[index8 + 1, count2] != null ? this.pivotValues[index8 + 1, count2].FormattedText : " ";
                    if (str1 != null && str1 == compInfo.BaseItem && this.valuesArea[row, count2 - this.pivotRows.Count] != null && (str2 != null && str2 == compInfo.FieldName || this.PivotCalculations.Count == 1))
                    {
                      obj4 = this.valuesArea[row, count2 - this.pivotRows.Count].GetResult();
                      break;
                    }
                  }
                }
                else if (uniqueText != null && uniqueText.Contains(compInfo.BaseItem))
                {
                  obj4 = index8 != -1 ? this.valuesArea[row, col].GetResult() : (object) null;
                  dictionary.Add(key, obj4);
                  ++key;
                }
                else if (dictionary.Count != 0)
                {
                  if (key1 >= key)
                    key1 = 0;
                  obj4 = dictionary[key1];
                  ++key1;
                }
              }
            }
            if (this.pivotValues[index1, index2] != null && obj4 != null && row != this.rowCount - valueRowStart - 1)
            {
              if (compInfo.CalculationType == CalculationType.PercentageOf)
                this.pivotValues[index1, index2].Value = (object) (Convert.ToDouble(result1) / Convert.ToDouble(obj4) * 100.0);
              if (compInfo.CalculationType == CalculationType.DifferenceFrom)
              {
                this.pivotValues[index1, index2].Value = Convert.ToDouble(result1) <= Convert.ToDouble(obj4) ? (object) (Convert.ToDouble(obj4) - Convert.ToDouble(result1)) : (object) (Convert.ToDouble(result1) - Convert.ToDouble(obj4));
                this.UsePercentageFormat = false;
              }
              if (compInfo.CalculationType == CalculationType.PercentageOfDifferenceFrom)
                this.pivotValues[index1, index2].Value = Convert.ToDouble(result1) <= Convert.ToDouble(obj4) ? (object) ((Convert.ToDouble(obj4) - Convert.ToDouble(result1)) / Convert.ToDouble(obj4) * 100.0) : (object) ((Convert.ToDouble(result1) - Convert.ToDouble(obj4)) / Convert.ToDouble(obj4) * 100.0);
              this.pivotValues[index1, index2].FormattedText = this.UsePercentageFormat ? ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%" : string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
              break;
            }
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) null;
              this.pivotValues[index1, index2].FormattedText = string.Empty;
              break;
            }
            break;
          case CalculationType.RunningTotalIn:
          case CalculationType.PercentageOfRunningTotalIn:
            if (compInfo.BaseField == null)
              throw new ArgumentNullException("Base Field value should be required.");
            this.FindRowColumnItems(pivotRows, pivotColumns);
            int index10 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
            object obj5 = (object) null;
            if (index10 != -1)
            {
              List<object> pivotBaseRowItem = this.PivotBaseRowItems[this.pivotRows.Count - 1] as List<object>;
              if (index10 == this.pivotRows.Count - 1)
              {
                int index11 = row != pivotBaseRowItem.Count + 1 ? 0 : row;
                if (this.valuesArea[index11, col] != null)
                  obj5 = this.valuesArea[index11, col].GetResult();
              }
              else
              {
                int count3 = pivotBaseRowItem.Count;
                if (row <= count3 && this.valuesArea[row, col] != null)
                  obj5 = this.valuesArea[row, col].GetResult();
                else if (this.valuesArea[row - count3 - 1, col] != null)
                  obj5 = this.valuesArea[row - count3 - 1, col].GetResult();
              }
            }
            else
            {
              int index12 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
              string str = (this.PivotBaseColumnItems[index12] as List<object>)[0].ToString();
              stringList.Clear();
              for (int count4 = this.pivotRows.Count; count4 < this.columnCount - 1; ++count4)
              {
                string uniqueText = this.pivotValues[index12, count4].UniqueText;
                if (uniqueText != null && uniqueText.Contains(str))
                  stringList.Add(uniqueText);
              }
              int count5 = stringList.Count;
              int index13 = this.pivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (p => p.FieldName == compInfo.FieldName));
              if (index12 != -1)
              {
                if (index12 == this.pivotColumns.Count - 1)
                {
                  int index14 = index12 != count5 + 1 ? index13 : col;
                  if (this.valuesArea[row, index14] != null)
                    obj5 = this.valuesArea[row, index14].GetResult();
                }
                else if (col < count5 && this.valuesArea[row, col] != null)
                  obj5 = this.valuesArea[row, col].GetResult();
                else if (this.valuesArea[row, col - count5] != null)
                  obj5 = this.valuesArea[row, col - count5].GetResult();
              }
            }
            if (this.pivotValues[index1, index2] != null && obj5 != null && row != this.rowCount - valueRowStart - 1)
            {
              if (compInfo.CalculationType == CalculationType.RunningTotalIn)
              {
                this.pivotValues[index1, index2].Value = Convert.ToDouble(result1) != Convert.ToDouble(obj5) ? (object) (Convert.ToDouble(result1) + Convert.ToDouble(obj5)) : (object) Convert.ToDouble(result1);
                this.pivotValues[index1, index2].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) (double) this.pivotValues[index1, index2].Value);
                break;
              }
              this.pivotValues[index1, index2].Value = Convert.ToDouble(result1) != Convert.ToDouble(obj5) ? (object) ((Convert.ToDouble(result1) + Convert.ToDouble(obj5)) / Convert.ToDouble(obj5) * 100.0) : (object) (Convert.ToDouble(result1) / Convert.ToDouble(obj5) * 100.0);
              this.pivotValues[index1, index2].FormattedText = ((double) this.pivotValues[index1, index2].Value).ToString("0.00", (IFormatProvider) CultureInfo.CurrentUICulture) + "%";
              break;
            }
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) null;
              this.pivotValues[index1, index2].FormattedText = string.Empty;
              break;
            }
            break;
          case CalculationType.RankSmallestToLargest:
          case CalculationType.RankLargestToSmallest:
            if (compInfo.BaseField == null)
              throw new ArgumentNullException("Base Field value should be required.");
            this.FindRowColumnItems(pivotRows, pivotColumns);
            int index15 = this.PivotRows.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
            object obj6 = (object) null;
            if (index15 != -1)
            {
              int num4 = !this.ShowCalculationsAsColumns || this.PivotCalculations.Count <= 1 ? 0 : 1;
              if (index15 != this.pivotRows.Count - 1 || this.pivotRows.Count == 1)
              {
                objectList1.Clear();
                string uniqueText1 = this.pivotValues[index1, index15 + num4] != null ? this.pivotValues[index1, index15 + num4].UniqueText : (string) null;
                if (uniqueText1 != null && !uniqueText1.Contains("Total"))
                {
                  for (int rowIndex = this.pivotColumns.Count + num4; rowIndex < this.rowCount - 1; ++rowIndex)
                  {
                    string uniqueText2 = this.pivotValues[rowIndex, index15 + num4] != null ? this.pivotValues[rowIndex, index15 + num4].UniqueText : (string) null;
                    if (uniqueText2 != null && !uniqueText2.Contains("Total") && !uniqueText2.Contains("\u0083") && this.valuesArea[rowIndex - (this.pivotColumns.Count + num4), col] != null)
                      objectList1.Add(this.valuesArea[rowIndex - (this.pivotColumns.Count + num4), col].GetResult());
                  }
                  objectList1.Sort();
                  if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                    objectList1.Reverse();
                  obj6 = (object) (objectList1.IndexOf(result1) + 1);
                }
              }
              else
              {
                objectList1.Clear();
                string uniqueText3 = this.pivotValues[index1, index15 - 1] != null ? this.pivotValues[index1, index15 - 1].UniqueText : (string) null;
                if (uniqueText3 != null && !uniqueText3.Contains("Total"))
                {
                  for (int rowIndex = this.pivotColumns.Count + num4; rowIndex < this.rowCount - 1; ++rowIndex)
                  {
                    string uniqueText4 = this.pivotValues[rowIndex, index15] != null ? this.pivotValues[rowIndex, index15].UniqueText : (string) null;
                    if (uniqueText4 != null && uniqueText4.Contains(uniqueText3) && this.valuesArea[rowIndex - (this.pivotColumns.Count + num4), col] != null)
                      objectList1.Add(this.valuesArea[rowIndex - (this.pivotColumns.Count + num4), col].GetResult());
                  }
                  objectList1.Sort();
                  if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                    objectList1.Reverse();
                  obj6 = (object) (objectList1.IndexOf(result1) + 1);
                }
              }
            }
            else
            {
              int index16 = this.PivotColumns.FindIndex((Predicate<PivotItem>) (i => i.FieldMappingName == compInfo.BaseField));
              if (index16 != -1)
              {
                List<object> objectList2 = new List<object>();
                int num5 = this.pivotRows.Count + this.pivotCalculations.FindIndex((Predicate<PivotComputationInfo>) (x => x.FieldName == compInfo.FieldName));
                if (index16 != this.pivotColumns.Count - 1 || this.pivotColumns.Count == 1)
                {
                  objectList1.Clear();
                  for (int count6 = this.pivotRows.Count; count6 < this.columnCount - 1; count6 += this.pivotCalculations.Count)
                  {
                    string uniqueText = this.pivotValues[index16, count6] != null ? this.pivotValues[index16, count6].UniqueText : (string) null;
                    if (uniqueText != null && !uniqueText.Contains("Total") && !uniqueText.Contains("\u0083") && this.valuesArea[row, count6 - num5] != null)
                      objectList1.Add(this.valuesArea[row, count6 - num5].GetResult());
                    else if (count6 - num5 < this.valuesArea.GetLength(1) && this.valuesArea[row, count6 - num5] != null)
                      objectList2.Add(this.valuesArea[row, count6 - num5].GetResult());
                  }
                  if (this.pivotValues[index16, index2] != null && this.pivotValues[index16, index2].UniqueText != null && !this.pivotValues[index16, index2].UniqueText.Contains("Total"))
                  {
                    objectList1.Sort();
                    if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                      objectList1.Reverse();
                    obj6 = (object) (objectList1.IndexOf(result1) + 1);
                  }
                  else
                  {
                    objectList2.Sort();
                    if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                      objectList2.Reverse();
                    obj6 = (object) (objectList2.IndexOf(result1) + 1);
                  }
                }
                else
                {
                  objectList1.Clear();
                  string uniqueText5 = this.pivotValues[index16 - 1, index2] != null ? this.pivotValues[index16 - 1, index2].UniqueText : (string) null;
                  if (uniqueText5 != null && !uniqueText5.Contains("Total"))
                  {
                    for (int count7 = this.pivotRows.Count; count7 < this.columnCount - 1; count7 += this.pivotCalculations.Count)
                    {
                      string uniqueText6 = this.pivotValues[index16, count7] != null ? this.pivotValues[index16, count7].UniqueText : (string) null;
                      if (uniqueText6 != null && uniqueText6.Contains(uniqueText5) && this.valuesArea[row, count7 - num5] != null)
                        objectList1.Add(this.valuesArea[row, count7 - num5].GetResult());
                    }
                    objectList1.Sort();
                    if (compInfo.CalculationType == CalculationType.RankLargestToSmallest)
                      objectList1.Reverse();
                    obj6 = (object) (objectList1.IndexOf(result1) + 1);
                  }
                }
              }
            }
            if (this.pivotValues[index1, index2] != null && obj6 != null)
            {
              this.pivotValues[index1, index2].Value = obj6;
              this.pivotValues[index1, index2].FormattedText = obj6.ToString();
              break;
            }
            if (this.pivotValues[index1, index2] != null && obj6 == null && result1 != null)
            {
              this.pivotValues[index1, index2].Value = (object) Convert.ToDouble(result1);
              this.pivotValues[index1, index2].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, result1);
              break;
            }
            if (this.pivotValues[index1, index2] != null)
            {
              this.pivotValues[index1, index2].Value = (object) null;
              this.pivotValues[index1, index2].FormattedText = string.Empty;
              break;
            }
            break;
        }
      }
      if (this.pivotValues[index1, index2] != null)
        this.pivotValues[index1, index2].Format = this.PivotCalculations[col % this.PivotCalculations.Count].Format;
    }
    else
    {
      if (this.valuesArea[row, col] != null || !this.ShowEmptyCells)
      {
        string format = $"{{0:{this.PivotCalculations[col % this.PivotCalculations.Count].Format}}}";
        if (this.pivotValues[index1, index2] != null)
        {
          this.pivotValues[index1, index2].FormattedText = string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) 0.0);
          this.pivotValues[index1, index2].Format = this.PivotCalculations[col % this.PivotCalculations.Count].Format;
        }
      }
      if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && this.pivotValues[this.PivotColumns.Count - 1, index2] != null && this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText != null && !this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText.ToUpper().Contains("TOTAL") && !this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText.Contains(this.PivotColumns[this.PivotColumns.Count - 1].TotalHeader))
        binaryList.AddIfUnique((IComparable) this.pivotValues[this.PivotColumns.Count - 1, index2].FormattedText);
    }
    if (isSummaryRow && this.pivotValues[index1, index2] != null)
    {
      this.pivotValues[index1, index2].CellType = PivotCellType.ValueCell | PivotCellType.TotalCell;
    }
    else
    {
      if (!this.IsSummaryColumn(index2, ref k))
        return;
      for (k = 0; k < (!this.ShowSubTotalsForChildren || binaryList.Count <= 0 || this.PivotColumns.Count <= 2 || index2 + k >= this.columnCount || this.pivotValues[0, index2] == null || this.pivotValues[0, index2].UniqueText == null ? (this.PivotCalculations.Count > 0 ? this.PivotCalculations.Count : 1) : this.PivotCalculations.Count + this.PivotCalculations.Count * binaryList.Count); ++k)
      {
        if (this.pivotValues[index1, index2 + k] == null)
          this.pivotValues[index1, index2 + k] = new PivotCellInfo()
          {
            RowIndex = index1,
            ColumnIndex = index2 + k
          };
        this.pivotValues[index1, index2 + k].CellType = PivotCellType.ValueCell | PivotCellType.TotalCell;
      }
      if (k <= this.PivotCalculations.Count || !this.ShowSubTotalsForChildren || binaryList.Count <= 0)
        return;
      binaryList.Clear();
    }
  }

  private void ProcessTotals3()
  {
    if (this.EditCellsInfo != null && this.EditCellsInfo.Count > 0)
      this.CellEditing();
    int length1 = this.valuesArea.GetLength(0);
    int num1 = this.rowHeaders.GetLength(1) + 1;
    int num2 = this.columnHeaders.GetLength(0) + 1;
    BinaryList binaryList1 = new BinaryList();
    BinaryList binaryList2 = new BinaryList();
    List<string> stringList = new List<string>();
    List<int> intList = new List<int>();
    List<SummaryBase> summaryBaseList1 = new List<SummaryBase>();
    List<SummaryBase> summaryBaseList2 = new List<SummaryBase>();
    int k1 = 0;
    int k2 = 0;
    int num3 = this.PivotRows.Count < 2 ? this.PivotRows.Count + 1 : this.PivotRows.Count;
    if (length1 > 0)
    {
      for (int index1 = 0; index1 < this.valuesArea.GetLength(1); ++index1)
      {
        PivotComputationInfo compInfo = this.PivotCalculations[index1 % this.PivotCalculations.Count];
        int num4 = this.columnHeaders.GetLength(0);
        if (this.ShowSubTotalsForChildren)
        {
          summaryBaseList2.Clear();
          summaryBaseList2.Add(this.PivotCalculations[index1 % this.PivotCalculations.Count].Summary.GetInstance());
        }
        if (!this.IsSummaryColumn(num1 + index1 - 1, ref k2))
        {
          summaryBaseList1.Clear();
          for (int index2 = 0; index2 < num3; ++index2)
            summaryBaseList1.Add(this.PivotCalculations[index1 % this.PivotCalculations.Count].Summary.GetInstance());
          for (int index3 = 0; index3 < this.valuesArea.GetLength(0); ++index3)
          {
            if (compInfo != null && compInfo.CalculationType == CalculationType.Distinct && compInfo.BaseItem != null && this.PivotRows.Any<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName == compInfo.BaseItem || x.FieldHeader == compInfo.BaseItem)))
            {
              int index4 = this.PivotRows.IndexOf(this.PivotRows.Where<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName == compInfo.BaseItem || x.FieldHeader == compInfo.BaseItem)).FirstOrDefault<PivotItem>());
              int index5 = index3 + this.PivotColumns.Count + (this.PivotCalculations.Count > 1 ? 1 : 0);
              if (index5 > -1 && index4 > -1 && this.rowHeaders[index5, index4] != null)
              {
                if (!stringList.Contains(this.rowHeaders[index5, index4].ToString()) && this.valuesArea[index3, index1] != null)
                  stringList.Add(this.rowHeaders[index5, index4].ToString());
                else if (this.valuesArea[index3, index1] != null)
                  intList.Add(index3);
              }
            }
            if (this.ShowSubTotalsForChildren && this.PivotRows.Count > 2 && this.rowHeaders[num2 + index3 - 1, this.PivotRows.Count - 1] != null)
              binaryList1.AddIfUnique(this.rowHeaders[num2 + index3 - 1, this.PivotRows.Count - 1]);
            if (this.IsSummaryRow(num2 + index3 - 1, ref k1))
            {
              if (this.ShowNullAsBlank && summaryBaseList1[k1].GetResult() == null)
              {
                SummaryBase instance = summaryBaseList1[k1].GetInstance();
                instance.ShowNullAsBlank = this.ShowNullAsBlank;
                this.valuesArea[index3, index1] = instance;
              }
              else if (this.ShowSubTotalsForChildren && this.PivotRows.Count > 2 && k1 == 0)
              {
                int index6 = index3 + binaryList1.Count;
                this.valuesArea[index6, index1] = summaryBaseList1[k1];
                summaryBaseList1[k1] = summaryBaseList1[k1].GetInstance();
                binaryList1.Clear();
                for (int index7 = index3; index7 < index6; ++index7)
                {
                  for (int index8 = index3; index8 >= num4; --index8)
                  {
                    if (this.rowHeaders[index8, this.PivotRows.Count - 1] != null && this.rowHeaders[num2 + index3 - 1, this.PivotRows.Count - 1].ToString() == this.rowHeaders[index8, this.PivotRows.Count - 1].ToString() && this.valuesArea[index8 - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 || this.PivotCalculations.Count > 0 && this.showSingleCalculationHeader ? 1 : 0)), index1] != null && this.valuesArea[index8 - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 || this.PivotCalculations.Count > 0 && this.showSingleCalculationHeader ? 1 : 0)), index1] != null)
                    {
                      foreach (SummaryBase summaryBase in summaryBaseList2)
                      {
                        summaryBase.ShowNullAsBlank = this.ShowNullAsBlank;
                        summaryBase.CombineSummary(this.valuesArea[index8 - (this.PivotColumns.Count + (this.PivotCalculations.Count > 1 || this.PivotCalculations.Count > 0 && this.showSingleCalculationHeader ? 1 : 0)), index1]);
                      }
                    }
                  }
                  this.valuesArea[index3, index1] = summaryBaseList2[k1];
                  summaryBaseList2[k1] = summaryBaseList2[k1].GetInstance();
                  ++index3;
                }
                num4 = num2 + index3;
              }
              else
              {
                this.valuesArea[index3, index1] = summaryBaseList1[k1];
                summaryBaseList1[k1] = summaryBaseList1[k1].GetInstance();
                if ((compInfo == null || compInfo.CalculationType != CalculationType.Distinct || compInfo.BaseItem == null || this.PivotRows.Count <= 2 ? (k1 == 0 ? 1 : 0) : (k1 != this.PivotRows.Count - 2 ? 1 : 0)) != 0)
                {
                  stringList.Clear();
                  intList.Clear();
                }
              }
            }
            else if (this.valuesArea[index3, index1] != null && this.valuesArea[index3, index1] != null)
            {
              foreach (SummaryBase summaryBase in summaryBaseList1)
              {
                summaryBase.ShowNullAsBlank = this.ShowNullAsBlank;
                if (this.ApplyFormattedSummary)
                {
                  string format = $"{{0:{this.PivotCalculations[index1 % this.PivotCalculations.Count].Format}}}";
                  SummaryType summaryType = this.PivotCalculations[index1 % this.PivotCalculations.Count].SummaryType;
                  if (summaryType == SummaryType.DoubleTotalSum && double.TryParse(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) ((DoubleTotalSummary) this.valuesArea[index3, index1]).total), out double _))
                    ((DoubleTotalSummary) this.valuesArea[index3, index1]).total = new double?(double.Parse(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) ((DoubleTotalSummary) this.valuesArea[index3, index1]).total)));
                  else if (summaryType == SummaryType.IntTotalSum && int.TryParse(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) ((IntTotalSummary) this.valuesArea[index3, index1]).total), out int _))
                    ((IntTotalSummary) this.valuesArea[index3, index1]).total = new int?(int.Parse(string.Format((IFormatProvider) CultureInfo.CurrentUICulture, format, (object) ((IntTotalSummary) this.valuesArea[index3, index1]).total)));
                }
                if (intList.Contains(index3) && summaryBaseList1.IndexOf(summaryBase) == k1 || !intList.Contains(index3))
                  summaryBase.CombineSummary(this.valuesArea[index3, index1]);
              }
            }
          }
          this.valuesArea[this.valuesArea.GetLength(0) - 1, index1] = summaryBaseList1[summaryBaseList1.Count - 1];
        }
      }
    }
    int count = this.PivotCalculations.Count;
    stringList.Clear();
    intList.Clear();
    int num5 = this.PivotColumns.Count < 2 ? this.PivotColumns.Count + 1 : this.PivotColumns.Count;
    for (int index9 = 0; index9 < this.valuesArea.GetLength(0); ++index9)
    {
      int length2 = this.rowHeaders.GetLength(1);
      int num6 = length2;
      summaryBaseList1.Clear();
      for (int index10 = 0; index10 < num5; ++index10)
      {
        for (int index11 = 0; index11 < this.PivotCalculations.Count; ++index11)
          summaryBaseList1.Add(this.PivotCalculations[index11].Summary.GetInstance());
        summaryBaseList1[index10].ShowNullAsBlank = this.ShowNullAsBlank;
      }
      for (int index12 = 0; index12 < this.valuesArea.GetLength(1) - this.grandColumnIndex; ++index12)
      {
        int num7 = 0;
        int index13 = 0;
        PivotComputationInfo compInfo = this.PivotCalculations[index12 % this.PivotCalculations.Count];
        if (compInfo.CalculationType == CalculationType.Distinct && compInfo.BaseItem != null && this.PivotColumns.Any<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName == compInfo.BaseItem || x.FieldHeader == compInfo.BaseItem)))
        {
          int index14 = this.PivotColumns.IndexOf(this.PivotColumns.Where<PivotItem>((System.Func<PivotItem, bool>) (x => x.FieldMappingName == compInfo.BaseItem || x.FieldHeader == compInfo.BaseItem)).FirstOrDefault<PivotItem>());
          if (this.columnHeaders[index14, index12 + this.PivotRows.Count] != null)
          {
            string str = this.PivotCalculations.Count <= 1 || !this.showCalculationsAsColumns || this.columnHeaders[this.PivotColumns.Count, index12 + this.PivotRows.Count] == null ? this.columnHeaders[index14, index12 + this.PivotRows.Count].ToString() : $"{this.columnHeaders[index14, index12 + this.PivotRows.Count].ToString()}.{this.columnHeaders[this.PivotColumns.Count, index12 + this.PivotRows.Count].ToString()}";
            if (!stringList.Contains(str) && this.valuesArea[index9, index12] != null && this.valuesArea[index9, index12] != null)
              stringList.Add(str);
            else if (this.valuesArea[index9, index12] != null && this.valuesArea[index9, index12] != null)
              intList.Add(index12);
          }
        }
        if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && this.columnHeaders[this.PivotColumns.Count - 1, num1 + index12 - 1] != null && !this.columnHeaders[this.PivotColumns.Count - 1, num1 + index12 - 1].ToString().Contains("Total"))
          binaryList2.AddIfUnique(this.columnHeaders[this.PivotColumns.Count - 1, num1 + index12 - 1]);
        if (this.IsSummaryColumn(num1 + index12 - 1, ref k2))
        {
          for (int index15 = 0; index15 < this.PivotCalculations.Count; ++index15)
          {
            compInfo = this.PivotCalculations[index12 % this.PivotCalculations.Count];
            if (this.ShowNullAsBlank && summaryBaseList1[k2 * this.PivotCalculations.Count + index15].GetResult() == null)
            {
              SummaryBase instance = summaryBaseList1[k2 * this.PivotCalculations.Count + index15].GetInstance();
              instance.ShowNullAsBlank = this.ShowNullAsBlank;
              this.valuesArea[index9, index12++] = instance;
            }
            else if (this.ShowSubTotalsForChildren && this.PivotColumns.Count > 2 && k2 == 0)
            {
              if (num7 == 0)
                num7 = index12;
              int num8 = index12 + binaryList2.Count * this.PivotCalculations.Count;
              SummaryBase[,] valuesArea = this.valuesArea;
              int index16 = index9;
              int index17 = num8;
              index12 = index17 + 1;
              SummaryBase summaryBase1 = summaryBaseList1[k2 * this.PivotCalculations.Count + index15];
              valuesArea[index16, index17] = summaryBase1;
              summaryBaseList1[k2 * this.PivotCalculations.Count + index15] = summaryBaseList1[k2 * this.PivotCalculations.Count + index15].GetInstance();
              binaryList2.Clear();
              if (index13 == 0)
              {
                for (index13 = num7; index13 < index12 - 1; ++index13)
                {
                  summaryBaseList2.Clear();
                  summaryBaseList2.Add(this.PivotCalculations[index13 % this.PivotCalculations.Count].Summary.GetInstance());
                  for (int index18 = index13; index18 >= num6 + this.PivotCalculations.Count + 1; index18 -= count)
                  {
                    if (index18 - (this.PivotCalculations.Count * 2 - this.PivotRows.Count) > -1 && this.columnHeaders[this.PivotColumns.Count - 1, index18 - (this.PivotCalculations.Count * 2 - this.PivotRows.Count)] != null && this.columnHeaders[this.PivotColumns.Count - 1, num1 + index13 - 1].ToString() == this.columnHeaders[this.PivotColumns.Count - 1, index18 - (this.PivotCalculations.Count * 2 - this.PivotRows.Count)].ToString() && this.valuesArea[index9, index18 - (this.PivotCalculations.Count * 2 - this.PivotRows.Count) - length2] != null)
                    {
                      foreach (SummaryBase summaryBase2 in summaryBaseList2)
                      {
                        summaryBase2.ShowNullAsBlank = this.ShowNullAsBlank;
                        summaryBase2.CombineSummary(this.valuesArea[index9, index18 - (this.PivotCalculations.Count * 2 - this.PivotRows.Count) - length2]);
                      }
                    }
                  }
                  this.valuesArea[index9, index13] = summaryBaseList2[k2];
                  summaryBaseList2[k2] = summaryBaseList2[k2].GetInstance();
                }
                num6 = num1 + index12;
              }
            }
            else
            {
              this.valuesArea[index9, index12++] = summaryBaseList1[k2 * this.PivotCalculations.Count + index15];
              summaryBaseList1[k2 * this.PivotCalculations.Count + index15] = summaryBaseList1[k2 * this.PivotCalculations.Count + index15].GetInstance();
              if (compInfo != null && compInfo.CalculationType == CalculationType.Distinct && compInfo.BaseItem != null && (this.PivotColumns.Count > 2 ? (k2 != this.PivotColumns.Count - 2 ? 1 : 0) : (k2 == 0 ? 1 : 0)) != 0)
              {
                stringList.Clear();
                intList.Clear();
              }
            }
          }
          --index12;
        }
        else if (this.valuesArea[index9, index12] != null && this.valuesArea[index9, index12] != null)
        {
          for (int index19 = index12 % this.PivotCalculations.Count; index19 < summaryBaseList1.Count; index19 += this.PivotCalculations.Count)
          {
            if (intList.Contains(index12) && summaryBaseList1.IndexOf(summaryBaseList1[index19]) == k2 * this.PivotCalculations.Count + index12 % this.PivotCalculations.Count || !intList.Contains(index12))
              summaryBaseList1[index19].CombineSummary(this.valuesArea[index9, index12]);
          }
        }
      }
      for (int index20 = 0; index20 < this.PivotCalculations.Count; ++index20)
      {
        if (this.valuesArea.GetLength(1) != 0)
          this.valuesArea[index9, this.valuesArea.GetLength(1) - this.PivotCalculations.Count + index20] = summaryBaseList1[summaryBaseList1.Count - this.PivotCalculations.Count + index20];
      }
    }
  }

  private int GetParentRowIndex(int currentRow, int column, bool isSummaryRow)
  {
    if (column == this.PivotRows.Count - 1)
      return currentRow;
    for (int index = currentRow + (this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0)); index < this.rowHeaders.GetLength(0) && (this.rowHeaders[index, column] != null || !isSummaryRow); ++index)
    {
      if (this.rowHeaders[index, column] != null && (this.rowHeaders[index, column].ToString().IndexOf(this.delimiter) > -1 || this.rowHeaders[index, column].ToString().IndexOf(this.GrandString + this.delimiter) > -1))
        return index - (this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0));
    }
    return -1;
  }

  private int GetParentColumnIndex(int row, int currentColumn, bool isSummaryColumn)
  {
    if (row == this.PivotColumns.Count - 1)
      return currentColumn;
    for (int index = currentColumn + this.PivotRows.Count; index < this.columnHeaders.GetLength(1); ++index)
    {
      if (this.columnHeaders[row, index] == null && isSummaryColumn)
        return index - this.PivotRows.Count;
      if (this.columnHeaders[row, index] != null && (this.columnHeaders[row, index].ToString().IndexOf(this.delimiter) > -1 || this.columnHeaders[row, index].ToString().IndexOf(this.GrandString + this.delimiter) > -1))
        return index - this.PivotRows.Count + currentColumn % this.PivotCalculations.Count;
    }
    return -1;
  }

  private int GetImmediateNextParentRowIndex(int currentRow, int column)
  {
    for (int nextParentRowIndex = currentRow + 1; nextParentRowIndex < this.rowHeaders.GetLength(0); ++nextParentRowIndex)
    {
      if (this.rowHeaders[nextParentRowIndex, column] != null && this.rowHeaders[nextParentRowIndex, column].ToString().IndexOf(this.delimiter) > -1)
        return nextParentRowIndex;
    }
    return -1;
  }

  private int GetImmediateNextParentColumnIndex(int row, int currentColumn)
  {
    for (int parentColumnIndex = currentColumn + 1; parentColumnIndex < this.columnHeaders.GetLength(1); ++parentColumnIndex)
    {
      if (this.rowHeaders[row, parentColumnIndex] != null && this.rowHeaders[row, parentColumnIndex].ToString().IndexOf(this.delimiter) > -1)
        return parentColumnIndex;
    }
    return -1;
  }

  private int[] GetNextParentRowIndex(int currentRowIndex, int parentColumnIndex)
  {
    int[] nextParentRowIndex = new int[2];
    for (int index1 = currentRowIndex + 1; index1 < this.rowHeaders.GetLength(0); ++index1)
    {
      if (parentColumnIndex > 0)
      {
        for (int index2 = parentColumnIndex - 1; index2 >= 0; --index2)
        {
          if (this.rowHeaders[index1, index2] != null && this.rowHeaders[index1, index2].ToString().IndexOf(this.delimiter) > -1)
          {
            nextParentRowIndex[0] = index1 - (this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0));
            nextParentRowIndex[1] = index2;
            return nextParentRowIndex;
          }
        }
      }
      else if (this.rowHeaders[index1, parentColumnIndex] != null && this.rowHeaders[index1, parentColumnIndex].ToString().IndexOf(this.GrandString + this.delimiter) > -1)
      {
        nextParentRowIndex[0] = index1 - (this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0));
        nextParentRowIndex[1] = parentColumnIndex;
        return nextParentRowIndex;
      }
    }
    nextParentRowIndex[0] = currentRowIndex - (this.PivotColumns.Count + (this.IsCalculationHeaderVisible ? 1 : 0));
    return nextParentRowIndex;
  }

  private int[] GetNextParentColumnIndex(int parentRowIndex, int currentColumnIndex)
  {
    int[] parentColumnIndex = new int[2];
    for (int index1 = currentColumnIndex + 1; index1 < this.columnHeaders.GetLength(1); ++index1)
    {
      if (parentRowIndex > 0)
      {
        for (int index2 = parentRowIndex - 1; index2 >= 0; --index2)
        {
          if (this.columnHeaders[index2, index1] != null && this.columnHeaders[index2, index1].ToString().IndexOf(this.delimiter) > -1)
          {
            parentColumnIndex[1] = index1 - (this.PivotRows.Count - (currentColumnIndex - this.PivotRows.Count) % this.PivotCalculations.Count);
            parentColumnIndex[0] = index2;
            return parentColumnIndex;
          }
        }
      }
      else if (this.columnHeaders[parentRowIndex, index1] != null && this.columnHeaders[parentRowIndex, index1].ToString().IndexOf(this.GrandString + this.delimiter) > -1)
      {
        parentColumnIndex[1] = index1 - (this.PivotRows.Count - (currentColumnIndex - this.PivotRows.Count) % this.PivotCalculations.Count);
        parentColumnIndex[0] = parentRowIndex;
        return parentColumnIndex;
      }
    }
    parentColumnIndex[1] = currentColumnIndex - this.PivotRows.Count;
    return parentColumnIndex;
  }

  private bool IsSummaryColumnWhileOnDemand(int colIndex)
  {
    bool flag = false;
    for (int rowIndex = 0; rowIndex < this.columnHeaders.GetLength(0); ++rowIndex)
    {
      if (rowIndex < this.rowCount && colIndex < this.ColumnCount && this.PivotValues != null && this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].Key != null && (this.PivotValues[rowIndex, colIndex].Key.IndexOf(this.delimiter) > -1 || this.pivotValues[rowIndex, colIndex].Key.IndexOf(this.grandString + this.delimiter) > -1))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private bool IsRowSummaryWhileOnDemand(int rowIndex)
  {
    bool flag = false;
    for (int colIndex = 0; colIndex < this.rowHeaders.GetLength(1); ++colIndex)
    {
      if (rowIndex < this.rowCount && colIndex < this.ColumnCount && this.PivotValues != null && this.PivotValues[rowIndex, colIndex] != null && this.PivotValues[rowIndex, colIndex].Key != null && (this.PivotValues[rowIndex, colIndex].Key.IndexOf(this.delimiter) > -1 || this.pivotValues[rowIndex, colIndex].Key.IndexOf(this.grandString + this.delimiter) > -1))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private bool IsGrandTotalCell(int rowIndex, int colIndex)
  {
    bool flag = false;
    if (colIndex < this.columnHeaders.GetLength(1))
    {
      for (int index = 0; index < this.columnHeaders.GetLength(0); ++index)
      {
        if (this.columnHeaderUniqueValues[index, colIndex] != null && this.columnHeaderUniqueValues[index, colIndex].ToString().IndexOf(this.grandString + this.delimiter) > -1)
        {
          flag = true;
          break;
        }
      }
    }
    if (!flag && rowIndex < this.rowHeaders.GetLength(0))
    {
      for (int index = 0; index < this.rowHeaders.GetLength(1); ++index)
      {
        if (this.rowHeaders[rowIndex, index] != null && this.rowHeaders[rowIndex, index].ToString().IndexOf(this.grandString + this.delimiter) > -1)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private List<IComparable> GetKeyAt(int i, int j)
  {
    return !this.EnableOnDemandCalculations && (this.sortKeys == null || this.sortKeys != null && this.sortKeys.Count == 0) ? this.GetKeyAt(i, j, false) : this.GetKeyAt(i, j, true);
  }

  private List<IComparable> GetKeyAt(int i, int j, bool isForGetRawItem)
  {
    List<IComparable> keyAt = new List<IComparable>();
    if (!isForGetRawItem)
    {
      int num1 = !this.isForRawItem || this.ShowCalculationsAsColumns ? this.PivotRows.Count : this.PivotColumns.Count;
      int num2 = !this.isForRawItem || this.ShowCalculationsAsColumns ? this.PivotColumns.Count : this.PivotRows.Count;
      if (i < this.rowHeaders.GetLength(0) && i > -1)
      {
        for (int index = 0; index < num1; ++index)
        {
          if (this.rowHeaders[i, index] != null)
          {
            keyAt.Add((IComparable) this.rowHeaders[i, index].ToString());
          }
          else
          {
            string headerUniqueValue = this.rowHeaderUniqueValues[i, index];
            if (headerUniqueValue != null)
            {
              string[] strArray = headerUniqueValue.Split('.');
              if (!string.IsNullOrEmpty(strArray[strArray.Length - 1]))
                keyAt.Add((IComparable) strArray[strArray.Length - 1]);
              else
                keyAt.Add((IComparable) null);
            }
            else
              keyAt.Add((IComparable) null);
          }
        }
      }
      if (j < this.columnHeaders.GetLength(1) && j > -1)
      {
        for (int index = 0; index < num2; ++index)
        {
          if (this.columnHeaders[index, j] != null)
            keyAt.Add((IComparable) this.columnHeaders[index, j].ToString());
          else if (j < this.columnHeaders.GetLength(1))
          {
            string headerUniqueValue = this.columnHeaderUniqueValues[index, j];
            if (headerUniqueValue != null)
            {
              string[] strArray = headerUniqueValue.Split('.');
              if (!string.IsNullOrEmpty(strArray[strArray.Length - 1]))
                keyAt.Add((IComparable) strArray[strArray.Length - 1]);
              else
                keyAt.Add((IComparable) null);
            }
            else
              keyAt.Add((IComparable) null);
          }
        }
      }
      else
      {
        if (this.PivotRows.Count == 0 || this.PivotCalculations.Count == 1)
          --j;
        for (int index = 0; index < this.PivotColumns.Count; ++index)
        {
          if (j < this.columnHeaders.GetLength(1) && this.columnHeaders[index, j] != null)
            keyAt.Add(this.columnHeaders[index, j]);
          --j;
        }
      }
    }
    else if (!this.ShowCalculationsAsColumns)
    {
      if (j < this.columnHeaders.GetLength(1) && j > -1)
      {
        for (int rowIndex = 0; rowIndex < this.PivotColumns.Count; ++rowIndex)
        {
          if (this[rowIndex, j] != null && this[rowIndex, j].Key != null)
            keyAt.Add((IComparable) this[rowIndex, j].Key.ToString());
          else
            keyAt.Add((IComparable) null);
        }
      }
      else
      {
        if (this.PivotRows.Count == 0 || this.PivotCalculations.Count == 1)
          --j;
        for (int rowIndex = 0; rowIndex < this.PivotColumns.Count; ++rowIndex)
        {
          if (this[rowIndex, j] != null)
            keyAt.Add((IComparable) this[rowIndex, j].Key);
        }
      }
      for (int columnIndex1 = 0; columnIndex1 < this.PivotRows.Count; ++columnIndex1)
      {
        if (this[i, columnIndex1] != null && this[i, columnIndex1].Key != null)
          keyAt.Add((IComparable) this[i, columnIndex1].Key.ToString());
        else
          keyAt.Add((IComparable) null);
      }
    }
    else
    {
      if (i < this.rowHeaders.GetLength(0) && i > -1)
      {
        for (int columnIndex1 = 0; columnIndex1 < this.PivotRows.Count; ++columnIndex1)
        {
          if (this[i, columnIndex1] != null && this[i, columnIndex1].Key != null)
            keyAt.Add((IComparable) this[i, columnIndex1].Key.ToString());
          else
            keyAt.Add((IComparable) null);
        }
      }
      if (j < this.columnHeaders.GetLength(1) && j > -1)
      {
        for (int rowIndex = 0; rowIndex < this.PivotColumns.Count; ++rowIndex)
        {
          if (this[rowIndex, j] != null && this[rowIndex, j].Key != null)
            keyAt.Add((IComparable) this[rowIndex, j].Key.ToString());
          else
            keyAt.Add((IComparable) null);
        }
      }
      else
      {
        for (int rowIndex = 0; rowIndex < this.PivotColumns.Count; ++rowIndex)
        {
          if (this[rowIndex, j] != null)
            keyAt.Add((IComparable) this[rowIndex, j].Key);
        }
      }
    }
    return keyAt;
  }

  private int GetValueStartColumn()
  {
    return !this.ShowCalculationsAsColumns ? (this.PivotCalculations.Count <= 1 && (this.PivotCalculations.Count <= 0 || !this.ShowSingleCalculationHeader) ? 0 : 1) : (this.PivotRows.Count <= 0 ? this.PivotRows.Count + 1 : this.PivotRows.Count);
  }

  private int GetValueStartRow()
  {
    return this.PivotColumns.Count <= 0 ? (this.PivotCalculations.Count <= 1 && (this.PivotCalculations.Count <= 0 || !this.ShowSingleCalculationHeader) ? 0 : 1) : (this.PivotCalculations.Count <= 1 && (this.PivotCalculations.Count <= 0 || !this.ShowSingleCalculationHeader) || !this.ShowCalculationsAsColumns ? this.PivotColumns.Count : this.PivotColumns.Count + 1);
  }

  private void DoUniqueValuesCount(List<PivotItem> list, BinaryList bList, int insertCount)
  {
    if (list.Count <= 1)
      return;
    BinaryList binaryList = new BinaryList();
    int num = -1;
    for (int index1 = list.Count - 2; index1 >= 0; --index1)
    {
      ++num;
      int count = bList.Count;
      if (count > 0)
      {
        IComparable comparable = ((KeysCalculationValues) bList[0]).Keys[num];
        int prevCount = 0;
        for (int index2 = 1; index2 < count; ++index2)
        {
          if (((KeysCalculationValues) bList[index2]).Keys != null)
          {
            IComparable key = ((KeysCalculationValues) bList[index2]).Keys[num];
            if (key != null)
            {
              if (num == 0 && this.ShowSubTotalsForChildren)
                binaryList.AddIfUnique(((KeysCalculationValues) bList[index2 - 1]).Keys[list.Count - 1]);
              if (key.CompareTo((object) comparable) != 0 || num > 0 && this.AnyPreviousLevelNotEqual(bList, index2, prevCount, num))
              {
                if (!this.ShowSubTotalsForChildren)
                {
                  for (int index3 = 0; index3 < insertCount; ++index3)
                    bList.Insert(index2, (IComparable) new KeysCalculationValues());
                  index2 += insertCount;
                  count += insertCount;
                }
                else if (list.Count > 2 && num == 0)
                {
                  for (int index4 = 0; index4 <= binaryList.Count; ++index4)
                    bList.Insert(index2, (IComparable) new KeysCalculationValues());
                  index2 += binaryList.Count + 1;
                  count += binaryList.Count + 1;
                  binaryList.Clear();
                }
                else
                {
                  for (int index5 = 0; index5 < insertCount; ++index5)
                    bList.Insert(index2, (IComparable) new KeysCalculationValues());
                  index2 += insertCount;
                  count += insertCount;
                }
                comparable = key;
                prevCount = index2;
              }
            }
            else if (comparable != null)
            {
              for (int index6 = 0; index6 < insertCount; ++index6)
                bList.Insert(index2, (IComparable) new KeysCalculationValues());
              index2 += insertCount;
              count += insertCount;
              comparable = key;
              prevCount = index2;
            }
          }
        }
        if (num == 0 && this.ShowSubTotalsForChildren)
          binaryList.AddIfUnique(((KeysCalculationValues) bList[count - 1]).Keys[list.Count - 1]);
      }
    }
    for (int index = 0; index < num + (!this.ShowSubTotalsForChildren || list.Count <= 2 ? 1 : binaryList.Count + 1); ++index)
      bList.Add((IComparable) new KeysCalculationValues());
  }

  private bool AnyPreviousLevelNotEqual(BinaryList bList, int i, int prevCount, int level)
  {
    if (this.PivotColumns == null || this.PivotRows == null)
      return false;
    for (int index = level - 1; index >= 0; --index)
    {
      IComparable key1 = ((KeysCalculationValues) bList[i]).Keys[index];
      IComparable key2 = ((KeysCalculationValues) bList[prevCount]).Keys[index];
      if (key1 != null && key1.CompareTo((object) key2) != 0)
        return true;
    }
    return false;
  }

  private IComparer[] GetComparers(List<PivotItem> pivotItems)
  {
    int count = pivotItems.Count;
    IComparer[] comparers = new IComparer[count];
    for (int index = 0; index < count; ++index)
    {
      comparers[index] = pivotItems[index].Comparer;
      bool flag = !string.IsNullOrEmpty(pivotItems[index].Format) && (pivotItems[index].Format.ToUpper() == "P" || pivotItems[index].Format.ToUpper() == "E");
      if (comparers[index] == null && !flag && this.ItemProperties != null && this.ItemProperties[pivotItems[index].FieldMappingName] != null)
      {
        comparers[index] = this.AddComparers(this.ItemProperties[pivotItems[index].FieldMappingName].PropertyType);
        pivotItems[index].Comparer = comparers[index];
      }
      else if (pivotItems[index].Comparer == null && flag)
        pivotItems[index].Comparer = comparers[index] = (IComparer) new DoubleComparer();
    }
    return comparers;
  }

  private PropertyDescriptor[] ProcessList(List<PivotItem> pivotItems)
  {
    int count = pivotItems.Count;
    PropertyDescriptor[] propertyDescriptorArray = new PropertyDescriptor[count];
    for (int index = 0; index < count; ++index)
    {
      if (this.ItemProperties != null)
        propertyDescriptorArray[index] = this.ItemProperties[pivotItems[index].FieldMappingName];
      if (propertyDescriptorArray[index] == null)
        propertyDescriptorArray[index] = this.GetComplexPropertyDescriptor(this.itemProperties, pivotItems[index].FieldMappingName);
    }
    return propertyDescriptorArray;
  }

  private PropertyDescriptor[] GetCalcValuesPDs()
  {
    PropertyDescriptor[] calcValuesPds = new PropertyDescriptor[this.PivotCalculations.Count];
    for (int index = 0; index < this.PivotCalculations.Count; ++index)
    {
      if (this.ItemProperties != null)
        calcValuesPds[index] = this.ItemProperties[this.PivotCalculations[index].FieldName];
      if (calcValuesPds[index] == null)
        calcValuesPds[index] = this.GetComplexPropertyDescriptor(this.itemProperties, this.PivotCalculations[index].FieldName);
    }
    return calcValuesPds;
  }

  private PropertyDescriptor GetComplexPropertyDescriptor(
    PropertyDescriptorCollection pdc,
    string columnName)
  {
    if (this.ItemProperties == null)
      return (PropertyDescriptor) null;
    PropertyDescriptorCollection descriptorCollection = (PropertyDescriptorCollection) null;
    if (columnName == null || pdc.Count == 0)
      return (PropertyDescriptor) null;
    if (((IEnumerable<string>) columnName.Split('.')).Count<string>() > 0)
    {
      int index = 0;
      while (true)
      {
        if (index < ((IEnumerable<string>) columnName.Split('.')).Count<string>())
        {
          if (index != ((IEnumerable<string>) columnName.Split('.')).Count<string>() - 1 || descriptorCollection == null)
          {
            if (pdc.Find(columnName.Split('.')[index], false) != null)
              descriptorCollection = TypeDescriptor.GetProperties(pdc.Find(columnName.Split('.')[index], false).PropertyType);
            ++index;
          }
          else
            break;
        }
        else
          goto label_12;
      }
      return descriptorCollection.Find(columnName.Split('.')[index], false);
    }
label_12:
    return (PropertyDescriptor) null;
  }

  private void OnPropertyChanged<R>(Expression<System.Func<FilterItemElement, R>> expr)
  {
    this.OnPropertyChanged(((MemberExpression) expr.Body).Member.Name);
  }

  private void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  private void AdjustSortKeysForMoving(int from, int to)
  {
    if (this.sortColIndexes == null || this.sortColIndexes.Count <= 0)
      return;
    int index1 = this.sortColIndexes.IndexOf(from);
    if (from < to)
    {
      for (int index2 = 0; index2 < this.sortColIndexes.Count; ++index2)
      {
        if (this.sortColIndexes[index2] >= from && this.sortColIndexes[index2] <= to)
        {
          List<int> sortColIndexes;
          int index3;
          (sortColIndexes = this.sortColIndexes)[index3 = index2] = sortColIndexes[index3] - 1;
        }
      }
    }
    else if (from > to)
    {
      for (int index4 = 0; index4 < this.sortColIndexes.Count; ++index4)
      {
        if (this.sortColIndexes[index4] >= to && this.sortColIndexes[index4] <= from)
        {
          List<int> sortColIndexes;
          int index5;
          (sortColIndexes = this.sortColIndexes)[index5 = index4] = sortColIndexes[index5] + 1;
        }
      }
    }
    if (index1 <= -1)
      return;
    this.sortColIndexes[index1] = to;
  }

  private ListSortDirection GetCalcSortDirection(ListSortDirection calcSortDirection)
  {
    return this.isColumnSorting ? (calcSortDirection != ListSortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending) : (calcSortDirection != ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending);
  }

  private int SwapPivotColumns(
    int colHeaderIndex,
    Dictionary<int, string> dictionColumnHeaders,
    Dictionary<int, List<PivotCellInfo>> tempDictionary,
    List<string> uniqueHeader,
    int col)
  {
    using (List<string>.Enumerator enumerator = uniqueHeader.GetEnumerator())
    {
label_15:
      while (enumerator.MoveNext())
      {
        string headerItem = enumerator.Current;
        int startKey = dictionColumnHeaders.Where<KeyValuePair<int, string>>((System.Func<KeyValuePair<int, string>, bool>) (pair => pair.Value == headerItem)).Select<KeyValuePair<int, string>, int>((System.Func<KeyValuePair<int, string>, int>) (pair => pair.Key)).FirstOrDefault<int>();
        if (headerItem == null)
          col -= this.PivotCalculations.Count;
        while (true)
        {
          if (startKey <= dictionColumnHeaders.ElementAt<KeyValuePair<int, string>>(dictionColumnHeaders.Count - 1).Key && dictionColumnHeaders[startKey] != null && dictionColumnHeaders[startKey].ToString().Contains(headerItem))
          {
            int index = 0;
            List<PivotCellInfo> pivotCellInfoList = new List<PivotCellInfo>();
            for (int rowIndex = colHeaderIndex; rowIndex < this.RowCount - this.PivotRows.Count; ++rowIndex)
            {
              if (this.PivotValues[rowIndex, col] != null)
                pivotCellInfoList.Add(this.PivotValues[rowIndex, col]);
              if (!tempDictionary.Keys.Contains<int>(startKey))
                this.PivotValues[rowIndex, col] = this.PivotValues[rowIndex, startKey];
              else if (tempDictionary.Any<KeyValuePair<int, List<PivotCellInfo>>>((System.Func<KeyValuePair<int, List<PivotCellInfo>>, bool>) (x => x.Key == startKey)))
              {
                this.PivotValues[rowIndex, col] = tempDictionary.Single<KeyValuePair<int, List<PivotCellInfo>>>((System.Func<KeyValuePair<int, List<PivotCellInfo>>, bool>) (x => x.Key == startKey)).Value[index];
                ++index;
              }
            }
            tempDictionary.Add(col, pivotCellInfoList);
            ++col;
            ++startKey;
          }
          else
            goto label_15;
        }
      }
    }
    return col;
  }

  private void SortRowPivotColumnAsValue(int colIndex)
  {
    List<PivotCellInfo> pivotCellInfoList = new List<PivotCellInfo>();
    int rowIndex = 1;
    while (rowIndex < this.RowCount && (this[rowIndex, colIndex] == null || (this[rowIndex, colIndex].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0))
      ++rowIndex;
label_8:
    while (rowIndex < this.RowCount)
    {
      PivotCellInfo pivotCellInfo = this[rowIndex, colIndex];
      pivotCellInfoList.Add(pivotCellInfo);
      rowIndex += pivotCellInfo.CellRange.Bottom - pivotCellInfo.CellRange.Top + 2;
      bool flag = true;
      while (true)
      {
        if (rowIndex < this.RowCount && (this[rowIndex, colIndex] == null || (this[rowIndex, colIndex].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0))
        {
          if (flag)
            flag = false;
          ++rowIndex;
        }
        else
          goto label_8;
      }
    }
    int capacity = this.RowCount - 1;
    List<PivotEngine.SortKeys> sortKeysList = new List<PivotEngine.SortKeys>(capacity);
    if (this.sortKeys == null)
    {
      for (int index = 0; index < capacity; ++index)
        sortKeysList.Add(new PivotEngine.SortKeys()
        {
          Index = index
        });
    }
    else
      sortKeysList = this.sortKeys;
    this.sortKeys = (List<PivotEngine.SortKeys>) null;
    this.sortKeys = sortKeysList;
  }

  public class CalcSortComparer : IComparer<PivotEngine.SortKeys>
  {
    private List<ListSortDirection> dirs = new List<ListSortDirection>();

    public CalcSortComparer(List<ListSortDirection> sortDirs) => this.dirs = sortDirs;

    public int Compare(PivotEngine.SortKeys x, PivotEngine.SortKeys y)
    {
      int num = 0;
      int count = this.dirs.Count;
      for (int index = 0; index < count; ++index)
      {
        num = x.Keys[index] != null || y.Keys[index] != null ? (x.Keys[index] != null ? (y.Keys[index] != null ? (!(x.Keys[index].GetType() != y.Keys[index].GetType()) ? x.Keys[index].CompareTo((object) y.Keys[index]) : (x.Keys[index] is string ? -1 : 1)) : 1) : -1) : 0;
        if (num != 0)
        {
          if (this.dirs[index] == ListSortDirection.Descending)
          {
            num = -num;
            break;
          }
          break;
        }
      }
      return num;
    }
  }

  public class SortKeys
  {
    public int Index { get; set; }

    public IComparable[] Keys { get; set; }

    public int BlockSize { get; set; }
  }

  private class ReverseOrderComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      if (x == null && y == null)
        return 0;
      if (y == null)
        return 1;
      return x == null ? -1 : -x.ToString().CompareTo(y.ToString());
    }
  }

  private class SortWithDirComparer : IComparer
  {
    public ListSortDirection dir;
    private IComparer comparer;

    public SortWithDirComparer(IComparer comparer, ListSortDirection dir)
    {
      this.comparer = comparer;
      this.dir = dir;
    }

    public int Compare(object x, object y)
    {
      int num = this.comparer.Compare(x, y);
      if (this.dir == ListSortDirection.Descending)
        num = -num;
      return num;
    }
  }
}
