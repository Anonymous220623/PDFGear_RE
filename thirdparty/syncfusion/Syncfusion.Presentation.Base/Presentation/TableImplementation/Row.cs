// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Row
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Row : IRow
{
  private Syncfusion.Presentation.TableImplementation.Cells _cells;
  private long _rowHeight;
  private Table _table;

  internal Row(Table table)
  {
    this._cells = new Syncfusion.Presentation.TableImplementation.Cells(this);
    this._table = table;
  }

  public ICells Cells => (ICells) this._cells;

  public double Height
  {
    get => Convert.ToDouble((double) this._rowHeight / 12700.0);
    set
    {
      if (value < 0.0 || value > 4032.0)
        throw new ArgumentException("Invalid Height " + value.ToString());
      this._rowHeight = (long) (value * 12700.0);
    }
  }

  internal Table Table => this._table;

  internal Cell AddCell(int column)
  {
    Cell cell = new Cell(this);
    cell.GetCellIndex(column, this.Table.Rows.IndexOf((IRow) this) + 1);
    this.AddCell(cell);
    return cell;
  }

  internal Cell CreateCell(int column)
  {
    Cell cell = column < 2 ? (Cell) this.Cells[column - 1].Clone() : (Cell) this.Cells[column - 2].Clone();
    cell.SetColumnSpan(0, true);
    (cell.TextBody as TextBody).SetParent(this.Table.BaseSlide);
    if (cell.TextBody.Paragraphs.Count != 0)
    {
      while (cell.TextBody.Paragraphs.Count - 1 > 0)
        cell.TextBody.Paragraphs.RemoveAt(0);
      Paragraph paragraph = cell.TextBody.Paragraphs[0] as Paragraph;
      IFont endParaProps = (IFont) paragraph.GetEndParaProps();
      if (paragraph.TextParts.Count > 0)
      {
        IFont font = paragraph.TextParts[cell.TextBody.Paragraphs[0].TextParts.Count - 1].Font;
        if (endParaProps == null)
        {
          if (font != null)
          {
            paragraph.SetEndParaProps(font);
            cell.PrevCellTextPartFont = font;
          }
          paragraph.SetIsLastPara(true);
        }
        else
          cell.PrevCellTextPartFont = endParaProps;
      }
      cell.TextBody.Paragraphs[0].TextParts.Clear();
    }
    cell.IsInsertedCell = true;
    cell.GetCellIndex(column, this.Table.Rows.IndexOf((IRow) this) + 1);
    return cell;
  }

  internal void AddCell(Cell cell) => this._cells.Add(cell);

  internal ICell AddCell()
  {
    Cell cell = new Cell(this);
    this.AddCell(cell);
    return (ICell) cell;
  }

  internal long ObtainHeight() => this._rowHeight;

  internal void SetHeight(long value) => this._rowHeight = value;

  internal void Layout(
    float usedHeight,
    float tableLeft,
    float tableTop,
    int rowIndex,
    ref float maxTextHeight)
  {
    int count = this.Cells.Count;
    float usedWidth = 0.0f;
    float totalTextHeight = 0.0f;
    for (int index = 0; index < count; ++index)
    {
      if (this.Cells[index] is Cell cell && this.Table.MergedCells.Contains(cell.CellIndex))
      {
        usedWidth += (float) this.Table.Columns[index].Width;
      }
      else
      {
        if (cell != null)
          usedWidth = cell.Layout(usedHeight, usedWidth, tableLeft, tableTop, rowIndex, index, ref totalTextHeight);
        if ((double) totalTextHeight > (double) maxTextHeight)
          maxTextHeight = totalTextHeight;
        totalTextHeight = 0.0f;
      }
    }
  }

  internal void Close()
  {
    if (this._cells == null)
      return;
    this._cells.Close();
    this._cells = (Syncfusion.Presentation.TableImplementation.Cells) null;
  }

  public IRow Clone()
  {
    Row row = (Row) this.MemberwiseClone();
    row._cells = this._cells.Clone();
    row._cells.SetParent(row);
    return (IRow) row;
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
