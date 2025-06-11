// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfDataSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

#nullable disable
namespace Syncfusion.Pdf.Tables;

internal class PdfDataSource
{
  private DataTable m_dataTable;
  private int m_rowCount;
  private int m_colCount;
  private DataColumn m_dataColumn;
  private Array m_array;
  private bool m_useSorting = true;
  private DataRow[] m_cachRows;

  internal bool UseSorting
  {
    get => this.m_useSorting;
    set => this.m_useSorting = value;
  }

  public int RowCount => this.m_rowCount;

  public int ColumnCount => this.GetVisibleColCount();

  public string[] ColumnNames => this.GetColumnsNames();

  public string[] ColumnCaptions => this.GetColumnsCaptions();

  private PdfDataSource()
  {
  }

  public PdfDataSource(DataTable table)
  {
    if (table == null)
      throw new ArgumentNullException("Data table can't be null", nameof (table));
    this.SetTable(table);
  }

  internal PdfDataSource(IEnumerable customSource)
  {
    if (customSource == null)
      return;
    DataTable table = new DataTable();
    PropertyInfo[] propertyInfoArray = (PropertyInfo[]) null;
    PdfRow pdfRow = new PdfRow();
    List<string> stringList1 = new List<string>();
    foreach (object obj in customSource)
    {
      if (obj != null)
      {
        propertyInfoArray = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (PropertyInfo propertyInfo in propertyInfoArray)
        {
          table.Columns.Add(propertyInfo.Name);
          stringList1.Add(propertyInfo.Name);
        }
        break;
      }
    }
    pdfRow.Values = (object[]) stringList1.ToArray();
    table.Rows.Add(pdfRow.Values);
    int count = table.Columns.Count;
    foreach (object obj in customSource)
    {
      List<string> stringList2 = new List<string>();
      foreach (PropertyInfo propertyInfo in propertyInfoArray)
      {
        PropertyInfo property = obj.GetType().GetProperty(propertyInfo.Name);
        stringList2.Add(Convert.ToString(property.GetValue(obj, (object[]) null)));
      }
      pdfRow.Values = (object[]) stringList2.ToArray();
      table.Rows.Add(pdfRow.Values);
    }
    if (table == null)
      throw new ArgumentNullException("Data table can't be null", "table");
    this.SetTable(table);
  }

  public PdfDataSource(DataSet dataSet, string tableName)
    : this(PdfDataSource.GetTableFromDataSet(dataSet, tableName))
  {
  }

  public PdfDataSource(DataView view)
    : this(PdfDataSource.GetTableFromDataView(view))
  {
  }

  public PdfDataSource(DataColumn column)
  {
    if (column == null)
      throw new ArgumentNullException("Column can't be null", nameof (column));
    this.m_dataColumn = column.Table != null ? column : throw new ArgumentNullException("Data column must belong to some table", nameof (column));
    this.m_colCount = 1;
    this.m_rowCount = this.m_dataColumn.Table.Rows.Count;
  }

  public PdfDataSource(Array array)
  {
    if (array == null)
      throw new ArgumentException("Array can'n be null", nameof (array));
    this.m_array = this.IsArrayValid(array, ref this.m_colCount) ? array : throw new ArgumentException("We don't suuport more than one or two dimensions arrays in this context or you array has diiferent length", nameof (array));
    this.m_rowCount = this.m_array.GetLength(0);
  }

  public string[] GetRow(ref int index)
  {
    if (index < 0)
      throw new IndexOutOfRangeException("The index must be less than rows count ormore or equels than zero");
    string[] row = (string[]) null;
    if (index < this.m_rowCount)
    {
      if (this.m_dataTable != null)
        row = this.GetRowFromTable(this.m_dataTable, ref index);
      if (this.m_dataColumn != null)
        row = this.GetRowFromColumn(this.m_dataColumn, ref index);
      if (this.m_array != null)
        row = this.GetRowFromArray(this.m_array, ref index);
    }
    return row;
  }

