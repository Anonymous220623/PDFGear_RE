// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Tables.ListObject
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Microsoft.Win32;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Tables;

internal class ListObject : IListObject
{
  private string m_strName;
  private IRange m_location;
  private IRange m_localRange;
  private IList<IListObjectColumn> m_columns;
  private int m_iIndex;
  private TableBuiltInStyles m_builtInStyle;
  private string m_strDisplayName;
  private WorksheetImpl m_worksheet;
  private int m_iTotalsRowCount;
  private bool m_bRowStripes = true;
  private bool m_bColumnStripes;
  private bool m_bTotalsRowShown;
  private int m_iInsertRowShift;
  private string m_TableStyleName;
  private bool m_bFirstColumn;
  private bool m_bLastColumn;
  private bool m_bHeaderRow = true;
  private QueryTableImpl m_queryTable;
  private ExcelTableType m_tableType = ExcelTableType.worksheet;
  private int m_iDataDxfId = int.MinValue;
  private int m_iHeaderRowBorderDxfId = int.MinValue;
  private int m_iHeaderRowDxfId = int.MinValue;
  private int m_iTableBorderDxfId = int.MinValue;
  private int m_iTotalsRowBorderDxfId = int.MinValue;
  private int m_iTotalsRowDxfId = int.MinValue;
  private bool m_isTableModified;
  private AutoFiltersCollection m_filters;
  private string m_alternativeText;
  private string m_summary;

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  public IRange Location
  {
    get => this.m_location;
    set
    {
      this.m_location = value;
      if (this.ShowAutoFilter)
        this.m_filters.UpdateRange(value);
      this.AddToNamedRange(this.m_location.Row, this.m_location.Column, this.m_location.LastRow, this.m_location.LastColumn, this.Name + "[All]");
      if (!this.m_bHeaderRow)
        return;
      this.AddToNamedRange(this.Location.Row, this.Location.Column, this.Location.Row, this.Location.LastColumn, this.Name + "[Headers]");
    }
  }

  internal IRange LocalRange
  {
    get => this.m_localRange != null ? this.m_localRange : this.m_location;
    set
    {
      this.m_localRange = value;
      this.AddToNamedRange(this.LocalRange.Row + 1, this.LocalRange.Column, this.LocalRange.LastRow, this.LocalRange.LastColumn, this.Name + "[Data]");
    }
  }

  public IList<IListObjectColumn> Columns => this.m_columns;

  public int Index
  {
    get => this.m_iIndex;
    internal set => this.m_iIndex = value;
  }

  public TableBuiltInStyles BuiltInTableStyle
  {
    get => this.m_builtInStyle;
    set
    {
      this.m_builtInStyle = value;
      this.m_TableStyleName = value.ToString();
    }
  }

  public IWorksheet Worksheet => (IWorksheet) this.m_worksheet;

  public string DisplayName
  {
    get => this.m_strDisplayName;
    set
    {
      if (!(this.m_worksheet.Workbook as WorkbookImpl).Loading)
        this.CheckValidName(value);
      this.m_strDisplayName = value;
    }
  }

  public int TotalsRowCount
  {
    get => this.m_iTotalsRowCount;
    internal set
    {
      this.m_iTotalsRowCount = value;
      if (this.m_iTotalsRowCount == 0)
        return;
      this.AddToNamedRange(this.Location.LastRow, this.Location.Column, this.Location.LastRow, this.Location.LastColumn, this.Name + "[Totals]");
    }
  }

  public bool TotalsRowShown
  {
    get => this.m_bTotalsRowShown;
    set => this.m_bTotalsRowShown = value;
  }

  public QueryTableImpl QueryTable
  {
    get
    {
      if (this.m_queryTable != null && this.m_queryTable.IsDeleted)
      {
        this.m_queryTable = (QueryTableImpl) null;
        this.m_tableType = ExcelTableType.worksheet;
      }
      return this.m_queryTable;
    }
    set => this.m_queryTable = value;
  }

  public string AlternativeText
  {
    get => this.m_alternativeText;
    set => this.m_alternativeText = value;
  }

  public string Summary
  {
    get => this.m_summary;
    set => this.m_summary = value;
  }

  public bool ShowTableStyleRowStripes
  {
    get => this.m_bRowStripes;
    set => this.m_bRowStripes = value;
  }

  public bool ShowTableStyleColumnStripes
  {
    get => this.m_bColumnStripes;
    set => this.m_bColumnStripes = value;
  }

  public int InsertRowShift
  {
    get => this.m_iInsertRowShift;
    set => this.m_iInsertRowShift = value;
  }

  public string TableStyleName
  {
    get => this.m_TableStyleName;
    set
    {
      this.m_TableStyleName = value;
      if (Enum.IsDefined(typeof (TableBuiltInStyles), (object) value))
        this.m_builtInStyle = (TableBuiltInStyles) Enum.Parse(typeof (TableBuiltInStyles), value, false);
      else
        this.m_builtInStyle = TableBuiltInStyles.None;
    }
  }

