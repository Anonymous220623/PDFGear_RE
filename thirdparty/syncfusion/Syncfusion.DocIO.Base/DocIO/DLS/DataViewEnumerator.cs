// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DataViewEnumerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Data;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class DataViewEnumerator : IRowsEnumerator
{
  private DataView m_dataView;
  private DataTable m_table;
  private DataRow m_row;
  private int m_currRowIndex = -1;
  private string[] m_columnNames;

  public DataViewEnumerator(DataView dataView)
  {
    this.m_dataView = dataView;
    this.m_table = dataView.Table;
    this.ReadColumnNames(this.m_table);
  }

  public int CurrentRowIndex => this.m_currRowIndex;

  public int RowsCount => this.m_dataView == null ? 1 : this.m_dataView.Count;

  public string TableName => this.m_dataView == null ? "" : this.m_dataView.Table.TableName;

  public bool IsEnd => this.m_currRowIndex >= this.RowsCount;

  public bool IsLast => this.m_currRowIndex >= this.RowsCount - 1;

  protected DataRow CurrentRow
  {
    get
    {
      if (this.m_currRowIndex >= this.RowsCount)
        return (DataRow) null;
      return this.m_dataView == null ? this.m_row : this.m_dataView[this.m_currRowIndex].Row;
    }
  }

  public void Reset() => this.m_currRowIndex = -1;

  public bool NextRow()
  {
    if (this.m_currRowIndex < this.RowsCount)
      ++this.m_currRowIndex;
    return !this.IsEnd;
  }

  public object GetCellValue(string columnName) => this.CurrentRow?[columnName];

  public string[] ColumnNames => this.m_columnNames;

  internal void Close()
  {
    if (this.m_dataView != null)
    {
      this.m_dataView.Dispose();
      this.m_dataView = (DataView) null;
    }
    if (this.m_table != null)
    {
      this.m_table.Dispose();
      this.m_table = (DataTable) null;
    }
    if (this.m_row != null)
      this.m_row = (DataRow) null;
    if (this.m_columnNames == null)
      return;
    this.m_columnNames = (string[]) null;
  }

  private void ReadColumnNames(DataTable dataTable)
  {
    this.m_columnNames = new string[dataTable.Columns.Count];
    for (int index = 0; index < this.m_columnNames.Length; ++index)
      this.m_columnNames[index] = this.m_table.Columns[index].ColumnName;
  }
}
