// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DataTableEnumerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class DataTableEnumerator : IRowsEnumerator
{
  private DataTable m_table;
  private DataRow m_row;
  private int m_currRowIndex = -1;
  private string[] m_columnsNames;
  private int m_matchingRecordsCount;
  private string m_command;
  private string m_tableName;
  internal IEnumerator m_MMtable;
  private Type m_userClassType;
  private int m_rowCount;

  public int CurrentRowIndex => this.m_currRowIndex;

  public int RowsCount
  {
    get
    {
      if (this.m_table != null)
        return this.m_table.Rows.Count;
      return this.m_rowCount == 0 && this.m_row != null && this.m_row.ItemArray != null && this.m_row.ItemArray.Length > 0 ? 1 : this.m_rowCount;
    }
  }

  public string TableName => this.m_table != null ? this.m_table.TableName : this.m_tableName;

  public bool IsEnd => this.m_currRowIndex >= this.RowsCount;

  public bool IsLast => this.m_currRowIndex >= this.RowsCount - 1;

  protected object CurrentRow
  {
    get
    {
      if (this.m_currRowIndex >= this.RowsCount)
        return (object) null;
      if (this.m_MMtable != null)
      {
        this.m_MMtable.Reset();
        for (int index = 0; index <= this.m_currRowIndex; ++index)
          this.m_MMtable.MoveNext();
        return this.m_MMtable.Current;
      }
      return this.m_table != null ? (object) this.m_table.Rows[this.m_currRowIndex] : (object) this.m_row;
    }
  }

  internal int MatchingRecordsCount
  {
    get => this.m_matchingRecordsCount;
    set => this.m_matchingRecordsCount = value;
  }

  internal string Command
  {
    get => this.m_command;
    set => this.m_command = value;
  }

  public DataTableEnumerator(DataTable table)
  {
    this.m_table = table;
    this.ReadColumnNames(this.m_table);
  }

  public DataTableEnumerator(DataRow row)
  {
    this.m_row = row;
    this.ReadColumnNames(row.Table);
  }

  public void Reset() => this.m_currRowIndex = -1;

  public bool NextRow()
  {
    if (this.m_currRowIndex < this.RowsCount)
      ++this.m_currRowIndex;
    return !this.IsEnd;
  }

  internal bool NextRow(string[] command)
  {
    if (this.m_currRowIndex < this.RowsCount)
    {
      ++this.m_currRowIndex;
      if (command.Length > 2)
      {
        while (this.m_currRowIndex < this.RowsCount && this.CurrentRow != null)
        {
          this.CurrentRow.GetType();
          if (this.CurrentRow is IDictionary<string, object>)
          {
            if (!(this.CurrentRow as IDictionary<string, object>).ContainsKey(command[0]) || !((this.CurrentRow as IDictionary<string, object>)[command[0]].ToString() == command[2]) && (!(command[1].ToLower() == "contains") || !(this.CurrentRow as IDictionary<string, object>)[command[0]].ToString().Contains(command[2])))
            {
              if (this.m_currRowIndex == this.RowsCount - 1)
              {
                ++this.m_currRowIndex;
                return true;
              }
              ++this.m_currRowIndex;
            }
            else
              break;
          }
          else
          {
            PropertyInfo[] properties = this.CurrentRow.GetType().GetProperties();
            int index = 0;
            for (int length = properties.Length; index < length; ++index)
            {
              PropertyInfo propertyInfo = properties[index];
              if (propertyInfo.Name == command[0] && propertyInfo.GetValue(this.CurrentRow, (object[]) null).ToString() == command[2])
                return !this.IsEnd;
            }
            if (this.m_currRowIndex == this.RowsCount - 1)
            {
              ++this.m_currRowIndex;
              return true;
            }
            ++this.m_currRowIndex;
          }
        }
      }
    }
    return !this.IsEnd;
  }

  public object GetCellValue(string columnName)
  {
    if (this.CurrentRow is DataRow)
      return (this.CurrentRow as DataRow)[columnName];
    if (this.CurrentRow != null)
    {
      this.CurrentRow.GetType();
      if (this.CurrentRow is IDictionary<string, object>)
        return (this.CurrentRow as IDictionary<string, object>).ContainsKey(columnName) ? (this.CurrentRow as IDictionary<string, object>)[columnName] : (object) string.Empty;
      PropertyInfo[] properties = this.m_userClassType.GetProperties();
      int index = 0;
      for (int length = properties.Length; index < length; ++index)
      {
        PropertyInfo propertyInfo = properties[index];
        if (propertyInfo.Name == columnName)
          return propertyInfo.GetValue(this.CurrentRow, (object[]) null);
      }
    }
    return (object) null;
  }

  public string[] ColumnNames => this.m_columnsNames;

  internal void Close()
  {
    if (this.m_table != null)
    {
      this.m_table.Dispose();
      this.m_table = (DataTable) null;
    }
    if (this.m_row != null)
      this.m_row = (DataRow) null;
    if (this.m_columnsNames == null)
      return;
    this.m_columnsNames = (string[]) null;
  }

  private void ReadColumnNames(DataTable table)
  {
    this.m_columnsNames = new string[table.Columns.Count];
    for (int index = 0; index < this.m_columnsNames.Length; ++index)
      this.m_columnsNames[index] = table.Columns[index].ColumnName;
  }

  public DataTableEnumerator(MailMergeDataTable table)
  {
    this.m_tableName = table.GroupName;
    table.SourceData.Reset();
    table.SourceData.MoveNext();
    this.MatchingRecordsCount = table.MatchingRecordsCount;
    this.Command = table.Command;
    this.m_MMtable = table.SourceData;
    try
    {
      this.m_userClassType = this.m_MMtable.Current.GetType();
      if (this.m_MMtable.Current is IDictionary<string, object>)
        this.ReadColumnNames(table);
      else if (this.m_MMtable.Current is DataRow)
        this.ReadColumnNamesInTable(this.m_MMtable);
      else
        this.ReadColumnNames(this.m_MMtable);
    }
    catch
    {
      this.m_userClassType = (Type) null;
      this.m_columnsNames = (string[]) null;
    }
    this.CalculRowCount();
  }

  private void ReadColumnNames(MailMergeDataTable table)
  {
    List<string> stringList = new List<string>();
    table.SourceData.Reset();
    while (table.SourceData.MoveNext())
    {
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) (table.SourceData.Current as IDictionary<string, object>))
      {
        if (!stringList.Contains(keyValuePair.Key))
          stringList.Add(keyValuePair.Key);
      }
    }
    this.m_columnsNames = stringList.ToArray();
  }

  private void ReadColumnNamesInTable(IEnumerator table)
  {
    List<string> stringList = new List<string>();
    if (table.Current is DataRow current)
    {
      DataTable table1 = current.Table;
      for (int index = 0; index < table1.Columns.Count; ++index)
        stringList.Add(table1.Columns[index].ColumnName);
    }
    this.m_columnsNames = stringList.ToArray();
  }

  private void ReadColumnNames(IEnumerator table)
  {
    List<string> stringList = new List<string>();
    PropertyInfo[] properties = this.m_userClassType.GetProperties();
    int index = 0;
    for (int length = properties.Length; index < length; ++index)
      stringList.Add(properties[index].Name);
    this.m_columnsNames = stringList.ToArray();
  }

  private void CalculRowCount()
  {
    this.m_MMtable.Reset();
    while (this.m_MMtable.MoveNext())
      ++this.m_rowCount;
    this.m_MMtable.Reset();
  }
}