  public bool IsColumnReadOnly(int index)
  {
    if (index < 0 || index >= this.GetVisibleColCount())
      throw new IndexOutOfRangeException("The index must be less than columns count ormore than or equal to zero.");
    bool flag = false;
    if (this.m_dataTable != null)
      flag = this.m_dataTable.Columns[this.GetVisibleIndex(index)].ReadOnly;
    if (this.m_dataColumn != null)
      flag = this.m_dataColumn.ReadOnly;
    if (this.m_array != null)
      flag = this.m_array.IsReadOnly;
    return flag;
  }

  public MappingType GetColumnMappingType(int index)
  {
    if (index < 0 || index >= this.GetVisibleColCount())
      throw new IndexOutOfRangeException("The index must be less than columns count ormore or equels than zero");
    MappingType columnMappingType = MappingType.Hidden;
    if (this.m_dataTable != null)
      columnMappingType = this.m_dataTable.Columns[this.GetVisibleIndex(index)].ColumnMapping;
    if (this.m_dataColumn != null)
      columnMappingType = this.m_dataColumn.ColumnMapping;
    if (this.m_array != null)
      throw new ArgumentException("Array does not have mapping type propety");
    return columnMappingType;
  }

  public Type GetColumnDataType(int index)
  {
    if (index < 0 || index >= this.GetVisibleColCount())
      throw new IndexOutOfRangeException("The index must be less than columns count ormore or equels than zero");
    Type columnDataType = (Type) null;
    if (this.m_dataTable != null)
      columnDataType = this.m_dataTable.Columns[this.GetVisibleIndex(index)].DataType;
    if (this.m_dataColumn != null)
      columnDataType = this.m_dataColumn.DataType;
    if (this.m_array != null)
      columnDataType = this.GetTypeOfArray(this.m_array);
    return columnDataType;
  }

  public object GetColumnDefaultValue(int index)
  {
    if (index < 0 || index >= this.GetVisibleColCount())
      throw new IndexOutOfRangeException("The index must be less than columns count ormore or equels than zero");
    object columnDefaultValue = (object) null;
    if (this.m_dataTable != null)
      columnDefaultValue = this.m_dataTable.Columns[this.GetVisibleIndex(index)].DefaultValue;
    if (this.m_dataColumn != null)
      columnDefaultValue = this.m_dataColumn.DefaultValue;
    if (this.m_array != null)
      throw new ArgumentException("Array does not have default value propety");
    return columnDefaultValue;
  }

  public bool AllowDBNull(int index)
  {
    if (index < 0 || index >= this.GetVisibleColCount())
      throw new IndexOutOfRangeException("The index must be less than columns count ormore or equels than zero");
    bool flag = false;
    if (this.m_dataTable != null)
      flag = this.m_dataTable.Columns[this.GetVisibleIndex(index)].AllowDBNull;
    if (this.m_dataColumn != null)
      flag = this.m_dataColumn.AllowDBNull;
    if (this.m_array != null)
      throw new ArgumentException("Array does not have allowDBNull propety");
    return flag;
  }

  private Type GetTypeOfArray(Array array)
  {
    Type typeOfArray = (Type) null;
    switch (array.Rank)
    {
      case 1:
        typeOfArray = !(array.GetValue(0) is Array array1) ? array.GetValue(0).GetType() : array1.GetValue(0).GetType();
        break;
      case 2:
        typeOfArray = array.GetValue(0, 0).GetType();
        break;
    }
    return typeOfArray;
  }

