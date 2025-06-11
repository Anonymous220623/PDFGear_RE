// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.Grouping.WorksheetGroup
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections.Grouping;

internal class WorksheetGroup : 
  CollectionBaseEx<IWorksheet>,
  IWorksheetGroup,
  IWorksheet,
  ITabSheet,
  IParentApplication,
  ICloneParent
{
  private WorkbookImpl m_book;
  private PageSetupGroup m_pageSetup;
  private IRange m_usedRange;
  private IMigrantRange m_migrantRange;
  private OfficeSheetView m_view;
  internal int unknown_formula_name = 9;

  public event RangeImpl.CellValueChangedEventHandler CellValueChanged;

  public event MissingFunctionEventHandler MissingFunction;

  public WorksheetGroup(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.Inserted += new CollectionBaseEx<IWorksheet>.CollectionChange(this.WorksheetGroup_Inserted);
    this.Removing += new CollectionBaseEx<IWorksheet>.CollectionChange(this.WorksheetGroup_Removing);
    this.Clearing += new CollectionBaseEx<IWorksheet>.CollectionClear(this.WorksheetGroup_Clearing);
    IWorksheets worksheets = this.m_book.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
    {
      IWorksheet worksheet = worksheets[Index];
      if (worksheet.IsSelected)
        this.Add(worksheet);
    }
  }

  private void FindParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent workbook");
  }

  public override object Clone(object parent)
  {
    WorksheetGroup worksheetGroup = new WorksheetGroup(this.Application, parent);
    IList<IWorksheet> innerList1 = (IList<IWorksheet>) this.InnerList;
    IList<IWorksheet> innerList2 = (IList<IWorksheet>) worksheetGroup.InnerList;
    WorkbookObjectsCollection objects = worksheetGroup.m_book.Objects;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      int realIndex = ((WorksheetBaseImpl) innerList1[index]).RealIndex;
      innerList2.Add(objects[realIndex] as IWorksheet);
    }
    return (object) worksheetGroup;
  }

  public int Add(ITabSheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.Workbook != this.m_book)
      throw new ArgumentOutOfRangeException(nameof (sheet), "Worksheets from different workbooks can't be grouped.");
    if (sheet.IsSelected && !this.m_book.IsWorkbookOpening)
      return -1;
    if (((WorksheetBaseImpl) sheet).WindowTwo.IsPaged)
      this.m_book.SetActiveWorksheet(sheet as WorksheetBaseImpl);
    int num;
    if (sheet is IWorksheet)
    {
      this.Add(sheet as IWorksheet);
      num = this.Count - 1;
    }
    else
      num = -1;
    return num;
  }

  public void Remove(ITabSheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (this.Count <= 1)
      return;
    this.Remove(sheet as IWorksheet);
    WorksheetBaseImpl sheet1 = this.List[0] as WorksheetBaseImpl;
    this.m_book.SetActiveWorksheet(sheet1);
    this.AppImplementation.SetActiveWorksheet(sheet1);
  }

  public void Select(ITabSheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    this.Clear();
    this.Add(sheet);
  }

  private void CreateMigrantRange()
  {
    this.m_migrantRange = (IMigrantRange) new MigrantRangeGroup(this.Application, (object) this);
  }

  public bool IsEmpty => this.Count == 0;

  public WorkbookImpl ParentWorkbook => this.m_book;

  public object GetValueRowCol(int row, int col)
  {
    IRange range = this[row, col];
    return range.HasFormula ? (object) range.Formula : (object) range.Value;
  }

  public void SetValueRowCol(object value, int row, int col)
  {
    if (value == null)
      return;
    this.SetValue(row, col, value.ToString());
  }

  public void WireParentObject()
  {
  }

  public IWorkbook Workbook => (IWorkbook) this.m_book;

  public IRange[] Cells => (IRange[]) null;

  public OfficeSheetView View
  {
    get => this.m_view;
    set => this.m_view = value;
  }

  public bool DisplayPageBreaks
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      bool displayPageBreaks = innerList[0].DisplayPageBreaks;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].DisplayPageBreaks != displayPageBreaks)
          return false;
      }
      return displayPageBreaks;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].DisplayPageBreaks = value;
    }
  }

  public bool HasOleObject
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public int Index => -1;

  public int TabIndex => -1;

  public bool ProtectDrawingObjects => false;

  public bool ProtectScenarios => false;

  public IRange[] MergedCells => (IRange[]) null;

  public string Name
  {
    get => (string) null;
    set => throw new NotSupportedException();
  }

  public INames Names => throw new NotSupportedException();

  public string CodeName
  {
    get => (string) null;
    set => throw new NotSupportedException();
  }

  public IPageSetup PageSetup
  {
    get
    {
      if (this.m_pageSetup == null)
        this.m_pageSetup = new PageSetupGroup(this.Application, (object) this);
      return (IPageSetup) this.m_pageSetup;
    }
  }

  public IRange Range => this.UsedRange;

  public IRange[] Rows => (IRange[]) null;

  public IRange[] Columns => (IRange[]) null;

  public double StandardHeight
  {
    get => 0.0;
    set
    {
    }
  }

  public bool StandardHeightFlag
  {
    get => false;
    set
    {
    }
  }

  public double StandardWidth
  {
    get => 0.0;
    set
    {
    }
  }

  public OfficeSheetType Type => throw new NotImplementedException();

  public IRange UsedRange
  {
    get
    {
      if (this.IsEmpty)
        return (IRange) null;
      WorksheetImpl inner = (WorksheetImpl) this.InnerList[0];
      int firstRow = inner.FirstRow;
      int lastRow = inner.LastRow;
      int firstColumn = inner.FirstColumn;
      int lastColumn = inner.LastColumn;
      if (this.m_usedRange == null || this.m_usedRange.Row != firstRow || this.m_usedRange.Column != firstColumn || this.m_usedRange.LastRow != lastRow || this.m_usedRange.LastColumn != lastColumn)
        this.m_usedRange = (IRange) new RangeGroup(this.Application, (object) this, firstRow, firstColumn, lastRow, lastColumn);
      return this.m_usedRange;
    }
  }

  public int Zoom
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int zoom = innerList[0].Zoom;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].Zoom != zoom)
          return int.MinValue;
      }
      return zoom;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].Zoom = value;
    }
  }

  public OfficeWorksheetVisibility Visibility
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      OfficeWorksheetVisibility visibility = innerList[0].Visibility;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].Visibility != visibility)
          return OfficeWorksheetVisibility.Visible;
      }
      return visibility;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].Visibility = value;
    }
  }

  public int VerticalSplit
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public int HorizontalSplit
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public int FirstVisibleRow
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public int FirstVisibleColumn
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public int ActivePane
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public bool IsDisplayZeros
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public bool IsGridLinesVisible
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      bool gridLinesVisible = innerList[0].IsGridLinesVisible;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].IsGridLinesVisible != gridLinesVisible)
          return false;
      }
      return gridLinesVisible;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].IsGridLinesVisible = value;
    }
  }

  public OfficeKnownColors GridLineColor
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public bool IsRowColumnHeadersVisible
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      bool columnHeadersVisible = innerList[0].IsRowColumnHeadersVisible;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].IsRowColumnHeadersVisible != columnHeadersVisible)
          return false;
      }
      return columnHeadersVisible;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].IsRowColumnHeadersVisible = value;
    }
  }

  public bool IsStringsPreserved
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      bool stringsPreserved = innerList[0].IsStringsPreserved;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].IsStringsPreserved != stringsPreserved)
          return false;
      }
      return stringsPreserved;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].IsStringsPreserved = value;
    }
  }

  public bool IsPasswordProtected => false;

  public IRange this[int row, int column] => (IRange) null;

  public IRange this[int row, int column, int lastRow, int lastColumn] => (IRange) null;

  public IRange this[string name] => (IRange) null;

  public IRange this[string name, bool IsR1C1Notation] => (IRange) null;

  public IRange[] UsedCells => throw new NotSupportedException();

  public IWorksheetCustomProperties CustomProperties => throw new NotSupportedException();

  public IMigrantRange MigrantRange
  {
    get
    {
      if (this.m_migrantRange == null)
        this.CreateMigrantRange();
      return this.m_migrantRange;
    }
  }

  public bool UseRangesCache
  {
    get
    {
      if (this.Count == 0)
        return false;
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      bool useRangesCache = innerList[0].UseRangesCache;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].UseRangesCache != useRangesCache)
          return false;
      }
      return useRangesCache;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].UseRangesCache = value;
    }
  }

  public OfficeSheetProtection Protection
  {
    get => throw new NotSupportedException("This property doesnot support in this case.");
  }

  public bool ProtectContents
  {
    get => throw new NotSupportedException("This property doesnot supported yet.");
  }

  public int TopVisibleRow
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public int LeftVisibleColumn
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public bool UsedRangeIncludesFormatting
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public void Activate() => throw new NotSupportedException();

  public void CopyToClipboard() => throw new NotSupportedException();

  void IWorksheet.Clear()
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Clear();
  }

  public void ClearData()
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].ClearData();
  }

  public bool Contains(int iRow, int iColumn)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    bool flag = innerList[0].Contains(iRow, iColumn);
    if (!flag)
      return flag;
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].Contains(iRow, iColumn) != flag)
        return false;
    }
    return flag;
  }

  public IRanges CreateRangesCollection() => (IRanges) null;

  public void CreateNamedRanges(string namedRange, string referRange, bool vertical)
  {
    throw new NotImplementedException();
  }

  public bool IsColumnVisible(int columnIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    bool flag = innerList[0].IsColumnVisible(columnIndex);
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].IsColumnVisible(columnIndex) != flag)
        return false;
    }
    return flag;
  }

  public void ShowColumn(int columnIndex, bool isVisible)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].ShowColumn(columnIndex, isVisible);
  }

  public void HideColumn(int columnIndex) => this.ShowColumn(columnIndex, false);

  public void HideRow(int rowIndex) => this.ShowRow(rowIndex, false);

  public bool IsRowVisible(int rowIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    bool flag = innerList[0].IsRowVisible(rowIndex);
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].IsRowVisible(rowIndex) != flag)
        return false;
    }
    return flag;
  }

  public void ShowRow(int rowIndex, bool isVisible)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].ShowRow(rowIndex, isVisible);
  }

  public void ShowRange(IRange range, bool isVisible)
  {
    foreach (IWorksheet inner in (IEnumerable<IWorksheet>) this.InnerList)
      inner.ShowRange(range, isVisible);
  }

  public void ShowRange(RangesCollection ranges, bool isVisible)
  {
    if (ranges.Count == 0)
      return;
    foreach (IRange range in ranges)
      this.ShowRange(range, isVisible);
  }

  public void ShowRange(IRange[] ranges, bool isVisible)
  {
    if (ranges.Length == 0)
      return;
    RangesCollection ranges1 = new RangesCollection(this.Application, (object) this);
    foreach (IRange range in ranges)
      ranges1.Add(range);
    this.ShowRange(ranges1, isVisible);
  }

  public void InsertRow(int index)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index1 = 0;
    for (int count = innerList.Count; index1 < count; ++index1)
      innerList[index1].InsertRow(index);
  }

  public void InsertRow(int iRowIndex, int iRowCount)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].InsertRow(iRowIndex, iRowCount);
  }

  public void InsertRow(int iRowIndex, int iRowCount, OfficeInsertOptions insertOptions)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].InsertRow(iRowIndex, iRowCount, insertOptions);
  }

  public void InsertColumn(int index)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index1 = 0;
    for (int count = innerList.Count; index1 < count; ++index1)
      innerList[index1].InsertColumn(index);
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].InsertColumn(iColumnIndex, iColumnCount);
  }

  public void InsertColumn(int iColumnIndex, int iColumnCount, OfficeInsertOptions options)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].InsertColumn(iColumnIndex, iColumnCount, options);
  }

  public void DeleteRow(int index)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index1 = 0;
    for (int count = innerList.Count; index1 < count; ++index1)
      innerList[index1].DeleteRow(index);
  }

  public void DeleteRow(int index, int count)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index1 = 0;
    for (int count1 = innerList.Count; index1 < count1; ++index1)
      innerList[index1].DeleteRow(index, count);
  }

  public void DeleteColumn(int index)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index1 = 0;
    for (int count = innerList.Count; index1 < count; ++index1)
      innerList[index1].DeleteColumn(index);
  }

  public void DeleteColumn(int index, int count)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index1 = 0;
    for (int count1 = innerList.Count; index1 < count1; ++index1)
      innerList[index1].DeleteColumn(index, count);
  }

  public int ImportArray(object[] arrObject, int firstRow, int firstColumn, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportArray(arrObject, firstRow, firstColumn, isVertical);
    return num;
  }

  public int ImportArray(string[] arrString, int firstRow, int firstColumn, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportArray(arrString, firstRow, firstColumn, isVertical);
    return num;
  }

  public int ImportArray(int[] arrInt, int firstRow, int firstColumn, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportArray(arrInt, firstRow, firstColumn, isVertical);
    return num;
  }

  public int ImportArray(double[] arrDouble, int firstRow, int firstColumn, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportArray(arrDouble, firstRow, firstColumn, isVertical);
    return num;
  }

  public int ImportArray(DateTime[] arrDateTime, int firstRow, int firstColumn, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportArray(arrDateTime, firstRow, firstColumn, isVertical);
    return num;
  }

  public int ImportArray(object[,] arrObject, int firstRow, int firstColumn)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportArray(arrObject, firstRow, firstColumn);
    return num;
  }

  public int ImportData(IEnumerable arrObject, int firstRow, int firstColumn, bool includeHeader)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportData(arrObject, firstRow, firstColumn, includeHeader);
    return num;
  }

  public int ImportDataColumn(
    DataColumn dataColumn,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataColumn(dataColumn, isFieldNameShown, firstRow, firstColumn);
    return num;
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn);
    return num;
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool preserveTypes)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, preserveTypes);
    return num;
  }

  public int ImportDataTable(DataTable dataTable, int firtRow, int firstColumn, bool importOnSave)
  {
    return 0;
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns);
    return num;
  }

  public int ImportDataTable(
    DataTable dataTable,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool preserveTypes)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataTable(dataTable, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, preserveTypes);
    return num;
  }

  public int ImportDataTable(DataTable dataTable, IName namedRange, bool isFieldNameShown)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, 0, 0);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, -1, -1);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol)
  {
    return this.ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, iMaxRow, iMaxCol, false);
  }

  public int ImportDataTable(
    DataTable dataTable,
    IName namedRange,
    bool isFieldNameShown,
    int rowOffset,
    int columnOffset,
    int iMaxRow,
    int iMaxCol,
    bool bPreserveTypes)
  {
    return this.InnerList.Count == 0 ? 0 : this.InnerList[0].ImportDataTable(dataTable, namedRange, isFieldNameShown, rowOffset, columnOffset, iMaxRow, iMaxCol, bPreserveTypes);
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn);
    return num;
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    bool bPreserveTypes)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, bPreserveTypes);
    return num;
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns);
    return num;
  }

  public int ImportDataView(
    DataView dataView,
    bool isFieldNameShown,
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    bool bPreserveTypes)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int num = 0;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      num = innerList[index].ImportDataView(dataView, isFieldNameShown, firstRow, firstColumn, maxRows, maxColumns, bPreserveTypes);
    return num;
  }

  public void RemovePanes() => throw new NotSupportedException();

  public DataTable ExportDataTable(
    int firstRow,
    int firstColumn,
    int maxRows,
    int maxColumns,
    ExcelExportDataTableOptions options)
  {
    throw new NotSupportedException();
  }

  public DataTable ExportDataTable(IRange dataRange, ExcelExportDataTableOptions options)
  {
    throw new NotSupportedException();
  }

  public void Protect(string password) => throw new NotSupportedException();

  public void Protect(string password, OfficeSheetProtection options)
  {
    throw new NotSupportedException();
  }

  public void Unprotect(string password) => throw new NotSupportedException();

  public IRange IntersectRanges(IRange range1, IRange range2) => (IRange) null;

  public IRange MergeRanges(IRange range1, IRange range2) => (IRange) null;

  public void AutofitRow(int rowIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].AutofitRow(rowIndex);
  }

  public void AutofitColumn(int colIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].AutofitColumn(colIndex);
  }

  public void Replace(string oldValue, string newValue)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, double newValue)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, DateTime newValue)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValue);
  }

  public void Replace(string oldValue, string[] newValues, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, int[] newValues, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, double[] newValues, bool isVertical)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValues, isVertical);
  }

  public void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValues, isFieldNamesShown);
  }

  public void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Replace(oldValue, newValues, isFieldNamesShown);
  }

  public void Remove()
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].Remove();
    this.Clear();
  }

  public void Move(int iNewIndex) => throw new NotSupportedException();

  public int ColumnWidthToPixels(double widthInChars)
  {
    return this.List[0].ColumnWidthToPixels(widthInChars);
  }

  public double PixelsToColumnWidth(int pixels) => this.List[0].PixelsToColumnWidth(pixels);

  public void SetColumnWidth(int iColumnIndex, double value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetColumnWidth(iColumnIndex, value);
  }

  public void SetColumnWidthInPixels(int iColumnIndex, int value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetColumnWidthInPixels(iColumnIndex, value);
  }

  public void SetColumnWidthInPixels(int iStartColumnIndex, int iCount, int value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetColumnWidthInPixels(iStartColumnIndex, iCount, value);
  }

  public void SetRowHeight(int iRow, double value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetRowHeight(iRow, value);
  }

  public void SetRowHeightInPixels(int iRowIndex, double value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetRowHeightInPixels(iRowIndex, value);
  }

  public void SetRowHeightInPixels(int iStartRowIndex, int iCount, double value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetRowHeightInPixels(iStartRowIndex, iCount, value);
  }

  public double GetColumnWidth(int iColumnIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    double columnWidth = innerList[0].GetColumnWidth(iColumnIndex);
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].GetColumnWidth(iColumnIndex) != columnWidth)
      {
        columnWidth = double.NaN;
        break;
      }
    }
    return columnWidth;
  }

  public int GetColumnWidthInPixels(int iColumnIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int columnWidthInPixels = innerList[0].GetColumnWidthInPixels(iColumnIndex);
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].GetColumnWidthInPixels(iColumnIndex) != columnWidthInPixels)
        return int.MinValue;
    }
    return columnWidthInPixels;
  }

  public double GetRowHeight(int iRowIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    double rowHeight = innerList[0].GetRowHeight(iRowIndex);
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].GetRowHeight(iRowIndex) != rowHeight)
        return double.NaN;
    }
    return rowHeight;
  }

  public int GetRowHeightInPixels(int iRowIndex)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int rowHeightInPixels = innerList[0].GetRowHeightInPixels(iRowIndex);
    int index = 1;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index].GetRowHeightInPixels(iRowIndex) != rowHeightInPixels)
        return int.MinValue;
    }
    return rowHeightInPixels;
  }

  public IRange FindFirst(string findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange FindFirst(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringStartsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindStringEndsWith(string findValue, OfficeFindType flags, bool ignoreCase)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange FindFirst(double findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange FindFirst(bool findValue) => throw new NotImplementedException();

  public IRange FindFirst(DateTime findValue) => throw new NotImplementedException();

  public IRange FindFirst(TimeSpan findValue) => throw new NotImplementedException();

  public IRange[] FindAll(string findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] FindAll(string findValue, OfficeFindType flags, OfficeFindOptions findOptions)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  public IRange[] FindAll(double findValue, OfficeFindType flags)
  {
    throw new NotImplementedException();
  }

  public IRange[] FindAll(bool findValue) => throw new NotImplementedException();

  public IRange[] FindAll(DateTime findValue) => throw new NotImplementedException();

  public IRange[] FindAll(TimeSpan findValue) => throw new NotImplementedException();

  public void SaveAs(string fileName, string separator) => throw new NotSupportedException();

  public void SaveAs(string fileName, string separator, Encoding encoding)
  {
    throw new NotSupportedException();
  }

  public void SaveAs(Stream stream, string separator) => throw new NotSupportedException();

  public void SaveAs(Stream stream, string separator, Encoding encoding)
  {
    throw new NotSupportedException();
  }

  public void SetDefaultColumnStyle(int iColumnIndex, IStyle defaultStyle)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetDefaultColumnStyle(iColumnIndex, defaultStyle);
  }

  public void SetDefaultColumnStyle(
    int iStartColumnIndex,
    int iEndColumnIndex,
    IStyle defaultStyle)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetDefaultColumnStyle(iStartColumnIndex, iEndColumnIndex, defaultStyle);
  }

  public void SetDefaultRowStyle(int iRowIndex, IStyle defaultStyle)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetDefaultRowStyle(iRowIndex, defaultStyle);
  }

  public void SetDefaultRowStyle(int iStartRowIndex, int iEndRowIndex, IStyle defaultStyle)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetDefaultRowStyle(iStartRowIndex, iEndRowIndex, defaultStyle);
  }

  public IStyle GetDefaultRowStyle(int iRowIndex) => (IStyle) null;

  public IStyle GetDefaultColumnStyle(int iColumnIndex) => (IStyle) null;

  public void FreeRange(IRange range) => throw new NotImplementedException();

  public void FreeRange(int iRow, int iColumn) => throw new NotImplementedException();

  public void SetValue(int iRow, int iColumn, string value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetValue(iRow, iColumn, value);
  }

  public void SetNumber(int iRow, int iColumn, double value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetNumber(iRow, iColumn, value);
  }

  public void SetBoolean(int iRow, int iColumn, bool value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetBoolean(iRow, iColumn, value);
  }

  public void SetText(int iRow, int iColumn, string value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetText(iRow, iColumn, value);
  }

  public void SetFormula(int iRow, int iColumn, string value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetFormula(iRow, iColumn, value);
  }

  public void SetError(int iRow, int iColumn, string value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetError(iRow, iColumn, value);
  }

  public void SetBlank(int iRow, int iColumn)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetBlank(iRow, iColumn);
  }

  public void SetFormulaNumberValue(int iRow, int iColumn, double value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetFormulaNumberValue(iRow, iColumn, value);
  }

  public void SetFormulaErrorValue(int iRow, int iColumn, string value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetFormulaErrorValue(iRow, iColumn, value);
  }

  public void SetFormulaBoolValue(int iRow, int iColumn, bool value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetFormulaBoolValue(iRow, iColumn, value);
  }

  public void SetFormulaStringValue(int iRow, int iColumn, string value)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].SetFormulaStringValue(iRow, iColumn, value);
  }

  public string GetText(int row, int column) => throw new NotSupportedException();

  public double GetNumber(int row, int column) => throw new NotSupportedException();

  public string GetFormula(int row, int column, bool bR1C1) => throw new NotSupportedException();

  public string GetError(int row, int column) => throw new NotSupportedException();

  public bool GetBoolean(int row, int column) => throw new NotSupportedException();

  public string GetFormulaStringValue(int row, int column) => throw new NotSupportedException();

  public double GetFormulaNumberValue(int row, int column) => throw new NotSupportedException();

  public string GetFormulaErrorValue(int row, int column) => throw new NotSupportedException();

  public bool GetFormulaBoolValue(int row, int column) => throw new NotSupportedException();

  public bool IsFreezePanes
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (!innerList[index].IsFreezePanes)
          return false;
      }
      return true;
    }
  }

  public IRange SplitCell => throw new NotImplementedException("Split Cell");

  public Image ConvertToImage(int firstRow, int firstColumn, int lastRow, int lastColumn)
  {
    throw new NotSupportedException();
  }

  public Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream stream)
  {
    throw new NotSupportedException();
  }

  public Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    EmfType emfType,
    Stream outputStream)
  {
    throw new NotImplementedException();
  }

  public Image ConvertToImage(
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream,
    EmfType emfType)
  {
    throw new NotImplementedException();
  }

  public OfficeKnownColors TabColor
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      OfficeKnownColors tabColor = innerList[0].TabColor;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].TabColor != tabColor)
          return OfficeKnownColors.Black;
      }
      return tabColor;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].TabColor = value;
    }
  }

  public Color TabColorRGB
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      Color tabColorRgb = innerList[0].TabColorRGB;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].TabColorRGB != tabColorRgb)
          return ColorExtension.Empty;
      }
      return tabColorRgb;
    }
    set
    {
    }
  }

  public IOfficeChartShapes Charts => throw new NotSupportedException();

  public IShapes Shapes => throw new NotSupportedException();

  public void Select()
  {
  }

  public void Unselect()
  {
  }

  public bool IsRightToLeft
  {
    get
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      bool isRightToLeft = innerList[0].IsRightToLeft;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].IsRightToLeft != isRightToLeft || !isRightToLeft)
          return false;
      }
      return isRightToLeft;
    }
    set
    {
      IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].IsRightToLeft = value;
    }
  }

  public bool IsSelected => true;

  public ITextBoxes TextBoxes => throw new NotSupportedException();

  private void WorksheetGroup_Inserted(object sender, CollectionChangeEventArgs<IWorksheet> args)
  {
    WorksheetBaseImpl worksheetBaseImpl = args.Value as WorksheetBaseImpl;
    worksheetBaseImpl.SelectTab();
    this.m_book.WindowOne.NumSelectedTabs = (ushort) this.Count;
    if (this.Count != 1)
      return;
    this.m_book.WindowOne.SelectedTab = (ushort) worksheetBaseImpl.RealIndex;
  }

  private void WorksheetGroup_Removing(object sender, CollectionChangeEventArgs<IWorksheet> args)
  {
    if (this.Count == 1)
      throw new ApplicationException("Can't deselect all worksheets.");
    args.Value.Unselect();
  }

  private void WorksheetGroup_Clearing()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      (this.List[index] as WorksheetBaseImpl).Unselect(false);
    this.m_book.WindowOne.NumSelectedTabs = (ushort) 0;
  }

  protected override void OnClear()
  {
    base.OnClear();
    if (this.m_book == null)
      this.m_book = (WorkbookImpl) null;
    if (this.m_pageSetup != null)
      this.m_pageSetup.Dispose();
    if (this.m_usedRange == null)
      return;
    this.m_usedRange = (IRange) null;
  }
}
