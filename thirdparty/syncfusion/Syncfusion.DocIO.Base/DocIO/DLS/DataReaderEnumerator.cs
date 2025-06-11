// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DataReaderEnumerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class DataReaderEnumerator : IRowsEnumerator
{
  private IDataReader m_dataReader;
  private List<List<string>> m_rows;
  private int m_currRowIndex = -1;
  private string[] m_columnNames;

  public int CurrentRowIndex => this.m_currRowIndex;

  public int RowsCount => this.m_rows.Count;

  public string TableName => this.m_dataReader.GetSchemaTable().TableName;

  public bool IsEnd => this.m_currRowIndex >= this.RowsCount;

  public bool IsLast => this.m_currRowIndex >= this.RowsCount - 1;

  protected List<string> CurrentRow
  {
    get
    {
      if (this.m_currRowIndex >= this.RowsCount)
        return (List<string>) null;
      return this.m_rows == null ? (List<string>) null : this.m_rows[this.m_currRowIndex];
    }
  }

  public string[] ColumnNames => this.m_columnNames;

  public DataReaderEnumerator(IDataReader dataReader)
  {
    this.m_dataReader = dataReader;
    this.m_rows = new List<List<string>>();
    this.m_columnNames = new string[this.m_dataReader.FieldCount];
    for (int i = 0; i < this.m_dataReader.FieldCount; ++i)
      this.m_columnNames[i] = this.m_dataReader.GetName(i);
    while (this.m_dataReader.Read())
    {
      List<string> stringList = new List<string>();
      for (int i = 0; i < this.m_dataReader.FieldCount; ++i)
        stringList.Add(this.m_dataReader[i].ToString());
      this.m_rows.Add(stringList);
    }
  }

  public void Reset() => this.m_currRowIndex = -1;

  public bool NextRow()
  {
    if (this.m_currRowIndex < this.RowsCount)
      ++this.m_currRowIndex;
    return !this.IsEnd;
  }

  public object GetCellValue(string columnName)
  {
    List<string> currentRow = this.CurrentRow;
    return currentRow == null ? (object) null : (object) currentRow[this.m_dataReader.GetOrdinal(columnName)];
  }

  internal void Close()
  {
    if (this.m_dataReader != null)
    {
      this.m_dataReader.Dispose();
      this.m_dataReader = (IDataReader) null;
    }
    if (this.m_rows != null)
    {
      for (int index = 0; index < this.m_rows.Count; ++index)
      {
        this.m_rows[index].Clear();
        this.m_rows[index] = (List<string>) null;
      }
      this.m_rows.Clear();
      this.m_rows = (List<List<string>>) null;
    }
    if (this.m_columnNames == null)
      return;
    this.m_columnNames = (string[]) null;
  }
}
