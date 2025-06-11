// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Rows
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Rows : IRows, IEnumerable<IRow>, IEnumerable
{
  private List<IRow> _list;
  private Table _table;

  public Rows(Table table)
  {
    this._table = table;
    this._list = new List<IRow>();
  }

  public int Count => this._list.Count;

  public IRow this[int rowIndex] => this._list[rowIndex];

  public IEnumerator<IRow> GetEnumerator() => (IEnumerator<IRow>) this._list.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public IRow Add()
  {
    Row tableRow = new Row(this._table);
    Row row = this._list[this.Count - 1] as Row;
    this.AddRow(tableRow);
    bool flag = false;
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      Cell cell = tableRow.AddCell(index + 1);
      if (row.Cells[index].ColumnSpan >= 2)
        flag = true;
      cell.ColumnWidth = row.Cells[index].ColumnWidth;
    }
    if (flag)
    {
      for (int index = 0; index < row.Cells.Count; ++index)
      {
        Cell cell = tableRow.Cells[index] as Cell;
        int columnSpan = row.Cells[index].ColumnSpan;
        if (columnSpan >= 2)
          cell.ColumnSpan = columnSpan;
      }
    }
    tableRow.SetHeight(row.ObtainHeight());
    return (IRow) tableRow;
  }

  public int Add(IRow row)
  {
    foreach (ICell cell in (IEnumerable<ICell>) row.Cells)
      (cell.TextBody as TextBody).SetParent(this._table.BaseSlide);
    this._list.Add(row);
    return this._list.IndexOf(row);
  }

  public void Insert(int index, IRow value)
  {
    foreach (ICell cell in (IEnumerable<ICell>) value.Cells)
      (cell.TextBody as TextBody).SetParent(this._table.BaseSlide);
    this._list.Insert(index, value);
  }

  public void RemoveAt(int index) => this._list.RemoveAt(index);

  public void Remove(IRow value) => this._list.Remove(value);

  public int IndexOf(IRow value) => this._list.IndexOf(value);

  public void Clear() => this._list.Clear();

  internal void AddRow(Row tableRow) => this._list.Add((IRow) tableRow);

  internal void Close()
  {
    if (this._list == null)
      return;
    foreach (Row row in this._list)
      row.Close();
    this._list.Clear();
    this._list = (List<IRow>) null;
  }

  public Rows Clone()
  {
    Rows rows = (Rows) this.MemberwiseClone();
    rows._list = this.CloneRowList();
    return rows;
  }

  private List<IRow> CloneRowList()
  {
    List<IRow> rowList = new List<IRow>();
    foreach (IRow row1 in this._list)
    {
      Row row2 = (Row) row1.Clone();
      rowList.Add((IRow) row2);
    }
    return rowList;
  }

  internal void SetParent(Table table)
  {
    this._table = table;
    foreach (Row row in this._list)
      row.SetParent(table);
  }

  internal void SetParent(BaseSlide newParent)
  {
    foreach (Row row in this._list)
      row.SetParent(newParent);
  }
}
