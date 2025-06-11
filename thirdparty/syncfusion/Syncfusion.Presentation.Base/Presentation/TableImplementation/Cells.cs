// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Cells
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Cells : ICells, IEnumerable<ICell>, IEnumerable
{
  private List<ICell> _cells;
  private Row _row;
  private Column _column;

  internal Cells(Row row)
  {
    this._row = row;
    this._cells = new List<ICell>();
  }

  internal Cells(Column column)
  {
    this._column = column;
    this._cells = new List<ICell>();
  }

  public int Count => this._cells.Count;

  public ICell this[int index] => this._cells[index];

  public IEnumerator<ICell> GetEnumerator() => (IEnumerator<ICell>) this._cells.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._cells.GetEnumerator();

  public ICell Add()
  {
    Cell cell = new Cell(this._row);
    this.Add(cell);
    return (ICell) cell;
  }

  public int Add(ICell cell)
  {
    this._cells.Add(cell);
    return this._cells.IndexOf(cell);
  }

  public void Insert(int index, ICell cell) => this._cells.Insert(index, cell);

  public void RemoveAt(int index) => this._cells.RemoveAt(index);

  public void Remove(ICell cell) => this._cells.Remove(cell);

  public int IndexOf(ICell cell) => this._cells.IndexOf(cell);

  public void Clear() => this._cells.Clear();

  internal void Add(Cell cell) => this._cells.Add((ICell) cell);

  internal void Close()
  {
    if (this._cells == null)
      return;
    foreach (Cell cell in this._cells)
      cell.Close();
    this._cells.Clear();
    this._cells = (List<ICell>) null;
  }

  public Cells Clone()
  {
    Cells cells = (Cells) this.MemberwiseClone();
    cells._cells = this.CloneCellList();
    return cells;
  }

  private List<ICell> CloneCellList()
  {
    List<ICell> cellList = new List<ICell>();
    foreach (ICell cell1 in this._cells)
    {
      Cell cell2 = (Cell) cell1.Clone();
      cellList.Add((ICell) cell2);
    }
    return cellList;
  }

  internal void SetParent(Row row)
  {
    this._row = row;
    foreach (Cell cell in this._cells)
      cell.SetParent(row);
  }

  internal void SetParent(Column column)
  {
    this._column = column;
    foreach (Cell cell in this._cells)
      cell.SetParent(column);
  }

  internal void SetParent(Table newParent)
  {
    foreach (Cell cell in this._cells)
      cell.SetParent(newParent);
  }
}