  private string[] GetColumnsNames()
  {
    string[] columnsNames = (string[]) null;
    if (this.m_dataTable != null)
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.m_colCount; ++index)
      {
        DataColumn column = this.m_dataTable.Columns[index];
        if (column.ColumnMapping != MappingType.Hidden)
          stringList.Add(column.ColumnName);
      }
      columnsNames = stringList.ToArray();
    }
    if (this.m_dataColumn != null && this.m_dataColumn.ColumnMapping != MappingType.Hidden)
      columnsNames = new string[1]
      {
        this.m_dataColumn.ColumnName
      };
    return columnsNames;
  }

  private string[] GetColumnsCaptions()
  {
    string[] columnsCaptions = (string[]) null;
    if (this.m_dataTable != null)
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.m_colCount; ++index)
      {
        DataColumn column = this.m_dataTable.Columns[index];
        if (column.ColumnMapping != MappingType.Hidden)
        {
          if (column.Caption != string.Empty)
            stringList.Add(column.Caption);
          else
            stringList.Add(column.ColumnName);
        }
      }
      columnsCaptions = stringList.ToArray();
    }
    if (this.m_dataColumn != null && this.m_dataColumn.ColumnMapping != MappingType.Hidden)
    {
      if (this.m_dataColumn.Caption != string.Empty)
        columnsCaptions = new string[1]
        {
          this.m_dataColumn.Caption
        };
      else
        columnsCaptions = new string[1]
        {
          this.m_dataColumn.ColumnName
        };
    }
    return columnsCaptions;
  }

  private bool IsArrayValid(Array array, ref int count)
  {
    bool flag = false;
    switch (array.Rank)
    {
      case 1:
        int num1 = 0;
        if (array.GetValue(0) is Array array2)
        {
          if (array2.Rank > 1)
          {
            flag = false;
            break;
          }
          int length1 = array2.GetLength(0);
          int index = 1;
          for (int length2 = array.Length; index < length2; ++index)
          {
            int num2 = 0;
            if (array.GetValue(index) is Array array1)
            {
              if (array1.Rank > 1)
              {
                flag = false;
                break;
              }
              num2 = array1.GetLength(0);
            }
            if (length1 != num2)
            {
              flag = false;
              break;
            }
            flag = true;
            count = length1;
          }
          break;
        }
        int num3;
        count = num3 = num1 + 1;
        flag = true;
        break;
      case 2:
        count = array.GetLength(1);
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  private void SetTable(DataTable table)
  {
    if (table.Columns.Count == 0)
      table.Columns.Add("Col0");
    this.m_dataTable = table;
    this.m_colCount = this.m_dataTable.Columns.Count;
    this.m_rowCount = this.m_dataTable.Rows.Count;
    this.m_dataTable.ColumnChanged += new DataColumnChangeEventHandler(this.dataTable_ColumnChanged);
    this.m_dataTable.RowChanged += new DataRowChangeEventHandler(this.dataTable_RowChanged);
    this.m_dataTable.RowDeleted += new DataRowChangeEventHandler(this.dataTable_RowDeleted);
  }

  private void dataTable_RowDeleted(object sender, DataRowChangeEventArgs e) => this.RefreshCache();

  private void dataTable_RowChanged(object sender, DataRowChangeEventArgs e) => this.RefreshCache();

  private void dataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
  {
    this.RefreshCache();
  }

  private void RefreshCache() => this.m_cachRows = (DataRow[]) null;

  private int GetVisibleColCount()
  {
    int visibleColCount = 0;
    if (this.m_dataTable != null)
    {
      for (int index = 0; index < this.m_colCount; ++index)
      {
        if (this.m_dataTable.Columns[index].ColumnMapping != MappingType.Hidden)
          ++visibleColCount;
      }
    }
    if (this.m_dataColumn != null)
      visibleColCount = this.m_dataColumn.ColumnMapping != MappingType.Hidden ? 1 : 0;
    if (this.m_array != null)
      visibleColCount = this.m_colCount;
    return visibleColCount;
  }

  private int GetVisibleIndex(int index)
  {
    if (index < 0 || index >= this.GetVisibleColCount())
      throw new IndexOutOfRangeException("The index must be less than columns count ormore than or equel to zero");
    int visibleIndex = 0;
    if (this.m_dataTable != null)
    {
      int num = index;
      int index1 = 0;
      while (num > -1)
      {
        if (this.m_dataTable.Columns[index1].ColumnMapping == MappingType.Hidden)
        {
          ++index1;
        }
        else
        {
          --num;
          ++index1;
        }
      }
      visibleIndex = index1 - 1;
    }
    if (this.m_dataColumn != null)
    {
      if (this.m_dataColumn.ColumnMapping == MappingType.Hidden)
        throw new ArgumentException("The source is DataColumn, but this column is hidden");
      visibleIndex = 0;
    }
    if (this.m_array != null)
      visibleIndex = index;
    return visibleIndex;
  }

  private string[] GetRowFromArray(Array array, ref int index)
  {
    string[] rowFromArray;
    switch (array.Rank)
    {
      case 1:
        if (!(array.GetValue(0) is Array))
        {
          rowFromArray = new string[this.m_colCount];
          for (int index1 = 0; index1 < this.m_colCount; ++index1)
          {
            object obj = array.GetValue(index);
            rowFromArray[index1] = Convert.ToString(obj);
          }
          break;
        }
        rowFromArray = new string[this.m_colCount];
        Array array1 = array.GetValue(index) as Array;
        for (int index2 = 0; index2 < this.m_colCount; ++index2)
        {
          object obj = array1.GetValue(index2);
          rowFromArray[index2] = Convert.ToString(obj);
        }
        break;
      case 2:
        rowFromArray = new string[this.m_colCount];
        for (int index2 = 0; index2 < this.m_colCount; ++index2)
        {
          object obj = array.GetValue(index, index2);
          rowFromArray[index2] = Convert.ToString(obj);
        }
        break;
      default:
        throw new ArgumentException("We don't suuport more than one or two dimensions arrays in this context or you array has diiferent length", nameof (array));
    }
    ++index;
    return rowFromArray;
  }

  private string[] GetRowFromColumn(DataColumn dataColumn, ref int index)
  {
    if (dataColumn.ColumnMapping == MappingType.Hidden)
      throw new ArgumentException("The source is DataColumn, but this column is hidden");
    string[] rowFromColumn;
    if (this.m_useSorting)
    {
      if (this.m_cachRows == null)
        this.m_cachRows = dataColumn.Table.Select();
      rowFromColumn = new string[1]
      {
        Convert.ToString(this.m_cachRows[index][dataColumn.ColumnName])
      };
    }
    else
      rowFromColumn = new string[1]
      {
        Convert.ToString(dataColumn.Table.Rows[index][dataColumn.ColumnName])
      };
    ++index;
    return rowFromColumn;
  }

  private string[] GetRowFromTable(DataTable dataTable, ref int index)
  {
    if (dataTable.Rows.Count <= 0)
      throw new ArgumentException("There is no rows in data source");
    if (index < 0 || index >= dataTable.Rows.Count)
      throw new IndexOutOfRangeException("The index must be less than rows count ormore or equels than zero");
    object[] itemArray;
    if (this.m_useSorting)
    {
      if (this.m_cachRows == null)
        this.m_cachRows = dataTable.Select();
      itemArray = this.m_cachRows[index].ItemArray;
    }
    else
      itemArray = dataTable.Rows[index].ItemArray;
    List<string> stringList = new List<string>();
    int index1 = 0;
    for (int length = itemArray.Length; index1 < length; ++index1)
    {
      if (dataTable.Columns[index1].ColumnMapping != MappingType.Hidden)
        stringList.Add(Convert.ToString(itemArray[index1]));
    }
    ++index;
    return stringList.ToArray();
  }

  private static DataTable GetTableFromDataSet(DataSet dataSet, string tableName)
  {
    if (dataSet == null)
      throw new ArgumentNullException("Data Set can't be null", nameof (dataSet));
    if (dataSet.Tables.Count <= 0)
      throw new ArgumentException("The data set should contain at least one data table", nameof (dataSet));
    DataTable table;
    if (tableName != null && tableName != string.Empty)
    {
      if (!dataSet.Tables.Contains(tableName))
        throw new ArgumentNullException("The data set should contain a table with specified table name", tableName);
      table = dataSet.Tables[tableName];
    }
    else
      table = dataSet.Tables[0];
    return table;
  }

  private static DataTable GetTableFromDataView(DataView view)
  {
    return view != null ? view.Table : throw new ArgumentNullException("Data view", nameof (view));
  }
}