  internal bool TableModified
  {
    get => this.m_isTableModified;
    set => this.m_isTableModified = value;
  }

  public bool ShowFirstColumn
  {
    get => this.m_bFirstColumn;
    set => this.m_bFirstColumn = value;
  }

  public bool ShowLastColumn
  {
    get => this.m_bLastColumn;
    set => this.m_bLastColumn = value;
  }

  public bool ShowHeaderRow
  {
    get => this.m_bHeaderRow;
    set
    {
      if (this.m_bHeaderRow == value)
        return;
      if (!this.m_worksheet.ParentWorkbook.Loading && this.Location != null)
      {
        if (!value)
        {
          this.Worksheet[this.Location.Row, this.Location.Column, this.Location.Row, this.Location.LastColumn].Text = string.Empty;
          this.Location = this.Worksheet[this.Location.Row + 1, this.Location.Column, this.Location.LastRow, this.Location.LastColumn];
        }
        else
        {
          int num1 = this.Location.Row - 1;
          this.Location = this.Worksheet[num1, this.Location.Column, this.Location.LastRow, this.Location.LastColumn];
          int column = this.Location.Column;
          int index = 0;
          for (int count = this.m_columns.Count; index < count; ++index)
          {
            int num2 = column + index;
            this.Location.Worksheet[num1, num2, num1, num2].Text = this.m_columns[index].Name;
          }
        }
      }
      this.m_bHeaderRow = value;
    }
  }

  public bool ShowTotals
  {
    get => this.m_iTotalsRowCount != 0;
    set
    {
      if (value == this.ShowTotals)
        return;
      IWorksheet worksheet = this.m_location.Worksheet;
      int maxRowCount = worksheet.Workbook.MaxRowCount;
      if (value)
      {
        this.TotalsRowShown = true;
        if (this.m_location.LastRow == maxRowCount)
          return;
        int row = this.m_location.LastRow + 1;
        worksheet[row, this.m_location.Column, maxRowCount - 1, this.m_location.LastColumn].MoveTo(worksheet[row + 1, this.m_location.Column]);
        worksheet[row - 1, this.m_location.Column, row - 1, this.m_location.LastColumn].CopyTo(worksheet[row, this.m_location.Column], ExcelCopyRangeOptions.CopyStyles);
        this.m_location = worksheet[this.m_location.Row, this.m_location.Column, this.m_location.LastRow, this.m_location.LastColumn];
        this.m_iTotalsRowCount = 1;
      }
      else
      {
        int lastRow = this.m_location.LastRow;
        bool flag = false;
        for (int index = 0; index < worksheet.ListObjects.Count; ++index)
        {
          IRange location = worksheet.ListObjects[index].Location;
          flag = (location.Column < this.m_location.Column && location.LastColumn >= this.m_location.Column || location.Column <= this.m_location.LastColumn && location.LastColumn > this.m_location.LastColumn) && this.m_location.LastRow < location.Row;
          if (flag)
            break;
        }
        if (!flag)
          worksheet[lastRow + 1, this.m_location.Column, maxRowCount - 1, this.m_location.LastColumn].MoveTo(worksheet[lastRow, this.m_location.Column]);
        else
          worksheet[this.m_location.LastRow, this.m_location.Column, this.m_location.LastRow, this.m_location.LastColumn].Clear();
        this.m_location = worksheet[this.m_location.Row, this.m_location.Column, this.m_location.LastRow - this.m_iTotalsRowCount, this.m_location.LastColumn];
        this.m_iTotalsRowCount = 0;
      }
    }
  }

  public ExcelTableType TableType
  {
    get => this.m_tableType;
    set => this.m_tableType = value;
  }

  internal int DataAreaFormatId
  {
    get => this.m_iDataDxfId;
    set => this.m_iDataDxfId = value;
  }

  internal int HeaderRowBorderFormatId
  {
    get => this.m_iHeaderRowBorderDxfId;
    set => this.m_iHeaderRowBorderDxfId = value;
  }

  internal int HeaderRowFormatId
  {
    get => this.m_iHeaderRowDxfId;
    set => this.m_iHeaderRowDxfId = value;
  }

  internal int TableBorderFormatId
  {
    get => this.m_iTableBorderDxfId;
    set => this.m_iTableBorderDxfId = value;
  }

  internal int TotalsRowBorderFormatId
  {
    get => this.m_iTotalsRowBorderDxfId;
    set => this.m_iTotalsRowBorderDxfId = value;
  }

  internal int TotalsRowFormatId
  {
    get => this.m_iTotalsRowDxfId;
    set => this.m_iTotalsRowDxfId = value;
  }

  public IAutoFilters AutoFilters
  {
    get
    {
      if (!this.ShowAutoFilter)
      {
        this.m_filters = new AutoFiltersCollection(this.Worksheet.Application, (object) this);
        this.m_filters.UpdateRange(this.m_location);
      }
      return (IAutoFilters) this.m_filters;
    }
  }

