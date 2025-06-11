// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Tables.ListObjectCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Tables;

public class ListObjectCollection : 
  List<IListObject>,
  IListObjects,
  IList<IListObject>,
  ICollection<IListObject>,
  IEnumerable<IListObject>,
  IEnumerable
{
  private int sheetindex;

  public ListObjectCollection(WorksheetImpl sheet) => this.sheetindex = sheet.Index;

  public IListObject Create(string name, IRange range)
  {
    WorkbookImpl workbook = range.Worksheet.Workbook as WorkbookImpl;
    if (!workbook.Loading)
    {
      if ((workbook.Names as WorkbookNamesCollection).IsNameExist(name))
        throw new Exception("Name already exist.Name must be unique");
      range = this.CheckRange(range);
    }
    ListObjectCollection.CheckOverLab(range);
    ListObject listObject = new ListObject(name, range, this.Count + 1);
    listObject.Name = name;
    listObject.Location = range;
    listObject.Index = ++workbook.MaxTableIndex;
    this.Add((IListObject) listObject);
    return (IListObject) listObject;
  }

  internal static void CheckOverLab(IRange range)
  {
    WorksheetImpl worksheet = range.Worksheet as WorksheetImpl;
    if (worksheet.IsParsing)
      return;
    int row1 = range.Row;
    int column1 = range.Column;
    int lastRow1 = range.LastRow;
    int lastColumn1 = range.LastColumn;
    for (int index = 0; index < worksheet.ListObjects.Count; ++index)
    {
      IListObject listObject = worksheet.ListObjects[index];
      int row2 = listObject.Location.Row;
      int column2 = listObject.Location.Column;
      int lastRow2 = listObject.Location.LastRow;
      int lastColumn2 = listObject.Location.LastColumn;
      if (row1 > row2 - 1 && row1 < lastRow2 + 1 && column1 > column2 - 1 && column1 < lastColumn2 + 1)
        throw new ArgumentException("A table cannot overlap a range that contains a PivotTable report, query results, protected cells or another table");
      if (row1 > row2 - 1 && row1 < lastRow2 + 1 && lastColumn1 > column2 - 1 && lastColumn1 < lastColumn2 + 1)
        throw new ArgumentException("A table cannot overlap a range that contains a PivotTable report, query results, protected cells or another table");
      if (lastRow1 > row2 - 1 && lastRow1 < lastRow2 + 1 && column1 > column2 - 1 && column1 < lastColumn2 + 1)
        throw new ArgumentException("A table cannot overlap a range that contains a PivotTable report, query results, protected cells or another table");
      if (lastRow1 > row2 - 1 && lastRow1 < lastRow2 + 1 && lastColumn1 > column2 - 1 && lastColumn1 < lastColumn2 + 1)
        throw new ArgumentException("A table cannot overlap a range that contains a PivotTable report, query results, protected cells or another table");
      if ((row1 < row2 - 1 && row1 < lastRow2 + 1 || lastRow1 > row2 - 1 && lastRow1 < lastRow2 + 1) && column2 > column1 && lastColumn2 < lastColumn1)
        throw new ArgumentException("A table cannot overlap a range that contains a PivotTable report, query results, protected cells or another table");
      if ((column1 < column2 - 1 && column1 < lastColumn2 + 1 || lastColumn1 > column2 - 1 && lastColumn1 < lastColumn2 + 1) && row2 > row1 && lastRow2 < lastRow1)
        throw new ArgumentException("A table cannot overlap a range that contains a PivotTable report, query results, protected cells or another table");
    }
  }

  private IRange CheckRange(IRange range)
  {
    int row = range.Row;
    if (row == range.LastRow)
    {
      IWorksheet worksheet = range.Worksheet;
      IRange usedRange = worksheet.UsedRange;
      int column = range.Column;
      if (usedRange.LastRow > range.LastRow)
      {
        IRange destination = worksheet[row + 2, column];
        worksheet[row + 1, column, usedRange.LastRow, range.LastColumn].MoveTo(destination);
      }
      range = worksheet[row, column, row + 1, range.LastColumn];
    }
    return range;
  }

  internal ListObjectCollection Clone(
    WorksheetImpl worksheet,
    Dictionary<string, string> hashWorksheetNames)
  {
    ListObjectCollection objectCollection = new ListObjectCollection(worksheet);
    WorkbookImpl parentWorkbook = worksheet.ParentWorkbook;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ListObject listObject = ((ListObject) this[index]).Clone(worksheet, hashWorksheetNames);
      listObject.Index = ++parentWorkbook.MaxTableIndex;
      string str = listObject.Name;
      while (objectCollection[str] != null)
      {
        string stringPart;
        int numberPart;
        listObject.SplitName(str, out stringPart, out numberPart);
        ++numberPart;
        str = stringPart + (object) numberPart;
        listObject.DisplayName = listObject.Name = str;
      }
      objectCollection.Add((IListObject) listObject);
    }
    return objectCollection;
  }

  public IListObject this[string name]
  {
    get
    {
      IListObject listObject1 = (IListObject) null;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IListObject listObject2 = this[index];
        if (listObject2.Name == name)
        {
          listObject1 = listObject2;
          break;
        }
      }
      return listObject1;
    }
  }

  public IListObject AddEx(
    ExcelListObjectSourceType type,
    IConnection connection,
    IRange Destinaion)
  {
    RangeImpl rangeImpl = Destinaion as RangeImpl;
    WorksheetImpl parent = rangeImpl.Parent as WorksheetImpl;
    if (rangeImpl.Worksheet.Index != this.sheetindex)
      throw new ArgumentException("The worksheet range for the table data must be on the same sheet as the table being created");
    ExternalConnection ExternalConnection = connection as ExternalConnection;
    ExternalConnection.IsExist = !ExternalConnection.IsExist ? true : throw new ArgumentException("This connection already exist");
    string str;
    if (connection.ODBCConnection != null)
    {
      str = ExternalConnection.ODBCConnection.ConnectionString.ToString();
    }
    else
    {
      if (connection.OLEDBConnection == null)
        throw new ArgumentException("Connection is invalid");
      str = ExternalConnection.OLEDBConnection.ConnectionString.ToString();
    }
    IWorkbook workbook = parent.Workbook;
    ListObject list = parent.ListObjects.Create(connection.Name, Destinaion) as ListObject;
    ExternalConnection.Range = list.Location;
    list.TableType = ExcelTableType.queryTable;
    ListObject listObject = list;
    listObject.QueryTable = new QueryTableImpl(parent.Application, parent.Parent, ExternalConnection);
    listObject.QueryTable.ConnectionString = (object) str;
    for (int index = 0; index < list.Columns.Count; ++index)
    {
      IListObjectColumn column = list.Columns[index];
      (column as ListObjectColumn).QueryTableFieldId = index + 1;
      (column as ListObjectColumn).Id = index + 1;
      listObject.QueryTable.QueryTableRefresh.QueryFields.Add(new QueryTableField(index + 1, index + 1, listObject.QueryTable.QueryTableRefresh));
      listObject.QueryTable.QueryTableRefresh.QueryFields[index].Name = column.Name;
    }
    listObject.QueryTable.Name = ExternalConnection.Name.Replace(" ", "") + "Query";
    listObject.DisplayName = $"{ExternalConnection.Name.Replace(" ", "")}_{list.Index.ToString()}";
    listObject.BuiltInTableStyle = TableBuiltInStyles.TableStyleMedium2;
    listObject.ShowTableStyleRowStripes = true;
    this.AddDefineName((IListObject) list);
    return (IListObject) listObject;
  }

  public void AddDefineName(IListObject list)
  {
    WorkbookImpl workbook = (list.Location.Worksheet as WorksheetImpl).Workbook as WorkbookImpl;
    NameImpl nameImpl = workbook.Names.Add(list.QueryTable.Name) as NameImpl;
    nameImpl.SetValue(workbook.FormulaUtil.ParseString(list.Location.AddressGlobal));
    nameImpl.RefersToRange = list.Location;
    nameImpl.IsQueryTableRange = true;
    nameImpl.SheetIndex = list.Worksheet.Index;
  }

  internal void Dispose()
  {
    for (int index = this.Count - 1; index > -1; --index)
      ((ListObject) this[index]).Dispose();
  }

  public new bool Remove(IListObject listObject)
  {
    listObject.Worksheet.Workbook.Names.Remove(listObject.Name);
    return base.Remove(listObject);
  }

  public new void RemoveAt(int index)
  {
    if (index >= this.Count)
      return;
    this.Remove(this[index]);
  }
}
