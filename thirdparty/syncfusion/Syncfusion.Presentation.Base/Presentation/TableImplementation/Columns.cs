// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Columns
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Columns : IColumns, IEnumerable<IColumn>, IEnumerable
{
  private List<IColumn> _list;
  private Table _table;

  public Columns(Table table)
  {
    this._table = table;
    this._list = new List<IColumn>();
  }

  public int Count => this._list.Count;

  public IColumn this[int columnIndex] => this._list[columnIndex];

  public IEnumerator<IColumn> GetEnumerator() => (IEnumerator<IColumn>) this._list.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public IColumn Add()
  {
    int int32 = Convert.ToInt32(this._table.Width / (double) this._table.Columns.Count);
    Column col = new Column(this._table);
    col.Index = this._table.Columns.Count;
    col.SetWidth((long) Helper.PointToEmu((double) int32));
    this.AddColumn(col);
    double num1 = 0.0;
    foreach (Column column in (IEnumerable<IColumn>) this._table.Columns)
      num1 += column.Width;
    double num2 = num1 / this._table.Width;
    int num3 = this._table.Columns.IndexOf((IColumn) col);
    foreach (Row row in (IEnumerable<IRow>) this._table.Rows)
    {
      row.AddCell(num3 + 1);
      for (int index = num3 + 1; index < row.Cells.Count; ++index)
        ++((Cell) row.Cells[index]).CellIndex;
      if (num3 >= 1)
      {
        int num4 = 0;
        int num5 = 0;
        for (int index = num3 - 1; index >= 0; --index)
        {
          num4 = row.Cells[index].ColumnSpan;
          ++num5;
          if (num4 > 0)
            break;
        }
        if (num4 >= 2 && num3 - num5 - 1 + num4 >= num3)
          ((Cell) row.Cells[num3 - num5]).SetColumnSpan(num4 + 1, true);
      }
    }
    foreach (Column column in (IEnumerable<IColumn>) this._table.Columns)
      column.Width /= num2;
    return (IColumn) col;
  }

  public int Add(IColumn column)
  {
    column = (column as Column).Clone();
    foreach (ICell cell in (IEnumerable<ICell>) column.Cells)
      (cell.TextBody as TextBody).SetParent(this._table.BaseSlide);
    this._list.Add(column);
    this.AddExistingColumnToCol(column);
    return this._list.IndexOf(column);
  }

  public int AddTableColumn(IColumn column)
  {
    this._list.Add(column);
    return this._list.Count - 1;
  }

  public void InserTableColumn(int index, IColumn column) => this._list.Insert(index, column);

  private void AddExistingColumnToCol(IColumn column)
  {
    Rows rows = this._table.Rows as Rows;
    for (int index = 0; index < rows.Count; ++index)
      ((Cells) rows[index].Cells).Add(column.Cells[index]);
    int num = this._table.Columns.Count - 1;
    (column as Column).Index = num;
  }

  public void Insert(int index, IColumn column)
  {
    if (this.Count < 0)
      throw new Exception("The row count cannot be negative");
    if (index < 0)
      throw new Exception("The index position cannot be negative");
    if (this.Count < index)
      throw new Exception("The column value exceeds the column collection");
    double num1 = 0.0;
    column = column != null ? (column as Column).Clone() : (IColumn) new Column(this);
    foreach (ICell cell in (IEnumerable<ICell>) column.Cells)
      (cell.TextBody as TextBody).SetParent(this._table.BaseSlide);
    column.Width = index == 0 ? this._table.Rows[0].Cells[index].ColumnWidth : this._table.Rows[0].Cells[index - 1].ColumnWidth;
    this._list.Insert(index, column);
    foreach (Column column1 in (IEnumerable<IColumn>) this._table.Columns)
      num1 += column1.Width;
    double num2 = num1 / this._table.Width;
    Rows rows = this._table.Rows as Rows;
    for (int index1 = 0; index1 < rows.Count; ++index1)
    {
      ((Cells) rows[index1].Cells).Insert(index, column.Cells[index1]);
      for (int index2 = index + 1; index2 < rows[index1].Cells.Count; ++index2)
        ++((Cell) rows[index1].Cells[index2]).CellIndex;
      if (index >= 1)
      {
        int num3 = 0;
        int num4 = 0;
        for (int index3 = index - 1; index3 >= 0; --index3)
        {
          num3 = rows[index1].Cells[index3].ColumnSpan;
          ++num4;
          if (num3 > 0)
            break;
        }
        if (num3 >= 2 && index - num4 - 1 + num3 >= index)
          ((Cell) rows[index1].Cells[index - num4]).SetColumnSpan(num3 + 1, true);
      }
    }
    (column as Column).Index = index;
    for (int columnIndex = index + 1; columnIndex < this.Count; ++columnIndex)
      ++(this._table.Columns[columnIndex] as Column).Index;
    foreach (Column column2 in (IEnumerable<IColumn>) this._table.Columns)
      column2.Width /= num2;
  }

  public void RemoveAt(int index)
  {
    foreach (Row row in (IEnumerable<IRow>) this._table.Rows)
      ((Cells) row.Cells).RemoveAt(index);
    this._list.RemoveAt(index);
  }

  public void Remove(IColumn item)
  {
    Rows rows = this._table.Rows as Rows;
    for (int index = 0; index < rows.Count; ++index)
      ((Cells) rows[index].Cells).Remove(item.Cells[index]);
    this._list.Remove(item);
  }

  public int IndexOf(IColumn item) => this._list.IndexOf(item);

  public void Clear() => this._list.Clear();

  public void AddColumn(Column col) => this._list.Add((IColumn) col);

  internal void Close()
  {
    if (this._list == null)
      return;
    foreach (Column column in this._list)
      column.Close();
    this._list.Clear();
    this._list = (List<IColumn>) null;
  }

  public Columns Clone()
  {
    Columns columns = (Columns) this.MemberwiseClone();
    columns._list = this.CloneColumnList();
    return columns;
  }

  private List<IColumn> CloneColumnList()
  {
    List<IColumn> columnList = new List<IColumn>();
    foreach (Column column1 in this._list)
    {
      Column column2 = (Column) column1.TableClone();
      columnList.Add((IColumn) column2);
    }
    return columnList;
  }

  internal void SetParent(Table table)
  {
    this._table = table;
    foreach (Column column in this._list)
      column.SetParent(table);
  }

  internal void SetParent(BaseSlide newParent)
  {
    foreach (Column column in this._list)
      column.SetParent(newParent);
  }
}