  public bool ShowAutoFilter
  {
    get => this.m_filters != null;
    set
    {
      if (this.ShowAutoFilter == value)
        return;
      if (value)
      {
        IAutoFilters autoFilters = this.AutoFilters;
      }
      else
      {
        foreach (IAutoFilter filter in (CollectionBase<object>) this.m_filters)
          (filter as AutoFilterImpl).Clear();
        this.m_filters.Clear();
        this.m_filters = (AutoFiltersCollection) null;
        for (int row1 = this.Location.Row; row1 <= this.Location.LastRow; ++row1)
        {
          IWorksheet worksheet = this.Worksheet;
          RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) this.m_worksheet, row1 - 1, false);
          if (row2 != null)
          {
            row2.m_isFilteredRow = false;
            row2.IsHidden = false;
          }
        }
      }
    }
  }

  public ListObject(string name, IRange location, int index)
  {
    this.m_worksheet = location.Worksheet as WorksheetImpl;
    this.DisplayName = this.Name = name;
    this.m_location = location;
    this.m_iIndex = index;
    this.m_builtInStyle = TableBuiltInStyles.None;
    int row = location.Row;
    IWorksheet worksheet = location.Worksheet;
    this.m_columns = (IList<IListObjectColumn>) new List<IListObjectColumn>();
    int num = 0;
    bool loading = this.m_worksheet.ParentWorkbook.Loading;
    List<string> columnNames = new List<string>();
    if (!loading)
    {
      this.m_filters = new AutoFiltersCollection(this.Worksheet.Application, (object) this);
      this.m_filters.UpdateRange(this.m_location);
    }
    location.UnMerge();
    int column = this.m_location.Column;
    for (int lastColumn = this.m_location.LastColumn; column <= lastColumn; ++column)
    {
      IRange range = worksheet[row, column];
      string name1 = range.CellStyle.IsFirstSymbolApostrophe ? range.DisplayText : range.Text;
      if (name1 == null || name1.Length == 0)
      {
        name1 = !(range.NumberFormat != "General") ? (range.Value == null || !(range.Value != string.Empty) ? "Column" + (++num).ToString() : range.Value.ToString()) : range.DisplayText;
        if (!loading)
          range.Text = name1;
      }
      columnNames.Add(name1);
      this.m_columns.Add((IListObjectColumn) new ListObjectColumn(name1, this.m_columns.Count + 1, this, column));
    }
    if (loading)
      return;
    this.UpdateColumnNames(columnNames);
  }

  internal ListObject Clone(WorksheetImpl worksheet, Dictionary<string, string> hashWorksheetNames)
  {
    ListObject listObject = (ListObject) this.MemberwiseClone();
    WorkbookImpl parentWorkbook = worksheet.ParentWorkbook;
    listObject.m_location = (this.m_location as ICombinedRange).Clone((object) worksheet, hashWorksheetNames, parentWorkbook);
    listObject.m_worksheet = worksheet;
    if (this.m_columns != null)
    {
      int count = this.m_columns.Count;
      listObject.m_columns = (IList<IListObjectColumn>) new List<IListObjectColumn>(count);
      for (int index = 0; index < count; ++index)
      {
        ListObjectColumn column = (ListObjectColumn) this.m_columns[index];
        listObject.m_columns.Add((IListObjectColumn) column.Clone(listObject));
      }
    }
    if (this.m_filters != null)
      listObject.m_filters = this.m_filters.Clone((IListObject) listObject);
    listObject.Name = (string) null;
    listObject.m_strDisplayName = Area3DPtg.ValidateSheetName(this.GenerateUniqueName(parentWorkbook, this.Name)) ? (listObject.Name = this.GenerateUniqueName(parentWorkbook, "table")) : (listObject.Name = this.GenerateUniqueName(parentWorkbook, this.Name));
    if (listObject.QueryTable != null)
    {
      QueryTableImpl queryTable = listObject.QueryTable;
      string stringPart;
      int numberPart;
      this.SplitName(queryTable.ExternalConnection.Name, out stringPart, out numberPart);
      ++numberPart;
      string str;
      for (str = stringPart + (object) numberPart; this.Checkconn_name(parentWorkbook.Connections, str) || this.Checkconn_name(parentWorkbook.DeletedConnections, str); str = stringPart + (object) numberPart)
        ++numberPart;
      listObject.QueryTable = queryTable.Clone(listObject, parentWorkbook, str);
    }
    return listObject;
  }

  private bool Checkconn_name(IConnections connections, string Name)
  {
    foreach (IConnection connection in (IEnumerable<IConnection>) connections)
    {
      if (connection.Name == Name)
        return true;
    }
    return false;
  }

  private string GenerateUniqueName(WorkbookImpl book, string proposedName)
  {
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (string.IsNullOrEmpty(proposedName))
      throw new ArgumentOutOfRangeException(nameof (proposedName));
    if (this.ListObjectNameExist(book, proposedName))
    {
      string stringPart;
      int numberPart;
      this.SplitName(proposedName, out stringPart, out numberPart);
      int num = numberPart + 1;
      for (proposedName = stringPart + (object) num; this.ListObjectNameExist(book, proposedName); proposedName = stringPart + (object) num)
        ++num;
    }
    return proposedName;
  }

  internal void SplitName(string proposedName, out string stringPart, out int numberPart)
  {
    stringPart = string.Empty;
    numberPart = 0;
    int length = proposedName.Length;
    while (length > 0 && char.IsDigit(proposedName[length - 1]))
      --length;
    if (length > 0)
      stringPart = proposedName.Substring(0, length);
    if (length >= proposedName.Length)
      return;
    string s = proposedName.Substring(length);
    numberPart = int.Parse(s);
  }

  internal bool ListObjectNameExist(WorkbookImpl book, string name)
  {
    IWorksheets worksheets = book.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
    {
      ListObjectCollection innerListObjects = ((WorksheetImpl) worksheets[Index]).InnerListObjects;
      if (innerListObjects != null && innerListObjects.Count != 0 && innerListObjects[name] != null)
        return true;
    }
    return false;
  }

  public void Refresh()
  {
    string oldValue1 = "_x000d_";
    string oldValue2 = "_x000a_";
    string oldValue3 = "_x0009_";
    ExternalConnection connection = this.QueryTable != null ? this.QueryTable.ExternalConnection : throw new ArgumentException("Query Table Not Exist");
    ConnectionPassword connectionPassword = new ConnectionPassword();
    connection.RaiseEvent((object) this, connectionPassword);
    bool isRefresh = true;
    if (connection.DataBaseType == ExcelConnectionsType.ConnectionTypeOLEDB)
    {
      OleDbConnection oleDbConnection = new OleDbConnection();
      string str1 = connection.DBConnectionString;
      if (connectionPassword.PasswordToConnectDB != null)
        str1 = str1.Insert(str1.LastIndexOf(";") + 1, $"password={connectionPassword.PasswordToConnectDB};");
      oleDbConnection.ConnectionString = str1;
      string upper = this.QueryTable.CommandText.ToString().ToUpper();
      upper.Replace(oldValue1, "");
      upper.Replace(oldValue2, "");
      upper.Replace(oldValue3, "");
      OleDbCommand selectCommand = new OleDbCommand();
      selectCommand.Connection = oleDbConnection;
      string Query = ListObject.UpdateQuery(connection, upper, isRefresh);
      selectCommand.CommandText = Query;
      if (this.checkCommandText(Query) && this.QueryTable.CommandType != ExcelCommandType.Sql)
      {
        string str2 = !Path.GetExtension(connection.SourceFile).ToLower().Contains("xls") ? "SELECT * FROM " + Query : $"SELECT * FROM [{Query}]";
        selectCommand.Parameters.AddWithValue("@Query", (object) str2);
        selectCommand.CommandText = "@Query";
      }
      DataTable dataTable = new DataTable();
      OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand);
      oleDbDataAdapter.Fill(dataTable);
      if (dataTable.Columns.Count != 0)
        this.FillTableData(dataTable);
      oleDbConnection.Dispose();
      dataTable.Dispose();
      oleDbDataAdapter.Dispose();
      selectCommand.Dispose();
    }
    else
    {
      if (connection.DataBaseType != ExcelConnectionsType.ConnectionTypeODBC)
        return;
      string query = this.QueryTable.CommandText.ToString();
      if (query.Contains(oldValue1))
        query = query.Replace(oldValue1, " ");
      if (query.Contains(oldValue2))
        query = query.Replace(oldValue2, " ");
      if (query.Contains(oldValue3))
        query = query.Replace(oldValue3, " ");
      OdbcCommand selectCommand = new OdbcCommand();
      DataTable dataTable = new DataTable();
      if (this.QueryTable.CommandType != ExcelCommandType.Sql)
        throw new ArgumentException("Command should be SQl for ODBC connection");
      OdbcConnection odbcConnection = new OdbcConnection();
      string str3 = connection.DBConnectionString;
      if (connectionPassword.PasswordToConnectDB != null)
        str3 = str3.Insert(str3.LastIndexOf(";") + 1, $"password={connectionPassword.PasswordToConnectDB};");
      odbcConnection.ConnectionString = str3;
      selectCommand.Connection = odbcConnection;
      string str4 = ListObject.UpdateQuery(connection, query, isRefresh);
      selectCommand.CommandText = str4;
      if (this.QueryTable.CommandType == ExcelCommandType.Sql && this.checkCommandText(str4))
        selectCommand.CommandType = CommandType.StoredProcedure;
      OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(selectCommand);
      if (isRefresh)
      {
        try
        {
          odbcDataAdapter.Fill(dataTable);
          if (dataTable.Columns.Count == 0)
            this.SqlConnection(connection, str4, connectionPassword);
          else
            this.FillTableData(dataTable);
        }
        catch
        {
          this.SqlConnection(connection, str4, connectionPassword);
        }
      }
      odbcDataAdapter.Dispose();
      odbcConnection.Dispose();
      dataTable.Dispose();
      selectCommand.Dispose();
    }
  }

  private static string UpdateQuery(ExternalConnection connection, string query, bool isRefresh)
  {
    MatchCollection matchCollection = new Regex("(\\?)|('[^']*\\?[^']*')").Matches(query);
    List<int> intList = new List<int>();
    foreach (Match match in matchCollection)
    {
      if (!match.Value.Contains("'"))
        intList.Add(match.Index);
    }
    if (intList.Count > 0 && intList.Count >= connection.Parameters.Count)
    {
      int num = 0;
      for (int index = 0; index < intList.Count; ++index)
      {
        ParameterImpl parameter = (ParameterImpl) connection.Parameters[index];
        switch (parameter.Type)
        {
          case ExcelParameterType.Prompt:
            object obj;
            if (parameter.RaiseEvent(out obj))
            {
              if (obj != null)
              {
                string str = $"'{obj.ToString()}'";
                query = query.Remove(intList[index] + num, 1).Insert(intList[index] + num, str);
                num += str.Length - 1;
                break;
              }
              break;
            }
            isRefresh = false;
            break;
          case ExcelParameterType.Constant:
            if (parameter.Value != null)
            {
              string str = $"'{parameter.Value.ToString()}'";
              query = query.Remove(intList[index] + num, 1).Insert(intList[index] + num, str);
              num += str.Length - 1;
              break;
            }
            isRefresh = false;
            break;
          case ExcelParameterType.Range:
            if (parameter.SourceRange != null)
            {
              IWorksheet worksheet = parameter.SourceRange.Worksheet;
              bool flag = false;
              if (worksheet.CalcEngine == null)
              {
                worksheet.EnableSheetCalculations();
                flag = true;
              }
              string str = $"'{parameter.SourceRange.CalculatedValue}'";
              query = query.Remove(intList[index] + num, 1).Insert(intList[index] + num, str);
              num += str.Length - 1;
              if (flag)
              {
                worksheet.DisableSheetCalculations();
                break;
              }
              break;
            }
            isRefresh = false;
            break;
        }
      }
    }
    return query;
  }

  private void SqlConnection(
    ExternalConnection connection,
    string query,
    ConnectionPassword password)
  {
    System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection();
    string[] strArray = connection.ConnectionString.ToString().Split(new string[1]
    {
      ";"
    }, StringSplitOptions.RemoveEmptyEntries);
    string str1 = string.Empty;
    string str2 = (string) null;
    string str3 = (string) null;
    string str4 = (string) null;
    string str5 = (string) null;
    string str6 = (string) null;
    string s = (string) null;
    foreach (string str7 in strArray)
    {
      if (!string.IsNullOrEmpty(str7))
      {
        int length = str7.IndexOf("=");
        switch (str7.Substring(0, length).ToUpper())
        {
          case "WSID":
            str3 = str7.Substring(length + 1);
            continue;
          case "DATABASE":
          case "INITIAL CATALOG":
            str5 = str7.Substring(length + 1);
            continue;
          case "TRUSTED_CONNECTION":
            str6 = str7.Substring(length + 1);
            if (str6.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
              str1 += "Integrated Security=True;";
              continue;
            }
            if (password.PasswordToConnectDB != null)
            {
              str2 = $"password={password.PasswordToConnectDB};";
              continue;
            }
            continue;
          case "UID":
            str1 = $"{str1}UID={str7.Substring(length + 1)};";
            continue;
          case "APP":
            str1 = $"{str1}APP={str7.Substring(length + 1)};";
            continue;
          case "PASSWORD":
          case "PWD":
            str2 = $"Password={str7.Substring(length + 1)};";
            continue;
          case "DSN":
            string database;
            str4 = this.GetServerNameFromRegistryKey(str7.Substring(length + 1), out database);
            if (string.IsNullOrEmpty(str5) && !string.IsNullOrEmpty(database))
            {
              str5 = database;
              continue;
            }
            continue;
          case "DATA SOURCE":
            str1 = $"{str1}Data Source={str7.Substring(length + 1)};";
            continue;
          case "CONNECTION TIMEOUT":
            s = str7.Substring(length + 1);
            str1 = $"{str1}Connection Timeout={s};";
            continue;
          default:
            continue;
        }
      }
    }
    if (str2 != null)
      str1 += str2;
    string str8 = (str4 == null || !(str4 != "") ? $"Data Source={str3};" : $"Data Source={str4};") + str1;
    if (!string.IsNullOrEmpty(str5))
      str8 = $"{str8}Initial Catalog={str5};";
    if (str6 == null && str2 == null)
      str8 += "Integrated Security=True;";
    sqlConnection.ConnectionString = str8;
    sqlConnection.Open();
    SqlCommand selectCommand = new SqlCommand(query);
    selectCommand.Connection = sqlConnection;
    int result;
    if (!string.IsNullOrEmpty(s) && int.TryParse(s, out result))
      selectCommand.CommandTimeout = result;
    DataTable dataTable = new DataTable();
    new SqlDataAdapter(selectCommand).Fill(dataTable);
    if (dataTable.Columns.Count == 0)
      return;
    this.FillTableData(dataTable);
  }

  private string GetServerNameFromRegistryKey(string DSN, out string database)
  {
    string server = "";
    database = "";
    RegistryKey localMachine = Registry.LocalMachine;
    RegistryKey users = Registry.Users;
    RegistryKey currentUser = Registry.CurrentUser;
    if (this.TryGetNameFromRegistryKey(DSN, currentUser, (string) null, out server, out database) || this.TryGetNameFromRegistryKey(DSN, localMachine, (string) null, out server, out database))
      return server;
    string[] subKeyNames = users.GetSubKeyNames();
    if (subKeyNames != null && subKeyNames.Length > 0)
    {
      foreach (string str in subKeyNames)
      {
        if (this.TryGetNameFromRegistryKey(DSN, users, (string) null, out server, out database))
          return server;
      }
    }
    return server;
  }

  private bool TryGetNameFromRegistryKey(
    string DSN,
    RegistryKey registryKey,
    string subKey,
    out string server,
    out string database)
  {
    server = "";
    database = "";
    RegistryKey registryKey1 = !string.IsNullOrEmpty(subKey) ? registryKey.OpenSubKey($"SOFTWARE\\ODBC\\odbc.ini\\{DSN}") : registryKey.OpenSubKey(string.Format(subKey + "SOFTWARE\\ODBC\\odbc.ini\\{0}", (object) DSN));
    if (registryKey1 != null)
    {
      object obj1 = registryKey1.GetValue("Server");
      if (obj1 != null)
      {
        server = obj1.ToString();
        object obj2 = registryKey1.GetValue("Database");
        if (obj2 != null)
          database = obj2.ToString();
        return true;
      }
    }
    return false;
  }

  private bool checkCommandText(string Query)
  {
    string[] strArray = new string[5]
    {
      "SELECT",
      "UPDATE",
      "DELETE",
      "INSERT",
      "ALTER"
    };
    foreach (string str in strArray)
    {
      if (Query.Contains(str))
        return false;
    }
    return true;
  }

  private void UpdateSqlProperties(DataTable Table)
  {
    int row1 = this.LocalRange.Row;
    int lastRow1 = this.LocalRange.LastRow;
    int column = this.LocalRange.Column;
    int lastColumn = this.LocalRange.LastColumn;
    int num1 = this.ShowHeaderRow ? 1 : 0;
    if (this.QueryTable.GrowShrinkType == GrowShrinkType.insertDelete)
    {
      int num2 = lastRow1 + (Table.Rows.Count - (lastRow1 - row1)) + num1;
      this.Worksheet.InsertRow(lastRow1 + 1, Table.Rows.Count - (lastRow1 - row1 + (this.ShowHeaderRow ? 0 : 1)), ExcelInsertOptions.FormatAsBefore);
      IRange range1 = this.Worksheet[lastRow1 + 1, column, num2 - 1, lastColumn];
      IRange range2 = this.Worksheet[num2, column, num2, lastColumn];
      int lastRow2 = range1.LastRow;
      int row2 = range1.Row;
      int lastRow3 = this.Worksheet.UsedRange.LastRow;
      if (this.Worksheet.ListObjects != null)
      {
        for (int index = 0; index < this.Worksheet.ListObjects.Count; ++index)
        {
          IRange location = this.Worksheet.ListObjects[index].Location;
          if (location.Row >= range1.Row && location.LastColumn <= this.Location.LastColumn && location.Column >= this.Location.Column)
          {
            ListObject listObject = this.Worksheet.ListObjects[index] as ListObject;
            WorkbookImpl workbook = this.Worksheet.Workbook as WorkbookImpl;
            if (listObject.QueryTable != null)
              (workbook.Names[listObject.QueryTable.Name] != null ? workbook.Names[listObject.QueryTable.Name] : this.Worksheet.Names[listObject.QueryTable.Name]).RefersToRange = listObject.Location;
          }
        }
      }
      for (int index = column; index <= lastColumn; ++index)
        this.Worksheet[row1 + 2, index, num2 - num1, index].NumberFormat = this.Worksheet[row1 + 1, index].NumberFormat;
    }
    else
    {
      if (this.QueryTable.GrowShrinkType != GrowShrinkType.insertClear)
        return;
      int num3 = lastRow1 + (Table.Rows.Count - (lastRow1 - row1)) + 1;
      IRange range3 = this.Worksheet[lastRow1 + 1, column, num3 - 1, lastColumn];
      IRange range4 = range3.Trim();
      int iRowCount = 0;
      for (int index = 0; index < range3.Rows.Length; ++index)
      {
        if (range4.Row == range3.Rows[index].Row)
        {
          iRowCount = range3.Rows.Length - index;
          break;
        }
      }
      if (iRowCount <= 0)
        return;
      this.Worksheet.InsertRow(range3.Row, iRowCount);
    }
  }

  private void FillTableData(DataTable Table)
  {
    this.Location.Clear();
    int row = this.Location.Row;
    int column = this.Location.Column;
    int count1 = Table.Rows.Count;
    int count2 = Table.Columns.Count;
    List<QueryTableField> queryFields = this.QueryTable.QueryTableRefresh.QueryFields;
    int lastRow = !this.ShowHeaderRow ? row + count1 - 1 : row + count1;
    int lastColumn = column + count2 - 1;
    if (this.Location.LastRow < lastRow && this.QueryTable.GrowShrinkType == GrowShrinkType.overwriteClear)
      ListObjectCollection.CheckOverLab(this.Worksheet[this.Location.LastRow + 1, column, lastRow, lastColumn]);
    if (this.Location.LastColumn < lastColumn && this.QueryTable.GrowShrinkType == GrowShrinkType.overwriteClear)
      ListObjectCollection.CheckOverLab(this.Worksheet[this.Location.Row, this.Location.LastColumn + 1, lastRow, lastColumn]);
    bool flag1 = this.QueryTable.QueryTableRefresh.QueryFields.Count == 0;
    bool flag2 = !flag1 && this.QueryTable.QueryTableRefresh.PreserveSortFilterLayout;
    int count3;
    if (flag2)
    {
      queryFields.Clear();
      count3 = this.Columns.Count;
    }
    else
    {
      this.Columns.Clear();
      queryFields.Clear();
      count3 = Table.Columns.Count;
    }
    List<string> stringList = new List<string>();
    if (flag2 && this.Columns.Count > 0 && count3 <= this.Columns.Count)
    {
      for (int index = 0; index < count3; ++index)
        stringList.Add(this.Columns[index].Name.ToString());
    }
    for (int index = 0; index < count3; ++index)
    {
      object obj = !flag2 || stringList.Count <= 0 ? (object) Table.Columns[index] : (object) stringList[index];
      if (this.ShowHeaderRow)
        this.Worksheet.SetText(row, column, obj.ToString());
      ListObjectColumn listObjectColumn = new ListObjectColumn(obj.ToString(), index + 1, this, index + 1);
      if (flag2)
      {
        listObjectColumn.QueryTableFieldId = index + 1;
        this.Columns[index] = (IListObjectColumn) listObjectColumn;
      }
      else
      {
        this.Columns.Add((IListObjectColumn) listObjectColumn);
        listObjectColumn.QueryTableFieldId = index + 1;
      }
      queryFields.Add(new QueryTableField(listObjectColumn.QueryTableFieldId, listObjectColumn.Id, this.QueryTable.QueryTableRefresh));
      queryFields[index].Name = listObjectColumn.Name;
      ++column;
    }
    if (!this.ShowHeaderRow)
      --row;
    int num = this.ShowHeaderRow ? 1 : 0;
    if (!flag1 && this.Location.Rows.Length < Table.Rows.Count + num)
      this.UpdateSqlProperties(Table);
    for (int index1 = 0; index1 < Table.Rows.Count; ++index1)
    {
      ++row;
      column = this.Location.Column;
      for (int index2 = 0; index2 < count3; ++index2)
      {
        object obj = !flag2 || stringList.Count <= 0 ? Table.Rows[index1][index2] : Table.Rows[index1][stringList[index2]];
        if (obj != DBNull.Value)
        {
          switch (!flag2 || stringList.Count <= 0 ? Table.Columns[index2].DataType.Name : Table.Columns[stringList[index2]].DataType.Name)
          {
            case "String":
              this.Worksheet.SetText(row, column, obj.ToString());
              break;
            case "DateTime":
              this.Worksheet[row, column].Value2 = obj;
              break;
            case "Double":
              this.Worksheet.SetNumber(row, column, (double) obj);
              break;
            case "Single":
              this.Worksheet.SetNumber(row, column, (double) Convert.ToSingle(obj));
              break;
            case "Int16":
              this.Worksheet.SetNumber(row, column, (double) Convert.ToInt16(obj));
              break;
            case "Int32":
              this.Worksheet.SetNumber(row, column, (double) (int) obj);
              break;
            case "Int64":
              this.Worksheet.SetNumber(row, column, (double) Convert.ToInt64(obj));
              break;
            case "Boolean":
              this.Worksheet.SetBoolean(row, column, Convert.ToBoolean(obj));
              break;
            case "Byte":
              this.Worksheet.SetNumber(row, column, (double) Convert.ToByte(obj));
              break;
            case "Decimal":
              this.Worksheet.SetNumber(row, column, Convert.ToDouble(obj));
              break;
            default:
              this.Worksheet.SetValue(row, column, obj.ToString());
              break;
          }
        }
        ++column;
      }
    }
    this.Location = Table.Rows.Count != 0 ? this.Worksheet[this.Location.Row, this.Location.Column, row, column - 1] : this.Worksheet[this.Location.Row, this.Location.Column, row + 1, column - 1];
    WorkbookImpl workbook = this.Worksheet.Workbook as WorkbookImpl;
    (workbook.Names[this.QueryTable.Name] != null ? workbook.Names[this.QueryTable.Name] : this.Worksheet.Names[this.QueryTable.Name]).RefersToRange = this.Location;
    row.ToString();
    column.ToString();
  }

  internal void Dispose()
  {
    if (this.QueryTable != null)
    {
      if (this.QueryTable.ExternalConnection != null)
        this.QueryTable.ExternalConnection.Dispose();
      this.QueryTable = (QueryTableImpl) null;
    }
    if (this.m_filters == null)
      return;
    this.m_filters.Clear();
  }

  private void AddToNamedRange(
    int startRow,
    int startColumn,
    int endRow,
    int endColumn,
    string name)
  {
    WorkbookImpl workbook = (this.Location.Worksheet as WorksheetImpl).Workbook as WorkbookImpl;
    NameImpl nameImpl = workbook.InnerNamesColection.Add(name) as NameImpl;
    nameImpl.SetValue(workbook.FormulaUtil.ParseString(this.Worksheet[startRow, startColumn, endRow, endColumn].AddressGlobal));
    nameImpl.RefersToRange = this.Worksheet[startRow, startColumn, endRow, endColumn];
    nameImpl.SheetIndex = this.Worksheet.Index;
    nameImpl.Visible = false;
    nameImpl.m_isTableNamedRange = true;
    if (!name.Contains("[All]") || workbook.Loading)
      return;
    this.checkAndCloneNameRange(name, workbook);
  }

  internal void checkAndCloneNameRange(string name, WorkbookImpl book)
  {
    string name1 = name.Replace("[All]", "");
    IName name2 = book.InnerNamesColection[name];
    NameImpl nameImpl = book.InnerNamesColection.Add(name1) as NameImpl;
    IRange refersToRange = name2.RefersToRange;
    int row = refersToRange.Row;
    int lastRow = refersToRange.LastRow;
    if (this.ShowHeaderRow)
      ++row;
    if (this.TotalsRowShown)
      --lastRow;
    if (row > lastRow)
    {
      row = refersToRange.Row;
      lastRow = refersToRange.LastRow;
    }
    IRange range = refersToRange.Worksheet[row, refersToRange.Column, lastRow, refersToRange.LastColumn];
    nameImpl.SetValue(book.FormulaUtil.ParseString(range.AddressGlobal));
    nameImpl.RefersToRange = range;
    nameImpl.SheetIndex = this.Worksheet.Index;
    nameImpl.Visible = false;
    nameImpl.m_isTableNamedRange = true;
  }

  internal void UpdateColumnNames(List<string> columnNames)
  {
    int num = 2;
    int index = 0;
    bool flag = true;
    string str1 = string.Empty;
    string str2 = string.Empty;
    while (index < this.m_columns.Count)
    {
      if (flag)
      {
        str2 = str1 = columnNames[index];
        columnNames.RemoveAt(index);
      }
      if ((flag ? (columnNames.IndexOf(str1) < 0 ? 0 : (columnNames.IndexOf(str1) < index ? 1 : 0)) : (columnNames.Contains(str1) ? 1 : 0)) != 0)
      {
        str1 = str2 + num.ToString();
        ++num;
        flag = false;
      }
      else
      {
        flag = true;
        (this.m_columns[index] as ListObjectColumn).SetName(str1);
        columnNames.Insert(index, str1);
        ++index;
      }
    }
  }

  private void CheckValidName(string name)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException(nameof (name));
      default:
        if (name.Length > (int) byte.MaxValue)
          throw new ArgumentException("Name should not be more than 255 characters length.");
        if (char.IsDigit(name[0]))
          throw new ArgumentException("This is not a valid name. Name should start with letter or underscore.");
        char[] anyOf = new char[23]
        {
          '~',
          '!',
          '@',
          '#',
          '$',
          '%',
          '^',
          '&',
          '*',
          '(',
          ')',
          '+',
          '-',
          '{',
          '}',
          '[',
          ']',
          ':',
          ';',
          '<',
          '>',
          ',',
          ' '
        };
        if (name.IndexOfAny(anyOf) != -1)
          throw new ArgumentException("This is not a valid name. Name should not contain space or characters not allowed.");
        if (!WorkbookNamesCollection.IsValidName(name, this.m_worksheet.Workbook as WorkbookImpl))
          throw new ArgumentException("This is not a valid name. Name should not be same as the cell name.");
        if (!(this.m_worksheet.Workbook.Names as WorkbookNamesCollection).IsNameExist(name))
          break;
        throw new ArgumentException("Name already exist.Name must be unique");
    }
  }
}
