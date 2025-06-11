// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Column
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Column : IColumn
{
  private Syncfusion.Presentation.TableImplementation.Cells _cells;
  private long _columnWidth;
  private Table _table;
  private int _index;
  private Columns _columns;

  internal Column(Table table)
  {
    this._cells = new Syncfusion.Presentation.TableImplementation.Cells(this);
    this._table = table;
  }

  internal Column(Columns col)
  {
    this._cells = new Syncfusion.Presentation.TableImplementation.Cells(this);
    this._columns = col;
  }

  public ICells Cells
  {
    get
    {
      if (this.Index != -1)
        this.GetCells(this.Index);
      return (ICells) this._cells;
    }
  }

  private void GetCells(int index)
  {
    Syncfusion.Presentation.TableImplementation.Cells cells = new Syncfusion.Presentation.TableImplementation.Cells(this);
    foreach (Row row in (IEnumerable<IRow>) this._table.Rows)
      cells.Add(row.Cells[index]);
    this._cells = cells;
  }

  public double Width
  {
    get => Convert.ToDouble((double) this._columnWidth / 12700.0);
    set
    {
      if (value < 0.0 || value > 4032.0)
        throw new ArgumentException("Invalid Width " + value.ToString());
      this._columnWidth = (long) (value * 12700.0);
    }
  }

  internal long ObtainWidth() => this._columnWidth;

  internal void SetWidth(long value) => this._columnWidth = value;

  internal int Index
  {
    get => this._index;
    set => this._index = value;
  }

  internal void Close()
  {
    if (this._cells == null)
      return;
    this._cells.Close();
    this._cells = (Syncfusion.Presentation.TableImplementation.Cells) null;
  }

  internal IColumn TableClone()
  {
    Column column = (Column) this.MemberwiseClone();
    column._cells = new Syncfusion.Presentation.TableImplementation.Cells(column);
    return (IColumn) column;
  }

  public IColumn Clone()
  {
    int int32 = Convert.ToInt32(this._table.Width / (double) this._table.Columns.Count);
    Column column = new Column(this._table);
    column.SetWidth((long) Helper.PointToEmu((double) int32));
    if (this.Cells != null)
    {
      column._cells = this._cells.Clone();
      column._cells.SetParent(column);
      column.Index = -1;
    }
    return (IColumn) column;
  }

  internal void SetParent(Table newParent)
  {
    this._table = newParent;
    this._cells.SetParent(newParent);
  }

  internal void SetParent(BaseSlide newParent)
  {
    foreach (Cell cell in this._cells)
      cell.SetParent(newParent);
  }
}
